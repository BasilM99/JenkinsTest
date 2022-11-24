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
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Web.Controllerss.Model;
using ArabyAds.AdFalcon.Web.Controllers.Model.Report;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Response;


namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class DashboardController : AuthorizedControllerBase
    {
        ISummaryService _SummaryService;
        private IAppSiteService _appsiteService;
        private IMetricService _MetricService;
        private IReportService _ReportService;
        private ICampaignService _CampaignService;
        private IPMPDealService _PMPDealService;
        private IPartyService _PartyService;
        private IAccountService _accountService;
        private IAdvertiserService _advertiserService;
        private IAudienceSegmentService _audienceSegmentService;
        private IKPIService _KPIService;
        private WriteDashboardDocumentsHelper _WriteDashboardHelper;


      
        public DashboardController()
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
            _KPIService = IoC.Instance.Resolve<IKPIService>();

        }
        [OutputCache(Duration = 24000, VaryByQueryKeys = new string[] { "type" })]

        public ActionResult GetMetricsByType(string type)
        {
            if (type == "ad")
            {
                type = "campaign";
            }
            else if (type == "app")
            {
                type = "appsite";
            }
            else if (type == "deal")
            {
                type = "deal";
            }
            else if (type == "lmpressionlog")
            {
                type = "audiance";
            }
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == type).ToList();
            List<MetricResultDto> metricDtoResultList = new List<MetricResultDto>();
            if (metricDtoList != null)
            {
                foreach (var item in metricDtoList)
                {
                    MetricResultDto result = new MetricResultDto();
                    result.Code = item.Code;
                    result.Color = item.Color;
                    result.CustomName = item.CustomName;
                    result.MetricTarget = item.MetricTarget;
                    result.MetricId = item.Id;
                    result.Name = item.Name;


                    metricDtoResultList.Add(result);
                }
            }
            // List <MetricResultDto> metricDtoList = 
            return Json(metricDtoResultList);
        }
        // [OutputCache(Duration = 2100, VaryByQueryKeys = new string[] { "chartType", "UserId"})]
        public ActionResult GetReportCriteriaForDashboardApi(string chartType, int UserId)
        {
            ReportSectionType SectionType = ReportSectionType.Undefined;

            if (chartType.ToLower() == "campaign" || chartType.ToLower() == "ad")
            {
                chartType = "ad";
                SectionType = ReportSectionType.Advertiser;

            }

            if (chartType.ToLower() == "appsite" || chartType.ToLower() == "app")
            {
                chartType = "app";
                SectionType = ReportSectionType.Publisher;

            }
            if (chartType.ToLower() == "deal" || chartType.ToLower() == "deals")
            {
                chartType = "deal";
                SectionType = ReportSectionType.Deals;

            }

            if (chartType.ToLower() == "impressionlog" || chartType.ToLower() == "lmpressionlog")
            {
                chartType = "impressionlog";
                SectionType = ReportSectionType.DataProvider;

            }

            int SectionTypeVal = (int)SectionType;
            var results = _ReportService.GetReportCriteriaForDashboard(new ValueMessageWrapper<int> { Value = SectionTypeVal });

            var ConfigForMeasureDimensionFilter = Config.ConfigForMeasureDimensionFilter;

            return Json(new { data = results, Config = ConfigForMeasureDimensionFilter });
            // return Json(results);
        }
        public ActionResult GetReportCriteriaForDashboardById(int Id)
        {
            var results = _ReportService.GetReportCriteriaForDashboardById(new ValueMessageWrapper<int> { Value = Id });


            return Json(results);
        }

        // [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId", "AdvertiserAccountId", "AdvertiserId", "dealId" })]
        public ActionResult GetReportCriteriaForDashboard([FromBody] DashBoardFilterModel Model)
        {
            var results = _ReportService.GetReportCriteriaForDashboard(new ValueMessageWrapper<int> { Value = Model.SectionType });
            var ConfigForMeasureDimensionFilter = Config.ConfigForMeasureDimensionFilter;

            return Json(new { data = results, Config = ConfigForMeasureDimensionFilter });
        }

        [HttpPost]
        public ActionResult SaveReportCriteriaForDashboard([FromBody] ReportCriteriaDto dto)
        {

            string chartType = dto.chartType;
            if (chartType.ToLower() == "campaign" || chartType.ToLower() == "ad")
            {
                chartType = "ad";
                dto.SectionType = ReportSectionType.Advertiser;

            }

            if (chartType.ToLower() == "appsite" || chartType.ToLower() == "app")
            {
                chartType = "app";
                dto.SectionType = ReportSectionType.Publisher;

            }
            if (chartType.ToLower() == "deal" || chartType.ToLower() == "deals")
            {
                chartType = "deal";
                dto.SectionType = ReportSectionType.Deals;

            }

            if (chartType.ToLower() == "impressionlog" || chartType.ToLower() == "impressionlog")
            {
                chartType = "impressionlog";
                dto.SectionType = ReportSectionType.DataProvider;

            }

            try
            {
                var results = _ReportService.SaveReportCriteriaForDashboard(dto);
                return Json(results);

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult(new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message });

                }
                return new JsonResult(new { status = "faild" });

            }

        }
        public ActionResult GetCampListApi([FromBody] DashBoardFilterModel Model)
        {
            string q = Model.q;
            int UserId = Model.UserId;

            int? AdvertiserAccountId = Model.AdvertiserAccountId;
            int? AdvertiserId = Model.AdvertiserId;


            int? dealId = Model.dealId;


            return Json(ReturnCampListResult(q, AdvertiserAccountId, AdvertiserId, dealId));
        }
        //[OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId", "dealId" })]

        public ActionResult GetAdvListApi([FromBody] DashBoardFilterModel Model)
        {
            string q = Model.q;
            int UserId = Model.UserId;


            int? dealId = Model.dealId;
            return Json(ReturnAdvListResult(q, dealId));
        }

        //[OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId" })]

        public ActionResult GetAppSiteListApi([FromBody] DashBoardFilterModel Model)
        {
            string q = Model.q;

            int UserId = Model.UserId;
            return Json(ReturnAppListResult(q));
        }

        // [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId" })]

        public ActionResult GetDealListApi([FromBody] DashBoardFilterModel Model)
        {
            string q = Model.q;
            int UserId = Model.UserId;
            return Json(ReturnDealListResult(q, null));


        }


        [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId", "AdvertiserAccountId", "AdvertiserId", "dealId" })]

        public ActionResult GetCampList(string q, int UserId, int? AdvertiserAccountId, int? AdvertiserId, int? dealId)
        {

            return Json(ReturnCampListResult(q, AdvertiserAccountId, AdvertiserId, dealId));
        }
        [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId", "dealId" })]

        public ActionResult GetAdvList(string q, int UserId, int? dealId)
        {

            return Json(ReturnAdvListResult(q, dealId));
        }

        [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId" })]

        public ActionResult GetAppSiteList(string q, int UserId)
        {

            return Json(ReturnAppListResult(q));
        }

        [OutputCache(Duration = 600, VaryByQueryKeys = new string[] { "q", "UserId", "AdvertiserAccountId" })]

        public ActionResult GetDealList(string q, int UserId, int? AdvertiserAccountId)
        {

            return Json(ReturnDealListResult(q, AdvertiserAccountId));


        }
        public virtual ActionResult getCampsBydeal(int dealid, int? AdvertiserAccountId)
        {
            var results = _PMPDealService.getCampsAdvertiserBydeal(new GetCampsAdvertiserBydealRequest { DealId = dealid, AdvertiserId = AdvertiserAccountId.HasValue ? AdvertiserAccountId.Value : 0 });


            List<CampaignDto> list = results.CampaignItems.ToList();
            List<AdvertiserAccountDto> AdvertiserAccountItems = results.AdvertiserAccountItems.ToList();
            return Json(new { results });

        }
        public virtual ActionResult getAdvertiserAccountsBydeal(int dealid)
        {
            List<AdvertiserAccountDto> list = _PMPDealService.getAdvertiserAccountsBydeal(new ValueMessageWrapper<int> { Value = dealid }).ToList();

            return Json(new { list });

        }




        private IEnumerable<CampaignListDto> ReturnCampListResult(string q, int? AdvertiserAccountId, int? AdvertiserId, int? dealId)
        {

            if (q == null)
            { q = string.Empty; }

            if (!dealId.HasValue)
            {
                var criteria = new CampaignCriteria() { Name = q, AdvertiserAccountId = AdvertiserAccountId, DataCreate = Framework.Utilities.Environment.GetServerTime().AddMonths(-12).ToUniversalTime().Date, ActiveCampaigns = true };
                criteria.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

                //GetAdvertiserId(criteria);
                if (AdvertiserId > 0)
                {
                    criteria.AdvertiserId = AdvertiserId;
                }
                var list = _CampaignService.QueryByCratiria(criteria);


                return list.Items;
            }
            else
            {
                var results = _PMPDealService.getCampsAdvertiserBydeal(new GetCampsAdvertiserBydealRequest { DealId = dealId.Value, AdvertiserId = AdvertiserAccountId.HasValue ? AdvertiserAccountId.Value : 0 });
                IList<CampaignListDto> resultsitems = new List<CampaignListDto>();
                foreach (var item in results.CampaignItems)
                {
                    if (item.Name.Contains(q) && !string.IsNullOrWhiteSpace(q) && resultsitems.Where(M => M.Id == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(new CampaignListDto { Id = item.ID, Name = item.Name });
                    else if (string.IsNullOrWhiteSpace(q) && resultsitems.Where(M => M.Id == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(new CampaignListDto { Id = item.ID, Name = item.Name });
                }
                return resultsitems;

            }
        }
        private IEnumerable<AdvertiserAccountListDto> ReturnAdvListResult(string q, int? dealId)
        {
            if (!dealId.HasValue)
            {
                AdvertiserAccountCriteria criteria = new AdvertiserAccountCriteria() { Name = q };

                criteria.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                criteria.showActive = true;
                var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
                var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
                criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
                if (!IsPrimaryUser)
                {

                    criteria.userId = UserId;

                }
                var list = _advertiserService.GetAccountAdvertiser(criteria);
                if (list != null)
                {
                    return list.Items;
                }
                return null;
            }
            else
            {


                List<AdvertiserAccountDto> list = _PMPDealService.getAdvertiserAccountsBydeal(new ValueMessageWrapper<int> { Value = dealId.Value }).ToList();
                IList<AdvertiserAccountListDto> resultsitems = new List<AdvertiserAccountListDto>();
                foreach (var item in list)
                {
                    if (item.Name.Contains(q) && !string.IsNullOrWhiteSpace(q))
                        resultsitems.Add(new AdvertiserAccountListDto { Id = item.Id, Name = item.Name });
                    else if (string.IsNullOrWhiteSpace(q))
                        resultsitems.Add(new AdvertiserAccountListDto { Id = item.Id, Name = item.Name });
                }
                return resultsitems;
            }
        }


        private IEnumerable<AppSiteListDto> ReturnAppListResult(string q)
        {
            //AdvertiserAccountCriteria criteria = new AdvertiserAccountCriteria() { Name = q };
            ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase criteria = new ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase() { Name = q };

            criteria.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            //criteria.showActive = true;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            //criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;

            }
            var list = _appsiteService.QueryByCratiriaForDashboard(criteria);
            if (list != null)
            {
                return list.Items;
            }
            return null;
        }


        private IEnumerable<PMPDealDto> ReturnDealListResult(string q, int? AdvertiserAccountId)
        {
            if (q == null)
            { q = string.Empty; }
            var list = _PMPDealService.GetAllPMPDealsByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value }).ToList();
            IList<PMPDealDto> resultsitems = new List<PMPDealDto>();
            if (!AdvertiserAccountId.HasValue)
            {
                foreach (var item in list)
                {
                    if (item.Name.Contains(q) && !string.IsNullOrWhiteSpace(q) && resultsitems.Where(M => M.ID == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(item);
                    else if (string.IsNullOrWhiteSpace(q) && resultsitems.Where(M => M.ID == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(item);
                }
            }
            else
            {

                foreach (var item in list)
                {
                    if (item.Name.Contains(q) && !string.IsNullOrWhiteSpace(q) && item.AdvertiserAccountId == AdvertiserAccountId.Value && resultsitems.Where(M => M.ID == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(item);
                    else if (string.IsNullOrWhiteSpace(q) && item.AdvertiserAccountId == AdvertiserAccountId.Value && resultsitems.Where(M => M.ID == item.ID).FirstOrDefault() == null)
                        resultsitems.Add(item);
                }
            }
            return resultsitems;
        }
        //[Authorize]
        public ActionResult Index(string chartType, int? id)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: Index : chartType = " + chartType);
            return View();
            

            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Dashboard", "SiteMapLocalizations"),
                Order = 1,
            });
            var advId = 0;
            if (id > 0)
            {
                var AdvertiserAccountName = _CampaignService.GetAdvertiserAccountString(new ValueMessageWrapper<int> { Value = id.Value });

                advId = _CampaignService.GetAdvertiserIdFromAccount(new ValueMessageWrapper<int> { Value = id.Value }).Value;
                //var AdvertiserName = _CampaignService.GetAdvertiserString(id.Value);
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers", "campaign"),
                                           Order = -2,
                                           ExtensionDropDown = true
                                       });
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)AccountRole.DataProvider)
            {
                chartType = "lmpressionlog";
            }

            if (chartType.ToLower() == "lmpressionlog")
            {

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole != (int)AccountRole.DataProvider)
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }

            }

            ViewData["chartType"] = chartType;
            if (id.HasValue)
            {
                _advertiserService.ValidateAdvertiser(new Services.Interfaces.Messages.ValidateAdvertiserRequest { AdvertiserId = id.Value });
                TempData["DashAdvertiserId"] = advId;
                TempData["DashAdvertiserAccountId"] = id.Value;
            }


            Framework.ApplicationContext.Instance.Logger.Warn("End func: Index : chartType = " + chartType);


            return View();
        }

        public ActionResult Chart()
        {


            return PartialView();
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult GAppChart(int? page, string orderBy)
        {

            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ViewData["AppSites"] = _appsiteService.GetAppSitesByAccountId(new ValueMessageWrapper<int> { Value = accountId });


            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AppSiteGeoLocationDto> appsitePerformanceList = BindAppGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            ViewData["AppSiteGeoLocation"] = appsitePerformanceList;
            ViewData["Total"] = count;


            decimal totalDayRevenue = _SummaryService.GetAccountTotalRevenue(new FromToDateMessage { FromDate = Framework.Utilities.Environment.GetServerTime().Date, ToDate = Framework.Utilities.Environment.GetServerTime() }).Value;

            ViewData["totalDayRevenue"] = FormatHelper.FormatMoney(totalDayRevenue);


            return PartialView();
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult GAdChart(int? page, string orderBy)
        {
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
            int count;
            List<AdGeoLocationDto> aGeoLocationList = BindAdGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
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

            return PartialView();

        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult DealChart(int? page, string orderBy)
        {
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }
            // ViewData["adgruops"] = new List<SelectListItem>();
            ViewData["deals"] = _PMPDealService.GetAllPMPDealsByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value }).ToList();
            var test = ViewData["deals"];
            ViewData["Campaigns"] = _CampaignService.QueryByCratiria(criteria).Items.ToList();
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "deal").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AdGeoLocationDto> aGeoLocationList = new List<AdGeoLocationDto>()/*BindAdGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType)*/;
            ViewData["AdGeoLocation"] = aGeoLocationList;
            ViewData["Total"] = 0;

            decimal totalDaySpend = 0;

            ViewData["totalDaySpend"] = FormatHelper.FormatMoney(totalDaySpend);

            return PartialView();

        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult DealPerformance(int? page, string orderBy, int period, int? id, int? subId, int? secondsubId, int? AdvertiserAccountId, IFormCollection collection)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "deal");

            int count;
            List<DealPerformanceDto> appsitePerformanceList = BindDealPerformance(out count, page, 10, orderColumn, orderType, period, id, subId, secondsubId, AdvertiserAccountId);
            ViewData.Model = appsitePerformanceList;
            return Json(new GridModel
            {
                Data = appsitePerformanceList,
                Total = count
            });
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult DealPerformance(int? page, string orderBy, int period, int? id, int? subId, int? secondsubId, int? AdvertiserAccountId)
        {

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "deal");
            int count;
            List<DealPerformanceDto> appsitePerformanceList = BindDealPerformance(out count, page, 10, orderColumn, orderType, period, id, subId, secondsubId, AdvertiserAccountId);

            ViewData["total"] = count;
            ViewData.Model = appsitePerformanceList;
            return PartialView();
        }
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        private List<DealPerformanceDto> BindDealPerformance(out int count, int? page, int itemsPerPage, string orderColumn, string orderType, int period, int? id, int? subId, int? secondsubId, int? AdvertiserAccountId)
        {
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);
            DashboardPerformanceCriteria criteria = new DashboardPerformanceCriteria();
            criteria.OrderType = orderType;
            criteria.OrderColumn = orderColumn;
            criteria.ItemsPerPage = itemsPerPage;
            criteria.PageNumber = (page == null ? 0 : page.Value - 1);
            criteria.FromDate = fromDate;
            criteria.ToDate = toDate;
            criteria.IdFilter = id;
            criteria.IdSubFilter = subId;
            criteria.IdSecondSubFilter = secondsubId;
            criteria.AccountAdvertiserId = AdvertiserAccountId.HasValue ? AdvertiserAccountId.Value : 0;

            List<DealPerformanceDto> appsitePerformanceList = _ReportService.GetDealPerformance(criteria);
            count = appsitePerformanceList.FirstOrDefault() != null ? (int)appsitePerformanceList.First().TotalCount : 0;//_ReportService.GetTotalAppSitePerformance(criteria);
            return appsitePerformanceList;
        }
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult DealPerformanceExport(int? page, string orderBy, string type, int period, int? id, int? subId, int? secondsubId, int? AdvertiserAccountId, string NameTitle = "")
        {
            string orderType;
            string orderColumn;
            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "deal");
            int count;
            List<DealPerformanceDto> appsitePerformanceList = BindDealPerformance(out count, null, int.MaxValue, orderColumn, orderType, period, id, subId, secondsubId, AdvertiserAccountId);
            // To Do: please to write it 
            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value = ResourcesUtilities.GetResource( "DealPerformanceFile","Global")},
             //new KeyValueDto(){Key = "Date From" , Value = String.Format("{0:dd-MM-yyyy}", fromDate)},
             //new KeyValueDto(){Key = "Date To" , Value = String.Format("{0:dd-MM-yyyy}", toDate)}
            };

            return _WriteDashboardHelper.BuildDealPerformanceExportFile(appsitePerformanceList, type, keyValueDtosList, NameTitle);


        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AppSiteGeoLocation(string orderBy, int? page, string country, string list, int period)
        {
            string orderType;
            string orderColumn;
            int? countryId, appSiteId;
            countryId = appSiteId = null;

            if (!string.IsNullOrEmpty(country))
                countryId = int.Parse(country);

            if (!string.IsNullOrEmpty(list))
                appSiteId = int.Parse(list);

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);

            int count;
            List<AppSiteGeoLocationDto> appsiteGeoLocationList = BindAppGeoLocation(out count, page, 10, fromDate, toDate, countryId, appSiteId, orderColumn, orderType);
            ViewData.Model = appsiteGeoLocationList;
            return Json(new GridModel
            {
                Data = appsiteGeoLocationList,
                Total = count
            });
        }
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AppSiteGeoLocationExport(string type, string country, string list, int period, string orderBy)
        {
            string orderType;
            string orderColumn;
            int? countryId, appSiteId;
            countryId = appSiteId = null;

            if (!string.IsNullOrEmpty(country))
                countryId = int.Parse(country);

            if (!string.IsNullOrEmpty(list))
                appSiteId = int.Parse(list);

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;
            int count;
            GetDates(period, out fromDate, out toDate);

            List<AppSiteGeoLocationDto> appsiteGeoLocationList = BindAppGeoLocation(out count, null, int.MaxValue, fromDate, toDate, countryId, appSiteId, orderColumn, orderType);

            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource("Title","Titles") , Value = ResourcesUtilities.GetResource( "AppGeoLocationFile","Global")},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "DateFrom","Global") , Value = String.Format("{0:dd-MM-yyyy}", fromDate)},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "DateTo","Global") , Value = String.Format("{0:dd-MM-yyyy}", toDate)}
            };

            return _WriteDashboardHelper.BuildAppGeoLocationExportFile(appsiteGeoLocationList, type, keyValueDtosList);

        }
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AppPerformance(int? page, string orderBy)
        {

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");
            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, page, 10, orderColumn, orderType);

            ViewData["total"] = count;
            ViewData.Model = appsitePerformanceList;
            return PartialView();
        }
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AppPerformanceExport(int? page, string orderBy, string type)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");
            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, null, int.MaxValue, orderColumn, orderType);

            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource("Title","Titles") , Value = ResourcesUtilities.GetResource( "PerformanceFile","Global")},
             //new KeyValueDto(){Key = "Date From" , Value = String.Format("{0:dd-MM-yyyy}", fromDate)},
             //new KeyValueDto(){Key = "Date To" , Value = String.Format("{0:dd-MM-yyyy}", toDate)}
            };
            return _WriteDashboardHelper.BuildAppPerformanceExportFile(appsitePerformanceList, type, keyValueDtosList);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AppPerformance(int? page, string orderBy, IFormCollection collection)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");

            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, page, 10, orderColumn, orderType);
            ViewData.Model = appsitePerformanceList;
            return Json(new GridModel
            {
                Data = appsitePerformanceList,
                Total = count
            });
        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AdGeoLocation([FromBody] AdGeoLocationGridFilter model)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: AdGeoLocation : orderBy = " + model.orderBy);

            string orderType;
            string orderColumn;
            int? countryId, campaignId;
            countryId = campaignId = null;
            if (model.skip != null && model.take != null)
                model.page = (model.skip.Value / model.take.Value) + 1;
            if (!string.IsNullOrEmpty(model.country))
                countryId = int.Parse(model.country);

            if (!string.IsNullOrEmpty(model.list))
                campaignId = int.Parse(model.list);

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(model.orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;

            GetDates(model.period, out fromDate, out toDate);

            int count;
            List<AdGeoLocationDto> adGeoLocationList = BindAdGeoLocation(out count, model.page, 10, fromDate, toDate, countryId, campaignId, orderColumn, orderType);
            ViewData.Model = adGeoLocationList;
            Framework.ApplicationContext.Instance.Logger.Warn("End func: AdGeoLocation : orderBy = " + model.orderBy);

            return Json(new GridModel
            {
                Data = adGeoLocationList,
                Total = count
            });
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AdGeoLocationExport(string type, string country, string list, int period, string orderBy)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: AdGeoLocationExport : orderBy = " + orderBy);

            string orderType;
            string orderColumn;
            int? countryId, campaignId;
            countryId = campaignId = null;

            if (!string.IsNullOrEmpty(country))
                countryId = int.Parse(country);

            if (!string.IsNullOrEmpty(list))
                campaignId = int.Parse(list);

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Order.GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;
            int count;
            GetDates(period, out fromDate, out toDate);

            List<AdGeoLocationDto> adGeoLocationList = BindAdGeoLocation(out count, null, int.MaxValue, fromDate, toDate, countryId, campaignId, orderColumn, orderType);
            Framework.ApplicationContext.Instance.Logger.Warn("End func: AdGeoLocationExport : orderBy = " + orderBy);


            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value = ResourcesUtilities.GetResource( "GeoLocationFile","Global")},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource("DateFrom","Global") , Value = String.Format("{0:dd-MM-yyyy}", fromDate)},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "DateTo","Global") , Value = String.Format("{0:dd-MM-yyyy}", toDate)}
            };

            return _WriteDashboardHelper.BuildAdGeoLocationExportFile(adGeoLocationList, type, keyValueDtosList);

        }

        public JsonResult GChartControl(int periodOption, string metricCode, string id, string type, string subId, string secondsubId, int? CompanyName, int? CampName, int? AdvertiserId)
        {

            if (type.ToLower() != "lmpressionlog")
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)AccountRole.DataProvider)
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }

            }
            if (type.ToLower() == "lmpressionlog")
            {

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole != (int)AccountRole.DataProvider)
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }

            }

            Framework.ApplicationContext.Instance.Logger.Warn("func: GChartControl : type = " + type);
            string OptionalParameter = "";
            DateTime fromDate, toDate;
            GetDates(periodOption, out fromDate, out toDate);

            string color = _MetricService.GetByCode(metricCode).Color;

            List<ChartDto> chartDtoList = new List<ChartDto>();

            if (type.ToLower() == "appsite")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;

                chartDtoList = _ReportService.GetAppSiteChart(criteria);
            }
            else if (type.ToLower() == "deal")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;

                int? subIdvar;
                if (string.IsNullOrEmpty(subId))
                    subIdvar = null;
                else
                    subIdvar = int.Parse(subId);

                criteria.IdSubFilter = subIdvar;

                int? secondsubIdvar;
                if (string.IsNullOrEmpty(secondsubId))
                    secondsubIdvar = null;
                else
                    secondsubIdvar = int.Parse(secondsubId);

                criteria.IdSecondSubFilter = secondsubIdvar;
                criteria.AccountAdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : 0;
                chartDtoList = _ReportService.GetDealChart(criteria);

            }
            else if (type.ToLower() == "lmpressionlog")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;
                criteria.CampName = CampName.HasValue ? CampName.Value : 0; ;
                criteria.CompanyName = CompanyName.HasValue ? CompanyName.Value : 0;
                criteria.AdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : 0;
                var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
                criteria.DPProviderId = DPP != null ? DPP.ID : null;

                /* int? subIdvar;
                 if (string.IsNullOrEmpty(subId))
                     subIdvar = null;
                 else
                     subIdvar = int.Parse(subId);

                 criteria.IdSubFilter = subIdvar;

                 int? secondsubIdvar;
                 if (string.IsNullOrEmpty(secondsubId))
                     secondsubIdvar = null;
                 else
                     secondsubIdvar = int.Parse(secondsubId);

                 criteria.IdSecondSubFilter = secondsubIdvar;
                 */
                chartDtoList = _ReportService.GetImpressionLogChart(criteria);
                //  try
                //{
                if (criteria.MetricCode == "REVAudi")
                {
                    decimal total = chartDtoList.Sum(x => Convert.ToDecimal(x.Yaxis));
                    OptionalParameter = FormatHelper.FormatMoney(total);
                }
                else
                {
                    criteria.MetricCode = "REVAudi";
                    var tempData = _ReportService.GetImpressionLogChart(criteria);
                    decimal total = tempData.Sum(x => Convert.ToDecimal(x.Yaxis));
                    OptionalParameter = FormatHelper.FormatMoney(total);

                }
                //}
                // to be discussed with Anas
                //catch (Exception)
                //{

                //    OptionalParameter = "0$";
                //}


            }
            else
            {
                int? campaignId;

                if (string.IsNullOrEmpty(id))
                    campaignId = null;
                else
                    campaignId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = campaignId;
                GetAdvertiserId(criteria);
                chartDtoList = _ReportService.GetAdChart(criteria);
            }

            var googleChartresult = new GoogleChartResult
            {
                Color = color,
                ToDate = Convert.ToDateTime(toDate).ToUniversalTime(),
                FromDate = Convert.ToDateTime(fromDate).ToUniversalTime(),
                ChartDtoList = chartDtoList,
                Width = 680,
                Height = 350,
                OptionalParameter = OptionalParameter
            };
            //if(type.ToLower() != "deal" ||(type.ToLower() == "deal" && googleChartresult.ChartPeriodType != GoogleChartResult.ChartPeriod.Day &&  googleChartresult.ChartPeriodType != GoogleChartResult.ChartPeriod.Week))

            if (type.ToLower() == "lmpressionlog")
            {
                TimeSpan diff = googleChartresult.ToDate - googleChartresult.FromDate;
                var dateDiff = googleChartresult.ToDate.Subtract(googleChartresult.FromDate).TotalDays;

                if ((dateDiff >= 1) && (dateDiff <= 7))
                {
                    googleChartresult.ForMonth = true;

                }
            }
            googleChartresult.ExecuteResult();

            if (googleChartresult.ChartDtoList != null)
            {
                if (googleChartresult.ChartDtoList.Count > 1)
                {

                    var last = googleChartresult.ChartDtoList.Last();


                    if (last.Yaxis != null && !(Convert.ToInt64(last.Yaxis) > 0))
                    {
                        //googleChartresult.ChartDtoList.Remove(last);
                        var beforelast = googleChartresult.ChartDtoList[googleChartresult.ChartDtoList.Count - 2];

                        if (beforelast.Yaxis != null && (Convert.ToInt64(beforelast.Yaxis) > 0))
                        {
                            last.Yaxis = null;
                        }

                    }
                }
            }
            Framework.ApplicationContext.Instance.Logger.Warn("End func: GChartControl : type = " + type);

            return Json(googleChartresult);
        }

        [HttpPost]
        public JsonResult GChartControlPost([FromBody] GChartControlModel model)
        {
            int periodOption = model.periodOption;
            string metricCode = model.metricCode;
            string id = model.id;
            string type = model.type;
            string subId = model.subId;
            string secondsubId = model.secondsubId;
            int? CompanyName = model.CompanyName;
            int? CampName = model.CampName;
            int? AdvertiserId = model.AdvertiserId;

            //if (ArabyAds.AdFalcon.Web.Controllers.Utilities.ChartPeriod.Day)
            //{ }
            if (type.ToLower() != "lmpressionlog")
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)AccountRole.DataProvider)
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }

            }
          
            Framework.ApplicationContext.Instance.Logger.Warn("func: GChartControl : type = " + type);
            string OptionalParameter = "";
            DateTime fromDate = Framework.Utilities.Environment.GetServerTime();
            DateTime toDate = Framework.Utilities.Environment.GetServerTime();
            //GetDates(periodOption, out fromDate, out toDate);
            if (model.from.HasValue)
            { fromDate = model.from.Value; }
            if (model.to.HasValue)
            { toDate = model.to.Value; }
            string color = _MetricService.GetByCode(metricCode).Color;

            List<ChartDto> chartDtoList = new List<ChartDto>();

            if (type.ToLower() == "appsite" || type.ToLower() == "app")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;

                chartDtoList = _ReportService.GetAppSiteChart(criteria);
            }
            else if (type.ToLower() == "deal")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;

                int? subIdvar;
                if (string.IsNullOrEmpty(subId))
                    subIdvar = null;
                else
                    subIdvar = int.Parse(subId);

                criteria.IdSubFilter = subIdvar;

                int? secondsubIdvar;
                if (string.IsNullOrEmpty(secondsubId))
                    secondsubIdvar = null;
                else
                    secondsubIdvar = int.Parse(secondsubId);

                criteria.IdSecondSubFilter = secondsubIdvar;
                criteria.AccountAdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : 0;
                chartDtoList = _ReportService.GetDealChart(criteria);

            }
            else if (type.ToLower() == "lmpressionlog")
            {
                int? appSiteId;

                if (string.IsNullOrEmpty(id))
                    appSiteId = null;
                else
                    appSiteId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = appSiteId;
                criteria.CampName = CampName.HasValue ? CampName.Value : 0; ;
                criteria.CompanyName = CompanyName.HasValue ? CompanyName.Value : 0;
                criteria.AdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : 0;
                var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
                criteria.DPProviderId = DPP != null ? DPP.ID : null;

                /* int? subIdvar;
                 if (string.IsNullOrEmpty(subId))
                     subIdvar = null;
                 else
                     subIdvar = int.Parse(subId);

                 criteria.IdSubFilter = subIdvar;

                 int? secondsubIdvar;
                 if (string.IsNullOrEmpty(secondsubId))
                     secondsubIdvar = null;
                 else
                     secondsubIdvar = int.Parse(secondsubId);

                 criteria.IdSecondSubFilter = secondsubIdvar;
                 */
                chartDtoList = _ReportService.GetImpressionLogChart(criteria);
                //  try
                //{
                if (criteria.MetricCode == "REVAudi")
                {
                    decimal total = chartDtoList.Sum(x => Convert.ToDecimal(x.Yaxis));
                    OptionalParameter = FormatHelper.FormatMoney(total);
                }
                else
                {
                    criteria.MetricCode = "REVAudi";
                    var tempData = _ReportService.GetImpressionLogChart(criteria);
                    decimal total = tempData.Sum(x => Convert.ToDecimal(x.Yaxis));
                    OptionalParameter = FormatHelper.FormatMoney(total);

                }
                //}
                // to be discussed with Anas
                //catch (Exception)
                //{

                //    OptionalParameter = "0$";
                //}


            }
            else
            {
                int? campaignId;

                if (string.IsNullOrEmpty(id))
                    campaignId = null;
                else
                    campaignId = int.Parse(id);

                DashboardChartCriteria criteria = new DashboardChartCriteria();
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;
                criteria.MetricCode = metricCode;
                criteria.IdFilter = campaignId;
                //GetAdvertiserId(criteria);

                criteria.AccountAdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : 0;

                chartDtoList = _ReportService.GetAdChart(criteria);
            }

            var googleChartresult = new GoogleChartResult
            {
                Color = color,
                ToDate = toDate,
                FromDate = fromDate,
                ChartDtoList = chartDtoList,
                Width = 680,
                Height = 350,
                OptionalParameter = OptionalParameter
            };
            //if(type.ToLower() != "deal" ||(type.ToLower() == "deal" && googleChartresult.ChartPeriodType != GoogleChartResult.ChartPeriod.Day &&  googleChartresult.ChartPeriodType != GoogleChartResult.ChartPeriod.Week))

            if (type.ToLower() == "lmpressionlog")
            {
                TimeSpan diff = googleChartresult.ToDate - googleChartresult.FromDate;
                var dateDiff = googleChartresult.ToDate.Subtract(googleChartresult.FromDate).TotalDays;

                if ((dateDiff >= 1) && (dateDiff <= 7))
                {
                    googleChartresult.ForMonth = true;

                }
            }
            googleChartresult.ExecuteNewResult();

            if (googleChartresult.ChartDtoList != null)
            {
                if (googleChartresult.ChartDtoList.Count > 1)
                {

                    var last = googleChartresult.ChartDtoList.Last();


                    if (last.Yaxis != null && !(Convert.ToInt64(last.Yaxis) > 0))
                    {
                        //googleChartresult.ChartDtoList.Remove(last);
                        var beforelast = googleChartresult.ChartDtoList[googleChartresult.ChartDtoList.Count - 2];

                        if (beforelast.Yaxis != null && (Convert.ToInt64(beforelast.Yaxis) > 0))
                        {
                            last.Yaxis = null;
                        }

                    }
                }
            }
            Framework.ApplicationContext.Instance.Logger.Warn("End func: GChartControl : type = " + type);

            return Json(googleChartresult);
        }



        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AdPerformance(int? page, string orderBy, IFormCollection collection)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: AdPerformance : orderBy = " + orderBy);

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "ad");

            int count;
            List<AdPerformanceDto> adPerformanceList = BindAdPerformance(out count, page, 10, orderColumn, orderType);
            ViewData.Model = adPerformanceList;
            Framework.ApplicationContext.Instance.Logger.Warn("End func: AdPerformance : orderBy = " + orderBy);

            return Json(new GridModel
            {
                Data = adPerformanceList,
                Total = count
            });
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AdPerformanceExport(int? page, string orderBy, string type)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: AdPerformanceExport : type = " + type);

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "ad");
            int count;
            List<AdPerformanceDto> appsitePerformanceList = BindAdPerformance(out count, page, int.MaxValue, orderColumn, orderType);
            Framework.ApplicationContext.Instance.Logger.Warn("End func: AdPerformanceExport : type = " + type);

            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key =ResourcesUtilities.GetResource("Title","Titles") , Value = ResourcesUtilities.GetResource( "AdPerformanceFile","Global")},

            };



            return _WriteDashboardHelper.BuildAdPerformanceExportFile(appsitePerformanceList, type, keyValueDtosList);
        }

        [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public ActionResult AdPerformance(int? page, string orderBy)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: AdPerformance : orderType = " + orderBy);


            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "ad");
            int count;
            List<AdPerformanceDto> adPerformanceList = BindAdPerformance(out count, page, 10, orderColumn, orderType);

            ViewData["total"] = count;
            ViewData.Model = adPerformanceList;
            Framework.ApplicationContext.Instance.Logger.Warn("End func: AdPerformance : orderType = " + orderBy);
            ViewData["AdPerformance"] = adPerformanceList;
            return PartialView();
        }

        public ActionResult ImpressionLogChart(int? page, string orderBy)
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

                ViewData["Audiences"] = new Model.Tree.TreeViewModel()
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
            return PartialView();

        }
        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = Code }).Value;

            return result;
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]

        public ActionResult ImpressionLogPerformance(int? page, string orderBy, int period, int? id, int? subId, int? secondsubId, int? CompanyName, int? CampName, int? AdvertiserId, IFormCollection collection)
        {
            string orderType;
            string orderColumn;
            DateTime? FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            DateTime? ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "lmpressionlog");

            int count;
            List<ImpressionLogPerformanceDto> appsitePerformanceList = BindImpressionLogPerformance(out count, page, 10, orderColumn, orderType, period, id, subId, secondsubId, CompanyName.HasValue ? CompanyName.Value : 0, CampName.HasValue ? CampName.Value : 0, AdvertiserId.HasValue ? AdvertiserId.Value : 0, FromDate, ToDate);
            ViewData.Model = appsitePerformanceList;
            return Json(new GridModel
            {
                Data = appsitePerformanceList,
                Total = count
            });
        }

        public ActionResult ImpressionLogPerformance(int? page, string orderBy, int period, int? id, int? subId, int? secondsubId, int? CompanyName, int? CampName, int? AdvertiserId)
        {

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "lmpressionlog");
            int count;
            List<ImpressionLogPerformanceDto> appsitePerformanceList = BindImpressionLogPerformance(out count, page, 10, orderColumn, orderType, period, id, subId, secondsubId, CompanyName.HasValue ? CompanyName.Value : 0, CampName.HasValue ? CampName.Value : 0, AdvertiserId.HasValue ? AdvertiserId.Value : 0);

            ViewData["total"] = count;
            ViewData.Model = appsitePerformanceList;
            return PartialView();
        }
        private List<ImpressionLogPerformanceDto> BindImpressionLogPerformance(out int count, int? page, int itemsPerPage, string orderColumn, string orderType, int period, int? id, int? subId, int? secondsubId, int CompanyName, int CampName, int AdvertiserId, DateTime? From = null, DateTime? To = null)
        {
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);
            DashboardPerformanceCriteria criteria = new DashboardPerformanceCriteria();
            criteria.OrderType = orderType;
            criteria.OrderColumn = orderColumn;
            criteria.ItemsPerPage = itemsPerPage;
            criteria.CampName = CampName;
            criteria.CompanyName = CompanyName;
            criteria.AdvertiserId = AdvertiserId;
            criteria.PageNumber = (page == null ? 0 : page.Value - 1);
            if (From.HasValue && To.HasValue)
            {
                criteria.FromDate = From.Value;
                criteria.ToDate = To.Value;
            }
            else
            {
                criteria.FromDate = fromDate;
                criteria.ToDate = toDate;

            }
            criteria.IdFilter = id;
            criteria.IdSubFilter = subId;
            criteria.IdSecondSubFilter = secondsubId;
            var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            criteria.DPProviderId = DPP != null ? DPP.ID : null;
            List<ImpressionLogPerformanceDto> appsitePerformanceList = _ReportService.GetImpressionLogPerformance(criteria);

            count = appsitePerformanceList.FirstOrDefault() != null ? (int)appsitePerformanceList.First().TotalCount : 0;

            //count = appsitePerformanceList.FirstOrDefault() != null ? appsitePerformanceList.Count() : 0;//_ReportService.GetTotalAppSitePerformance(criteria);
            return appsitePerformanceList;
        }


        public ActionResult ImpressionLogPerformanceExport(int? page, string orderBy, string type, int period, int? id, int? subId, int? secondsubId, bool showCampaign, bool showAdvertiser, int? CompanyName, int? CampName, int? AdvertiserId, string NameTitle = "")
        {
            string orderType;
            string orderColumn;
            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "lmpressionlog");
            int count;
            DateTime? FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            DateTime? ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            List<ImpressionLogPerformanceDto> appsitePerformanceList = BindImpressionLogPerformance(out count, null, int.MaxValue, orderColumn, orderType, period, id, subId, secondsubId, CompanyName.HasValue ? CompanyName.Value : 0, CampName.HasValue ? CampName.Value : 0, AdvertiserId.HasValue ? AdvertiserId.Value : 0, FromDate, ToDate);
            // To Do: please to write it 
            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value = ResourcesUtilities.GetResource( "ImpressionLogPerformanceFile","Global")},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "DateFrom","Global"),Value = String.Format("{0:dd-MM-yyyy}", FromDate)},
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "DateTo","Global") , Value = String.Format("{0:dd-MM-yyyy}", ToDate)}
            };


            return _WriteDashboardHelper.BuildImpressionLogPerformanceExportFile(appsitePerformanceList, type, true, true, keyValueDtosList, NameTitle);


        }
        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "q", "period" })]
        public ActionResult GetAdvListForDP(string q, int period)
        {
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);
            var datefromint = Convert.ToInt32(fromDate.ToString("yyyyMMdd"));
            var datetoint = Convert.ToInt32(toDate.ToString("yyyyMMdd"));

            var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            var list = _ReportService.getAdvertisersListForDP(new QueryDataProviderRequest { DataProviderId = DPP.ID.Value, Culture = culture, q = q, DateFrom = datefromint, DateTo = datetoint });
            return Json(list);
        }
        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "q", "period" })]
        public ActionResult GetAgencyListForDP(string q, int period)
        {
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);
            var datefromint = Convert.ToInt32(fromDate.ToString("yyyyMMdd"));
            var datetoint = Convert.ToInt32(toDate.ToString("yyyyMMdd"));
            var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            var list = _ReportService.getAgencyForDP(new QueryDataProviderRequest { DataProviderId = DPP.ID.Value, q = q, DateFrom = datefromint, DateTo = datetoint });
            return Json(list);
        }

        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "q", "period" })]
        public ActionResult GetCampaignListForDP(string q, int period)
        {
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);
            var datefromint = Convert.ToInt32(fromDate.ToString("yyyyMMdd"));
            var datetoint = Convert.ToInt32(toDate.ToString("yyyyMMdd"));
            var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            var list = _ReportService.getCampaignForDP(new QueryDataProviderRequest { DataProviderId = DPP.ID.Value, q = q, DateFrom = datefromint, DateTo = datetoint });
            return Json(list);
        }
        public ActionResult AudienceSegmentsExport(int? page, string orderBy, string type, int period, int? id, int? subId, int? secondsubId, bool showCampaign, bool showAdvertiser, string NameTitle = "")
        {

            var DPP = _PartyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });


            var audianceList = _audienceSegmentService.getAudianceSegmentsByDataProviderToWrite(new GetAudienceSegByDataProviderRequest { Id = DPP.ID.Value, q = "", cultures = "" });
            // To Do: please to write it 
            List<KeyValueDto> keyValueDtosList = new List<KeyValueDto>
            {
             new KeyValueDto(){Key = ResourcesUtilities.GetResource( "Title","Titles") , Value = ResourcesUtilities.GetResource( "AudienceSegmentsPerformanceFile","Global")},

            };

            return _WriteDashboardHelper.BuildIAudienceSegmentsPerformanceExportFile(audianceList.ToList(), type, true, true, keyValueDtosList, NameTitle);


        }


        #region Private Members

        private void GetDates(int period, out DateTime fromDate, out DateTime toDate)
        {

            switch (period)
            {
                case 0:
                    fromDate = Framework.Utilities.Environment.GetServerTime().Date;
                    toDate = Framework.Utilities.Environment.GetServerTime();
                    break;
                case 1:
                    fromDate = Framework.Utilities.Environment.GetServerTime().AddDays(-1).ToUniversalTime().Date;
                    toDate = Framework.Utilities.Environment.GetServerTime().ToUniversalTime().Date.AddSeconds(-1);
                    break;
                case 2:
                    fromDate = Framework.Utilities.Environment.GetServerTime().AddDays(-7).ToUniversalTime().Date;
                    toDate = Framework.Utilities.Environment.GetServerTime().ToUniversalTime().Date.AddSeconds(-1);
                    break;
                case 3:
                    fromDate = Framework.Utilities.Environment.GetServerTime().AddDays((Framework.Utilities.Environment.GetServerTime().Day - 1) * -1).ToUniversalTime().Date;
                    toDate = Framework.Utilities.Environment.GetServerTime().ToUniversalTime();
                    break;
                case 4:
                    fromDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-1).AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-1).Day - 1) * -1).ToUniversalTime().Date;
                    //var tempdate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-1);//new DateTime(ArabyAds.Framework.Utilities.Environment.GetServerTime().Year, Framework.Utilities.Environment.GetServerTime().Month, 1).ToUniversalTime().Date;
                    //fromDate = new DateTime(tempdate.Year, tempdate.Month, 1).ToUniversalTime().Date;
                    //toDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().Day) * -1).AddDays(1).ToUniversalTime().Date;
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                case 5:
                    fromDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-3).AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-3).Day - 1) * -1).ToUniversalTime().Date;
                    //var tempdate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-1);//new DateTime(ArabyAds.Framework.Utilities.Environment.GetServerTime().Year, Framework.Utilities.Environment.GetServerTime().Month, 1).ToUniversalTime().Date;
                    //fromDate = new DateTime(tempdate.Year, tempdate.Month, 1).ToUniversalTime().Date;
                    //toDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().Day) * -1).AddDays(1).ToUniversalTime().Date;
                    toDate = fromDate.AddMonths(3).AddDays(-1);
                    break;
                case 6:
                    fromDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-6).AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-6).Day - 1) * -1).ToUniversalTime().Date;
                    //var tempdate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMonths(-1);//new DateTime(ArabyAds.Framework.Utilities.Environment.GetServerTime().Year, Framework.Utilities.Environment.GetServerTime().Month, 1).ToUniversalTime().Date;
                    //fromDate = new DateTime(tempdate.Year, tempdate.Month, 1).ToUniversalTime().Date;
                    //toDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays((ArabyAds.Framework.Utilities.Environment.GetServerTime().Day) * -1).AddDays(1).ToUniversalTime().Date;
                    toDate = fromDate.AddMonths(6).AddDays(-1);
                    break;
                default:
                    fromDate = ArabyAds.Framework.Utilities.Environment.GetServerTime().Date;
                    toDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                    break;
            }

        }

        private void GetPerformanceOrderSetting(string order, out string orderColumn, out string orderType, string type)
        {

            if (string.IsNullOrEmpty(order))
            {
                if (string.IsNullOrEmpty(type) || type.ToLower() == "appsite")
                    orderColumn = "AppSiteName";

                if (type.ToLower() == "deal" || type.ToLower() == "lmpressionlog")
                    orderColumn = "Name";
                else
                    orderColumn = "CampaignName";

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

        private List<AppSitePerformanceDto> BindAppPerformance(out int count, int? page, int itemsPerPage, string orderColumn, string orderType)
        {
            DashboardPerformanceCriteria criteria = new DashboardPerformanceCriteria();
            criteria.OrderType = orderType;
            criteria.OrderColumn = orderColumn;
            criteria.ItemsPerPage = itemsPerPage;
            criteria.PageNumber = (page == null ? 0 : page.Value - 1);
            criteria.FromDate = Framework.Utilities.Environment.GetServerTime().Date;
            criteria.ToDate = Framework.Utilities.Environment.GetServerTime();

            List<AppSitePerformanceDto> appsitePerformanceList = _ReportService.GetAppSitePerformance(criteria);
            count = appsitePerformanceList.FirstOrDefault() != null ? (int)appsitePerformanceList.First().TotalCount : 0;//_ReportService.GetTotalAppSitePerformance(criteria);
            return appsitePerformanceList;
        }

        private void GetOrderSetting(string order, out string orderColumn, out string orderType)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: GetOrderSetting : order = " + order);

            if (string.IsNullOrEmpty(order))
            {
                orderColumn = "CountryName";
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
            Framework.ApplicationContext.Instance.Logger.Warn("end func: GetOrderSetting : order = " + order);

        }


        private List<AppSiteGeoLocationDto> BindAppGeoLocation(out int count, int? page, int numberofRecords, DateTime fromDate, DateTime toDate, int? countryId, int? appSiteId, string orderColumn, string orderType)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            DashboardGeoLocationCriteria criteria = new DashboardGeoLocationCriteria();
            criteria.IdFilter = appSiteId;
            criteria.FromDate = fromDate;
            criteria.ToDate = toDate;
            criteria.ItemsPerPage = numberofRecords;
            criteria.PageNumber = (page.HasValue ? page.Value - 1 : 0);
            criteria.OrderColumn = orderColumn;
            criteria.OrderType = orderType;
            criteria.CountryId = countryId;
            criteria.IsPrimaryUser = IsPrimaryUser;
            criteria.userId = UserId;
            List<AppSiteGeoLocationDto> appsitePerformanceList = _ReportService.GetAppSiteGeoLocation(criteria);

            count = appsitePerformanceList.FirstOrDefault() != null ? (int)appsitePerformanceList.First().TotalCount : 0;//_ReportService.GetTotalAppSiteGeoLocation(criteria);

            return appsitePerformanceList;
        }

        private List<AdGeoLocationDto> BindAdGeoLocation(out int count, int? page, int numberofRecords, DateTime fromDate, DateTime toDate, int? countryId, int? campaignId, string orderColumn, string orderType)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: BindAdGeoLocation : orderType = " + orderType);

            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            var criteria = new DashboardGeoLocationCriteria
            {
                IdFilter = campaignId,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = (page.HasValue ? page.Value - 1 : 0),
                ItemsPerPage = numberofRecords,
                OrderColumn = orderColumn,
                OrderType = orderType,
                CountryId = countryId,
                IsPrimaryUser = IsPrimaryUser,
                userId = UserId
            };
            GetAdvertiserId(criteria);
            List<AdGeoLocationDto> adGeoLocationList = _ReportService.GetAdGeoLocation(criteria);

            count = adGeoLocationList.FirstOrDefault() != null ? (int)adGeoLocationList.First().TotalCount : 0;//_ReportService.GetTotalAdGeoLocation(criteria);
            Framework.ApplicationContext.Instance.Logger.Warn("End func: BindAdGeoLocation : orderType = " + orderType);

            return adGeoLocationList;
        }

        private List<AdPerformanceDto> BindAdPerformance(out int count, int? page, int itemsPerPage, string orderColumn, string orderType)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: BindAdPerformance : orderType = " + orderType);

            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            var criteria = new DashboardPerformanceCriteria
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = (page == null ? 0 : page.Value - 1),
                OrderColumn = orderColumn,
                OrderType = orderType,
                FromDate = Framework.Utilities.Environment.GetServerTime().ToUniversalTime().Date,
                ToDate = Framework.Utilities.Environment.GetServerTime().ToUniversalTime(),
                IsPrimaryUser = IsPrimaryUser,
                userId = UserId
            };
            GetAdvertiserId(criteria);
            List<AdPerformanceDto> adPerformanceList = _ReportService.GetAdPerformance(criteria);
            count = adPerformanceList.FirstOrDefault() != null ? (int)adPerformanceList.First().TotalCount : 0; //_ReportService.GetTotalAdPerformance(criteria);
            Framework.ApplicationContext.Instance.Logger.Warn("End func: BindAdPerformance : orderType = " + orderType);

            return adPerformanceList;
        }


        private void GetAdvertiserId(BaseCriteriaDto item)
        {

            if (RouteData.Values["id"] != null)
            {
                item.AccountAdvertiserId = Convert.ToInt32(RouteData.Values["id"]);
                return;
            }
            else if (Request.Query.ContainsKey("id"))
            {
                item.AccountAdvertiserId = Convert.ToInt32(Request.Query["id"]);
                return;
            }
            else if (Request.HasFormContentType && Request.Form.ContainsKey("AdvertiserAccountId") && !string.IsNullOrEmpty(Request.Form["AdvertiserAccountId"]))
            {
                item.AccountAdvertiserId = Convert.ToInt32(Request.Form["AdvertiserAccountId"]);
                return;
            }
            else if (Request.HasFormContentType && Request.Form.ContainsKey("AdvertiserId") && !string.IsNullOrEmpty(Request.Form["AdvertiserId"]))
            {
                item.AdvertiserId = Convert.ToInt32(Request.Form["AdvertiserId"]);
                return;
            }

        }

        private void GetAdvertiserId(CampaignCriteria item)
        {

            if (RouteData.Values["id"] != null)
            {
                item.AdvertiserAccountId = Convert.ToInt32(RouteData.Values["id"]);
                return;
            }
            else if (Request.Query.ContainsKey("id"))
            {
                item.AdvertiserAccountId = Convert.ToInt32(Request.Query["id"]);
                return;
            }
            else if (Request.HasFormContentType && Request.Form.ContainsKey("AdvertiserAccountId") && !string.IsNullOrEmpty(Request.Form["AdvertiserAccountId"]))
            {
                item.AdvertiserAccountId = Convert.ToInt32(Request.Form["AdvertiserAccountId"]);
                return;
            }
            else if (Request.HasFormContentType && Request.Form.ContainsKey("AdvertiserId") && !string.IsNullOrEmpty(Request.Form["AdvertiserId"]))
            {
                item.AdvertiserId = Convert.ToInt32(Request.Form["AdvertiserId"]);
                return;
            }
        }
        #endregion

        public ActionResult GetExternalDataProviderQueryResultAllResultActionResult()
        {
            var oPartyDtoList = GetExternalDataProviderQueryResultAllResult();
            return Json(new { status = "1", result = oPartyDtoList });
        }

        public ActionResult GetBulkKPI([FromBody] GetBulkKPIRequest request)
        {
            IList<KPIDTO> kpis = new List<KPIDTO>();
            IList<KPIConfigDto> AllKpiConfigs = null;
            var kpiRequest = new GetKPIRequest { FromDate = request.FromDate, ToDate = request.ToDate };

            switch (request.KPIScope)
            {

                case KPIScope.Campaign:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                   
                    break;


                case KPIScope.AdGroup:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                   
                    break;


                case KPIScope.Ads:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                    break;
                case KPIScope.Advertiser:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForAdvertisers();
                  
                    int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                    request.AccountId = accountId;
                    break;
                case KPIScope.DataProvider:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForDataProviders();
                 
                    break;
                case KPIScope.Deals:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForDeals();
                   
                    break;
                case KPIScope.Publisher:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForPublishers();
                   
                    break;
                default:
                    break;

            }

            foreach (var filter in request.Filters)
            {
                KPIConfigDto KpiConfig = _KPIService.GetKPIConfig(ValueMessageWrapper.Create(filter));
                if (KpiConfig == null) continue;
                kpiRequest.KPIScope = request.KPIScope;
                kpiRequest.Ids = request.Ids;
                kpiRequest.DataBaseFields = new List<string> { KpiConfig.DataBaseField };
                kpiRequest.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                var kpiDTOs = _KPIService.GetKPIs(kpiRequest);
                if (kpiDTOs?.Count > 0)
                {
                    FillKpiDTO(kpiDTOs[0], KpiConfig);
                    kpis.Add( kpiDTOs[0]);
                }
            }
            return Json(new { defaultKPIs = kpis , AllKPIConfigs = AllKpiConfigs });
        }

        public ActionResult GetKPI([FromBody] GetKPIRequest request)
        {
            KPIConfigDto KpiConfig = _KPIService.GetKPIConfig(ValueMessageWrapper.Create(request.KpiConfigId.Value));
            if (KpiConfig == null) return null;
            request.DataBaseFields = new List<string> { KpiConfig.DataBaseField };
            request.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var kpiDTOs = _KPIService.GetKPIs(request);
            if (kpiDTOs?.Count > 0)
            {
                FillKpiDTO(kpiDTOs[0], KpiConfig);
                return Json(kpiDTOs[0]);
            }
            return Json(null);
        }
        public ActionResult GetDefaultKPIs([FromBody] GetKPIRequest request)
        {
            IList<KPIConfigDto> defaultKpiConfigs = null;
            IList<KPIConfigDto> noneDefaultKpiConfigs = null;

            IList<KPIConfigDto> AllKpiConfigs = null;
            switch (request.KPIScope)
            {

                case KPIScope.Campaign:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); // _KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(false));
                    break;


                case KPIScope.AdGroup:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); // _KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(false));
                    break;


                case KPIScope.Ads:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForCampaigns();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); //_KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForCampaigns(ValueMessageWrapper.Create(false));
                    break;
                case KPIScope.Advertiser:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForAdvertisers();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); //_KPIService.GetKPIConfigsForAdvertisers(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForAdvertisers(ValueMessageWrapper.Create(false));
                    int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                    request.AccountId = accountId;
                    break;
                case KPIScope.DataProvider:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForDataProviders();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); //_KPIService.GetKPIConfigsForDataProviders(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForDataProviders(ValueMessageWrapper.Create(false));
                    break;
                case KPIScope.Deals:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForDeals();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); //_KPIService.GetKPIConfigsForDeals(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForDeals(ValueMessageWrapper.Create(false));
                    break;
                case KPIScope.Publisher:
                    AllKpiConfigs = _KPIService.GetKPIConfigsAllForPublishers();
                    defaultKpiConfigs = AllKpiConfigs.Where(m => m.IsDefault).ToList(); //_KPIService.GetKPIConfigsForPublishers(ValueMessageWrapper.Create(true));
                    noneDefaultKpiConfigs = AllKpiConfigs.Where(m => !m.IsDefault).ToList();  //_KPIService.GetKPIConfigsForPublishers(ValueMessageWrapper.Create(false));
                    break;
                default:
                    break;

            }
            IList<KPIDTO> kpiDTOS = null;
            if (defaultKpiConfigs?.Count > 0)
            {
                request.DataBaseFields = defaultKpiConfigs.Select(m => m.DataBaseField).ToList();
                request.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

                kpiDTOS = _KPIService.GetKPIs(request);
                foreach (var kpiDTO in kpiDTOS)
                {
                    var config = defaultKpiConfigs.Single(mbox => mbox.DataBaseField == kpiDTO.DBField);
                    FillKpiDTO(kpiDTO, config);
                }
            }
            return Json(new { defaultKPIs = kpiDTOS ?? new List<KPIDTO>(), otherKPIConfigs = noneDefaultKpiConfigs, AllKPIConfigs = AllKpiConfigs });
        }
        private void FillKpiDTO(KPIDTO kpiDTO, KPIConfigDto config)
        {
            kpiDTO.Title = config.Name.Value;
            if (kpiDTO.MetricValue.HasValue)
            {
                if (config.DisplayFormat == "C")
                    kpiDTO.MainValue = FormatHelper.FormatMoney(kpiDTO.MetricValue.Value);
                else if (config.DisplayFormat == "P")
                    kpiDTO.MainValue = FormatHelper.FormatPercentage(kpiDTO.MetricValue.Value);
                else if (config.DisplayFormat == "N")
                    kpiDTO.MainValue = kpiDTO.MetricValue.Value.ToString();
            }
            else
            {
                kpiDTO.MainValue = "N/A";
            }
            kpiDTO.Icon = config.Icon;
            kpiDTO.PercentValue = kpiDTO.MetricGrowthValue.HasValue ? FormatHelper.FormatPercentageAsValue((decimal)kpiDTO.MetricGrowthValue.Value) : "N/A";
            if (kpiDTO.MetricGrowthValue != 0)
                kpiDTO.PercentValueState = kpiDTO.MetricGrowthValue > 0 ? "up" : "down";
            kpiDTO.PercentValueDisc = "From previous period";
            kpiDTO.Id = config.ID;
        }
        public ActionResult GetDashboardCardData(string chartType)
        {
            return Json(
                new
                {
                    data = new[]{
                        new {
                            title="Impressions",
                            mainValue="58874",
                            icon="ri-stack-line font-size-48",
                            percentValue="2.4%",
                            percentValueState="up",
                            percentValueDisc="From previous period",
                            Id=1,
                        },
                        new {
                            title="Clicks",
                            mainValue="3475",
                            icon="ri-drag-drop-line mr-3 font-size-48",
                            percentValue="47.1%",
                            percentValueState="up",
                            percentValueDisc="From previous period",
                              Id=2,
                        },
                        new {
                            title="Conversions",
                            mainValue="578",
                            icon="ri-store-2-line font-size-48",
                            percentValue="2.4%",
                            percentValueState="down",
                            percentValueDisc="From previous period",
                             Id=5,
                        },
                        new {
                            title="Total Spend",
                            mainValue="$8715",
                            icon="ri-money-dollar-box-line font-size-48",
                            percentValue="2.4%",
                            percentValueState="up",
                            percentValueDisc="From previous period",
                               Id=5,
                        }
                    },
                    KPIConfig = _ReportService.GetKPIConfigs()
                });
        }






    }

}
