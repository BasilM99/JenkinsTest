using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.IO;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using System.Drawing;
using Noqoush.Framework.Utilities;
using Telerik.Web.Mvc;

using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Web.Helper;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Services.Services;
using Noqoush.Framework.Logging;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class DashboardTestController : AuthorizedControllerBase
    {
        ISummaryService _SummaryService;
        private IAppSiteService _appsiteService;
        private IMetricTestService _MetricService;
        private IReportTestService _ReportService;
        private ICampaignService _CampaignService;
        private WriteDashboardDocumentsHelper _WriteDashboardHelper;

        public DashboardTestController(IAppSiteService appSiteService, IMetricTestService metricService, IReportTestService reportService,ICampaignService campaignService,ISummaryService summaryService)
        {
            _appsiteService = appSiteService;
            _MetricService = metricService;
            _ReportService = reportService;
            _CampaignService = campaignService;
            _WriteDashboardHelper = new WriteDashboardDocumentsHelper();
            _SummaryService = summaryService;
        }

        public ActionResult Index(string chartType)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Dashboard", "SiteMapLocalizations"),
                Order = 1,
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            ViewData["chartType"] = chartType;
            return View();
        }

        public ActionResult Chart()
        {
            return PartialView();
        }

        public ActionResult AppChart(int? page, string orderBy)
        {

            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            ViewData["AppSites"] = _appsiteService.GetAppSitesByAccountId(accountId);
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AppSiteGeoLocationDto> appsitePerformanceList = BindAppGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            ViewData["AppSiteGeoLocation"] = appsitePerformanceList;
            ViewData["Total"] = count;


            decimal totalDayRevenue = _SummaryService.GetAccountTotalRevenue(Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime());

            ViewData["totalDayRevenue"] = FormatHelper.FormatMoney(totalDayRevenue);


            return PartialView();
        }

        public ActionResult AdChart(int? page, string orderBy)
        {
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

                criteria.IsPrimaryUser = IsPrimaryUser;
            }
                ViewData["Campaigns"] = _CampaignService.QueryByCratiria(criteria).Items.ToList();
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AdGeoLocationDto> aGeoLocationList = BindAdGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            ViewData["AdGeoLocation"] = aGeoLocationList;
            ViewData["Total"] = count;

            decimal totalDaySpend = _SummaryService.GetAccountTotalSpend(Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime());

            ViewData["totalDaySpend"] = FormatHelper.FormatMoney(totalDaySpend);

            return PartialView();

        }
        public ActionResult GAppChart(int? page, string orderBy)
        {

            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ViewData["AppSites"] = _appsiteService.GetAppSitesByAccountId(accountId);

          
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AppSiteGeoLocationDto> appsitePerformanceList = BindAppGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            ViewData["AppSiteGeoLocation"] = appsitePerformanceList;
            ViewData["Total"] = count;


            decimal totalDayRevenue = _SummaryService.GetAccountTotalRevenue(Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime());

            ViewData["totalDayRevenue"] = FormatHelper.FormatMoney(totalDayRevenue);


            return PartialView();
        }

        public ActionResult GAdChart(int? page, string orderBy)
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
            ViewData["Campaigns"] = _CampaignService.QueryByCratiria(criteria).Items.ToList();
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "campaign").ToList();
            ViewData["Metrics"] = metricDtoList;

            string orderType;
            string orderColumn;

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            int count;
            List<AdGeoLocationDto> aGeoLocationList = BindAdGeoLocation(out count, page, 10, Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime(), null, null, orderColumn, orderType);
            ViewData["AdGeoLocation"] = aGeoLocationList;
            ViewData["Total"] = count;

            decimal totalDaySpend = _SummaryService.GetAccountTotalSpend(Framework.Utilities.Environment.GetServerTime().Date, Framework.Utilities.Environment.GetServerTime());

            ViewData["totalDaySpend"] = FormatHelper.FormatMoney(totalDaySpend);

            return PartialView();

        }
        [GridAction(EnableCustomBinding=true)]
        [HttpPost]
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

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate,toDate;

            GetDates(period,out fromDate,out toDate);

            int count;
            List<AppSiteGeoLocationDto> appsiteGeoLocationList = BindAppGeoLocation(out count, page, 10, fromDate, toDate, countryId, appSiteId, orderColumn, orderType);
            ViewData.Model = appsiteGeoLocationList;
            return View(new GridModel
            {
                Data = appsiteGeoLocationList,
                Total = count
            });
        }

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

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;
            int count;
            GetDates(period, out fromDate, out toDate);

            List<AppSiteGeoLocationDto> appsiteGeoLocationList = BindAppGeoLocation(out count, null, int.MaxValue, fromDate, toDate, countryId, appSiteId, orderColumn, orderType);

            return _WriteDashboardHelper.BuildAppGeoLocationExportFile(appsiteGeoLocationList, type);

        }

        public ActionResult AppPerformance(int? page, string orderBy)
        {

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");
            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, page,10, orderColumn, orderType);

            ViewData["total"] = count;
            ViewData.Model = appsitePerformanceList;
            return PartialView();
        }

        public ActionResult AppPerformanceExport(int? page, string orderBy,string type)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");
            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, null, int.MaxValue,orderColumn, orderType);

            return _WriteDashboardHelper.BuildAppPerformanceExportFile(appsitePerformanceList, type);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding=true)]
        public ActionResult AppPerformance(int? page,string orderBy,FormCollection collection)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "appsite");

            int count;
            List<AppSitePerformanceDto> appsitePerformanceList = BindAppPerformance(out count, page, 10, orderColumn, orderType);
            ViewData.Model = appsitePerformanceList;
            return View(new GridModel
                {
                    Data = appsitePerformanceList,
                    Total = count
                });
        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        public ActionResult AdGeoLocation(string orderBy, int? page, string country, string list, int period)
        {
            string orderType;
            string orderColumn;
            int? countryId, campaignId;
            countryId = campaignId = null;

            if (!string.IsNullOrEmpty(country))
                countryId = int.Parse(country);

            if (!string.IsNullOrEmpty(list))
                campaignId = int.Parse(list);

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;

            GetDates(period, out fromDate, out toDate);

            int count;
            List<AdGeoLocationDto> adGeoLocationList = BindAdGeoLocation(out count, page, 10, fromDate, toDate, countryId, campaignId, orderColumn, orderType);
            ViewData.Model = adGeoLocationList;
            return View(new GridModel
            {
                Data = adGeoLocationList,
                Total =count
            });
        }

        public ActionResult AdGeoLocationExport(string type, string country, string list, int period, string orderBy)
        {
            string orderType;
            string orderColumn;
            int? countryId, campaignId;
            countryId = campaignId = null;

            if (!string.IsNullOrEmpty(country))
                countryId = int.Parse(country);

            if (!string.IsNullOrEmpty(list))
                campaignId = int.Parse(list);

            GetOrderSetting(orderBy, out orderColumn, out orderType);
            DateTime fromDate, toDate;
            int count;
            GetDates(period, out fromDate, out toDate);

            List<AdGeoLocationDto> adGeoLocationList = BindAdGeoLocation(out count, null, int.MaxValue, fromDate, toDate, countryId, campaignId, orderColumn, orderType);

            return _WriteDashboardHelper.BuildAdGeoLocationExportFile(adGeoLocationList, type);

        }

        public ActionResult ChartControl(int periodOption, string metricCode, string id, string type)
        {
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

                chartDtoList = _ReportService.GetAdChart(criteria);
            }

            return new ChartResult { Color = color, ToDate = Convert.ToDateTime(toDate).ToUniversalTime(), FromDate = Convert.ToDateTime(fromDate).ToUniversalTime(), ChartDtoList = chartDtoList, Width = 680, Height = 350 };
        }
        public JsonResult GChartControl(int periodOption, string metricCode, string id, string type)
        {
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

                chartDtoList = _ReportService.GetAdChart(criteria);
            }

            var googleChartresult= new GoogleChartResult { Color = color, ToDate = Convert.ToDateTime(toDate).ToUniversalTime(), FromDate = Convert.ToDateTime(fromDate).ToUniversalTime(), ChartDtoList = chartDtoList, Width = 680, Height = 350 };
            googleChartresult.ExecuteResult();
            return  Json(googleChartresult,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult AdPerformance(int? page, string orderBy, FormCollection collection)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "ad");

            int count;
            List<AdPerformanceDto> adPerformanceList = BindAdPerformance(out count, page, 10, orderColumn, orderType);
            ViewData.Model = adPerformanceList;
            return View(new GridModel
            {
                Data = adPerformanceList,
                Total = count
            });
        }

        public ActionResult AdPerformanceExport(int? page, string orderBy, string type)
        {
            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType, "ad");
            int count;
            List<AdPerformanceDto> appsitePerformanceList = BindAdPerformance(out count, page, int.MaxValue, orderColumn, orderType);

            return _WriteDashboardHelper.BuildAdPerformanceExportFile(appsitePerformanceList, type);
        }

        public ActionResult AdPerformance(int? page, string orderBy)
        {

            string orderType;
            string orderColumn;

            GetPerformanceOrderSetting(orderBy, out orderColumn, out orderType,"ad");
            int count;
            List<AdPerformanceDto> adPerformanceList = BindAdPerformance(out count, page, 10, orderColumn, orderType);

            ViewData["total"] = count;
            ViewData.Model = adPerformanceList;
            return PartialView();
        }

        #region Private Members

        private void GetDates(int period,out DateTime fromDate,out DateTime toDate)
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
                    fromDate = Noqoush.Framework.Utilities.Environment.GetServerTime().AddMonths(-1).AddDays((Noqoush.Framework.Utilities.Environment.GetServerTime().AddMonths(-1).Day - 1) * -1).ToUniversalTime().Date;
                    //var tempdate = Noqoush.Framework.Utilities.Environment.GetServerTime().AddMonths(-1);//new DateTime(Noqoush.Framework.Utilities.Environment.GetServerTime().Year, Framework.Utilities.Environment.GetServerTime().Month, 1).ToUniversalTime().Date;
                    //fromDate = new DateTime(tempdate.Year, tempdate.Month, 1).ToUniversalTime().Date;
                    //toDate = Noqoush.Framework.Utilities.Environment.GetServerTime().AddDays((Noqoush.Framework.Utilities.Environment.GetServerTime().Day) * -1).AddDays(1).ToUniversalTime().Date;
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                default:
                    fromDate = Noqoush.Framework.Utilities.Environment.GetServerTime().Date;
                    toDate = Noqoush.Framework.Utilities.Environment.GetServerTime();
                    break;
            }
        }

        private void GetPerformanceOrderSetting(string order,out string orderColumn, out string orderType,string type)
        {
            if (string.IsNullOrEmpty(order))
            {
                if (string.IsNullOrEmpty(type) || type.ToLower() == "appsite")
                    orderColumn = "AppSiteName";
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

        private List<AppSitePerformanceDto> BindAppPerformance(out int count,int? page,int itemsPerPage,string orderColumn,string orderType)
        {
            DashboardPerformanceCriteria criteria = new DashboardPerformanceCriteria();
            criteria.OrderType = orderType;
            criteria.OrderColumn = orderColumn;
            criteria.ItemsPerPage = itemsPerPage;
            criteria.PageNumber = (page == null ? 0 : page.Value - 1);
            criteria.FromDate = Framework.Utilities.Environment.GetServerTime().Date;
            criteria.ToDate = Framework.Utilities.Environment.GetServerTime();

            List<AppSitePerformanceDto> appsitePerformanceList = _ReportService.GetAppSitePerformance(criteria);
            count = appsitePerformanceList.FirstOrDefault() != null ?(int)appsitePerformanceList.First().TotalCount : 0;//_ReportService.GetTotalAppSitePerformance(criteria);
            return appsitePerformanceList;
        }

        private void GetOrderSetting(string order, out string orderColumn, out string orderType)
        {
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
        }

        private List<AppSiteGeoLocationDto> BindAppGeoLocation(out int count, int? page,int numberofRecords,DateTime fromDate,DateTime toDate,int? countryId,int? appSiteId, string orderColumn, string orderType)
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
           
            count = appsitePerformanceList.FirstOrDefault()!=null?(int)appsitePerformanceList.First().TotalCount:0;//_ReportService.GetTotalAppSiteGeoLocation(criteria);

            return appsitePerformanceList;
        }

        private List<AdGeoLocationDto> BindAdGeoLocation(out int count, int? page, int numberofRecords, DateTime fromDate, DateTime toDate, int? countryId, int? campaignId, string orderColumn, string orderType)
        {
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

            List<AdGeoLocationDto> adGeoLocationList = _ReportService.GetAdGeoLocation(criteria);

            count = adGeoLocationList.FirstOrDefault() != null ? (int)adGeoLocationList.First().TotalCount : 0;//_ReportService.GetTotalAdGeoLocation(criteria);

            return adGeoLocationList;
        }

        private List<AdPerformanceDto> BindAdPerformance(out int count, int? page, int itemsPerPage, string orderColumn, string orderType)
        {
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

            List<AdPerformanceDto> adPerformanceList = _ReportService.GetAdPerformance(criteria);
            count = adPerformanceList.FirstOrDefault() != null ? (int)adPerformanceList.First().TotalCount : 0; //_ReportService.GetTotalAdPerformance(criteria);
            return adPerformanceList;
        }

        #endregion

    }
}
