using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Telerik.Web.Mvc;
using System.IO;
using System.Drawing;
using Noqoush.AdFalcon.Web.Helper;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{

    public class ReportsTestController : AuthorizedControllerBase
    {
        private IAppSiteService _appsiteService;
        private IMetricTestService _MetricService;
        private IReportTestService _ReportService;
        private ICampaignService _CampaignsService;
        protected IAccountService _accountService;

        private WriteReportDocumentsHelper _WriteReportHelper;

        public ReportsTestController(IAppSiteService appSiteService, IMetricTestService metricService, IReportTestService reportService, ICampaignService campaignService, IAccountService accountService)
        {
            _appsiteService = appSiteService;
            _MetricService = metricService;
            _ReportService = reportService;
            _CampaignsService = campaignService;
            _accountService = accountService;
            _WriteReportHelper = new WriteReportDocumentsHelper();
        }

        public ActionResult Index(string reportType)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Reports", "Titles"),
                Order = 1,
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId)
            {
                if (string.IsNullOrEmpty(reportType) || reportType.ToLower() == "app")
                {
                    if (!(Config.IsAppOpsAdmin))
                    {
                        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                    }
                }
                else
                {
                    if (!(Config.IsAdOpsAdmin))
                    {
                        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                    }
                }
            }

            return View();
        }
        public ActionResult SaveCampaignReport(CampaignReportSchedulingSaveModel CampaignReportScheduling)
        {
            string message = "";
            bool result = false;
            int id = 0;
            try
            {
                var AllReportRecipient = !string.IsNullOrEmpty(CampaignReportScheduling.AllReportRecipient) ? CampaignReportScheduling.AllReportRecipient.Split(',').ToList() : new List<string>();
                ReportSchedulerDto reportSchedulerDto = new ReportSchedulerDto
                {
                    AllReportRecipient = AllReportRecipient.Select(x => new ReportRecipientDTO { Email = x }).ToList(),
                    Name = CampaignReportScheduling.Name,
                    EndDate = CampaignReportScheduling.SchedulingEndtDate.HasValue ? (DateTime?)new DateTime(CampaignReportScheduling.SchedulingEndtDate.Value.Year, CampaignReportScheduling.SchedulingEndtDate.Value.Month, CampaignReportScheduling.SchedulingEndtDate.Value.Day, 23, 59, 0) : null,
                    StartDate = CampaignReportScheduling.SchedulingStartDate,
                    TimeSentAt = CampaignReportScheduling.TimeSentAt.HasValue ? CampaignReportScheduling.TimeSentAt.Value : Framework.Utilities.Environment.GetServerTime(),
                    RecurrenceType = CampaignReportScheduling.RecurrenceType,
                    EmailIntroduction = CampaignReportScheduling.EmailIntroduction,
                    ReportSectionType = CampaignReportScheduling.ReportSectionType,
                    DateRecurrenceType = CampaignReportScheduling.DateRecurrenceType,
                    WeekDay = CampaignReportScheduling.WeekDay,
                    MonthDay = CampaignReportScheduling.MonthDay,
                    AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                    EmailSubject = CampaignReportScheduling.EmailSubject,
                    PreferedName = CampaignReportScheduling.PreferedName,
                    IsActive = CampaignReportScheduling.IsActive,
                    ID = CampaignReportScheduling.Id,
                    IsSunday = CampaignReportScheduling.IsSunday,
                    IsMonday = CampaignReportScheduling.IsMonday,
                    IsTuesday = CampaignReportScheduling.IsTuesday,
                    IsWednesday = CampaignReportScheduling.IsWednesday,
                    IsThursday = CampaignReportScheduling.IsThursday,
                    IsFriday = CampaignReportScheduling.IsFriday,

                    //ReportCriteriaDto
                    ReportDto = new ReportCriteriaDto
                    {
                        SummaryBy = CampaignReportScheduling.SummaryBy,
                        Layout = CampaignReportScheduling.Layout,
                        ItemsList = CampaignReportScheduling.ItemsList,
                        AdvancedCriteria = CampaignReportScheduling.AdvancedCriteria,
                        MetricCode = CampaignReportScheduling.MetricCode,
                        CampaignType = CampaignReportScheduling.CampaignType,
                        GroupByName = CampaignReportScheduling.GroupByName,
                        IsAccumulated = CampaignReportScheduling.IsAccumulated,
                        TabId = CampaignReportScheduling.TabId,
                        DeviceCategory = CampaignReportScheduling.DeviceCategory,
                        CriteriaOpt = CampaignReportScheduling.CriteriaOpt,
                        ToDate = new DateTime(CampaignReportScheduling.ToDate.Year, CampaignReportScheduling.ToDate.Month, CampaignReportScheduling.ToDate.Day, 0, 0, 0),
                        FromDate = new DateTime(CampaignReportScheduling.FromDate.Year, CampaignReportScheduling.FromDate.Month, CampaignReportScheduling.FromDate.Day, 23, 59, 0),
                        AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value
                    }
                };

                id = _ReportService.SaveSchadulingReport(reportSchedulerDto);
                message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), string.Empty);
                result = true;
            }
            catch (Exception ex)
            {
                message = ResourcesUtilities.GetResource("Exception", "Global");
            }

            return Json(new { Result = result, Message = message, id = id });
        }

        private CampaignReportSchedulingViewModel GetCampaignSchadulingReportModel(int? id, string reportType = "ad")
        {

            CampaignReportSchedulingViewModel model = new CampaignReportSchedulingViewModel();
            var RecurrenceTypeSelection = RecurrenceType.Month;
            int MonthSelection = 1, DaysSelection = 1;
            if (id.HasValue && id > 0)
            {
                model.ReportSchedulerDto = _ReportService.GetSchadulingReport((int)id);
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                DaysSelection = (int)model.ReportSchedulerDto.WeekDay;
            }
            else
            {
                model.ReportSchedulerDto = new ReportSchedulerDto();
                model.RecipientEmail = new List<string>();
                model.ReportSchedulerDto.ReportDto = new ReportCriteriaDto();
                model.ReportSchedulerDto.Name = ResourcesUtilities.GetResource("ScheduleNameDefault", "Report");
                model.ReportSchedulerDto.ReportDto.FromDate = DateTime.Now;
                model.ReportSchedulerDto.ReportDto.ToDate = DateTime.Now;
                model.ReportSchedulerDto.DateRecurrenceType = DateRecurrenceType.Today;
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 4;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = DateTime.Now;
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = RecurrenceType.Month.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month },new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=RecurrenceType.Week.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}, new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=RecurrenceType.Day.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };
            return model;
        }

        public ActionResult CampaignReport(int? id)
        {
            var campains = new TreeViewModel();
            campains = new TreeViewModel()
            {
                Url = Url.Action("GetAdvertiserItems", "ReportsTest", new { type = "", id = id }),
                Name = "AdsList",
                Id = "AdsList",
                IsAjax = true
            };
            int Id = id.HasValue ? (int)id : 0;

            campains.CampaignReportSchaduling = GetCampaignSchadulingReportModel(Id);
            campains.Url = Url.Action("GetAdvertiserItems", "ReportsTest", new { type = campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto.TabId, id = id });
            return PartialView(campains);
        }

        //public ActionResult CampaignReportExport(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string groupByName, string exportType)
        //{
        //    List<CampaignCommonReportDto> reportingList;
        //    int counter;

        //    bool result = GetCampaignReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, "1", int.MaxValue, orderBy, groupByName, out reportingList, out counter);

        //    return _WriteReportHelper.BuildCampaignReportFile(reportingList, exportType);
        //}

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult CampaignReport(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string groupByName)
        {
            List<CampaignCommonReportDto> reportingList;
            int counter;
            bool result = false;
            try
            {

                result = GetCampaignReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, page, 10, orderBy, groupByName, out reportingList, out counter);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("Exception", "Global"));
            }

            if (result)
            {
                return View(new GridModel
                {
                    Data = reportingList,
                    Total = counter
                });
            }
            else
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("FromDateandToDate", "Errors"));
            }
        }

        public ActionResult CampaignChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
        }

        public ActionResult GenerateCampaignChart(string metricCode, string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string groupByName)
        {
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {
                string color = _MetricService.GetByCode(metricCode).Color;

                List<ChartDto> chartDtoList;
                chartDtoList = GetCampaignChartData(fromdate, toDate, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, metricCode);


                return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return new EmptyResult();
            }

        }



        public ActionResult GCampaignChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
        }

        public JsonResult GGenerateCampaignChart(string metricCode, string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string groupByName)
        {
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {
                string color = _MetricService.GetByCode(metricCode).Color;

                List<ChartDto> chartDtoList;
                chartDtoList = GetCampaignChartData(fromdate, toDate, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, metricCode);
                var googleChartresult = new GoogleChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
                googleChartresult.ExecuteResult();
                return Json(googleChartresult, JsonRequestBehavior.AllowGet);

                // return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return null;
            }

        }

        public ActionResult GetAdvertiserItems(string type, int? id)
        {
            List<TreeDto> adItems;
            if (!string.IsNullOrEmpty(type))
            {
                type = type.ToLower();
            }
            switch (type)
            {
                case "ad":
                    adItems = _CampaignsService.GetAdsTree().ToList();
                    break;
                case "adgroup":
                    adItems = _CampaignsService.GetAdGroupsTree().ToList();
                    break;
                case "campaign":
                    adItems = _CampaignsService.GetCampaignsTree().ToList();
                    break;
                default:
                    adItems = _CampaignsService.GetAdsTree().ToList();
                    break;
            }

            var adList = TreeModel.GetTreeNodes(adItems);
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = id.HasValue & id > 0 ? FillTree(adList, (int)id) : adList;

            return result;
        }


        private IList<TreeModel> FillTree(IList<TreeModel> adList, int id)
        {
            var Criteria = _ReportService.GetSchadulingReport(id);

            if (Criteria != null)
            {
                var ReportDto = Criteria.ReportDto;
                var itemlist = !string.IsNullOrEmpty(ReportDto.ItemsList) ? ReportDto.ItemsList.Split(',').ToList() : new List<string>();

                foreach (string ID in itemlist)
                {
                    foreach (var x in adList)// Campaigns 
                    {
                        if (x.attributes.id == ID)
                        {
                            x.attributes.selected = true;
                        }
                        else if (x.children.Count > 0)
                        {
                            foreach (var y in x.children)// Adgruops
                            {
                                if (y.attributes.id == ID)
                                {
                                    y.attributes.selected = true;
                                }
                                else if (y.children.Count > 0)
                                {
                                    foreach (var w in y.children)// Ads
                                    {
                                        if (w.attributes.id == ID)
                                        {
                                            w.attributes.selected = true;
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }

            return adList;
        }

        public ActionResult AppReport(int? id)
        {
            var apps = new TreeViewModel()
            {
                Url = Url.Action("GetAppsTree", "ReportsTest", new { id = id }),
                Name = "AdsList",
                Id = "AdsList",
                IsAjax = true
            };
            int Id = id.HasValue ? (int)id : 0;

            apps.CampaignReportSchaduling = GetCampaignSchadulingReportModel(Id, "app");
            return PartialView(apps);
        }
        public ActionResult GetAppsTree(int? id)
        {
            List<TreeDto> appItems;

            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            appItems = _appsiteService.GetAppSitesTreeByAccountId(accountId);

            var appList = TreeModel.GetTreeNodes(appItems);
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = appList;
            if (id.HasValue & id > 0)
            {
                var items = FillTree(appList, (int)id);
                result.Data = items != null ? items : appList;
            }
            return result;
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult AppReports(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy)
        {
            List<AppCommonReportDto> reportingList;
            int counter;
            bool result = false;

            try
            {
                result = GetAppReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, page, 10, orderBy, out reportingList, out counter);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("Exception", "Global"));
            }

            if (result)
            {
                return View(new GridModel
                {
                    Data = reportingList,
                    Total = counter
                });
            }
            else
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("FromDateandToDate", "Errors"));
            }
        }

        //public ActionResult AppReportExport(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string exportType)
        //{
        //    //List<AppCommonReportDto> reportingList;
        //    //int counter;

        //    //bool result = GetAppReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, "1", int.MaxValue, orderBy, out reportingList, out counter);

        //    //return _WriteReportHelper.BuildAppReportFile(reportingList, exportType);
        //}

        public ActionResult AppChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
        }


        public ActionResult GenerateAppChart(string metricCode, string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId)
        {
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {
                string color = _MetricService.GetByCode(metricCode).Color;

                List<ChartDto> chartDtoList;
                chartDtoList = GetAppChartData(fromdate, toDate, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, metricCode);


                return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return new EmptyResult();
            }

        }


        public ActionResult GAppChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
        }


        public JsonResult GGenerateAppChart(string metricCode, string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId)
        {
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {
                string color = _MetricService.GetByCode(metricCode).Color;

                List<ChartDto> chartDtoList;
                chartDtoList = GetAppChartData(fromdate, toDate, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, metricCode);


                var googleChartresult = new GoogleChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
                googleChartresult.ExecuteResult();
                return Json(googleChartresult, JsonRequestBehavior.AllowGet);
                // return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return null;
            }

        }

        #region Private Members

        private List<ChartDto> GetCampaignChartData(string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string metricCode)
        {
            List<ChartDto> chartDtoList = new List<ChartDto>();

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {

                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.FromDate = Convert.ToDateTime(fromdate);
                criteriaDto.ToDate = Convert.ToDateTime(toDate);
                criteriaDto.MetricCode = metricCode;
                criteriaDto.CampaignType = CampaignType.Normal;

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
                        chartDtoList = _ReportService.GetCampaignReportChart(criteriaDto);
                        break;
                    case "adgroup":
                        chartDtoList = _ReportService.GetAdGroupReportChart(criteriaDto);
                        break;
                    case "ad":
                        chartDtoList = _ReportService.GetAdReportChart(criteriaDto);
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
                        chartDtoList = _ReportService.GetCampaignOperatorReportChart(criteriaDto);
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
                                chartDtoList = _ReportService.GetCampaignPlatformReportChart(criteriaDto);
                                break;
                            case "manufactor":
                                chartDtoList = _ReportService.GetCampaignManuFactorReportChart(criteriaDto);
                                break;
                            default:
                                chartDtoList = _ReportService.GetCampaignManuFactorReportChart(criteriaDto);
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
                        chartDtoList = _ReportService.GetCampaignGeoLocationReportChart(criteriaDto);
                        break;
                    default:
                        break;
                }
            }

            return chartDtoList;
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

        private bool GetCampaignReportData(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, int itemsPerPage, string orderBy, string groupByName, out List<CampaignCommonReportDto> reportingList, out int counter)
        {

            if (!(string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(toDate)))
            {
                string orderByColumn, orderType;
                GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.FromDate = Convert.ToDateTime(fromdate);
                criteriaDto.ToDate = Convert.ToDateTime(toDate);
                criteriaDto.SummaryBy = int.Parse(summaryBy);
                criteriaDto.Layout = layout;
                criteriaDto.ItemsPerPage = itemsPerPage;
                criteriaDto.PageNumber = (string.IsNullOrEmpty(page) ? 0 : int.Parse(page) - 1);
                criteriaDto.OrderType = orderType;
                criteriaDto.OrderColumn = orderByColumn;

                criteriaDto.GroupByName = string.IsNullOrEmpty(groupByName) ? false : bool.Parse(groupByName);

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
                        reportingList = _ReportService.GetCampaignReport(criteriaDto);
                        counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignReport(criteriaDto);
                        break;
                    case "adgroup":
                        reportingList = _ReportService.GetAdGroupReport(criteriaDto);
                        counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalAdGroupReport(criteriaDto);
                        break;
                    case "ad":
                        reportingList = _ReportService.GetAdReport(criteriaDto);
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
                        reportingList = _ReportService.GetCampaignOperatorReport(criteriaDto);
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
                                reportingList = _ReportService.GetCampaignPlatformReport(criteriaDto);
                                counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignPlatformReport(criteriaDto);
                                break;
                            case "manufactor":
                                reportingList = _ReportService.GetCampaignManuFactorReport(criteriaDto);
                                counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalCampaignManuFactorReport(criteriaDto);
                                break;
                            default:
                                reportingList = _ReportService.GetCampaignManuFactorReport(criteriaDto);
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
                        reportingList = _ReportService.GetCampaignGeoLocationReport(criteriaDto);
                        counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//_ReportService.GetTotalCampaignGeoLocationReport(criteriaDto);
                        break;
                    default:
                        reportingList = new List<CampaignCommonReportDto>();
                        counter = 0;
                        break;
                }

                return true;
            }
            else
            {
                counter = 0;
                reportingList = new List<CampaignCommonReportDto>();
                return false;
            }
        }

        private bool GetAppReportData(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, int itemsPerPage, string orderBy, out List<AppCommonReportDto> reportingList, out int counter)
        {


            if (!(string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(toDate)))
            {
                string orderByColumn, orderType;
                GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.FromDate = Convert.ToDateTime(fromdate);
                criteriaDto.ToDate = Convert.ToDateTime(toDate);
                criteriaDto.SummaryBy = int.Parse(summaryBy);
                criteriaDto.Layout = layout;
                criteriaDto.ItemsPerPage = itemsPerPage;
                criteriaDto.PageNumber = (string.IsNullOrEmpty(page) ? 0 : int.Parse(page) - 1);
                criteriaDto.OrderType = orderType;
                criteriaDto.OrderColumn = orderByColumn;

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
                        reportingList = _ReportService.GetAppReport(criteriaDto);
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

                        reportingList = _ReportService.GetAppOperatorReport(criteriaDto);
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
                                reportingList = _ReportService.GetAppPlatformReport(criteriaDto);
                                counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//  _ReportService.GetTotalAppPlatformReport(criteriaDto);
                                break;
                            case "manufactor":
                                reportingList = _ReportService.GetAppManuFactorReport(criteriaDto);
                                counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;//  _ReportService.GetTotalAppManuFactorReport(criteriaDto);
                                break;
                            default:
                                reportingList = _ReportService.GetAppManuFactorReport(criteriaDto);
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


                        reportingList = _ReportService.GetAppGeoLocationReport(criteriaDto);
                        counter = reportingList.FirstOrDefault() == null ? 0 : (int)reportingList.First().TotalCount;// _ReportService.GetTotalAppGeoLocationReport(criteriaDto);
                        break;
                    default:
                        reportingList = new List<AppCommonReportDto>();
                        counter = 0;
                        break;
                }

                return true;
            }
            else
            {
                counter = 0;
                reportingList = new List<AppCommonReportDto>();
                return false;
            }
        }


        private List<ChartDto> GetAppChartData(string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string metricCode)
        {
            List<ChartDto> chartDtoList = new List<ChartDto>();

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {

                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.FromDate = Convert.ToDateTime(fromdate);
                criteriaDto.ToDate = Convert.ToDateTime(toDate);
                criteriaDto.MetricCode = metricCode;

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
                        chartDtoList = _ReportService.GetAppReportChart(criteriaDto);
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
                        chartDtoList = _ReportService.GetAppOperatorReportChart(criteriaDto);
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
                                chartDtoList = _ReportService.GetAppPlatformReportChart(criteriaDto);
                                break;
                            case "manufactor":
                                chartDtoList = _ReportService.GetAppManuFactorReportChart(criteriaDto);
                                break;
                            default:
                                chartDtoList = _ReportService.GetAppManuFactorReportChart(criteriaDto);
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
                        chartDtoList = _ReportService.GetAppGeoLocationReportChart(criteriaDto);
                        break;
                    default:
                        break;
                }
            }

            return chartDtoList;
        }

        #endregion

        #region ReportJobMaster
        public string testReporSchduling()
        {

            ReportCriteriaDto dsdsd = new ReportCriteriaDto();
            dsdsd.MetricCode = "fdfdf";
            dsdsd.TabId = "Opt";
            dsdsd.FromDate = DateTime.Now;
            dsdsd.ToDate = DateTime.Now.AddDays(-20);
            List<ReportRecipientDTO> AllReportRecipient = new List<ReportRecipientDTO>();
            AllReportRecipient.Add(new ReportRecipientDTO { Email = "anashantash@yahoo.com" });
            AllReportRecipient.Add(new ReportRecipientDTO { Email = "anasa@noqoush.com" });

            ReportSchedulerDto testob = new ReportSchedulerDto
            {
                Name = "fdfsdf4344",
                AllReportRecipient = AllReportRecipient,
                AccountId = 87062,
                TimeSentAt = DateTime.Now.AddMinutes(10),
                ReportDto = dsdsd,
                StartDate = DateTime.Now,
                RecurrenceType = RecurrenceType.Week,
                ReportSectionType = ReportSectionType.Publisher,
                WeekDay = WeekDay.Monday,
            };
            _ReportService.SaveSchadulingReport(testob);
            var results = _ReportService.GetSchadulingReport(80001);
            return "done";
        }
        public ActionResult IndexReportsJob(string reportType)
        {
            ReportSectionType reptSec;
            if (reportType == "ad")
            {

                reptSec = ReportSectionType.Advertiser;
            }
            else
            {
                reptSec = ReportSectionType.Publisher;
            }
            Noqoush.AdFalcon.Web.Controllers.Model.Report.ListViewModel lis = LoadReportSchedulerData(null, reptSec);
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Titles", "JobGrid"),
                Order = 1,
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            return View("IndexReportsJob", lis);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IndexReportsJob(string reportType, int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete 
                _ReportService.DeleteSchadulingReportBulk(checkedRecords);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
                {
                    //run  selected 
                    _ReportService.RunSchadulingReport(checkedRecords);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
                    {
                        //pause selected 
                        _ReportService.PauseSchadulingReport(checkedRecords);
                    }
                    else
                        if (!string.IsNullOrWhiteSpace(Request.Form["Send"]))
                    {
                        _ReportService.SendSchadulingReport(checkedRecords);
                    }
                }
            }


            return RedirectToAction("IndexReportsJob", new { reportType = reportType });
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _IndexReportsJob(string reportType)
        {

            ReportSectionType reptSec;
            if (reportType == "ad" || Request.QueryString["reportType"] == "ad")
            {

                reptSec = ReportSectionType.Advertiser;
            }
            else
            {
                reptSec = ReportSectionType.Publisher;
            }
            var result = GetReportSchedulerQueryResult(null, reptSec);
            return View("IndexReportsJob", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        protected Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter getDefualtFilter()
        {
            Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            //filter.StatusId = string.IsNullOrWhiteSpace(Request.Form["StatusId"]) ? (int?)null : Convert.ToInt32(Request.Form["StatusId"]);

            return filter;
        }
        protected ReportSchedulerCriteria GetReportSchedulerCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new ReportSchedulerCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected ResultReportSchedulerDto GetReportSchedulerQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter filter, ReportSectionType sec)
        {
            var criteria = GetReportSchedulerCriteria(filter);
            criteria.ReportSectionType = sec;
            var result = _ReportService.QueryByCratiriaForReportSchaduling(criteria);
            if (result.Items != null)
            {
                var GMT = ResourcesUtilities.GetResource("UTC", "Global");

                foreach (var item in result.Items)
                {
                    item.LastRunningDateString = item.LastRunningDate.HasValue ? item.LastRunningDate.Value.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) + " " + item.LastRunningDate.Value.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.TimeFormat) + " " + GMT : "";
                    item.EndDateString = item.EndDate.HasValue ? item.EndDate.Value.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) : "";
                }
            }
            return result;
        }
        protected Noqoush.AdFalcon.Web.Controllers.Model.Report.ListViewModel LoadReportSchedulerData(Noqoush.AdFalcon.Web.Controllers.Model.Report.Filter filter, ReportSectionType sec)
        {
            var result = GetReportSchedulerQueryResult(filter, sec);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions
            var actions = GetReportSchedulerActions(sec);
            var toolTips = GetReportSchedulerTooltips(sec);
            #endregion

            //load the statues 
            //var statues = _adGroupStatusService.GetAll();
            //var statuesDropDown = GetSelectList();
            //foreach (var item in statues)
            //{
            //    var selectItem = new SelectListItem();
            //    selectItem.Value = item.ID.ToString();
            //    selectItem.Text = item.Name.ToString();
            //    statuesDropDown.Add(selectItem);
            //}
            return new Noqoush.AdFalcon.Web.Controllers.Model.Report.ListViewModel()
            {

                Items = items,

                TopActions = actions,
                BelowAction = actions,
                ToolTips = toolTips

            };
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetReportSchedulerTooltips(ReportSectionType sec)
        {
            // Create the tool tip actions
            string reportType = "ad";
            if (sec == ReportSectionType.Publisher)
            {
                reportType = "app";
            }
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Index",
                            ControllerName="ReportsTest",
                            ExtraPrams3 = reportType

                        },

                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Document", "JobGrid"),
                            ClassName = "grid-tool-tip-reports",
                            ActionName = "DownloadReport",
                            ExtraPrams = "documentDownload",
                            Type = ActionType.ajax,
                            AjaxType=AjaxType.Download,
                            CallBack = ""
                        },
                            new AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-copy",
                            ActionName = "RedirectToAuditTrial",

                        }
                };
            return toolTips;
        }
        public virtual ActionResult RedirectToAuditTrial(int id)
        {
            string originalPath = new Uri(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri).OriginalString;

            try
            {
                int objectRootTypeId = _accountService.GetObjectRootTypeId("Noqoush.AdFalcon.Domain.Common.Model.Core.ReportRecipient");

                return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = id, objectRootTypeId = objectRootTypeId, returnUrl = originalPath });
            }
            catch (Exception e)
            {

                throw e;
            }


        }
        public ActionResult DownloadReport(int Docid)
        {


            return Content(ResourcesUtilities.GetResource("CloneAdGroupError", "Errors"));

        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetReportSchedulerActions(ReportSectionType sec)
        {
            string reportType = "ad";
            if (sec == ReportSectionType.Publisher)
            {
                reportType = "app";
            }
            // create the actions
            var actions = new List<AdFalcon.Web.Controllers.Model.Action>
                {
                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("ActivateNow", "Report"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Activate", "Confirmation") // like are u sure ?

                        },
                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?
                        },
                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?
                        },
                           new AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Send",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("RunNow", "Report"),
                            ExtraPrams = ResourcesUtilities.GetResource("SelectConfirmation", "Report"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Send", "Confirmation") // like are u sure ?

                        },
                    new AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Index",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewReportSchedule", "Commands"),
                            ExtraPrams = reportType,
                            ExtraPrams3="report"
                        }

                };
            return actions;
        }
        #endregion

    }

   

}
