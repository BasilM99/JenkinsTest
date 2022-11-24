
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.HouseAd;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DSP,AccountRole.DataProvider })]
    public class HouseAdController : CampaignController
    {
        protected IHouseAdService houseAdService;
        private ITrackingEventService _trackingEventService;
        private IAccountService _accountService;

        public HouseAdController(
                        IAudienceSegmentService AudienceSegmentService,
                                                                     IAdvertiserService AdvertiserService,

                        IAccountService accountService,

                                ICampaignService campaignService,
                                IKeywordService keywordService,
                                IObjectiveTypeService objectiveTypeService,
                                ICreativeUnitService creativeUnitService,
                                IAgeGroupService ageGroupService,
                                ITileImageService tileImageService,
                                                          IAppSiteService appSiteService,

            ITrackingEventService trackingEventService,
                                IAdCreativeStatusService adCreativeStatusService, IDeviceCapabilityService deviceCapabilityService,
                                IHouseAdService houseAdService, IRichMediaRequiredProtocolService richMediaRequiredProtocolService,
                                ILocationService locationService, IDeviceTypeService deviceTypeService, IPlatformService platformService, ICostModelWrapperService costModelWrapperService, IVideoTypeService videoTypeService, IVideoDeliveryMethodsService videoDeliveryMethodsService,
             IAppSiteTypeService appSiteTypeService, IAppMarketingPartnerService appMarketingPartnerService, IUserService userService, ICreativeVendorService creativeVendorService, ILanguageService languageService,IReportService ReportService, IPartyService partService, ILookupService lookupService)
            : base(AudienceSegmentService, accountService, campaignService, keywordService, objectiveTypeService, creativeUnitService, ageGroupService, tileImageService, adCreativeStatusService, deviceCapabilityService, richMediaRequiredProtocolService, locationService, deviceTypeService, trackingEventService, platformService, costModelWrapperService, videoTypeService, videoDeliveryMethodsService, appSiteTypeService, appMarketingPartnerService, userService, appSiteService, AdvertiserService, creativeVendorService, languageService, ReportService, partService, lookupService)
        {
            this.houseAdService = houseAdService;
        }

        //public virtual ActionResult Create(int? id)
        //{
        //    int? campaignId = id;
        //    string advertiserName = string.Empty;

        //    if (campaignId.HasValue)
        //    {
        //        CampaignDto campaignDto = _campaignService.Get(campaignId.Value, CampaignType.Normal);

        //        ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //        {
        //            Id = "Advertisers_Name",
        //            Name = "Advertisers.Name",
        //            ActionName = "GetAdvertisers",
        //            ControllerName = "Advertiser",
        //            LabelExpression = "item.Name",
        //            ValueExpression = "item.Id",
        //            IsAjax = true,
        //            IsRequired = true,
        //            ChangeCallBack = "AdvertisersChanged",
        //            CurrentText = campaignDto.Advertiser != null ? campaignDto.Advertiser.Name.ToString() : string.Empty
        //        };
        //        //this is update
        //        #region BreadCrumb

        //        var breadCrumbLinks = new List<BreadCrumbModel>
        //                              {
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =campaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
        //                                          Order = 2
        //                                      },
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
        //                                          Order = 1,
        //                                          Url = Url.Action("Index")
        //                                      }
        //                              };
        //        ViewData["BreadCrumbLinks"] = breadCrumbLinks;
        //        #endregion
        //        return View(campaignDto);
        //    }
        //    else
        //    {
        //        ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //        {
        //            Id = "Advertisers_Name",
        //            Name = "Advertisers.Name",
        //            ActionName = "GetAdvertisers",
        //            ControllerName = "Advertiser",
        //            LabelExpression = "item.Name",
        //            ValueExpression = "item.Id",
        //            IsAjax = true,
        //            IsRequired = true,
        //            ChangeCallBack = "AdvertisersChanged",
        //            CurrentText = string.Empty
        //        };
        //        #region BreadCrumb

        //        var breadCrumbLinks = new List<BreadCrumbModel>
        //                              {
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource("NewCampaign","SiteMapLocalizations"),
        //                                          Order = 2
        //                                      },
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
        //                                          Order = 1,
        //                                          Url = Url.Action("Index")
        //                                      }
        //                              };

        //        ViewData["BreadCrumbLinks"] = breadCrumbLinks;

        //        #endregion
        //        return View();
        //    }
        //}
        //[AcceptVerbs(HttpVerbs.Post)]
        //[DenyRole(Roles = "AccountManager")]
        //public virtual ActionResult Create(CampaignDto campaignDto, int? id, string returnUrl)
        //{
        //    #region BreadCrumb
        //    //TODO:osaleh to use the old name not the new name in breadcrumb if an exception is been thrown 
        //    var breadCrumbLinks = new List<BreadCrumbModel>
        //                              {
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =campaignDto.Name,//ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
        //                                          Order = 2
        //                                      },
        //                                  new BreadCrumbModel()
        //                                      {
        //                                          Text =ResourcesUtilities.GetResource(GetResourceKey("CampaignList"), "SiteMapLocalizations"),
        //                                          Order = 1,
        //                                          Url = Url.Action("Index")
        //                                      }
        //                              };
        //    ViewData["BreadCrumbLinks"] = breadCrumbLinks;
        //    #endregion
        //    int? campaignId = id;

        //    if (ModelState.IsValid)
        //    {
        //        if (campaignId.HasValue)
        //        {
        //            //this is update
        //            campaignDto.ID = campaignId.Value;
        //            campaignDto.CampaignType = CampaignType.Normal;
        //            try
        //            {

        //                var result = _campaignService.Save(campaignDto);
        //                if (result.Warnings != null)
        //                {
        //                    foreach (var warning in result.Warnings)
        //                    {
        //                        AddMessages(warning.Message, MessagesType.Warning);
        //                    }
        //                }
        //                AddSuccessfullyMsg();
        //                MoveMessagesTempData();
        //                if (string.IsNullOrWhiteSpace(returnUrl))
        //                {
        //                    return RedirectToAction("Create", new { id = result.ID });
        //                }
        //                else
        //                {
        //                    return RedirectToAction("Create", new { id = result.ID, returnUrl = returnUrl });
        //                }
        //            }
        //            catch (BusinessException exception)
        //            {
        //                foreach (var errorData in exception.Errors)
        //                {
        //                    AddMessages(errorData.Message, MessagesType.Error);
        //                }
        //            }
        //            ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //            {
        //                Id = "Advertisers_Name",
        //                Name = "Advertisers.Name",
        //                ActionName = "GetAdvertisers",
        //                ControllerName = "Advertiser",
        //                LabelExpression = "item.Name",
        //                ValueExpression = "item.Id",
        //                IsAjax = true,
        //                ChangeCallBack = "AdvertisersChanged",
        //                CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
        //            };
        //            return View(campaignDto);
        //        }
        //        else
        //        {
        //            int newId = 0;
        //            try
        //            {
        //                campaignDto.CampaignType = CampaignType.Normal;
        //                var result = _campaignService.Save(campaignDto);
        //                newId = result.ID;
        //                if (result.Warnings != null)
        //                {
        //                    foreach (var warning in result.Warnings)
        //                    {
        //                        AddMessages(warning.Message, MessagesType.Warning);
        //                    }
        //                }
        //                AddSuccessfullyMsg();
        //            }
        //            catch (BusinessException exception)
        //            {
        //                foreach (var errorData in exception.Errors)
        //                {
        //                    AddMessages(errorData.Message, MessagesType.Error);
        //                }
        //                ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //                {
        //                    Id = "Advertisers_Name",
        //                    Name = "Advertisers.Name",
        //                    ActionName = "GetAdvertisers",
        //                    ControllerName = "Advertiser",
        //                    LabelExpression = "item.Name",
        //                    ValueExpression = "item.Id",
        //                    IsAjax = true,
        //                    ChangeCallBack = "AdvertisersChanged",
        //                    CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
        //                };
        //                return View(campaignDto);
        //            }
        //            MoveMessagesTempData();
        //            if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
        //            {
        //                return RedirectToAction("Objective", new { id = newId });
        //            }
        //            else
        //            {
        //                if (string.IsNullOrWhiteSpace(returnUrl))
        //                {
        //                    return RedirectToAction("Create", new { id = newId });
        //                }
        //                else
        //                {
        //                    return RedirectToAction("Create", new { id = newId, returnUrl = returnUrl });
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        ViewBag.AdvertiserAutoComplete = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
        //        {
        //            Id = "Advertisers_Name",
        //            Name = "Advertisers.Name",
        //            ActionName = "GetAdvertisers",
        //            ControllerName = "Advertiser",
        //            LabelExpression = "item.Name",
        //            ValueExpression = "item.Id",
        //            IsAjax = true,
        //            ChangeCallBack = "AdvertisersChanged",
        //            CurrentText = campaignDto.Advertiser != null && _AdvertiserService.Get(campaignDto.Advertiser.ID) != null ? _AdvertiserService.Get(campaignDto.Advertiser.ID).Name.ToString() : string.Empty
        //        };
        //        return View(campaignDto);
        //    }
        //}
        protected override string GetResourceKey(string resourceKey)
        {
            return "HouseAdList";
        }
        #region Campaign
        #region Index
        #region Helpers
        override protected List<Model.Action> GetCampaignAction()
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

                           
                        },
                    new Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewCampaign", "Commands")
                        }
                };
            #endregion

            return actions;
        }

        override protected List<Model.Action> GetCampaignTooltip()
        {
            // Create the tool tip actions
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "Create"
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
                            ActionName = "CopyCampaign",
                            Type = ActionType.ajax,
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
            return toolTips;
        }
        protected override CampaignListResultDto GetQueryResult(Model.Campaign.Filter filter)
        {
            var criteria = GetCriteria(filter);
            var result = houseAdService.QueryHouseAdsCampaignsCratiria(criteria);
            return result;
        }
        protected override Model.Campaign.ListViewModel LoadData(Model.Campaign.Filter filter)
        {
            return base.LoadData(filter);
        }
        #endregion
        public override ActionResult Index(int? AdvertiseraccId)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                {
                    new BreadCrumbModel()
                        {
                            Text = ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                            Order = 1,
                        }
                };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View(LoadData(null));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public override ActionResult Index(int? AdvertiseraccId,int[] checkedRecords)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete Campaigns

                _campaignService.Delete(checkedRecords);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["run"]))
                {
                    //run  selected Campaigns
                    _campaignService.Run(checkedRecords);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["pause"]))
                    {
                        //pause selected Campaigns
                        _campaignService.Pause(checkedRecords);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [GridAction(EnableCustomBinding = true)]
        public override ActionResult _Index(int? AdvertiseraccId)
        {

            var result = GetQueryResult(null);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        #endregion
        #region Create
        public override ActionResult Create(int? AdvertiseraccId, int? id)
        {
            int? houseAdId = id;

            if (houseAdId.HasValue)
            {
                var houseAdCampaign = houseAdService.GetHouseAdCampaign(houseAdId.Value);
                //this is update
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =houseAdCampaign.Name,
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index")
                                              }
                                      };
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
                #endregion
                return View(houseAdCampaign);
            }
            else
            {
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("NewHouseAd","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index")
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
                #endregion
                return View();
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public override ActionResult Create(CampaignDto campaignDto, int? AdvertiseraccId, int? id, string returnUrl)
        {
            #region BreadCrumb
            //TODO:osaleh to use the old name not the new name in breadcrumb if an exception is been thrown 
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =campaignDto.Name,
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index")
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion
            int? houseAdId = id;
            campaignDto.CampaignType = CampaignType.AdHouse;
            if (ModelState.IsValid)
            {
                if (houseAdId.HasValue)
                {
                    //this is update
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
                            return RedirectToAction("Create", new { id = result.ID });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { id = result.ID, returnUrl = returnUrl });
                        }
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }
                    }
                    return View(campaignDto);
                }
                else
                {
                    int newId = 0;
                    try
                    {
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
                        return View(campaignDto);
                    }
                    MoveMessagesTempData();
                    if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
                    {
                        return RedirectToAction("CreateHouseAd", new { id = newId });
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Create", new { id = newId });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { id = newId, returnUrl = returnUrl });
                        }
                    }
                }

            }
            else
            {
                return View(campaignDto);
            }
        }
        #endregion
        #endregion

        #region Ad Groups

        #region Index
        override protected List<Model.Action> GetAdGroupTooltips(int campaignId,int AdvertiseraccId= 0)
        {
            // Create the tool tip actions
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {
                    new Model.Action()
                        {
                            Code = "0",
                            DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                            ClassName = "grid-tool-tip-edit",
                            ActionName = "CreateHouseAd",
                            ExtraPrams = campaignId
                        },
                        new Model.Action()
                        {
                            Code = "1",
                            DisplayText = ResourcesUtilities.GetResource("Targeting", "Commands"),
                            ClassName = "grid-tool-tip-targeting",
                            ActionName = "Targeting",
                            ExtraPrams = campaignId
                        },
                    new Model.Action()
                        {
                            Code = "2",
                            DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                            ClassName = "grid-tool-tip-reports",
                            URL = Url.Action("index", "Reports", new {reportType = "ad"})
                        },
                    new Model.Action()
                        {
                            Code = "3",
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
                            ExtraPrams = campaignId

                       }
                };
            return toolTips;
        }
        public override ActionResult RedirectToAuditTrial(int id)
        {
            return base.RedirectToAuditTrial(id);

        }

        override protected List<Model.Action> GetAdGroupActions(int campaignId, int AdvertiseraccId = 0)
        {
            // create the actions
            var actions = new List<Model.Action>
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
                            ActionName = "CreateHouseAd",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddNewGroup", "Commands"),
                            ExtraPrams = campaignId
                        }
                };
            return actions;
        }
        #endregion
        #region Objective
        public ActionResult CreateHouseAd(int id, int? adGroupId)
        {
            var objectiveViewModel = new HouseAdSaveModel();
            if (!adGroupId.HasValue)
            {
                var campaign = _campaignService.Get(id, CampaignType.AdHouse, CampaignType.Undefined);
                objectiveViewModel = new HouseAdSaveModel
                {
                    CampaignId = id,
                    Name = campaign.Name
                };
            }
            else
            {
                var houseAd = houseAdService.Get(adGroupId.Value);
                objectiveViewModel = new HouseAdSaveModel
                {
                    CampaignId = id,
                    DeliveryMode = houseAd.DeliveryMode,
                    DestinationAppSites = string.Join(",", houseAd.DestinationAppSites.Select(x => x.ID)),
                    ForAppSite = houseAd.ForAppSite.ID,
                    Name = houseAd.AdGroup.Name
                };
            }

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
                                                  Text =objectiveViewModel.Name,
                                                  Order = 2,
                                                  Url=Url.Action("create",new {id= id,adGroupId=adGroupId})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View(objectiveViewModel);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult CreateHouseAd(int id, int? adGroupId, HouseAdSaveModel houseAdSaveModel)
        {
            int campaignId = id;
            if (ModelState.IsValid)
            {
                var adGroupDto = new HouseAdGroupDto()
                {
                    ID = adGroupId.HasValue ? adGroupId.Value : 0,
                    CampaignId = campaignId,
                    Name = houseAdSaveModel.Name,
                    DestinationAppSites = houseAdSaveModel.DestinationAppSites.Split(',').Select(x => Convert.ToInt32(x)).ToList(),
                    DeliveryMode = houseAdSaveModel.DeliveryMode,
                    ForAppSite = houseAdSaveModel.ForAppSite,
                };
                try
                {
                    var groupid = houseAdService.SaveAdGroup(adGroupDto);
                    if (!string.IsNullOrWhiteSpace(Request.Form["Save"]))
                    {
                        AddSuccessfullyMsg();
                        MoveMessagesTempData();
                        return RedirectToAction("CreateHouseAd", new { id = campaignId, adGroupId = groupid });
                    }
                    else
                    {
                        return RedirectToAction("Targeting", new { id = campaignId, adGroupId = groupid });
                    }
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }

            }
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
                                                  Text =houseAdSaveModel.Name,
                                                  Order = 2,
                                                  Url=Url.Action("create",new {id= id,adGroupId=adGroupId})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(houseAdSaveModel);
        }
        #endregion
        #region Targeting
        #region Helpers
        #endregion
        #region Actions
        override public ActionResult Targeting( int id, int adGroupId, bool returenFromAdsPage = false)
        {
            int campaignId = id;
            var model = GetTargetingViewModel(campaignId, adGroupId);
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AdGroupName,
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
                                                  Text =model.CampaignName,
                                                  Order = 2,
                                                  Url=Url.Action("create",new {id= id})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ChangeJavaScriptSet("houseAdTargetingActionJs");
            return View(model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        override public ActionResult Targeting(TargetingSaveModel targetingSaveModel, BidGetModel bidGetModel, string returnUrl, bool IsContinue = false)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //TODO : Osaleh to Move this code
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
                                    if (string.IsNullOrWhiteSpace(targetingSaveModel.Platforms))
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
                    var saveModel = GetTargetingSaveDto(targetingSaveModel);
                    saveModel.BinInfo = null;
                    saveModel.isFromHouseAd = true;
                    _campaignService.SaveTargeting(saveModel);
                    if (!string.IsNullOrWhiteSpace(Request.Form["Continue"]))
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
                }

            }
            var model = GetTargetingViewModel(targetingSaveModel.CampaignId, targetingSaveModel.AdGroupId);
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AdGroup", "SiteMapLocalizations"),
                                                  Order = 4,
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAdGroups", "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url=Url.Action("Groups",new {id= targetingSaveModel.CampaignId})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =model.CampaignName,//ResourcesUtilities.GetResource("Campaign", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url=Url.Action("create",new {id= targetingSaveModel.CampaignId})
                                              },
                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("HouseAdList", "SiteMapLocalizations"),
                                                  Url=Url.Action("Index"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            ChangeJavaScriptSet("houseAdTargetingActionJs");
            return View(model);
        }
        public JsonResult GetBid(BidGetModel bidGetModel)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
        #endregion

        #region Ads
        #region Index
        override protected List<Model.Action> GetAdTooltips(int CampaignId, int GroupId, int AdvertiseraccId = 0)
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
                        },
                     new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",
                            ExtraPrams = CampaignId


        }
                };
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

        override protected List<Model.Action> GetAdActions(int CampaignId, int GroupId, AdsSearchDto result, int AdvertiseraccId = 0)
        {
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


            actions.Add(new Model.Action()
            {
                ActionName = "Creative",
                ClassName = "primary-btn",
                Type = ActionType.Link,
                DisplayText = ResourcesUtilities.GetResource("AddNewAds", "Commands"),
                ExtraPrams = CampaignId,
                ExtraPrams2 = GroupId
            });
            return actions;
        }

        #endregion
        override public ActionResult Creative(int id, int? adTypeId, int adGroupId, int? adId)
        {
            int campaignId = id;
            var model = GetCreativeViewModel(campaignId, adGroupId, adId, adTypeId);
            #region BreadCrumb
            if (adId.HasValue)
            {
                var breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("Ad", "SiteMapLocalizations"),
                                                      Order = 6,
                                                  },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url =Url.Action("Ads", new {id = id, adGroupId = adGroupId, isHouseAd=true})
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
                                                      Url = Url.Action("create", new {id = id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("HouseAdList","SiteMapLocalizations"),
                                                      Url = Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            }
            else
            {
                var breadCrumbLinks = new List<BreadCrumbModel>
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
                                                      Url = Url.Action("create", new {id = id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("HouseAdList","SiteMapLocalizations"),
                                                      Url = Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            }

            #endregion
            var viewName = string.Empty;

            switch (model.AdCreativeDto.Group.ActionTypeId)
            {
                case AdActionTypeIds.RichMedia:
                    {
                        if (!adId.HasValue && !Config.IsAdministrationApp)
                        {
                            throw new NotAuthenticatedException();
                        }
                        viewName = "RichMediaCreative";
                        model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                        break;
                    }
                case AdActionTypeIds.Interstitial:
                    {
                        if (!adId.HasValue && !Config.IsAdministrationApp)
                        {
                            throw new NotAuthenticatedException();
                        }
                        viewName = "InterstitialCreative";
                        model.AdCreativeDto.TypeId = AdTypeIds.RichMedia;
                        break;
                    }
                default:
                    {
                        viewName = "Creative";
                        break;
                    }

            }
            ChangeJavaScriptSet("adCreativeActionJs");
            return View("Creative", model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        override public ActionResult Creative( int id, int adGroupId, int? adId, CreativeSaveViewModel model, string returnUrl)
        {
            int campaignId = id;
            CreativeViewModel viewModel = null;
            if (ModelState.IsValid)
            {
                //update
                try
                {
                    var adCreative = GetAdCreativeSaveDto(model, adGroupId, campaignId);
                    adCreative.ID = adId.HasValue ? adId.Value : 0;
                    adId = _campaignService.SaveAd(campaignId, adGroupId, adCreative);
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Summary", new { id = campaignId, adGroupId = adGroupId, adId = adId });
                    }
                    else
                    {
                        return RedirectToAction("Summary",
                                                new
                                                {
                                                    id = campaignId,
                                                    adGroupId = adGroupId,
                                                    adId = adId,
                                                    returnUrl = returnUrl
                                                });
                    }
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

            #region BreadCrumb

            if (adId.HasValue)
            {
                var breadCrumbLinks = new List<BreadCrumbModel>
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
                                                      Url = Url.Action("create", new {id = id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("HouseAdList","SiteMapLocalizations"),
                                                      Url = Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            }
            else
            {
                var breadCrumbLinks = new List<BreadCrumbModel>
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
                                                      Url = Url.Action("create", new {id = id})
                                                  },
                                              new BreadCrumbModel()
                                                  {
                                                      Text =ResourcesUtilities.GetResource("HouseAdList","SiteMapLocalizations"),
                                                      Url = Url.Action("Index"),
                                                      Order = 1,
                                                  }
                                          };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            }

            #endregion
            ChangeJavaScriptSet("adCreativeActionJs");
            return View("Creative", viewModel);

        }
        override public ActionResult SetAdsBid(int campaignId, int adGroupId, int[] adIds, string bid)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
