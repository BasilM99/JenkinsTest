using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.Resources;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.Utilities.EmailsSender;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule
{

    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class GenerateReport : IJob
    {
        private static IReportService _ReportService;
        private static IMailSender _mailSender;
        private static ArabyAds.Framework.Resources.IResourceService _ResourceService;
        static readonly object LockObj = new object();
        private static ArabyAds.Framework.ConfigurationSetting.IConfigurationSettingService _configurationManager;
        private static ArabyAds.Framework.ConfigurationSetting.ConfigurationManager _configSettingManger;
        static GenerateReport()
        {

            _ReportService = SchedulerHelper.ReportService;
            _mailSender = SchedulerHelper.MailSender;
            _ResourceService = SchedulerHelper.ResourceService;
            _configurationManager = SchedulerHelper.ConfigurationManager;
            _configSettingManger = new ConfigurationManager(_configurationManager);

        }

        public Task Execute(IJobExecutionContext context)
        {
            int jobId = 0;
            JobKey key = context.JobDetail.Key;

            try
            {



                Dictionary<string, string> dics = new Dictionary<string, string>();
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                List<AppCommonReportDto> reportingAppList = new List<AppCommonReportDto>();
                List<CampaignCommonReportDto> reportingCampaignCommonList = new List<CampaignCommonReportDto>();
                WriteReportDocumentsHelper wRDoc = new WriteReportDocumentsHelper();
                string completePath = string.Empty;
                jobId = dataMap.GetIntValue("ID");
                _ReportService.UpdateJobToNotFinished(ValueMessageWrapper.Create(jobId));
                var dto = _ReportService.GetSchadulingReport(ValueMessageWrapper.Create(jobId));

                SchedulerHelper.LanguageCode = dto.LanguageCode;
                int counter = 0;
                bool emailsent = false;
                if (dto.DateRecurrenceType == DateRecurrenceType.Specific)
                {
                    if (dto.ReportDto.FromDate < Framework.Utilities.Environment.GetServerTime().AddDays(-1))
                    {
                        dto.ReportDto.ToDate = Framework.Utilities.Environment.GetServerTime().AddDays(-1);
                        dto.ReportDto.ToDate = new DateTime(dto.ReportDto.ToDate.Year, dto.ReportDto.ToDate.Month, dto.ReportDto.ToDate.Day, 23, 59, 0);

                    }
                    else
                    {

                        dto.ReportDto.ToDate = new DateTime(dto.ReportDto.FromDate.Year, dto.ReportDto.FromDate.Month, dto.ReportDto.FromDate.Day, 23, 59, 0);
                    }

                }
                dto.ReportDto.FromDateString = dto.ReportDto.FromDate.ToString(_configSettingManger.GetConfigurationSetting(0, 0, "ShortDateFormat"));
                dto.ReportDto.ToDateString = dto.ReportDto.ToDate.ToString(_configSettingManger.GetConfigurationSetting(0, 0, "ShortDateFormat"));
                dto.ReportDto.IsPrimaryUser = dto.IsPrimaryUser;
                dto.ReportDto.userId = dto.UserId;


                string emailTemplate =  _ResourceService.GetResource(new ResourceRequest { Key = "ScheduleEmail", ResourceSet = "Emails", CultureCode = SchedulerHelper.GetCultureStr() }).Result;
                emailTemplate = emailTemplate.Replace("@EmailIntroduction", dto.EmailIntroduction);
                emailTemplate = emailTemplate.Replace("@Year", DateTime.Now.Year.ToString());

                string email = _ReportService.GetEmailAdminForReport();


                if (dto.AllReportRecipient != null)
                {
                    foreach (var item in dto.AllReportRecipient)
                    {
                        dics.Add(item.Email, string.Empty);

                    }
                }
                //if (!string.IsNullOrEmpty(dto.PreferedName))
                //{

                //    dto.ReportDto.GroupByName = false;

                //    //dto.ReportDto.Layout = "summary";
                //}
                string emailBoday = string.Empty;
                lock (LockObj)
                {
                    GenerateSubject(dto);

                    if (!dto.IsForQueryBuilder)
                    {
                        if (dto.ReportSectionType == ReportSectionType.Advertiser)
                        {
                            bool showCI = false;
                            GetCampaignReportData(dto.ReportDto.FromDate, dto.ReportDto.ToDate, dto.ReportDto.SummaryBy.ToString(), dto.ReportDto.Layout, dto.ReportDto.CriteriaOpt, dto.ReportDto.ItemsList, dto.ReportDto.AdvancedCriteria, dto.ReportDto.DeviceCategory, dto.ReportDto.TabId, dto.ReportDto.IsAccumulated, "1", int.MaxValue, dto.ReportDto.OrderColumn, dto.ReportDto.GroupByName.ToString(), dto.ReportDto, out reportingCampaignCommonList, out counter);
                            if ((dto.ReportDto.SummaryBy == 1 || dto.ReportDto.SummaryBy == 4 || dto.ReportDto.SummaryBy == 3) && dto.ReportDto.Layout == "detailed" && !dto.ReportDto.GroupByName)
                            {
                                showCI = true;
                            }

                            HandleCommonNameCaseForCampaignReport(dto, reportingCampaignCommonList);
                            var TotalStamp = wRDoc.GetGrandTotalForCampaignReport(reportingCampaignCommonList);

                            if ((dto.ReportDto.SummaryBy == 1 || dto.ReportDto.SummaryBy == 4) && dto.ReportDto.Layout == "detailed" && !dto.ReportDto.GroupByName && (dto.ReportDto.TabId.ToLower() == "campaign" || dto.ReportDto.TabId.ToLower() == "adgroup"))
                            {
                                completePath = wRDoc.BuildReportFile<CampaignCommonReportDto>(reportingCampaignCommonList, "csv", dto, ref emailBoday, TotalStamp);
                            }
                            else
                            {
                                completePath = wRDoc.BuildReportFile<CampaignCommonReportDto>(reportingCampaignCommonList, "csv", dto, ref emailBoday, TotalStamp, false, false);
                            }

                        }

                        else if (dto.ReportSectionType == ReportSectionType.Publisher)
                        {
                            GetAppReportData(dto.ReportDto.FromDate, dto.ReportDto.ToDate, dto.ReportDto.SummaryBy.ToString(), dto.ReportDto.Layout, dto.ReportDto.CriteriaOpt, dto.ReportDto.ItemsList, dto.ReportDto.AdvancedCriteria, dto.ReportDto.DeviceCategory, dto.ReportDto.TabId, dto.ReportDto.IsAccumulated, "1", int.MaxValue, dto.ReportDto.OrderColumn, dto.ReportDto, out reportingAppList, out counter);
                            HandleCommonNameCaseForAppReport(dto, reportingAppList);
                            var itemRep = wRDoc.GetGrandTotalForAppReport(reportingAppList);
                            completePath = wRDoc.BuildReportFile<AppCommonReportDto>(reportingAppList, "csv", dto, ref emailBoday, itemRep);


                        }
                    }
                    else
                    {
                        //GetAppReportData(dto.ReportDto.FromDate, dto.ReportDto.ToDate, dto.ReportDto.SummaryBy.ToString(), dto.ReportDto.Layout, dto.ReportDto.CriteriaOpt, dto.ReportDto.ItemsList, dto.ReportDto.AdvancedCriteria, dto.ReportDto.DeviceCategory, dto.ReportDto.TabId, dto.ReportDto.IsAccumulated, "1", int.MaxValue, dto.ReportDto.OrderColumn, dto.ReportDto, out reportingAppList, out counter);
                        DataModelDto dtoDataModel = new DataModelDto();
                        dtoDataModel.ColumnsIds = dto.ReportDto.ColumnsIds;
                        dtoDataModel.ColumnsIdsString = dto.ReportDto.ColumnsIdsString;
                            dtoDataModel.fact = dto.ReportDto.fact;
                     
                        dtoDataModel.from = dto.ReportDto.FromDate;
                        dtoDataModel.MeasuresIds = dto.ReportDto.MeasuresIds;
                        dtoDataModel.MeasuresIdsString = dto.ReportDto.MeasuresIdsString;
                        dtoDataModel.pageNumber = dto.ReportDto.PageNumber;
                        //dtoDataModel.Querydata = dto.ReportDto.QueryJsonData;
                        dtoDataModel.QueryJsonData = dto.ReportDto.QueryJsonData;
                        dtoDataModel.SummaryBy = dto.ReportDto.SummaryBy;
                        dtoDataModel.to = dto.ReportDto.ToDate;
                        dtoDataModel.accountId = dto.AccountId;
                        dtoDataModel.IncludeId = dto.ReportDto.IncludeId;
                        var rows= _ReportService.ExecuteAndFilterResult(dtoDataModel);

                        completePath = wRDoc.BuildCSVReportFile(rows, "csv", dto, ref emailBoday);

                    }



                    emailTemplate = emailTemplate.Replace("@Fields", emailBoday);
                    if (dics.Count > 0)
                    {
                        emailsent = _mailSender.SendEmail("", email, dics, dto.EmailSubject, emailTemplate, true, "", completePath);
                    }
                    else
                    {

                        emailsent = true;
                    }
                    if (emailsent)
                    {
                        _ReportService.UpdateJobToSchduledAfterRun(new UpdateJobToSchduledAfterRunRequest { JobID = jobId, ReportFilePath = completePath });


                    }
                }
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message + " JobID:" + jobId, ex);
                JobExecutionException exJob = new JobExecutionException(ex);
                exJob.RefireImmediately = false;
                if (JsonConfigurationManager.AppSettings["RefireAfterException"] == "True")
                {
                    exJob.RefireImmediately = true;
                    //wait 3 minutes

                    Thread.Sleep(180000);
                }
                throw exJob;
            }
            finally
            {


            }
            //SendNow
            if (key.Name.Contains("JobNow"))
            {
                _ReportService.UpdateJobToFinishedNoFireTime(ValueMessageWrapper.Create(jobId));
            }
            else
            {
                var nextFireTime = SchedulerHelper.GetNextFireTime(context);
                DateTime? dtSch = null;
                if (nextFireTime.HasValue)
                {
                    dtSch = nextFireTime.Value.DateTime;
                }
                _ReportService.UpdateJobToFinished( new UpdateJobToFinishedRequest { JobID = jobId, NextFireTime = dtSch });
            }

            return Task.CompletedTask;
        }
        private bool GetCampaignReportData(DateTime fromdate, DateTime toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, bool IsAccumulated, string page, int itemsPerPage, string orderBy, string groupByName, ReportCriteriaDto dto, out List<CampaignCommonReportDto> reportingList, out int counter)
        {
            //if (!(string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(toDate)))
            //{
            string orderByColumn, orderType;
            GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
            ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
            criteriaDto.CampaignType = CampaignType.Normal;
            criteriaDto.NotInCampaignType = CampaignType.AdHouse;
            
            criteriaDto.FromDate = fromdate;
            criteriaDto.ToDate = toDate;
            criteriaDto.SummaryBy = int.Parse(summaryBy);
            criteriaDto.Layout = layout;
            criteriaDto.ItemsPerPage = itemsPerPage;
            criteriaDto.PageNumber = (string.IsNullOrEmpty(page) ? 0 : int.Parse(page) - 1);
            criteriaDto.OrderType = orderType;
            criteriaDto.OrderColumn = orderByColumn;
            criteriaDto.GroupByName = string.IsNullOrEmpty(groupByName) ? false : bool.Parse(groupByName);
            criteriaDto.AccountId = dto.AccountId;
            criteriaDto.userId = dto.userId;
            criteriaDto.IsPrimaryUser = dto.IsPrimaryUser;
            criteriaDto.IsAccumulated = IsAccumulated;
            criteriaDto.AdvertiserId = dto.AdvertiserId;
            criteriaDto.AccountAdvertiserId = dto.AccountAdvertiserId;
            if (criteriaOpt.ToLower() != "specific")
            {
                criteriaDto.ItemsList = null;
            }
            else
            {
                if (string.IsNullOrEmpty(AdsList))
                {
                    criteriaDto.ItemsList = null;
                }
                else
                {
                    AdsList = AdsList.Trim(new char[] { ',' });
                    criteriaDto.ItemsList = AdsList;
                }
            }

            switch (tabId.ToLower())
            {
                case "campaign":
                    reportingList = _ReportService.GetCampaignReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignReport(criteriaDto);
                    break;
                case "adgroup":
                    reportingList = _ReportService.GetAdGroupReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalAdGroupReport(criteriaDto);
                    break;
                case "ad":
                    reportingList = _ReportService.GetAdReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalAdReport(criteriaDto);
                    break;
                case "operator":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }
                    reportingList = _ReportService.GetCampaignOperatorReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalCampaignOperatorReport(criteriaDto);
                    break;
                case "devicemodel":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }

                    switch (deviceCategory.ToLower())
                    {
                        case "platform":
                            reportingList = _ReportService.GetCampaignPlatformReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignPlatformReport(criteriaDto);
                            break;
                        case "manufactor":
                            reportingList = _ReportService.GetCampaignManuFactorReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalCampaignManuFactorReport(criteriaDto);
                            break;
                        default:
                            reportingList = _ReportService.GetCampaignManuFactorReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignManuFactorReport(criteriaDto);
                            break;
                    }
                    break;
                case "geolocation":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }
                    reportingList = _ReportService.GetCampaignGeoLocationReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignGeoLocationReport(criteriaDto);
                    break;
                case "audiancesegmentforadvertiser":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }
                    reportingList = _ReportService.GetCampaignAudianceSegmentForAdvertiserReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignGeoLocationReport(criteriaDto);
                    break;
                case "subappsite":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }
                    reportingList = _ReportService.GetCampaignSubAppSiteReport(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignGeoLocationReport(criteriaDto);
                    break;
                default:
                    reportingList = new List<CampaignCommonReportDto>();
                    counter = 0;
                    break;
            }

            return true;
            //}

        }
        private bool GetAppReportData(DateTime fromdate, DateTime toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, bool IsAccumulated, string page, int itemsPerPage, string orderBy, ReportCriteriaDto dto, out List<AppCommonReportDto> reportingList, out int counter)
        {

            string orderByColumn, orderType;
            GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
            ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
            criteriaDto.CampaignType = CampaignType.Normal;
            criteriaDto.NotInCampaignType = CampaignType.AdHouse;
            criteriaDto.FromDate = fromdate;
            criteriaDto.ToDate = toDate;
            criteriaDto.SummaryBy = int.Parse(summaryBy);
            criteriaDto.Layout = layout;
            criteriaDto.ItemsPerPage = itemsPerPage;
            criteriaDto.PageNumber = (string.IsNullOrEmpty(page) ? 0 : int.Parse(page) - 1);
            criteriaDto.OrderType = orderType;
            criteriaDto.OrderColumn = orderByColumn;
            criteriaDto.AccountId = dto.AccountId;
            criteriaDto.userId = dto.userId;
            criteriaDto.IsPrimaryUser = dto.IsPrimaryUser;
            criteriaDto.IsAccumulated = IsAccumulated;
            if (criteriaOpt.ToLower() != "specific")
            {
                criteriaDto.ItemsList = null;
            }
            else
            {
                if (string.IsNullOrEmpty(AdsList))
                {
                    criteriaDto.ItemsList = null;
                }
                else
                {
                    AdsList = AdsList.Trim(new char[] { ',' });
                    criteriaDto.ItemsList = AdsList;
                }
            }

            switch (tabId.ToLower())
            {
                case "app":
                    reportingList = _ReportService.GetAppReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalAppReport(criteriaDto);
                    break;
                case "operator":

                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }

                    reportingList = _ReportService.GetAppOperatorReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalAppOperatorReport(criteriaDto);
                    break;
                case "devicemodel":
                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }
                    switch (deviceCategory.ToLower())
                    {
                        case "platform":
                            reportingList = _ReportService.GetAppPlatformReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//  _ReportService.GetTotalAppPlatformReport(criteriaDto);
                            break;
                        case "manufactor":
                            reportingList = _ReportService.GetAppManuFactorReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//  _ReportService.GetTotalAppManuFactorReport(criteriaDto);
                            break;
                        default:
                            reportingList = _ReportService.GetAppManuFactorReportForEmailService(criteriaDto);
                            counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//  _ReportService.GetTotalAppManuFactorReport(criteriaDto);
                            break;
                    }
                    break;
                case "geolocation":

                    if (string.IsNullOrEmpty(advancedCriteria))
                    {
                        criteriaDto.AdvancedCriteria = null;
                    }
                    else
                    {
                        criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                    }


                    reportingList = _ReportService.GetAppGeoLocationReportForEmailService(criteriaDto);
                    counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalAppGeoLocationReport(criteriaDto);
                    break;
                default:
                    reportingList = new List<AppCommonReportDto>();
                    counter = 0;
                    break;
            }

            return true;

        }
        private void GetOrderSetting(string order, string layout, out string orderColumn, out string orderType)
        {
            if (string.IsNullOrEmpty(order))
            {
                if (layout == "detailed")
                {
                    orderColumn = "Date,Name";
                }
                else
                {
                    orderColumn = "Date";
                }

                orderType = "asc";
            }
            else
            {
                orderColumn = order.Split('-')[0];
                orderType = order.Split('-')[1];
            }
            //TODO:Osaleh to add support for Grid sorting and remove this temp solution 
            if (orderColumn.EndsWith("Text"))
            {
                orderColumn = orderColumn.Replace("Text", "");
            }
            if (orderColumn == "Ctr")
            {
                orderColumn = "CTR";
            }
            if (orderColumn == "DateRange")
            {
                orderColumn = "Date";
            }
        }
        private List<CampaignCommonReportDto> HandleCommonNameCaseForCampaignReport(ReportSchedulerDto dto, List<CampaignCommonReportDto> reportingList)
        {
            List<CampaignCommonReportDto> finalresult = reportingList;
            CampaignCommonReportDto dtoitem;
            if (!string.IsNullOrEmpty(dto.PreferedName))
            {
                finalresult = new List<CampaignCommonReportDto>();
                if (dto.ReportDto.Layout == "summary")
                {
                    //reportingList.Select(M => M.SubName.Replace(M.SubName, dto.PreferedName));
                    if (reportingList != null)
                    {
                        foreach (var item in reportingList)
                        {
                            item.SubName = dto.PreferedName;
                            // finalresult.Add(item);
                        }
                    }
                }
                //if (reportingList != null)
                //{




                //    var grouped = reportingList.GroupBy(M => M.Date);
                //    foreach (var item in grouped)
                //    {
                //        dtoitem = new CampaignCommonReportDto();

                //        foreach (var gpItem in item)
                //        {
                //            dtoitem.SubName = gpItem.SubName;
                //            dtoitem.Name = gpItem.Name;
                //            dtoitem.Date = gpItem.Date;
                //            dtoitem.TimeId = gpItem.TimeId;
                //            dtoitem.DateRange = gpItem.DateRange; ;
                //            //dtoitem.Date = dtoitem.Date;
                //            dtoitem.TotalCount = dtoitem.TotalCount + gpItem.TotalCount;
                //            dtoitem.Impress = dtoitem.Impress + gpItem.Impress;
                //            dtoitem.Clicks = dtoitem.Clicks + gpItem.Clicks;
                //            dtoitem.Spend = dtoitem.Spend + gpItem.Spend;

                //            dtoitem.AvgCPC = dtoitem.AvgCPC + gpItem.AvgCPC;
                //            dtoitem.CTR = dtoitem.CTR + gpItem.CTR;

                //        }
                //        finalresult.Add(dtoitem);

                //    }

                //}
            }
            return reportingList;

        }
        private List<AppCommonReportDto> HandleCommonNameCaseForAppReport(ReportSchedulerDto dto, List<AppCommonReportDto> reportingList)
        {





            List<AppCommonReportDto> finalresult = reportingList;
            AppCommonReportDto dtoitem;
            if (!string.IsNullOrEmpty(dto.PreferedName))
            {
                finalresult = new List<AppCommonReportDto>();
                if (dto.ReportDto.Layout == "summary")
                {
                    //reportingList.Select(M => M.SubName.Replace(M.SubName, dto.PreferedName));
                    if (reportingList != null)
                    {
                        foreach (var item in reportingList)
                        {
                            item.SubName = dto.PreferedName;
                            //finalresult.Add(item);
                        }
                    }
                }
                //if (reportingList != null)
                //{




                //    var grouped = reportingList.GroupBy(M => M.Date);
                //    foreach (var item in grouped)
                //    {
                //        dtoitem = new AppCommonReportDto();

                //        foreach (var gpItem in item)
                //        {
                //            dtoitem.SubName = gpItem.SubName;
                //            dtoitem.Name = gpItem.Name;
                //            dtoitem.Date = gpItem.Date;
                //            dtoitem.TimeId = gpItem.TimeId;
                //            dtoitem.DateRange = gpItem.DateRange;
                //            //dtoitem.Date = dtoitem.Date;
                //            dtoitem.TotalCount = dtoitem.TotalCount + gpItem.TotalCount;
                //            dtoitem.AdImpress = dtoitem.AdImpress + gpItem.AdImpress;
                //            dtoitem.Clicks = dtoitem.Clicks + gpItem.Clicks;
                //            dtoitem.Revenue = dtoitem.Revenue + gpItem.Revenue;

                //            dtoitem.AdRequests = dtoitem.AdRequests + gpItem.AdRequests;
                //            //dtoitem.CTR = dtoitem.CTR + gpItem.CTR;

                //        }
                //        finalresult.Add(dtoitem);

                //    }

                //}
            }
            return reportingList;
        }
        private void GenerateSubject(ReportSchedulerDto dto)
        {
            dto.FileName = dto.ReportId + "";
            dto.CompositeName = string.Empty;
            if (dto.RecurrenceType == RecurrenceType.Day)
                dto.EmailSubject = dto.EmailSubject.Replace("{Recurrence}", _ResourceService.GetResource(new ResourceRequest { Key = "Daily", ResourceSet = "Time", CultureCode = SchedulerHelper.GetCultureStr() }).Result);
            if (dto.RecurrenceType == RecurrenceType.Week)
                dto.EmailSubject = dto.EmailSubject.Replace("{Recurrence}", _ResourceService.GetResource(new ResourceRequest { Key = "Weekly", ResourceSet = "Time", CultureCode = SchedulerHelper.GetCultureStr() }).Result);
            if (dto.RecurrenceType == RecurrenceType.Month)
                dto.EmailSubject = dto.EmailSubject.Replace("{Recurrence}", _ResourceService.GetResource(new ResourceRequest { Key = "Monthly", ResourceSet = "Time", CultureCode = SchedulerHelper.GetCultureStr() }).Result);
            if (dto.RecurrenceType == RecurrenceType.Month)
                dto.EmailSubject = dto.EmailSubject.Replace("{Recurrence}", _ResourceService.GetResource(new ResourceRequest { Key = "Monthly", ResourceSet = "Time", CultureCode = SchedulerHelper.GetCultureStr() }).Result);
            string substitutionName = string.Empty;
            List<int> Ids = new List<int>();

            int tempId = 0;

            if (!string.IsNullOrEmpty(dto.ReportDto.ItemsList))
            {
                string AdsList = dto.ReportDto.ItemsList.Trim(new char[] { ',' });
                var arrString = AdsList.Split(new char[] { ',' });
                foreach (var id in arrString)
                {
                    tempId = 0;
                    int.TryParse(id, out tempId);
                    if (tempId > 0)
                        Ids.Add(tempId);
                }
            }
            if (Ids.Count == 1 && dto.ReportDto.CriteriaOpt.ToLower() == "specific" && !dto.IsForQueryBuilder)
            {
                switch (dto.ReportDto.TabId.ToLower())
                {
                    case "campaign":
                        substitutionName = _ReportService.GetCampaignName(ValueMessageWrapper.Create(Ids[0]));
                        dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "CampaignSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                        break;
                    case "adgroup":
                        substitutionName = _ReportService.GetAdGroupName(ValueMessageWrapper.Create(Ids[0]));
                        dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AdGroupSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                        break;
                    case "ad":
                        substitutionName = _ReportService.GetAdName(ValueMessageWrapper.Create(Ids[0]));
                        dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AdSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                        break;
                    case "app":
                        substitutionName = _ReportService.GetAppSiteName(ValueMessageWrapper.Create(Ids[0]));
                        dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AppSiteSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                        break;
                }

                if (!string.IsNullOrEmpty(substitutionName))
                {
                    dto.PreferedName = substitutionName;

                }
                // dto.CompositeName = substitutionName;
            }


            if (dto.IsForQueryBuilder)
            {
                string copqueryJsonData = dto.ReportDto.QueryJsonData;
                if (!string.IsNullOrWhiteSpace(copqueryJsonData))
                {
                    copqueryJsonData = copqueryJsonData.Replace("[", "\"");
                    copqueryJsonData = copqueryJsonData.Replace("]", "\"");
                }
                var Querydata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(copqueryJsonData);
                if (Querydata != null)
                {
                    int QuerydataCount = Querydata.Count();
                    int z = 0;
                    foreach (KeyValuePair<string, string> entry in Querydata)
                    {
                        List<int> TagIds = new List<int>();
                        if (!string.IsNullOrWhiteSpace(entry.Value))
                        {
                            TagIds = entry.Value.Split(',').Select(int.Parse).ToList();

                        }
                        if (TagIds != null && TagIds.Count == 1)
                        {
                            if (int.Parse(entry.Key) == 6)
                            {
                                substitutionName = _ReportService.GetAdNameForQ(ValueMessageWrapper.Create(TagIds[0]));
                                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AdSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                                break;
                            }
                            else if (int.Parse(entry.Key) == 2)
                            {
                                substitutionName = _ReportService.GetAdGroupNameForQ(ValueMessageWrapper.Create(TagIds[0]));
                                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AdGroupSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                                break;
                            }
                            else if (int.Parse(entry.Key) == 1)
                            {
                                substitutionName = _ReportService.GetCampaignName(ValueMessageWrapper.Create(TagIds[0]));
                                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "CampaignSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                                break;

                            }
                            else if (int.Parse(entry.Key) == 5)
                            {
                                substitutionName = _ReportService.GetAppSiteName(ValueMessageWrapper.Create(TagIds[0]));
                                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", _ResourceService.GetResource(new ResourceRequest { Key = "AppSiteSubject", ResourceSet = "Report", CultureCode = SchedulerHelper.GetCultureStr() }).Result + substitutionName);
                                break;
                            }

                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(dto.PreferedName) &&!dto.IsForQueryBuilder)
            {
                substitutionName = dto.PreferedName;
                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", dto.PreferedName);

            }
            else if (!string.IsNullOrEmpty(substitutionName) && !dto.IsForQueryBuilder)
            {
                dto.PreferedName = substitutionName;

            }


            if (!string.IsNullOrEmpty(dto.PreferedName) && dto.IsForQueryBuilder && string.IsNullOrEmpty(substitutionName))
            {
                substitutionName = dto.PreferedName;
                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", dto.PreferedName);

            }
            else if (!string.IsNullOrEmpty(substitutionName) && dto.IsForQueryBuilder)
            {
                dto.PreferedName = substitutionName;

            }
             


            if (string.IsNullOrEmpty(substitutionName))
            {
                substitutionName = DateTime.Now.ToString("yyyyMMdd");

                dto.EmailSubject = dto.EmailSubject.Replace(" - {EntityName}", string.Empty);
                dto.EmailSubject = dto.EmailSubject.Replace("{EntityName}", string.Empty);
                dto.FileName = dto.FileName + "_" + substitutionName + "_" + DateTime.Now.ToString("HHmmss");
            }
            else
            {
                dto.FileName = dto.FileName + "_" + substitutionName + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");
            }
            dto.EmailSubject = dto.EmailSubject.Replace("{ReportId}", dto.ReportId.ToString());
            dto.ReportTitle = dto.EmailSubject;
            dto.ReportTitle = dto.ReportTitle.Replace(" - {Date}", string.Empty);
            dto.ReportTitle = dto.ReportTitle.Replace("{Date}", string.Empty);
            dto.ReportTitle = dto.ReportTitle.Trim();
            dto.EmailSubject = dto.EmailSubject.Replace("{Date}", String.Format(JsonConfigurationManager.AppSettings["DateFormatService"], DateTime.Now));

        }

    }
}
