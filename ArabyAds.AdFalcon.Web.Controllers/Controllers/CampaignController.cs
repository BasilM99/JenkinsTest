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

using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Telerik.Web.Mvc.UI;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;
using Noqoush.AdFalcon.Web.Controllers.Model.Advertiser;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    public class CampaignController : AuthorizedControllerBase
    {
        protected ILookupService _lookupService;
        protected const string _nativeAdIconsGroup = "9";
        protected const string _nativeAdImagesGroup = "10";
        protected const string _separator = "&";
        protected const string _bidSeparator = ",";
        private const string IMPRESSIONEVENT = "000imp";
        private const string CLICKEVENT = "000clk";
        private IAppSiteTypeService _appSiteTypeService;
        protected IKeywordService _keywordService;
        protected ICampaignService _campaignService;
        protected IObjectiveTypeService _objectiveTypeService;
        protected ICreativeUnitService _creativeUnitService;
        protected IAgeGroupService _ageGroupService;
        protected IAudienceSegmentService _audienceSegmentService;
        private IAppSiteService _appSiteService;
        protected IAdvertiserService _AdvertiserService;

        protected ITileImageService _tileImageService;
        protected IAdCreativeStatusService _adCreativeStatusService;
        protected IDeviceCapabilityService _deviceCapabilityService;
        protected IRichMediaRequiredProtocolService _richMediaRequiredProtocolService;
        protected ILocationService _locationService;
        protected IDeviceTypeService _deviceTypeService;
        protected IPlatformService _platformService;
        protected ICostModelWrapperService _CostModelWrapperService;
        protected IVideoTypeService _videoTypeService;
        private ITrackingEventService _trackingEventService;
        protected IAccountService _accountService;
        protected IVideoDeliveryMethodsService _videoDeliveryMethodsService;
        protected IAppMarketingPartnerService _appMarketingPartnerServic;

        protected ICreativeVendorService _creativeVendorService;
        protected ILanguageService _languageService;
        protected IUserService _userService;
        protected IReportService _reportService;

        protected IPartyService _partyService;
        public CampaignController(
            IAudienceSegmentService AudienceSegmentService,
            IAccountService accountService,
        ICampaignService campaignService,
                                IKeywordService keywordService,
                                IObjectiveTypeService objectiveTypeService,
                                ICreativeUnitService creativeUnitService,
                                IAgeGroupService ageGroupService,
                                ITileImageService tileImageService,
                                IAdCreativeStatusService adCreativeStatusService,
                                IDeviceCapabilityService deviceCapabilityService,
                                IRichMediaRequiredProtocolService richMediaRequiredProtocolService,
                                ILocationService locationService,
                                IDeviceTypeService deviceTypeService,
                                ITrackingEventService trackingEventService,
                                IPlatformService platformService,
                                ICostModelWrapperService costModelWrapperService,
                               IVideoTypeService videoTypeService,

           IVideoDeliveryMethodsService videoDeliveryMethodsService, IAppSiteTypeService appSiteTypeService, IAppMarketingPartnerService appMarketingPartnerService, IUserService userService, IAppSiteService appSiteService, IAdvertiserService AdvertiserService

, ICreativeVendorService creativeVendorService, ILanguageService LanguageService, IReportService reportService
            , IPartyService partService, ILookupService lookupService
)
        {
            _lookupService = lookupService;
            _audienceSegmentService = AudienceSegmentService;
            _keywordService = keywordService;
            _campaignService = campaignService;
            _objectiveTypeService = objectiveTypeService;
            _creativeUnitService = creativeUnitService;
            _ageGroupService = ageGroupService;
            _tileImageService = tileImageService;
            _adCreativeStatusService = adCreativeStatusService;
            _deviceCapabilityService = deviceCapabilityService;
            _richMediaRequiredProtocolService = richMediaRequiredProtocolService;
            _locationService = locationService;
            _deviceTypeService = deviceTypeService;
            _trackingEventService = trackingEventService;
            _platformService = platformService;
            _CostModelWrapperService = costModelWrapperService;
            _videoTypeService = videoTypeService;
            _videoDeliveryMethodsService = videoDeliveryMethodsService;
            _appSiteTypeService = appSiteTypeService;
            _appMarketingPartnerServic = appMarketingPartnerService;
            _accountService = accountService;
            this._appSiteService = appSiteService;
            _AdvertiserService = AdvertiserService;
            _userService = userService;
            _creativeVendorService = creativeVendorService;
            _languageService = LanguageService;
            _reportService = reportService;
            this._partyService = partService;
        }
        public ActionResult anugualrtest()
        {


            return View("~/Views/Campaign/anugualrtest.cshtml");
        }

        #region Campaign
        #region Create

        public virtual ActionResult CreateRD(int? id)
        {
            var AdvertsId = _campaignService.GetAdvertiserAccountIdByCampaignId(id.Value);

            return !Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ? RedirectToAction("CreateAll", "Campaign", new { id = id, AdvertiseraccId = AdvertsId }): RedirectToAction("Create", "Campaign", new { id = id, AdvertiseraccId = AdvertsId });
        }
        public virtual ActionResult Create(int? AdvertiseraccId, int? id)
        {
         // Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase("CampaignStarted", id.Value.ToString(), null,null));

            //Framework.EventBroker.EventBroker.Instance.Flush();
            int? campaignId = id;
            //string advertiserName = string.Empty;
            int advId = 0;
            string advertiserAccountName = string.Empty;
            if (AdvertiseraccId > 0)
            {
                advertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
                advId = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);
               
            }
            if (campaignId.HasValue)
            {
                CampaignDto campaignDto = _campaignService.Get(campaignId.Value, CampaignType.Normal,CampaignType.ProgrammaticGuaranteed);

                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    IsRequired = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = campaignDto.Advertiser != null ? campaignDto.Advertiser.Name.ToString() : string.Empty
                };
                //this is update
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =campaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                              Url = Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId})
                                              }
                                           ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                   Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown = true
                                              }
                                      };
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
                #endregion
                if (AdvertiseraccId>0)
                {
                    if (!_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId.Value))
                    {

                        campaignDto.IsReadOnly = true;

                    }
                }
                return View(campaignDto);
            }
            else
            {
                CampaignDto campaignDto = new CampaignDto();
                campaignDto.Advertiser = _AdvertiserService.Get(advId);
                campaignDto.AdvertiserAccountId = AdvertiseraccId.Value;
                campaignDto.StartDate = Noqoush.Framework.Utilities.Environment.GetServerTime();
                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    IsRequired = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = string.Empty
                };
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("NewCampaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order =3,
                                                  Url = Url.Action("Index", new { AdvertiseraccId=id})
                                              }

                                          ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers"),
                                                  ExtensionDropDown = true
                                              }

                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                #endregion
                if (AdvertiseraccId > 0)
                {

                    ViewData["CreateAdvertiserId"] = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);

                }
                if (!_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId.Value))
                {

                    campaignDto.IsReadOnly = true;

                }
                return View(campaignDto);
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]
        public virtual ActionResult Create(CampaignDto campaignDto, int? AdvertiseraccId, int? id, string returnUrl)
        {
            //string advertiserName = string.Empty;
            string advertiserAccountName = string.Empty;

            ViewData["CreateAdvertiserAccountId"] = AdvertiseraccId.Value;

            if (AdvertiseraccId > 0)
            {

                ViewData["CreateAdvertiserId"] = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);

                // advertiserName = _campaignService.GetAdvertiserString(AdvertiserId.Value);
                advertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
            }
            #region BreadCrumb
            //TODO:osaleh to use the old name not the new name in breadcrumb if an exception is been thrown 
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =campaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url = Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId})
                                              }
                                             ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                   Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown = true
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            int? campaignId = id;

            if (ModelState.IsValid)
            {
                if (campaignId.HasValue)
                {
                    //this is update
                    campaignDto.ID = campaignId.Value;
                   // campaignDto.CampaignType = CampaignType.Normal;
                    try
                    {

                        var result = _campaignService.Save(campaignDto);
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
                            return RedirectToAction("Create", new { AdvertiseraccId = AdvertiseraccId, id = result.ID });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { AdvertiseraccId = AdvertiseraccId, id = result.ID, returnUrl = returnUrl });
                        }
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                    }
                    ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                    {
                        Id = "Advertisers_Name",
                        Name = "Advertisers.Name",
                        ActionName = "GetAdvertisers",
                        ControllerName = "Advertiser",
                        LabelExpression = "item.Name",
                        ValueExpression = "item.Id",
                        IsAjax = true,
                        ChangeCallBack = "AdvertisersChanged",
                        CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
                    };
                    return View(campaignDto);
                }
                else
                {
                    int newId = 0;
                    try
                    {
                        campaignDto.CampaignType = CampaignType.Normal;
                        var result = _campaignService.Save(campaignDto);
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
                        ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                        {
                            Id = "Advertisers_Name",
                            Name = "Advertisers.Name",
                            ActionName = "GetAdvertisers",
                            ControllerName = "Advertiser",
                            LabelExpression = "item.Name",
                            ValueExpression = "item.Id",
                            IsAjax = true,
                            ChangeCallBack = "AdvertisersChanged",
                            CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
                        };
                        return View(campaignDto);
                    }
                    MoveMessagesTempData();
                    if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
                    {
                        return RedirectToAction("Objective", new { id = newId });
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Create", new { AdvertiseraccId = AdvertiseraccId, id = newId });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { AdvertiseraccId = AdvertiseraccId, id = newId, returnUrl = returnUrl });
                        }
                    }
                }

            }
            else
            {
                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
                };
                return View(campaignDto);
            }
        }
        #endregion
        #region Index
        #region Helpers
        protected Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter getDefualtFilter(int? AdvertiserId = null, int? AdvertiserAccountId = null)
        {
            Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter = new Model.Campaign.Filter();
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? string.Empty : Request.Form["Name"];
            filter.AdvertiserId = AdvertiserId.HasValue ? AdvertiserId.Value : (int?)null;
            filter.AdvertiserAccountId = AdvertiserAccountId.HasValue ? AdvertiserAccountId.Value : (int?)null;
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            filter.StatusId = string.IsNullOrWhiteSpace(Request.Form["StatusId"]) ? (int?)null : Convert.ToInt32(Request.Form["StatusId"]);
            filter.TypeId = string.IsNullOrWhiteSpace(Request.Form["TypeId"]) ? (int?)null : Convert.ToInt32(Request.Form["TypeId"]);
            filter.Domain = string.IsNullOrWhiteSpace(Request.Form["Domain"]) ? string.Empty : Request.Form["Domain"];
            filter.BundleId = string.IsNullOrWhiteSpace(Request.Form["BundleId"]) ? string.Empty : Request.Form["BundleId"];
            filter.showGlobal = string.IsNullOrWhiteSpace(Request.Form["showGlobal"]) ? false : Convert.ToBoolean(Request.Form["showGlobal"]);
            filter.showAccountAdv = string.IsNullOrWhiteSpace(Request.Form["showAccountAdv"]) ? false : Convert.ToBoolean(Request.Form["showAccountAdv"]);
            filter.showRoot = string.IsNullOrWhiteSpace(Request.Form["showRoot"]) ? false : Convert.ToBoolean(Request.Form["showRoot"]);
            filter.showArchived = string.IsNullOrWhiteSpace(Request.Form["showArchived"]) ? false : Convert.ToBoolean(Request.Form["showArchived"]);
            return filter;
        }
        protected CampaignCriteria GetCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new CampaignCriteria
            {
                AdvertiserId = filter.AdvertiserId.HasValue ? filter.AdvertiserId : null,
                AdvertiserAccountId = filter.AdvertiserAccountId.HasValue ? filter.AdvertiserAccountId : null,
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                Name = filter.Name
                //StatusId = filter.StatusId
            };

            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }

            return criteria;
        }
        protected virtual CampaignListResultDto GetQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            var criteria = GetCriteria(filter);
            var result = _campaignService.QueryByCratiria(criteria);
            return result;
        }
        protected virtual ListViewModel LoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            var result = GetQueryResult(filter);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            // create the actions
            List<Action> actions = null;
            List<Action> toolTips = null;

            actions = filter != null && filter.AdvertiserAccountId.HasValue ? GetCampaignAction(filter.AdvertiserAccountId.Value) : GetCampaignAction();
            toolTips = filter != null && filter.AdvertiserAccountId.HasValue ? GetCampaignTooltip(filter.AdvertiserAccountId.Value) : GetCampaignTooltip();

            //load the statues 
            //var statues = _campaignStatusService.GetAll();
            //var statuesDropDown = GetSelectList();
            //foreach (var item in statues)
            //{
            //    var selectItem = new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() };
            //    statuesDropDown.Add(selectItem);
            //}
            return new ListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                ToolTips = toolTips,
                Performance = result.Performance != null ? result.Performance : new Services.Interfaces.DTOs.Reports.AdvertiserPerformanceDto(),
                AdvertiserId = result.AdvertiserId,
                AdvertiserName = result.AdvertiserName,
                AdvertiserAccountId = result.AdvertiserAccountId,
                AdvertiserAccountName = result.AdvertiserAccountName,
                PreventEdit = result.AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(result.AdvertiserAccountId) : false
                //, Statuses = statuesDropDown 
            };
        }

        protected virtual List<Action> GetCampaignAction(int AdvertiserAccountId)
        {
            var actions = new List<Model.Action>();
            #region Actions

            actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Run", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Run", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },new Model.Action()
                        {
                            ActionName = Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            ExtraPrams=AdvertiserAccountId,
                            DisplayText = ResourcesUtilities.GetResource("AddNewCampaign", "Commands")
                        }

                };

            #endregion





            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {
                actions = new List<Model.Action>();


            }

            return actions;
        }
        protected virtual List<Model.Action> GetCampaignTooltip(int AdvertiserAccountId)
        {
            var actions = new List<Model.Action>();
            // Create the tool tip actions
            actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName =  Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",
                            ExtraPrams=AdvertiserAccountId
                        },
                    new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                            ClassName = "grid-tool-tip-reports",
                            URL = Url.Action("index", "Reports", new {reportType = "ad"})
                        }
                    ,
                    new Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Copy", "Commands"),
                            ClassName = "grid-tool-tip-copy",
                            ActionName = "CopyCampaign",
                            Type = ActionType.ajax,
                            AjaxType = AjaxType.clone,
                            CallBack = "refreshCampaignGrid();"
                        },
                          new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }

                };



            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {
                actions = new List<Model.Action>();

                actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                             DisplayText = ResourcesUtilities.GetResource("View", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName =  Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",
                            ExtraPrams=AdvertiserAccountId
                        } };
            }
            return actions;
        }



        protected virtual List<Action> GetCampaignAction()
        {
            #region Actions

            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Run", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Run", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?

                        },
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Campaign"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        }

                };
            #endregion

            return actions;
        }

        protected virtual List<Model.Action> GetCampaignTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "CreateRD"
                        },
                    new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                            ClassName = "grid-tool-tip-reports",
                            URL = Url.Action("index", "Reports", new {reportType = "ad"})
                        }
                    ,
                    new Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Copy", "Commands"),
                            ClassName = "grid-tool-tip-copy",
                            ActionName = "CopyCampaign",
                            Type = ActionType.ajax,
                            AjaxType = AjaxType.clone,
                            CallBack = "refreshCampaignGrid();"
                        },
                          new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


        }

                };
        }
        #endregion

        #region Actions

        public virtual ActionResult CopyCampaign(int id, string name)
        {
            var result = _campaignService.CloneCampaign(id, name);
            if (result.success)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 0;
            }
            return Content(result.Massage);
        }
        public virtual ActionResult RenameGroup(int GroupId, int CampiagnId, string name)
        {
            var result = _campaignService.RenameGroup(CampiagnId, GroupId, name);
            if (!string.IsNullOrWhiteSpace(result))
            {
                Response.StatusCode = 200;
                return Content(result);
            }
            else
            {
                Response.StatusCode = 0;
                return Content(ResourcesUtilities.GetResource("CloneCampaignError", "Errors"));
            }
        }

        public virtual ActionResult RedirectToAuditTrial(int id)
        {

            string originalPath = new Uri(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri).OriginalString;

            try
            {
                int objectRootTypeId = _accountService.GetObjectRootTypeId("Noqoush.AdFalcon.Domain.Common.Model.Campaign.Campaign");
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

                return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = id, objectRootTypeId = objectRootTypeId, returnUrl = originalPath });
            }
            catch (Exception e)
            {

                throw e;
            }


        }
        public virtual ActionResult Index(int? AdvertiseraccId)
        {
            string advertisrName = string.Empty;
            string advertisrAccountName = string.Empty;
            if (AdvertiseraccId.HasValue)
            {
                //if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(AdvertiseraccId.Value))
                //{
                //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                //}
                advertisrAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
            }


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();
            if (AdvertiseraccId.HasValue)
            {
                breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                   Url = Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId})
                                              }
                                             ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertisrAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers"),
                                                  ExtensionDropDown = true
                                              }
                                      };
            }
            else
            {

                breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                     new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers")

                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AdvertisersCampaignList", "Global"),
                                                  Order = 2,
                                              }

                                      };

                if (!OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                {

                    throw new AccountNotValidException();
                }
            }


            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var filter = getDefualtFilter(null, AdvertiseraccId);
            ShowupdateLastActionDone();
            return View(LoadData(filter));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Index(int? AdvertiseraccId, int[] checkedRecords)
        {


            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _campaignService.Delete(checkedRecords);
                updateLastActionDone(ResourcesUtilities.GetResource("CampaignsDeletedSuccessfully", "Campaign"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
                {
                    //run  selected Campaigns
                    _campaignService.Run(checkedRecords);
                    updateLastActionDone(ResourcesUtilities.GetResource("CampaignsRunSuccessfully", "Campaign"));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
                    {
                        //pause selected Campaigns
                        _campaignService.Pause(checkedRecords);
                        updateLastActionDone(ResourcesUtilities.GetResource("CampaignsPausedSuccessfully", "Campaign"));
                    }
                }
            }
            string advertisrName = string.Empty;
            string advertisrAccountName = string.Empty;
            if (AdvertiseraccId.HasValue)
            {
                advertisrAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
                //   advertisrName = _campaignService.GetAdvertiserString(AdvertiserId.Value);

            }
            var breadCrumbLinks = new List<BreadCrumbModel>();
            #region BreadCrumb
            if (!AdvertiseraccId.HasValue)
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 1,
                                              }
                                      };
            }
            else
            {
                breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                 Url =  Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId.Value})
                                              }
                                             ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertisrAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers")
                                              }
                                      };
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            if (!AdvertiseraccId.HasValue)
                return RedirectToAction("index");
            else
                return RedirectToAction("index", new { AdvertiseraccId = AdvertiseraccId });
        }
        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _Index(int? AdvertiseraccId)
        {
            CampaignListResultDto result = null;
            var filter = getDefualtFilter(null, AdvertiseraccId);
            result = GetQueryResult(filter);


            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        //public virtual ActionResult CampaignSearch()
        //{
        //    var result = GetQueryResult(null);
        //    ViewData["total"] = result.TotalCount;
        //    return PartialView(result);

        //}

        #endregion
        #endregion
        #endregion
        #region Ad Groups
        #region Index
        #region Helpers

        protected AdGroupCriteria GetAdGroupCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdGroupCriteria
            {
                DateFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DateTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.Name
                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected AdGroupSearchDto GetAdGroupQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int campaignId)
        {
            var criteria = GetAdGroupCriteria(filter);
            criteria.CampaignId = campaignId;

            var result = _campaignService.QueryGroupsByCratiria(criteria);
            return result;
        }
        protected AdGroupListViewModel LoadGroupsData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int campaignId)
        {
            var result = GetAdGroupQueryResult(filter, campaignId);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            #region Actions
            var actions = GetAdGroupActions(campaignId, result.AdvertiserAccountId);
            var toolTips = GetAdGroupTooltips(campaignId, result.AdvertiserAccountId);
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
            return new AdGroupListViewModel()
            {
                CampaignName = result.CampaignName,
                AdvertiserName = result.AdvertiserName,
                AdvertiserId = result.AdvertiserId,
                AdvertiserAccountName = result.AdvertiserAccountName,
                AdvertiserAccountId = result.AdvertiserAccountId,
                Items = items,
                Performance = result.Performance,
                TopActions = actions,
                BelowAction = actions,
                ToolTips = toolTips,
                //Statuses = statuesDropDown,
                CampaignId = campaignId,
                PreventEdit = result.AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(result.AdvertiserAccountId) :false
            };
        }

        protected virtual List<Action> GetAdGroupTooltips(int campaignId, int AdvertiseraccId = 0)
        {
            // Create the tool tip actions
            var toolTips = new List<Model.Action>();
            toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Targeting",
                            ExtraPrams = campaignId
                        },
                    new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                            ClassName = "grid-tool-tip-reports",
                            URL = Url.Action("index", "Reports", new {reportType = "ad"})
                        },
                    new Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Copy", "Commands"),
                            ClassName = "grid-tool-tip-copy",
                            ActionName = "CopyAdGroup",
                            ExtraPrams = campaignId,
                            Type = ActionType.ajax,
                            CallBack = "refreshCampaignGrid();"
                        },
                     new Model.Action()
                        {
                            Code = "3",
                            DisplayText = ResourcesUtilities.GetResource("Rename", "Commands"),
                            ClassName = "grid-tool-tip-rename",
                            ActionName = "RenameGroup?GroupId=",
                            Type = ActionType.ajax,
                            AjaxType = AjaxType.rename,
                            CallBack = "refreshCampaignGrid()"
                        },

                          new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",
                            ExtraPrams = campaignId,


                        }
                };


            if (AdvertiseraccId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
            {
                toolTips = new List<Model.Action>();

                toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("View", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Targeting",
                            ExtraPrams = campaignId
                        }
                };


            }
            return toolTips;
        }

        protected virtual List<Action> GetAdGroupActions(int campaignId, int AdvertiseraccId = 0)
        {
            // create the actions
            var actions = new List<Model.Action>();
            actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Run", "Commands"),
                              ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "AdGroup"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Run", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "Commands"),
                              ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "AdGroup"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                             ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "AdGroup"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "Objective",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewGroup", "Commands"),
                            ExtraPrams = campaignId
                        }
                };


            if (AdvertiseraccId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
            {
                actions = new List<Model.Action>();


            }
            return actions;
        }
        #endregion
        #region Actions
        public ActionResult CopyAdGroup(int id, int adGroupId, string name)
        {
            ResponseDto result = _campaignService.CloneAdGroup(id, adGroupId, name);
            if (result.success)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 0;
            }
            return Content(result.Massage);
        }
        public ActionResult Groups(int id)
        {
            int campaignId = id;
            //if (!_campaignService.IsReadOrWriteCampaign(campaignId))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}
            AdGroupListViewModel groups = LoadGroupsData(null, campaignId);

            if (groups.AdvertiserAccountId > 0)
            {
                if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(groups.AdvertiserAccountId))
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }
            }

            string advertisrName = string.Empty;
            var breadCrumbLinks = new List<BreadCrumbModel>();
            #region BreadCrumb
            if (string.IsNullOrEmpty(groups.AdvertiserAccountName))
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 3,
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =groups.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                                Url =  Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };
            }
            else
            {

                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 5,
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =groups.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {AdvertiseraccId= groups.AdvertiserAccountId, id=id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index", new { AdvertiseraccId=groups.AdvertiserAccountId}),
                                                  Order = 3,
                                              }

                                       ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =groups.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2

                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Url=Url.Action("AccountAdvertisers"),
                                                  Order = 1,
                                                  ExtensionDropDown = true
                                              }
                                      };
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            ShowupdateLastActionDone();
            return View("IndexGroup", groups);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Groups(int id, int[] checkedRecords)
        {
            int campaignId = id;
            string advertisrName = string.Empty;

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _campaignService.DeleteGroups(campaignId, checkedRecords);
                updateLastActionDone(ResourcesUtilities.GetResource("AdGroupsDeletedSuccessfully", "Campaign")); 
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
                {
                    //run  selected Campaigns
                    _campaignService.RunGroups(campaignId, checkedRecords);

                    updateLastActionDone(ResourcesUtilities.GetResource("AdGroupsRunSuccessfully", "Campaign")); 
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
                    {
                        //pause selected Campaigns
                        _campaignService.PauseGroups(campaignId, checkedRecords);
                        updateLastActionDone(ResourcesUtilities.GetResource("AdGroupsPausedSuccessfully", "Campaign"));
                    }
                }
            }



           
            return RedirectToAction("Groups", new { id = campaignId });
        }

        public void updateLastActionDone(string lastactiondone)
        {
            var userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            userInfo.LastActionDone = lastactiondone;
            OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);

        }

        public void ShowupdateLastActionDone()
        {
            var userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            if (!string.IsNullOrEmpty(userInfo.LastActionDone))
            {
                AddMessages(userInfo.LastActionDone, MessagesType.Success);
                userInfo.LastActionDone = string.Empty;
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);
            }

        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Groups(int id)
        {

            int campaignId = id;
            var result = GetAdGroupQueryResult(null, campaignId);
            return View("IndexGroup", new GridModel { Data = result.Items, Total = Convert.ToInt32(result.TotalCount) });
        }

        #endregion

        #endregion
        #region Targeting
        #region Helpers
        protected void FillTargeting(TargetingViewModel targetingViewModel, IEnumerable<TargetingBaseDto> Targetings)
        {
            #region Keywords
            //get keyword
            var keywordsList = Targetings.OfType<KeywordTargetingDto>().Select(obj => obj.Keyword).Where(M=>M.Code!= "sensit").ToList();
            var keywordTargetings = Targetings.OfType<KeywordTargetingDto>().ToList();

            var LanguageList = Targetings.OfType<LanguageTargetingDto>().Select(obj => obj.Language).ToList();
            var LanguageTargetings = Targetings.OfType<LanguageTargetingDto>().ToList();

            targetingViewModel.KeywordTargetingViewModel.KeywordsTargeting = keywordTargetings;
            targetingViewModel.KeywordTargetingViewModel.KeywordViewModel.Keywords = keywordsList;
            #endregion
            #region Demographic
            //get demographic Targeting
            var demographicTargetingsList = Targetings.OfType<DemographicTargetingDto>().ToList();
            targetingViewModel.DemographicTargetingView.DemographicTargeting = demographicTargetingsList.FirstOrDefault();
            if ((targetingViewModel.DemographicTargetingView.DemographicTargeting != null) && (targetingViewModel.DemographicTargetingView.DemographicTargeting.Demographic != null))
            {
                if (targetingViewModel.DemographicTargetingView.DemographicTargeting.Demographic.AgeGroup != null)
                {
                    var ageGroupItem = targetingViewModel.DemographicTargetingView.AgeGroups.FirstOrDefault(item => item.Value == targetingViewModel.DemographicTargetingView.DemographicTargeting.Demographic.AgeGroup.ID.ToString());
                    if (ageGroupItem != null)
                    {
                        ageGroupItem.Selected = true;
                    }
                }
            }
            #endregion
            #region Device Targeting

            //get Device Targeting
            var deviceTargetings = Targetings.OfType<DeviceTargetingDto>().ToList();
            //init type is All
            targetingViewModel.DeviceTargetingView.deviceTargeting = deviceTargetings;
            targetingViewModel.DeviceTargetingView.LanguagesTargeting = LanguageTargetings;
            if ((deviceTargetings.Count > 0) && (deviceTargetings.First() != null))
            {
                targetingViewModel.DeviceTargetingView.Type = deviceTargetings.First().TargetingType != null
                                                              ? deviceTargetings.First().TargetingType.ID
                                                              : 0;




                switch (targetingViewModel.DeviceTargetingView.Type)
                {
                    case 1:
                        {
                            if (deviceTargetings.First().Platforms != null)
                            {
                                //targetingViewModel.DeviceTargetingView.Platforms.SelectedValues =
                                //    deviceTargetings.FirstOrDefault().Platforms.Select(obj => obj.ID.ToString()).ToArray();

                                List<PlatformDto> platforms = targetingViewModel.DeviceTargetingView.Platforms;

                                foreach (var obj in deviceTargetings.First().Platforms)
                                {
                                    PlatformDto platformDto = platforms.Where(p => p.ID == obj.ID).SingleOrDefault();
                                    platformDto.IsSelected = true;
                                    PlatformVersionDto version = obj.Versions.Where(p => p.IsSelected).SingleOrDefault();
                                    if (version != null)
                                    {
                                        var platformVersion = platformDto.Versions.Where(p => p.Code == version.Code).SingleOrDefault();
                                        platformVersion.IsSelected = true;
                                    }
                                }

                            }
                            break;
                        }
                    case 2:
                        {
                            if (deviceTargetings.First().Manufacturers != null)
                            {
                                //targetingViewModel.DeviceTargetingView.Manufacturers.SelectedValues =
                                //    deviceTargetings.FirstOrDefault().Manufacturers.Select(obj => obj.ID.ToString()).ToArray();
                                targetingViewModel.DeviceTargetingView.Manufacturers.SelectedValues = new List<TreeSelectedValue>();
                                foreach (var obj in deviceTargetings.First().Manufacturers)
                                {
                                    targetingViewModel.DeviceTargetingView.Manufacturers.SelectedValues.Add(new TreeSelectedValue { Id = obj.ID.ToString() });
                                }

                            }
                            break;
                        }
                    case 3:
                        {
                            //toDO:osaleh to add support for the model list
                            if (deviceTargetings.First().Devices != null)
                            {
                                //targetingViewModel.DeviceTargetingView.DevicesTree.SelectedValues = deviceTargetings.FirstOrDefault().Devices.Select(obj => obj.ID.ToString()).ToArray();
                                targetingViewModel.DeviceTargetingView.DevicesTree.SelectedValues = new List<TreeSelectedValue>();
                                foreach (var obj in deviceTargetings.First().Devices)
                                {
                                    targetingViewModel.DeviceTargetingView.DevicesTree.SelectedValues.Add(new TreeSelectedValue { Id = obj.ID.ToString() });
                                }

                                foreach (var item in deviceTargetings.First().Devices)
                                {
                                    var info = new Dictionary<string, string>();
                                    info["ManufacturerId"] = item.Manufacturer.ID.ToString();
                                    info["PlatformId"] = item.Platform.ID.ToString();
                                    targetingViewModel.DeviceTargetingView.DevicesTree.SelectedItems.Add(new LookupItem
                                    {
                                        Id = item.ID.ToString(),
                                        DispalValue = item.Name.ToString(),
                                        Info = info
                                    });
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            var devices = targetingViewModel.DeviceTargetingView.Devices;

                            // ALID
                            devices.SelectedValues = new List<TreeSelectedValue>();
                            devices.IsSelectAll = false;

                            if (targetingViewModel.DeviceTargetingView.Devices != null)
                            {
                                //targetingViewModel.DeviceTargetingView.Devices.SelectedValues = deviceTargetings.FirstOrDefault().Devices.Select(obj => obj.ID.ToString()).ToArray();
                                foreach (var obj in deviceTargetings.First().Devices)
                                {
                                    devices.SelectedValues.Add(new TreeSelectedValue { Id = obj.ID.ToString(), Key = "Models" });
                                }
                            }


                            if ((deviceTargetings.First().Manufacturers != null) && (deviceTargetings.First().Manufacturers.Count() > 0))
                            {

                                foreach (var obj in deviceTargetings.First().Manufacturers)
                                {
                                    devices.SelectedValues.Add(new TreeSelectedValue { Id = obj.ID.ToString(), Key = "Manufacturers" });
                                }
                            }

                            if (deviceTargetings.First().Platforms != null)
                            {
                                var platformTargeting = deviceTargetings.First().Platforms;
                                // var firstPlatform = platformTargeting.FirstOrDefault();
                                foreach (var platform in platformTargeting)
                                {
                                    string platformCode = platform.Versions.Where(p => p.IsSelected).Select(p => p.Code).FirstOrDefault();

                                    if (!string.IsNullOrEmpty(platformCode))
                                    {

                                        var viewModelPlatforms = targetingViewModel.DeviceTargetingView.Platforms.Where(p => p.ID == platform.ID).SingleOrDefault();
                                        viewModelPlatforms.IsSelected = true;
                                        viewModelPlatforms.Versions.Where(p => p.Code == platformCode).SingleOrDefault().IsSelected = true;
                                    }
                                }
                                foreach (var obj in platformTargeting.Where(x => x.IsSelected).ToList())
                                {
                                    devices.SelectedValues.Add(new TreeSelectedValue { Id = obj.ID.ToString(), Key = "Platforms" });
                                }

                            }

                            break;
                        }
                    case 5:
                        {
                            if (deviceTargetings.First().DeviceCapabilities != null)
                            {
                                foreach (var obj in deviceTargetings.First().DeviceCapabilities.Where(x => x.IsInclude))
                                {
                                    var item = targetingViewModel.DeviceTargetingView.DeviceCapabilities.FirstOrDefault(x => x.ID == obj.ID);
                                    if (item != null)
                                    {
                                        item.Selected = true;
                                        item.IsInclude = true;
                                    }
                                }
                            }
                            break;
                        }
                }

                if (deviceTargetings.First().DeviceCapabilities != null)
                {
                    foreach (var obj in deviceTargetings.First().DeviceCapabilities.Where(x => !x.IsInclude))
                    {
                        var item = targetingViewModel.DeviceTargetingView.DeviceCapabilities.FirstOrDefault(x => x.ID == obj.ID);
                        if (item != null)
                        {
                            item.Selected = true;
                            item.IsInclude = false;
                        }
                    }
                }

            }

            #endregion
            #region Geographies
            //get Geographies Targeting
            var geographicsTargetings = Targetings.OfType<GeographicTargetingDto>().ToList();
            // targetingViewModel.Geographics.SelectedValues = geographicsTargetings.Select(obj => obj.Location.Id.ToString()).ToArray();
            targetingViewModel.Geographics.GeographicalAreas.SelectedValues = new List<TreeSelectedValue>();
            foreach (var obj in geographicsTargetings)
            {
                targetingViewModel.Geographics.GeographicalAreas.SelectedValues.Add(new TreeSelectedValue { Id = obj.Location.ID.ToString() });
            }
            targetingViewModel.Geographics.GeographicalAreas.IsAll = targetingViewModel.Geographics.GeographicalAreas.SelectedValues.Count() > 0 ? 2 : 1;

            var geoFencingTargetings = Targetings.OfType<GeoFencingTargetingDto>().ToList();

            if (geoFencingTargetings.Count > 0)
            {
                targetingViewModel.Geographics.GeoFencings = geoFencingTargetings;
            }

            #endregion
            #region Operators
            //get Operators Targeting
            var operatersTargetings = Targetings.OfType<OperatorTargetingDto>().ToList();
            targetingViewModel.OperaterTargetingView.Operaters.SelectedValues = new List<TreeSelectedValue>();
            targetingViewModel.URLs = new List<URLTargetingView>();

            var urlTargetings = Targetings.OfType<URLTargetingDto>().ToList();
            if (urlTargetings != null && urlTargetings.Count > 0)
            {
                List<URLTargetingView> urlModelTargeting = new List<URLTargetingView>();
                foreach (var item in urlTargetings)
                {
                    urlModelTargeting.Add(new URLTargetingView() { ID = item.ID, URL = item.URL });
                }

                targetingViewModel.URLs = urlModelTargeting;
            }

            foreach (var obj in operatersTargetings)
            {
                targetingViewModel.OperaterTargetingView.Operaters.SelectedValues.Add(new TreeSelectedValue { Id = obj.Operator.Id.ToString() });
            }
            targetingViewModel.OperaterTargetingView.Operaters.IsAll = targetingViewModel.OperaterTargetingView.Operaters.SelectedValues.Any() ? 2 : 1;
            if (operatersTargetings.Any(x => x.Operator.Id == Config.WIFIOperaterId))
            {
                targetingViewModel.OperaterTargetingView.Operaters.IsAll = 3;
            }
            #region IP Range
            //get ips
            var ipTargetings = Targetings.OfType<IPTargetingDto>().ToList();
            if (ipTargetings.Count > 0)
            {
                targetingViewModel.OperaterTargetingView.Operaters.IsAll = 4;
            }
            if (ipTargetings.Count > 0)
            {
                targetingViewModel.OperaterTargetingView.IPRanges = new List<IPTargetingView>();
            }
            foreach (var ipTargetingDto in ipTargetings)
            {
                targetingViewModel.OperaterTargetingView.IPRanges.Add(new IPTargetingView
                {
                    ID = ipTargetingDto.ID,
                    StartRange = ipTargetingDto.StartRange,
                    EndRange = ipTargetingDto.EndRange,
                    Description = ipTargetingDto.Description
                });
            }

            #endregion
            #endregion
            #region videoTargeing


            var VideoTargeting = Targetings.OfType<VideoTargetingDto>().ToList();

            if (VideoTargeting != null && VideoTargeting.Count > 0)
            {
                var Videodto = VideoTargeting[0];


                targetingViewModel.PlacementType_InStream = Videodto.PlacementType_InStream;
                targetingViewModel.PlacementType_OutStream = Videodto.PlacementType_OutStream;
                targetingViewModel.PlacementType_Interstitial = Videodto.PlacementType_Interstitial;
                targetingViewModel.PlacementType_Undetermined = Videodto.PlacementType_Undetermined;
                targetingViewModel.InStreamPosition_PreRoll = Videodto.InStreamPosition_PreRoll;
                targetingViewModel.InStreamPosition_MidRoll = Videodto.InStreamPosition_MidRoll;
                targetingViewModel.InStreamPosition_PostRoll = Videodto.InStreamPosition_PostRoll;
                targetingViewModel.InStreamPosition_Undetermined = Videodto.InStreamPosition_Undetermined;
                targetingViewModel.SkippableAds_SkippableAdSpaces = Videodto.SkippableAds_SkippableAdSpaces;
                targetingViewModel.SkippableAds_NonSkippableAdSpaces = Videodto.SkippableAds_NonSkippableAdSpaces;
                targetingViewModel.SkippableAds_Undetermined = Videodto.SkippableAds_Undetermined;
                targetingViewModel.Playback_AutoPlaySoundOn = Videodto.Playback_AutoPlaySoundOn;
                targetingViewModel.Playback_AutoPlaySoundOff = Videodto.Playback_AutoPlaySoundOff;
                targetingViewModel.Playback_ClickToPlay = Videodto.Playback_ClickToPlay;
                targetingViewModel.Playback_Undetermined = Videodto.Playback_Undetermined;
                targetingViewModel.Video_RewardedAds = Videodto.RewardedAds;
                targetingViewModel.RewardedAdOnly = Videodto.RewardedAdOnly;
                targetingViewModel.Video_MatchOrientation = Videodto.MatchOrientation;

            }
            else
            {


                targetingViewModel.PlacementType_InStream = true;
                targetingViewModel.PlacementType_OutStream = true;
                targetingViewModel.PlacementType_Interstitial = true;
                targetingViewModel.PlacementType_Undetermined = true;
                targetingViewModel.InStreamPosition_PreRoll = true;
                targetingViewModel.InStreamPosition_MidRoll = true;
                targetingViewModel.InStreamPosition_PostRoll = true;
                targetingViewModel.InStreamPosition_Undetermined = true;
                targetingViewModel.SkippableAds_SkippableAdSpaces = true;
                targetingViewModel.SkippableAds_NonSkippableAdSpaces = true;
                targetingViewModel.SkippableAds_Undetermined = true;
                targetingViewModel.Playback_AutoPlaySoundOn = true;
                targetingViewModel.Playback_AutoPlaySoundOff = true;
                targetingViewModel.Playback_ClickToPlay = true;
                targetingViewModel.Playback_Undetermined = true;
                targetingViewModel.Video_RewardedAds = true;
                targetingViewModel.Video_MatchOrientation = false;



            }
            #endregion
            #region  Conversion

            targetingViewModel.AdEvents =_campaignService.GetAdGroupTrackingEvents(new AdGroupTrackingEventCriteriaDto { CampaignId= targetingViewModel.CampaignId, LoadDetaultTrackingEvents = targetingViewModel.LoadDetaultTrackingEvents, AdGroupId= targetingViewModel.AdGroupId, CostModelWrapperId= targetingViewModel.CostModelWrapperId.HasValue? targetingViewModel.CostModelWrapperId.Value:0 });
            targetingViewModel.ConversionEvents= _campaignService.GetAccountConversionEvents();
            #endregion

         

           
        }
        protected Services.Interfaces.DTOs.Campaign.Targeting.TargetingSaveDto GetTargetingSaveDto(TargetingSaveModel targetingSaveModel)
        {

            //int? deviceTypeId = new int?();
            //if (targetingSaveModel.DeviceType != null)
            //{
            //    if (targetingSaveModel.DeviceType.Count() != 2)
            //    {
            //        deviceTypeId = targetingSaveModel.DeviceType.First();
            //    }
            //}
            var ser = new JavaScriptSerializer();

            var targetingSaveDto = new Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting.TargetingSaveDto
            {
                AdGroupId = targetingSaveModel.AdGroupId,
                CampaignId = targetingSaveModel.CampaignId,
                DeviceTargetingTypeId = targetingSaveModel.DeviceTargetingTypeId,
                AgeGroupId = targetingSaveModel.AgeGroupId,
                Budget = targetingSaveModel.Budget,
                DailyBudget = targetingSaveModel.DailyBudget,
                AllowInclude = targetingSaveModel.AllowInclude,
                Gender = targetingSaveModel.Gender,
                Bid = targetingSaveModel.Bid,
                AudianceDiscountPrice = targetingSaveModel.AudianceDiscountPrice,
                TrackInstalls = targetingSaveModel.TrackInstalls,
                changedAudiances = targetingSaveModel.changedAudiances,
                OpenInExternalBrowser = targetingSaveModel.OpenInExternalBrowser,
                LanguagesIds = targetingSaveModel.LanguageType,
                AllowOpenAuction = targetingSaveModel.AllowOpenAuction,
                group = targetingSaveModel.group,
                BiddingStrategy = (BiddingStrategy)targetingSaveModel.BiddingStrategy,
                AdPosition_Unknown = targetingSaveModel.AdPosition_Unknown,
                AdPosition_AboveTheFold = targetingSaveModel.AdPosition_AboveTheFold,
                AdPosition_BelowTheFold = targetingSaveModel.AdPosition_BelowTheFold,
                AdPosition_Enabled = targetingSaveModel.AdPosition_Enabled,
                ViewabilityVendorId = targetingSaveModel.ViewabilityVendorId,

                groupAudianceString = targetingSaveModel.groupAudianceString,

                CostModelWrapper = targetingSaveModel.CostModelWrapper,
                DisableProxyTraffic = targetingSaveModel.DisableProxyTraffic,
                IsWifi = targetingSaveModel.IsWifi,
                TargetingConnectionType = targetingSaveModel.TargetingConnectionType,
                IsCellular = targetingSaveModel.IsCellular,

                OperatorTargetingIsAll = targetingSaveModel.OperatorTargetingIsAll,
                DeviceTypeIds = targetingSaveModel.DeviceType,

                PlacementType_InStream = targetingSaveModel.PlacementType_InStream,
                PlacementType_OutStream = targetingSaveModel.PlacementType_OutStream,
                PlacementType_Interstitial = targetingSaveModel.PlacementType_Interstitial,
                PlacementType_Undetermined = targetingSaveModel.PlacementType_Undetermined,
                InStreamPosition_PreRoll = targetingSaveModel.InStreamPosition_PreRoll,
                InStreamPosition_MidRoll = targetingSaveModel.InStreamPosition_MidRoll,
                InStreamPosition_PostRoll = targetingSaveModel.InStreamPosition_PostRoll,
                InStreamPosition_Undetermined = targetingSaveModel.InStreamPosition_Undetermined,
                SkippableAds_SkippableAdSpaces = targetingSaveModel.SkippableAds_SkippableAdSpaces,
                SkippableAds_NonSkippableAdSpaces = targetingSaveModel.SkippableAds_NonSkippableAdSpaces,
                SkippableAds_Undetermined = targetingSaveModel.SkippableAds_Undetermined,
                Playback_AutoPlaySoundOn = targetingSaveModel.Playback_AutoPlaySoundOn,
                Playback_AutoPlaySoundOff = targetingSaveModel.Playback_AutoPlaySoundOff,
                Playback_ClickToPlay = targetingSaveModel.Playback_ClickToPlay,
                Playback_Undetermined = targetingSaveModel.Playback_Undetermined,
                Video_RewardedAds = targetingSaveModel.Video_RewardedAds,
                RewardedAdOnly = targetingSaveModel.RewardedAdOnly,
              
                Video_MatchOrientation = targetingSaveModel.Video_MatchOrientation,

            };



            if (targetingSaveModel.AdEventItems != null)
            {
                targetingSaveDto.AdEventItems = targetingSaveModel.AdEventItems.Where(M => M.IsDeleted == false).ToList();
            }
            else
            {
                targetingSaveDto.AdEventItems = new List<AdGroupTrackingEventDto>();


            }
            if (targetingSaveModel.ConversionItems != null)
            {
                targetingSaveDto.ConversionItems = targetingSaveModel.ConversionItems.Where(M => M.IsDeleted == false).ToList();
            }

            else
            {

                targetingSaveDto.ConversionItems = new List<AdGroupConversionEventDto>();
            }
            targetingSaveDto.BidOptimizationValue = targetingSaveModel.BidOptimizationValue;

            targetingSaveDto.MaxBidPrice = targetingSaveModel.MaxBidPrice;

            targetingSaveDto.KeepBiddingAtMinimum = targetingSaveModel.KeepBiddingAtMinimum;

            targetingSaveDto.BidOptimizationType = targetingSaveModel.BidOptimizationType;




            targetingSaveDto.FeesAddList = targetingSaveModel.FeesAddList;
            // get serializer object

            #region Tracking Events
            if (!string.IsNullOrWhiteSpace(targetingSaveModel.DeletedTrackingEvents))
            {
                var trackingEventsRangeStr = targetingSaveModel.DeletedTrackingEvents.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var trackingEventsCodeRangeStr = targetingSaveModel.DeletedTrackingCodeEvents.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedTrackingEvents = trackingEventsRangeStr.Select(trackingEvent => Convert.ToInt32(trackingEvent)).Distinct().ToArray();
                targetingSaveDto.DeletedTrackingCodeEvents = trackingEventsCodeRangeStr.Select(trackingEvent => Convert.ToString(trackingEvent)).Distinct().ToArray();
            }
            else
            {
                targetingSaveDto.DeletedTrackingEvents = new List<int>();
                targetingSaveDto.DeletedTrackingCodeEvents = new List<string>();
            }

            IList<AdGroupTrackingEventSaveModel> trackingEventsDto = ser.Deserialize<IList<AdGroupTrackingEventSaveModel>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedTrackingEvents) ? "[]" : targetingSaveModel.InsertedTrackingEvents);

            if (trackingEventsDto != null)
            {
                targetingSaveDto.InsertedTrackingEvents = new List<AdGroupTrackingEventSaveDto>();

                foreach (var item in trackingEventsDto.Reverse())
                {
                    targetingSaveDto.InsertedTrackingEvents.Add(new AdGroupTrackingEventSaveDto()
                    {
                        Code = item.Code,
                        Description = item.Description,
                        IsBillable = item.IsBillable,
                        AllowDuplicate = item.AllowDuplicate,
                        AllPreRequisitesRequired = item.AllPreRequisitesRequired,
                        PreRequisites = item.PreRequisitesList,
                       // ValidFor = item.ValidFor,
                        SegmentsId = item.SegmentsId,
                        SegmentString = item.SegmentString

                    });
                }
            }

            #endregion

            #region CostElements

            if (!string.IsNullOrWhiteSpace(targetingSaveModel.DeletedCostElements))
            {
                var costElementsStr = targetingSaveModel.DeletedCostElements.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedCostElements = costElementsStr.Select(costelement => Convert.ToInt32(costelement)).Distinct().ToArray();
            }
            else
            {
                targetingSaveDto.DeletedCostElements = new List<int>();
            }

            IList<AdGroupCostElementSaveDto> costElementsDto = ser.Deserialize<IList<AdGroupCostElementSaveDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedCostElements) ? "[]" : targetingSaveModel.InsertedCostElements);

            if (costElementsDto != null)
            {
                targetingSaveDto.InsertedCostElements = new List<AdGroupCostElementSaveDto>();

                targetingSaveDto.InsertedCostElements.AddRange(costElementsDto.Reverse());
            }

            if (!string.IsNullOrEmpty(targetingSaveModel.UpdatedCostElements))
            {
                Dictionary<int, decimal> updatedCostElementsValues = ser.Deserialize<IList<AdGroupCostElementDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.UpdatedCostElements) ? "[]" : targetingSaveModel.UpdatedCostElements)
                                                                    .ToDictionary(p => p.ID, p => p.Value);

                targetingSaveDto.UpdatedCostElements = updatedCostElementsValues;
            }

            #endregion

            #region Keywords
            //get deleted keywords
            if (targetingSaveModel.DeletedKeywords != null)
            {
                targetingSaveDto.DeletedKeywords = targetingSaveModel.DeletedKeywords.Distinct().ToArray();
            }
            //get new keywords
            if (targetingSaveModel.NewKeywords != null)
            {
                targetingSaveDto.NewKeywords = targetingSaveModel.NewKeywords.Distinct().ToArray();
            }

           
                targetingSaveDto.ExcludeSensitiveCategories = targetingSaveModel.ExcludeSensitiveCategories;
         
            #endregion

            #region Geographies
            //if not all locations
            if (targetingSaveModel.GeographicTargetingIsAll != 1)
            {
                if (targetingSaveModel.Geographies != null)
                {
                    var geographiesStr = targetingSaveModel.Geographies.Split(_separator.ToCharArray(),
                                                                              StringSplitOptions.RemoveEmptyEntries);
                    var locationIds = geographiesStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToList();

                    var countries = _locationService.GetAll().Where(p => p.Type == Services.Interfaces.DTOs.Core.LocationType.Country);

                    foreach (var country in countries)
                    {
                        if (country.Locations != null && country.Locations.Count() != 0)
                        {
                            var currentCountryLocations = country.Locations.Where(p => locationIds.Contains(p.ID)).Select(p => p.ID).ToList();

                            if (currentCountryLocations.Count == country.Locations.Count())
                            {
                                locationIds.RemoveAll(p => currentCountryLocations.Contains(p));
                                locationIds.Add(country.ID);
                            }
                        }
                    }

                    targetingSaveDto.Geographies = locationIds.ToArray();
                }
            }

            #endregion
            #region Operators
            switch (targetingSaveModel.OperatorTargetingIsAll)
            {
                case 1://All Operators
                    {
                        break;
                    }
                case 2://specific Operators
                    {
                        //if 
                        if ((targetingSaveModel.Operators != null))
                        {
                            var operatorsStr = targetingSaveModel.Operators.Split(_separator.ToCharArray(),
                                                                                  StringSplitOptions.RemoveEmptyEntries);
                            targetingSaveDto.Operators =
                                operatorsStr.Select(_operator => Convert.ToInt32(_operator)).Distinct().ToArray();
                        }
                        break;
                    }
                case 3://WIFI Only
                    {
                        targetingSaveDto.Operators = new int[] { Config.WIFIOperaterId };
                        break;
                    }
                case 4://IP Ranges
                    {
                        targetingSaveDto.Operators = new int[] { };
                        break;
                    }
            }
            #endregion

            #region Devices
            //if not all devices
            if (targetingSaveDto.DeviceTargetingTypeId > 0)
            {
                if ((targetingSaveDto.DeviceTargetingTypeId == 1) && (targetingSaveModel.Platforms != null))
                {
                    var platformsStr = targetingSaveModel.Platforms.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    var versionsDictionary = new List<KeyValuePair<int, string>>();

                    if (targetingSaveModel.PlatformVersions != null)
                    {
                        versionsDictionary = (from platformVersions in targetingSaveModel.PlatformVersions.Where(p => !string.IsNullOrEmpty(p))
                                              let version = platformVersions.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                                              select new KeyValuePair<int, string>(int.Parse(version[0]), version[1])).ToList();
                    }

                    targetingSaveDto.Platforms = new Dictionary<int, string>();

                    foreach (var item in platformsStr.Select(platform => Convert.ToInt32(platform)).Distinct().ToList())
                    {
                        int platformId = item;

                        var version = versionsDictionary.Where(p => p.Key == item).SingleOrDefault();

                        targetingSaveDto.Platforms.Add(item, version.Equals(default(KeyValuePair<int, string>)) ? null : version.Value);
                    }

                }

                if ((targetingSaveDto.DeviceTargetingTypeId == 2) && (targetingSaveModel.Manufacturers != null))
                {
                    var manufacturersStr = targetingSaveModel.Manufacturers.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    targetingSaveDto.Manufacturers = manufacturersStr.Select(manufacturer => Convert.ToInt32(manufacturer)).Distinct().ToList();
                }
                if ((targetingSaveDto.DeviceTargetingTypeId == 3) && (targetingSaveModel.ModelId != null) && (targetingSaveModel.ModelId.Length > 0))
                {
                    targetingSaveDto.Models = targetingSaveModel.ModelId;
                }
                if (targetingSaveDto.DeviceTargetingTypeId == 4)
                {
                    if (targetingSaveModel.Models != null)
                    {
                        var modelsStr = targetingSaveModel.Models.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        targetingSaveDto.Models = modelsStr.Select(model => Convert.ToInt32(model)).Distinct().ToArray();
                    }
                    //if (targetingSaveModel.Manufacturers != null)
                    //{
                    //    var manufacturersStr = targetingSaveModel.Manufacturers.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //    targetingSaveDto.Manufacturers = manufacturersStr.Select(manufacturer => Convert.ToInt32(manufacturer)).Distinct().ToList();
                    //}

                    //if (targetingSaveModel.Platforms == null)
                    //{
                    //    var targetingListDto = _campaignService.GetTargeting(targetingSaveDto.CampaignId, targetingSaveDto.AdGroupId);
                    //    var actionTypeConstraint = targetingListDto.AdActionTypeDto != null ? targetingListDto.AdActionTypeDto.Constraints : null;

                    //    if (actionTypeConstraint != null && actionTypeConstraint.Count > 0)
                    //    {
                    //        //ALID
                    //        int[] platformsIds = actionTypeConstraint.Select(x => x.Platform.ID).ToArray();
                    //        targetingSaveModel.Platforms = string.Join(_separator, platformsIds.Select(x => x.ToString()));//  actionTypeConstraint.Select(x => x.Platform.ID.ToString() + _separator.ToCharArray()).ToString();
                    //    }

                    //}

                    //if (targetingSaveModel.Platforms != null)
                    //{
                    //    targetingSaveDto.Platforms = new Dictionary<int, string>();
                    //    var platformsStr = targetingSaveModel.Platforms.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //    var versionsDictionary = (from platformVersions in targetingSaveModel.PlatformVersions.Where(p => !string.IsNullOrEmpty(p))
                    //                              let version = platformVersions.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                    //                              select new KeyValuePair<int, string>(int.Parse(version[0]), version[1])).ToList();


                    //    foreach (var item in platformsStr.Select(platform => Convert.ToInt32(platform)).Distinct().ToList())
                    //    {
                    //        string version = versionsDictionary.Where(p => p.Key == item).SingleOrDefault().Equals(default(KeyValuePair<int, string>)) ? null : versionsDictionary.Where(p => p.Key == item).SingleOrDefault().Value;
                    //        targetingSaveDto.Platforms.Add(item, version);
                    //    }

                    //}

                    var platfromTree = ser.Deserialize<IList<PlatfromTree>>(string.IsNullOrWhiteSpace(targetingSaveModel.platfromTree) ? "[]" : targetingSaveModel.platfromTree);
                    if (platfromTree != null)
                    {

                        targetingSaveDto.platfromTree = platfromTree;

                    }
                    else
                    {

                        throw new InvalidOperationException("platfrom must not be empty");

                    }


                    targetingSaveDto.Platforms = new Dictionary<int, string>();
                    var platformsStr = platfromTree.Select(x => x.Id).ToList();

                    var versionsDictionary = (from platformVersions in targetingSaveModel.PlatformVersions.Where(p => !string.IsNullOrEmpty(p))
                                              let version = platformVersions.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                                              select new KeyValuePair<int, string>(int.Parse(version[0]), version[1])).ToList();


                    foreach (var item in platformsStr.Select(platform => Convert.ToInt32(platform)).Distinct().ToList())
                    {
                        string version = versionsDictionary.Where(p => p.Key == item).SingleOrDefault().Equals(default(KeyValuePair<int, string>)) ? null : versionsDictionary.Where(p => p.Key == item).SingleOrDefault().Value;
                        targetingSaveDto.Platforms.Add(item, version);
                    }


                }
                if ((targetingSaveDto.DeviceTargetingTypeId == 5) && (targetingSaveModel.DeviceCapabilities != null))
                {
                    var capabilitiesStr = targetingSaveModel.DeviceCapabilities.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    targetingSaveDto.DeviceCapabilities = capabilitiesStr.Select(capability => Convert.ToInt32(capability)).Distinct().ToArray();
                }

            }
            if ((targetingSaveModel.ExcludeDeviceCapability != null))
            {
                var capabilitiesStr = targetingSaveModel.ExcludeDeviceCapability.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.ExcludeDeviceCapability = capabilitiesStr.Select(capability => Convert.ToInt32(capability)).Distinct().ToArray();
            }

            #endregion

            #region IP Ranges

            // 
            if (!string.IsNullOrWhiteSpace(targetingSaveModel.DeletedIPRanges))
            {
                var ipRangesStr = targetingSaveModel.DeletedIPRanges.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedIPRanges = ipRangesStr.Select(ipRange => Convert.ToInt32(ipRange)).Distinct().ToArray();
            }
            else
            {
                targetingSaveDto.DeletedIPRanges = new List<int>();
            }
            targetingSaveDto.InsertedIPRanges = ser.Deserialize<IList<IPTargetingDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedIPRanges) ? "[]" : targetingSaveModel.InsertedIPRanges);

            //read ip targeting file
            List<string> ipFileStr;
            var IPFile = Request.Files["IPFile"];
            if (IPFile != null && IPFile.ContentLength != 0)
            {
                using (var streamReader = new StreamReader(IPFile.InputStream))
                {
                    ipFileStr = streamReader.ReadToEnd().Split('\n').Select(x => x.Trim('\r')).ToList();
                    ipFileStr = ipFileStr.Where(x => x != "").ToList();
                    foreach (string ip in ipFileStr)
                    {
                        string[] startAndEnd = ip.Split(',').ToArray();
                        IPAddress ipAddress;
                        if (!IPAddress.TryParse(startAndEnd[0], out ipAddress) || !IPAddress.TryParse(startAndEnd[1], out ipAddress))
                        {
                            throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                        }

                        var ips = IpHelper.ParseIpRange(ip, true);
                        if (ips == null)
                        {
                            throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                        }



                    }
                    var fileIpTargetings = IpHelper.BuildIpRanges(ipFileStr)
                                                   .Select(x =>
                                                           new IPTargetingDto
                                                           {
                                                               StartRange = x.StartRangeString,
                                                               EndRange = x.EndRangeString
                                                           });
                    targetingSaveDto.InsertedIPRanges.AddRange(fileIpTargetings);

                }
            }

            #endregion

            #region GenFencing

            if (!string.IsNullOrWhiteSpace(targetingSaveModel.DeletedGeoFencing))
            {
                var geoFencingStr = targetingSaveModel.DeletedGeoFencing.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedGeoFencings = geoFencingStr.Select(ipRange => Convert.ToInt32(ipRange)).Distinct().ToArray();
            }
            else
            {
                targetingSaveDto.DeletedGeoFencings = new List<int>();
            }


            IList<GeoFencingUITargeting> geoFencingUIMapping = ser.Deserialize<IList<GeoFencingUITargeting>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedGeoFencing) ? "[]" : targetingSaveModel.InsertedGeoFencing);
            targetingSaveDto.InsertedGeoFencings = new List<GeoFencingTargetingDto>();

            foreach (var item in geoFencingUIMapping)
            {
                decimal latitude, longtitude, radious;
                if (decimal.TryParse(item.Latitude, out latitude) && decimal.TryParse(item.Longitude, out longtitude) && decimal.TryParse(item.Radius, out radious))
                {
                    if (radious > 0 && (longtitude <= 180 && longtitude >= -180) && (latitude <= 90 && latitude >= -90))
                    {
                        GeoFencingTargetingDto targetingDto = new GeoFencingTargetingDto()
                        {
                            Longitude = longtitude,
                            Latitude = latitude,
                            Radius = radious
                        };

                        targetingSaveDto.InsertedGeoFencings.Add(targetingDto);
                    }
                }
            }

            #endregion

            #region URLTargeting

            // 
            if (!string.IsNullOrWhiteSpace(targetingSaveModel.DeletedURLTargeting))
            {
                var urlRangesStr = targetingSaveModel.DeletedURLTargeting.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedURLTargeting = urlRangesStr.Select(urlTarget => Convert.ToInt32(urlTarget)).Distinct().ToArray();
            }
            else
            {
                targetingSaveDto.DeletedURLTargeting = new List<int>();
            }

            targetingSaveDto.InsertedURLTargeting = ser.Deserialize<IList<URLTargetingDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedURLTargeting) ? "[]" : targetingSaveModel.InsertedURLTargeting);


            #endregion

            #region Bid Config

            targetingSaveDto.InsertedBidConfigItems = ser.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.InserteCampaignBidConfigs) ? "[]" : targetingSaveModel.InserteCampaignBidConfigs);
            targetingSaveDto.UpdatedBidConfigItems = ser.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.UpdatedCampaignBidConfiges) ? "[]" : targetingSaveModel.UpdatedCampaignBidConfiges);
            // campaignBidConfigSaveDTo.DeletedCampaignBidConfigs = ser.Deserialize<IList<int>>(string.IsNullOrWhiteSpace(targetingSaveModel.DeletedCampaignBidConfigs) ? "[]" : targetingSaveModel.DeletedCampaignBidConfigs);
            targetingSaveDto.UpdatedNotCompatableCampaignBidConfiges = ser.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.UpdatedNotCompatableCampaignBidConfiges) ? "[]" : targetingSaveModel.UpdatedNotCompatableCampaignBidConfiges);

            if (!string.IsNullOrEmpty(targetingSaveModel.DeletedCampaignBidConfigs))
            {
                var deletedAssignedAppsitesAr = targetingSaveModel.DeletedCampaignBidConfigs.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedCampaignBidConfigs = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
            }

            #endregion
            #region Inventory Source

            targetingSaveDto.Runtype = targetingSaveModel.Runtype;
            List<string> IdsString = new List<string>();
            if (targetingSaveModel.SSPCheckedString != null)
            {
                IdsString = targetingSaveModel.SSPCheckedString.Split(',').Where(x => x != "" && x != ",").ToList();
            }
            List<int> ids = IdsString.Select(x => Convert.ToInt32(x)).ToList();

            targetingSaveDto.CheckedSSP = new List<int>();
            targetingSaveDto.CheckedSSP = ids;
            if (targetingSaveModel.CheckedSSP != null)
            {
                foreach (var chekSS in targetingSaveModel.CheckedSSP)
                {
                    targetingSaveDto.CheckedSSP.Add(chekSS);

                }
            }
            targetingSaveDto.InsertedInventorySourceItems = ser.Deserialize<IList<InventorySourceDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.InsertedInventorySources) ? "[]" : targetingSaveModel.InsertedInventorySources);
            targetingSaveDto.UpdatedInventorySourceItems = ser.Deserialize<IList<InventorySourceDto>>(string.IsNullOrWhiteSpace(targetingSaveModel.UpdatedInventorySources) ? "[]" : targetingSaveModel.UpdatedInventorySources);

            if (!string.IsNullOrEmpty(targetingSaveModel.DeletedInventorySources))
            {
                var deletedAssignedAppsitesAr = targetingSaveModel.DeletedInventorySources.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                targetingSaveDto.DeletedInventoryItemsSources = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
            }

            #endregion
            #region PMPDeal
            targetingSaveDto.InsertePMPDealConfigs = targetingSaveModel.InsertePMPDealConfigs;
            targetingSaveDto.DeletedPMPDealConfigs = targetingSaveModel.DeletedPMPDealConfigs;
            #endregion

            #region Master List
            targetingSaveDto.InserteMasterListConfigs = targetingSaveModel.InserteMasterListConfigs;
            targetingSaveDto.DeletedMasterListConfigs = targetingSaveModel.DeletedMasterListConfigs;
            #endregion

            #region  Conversion Items

            targetingSaveDto.ConversionSetting = targetingSaveModel.ConversionSetting;
            targetingSaveDto.ConversionType = targetingSaveModel.ConversionType;
            targetingSaveDto.ViewAttribuation = targetingSaveModel.ViewAttribuation;
            targetingSaveDto.ClickAttribuation = targetingSaveModel.ClickAttribuation;
            targetingSaveDto.CountingAttribuation = targetingSaveModel.CountingAttribuation;
            targetingSaveDto.CountingTypeAttribuation = targetingSaveModel.CountingTypeAttribuation;


            #endregion
            return targetingSaveDto;
        }
        protected BidDto GetBidDto(TargetingSaveModel targetingSaveModel, BidGetModel bidGetModel, string separator = _bidSeparator)
        {
            var bidDto = new BidDto
            {
                DeviceTargetingTypeId = bidGetModel.DeviceTargetingTypeId,
                ActionType = bidGetModel.ActionTypeId,
                Demographic = bidGetModel.Demographic,
                AdTypeId = bidGetModel.AdTypeId
            };


            if (bidGetModel.Keywords != null)
            {
                bidDto.Keywords = bidGetModel.Keywords.Select(keyword => Convert.ToInt32(keyword)).Distinct().ToArray();
            }

            if (((targetingSaveModel == null) || (targetingSaveModel.GeographicTargetingIsAll != 1)) && (bidGetModel.Geographies != null))
            {
                var geographiesStr = bidGetModel.Geographies.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bidDto.Geographies = geographiesStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToArray();
            }

            if ((bidDto.DeviceTargetingTypeId != 2) && (bidGetModel.Platforms != null))
            {
                var platformsStr = bidGetModel.Platforms.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bidDto.Platforms = platformsStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToArray();
            }

            if ((bidDto.DeviceTargetingTypeId != 1) && (bidGetModel.Manufacturers != null))
            {
                var manufacturersStr = bidGetModel.Manufacturers.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bidDto.Manufacturers = manufacturersStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToArray();
            }

            if (((targetingSaveModel == null) || (targetingSaveModel.OperatorTargetingIsAll != 1)) && (bidGetModel.Operators != null))
            {
                var oeratorsStr = bidGetModel.Operators.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bidDto.Operators = oeratorsStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToArray();
            }

            if ((bidDto.DeviceTargetingTypeId == 5) && (bidGetModel.DeviceCapabilities != null))
            {
                var capabilitiesStr = bidGetModel.DeviceCapabilities.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bidDto.DeviceCapabilities = capabilitiesStr.Select(geography => Convert.ToInt32(geography)).Distinct().ToArray();
            }
            return bidDto;
        }
        protected void GetBidGetModel(BidGetModel bidGetModel, TargetingSaveModel targetingSaveModel)
        {
            //get the Keywords
            var Keywords = new List<string>();
            //foreach (var newKeyword in targetingSaveModel.NewKeywords)
            //{
            //    int keywordid = 0;
            //    if(Int32.TryParse(newKeyword, out keywordid))
            //    {
            //        Keywords.Add(keywordid.ToString());
            //    }
            //}

            //Demographic
            if ((targetingSaveModel.AgeGroupId > 0) || (targetingSaveModel.Gender > 0))
            {
                bidGetModel.Demographic = 1;
            }
            //ActionTypeId
            bidGetModel.Keywords = Keywords.ToArray();
            bidGetModel.DeviceTargetingTypeId = targetingSaveModel.DeviceTargetingTypeId;
        }
        protected TargetingViewModel GetTargetingViewModel(int campaignId, int adGroupId)
        {

            // Get Targeting Info
            var targetingListDto = _campaignService.GetTargeting(campaignId, adGroupId);

            //Load Device Capabilities
            var deviceCapabilities = _deviceCapabilityService.GetAll();

            var platforms = new List<PlatformDto>(_platformService.GetAll().Select(p => p.ShallowCopy()));

            //Load Age Groups List
            var optionalItem = new SelectListItem { Value = "0", Text = ResourcesUtilities.GetResource("All", "Global") };
            var ageGroupDtos = _ageGroupService.GetAll().ToList();
            var ageGroups = new List<SelectListItem> { optionalItem };
            ageGroups.AddRange(ageGroupDtos.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));
            var DPExternalProvidersList = _partyService.GetAllExternalDPPartner(campaignId);
            var DPInternalProvidersList = _partyService.GetAllInternalDPPartner(campaignId);

            var optionalItemChooseOne = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };



            var ExternalDataProviders = new List<SelectListItem> { optionalItemChooseOne };
            ExternalDataProviders.AddRange(DPExternalProvidersList.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));


            //Load Cost Models List
            var costModelWrappersList = _CostModelWrapperService.GetAll();
            var costModelWrappers = new List<SelectListItem>();

            // var adGroupSettings = _campaignService.GetAdGroupSettings(campaignId, adGroupId);

             

            // If campaign costmodelwrapper has been determiend
            if (targetingListDto.CampaignCostModelWrapper.HasValue)
            {
                CostModelWrapperDto costModelWrapper = costModelWrappersList.Where(p => p.ID == targetingListDto.CampaignCostModelWrapper.Value).Single();

                costModelWrappers.Add(new SelectListItem() { Value = costModelWrapper.ID.ToString(), Text = costModelWrapper.Name.ToString() });
            }
            else
            {
                // Filter CostModelWrappers base on AdActionType allowed CostModelWrappers
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)AccountRole.DSP || (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount != null && OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.AccountRole == (int)AccountRole.DSP))
                {

                    targetingListDto.AdActionTypeDto.AdActionCostModelWrappers = targetingListDto.AdActionTypeDto.AdActionCostModelWrappers.Where(M => M.Scope == AppScope.DSP || M.Scope == AppScope.Both).ToList();
                }
                else

                {
                    targetingListDto.AdActionTypeDto.AdActionCostModelWrappers = targetingListDto.AdActionTypeDto.AdActionCostModelWrappers.Where(M => M.Scope == AppScope.Network || M.Scope == AppScope.Both).ToList();

                }
                costModelWrappers.AddRange(costModelWrappersList.Where(p => targetingListDto.AdActionTypeDto.AdActionCostModelWrappers.Any(M => M.CostModelWrapperId == p.ID)).Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name.ToString() }));
            }

            // Malik Hassan: This code should be enhanced to be configured from database
            int cppvId = Convert.ToInt32(CostModelWrapperEnum.CPPV);
            if (!Config.IsAdministrationApp)
            {
                if (costModelWrappers.Any(p => p.Value == cppvId.ToString()))
                {
                    if (targetingListDto.CostModelWrapper != cppvId)
                    {
                        var cppvCostModelWrapper = costModelWrappers.Where(p => p.Value == cppvId.ToString()).Single();
                        costModelWrappers.Remove(cppvCostModelWrapper);
                    }
                }

                int cpiId = Convert.ToInt32(CostModelWrapperEnum.CPI);

                int cpaId = Convert.ToInt32(CostModelWrapperEnum.CPA);



                if (costModelWrappers.Any(p => p.Value == cpiId.ToString()))
                {
                    if (targetingListDto.CostModelWrapper != cpiId)
                    {
                        var cppvCostModelWrapper = costModelWrappers.Where(p => p.Value == cpiId.ToString()).Single();
                        costModelWrappers.Remove(cppvCostModelWrapper);
                    }
                }


                if (costModelWrappers.Any(p => p.Value == cpaId.ToString()))
                {
                    if (targetingListDto.CostModelWrapper != cpaId)
                    {
                        var cppvCostModelWrapper = costModelWrappers.Where(p => p.Value == cpaId.ToString()).Single();
                        costModelWrappers.Remove(cppvCostModelWrapper);
                    }
                }
            }

            //var languages=_languageService.GetAll();
            var languagesItems = new List<SelectListItem>();


            var costmodel = costModelWrappers.FirstOrDefault(costModel => costModel.Value == ((int)targetingListDto.CostModelWrapper).ToString());
            if (costmodel != null)
            {
                costmodel.Selected = true;
            }

            var keywordauto = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Kewords_Name",
                Name = "Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged"
            };
            //get the Keyword Tag Cloud
            //TODo: Osaleh to get item count from Configuration setting
            var keywords = _keywordService.GetTop(null);
            var keywordTags = keywords.Select(keywordDto => new TagCloud() { Id = keywordDto.ID, DispalValue = keywordDto.Name.ToString(), Rank = keywordDto.Rank }).ToList();




            var Targetings = targetingListDto.Targeting;
            // ALID
            var actionTypeConstraint = targetingListDto.AdActionTypeDto != null ? targetingListDto.AdActionTypeDto.Constraints : null;

            var selectedDeviceTypes = new List<int>();
            if (targetingListDto.Targeting.OfType<DeviceTargetingDto>().Count() == 0 || targetingListDto.Targeting.OfType<DeviceTargetingDto>().FirstOrDefault().DeviceTypes.Count() == 0)
            {
                selectedDeviceTypes = new List<int> { (int)DeviceTypeEnum.SmartPhone, (int)DeviceTypeEnum.Tablet };
            }
            else
            {
                if (targetingListDto.Targeting.OfType<DeviceTargetingDto>().ToList().Count() > 1 && targetingListDto.Targeting.OfType<DeviceTargetingDto>().FirstOrDefault().DeviceTypes.First().ID == (int)DeviceTypeEnum.Any)
                {
                    selectedDeviceTypes = new List<int> { (int)DeviceTypeEnum.SmartPhone, (int)DeviceTypeEnum.Tablet };
                }
                else
                {
                    selectedDeviceTypes = targetingListDto.Targeting.OfType<DeviceTargetingDto>().FirstOrDefault().DeviceTypes.Select(p => p.ID).ToList();
                }
            }
            var selectedLanguages = new List<int>();
            //if (targetingListDto.Targeting.OfType<LanguageTargetingDto>().Count() == 0)
            //{
            //    selectedLanguages = languages.Select(P=>P.Id).ToList<int>();
            //}
            //  else
            //{
            if (targetingListDto.Targeting.OfType<LanguageTargetingDto>().ToList().Count() > 0)
            {
                selectedLanguages = targetingListDto.Targeting.OfType<LanguageTargetingDto>().ToList().Select(P => P.Language.ID).ToList();
            }
            //else
            //{
            //    selectedLanguages = languages.Select(P => P.Id).ToList<int>(); 
            //}
            // }


            /* foreach (var item in languages)
             {
                 languagesItems.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name.Value, Selected = selectedLanguages.Contains(item.ID) });

             }*/

            // Load all device types
            var deviceTypes = new List<SelectListItem>
                                {
                                    new SelectListItem()
                                        {

                                            Text = ResourcesUtilities.GetResource("SmartPhone", "Campaign",new CultureInfo("en-US")),
                                            Value = ((int)DeviceTypeEnum.SmartPhone).ToString(),
                                            Selected = selectedDeviceTypes.Contains((int)DeviceTypeEnum.SmartPhone)
                                        },
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("Tablet", "Campaign",new CultureInfo("en-US")),
                                            Value = ((int)DeviceTypeEnum.Tablet).ToString(),
                                            Selected = selectedDeviceTypes.Contains((int)DeviceTypeEnum.Tablet)
                                        }
                                };

            var model = new TargetingViewModel
            {

                HasGeofencingTargeting = targetingListDto.AllowGeofencing,
                HasInternalDPPartner= DPInternalProvidersList!=null && DPInternalProvidersList.Count()>0,
                Bid = targetingListDto.Bid,
                ExcludeSensitiveCategories= targetingListDto.ExcludeSensitiveCategories,
                UniqueId = targetingListDto.UniqueId,
                Budget = targetingListDto.Budget,
                DailyBudget = targetingListDto.DailyBudget,
                DataBid = targetingListDto.DataBid,
                MaxDataBid = targetingListDto.MaxDataBid,
                AudianceDiscountPrice = targetingListDto.AudianceDiscountPrice,
                DiscountedBid = targetingListDto.DiscountedBid,
                DiscountDto = targetingListDto.DiscountDto,
                LoadDetaultTrackingEvents = targetingListDto.LoadDefaultsTrackingEvents,
                CostModelWrapperId = targetingListDto.CostModelWrapper,
                CampaignName = targetingListDto.CampaignName,
                AdvertiserName = targetingListDto.AdvertiserName,
                AdvertiserId = targetingListDto.AdvertiserId,
                FeesList=GetFeeList("fee", 0),
                FeesAddList= targetingListDto.FeesAdded.ToList(),
                AdvertiserAccountName = targetingListDto.AdvertiserAccountName,
                AdvertiserAccountId = targetingListDto.AdvertiserAccountId,
                AdPosition_Unknown= targetingListDto.AdPosition_Unknown,
                ViewabilityVendorId = targetingListDto.ViewabilityVendorId,
                AdPosition_AboveTheFold = targetingListDto.AdPosition_AboveTheFold,
                AdPosition_BelowTheFold = targetingListDto.AdPosition_BelowTheFold,
                AdPosition_Enabled = targetingListDto.AdPosition_Enabled,

                CountExternalAudienceList = targetingListDto.CountExternalAudienceList,
                DataPriceAudienceSegment = targetingListDto.DataPriceAudienceSegment,


                ExternalDataProvider = ExternalDataProviders,

                AllowOpenAuction = targetingListDto.AllowOpenAuction,
                group = targetingListDto.group,
                groupAudianceString = targetingListDto.groupAudianceString,
                AdGroupName = targetingListDto.AdGroupName,
                IsClientLocked = targetingListDto.IsClientLocked,
                IsClientReadOnly = targetingListDto.IsClientReadOnly,
                IsPricingModelChanged = targetingListDto.IsPricingModelChanged,
                IsHasAds = targetingListDto.IsHasAds,
                AdGroupId = adGroupId,
                CampaignId = campaignId,
                TrackInstalls = targetingListDto.TrackInstalls,
                OpenInExternalBrowser = targetingListDto.OpenInExternalBrowser,
                AdActionTypeId = targetingListDto.AdActionTypeDto != null ? targetingListDto.AdActionTypeDto.ID : -1,
                AdTypeId = targetingListDto.AdType != null ? targetingListDto.AdType.ID : new int?(),
                DemographicTargetingView = new DemographicTargetingViewModel { AgeGroups = ageGroups },
                CostModels = costModelWrappers,
                PMPDealConfigList = new List<PMPDealDto>(),
                MasterListConfigList = new List<AdvertiserAccountMasterAppSiteDto>(),

                KeywordTargetingViewModel = new KeywordTargetingViewModel()
                {
                    KeywordViewModel = new Noqoush.AdFalcon.Web.Controllers.Model.AppSite.KeywordViewModel()
                    {
                        Prefix = "",
                        KewordAuto = keywordauto,
                        KeywordTags = keywordTags,
                        AllowInsert = false,
                        AllowInclude= targetingListDto.AllowInclude,
                        ExcludeSensitiveCategories = targetingListDto.ExcludeSensitiveCategories
                    },
                     
                },
                OperaterTargetingView = new OperaterViewModel
                {
                    DisableProxyTraffic = targetingListDto.DisableProxyTraffic,
                    IsWifi = targetingListDto.IsWifi,
                    IsCellular = targetingListDto.IsCellular,
                    TargetingConnectionType = targetingListDto.TargetingConnectionType,
                    Operaters = new Model.Tree.TreeViewModel()
                    {
                        Url = Url.Action("GetTreeData", "Operator"),
                        Name = "Operators",
                        Id = "Operators",
                        IsAjax = true
                    }
                },
                Geographics = new GeographicViewModel()
                {
                    GeographicalAreas = new Model.Tree.TreeViewModel()
                    {
                        Url = Url.Action("GetTreeData", "Country"),
                        Name = "Geographies",
                        Id = "Geographies",
                        IsAjax = true
                    }
                },

                Audiences = new Model.Tree.TreeViewModel()
                {
                    Url = Url.Action("GetTreeData", "AudienceSegment", new { CampaignId = campaignId }),
                    Name = "Audiences",
                    Id = "Audiences",
                    IsAjax = true
                },

                DeviceTargetingView = new DeviceTargetingViewModel()
                {

                    DeviceCapabilities = deviceCapabilities.Select(item => (DeviceCapabilityDto)item.Clone()).ToList(),
                    DeviceTypeModel = actionTypeConstraint.Count == 0 || actionTypeConstraint.Where(x => x.DeviceConstraint == -1).Count() > 0 ? new DeviceTypeViewModel() { DeviceTypes = deviceTypes } : null,
                    Platforms = platforms,
                    Languages = languagesItems,
                    Manufacturers = new Model.Tree.TreeViewModel
                    {
                        Url = Url.Action("GetTreeData", "Manufacturer"),
                        Name = "Manufacturers",
                        Id = "Manufacturers",
                        IsAjax = true
                    },
                    DevicesTree = new Model.Tree.TreeViewModel()
                    {

                        Url = Url.Action("GetTreeData", "Device", new
                        {
                            campaignId = campaignId,
                            adGroupId = adGroupId
                        }),
                        Name = "Devices",
                        Id = "Devices",
                        IsAjax = true,
                        IsSubLevel = false
                    }
                }
            };
     
            //model.EpochValues = new List<ushort>();
            //model.EpochValues.Add(EpochDays(1));
            //model.EpochValues.Add(EpochDays(2));
            //model.EpochValues.Add(EpochDays(3));
            //model.EpochValues.Add(EpochDays(5));
            //model.EpochValues.Add(EpochDays(10));
            //model.EpochValues.Add(EpochDays(15));
            //model.EpochValues.Add(EpochDays(30));
            //model.EpochValues.Add(EpochDays(60));
            //model.EpochValues.Add(EpochDays(90));
            //model.EpochValues.Add(EpochDays(180));
            //model.EpochValues.Add(EpochDays(360));

          


            model.BiddingStrategy= (int)targetingListDto.BiddingStrategy;
            if (targetingListDto.MasterList != null)
                model.MasterListConfigList = targetingListDto.MasterList;
            ViewBag.IsVideoActionType = targetingListDto.IsVideoActionType;
            model.IsVideoActionType = targetingListDto.IsVideoActionType;
            model.DeviceTargetingView.LanguageType = selectedLanguages.ToArray();
            if (actionTypeConstraint != null && actionTypeConstraint.Count > 0)
            {
                model.DeviceTargetingView.Type = 4;
                bool selectAll = (model.Bid == 0);

                string deviceUrl = Url.Action("GetTreeData", "Device", new
                {
                    campaignId = campaignId,
                    adGroupId = adGroupId
                });

                foreach (var constraint in actionTypeConstraint)
                {
                    model.DeviceTargetingView.Platforms.Where(p => p.ID == constraint.Platform.ID).SingleOrDefault().IsSelected = true;
                }
                //if (selectedDeviceTypes.Count != 2)
                //{
                //    deviceUrl = Url.Action("GetTreeData", "Device", actionTypeConstraint);
                //}
                model.DeviceTargetingView.Devices = new Model.Tree.TreeViewModel
                {
                    Name = "Devices",
                    Url = deviceUrl,
                    Id = "Devices",
                    IsAjax = true,
                    //  IsSubLevel = actionTypeConstraint.DeviceConstraint < 0,
                    IsSelectAll = selectAll
                }
          ;
            }


            // CampaignBidConfigs
            //var campaignBidConfigDto = _campaignService.GetCampaignBidConfigs(campaignId, adGroupId);

            var ddealsandsources = targetingListDto.MultiSources;
            var adGroupSettings = ddealsandsources.AdGroupSettings;
            var campaignBidConfigDto = ddealsandsources.BidConfigs;
            var campaignBidConfigModel = new CampaignBidConfigModel
            {

                CampaignBidConfigList = campaignBidConfigDto.CampaignBidConfigDtos.ToList()
            };

            // InventorySources
            var InventorySourceDto = ddealsandsources.Sources;   /*_campaignService.GetInventorySources(campaignId, adGroupId)*/;
            string Runtype = "true";
            //if (InventorySourceDto.InventorySourceDtos != null && InventorySourceDto.InventorySourceDtos.Count > 0)
            //{
            //    Runtype = "false";

            //}

            if (targetingListDto.RunAllExchanges)
            {
                Runtype = "true";
            }
            else
            {
                Runtype = "false";

            }


            model.BidOptimizationValue = targetingListDto.BidOptimizationValue;

            model.MaxBidPrice = targetingListDto.MaxBidPrice;

            model.KeepBiddingAtMinimum = targetingListDto.KeepBiddingAtMinimum;

            model.BidOptimizationType =(int) targetingListDto.BidOptimizationType;
          model.ParentIdOfFirstParty=  _AdvertiserService.GetRootIdofFirstParty();

            model.ConversionSetting = targetingListDto.ConversionSetting;
            model.ConversionType = targetingListDto.ConversionType;
            model.ViewAttribuation = targetingListDto.ViewAttribuation;
            model.ClickAttribuation = targetingListDto.ClickAttribuation;
            model.CountingAttribuation = targetingListDto.CountingAttribuation;
            model.CountingTypeAttribuation = targetingListDto.CountingTypeAttribuation;

            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected =model.BidOptimizationType==0
                                              }};

            //lookupsList.Insert(0, lookupsList[0]);


            //okupsList.Add(new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") });

            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MaximizeCTR).ToString(), Text = BidOptimizationType.MaximizeCTR.ToText(), Selected = ((int)BidOptimizationType.MaximizeCTR) == (int)model.BidOptimizationType});
            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MinimizeCPC).ToString(), Text = BidOptimizationType.MinimizeCPC.ToText(), Selected = ((int)BidOptimizationType.MinimizeCPC) == (int)model.BidOptimizationType });
            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MinimizeCPA).ToString(), Text = BidOptimizationType.MinimizeCPA.ToText(), Selected = ((int)BidOptimizationType.MinimizeCPA) == (int)model.BidOptimizationType });

            if (model.IsVideoActionType)
            {
                lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MinimizeeCPVCV).ToString(), Text = BidOptimizationType.MinimizeeCPVCV.ToText(), Selected = ((int)BidOptimizationType.MinimizeeCPVCV) == (int)model.BidOptimizationType});
                lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MaximizeVCVR).ToString(), Text = BidOptimizationType.MaximizeVCVR.ToText(), Selected = ((int)BidOptimizationType.MaximizeVCVR) == (int)model.BidOptimizationType });

            }



            ViewData["BidOptimizationTypeList"] = lookupsList;
            IList<int> SSPList = new List<int>();
            if (InventorySourceDto != null)
            {
                SSPList = InventorySourceDto.InventorySourceDtos.Where(M => M.SSPId > 0).Select(M => M.SSPId).ToList().Distinct().ToList();
                InventorySourceDto.InventorySourceDtos = InventorySourceDto.InventorySourceDtos.Where(M => M.SubAppSiteId > 0).ToList();
            }
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria
            {

            });

            foreach (var SSpItem in SSPList)
            {
                var ResultItem = UsersListResult.Items.Where(M => M.Id == SSpItem).SingleOrDefault();

                if (ResultItem != null)
                {
                    ResultItem.ExchangeChecked = true;

                }
            }

            var InventorySourceModel = new Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal.InventorySourceModel

            {
                Runtype = Runtype,
                CheckedSSP = SSPList,
                SSPCheckedString = string.Join(",", SSPList),
                BusinessPartners = UsersListResult.Items.ToList(),
                InventorySourceList = InventorySourceDto.InventorySourceDtos.ToList()
            };

            model.InventorySourceModel = InventorySourceModel;
            var pmpDeals = ddealsandsources.Deals  /* _campaignService.GetPMPDealConfigConfigs(campaignId, adGroupId);*/ ;
            if (pmpDeals != null)
                model.PMPDealConfigList = pmpDeals;


            model.CampaignBidConfigModel = campaignBidConfigModel;
            model.AdGroupSettings = new AdGroupSettingsViewModel() { DailyBudget = adGroupSettings.DailyBudget, Budget = adGroupSettings.Budget, CampaigBudget = adGroupSettings.CampaignBudget };


            #region Conversion


            if (targetingListDto.AdEventItems != null)
            {
                model.AdEventItems = targetingListDto.AdEventItems;
                model.AdEventIndexItems = targetingListDto.AdEventItems.Count - 1;
            }
            else
            {

                model.AdEventItems = new List<AdGroupTrackingEventDto>();
                model.AdEventIndexItems = -1;

            }

            if (targetingListDto.ConversionItems != null)
            {
                model.ConversionItems = targetingListDto.ConversionItems;
                model.ConversionIndexItems = targetingListDto.ConversionItems.Count - 1;
            }
            else
            {

                model.ConversionItems = new List<AdGroupConversionEventDto>();
                model.ConversionIndexItems = -1;
            }
            #endregion
            if (model.CostModels != null && model.CostModels.FirstOrDefault() != null)
            {
                if (model.LoadDetaultTrackingEvents)
                {
                    int costModelD = 0;
                    int.TryParse(model.CostModels.First().Value, out costModelD);

                    _campaignService.AddDefaultAdGroupTrackingEventById(model.AdGroupId, costModelD, null);


                    model.LoadDetaultTrackingEvents = false;
                    model.CostModelWrapperId = costModelD;
                }
            }


            IList<MetricVendorDto> MetricVendors = _campaignService.getMetricVendors();


            model.MetricVendors = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = !model.ViewabilityVendorId.HasValue ||   model.ViewabilityVendorId==0
                                              }};
            model.MetricVendors.AddRange(
                MetricVendors.Select(
                    item => new SelectListItem()
                    {
                        Value = string.Format("{0}", item.ID.ToString()),
                        Text = item.Name.ToString(),
                        Selected = item.ID == model.ViewabilityVendorId
                    }));


            FillTargeting(model, Targetings);

            return model;
        }


        private  ushort EpochDays(int days)
        {

            DateTime utc = Noqoush.Framework.Utilities.Environment.GetServerTime();
            utc= utc.AddDays(-1*days);

            var epoch = utc - new DateTime(1970, 1, 1, 0, 0, 0);
                     


            return (ushort)((long)epoch.TotalSeconds / 86400);
        }

        public ActionResult GetAudiencOjects(string ids)
        {

            string[] a = ids.Split(',');
            List<AudienceSegmentViewModel> returnList = new List<AudienceSegmentViewModel>();

            foreach (string i in a)
            {
                if (i != "")
                {
                    var item = _audienceSegmentService.get(Convert.ToInt32(i));
                    AudienceSegmentViewModel Object = new AudienceSegmentViewModel
                    {
                        Name = item.Name.Value,
                        Price = item.Price,
                        Path = item.Path,
                        id = item.ID,
                        ParentId = item.ParentId
                    };
                    returnList.Add(Object);
                }
            }


            return Json(returnList,JsonRequestBehavior.AllowGet);

        }

        #endregion
        #region Actions


        [GridAction(EnableCustomBinding = true)]
        public ActionResult GridPMPDealConfigData(int id, int adGroupId)
        {
            var result = _campaignService.GetPMPDealConfigConfigs(id, adGroupId);
            return View(new GridModel
            {
                Data = result,
                Total = result != null ? result.Count : 0
            });
        }
        public ActionResult PMPDealConfigData(int id, int adGroupId)
        {
            var result = _campaignService.GetPMPDealConfigConfigs(id, adGroupId);


            return Json(new { PMPDealConfigList = result != null ? result.ToList() : new List<Services.Interfaces.DTOs.Account.PMP.PMPDealDto>() });
        }
        protected virtual string GetResourceKey(string resourceKey)
        {
            return "CampaignList";
        }
        public virtual ActionResult Targeting(int id, int adGroupId, bool returenFromAdsPage = false)
        {
            int campaignId = id;

            //if (!_campaignService.IsReadOrWriteCampaign(campaignId))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}

            var model = GetTargetingViewModel(campaignId, adGroupId);
            //if (model.AdvertiserAccountId > 0)
            //{
            //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(model.AdvertiserAccountId))
            //    {
            //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //    }
            //}
            try
            {
                if (!Config.IsAdmin)
                {
                    //send a massage to the user if the bid is zero , when trying to open the ads page.
                    if (model.Bid == 0 && returenFromAdsPage)
                    {
                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                        throw error;

                    }
                }
                if (!( model.CostModelWrapperId.Value>0) && model.FeesAddList!=null && model.FeesAddList.Count>0 && model.FeesAddList.Where(M=>M.ID>0).ToList().Count()>0)
                {
                   
                        AddMessages(ResourcesUtilities.GetResource("AddDefaultFee", "Global"), MessagesType.Warning);
                   // MoveMessagesTempData();
                }
            }

            catch (BusinessException exception)
            {
                foreach (var errorData in exception.Errors)
                {
                    AddMessages(errorData.Message, MessagesType.Error);
                }
            }
            var breadCrumbLinks = new List<BreadCrumbModel>();
            #region BreadCrumb
            if (!(model.AdvertiserAccountId > 0))
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                  Order = 4,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url=Url.Action("Groups",new {id= id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };
            }
            else
            {

                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                  Order = 6,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url=Url.Action("Groups",new {id= id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {AdvertiseraccId= model.AdvertiserAccountId,id=id})
                                              },

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index", new { AdvertiseraccId=model.AdvertiserAccountId}),
                                                  Order = 3,
                                              }

                                       ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2

                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Url=Url.Action("AccountAdvertisers"),
                                                  Order = 1,
                                                  ExtensionDropDown = true
                                              }
                                      };
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ChangeJavaScriptSet("targetingActionJs");

            ViewBag.AudienceAllowed = checkAdPermissions(PortalPermissionsCode.Audience);
            if (!(model.HasInternalDPPartner || (model.ExternalDataProvider!=null && model.ExternalDataProvider.Count()>0)))
            {
                ViewBag.AudienceAllowed = false;
            }

            ViewBag.PMPDealAllowed = checkAdPermissions(PortalPermissionsCode.PMPDeal);
            ViewBag.InventorySourcesAllowed = checkAdPermissions(PortalPermissionsCode.InventorySource);



            return View(model);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Targeting(TargetingSaveModel targetingSaveModel, BidGetModel bidGetModel, string returnUrl, bool IsContinue = false)
        {
            var exceptionHappen = false;

            if (ModelState.IsValid)
            {
                //dynamic jsonObj = null;
                //if (!string.IsNullOrEmpty(targetingSaveModel.groupAudianceString))
                //{


                //    jsonObj = new JavaScriptSerializer().Deserialize(targetingSaveModel.groupAudianceString, typeof(group));
                //    targetingSaveModel.group = jsonObj;
                //}
                try
                {
                    if (targetingSaveModel.OperatorTargetingIsAll == 4)
                    {
                        targetingSaveModel.Operators = "";
                        bidGetModel.Operators = "";
                    }
                    if (targetingSaveModel.DeviceTargetingTypeId != 0)
                    {
                        switch (targetingSaveModel.DeviceTargetingTypeId)
                        {
                            case 0:
                                {
                                    // isAll =>do nothing
                                    break;
                                }
                            case 1:
                                {
                                    // Platforms
                                    if (string.IsNullOrWhiteSpace(targetingSaveModel.Platforms) || targetingSaveModel.Platforms.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Count() == 0)
                                    {
                                        targetingSaveModel.DeviceTargetingTypeId = 0;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    // Manufacturers
                                    if (string.IsNullOrWhiteSpace(targetingSaveModel.Manufacturers))
                                    {
                                        targetingSaveModel.DeviceTargetingTypeId = 0;
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    // Models
                                    if ((targetingSaveModel.ModelId == null) || (targetingSaveModel.ModelId.Length == 0))
                                    {
                                        targetingSaveModel.DeviceTargetingTypeId = 0;
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    if ((string.IsNullOrWhiteSpace(targetingSaveModel.Models)) &&
                                        (string.IsNullOrWhiteSpace(targetingSaveModel.Manufacturers)) &&
                                        (string.IsNullOrWhiteSpace(targetingSaveModel.Platforms)))
                                    {
                                        targetingSaveModel.DeviceTargetingTypeId = 0;
                                    }

                                    break;
                                }
                            case 5:
                                {
                                    // Device Capabilities
                                    if (string.IsNullOrWhiteSpace(targetingSaveModel.DeviceCapabilities))
                                    {
                                        targetingSaveModel.DeviceTargetingTypeId = 0;
                                    }
                                    break;
                                }
                        }
                    }
                    GetBidGetModel(bidGetModel, targetingSaveModel);
                    var bid = GetBidDto(targetingSaveModel, bidGetModel, _separator);
                    var saveModel = GetTargetingSaveDto(targetingSaveModel);

                    if (targetingSaveModel.AllowBidToBeZero)
                    {
                        saveModel.Bid = 0;
                    }
                    saveModel.BinInfo = bid;
                 
                    var dtoResult = _campaignService.SaveTargeting(saveModel);
                    //_campaignService.SaveAdGroupTrackingEventsPrerequisites(saveModel.CampaignId, saveModel.AdGroupId, saveModel.InsertedTrackingEvents.ToDictionary(p => p.Code, p => p.PreRequisites));
                    if (dtoResult.PMPDealConfictAdType)
                    {
                        string massage = ResourcesUtilities.GetResource("PMPDealTargetingConfictAdType", "Global");


                        AddMessages(massage, MessagesType.Warning);
                    }
                    if (dtoResult.PMPDealConfictWithInventorySource)
                    {
                        string massage = ResourcesUtilities.GetResource("DealConflictInventorySource", "SSPPartner");


                        AddMessages(massage, MessagesType.Warning);
                    }


                    if (dtoResult.PMPDealConfictPrice)
                    {

                        AddMessages(ResourcesUtilities.GetResource("PMPDealTargetingConfictPrice", "Global"), MessagesType.Warning);
                    }

                    if (Config.IsAdministrationApp)
                    {
                        if (dtoResult.AddDefaultCostElement)
                        {

                            AddMessages(ResourcesUtilities.GetResource("AddDefaultCostElement", "Global"), MessagesType.Warning);
                        }
                    }

                    if (dtoResult.AddDefaultFee)
                    {

                        AddMessages(ResourcesUtilities.GetResource("AddDefaultFee", "Global"), MessagesType.Warning);
                    }
                    if (dtoResult.PMPDealConfictCountries)
                    {

                        AddMessages(ResourcesUtilities.GetResource("PMPDealTargetingConfictCountry", "Global"), MessagesType.Warning);
                    }

                    if (dtoResult.PMPDealInventorySourceConflicts)
                    {

                        AddMessages(ResourcesUtilities.GetResource("InventroySourceDealsConflict", "Global"), MessagesType.Warning);
                    }
                    if (dtoResult.InventroySourceAllowGeofencing)
                    {

                        AddMessages(ResourcesUtilities.GetResource("InventroySourceAllowGeofencing", "Global"), MessagesType.Warning);
                    }
                    if (dtoResult.AdminLessThanMinBid)
                    {

                        AddMessages(ResourcesUtilities.GetResource("MinBidErrorLess", "Global"), MessagesType.Warning);
                    }
                    if (dtoResult.DealAllowGeofencing)
                    {
                        string massage = ResourcesUtilities.GetResource("DealAllowGeofencingLess", "Global");
                        AddMessages(massage, MessagesType.Warning);
                    }
                    if (IsContinue)
                    {
                        return RedirectToAction("Creative", new { id = targetingSaveModel.CampaignId, adGroupId = targetingSaveModel.AdGroupId });
                    }
                    else
                    {
                        AddSuccessfullyMsg();
                        MoveMessagesTempData();
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Targeting",
                                                    new
                                                    {
                                                        id = targetingSaveModel.CampaignId,
                                                        adGroupId = targetingSaveModel.AdGroupId
                                                    });
                        }
                        else
                        {
                            return RedirectToAction("Targeting",
                                                    new
                                                    {
                                                        id = targetingSaveModel.CampaignId,
                                                        adGroupId = targetingSaveModel.AdGroupId,
                                                        returnUrl
                                                    });
                        }
                    }
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }

                    exceptionHappen = true;


                }

            }
            var model = GetTargetingViewModel(targetingSaveModel.CampaignId, targetingSaveModel.AdGroupId);
            if (exceptionHappen)
            { 
            var breadCrumbLinks = new List<BreadCrumbModel>();
                var id=model.CampaignId;
            #region BreadCrumb
                if (!(model.AdvertiserAccountId > 0))
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                  Order = 4,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url=Url.Action("Groups",new {id= id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };
            }
            else
            {

                breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                  Order = 6,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url=Url.Action("Groups",new {id= id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 4,
                                                  Url=Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {AdvertiseraccId= model.AdvertiserAccountId,id=id})
                                              },

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index", new { AdvertiseraccId=model.AdvertiserAccountId}),
                                                  Order = 3,
                                              }

                                       ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2

                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Url=Url.Action("AccountAdvertisers"),
                                                  Order = 1,
                                                  ExtensionDropDown = true
                                              }
                                      };
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
        }

            //var breadCrumbLinks = new List<BreadCrumbModel>();
            //#region BreadCrumb
            //if (!(model.AdvertiserId > 0))
            //{
            //    breadCrumbLinks = new List<BreadCrumbModel>
            //                          {
            //                              new BreadCrumbModel()
            //                                  {
            //                                      Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
            //                                      Order = 4,
            //                                  },
            //                               new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
            //                                      Order = 3,
            //                                      Url=Url.Action("Groups",new {id= id})
            //                                  },
            //                             new BreadCrumbModel()
            //                                  {
            //                                      Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
            //                                      Order = 2,
            //                                      Url=Url.Action("create",new {id= id})
            //                                  },
            //                           new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
            //                                      Url=Url.Action("Index"),
            //                                      Order = 1,
            //                                  }
            //                          };
            //}
            //else
            //{

            //    breadCrumbLinks = new List<BreadCrumbModel>
            //                          {
            //                              new BreadCrumbModel()
            //                                  {
            //                                      Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
            //                                      Order = 6,
            //                                  },
            //                               new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
            //                                      Order = 5,
            //                                      Url=Url.Action("Groups",new {id= id})
            //                                  },
            //                             new BreadCrumbModel()
            //                                  {
            //                                      Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
            //                                      Order = 4,
            //                                      Url=Url.Action("create",new {id= id})
            //                                  },

            //                           new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
            //                                      Url=Url.Action("Index", new { AdvertiserId=model.AdvertiserId}),
            //                                      Order = 3,
            //                                  }

            //                           ,
            //                              new BreadCrumbModel()
            //                                  {
            //                                      Text =model.AdvertiserName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
            //                                      Order = 2

            //                                  },
            //                           new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
            //                                      Url=Url.Action("AccountAdvertisers"),
            //                                      Order = 1,
            //                                  }
            //                          };
            //}

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion
            ViewBag.AudienceAllowed = checkAdPermissions(PortalPermissionsCode.Audience);
            if (!(model.HasInternalDPPartner || (model.ExternalDataProvider != null && model.ExternalDataProvider.Count() > 0)))
            {
                ViewBag.AudienceAllowed = false;
            }
            ViewBag.PMPDealAllowed = checkAdPermissions(PortalPermissionsCode.PMPDeal);
            ViewBag.InventorySourcesAllowed = checkAdPermissions(PortalPermissionsCode.InventorySource);


            ChangeJavaScriptSet("targetingActionJs");
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]

        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult SaveExternalAudianceSegmentTargeting(ExternalAudianceSegmentTargetingModel targetingSaveModel)
        {
            TargetingResultDto dtoResult = new TargetingResultDto();
            try
            {
                string result = string.Empty;
                if (targetingSaveModel.Segments != null && targetingSaveModel.Segments.Count > 0)
                    result = _campaignService.SaveSegmentsForTargeting(targetingSaveModel.AdGroupId, targetingSaveModel.AccAdvertiserId, targetingSaveModel.DataProviderId, targetingSaveModel.Segments);
                dtoResult = _campaignService.AddExternalAudSegmentTargeting(targetingSaveModel.AdGroupId, targetingSaveModel.AccAdvertiserId, targetingSaveModel.DataProviderId, targetingSaveModel.Segments, result);
                if (dtoResult.FireEvents)
                {
                    _campaignService.PublishTargetingEvent(dtoResult);
                }
                dtoResult.Result = true;
                dtoResult.Message = ResourcesUtilities.GetResource("ExportTargetingSegment", "Global");
            }
            catch (BusinessException exception)
            {

                dtoResult.Message = exception.Errors[0].Message;

            }

            return Json(dtoResult, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]

        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult SaveExternalAudianceSegmentTargetingWithDel(ExternalAudianceSegmentTargetingModel targetingSaveModel)
        {
            TargetingResultDto dtoResult = new TargetingResultDto();
            try
            {
                string result = string.Empty;
                if (targetingSaveModel.Segments != null && targetingSaveModel.Segments.Count > 0)
                    result = _campaignService.SaveSegmentsForTargetingForDel(targetingSaveModel.AdGroupId, targetingSaveModel.AccAdvertiserId, targetingSaveModel.DataProviderId, targetingSaveModel.Segments);
                //dtoResult = _campaignService.AddExternalAudSegmentTargeting(targetingSaveModel.AdGroupId, targetingSaveModel.AccAdvertiserId, targetingSaveModel.DataProviderId, targetingSaveModel.Segments, result);
                if (dtoResult.FireEvents)
                {
                    _campaignService.PublishTargetingEvent(dtoResult);
                }
                dtoResult.Result = true;
                dtoResult.Message = ResourcesUtilities.GetResource("ExportTargetingSegmentWithDel", "Global");
            }
            catch (BusinessException exception)
            {

                dtoResult.Message = exception.Errors[0].Message;

            }

            return Json(dtoResult, JsonRequestBehavior.AllowGet);
        }



        public virtual JsonResult GetBid(BidGetModel bidGetModel)
        {
            //TODO:Osaleh to remove this temp code
            var bid = _campaignService.GetMinBid(GetBidDto(null, bidGetModel));


            var jsonBidValues = JsonConvert.SerializeObject(bid.CostModelsWrappersBidValues);

            return Json(jsonBidValues, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public JsonResult AdGroupSettings(AdGroupSettingsDto settingsDto)
        {
            JsonResult result;
            try
            {
                _campaignService.SaveAdGroupSettings(settingsDto);
                result = new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
                return result;
            }
            catch (BusinessException exception)
            {
                result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet };
                foreach (var errorData in exception.Errors)
                {
                    result.Data += errorData.Message;
                }
                return result;
            }
        }

        #region AdGroupTrackingEvents

        public ActionResult TrackingEvents(int id, int adgroupId, bool loadDefaultTrackingEvents)
        {
            ViewData.Model = new AdGroupTrackingEventResultDto();
            ViewData["CampaignId"] = id;
            ViewData["AdGroupId"] = adgroupId;
            ViewData["MaxAdGroupTrackingEvents"] = Config.MaxAdGroupTrackingEvents;
            ViewData["LoadDefaultTrackingEvents"] = loadDefaultTrackingEvents;
            return PartialView("TrackingEvents/TrackingEvents");
        }



        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult TrackingEvent(int id, int adGroupId)
        {

            var trackingEventsList = _campaignService.GetAccountTrackingEvents();
            var trackingEventsNamesIdsList = trackingEventsList.Items.Select(p => new DropDownItem { Text = p.Description, Value = p.IsCustom.ToString() }).ToList();
            var trackingEventsCodesIdsList = trackingEventsList.Items.Select(p => new DropDownItem { Text = p.Code, Value = p.IsCustom.ToString() }).ToList();
            trackingEventsNamesIdsList.Insert(0, (new DropDownItem { Text = ResourcesUtilities.GetResource("Select", "Global"), Value = string.Empty }));
            trackingEventsCodesIdsList.Insert(0, (new DropDownItem { Text = ResourcesUtilities.GetResource("Select", "Global"), Value = string.Empty }));
            ViewData["trackingEventsNamesIdsList"] = trackingEventsNamesIdsList;
            ViewData["trackingEventsCodesIdsList"] = trackingEventsCodesIdsList;

            return PartialView("TrackingEvents/TrackingEvent");
        }

        public ActionResult GetAccountTrackingEvents()
        {
            var trackingEventsList = _campaignService.GetAccountTrackingEvents();

            return Json(trackingEventsList, JsonRequestBehavior.AllowGet);
          
        }
        public ActionResult GetAccountConversionEvents()
        {
            var trackingEventsList = _campaignService.GetAccountConversionEvents();

            return Json(trackingEventsList, JsonRequestBehavior.AllowGet);

        }


        //[DenyRole(Roles = "AppOps")]
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _AdGroupTrackingEvents(AdGroupTrackingEventCriteriaDto criteria, int id)
        {
            criteria.CampaignId = id;
            var result = _campaignService.GetAdGroupTrackingEvents(criteria);
            if (result.Items != null)
            {
                result.Items = result.Items.OrderBy(p => p.Id).ToList();
            }


            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        [HttpPost]
        public ActionResult CheckDeleteTrackingEvent(int id, int adGroupId, List<string> adGroupTrackingEventCodes, bool checkStandards, int? costModelWrapperId)
        {
            string statusMessage = string.Empty;
            adGroupTrackingEventCodes = adGroupTrackingEventCodes == null ? adGroupTrackingEventCodes : adGroupTrackingEventCodes.Where(p => !string.IsNullOrEmpty(p)).ToList();
            try
            {
                KeyValuePair<bool, string> result = _campaignService.IsDeleteTrackingEventAllowed(id, adGroupId, adGroupTrackingEventCodes, checkStandards, costModelWrapperId);
                if (!result.Key)
                {
                    statusMessage = result.Value;
                    Response.StatusCode = 0;

                    throw new BusinessException(new List<ErrorData> { new ErrorData("", null, statusMessage) });
                }

            }
            catch (BusinessException x)
            {
                foreach (var item in x.Errors)
                {
                    statusMessage = item.Message;
                    Response.StatusCode = 0;
                }

            }

            return Content(statusMessage);
        }

        public ActionResult CheckUniqueEventCode(string code)
        {
            bool result = _campaignService.CheckEventUniqueByCode(code);

            return Content(result.ToString().ToLower());
        }
        public ActionResult checkSystemEventFraud(string code, string name)
        {
            code = code.Replace(" ", string.Empty);
            name = name.Replace(" ", string.Empty);
            bool result = _campaignService.checkSystemEventFraud(code, name);

            return Content(result.ToString().ToLower());
        }



        #endregion
        #endregion
        #endregion
        #region Objective
        #region Helper

        protected bool IsDownloadAction(int adActionId)
        {
            return (adActionId == (int)AdActionTypeIds.DownloadiPhoneApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadiPadApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadiOSUniversalApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadAndroidApplication);


        }
        private ObjectiveViewModel GetObjectiveViewModel(CampaignDto campaign)
        {
            var objectiveTypes = GetObjectiveTypes(campaign.CostModelWrapper);

            var objectiveViewModel = new ObjectiveViewModel { Items = objectiveTypes, CampaignId = campaign.ID, NativeShow = checkAdPermissions(PortalPermissionsCode.NativeAd) };
            return objectiveViewModel;
        }
        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(Code);

            return result;
        }

        private bool isDSP()
        {
            var impersonatedAccount = Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().ImpersonatedAccount;
            var Account = Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>();

            if (impersonatedAccount != null)
            {
                return _accountService.GetAccountRole((int)impersonatedAccount.AccountId) == (int)AccountRole.DSP;
            }
            else
            {
                return Account.AccountRole == (int)AccountRole.DSP;

            }
        }
        private void ValidateObjectiveTypes(ref List<ObjectiveTypeDto> objectiveTypes)
        {
            foreach (ObjectiveTypeDto Objective in objectiveTypes)
            {
                foreach (AdActionTypeDto AdActionTypes in Objective.AdActionTypes)
                {
                    foreach (AdTypeDto AdType in AdActionTypes.AdTypes)
                    {

                        if (AdType.AdPermission != null && !checkAdPermissions(AdType.AdPermission.Code))
                        {
                            AdType.hide = true;
                        }
                        else if (AdType.AdPermission == null)
                        {
                            foreach (AdSubtypeDto Subtypes in AdType.Subtypes)
                            {
                                if (!checkAdPermissions(Subtypes.Permission.Code) || Subtypes.AdActionTypeIds.Where(x => x == AdActionTypes.ID).Count() == 0)
                                {
                                    Subtypes.hide = true;

                                }
                            }
                            if (AdType.Subtypes.Count > 0)
                            {
                                AdType.Subtypes = AdType.Subtypes.Where(x => !x.hide).ToList();
                                if (AdType.Subtypes.Count < 1)
                                {
                                    AdType.hide = true;

                                }
                            }

                        }
                    }

                    if (AdActionTypes.AdTypes.Where(x => !x.hide).ToList().Count < 1)
                    {
                        AdActionTypes.hide = true;
                    }

                    AdActionTypes.AdTypes = AdActionTypes.AdTypes.Where(x => !x.hide).ToList();

                }
                Objective.AdActionTypes = Objective.AdActionTypes.Where(x => !x.hide).ToList();

            }
        }
        protected virtual List<ObjectiveTypeDto> GetObjectiveTypes(int? costModel)
        {
            var objectiveTypes = _objectiveTypeService.GetAll().ToList();

            List<ObjectiveTypeDto> campaingObjectiveTypes = new List<ObjectiveTypeDto>();

            if (!Config.IsAdmin)
                ValidateObjectiveTypes(ref objectiveTypes);

            if (costModel == null)
            {
                foreach (var objectiveTypeDto in objectiveTypes)
                {
                    ObjectiveTypeDto objectiveType = new ObjectiveTypeDto() { ID = objectiveTypeDto.ID, Name = objectiveTypeDto.Name };

                    List<AdActionTypeDto> adActionTypes = objectiveTypeDto.AdActionTypes.ToList();

                    objectiveType.AdActionTypes = adActionTypes;


                    campaingObjectiveTypes.Add(objectiveType);
                }
            }
            else
            {
                foreach (var objectiveTypeDto in objectiveTypes)
                {
                    ObjectiveTypeDto objectiveType = new ObjectiveTypeDto() { ID = objectiveTypeDto.ID, Name = objectiveTypeDto.Name };

                    objectiveType.AdActionTypes = objectiveTypeDto.AdActionTypes.Where(x => x.CostModelWrappers != null && x.CostModelWrappers.Contains(costModel.Value));

                    if (objectiveType.AdActionTypes != null && objectiveType.AdActionTypes.Count() != 0)
                    {
                        campaingObjectiveTypes.Add(objectiveType);
                    }
                }
            }

            return campaingObjectiveTypes;
        }

        #endregion
        public virtual ActionResult Objective(int id)
        {
            var campaign = _campaignService.Get(id, CampaignType.Normal, CampaignType.ProgrammaticGuaranteed);

            //if (!_campaignService.IsReadOrWriteCampaign(campaign.ID))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}

            //if (campaign.AdvertiserAccountId > 0)
            //{
            //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(campaign.AdvertiserAccountId))
            //    {
            //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //    }
            //}

            var objectiveViewModel = GetObjectiveViewModel(campaign);

            objectiveViewModel.AdvertiserId = campaign.AdvertiserId;
            objectiveViewModel.AdvertiserName = campaign.AdvertiserName;
            objectiveViewModel.AdvertiserAccountId = campaign.AdvertiserAccountId;
            objectiveViewModel.AdvertiserAccountName = campaign.AdvertiserAccountName;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AdGroupObjective", "SiteMapLocalizations"),
                                                  Order = 4,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url=Url.Action("Groups",new {id= id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =campaign.Name,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=objectiveViewModel.AdvertiserAccountId>0?Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=objectiveViewModel.AdvertiserAccountId,id=id }):Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=objectiveViewModel.AdvertiserAccountId>0?Url.Action("Index", new { AdvertiseraccId=objectiveViewModel.AdvertiserAccountId}):Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };


            if (objectiveViewModel.AdvertiserAccountId > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = objectiveViewModel.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers"),
                                           Order = -2,
                                           ExtensionDropDown = true
                                       });
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(objectiveViewModel);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Objective(int id, ObjectiveViewModel objectiveViewModel)
        {
            int campaignId = id;
            string advertisrName = string.Empty;

            if (ModelState.IsValid)
            {
                var adGroupDto = new AdGroupDto()
                {
                    ActionTypeId = (AdActionTypeIds)objectiveViewModel.ActionTypeId,
                    CampaignId = campaignId,
                    Name = objectiveViewModel.Name,
                    ObjectiveTypeId = (AdGroupObjectiveTypeIds)objectiveViewModel.ObjectiveTypeId,
                    TypeId = objectiveViewModel.IsNativeAd ? AdTypeIds.NativeAd : new AdTypeIds?()
                };
                var groupid = _campaignService.SaveAdGroup(adGroupDto, true);
                if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
                {
                    return RedirectToAction("Targeting", new { id = campaignId, adGroupId = groupid });
                }
                else
                {
                    return RedirectToAction("Groups", new { id = campaignId });
                }
            }

            var campaign = _campaignService.Get(id, CampaignType.Normal, CampaignType.ProgrammaticGuaranteed);
            objectiveViewModel.Items = GetObjectiveTypes(campaign.CostModelWrapper);
            return View(objectiveViewModel);
        }
        #endregion
        #endregion
        #region Ads

        #region Index

        #region Helpers
        protected AdsCriteria GetAdCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdsCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.Name
                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected AdsSearchDto GetAdQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int campaignId, int GroupId)
        {
            try
            {
                var criteria = GetAdCriteria(filter);
                criteria.CampaignId = campaignId;
                criteria.GroupId = GroupId;
                var result = _campaignService.QueryAdsByCratiria(criteria);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        protected AdListViewModel LoadAdsData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int CampaignId, int GroupId)
        {
            var result = GetAdQueryResult(filter, CampaignId, GroupId);
            ViewData["total"] = result.TotalCount;

            #region Actions
            var actions = GetAdActions(CampaignId, GroupId, result, result.AdvertiserAccountId);
            var toolTips = GetAdTooltips(CampaignId, GroupId, result.AdvertiserAccountId);

            #endregion
            return new AdListViewModel()
            {
                Items = result.Items,
                Performance = result.Performance,
                CampaignName = result.CampaignName,
                AdvertiserId = result.AdvertiserId,
                AdvertiserName = result.AdvertiserName,

                AdvertiserAccountId = result.AdvertiserAccountId,
                AdvertiserAccountName = result.AdvertiserAccountName,
                AdGroupName = result.AdGroup.Name,
                TopActions = actions,
                BelowAction = actions,
                //Statuses = statuesDropDown,
                CampaignId = CampaignId,
                AdGroupId = GroupId,
                ToolTips = toolTips,
                PreventEdit = result.AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(result.AdvertiserAccountId) : false
            };
        }

        protected virtual List<Action> GetAdTooltips(int CampaignId, int GroupId, int AdvertiseraccId = 0)
        {
            // Create the tool tip actions
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Creative",
                            ExtraPrams = CampaignId + "/" + GroupId
                        },
                    new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                            ClassName = "grid-tool-tip-reports",
                            URL = Url.Action("index", "Reports", new {reportType = "ad"})
                        }
                    ,
                    new Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Copy", "Commands"),
                            ClassName = "grid-tool-tip-copy",
                            ActionName = "CopyAd",
                            ExtraPrams = CampaignId + "/" + GroupId,
                            Type = ActionType.ajax,
                            CallBack = "refreshCampaignGrid();"
                        }, new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",
                            ExtraPrams = CampaignId,


                        }
                };


            if (AdvertiseraccId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
            {
                toolTips = new List<Model.Action>();

                toolTips.Add(
                    new Model.Action()
                    {
                        Code = "0",
                        DisplayText = ResourcesUtilities.GetResource("View", "Commands"),
                        ClassName = "grid-tool-tip-edit",
                        ActionName = "Creative",
                        ExtraPrams = CampaignId + "/" + GroupId
                    });
            }

            if (Config.IsAdOpsAdminInAdminApp)
            {
                toolTips.Add(
                    new Model.Action()
                    {
                        Code = "3",
                        DisplayText = ResourcesUtilities.GetResource("AdOps", "Commands"),
                        ClassName = "grid-tool-tip-AdOps",
                        ActionName = "AdDetails",
                        ControllerName = "Campaign",
                        ExtraPrams = CampaignId + "/" + GroupId,
                    });
            }

            return toolTips;
        }

        protected virtual List<Action> GetAdActions(int CampaignId, int GroupId, AdsSearchDto result, int AdvertiseraccId = 0)
        {
            AdTypeIds adtypeid = _campaignService.GetAddTypesByAddGroupAction(GroupId);
            // create the actions
            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "run",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Run", "Commands"),
                              ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Ad"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Run", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "pause",
                            ClassName = "btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Pause", "Commands"),
                              ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Ad"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Pause", "Confirmation") // like are u sure ?
                        },
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Ad"),// like please select at least one element 
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                        }
                };

            if (result.Performance.Bid != 0 || Config.IsAdmin)
            {
                AdTypeIds? adtypeId = result.AdGroup.TypeId;
                bool showAdButton = true;
                //if (adtypeId.HasValue)
                //{
                //    if (adtypeId.Value == AdTypeIds.NativeAd && !Config.IsAdministrationApp)
                //    {
                //        showAdButton = false;
                //    }
                //}
                //else
                //{
                //    if ((result.AdGroup.ActionTypeId == AdActionTypeIds.RichMedia || result.AdGroup.ActionTypeId == AdActionTypeIds.Interstitial) && !Config.IsAdministrationApp)
                //    {
                //        showAdButton = false;
                //    }
                //}
                if (result.IsClientLocked || result.IsClientReadOnly)
                { showAdButton = false; }
               
                if (!_campaignService.DoesContainDataProviderAllowImpressionTracker(CampaignId, GroupId))
                {
                 int adActionTypeId=   _campaignService.GetActionTypeByadGroup(GroupId);
                    if (adActionTypeId==(int)AdActionTypeIds.Interstitial)
                    showAdButton = false;
                }
                if (showAdButton)
                {

                    actions.Add(new Model.Action()
                    {
                        ActionName = "Creative",
                        ClassName = "primary-btn",
                        Type = ActionType.Link,
                        DisplayText = ResourcesUtilities.GetResource("AddNewAds", "Commands"),
                        ExtraPrams = CampaignId,
                        ExtraPrams2 = GroupId,
                        ExtraPrams3 = null
                    });
                    //if (adtypeid == AdTypeIds.TrackingAd)
                    //{
                    //    //This kind of app for adminstration
                    //    if (Config.IsAdministrationApp || checkAdPermissions(PortalPermissionsCode.TrackingAd))
                    //    {
                    //        actions.Add(new Model.Action()
                    //        {
                    //            ActionName = "Creative",
                    //            ClassName = "primary-btn",
                    //            Type = ActionType.Link,
                    //            DisplayText = ResourcesUtilities.GetResource("AddNewTrackerAd", "Commands"),
                    //            ExtraPrams = CampaignId,
                    //            ExtraPrams2 = GroupId,
                    //            ExtraPrams3 = (int)AdTypeIds.TrackingAd
                    //        });
                    //    }

                    //}
                }
            }


            if (AdvertiseraccId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId))
            {
                actions = new List<Model.Action>();

            }

            return actions;
        }
        #endregion

        #region Actions
        public ActionResult CopyAd(int id, int adGroupId, int itemId, string name)
        {
            ResponseDto result = _campaignService.CloneAd(id, adGroupId, itemId, name);
            if (result.success)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 0;
            }
            return Content(result.Massage);

        }
        public ActionResult Ads(int id, int adGroupId, string message, bool isHouseAd = false)
        {
            int campaignId = id;
            AdListViewModel ads = LoadAdsData(null, campaignId, adGroupId);

            //if (!_campaignService.IsReadOrWriteCampaign(campaignId))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}

            //if (ads.AdvertiserAccountId > 0)
            //{
            //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(ads.AdvertiserAccountId))
            //    {
            //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //    }
            //}

            var adGroup = _campaignService.GetTargeting(campaignId, adGroupId);
            if (!Config.IsAdmin)
            {
                if (adGroup.Bid == 0 && !isHouseAd)
                {
                    return RedirectToAction("Targeting", new { id = id, adGroupId = adGroupId, returenFromAdsPage = true });
                }
            }

            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ads.AdGroupName,//ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                   Order = 4,
                                                   Url=Url.Action("Targeting",new {id= id,adGroupId=adGroupId})
                                              },
                                        new BreadCrumbModel()
                                                {
                                                    Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                    Order = 3,
                                                    Url=Url.Action("Groups",new {id= id})
                                                },
                                         new BreadCrumbModel()
                                              {
                                                  Text =ads.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=ads.AdvertiserAccountId>0 ? Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=ads.AdvertiserAccountId,id=id}) :Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=ads.AdvertiserAccountId>0 ? Url.Action("Index", new { AdvertiseraccId=ads.AdvertiserAccountId}) :Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };


            if (ads.AdvertiserAccountId > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = ads.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers"),
                                           Order = -2,
                                           ExtensionDropDown = true
                                       });
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (!string.IsNullOrEmpty(message))
            {
                switch (message.ToLower())
                {
                    case "notauthenticated":
                        AddMessages(ResourcesUtilities.GetResource("NotAuthenticated", "Campaign"), MessagesType.Warning);
                        break;
                    default:
                        break;
                }

            }
            ShowupdateLastActionDone();
            return View("IndexAds", ads);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Ads(int id, int adGroupId, int[] checkedRecords)
        {
            int campaignId = id;


            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns
                _campaignService.DeleteAds(campaignId, adGroupId, checkedRecords);
                updateLastActionDone(ResourcesUtilities.GetResource("AdsDeletedSuccessfully", "Campaign"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
                {
                    //run  selected Campaigns
                    _campaignService.RunAds(campaignId, adGroupId, checkedRecords);
                    updateLastActionDone(ResourcesUtilities.GetResource("AdsRunSuccessfully", "Campaign"));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
                    {
                        //pause selected Campaigns
                        _campaignService.PauseAds(campaignId, adGroupId, checkedRecords);
                        updateLastActionDone(ResourcesUtilities.GetResource("AdsPausedSuccessfully", "Campaign")); 
                    }
                }
            }
            return RedirectToAction("Ads", new { id = id, adGroupId = adGroupId });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Ads(int id, int adGroupId)
        {
            int campaignId = id;


            var result = GetAdQueryResult(null, campaignId, adGroupId);
            return View("IndexAds", new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        public JsonResult AdsLessBid(decimal bid, int campaignId, int adGroupId)
        {
            var items = _campaignService.QueryAdsLessBid(campaignId, adGroupId, bid);
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public JsonResult AdsMoreBid(decimal bid, int campaignId, int adGroupId)
        {
            var items = _campaignService.QueryAdsMoreBid(campaignId, adGroupId, bid);
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        #endregion

        #endregion


        #region Create
        #region Helpers

        protected IEnumerable<CreativeUnitViewModel> GetTileImages(int adActionId)
        {
            var item = _tileImageService.GetAllByAdAction(adActionId);
            if (item == null)
                return null;
            var returnItems = new List<CreativeUnitViewModel>();
            foreach (var tileImageDocumentDto in item.Images)
            {
                if (tileImageDocumentDto.Document != null)
                {
                    var tileInfo = tileImageDocumentDto.TileImageSize.TitleSize;
                    returnItems.Add(new CreativeUnitViewModel()
                    {
                        DisplayText = tileInfo.Name,
                        TileImageDocumentDto = tileImageDocumentDto,
                        Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)AdTypeIds.Text, "0", adActionId.ToString(), tileImageDocumentDto.TileImageSize.ID),
                    });
                }
            }
            return returnItems;
        }
        protected IEnumerable<CreativeUnitViewModel> GetTileImages(int adActionId, IEnumerable<AdCreativeUnitDto> docIds)
        {
            var item = _tileImageService.GetAllByAdAction(adActionId);
            if (item == null)
                return null;
            var returnItems = new List<CreativeUnitViewModel>();

            // Phone Tile
            var phoneTileImageDocumentDto = item.Images.Where(p => p.TileImageSize.DeviceType == (int)DeviceTypeEnum.SmartPhone).Single();
            var phoneTileInfo = phoneTileImageDocumentDto.TileImageSize.TitleSize;
            var phoneTitleDocument = docIds.ToList().FirstOrDefault(doc => doc.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone);
            returnItems.Add(new CreativeUnitViewModel()
            {
                DisplayText = phoneTileInfo.Name,
                TileImageDocumentDto = phoneTileImageDocumentDto,
                Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)AdTypeIds.Text, "0", adActionId.ToString(), (int)phoneTileImageDocumentDto.TileImageSize.ID),
                DocumentId = phoneTitleDocument == null ? (int?)null : phoneTitleDocument.DocumentId,
                Content = phoneTitleDocument != null ? phoneTitleDocument.Content : string.Empty,
                ImpressionTrackerRedirect = phoneTitleDocument != null ? phoneTitleDocument.ImpressionTrackerRedirect : null,

                ImpressionTrackerJSRedirect = phoneTitleDocument != null ? phoneTitleDocument.ImpressionTrackerJSRedirect : null
            });

            // Tablet Tile
            var tabletTileImageDocumentDto = item.Images.Where(p => p.TileImageSize.DeviceType == (int)DeviceTypeEnum.Tablet).Single();
            var tabletTileInfo = tabletTileImageDocumentDto.TileImageSize.TitleSize;
            var tabletTitleDocument = docIds.ToList().FirstOrDefault(doc => doc.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet);
            returnItems.Add(new CreativeUnitViewModel()
            {
                DisplayText = tabletTileInfo.Name,
                TileImageDocumentDto = tabletTileImageDocumentDto,
                Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)AdTypeIds.Text, "0", adActionId.ToString(), (int)tabletTileImageDocumentDto.TileImageSize.ID),
                DocumentId = tabletTitleDocument == null ? (int?)null : tabletTitleDocument.DocumentId,
                Content = tabletTitleDocument != null ? tabletTitleDocument.Content : string.Empty,
                ImpressionTrackerRedirect = tabletTitleDocument != null ? tabletTitleDocument.ImpressionTrackerRedirect : null,
                ImpressionTrackerJSRedirect = tabletTitleDocument != null ? tabletTitleDocument.ImpressionTrackerJSRedirect : null
            });

            return returnItems;
        }
        protected IEnumerable<CreativeUnitDto> GetCreativeUnits(DeviceTypeEnum deviceType, AdTypeIds? adType, AdSubTypes? adSubType = null, string group = null)
        {
            var creativeUnits = _creativeUnitService.GetBy(deviceType, adType, adSubType, group);
            return creativeUnits;
        }
        //
        protected IEnumerable<CreativeUnitDto> GetCreativeUnitsByCriteria(int? creativeUnitId, DeviceTypeEnum deviceType, AdTypeIds? adType, string group = null)
        {
            var creativeUnits = _creativeUnitService.GetByCriteria(creativeUnitId, (int)deviceType, group, (int?)adType);
            return creativeUnits;
        }
        private IList<CreativeUnitViewModel> GetVideoCardCreatives(IList<AdCreativeUnitDto> adCreativeUnits, string creativesGroup)
        {
            var creativeUnits = GetCreativeUnitsByCriteria(null, DeviceTypeEnum.Any, null, creativesGroup);
            //  creativeUnits = creativeUnits.Where(p => p.DeviceType.ID == (int)DeviceTypeEnum.Any).ToList();

            var returnFiles = new List<CreativeUnitViewModel>();
            foreach (var creativeUnitDto in creativeUnits)
            {
                var adCreativeUnit = adCreativeUnits == null ? null : adCreativeUnits.FirstOrDefault(item => item.CreativeUnitId == creativeUnitDto.ID);

                returnFiles.Add(new CreativeUnitViewModel()
                {
                    DocumentId = adCreativeUnit == null ? (int?)null : adCreativeUnit.DocumentId,
                    Content = adCreativeUnit != null ? adCreativeUnit.Content : string.Empty,
                    DisplayText = string.Format("{0}x{1}", creativeUnitDto.Width, creativeUnitDto.Height),
                    CreativeUnitDto = creativeUnitDto,
                    DeviceType = DeviceTypeEnum.Any,
                    AdTypeId = (int)AdTypeIds.VideoEndCard,
                    Name = string.Format("CreativeUnit_{0}_{1}", creativesGroup, creativeUnitDto.ID.ToString()),
                });
            }

            returnFiles = returnFiles.ToList().OrderByDescending(p => p.CreativeUnitDto.RequiredType).ToList();
            returnFiles = returnFiles.OrderByDescending(p => p.CreativeUnitDto.Width * p.CreativeUnitDto.Height).ToList();
            return returnFiles;

        }
        private IList<CreativeUnitViewModel> GetNativeAdsCreatives(IList<AdCreativeUnitDto> adCreativeUnits, string creativesGroup)
        {
            var creativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, AdTypeIds.NativeAd, null, creativesGroup);
            creativeUnits = creativeUnits.Where(p => p.DeviceType.ID == (int)DeviceTypeEnum.Any).ToList();

            var returnFiles = new List<CreativeUnitViewModel>();
            foreach (var creativeUnitDto in creativeUnits)
            {
                var adCreativeUnit = adCreativeUnits == null ? null : adCreativeUnits.FirstOrDefault(item => item.CreativeUnitId == creativeUnitDto.ID);

                returnFiles.Add(new CreativeUnitViewModel()
                {
                    DocumentId = adCreativeUnit == null ? (int?)null : adCreativeUnit.DocumentId,
                    Content = adCreativeUnit != null ? adCreativeUnit.Content : string.Empty,
                    DisplayText = string.Format("{0}x{1}", creativeUnitDto.Width, creativeUnitDto.Height),
                    CreativeUnitDto = creativeUnitDto,
                    DeviceType = DeviceTypeEnum.Any,
                    AdTypeId = (int)AdTypeIds.NativeAd,
                    Name = string.Format("CreativeUnit_{0}_{1}", creativesGroup, creativeUnitDto.ID.ToString()),
                });
            }

            returnFiles = returnFiles.ToList().OrderByDescending(p => p.CreativeUnitDto.RequiredType).ToList();
            returnFiles = returnFiles.OrderByDescending(p => p.CreativeUnitDto.Width * p.CreativeUnitDto.Height).ToList();
            return returnFiles;

        }

        protected IEnumerable<CreativeUnitViewModel> GetAdCreativeUnits(DeviceTypeEnum deviceType, IList<AdCreativeUnitDto> adCreativeUnits, AdTypeIds adType, AdSubTypes? adSubType = null)
        {

            var creativeUnits = GetCreativeUnits(deviceType, adType, adSubType);
            var returnFiles = new List<CreativeUnitViewModel>();
            foreach (var creativeUnitDto in creativeUnits)
            {
                var adCreativeUnit = adCreativeUnits.FirstOrDefault(item => item.CreativeUnitId == creativeUnitDto.ID);

                returnFiles.Add(new CreativeUnitViewModel()
                {
                    DocumentId = adCreativeUnit == null ? (int?)null : adCreativeUnit.DocumentId,
                    Content = adCreativeUnit != null ? adCreativeUnit.Content : string.Empty,
                    ImpressionTrackerRedirect = adCreativeUnit != null ? adCreativeUnit.ImpressionTrackerRedirect : string.Empty,
                    ImpressionTrackerJSRedirect = adCreativeUnit != null ? adCreativeUnit.ImpressionTrackerJSRedirect : string.Empty,
                    CreativeUnitDto = creativeUnitDto,
                    DeviceType = deviceType,
                    DisplayText = creativeUnitDto.Name,
                    AdTypeId = (int)adType,
                    Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)adType, adSubType != null && adSubType.HasValue ? ((int)adSubType.Value).ToString() : "0", (int)deviceType, creativeUnitDto.ID.ToString()),
                });
            }

            return returnFiles;
        }

        protected IEnumerable<CreativeUnitViewModel> GetCreativeUnitsViewModel(DeviceTypeEnum deviceType, AdTypeIds adType, AdSubTypes? adSubType = null)
        {
            var creativeUnits = GetCreativeUnits(deviceType, adType, adSubType);

            var result = creativeUnits.Select(creativeUnitDto => new CreativeUnitViewModel()
            {
                DisplayText = creativeUnitDto.Name,
                CreativeUnitDto = creativeUnitDto,
                OrientationReplacementId = creativeUnitDto.OrientationReplacementId,
                DeviceType = deviceType,
                AdTypeId = (int)adType,
                Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)adType, adSubType != null && adSubType.HasValue ? ((int)adSubType.Value).ToString() : "0", (int)deviceType, creativeUnitDto.ID.ToString()),
            }).ToList();
            return result;
        }
        private string GetInstreamVideUplaodDisplyMsg(IEnumerable<CreativeUnitDto> instreamCreativeUnits)
        {
            string displayText = string.Empty;
            foreach (var creativeUnitDto in instreamCreativeUnits)
            {
                if (string.IsNullOrEmpty(displayText))
                {
                    displayText = creativeUnitDto.Description;
                }
                else
                {
                    displayText += "," + creativeUnitDto.Description;
                }

            }
            return displayText;
        }

        protected IEnumerable<CreativeUnitViewModel> GetInstreamVideoCreativeUnitsViewModel(DeviceTypeEnum deviceType, AdTypeIds adType, AdSubTypes? adSubType = null)
        {
            string displayText = string.Empty;
            var instreamCreativeUnits = GetCreativeUnits(deviceType, adType, adSubType);
            // var creativeUnitDto = instreamCreativeUnits.Where(M=>M.Code=="28").FirstOrDefault();
            var creativeUnitDto = _creativeUnitService.GetById(21);
            IList<CreativeUnitViewModel> list = new List<CreativeUnitViewModel>();
            var result = new CreativeUnitViewModel()
            {
                DisplayText = GetInstreamVideUplaodDisplyMsg(instreamCreativeUnits),
                CreativeUnitDto = creativeUnitDto,
                OrientationReplacementId = creativeUnitDto.OrientationReplacementId,
                DeviceType = deviceType,
                AdTypeId = (int)adType,
                Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)adType, adSubType != null && adSubType.HasValue ? ((int)adSubType.Value).ToString() : "0", (int)deviceType, creativeUnitDto.ID.ToString())


            };
            list.Add(result);
            return list;
        }

        /// <summary>
        /// use this protected function to get the Creative View Model object 
        /// </summary>
        /// <param name="titleImageId">use this id to set selected for the Tile Image DropDown Option</param>
        /// <returns></returns>
        protected CreativeViewModel GetCreativeViewModel(int titleImageId)
        {
            var model = new CreativeViewModel();
            var value = "-1";
            //Load Tile Image List
            #region Tile Images
            var optionalItem = new SelectListItem { Value = value, Text = ResourcesUtilities.GetResource("Custom", "Global") };
            List<TileImageDto> TileImageDtos = _tileImageService.GetAll().ToList();
            var tileImageList = new List<SelectListItem> { optionalItem };
            foreach (var item in TileImageDtos)
            {
                var selectItem = new SelectListItem
                {
                    Value = item.ID.ToString() + "#" + GetTileImageValues(item),
                    Text = item.Name.ToString()
                };
                if (titleImageId == item.ID)
                {
                    value = item.ID.ToString() + "#" + GetTileImageValues(item);
                    optionalItem.Selected = false;
                    selectItem.Selected = true;
                }


                tileImageList.Add(selectItem);
            }
            model.TileImageViewModel = new TileImageViewModel()
            {
                TileImageList = tileImageList,
                SelectedValue = value
            };
            #endregion

            return model;
        }
        protected string GetTileImageValues(TileImageDto tileImage)
        {
            string returnString = string.Empty;
            foreach (var tileImageDocumentDto in tileImage.Images)
            {
                returnString += string.Format("{0}&{1}#", tileImageDocumentDto.TileImageSize.ID, tileImageDocumentDto.Document.ID);
            }
            return returnString;
        }
        protected CreativeViewModel GetCreativeViewModel(int id, int adGroupId, int? adId, int? adType = null)
        {
            int campaignId = id;

            ViewBag.CreativeVendorAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "CreativeVendor_Name",
                Name = "CreativeVendor.Name",
                ActionName = "GetCreativeVendors",
                ControllerName = "CreativeUnit",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "CreativeVendorChanged"

            };

            CreativeViewModel model = null;
            var item = _campaignService.GetAdCreative(campaignId, adGroupId, adId, adType);

            if (adId.HasValue)
            {

                model = GetCreativeViewModel(item.TileImageId);
                model.AdCreativeDto = item;
                model.WrapperContent = item.WrapperContent;
                model.ClickTags = item.ClickTags;
                model.ThirdPartyTrackers = item.ThirdPartyTrackers;
                if (model.AdCreativeDto.CreativeVendorIds != null && model.AdCreativeDto.CreativeVendorIds.Count > 0)
                {

                    model.AdCreativeVendorIds = model.AdCreativeDto.AdCreativeVendorIds;
                    model.CreativeVendorIds = model.AdCreativeDto.CreativeVendorIds;

                }
                if (item.AdBannerType.HasValue)
                {
                    switch (item.AdBannerType.Value)
                    {
                        case DeviceTypeEnum.Any:
                            model.AdBannerTypeName = ResourcesUtilities.GetResource("SmartPhone", "Campaign");
                            break;
                        case DeviceTypeEnum.SmartPhone:
                            model.AdBannerTypeName = ResourcesUtilities.GetResource("SmartPhone", "Campaign");
                            break;
                        case DeviceTypeEnum.Tablet:
                            model.AdBannerTypeName = ResourcesUtilities.GetResource("Tablet", "Campaign");
                            break;
                        default:
                            break;
                    }
                }

                switch (item.TypeId)
                {
                    case AdTypeIds.Text:
                        {
                            //this is Text Creative
                            model.TileImageViewModel.TileImages = GetTileImages(item.AdActionId, item.CreativeUnitsContent);
                            break;
                        }
                    case AdTypeIds.NativeAd:
                        {
                            model.NativeAdIcons = GetNativeAdsCreatives(item.NativeAdIcons, _nativeAdIconsGroup);
                            model.NativeAdImages = GetNativeAdsCreatives(null, _nativeAdImagesGroup);
                            ViewData["IconsContainer"] = _nativeAdIconsGroup;
                            ViewData["ImagesContainer"] = _nativeAdImagesGroup;
                        }

                        break;
                    case AdTypeIds.Banner:
                    case AdTypeIds.PlainHTML:
                        {
                            if (item.TypeId == AdTypeIds.PlainHTML)
                                model.AdCreativeDto.AdActionValue.Trackers = new List<AdActionValueTrackerDto>();

                            //this is Banner or Plain HTML Creative
                            switch (item.AdBannerType.Value)
                            {
                                case DeviceTypeEnum.SmartPhone:
                                    {
                                        var resutltsm= GetAdCreativeUnits(DeviceTypeEnum.Any, item.CreativeUnitsContent, item.TypeId).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                                        resutltsm[0].DeviceType = DeviceTypeEnum.SmartPhone;
                                        resutltsm[0].Name = resutltsm[0].Name.Replace("1_0_0", "1_0_1");

                                        resutltsm[0].Name = resutltsm[0].Name.Replace("3_0_0", "3_0_1");
                                        model.PhoneCreativeUnits = new Dictionary
                                           <int, IEnumerable<CreativeUnitViewModel>>
                                            {
                                                {
                                                    (int)item.TypeId,GetAdCreativeUnits(DeviceTypeEnum.SmartPhone,item.CreativeUnitsContent, item.TypeId).Concat(   resutltsm)
                                                        .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                }
                                            };




                                        var first = model.PhoneCreativeUnits.FirstOrDefault().Value.FirstOrDefault();
                                        if ((first != null))
                                            first.ShowCopy = true;

                                        break;
                                    }
                                case DeviceTypeEnum.Tablet:
                                    {
                                        var resutltsm = GetAdCreativeUnits(DeviceTypeEnum.Any, item.CreativeUnitsContent, item.TypeId).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                                        resutltsm[0].DeviceType = DeviceTypeEnum.Tablet;
                                        resutltsm[0].Name = resutltsm[0].Name.Replace("1_0_0", "1_0_2");

                                        resutltsm[0].Name = resutltsm[0].Name.Replace("3_0_0", "3_0_2");
                                        model.TabletCreativeUnits = new Dictionary
                                            <int, IEnumerable<CreativeUnitViewModel>>
                                            {
                                                {
                                                    (int) item.TypeId,
                                                    GetAdCreativeUnits(DeviceTypeEnum.Tablet, item.CreativeUnitsContent,item.TypeId).ToList().Concat( resutltsm)
                                                       .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                }
                                            };



                                        break;
                                    }
                            }

                            break;
                        }
                    case AdTypeIds.RichMedia:
                        {
                            //this is Rich Media Creative
                            GetRichMediaProtocol(model, item);

                            if (item.AdSubType.HasValue)
                            {
                                switch (item.AdSubType.Value)
                                {
                                    case AdSubTypes.ExpandableRichMedia:
                                        model.AdSubTypeName = ResourcesUtilities.GetResource("ExpandableRichMedia", "Campaign");
                                        break;
                                    case AdSubTypes.HTML5Interstitial:
                                    case AdSubTypes.HTML5RichMedia:
                                        model.ClickMethod = item.ClickMethod;
                                        model.AdSubTypeName = ResourcesUtilities.GetResource("HTML5RichMedia", "Campaign");
                                        var CreativeUnitFirst = item.CreativeUnitsContent.FirstOrDefault();
                                        var results5 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                                        results5[0].DeviceType = DeviceTypeEnum.Tablet;
                                        results5[0].Name = results5[0].Name.Replace("_0", "_2");
                                        var listsCreatives = GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Concat(results5).Concat(GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).ToList());

                                        if (item.AdSubType.Value == AdSubTypes.HTML5Interstitial)
                                        {
                                            model.AdSubTypeName = ResourcesUtilities.GetResource("HTML5Interstitial", "Campaign");
                                            listsCreatives = GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial).Concat(GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial).ToList());
                                        }
                                        model.SelectedHTML5AdCreativeId = CreativeUnitFirst.ID;

                                        model.SelectedHTML5CreativeId = CreativeUnitFirst.CreativeUnitId;
                                        model.SelectedHTML5DocumentId = CreativeUnitFirst.DocumentId.Value;
                                        model.SelectedHTML5FileName = CreativeUnitFirst.DocumentName;
                                        //model.AdCreativeDto.AdActionId ==
                                        model.HTML5Creatives = new List<SelectListItem>() {   new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("Select", "Global"),
                            Value = "0",
                            Selected = false
                        }};
                                        foreach (var listCreative in listsCreatives)
                                        {
                                            model.HTML5Creatives.Add(new SelectListItem()
                                            {


                                                Text = listCreative.DisplayText,
                                                Value = listCreative.CreativeUnitDto.ID.ToString(),
                                                Selected = CreativeUnitFirst.CreativeUnitId == listCreative.CreativeUnitDto.ID

                                            });

                                        }
                                        break;
                                    case AdSubTypes.JavaScriptRichMedia:
                                        model.AdSubTypeName = ResourcesUtilities.GetResource("JavaScriptRichMedia", "Campaign");
                                        break;
                                    case AdSubTypes.ExternalUrlInterstitial:
                                        model.AdSubTypeName = ResourcesUtilities.GetResource("ExternalUrlInterstitial", "Campaign");
                                        break;
                                    case AdSubTypes.JavaScriptInterstitial:
                                        model.AdSubTypeName = ResourcesUtilities.GetResource("JavaScriptInterstitial", "Campaign");
                                        break;
                                    default:
                                        break;
                                }
                            }

                            switch (item.AdBannerType.Value)
                            {
                                case DeviceTypeEnum.SmartPhone:
                                    {
                                        var resutltsm = new List<CreativeUnitViewModel>();
                                        if (item.AdSubType.HasValue && ((int)item.AdSubType.Value == 1 || (int)item.AdSubType.Value == 2))
                                        {
                                            resutltsm = GetAdCreativeUnits(DeviceTypeEnum.Any, item.CreativeUnitsContent, item.TypeId, item.AdSubType).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                                            resutltsm[0].DeviceType = DeviceTypeEnum.SmartPhone;
                                            resutltsm[0].Name = resutltsm[0].Name.Replace("_0", "_1");
                                        }

                                        model.PhoneCreativeUnits = new Dictionary
                                                        <int, IEnumerable<CreativeUnitViewModel>>
                                                        {
                                                            {
                                                                (int) item.AdSubType,
                                                                GetAdCreativeUnits(DeviceTypeEnum.SmartPhone,
                                                                                   item.CreativeUnitsContent,
                                                                                   item.TypeId,
                                                                                   item.AdSubType).Concat(resutltsm)
                                                                    .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                            }
                                                        };

                                        var first = model.PhoneCreativeUnits.FirstOrDefault().Value.FirstOrDefault();
                                        if ((first != null)) { first.ShowCopy = true; }

                                        break;
                                    }
                                case DeviceTypeEnum.Tablet:
                                    {
                                        var resutltsm = new List<CreativeUnitViewModel>();
                                        if (item.AdSubType.HasValue && ((int)item.AdSubType.Value ==1 || (int)item.AdSubType.Value ==2 ))
                                        {
                                             resutltsm = GetAdCreativeUnits(DeviceTypeEnum.Any, item.CreativeUnitsContent, item.TypeId, item.AdSubType).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                                            resutltsm[0].DeviceType = DeviceTypeEnum.Tablet;
                                            resutltsm[0].Name = resutltsm[0].Name.Replace("_0", "_2");
                                        }

                                        model.TabletCreativeUnits = new Dictionary
                                                         <int, IEnumerable<CreativeUnitViewModel>>
                                                        {
                                                            {
                                                                (int) item.AdSubType,GetAdCreativeUnits(DeviceTypeEnum.Tablet,
                                                                                   item.CreativeUnitsContent,
                                                                                   item.TypeId, item.AdSubType).Concat(resutltsm)
                                                                    .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                            }
                                                        };


                                        break;
                                    }
                            }
                            break;
                        }
                    case AdTypeIds.TrackingAd:
                        {
                            if (item.TypeId == AdTypeIds.PlainHTML)
                                model.AdCreativeDto.AdActionValue.Trackers = new List<AdActionValueTrackerDto>();

                            //this is Banner or Plain HTML Creative
                            switch (item.AdBannerType.Value)
                            {
                                case DeviceTypeEnum.SmartPhone:
                                    {

                                        model.PhoneCreativeUnits = new Dictionary
                                           <int, IEnumerable<CreativeUnitViewModel>>
                                            {
                                                {
                                                    (int)item.TypeId,GetAdCreativeUnits(DeviceTypeEnum.SmartPhone,item.CreativeUnitsContent, item.TypeId)
                                                        .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                }
                                            };




                                        var first = model.PhoneCreativeUnits.FirstOrDefault().Value.FirstOrDefault();
                                        if ((first != null))
                                            first.ShowCopy = true;

                                        break;
                                    }
                                case DeviceTypeEnum.Tablet:
                                    {
                                        model.TabletCreativeUnits = new Dictionary
                                            <int, IEnumerable<CreativeUnitViewModel>>
                                            {
                                                {
                                                    (int) item.TypeId,
                                                    GetAdCreativeUnits(DeviceTypeEnum.Tablet, item.CreativeUnitsContent,item.TypeId).ToList()
                                                       .Where(x =>
                                                            (item.EnvironmentType==EnvironmentType.All||(x.CreativeUnitDto.EnvironmentType == EnvironmentType.All ||x.CreativeUnitDto.EnvironmentType == item.EnvironmentType))&&
                                                            (item.OrientationType==OrientationType.Any||(x.CreativeUnitDto.OrientationType == OrientationType.Any ||x.CreativeUnitDto.OrientationType == item.OrientationType))).ToList()
                                                }
                                            };



                                        break;
                                    }
                            }

                            break;
                        }
                    case AdTypeIds.InStreamVideo:
                        {
                            //    model.TabletCreativeUnits.Add((int)AdTypeIds.InStreamVideo, GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo));

                            model.InStreamVideos = GetInstreamVideoAdCreativeUnits(adGroupId, campaignId, model, item.CreativeUnitsContent);



                            model.VideoEndCards = item.VideoEndCards;

                            model.AdCreativeDto.VideoEndCardAdImages = new List<AdCreativeUnitDto>();
                            model.AdCreativeDto.ImageUrls = new List<CreativeUnitDto>();


                            model.AdCreativeDto.IsVpaid = item.IsVpaid;
                            model.AdCreativeDto.Vpaid_1 = item.Vpaid_1;
                            model.AdCreativeDto.Vpaid_2 = item.Vpaid_2;
                            model.AdCreativeDto.ImpressionTrackingURL = item.ImpressionTrackingURL;
                            model.AdCreativeDto.AdActionValueImpressionTracker = new AdActionValueDto();
                            model.AdCreativeDto.AdActionValueImpressionTracker.Trackers = new List<AdActionValueTrackerDto>();

                            if (model.AdCreativeDto.ImpressionTrackingURL != null && model.AdCreativeDto.ImpressionTrackingURL.Count > 0)
                            {

                                foreach (var URL in model.AdCreativeDto.ImpressionTrackingURL)
                                {
                                    model.AdCreativeDto.AdActionValueImpressionTracker.Trackers.Add(new AdActionValueTrackerDto { URL = URL });
                                }

                            }
                            if (model.VideoEndCards != null && model.VideoEndCards.Count > 0)
                            {
                                var videoEndCardCreativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, AdTypeIds.VideoEndCard, AdSubTypes.VideoEndCard, "13").ToList();
                                //adCreativeSaveDto.VideoEndCardFluidURL = model.AdCreativeDto.VideoEndCardFluidURL;
                                //adCreativeSaveDto.VideoEndCardFluid = model.AdCreativeDto.VideoEndCardFluid;
                                model.AdCreativeDto.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                                foreach (var videoEndCard in model.VideoEndCards)
                                {
                                    model.AdCreativeDto.VideoEndCardCreativeUnitsContent.Add(videoEndCard.CreativeUnitsContent[0]);
                                }

                                model.AdCreativeDto.CardType = model.VideoEndCards[0].CardType;
                                if (model.AdCreativeDto.CardType == VideoEndCardType.Static)
                                {
                                    model.AdCreativeDto.IsStatic = true;
                                }
                                model.AdCreativeDto.AutoCloseWaitInSeconds = model.VideoEndCards[0].EnableAutoClose ? model.VideoEndCards[0].AutoCloseWaitInSeconds : 7.5;
                                model.AdCreativeDto.EnableAutoClose = model.VideoEndCards[0].EnableAutoClose;
                                model.AdCreativeDto.AdActionValueVideoEndCardURL = model.VideoEndCards[0].AdActionValue != null ? model.VideoEndCards[0].AdActionValue.Value : "";
                                model.AdCreativeDto.VideoEndCardsTrackingURL = model.VideoEndCards[0].VideoEndCardsTrackingURL;
                                model.AdCreativeDto.VideoEndCardFluidURL = model.AdCreativeDto.VideoEndCardFluid && model.VideoEndCards[0].CreativeUnitsContent[0] != null ? model.VideoEndCards[0].CreativeUnitsContent[0].Content : "";

                                model.AdCreativeDto.AdActionValueVideoEndCard = new AdActionValueDto();
                                model.AdCreativeDto.AdActionValueVideoEndCard.Value = model.AdCreativeDto.AdActionValueVideoEndCardURL;
                                model.AdCreativeDto.AdActionValueVideoEndCard.Value2 = model.AdCreativeDto.AdActionValueVideoEndCardURL;
                                model.AdCreativeDto.AdActionValueVideoEndCard.Trackers = new List<AdActionValueTrackerDto>();

                                if (model.AdCreativeDto.VideoEndCardsTrackingURL != null && model.AdCreativeDto.VideoEndCardsTrackingURL.Count > 0)
                                {

                                    foreach (var URL in model.AdCreativeDto.VideoEndCardsTrackingURL)
                                    {
                                        model.AdCreativeDto.AdActionValueVideoEndCard.Trackers.Add(new AdActionValueTrackerDto { URL = URL });
                                    }

                                }

                                model.AdCreativeDto.ImageUrls = GetCreativeUnitsByCriteria(null, DeviceTypeEnum.Any, null, "13").ToList();


                                if (model.AdCreativeDto.CardType == VideoEndCardType.Dynamic)
                                {
                                    if (model.AdCreativeDto.VideoEndCardCreativeUnitsContent != null && model.AdCreativeDto.VideoEndCardCreativeUnitsContent.Count > 0)
                                    {

                                        //  model.AdCreativeDto.ImageUrls = GetCreativeUnitsByCriteria(null, DeviceTypeEnum.Any, null, "13").ToList();
                                        foreach (var cont in model.AdCreativeDto.VideoEndCardCreativeUnitsContent)
                                        {
                                            var creativUnit = videoEndCardCreativeUnits.Where(M => M.ID == cont.CreativeUnitId).SingleOrDefault();
                                            var itemsImages = model.AdCreativeDto.ImageUrls.Where(M => M.ID == cont.CreativeUnitId).Single();
                                            itemsImages.Url = cont.Content;

                                            //model.AdCreativeDto.ImageUrls.Add(new CreativeUnitDto { Width = creativUnit.Width, Height = creativUnit.Height, Url = cont.Content });

                                        }
                                    }

                                }
                                else
                                {



                                    if (model.AdCreativeDto.VideoEndCardCreativeUnitsContent != null && model.AdCreativeDto.VideoEndCardCreativeUnitsContent.Count > 0)
                                    {
                                        foreach (var creativeUnit in model.AdCreativeDto.VideoEndCardCreativeUnitsContent)
                                        {
                                            if (creativeUnit.DocumentId.HasValue)
                                            {
                                                var creativeUnitName = string.Format("CreativeUnit_{0}_{1}", "13", creativeUnit.ID.ToString());
                                                var creativUnit = videoEndCardCreativeUnits.Where(M => M.ID == creativeUnit.CreativeUnitId).SingleOrDefault();
                                                var adCreativeUnit2 = new AdCreativeUnitDto
                                                {
                                                    CreativeUnitId = creativeUnit.CreativeUnitId,
                                                    DocumentId = creativeUnit.DocumentId,
                                                    CreativeUnit = new CreativeUnitDto { Width = creativUnit.Width, Height = creativUnit.Height }
                                                };


                                                model.AdCreativeDto.VideoEndCardAdImages.Add(adCreativeUnit2);

                                            }
                                        }


                                        // model.AdCreativeDto.VideoEndCardAdImages == GetVideoCardCreatives(model.AdCreativeDto.VideoEndCardCreativeUnitsContent, "11");
                                    }
                                }

                                model.VideoEndCards = item.VideoEndCards;
                                model.VideoEndCardAdImages = GetVideoCardCreatives(null, "13");


                            }
                            else
                            {
                                model.AdCreativeDto.ImageUrls = GetCreativeUnitsByCriteria(null, DeviceTypeEnum.Any, null, "13").ToList();

                                model.VideoEndCardAdImages = GetVideoCardCreatives(null, "13");
                            }

                            ViewData["ImagesContainer"] = "13";
                            break;
                        }
                }
            }
            else
            {
                //to load the Creative View Model without set the Tile Image
                model = GetCreativeViewModel(-1);
                model.AdCreativeDto = item;

                model.AdBannerTypes = new List<SelectListItem>
                                {
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("SmartPhone", "Campaign"),
                                            Value = "Phone",
                                            Selected = true
                                        },
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("Tablet", "Campaign"),
                                            Value = "Tablet",
                                        }
                                };
                switch (item.Group.ActionTypeId)
                {
                    case AdActionTypeIds.RichMedia:
                        {
                            model.AdSubTypes = new List<SelectListItem>
                                {
                                    new SelectListItem()
                                        {
                                            Text =  ResourcesUtilities.GetResource("JavaScriptRichMedia", "Campaign") ,
                                            Value = "2",
                                            Selected = true
                                        }
                                       ,
                                      new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("HTML5RichMedia", "Campaign"),
                                            Value = "7",
                                        },
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("ExpandableRichMedia", "Campaign"),
                                            Value = "1",
                                        }
                                 
                                };
                            if (model.AdCreativeDto.ID<1 &&! _campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId,adGroupId))
                            {


                                model.AdSubTypes = new List<SelectListItem>
                                {

                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("ExpandableRichMedia", "Campaign"),
                                            Value = "1",
                                                Selected = true
                                        }

                                };
                            }
                            model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            model.AdCreativeDto.AdSubType = AdSubTypes.ExpandableRichMedia;
                            model.AdCreativeDto.AdBannerType = DeviceTypeEnum.SmartPhone;
                            GetRichMediaProtocol(model, item);

                            //get the Default Phone Creative Units
                            //model.PhoneCreativeUnits.Add((int)AdSubTypes.ExpandableRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia));
                            //model.PhoneCreativeUnits.Add((int)AdSubTypes.JavaScriptRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia));


                            //get the Default Tablet Creative Units
                            //model.TabletCreativeUnits.Add((int)AdSubTypes.ExpandableRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia));
                            //model.TabletCreativeUnits.Add((int)AdSubTypes.JavaScriptRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia));



                            var results1 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results1[0].DeviceType = DeviceTypeEnum.SmartPhone;
                            results1[0].Name = results1[0].Name.Replace("_0", "_1");
                            var results2 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results2[0].DeviceType = DeviceTypeEnum.SmartPhone;
                            results2[0].Name = results2[0].Name.Replace("_0", "_1");

                            var results6 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results6[0].DeviceType = DeviceTypeEnum.SmartPhone;
                            results6[0].Name = results6[0].Name.Replace("_0", "_1");


                            var results3 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results3[0].DeviceType = DeviceTypeEnum.Tablet;
                            results3[0].Name = results3[0].Name.Replace("_0", "_2");
                            var results4 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results4[0].DeviceType = DeviceTypeEnum.Tablet;
                            results4[0].Name = results4[0].Name.Replace("_0", "_2");
                            var results5 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            results5[0].DeviceType = DeviceTypeEnum.Tablet;
                            results5[0].Name = results5[0].Name.Replace("_0", "_2");

                            //get the Default Phone Creative Units
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.ExpandableRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia).ToList().Concat(results1));
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.JavaScriptRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia).Concat(results2));
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.HTML5RichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).ToList().Concat(results6));

                            //get the Default Tablet Creative Units
                            model.TabletCreativeUnits.Add((int)AdSubTypes.ExpandableRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.ExpandableRichMedia).Concat(results3));
                            model.TabletCreativeUnits.Add((int)AdSubTypes.JavaScriptRichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.JavaScriptRichMedia).Concat(results4));
                            model.TabletCreativeUnits.Add((int)AdSubTypes.HTML5RichMedia, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Concat(results5));



                           var listsCreatives= GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).Concat(results5).Concat(GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5RichMedia).ToList());
                            model.HTML5Creatives = new List<SelectListItem>() {   new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("Select", "Global"),
                            Value = "0",
                            Selected = true
                        }};
                            foreach (var listCreative in listsCreatives)
                            {
                                model.HTML5Creatives.Add( new SelectListItem() {


                                    Text = listCreative.DisplayText,
                                    Value = listCreative.CreativeUnitDto.ID.ToString(),
                                    Selected = false

                                });

                            }


                          break;
                        }
                    case AdActionTypeIds.Interstitial:
                        {

                            model.AdSubTypes = new List<SelectListItem>
                                {
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("JavaScriptInterstitial", "Campaign"),
                                            Value = "3",
                                            Selected = true
                                        },
                                        
                                    new SelectListItem()
                                        {
                                            Text =  ResourcesUtilities.GetResource("HTML5Interstitial", "Campaign"),
                                            Value = "8",
                                        },
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("ExternalUrlInterstitial", "Campaign"),
                                            Value = "4",
                                        }
                                
                                };

                            if (model.AdCreativeDto.ID < 1 && !_campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId, adGroupId))
                            {
                                model.AdSubTypes = new List<SelectListItem>
                                {
                                

                                };

                            }


                                model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            model.AdCreativeDto.AdSubType = AdSubTypes.JavaScriptInterstitial;
                            model.AdCreativeDto.AdBannerType = DeviceTypeEnum.SmartPhone;
                            GetRichMediaProtocol(model, item);
                          var results1=  GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.JavaScriptInterstitial).Where(m=>m.CreativeUnitDto.ID==8).ToList();
                            if (results1 != null && results1.Count > 0)
                            {
                                results1[0].DeviceType = DeviceTypeEnum.SmartPhone;
                                results1[0].Name = results1[0].Name.Replace("_0", "_1");
                            }
                            var results2 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.ExternalUrlInterstitial).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            if (results2 != null && results2.Count > 0)
                            {
                                results2[0].DeviceType = DeviceTypeEnum.SmartPhone;
                                results2[0].Name = results2[0].Name.Replace("_0", "_1");
                            }
                            var results3 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.JavaScriptInterstitial).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            if (results3 != null && results3.Count > 0)
                            {
                                results3[0].DeviceType = DeviceTypeEnum.Tablet;
                                results3[0].Name = results3[0].Name.Replace("_0", "_2");
                            }
                            var results4 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.RichMedia, AdSubTypes.ExternalUrlInterstitial).Where(m => m.CreativeUnitDto.ID == 8).ToList();
                            if (results4 != null && results4.Count > 0)
                            {
                                results4[0].DeviceType = DeviceTypeEnum.Tablet;
                                results4[0].Name = results4[0].Name.Replace("_0", "_2");
                            }
                            //get the Default Phone Creative Units
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.JavaScriptInterstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.JavaScriptInterstitial));
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.ExternalUrlInterstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.ExternalUrlInterstitial));
                            model.PhoneCreativeUnits.Add((int)AdSubTypes.HTML5Interstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial));


                            //get the Default Tablet Creative Units
                            model.TabletCreativeUnits.Add((int)AdSubTypes.JavaScriptInterstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.JavaScriptInterstitial));
                            model.TabletCreativeUnits.Add((int)AdSubTypes.ExternalUrlInterstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.ExternalUrlInterstitial));
                            model.TabletCreativeUnits.Add((int)AdSubTypes.HTML5Interstitial, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial));



                            var listsCreatives = GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial).Concat(GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.RichMedia, AdSubTypes.HTML5Interstitial).ToList());
                            model.HTML5Creatives = new List<SelectListItem>() {   new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("Select", "Global"),
                            Value = "0",
                            Selected = true
                        }};
                            foreach (var listCreative in listsCreatives)
                            {
                                model.HTML5Creatives.Add(new SelectListItem()
                                {


                                    Text = listCreative.DisplayText,
                                    Value = listCreative.CreativeUnitDto.ID.ToString(),
                                    Selected = false

                                });

                            }

                            break;
                        }
                    case AdActionTypeIds.VideoStreaming:
                        {
                            ViewData["VideoContainer"] = "VideoContainer";
                            model.AdCreativeDto.TypeId = AdTypeIds.InStreamVideo;
                            model.AdCreativeDto.AdSubType = AdSubTypes.VideoLinear;
                            model.InStreamVideos = GetInstreamVideoCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo, AdSubTypes.VideoLinear);// GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo);
                            model.TabletCreativeUnits.Add((int)AdTypeIds.InStreamVideo, model.InStreamVideos);

                            model.VideoDeliveryMethods = new List<SelectListItem>();
                            foreach (VideoDeliveryMethodDto videoDeliveryMethods in _videoDeliveryMethodsService.GetAll())
                            {
                                if (videoDeliveryMethods.Code == ((int)VideoDeliveryMethodType.Streaming).ToString())
                                {
                                    continue;

                                }
                                model.VideoDeliveryMethods.Add(
                                    new SelectListItem()
                                    {
                                        Value = videoDeliveryMethods.ID.ToString(),
                                        Text = videoDeliveryMethods.Name
                                    }
                                    );
                            }
                            AdGroupTrackingEventCriteriaDto adGroupTrackingEventCriteriaDto = new Services.Interfaces.DTOs.Campaign.AdGroupTrackingEventCriteriaDto { AdGroupId = adGroupId, CampaignId = campaignId };
                            var adGroupTrackingEvent = _campaignService.GetAdGroupTrackingEvents(adGroupTrackingEventCriteriaDto);

                            foreach (CreativeUnitViewModel unitViewModel in model.InStreamVideos)
                            {
                                unitViewModel.ImpressionTrackerRedirectList = new List<AdCreativeUnitTrackerDto>();
                                var adGroupEvents = adGroupTrackingEvent.Items.Where(p => p.Code.ToLower() != IMPRESSIONEVENT && p.Code.ToLower() != CLICKEVENT && !p.IsCustom);
                                foreach (AdGroupTrackingEventDto trackingEvent in adGroupEvents)
                                {
                                    unitViewModel.ImpressionTrackerRedirectList.Add(new AdCreativeUnitTrackerDto() { AdGroupEventId = trackingEvent.Id, Url = trackingEvent.Description });
                                    // = trackingEvent.Description;
                                }
                            }

                            model.VideoEndCards = item.VideoEndCards;
                            model.AdCreativeDto.AutoCloseWaitInSeconds = 7.5;


                            model.VideoEndCardAdImages = GetVideoCardCreatives(null, "13");
                            model.AdCreativeDto.ImageUrls = GetCreativeUnitsByCriteria(null, DeviceTypeEnum.Any, null, "13").ToList();
                            ViewData["ImagesContainer"] = "13";

                            // adGroupItem.CampaignId
                            break;
                        }
                    default:
                        {
                            if (item.TypeId > 0 && item.TypeId == AdTypeIds.TrackingAd)
                            {
                                model.AdTypes = new List<SelectListItem>
                                {

                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("Banner", "Campaign"),
                                            Value = "BannerCreative",
                                        }

                                };
                            }
                            else
                            {


                                model.AdTypes = new List<SelectListItem>
                                {
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("TextAd", "Campaign"),
                                            Value = "TextCreative",
                                            Selected = true
                                        },
                                    new SelectListItem()
                                        {
                                            Text = ResourcesUtilities.GetResource("Banner", "Campaign"),
                                            Value = "BannerCreative",
                                        }

                                };
                                if ((checkAdPermissions(PortalPermissionsCode.PlainHTML)&& _campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId, adGroupId)) || Config.IsAdOpsAdmin  )
                                {
                                    model.AdTypes.Add(new SelectListItem()
                                    {
                                        Text = ResourcesUtilities.GetResource("PlainHTML", "Campaign"),
                                        Value = "HTMLCreative",
                                    });
                                }

                            }

                            var results = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.Banner).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                            results[0].DeviceType = DeviceTypeEnum.SmartPhone;

                            results[0].Name = results[0].Name.Replace("1_0_0", "1_0_1");
                            //get the Default Phone AdCreativeUnits
                            model.PhoneCreativeUnits.Add((int)AdTypeIds.Banner, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.Banner).Concat(results));

                            //model.PhoneCreativeUnits.Add((int)AdTypeIds.Banner, );
                            var results2 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.Banner).Where(M => M.CreativeUnitDto.ID == 8).ToList();

                            results2[0].Name = results2[0].Name.Replace("1_0_0", "1_0_2");
                            results2[0].DeviceType = DeviceTypeEnum.Tablet;

                            //get the Default Tablet AdCreativeUnits
                            model.TabletCreativeUnits.Add((int)AdTypeIds.Banner, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.Banner).Concat(results2));


                            // Add Copy Feature to the first item in the phone banners only
                            model.PhoneCreativeUnits.Where(p => p.Key == (int)AdTypeIds.Banner).First().Value.First().ShowCopy = true;
                            var results3 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.PlainHTML).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                                  results3[0].DeviceType = DeviceTypeEnum.SmartPhone;
                            results3[0].Name = results3[0].Name.Replace("3_0_0", "3_0_1");
                            var results4 = GetCreativeUnitsViewModel(DeviceTypeEnum.Any, AdTypeIds.PlainHTML).Where(M => M.CreativeUnitDto.ID == 8).ToList();
                            results4[0].DeviceType = DeviceTypeEnum.Tablet;
                            results4[0].Name = results4[0].Name.Replace("3_0_0", "3_0_2");
                            //get the Default Plain HTML Phone Creative Units
                            model.PhoneCreativeUnits.Add((int)AdTypeIds.PlainHTML, GetCreativeUnitsViewModel(DeviceTypeEnum.SmartPhone, AdTypeIds.PlainHTML).Concat(results3));
                            //get the Default Plain HTML Tablet Creative Units
                            model.TabletCreativeUnits.Add((int)AdTypeIds.PlainHTML, GetCreativeUnitsViewModel(DeviceTypeEnum.Tablet, AdTypeIds.PlainHTML).Concat(results4));

                            model.NativeAdImages = GetNativeAdsCreatives(null, _nativeAdImagesGroup);
                            model.NativeAdIcons = GetNativeAdsCreatives(null, _nativeAdIconsGroup);
                            ViewData["IconsContainer"] = _nativeAdIconsGroup;
                            ViewData["ImagesContainer"] = _nativeAdImagesGroup;
                            model.AdCreativeDto.ShowIfInstalled = true;
                            //get the Default Tile Images
                            model.TileImageViewModel.TileImages = GetTileImages(item.AdActionId);
                            break;
                        }

                }

            }

            model.AdvertiserId = item.AdvertiserId;
            model.AdvertiserName = item.AdvertiserName;
            model.AdvertiserAccountId = item.AdvertiserAccountId;
            model.AdvertiserAccountName = item.AdvertiserAccountName;
            model.CampaignName = item.CampaignName;
            model.IsClientLocked = item.IsClientLocked;
            model.IsClientReadOnly = item.IsClientReadOnly;
            model.AdGroupName = item.AdGroupName;
            model.DiscountedBid = item.DiscountedBid;
            model.DiscountDto = item.DiscountDto;
            model.IsSecureCompliantRich = model.IsSecureCompliant = item.IsSecureCompliant;
            model.IsAllowedToSaveImpressionTracker = _campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId, adGroupId);
            return model;
        }

        protected IList<CreativeUnitViewModel> GetInstreamVideoAdCreativeUnits(int adGroupId, int campaignId, CreativeViewModel model, IList<AdCreativeUnitDto> adCreativeUnits)
        {
            IList<CreativeUnitViewModel> creativeUnitViewModelList = new List<CreativeUnitViewModel>();
            ViewData["VideoContainer"] = "VideoContainer";
            model.VideoTypes = new List<SelectListItem>();
            model.AdCreativeDto.TypeId = AdTypeIds.InStreamVideo;
            model.AdCreativeDto.AdSubType = AdSubTypes.VideoLinear;
            model.VideoDeliveryMethods = new List<SelectListItem>();
            foreach (VideoDeliveryMethodDto videoDeliveryMethods in _videoDeliveryMethodsService.GetAll())
            {
                if (videoDeliveryMethods.Code == ((int)VideoDeliveryMethodType.Streaming).ToString())
                {
                    continue;

                }
                model.VideoDeliveryMethods.Add(
                    new SelectListItem()
                    {
                        Value = videoDeliveryMethods.ID.ToString(),
                        Text = videoDeliveryMethods.Name
                    }
                    );
            }

            var creativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo, AdSubTypes.VideoLinear);
            model.AdCreativeDto.TypeId = AdTypeIds.InStreamVideo;
            model.AdCreativeDto.AdSubType = AdSubTypes.VideoLinear;
            var returnFiles = new List<CreativeUnitViewModel>();
            string displayText = GetInstreamVideUplaodDisplyMsg(creativeUnits);

            AdGroupTrackingEventCriteriaDto adGroupTrackingEventCriteriaDto = new Services.Interfaces.DTOs.Campaign.AdGroupTrackingEventCriteriaDto { AdGroupId = adGroupId, CampaignId = campaignId };
            var adGroupTrackingEvent = _campaignService.GetAdGroupTrackingEvents(adGroupTrackingEventCriteriaDto);

            var adGroupEvents = adGroupTrackingEvent.Items.Where(p => p.Code.ToLower() != IMPRESSIONEVENT && p.Code.ToLower() != CLICKEVENT && !p.IsCustom);
            // var defaulCreativeUnit= creativeUnits.Where(M => M.Code == "28").FirstOrDefault();
            var defaulCreativeUnit = _creativeUnitService.GetById(21);
            foreach (var creativeUnitDto in creativeUnits)
            {

                var adCreativeUnit = adCreativeUnits == null ? null : adCreativeUnits.FirstOrDefault(item => item.InStreamVideoCreativeUnit.OriginalCreativeUnitID == creativeUnitDto.ID);

                if (adCreativeUnit != null)
                {

                    var creativeUnitViewModel = new CreativeUnitViewModel()
                    {
                        DocumentId = adCreativeUnit == null ? (int?)null : adCreativeUnit.DocumentId,
                        Content = adCreativeUnit != null ? adCreativeUnit.Content : string.Empty,
                        DisplayText = creativeUnitDto.Name,
                        //string.Format("{0}x{1}", creativeUnitDto.Width, creativeUnitDto.Height),
                        CreativeUnitDto = creativeUnitDto,
                        DeviceType = DeviceTypeEnum.Any,
                        AdTypeId = (int)AdTypeIds.InStreamVideo,

                        Name = string.Format("CreativeUnit_{0}", creativeUnitDto.ID.ToString()),
                        //alid
                        ImpressionTrackerRedirectList = adCreativeUnit.InStreamVideoCreativeUnit.ImpressionTrackerRedirectList.ToList(),
                        InStreamVideoCreativeUnit = adCreativeUnit.InStreamVideoCreativeUnit
                    };
                    creativeUnitViewModel.CreativeUnitDto.Formats = defaulCreativeUnit.Formats;
                    foreach (AdGroupTrackingEventDto trackingEvent in adGroupEvents)
                    {
                        if (creativeUnitViewModel.ImpressionTrackerRedirectList.Where(x => x.AdGroupEventId == trackingEvent.Id).Count() <= 0)
                        {
                            creativeUnitViewModel.ImpressionTrackerRedirectList.Add(new AdCreativeUnitTrackerDto() { AdGroupEventId = trackingEvent.Id, Url = trackingEvent.Description });
                        }
                    }

                    returnFiles.Add(creativeUnitViewModel);
                }
            }
            foreach (var creativeUnit in returnFiles)
            {
                creativeUnit.DisplayText = displayText;
            }

            model.TabletCreativeUnits.Add((int)AdTypeIds.InStreamVideo, returnFiles);

            returnFiles = returnFiles.ToList().OrderByDescending(p => p.CreativeUnitDto.RequiredType).ToList();
            // returnFiles = returnFiles.OrderByDescending(p => p.CreativeUnitDto.Width * p.CreativeUnitDto.Height).ToList();
            return returnFiles;
        }

        private void GetRichMediaProtocol(CreativeViewModel model, AdCreativeDto item)
        {
            model.RichMediaProtocols = _richMediaRequiredProtocolService.GetAll();
            bool richMediaProtocolSelected = item.RichMediaRequiredProtocol == null;
            model.RichMediaRequiredProtocolsList = new List<SelectListItem>
                {
                    new SelectListItem()
                        {
                            Text =ResourcesUtilities.GetResource("NoneRichMediaRequiredProtocol","Campaign"),
                            Value = "0",
                            Selected = richMediaProtocolSelected
                        }
                };
            model.IsMandatory = item.IsMandatory;
            foreach (RichMediaRequiredProtocolDto richMediaRequiredProtocolDto in model.RichMediaProtocols)
            {
                model.RichMediaRequiredProtocolsList.Add(
                    new SelectListItem()
                    {
                        Selected =
                                item.RichMediaRequiredProtocol != null &&
                                item.RichMediaRequiredProtocol.ID == richMediaRequiredProtocolDto.ID,
                        Value = richMediaRequiredProtocolDto.ID.ToString(),
                        Text = richMediaRequiredProtocolDto.Name
                    }
                    );
            }
        }

        protected AdCreativeSaveDto GetAdCreativeSaveDto(CreativeSaveViewModel model, int adGroupId, int campaignId)
        {
            var isDownloadAction = model.AdCreativeDto.IsDownloadAction;


            var adCreativeSaveDto = new AdCreativeSaveDto
            {
                ID = model.AdCreativeDto.ID,
                IsAdPaused = model.AdCreativeDto.IsAdPaused,
                AdActionValue = model.AdCreativeDto.AdActionValue,
                AdText = model.AdCreativeDto.AdText,
                Bid = model.AdCreativeDto.Bid,
                AdBannerType = model.AdBannerTypeId,
                AdSubType = model.AdSubType,
                ClickTags = model.ClickTags,
                ThirdPartyTrackers = model.ThirdPartyTrackers,
                Name = model.AdCreativeDto.Name,
                TypeId = model.AdTypeId,
                TileImageId = model.TileImage,
                ClickMethod = model.ClickMethod,
                IsSecureCompliant = model.AdCreativeDto.IsSecureCompliant,
                OrientationType = model.OrientationType,
                EnvironmentType = model.EnvironmentType,
                Banners = new List<AdCreativeUnitDto>(),
                TileImageInformationList = new List<TileImageInformationDto>(),
                NativeAdIcons = new List<AdCreativeUnitDto>(),
                NativeAdImages = new List<AdCreativeUnitDto>(),
                VideoEndCards = new List<AdCreativeSaveDto>(),
                RichMediaRequiredProtocol = model.RichMediaRequiredProtocol,
                IsMandatory = model.IsMandatory,
                Description = model.AdCreativeDto.Description == null ? string.Empty : model.AdCreativeDto.Description,
                ActionText = model.AdCreativeDto.ActionText,
                AppUrl = isDownloadAction ? model.AdCreativeDto.AppUrl : null,
                ShowIfInstalled = isDownloadAction ? model.AdCreativeDto.ShowIfInstalled : false,
                StarRating = isDownloadAction ? model.AdCreativeDto.StarRating : new int?(),
                IsAdChanged = model.AdCreativeDto.IsAdChanged,
                CreativeVendorIds = model.CreativeVendorIds,
                AdCreativeVendorIds = model.AdCreativeVendorIds,
                IsCreativeVendorChanged = model.IsCreativeVendorChanged,
                IsVideo = model.AdCreativeDto.IsVideo

            };


            if (model.AdCreativeDto.AdActionValue != null)
            {
                if (Request.Form["ClickTrackers"] != null)
                {
                    model.AdCreativeDto.AdActionValue.Trackers = Request.Form["ClickTrackers"].Split(',')
                        .Select(p => new Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto() { URL = !string.IsNullOrEmpty(p) ? p.Trim() : string.Empty })
                         .Where(p => !string.IsNullOrEmpty(p.URL))
                        .ToList();

                }
            }

            adCreativeSaveDto.EnableEventsPostback = model.EnableEventsPostback;
            adCreativeSaveDto.VerifyTargetingCriteria = model.VerifyTargetingCriteria;


            adCreativeSaveDto.VerifyDailyBudget = model.VerifyDailyBudget;
            adCreativeSaveDto.VerifyCampaignStartAndEndDate = model.VerifyCampaignStartAndEndDate;
            adCreativeSaveDto.ValidateRequestDeviceAndLocationData = model.ValidateRequestDeviceAndLocationData;

            adCreativeSaveDto.UpdateEventsFrequency = model.UpdateEventsFrequency;

            adCreativeSaveDto.VerifyPrerequisiteEvents = model.VerifyPrerequisiteEvents;
            adCreativeSaveDto.UpdateTags = model.UpdateTags;
            adCreativeSaveDto.VerifyEventsFrequency = model.VerifyEventsFrequency;

            switch (model.AdTypeId)
            {
                case AdTypeIds.Text:
                    {
                        adCreativeSaveDto.OrientationType = OrientationType.Any;
                        adCreativeSaveDto.EnvironmentType = EnvironmentType.All;
                        //save text Creative
                        var suffix = string.Empty;
                        //custom tile Image

                        var item = _tileImageService.GetAllByAdAction(model.AdCreativeDto.AdActionId);
                        if (item == null)
                            return null;
                        foreach (var tileImageDocumentDto in item.Images)
                        {
                            if (tileImageDocumentDto.Document != null)
                            {
                                var tileInfo = tileImageDocumentDto.TileImageSize;
                                var creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}",
                                                                     (int)AdTypeIds.Text, "0",
                                                                     model.AdCreativeDto.AdActionId.ToString(),
                                                                     (int)tileInfo.ID);

                                var content = Request.Form[creativeUnitName];
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    int? docId = null;

                                    if (content.StartsWith(suffix + ","))
                                    {
                                        docId = Convert.ToInt32(content.Split(',')[2]);
                                    }
                                    else
                                    {
                                        docId = Convert.ToInt32(content);
                                    }

                                    var tileImageDocument = new TileImageDocumentDto
                                    {
                                        TileImageSize = new TileImageSizeDto() { ID = Convert.ToInt32(tileInfo.ID) },
                                        Document = new DocumentBaseDto { ID = Convert.ToInt32(docId) }
                                    };

                                    string impressionTrackerName = string.Format("{0}-ImpressionTrackerRedirect", creativeUnitName);
                                    string impressionTracker = null;

                                    if (Request.Form[impressionTrackerName] != null)
                                    {
                                        impressionTracker = Request.Form[impressionTrackerName];
                                    }

                                    TileImageInformationDto tileInformationDto = new TileImageInformationDto()
                                    {
                                        TileImage = tileImageDocument,
                                        ImpressionTrackerRedirect = impressionTracker
                                    };

                                    adCreativeSaveDto.TileImageInformationList.Add(tileInformationDto);
                                }

                            }
                        }
                        break;
                    }
                case AdTypeIds.NativeAd:
                    {
                        var iconsCreativeUnits = GetCreativeUnits(0, AdTypeIds.NativeAd, null, _nativeAdIconsGroup);
                        foreach (var creativeUnit in iconsCreativeUnits)
                        {
                            var creativeUnitName = string.Format("CreativeUnit_{0}_{1}", _nativeAdIconsGroup, creativeUnit.ID.ToString());
                            var adCreativeUnit = new AdCreativeUnitDto
                            {
                                CreativeUnitId = creativeUnit.ID,
                                Content = Request.Form[creativeUnitName]
                            };
                            if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                            {
                                adCreativeSaveDto.NativeAdIcons.Add(adCreativeUnit);
                            }
                        }

                        var imagesCreativeUnits = GetCreativeUnits(0, AdTypeIds.NativeAd, null, _nativeAdImagesGroup);
                        foreach (var creativeUnit in imagesCreativeUnits)
                        {
                            var creativeUnitName = string.Format("CreativeUnit_{0}_{1}", _nativeAdImagesGroup, creativeUnit.ID.ToString());
                            var adCreativeUnit = new AdCreativeUnitDto
                            {
                                CreativeUnitId = creativeUnit.ID,
                                Content = Request.Form[creativeUnitName]
                            };
                            if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                            {
                                adCreativeSaveDto.NativeAdImages.Add(adCreativeUnit);
                            }
                        }

                        var impressionTrackerRedirect = Request.Form["ImpressionTrackerRedirect"];
                        var impressionTrackerJSRedirect = Request.Form["ImpressionTrackerJSRedirect"];
                      
                        if (!string.IsNullOrEmpty(impressionTrackerRedirect) || !string.IsNullOrEmpty(impressionTrackerJSRedirect))
                        {
                            adCreativeSaveDto.Banners.Add(new AdCreativeUnitDto() { ImpressionTrackerRedirect = impressionTrackerRedirect, ImpressionTrackerJSRedirect= impressionTrackerJSRedirect });
                        }
                    }
                    break;
                case AdTypeIds.Banner:
                    {
                        adCreativeSaveDto.OrientationType = OrientationType.Any;
                        //get document Ids
                        //get Creative Unit Content
                        var creativeUnits = GetCreativeUnits(model.AdBannerTypeId.Value, model.AdTypeId).ToList();
                      var results=  GetCreativeUnits(DeviceTypeEnum.Any, model.AdTypeId).Where(M => M.ID == 8).ToList();

                        creativeUnits.AddRange(results);
                        foreach (var creativeUnit in creativeUnits)
                        {
                            //TODO:we should get the creative value using model binder , not Request.Form
                            if (
                                (adCreativeSaveDto.EnvironmentType == EnvironmentType.All || (creativeUnit.EnvironmentType == EnvironmentType.All || creativeUnit.EnvironmentType == model.EnvironmentType)) &&
                                (adCreativeSaveDto.OrientationType == OrientationType.Any || (creativeUnit.OrientationType == OrientationType.Any || creativeUnit.OrientationType == model.OrientationType)))
                            {
                                var creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.AdTypeId, "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());
                                string impressionTrackerName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}-ImpressionTrackerRedirect", (int)model.AdTypeId, "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());

                                string impressionTrackerJSName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}-ImpressionTrackerJSRedirect", (int)model.AdTypeId, "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());
                                var adCreativeUnit = new AdCreativeUnitDto
                                {
                                    CreativeUnitId = creativeUnit.ID,
                                    Content = Request.Form[creativeUnitName],
                                    ImpressionTrackerRedirect = Request.Form[impressionTrackerName],
                                    ImpressionTrackerJSRedirect = Request.Form[impressionTrackerJSName]
                                };
                                if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                                {
                                    adCreativeSaveDto.Banners.Add(adCreativeUnit);
                                }
                            }
                        }

                        break;
                    }
                case AdTypeIds.TrackingAd:
                    {
                        adCreativeSaveDto.OrientationType = OrientationType.Any;



                        adCreativeSaveDto.AppMarketingPartnerId = model.AdCreativeDto.AppMarketingPartnerId;

                        adCreativeSaveDto.ClickTrackerUrl = string.Empty;

                        adCreativeSaveDto.AdBannerType = DeviceTypeEnum.Any;
                        //get document Ids
                        //get Creative Unit Content
                        //var creativeUnits = GetCreativeUnits(adCreativeSaveDto.AdBannerType.Value, model.AdTypeId).ToList();
                        //foreach (var creativeUnit in creativeUnits)
                        //{
                        //    //TODO:we should get the creative value using model binder , not Request.Form
                        //    if (
                        //        (adCreativeSaveDto.EnvironmentType == EnvironmentType.All || (creativeUnit.EnvironmentType == EnvironmentType.All || creativeUnit.EnvironmentType == model.EnvironmentType)) &&
                        //        (adCreativeSaveDto.OrientationType == OrientationType.Any || (creativeUnit.OrientationType == OrientationType.Any || creativeUnit.OrientationType == model.OrientationType)))
                        //    {
                        //        var creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.AdTypeId, "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());
                        //        string impressionTrackerName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}-ImpressionTrackerRedirect", (int)model.AdTypeId, "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());
                        //        var adCreativeUnit = new AdCreativeUnitDto
                        //        {
                        //            CreativeUnitId = creativeUnit.ID,
                        //            Content = Request.Form[creativeUnitName],
                        //            ImpressionTrackerRedirect = Request.Form[impressionTrackerName]
                        //        };
                        //        if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                        //        {
                        //            adCreativeSaveDto.Banners.Add(adCreativeUnit);
                        //        }
                        //    }
                        //}

                        break;
                    }
                case AdTypeIds.PlainHTML:
                case AdTypeIds.RichMedia:
                    {
                        var errors = new List<ErrorData>();
                        //if (model.AdTypeId == AdTypeIds.RichMedia)
                        //{
                        //    //Only Admin in Admin App can create and update Rich Media Ads
                        //    if (!Config.IsAdministrationApp)
                        //    {
                        //        throw new NotAuthenticatedException();
                        //    }
                        //}
                        //get Creative Unit Content
                        var creativeUnits = GetCreativeUnits(model.AdBannerTypeId.Value, model.AdTypeId, model.AdSubType).ToList();
                         creativeUnits.AddRange( GetCreativeUnits(DeviceTypeEnum.Any, model.AdTypeId, model.AdSubType).Where(M=>M.ID==8).ToList());
                        foreach (var creativeUnit in creativeUnits)
                        {
                            //TODO:we should get the creative value using model binder , not Request.Form
                            if ((adCreativeSaveDto.EnvironmentType == EnvironmentType.All || (creativeUnit.EnvironmentType == EnvironmentType.All || creativeUnit.EnvironmentType == model.EnvironmentType)) &&
                                (adCreativeSaveDto.OrientationType == OrientationType.Any || (creativeUnit.OrientationType == OrientationType.Any || creativeUnit.OrientationType == model.OrientationType)))
                            {
                                var creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.AdTypeId, model.AdSubType.HasValue ? ((int)model.AdSubType.Value).ToString() : "0", (int)model.AdBannerTypeId.Value, creativeUnit.ID.ToString());
                                var adCreativeUnit = new AdCreativeUnitDto
                                {
                                    CreativeUnitId = creativeUnit.ID,
                                    Content = Request.Form[creativeUnitName]
                                };

                                // Copy same size with same content
                                if (string.IsNullOrEmpty(adCreativeUnit.Content) && creativeUnit.OrientationReplacementId.HasValue)
                                {
                                    CreativeUnitDto replacementCreativeUnit = creativeUnits.Where(p => p.AdSupportedId == creativeUnit.OrientationReplacementId).SingleOrDefault();

                                    if (replacementCreativeUnit != null)
                                    {
                                        string replacementContent = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.AdTypeId, model.AdSubType.HasValue ? ((int)model.AdSubType.Value).ToString() : "0", (int)model.AdBannerTypeId.Value, replacementCreativeUnit.ID.ToString());
                                        var content = Request.Form[replacementContent];

                                        if (!string.IsNullOrEmpty(content))
                                        {
                                            adCreativeUnit.Content = content;
                                        }
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                                {
                                    var isValid = true;
                                    if (model.AdTypeId == AdTypeIds.RichMedia)
                                    {
                                        switch (model.AdSubType)
                                        {
                                            case AdSubTypes.JavaScriptInterstitial:
                                            case AdSubTypes.JavaScriptRichMedia:
                                                {
                                                    isValid = IsFormattedAdCreativeContent(adCreativeUnit.Content);
                                                    break;
                                                }
                                        }
                                    }
                                    else if (model.AdTypeId == AdTypeIds.PlainHTML)
                                    {
                                        isValid = IsFormattedAdCreativeContent(adCreativeUnit.Content);
                                    }
                                    if (!isValid)
                                    {
                                        throw new BusinessException()
                                        {
                                            Errors = new List<ErrorData>()
                                                    {
                                                        new ErrorData()
                                                            {
                                                                ID ="MissingAdTags"
                                                            }
                                                    }
                                        };
                                    }

                                  
                                    adCreativeSaveDto.Banners.Add(adCreativeUnit);
                                }
                            }
                        }

                        if (model.SelectedHTML5DocumentId > 0 && model.SelectedHTML5CreativeId > 0)
                        {

                            var adCreativeUnit2 = new AdCreativeUnitDto
                            {
                                CreativeUnitId = model.SelectedHTML5CreativeId,
                                Content = model.SelectedHTML5DocumentId.ToString()
                            };

                            adCreativeSaveDto.Banners.Add(adCreativeUnit2);
                        }

                        break;
                    }
                case AdTypeIds.InStreamVideo:
                    {
                        var creativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo, AdSubTypes.VideoLinear);

                        int vidoeDocId = 0; int thumbnailDocId = 0; int creativeUnitId = 0;// = creativeUnits.FirstOrDefault().ID;
                        int videoHeight = 0; int vidoeWidth = 0; int bitRate = 0; int vidoeDuration = 0; string videoFormat = string.Empty;
                        adCreativeSaveDto.InStreamVideos = new List<AdCreativeUnitDto>();
                        //  var creativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, model.AdTypeId, model.AdSubType);

                        var videoAdCreativeUnit = model.AdCreativeDto.CreativeUnitsContent != null ? model.AdCreativeDto.CreativeUnitsContent.FirstOrDefault() : null;
                        //foreach (var creativeUnit in creativeUnits)
                        //{
                        var creativeUnitContent = string.Format("CreativeUnitId");
                        var vidoeDocIdContent = string.Format("CreativeUnit_VidoeDocId");//_{0}", creativeUnit.ID);
                        var thumbnailDocIdContent = string.Format("CreativeUnit_ThumbnailDocId");//_{0}", creativeUnit.ID);
                        var videoHeightContent = string.Format("CreativeUnit_VideoHeight");//_{0}", creativeUnit.ID);
                        var videoWidthContent = string.Format("CreativeUnit_VideoWidth");//_{0}", creativeUnit.ID);
                        var videoDurationContent = string.Format("CreativeUnit_Duration");//_{0}", creativeUnit.ID);
                        var bitRateContent = string.Format("CreativeUnit_BitRate");//_{0}", creativeUnit.ID);
                        var bitVideoContent = string.Format("CreativeUnit_VideoFormat");//_{0}", creativeUnit.ID);

                        //if (!int.TryParse(Request.Form[vidoeDocIdContent], out vidoeDocId))
                        //{
                        //    continue;
                        //}
                        int.TryParse(Request.Form[creativeUnitContent], out creativeUnitId);
                        int.TryParse(Request.Form[vidoeDocIdContent], out vidoeDocId);
                        int.TryParse(Request.Form[thumbnailDocIdContent], out thumbnailDocId);

                        int.TryParse(Request.Form[videoHeightContent], out videoHeight);
                        int.TryParse(Request.Form[videoWidthContent], out vidoeWidth);
                        int.TryParse(Request.Form[videoDurationContent], out vidoeDuration);
                        int.TryParse(Request.Form[bitRateContent], out bitRate);

                        int videoType;
                        if (string.IsNullOrEmpty(Request.Form[bitVideoContent]))
                        {
                            videoType = _campaignService.GetMIMEType("video/mp4");
                        }
                        else
                        {
                            videoType = _campaignService.GetMIMEType(Request.Form[bitVideoContent]);

                        }
                        var adCreativeUnit = new AdCreativeUnitDto
                        {
                            CreativeUnitId = creativeUnitId,
                            DocumentId = vidoeDocId,
                            InStreamVideoCreativeUnit = new InStreamVideoCreativeUnitDto()
                            {
                                ThumbnailDocId = thumbnailDocId,
                                VideoType = videoType, //videoAdCreativeUnit.InStreamVideoCreativeUnit.VideoType,
                                DeliveryMethod = videoAdCreativeUnit != null ? videoAdCreativeUnit.InStreamVideoCreativeUnit.DeliveryMethod : 2,
                                VideoWidth = vidoeWidth,
                                VideoHeight = videoHeight,
                                VideoDuration = vidoeDuration,
                                BitRate = bitRate,
                                Xml = model.AdCreativeDto.Xml,
                                XmlUrl = model.AdCreativeDto.XMlUrl,
                                IsXmlUrl = model.AdCreativeDto.IsXmlUrl,
                                IsVideo = model.AdCreativeDto.IsVideo,
                                Vpaid = model.AdCreativeDto.IsVpaid,
                                Vpaid_1 = model.AdCreativeDto.Vpaid_1,
                                Vpaid_2 = model.AdCreativeDto.Vpaid_2,
                            }
                        };

                        SetInstreamVideoTrackingEventsUrls(adGroupId, campaignId, creativeUnitId, adCreativeUnit.InStreamVideoCreativeUnit, model.AdCreativeDto.IsVideo);
                        adCreativeSaveDto.InStreamVideos.Add(adCreativeUnit);
                        // }





                        //get document Ids
                        //get Creative Unit Content
                        var videoEndCardCreativeUnits = GetCreativeUnits(DeviceTypeEnum.Any, AdTypeIds.VideoEndCard, AdSubTypes.VideoEndCard, "13").ToList();

                        adCreativeSaveDto.VideoEndCardFluidURL = model.AdCreativeDto.VideoEndCardFluidURL;
                        adCreativeSaveDto.VideoEndCardFluid = model.AdCreativeDto.CardType == VideoEndCardType.Static ? false : model.AdCreativeDto.VideoEndCardFluid;
                        adCreativeSaveDto.VideoEndCards = model.VideoEndCards;
                        adCreativeSaveDto.VideoEndCardCreativeUnitsContent = model.AdCreativeDto.VideoEndCardCreativeUnitsContent;
                        adCreativeSaveDto.CardType = model.AdCreativeDto.CardType;
                        if (model.AdCreativeDto.IsStatic)
                        {

                            adCreativeSaveDto.CardType = VideoEndCardType.Static;
                        }
                        else
                        {
                            adCreativeSaveDto.CardType = VideoEndCardType.Dynamic;

                        }
                        adCreativeSaveDto.AutoCloseWaitInSeconds = model.AdCreativeDto.EnableAutoClose ? model.AdCreativeDto.AutoCloseWaitInSeconds : 7.5;
                        adCreativeSaveDto.EnableAutoClose = model.AdCreativeDto.EnableAutoClose;
                        adCreativeSaveDto.AdActionValueVideoEndCardURL = adCreativeSaveDto.CardType == VideoEndCardType.Dynamic ? "" : model.AdCreativeDto.AdActionValueVideoEndCardURL;

                        adCreativeSaveDto.AdActionValueVideoEndCard = model.AdCreativeDto.AdActionValueVideoEndCard;
                        if (adCreativeSaveDto.AdActionValueVideoEndCard == null && !string.IsNullOrEmpty(adCreativeSaveDto.AdActionValueVideoEndCardURL))

                        {

                            adCreativeSaveDto.AdActionValueVideoEndCard = new AdActionValueDto();
                            adCreativeSaveDto.AdActionValueVideoEndCard.Value = adCreativeSaveDto.AdActionValueVideoEndCardURL;
                            adCreativeSaveDto.AdActionValueVideoEndCard.Value2 = adCreativeSaveDto.AdActionValueVideoEndCardURL;
                        }
                        if (adCreativeSaveDto.AdActionValueVideoEndCard != null)
                        {

                            if (!string.IsNullOrWhiteSpace(adCreativeSaveDto.AdActionValueVideoEndCard.Value2))
                                adCreativeSaveDto.AdActionValueVideoEndCardURL = adCreativeSaveDto.CardType == VideoEndCardType.Dynamic ? "" : adCreativeSaveDto.AdActionValueVideoEndCard.Value2.Trim();
                            else
                                adCreativeSaveDto.AdActionValueVideoEndCardURL = string.Empty;
                            // adfalcondummy.com
                            if (adCreativeSaveDto.AdActionValueVideoEndCardURL == "http://adfalcondummy.com")
                            {

                                adCreativeSaveDto.AdActionValueVideoEndCardURL = string.Empty;
                            }

                            if (adCreativeSaveDto.AdActionValueVideoEndCard.Trackers != null && adCreativeSaveDto.AdActionValueVideoEndCard.Trackers.Count > 0)
                                adCreativeSaveDto.VideoEndCardsTrackingURL = adCreativeSaveDto.AdActionValueVideoEndCard.Trackers.Select(X => X.URL).ToList();

                            if (adCreativeSaveDto.AdActionValueImpressionTracker != null && adCreativeSaveDto.AdActionValueImpressionTracker.Trackers != null && adCreativeSaveDto.AdActionValueImpressionTracker.Trackers.Count > 0)
                                adCreativeSaveDto.ImpressionTrackingURL = adCreativeSaveDto.AdActionValueImpressionTracker.Trackers.Select(X => X.URL).ToList();

                        }

                        if (Request.Form["EndCardClickTracker"] != null)
                        {
                            adCreativeSaveDto.VideoEndCardsTrackingURL = Request.Form["EndCardClickTracker"].Split(',')

                                .ToList();

                        }


                        if (Request.Form["ImpressionClickTracker"] != null)
                        {
                            adCreativeSaveDto.ImpressionTrackingURL = Request.Form["ImpressionClickTracker"].Split(',')

                                .ToList();

                        }

                        if ((model.AdCreativeDto.ImageUrls != null || model.AdCreativeDto.VideoEndCardAdImages != null || adCreativeSaveDto.CardType == VideoEndCardType.Static) && !model.AdCreativeDto.VideoEndCardFluid)

                        {
                            adCreativeSaveDto.VideoEndCards = new List<AdCreativeSaveDto>();

                            adCreativeSaveDto.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                            if (adCreativeSaveDto.CardType == VideoEndCardType.Dynamic)
                            {
                                if (model.AdCreativeDto.ImageUrls != null && model.AdCreativeDto.ImageUrls.Count > 0)
                                {


                                    foreach (var imgURL in model.AdCreativeDto.ImageUrls)
                                    {
                                        if (!string.IsNullOrEmpty(imgURL.Url))
                                        {
                                            var creativUnit = videoEndCardCreativeUnits.Where(M => M.Width == imgURL.Width && M.Height == imgURL.Height).SingleOrDefault();

                                            adCreativeSaveDto.VideoEndCardCreativeUnitsContent.Add(new AdCreativeUnitDto { CreativeUnitId = creativUnit.ID, Content = imgURL.Url });
                                        }
                                    }
                                }

                            }
                            else
                            {





                                foreach (var creativeUnit in videoEndCardCreativeUnits)
                                {
                                    var creativeUnitName = string.Format("CreativeUnit_{0}_{1}", "13", creativeUnit.ID.ToString());

                                    var adCreativeUnit2 = new AdCreativeUnitDto
                                    {
                                        CreativeUnitId = creativeUnit.ID,
                                        Content = Request.Form[creativeUnitName]
                                    };


                                    if (!string.IsNullOrWhiteSpace(adCreativeUnit2.Content))
                                    {
                                        adCreativeSaveDto.VideoEndCardCreativeUnitsContent.Add(adCreativeUnit2);
                                    }

                                }
                            }
                            foreach (var creativeUnit in adCreativeSaveDto.VideoEndCardCreativeUnitsContent)
                            {
                                var videEndCard = new AdCreativeSaveDto();

                                if (!string.IsNullOrEmpty(adCreativeSaveDto.AdActionValueVideoEndCardURL))
                                {
                                    videEndCard.AdActionValue = new AdActionValueDto();
                                    videEndCard.AdActionValue.Value = adCreativeSaveDto.AdActionValueVideoEndCardURL;

                                }

                                videEndCard.CardType = adCreativeSaveDto.CardType;
                                videEndCard.AutoCloseWaitInSeconds = adCreativeSaveDto.AutoCloseWaitInSeconds;
                                videEndCard.EnableAutoClose = adCreativeSaveDto.EnableAutoClose;
                                videEndCard.CreativeUnitsContent = new List<AdCreativeUnitDto>();

                                videEndCard.CreativeUnitsContent.Add(creativeUnit);

                                adCreativeSaveDto.VideoEndCards.Add(videEndCard);
                            }

                        }

                        else if (adCreativeSaveDto.VideoEndCardFluid)
                        {

                            adCreativeSaveDto.VideoEndCards = new List<AdCreativeSaveDto>();

                            foreach (var creativeUnit in videoEndCardCreativeUnits)
                            {
                                var videEndCard = new AdCreativeSaveDto();

                                if (!string.IsNullOrEmpty(adCreativeSaveDto.AdActionValueVideoEndCardURL))
                                {
                                    videEndCard.AdActionValue = new AdActionValueDto();
                                    videEndCard.AdActionValue.Value = adCreativeSaveDto.AdActionValueVideoEndCardURL;

                                }

                                videEndCard.CardType = adCreativeSaveDto.CardType;
                                videEndCard.AutoCloseWaitInSeconds = adCreativeSaveDto.AutoCloseWaitInSeconds;
                                videEndCard.EnableAutoClose = adCreativeSaveDto.EnableAutoClose;
                                videEndCard.CreativeUnitsContent = new List<AdCreativeUnitDto>();

                                AdCreativeUnitDto creativeUnitD = new AdCreativeUnitDto();

                                creativeUnitD.Content = adCreativeSaveDto.VideoEndCardFluidURL;
                                creativeUnitD.CreativeUnitId = creativeUnit.ID;

                                videEndCard.CreativeUnitsContent.Add(creativeUnitD);

                                adCreativeSaveDto.VideoEndCards.Add(videEndCard);
                            }


                        }
                        else
                        {
                            adCreativeSaveDto.VideoEndCards = new List<AdCreativeSaveDto>();

                        }



                        break;
                    }

            }
            return adCreativeSaveDto;
        }

        private void SetInstreamVideoTrackingEventsUrls(int adGroupId, int campaignId, int adCreativeUnitId, InStreamVideoCreativeUnitDto inStreamVideoCreativeUnitDto, bool isvideo = false)
        {
            AdGroupTrackingEventCriteriaDto adGroupTrackingEventCriteriaDto = new Services.Interfaces.DTOs.Campaign.AdGroupTrackingEventCriteriaDto { AdGroupId = adGroupId, CampaignId = campaignId };
            var adGroupTrackingEvent = _campaignService.GetAdGroupTrackingEvents(adGroupTrackingEventCriteriaDto);
            var trackingEventUrls = string.Empty;
            IList<AdActionValueTrackerDto> trackers = null;
            inStreamVideoCreativeUnitDto.ImpressionTrackerRedirectList = new List<AdCreativeUnitTrackerDto>();
            var adGroupdEvents = adGroupTrackingEvent.Items.Where(p => p.Code.ToLower() != IMPRESSIONEVENT && p.Code.ToLower() != CLICKEVENT && !p.IsCustom);
            foreach (AdGroupTrackingEventDto trackingEvent in adGroupdEvents)
            {
                trackers = Request.Form["ClickTrackers" + trackingEvent.Id].Split(',')
                        .Select(p =>
                            new Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto() { URL = !string.IsNullOrEmpty(p) ? p.Trim() : string.Empty })
                    .Where(p => !string.IsNullOrEmpty(p.URL))
                        .ToList();
                // unitViewModel.ImpressionTrackerRedirectList.Add(new AdCreativeUnitTrackerDto() { AdGroupEventId = trackingEvent.Id, Url = trackingEvent.Description });

                //if (trackers.Count <= 0)
                //{
                //    trackers.Add(new AdActionValueTrackerDto() { URL = string.Empty });
                //}
                //if (inStreamVideoCreativeUnitDto.ImpressionTrackerRedirectList.Where(x => x.AdGroupEventId == trackingEvent.Id).FirstOrDefault() != null)
                //{
                inStreamVideoCreativeUnitDto.ImpressionTrackerRedirectList.Add(
                    new AdCreativeUnitTrackerDto()
                    {
                        AdGroupEventId = trackingEvent.Id,
                        AdCreativeUnitId = adCreativeUnitId,
                        ImpressionURls = trackers
                    }
                );
                //    }
            }

        }
        [ValidateInput(false)]
        public bool IsFormattedAdCreativeContent(string content)
        {
            
            return _campaignService.IsFormatedAdCreativUnit(content);
        }
        #endregion
        public virtual ActionResult Creative(int id, int? adTypeId, int adGroupId, int? adId)
        {
            int campaignId = id;
            string advertisrName = string.Empty;

            var model = GetCreativeViewModel(campaignId, adGroupId, adId, adTypeId);
            if (adTypeId.HasValue)
            {
                model.AdCreativeDto.TypeId = (AdTypeIds)adTypeId.Value;
                model.AdCreativeDto.Group.TypeId = (AdTypeIds)adTypeId.Value;
            }
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>();
            if (adId.HasValue)
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                    Text =model.AdCreativeDto.Name,
                                                      Order = 6,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId})
                                              },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 3,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 2,
                                                         Url=model.AdvertiserAccountId>0 ? Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=model.AdvertiserAccountId,id=id}) :Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"),"SiteMapLocalizations"),
                                                        Url = model.AdvertiserAccountId>0?   Url.Action("Index", new {  AdvertiseraccId=model.AdvertiserAccountId}):Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };

            }
            else
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("NewAd", "SiteMapLocalizations"),
                                                      Order = 6,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId})
                                              },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.AdGroupName,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 3,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                     Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 2,
                                                             Url=model.AdvertiserAccountId>0 ? Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=model.AdvertiserAccountId,id=id}) :Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"),"SiteMapLocalizations"),
                                                         Url = model.AdvertiserAccountId>0?   Url.Action("Index", new {  AdvertiseraccId=model.AdvertiserAccountId}):Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };



            }
            if (model.AdvertiserAccountId > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = model.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers"),
                                           ExtensionDropDown = true,
                                           Order = -2,
                                       });
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            var viewName = string.Empty;
            string returnlUrl = string.Empty;

            if (adId.HasValue)
            {
                returnlUrl = Url.Action("Ads", "Campaign", new { id = campaignId, adGroupId = adGroupId });
            }
            else
            {
                returnlUrl = Url.Action("Ads", "Campaign", new { id = campaignId, adGroupId = adGroupId, message = "notauthenticated" });
            }
            if (model.AdCreativeDto.Group.TypeId == AdTypeIds.TrackingAd || model.AdCreativeDto.TypeId == AdTypeIds.TrackingAd)
            {
                if (model.AdCreativeDto.ID == 0)
                {
                    var appMarketingPartners = _appMarketingPartnerServic.GetAll();
                    var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };
                    var appMarketingPartnersList = new List<SelectListItem>();
                    appMarketingPartnersList.Add(optionalItem);
                    appMarketingPartnersList.AddRange(appMarketingPartners.Select(item => new SelectListItem
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name.Value
                    }));
                    model.AppMarketingPartners = appMarketingPartnersList;
                }
            }

            if (model.AdCreativeDto.Group.TypeId.HasValue)
            {
                //if (!Config.IsAdministrationApp)
                //{
                //    if (adId.HasValue)
                //    {
                //        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                //    }
                //    else
                //    {
                //        return Redirect(returnlUrl);
                //    }
                //}
                viewName = model.AdCreativeDto.Group.TypeId.Value.ToString() + "Creative";
                model.AdCreativeDto.TypeId = model.AdCreativeDto.Group.TypeId.Value;
                model.AdCreativeDto.IsDownloadAction = IsDownloadAction(model.AdCreativeDto.AdActionId);

                if (model.AdCreativeDto.Group.TypeId == AdTypeIds.TrackingAd || model.AdCreativeDto.TypeId == AdTypeIds.TrackingAd)
                {
                    viewName = "TrackingAdCreative";
                }
            }
            else
            {
                switch (model.AdCreativeDto.Group.ActionTypeId)
                {
                    case AdActionTypeIds.RichMedia:
                        {
                            //if (!Config.IsAdministrationApp)
                            //{
                            //    if (adId.HasValue)
                            //    {
                            //        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                            //    }
                            //    else
                            //    {
                            //        return Redirect(returnlUrl);
                            //    }
                            //}
                            viewName = "RichMediaCreative";
                            model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            break;
                        }
                    case AdActionTypeIds.Interstitial:
                        {
                            //if (!Config.IsAdministrationApp)
                            //{
                            //    if (adId.HasValue)
                            //    {
                            //        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                            //    }
                            //    else
                            //    {
                            //        return Redirect(returnlUrl);
                            //    }
                            //}
                            viewName = "InterstitialCreative";
                            model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            if(!adId.HasValue || adId == 0)
                                model.IsMandatory = true;
                            break;
                        }
                    case AdActionTypeIds.VideoStreaming:
                        {
                            //if (!Config.IsAdministrationApp)
                            //{
                            //    if (adId.HasValue)
                            //    {
                            //        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl });
                            //    }
                            //    else
                            //    {
                            //        return Redirect(returnlUrl);
                            //    }
                            //}
                            viewName = "InStreamVideoCreative";
                            model.AdCreativeDto.TypeId = AdTypeIds.InStreamVideo;


                            model.AdCreativeDto.AdSubType = AdSubTypes.VideoLinear;
                            if (!adId.HasValue)
                                model.AdCreativeDto.IsVideo = true;

                            //if(!adId.HasValue)
                            //model.AdCreativeDto.IsVideo = true;

                            //if (!adId.HasValue)
                            //    model.AdCreativeDto.IsVideo = true;

                            break;
                        }
                    default:
                        {
                            if (model.AdCreativeDto.Group.TypeId == AdTypeIds.TrackingAd || model.AdCreativeDto.TypeId == AdTypeIds.TrackingAd)
                            {


                                viewName = "TrackingAdCreative";
                                break;
                            }
                            viewName = "Creative";
                            break;
                        }

                }
            }
            ChangeJavaScriptSet("adCreativeActionJs");
            ViewData["VideoContainer"] = "VideoContainer";
            return View(viewName, model); ;
        }


        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]
        public virtual ActionResult Creative(int id, int adGroupId, int? adId, CreativeSaveViewModel model, string returnUrl)
        {
            int campaignId = id;
            CreativeViewModel viewModel = null;



            bool isUrlRequired = true;
            if (model.AdTypeId == AdTypeIds.PlainHTML || model.AdTypeId == AdTypeIds.RichMedia)
            {
                if (model.AdTypeId == AdTypeIds.RichMedia && model.AdSubType.Value == AdSubTypes.JavaScriptRichMedia)
                {
                    isUrlRequired = false;
                }
                else if (model.AdTypeId == AdTypeIds.RichMedia &&( model.AdSubType.Value == AdSubTypes.HTML5RichMedia || model.AdSubType.Value == AdSubTypes.HTML5Interstitial || model.AdSubType.Value == AdSubTypes.JavaScriptInterstitial || model.AdSubType.Value == AdSubTypes.ExternalUrlInterstitial))
                {
                    isUrlRequired = false;

                }

                else
                {
                    if (model.AdTypeId == AdTypeIds.PlainHTML)
                    {
                        isUrlRequired = false;
                    }
                }
            }

           // if (true)
            {
                var test = ModelState.Values.Where(p => p.Errors.Count() != 0).ToList();
                if (ModelState.IsValid || (!isUrlRequired && ModelState.Values.Where(p => p.Errors.Count() != 0).Count() == 1))
                {
                    try
                    {
                        var adCreative = GetAdCreativeSaveDto(model, adGroupId, id);

                        if (adCreative.AdActionValue != null && (string.IsNullOrEmpty(adCreative.AdActionValue.Value) && string.IsNullOrEmpty(adCreative.AdActionValue.Value2)))
                        {
                            adCreative.AdActionValue = null;
                        }

                        adCreative.ID = adId.HasValue ? adId.Value : 0;
                        if (model.AdTypeId == AdTypeIds.InStreamVideo)
                        {

                            validateVast(adCreative);
                        }
                        adCreative.WrapperContent = model.AdCreativeDto.WrapperContent;
                        adId = _campaignService.SaveAd(campaignId, adGroupId, adCreative);
                        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId });
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                        viewModel = GetCreativeViewModel(campaignId, adGroupId, adId);
                    }
                }
                else
                {
                    viewModel = GetCreativeViewModel(campaignId, adGroupId, adId);
                }
            }
            //else
            //{
               // viewModel = GetCreativeViewModel(campaignId, adGroupId, adId);
           // }


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();
            if (adId.HasValue)
            {

                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =viewModel.AdCreativeDto.Name,//ResourcesUtilities.GetResource("Ad", "SiteMapLocalizations"),
                                                      Order =6,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId})
                                              },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =viewModel.AdGroupName,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 3,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =viewModel.CampaignName,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 2,
                                                                 Url=viewModel.AdvertiserAccountId>0 ? Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=viewModel.AdvertiserAccountId,id=id}) :Url.Action(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"),"SiteMapLocalizations"),
                                                      Url = viewModel.AdvertiserAccountId>0?   Url.Action("Index", new {  AdvertiseraccId=viewModel.AdvertiserAccountId}):Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };


            }
            else
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("NewAd", "SiteMapLocalizations"),
                                                      Order = 6,
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                      Order = 5,
                                                      Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =viewModel.AdGroupName,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 3,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =viewModel.CampaignName,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 2,
                                                   Url=viewModel.AdvertiserAccountId>0 ? Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll", new { AdvertiseraccId=viewModel.AdvertiserAccountId,id=id}) :Url.Action( Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp ?"Create":"CreateAll",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"),"SiteMapLocalizations"),
                                                          Url = viewModel.AdvertiserAccountId>0?   Url.Action("Index", new {  AdvertiseraccId=viewModel.AdvertiserAccountId}):Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };


            }
            if (viewModel.AdvertiserAccountId > 0)
            {
                breadCrumbLinks.Add(
                                          new BreadCrumbModel()
                                          {
                                              Text = viewModel.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = -1

                                          });

                breadCrumbLinks.Add(
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers"),
                                           ExtensionDropDown = true,
                                           Order = -2,
                                       });
            }
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            ChangeJavaScriptSet("adCreativeActionJs");
            var viewName = string.Empty;

            string returnlUrl = string.Empty;
            if (adId.HasValue)
            {
                returnlUrl = Url.Action("Ads", "Campaign", new { id = campaignId, adGroupId = adGroupId });
            }
            else
            {
                returnlUrl = Url.Action("Ads", "Campaign", new { id = campaignId, adGroupId = adGroupId, message = "notauthenticated" });
            }

            if (viewModel.AdCreativeDto.Group.TypeId.HasValue)
            {
                if (!Config.IsAdministrationApp)
                {
                    if (adId.HasValue)
                    {
                        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                    }
                    else
                    {
                        return Redirect(returnlUrl);
                    }
                }
                viewName = viewModel.AdCreativeDto.Group.TypeId.Value.ToString() + "Creative";
                viewModel.AdCreativeDto.TypeId = viewModel.AdCreativeDto.Group.TypeId.Value;
                viewModel.AdCreativeDto.IsDownloadAction = IsDownloadAction(viewModel.AdCreativeDto.AdActionId);
            }
            else
            {
                switch (viewModel.AdCreativeDto.Group.ActionTypeId)
                {
                    case AdActionTypeIds.RichMedia:
                        {
                            if (!(Config.IsAdministrationApp))
                            {
                                if (adId.HasValue)
                                {
                                    return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                                }
                                else
                                {
                                    return Redirect(returnlUrl);
                                }
                            }
                            viewName = "RichMediaCreative";
                            viewModel.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            break;
                        }
                    case AdActionTypeIds.Interstitial:
                        {
                            if (!Config.IsAdministrationApp)
                            {
                                if (adId.HasValue)
                                {
                                    return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId.Value, returnurl = returnlUrl, message = "notauthenticated" });
                                }
                                else
                                {
                                    return Redirect(returnlUrl);
                                }
                            }
                            viewName = "InterstitialCreative";
                            viewModel.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                            break;
                        }
                    case AdActionTypeIds.VideoStreaming:
                        {

                            viewName = "InStreamVideoCreative";
                            viewModel.AdCreativeDto.TypeId = AdTypeIds.InStreamVideo;
                            viewModel.AdCreativeDto.AdSubType = AdSubTypes.VideoLinear;
                            break;
                        }
                    default:
                        {
                            viewName = "Creative";
                            break;
                        }

                }
            }

            FillUserData(viewModel, model, campaignId, adGroupId);
            return View(viewName, viewModel);

        }
        public void validateVast(AdCreativeSaveDto adCreative)
        {

            string XsdsFolderPath = System.Web.HttpContext.Current.Server.MapPath(@"~/VastXsd");
            XDocument xmlFile = null;
            if (!adCreative.IsVideo)
            {
                adCreative.AdActionValue.Value = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.XmlUrl) && adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.IsXmlUrl)
                    {

                        xmlFile = DocumentUtility.downloadXml(adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.XmlUrl);

                    }
                    else if (!string.IsNullOrEmpty(adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml) && !adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.IsXmlUrl)
                    {
                        adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml = adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml.ToString().Trim('\r', '\n').Trim();
                        adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml= adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml.Replace("xmlns=\"https://www.iab.com/VAST",string.Empty);
                        adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml= adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml.Replace("xmlns=\"http://www.iab.com/VAST", string.Empty);

                        xmlFile = XDocument.Parse(adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.Xml);

                    }
                }
                catch (Exception e)
                {

                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "XmlNotValid" });
                    throw error;
                }

                if (xmlFile == null)
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "NotFoundFile" });
                    throw error;
                }
                else
                {
                    var result = DocumentUtility.IsValidXml(xmlFile.ToString(), XsdsFolderPath);
                    if (!result)
                    {
                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "XmlNotValid" });
                        throw error;
                    }

                    //if there is non linear ad


                    //xmlFile.ToString().co

                    //    ClickThrough
                    //HTMLResource
                    bool comefromWrapper = false;
                    bool sslCompl = false;
                    checkVastCondition(xmlFile, adCreative, ref comefromWrapper, ref sslCompl);
                    int ThumbnailDocId = 0;
                    try
                    {
                        byte[] response = new System.Net.WebClient().DownloadData(adCreative.MediaFilesSupported.FirstOrDefault().URL);

                        string FileUrl = DocumentUtility.CopyFileToFolder(response, System.Web.HttpContext.Current.Server.MapPath("~/UploadedVideos"), adCreative.Name + ".mp4");

                        DocumentUtility.SaveVideoThumbnail(FileUrl, out ThumbnailDocId);
                        DocumentUtility.DeleteFile(FileUrl);

                        adCreative.InStreamVideos.FirstOrDefault().InStreamVideoCreativeUnit.ThumbnailDocId = ThumbnailDocId;
                        adCreative.InStreamVideos.FirstOrDefault().CreativeUnitId = adCreative.MediaFilesSupported.FirstOrDefault().CreativeUnitId;
                        adCreative.IsSecureCompliant = true;
                    }
                    catch (Exception ex)
                    {


                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "NotFoundFile" });
                        throw error;
                    }
                }

            }
            else
            {

                if (adCreative.InStreamVideos != null && !(adCreative.InStreamVideos.FirstOrDefault().DocumentId.HasValue))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "UploadVideo" });
                    throw error;

                }
                else if (adCreative.InStreamVideos != null && !(adCreative.InStreamVideos.FirstOrDefault().DocumentId.HasValue && adCreative.InStreamVideos.FirstOrDefault().DocumentId.Value > 0))
                {

                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "UploadVideo" });
                    throw error;
                }
            }
        }

        private bool checkVastCondition(XDocument xmlFile, AdCreativeSaveDto adCreative, ref bool comefromWrapper, ref bool sslCompl)
        {


            //OneAd
            bool isWrapper = false;
          
            //Display all the book titles.
            var ads = xmlFile.Root.Elements("Ad");
            if (ads.ToList() != null && (ads.ToList().Count > 1) || (ads.ToList() == null))
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "XMLVASTAD" });
                throw error;
            }
            //var doc = new XmlDocument();
            //doc.LoadXml(xmlFile.ToString());

            var WrapperURl = xmlFile.Root.Descendants("VASTAdTagURI");
            if (WrapperURl.ToList() != null && (WrapperURl.ToList().Count > 0))
            {
                /* if (!comefromWrapper)
                 {
                     string WrapperURL = WrapperURl.ToList()[0].Value;
                     WrapperURL = WrapperURL.Replace(" ", "");
                     WrapperURL = WrapperURL.Replace("]]>", "");
                     WrapperURL = WrapperURL.Replace("<![CDATA[", "");

                     string XsdsFolderPath = System.Web.HttpContext.Current.Server.MapPath(@"~/VastXsd");
                     var xmlWrapperFile = VASTUtility.downloadXml(WrapperURL);
                     if (xmlWrapperFile == null)
                     {
                         var error = new BusinessException();
                         error.Errors.Add(new ErrorData { ID = "NotFoundFile" });
                         throw error;
                     }
                     else
                     {
                         var result = VASTUtility.IsValidXml(xmlWrapperFile.ToString(), XsdsFolderPath);
                         if (!result)
                         {
                             var error = new BusinessException();
                             error.Errors.Add(new ErrorData { ID = "XmlNotValid" });
                             throw error;
                         }
                         comefromWrapper = true;
                         checkVastCondition(xmlWrapperFile, adCreative, ref comefromWrapper, ref sslCompl);
                         isWrapper = true;



                     }

                 }
                 else
                 {
                     var error = new BusinessException();
                     error.Errors.Add(new ErrorData { ID = "XMLVASTOneWrapper" });
                     throw error;


                 }*/

                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "XMLVASTNOWrapper" });
                throw error;
            }

            var nonlinearAds = xmlFile.Root.Descendants("NonLinearAds");
            if (nonlinearAds.ToList() != null && nonlinearAds.ToList().Count > 0)
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "XMLVASTNonLinearAds" });
                throw error;
            }


            var HTMLResourceAds = xmlFile.Root.Descendants("HTMLResource");
            if (HTMLResourceAds.ToList() != null && HTMLResourceAds.ToList().Count > 0)
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "XMLVASTHTMLResource" });
                throw error;
            }

            //OneLinear
            if (!isWrapper)
            {
                var linearAds = xmlFile.Root.Descendants("Linear");
                if (linearAds.ToList() != null && linearAds.ToList().Count > 1 || (linearAds.ToList() == null))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "XMLVASTLinear" });
                    throw error;
                }
            }

            //calcuclate ssl 





            //CompanionClickThrough
            //ClickThrough

            string xmlFileString = xmlFile.Root.ToString();
            var CompanionClickThroughAds = xmlFile.Root.Descendants("CompanionClickThrough");
            if (CompanionClickThroughAds.ToList() != null && CompanionClickThroughAds.ToList().Count > 0)
            {
                foreach (var clickCom in CompanionClickThroughAds)
                {

                    xmlFileString = xmlFileString.Replace(clickCom.Value, "");

                }
            }



            if (xmlFileString != null)
            {

                xmlFileString = xmlFileString.Replace("xsi:schemaLocation=\"http", "");
                xmlFileString = xmlFileString.Replace("xmlns:xsi=\"http", "");
                xmlFileString = xmlFileString.Replace("xmlns:xs=\"http", "");
                xmlFileString = xmlFileString.Replace("xmlns=\"http", "");
                xmlFileString = xmlFileString.Replace("targetNamespace=\"http", "");
                xmlFileString = xmlFileString.Replace("xsi:noNamespaceSchemaLocation=\"http", "");


                xmlFileString = xmlFileString.Replace("xsi:schemaLocation=&quot;http", "");
        
                xmlFileString = xmlFileString.Replace("xmlns:xsi=&quot;http", "");
                xmlFileString = xmlFileString.Replace("xmlns:xs=&quot;http", "");
                xmlFileString = xmlFileString.Replace("xmlns=&quot;http", "");
                xmlFileString = xmlFileString.Replace("targetNamespace=&quot;http", "");
                xmlFileString = xmlFileString.Replace("xsi:noNamespaceSchemaLocation=&quot;http", "");

            }

            //string xmlFileString = xmlFile.ToString();
            var ClickThroughAds = xmlFile.Root.Descendants("ClickThrough");
            if (ClickThroughAds.ToList() != null && ClickThroughAds.ToList().Count > 0)
            {
                foreach (var clickCom in ClickThroughAds)
                {

                    xmlFileString = xmlFileString.Replace(clickCom.Value, "");

                }
            }
            var AdParameterss = xmlFile.Root.Descendants("AdParameters");
            if (AdParameterss.ToList() != null && AdParameterss.ToList().Count > 0)
            {
                //foreach (var clickCom in AdParameterss)
                //{

                //    xmlFileString = xmlFileString.Replace(clickCom.Value, "");

                //}
                var startIndexOfAdParameters = xmlFileString.IndexOf("<AdParameters");
                var LastIndexOfAdParameters = xmlFileString.LastIndexOf("</AdParameters>");

                StringBuilder sb = new StringBuilder(xmlFileString);
                sb = sb.Remove(startIndexOfAdParameters, (LastIndexOfAdParameters- startIndexOfAdParameters) + 15);
                xmlFileString = sb.ToString();

            }
            bool containshttp = xmlFileString.Contains("http://");
            if (containshttp)
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "XMLVASTSSL" });
                throw error;
            }
            if (!isWrapper)
            {


                string version = xmlFile.Root.Attribute("version").Value;


                if (version == "2.0" || version == "2")
                {
                    adCreative.VASTProtocol = VASTProtocolsVersion.VAST2;

                }
                else if (version == "3.0" || version == "3")
                {


                    adCreative.VASTProtocol = VASTProtocolsVersion.VAST3;
                }
                else if (version == "4.0" || version == "4")
                {


                    adCreative.VASTProtocol = VASTProtocolsVersion.VAST4;
                }
                else if (version == "4.1" || version == "41")
                {


                    adCreative.VASTProtocol = VASTProtocolsVersion.VAST41;
                }
                else if (version == "4.2" || version == "42")
                {


                    adCreative.VASTProtocol = VASTProtocolsVersion.VAST42;
                }
                adCreative.VideoMediaFile = new VideoMediaFileDto();
                var Durations = xmlFile.Root.Descendants("Duration");
                if (Durations.ToList() != null && Durations.ToList().Count > 1)
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "XMLVASTDuration" });
                    throw error;
                }
                else
                {

                    var DurationInSeconnds = Durations.ToList()[0].Value;

                    TimeSpan ts = TimeSpan.Parse(DurationInSeconnds);
                    //DateTime dateTime = DateTime.ParseExact(DurationInSeconnds, "HH:mm:ss",
                    //                    CultureInfo.InvariantCulture);
                    if (!(ts.TotalSeconds <= (Config.InstreamVideoDuraionLimit)))
                    {



                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "XMLVASTDuration" });
                        throw error;

                    }
                    else
                    {

                        adCreative.VideoMediaFile.duration = (int)ts.TotalSeconds;
                    }


                }


                //CheckMediaFiles
                var MediaFiles = xmlFile.Root.Descendants("MediaFile");
                IList<int> CreativeUnitsAppliedinxml = new List<int>();


                var CreativeUnits = _creativeUnitService.GetBy(DeviceTypeEnum.Any, AdTypeIds.InStreamVideo, AdSubTypes.VideoLinear, string.Empty);
                //                640x480, 800x600, 1024x768
                //1920x1080, , 1280x720,854x480,640x360
                var RequredCreativeUnits = CreativeUnits.Where(M => M.Width == 640 || M.Height == 640 || M.Height == 480 || M.Width == 480 || M.Height == 1280 || M.Width == 1280).Select(M => M.ID);
                CreativeUnitDto ceativeUnitDto = null;
                var isPortrait = false;
                var isLandScape = false;

                adCreative.MediaFilesSupported = new List<VideoMediaFileDto>();
                if (MediaFiles.ToList() != null && MediaFiles.ToList().Count > 0)
                {
                    var files = MediaFiles.Where(M => M.Attribute("delivery").Value == "progressive" && M.Attribute("type").Value == "video/mp4").ToList();
                    if (files != null && files.ToList().Count > 0)
                    {

                        foreach (var creativeUnit in CreativeUnits)
                        {
                            var filesExist = files.Where(M => M.Attribute("width").Value == creativeUnit.Width.ToString() && M.Attribute("height").Value == creativeUnit.Height.ToString()).ToList();
                            if (filesExist != null)
                            {
                                foreach (var file in filesExist)
                                {
                                    var bitRate = "0";
                                    if (file.Attribute("bitrate") != null)
                                        bitRate = file.Attribute("bitrate").Value;
                                    var mimieType = file.Attribute("type").Value;
                                    var intBitRate = Convert.ToInt32(bitRate);
                                    var MIMETypeId = _campaignService.GetMIMEType(mimieType);
                                    if (intBitRate >= 0)
                                    {

                                        if (MIMETypeId > 0)
                                        {
                                            adCreative.VideoMediaFile.VideoTypeId = MIMETypeId;
                                        }
                                        adCreative.VideoMediaFile.bitRate = intBitRate;
                                        CreativeUnitsAppliedinxml.Add(creativeUnit.ID);
                                        ceativeUnitDto = creativeUnit;
                                        if (creativeUnit.OrientationType == OrientationType.Landscape)
                                        {
                                            isLandScape = true;


                                        }
                                        if (creativeUnit.OrientationType == OrientationType.Portrait)
                                        {
                                            isPortrait = true;
                                        }
                                        adCreative.MediaFilesSupported.Add(new VideoMediaFileDto { bitRate = intBitRate, CreativeUnitId = creativeUnit.ID, VideoTypeId = MIMETypeId, URL = file.Value });
                                    }
                                }




                            }

                        }
                    }
                }

                if (!(CreativeUnitsAppliedinxml.Count > 0))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "XMLVASTMediaFile" });
                    throw error;
                }
                else
                {

                    if (!CreativeUnitsAppliedinxml.Any(M => RequredCreativeUnits.Contains(M)))
                    {
                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "XMLVASTMediaFile" });
                        throw error;
                    }
                    if (isPortrait)
                    {
                        adCreative.OrientationType = OrientationType.Portrait;
                    }
                    if (isLandScape)
                    {
                        adCreative.OrientationType = OrientationType.Landscape;
                    }

                    if (isLandScape && isPortrait)
                    {
                        adCreative.OrientationType = OrientationType.Any;
                    }
                    adCreative.VideoMediaFile.height = ceativeUnitDto.Height;
                    adCreative.VideoMediaFile.width = ceativeUnitDto.Width;
                    // adCreative.MediaFilesSupported = CreativeUnitsAppliedinxml;
                }
            }
            return sslCompl;

        }

        private void FillUserData(CreativeViewModel viewModel, CreativeSaveViewModel model, int campaignId, int adGroupId)
        {
            viewModel.AdCreativeDto.TypeId = model.AdTypeId;
            viewModel.AdCreativeDto.AdBannerType = model.AdBannerTypeId;
            viewModel.AdCreativeDto.AdSubType = model.AdSubType;

            viewModel.AdCreativeDto.ShowIfInstalled = model.AdCreativeDto.ShowIfInstalled;

            if (viewModel.AdTypes != null)
            {
                string modelAdType = string.Format("{0}Creative", model.AdTypeId.ToString().ToLower() != "plainhtml" ? model.AdTypeId.ToString() : "HTML");
                foreach (var item in viewModel.AdTypes)
                {
                    item.Selected = false;
                    if (item.Value == modelAdType)
                    {
                        item.Selected = true;
                    }
                }
            }

            if (model.AdBannerTypeId.HasValue && viewModel.AdBannerTypes != null)
            {
                foreach (var item in viewModel.AdBannerTypes)
                {
                    item.Selected = false;
                    if ((item.Value == "SmartPhone" || item.Value == "Phone" ? "SmartPhone" : "Tablet") == model.AdBannerTypeId.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }

            foreach (var item in viewModel.EnvironmentTypes)
            {
                item.Selected = false;
                if (item.Value == ((int)model.EnvironmentType).ToString())
                {
                    item.Selected = true;
                }
            }
            foreach (var item in viewModel.ClickMethods)
            {
                item.Selected = false;
                if (item.Value == ((int)model.ClickMethod).ToString())
                {
                    item.Selected = true;
                }
            }
            foreach (var item in viewModel.OrientationTypes)
            {
                item.Selected = false;
                if (item.Value == ((int)model.OrientationType).ToString())
                {
                    item.Selected = true;
                }
            }

            var adcreativeDto = _campaignService.GetAdGroup(campaignId, adGroupId);
            switch (viewModel.AdCreativeDto.TypeId)
            {
                case AdTypeIds.Text:

                    viewModel.TileImageViewModel.TileImages = GetTileImages((int)adcreativeDto.ActionTypeId);
                    foreach (var item in viewModel.TileImageViewModel.TileImages)
                    {
                        var creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}",
                                                                   (int)AdTypeIds.Text, "0",
                                                                   ((int)adcreativeDto.ActionTypeId).ToString(),
                                                                   (int)item.TileImageDocumentDto.TileImageSize.ID);

                        var content = Request.Form[creativeUnitName];
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            int? docId = null;
                            if (content.StartsWith(","))
                            {
                                docId = Convert.ToInt32(content.Split(',')[2]);
                            }
                            else
                            {
                                docId = Convert.ToInt32(content);
                            }
                            item.DocumentId = docId.Value;
                        }

                    }

                    if (model.TileImage.ToString() != "-1")
                    {
                        foreach (var item in viewModel.TileImageViewModel.TileImageList)
                        {
                            item.Selected = false;
                            if (item.Value.StartsWith(model.TileImage.ToString()))
                            {
                                item.Selected = true;
                                ModelState["tileimage"] = new ModelState() { Value = new ValueProviderResult(item.Value, item.Value, null) };
                                viewModel.TileImageViewModel.IsAllowedToSaveImpressionTracker = _campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId, adGroupId);
                                viewModel.TileImageViewModel.SelectedValue = item.Value;
                            }
                        }
                    }
                    else
                    {
                        viewModel.TileImageViewModel.IsAllowedToSaveImpressionTracker = _campaignService.DoesContainDataProviderAllowImpressionTracker(campaignId, adGroupId);
                        viewModel.TileImageViewModel.SelectedValue = "-1";
                        ModelState["tileimage"] = new ModelState() { Value = new ValueProviderResult("-1", "-1", null) };
                    }

                    break;
                case AdTypeIds.NativeAd:
                case AdTypeIds.Banner:
                case AdTypeIds.PlainHTML:

                    var creatives = viewModel.AdCreativeDto.AdBannerType.Value == DeviceTypeEnum.SmartPhone ? viewModel.PhoneCreativeUnits[(int)model.AdTypeId] : viewModel.TabletCreativeUnits[(int)model.AdTypeId];

                    foreach (var creative in creatives)
                    {
                        string content = Request.Form[creative.Name];
                        if (!string.IsNullOrEmpty(content))
                        {
                            creative.Content = content;
                            int documentId;
                            if (int.TryParse(content, out documentId))
                            {
                                creative.DocumentId = documentId;
                            }
                        }
                        else
                        {
                            creative.Content = string.Empty;
                            creative.DocumentId = null;
                        }
                    }

                    if (viewModel.AdCreativeDto.TypeId == AdTypeIds.NativeAd)
                    {
                        foreach (var item in viewModel.NativeAdIcons)
                        {
                            string content = Request.Form[item.Name];
                            if (!string.IsNullOrEmpty(content))
                            {
                                item.Content = content;
                                int documentId;
                                if (int.TryParse(content, out documentId))
                                {
                                    item.DocumentId = documentId;
                                }
                            }
                            else
                            {
                                item.Content = string.Empty;
                                item.DocumentId = null;
                            }
                        }
                    }

                    break;
                case AdTypeIds.InStreamVideo:
                    viewModel.AdCreativeDto.IsVideo = model.AdCreativeDto.IsVideo;
                    break;
                case AdTypeIds.RichMedia:
                    var creativesRichMedia = viewModel.AdCreativeDto.AdBannerType.Value == DeviceTypeEnum.SmartPhone ? viewModel.PhoneCreativeUnits[(int)model.AdSubType.Value] : viewModel.TabletCreativeUnits[(int)model.AdSubType.Value];

                    foreach (var creative in creativesRichMedia)
                    {
                        string content = Request.Form[creative.Name];
                        if (!string.IsNullOrEmpty(content))
                        {
                            creative.Content = content;
                            int documentId;
                            if (int.TryParse(content, out documentId))
                            {
                                creative.DocumentId = documentId;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual ActionResult SetAdsBid(int campaignId, int adGroupId, int[] adIds, string bid)
        {
            var Status = "OK";
            try
            {
                if (adIds != null && adIds.Count() > 0)
                {
                    _campaignService.SetAdsBid(campaignId, adGroupId, adIds, Convert.ToDecimal(bid));
                }
            }
            catch
            {
                Status = "500";
            }
            return Json(new { status = Status }, "text/plain");
        }

        public ActionResult Summary(int id, int adGroupId, int adId, string message)
        {
            int campaignId = id;
            string advertisrName = string.Empty;


            var model = _campaignService.GetAdSummary(campaignId, adGroupId, adId);
            bool isHouseAd = model.Campaign.CampaignType.ToLower().Contains("house") ? true : false;
            ViewData["IsDownloadAction"] = IsDownloadAction(model.ActionId);
            var breadCrumbLinks = new List<BreadCrumbModel>();
            #region BreadCrumb
            if (!(model.AdvertiserAccountId > 0))
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Name,//ResourcesUtilities.GetResource("Ad", "SiteMapLocalizations"),
                                                      Order =6,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId, isHouseAd= isHouseAd})
                                              },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Group.Name,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 3,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Campaign.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 2,
                                                      Url =Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp? Url.Action("create",  new {id = id}):Url.Action("CreateAll", new {id = id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"),"SiteMapLocalizations"),
                                                      Url = Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };
            }
            else
            {

                breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Name,//ResourcesUtilities.GetResource("Ad", "SiteMapLocalizations"),
                                                      Order =8,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 7,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId, isHouseAd= isHouseAd})
                                              },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Group.Name,//ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                      Order = 6,
                                                      Url =Url.Action("Targeting", new {id = id, adGroupId = adGroupId})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                      Order = 5,
                                                      Url=Url.Action("Groups",new {id= id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =model.Campaign.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                      Order = 4,
                                                      Url =Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp? Url.Action("create", new {AdvertiseraccId = model.AdvertiserAccountId, id=id}):Url.Action("CreateAll", new {AdvertiseraccId = model.AdvertiserAccountId, id=id})

                                              
                                                  },

                                                 new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Url=Url.Action("Index", new { AdvertiseraccId=model.AdvertiserAccountId}),
                                                  Order = 3,
                                              }

                                       ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2

                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Url=Url.Action("AccountAdvertisers"),
                                                  Order = 1,
                                                      ExtensionDropDown = true
                                              }
                                          };

            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (string.IsNullOrEmpty(message))
            {
                AddMessages(ResourcesUtilities.GetResource("AdCreativeMsg", "Campaign"), MessagesType.Success);
            }
            else
            {
                switch (message.ToLower())
                {
                    case "notauthenticated":
                        AddMessages(ResourcesUtilities.GetResource("NotAuthenticated", "Campaign"), MessagesType.Warning);
                        ViewData["AllowAddAd"] = false;
                        break;
                    default:
                        break;
                }
            }

            if (model.Warnings != null)
            {
                foreach (var warning in model.Warnings)
                {
                    AddMessages(warning.Message, MessagesType.Warning);
                }
            }
            ChangeJavaScriptSet("adCreativeSummaryJs");
            model.isSummary = true;
            return View("Summary/CreativeMsg", model);
        }








        #endregion
        #region Utility


        [ValidateInput(false)]
        [HttpPost]
        public virtual JsonResult FormatContent(int creativeId)
        {
            using (var reader = new StreamReader(Request.InputStream))
            {
                var content = reader.ReadToEnd();

                var formattedContent = _campaignService.FormatAdCreativeContent(content, creativeId);
                var result = new JsonResult
                {
                    Data = new { Content = formattedContent.Content, IsValid = formattedContent.IsValid },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return result;
            }
        }

        #endregion

        #endregion
        #region ipRange

        [GridAction]
        public ActionResult Dummy()
        {
            return Content("");
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult DummySelect()
        {
            var result = new List<IPTargetingDto>();
            return View(new GridModel
            {
                Data = result,
                Total = 0
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult _DummyDelete(int id)
        {
            //Rebind the grid
            var result = new List<IPTargetingDto>();
            return View(new GridModel(result));
        }

        #endregion
        #region URL
        [GridAction(EnableCustomBinding = true)]
        public ActionResult UrlTargetingActions()
        {
            var result = new List<URLTargetingView>();
            return View(new GridModel
            {
                Data = result,
                Total = 0
            });
        }

        #endregion

        #region video Streaming
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult UploadFile()
        {
            var httpPostedFile = Request.Files[0];
            var documentId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            byte[] thumnail = new byte[100];
            Request.Files[0].InputStream.Read(thumnail, 1, 99);
            if (httpPostedFile != null)
            {
                // ThumbnailUtils
                // Validate the uploaded file if you want like content length(optional)
                // HttpContext.Current.Response.ContentType = "text/HTML";  
                //var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                // Get the complete file path
                var uploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/UploadedVideos");
                if (!Directory.Exists(uploadFilesDir))
                {
                    Directory.CreateDirectory(uploadFilesDir);
                }
                var fileSavePath = Path.Combine(uploadFilesDir, httpPostedFile.FileName);

                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
                //    httpPostedFile.

            }

            string imageBase64Data = Convert.ToBase64String(thumnail);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageData = imageDataURL;
            //  FileContentResult
            //return Content(imageDataURL);

            // return new FileContentResult(thumnail, "image/jpeg");
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = 1, Width = width, Height = height }, "text/plain");
            //   return Content("Uploaded Successfully");
        }
        #endregion

        [HttpGet]
        public ActionResult GetSSP(int? pageNumber, string Ids, string filterParam, string filterExchangeName, string columnName, string direction)
        {
            List<SubAppsiteDto> data = null;
            SubAppSiteListResultDto list = null;
            if (!string.IsNullOrEmpty(Ids))
            {
                var IdsList = Ids.Split(',').Where(x => x != "" && x != ",").ToArray();
                List<int> ids = IdsList.Select(x => Convert.ToInt32(x)).ToList();
                if (ids.Count > 0)
                {
                    var criteria = new Noqoush.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria
                    {
                        AccountIds = ids.ToArray(),
                        Size = 100,
                        Page = (int)pageNumber,
                        QuickSearchField = filterParam,
                        QuickSearchExchangeNameField = filterExchangeName,
                        Desc = direction == "desc",
                        IsForSSP = true,
                        FieldName = columnName
                    };
                    list = _appSiteService.SearchSubAppsites(criteria);
                    data = list.Items.ToList();

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [PermissionsAuthorize(Permission = PortalPermissionsCode.PMPDeal, Roles = "Administrator,AccountManager,AdOps,AppOps")]

        public JsonResult GetAppSitesByAccountId(Noqoush.AdFalcon.Web.Controllers.Model.AppSite.AppSiteSearchModel appSiteSearchModel)
        {
            JsonResult josnResult;
            try
            {
                var appSiteSearchCriteria = new AllAppSiteCriteria()
                {
                    AccountId = appSiteSearchModel.AccountId,
                    AccountName = appSiteSearchModel.Name,
                    IgnoreIsPrimaryUser = appSiteSearchModel.IgnoreIsPrimaryUser,
                    CompanyName = appSiteSearchModel.Name,
                    Name = appSiteSearchModel.AppSiteName,
                    SubPublisherId = appSiteSearchModel.SubPublisher,
                    DateFrom = appSiteSearchModel.DateFrom,
                    DateTo = appSiteSearchModel.DateTo,
                    Email = appSiteSearchModel.Email,
                    TypeId = appSiteSearchModel.TypeId,
                    Page = appSiteSearchModel.Page,
                    Size = appSiteSearchModel.Size
                };
                var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
                var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

                if (!IsPrimaryUser)
                {

                    appSiteSearchCriteria.UserId = UserId;
                    //appCriteria.UserId = UserId;
                }
                if (appSiteSearchCriteria.IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
                {
                    appSiteSearchCriteria.UserId = null;
                }
                var result = _appSiteService.GetAllActive(appSiteSearchCriteria);
                //var result = "";
                josnResult = new JsonResult { Data = result };
                return josnResult;
                //var result = _appSiteService.GetAppSitesByAccountId(accountId);
                //josnResult = new JsonResult { Data = result };
                //return josnResult;
            }
            catch (BusinessException exception)
            {
                josnResult = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet };
                foreach (var errorData in exception.Errors)
                {
                    josnResult.Data += errorData.Message;
                }
                return josnResult;
            }
        }

        [PermissionsAuthorize(Permission = PortalPermissionsCode.PMPDeal, Roles = "Administrator,AccountManager,AdOps,AppOps")]
        public JsonResult GetAppSubPublishers(int appSiteId, string SubPublisherId)
        {
            JsonResult result;
            try
            {
                // IList<string> subPublisher = _appSiteService.GetSubAppsites(appSiteId).Select(x => x.SubPublisherId).ToList<string>();// new List<string>() { "1", "2", "3" };
                var subPublisher = _appSiteService.GetSubAppsites(appSiteId, SubPublisherId).ToList();// new List<string>() { "1", "2", "3" };
                result = new JsonResult { Data = subPublisher, MaxJsonLength = int.MaxValue };
                return result;
            }
            catch (BusinessException exception)
            {
                result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet };
                foreach (var errorData in exception.Errors)
                {
                    result.Data += errorData.Message;
                }
                return result;
            }
        }
        public ActionResult InventorySourcePartial(int id, int adGroupId)
        {

            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria
            {

            });
            var model = new Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal.InventorySourceModel
            {
                CampaignId = id,
                Types = typesDropDown,
                AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() },
                SubPublishers = new List<SelectListItem>(),
                Accounts = new List<AccountViewModel>(),
                CheckedSSP = new List<int>(),
                BusinessPartners = UsersListResult.Items.ToList(),
                InventorySourceList = new List<InventorySourceDto>()
            };

            return PartialView("Targeting/InventorySource", model);
        }


        public ActionResult GetListOfData(int[] data)
        {
            return View();
        }



        public ActionResult InventorySourceData(int id, int adGroupId)
        {


            return Json(new { InventorySourceList = new List<InventorySourceDto>() });
        }
        [HttpPost]
        public ActionResult CalculateAudiancePrice(string AudianceRulesJson)
        {
            if (!string.IsNullOrEmpty(AudianceRulesJson))
            {


                var AudicanceBillSummary = _campaignService.DeserializeRule(AudianceRulesJson);

                if (AudicanceBillSummary != null)
                {
                    return Json(new { MaxValue = AudicanceBillSummary.MaxValue * 1000, MinValue = AudicanceBillSummary.MinValue * 1000 });

                }
            }
            return Json(new { MaxValue = 0, MinValue = 0 });
        }

        [GridAction(EnableCustomBinding = true)]

        public virtual ActionResult GetInventorySource(int? id, int? adGroupId)
        {
            if (!id.HasValue || !adGroupId.HasValue)
            {
                return View(new GridModel
                {
                    Data = new List<InventorySourceDto>(),
                    Total = 0
                });
            }

            var campaignBidConfigDto = _campaignService.GetInventorySources((int)id, (int)adGroupId);

            if (campaignBidConfigDto.InventorySourceDtos != null)
            {

                campaignBidConfigDto.InventorySourceDtos = campaignBidConfigDto.InventorySourceDtos.Where(M => M.SubAppSiteId > 0).ToList();
            }
            return View(new GridModel
            {
                Data = campaignBidConfigDto.InventorySourceDtos.ToList(),
                Total = campaignBidConfigDto.InventorySourceDtos.Count
            });
        }

        public JsonResult AccountSSPSearch(Noqoush.AdFalcon.Web.Controllers.Model.AppSite.AppSiteSearchModel appSiteSearchModel)
        {


            var appSiteSearchCriteria = new AllAppSiteCriteria()
            {
                AccountId = appSiteSearchModel.AccountId,
                IgnoreIsPrimaryUser = appSiteSearchModel.IgnoreIsPrimaryUser,
                AccountName = appSiteSearchModel.Name,
                //CompanyName = appSiteSearchModel.Name,
                Name = appSiteSearchModel.AppSiteName,
                SubPublisherId = appSiteSearchModel.SubPublisher,
                DateFrom = appSiteSearchModel.DateFrom,
                DateTo = appSiteSearchModel.DateTo,
                Email = appSiteSearchModel.Email,
                TypeId = appSiteSearchModel.TypeId,
                Page = appSiteSearchModel.Page,
                Size = appSiteSearchModel.Size
            };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                appSiteSearchCriteria.UserId = UserId;
                //appCriteria.UserId = UserId;
            }
            if (appSiteSearchCriteria.IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
            {

                appSiteSearchCriteria.UserId = null;
            }
            JsonResult josnResult;
            try
            {
                //  var result = _appSiteService.GetAllActive(appSiteSearchCriteria);
                var result = _userService.GetSSPUsers(appSiteSearchCriteria);
                // var result = _appSiteService.GetAllActive(appSiteSearchCriteria);

                josnResult = new JsonResult { Data = result };

                return josnResult;
            }
            catch (BusinessException exception)
            {
                josnResult = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet };
                foreach (var errorData in exception.Errors)
                {
                    josnResult.Data += errorData.Message;
                }
                return josnResult;
            }


        }

        [HttpPost]
        public ActionResult InventorySource(InventorySourceModel InventorySourceModel)
        {
            var ser = new JavaScriptSerializer();
            InventorySourceSaveDTo campaignBidConfigSaveDTo = new InventorySourceSaveDTo();
            campaignBidConfigSaveDTo.CampaignId = InventorySourceModel.CampaignId;
            campaignBidConfigSaveDTo.InsertedItems = ser.Deserialize<IList<InventorySourceDto>>(string.IsNullOrWhiteSpace(InventorySourceModel.InsertedInventorySources) ? "[]" : InventorySourceModel.InsertedInventorySources);
            campaignBidConfigSaveDTo.UpdatedItems = ser.Deserialize<IList<InventorySourceDto>>(string.IsNullOrWhiteSpace(InventorySourceModel.UpdatedInventorySources) ? "[]" : InventorySourceModel.UpdatedInventorySources);

            if (!string.IsNullOrEmpty(InventorySourceModel.DeletedInventorySources))
            {
                var deletedAssignedAppsitesAr = InventorySourceModel.DeletedInventorySources.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                campaignBidConfigSaveDTo.DeletedInventorySources = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
            }
            // _campaignService.SaveCampaignBidConfig(campaignBidConfigSaveDTo);
            return RedirectToAction("InventorySource", new { Id = InventorySourceModel.CampaignId });
        }




        //public ActionResult getimage(string url)
        //{
        //    using (WebClient client = new WebClient())
        //    {
        //        try
        //        {
        //            byte[] bytes = client.DownloadData(new Uri(url));
        //            string Base64String = Convert.ToBase64String(bytes);
        //            return Json(Base64String);
        //        }
        //        catch (Exception e)
        //        {

        //            throw e;
        //        }

        //    }

        //    return null;
        //}

        #region Impression Metric 
        private ImpressionMetricTargetingResultDto getImpressionViewModel(int adGroupId)
        {
            var result = new ImpressionMetricTargetingResultDto();
            var filter = getDefualtFilter();

            result = _campaignService.GetImpressionMetricTargetings(new ImpressionMetricCriteria
            {
                AdGroupId = adGroupId,
                Size = 10

            });

            return result;
        }



        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public ActionResult ImpressionMetrics(int adGroupId)
        {
            var result = getImpressionViewModel(adGroupId);
            IList<ImpressionMetricDto> ImpressionMetric = _campaignService.GetImpressionMetrics();
 
            var ImpressionMetricModel = new ImpressionMetricViewDialogModel { };
            ImpressionMetricModel.ImpressionMetrics = new List<SelectListItem>();
            foreach (ImpressionMetricDto item in ImpressionMetric)
            {
                ImpressionMetricModel.ImpressionMetrics.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.ID.ToString() });
            }
            ImpressionMetricModel.MetricVendors = new List<SelectListItem>();
            ImpressionMetricModel.MetricVendors.Add(new SelectListItem { Text = ResourcesUtilities.GetResource("Any"), Value = "0" });
            IList<MetricVendorDto> MetricVendors = _campaignService.getMetricVendors();
            foreach (MetricVendorDto item in MetricVendors)
            {
                ImpressionMetricModel.MetricVendors.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.ID.ToString() });
            }
            return PartialView("ImpressionMetric/ImpressionMetrics", new ImpressionMetricViewModel { AllItems = result, ImpressionMetricDialog = ImpressionMetricModel });
        }

        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public ActionResult getImpressionMetricVendors(int ImpressionMetricsId)
        {
            try
            {
                ImpressionMetricDto ImpressionMetric = _campaignService.GetImpressionMetric(ImpressionMetricsId);
                string MetricVendors = string.Join(",", ImpressionMetric.MetricVendors.Select(x => x.ID.ToString()).ToList());
                return Json(new { Success = true, MetricVendorIds = MetricVendors }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var str = ex.Message;
                return Json(new { Success = false, MetricVendorIds = "", ErrorMessage = str }, JsonRequestBehavior.AllowGet);
            }
        }

        [GridAction(EnableCustomBinding = true)]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public ActionResult _ImpressionMetrics(int adGroupId)
        {
            var result = getImpressionViewModel(adGroupId);

            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public JsonResult SaveImpressionMetric(int? id, int typeId, int vavId, float value, bool Igonre, int campaignId, int AdGroupId)
        {
            var ImpressionMetric = new ImpressionMetricTargetingDto
            {
                Ignore = Igonre,
                MinValue = value,
                ImpressionMetric = new ImpressionMetricDto { ID = typeId },
                MetricVendor = new MetricVendorDto { ID = vavId },
                campaignId = campaignId,
                AdGroupId = AdGroupId,
                ID = id.HasValue ? id.Value : 0
            };
            try
            {
                _campaignService.SaveImpressionMetricTargeting(ImpressionMetric);

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                return Json(new { Success = false, ErrorMessage = str }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("ImpressionMetric", "Campaign")) }, JsonRequestBehavior.AllowGet);
        }

        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public JsonResult DeleteImpressionMetric(int id)
        {
            try
            {
                _campaignService.DeleteImpressionMetricTargeting(id);

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                return Json(new { Success = false, ErrorMessage = str }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("DeleteSuccessfully", "Global"), ResourcesUtilities.GetResource("ImpressionMetric", "Campaign")) }, JsonRequestBehavior.AllowGet);
        }



        #endregion



        #region AdvertiserAccount

        #region Create


        [HttpGet]
        [DenyNonPrimaryRole]

        public ActionResult AccountAdvertiserSettings(int id)
        {

            string advertisrName = string.Empty;
            string advertisrAccountName = string.Empty;

            advertisrAccountName = _campaignService.GetAdvertiserAccountString(id);


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();

            breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertisrAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers"),
                                                  ExtensionDropDown = true
                                              }
                                      };



            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            AdvertiserAccountSettings Settings = _AdvertiserService.GetAdvertiserAccountSettings(id);

            AdvertiserAssignmentModel model = new AdvertiserAssignmentModel();

            model.AdvertiserAccountId = id;
            model.IsRestricted = Settings != null ? Settings.IsRestricted : false;
            model.AdvertiserName = advertisrAccountName;

            var Criteria = new UserCriteriaBase
            {
                AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId,
                hideCurrentUser = true,
                Page = -1
            };
            var users = new List<CustomSelectListItem>();
            users = _userService.QueryByCratiria(Criteria).Items.Select(x => new CustomSelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.Id.ToString(), Title = x.EmailAddress }).ToList();
            model.AgencyCommissionValue = Settings.AgencyCommissionValue;
            model.AgencyCommission = Settings.AgencyCommission;
            model.Users = users;
            model.ReadUsers = string.Join(",", Settings.Assignments.Where(x => x.Read).Select(x => x.User.Id.ToString()).ToList());
            model.WriteUsers = string.Join(",", Settings.Assignments.Where(x => x.Write).Select(x => x.User.Id.ToString()).ToList());

            return View("~/Views/Campaign/Advertiser/Settings.cshtml", model);
        }
        [HttpPost]
        [DenyNonPrimaryRole]

        public ActionResult AccountAdvertiserSettings(AdvertiserAssignmentModel model)
        {

            string advertisrName = string.Empty;
            string advertisrAccountName = string.Empty;

            advertisrAccountName = _campaignService.GetAdvertiserAccountString(model.AdvertiserAccountId);


            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();

            breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertisrAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers"),
                                                  ExtensionDropDown = true
                                              }
                                      };



            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }

            List<AdvertiserAccountUserDto> items = new List<AdvertiserAccountUserDto>();
            List<int> write = new List<int>();
            List<int> read = new List<int>();

            if (!string.IsNullOrEmpty(model.ReadUsers))
                read = model.ReadUsers.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToList();
            if (!string.IsNullOrEmpty(model.WriteUsers))
                write = model.WriteUsers.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToList();


            foreach (int id in read)
            {
                items.Add(new AdvertiserAccountUserDto
                {
                    Read = true,
                    Write = false,
                    User = new UserDto { Id = id },
                    Link = new AdvertiserAccountDto { Id = model.AdvertiserAccountId }
                });
            }

            foreach (int id in write)
            {
                items.Add(new AdvertiserAccountUserDto
                {
                    Read = false,
                    Write = true,
                    User = new UserDto { Id = id },
                    Link = new AdvertiserAccountDto { Id = model.AdvertiserAccountId }
                });
            }

            try
            {
                AdvertiserAccountSettings settings = new AdvertiserAccountSettings();
                settings.Assignments = items;
                settings.AccountAdvertiserId = model.AdvertiserAccountId;
                settings.IsRestricted = model.IsRestricted;
                settings.AgencyCommission = model.AgencyCommission;
                settings.AgencyCommissionValue = model.AgencyCommissionValue;
                _AdvertiserService.SaveAdvertiserAccountSettings(settings);
                AddSuccessfullyMsg();


            }
            catch (Exception e)
            {

                throw e;
            }

            var users = new List<CustomSelectListItem>();
            var Criteria = new UserCriteriaBase
            {
                AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId,
                hideCurrentUser = true,
                Page = -1
            };
            users = _userService.QueryByCratiria(Criteria).Items.Select(x => new CustomSelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.Id.ToString(), Title = x.EmailAddress }).ToList();
            model.Users = users;
            model.AdvertiserName = advertisrAccountName;

            return View("~/Views/Campaign/Advertiser/Settings.cshtml", model);


        }
        #endregion

        #region Index

        #region Actions
        public ActionResult SaveAdvertiserAccount(int AdvirtiserId, string name)
        {
            try
            {
                AdvertiserAccountDto item = new AdvertiserAccountDto
                {
                    Advertiser = new AdvertiserDto { ID = AdvirtiserId },
                    AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                    UserId = (int)Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId,
                    Name = name
                };
                _AdvertiserService.SaveAdvertiserAccount(item);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }
        public virtual ActionResult AccountAdvertisers(int? Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;


            if (Id.HasValue && IsPrimaryUser)
            {
                var nameOfuser = _userService.GetUserNameById(Id.Value);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MyUsers","User"),
                                                     Url = Url.Action("MyUsers","Users"),
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =nameOfuser,
                                                  Order = 2,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            #endregion
            ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Advertisers_Name",
                Name = "Advertisers.Name",
                ActionName = "GetAdvertisers",
                ControllerName = "Advertiser",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                PlaceHolder = ResourcesUtilities.GetResource("SelectAdvertiserRequired", "Advertiser"),
                IsAjax = true,
                IsRequired = true,
                ChangeCallBack = "AdvertisersChanged",
                CurrentText = string.Empty
            };
            ViewData["AllowDelete"] = breadCrumbLinks;


            return View("Advertiser/Index", AdvertiserAccountLoadData(null, Id));
        }








        //public virtual ActionResult AccountAdvertisersMaster(int? Id)
        //{
        //    var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
        //    #region BreadCrumb

        //    var breadCrumbLinks = new List<BreadCrumbModel>
        //                              {
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource("Advertisers"),
        //                                          Order = 1,
        //                                      }
        //                              };

        //    ViewData["BreadCrumbLinks"] = breadCrumbLinks;


        //    if (Id.HasValue && IsPrimaryUser)
        //    {
        //        var nameOfuser = _userService.GetUserNameById(Id.Value);
        //        var breadCrumbLinks2 = new List<BreadCrumbModel>
        //                              { new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource("MyUsers","User"),
        //                                             Url = Url.Action("MyUsers","Users"),
        //                                          Order = 1,
        //                                      },
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =nameOfuser,
        //                                          Order = 2,
        //                                      }
        //                              };

        //        ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
        //    }
        //    #endregion
        //    ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //    {
        //        Id = "Advertisers_Name",
        //        Name = "Advertisers.Name",
        //        ActionName = "GetAdvertisers",
        //        ControllerName = "Advertiser",
        //        LabelExpression = "item.Name",
        //        ValueExpression = "item.Id",
        //        PlaceHolder = ResourcesUtilities.GetResource("SelectAdvertiserRequired", "Advertiser"),
        //        IsAjax = true,
        //        IsRequired = true,
        //        ChangeCallBack = "AdvertisersChanged",
        //        CurrentText = string.Empty
        //    };
        //    ViewData["AllowDelete"] = breadCrumbLinks;


        //    return View("MasterAppSite/Index", AdvertiserAccountLoadData(null, Id));
        //}



















        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult AccountAdvertisers(int? Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id.HasValue && IsPrimaryUser)
            {
                var nameOfuser = _userService.GetUserNameById(Id.Value);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MyUsers","User"),
                                                     Url = Url.Action("MyUsers","Users"),
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =nameOfuser,
                                                  Order = 2,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.Delete(checkedRecords);

            }
            if (Id.HasValue && IsPrimaryUser)
                return RedirectToAction("AccountAdvertisers", new { Id = Id });

            else
                return RedirectToAction("AccountAdvertisers");
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AccountAdvertisers(int? Id)
        {

            var result = GetAccountAdvertisersQueryResult(null, Id);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }



        #endregion

        #region Helpers

        protected AdvertiserAccountCriteria GetAccountAdvertisersCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdvertiserAccountCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                Name = filter.Name,
                showArchived = filter.showArchived
            };

            criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;

            }
            if (userIdvar.HasValue && IsPrimaryUser)
            {
                criteria.userId = userIdvar;

            }
            return criteria;
        }
        protected virtual AdvertiserAccountListResultDto GetAccountAdvertisersQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            var criteria = GetAccountAdvertisersCriteria(filter, userIdvar);
            var result = _AdvertiserService.GetAccountAdvertiser(criteria);
            return result;
        }
        protected virtual AdvertiserAccountListViewModel AdvertiserAccountLoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            var result = GetAccountAdvertisersQueryResult(filter, userIdvar);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;


            var actions = GetAdvertiserAccountAction();

            return new AdvertiserAccountListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                ToolTips = GetAdvertiserAccountTooltips(),
                //PreventEdit = AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) : !OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser

            };
        }

        protected virtual List<Action> GetAdvertiserAccountTooltips()
        {
            int code = 2;
            // Create the tool tip actions
            if (Noqoush.Framework.OperationContext.Current.UserInfo
                                                                                    <Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                                                                    ().Permissions != null && Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                                                                    ().Permissions.Where(x => x == (int)Noqoush.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.PMPDeal).Count() > 0)
            {

                var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                    {
                        Code = "2",
                        DisplayText = ResourcesUtilities.GetResource("Campains", "Menu"),
                        ClassName = "grid-tool-tip-campaigns",
                        ActionName = "Index",
                        ControllerName ="Campaign",
                        ExtraPrams="AccountAdvertisers"

                    },
                  
                    new Model.Action()
                    {
                        Code = "6",
                        DisplayText = ResourcesUtilities.GetResource("AudienceList", "Global"),
                        ClassName = "grid-tool-tip-Audience-Lists",
                        ActionName = "AudienceList",
                        ControllerName = "Campaign",


                    },
                new Model.Action()
                    {
                        Code = "1",
                        DisplayText = ResourcesUtilities.GetResource("Dashboard", "Menu"),
                        ClassName = "grid-tool-tip-reports",
                        ActionName = "Index",
                        ControllerName ="Dashboard",
                        ExtraPrams2="ad"
                    },

                new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Deals", "PMPDeal"),
                            ClassName = "grid-tool-tip-deals",
                            ActionName = "Index",
                            ControllerName ="Deals"
                        }
                         


                };

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                {


                    toolTips.Add(new Model.Action()
                    {
                        Code = "5",
                        DisplayText = ResourcesUtilities.GetResource("ContentLists", "Global"),
                        ClassName = "grid-tool-tip-content-lists",
                        ActionName = "MasterAppSites",
                        ControllerName = "Campaign",


                    });




                    toolTips.Add(
                       new Model.Action()
                       {
                           Code = "7",
                           DisplayText = ResourcesUtilities.GetResource("Pixels", "Targeting"),//ResourcesUtilities.GetResource("Campains", "Menu"),
                           ClassName = "grid-tool-tip-Tracking-Pixel",
                           ActionName = "TrackingPixel",
                           ControllerName = "Campaign"

                       });
                    toolTips.Add(new Model.Action()
                    {
                        Code = "3",
                        DisplayText = ResourcesUtilities.GetResource("Settings", "AppSite"),
                        ClassName = "grid-tool-tip-setting",
                        ActionName = "AccountAdvertiserSettings",
                        ControllerName = "Campaign"
                    });


                }
                else
                {


                    toolTips.Add(new Model.Action()
                    {
                        Code = "5",
                        DisplayText = ResourcesUtilities.GetResource("ContentLists", "Global"),
                        ClassName = "grid-tool-tip-content-lists",
                        ActionName = "MasterAppSites",
                        ControllerName = "Campaign",


                    });




                    toolTips.Add(
                       new Model.Action()
                       {
                           Code = "7",
                           DisplayText = ResourcesUtilities.GetResource("Pixels", "Targeting"),//ResourcesUtilities.GetResource("Campains", "Menu"),
                           ClassName = "grid-tool-tip-Tracking-Pixel",
                           ActionName = "TrackingPixel",
                           ControllerName = "Campaign"

                       });
                }

              
                if (Config.IsAdOpsAdmin)
                {
                    toolTips.Add(new Model.Action()
                    {
                        Code = "4",
                        DisplayText = ResourcesUtilities.GetResource("unArchive", "PMPDeals"),
                        ClassName = "grid-tool-tip-unarchive",
                        ActionName = "unArchiveAdvertiser",
                        ControllerName = "Campaign",
                        ExtraPrams = "unArchive"

                    });


                    toolTips.Add(new Model.Action()
                    {
                        Code = "6",
                        DisplayText = "Lookalike",
                        ClassName = "grid-tool-tip-Audience-Lists",
                        ActionName = "AudienceListForAdmin",
                        ControllerName = "Campaign",


                    });

                    //code++;
                }



                return toolTips;
            }


            else
            {

                var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {



                    new Model.Action()
                    {
                        Code = "2",
                        DisplayText = ResourcesUtilities.GetResource("Campains", "Menu"),
                        ClassName = "grid-tool-tip-campaigns",
                        ActionName = "Index",
                        ControllerName ="Campaign",
                        ExtraPrams="AccountAdvertisers"

                    },
                   
                    new Model.Action()
                    {
                    Code = "6",
                    DisplayText = ResourcesUtilities.GetResource("AudienceList", "Global"),
                    ClassName = "grid-tool-tip-Audience-Lists",
                    ActionName = "AudienceList",
                    ControllerName = "Campaign",


                },
                        new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Dashboard", "Menu"),
                            ClassName = "grid-tool-tip-reports",
                            ActionName = "Index",
                            ControllerName ="Dashboard",
                            ExtraPrams2="ad"
                        },
                        
                };

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                {


                    toolTips.Add(new Model.Action()
                    {
                        Code = "5",
                        DisplayText = ResourcesUtilities.GetResource("ContentLists", "Global"),
                        ClassName = "grid-tool-tip-content-lists",
                        ActionName = "MasterAppSites",
                        ControllerName = "Campaign",


                    });

                    toolTips.Add(
                       new Model.Action()
                       {
                           Code = "7",
                           DisplayText = ResourcesUtilities.GetResource("Pixels", "Targeting"),//ResourcesUtilities.GetResource("Campains", "Menu"),
                           ClassName = "grid-tool-tip-Tracking-Pixel",
                           ActionName = "TrackingPixel",
                           ControllerName = "Campaign"

                       });
                    toolTips.Add(new Model.Action()
                    {
                        Code = "3",
                        DisplayText = ResourcesUtilities.GetResource("Settings", "AppSite"),
                        ClassName = "grid-tool-tip-setting",
                        ActionName = "AccountAdvertiserSettings",
                        ControllerName = "Campaign"
                        //ExtraPrams = "AccountAdvertisers"
                    });
                }
                else
                {
                    toolTips.Add(new Model.Action()
                    {
                        Code = "5",
                        DisplayText = ResourcesUtilities.GetResource("ContentLists", "Global"),
                        ClassName = "grid-tool-tip-content-lists",
                        ActionName = "MasterAppSites",
                        ControllerName = "Campaign",


                    });

                    toolTips.Add(
                       new Model.Action()
                       {
                           Code = "7",
                           DisplayText = ResourcesUtilities.GetResource("Pixels", "Targeting"),//ResourcesUtilities.GetResource("Campains", "Menu"),
                           ClassName = "grid-tool-tip-Tracking-Pixel",
                           ActionName = "TrackingPixel",
                           ControllerName = "Campaign"

                       });
                }
              
                if (Config.IsAdOpsAdmin)
                {
                    toolTips.Add(new Model.Action()
                    {
                        Code = "4",
                        DisplayText = ResourcesUtilities.GetResource("unArchive", "PMPDeals"),
                        ClassName = "grid-tool-tip-unarchive",
                        ActionName = "unArchiveAdvertiser",
                        ControllerName = "Campaign",
                        ExtraPrams = "unArchive"

                    });
                    code++;


                    toolTips.Add(new Model.Action()
                    {
                        Code = "6",
                        DisplayText = "Lookalike",
                        ClassName = "grid-tool-tip-Audience-Lists",
                        ActionName = "AudienceListForAdmin",
                        ControllerName = "Campaign",


                    });
                    code++;

                }
                return toolTips;
            }

        }

        public virtual ActionResult unArchiveAdvertiser(int id)
        {

            _AdvertiserService.unArchive(id);
            return RedirectToAction("AccountAdvertisers", "Campaign");

        }

        protected virtual List<Action> GetAdvertiserAccountAction()
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Archive", "PMPDeal"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Advertiser"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Archive", "Confirmation") // like are u sure ?

                           
                        },

                    new Model.Action()
                        {
                            ActionName = "index",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AllCampaigns", "AdChart")
                        },
                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("AddNewAdvertiser", "Advertiser"),
                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };

            if (!OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                //actions.RemoveAt(0);

                // actions.RemoveAt(1);
                actions = new List<Model.Action>();
            }
            #endregion

            return actions;
        }

        #endregion

        #endregion

        #endregion


        #region MasterAppSite

        #region Actions
        public ActionResult SaveMasterAppSites(AdvertiserAccountMasterAppSiteDto dto)
        {
            try
            {


                dto.GlobalScope = false;
                dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                dto.UserId = (int)Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;


                _AdvertiserService.SaveAdvertiserAccountMasterAppSite(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }
        public ActionResult GetMasterAppSiteItem(int Id)
        {
         var items=   _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id);
            return Json(items, JsonRequestBehavior.AllowGet);

        }
  
        public virtual ActionResult MasterAppSites(int? Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSiteLists","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;


            if (Id.HasValue )
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id.Value);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSiteLists","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            #endregion
          
            ViewData["AllowDelete"] = breadCrumbLinks;


           return View("MasterAppSite/Index", MasterAppSiteLoadData(null, Id,false,false));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult MasterAppSites(int? Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSite","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id.HasValue)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id.Value);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSiteLists","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeleteAdvertiserAccountMasterAppSite(checkedRecords);

            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Activate"]))
            {
                _AdvertiserService.ActivateAdvertiserAccountMasterAppSite(checkedRecords);

            }
            if (!string.IsNullOrWhiteSpace(Request.Form["DeActivate"]))
            {
                _AdvertiserService.DeActivateAdvertiserAccountMasterAppSite(checkedRecords);

            }
            if (Id.HasValue )
                return RedirectToAction("MasterAppSites", new { Id = Id });

            else
                return RedirectToAction("MasterAppSites");
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _MasterAppSites(int? Id)
        {

            var result = GetMasterAppSiteQueryResult(null, Id);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        public ActionResult MasterListSearch()
        {

            int CampId = Convert.ToInt32(RouteData.Values["id"]);
            
            int adveraccId = _campaignService.GetAdvertiserAccountIdByCampaignId(CampId);
            var filter = getDefualtFilter();
            filter.showAccountAdv = true;
            MasterAppSiteListViewModel model = MasterAppSiteLoadData(filter, adveraccId, filter.showGlobal);
            return PartialView(model);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _MasterListSearch()
        {
            int CampId = Convert.ToInt32(RouteData.Values["id"]);
            int adveraccId = _campaignService.GetAdvertiserAccountIdByCampaignId(CampId);
            var filter = getDefualtFilter();
            var result = MasterAppSiteLoadData(filter, adveraccId, filter.showGlobal);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(ViewData["total"])
            });


        }
        #endregion

        #region Helpers

        protected AdvertiserAccountMasterAppSiteCriteria GetMasterAppSiteCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdvertiserAccountMasterAppSiteCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                AccountId =Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                Name = filter.Name,
                showArchived = filter.showArchived
            };
            if (filter.showRoot)
                criteria.AccountId = null;
            if (filter.StatusId.HasValue && filter.StatusId > 0)
                criteria.Status = (MasterAppSiteStatus)filter.StatusId.Value;

            if (filter.TypeId.HasValue && filter.TypeId>0)
                criteria.Type = (MasterAppSiteType)filter.TypeId.Value;
            criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            //criteria.userId = UserId;

            criteria.GlobalScope = true;

            if (userIdvar.HasValue)
            {
                criteria.AdvAccountId = userIdvar;

                criteria.GlobalScope = false;
            }
          
              
            return criteria;
        }
        protected virtual AdvertiserAccountMasterAppSiteResultDto GetMasterAppSiteQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar,bool showGlobal=false)
        {
            var criteria = GetMasterAppSiteCriteria(filter, userIdvar);
            if (showGlobal)
            {
                criteria.showGlobalAndAccount = true;
               
            }
            else if(criteria.AccountId.HasValue)
                criteria.GlobalScope = false;
            if (filter!=null && filter.showAccountAdv)
            {
                criteria.showAccountAndAdvertiser = true;
            }
            var result = _AdvertiserService.GetAdvertiserAccountMasterAppSite(criteria);
            return result;
        }
        protected virtual MasterAppSiteListViewModel MasterAppSiteLoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal=false, bool isMaster = true)
        {
            var result = GetMasterAppSiteQueryResult(filter, userIdvar, showGlobal);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;


      
            var AdvertiserAccountId = 0;
            var advertiserId = 0;
            if (userIdvar.HasValue)
            {
                AdvertiserAccountId = userIdvar.Value;

              var accObje=  _AdvertiserService.GetAccountAdvertiserById(AdvertiserAccountId);

                advertiserId = accObje.AdvertiserId.Value;
            }
            var actions = GetMasterAppSiteAction(isMaster, AdvertiserAccountId);
            return new MasterAppSiteListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                AdvertiserAccountId= AdvertiserAccountId,
                AdvertiserId= advertiserId,
                ToolTips = GetMasterAppSiteTooltips(AdvertiserAccountId),
                PreventEdit = AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) : false

            };
        }

        protected virtual List<Action> GetMasterAppSiteTooltips(int AdvertiserAccountId)
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
                            AjaxType=AjaxType.rename,
                             Type=ActionType.ajax,
                           ExtraPrams ="Dialog"


                }};


            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }



            return toolTips;
          

        }

      /*  public virtual ActionResult unArchiveAdvertiser(int id)
        {

            _AdvertiserService.unArchive(id);
            return RedirectToAction("AccountAdvertisers", "Campaign");

        }
        */
        protected virtual List<Action> GetMasterAppSiteAction(bool isMaster=true, int AdvertiserAccountId=0)
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,


                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "MasterAppSite"),
                            ExtraPrams2 = ResourcesUtilities.GetResource( "DeleteContentLists ","Campaign")// like are u sure


                           
                        },
                          new Model.Action()
                        {
                            ActionName = "Activate",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,

                            DisplayText = ResourcesUtilities.GetResource("ActivateNow", "Report"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "MasterAppSite"),
                             ExtraPrams2 = ResourcesUtilities.GetResource("Activate","Confirmation" )


                        },

                           new Model.Action()
                        {
                            ActionName = "DeActivate",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("DeActivate", "Global"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "MasterAppSite"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Confirmation" , "DeActivate") // like are u sure ?

       

                           
                       },
                    
                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,

                            DisplayText =isMaster? ResourcesUtilities.GetResource("AddNewMasterAppSite", "Global"): ResourcesUtilities.GetResource("AddNewContentAppSite", "Global"),

                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };


            #endregion
            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }

            return actions;
        }


        [GridAction(EnableCustomBinding = true)]
        public ActionResult GridMasterListConfigData(int id, int adGroupId)
        {
            var result = _campaignService.GetMasterListConfigConfigs(id, adGroupId);
            return View(new GridModel
            {
                Data = result,
                Total = result != null ? result.Count : 0
            });
        }

        #endregion

        #endregion

        #region MasterAppSite Item

        #region Actions
        public ActionResult SaveMasterAppSiteItems(AdvertiserAccountMasterAppSiteItemDto dto)
        {
            try
            {



                dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                dto.UserId = (int)Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;


                _AdvertiserService.SaveAdvertiserAccountMasterAppSiteItem(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }
        public ActionResult SaveMasterAppSiteItemsBulk(AdvertiserAccountMasterAppSiteItemResultDto item)
        {
            try
            {
                if (item.Items != null)
                {
                    foreach (var dto in item.Items)
                    {

                        dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                        dto.UserId = (int)Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;


                        _AdvertiserService.SaveAdvertiserAccountMasterAppSiteItem(dto);
                    }
                }
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }
        public virtual ActionResult GlobalMasterAppSiteItems(int? Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            ViewData["LinkId"] = Id;
            AdvertiserAccountMasterAppSiteDto dto = new AdvertiserAccountMasterAppSiteDto();
            if (Id.HasValue)
            {
                dto = _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id.Value);
                AdvertiserAccountListDto advDto = new AdvertiserAccountListDto();
                if (dto.LinkId > 0)
                {
                    advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);
                }
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MasterAppSiteLists","Global") ,
                                                     Url =dto.LinkId > 0? Url.Action("GlobalMasterAppSites", new { Id=dto.LinkId}): Url.Action("GlobalMasterAppSites", new { Id=""}),
                                                  Order = 3,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 4,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("MasterAppSiteItems","Global")  ,
                                                  Order = 5,
                                              }
                                      };

                if (dto.LinkId > 0)
                {
                    //var advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);



                    var extensions = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),

                                                  Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =advDto.Name,
                                                  Order = 2,
                                              }};

                    breadCrumbLinks2.AddRange(extensions);
                }

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }

            // ViewData["BreadCrumbLinks"] = breadCrumbLinks2;

            #endregion



            var filter = getDefualtFilter();
            filter.showRoot = true;
            return View("GlobalMasterAppSiteItem/Index", MasterAppSiteItemLoadData(filter, Id, dto.Name));
        }
        public virtual ActionResult MasterAppSiteItems(int? Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            ViewData["LinkId"] = Id;
            AdvertiserAccountMasterAppSiteDto dto = new AdvertiserAccountMasterAppSiteDto();
            if (Id.HasValue)
            {
                 dto = _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id.Value);
                AdvertiserAccountListDto advDto = new AdvertiserAccountListDto();
                if (dto.LinkId > 0)
                {
                    advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);
                }
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSiteLists","Global") ,
                                                     Url =dto.LinkId > 0? Url.Action("MasterAppSites", new { Id=dto.LinkId}): Url.Action("MasterAppSites", new { Id=""}),
                                                  Order = 3,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 4,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("ContentAppSiteItems","Global")  ,
                                                  Order = 5,
                                              }
                                      };

                if (dto.LinkId > 0)
                {
                    //var advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);



                    var extensions = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),

                                                  Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =advDto.Name,
                                                  Order = 2,
                                              }};

                    breadCrumbLinks2.AddRange(extensions);
                }

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
        
            // ViewData["BreadCrumbLinks"] = breadCrumbLinks2;

            #endregion




            return View("MasterAppSiteItem/Index", MasterAppSiteItemLoadData(null, Id, dto.Name,false));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult GlobalMasterAppSiteItems(int? Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;



            if (Id.HasValue)
            {
                var dto = _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id.Value);
                AdvertiserAccountListDto advDto = new AdvertiserAccountListDto();
                if (dto.LinkId > 0)
                {
                    advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);
                }
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MasterAppSiteLists","Global") ,
                                                     Url =dto.LinkId > 0? Url.Action("GlobalMasterAppSites", new { Id=dto.LinkId}): Url.Action("GlobalMasterAppSites", new { Id=""}),
                                                  Order = 3,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 4,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("MasterAppSiteItems","Global")  ,
                                                  Order = 5,
                                              }
                                      };

                if (dto.LinkId > 0)
                {
                    //var advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);



                    var extensions = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),

                                                  Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =advDto.Name,
                                                  Order = 2,
                                              }};

                    breadCrumbLinks2.AddRange(extensions);
                }

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeleteAdvertiserAccountMasterAppSiteItem(checkedRecords);

            }

            if (Id.HasValue)
                return RedirectToAction("MasterAppSiteItems", new { Id = Id });

            else
                return RedirectToAction("MasterAppSiteItems");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult MasterAppSiteItems(int? Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;



            if (Id.HasValue)
            {
                var dto = _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id.Value);
                AdvertiserAccountListDto advDto = new AdvertiserAccountListDto();
                if (dto.LinkId > 0)
                {
                    advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);
                }
                    var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSiteLists","Global") ,
                                                     Url =dto.LinkId > 0? Url.Action("MasterAppSites", new { Id=dto.LinkId}): Url.Action("MasterAppSites", new { Id=""}),
                                                  Order = 3,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 4,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("ContentAppSiteItems","Global")  ,
                                                  Order = 5,
                                              }
                                      };

                if (dto.LinkId > 0)
                {
                    //var advDto = _AdvertiserService.GetAccountAdvertiserById(dto.LinkId);



                    var extensions = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                               Text = ResourcesUtilities.GetResource("Advertisers", "Global"),

                                                  Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =advDto.Name,
                                                  Order = 2,
                                              }};

                    breadCrumbLinks2.AddRange(extensions);
                }

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeleteAdvertiserAccountMasterAppSiteItem(checkedRecords);

            }
           
            if (Id.HasValue)
                return RedirectToAction("MasterAppSiteItems", new { Id = Id });

            else
                return RedirectToAction("MasterAppSiteItems");
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _MasterAppSiteItems(int? Id)
        {
            AdvertiserAccountMasterAppSiteDto dto = new AdvertiserAccountMasterAppSiteDto();
            if (Id.HasValue)
            {
                dto = _AdvertiserService.GetAdvertiserAccountMasterAppSiteById(Id.Value);
            }
                var result = GetMasterAppSiteItemQueryResult(null, Id,dto.Name);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }



        #endregion

        #region Helpers

        protected AdvertiserAccountMasterAppSiteItemCriteria GetMasterAppSiteItemCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar,string Name)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdvertiserAccountMasterAppSiteItemCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                Name = filter.Name,
                Domain = filter.Domain,
                BundleId = filter.BundleId,

                showArchived = filter.showArchived
            };
            if (filter.TypeId.HasValue)
                criteria.Type = (MasterAppSiteItemType)filter.TypeId.Value;

            criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            criteria.userId = UserId;


            if (userIdvar.HasValue)
            {
                criteria.MasterListId = userIdvar.Value;

            }
            return criteria;
        }
        protected virtual AdvertiserAccountMasterAppSiteItemResultDto GetMasterAppSiteItemQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, string Name)
        {
            var criteria = GetMasterAppSiteItemCriteria(filter, userIdvar,Name);
            var result = _AdvertiserService.GetAdvertiserAccountMasterAppSiteItem(criteria);
            return result;
        }
        protected virtual MasterAppSiteItemListViewModel MasterAppSiteItemLoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, string Name, bool isMaster=true)
        {
            var result = GetMasterAppSiteItemQueryResult(filter, userIdvar, Name);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;


            var actions = GetMasterAppSiteItemAction(isMaster);

            return new MasterAppSiteItemListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions, 
                ListName= Name,
                //ToolTips = GetMasterAppSiteTooltips(filter.TypeId),
                //PreventEdit = AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) : !OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser

            };
        }

        protected virtual List<Action> GetMasterAppSiteItemTooltips()
        {

            // Create the tool tip actions


            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {




                };





            return toolTips;


        }

        /*  public virtual ActionResult unArchiveAdvertiser(int id)
          {

              _AdvertiserService.unArchive(id);
              return RedirectToAction("AccountAdvertisers", "Campaign");

          }
          */
        protected virtual List<Action> GetMasterAppSiteItemAction(bool isMaster=true)
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                               ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "MasterAppSiteItem"),
                            ExtraPrams2 = ResourcesUtilities.GetResource( "Confirmation","Campaign") // like are u sure ?e are u sure ?

                           
                        },
                         

                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,
                            DisplayText = isMaster?ResourcesUtilities.GetResource("AddNewMasterAppSiteItem", "Global"):ResourcesUtilities.GetResource("AddNewContentAppSiteItem", "Global"),
                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };


            #endregion

            return actions;
        }

        #endregion

        #endregion




        #region TrafficPlanner
        [PermissionsAuthorize(Permission = PortalPermissionsCode.TrafficPlanner, Roles = "Administrator,adops,AccountManager")]
        public ActionResult TrafficPlanner()
        {
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("TrafficPlanner"),
                                                  Order = 1
                                              }

                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            TrafficPlannerViewModel model = GetTrafficPlannerViewModel();
            ViewData["total"] = 0;


            return View("~/Views/Campaign/TrafficPlanning/TrafficPlanner.cshtml", model);
        }

        private TrafficPlannerViewModel GetTrafficPlannerViewModel()
        {
            TrafficPlannerViewModel model = new TrafficPlannerViewModel();

            model.Platforms = new Select2ViewModel
            {
                Id = "Platforms",
                ActionName = "GetData",
                ControllerName = "Platform",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                Single = true,
                AllowClear = true,
                IsTree = false,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("PlatformSelect", "Global"),
            };
            model.AppSites = new Select2ViewModel
            {
                Id = "AppSites",
                ActionName = "GetSSPPartnersAsAppsites",
                ControllerName = "Common",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
             
                IsTree = false,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("SourceSelect", "Global"),
            };
            model.Languages = new Select2ViewModel
            {
                Id = "Languages",
                ActionName = "GetLanguages",
                ControllerName = "Common",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                Single = true,
                AllowClear = true,
                IsTree = false,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("LanguageSource", "Global"),
            };
            model.Operators = new Select2ViewModel
            {
                Id = "Operators",
                ActionName = "GetTreeData",
                ControllerName = "Operator",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                IsTree = true,
                Single = true,
                AllowClear = true,
                disabled = true,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("OperatorSelect", "Global"),
            };
            model.Countries = new Select2ViewModel
            {
                Id = "Countries",
                ActionName = "GetTreeData",
                ControllerName = "Country",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                IsTree = true,
                Single = true,
                AllowClear = true,
                disabled = true,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("CountrySelect", "Global"),
                //OnSelectFunctions = "FixCountriesTree",
                //OnReadyFunctions= "FixCountriesLimit"
            };
            //Load Age Groups List
            var optionalItem = new SelectListItem { Value = "0", Text = ResourcesUtilities.GetResource("All", "Global") };
            var ageGroupDtos = _ageGroupService.GetAll().ToList();
            var ageGroups = new List<SelectListItem> { optionalItem };
            ageGroups.AddRange(ageGroupDtos.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            model.DemographicTargetingView = new DemographicTargetingViewModel();
            model.DemographicTargetingView.AgeGroups = ageGroups;

            return model;
        }

        public ActionResult TPGrid()
        {
            List<CampaignCommonReportDto> Model = new List<CampaignCommonReportDto>();

            ViewData["total"] = Model.Count;
            return PartialView("~/Views/Campaign/TrafficPlanning/TPGrid.cshtml", Model);
        }


        [GridAction(EnableCustomBinding = true)]
        public ActionResult _TPGrid()
        {
            List<CampaignCommonReportDto> Model = new List<CampaignCommonReportDto>();

            var ser = new JavaScriptSerializer();

            TrafficPlannerCriteria Data = ser.Deserialize<TrafficPlannerCriteria>(string.IsNullOrWhiteSpace(Request.Form["obj"]) ? "[]" : Request.Form["obj"]);
            if (Data != null && Data.IsRun)
            {
                TrafficPlannerCriteriaDto obj = GetTrafficPlannerCriteria(Data);

                switch (obj.Type)
                {
                    case 0:
                        Model = _reportService.GetCampaignSubAppSiteReportTraficPlannerDrillDown(obj);
                        break;
                    case 1:
                        Model = _reportService.GetCampaignAdSizeReportTraficPlannerDrillDown(obj);
                        break;
                    case 2:
                        Model = _reportService.GetCampaignDeviceTypeReportTraficPlannerDrillDown(obj);
                        break;
                    case 3:
                        Model = _reportService.GetCampaignOSReportTraficPlannerDrillDown(obj);
                        break;
                    case 4:
                        Model = _reportService.GetCampaignCountryReportTraficPlannerDrillDown(obj);
                        break;
                    case 5:
                        Model = _reportService.GetCampaignAdTypeGroupReportTraficPlannerDrillDown(obj);
                        break;
                    case 6:
                        Model = _reportService.GetCampaignGenderReportTraficPlannerDrillDown(obj);
                        break;

                    case 7:
                        Model = _reportService.GetEnvironmentReportTraficPlannerDrillDown(obj);
                        break;
                    default:
                        Model = _reportService.GetCampaignSubAppSiteReportTraficPlannerDrillDown(obj);
                        break;
                }
            }

            ViewData["total"] = Model.Count() > 0 ? Model.First().TotalCount : 0;
            return View(new GridModel
            {
                Data = Model,
                Total = Convert.ToInt32(ViewData["total"])
            });
        }

        private TrafficPlannerCriteriaDto GetTrafficPlannerCriteria(TrafficPlannerCriteria obj)
        {
            TrafficPlannerCriteriaDto returnObj = new TrafficPlannerCriteriaDto();

            returnObj.AdFormats = obj.AdFormats.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.AdSizes = obj.AdSizes.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.AppSites = obj.AppSites.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.Countries = obj.Countries.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.languages = obj.languages.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.Operators = obj.Operators.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.Platforms = obj.Platforms.Split(',').Where(x => x != "" && x != ",").Select(x => Convert.ToInt32(x)).ToArray();
            returnObj.DeviceTypeId = obj.DeviceTypeId;
            returnObj.GenderType = obj.GenderType;
            returnObj.EnvironmentType = obj.EnvironmentType;
            returnObj.AgeGroup = obj.AgeGroup;
            returnObj.Weekid = _campaignService.GetPublisherCounterCurrentWeek(); //20180128;
            returnObj.Type = obj.Type;
            returnObj.PageIndex = string.IsNullOrWhiteSpace(Request.Form["page"]) ? 0 : Convert.ToInt32(Request.Form["page"]) - 1;
            returnObj.Size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? 10 : Convert.ToInt32(Request.Form["size"]);
            var list = _locationService.GetContinentsByCountries(returnObj.Countries);
            if (list != null)
                returnObj.Continents = list.Select(x => x.ID).ToArray();

            return returnObj;
        }


        #region chart

        [HttpPost]
        public ActionResult TrafficPlannerChart(string jsdata)
        {
            var ser = new JavaScriptSerializer();

            TrafficPlannerCriteria Data = ser.Deserialize<TrafficPlannerCriteria>(string.IsNullOrWhiteSpace(jsdata) ? "[]" : jsdata);

            TrafficPlannerCriteriaDto obj = GetTrafficPlannerCriteria(Data);
            obj.isChartType = true;
            List<CampaignCommonReportDto> result;
            long counter;
            string colmunName = "";
            obj.Size = Config.PageSize;
            obj.PageIndex = 0;
            string jsonGoogleDataTable = "NoData";
            GetTopAccountsPerformanceReport(obj, out result, out counter, out colmunName);
            if (result.Count() > 0 && result.Where(x => x.UniqueImp > 0).Count() > 0)
            {
                jsonGoogleDataTable = FormatDataToCharts(result, counter, colmunName);
            }
            return Content(jsonGoogleDataTable);
        }

        private void GetTopAccountsPerformanceReport(TrafficPlannerCriteriaDto obj, out List<CampaignCommonReportDto> resultList, out long counter, out string name)
        {
            switch (obj.Type)
            {
                case 1:
                    resultList = _reportService.GetCampaignAdSizeReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("AdSize", "PMPDealTargetings");
                    break;
                case 2:
                    resultList = _reportService.GetCampaignDeviceTypeReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("DeviceType", "BidConfigType");
                    break;
                case 3:
                    resultList = _reportService.GetCampaignOSReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("Platform", "Lookup");
                    break;
                case 4:
                    resultList = _reportService.GetCampaignCountryReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("ByCountry", "Chart");
                    break;
                case 5:
                    resultList = _reportService.GetCampaignAdTypeGroupReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("AdFormat", "PMPDeal");
                    break;
                case 6:
                    resultList = _reportService.GetCampaignGenderReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("Gender");
                    break;
                case 7:
                    resultList = _reportService.GetEnvironmentReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("EnvironmentType", "Campaign");
                    break;
                default:
                    resultList = _reportService.GetCampaignCountryReportTraficPlannerDrillDown(obj);
                    name = Framework.Resources.ResourceManager.Instance.GetResource("ByCountry", "Chart");
                    break;
            }
            counter = resultList != null && resultList.Count > 0 ? resultList.First().TotalCount : 0;

        }


        private string FormatDataToCharts<T>(IList<T> result, long counter, string colmunName)
           where T : BaseCampaignResultDto
        {

            var dataArray = ConvertToBaseChartDashboardView(result, "UniqueImp");


            List<GoogleDataTableColumn> columnsList = new List<GoogleDataTableColumn>()
            {
                new GoogleDataTableColumn()
                {
                    Id= colmunName,
                    Label = "",
                    Type = "string"
                },
                new GoogleDataTableColumn()
                {
                    Id= "MetricValue",
                    Label = Framework.Resources.ResourceManager.Instance.GetResource("UniqueImp", "Report"),
                    Type = "number"
                }
            };

            string jsonGoogleDataTable = GoogleControlsHelper.ConvertIListToDataTable<BaseChartDashboardView>(dataArray, columnsList);

            return jsonGoogleDataTable;

        }
        private IList<BaseChartDashboardView> ConvertToBaseChartDashboardView<T>(IList<T> result, string metricName)
        where T : BaseCampaignResultDto
        {
            var dataArray = result.Select(p => new BaseChartDashboardView()
            {
                Name = p.Name,
                MetricValue = Convert.ChangeType(typeof(BaseCampaignResultDto).GetProperty(metricName).GetValue(p),
                typeof(BaseCampaignResultDto).GetProperty(metricName).PropertyType)
            }).OrderByDescending(p => p.MetricValue).ToList();

            return dataArray;
        }



        #endregion

        #endregion

        private string GetFeeValue(FeeDto element)
        {
            IList<string> keyValueList = new List<string>();

            switch (element.TypeId)
            {
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Fixed:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value* item.CostModelWrapper.Factor));
                        }
                        break;
                    }
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value * 100));
                        }
                        break;
                    }
            }

            return string.Join(",", keyValueList);
        }
        private List<SelectListItem> GetFeeList(string lookupType, int selectedValue)
        {
            var items = _lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = lookupType });
            items.Items = items.Items.Where(M=> (M as FeeDto).IsAutoAdded == false && (M as FeeDto).FeeCalculatedFrom != FeeCalculatedFrom.System);

          items.Items = items.Items.OrderBy(x => x.Name.Value, StringComparer.InvariantCultureIgnoreCase).ToList();
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0#1#0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};
            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = string.Format("{0}#{1}#{2}", item.ID.ToString(), (item as FeeDto).TypeId, GetFeeValue(item as FeeDto)),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }));
            return lookupsList;
        }
        public ActionResult React()
        {
        

            return View("~/Views/Campaign/React.cshtml");
        }
        [HttpGet]
        public ActionResult GetAudienceListCounter(string dpName)
        {
            var latest = _campaignService.GetAudienceListCounter(dpName);

            return Json(new { latestCounter = latest }, JsonRequestBehavior.AllowGet);

        }






        public ActionResult CampSettings(int? id)
        {
            if (!id.HasValue)
            {
                throw new Noqoush.AdFalcon.Exceptions.Core.DataNotFoundException();
            }
            var campaignSettingsDto = _campaignService.GetCampSettings(id.Value);
            var AdvertiserId = _campaignService.GetCampaignAdvertiser(id.Value);
            var AdvertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(id.Value);
            string AdvertiserName = _campaignService.GetAdvertiserString(AdvertiserId);

            string AdvertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiserAccountId);
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteSettings","SiteMapLocalizations"),
                                                  Order =5
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =campaignSettingsDto.Name,
                                                  Url = Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp?  Url.Action("Create", "Campaign", new {AdvertiseraccId=AdvertiserAccountId,id = id}): Url.Action("CreateAll", "Campaign", new {AdvertiseraccId=AdvertiserAccountId,id = id}),
                                                  Order = 4
                                              },

                                            new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignList", "SiteMapLocalizations"),
                                                  Order = 3,
                                                 Url = Url.Action("Index", new { AdvertiseraccId = AdvertiserAccountId })
                                              }

                                          ,
                                          new BreadCrumbModel()
                                          {
                                              Text = AdvertiserAccountName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                              Order = 2

                                          },
                                       new BreadCrumbModel()
                                       {
                                           Text = ResourcesUtilities.GetResource("Advertisers", "Global"),
                                           Url = Url.Action("AccountAdvertisers"),
                                           Order = 1,
                                       }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

         
         
     
            ViewData["AdvertiserIdForTab"] = AdvertiserId;

            ViewData["AdvertiserAccountIdForTab"] = AdvertiserAccountId;

            if (AdvertiserAccountId > 0)
            {
                if (!_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
                {

                    campaignSettingsDto.IsReadOnly = true;

                }
            }
            return View("CampSettings", campaignSettingsDto);
        }

        //[DenyRole(Roles = "AppOps")]
        [AcceptVerbs(HttpVerbs.Post)]


        public ActionResult CampSettings(CampaignSettingsDto settingsDto, int id, string returnUrl)
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteSettings","SiteMapLocalizations"),
                                                  Order = 3
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =settingsDto.Name,
                                                  Url = Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp? Url.Action("Create", "Campaign", new {id = id,returnUrl=returnUrl}):Url.Action("CreateAll", "Campaign", new {id = id,returnUrl=returnUrl}),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignList", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index")
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    CampaignSettingsDto settingsDtoData = settingsDto;
                 
                        _campaignService.SaveCampSettings(settingsDtoData);
             
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("CampSettings", new { id = id, returnUrl = returnUrl });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }

      
            //ViewData["CreateAdvertiserId"] = AdvertiserId;
            return View("CampSettings", settingsDto);
        }



        #region Ad Group Dynamic bidding

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AdGroupDynamicBiddingConfig(int id, int adGroupId, int? adId)
        {
            AdGroupDynamicBiddingConfigDto viewModel = new AdGroupDynamicBiddingConfigDto();
            if (adId.HasValue)
            {
                viewModel = _campaignService.GetAdGroupDynamicBiddingConfig(id, adGroupId, adId.Value);
            }

         

         
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected =viewModel.Type==0
                                              }};

            //lookupsList.Insert(0, lookupsList[0]);


            //okupsList.Add(new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") });

            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MaximizeCTR).ToString(), Text = BidOptimizationType.MaximizeCTR.ToText(), Selected = ((int)BidOptimizationType.MaximizeCTR)  == (int)viewModel.Type });
            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MinimizeCPC).ToString(), Text = BidOptimizationType.MinimizeCPC.ToText(), Selected = ((int)BidOptimizationType.MinimizeCPC)  == (int)viewModel.Type });
            lookupsList.Add(new SelectListItem { Value = ((int)BidOptimizationType.MinimizeCPA).ToString(), Text = BidOptimizationType.MinimizeCPA.ToText(), Selected = ((int)BidOptimizationType.MinimizeCPA) == (int)viewModel.Type });



            ViewData["BidOptimizationTypeList"] = lookupsList;

            return PartialView("DynamicBidding/DynamicBidding", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAdGroupDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto, bool stop = false)
        {
            //set default times 
        

            try
            {
                if (!saveDto.ID.HasValue)
                {
                    //add
                    _campaignService.AddDynamicBiddingConfig(saveDto);
                }
                else
                {
                    if (stop)
                        //stop
                        _campaignService.RemoveDynamicBiddingConfig(saveDto);
                    else
                        //edit
                        _campaignService.UpdateDynamicBiddingConfig(saveDto);
                }

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                return Json(new { Success = false, ErrorMessage = str });
            }
            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("DynamicBidding", "DynamicBidding")) });
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AdGroupDynamicBiddingConfigs(int id, int adGroupId)
        {
            int campaignId = id;
            var model = LoadAdGroupDynamicBiddingData(null, campaignId, adGroupId);
            model.Elements = GetAdGroupDynamicBiddingConfigQueryResult(null, id, adGroupId);
            return PartialView("DynamicBidding/DynamicBiddings", model);
        }

        // [DenyRole(Roles = "AppOps")]
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _AdGroupDynamicBiddingConfigs(int id, int adGroupId)
        {
            int campaignId = id;
            var result = GetAdGroupDynamicBiddingConfigQueryResult(null, campaignId, adGroupId);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        private AdGroupDynamicBiddingListViewModel LoadAdGroupDynamicBiddingData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int CampaignId, int GroupId)
        {
            //var result = GetAdQueryResult(filter, CampaignId, GroupId);
            //ViewData["total"] = result.TotalCount;
            #region Actions
            // create the actions
            var actions = new List<AdFalcon.Web.Controllers.Model.Action>();
            // Create the tool tip actions
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                               {
                                      new AdFalcon.Web.Controllers.Model.Action()
                                        {
                                           Code = "1",
                                           DisplayText = ResourcesUtilities.GetResource("Stop","Commands"),
                                           ClassName = "grid-tool-tip-remove",
                                           ActionName = "RemoveCostElement",
                                           Type = ActionType.ajax,
                                           CallBack="generateCostElementsGrid();",
                                           ExtraPrams = CampaignId,
                                           ExtraPrams2 = GroupId,

                                        }

                               };
            #endregion

            //var adGroupSettings = _campaignService.GetAdGroupSettings(CampaignId, GroupId);
            return new AdGroupDynamicBiddingListViewModel()
            {
                Elements = new AdGroupDynamicBiddingConfigResultDto(),//result.Items,
                TopActions = actions,
                BelowAction = actions,
                CampaignId = CampaignId,
                AdGroupId = GroupId,
                ToolTips = toolTips,
            };
        }
        private AdGroupCostElementCriteria GetAdGroupDynamicBiddingConfigCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdGroupCostElementCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
            };
            criteria.Page = criteria.Page - 1;
            return criteria;
        }
        private AdGroupDynamicBiddingConfigResultDto GetAdGroupDynamicBiddingConfigQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int campaignId, int GroupId)
        {
            var criteria = GetAdGroupDynamicBiddingConfigCriteria(filter);
            criteria.CampaignId = campaignId;
            criteria.AdGroupId = GroupId;
            var result = _campaignService.GetAdGroupDynamicBiddingConfigs(criteria);
            return result;
        }
        #endregion






        #region Audience List For Advertiser

        #region Actions
        public ActionResult SaveAudienceList(AudienceSegmentDto dto)
        {
            try
            {


             
                dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;



                _AdvertiserService.SaveAudienceSegmentPerAdvertiser(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }

        public ActionResult SaveAudienceListForAdmin(AudienceSegmentDto dto)
        {
            try
            {



                dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;



                _AdvertiserService.SaveAudienceSegmentPerAdvertiserForAdmin(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }


        public ActionResult GetAudienceList(int Id)
        {
            var items = _AdvertiserService.GetAudienceSegmentDto(Id);
            return Json(items, JsonRequestBehavior.AllowGet);

        }



        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public virtual ActionResult AudienceListForAdmin(int Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb


            var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
            var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AudienceList","Global")  ,
                                                  Order = 3,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks2;

            #endregion





            return View("AudienceListForAdmin/Index", AudienceListLoadDataForAdmin(null, Id, false, false));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public virtual ActionResult AudienceListForAdmin(int Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSite","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id > 0)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AudienceList","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeleteAudienceSegmentPerAdvertiserForAdmin(checkedRecords);

            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Activate"]))
            {
                _AdvertiserService.UnDeleteAudienceSegmentPerAdvertiser(checkedRecords);

            }
            return RedirectToAction("AudienceListForAdmin", new { Id = Id });


        }
        public virtual ActionResult AudienceList(int Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

         
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AudienceList","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;

            #endregion
         




            return View("AudienceList/Index", AudienceListLoadData(null, Id, false, false));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult AudienceList(int Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSite","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id>0)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AudienceList","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeleteAudienceSegmentPerAdvertiser(checkedRecords);

            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Activate"]))
            {
                _AdvertiserService.UnDeleteAudienceSegmentPerAdvertiser(checkedRecords);

            }
            return RedirectToAction("AudienceList", new { Id = Id });

          
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AudienceList(int Id)
        {

            var result = GetAudienceListQueryResult(null, Id);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

  
        #endregion

        #region Helpers

        protected AudienceSegmentCriteria GetAudienceListCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AudienceSegmentCriteria
            {
               
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
               DataFrom= filter.FromDate,
               DataTo= filter.ToDate,
                Name = filter.Name,
                showArchived = filter.showArchived
            };


            criteria.Value = criteria.Name;
            criteria.Culture = Thread.CurrentThread.CurrentUICulture.Name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            //criteria.userId = UserId;

         

            if (userIdvar.HasValue)
            {
                criteria.AdvAccountId = userIdvar.Value;

               
            }


            return criteria;
        }
        protected virtual AudienceSegmentResultResultDto GetAudienceListQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal = false)
        {
            var criteria = GetAudienceListCriteria(filter, userIdvar);
          
            var result = _AdvertiserService.GetAudienceSegmentsPerAdvertiser(criteria);
            return result;
        }
        protected virtual AudienceListViewModel AudienceListLoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal = false, bool isMaster = true)
        {
            var result = GetAudienceListQueryResult(filter, userIdvar, showGlobal);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;


     
            var AdvertiserAccountId = 0;
            var advertiserId = 0;
            if (userIdvar.HasValue)
            {
                AdvertiserAccountId = userIdvar.Value;

                var accObje = _AdvertiserService.GetAccountAdvertiserById(AdvertiserAccountId);

                advertiserId = accObje.AdvertiserId.Value;
            }
            var actions = GetAudienceListAction(AdvertiserAccountId);
            var obj = new AudienceListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                AdvertiserAccountId = AdvertiserAccountId,
                AdvertiserId = advertiserId,
                ToolTips = GetAudienceListTooltips(AdvertiserAccountId),
                PreventEdit = AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) :false

            };

            obj.Countries = new Select2ViewModel
            {
                Id = "Countries",
                ActionName = "GetTreeData",
                ControllerName = "Country",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                IsTree = true,
                Single = false,
                AllowClear = true,
                disabled = true,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("CountrySelect", "Global"),
                //OnSelectFunctions = "FixCountriesTree",
                //OnReadyFunctions= "FixCountriesLimit"
            };
            return obj;
        }
        protected virtual AudienceListViewModel AudienceListLoadDataForAdmin(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal = false, bool isMaster = true)
        {
            var result = GetAudienceListQueryResult(filter, userIdvar, showGlobal);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;



            var AdvertiserAccountId = 0;
            var advertiserId = 0;
            if (userIdvar.HasValue)
            {
                AdvertiserAccountId = userIdvar.Value;

                var accObje = _AdvertiserService.GetAccountAdvertiserById(AdvertiserAccountId);

                advertiserId = accObje.AdvertiserId.Value;
            }
            var actions = GetAudienceListActionForAdmin(AdvertiserAccountId);
            var obj = new AudienceListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                AdvertiserAccountId = AdvertiserAccountId,
                AdvertiserId = advertiserId,
                ToolTips = GetAudienceListTooltips(AdvertiserAccountId),
                PreventEdit = AdvertiserAccountId > 0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) : false

            };

            obj.Countries = new Select2ViewModel
            {
                Id = "Countries",
                ActionName = "GetTreeData",
                ControllerName = "Country",
                ClintSideResourceFunction = "",
                IsServerSide = true,
                OptionalParameter = "",
                IsTree = true,
                Single = false,
                AllowClear = true,
                disabled = true,
                PlaceHolder = Framework.Resources.ResourceManager.Instance.GetResource("CountrySelect", "Global"),
                //OnSelectFunctions = "FixCountriesTree",
                //OnReadyFunctions= "FixCountriesLimit"
            };
            return obj;
        }

        protected virtual List<Action> GetAudienceListTooltips(int AdvertiserAccountId)
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
                            AjaxType=AjaxType.rename,
                             Type=ActionType.ajax,
                           ExtraPrams ="Dialog"


                    },
                    new Model.Action()
                    {
                        Code = "1",
                        DisplayText = ResourcesUtilities.GetResource("UploadDevices", "AudienceList"),
                        ClassName = "grid-tool-tip-upload-devices-id",
                        ActionName = "Create",
                        AjaxType=AjaxType.clone,
                        Type=ActionType.ajax,
                        ExtraPrams ="Dialog"


                    }
                };

            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }



            return toolTips;


        }

       
        protected virtual List<Action> GetAudienceListAction( int AdvertiserAccountId)
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,


                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "AudienceList"),
                            ExtraPrams2 = ResourcesUtilities.GetResource( "DeleteAudienceLists","Campaign")// like are u sure


                           
                        }

                    ,
                  
                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,

                            DisplayText =ResourcesUtilities.GetResource("AddNewAudienceList", "Global"),

                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };


            #endregion
            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }
            return actions;
        }
        protected virtual List<Action> GetAudienceListActionForAdmin(int AdvertiserAccountId)
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,


                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "AudienceList"),
                            ExtraPrams2 = ResourcesUtilities.GetResource( "DeleteAudienceLists","Campaign")// like are u sure


                           
                        }

                    ,

                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,

                            DisplayText ="Add New Lookalike Audience Segments",

                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };


            #endregion
            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }
            return actions;
        }

        public ActionResult GetSegmentsList(string q, int AdvAccId)
        {

            return Json(ReturnSegmentsListResult(q, AdvAccId), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<AudienceSegmentDto> ReturnSegmentsListResult(string q, int AdvAccId)
        {
            AudienceSegmentCriteria criteria = new AudienceSegmentCriteria() { Value = q,AdvAccountId= AdvAccId };

            
            var list = _AdvertiserService.GetAudienceSegmentsPerAdvertiser(criteria);
            if (list != null)
            {
                return list.Items;
            }
            return null;
        }

        public ActionResult GetBySegmentsId(string ids)
        {
            IList<AudienceSegmentDto> results = new List<AudienceSegmentDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results, JsonRequestBehavior.AllowGet);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();

            foreach (var id in TagIds)
            {
                var item = _AdvertiserService.GetAudienceSegmentDto(id);
                results.Add(item);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAdGroupTrackingEvent(AdGroupTrackingEventSaveDto dto)
        {
            try
            {



               



                _campaignService.UpdateAdGroupTrackingEvent(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }

        #endregion

        #endregion


        #region Tracking Pixel

        #region Actions

        public ActionResult SaveTrackingPixel(PixelDto dto)
        {
            try
            {



                dto.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

                dto.UserId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

                _AdvertiserService.SavePixel(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }



        public ActionResult GetTrackingPixel(int Id)
        {
            var items = _AdvertiserService.GetPixelById(Id);
            return Json(items, JsonRequestBehavior.AllowGet);

        }

        public virtual ActionResult TrackingPixel(int Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb


            var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
            var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("TrackingPixel", "Menu")  ,
                                                  Order = 3,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks2;

            #endregion




            return View("TrackingPixel/Index", TrackingPixelLoadData(null, Id, false, false));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult TrackingPixel(int Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ContentAppSite","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id > 0)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(Id);
                var breadCrumbLinks2 = new List<BreadCrumbModel>
                                      { new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers"),
                                                     Url = Url.Action("AccountAdvertisers"),
                                                      ExtensionDropDown=true,
                                                  Order = 1,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =dto.Name,
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AudienceList","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                _AdvertiserService.DeletePixel(checkedRecords);

            }

            if (!string.IsNullOrWhiteSpace(Request.Form["DeActivate"]))
            {
                _AdvertiserService.DeActivatePixel(checkedRecords);

            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Activate"]))
            {
                _AdvertiserService.ActivatePixel(checkedRecords);

            }
            return RedirectToAction("TrackingPixel", new { Id = Id });


        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _TrackingPixel(int Id)
        {

            var result = GetTrackingPixelQueryResult(null, Id);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        public ActionResult GetTagText(int Id, int? TagId, int HttpType)
        {
            if (TagId.HasValue)
            { 
            var pixelObjec = _AdvertiserService.GetPixelById(Id);

       
        

                if (TagId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.TrackingPixel.ScriptTag)
                {
                    var TrackConversionsUrlHttp = string.Format(@Config.ScriptTagConv, pixelObjec.Code,pixelObjec.LinkId);
                    var TrackConversionsUrlHttps = string.Format(@Config.ScriptTagConv, pixelObjec.Code, pixelObjec.LinkId);

                    if(HttpType==3)
                    return Content(
                    TrackConversionsUrlHttp
            );
                    else

                        return Content(
               TrackConversionsUrlHttps
       );
                }
                else if (TagId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.TrackingPixel.ImageURL)
            {
                    var TrackConversionsUrlHttp = @Config.ImageURLHTTPConv.Replace("{0}", pixelObjec.Code.ToString());
                    var TrackConversionsUrlHttps = @Config.ImageURLHTTPSConv.Replace("{0}", pixelObjec.Code.ToString());

                    if (HttpType == 3)
                        return Content(
                        TrackConversionsUrlHttp
                );
                    else

                        return Content(
               TrackConversionsUrlHttps
       );
                }
                else if (TagId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.TrackingPixel.MobileTrackingURLAND)
                {
                    var TrackConversionsUrlHttp = @Config.MobileTrackingURLConvHTTPAND.Replace("{0}", pixelObjec.Code.ToString());
                    var TrackConversionsUrlHttps = @Config.MobileTrackingURLConvHTTPSAND.Replace("{0}", pixelObjec.Code.ToString());

                    if (HttpType == 3)
                        return Content(
                        TrackConversionsUrlHttp
                );
                    else

                        return Content(
               TrackConversionsUrlHttps
       );
                }
                else if (TagId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.TrackingPixel.MobileTrackingURLIOS)
                {
                    var TrackConversionsUrlHttp = @Config.MobileTrackingURLConvHTTPIOS.Replace("{0}", pixelObjec.Code.ToString());
                    var TrackConversionsUrlHttps = @Config.MobileTrackingURLConvHTTPSIOS.Replace("{0}", pixelObjec.Code.ToString());

                    if (HttpType == 3)
                        return Content(
                        TrackConversionsUrlHttp
                );
                    else

                        return Content(
               TrackConversionsUrlHttps
       );
                }
                else if (TagId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.TrackingPixel.ImageURL)
                {
                    var TrackConversionsUrlHttp = @Config.ImageURLHTTPConv.Replace("{0}", pixelObjec.Code.ToString());
                    var TrackConversionsUrlHttps =@Config.ImageURLHTTPSConv.Replace("{0}", pixelObjec.Code.ToString());

                    if (HttpType == 3)
                        return Content(
                        TrackConversionsUrlHttp
                );
                    else

                        return Content(
               TrackConversionsUrlHttps
       );
                }


            }
            return Content("");


        

       
         
        }
        #endregion
        #region Helpers

        protected PixelCriteria GetTrackingPixelCriteria(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new PixelCriteria
            {

                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,

                Name = filter.Name,
                showArchived = filter.showArchived
            };


            //criteria.value = criteria.name;
            //criteria.culture = thread.currentthread.currentuiculture.name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            //criteria.userId = UserId;

            if (filter.StatusId.HasValue && filter.StatusId > 0)
                criteria.Status = (PixelStatus)filter.StatusId.Value;

            if (userIdvar.HasValue)
            {
                criteria.AdvAccountId = userIdvar.Value;


            }


            return criteria;
        }
        protected virtual PixelResultDto GetTrackingPixelQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal = false)
        {
            var criteria = GetTrackingPixelCriteria(filter, userIdvar);

            var result = _AdvertiserService.GetPixel(criteria);
            return result;
        }
        protected virtual PixelResultViewModel TrackingPixelLoadData(Noqoush.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int? userIdvar, bool showGlobal = false, bool isMaster = true)
        {
            var result = GetTrackingPixelQueryResult(filter, userIdvar, showGlobal);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;


        
            var AdvertiserAccountId = 0;
            var advertiserId = 0;
            if (userIdvar.HasValue)
            {
                AdvertiserAccountId = userIdvar.Value;

                var accObje = _AdvertiserService.GetAccountAdvertiserById(AdvertiserAccountId);

                advertiserId = accObje.AdvertiserId.Value;
            }
            var actions = GetTrackingPixelAction(AdvertiserAccountId);
            return new PixelResultViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                AdvertiserAccountId = AdvertiserAccountId,
                AdvertiserId = advertiserId,
                ToolTips = GetTrackingPixelTooltips(AdvertiserAccountId),
                PreventEdit = AdvertiserAccountId >0 ? !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId) :false

            };
        }

        protected virtual List<Action> GetTrackingPixelTooltips(int AdvertiserAccountId)
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
                            AjaxType=AjaxType.rename,
                             Type=ActionType.ajax,
                           ExtraPrams ="Dialog"


                    },
                    new Model.Action()
                      {
                            Code = "1",
                             DisplayText = ResourcesUtilities.GetResource("GetTags", "Global"),
                            ClassName = "grid-tool-tip-Tags",
                            ActionName = "Create",
                            AjaxType=AjaxType.clone,
                             Type=ActionType.ajax,
                           ExtraPrams ="Dialog"


                    }
                };




            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>();
            }
            return toolTips;


        }


        protected virtual List<Action> GetTrackingPixelAction(int AdvertiserAccountId)
        {
            #region Actions



            var actions = new List<Model.Action>
                {
                    new Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,


                            DisplayText = ResourcesUtilities.GetResource("Archive", "PMPDeal"),
                            ExtraPrams =ResourcesUtilities.GetResource("SelectConfirmation", "Pixel"),
                            ExtraPrams2 = ResourcesUtilities.GetResource( "DeletePixelLists","Campaign")// like are u sure


                           
                        }

                    ,

                    new Model.Action()
                    {
                        ActionName = "Activate",
                        ClassName = "delete-button",
                        Type = ActionType.Submit,

                        DisplayText = ResourcesUtilities.GetResource("ActivateNow", "Report"),
                        ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Pixel"),
                        ExtraPrams2 = ResourcesUtilities.GetResource("Activate","Confirmation" )


                    },

                    new Model.Action()
                    {
                        ActionName = "DeActivate",
                        ClassName = "delete-button",
                        Type = ActionType.Submit,
                        DisplayText = ResourcesUtilities.GetResource("DeActivate", "Global"),
                        ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Pixel"),
                        ExtraPrams2 = ResourcesUtilities.GetResource("Confirmation" , "DeActivate") // like are u sure ?

       

                           
                    },
                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Submit,

                            DisplayText =ResourcesUtilities.GetResource("AddNewTrackingPixel", "Global"),

                            onClickEvent="AddAdvertiser()",
                            ExtraPrams ="Dialog"
                        }
                };


            #endregion
            if (AdvertiserAccountId > 0 && !_AdvertiserService.IsSubUserHasWriteMode(AdvertiserAccountId))
            {

                actions = new List<Model.Action>();
            }
                return actions;
        }


        public ActionResult GetSegmentsList2(string q, int AdvAccId)
        {

            return Json(ReturnSegmentsListResult2(q, AdvAccId), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<AudienceSegmentDto> ReturnSegmentsListResult2(string q, int AdvAccId)
        {
            AudienceSegmentCriteria criteria = new AudienceSegmentCriteria() { Value = q, AdvAccountId = AdvAccId };


            var list = _AdvertiserService.GetAudienceSegmentsPerAdvertiser(criteria);
            if (list != null)
            {
                return list.Items;
            }
            return null;
        }

        public ActionResult GetBySegmentsId2(string ids)
        {
            IList<AudienceSegmentDto> results = new List<AudienceSegmentDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results, JsonRequestBehavior.AllowGet);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();

            foreach (var id in TagIds)
            {
                var item = _AdvertiserService.GetAudienceSegmentDto(id);
                results.Add(item);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAdGroupTrackingEvent2(AdGroupTrackingEventSaveDto dto)
        {
            try
            {







                _campaignService.UpdateAdGroupTrackingEvent(dto);
                return new JsonResult { Data = new { status = "success" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    return new JsonResult { Data = new { status = "businessException", Message = (e as BusinessException).Errors.FirstOrDefault().Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return new JsonResult { Data = new { status = "faild" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }



        public ActionResult GetPixelsList(string q, int AdvAccId)
        {

            return Json(ReturnPixelsListResult(q, AdvAccId), JsonRequestBehavior.AllowGet);
        }


        private IEnumerable<PixelDto> ReturnPixelsListResult(string q, int AdvAccId)
        {
            PixelCriteria criteria = new PixelCriteria() { Value = q, AdvAccountId = AdvAccId };


            var list = _AdvertiserService.GetPixelsPerAdvertiser(criteria);
            if (list != null)
            {
                return list.Items;
            }
            return null;
        }
        #endregion
        #endregion




        public virtual ActionResult CreateAll(int? AdvertiseraccId, int? id)
        {
            // Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase("CampaignStarted", id.Value.ToString(), null,null));

            //Framework.EventBroker.EventBroker.Instance.Flush();
            int? campaignId = id;
            //string advertiserName = string.Empty;
            int advId = 0;
            string advertiserAccountName = string.Empty;
            if (AdvertiseraccId > 0)
            {
                advertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
                advId = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);
                if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(AdvertiseraccId.Value))
                {

                    throw new AccountNotValidException();

                }
            }
            if (campaignId.HasValue)
            {
                CampaignDto campaignDto = _campaignService.Get(campaignId.Value, CampaignType.Normal,CampaignType.ProgrammaticGuaranteed);


                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    IsRequired = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = campaignDto.Advertiser != null ? campaignDto.Advertiser.Name.ToString() : string.Empty
                };
                //this is update
                #region BreadCrumb


                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =campaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                              Url = Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId})
                                              }
                                           ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                   Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown = true
                                              }
                                      };
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
                #endregion

                var campaignSettingsDto = _campaignService.GetCampSettings(id.Value);
                var AdvertiserId = _campaignService.GetCampaignAdvertiser(id.Value);
                var AdvertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(id.Value);
                string AdvertiserName = _campaignService.GetAdvertiserString(AdvertiserId);

                string AdvertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiserAccountId);



                CampaignAllDto oCampaignAllDto = new CampaignAllDto()
                {
                    oCampaignDto = campaignDto,
                    oCampaignSettingsDto = campaignSettingsDto
                };


                ViewData["AdvertiserIdForTab"] = AdvertiserId;

                ViewData["AdvertiserAccountIdForTab"] = AdvertiserAccountId;
                if (AdvertiseraccId > 0)
                {
                    if (!_AdvertiserService.IsSubUserHasWriteMode(AdvertiseraccId.Value))
                    {

                        campaignDto.IsReadOnly = true;

                    }
                }
                if (campaignDto.IsReadOnly)
                {
                    AddMessages(ResourcesUtilities.GetResource("LockedCampWarning", "Campaign"), MessagesType.Warning);
                }
                return View(oCampaignAllDto);
            }
            else
            {
                CampaignDto campaignDto = new CampaignDto();
                campaignDto.Advertiser = _AdvertiserService.Get(advId);
                campaignDto.AdvertiserAccountId = AdvertiseraccId.Value;
                campaignDto.StartDate = Noqoush.Framework.Utilities.Environment.GetServerTime();

                CampaignAllDto oCampaignAllDto = new CampaignAllDto()
                {
                    oCampaignDto = campaignDto,
                    oCampaignSettingsDto = new CampaignSettingsDto()
                };

                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    IsRequired = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = string.Empty
                };
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("NewCampaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order =3,
                                                  Url = Url.Action("Index", new { AdvertiseraccId=id})
                                              }

                                          ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                  Url = Url.Action("AccountAdvertisers"),
                                                  ExtensionDropDown = true
                                              }

                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                #endregion
                if (AdvertiseraccId > 0)
                {

                    ViewData["CreateAdvertiserId"] = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);

                }
                return View(oCampaignAllDto);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]
        public virtual ActionResult CreateAll(CampaignAllDto oCampaignAllDto, int? AdvertiseraccId, int? id, string returnUrl)
        {
            //string advertiserName = string.Empty;
            string advertiserAccountName = string.Empty;

            ViewData["CreateAdvertiserAccountId"] = AdvertiseraccId.Value;

            if (AdvertiseraccId > 0)
            {

                ViewData["CreateAdvertiserId"] = _campaignService.GetAdvertiserIdFromAccount(AdvertiseraccId.Value);

                // advertiserName = _campaignService.GetAdvertiserString(AdvertiserId.Value);
                advertiserAccountName = _campaignService.GetAdvertiserAccountString(AdvertiseraccId.Value);
            }
            #region BreadCrumb
            //TODO:osaleh to use the old name not the new name in breadcrumb if an exception is been thrown 
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = oCampaignAllDto.oCampaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url = Url.Action("Index", new { AdvertiseraccId=AdvertiseraccId})
                                              }
                                             ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =advertiserAccountName,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Advertisers", "Global"),
                                                  Order = 1,
                                                   Url = Url.Action("AccountAdvertisers"),
                                                   ExtensionDropDown = true
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            int? campaignId = id;

            if (ModelState.IsValid)
            {
                if (campaignId.HasValue)
                {
                    //this is update
                    oCampaignAllDto.oCampaignDto.ID = campaignId.Value;
                   // oCampaignAllDto.oCampaignDto.CampaignType = CampaignType.Normal;
                    try
                    {

                        // var result = _campaignService.Save(oCampaignAllDto.oCampaignDto);
                      //  oCampaignAllDto.oCampaignDto.CampaignType = CampaignType.Normal;
                        var result = _campaignService.Save(oCampaignAllDto.oCampaignDto);
                        if (result.Warnings != null)
                        {
                            foreach (var warning in result.Warnings)
                            {
                                AddMessages(warning.Message, MessagesType.Warning);
                            }
                        }
                        //CampaignSettingsDto settingsDtoData = oCampaignAllDto.oCampaignSettingsDto;
                        //_campaignService.SaveCampSettings(settingsDtoData);
                        CampaignSettingsDto settingsDtoData = oCampaignAllDto.oCampaignSettingsDto;
                        settingsDtoData.ID = campaignId.Value;
                        _campaignService.SaveCampSettings(settingsDtoData);
                        AddSuccessfullyMsg();
                        MoveMessagesTempData();
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("CreateAll", new { AdvertiseraccId = AdvertiseraccId, id = result.ID });
                        }
                        else
                        {
                            return RedirectToAction("CreateAll", new { AdvertiseraccId = AdvertiseraccId, id = result.ID, returnUrl = returnUrl });
                        }

                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                    }
                    ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                    {
                        Id = "Advertisers_Name",
                        Name = "Advertisers.Name",
                        ActionName = "GetAdvertisers",
                        ControllerName = "Advertiser",
                        LabelExpression = "item.Name",
                        ValueExpression = "item.Id",
                        IsAjax = true,
                        ChangeCallBack = "AdvertisersChanged",
                        CurrentText = oCampaignAllDto.oCampaignDto.Advertiser != null && _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID).Name.ToString() : string.Empty
                    };
                    return View(oCampaignAllDto);
                }
                else
                {
                    int newId = 0;
                    try
                    {
                        oCampaignAllDto.oCampaignDto.CampaignType = CampaignType.Normal;
                        var result = _campaignService.Save(oCampaignAllDto.oCampaignDto);
                        newId = result.ID;
                        if (result.Warnings != null)
                        {
                            foreach (var warning in result.Warnings)
                            {
                                AddMessages(warning.Message, MessagesType.Warning);
                            }
                        }

                        CampaignSettingsDto settingsDtoData = oCampaignAllDto.oCampaignSettingsDto;
                        settingsDtoData.ID = newId;
                        _campaignService.SaveCampSettings(settingsDtoData);

                        AddSuccessfullyMsg();
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                        ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                        {
                            Id = "Advertisers_Name",
                            Name = "Advertisers.Name",
                            ActionName = "GetAdvertisers",
                            ControllerName = "Advertiser",
                            LabelExpression = "item.Name",
                            ValueExpression = "item.Id",
                            IsAjax = true,
                            ChangeCallBack = "AdvertisersChanged",
                            CurrentText = oCampaignAllDto.oCampaignDto.Advertiser != null && _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID).Name.ToString() : string.Empty
                        };
                        return View(oCampaignAllDto);
                    }
                    MoveMessagesTempData();
                    if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
                    {
                        return RedirectToAction("Objective", new { id = newId });
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("CreateAll", new { AdvertiseraccId = AdvertiseraccId, id = newId });
                        }
                        else
                        {
                            return RedirectToAction("CreateAll", new { AdvertiseraccId = AdvertiseraccId, id = newId, returnUrl = returnUrl });
                        }
                    }
                }

            }
            else
            {
                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
                {
                    Id = "Advertisers_Name",
                    Name = "Advertisers.Name",
                    ActionName = "GetAdvertisers",
                    ControllerName = "Advertiser",
                    LabelExpression = "item.Name",
                    ValueExpression = "item.Id",
                    IsAjax = true,
                    ChangeCallBack = "AdvertisersChanged",
                    CurrentText = oCampaignAllDto.oCampaignDto.Advertiser != null && _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID).Name.ToString() : string.Empty
                };
                return View(oCampaignAllDto);
            }
        }

    }
}
