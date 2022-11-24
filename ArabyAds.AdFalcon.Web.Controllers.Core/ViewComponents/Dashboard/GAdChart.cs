using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

using System.IO;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using System.Drawing;
using ArabyAds.Framework.Utilities;

using iTextSharp.text.pdf;
using iTextSharp.text;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Web.Core.Helper;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Services.Services;
using ArabyAds.Framework.Logging;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.PMP;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Dashboard
{

    public class GAdChart : ViewComponent
    {
        private static ISummaryService _SummaryService;
        private static IAppSiteService _appsiteService;
        private static IMetricService _MetricService;
        private static IReportService _ReportService;
        private static ICampaignService _CampaignService;
        private static IPMPDealService _PMPDealService;
        private static IPartyService _PartyService;
        private static IAccountService _accountService;
        private static IAdvertiserService _advertiserService;
        private static IAudienceSegmentService _audienceSegmentService;
        private static WriteDashboardDocumentsHelper _WriteDashboardHelper;

        static GAdChart()
        {

            _appsiteService = IoC.Instance.Resolve<IAppSiteService>();
            _MetricService = IoC.Instance.Resolve<IMetricService>();
            _ReportService = IoC.Instance.Resolve<IReportService>();
            _CampaignService = IoC.Instance.Resolve<ICampaignService>();
            _WriteDashboardHelper = new WriteDashboardDocumentsHelper();
            _SummaryService = IoC.Instance.Resolve<ISummaryService>();
            _PMPDealService = IoC.Instance.Resolve<IPMPDealService>();
            _PartyService = IoC.Instance.Resolve<IPartyService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();
            _advertiserService = IoC.Instance.Resolve<IAdvertiserService>();
            _audienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();

        }
        public GAdChart()
        {
        }


        public async Task<IViewComponentResult> InvokeAsync(
       int? page, string orderBy)
        {
            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Security.CheckDenyRoleSecurity(new AccountRole[] { AccountRole.DataProvider }, null, null);

            Framework.ApplicationContext.Instance.Logger.Warn("func: GAdChart : orderBy = " + orderBy);
            // _ReportService.GetAppSiteStatisticsReport( new Services.Interfaces.DTOs.Reports.API.Criteria.AppSiteStatisticsCriteriaDto());
            // _ReportService.GetAppSiteStatisticsGeoReport(new Services.Interfaces.DTOs.Reports.API.Criteria.AppSiteStatisticsCriteriaDto());
            ViewData["UserId"] = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }
            if (RouteData.Values["id"] != null)
            {
                criteria.AdvertiserAccountId = (int?)Convert.ToInt32(RouteData.Values["id"]);


            }
            // ViewData["Campaigns"] = _CampaignService.QueryByCratiria(criteria).Items.ToList();


            ViewBag.CampListAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "CampList_Name",
                Name = "CampList.Name",
                ActionName = (RouteData.Values["id"] != null) ? "GetCampList/" + RouteData.Values["id"] : "GetCampList",
                ControllerName = "Dashboard",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                IsRequired = false,
                ChangeCallBack = "ReportCampaignChanged",
                CurrentText = "",
                UseName = true,
                PlaceHolder = ResourcesUtilities.GetResource("SelectCampRequired", "Campaign")
            };

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
                ChangeCallBack = "ReportAdvertisersChanged",
                CurrentText = "",
                PlaceHolder = ResourcesUtilities.GetResource("SelectAdvertiserRequired", "Advertiser")
            };

            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count=0;
            //   List<AdGeoLocationDto> aGeoLocationList = BindAdGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            List<AdGeoLocationDto> aGeoLocationList = new List<AdGeoLocationDto>();

            ViewData["AdGeoLocation"] = aGeoLocationList;
            ViewData["Total"] = count;
            decimal totalDaySpend = 0;
            if (!criteria.AdvertiserAccountId.HasValue)
            {
                totalDaySpend = _SummaryService.GetAccountTotalSpend(new FromToDateMessage { FromDate = Framework.Utilities.Environment.GetServerTime().Date, ToDate = Framework.Utilities.Environment.GetServerTime() }).Value;
            }
            else
            {
                var Id = criteria.AdvertiserAccountId.Value;
                totalDaySpend = _SummaryService.GetAdvertiserAccountTotalSpend(new GetAdvertiserAccountTotalSpendRequest { FromDate = Framework.Utilities.Environment.GetServerTime().Date, ToDate = Framework.Utilities.Environment.GetServerTime(), Id = Id }).Value;
            }
            ViewData["totalDaySpend"] = FormatHelper.FormatMoney(totalDaySpend);

            Framework.ApplicationContext.Instance.Logger.Warn("End func: GAdChart : orderBy = " + orderBy);
            return View("GAdChart");
        }

    }
}
