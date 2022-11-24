﻿using System;
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
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Dashboard
{

    public class ImpressionLogChart : ViewComponent
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

        static ImpressionLogChart()
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
        public ImpressionLogChart()
        {
        }

      
        public async Task<IViewComponentResult> InvokeAsync(
       int? page, string orderBy)
        {
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser) criteria.userId = UserId;


            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "audiance").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);

            List<AdGeoLocationDto> aGeoLocationList = new List<AdGeoLocationDto>();
            ViewData["AdGeoLocation"] = aGeoLocationList;
            ViewData["Total"] = 0;


            ViewData["totalDaySpend"] = "0";
            var DPPartner = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = criteria.AccountId });
            if (DPPartner != null)
            {

                ViewData["Audiences"] = new TreeViewModel()
                {
                    Url = Url.Action("GetTreeData", "AudienceSegment", new { ProviderId = DPPartner.ID, withPrice = true }),
                    Name = "Audiences",
                    Id = "Audiences",
                    IsAjax = true
                };
            }
            else
            {
                ViewData["Audiences"] = null;
            }
            ViewBag.AudienceAllowed = checkAdPermissions(PortalPermissionsCode.Audience);

            return View("ImpressionLogChart");
        }
        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = Code }).Value;

            return result;
        }
    }
}
