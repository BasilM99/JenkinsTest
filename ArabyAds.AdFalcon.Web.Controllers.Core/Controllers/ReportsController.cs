using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using Telerik.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Drawing;
using ArabyAds.AdFalcon.Web.Core.Helper;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;

using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Web.Controllers.Model.Report;
using Telerik.Web.Mvc.UI;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Telerik.Web.Mvc.Extensions;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]

    public class ReportsController : AuthorizedControllerBase
    {
        private IAppSiteService _appsiteService;
        private IMetricService _MetricService;
        private IReportService _ReportService;
        private ICampaignService _CampaignsService;
        protected IAccountService _accountService;
        protected IAudienceSegmentService _AudienceSegmentService;
        private WriteReportDocumentsHelper _WriteReportHelper;

        public ReportsController()
        {
            _AudienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
            _appsiteService = IoC.Instance.Resolve<IAppSiteService>();
            _MetricService = IoC.Instance.Resolve<IMetricService>();
            _ReportService = IoC.Instance.Resolve<IReportService>();
            _CampaignsService = IoC.Instance.Resolve<ICampaignService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

            _WriteReportHelper = new WriteReportDocumentsHelper();
        }


        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = Code }).Value;

            return result;
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
            if (!string.IsNullOrEmpty(Request.Query["id"]))
            {
                var Job = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = Convert.ToInt32(Request.Query["id"]) });

                if (Job.IsForQueryBuilder)
                {

                    return RedirectToAction("Filter", "Filter", new { id = Job.ID });
                }
            }

            if (!string.IsNullOrEmpty(Request.Query["Id"]))
            {
                var Job = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = Convert.ToInt32(Request.Query["Id"]) });

                if (Job.IsForQueryBuilder)
                {

                    return RedirectToAction("Filter", "Filter", new { id = Job.ID });
                }
            }

            if (!string.IsNullOrEmpty(Request.Query["ID"]))
            {
                var Job = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = Convert.ToInt32(Request.Query["ID"]) });

                if (Job.IsForQueryBuilder)
                {

                    return RedirectToAction("Filter", "Filter", new { id = Job.ID });
                }
            }
            return View();
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCampaignReport(CampaignReportSchedulingSaveModel CampaignReportScheduling)
        {
            string message = "";
            bool result = false;
            int id = 0;


            IList<int> TempColumns = new List<int>();
            if (!string.IsNullOrEmpty(CampaignReportScheduling.metriceColumns))
            {

                var stringColumnsArray = CampaignReportScheduling.metriceColumns.Split(',');
                foreach (string i in stringColumnsArray)
                {
                    if (i != "")
                    {
                        TempColumns.Add(Convert.ToInt32(i));

                    }
                }
            }
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
                    MatixColumns = TempColumns,
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
                        AdvertiserId = CampaignReportScheduling.AdvertiserId,
                        AccountAdvertiserId = CampaignReportScheduling.AccountAdvertiserId,
                        CampaignType = CampaignReportScheduling.CampaignType,
                        GroupByName = CampaignReportScheduling.GroupByName,
                        IsAccumulated = CampaignReportScheduling.IsAccumulated,
                        TabId = CampaignReportScheduling.TabId,
                        DeviceCategory = CampaignReportScheduling.DeviceCategory,
                        CriteriaOpt = CampaignReportScheduling.CriteriaOpt,
                        ToDate = new DateTime(CampaignReportScheduling.ToDate.Year, CampaignReportScheduling.ToDate.Month, CampaignReportScheduling.ToDate.Day, 23, 59, 0),
                        FromDate = new DateTime(CampaignReportScheduling.FromDate.Year, CampaignReportScheduling.FromDate.Month, CampaignReportScheduling.FromDate.Day, 0, 0, 0),
                        AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                        /*ColumnsIdsString = CampaignReportScheduling.ColumnsIdsString,
                        MeasuresIdsString = CampaignReportScheduling.MeasuresIdsString,
                        fact = CampaignReportScheduling.fact,
                        QueryJsonData = CampaignReportScheduling.QueryJsonData*/

                    }
                };

                id = _ReportService.SaveSchadulingReport(reportSchedulerDto).Value;
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
            IList<int> ColumnsToBeSelected = new List<int>();

            if (id.HasValue && id > 0)
            {
                model.ReportSchedulerDto = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = (int)id });
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                DaysSelection = (int)model.ReportSchedulerDto.WeekDay;
            }
            else
            {
                model.ReportSchedulerDto = new ReportSchedulerDto();
                model.RecipientEmail = new List<string>();
                model.ReportSchedulerDto.ReportDto = new ReportCriteriaDto();
                model.ReportSchedulerDto.Name = ResourcesUtilities.GetResource("ScheduleNameDefault", "Report");
                model.ReportSchedulerDto.ReportDto.FromDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.ReportDto.ToDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.DateRecurrenceType = DateRecurrenceType.Today;
                model.ReportSchedulerDto.ReportDto.FromDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 4;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = RecurrenceType.Month.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month },new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=RecurrenceType.Week.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}, new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=RecurrenceType.Day.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };

            return model;
        }
        [DenyRole(Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
        public ActionResult CampaignReport(int? id)
        {
            var campains = new Model.Tree.TreeViewModel();
            campains = new Model.Tree.TreeViewModel()
            {
                Url = Url.Action("GetAdvertiserItems", "Reports", new { type = "", id = id }),
                Name = "AdsList",
                Id = "AdsList",
                IsAjax = true
            };
            int Id = id.HasValue ? (int)id : 0;
            campains.ShowAudienceSegmentUsage = false;
            var data = _AudienceSegmentService.GetAll(null);

            if (data != null && data.Count > 0)
            { campains.ShowAudienceSegmentUsage = true; }
            campains.CampaignReportSchaduling = GetCampaignSchadulingReportModel(Id);
            if (Config.IsAdOpsAdminInAdminApp || Config.IsAppOpsAdminInAdminApp)
                ViewBag.SchadulingReportAllowed = true;
            else
                ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = PortalPermissionsCode.ReportSchedule }).Value || Config.IsAppOps;
            campains.Url = Url.Action("GetAdvertiserItems", "Reports", new { type = campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto.TabId, id = id });
            if (campains.CampaignReportSchaduling.ReportSchedulerDto != null && campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto != null && campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto.AccountAdvertiserId > 0)
                campains.Url = Url.Action("GetAdvertiserItems", "Reports", new { type = campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto.TabId, id = id, AdvertiserId = campains.CampaignReportSchaduling.ReportSchedulerDto.ReportDto.AccountAdvertiserId });
            campains.ColumnViewModel = new Model.Report.ColumnViewModel();
            campains.ColumnViewModel = GetColumnViewModel("ad");
            campains.ColumnViewModel.SelectableColumns = campains.ColumnViewModel.Columns.Where(x => !x.Hide).Count();
            if (id.HasValue)
            {

                campains.ReportTempName = campains.CampaignReportSchaduling.ReportSchedulerDto.Name;
                List<int> MatixColumns = campains.CampaignReportSchaduling.ReportSchedulerDto.MatixColumns.ToList();
                if (MatixColumns != null && MatixColumns.Count > 0)
                {
                    if (campains.ColumnViewModel.Columns != null)
                    {
                        foreach (var column in campains.ColumnViewModel.Columns)
                        {
                            column.IsSelected = false;

                        }
                    }

                    foreach (var columnMarix in MatixColumns)
                    {
                        var ColumnOb = campains.ColumnViewModel.Columns.Where(M => M.Id == columnMarix).SingleOrDefault();
                        if (ColumnOb != null)
                        {
                            ColumnOb.IsSelected = true;

                        }
                    }
                }

            }
            ViewBag.AdvertiserAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Advertisers_Name",
                Name = "Advertisers.Name",
                ActionName = "GetAccountAdvertisers",
                ControllerName = "Advertiser",
                LabelExpression = "item.Name",
                ValueExpression = @"item.AdvertiserAccId",
                IsAjax = true,
                IsRequired = false,
                UseName = true,
                ChangeCallBack = "ReportAdvertisersChanged",
                CurrentText = campains.CampaignReportSchaduling.ReportSchedulerDto.AdvertiserAccountName,
                PlaceHolder = ResourcesUtilities.GetResource("SelectAdvertiserRequired", "Advertiser")
            };
            return PartialView(campains);
        }



        private ColumnViewModel GetColumnViewModel(string ReportType)
        {

            var ColumnViewModel = new Model.Report.ColumnViewModel();

            if (ReportType.ToLower() == "ad")
                ColumnViewModel.Columns = _ReportService.GetmetriceColumnsForAdvertiser();
            else
                ColumnViewModel.Columns = _ReportService.GetmetriceColumnsForPublisher();

            return ColumnViewModel;
        }
        public ActionResult CampaignReportExport(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string groupByName, string exportType, string metriceColumns, string AdvertiserId, string AccountAdvertiserId)
        {
            List<CampaignCommonReportDto> reportingList;
            int counter;

            bool result = GetCampaignReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, "1", int.MaxValue, orderBy, groupByName, AdvertiserId, AccountAdvertiserId, out reportingList, out counter);
            string name = string.Format("{0}_{1}_{2}", fromdate.ToString(), toDate.ToString(), tabId);
            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value = ReturnCampaingReportTitle(tabId,deviceCategory)},
              new KeyValueDto(){Key = "Date From" , Value = String.Format("{0:dd-MM-yyyy}", fromdate)},
             new KeyValueDto(){Key = "Date To" , Value = String.Format("{0:dd-MM-yyyy}", toDate)}

            };
            return _WriteReportHelper.BuildReportFile(reportingList, metriceColumns, exportType, keyValueDtosList, name);
        }

        //*******************************************************************************************************************************************************
        private String ReturnCampaingReportTitle(string tabId, string deviceCategory)
        {

            string title = "";
            switch (tabId.ToLower())
            {
                case "campaign":
                    title = ResourcesUtilities.GetResource("campaignReportTitle", "Global");
                    break;
                case "adgroup":
                    title = ResourcesUtilities.GetResource("adgroupReportTitle", "Global");
                    break;
                case "ad":
                    title = ResourcesUtilities.GetResource("adReportTitle", "Global");
                    break;
                case "subappsite":
                    title = ResourcesUtilities.GetResource("subappsiteReportTitle", "Global");

                    break;

                case "operator":
                    title = ResourcesUtilities.GetResource("CampoperatorReportFile", "Global");
                    break;
                case "devicemodel":
                    {
                        title = ResourcesUtilities.GetResource("devicemodelReportCampaignFile", "Global");

                        switch (deviceCategory.ToLower())
                        {
                            case "platform":
                                title = ResourcesUtilities.GetResource("CampPlatformReportFile", "Global");
                                break;
                            case "manufactor":
                                title = ResourcesUtilities.GetResource("CampManufacturerReportFile", "Global");
                                break;
                            default:
                                title = ResourcesUtilities.GetResource("devicemodelReportCampaignFile", "Global");
                                break;
                        }
                    }
                    break;
                case "geolocation":
                    title = ResourcesUtilities.GetResource("geolocationReportCampaignFile", "Global");
                    break;

                case "audiancesegmentforadvertiser":
                    title = ResourcesUtilities.GetResource("audiancesegmentforadvertiserReportTitle", "Global");
                    break;
                default:
                    break;
            }
            return title;
        }


        private String ReturnPuplisherReportTitle(string tabId, string deviceCategory)
        {
            string title = "";
            switch (tabId.ToLower())
            {
                case "app":
                    title = ResourcesUtilities.GetResource("appReportFile", "Global");
                    break;
                case "operator":

                    title = ResourcesUtilities.GetResource("operatorReportFile", "Global");
                    break;
                case "devicemodel":
                    title = ResourcesUtilities.GetResource("devicemodelReportFile", "Global");

                    switch (deviceCategory.ToLower())
                    {
                        case "platform":
                            title = ResourcesUtilities.GetResource("PlatformReportFile", "Global");
                            break;
                        case "manufactor":
                            title = ResourcesUtilities.GetResource("ManufacturerReportFile", "Global");
                            break;
                        default:
                            title = ResourcesUtilities.GetResource("devicemodelReportFile", "Global");
                            break;
                    }
                    break;

                case "geolocation":
                    title = ResourcesUtilities.GetResource("geolocationReportFile", "Global");
                    break;
            }
            return title;
        }

        //*******************************************************************************************************************************************************

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult CampaignReport(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string groupByName, string AdvertiserId, string AccountAdvertiserId)
        {
            List<CampaignCommonReportDto> reportingList;
            int counter;
            bool result = false;
            try
            {

                result = GetCampaignReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, page, 10, orderBy, groupByName, AdvertiserId, AccountAdvertiserId, out reportingList, out counter);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("Exception", "Global"));
            }

            if (result)
            {
                return Json(new GridModel
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


        public ActionResult CampaignGrid(string ColumnIds)
        {
            GridReportModel Model = new GridReportModel();
            int[] Columns = null;
            IList<int> TempColumns = new List<int>();
            if (!string.IsNullOrEmpty(ColumnIds))
            {

                var stringColumnsArray = ColumnIds.Split(',');
                foreach (string i in stringColumnsArray)
                {
                    if (i != "")
                    {
                        TempColumns.Add(Convert.ToInt32(i));

                    }
                }
            }
            if (TempColumns != null
                 && TempColumns.Count > 0)
            {
                Columns = TempColumns.ToArray();
            }
            var ColumnsData = _ReportService.GetmetriceColumnsForAdvertiser();
            if (Columns != null && Columns.Length > 0)
            {

                ColumnsData = ColumnsData.Where(M => Columns.Contains(M.Id)).ToList();
            }
            else
            {
                ColumnsData = ColumnsData.Where(M => M.IsSelected == true).ToList();



            }
           Model.GridColumnSettings = new List<Kendo.Mvc.UI.GridColumnSettings>(); 
            foreach (var ColumnData in ColumnsData)
            {

                Model.GridColumnSettings.Add(new Kendo.Mvc.UI.GridColumnSettings 
                {
                    Member = ColumnData.AppFieldName,
                    Title = ResourcesUtilities.GetResource(ColumnData.HeaderResourceKey, ColumnData.HeaderResourceSet),
                    Format = ColumnData.Format
                });
            }

            return PartialView(Model);
        }
        public ActionResult testGrid(string ColumnIds)
        {
            GridReportModel Model = new GridReportModel();
            int[] Columns = null;
            IList<int> TempColumns = new List<int>();
            if (!string.IsNullOrEmpty(ColumnIds))
            {

                var stringColumnsArray = ColumnIds.Split(',');
                foreach (string i in stringColumnsArray)
                {
                    if (i != "")
                    {
                        TempColumns.Add(Convert.ToInt32(i));

                    }
                }
            }
            if (TempColumns != null
                 && TempColumns.Count > 0)
            {
                Columns = TempColumns.ToArray();
            }
            var col1 = _ReportService.GetColumn(new ValueMessageWrapper<int> { Value = 1 });
            var col2 = _ReportService.GetColumn(new ValueMessageWrapper<int> { Value = 2 });
            var col3 = _ReportService.GetColumn(new ValueMessageWrapper<int> { Value = 14 });
            List<metriceColumnDto> cols = new List<metriceColumnDto>();
            cols.Add(col1);
            cols.Add(col2);
            cols.Add(col3);

            var ColumnsData = cols;
            if (Columns != null && Columns.Length > 0)
            {

                ColumnsData = ColumnsData.Where(M => Columns.Contains(M.Id)).ToList();
            }
            else
            {
                ColumnsData = ColumnsData.Where(M => M.IsSelected == true).ToList();



            }
            Model.GridColumnSettings = new List<Kendo.Mvc.UI.GridColumnSettings>(); 
            foreach (var ColumnData in ColumnsData)
            {

                Model.GridColumnSettings.Add(new Kendo.Mvc.UI.GridColumnSettings
                {
                    Member = ColumnData.AppFieldName,
                    Title = ResourcesUtilities.GetResource(ColumnData.HeaderResourceKey, ColumnData.HeaderResourceSet),
                    Format = ColumnData.Format
                });
            }

            return PartialView(Model);
        }
        public ActionResult AppGrid(string ColumnIds)
        {
            GridReportModel Model = new GridReportModel();
            var ColumnsData = _ReportService.GetmetriceColumnsForPublisher();
            int[] Columns = null;
            IList<int> TempColumns = new List<int>();
            if (!string.IsNullOrEmpty(ColumnIds))
            {
                var stringColumnsArray = ColumnIds.Split(',');
                foreach (string i in stringColumnsArray)
                {
                    if (i != "")
                    {
                        TempColumns.Add(Convert.ToInt32(i));

                    }
                }
            }

            if (TempColumns != null
                 && TempColumns.Count > 0)
            {
                Columns = TempColumns.ToArray();
            }
            if (Columns != null && Columns.Length > 0)
            {

                ColumnsData = ColumnsData.Where(M => Columns.Contains(M.Id)).ToList();
            }
            else
            {
                ColumnsData = ColumnsData.Where(M => M.IsSelected == true).ToList();



            }
            Model.GridColumnSettings = new List<Kendo.Mvc.UI.GridColumnSettings>();
            foreach (var ColumnData in ColumnsData)
            {

                Model.GridColumnSettings.Add(new Kendo.Mvc.UI.GridColumnSettings
                {
                    Member = ColumnData.AppFieldName,
                    Title = ResourcesUtilities.GetResource(ColumnData.HeaderResourceKey, ColumnData.HeaderResourceSet),
                    Format = ColumnData.Format

                });
            }
            

            return PartialView(Model);
        }


        public ActionResult GCampaignChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
        }

        public JsonResult GGenerateCampaignChart(string metricCode, string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string groupByName, string AdvertiserId, string AccountAdvertiserId)
        {
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {
                string color = _MetricService.GetByCode(metricCode).Color;

                List<ChartDto> chartDtoList;
                chartDtoList = GetCampaignChartData(fromdate, toDate, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, metricCode, AdvertiserId, AccountAdvertiserId);
                var googleChartresult = new GoogleChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };

                if (tabId.ToLower() == "audiancesegmentforadvertiser")
                {
                    TimeSpan diff = googleChartresult.ToDate - googleChartresult.FromDate;
                    var dateDiff = googleChartresult.ToDate.Subtract(googleChartresult.FromDate).TotalDays;

                    if ((dateDiff >= 0) && (dateDiff < 7))
                    {
                        googleChartresult.ForWeek = true;

                    }
                }
                googleChartresult.ExecuteResult();
                return Json(googleChartresult);

                // return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return null;
            }

        }

        public ActionResult GetAdvertiserItems(string type, int? id, int? AdvertiserId)
        {
            List<TreeDto> adItems;
            if (!string.IsNullOrEmpty(type))
            {
                type = type.ToLower();
            }
            if (AdvertiserId.HasValue)
            {

                if (AdvertiserId == 0)
                {
                    AdvertiserId = null;
                }
            }
            switch (type)
            {
                case "ad":
                    adItems = _CampaignsService.GetAdsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "adgroup":
                    adItems = _CampaignsService.GetAdGroupsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "campaign":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "devicemodel":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "geolocation":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "operator":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "subappsite":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                case "audiancesegmentforadvertiser":
                    adItems = _CampaignsService.GetCampaignsAdvTree(new ValueMessageWrapper<int?> { Value = AdvertiserId }).ToList();
                    break;
                default:
                    adItems = _CampaignsService.GetAdsTree().ToList();
                    break;
            }

            var adList = TreeModel.GetTreeNodes(adItems);
            JsonResult result = new JsonResult(null);
            result.Value = id.HasValue & id > 0 ? FillTree(adList, (int)id) : adList;

            return result;
        }


        private IList<TreeModel> FillTree(IList<TreeModel> adList, int id)
        {
            var Criteria = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = id });

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
            var apps = new Model.Tree.TreeViewModel()
            {
                Url = Url.Action("GetAppsTree", "Reports", new { id = id }),
                Name = "AdsList",
                Id = "AdsList",
                IsAjax = true
            };
            int Id = id.HasValue ? (int)id : 0;
            //ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(PortalPermissionsCode.ReportSchedule) || Config.IsAdmin;

            if (Config.IsAdOpsAdminInAdminApp || Config.IsAppOpsAdminInAdminApp)
                ViewBag.SchadulingReportAllowed = true;
            else
                ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = PortalPermissionsCode.ReportSchedule }).Value || Config.IsAppOps;
            apps.CampaignReportSchaduling = GetCampaignSchadulingReportModel(Id, "app");

            apps.ColumnViewModel = new Model.Report.ColumnViewModel();
            apps.ColumnViewModel = GetColumnViewModel("app");
            apps.ColumnViewModel.SelectableColumns = apps.ColumnViewModel.Columns.Where(x => !x.Hide).Count();

            if (id.HasValue)
            {
                apps.ReportTempName = apps.CampaignReportSchaduling.ReportSchedulerDto.Name;
                List<int> MatixColumns = apps.CampaignReportSchaduling.ReportSchedulerDto.MatixColumns.ToList();
                if (MatixColumns != null && MatixColumns.Count > 0)
                {
                    if (apps.ColumnViewModel.Columns != null)
                    {
                        foreach (var column in apps.ColumnViewModel.Columns)
                        {
                            column.IsSelected = false;

                        }
                    }

                    foreach (var columnMarix in MatixColumns)
                    {
                        var ColumnOb = apps.ColumnViewModel.Columns.Where(M => M.Id == columnMarix).SingleOrDefault();
                        if (ColumnOb != null)
                        {
                            ColumnOb.IsSelected = true;

                        }
                    }
                }

            }


            return PartialView(apps);
        }
        public ActionResult GetAppsTree(int? id)
        {
            List<TreeDto> appItems;

            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            appItems = _appsiteService.GetAppSitesTreeByAccountId(new ValueMessageWrapper<int> { Value = accountId });

            var appList = TreeModel.GetTreeNodes(appItems);
            JsonResult result = new JsonResult(null);
            result.Value = appList;
            if (id.HasValue & id > 0)
            {
                var items = FillTree(appList, (int)id);
                result.Value = items != null ? items : appList;
            }
            return result;
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult AppReports(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy)
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
                return Json(new GridModel
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

        [HttpPost]
        public ActionResult AppReportExport(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string exportType, string metriceColumns)
        {
            List<AppCommonReportDto> reportingList;
            int counter;

            bool result = GetAppReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, "1", int.MaxValue, orderBy, out reportingList, out counter);
            string name = string.Format("{0}_{1}_{2}", fromdate.ToString(), toDate.ToString(), tabId);
            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value =  ReturnPuplisherReportTitle(tabId,deviceCategory)},
             new KeyValueDto(){Key = "Date From" , Value = String.Format("{0:dd-MM-yyyy}", fromdate)},
             new KeyValueDto(){Key = "Date To" , Value = String.Format("{0:dd-MM-yyyy}", toDate)}

            };
            return _WriteReportHelper.BuildReportFile(reportingList, metriceColumns, exportType, keyValueDtosList, name);
        }

        public ActionResult AppChart()
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;
            return PartialView();
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
                return Json(googleChartresult);
                // return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate), FromDate = Convert.ToDateTime(fromdate), ChartDtoList = chartDtoList, Width = 680, Height = 300 };
            }
            else
            {
                return null;
            }

        }

        #region Private Members

        private List<ChartDto> GetCampaignChartData(string fromdate, string toDate, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string metricCode, string AdvertiserId, string AccountAdvertiserId)
        {
            List<ChartDto> chartDtoList = new List<ChartDto>();

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
            {

                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.FromDate = Convert.ToDateTime(fromdate);
                criteriaDto.ToDate = Convert.ToDateTime(toDate);
                if (!string.IsNullOrEmpty(AdvertiserId))
                    criteriaDto.AdvertiserId = Convert.ToInt32(AdvertiserId);

                if (!string.IsNullOrEmpty(AccountAdvertiserId))
                    criteriaDto.AccountAdvertiserId = Convert.ToInt32(AccountAdvertiserId);
                criteriaDto.MetricCode = metricCode;
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.NotInCampaignType = CampaignType.AdHouse;
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
                    case "subappsite":
                        chartDtoList = _ReportService.GetCampaignAppSiteReportChart(criteriaDto);

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

                    case "audiancesegmentforadvertiser":
                        if (string.IsNullOrEmpty(advancedCriteria))
                        {
                            criteriaDto.AdvancedCriteria = null;
                        }
                        else
                        {
                            criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                        }
                        chartDtoList = _ReportService.GetCampaignAudianceSegmentReportChart(criteriaDto);
                        break;
                    default:
                        break;
                }
            }

            return chartDtoList;
        }

        private void GetOrderSetting(string order, string layout, out string orderColumn, out string orderType)
        {
            if (string.IsNullOrEmpty(order) || order == "undefined")
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

        private bool GetCampaignReportData(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, int itemsPerPage, string orderBy, string groupByName, string AdvertiserId, string AccountAdvertiserId, out List<CampaignCommonReportDto> reportingList, out int counter)
        {

            if (!(string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(toDate)))
            {
                string orderByColumn, orderType;
                GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.NotInCampaignType = CampaignType.AdHouse;
                if (!string.IsNullOrEmpty(AdvertiserId))
                    criteriaDto.AdvertiserId = Convert.ToInt32(AdvertiserId);
                if (!string.IsNullOrEmpty(AccountAdvertiserId))
                    criteriaDto.AccountAdvertiserId = Convert.ToInt32(AccountAdvertiserId);
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

                    case "audiancesegmentforadvertiser":
                        if (string.IsNullOrEmpty(advancedCriteria))
                        {
                            criteriaDto.AdvancedCriteria = null;
                        }
                        else
                        {
                            criteriaDto.AdvancedCriteria = advancedCriteria.Trim(new char[] { ',' });
                        }
                        reportingList = _ReportService.GetCampaignAudianceSegmentReport(criteriaDto);
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
            }
            else
            {
                counter = 0;
                reportingList = new List<CampaignCommonReportDto>();
                return false;
            }
        }

        private bool GetAppReportData(IFormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, int itemsPerPage, string orderBy, out List<AppCommonReportDto> reportingList, out int counter)
        {


            if (!(string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(toDate)))
            {
                string orderByColumn, orderType;
                GetOrderSetting(orderBy, layout, out orderByColumn, out orderType);
                ReportCriteriaDto criteriaDto = new ReportCriteriaDto();
                criteriaDto.CampaignType = CampaignType.Normal;
                criteriaDto.NotInCampaignType = CampaignType.AdHouse;
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
                criteriaDto.NotInCampaignType = CampaignType.AdHouse;
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
            dsdsd.FromDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
            dsdsd.ToDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(-20);
            List<ReportRecipientDTO> AllReportRecipient = new List<ReportRecipientDTO>();
            AllReportRecipient.Add(new ReportRecipientDTO { Email = "anashantash@yahoo.com" });
            AllReportRecipient.Add(new ReportRecipientDTO { Email = "anasa@noqoush.com" });

            ReportSchedulerDto testob = new ReportSchedulerDto
            {
                Name = "fdfsdf4344",
                AllReportRecipient = AllReportRecipient,
                AccountId = 87062,
                TimeSentAt = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMinutes(10),
                ReportDto = dsdsd,
                StartDate = ArabyAds.Framework.Utilities.Environment.GetServerTime(),
                RecurrenceType = RecurrenceType.Week,
                ReportSectionType = ReportSectionType.Publisher,
                WeekDay = WeekDay.Monday,
            };
            _ReportService.SaveSchadulingReport(testob);
            var results = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = 80001 });
            return "done";
        }
        [PermissionsAuthorize(Permission = PortalPermissionsCode.ReportSchedule, Roles = "Administrator,adops,appops")]

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
            ArabyAds.AdFalcon.Web.Controllers.Model.Report.ListViewModel lis = LoadReportSchedulerData(null, reptSec);
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Reports", "Menu"),
                Order = 1,
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            return View("IndexReportsJob", lis);
        }
        [AcceptVerbs("Post")]
        [PermissionsAuthorize(Permission = PortalPermissionsCode.ReportSchedule, Roles = "Administrator,adops,appops")]

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

            AddSuccessfullyMsgMs(string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("Reports", "Commands")));
           return  Json(true, ResourcesUtilities.GetResource("Reports", "Commands"), ResponseStatus.success);



            // return RedirectToAction("IndexReportsJob", new { reportType = reportType });
        }
        [GridAction(EnableCustomBinding = true)]
        [PermissionsAuthorize(Permission = PortalPermissionsCode.ReportSchedule, Roles = "Administrator,adops,appops")]

        public ActionResult _IndexReportsJob(string reportType)
        {

            ReportSectionType reptSec;
            if (reportType == "ad" || Request.Query["reportType"] == "ad")
            {

                reptSec = ReportSectionType.Advertiser;
            }
            else
            {
                reptSec = ReportSectionType.Publisher;
            }
            var result = GetReportSchedulerQueryResult(null, reptSec);
            return Json( new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        protected ArabyAds.AdFalcon.Web.Controllers.Model.Report.Filter getDefualtFilter()
        {
            ArabyAds.AdFalcon.Web.Controllers.Model.Report.Filter filter = new Model.Report.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            //filter.StatusId = string.IsNullOrWhiteSpace(Request.Form["StatusId"]) ? (int?)null : Convert.ToInt32(Request.Form["StatusId"]);

            return filter;
        }
        protected ReportSchedulerCriteria GetReportSchedulerCriteria(ArabyAds.AdFalcon.Web.Controllers.Model.Report.Filter filter)
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
        protected ResultReportSchedulerDto GetReportSchedulerQueryResult(ArabyAds.AdFalcon.Web.Controllers.Model.Report.Filter filter, ReportSectionType sec)
        {
            var criteria = GetReportSchedulerCriteria(filter);
            criteria.ReportSectionType = sec;
            var result = _ReportService.QueryByCratiriaForReportSchaduling(criteria);
            if (result.Items != null)
            {
                var GMT = ResourcesUtilities.GetResource("UTC", "Global");

                foreach (var item in result.Items)
                {
                    item.LastRunningDateString = item.LastRunningDate.HasValue ? item.LastRunningDate.Value.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) + " " + item.LastRunningDate.Value.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.TimeFormat) + " " + GMT : "";
                    item.EndDateString = item.EndDate.HasValue ? item.EndDate.Value.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) : "";

                    if (!item.IsForQueryBuilder)
                        item.URLLink = Url.Action("index", "Reports");
                    else
                        item.URLLink = Url.Action("Filter", "Filter");
                }
            }
            return result;
        }
        protected ArabyAds.AdFalcon.Web.Controllers.Model.Report.ListViewModel LoadReportSchedulerData(ArabyAds.AdFalcon.Web.Controllers.Model.Report.Filter filter, ReportSectionType sec)
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
            return new ArabyAds.AdFalcon.Web.Controllers.Model.Report.ListViewModel()
            {

                Items = items,

                TopActions = actions,
                BelowAction = actions,
                ToolTips = toolTips

            };
        }

        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetReportSchedulerTooltips(ReportSectionType sec)
        {
            // Create the tool tip actions
            string reportType = "ad";
            if (sec == ReportSectionType.Publisher)
            {
                reportType = "app";
            }
            var toolTips = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Index",
                            ControllerName="Reports",
                            ExtraPrams3 = reportType

                        },
                      new Model.Action()
                        {
                            Code = "2",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",

                        },
                    new Model.Action()
                        {
                            Code = "3",
                            DisplayText = ResourcesUtilities.GetResource("Document", "JobGrid"),
                            ClassName = "grid-tool-tip-reports",
                            ActionName = "DownloadReport",
                            ExtraPrams = "documentDownload",
                            Type = ActionType.ajax,
                            AjaxType=Model.AjaxType.Download,
                            CallBack = ""
                        }

                };
            return toolTips;
        }
        public virtual ActionResult RedirectToAuditTrial(int id)
        {

            
                int objectRootTypeId = _accountService.GetObjectRootTypeId("ArabyAds.AdFalcon.Domain.Model.Core.ReportRecipient").Value;

            //return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = id, objectRootTypeId = objectRootTypeId, returnUrl = originalPath });
            var url = Url.Action("AuditTrialSessions", "User",
                                                                         new { objectRootId = id, objectRootTypeId = objectRootTypeId }, "https", JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString());
            return Redirect(url);


        }
        public ActionResult DownloadReport(int Docid)
        {


            return Content(ResourcesUtilities.GetResource("CloneAdGroupError", "Errors"));

        }
        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetReportSchedulerActions(ReportSectionType sec)
        {
            string reportType = "ad";
            if (sec == ReportSectionType.Publisher)
            {
                reportType = "app";
            }
            // create the actions
            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Activate", "ReportSchedule"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Activate", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "ReportSchedule"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Report"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?
                        },
                           new Model.Action()
                        {
                            ActionName = "Send",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("RunNow", "Report"),
                            ExtraPrams = ResourcesUtilities.GetResource("SelectConfirmation", "Report"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Send", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "Index",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewReportTemplate", "Report"),
                            ExtraPrams = reportType,
                            ExtraPrams3="report",
                                                          ExtraPrams4="IsSchedule",

                        },
                    new Model.Action()
                        {
                            ActionName = "Index",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AdHocReport", "Report"),
                            ExtraPrams = reportType,
                            ExtraPrams3="report",

                        }




                };

            if (sec != ReportSectionType.Publisher && checkAdPermissions(PortalPermissionsCode.QueryBuilder))
            {
                actions.Add(new Model.Action()
                {
                    ActionName = "Filter",
                    ControllerName = "Filter",
                    ClassName = "primary-btn",
                    Type = ActionType.Link,
                    DisplayText = ResourcesUtilities.GetResource("QueryBuilderReport", "Report"),
                    ExtraPrams = reportType,
                    ExtraPrams3 = "report",

                });
            }
            return actions;
        }
        #endregion

    }



}
