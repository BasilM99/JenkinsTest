using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.SSP;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Telerik.Web.Mvc;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class PartnerController : AuthorizedControllerBase
    {
        protected ISupplyService _demandSupplyService;
        protected ILookupService _lookupService;
        private IAppSiteTypeService _appSiteTypeService;
        private IPartyService _partyService;
        protected IAccountService _accountService;

        private IAppSiteService _appSiteService;
        private IAppSiteStatusService _appSiteStatusService;
        public PartnerController(IAccountService accountService,
ISupplyService DemandSupplyService, ILookupService lookupService, IAppSiteService appSiteService, IAppSiteStatusService appSiteStatusService, IAppSiteTypeService appSiteTypeService, IPartyService partyService)
        {
            _demandSupplyService = DemandSupplyService;
            _lookupService = lookupService;

            _accountService = accountService;

            _appSiteService = appSiteService;
            _appSiteStatusService = appSiteStatusService;
            _appSiteTypeService = appSiteTypeService;
            _partyService = partyService;
        }

        #region Partner

        #region index

        #region actions
        public virtual ActionResult Index()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(LoadData(null));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Index(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteBusinessPartner(checkedRecords);
            }

            return RedirectToAction("index");
        }


        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _Index()
        {
            var result = GetQueryResult(null);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }




        #endregion

        #region helpers

        protected Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter getDefualtFilter()
        {
            Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"];
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            // filter.typeId = 4;
            return filter;
        }
        protected PartnerCriteria GetCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new PartnerCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                PartnerName = string.IsNullOrEmpty(filter.name) ? null : filter.name,
                //  TypeId = filter.typeId
                //AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
            };
            return criteria;
        }
        protected virtual ResultPartnerDto GetQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            var criteria = GetCriteria(filter);
            var result = _demandSupplyService.QueryByCratiriaForPartner(criteria);
            return result;
        }
        protected virtual ListViewModel LoadData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            var result = GetQueryResult(filter);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            // create the actions
            var actions = GetPartnerAction();
            var tooltips = GetPartnerToolTips();
            return new ListViewModel()
            {
                Items = items,
                TopActions = actions,
                ToolTips = tooltips
            };
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetPartnerAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Archive", "PMPDeal"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPPartners"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Archive", "Confirmation") // like are u sure ?

                           
                        }
                };
            #endregion

            return actions;
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetPartnerToolTips()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new   Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Deals", "SSPDealCampaign"),
                            ClassName = "grid-tool-tip-reports",
                            ActionName = "DealCampaignMappings"
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }
                };
            #endregion

            return actions;
        }


        public virtual ActionResult RedirectToAuditTrial(int id)
        {
            string originalPath = new Uri(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri).OriginalString;

            try
            {
                int objectRootTypeId = _accountService.GetObjectRootTypeId("Noqoush.AdFalcon.Domain.Common.Model.Account.Account");
                //List<BreadCrumbModel> TraveledBreadCrumbLinks = new List<BreadCrumbModel>();
                //switch (id)
                //{
                //    case 0:
                //        TraveledBreadCrumbLinks = new List<BreadCrumbModel>
                //                      {
                //                          new BreadCrumbModel()
                //                              {
                //                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                //                                  Order = 1,
                //                              }
                //                      };

                //        break;
                //    default:
                //        break;
                //}

                return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId, objectRootTypeId = objectRootTypeId , returnUrl = originalPath });
            }
            catch (Exception e)
            {

                throw e;
            }


        }
        #endregion

        #endregion

        #endregion


        #region site

        #region index

        #region actions
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult SitesSaveForm(int? Id)
        {
            var model = new SitesListViewModel();
            int item_id = Id.HasValue ? Id.Value : 0;
            model.SaveDto = item_id < 1 ? null : _demandSupplyService.GetSitePartner(item_id);

            if (item_id < 1)
            {
                model.BusinessId = int.Parse(Request.QueryString["BusinessId"]);
                //model.SiteId = int.Parse(Request.QueryString["SiteId"]);
            }
            model.DialogTitle = item_id < 1 ? ResourcesUtilities.GetResource("AddDialogTitle", "SSPSites") : ResourcesUtilities.GetResource("UpdateDialogTitle", "SSPSites");
            model.DialogWidth = 550;
            model.DialogHeight = 315;
            return PartialView("SiteSave", model);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SitesSave(int? Id, SitesListViewModel site)
        {

            {
                try
                {

                    _demandSupplyService.SaveSitePartner(site.SaveDto);

                }
                catch (BusinessException exception)
                {

                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true });
        }
        public ActionResult Sites(int Id)
        {
            SitesListViewModel Sites = LoadSiteData(null, Id);
            Sites.SaveUrl = Url.Action("SitesSave");
            Sites.GetUrl = Url.Action("SitesSaveForm") + "{0}?BusinessId=" + Sites.BusinessId;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+Sites.BusinessName*/,
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                              ,new BreadCrumbModel()
                                              {
                                                  Text =Sites.BusinessName,
                                                  Url=" ",
                                                  Order = 2,
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View("IndexSite", Sites);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Sites(int Id)
        {
            var result = GetSitesQueryResult(null, Id);
            return View("IndexSite", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Sites(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteSitePartner(checkedRecords);
            }

            return RedirectToAction("Sites");
        }

        #endregion
        #region helpers


        protected PartnerSiteCriteria GetSitesCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new PartnerSiteCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                SiteName = filter.name
            };
            return criteria;
        }
        protected ResultPartnerSiteDto GetSitesQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int PartnerId)
        {
            var criteria = GetSitesCriteria(filter);
            criteria.PartnerId = PartnerId;
            var result = _demandSupplyService.QueryByCratiriaForSitePartner(criteria);
            return result;
        }

        protected SitesListViewModel LoadSiteData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int PartnerId)
        {
            var result = GetSitesQueryResult(filter, PartnerId);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions
            var actions = GetSiteAction();

            #endregion
            var tooltip = GetSiteTooltip();

            return new SitesListViewModel()
            {
                BusinessId = result.BusinessId,
                Items = items,
                BusinessName = result.BusinessName,
                TopActions = actions,
                BelowAction = null,
                ToolTips = tooltip
            };
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetSiteAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPSites"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewSite", "SSPCommands")
                            ,   ExtraPrams3="Partner"
                        }
                };
            #endregion

            return actions;
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetSiteTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Create",
                            ExtraPrams3="Partner"
                        },
                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }
                };
        }
        #endregion

        #endregion


        #endregion


        #region siteZone

        #region Index

        #region Helpers
        protected SiteZoneCriteria GetSiteZoneCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new SiteZoneCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                ZoneName = filter.name
            };
            return criteria;
        }
        protected ResultSiteZoneDto GetSiteZoneQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int BusinessId, int SiteId)
        {
            var criteria = GetSiteZoneCriteria(filter);
            criteria.BusinessId = BusinessId;
            criteria.SiteId = SiteId;
            var result = _demandSupplyService.QueryByCratiriaForSiteZone(criteria);
            return result;
        }
        protected SitesZoneListViewModel LoadSiteZoneData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int BusinessId, int SiteId)
        {
            var result = GetSiteZoneQueryResult(filter, BusinessId, SiteId);
            ViewData["total"] = result.TotalCount;
            var actions = GetSiteZoneAction();
            return new SitesZoneListViewModel()
            {
                Items = result.Items,
                BusinessName = result.BusinessName,
                BusinessId = result.BusinessId,
                SiteName = result.SiteName,
                SiteIdStr = result.SiteIdStr,
                SiteId = result.SiteId,
                TopActions = actions,

                ToolTips = GetSiteZoneTooltip(result.SiteId)


            };
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetSiteZoneAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPSiteZones"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewSiteZone", "SSPCommands"),
                             ExtraPrams3="Partner"
                        }
                };
            #endregion

            return actions;
        }

        #endregion

        #region Actions

        public ActionResult SitesZonesSaveForm(int? Id)
        {
            var model = new SitesZoneListViewModel();
            int item_id = Id.HasValue ? Id.Value : 0;
            model.SaveDto = item_id < 1 ? null : _demandSupplyService.GetSiteZone(item_id);

            if (item_id < 1)
            {
                model.BusinessId = int.Parse(Request.QueryString["BusinessId"]);
                model.SiteId = int.Parse(Request.QueryString["SiteId"]);
                model.SiteName = _demandSupplyService.GetSitePartner(model.SiteId).SiteName;
            }
            else
            {

                model.SiteName = _demandSupplyService.GetSitePartner(model.SaveDto.SiteID).SiteName;

            }
            model.DialogTitle = item_id < 1 ? ResourcesUtilities.GetResource("AddDialogTitle", "SSPSiteZones") : ResourcesUtilities.GetResource("UpdateDialogTitle", "SSPSiteZones");
            model.DialogWidth = 550;
            model.DialogHeight = 315;
            return PartialView("SiteZoneSave", model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SitesZonesSave(int? Id, SitesZoneListViewModel Zone)
        {

            {
                try
                {

                    _demandSupplyService.SaveSiteZone(Zone.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true });
        }

        public virtual ActionResult SiteZones(int SiteId, int Id)
        {
            var siteZoneData = LoadSiteZoneData(null, Id, SiteId);

            #region breadCrumbLinks


            siteZoneData.SaveUrl = Url.Action("SitesZonesSave");

            siteZoneData.GetUrl = Url.Action("SitesZonesSaveForm") + "{0}?BusinessId=" + siteZoneData.BusinessId + "&SiteID=" + siteZoneData.SiteId;


            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+siteZoneData.BusinessName*/,
                                                Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                               ,new BreadCrumbModel()
                                              {
                                                  Text =siteZoneData.BusinessName,
                                                  Url=" ",
                                                  Order = 2,
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Menu", "SSPSites")/*+": "+siteZoneData.SiteName*/,
                                                         Url=Url.Action("Sites", new {Id =siteZoneData.BusinessId }),
                                                  Order = 3,
                                              },
                                               new BreadCrumbModel()
                                              {
                                                  Text =  siteZoneData.SiteName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                     Url=""
                                              },

                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View("IndexSiteZone", siteZoneData);

        }

        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _SiteZones(int SiteId, int Id)
        {


            var result = GetSiteZoneQueryResult(null, Id, SiteId);

            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult SiteZones(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteSiteZone(checkedRecords);
            }

            return RedirectToAction("SiteZones");
        }



        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetSiteZoneTooltip(int SiteId)
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Create",
                            ExtraPrams3="Partner"
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Prices", "SSPFloorPrices"),
                            ClassName = "grid-tool-tip-reports",
                            ActionName = "FloorPrices/"+SiteId
                        },
                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }


                };
        }
        #endregion

        #endregion

        #endregion



        #region siteZoneMappings

        #region Helpers

        protected SiteZoneMappingCriteria GetSiteMappingsCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new SiteZoneMappingCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                MappingName = filter.name
            };

            return criteria;
        }
        protected ResultSiteZoneMapping GetSiteMappingsQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int Id, int SiteId, int ZoneId)
        {
            var criteria = GetSiteMappingsCriteria(filter);
            criteria.ZoneId = ZoneId;
            criteria.SiteId = SiteId;
            criteria.BusinessId = Id;
            criteria.AppSiteName = criteria.MappingName;
            var result = _demandSupplyService.QueryByCratiriaForSiteZoneMapping(criteria);
            return result;
        }
        protected SiteZoneMappingsListViewModel LoadSiteMappingsData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int Id, int SiteId, int ZoneId)
        {
            var result = GetSiteMappingsQueryResult(filter, Id, SiteId, ZoneId);
            ViewData["total"] = result.TotalCount;
            var action = GetSiteMappingsAction();
            var toolTip = GetMappingTooltip();
            return new SiteZoneMappingsListViewModel()
            {
                Items = result.Items,
                ZoneName = result.ZoneName,
                ZoneId = result.ZoneId,
                SiteName = result.SiteName,
                ZoneIdStr = result.ZoneIdStr,
                SiteIdStr = result.SiteIdStr,
                SiteId = result.SiteId,
                BusinessName = result.BusinessName,
                BusinessId = result.BusinessId,
                TopActions = action,
                ToolTips = toolTip
            };
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetMappingTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Create",
                            ExtraPrams3="Partner"
                        },
                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }
                };
        }

        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetSiteMappingsAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPSiteZoneMappings"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewMapping", "SSPCommands"),
                            ExtraPrams3="Partner"
                        }
                };
            #endregion

            return actions;
        }

        #endregion

        #region Actions
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult SiteZoneMappingSaveForm(int? Id)
        {
            var model = new SiteZoneMappingsListViewModel();

            int item_id = Id.HasValue ? Id.Value : 0;
            model.SaveDto = item_id < 1 ? null : _demandSupplyService.GetSiteZoneMapping(item_id);
            //model.SaveDto = null ;
            model.AdTypes = GetList(LookupNames.AdType, null);
            var selectedOne = model.AdTypes.Where(M => M.Value == "").Single();
            selectedOne.Text = ResourcesUtilities.GetResource("All", "Global");

            model.NativeAdLayouts = GetList(LookupNames.NativeAdLayout, null);
            model.DeviceTypes = GetList(LookupNames.DeviceType, null);
            selectedOne = model.DeviceTypes.Where(M => M.Value == "" + (int)Noqoush.AdFalcon.Domain.Common.Model.Core.DeviceTypeEnum.Any).Single();
            selectedOne.Selected = true;

            model.Interstitials = new List<SelectListItem>();
            SelectListItem yes = new SelectListItem();
            yes.Text = ResourcesUtilities.GetResource("Yes", "Global");
            yes.Value = "true";
            model.Interstitials.Add(yes);
            SelectListItem No = new SelectListItem();
            No.Text = ResourcesUtilities.GetResource("No", "Global");
            No.Value = "false";
            model.Interstitials.Add(No);
            SelectListItem allNo = new SelectListItem();
            allNo.Text = ResourcesUtilities.GetResource("Any", "Global");
            allNo.Selected=true;
            allNo.Value = "";
            model.Interstitials.Add(allNo);
            if (item_id < 1)
            {
                model.ZoneId = int.Parse(Request.QueryString["ZoneId"]);
                model.SiteId = int.Parse(Request.QueryString["SiteId"]);

            }
            model.BusinessName = Request.QueryString["BusinessName"];
            model.SiteName = Request.QueryString["SiteName"];
            model.ZoneName = Request.QueryString["ZoneName"];

            model.DialogTitle = item_id < 1 ? ResourcesUtilities.GetResource("AddDialogTitle", "SSPSiteZoneMappings") : ResourcesUtilities.GetResource("UpdateDialogTitle", "SSPSiteZoneMappings");
            model.DialogWidth = 1100;
            if (item_id < 1)
            {
                model.DialogHeight = 750;
            }
            else
            {
                model.DialogHeight = 400;
            }

            var statues = _appSiteStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();
            //   var campaign= _campaignService.Get(id)
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            //load all types
            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();



            model.AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() };
            model.Statuses = statuesDropDown;
            model.Types = typesDropDown;
            model.DateTo = serverDateTime;
            model.DateFrom = serverDateTime.AddDays(-30);
            model.SubPublishers = new List<SelectListItem>();


            return PartialView("SiteZoneMappingSave", model);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SiteZoneMappingSave(int? Id, string[] appSiteId,
                            string[] deletedAppSiteId, SiteZoneMappingsListViewModel site)
        {

            {
                try
                {
                    var ser = new JavaScriptSerializer();
                    if (appSiteId == null)
                        appSiteId = new string[0];
                    if (deletedAppSiteId == null)
                        deletedAppSiteId = new string[0];

                    var ids = appSiteId.Select(s => Convert.ToInt32(s)).ToArray();
                    var deletedIds = deletedAppSiteId.Select(s => Convert.ToInt32(s)).ToArray();

                    var adsToCopyAppsitesList = new List<int>();
                    var InsertedItems = ser.Deserialize<IList<AssignedAppsitesDto>>(string.IsNullOrWhiteSpace(site.InsertedAssignedAppsites) ? "[]" : site.InsertedAssignedAppsites);


                    //adsToCopyAppsitesList = adsToCopyAppsites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
                    _demandSupplyService.SaveSiteZoneMapping(site.SaveDto, InsertedItems);
                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true });
        }
        public virtual ActionResult siteZoneMappings(int Id, int SiteId, int ZoneId)
        {
            var data = LoadSiteMappingsData(null, Id, SiteId, ZoneId);

            data.SaveUrl = Url.Action("SiteZoneMappingSave");
            data.GetUrl = Url.Action("SiteZoneMappingSaveForm") + "{0}?ZoneId=" + ZoneId + @"&" + "SiteId=" + SiteId + @"&" + "BusinessName=" + data.BusinessName + @"&" + "SiteName=" + data.SiteName + @"&" + "ZoneName=" + data.ZoneName;

            #region BreadCrumb



            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+data.BusinessName*/,
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              },
                                                    new BreadCrumbModel()
                                              {
                                                  Text =data.BusinessName,
                                                  Url=" ",
                                                  Order = 2,
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Menu", "SSPSites")/*+": "+data.SiteName*/,
                                                         Url=Url.Action("Sites", new {Id =data.BusinessId }),

                                                  Order = 3,
                                              },
                                                   new BreadCrumbModel()
                                              {
                                                  Text =  data.SiteName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                     Url=""
                                              },
                                        new BreadCrumbModel()
                                                {
                                                    Url=Url.Action("SiteZones", new {Id =data.BusinessId , SiteId = data.SiteId}),
                                                    Text =ResourcesUtilities.GetResource("Menu", "SSPSiteZones")/*+": "+data.ZoneName*/,
                                                    Order = 5,
                                                }
                                                ,
                                        new BreadCrumbModel()
                                                {
                                                    Url="",
                                                    Text =data.ZoneName,
                                                    Order = 6,
                                                }

                                      };


            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View("IndexSiteZoneMapping", data);
        }


        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _siteZoneMappings(int Id, int SiteId, int ZoneId)
        {

            var result = GetSiteMappingsQueryResult(null, Id, SiteId, ZoneId);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult siteZoneMappings(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteSiteZoneMapping(checkedRecords);
            }

            return RedirectToAction("siteZoneMappings");
        }
        #endregion

        #endregion







        #region FloorPrice
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult FloorPriceSaveForm(int? Id)
        {
            var model = new FloorPriceListViewModel();
            int item_id = Id.HasValue ? Id.Value : 0;
            model.SaveDto = item_id < 1 ? null : _demandSupplyService.GetFloorPrice(item_id);
            model.FloorPriceConfigTypes = GetList(LookupNames.BidConfigType, model.SaveDto == null ? (int?)0 : (int)(model.SaveDto.ConfigType));

            model.FloorPriceConfigTypes = model.FloorPriceConfigTypes.Where(M => M.Value != "0").ToList();
            if (item_id < 1)
            {

                model.ZoneId = int.Parse(Request.QueryString["ZoneId"]);
                model.SiteId = int.Parse(Request.QueryString["SiteId"]);

            }
            else
            {
                if (model.SaveDto.ConfigType != FloorPriceConfigType.Base)
                {

                    var lookupdto = _lookupService.GetLookup(new LookupGetCriteria { Id = model.SaveDto.TargetingId, LookType = model.SaveDto.ConfigType.ToString() });
                    model.SaveDto.TargetingName = lookupdto.Name.Value;
                }

            }
            model.DialogTitle = item_id < 1 ? ResourcesUtilities.GetResource("AddDialogTitle", "SSPFloorPrices") : ResourcesUtilities.GetResource("UpdateDialogTitle", "SSPFloorPrices");
            model.DialogWidth = 550;
            model.DialogHeight = 315;
            return PartialView("FloorPriceSave", model);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult FloorPriceBaseSave(FloorPriceListViewModel floorPrice)
        {
            int basePriceId = 0;
            {
                try


                {

                    FloorPriceConfigDto SaveDto = new FloorPriceConfigDto();
                    SaveDto.ID = floorPrice.BaseId;
                    SaveDto.Price = floorPrice.Price;
                    SaveDto.ConfigType = FloorPriceConfigType.Base;
                    SaveDto.ConfigTypeId = (int)FloorPriceConfigType.Base;

                    SaveDto.ZoneID = floorPrice.ZoneId;
                    SaveDto.SiteID = floorPrice.SiteId;
                    _demandSupplyService.SaveFloorPrice(SaveDto);
                    var dtoPrice = _demandSupplyService.GetBaseFloorPrice(floorPrice.SiteId, floorPrice.ZoneId);
                    if (dtoPrice != null && dtoPrice.ID > 0)
                    {

                        basePriceId = dtoPrice.ID;
                    }

                    //FloorPriceListViewModel floorPrices = LoadFloorPriceData(null, floorPrice.ZoneId, floorPrice.SiteId);



                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true, basePriceId = basePriceId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult FloorPriceSave(int? Id, FloorPriceListViewModel floorPrice)
        {
            int basePriceId = 0;
            {
                try
                {
                    if (floorPrice.SaveDto.ConfigTypeId > 0)
                    {
                        floorPrice.SaveDto.ConfigType = (FloorPriceConfigType)floorPrice.SaveDto.ConfigTypeId;// Enum.Parse(typeof(),floorPrice.SaveDto.ConfigTypeId);



                    }


                    _demandSupplyService.SaveFloorPrice(floorPrice.SaveDto);
                    var dtoPrice = _demandSupplyService.GetBaseFloorPrice(floorPrice.SaveDto.SiteID, floorPrice.SaveDto.ZoneID);
                    if (dtoPrice != null && dtoPrice.ID > 0)
                    {

                        basePriceId = dtoPrice.ID;
                    }
                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true, basePriceId = basePriceId });
        }
        public ActionResult FloorPrices(int SiteId, int Id)
        {
            FloorPriceListViewModel floorPrices = LoadFloorPriceData(null, Id, SiteId);

            var dtoPrice = _demandSupplyService.GetBaseFloorPrice(SiteId, Id);
            if (dtoPrice != null && dtoPrice.ID > 0)
            {
                floorPrices.Price = dtoPrice.Price;
                floorPrices.BaseId = dtoPrice.ID;
            }
            floorPrices.Types = GetList(LookupNames.BidConfigType, 0);
            foreach (FloorPriceConfigDto item in floorPrices.Items)
            {
                if (item.ConfigType != FloorPriceConfigType.Base)
                {
                    item.TargetingName = _lookupService.GetLookup(new LookupGetCriteria
                    {
                        Id = item.TargetingId,
                        LookType = item.ConfigType.ToString()
                    }).Name;


                }
                else
                {
                    item.TargetingName = item.ConfigType.ToString();
                }
            }


            floorPrices.SaveUrl = Url.Action("FloorPriceSave");

            floorPrices.GetUrl = Url.Action("FloorPriceSaveForm") + "{0}?ZoneId=" + Id + @"&" + "SiteId=" + SiteId;


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+data.BusinessName*/,
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              },
                                                      new BreadCrumbModel()
                                              {
                                                  Text =floorPrices.BusinessName,
                                                  Url=" ",
                                                  Order = 2,
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Menu", "SSPSites")/*+": "+data.SiteName*/,
                                                         Url=Url.Action("Sites", new {Id =floorPrices.BusinessId }),

                                                  Order = 3,
                                              },
                                                   new BreadCrumbModel()
                                              {
                                                  Text =  floorPrices.SiteName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                     Url=""
                                              },
                                        new BreadCrumbModel()
                                                {
                                                    Url=Url.Action("SiteZones", new {Id =floorPrices.BusinessId , SiteId = floorPrices.SiteId}),
                                                    Text =ResourcesUtilities.GetResource("Menu", "SSPSiteZones")/*+": "+data.ZoneName*/,
                                                    Order = 5,
                                                }
                                                ,
                                        new BreadCrumbModel()
                                                {
                                                    Url="",
                                                    Text =floorPrices.ZoneName,
                                                    Order = 6,
                                                }

                                      };


            // var breadCrumbLinks = new List<BreadCrumbModel>
            //                         {

            //                          new BreadCrumbModel()
            //                                 {
            //                                     Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+floorPrices.BusinessName*/,
            //                                      Url=Url.Action("Index"),
            //                                     Order = 1,
            //                                 },
            //                          new BreadCrumbModel()
            //                                 {
            //                                     Text =ResourcesUtilities.GetResource("Menu", "SSPSites")/*+": "+floorPrices.SiteName*/,
            //                                        Url=Url.Action("Sites", new {Id =floorPrices.BusinessId }),


            //                                     Order = 2,
            //                                 },
            //                           new BreadCrumbModel()
            //                                   {
            //                                       Text =ResourcesUtilities.GetResource("Menu", "SSPSiteZones")/*+": "+floorPrices.ZoneName*/,
            //                                             Url=Url.Action("SiteZones", new {Id =floorPrices.BusinessId , SiteId = floorPrices.SiteId}),
            //                                       Order = 3,
            //                                   }

            //                         };



            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View("IndexFloorPrice", floorPrices);
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _FloorPrices(int SiteId, int Id)
        {
            var result = GetFloorPriceQueryResult(null, Id, SiteId);

            foreach (FloorPriceConfigDto item in result.Items)
            {
                if (item.ConfigType != FloorPriceConfigType.Base)
                {
                    item.TargetingName = _lookupService.GetLookup(new LookupGetCriteria
                    {
                        Id = item.TargetingId,
                        LookType = item.ConfigType.ToString()
                    }).Name;
                }
                else
                {
                    item.TargetingName = item.ConfigType.ToString();
                }

            }
            return View("IndexFloorPrice", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult FloorPrices(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteFloorPrice(checkedRecords);
            }

            return RedirectToAction("FloorPrices");
        }

        protected FloorPriceCriteria GetFloorPriceCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();

            FloorPriceConfigType val = FloorPriceConfigType.Undefined;
            if (!string.IsNullOrWhiteSpace(Request.Form["FloorTypeId"]))
            {

                Enum.TryParse<FloorPriceConfigType>(Request.Form["FloorTypeId"], out val);
            }
            var criteria = new FloorPriceCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.name,
                ConfigType = val

                //StatusId = filter.StatusId

            };

            return criteria;
        }
        protected ResultFloorPriceConfigDto GetFloorPriceQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int ZoneId, int SiteId)
        {
            var criteria = GetFloorPriceCriteria(filter);
            criteria.ZoneId = ZoneId;
            criteria.SiteId = SiteId;
            var result = _demandSupplyService.QueryByCratiriaForFloorPrice(criteria);
            return result;
        }

        protected FloorPriceListViewModel LoadFloorPriceData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int ZoneId, int SiteId)
        {
            var result = GetFloorPriceQueryResult(filter, ZoneId, SiteId);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions
            var action = GetFloorPriceAction();
            #endregion
            var toolTip = GetFloorPriceTooltip();

            return new FloorPriceListViewModel()
            {

                Items = items,
                SiteId = SiteId,
                ZoneId = ZoneId,
                ZoneIdStr = result.ZoneIdStr,
                SiteIdStr = result.SiteIdStr,
                BusinessId = result.BusinessId,
                ZoneName = result.ZoneName,
                BusinessName = result.BusinessName,
                SiteName = result.SiteName,
                TopActions = action,
                BelowAction = null,
                ToolTips = toolTip
            };
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetFloorPriceAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPFloorPrices"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNew", "SSPFloorPrices"),
                                ExtraPrams3="Partner"
                        }
                };
            #endregion

            return actions;
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetFloorPriceTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "FloorPriceSaveForm",
                              ExtraPrams3="Partner"
                        },
                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }


                };
        }
        #endregion




        #region campaingdealmappiong
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult DealCampaignMappingSaveForm(int? Id)
        {
            var model = new DealCampaignMappingListViewModel();
            int item_id = Id.HasValue ? Id.Value : 0;
            model.SaveDto = item_id < 1 ? null : _demandSupplyService.GetDealCampaignMapping(item_id);
            // model.FloorPriceConfigTypes = GetList(LookupNames.BidConfigType, model.SaveDto == null ? (int?)0 : (int)(model.SaveDto.ConfigType));
            if (item_id < 1)
            {

                //model.ZoneId = int.Parse(Request.QueryString["ZoneId"]);
                model.BusinessId = int.Parse(Request.QueryString["BusinessId"]);

            }

            model.AccountId = int.Parse(Request.QueryString["AccountId"]);
            model.DialogTitle = item_id < 1 ? ResourcesUtilities.GetResource("AddDialogTitle", "SSPDealCampaign") : ResourcesUtilities.GetResource("UpdateDialogTitle", "SSPDealCampaign");
            model.DialogWidth = 550;
            model.DialogHeight = 235;
            return PartialView("DealCampaignMappingSave", model);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult DealCampaignMappingSave(int? Id, DealCampaignMappingListViewModel floorPrice)
        {

            {
                try
                {

                    _demandSupplyService.SaveDealCampaignMapping(floorPrice.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, ErrorMessage = errors });
                }
            }

            return Json(new { Success = true });
        }
        public ActionResult DealCampaignMappings(int Id)
        {
            DealCampaignMappingListViewModel floorPrices = LoadDealCampaignMappingData(null, Id);
            var bizDto = _partyService.GetBusinessPartner(Id);
            floorPrices.SaveUrl = Url.Action("DealCampaignMappingSave");
            floorPrices.GetUrl = Url.Action("DealCampaignMappingSaveForm") + "{0}?BusinessId=" + Id + "&AccountId=" + bizDto.AccountId;


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu")/*+": "+floorPrices.BusinessName*/,
                                                   Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                     ,
                                                      new BreadCrumbModel()
                                              {
                                                  Text =floorPrices.BusinessName,
                                                  Url=" ",
                                                  Order = 2,
                                              }

                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View("IndexDealCampaignMapping", floorPrices);
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _DealCampaignMappings(int Id)
        {
            var result = GetDealCampaignMappinQueryResult(null, Id);
            return View("IndexDealCampaignMapping", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult DealCampaignMappings(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _demandSupplyService.DeleteDealCampaignMapping(checkedRecords);
            }

            return RedirectToAction("DealCampaignMappings");
        }

        protected DealCampaignMappingCriteria GetDealCampaignMappinCriteria(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new DealCampaignMappingCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                CampaignName = filter.name

                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected ResultDealCampaignMappingDto GetDealCampaignMappinQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int Id)
        {
            var criteria = GetDealCampaignMappinCriteria(filter);
            criteria.PartnerId = Id;

            var result = _demandSupplyService.QueryByCratiriaForDealCampaignMapping(criteria);
            return result;
        }

        protected DealCampaignMappingListViewModel LoadDealCampaignMappingData(Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter, int Id)
        {
            var result = GetDealCampaignMappinQueryResult(filter, Id);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions
            var action = GetDealMappingAction(Id);
            #endregion
            var toolTip = GetDealCampaignTooltip();

            return new DealCampaignMappingListViewModel()
            {

                Items = items,

                BusinessId = result.BusinessId,

                BusinessName = result.BusinessName,

                TopActions = action,
                BelowAction = null,
                ToolTips = toolTip
            };
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetDealMappingAction(int Id)
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "SSPDealCampaign"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNew", "SSPDealCampaign"),
                                ExtraPrams3="Partner"
                        }
                };
            #endregion

            return actions;
        }
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetDealCampaignTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


                     }


                };
        }
        #endregion

        private List<SelectListItem> GetList(string lookupType, int? selectedValue)
        {
            LookupListResultDto items = null;
            IList<LookupDto> itemslookup = new List<LookupDto>(); ;
            if (lookupType != LookupNames.AdType && lookupType != LookupNames.NativeAdLayout && lookupType != LookupNames.BidConfigType)
                items = _lookupService.GetAllLookup(new LookupCriteriaBase { LookType = lookupType });
            else
            {
                if (lookupType == LookupNames.AdType)
                {
                    items = _lookupService.GetAdTypes();
                    foreach (var item in items.Items)
                    {
                        if (item.ID != (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.AdTypeIds.TrackingAd)
                        {
                            itemslookup.Add(item);
                        }

                    }
                    items.Items = itemslookup;
                    items.TotalCount = items.TotalCount - 1;
                }

                if (lookupType == LookupNames.NativeAdLayout)
                {
                    items = _lookupService.GetNativeAdLayouts();
                }
                if (lookupType == LookupNames.BidConfigType)
                {
                    items = _demandSupplyService.GetBidConfigTypeList();
                }

            }
            var lookupsList = new List<SelectListItem>();
            if (lookupType != LookupNames.BidConfigType && lookupType != LookupNames.DeviceType)
                lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};
            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }));
            return lookupsList;
        }
    }
}
