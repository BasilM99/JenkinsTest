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
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Reports
{
    public class AppReport : ViewComponent
    {
        private static IAppSiteService _appsiteService;
        private static IMetricService _MetricService;
        private static IReportService _ReportService;
        private static ICampaignService _CampaignsService;
        protected static IAccountService _accountService;
        protected static IAudienceSegmentService _AudienceSegmentService;
        private static WriteReportDocumentsHelper _WriteReportHelper;
        static AppReport()
        {
            _AudienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
            _appsiteService = IoC.Instance.Resolve<IAppSiteService>();
            _MetricService = IoC.Instance.Resolve<IMetricService>();
            _ReportService = IoC.Instance.Resolve<IReportService>();
            _CampaignsService = IoC.Instance.Resolve<ICampaignService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

            _WriteReportHelper = new WriteReportDocumentsHelper();
        }
        public AppReport()
        {

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
        private ColumnViewModel GetColumnViewModel(string ReportType)
        {

            var ColumnViewModel = new ColumnViewModel();

            if (ReportType.ToLower() == "ad")
                ColumnViewModel.Columns = _ReportService.GetmetriceColumnsForAdvertiser();
            else
                ColumnViewModel.Columns = _ReportService.GetmetriceColumnsForPublisher();

            return ColumnViewModel;
        }
        public async Task<IViewComponentResult> InvokeAsync(
       int? id)
        {
           // ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Security.CheckDenyRoleSecurity(new AccountRole[0], new string[] { "AppOps" }, new string[] { "Administrator", "AccountManager", "AdOps" }, true);
            var apps = new TreeViewModel()
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

            apps.ColumnViewModel = new ColumnViewModel();
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


            return View("AppReport", apps);
        }

    }
}
