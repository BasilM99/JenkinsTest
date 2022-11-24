using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using Noqoush.Framework;
using Noqoush.Framework.Utilities;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc.Extensions;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using System.Globalization;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Telerik.Web.Mvc.UI;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.PMP;
using Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Exceptions;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    [PermissionsAuthorize(Permission = PortalPermissionsCode.PMPDeal, Roles = "Administrator,adops,AccountManager")]
    public class DealsController : AuthorizedControllerBase
    {
        private IAppSiteService _appsiteService;
        private IMetricService _MetricService;
        private IReportService _ReportService;
        private ICampaignService _CampaignsService;
        protected IAccountService _accountService;

        protected IPMPDealService _PMPDealService;
        protected IAdvertiserService _AdvertiserService;
        public DealsController(IAppSiteService appSiteService, IMetricService metricService, IReportService reportService, ICampaignService campaignService, IAccountService accountService, IPMPDealService PMPDealService, IAdvertiserService AdvertiserService)
        {
            _appsiteService = appSiteService;
            _MetricService = metricService;
            _ReportService = reportService;
            _CampaignsService = campaignService;
            _accountService = accountService;
            _PMPDealService = PMPDealService;
            _AdvertiserService = AdvertiserService;

        }


        #region Deals Index

        public ActionResult Index(int? id, bool AllowGlobalization = false)
        {
            ViewData["AllowGlobalization"] = AllowGlobalization;
            var filter = getDefualtFilter(null,id);
            string advertisrName = string.Empty;
            string advertisrAccountName = string.Empty;
            if (id.HasValue && id>0)
            {
                advertisrAccountName = _CampaignsService.GetAdvertiserAccountString(id.Value);

                ViewData["AdvertiserId"] = _CampaignsService.GetAdvertiserIdFromAccount(id.Value);
                advertisrName = _CampaignsService.GetAdvertiserString((int)ViewData["AdvertiserId"]);
                filter.ShowAdvertiser = true;
              
                ViewData["AdvertiserAccountId"] = id.Value;
            }
            Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal.PMPDealListViewModel lis = LoadPMPDealData(filter, AllowGlobalization);
            
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = AllowGlobalization && Config.IsAdministrationApp ? ResourcesUtilities.GetResource("GlobalDeals", "Deal") : ResourcesUtilities.GetResource("PMPDeals", "Titles"),
                Order = 1,
            });
            if (id.HasValue && id > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = advertisrAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers","campaign"),
                                           Order = -2,
                                           ExtensionDropDown=true
                                       });
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion





            return View(lis);
        }

        public ActionResult PMPSearch()
        {
            int CampId = Convert.ToInt32(RouteData.Values["id"]);
            int adveraccId = _CampaignsService.GetAdvertiserAccountIdByCampaignId(CampId);
            int adverId = _CampaignsService.GetAdvertiserIdByCampaignId(CampId);


            var filter = getDefualtFilter(adverId, adveraccId);
            PMPDealListViewModel model = LoadPMPDealData(filter);
            return PartialView(model);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _PMPSearch()
        {
            int CampId = Convert.ToInt32(RouteData.Values["id"]);
            int adveraccId = _CampaignsService.GetAdvertiserAccountIdByCampaignId(CampId);
            int adverId = _CampaignsService.GetAdvertiserIdByCampaignId(CampId);
            var filter = getDefualtFilter(adverId, adveraccId);
            var result = LoadPMPDealData(filter);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });


        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Index(int? id, int[] checkedRecords)
        {
            string advertisrName = string.Empty;

            string advertisrAccountName = string.Empty;
            if (id.HasValue && id > 0)
            {

              //  advertisrName = _CampaignsService.GetAdvertiserString(id.Value);
                advertisrAccountName = _CampaignsService.GetAdvertiserAccountString(id.Value);
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete 
                _PMPDealService.Delete(checkedRecords);
            }
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
            //    {
            //        //run  selected 
            //        _ReportService.RunSchadulingReport(checkedRecords);
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
            //        {
            //            //pause selected 
            //            _ReportService.PauseSchadulingReport(checkedRecords);
            //        }
            //        else
            //            if (!string.IsNullOrWhiteSpace(Request.Form["Send"]))
            //        {
            //            _ReportService.SendSchadulingReport(checkedRecords);
            //        }
            //    }
            //}

            if (id.HasValue && id>0)
            {

                return RedirectToAction("Index", new { id = id, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
            }
            else
            {
                return RedirectToAction("Index", new { AllowGlobalization = Request.QueryString["AllowGlobalization"] });
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult IndexPMPDeals(int? id, int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete 
                _PMPDealService.Delete(checkedRecords);
            }
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
            //    {
            //        //run  selected 
            //        _ReportService.RunSchadulingReport(checkedRecords);
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
            //        {
            //            //pause selected 
            //            _ReportService.PauseSchadulingReport(checkedRecords);
            //        }
            //        else
            //            if (!string.IsNullOrWhiteSpace(Request.Form["Send"]))
            //        {
            //            _ReportService.SendSchadulingReport(checkedRecords);
            //        }
            //    }
            //}


            if (id.HasValue && id > 0)
            {

                return RedirectToAction("Index", new { id = id, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
            }
            else
            {
                return RedirectToAction("Index", new {  AllowGlobalization = Request.QueryString["AllowGlobalization"] });
            }
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _IndexPMPDeals(int? id, bool AllowGlobalization = false)
        {
            var filter = getDefualtFilter(null,id);
            string advertisrName = string.Empty;

            string advertisrAccountName = string.Empty;
               
            if (id.HasValue && id > 0)
            {

                advertisrAccountName = _CampaignsService.GetAdvertiserAccountString(id.Value);
                filter.ShowAdvertiser = true;
            }

            var result = GetPMPDealQueryResult(filter, AllowGlobalization);
            return View("Index", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        protected Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter getDefualtFilter(int? AdvertiserId = null, int? AdvertiserAccountId = null)
        {
            Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter();
            filter.ExchangeId = string.IsNullOrWhiteSpace(Request.Form["ExchangeId"]) ? (int?)null : Convert.ToInt32(Request.Form["ExchangeId"]);
            filter.PublisherId = string.IsNullOrWhiteSpace(Request.Form["PublisherId"]) ? (int?)null : Convert.ToInt32(Request.Form["PublisherId"]);
            filter.PublisherName = string.IsNullOrWhiteSpace(Request.Form["PublisherName"]) ? string.Empty : Request.Form["PublisherName"];
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["DealName"]) ? string.Empty : Request.Form["DealName"];
            filter.AdFormat = string.IsNullOrWhiteSpace(Request.Form["AdFormat"]) ? null : Request.Form["AdFormat"].Split(',').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
            filter.IsGlobal = string.IsNullOrWhiteSpace(Request.Form["IsGlobal"]) ? false : Convert.ToBoolean(Request.Form["IsGlobal"]);
            filter.AdvertiserAccountId = AdvertiserAccountId.HasValue && AdvertiserAccountId > 0 ? AdvertiserAccountId.Value : (int?)null;
            filter.AdvertiserId = AdvertiserId.HasValue && AdvertiserId > 0? AdvertiserId.Value : (int?)null;
            filter.showArchived = string.IsNullOrWhiteSpace(Request.Form["showArchived"]) ? false : Convert.ToBoolean(Request.Form["showArchived"]);
            filter.Countries = string.IsNullOrWhiteSpace(Request.Form["Countries"]) ? null : Request.Form["Countries"].Split(',').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
            filter.AdSize = string.IsNullOrWhiteSpace(Request.Form["AdSize"]) ? null : Request.Form["AdSize"].Split(',').Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDateFilter"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDateFilter"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDatefilter"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDatefilter"], Config.ShortDateFormat, null);
            //filter.Accountid= string.IsNullOrWhiteSpace(Request.Form["StatusId"]) ? (int?)null : Convert.ToInt32(Request.Form["StatusId"]);
            filter.ExchangeFiltred = string.IsNullOrWhiteSpace(Request.Form["SSPCheckedIDs"]) ? null : Request.Form["SSPCheckedIDs"].Split(',').Select(x => Convert.ToInt32(x)).ToList();

            return filter;
        }
        protected PMPDealCriteria GetPMPDealCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter filter, bool AllowGlobalization = false)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new PMPDealCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                ExchangeId = filter.ExchangeId,
                PublisherId = filter.PublisherId,
                AdFormats = filter.AdFormat,
                Name = filter.Name,
                Archived = filter.showArchived,
                IsGlobal = filter.IsGlobal,
                AdvertiserId = filter.AdvertiserId,
                AdvertiserAccountId = filter.AdvertiserAccountId,
                Geographies = filter.Countries,
                AdSizes = filter.AdSize,
                ShowAdvertiser = filter.ShowAdvertiser,
                PublisherName = filter.PublisherName,
                ExchangeFiltred = filter.ExchangeFiltred

                //StatusId = filter.StatusId
            };

            if (AllowGlobalization && Config.IsAdminInAdminApp)
            {
                criteria.OnlyGlobal = true;

            }
            return criteria;
        }
        protected ResultPMPDealDto GetPMPDealQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter filter, bool AllowGlobalization = false)
        {
            var criteria = GetPMPDealCriteria(filter, AllowGlobalization);

            var result = _PMPDealService.QueryByCratiriaForPMPDeal(criteria);
            if (result.Items != null)
            {


                foreach (var item in result.Items)
                {

                    item.EndDateString = item.EndDate.HasValue ? item.EndDate.Value.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) : "";
                    item.StartDateString = item.StartDate.HasValue ? item.StartDate.Value.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat) : "";
                }
            }
            return result;
        }
        protected Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal.PMPDealListViewModel LoadPMPDealData(Noqoush.AdFalcon.Web.Controllers.Model.Deals.Filter filter, bool AllowGlobalization = false)
        {
            var result = GetPMPDealQueryResult(filter, AllowGlobalization);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions


           var actions = filter != null && filter.AdvertiserAccountId.HasValue ? GetPMPDealActions(filter.AdvertiserAccountId.Value, AllowGlobalization) : GetPMPDealActions(0, AllowGlobalization);
           var toolTips = filter != null && filter.AdvertiserAccountId.HasValue ? GetPMPDealTooltips(filter.AdvertiserAccountId.Value, AllowGlobalization) : GetPMPDealTooltips(0, AllowGlobalization);


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
            return new Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal.PMPDealListViewModel()
            {

                Items = items,
                TotalCount = (int)result.TotalCount,
                TopActions = actions,
                BelowAction = actions,
                ToolTips = toolTips,
            

                 PreventEdit = filter.AdvertiserAccountId.HasValue && filter.AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(filter.AdvertiserAccountId.Value) :false

            };
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetPMPDealTooltips(int AdvertiserAccountId, bool AllowGlobalization = false)
        {
            // Create the tool tip actions

            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Create",
                            ControllerName="Deals",
                           ExtraPrams = AdvertiserAccountId


                        }

               
                    //,
                        //    new Model.Action()
                        //{
                        //    Code = "0",
                        //    DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                        //    ClassName = "grid-tool-tip-copy",
                        //    ActionName = "RedirectToAuditTrial",

                        //}
                };

            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {
                toolTips = new List<Model.Action>();
                toolTips.Add(

                      new Model.Action()
                      {
                          Code = "0",
                          DisplayText = ResourcesUtilities.GetResource("View", "Commands"),
                          ClassName = "grid-tool-tip-edit",
                          ActionName = "Create",
                          ControllerName = "Deals",
                          ExtraPrams = AdvertiserAccountId


                      }

                    );

            }

            return toolTips;
        }


        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetPMPDealActions(int AdvertiserAccountId, bool AllowGlobalization = false)
        {

            // create the actions
            var actions = new List<Model.Action>
                {

                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Archive", "PMPDeal"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "PMPDeal"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Archive", "Confirmation") // like are u sure ?
                        },

                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            ControllerName="Deals",

                            DisplayText = AllowGlobalization && Config.IsAdministrationApp ? ResourcesUtilities.GetResource("AddNewGlobal", "Deal"):ResourcesUtilities.GetResource("AddNewPMPDeals", "Commands"),
                             ExtraPrams=AdvertiserAccountId,
                        ExtraPrams3 ="DealsAddNew",
                              ExtraPrams4 = AllowGlobalization,


                        }

                };

            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {
                actions = new List<Model.Action>();

            }
            return actions;
        }



        #endregion
    

        public virtual ActionResult Create(int AdvertiseraccId, int? id, bool AllowGlobalization = false)
        {
            int? PMPId = id;

            string advertisrName = string.Empty;

            string advertisrAccountName = string.Empty;
            int advId = 0;
            if (AdvertiseraccId > 0)
            {
                advertisrAccountName = _CampaignsService.GetAdvertiserAccountString(AdvertiseraccId);
                advId = _CampaignsService.GetAdvertiserIdFromAccount(AdvertiseraccId);

                advertisrName = _CampaignsService.GetAdvertiserString(advId);

                if (!_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
                {

                    throw new AccountNotValidException();

                }

            }
            ViewData["AllowGlobalization"] = AllowGlobalization;

            if (PMPId.HasValue)
            {
                PMPDealDto PMPDealDto = null;

                PMPDealDto = _PMPDealService.GetDealPMPDeal(PMPId.Value);
                PMPDealDto.AdvertiserAccountId = AdvertiseraccId > 0 ? AdvertiseraccId : 0;
                PMPDealDto.AdvertiserId = advId > 0 ? advId : 0;
                if (PMPDealDto.PMPTargetingSaveDto == null)

                {
                    PMPDealDto.PMPTargetingSaveDto = new PMPTargetingSaveDto();



                }
                ViewData["AllowGlobalization"] = PMPDealDto.IsGlobal;
                AllowGlobalization = PMPDealDto.IsGlobal;
                #region BreadCrumb


                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =PMPDealDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =AllowGlobalization && Config.IsAdministrationApp ? ResourcesUtilities.GetResource("GlobalDeals", "Deal") :ResourcesUtilities.GetResource("PMPDeals", "PMPDeal"),
                                                  Order = 1,
                                                  Url = AllowGlobalization && Config.IsAdministrationApp ? Url.Action("Index", new { id=AdvertiseraccId, AllowGlobalization = AllowGlobalization }):Url.Action("Index", new {id=AdvertiseraccId})
                                              }
                                      };

                if (AdvertiseraccId > 0)
                {
                    breadCrumbLinks.Add(
                                              new BreadCrumbModel()
                                              {
                                                  Text = advertisrAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = -1

                                              });

                    breadCrumbLinks.Add(
                                           new BreadCrumbModel()
                                           {
                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                               Url = Url.Action("AccountAdvertisers","campaign"),
                                               Order = -2,
                                               ExtensionDropDown =true
                                           });
                }
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                if (AdvertiseraccId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
                {
                    PMPDealDto.IsReadOnly = true;
       

                           AddMessages(ResourcesUtilities.GetResource("DealReadOnly", "Global"), MessagesType.Warning);
                }
                #endregion
                return View(PMPDealDto);
            }
            else
            {

                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("NewDeal","PMPDeal"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                 Text =AllowGlobalization && Config.IsAdministrationApp ? ResourcesUtilities.GetResource("GlobalDeals", "Deal") :ResourcesUtilities.GetResource("PMPDeals", "PMPDeal"),
                                                  Order = 1,
                                                  Url = AllowGlobalization && Config.IsAdministrationApp ? Url.Action("Index", new {AllowGlobalization = AllowGlobalization, id=AdvertiseraccId }):Url.Action("Index", new { id=AdvertiseraccId})
                                              }
                                      };



                if (AdvertiseraccId > 0)
                {
                    breadCrumbLinks.Add(
                                              new BreadCrumbModel()
                                              {
                                                  Text = advertisrAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = -1

                                              });

                    breadCrumbLinks.Add(
                                           new BreadCrumbModel()
                                           {
                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                               Url = Url.Action("AccountAdvertisers", "campaign"),
                                               Order = -2,
                                           });
                }
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                #endregion

                return View();
            }

        }
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Create(PMPDealDto PMPDeal, int AdvertiseraccId, int? id, string returnUrl)
        {
            string advertisrName = string.Empty;

            string advertisrAccountName = string.Empty;
            int advId = 0;
            if (AdvertiseraccId > 0)
            {
                advertisrAccountName = _CampaignsService.GetAdvertiserAccountString(AdvertiseraccId);
                advId = _CampaignsService.GetAdvertiserIdFromAccount(AdvertiseraccId);

                advertisrName = _CampaignsService.GetAdvertiserString(advId);

            }
            #region BreadCrumb
            //TODO:osaleh to use the old name not the new name in breadcrumb if an exception is been thrown 
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =PMPDeal.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 2
                                              }
                                      };


            if (id > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = advertisrName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers", "campaign"),
                                           Order = -2,
                                       });
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            int? PMPDealId = id;
            if (ModelState.IsValid)
            {
                if (PMPDealId.HasValue)
                {
                    //this is update
                    PMPDeal.ID = PMPDealId.Value;
                    PMPDeal.AdvertiserAccountId = AdvertiseraccId > 0 ? AdvertiseraccId : 0;

                    PMPDeal.AdvertiserId = advId > 0 ? advId : 0;
                    try
                    {

                        var result = _PMPDealService.SavePMPDeal(PMPDeal);
                        if (result.Warnings != null)
                        {
                            foreach (var warning in result.Warnings)
                            {
                                AddMessages(warning.Message, MessagesType.Warning);
                            }
                        }
                        AddSuccessfullyMsg();
                        MoveMessagesTempData();
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Create", new { id = result.ID, AdvertiseraccId = AdvertiseraccId, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { id = result.ID, AdvertiseraccId = AdvertiseraccId, returnUrl = returnUrl, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
                        }
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                    }
                    return View(PMPDeal);
                }
                else
                {
                    PMPDeal.AdvertiserAccountId = AdvertiseraccId > 0 ? AdvertiseraccId : 0;
                    PMPDeal.AdvertiserId = advId > 0 ? advId : 0;
                    int newId = 0;
                    PMPDeal.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                    PMPDeal.UserId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

                    PMPDeal.CreationDate = Framework.Utilities.Environment.GetServerTime();
                    try
                    {
                        var result = _PMPDealService.SavePMPDeal(PMPDeal);
                        newId = result.ID;
                        if (result.Warnings != null)
                        {
                            foreach (var warning in result.Warnings)
                            {
                                AddMessages(warning.Message, MessagesType.Warning);
                            }
                        }
                        AddSuccessfullyMsg();
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                        return View(PMPDeal);
                    }
                    MoveMessagesTempData();

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Create", new { id = newId, AdvertiseraccId = AdvertiseraccId, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
                    }
                    else
                    {
                        return RedirectToAction("Create", new { id = newId, AdvertiseraccId = AdvertiseraccId, returnUrl = returnUrl, AllowGlobalization = Request.QueryString["AllowGlobalization"] });
                    }

                }

            }
            else
            {
                return View(PMPDeal);
            }

        }

        public virtual ActionResult GetAdFormatsTree(string types)
        {
            string[] typesList = null;
            List<int> typesListToSend = null;
            IList<TreeDto> tree = new List<TreeDto>();

            if (!string.IsNullOrEmpty(types))
                typesList = types.Split(',');

            if (typesList != null && typesList.Count() > 0)
                typesListToSend = typesList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();

            if (typesListToSend != null && typesListToSend.Count > 0)
            {
                try
                {
                    tree = _PMPDealService.GetAdFormatsTree(typesListToSend);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            return Json(new { tree }, JsonRequestBehavior.AllowGet);

        }

        public virtual ActionResult getCampsBydeal(int dealid, int? AdvertiserAccountId)
        {
            var results=_PMPDealService.getCampsAdvertiserBydeal(dealid, AdvertiserAccountId.HasValue ? AdvertiserAccountId.Value : 0);


            List<CampaignDto> list = results.CampaignItems.ToList();
            List<AdvertiserAccountDto> AdvertiserAccountItems = results.AdvertiserAccountItems.ToList();
            return Json(new { results }, JsonRequestBehavior.AllowGet);

        }
        public virtual ActionResult getAdvertiserAccountsBydeal(int dealid)
        {
            List<AdvertiserAccountDto> list = _PMPDealService.getAdvertiserAccountsBydeal(dealid).ToList();

            return Json(new { list }, JsonRequestBehavior.AllowGet);

        }
        
        public virtual ActionResult getDealCampsAdgruops(int dealId, int campId)
        {
            List<AdGroupDto> list = _PMPDealService.getDealCampsAdgruops(dealId, campId).ToList();

            return Json(new { list }, JsonRequestBehavior.AllowGet);

        }
        public virtual ActionResult Tree()
        {


            return View();

        }


    }
}
