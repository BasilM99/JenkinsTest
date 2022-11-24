using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Telerik.Web.Mvc;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

using Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance;
using System.Threading;
using System.Globalization;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Common.UserInfo;

namespace Noqoush.AdFalcon.Administration.Web.Controllers
{
    public class AppOpsTestController : AuthorizedControllerBase
    {
        private IAppSiteService _appSiteService;
        private IAppSiteStatusService _appSiteStatusService;
        private IAppSiteTypeService _appSiteTypeService;
        private IPerformanceReportTestService _performanceReport;
        private ICountryService _countryService;
        private IUserService _userService;

        private Dictionary<string, string> AppChartMetrics = new Dictionary<string, string>();

        public AppOpsTestController(IAppSiteService appSiteService,
                                IAppSiteStatusService appSiteStatusService, IAppSiteTypeService appSiteTypeService, IPerformanceReportTestService performanceReportService,
                                ICountryService countryService, IUserService userService)
        {
            _appSiteService = appSiteService;
            _appSiteStatusService = appSiteStatusService;
            _appSiteTypeService = appSiteTypeService;
            _performanceReport = performanceReportService;
            _countryService = countryService;
            _userService = userService;

            AppChartMetrics = FillAppChartMetrics();
        }

        #region Actions

        #region Performance


        [AuthorizeRole(Roles = "Administrator")]
        public ActionResult Dashboard()
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AppOpsDashboard","SiteMapLocalizations"),
                                                          Order = 1
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion


            var selectItems = AppChartMetrics.Select(p => new SelectListItem() { Value = p.Key, Text = p.Value }).ToList();
            selectItems.First().Selected = true;

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();

            ViewData.Model = new DashboardViewModel()
            {
                DateTo = serverDateTime,
                DateFrom = serverDateTime.AddDays(-7),
                MetricsList = selectItems
            };

            return View();
        }

        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator")]
        public ActionResult TopAccountsGrid(BaseAppSiteAccountPerformanceSearchInfo searchInfo)
        {
            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";


            BaseAppSitePerformanceDetailsCriteria criteria = new BaseAppSitePerformanceDetailsCriteria();

            GetTopAccountsPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var dataArray = ConvertToBaseChartDashboardView(result, searchInfo.MetricValue);

            return View(new GridModel
            {
                Data = dataArray,
                Total = (int)counter
            });
        }

        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator")]
        public ActionResult TopAppSitesGrid(BaseAppSitePerformanceSearchInfo searchInfo)
        {

            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";


            BaseAppSitePerformanceDetailsCriteria criteria = new BaseAppSitePerformanceDetailsCriteria();

            GetTopAppSitesPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var dataArray = ConvertToBaseChartDashboardView(result, searchInfo.MetricValue);

            return View(new GridModel
            {
                Data = dataArray,
                Total = (int)counter
            });
        }


        [AuthorizeRole(Roles = "Administrator")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TopAccountsChart(BaseAppSiteAccountPerformanceSearchInfo searchInfo)
        {
            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;
            BaseAppSitePerformanceDetailsCriteria criteria = new BaseAppSitePerformanceDetailsCriteria();

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";

            GetTopAccountsPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var jsonGoogleDataTable = FormatDataToCharts(result, counter, totalMetric, searchInfo.OrderColumn);

            return Content(jsonGoogleDataTable);
        }

        [AuthorizeRole(Roles = "Administrator")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TopAppSitesChart(BaseAppSitePerformanceSearchInfo searchInfo)
        {
            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            BaseAppSitePerformanceDetailsCriteria criteria = new BaseAppSitePerformanceDetailsCriteria();

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";

            GetTopAppSitesPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var jsonGoogleDataTable = FormatDataToCharts(result, counter, totalMetric, searchInfo.OrderColumn);

            return Content(jsonGoogleDataTable);
        }

        [AuthorizeRole(Roles = "Administrator")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PlatformsChart(BaseSearchInfo searchInfo, string orderColumn)
        {
            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;
            BaseAppSitePerformanceDetailsCriteria criteria = new BaseAppSitePerformanceDetailsCriteria();

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;

            GetPlatformsPerformanceReport(criteria, searchInfo, orderColumn, "desc", out result, out counter, out totalMetric);

            var jsonGoogleDataTable = FormatDataToCharts(result, counter, totalMetric, orderColumn);

            return Content(jsonGoogleDataTable);
        }



        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult AppSitesPerformance(int? Id, string dateFrom, string dateTo)
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AppSitePerformance","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AppOpsDashboard","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Dashboard", "AppOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.isAdmin = true;

            var selectItems = AppChartMetrics.Select(p => new SelectListItem() { Value = p.Key, Text = p.Value }).ToList();
            selectItems.First().Selected = true;

            var countriesList = _countryService.GetAll().Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name.ToString() }).OrderBy(p => p.Text);

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();

            if (Id.HasValue)
            {
                ViewData["AccountId"] = Id.Value;
                UserDto userInfo = _userService.GetUserByAccount(Id.Value,null);
                string accountName = !string.IsNullOrEmpty(userInfo.Company) ? userInfo.Company : userInfo.ToString();
                ViewData["AccountName"] = accountName;
            }

            AppOpsAppSitePerformanceViewModel model = new AppOpsAppSitePerformanceViewModel()
            {
                MetricsList = selectItems,
                DateTo = string.IsNullOrEmpty(dateTo) ? serverDateTime : DateTime.ParseExact(dateTo, Config.MainShortDateFormat, CultureInfo.InvariantCulture),
                DateFrom = string.IsNullOrEmpty(dateFrom) ? serverDateTime.AddDays(-7) : DateTime.ParseExact(dateFrom, Config.MainShortDateFormat, CultureInfo.InvariantCulture),
                Countries = countriesList
            };

            return View(model);
        }


        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult AppSitesPerformanceGrid(BaseAppSitePerformanceSearchInfo searchInfo, string countries, int? Id)
        {
            if (searchInfo.CountryIds == null && !string.IsNullOrEmpty(countries))
            {
                searchInfo.CountryIds = countries.Split(',').ToList().Select(p => int.Parse(p)).ToList();
            }

            if (Id.HasValue)
            {
                searchInfo.AccountId = Id;
            }

            var criteria = new BaseAppSitePerformanceDetailsCriteria();

            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            searchInfo.OrderBy = "desc";

            GetTopAppSitesPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            return View(new GridModel
            {
                Data = result,
                Total = (int)counter
            });
        }

        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        public ActionResult AppSitesPerformanceChart(BaseAppSitePerformanceSearchInfo searchInfo, int? Id)
        {
            var criteria = new BaseAppSitePerformanceDetailsCriteria();

            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            if (Id.HasValue)
            {
                searchInfo.AccountId = Id;
            }

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";

            GetTopAppSitesPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var jsonGoogleDataTable = FormatDataToCharts(result, counter, totalMetric, criteria.OrderColumn);

            return Content(jsonGoogleDataTable);
        }

        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult AccountsPerformance(string dateFrom, string dateTo)
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AccountsPerformance","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AppOpsDashboard","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Dashboard", "AppOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.isAdmin = true;

            var selectItems = AppChartMetrics.Select(p => new SelectListItem() { Value = p.Key, Text = p.Value }).ToList();
            selectItems.First().Selected = true;

            var countriesList = _countryService.GetAll().Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name.ToString() }).OrderBy(p => p.Text);

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();

            AppOpsAccountPerformanceViewModel model = new AppOpsAccountPerformanceViewModel()
            {
                MetricsList = selectItems,
                DateTo = string.IsNullOrEmpty(dateTo) ? serverDateTime : DateTime.ParseExact(dateTo, Config.MainShortDateFormat, CultureInfo.InvariantCulture),
                DateFrom = string.IsNullOrEmpty(dateFrom) ? serverDateTime.AddDays(-7) : DateTime.ParseExact(dateFrom, Config.MainShortDateFormat, CultureInfo.InvariantCulture),
                Countries = countriesList
            };



            return View(model);
        }


        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult AccountsPerformanceGrid(BaseAppSiteAccountPerformanceSearchInfo searchInfo, string countries)
        {
            if (searchInfo.CountryIds == null && !string.IsNullOrEmpty(countries))
            {
                searchInfo.CountryIds = countries.Split(',').ToList().Select(p => int.Parse(p)).ToList();
            }

            var criteria = new BaseAppSitePerformanceDetailsCriteria();

            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            searchInfo.OrderBy = "desc";

            GetTopAccountsPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            return View(new GridModel
            {
                Data = result,
                Total = (int)counter
            });
        }



        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        public ActionResult AccountsPerformanceChart(BaseAppSiteAccountPerformanceSearchInfo searchInfo)
        {
            var criteria = new BaseAppSitePerformanceDetailsCriteria();

            IList<BaseAppSitePerformanceDetailsDto> result;
            long counter;
            long totalMetric;

            searchInfo.Size = Config.PageSize;
            searchInfo.Page = null;
            searchInfo.OrderBy = "desc";

            GetTopAccountsPerformanceReport(criteria, searchInfo, out result, out counter, out totalMetric);

            var jsonGoogleDataTable = FormatDataToCharts(result, counter, totalMetric, criteria.OrderColumn);

            return Content(jsonGoogleDataTable);
        }


        #endregion

        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult AppSiteManagement()
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AppOps","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AppOpsDashboard","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Dashboard", "AppOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.isAdmin = true;
            //load the statues
            var statues = _appSiteStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();

            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            //load all types
            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();

            var model = new AppOpsManagementViewModel
            {
                AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() },
                Statuses = statuesDropDown,
                Types = typesDropDown,
                DateTo = serverDateTime,
                DateFrom = serverDateTime.AddDays(-30)
            };
            return View(model);
        }


        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult _AppSiteManagement(DateTime? DateFrom, DateTime? DateTo, string PublisherId, int? StatusId, int? TypeId, string AppSiteName, string CompanyName, string AccountName, int page, int size)
        {

            var criteria = new AppSiteCriteria
            {
                PublisherId = PublisherId,
                DateFrom = DateFrom,
                DateTo = DateTo,
                StatusId = StatusId,
                TypeId = TypeId,
                Name = AppSiteName,
                CompanyName = CompanyName,
                AccountName = AccountName,
                Page = page,
                Size = size
            };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
                //appCriteria.UserId = UserId;
            }
            if ((!criteria.DateFrom.HasValue) && (!criteria.DateTo.HasValue))
            {
                //get current month
                criteria.DateTo = Framework.Utilities.Environment.GetServerTime().Date;
                criteria.DateFrom = criteria.DateTo.Value.AddDays(-30);
            }

            var result = _appSiteService.QueryByAppOpsCratiria(criteria);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        #endregion


        #region Helpers

        private void GetTopAccountsPerformanceReport(BaseAppSitePerformanceDetailsCriteria criteria, BaseAppSiteAccountPerformanceSearchInfo searchInfo, out IList<BaseAppSitePerformanceDetailsDto> resultList, out long counter, out long totalMetric)
        {
            if (criteria == null) { throw new ArgumentNullException(); }

            criteria = GetBaseCritieria<BaseAppSitePerformanceDetailsCriteria>(criteria, searchInfo.FromDate, searchInfo.ToDate, searchInfo.Page, searchInfo.Size, searchInfo.MetricValue, searchInfo.OrderBy);
            criteria.CampaignType = CampaignType.Normal;
            criteria.CountryIds = searchInfo.CountryIds == null || searchInfo.CountryIds.Count() == 0 ? new List<int>() : searchInfo.CountryIds.Select(p => p).ToList();

            resultList = _performanceReport.GetAccountsPerformanceDetails(searchInfo.AccountName, searchInfo.AppSiteName, criteria);
            counter = resultList != null && resultList.Count > 0 ? resultList.First().TotalCount : 0;
            totalMetric = resultList != null && resultList.Count > 0 ? resultList.First().TotalMetricSum : 0;
        }

        private void GetTopAppSitesPerformanceReport(BaseAppSitePerformanceDetailsCriteria criteria, BaseAppSitePerformanceSearchInfo searchInfo, out IList<BaseAppSitePerformanceDetailsDto> resultList, out long counter, out long totalMetric)
        {
            if (criteria == null) { throw new ArgumentNullException(); }

            criteria = GetBaseCritieria<BaseAppSitePerformanceDetailsCriteria>(criteria, searchInfo.FromDate, searchInfo.ToDate, searchInfo.Page, searchInfo.Size, searchInfo.MetricValue, searchInfo.OrderBy);
            criteria.CampaignType = CampaignType.Normal;
            criteria.CountryIds = searchInfo.CountryIds == null || searchInfo.CountryIds.Count() == 0 ? new List<int>() : searchInfo.CountryIds.Select(p => p).ToList();
            if (searchInfo.AccountId.HasValue)
            {
                criteria.AccountIds = new int[] { searchInfo.AccountId.Value };
            }
            resultList = _performanceReport.GetAppSitesPerformanceDetails(searchInfo.AccountName, searchInfo.AppSiteName, criteria);
            counter = resultList != null && resultList.Count > 0 ? resultList.First().TotalCount : 0;
            totalMetric = resultList != null && resultList.Count > 0 ? resultList.First().TotalMetricSum : 0;
        }

        private void GetPlatformsPerformanceReport(BaseAppSitePerformanceDetailsCriteria criteria, BaseSearchInfo searchInfo, string orderColumn, string orderBy, out IList<BaseAppSitePerformanceDetailsDto> resultList, out long counter, out long totalMetric)
        {
            if (criteria == null) { throw new ArgumentNullException(); }

            criteria = GetBaseCritieria<BaseAppSitePerformanceDetailsCriteria>(criteria, searchInfo.FromDate, searchInfo.ToDate, searchInfo.Page, searchInfo.Size, orderColumn, orderBy);
            criteria.CampaignType = CampaignType.Normal;

            resultList = _performanceReport.GetPlatformsPerformanceDetails(criteria);
            counter = resultList != null && resultList.Count > 0 ? resultList.First().TotalCount : 0;
            totalMetric = resultList != null && resultList.Count > 0 ? resultList.First().TotalMetricSum : 0;
        }

        private T GetBaseCritieria<T>(T criteria, string fromDate, string toDate, int? pageNumber, int? itemsPerPage, string orderColumn, string orderBy)
            where T : BasePagingCriteriaDto
        {
            FixOrderParameters(ref orderColumn, ref orderBy);
            FixDates(ref fromDate, ref toDate);

            criteria.FromDate = DateTime.ParseExact(fromDate, Noqoush.AdFalcon.Web.Controllers.Utilities.Config.MainShortDateFormat, CultureInfo.InvariantCulture);
            criteria.ToDate = DateTime.ParseExact(toDate, Noqoush.AdFalcon.Web.Controllers.Utilities.Config.MainShortDateFormat, CultureInfo.InvariantCulture);

            criteria.ItemsPerPage = itemsPerPage == null ? Config.PageSize : itemsPerPage.Value;
            criteria.PageNumber = pageNumber == null ? 0 : pageNumber.Value - 1;
            criteria.OrderColumn = string.IsNullOrEmpty(orderColumn) ? "requests" : MatchPropertyNameWithOrderColumn(orderColumn);
            criteria.OrderType = string.IsNullOrEmpty(orderBy) ? "desc" : orderBy;

            return criteria;
        }

        private string FormatDataToCharts<T>(IList<T> result, long counter, long totalMetric, string metricName)
            where T : BaseAppSiteResultDto
        {
            var keyValueMetric = this.AppChartMetrics.Where(p => string.Equals(p.Key, metricName, StringComparison.CurrentCultureIgnoreCase)).Single();

            var dataArray = ConvertToBaseChartDashboardView(result, metricName);

            //AddMtricPercentage(dataArray, totalMetric);

            List<GoogleDataTableColumn> columnsList = new List<GoogleDataTableColumn>()
            {
                new GoogleDataTableColumn()
                {
                    Id= "AccountName",
                    Label = ResourcesUtilities.GetResource("AccountName","TopAccounts"),
                    Type = "string"
                },
                new GoogleDataTableColumn()
                {
                    Id= "MetricValue",
                    Label = keyValueMetric.Value,
                    Type = "number"
                }
            };

            string jsonGoogleDataTable = GoogleControlsHelper.ConvertIListToDataTable<BaseChartDashboardView>(dataArray, columnsList);

            return jsonGoogleDataTable;

        }


        private void FixDates(ref string fromDate, ref string toDate)
        {
            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();
            string shortDateTime = Noqoush.AdFalcon.Web.Controllers.Utilities.Config.MainShortDateFormat;

            if (string.IsNullOrEmpty(fromDate))
            {
                fromDate = serverDateTime.AddDays(-7).ToString(shortDateTime);
            }

            if (string.IsNullOrEmpty(toDate))
            {
                toDate = serverDateTime.ToString(shortDateTime);
            }
        }

        /// <summary>
        /// Get the property name in AccountPerformanceDetailsDto class
        /// </summary>
        /// <param name="metricName"></param>
        /// <returns></returns>
        private string MatchOrderColumnWithPropertyName(string metricName)
        {
            string propertyName = string.Empty;

            switch (metricName.ToLower())
            {
                case "requests":
                    propertyName = "AdRequests";
                    break;
                case "impressions":
                    propertyName = "AdImpress";
                    break;
                case "clicks":
                    propertyName = "Clicks";
                    break;
                case "revenue":
                    propertyName = "Revenue";
                    break;
                default:
                    break;
            }

            return propertyName;
        }

        private string MatchPropertyNameWithOrderColumn(string propertyName)
        {
            string columnName = string.Empty;

            switch (propertyName.ToLower())
            {
                case "adrequests":
                    columnName = "requests";
                    break;
                case "adimpress":
                    columnName = "impressions";
                    break;
                case "clicks":
                    columnName = "clicks";
                    break;
                case "revenuetext":
                    columnName = "revenue";
                    break;
                case "fillratetext":
                    columnName = "requests";
                    break;
                case "ctrtext":
                    columnName = "requests";
                    break;
                case "ecpm":
                    columnName = "requests";
                    break;
                default:
                    columnName = propertyName;
                    break;
            }

            return columnName;
        }

        private IList<BaseChartDashboardView> ConvertToBaseChartDashboardView<T>(IList<T> result, string metricName)
            where T : BaseAppSiteResultDto
        {
            string propertyName = MatchOrderColumnWithPropertyName(metricName);

            var dataArray = result.Select(p => new BaseChartDashboardView() { Name = p.Name, MetricValue = Convert.ChangeType(typeof(BaseAppSiteResultDto).GetProperty(propertyName).GetValue(p), typeof(BaseAppSiteResultDto).GetProperty(propertyName).PropertyType) }).OrderByDescending(p => p.MetricValue).ToList();

            return dataArray;
        }

        private void FixOrderParameters(ref string orderColumn, ref string orderBy)
        {
            orderBy = string.IsNullOrEmpty(orderBy) ? null : orderBy.ToLower();

            if (string.IsNullOrEmpty(orderColumn) && (!string.IsNullOrEmpty(orderBy) && orderBy != "asc" && orderBy != "desc"))
            {
                if (orderBy.IndexOf("-") != -1)
                {
                    orderColumn = orderBy.Substring(0, orderBy.IndexOf("-"));

                    orderBy = orderBy.Substring(orderBy.IndexOf("-") + 1);
                }

            }
        }

        //private void AddMtricPercentage(IList<BaseChartDashboardView> result, long totalMetric)
        //{
        //    foreach (var item in result)
        //    {
        //        long metricValue = item.MetricValue;

        //        item.MetricPercentage = Math.Round((double)metricValue / totalMetric, 2) * 100;
        //    }
        //}

        private Dictionary<string, string> FillAppChartMetrics()
        {
            return (new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string,string>("Requests",ResourcesUtilities.GetResource("AdRequests", "AppChart")),
               new KeyValuePair<string,string>("Impressions",ResourcesUtilities.GetResource("AdImpress", "AppChart")),
               new KeyValuePair<string,string>("Clicks",ResourcesUtilities.GetResource("AdClicks", "AppChart")),
               new KeyValuePair<string,string>("Revenue",ResourcesUtilities.GetResource("Revenue", "AppChart"))
            }).ToDictionary(p => p.Key, p => p.Value);
        }

        #endregion
    }

}
