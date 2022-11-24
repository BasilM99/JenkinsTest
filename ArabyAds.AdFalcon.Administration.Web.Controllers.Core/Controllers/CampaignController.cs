using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Account;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.CostElemnts;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;

using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using System.Text.Json;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Web.Controllers;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections;
using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class CampaignController : ArabyAds.AdFalcon.Web.Controllers.Controllers.CampaignController
    {
        protected ILookupService lookupService;
        protected IAdCreativeAttributeService adCreativeAttributeService;
        private IAppSiteTypeService _appSiteTypeService;
        private ITrackingEventService _trackingEventService;
        private IAppSiteService _appSiteService;
        private IAppSiteStatusService _appSiteStatusService;
        private IUserService _userService;
        private IAccountService _accountService;
        protected IPartyService partyService;
        //protected ICreativeVendorService creativeVendorService;
        private IFundTransactionService _fundTransactionService;
        public CampaignController(
                     IWebHostEnvironment host, IOptions<JsonOptions> jsonOptions, FilterController filterController) : base(host, jsonOptions, filterController)

        {
            this.lookupService = IoC.Instance.Resolve<ILookupService>(); ;
            this.adCreativeAttributeService = IoC.Instance.Resolve<IAdCreativeAttributeService>(); ;
            this._appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>(); ;
            this._trackingEventService = IoC.Instance.Resolve<ITrackingEventService>(); ;
            this._appSiteStatusService = IoC.Instance.Resolve<IAppSiteStatusService>(); ;
            this._userService = IoC.Instance.Resolve<IUserService>(); ;
            this._appSiteService = IoC.Instance.Resolve<IAppSiteService>(); ;
            this.partyService = IoC.Instance.Resolve<IPartyService>(); ;
            this._fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>(); ;
        }

        public ActionResult testReceiveEvent()
        {

            _fundTransactionService.testPublickEventKafka();

            return null;

        }

        #region Helpers
        private ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter getDefualtFilter()
        {
            var filter = new AdFalcon.Web.Controllers.Model.Campaign.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            return filter;
        }
        private AdGroupCostElementCriteria GetAdCriteria(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
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
        private AdRequestCriteria GetAdRequestCriteria(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AdRequestCriteria
            {
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
            };
            criteria.Page = criteria.Page - 1;
            return criteria;
        }
        private AdGroupCostElementResultDto GetAdQueryResult(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int campaignId, int GroupId)
        {
            var criteria = GetAdCriteria(filter);
            criteria.CampaignId = campaignId;
            criteria.AdGroupId = GroupId;
            var result = _campaignService.GetAdGroupCostElements(criteria);
            return result;
        }

        private AdRequestTargetingDtoResultDto GetAdRequestQueryResult(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int GroupId)
        {
            var criteria = GetAdRequestCriteria(filter);
            criteria.AdGroupId = GroupId;
            var result = _campaignService.GetAdRequestTargetings(criteria);
            return result;
        }
        private CostElementsListViewModel LoadCostElementsData(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int CampaignId, int GroupId)
        {
            //var result = GetAdQueryResult(filter, CampaignId, GroupId);
            //ViewData["total"] = result.TotalCount;
            #region Actions
            // create the actions
            var actions = new List<AdFalcon.Web.Controllers.Model.Action>();
            // Create the tool tip actions
            var toolTips = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
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

            var adGroupSettings = _campaignService.GetAdGroupSettings(new CampaignIdAdgroupIdMessage { CampaignId = CampaignId, AdgroupId = GroupId });
            return new CostElementsListViewModel()
            {
                Elements = new AdGroupCostElementResultDto(),//result.Items,
                TopActions = actions,
                BelowAction = actions,
                CampaignId = CampaignId,
                AdGroupId = GroupId,
                ToolTips = toolTips,
            };
        }
        private string GetCostValue(CostElementDto element)
        {
            IList<string> keyValueList = new List<string>();

            switch (element.TypeId)
            {
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Fixed:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value));
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
        private List<SelectListItem> GetCostElementList(string lookupType, int selectedValue)
        {
            var items = lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = lookupType });
            items.Items = items.Items.OrderBy(x => x.Name.Value, StringComparer.InvariantCultureIgnoreCase).ToList();
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0#1#0#1",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};

            if (LookupNames.CostElement == lookupType)
            {
                lookupsList.AddRange(
                    items.Items.Select(
                        item => new SelectListItem()
                        {
                            Value = string.Format("{0}#{1}#{2}#{3}#{4}", item.ID.ToString(), (item as CostElementDto).Scope, (item as CostElementDto).TypeId, GetCostValue(item as CostElementDto), (int)(item as CostElementDto).CostElementCalculatedFrom),
                            Text = item.CustomName.ToString(),
                            Selected = item.ID == selectedValue
                        }));

            }
            else

            {

                lookupsList.AddRange(
                     items.Items.Select(
                         item => new SelectListItem()
                         {
                             Value = string.Format("{0}#{1}#{2}#{3}#{4}", item.ID.ToString(), (item as CostElementDto).Scope, (item as CostElementDto).TypeId, GetCostValue(item as CostElementDto), (int)(item as CostElementDto).CostElementCalculatedFrom),
                             Text = item.Name.ToString(),
                             Selected = item.ID == selectedValue
                         }));
            }

            return lookupsList;
        }
        #endregion
        #region AdAprove

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult AdDetails(int id, int adGroupId, int adId)
        {

            /*int campaignId = id;
            //if (!_campaignService.IsReadOrWriteCampaign(campaignId))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}
            var model = _campaignService.GetAdFullSummary(new CampaignIdAdgroupIdAdIdMessage { CampaignId = campaignId, AdgroupId = adGroupId, AdId = adId });


            //if (model.AdvertiserAccountId > 0)
            //{
            //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(model.AdvertiserAccountId))
            //    {
            //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //    }
            //}

            var summaryViewModel = GetFullSummaryViewModel(model);
            summaryViewModel.ViewSummary.isSummary = false;
            ViewData["IsDownloadAction"] = IsDownloadAction(model.ActionId);


            #region BreadCrumb

            string controllerName = model.Campaign.CampaignTypeEnum == (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.CampaignType.Normal || summaryViewModel.ViewSummary.Campaign.CampaignTypeEnum == (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.CampaignType.ProgrammaticGuaranteed ? "Campaign" : "HouseAd";



            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = model.Name,
                                                  //ResourcesUtilities.GetResource("Ad", "SiteMapLocalizations"),
                                                  Order = 6,
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =
                                                      ResourcesUtilities.GetResource("Ads", "SiteMapLocalizations"),
                                                  Order = 5,
                                                  Url = Url.Action("Ads",controllerName, new {id = id, adGroupId = adGroupId})
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = model.Group.Name,
                                                  //ResourcesUtilities.GetResource("AdGroup","SiteMapLocalizations"),
                                                  Order = 4,
                                                  Url =
                                                      Url.Action("Targeting",controllerName, new {id = id, adGroupId = adGroupId})
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =
                                                      ResourcesUtilities.GetResource("CampaignAdGroups",
                                                                                     "SiteMapLocalizations"),
                                                  Order = 3,
                                                  Url = Url.Action("Groups",controllerName, new {id = id})
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = model.Campaign.Name,
                                                  //ResourcesUtilities.GetResource("Campaign","SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url =model.AdvertiserAccountId > 0?  Url.Action("create",controllerName, new {AdvertiseraccId = model.AdvertiserAccountId, id=id}):Url.Action("create",controllerName, new {id = id})
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =
                                                      ResourcesUtilities.GetResource(controllerName =="HouseAd" ? "HouseAdList" :"CampaignList",
                                                                                     "SiteMapLocalizations"),
                                                  Url = model.AdvertiserAccountId > 0? Url.Action("Index",controllerName, new { AdvertiseraccId=model.AdvertiserAccountId} ):Url.Action("Index",controllerName),
                                                  Order = 1,
                                              }
                                      };

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
                                           Order = -2,
                                       });
            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.KeywordAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Kewords_Name",
                Name = "Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged",
                CurrentText =
                    model.Keyword != null
                        ? model.Keyword.Name.ToString()
                        : string.Empty
            };



            ViewBag.LanguageAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Languages_Name",
                Name = "Languages.Name",
                ActionName = "GetLanguages",
                ControllerName = "Misc",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "LanguageChanged",
                CurrentText =
                  model.Language != null
                      ? model.Language.Name.ToString()
                      : string.Empty
            };


            var temp_data_msgs = TempData["TempErrorMessages"] as List<string>;
            if (temp_data_msgs == null || temp_data_msgs.Count == 0)
            {
                if (model.Warnings != null)
                {
                    foreach (var warning in model.Warnings)
                    {
                        AddMessages(warning.Message, MessagesType.Warning);
                    }
                }
            }
            else
            {
                foreach (var dataMsg in temp_data_msgs)
                {
                    AddMessages(dataMsg, MessagesType.Error);
                }

            }
            ChangeJavaScriptSet("adCreativeSummaryJs");*/
            return View("Summary/AdDetails");
        }



        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult GetAdDetails(int id, int adGroupId, int adId)
        {

            int campaignId = id;
        
            var model = _campaignService.GetAdFullSummary(new CampaignIdAdgroupIdAdIdMessage { CampaignId = campaignId, AdgroupId = adGroupId, AdId = adId });


            var summaryViewModel = GetFullSummaryViewModel(model);
            summaryViewModel.ViewSummary.isSummary = false;
           // ViewData["IsDownloadAction"] = IsDownloadAction(model.ActionId);


         

      
            var temp_data_msgs = TempData["TempErrorMessages"] as List<string>;
            if (temp_data_msgs == null || temp_data_msgs.Count == 0)
            {
                if (model.Warnings != null)
                {
                    foreach (var warning in model.Warnings)
                    {
                        AddWarnningMsgs(warning.Message);
                    }
                }
            }
            else
            {
                foreach (var dataMsg in temp_data_msgs)
                {
                    AddErrorMsgs(dataMsg);
                }

            }
            //ChangeJavaScriptSet("adCreativeSummaryJs");
            return Json(model, "AdDetails", ResponseStatus.success);
        }
        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AdDetails(int id, int adGroupId, int adId, string[] appSiteId,
                                    string[] deletedAppSiteId, string RunType,
                                    bool Include, string DomainURL, string adsToCopyAppsites, KeywordDto Keyword, string InsertedAppsites)
        {
            //if (!_campaignService.IsReadOrWriteCampaign(id))
            //{
            //    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            //}



            // var ser = new JavaScriptSerializer();


            LanguageDto lang = new LanguageDto();
            if (!string.IsNullOrWhiteSpace(Request.Form["Language.ID"]))
            {
                lang.ID = Convert.ToInt32(Request.Form["Language.ID"]);

            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Reject"]))
            {
                _campaignService.RejectAd(new CampaignIdAdgroupIdAdIdMessage { CampaignId = id, AdgroupId = adGroupId, AdId = adId });
            }
            //if (!string.IsNullOrWhiteSpace(Request.Form["Approve"]))
            else
            {
                if (appSiteId == null)
                    appSiteId = new string[0];
                if (deletedAppSiteId == null)
                    deletedAppSiteId = new string[0];

                var ids = appSiteId.Select(s => Convert.ToInt32(s)).ToArray();
                var deletedIds = deletedAppSiteId.Select(s => Convert.ToInt32(s)).ToArray();

                var adsToCopyAppsitesList = new List<int>();

                if (!string.IsNullOrEmpty(adsToCopyAppsites))
                    adsToCopyAppsitesList = adsToCopyAppsites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();

                var approveAdDto = new ApproveAdDto
                {
                    CampaignId = id,
                    GroupId = adGroupId,
                    AdId = adId,
                    AppSiteIds = ids,
                    DeletedAppSiteIds = deletedIds,
                    RunType = RunType,
                    Include = Include,
                    DomainURL = DomainURL,
                    KeywordId = Keyword.ID == 0 ? null : new int?(Keyword.ID),
                    LanguageId = lang.ID == 0 ? null : new int?(lang.ID),
                    UpdatedCampaignBidConfigDtos = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(InsertedAppsites) ? "[]" : InsertedAppsites, _jsonOptions)
                };

                var model = _campaignService.GetAdFullSummary(new CampaignIdAdgroupIdAdIdMessage { CampaignId = approveAdDto.CampaignId, AdgroupId = approveAdDto.GroupId, AdId = approveAdDto.AdId });


                //if (model.AdvertiserAccountId > 0)
                //{
                //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(model.AdvertiserAccountId))
                //    {
                //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                //    }
                //}

                GetUploadedSnapshots(approveAdDto, model);
                GetCreativesAttributes(approveAdDto, model);
                try
                {
                    approveAdDto.AdsToCopyAppSites = adsToCopyAppsitesList.ToArray();

                    _campaignService.ApproveAd(approveAdDto);
                    AddSuccessfullyMsg();
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            MoveMessagesTempData();
            ChangeJavaScriptSet("adCreativeSummaryJs");
            return RedirectToAction("AdDetails", new { id = id, adGroupId = adGroupId, adId = adId });
        }




        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAdDetails([FromBody]AdDetailsSave adDetails)
        {
            

            LanguageDto lang = new LanguageDto();
            if (!string.IsNullOrWhiteSpace(adDetails.LanguageId))
            {
                lang.ID = Convert.ToInt32(adDetails.LanguageId);

            }
            if (adDetails.Reject)
            {
                _campaignService.RejectAd(new CampaignIdAdgroupIdAdIdMessage { CampaignId = adDetails.id, AdgroupId = adDetails.adGroupId, AdId = adDetails.adId });
                AddSuccessfullyMsg("RejectedSuccessfully", "Ad");
                return Json(ResourcesUtilities.GetResource("Reject", "Commands"), ResponseStatus.success);
            }
            //if (!string.IsNullOrWhiteSpace(Request.Form["Approve"]))
            else
            {
                if (adDetails.appSiteId == null)
                    adDetails.appSiteId = new string[0];
                if (adDetails.deletedAppSiteId == null)
                    adDetails.deletedAppSiteId = new string[0];

                var ids = adDetails.appSiteId.Select(s => Convert.ToInt32(s)).ToArray();
                var deletedIds = adDetails.deletedAppSiteId.Select(s => Convert.ToInt32(s)).ToArray();

                var adsToCopyAppsitesList = new List<int>();

                if (!string.IsNullOrEmpty(adDetails.adsToCopyAppsites))
                    adsToCopyAppsitesList = adDetails.adsToCopyAppsites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();

                var approveAdDto = new ApproveAdDto
                {
                    CampaignId = adDetails.id,
                    GroupId = adDetails.adGroupId,
                    AdId = adDetails.adId,
                    AppSiteIds = ids,
                    DeletedAppSiteIds = deletedIds,
                    RunType = adDetails.RunType,
                    Include = adDetails.Include,
                    //DomainURL = DomainURL,
                    KeywordId = string.IsNullOrWhiteSpace(adDetails.KeywordId) ? null : new int?(Convert.ToInt32(adDetails.KeywordId)),
                    LanguageId = string.IsNullOrWhiteSpace(adDetails.LanguageId) ? null : new int?(Convert.ToInt32(adDetails.LanguageId)),
                   UpdatedCampaignBidConfigDtos = adDetails.UpdatedCampaignBidConfigDtos,
                   Snapshots = adDetails.CreativeUnitsContent.ToList()


                };

                var model = _campaignService.GetAdFullSummary(new CampaignIdAdgroupIdAdIdMessage { CampaignId = approveAdDto.CampaignId, AdgroupId = approveAdDto.GroupId, AdId = approveAdDto.AdId });


                //if (model.AdvertiserAccountId > 0)
                //{
                //    if (!_AdvertiserService.IsReadOrWriteAdvertiserAccount(model.AdvertiserAccountId))
                //    {
                //        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                //    }
                //}

               // GetUploadedSnapshots(approveAdDto, model);
               // GetCreativesAttributes(approveAdDto, model);
                try
                {
                    approveAdDto.AdsToCopyAppSites = adsToCopyAppsitesList.ToArray();

                    _campaignService.ApproveAdForNewUI(approveAdDto);
                   
                }
                catch (BusinessException exception)
                {
                    AddErrorMsgs(exception);
                    return Json("Ad Approval", ResponseStatus.businessException);
                }
            }
            //MoveMessagesTempData();
            // ChangeJavaScriptSet("adCreativeSummaryJs");
            //return RedirectToAction("AdDetails", new { id = id, adGroupId = adGroupId, adId = adId });

            AddSuccessfullyMsg(formattedStrings: "Ad Approval");
            return Json("Ad Approval", ResponseStatus.success);
        }



        public ActionResult GetAttributesSettings()
        {
           var  attributes= adCreativeAttributeService.GetAll();
           
            return Json(attributes);
        }
        public ActionResult AttributesSettingDialog()
        {
            ViewData.Model = adCreativeAttributeService.GetAll();
            ViewData["AttributesDialog"] = true;
            return PartialView("Summary/CreativeSetting/AttributesSettingDialog");
        }

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult AccountCampaigns(int accountId, int adId)
        {
            IEnumerable<AdCreativeDtoBase> adsCreatives = _campaignService.GetUnApprovedAdsFromAdGroupOfAd(new ValueMessageWrapper<int> { Value = adId });

            var campains = new TreeViewModel()
            {
                Url = Url.Action("AccountTreeCampaigns", "Campaign", new { accountId = accountId, adId = adId }),
                Name = "AdsCopyList",
                Id = "AdsCopyList",
                IsAjax = true,
                SelectedValues = adsCreatives.Select(p => new TreeSelectedValue() { Id = p.ID.ToString(), Key = "Ads" }).ToList()
            };

            return PartialView("Summary/AccountCampaigns", campains);

        }

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult AccountTreeCampaigns(int accountId, int adId)
        {
            IEnumerable<TreeDto> adsTree = _campaignService.GetAdsTree(new GetAdsTreeRequest { AccountId = accountId, AdId = adId });
            var adList = TreeModel.GetTreeNodes(adsTree, false);

            JsonResult result = new JsonResult(null);
            result.Value = adList;
            return result;
        }

        #endregion
        #region Cost Element

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult CostElement(int id, int adGroupId, int? adId)
        {
            AdGroupCostElementDto viewModel = new AdGroupCostElementDto();
            if (adId.HasValue)
            {
                viewModel = _campaignService.GetAdGroupCostElement(new GetAdGroupCostElementRequest { CampaignId = id, AdgroupId = adGroupId });
            }

            //load cost elements
            ViewData["CostElements"] = GetCostElementList(LookupNames.CostElement, viewModel == null ? 0 : viewModel.CostElementId);

            var Providers = partyService.QueryByCriteria(new PartyCriteria { Visible = true, Type = PartyType.DP });
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected =viewModel.ProviderId==0
                                              }};
            List<SelectListItem> ProvidersList = Providers.Items.Select(
                item => new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name.ToString(),
                    Selected = viewModel != null && viewModel.ProviderId == item.ID
                }).ToList();
            ProvidersList.Insert(0, lookupsList[0]);
            ViewData["Providers"] = ProvidersList;

            return PartialView("CostElement/CostElement", viewModel);
        }

        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElement([FromBody] AdGroupCostElementSaveDto saveDto)
        {
            //set default times 
            var time = Framework.Utilities.Environment.GetServerTime();
            saveDto.FromDate = new DateTime(saveDto.FromDate.Year, saveDto.FromDate.Month, saveDto.FromDate.Day, time.Hour, time.Minute, time.Second);
            if (saveDto.ToDate != null)
            {
                var toDate = (DateTime)saveDto.ToDate;
                saveDto.ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, time.Hour, time.Minute, time.Second);
            }

            try
            {
                if (!saveDto.ID.HasValue)
                {
                    //add
                    _campaignService.AddCostElements(saveDto);
                }
                else
                {
                    if (saveDto.stop)
                        //stop
                        _campaignService.RemoveCostElements(saveDto);
                    else
                        //edit
                        _campaignService.UpdateCostElements(saveDto);
                }

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                NotificationMessages.Add(new ResultMessage { Message = str, Type = MessagesType.Error });
                return Json(new { ResultSaved = false, MessageTitle = ResourcesUtilities.GetResource("CostElement", "CostElements"), Messages = NotificationMessages, ResponseStatus.businessException });
            }

            NotificationMessages.Add(new ResultMessage { Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements")), Type = MessagesType.Success });

            return Json(new { ResultSaved = true, MessageTitle = ResourcesUtilities.GetResource("CostElement", "CostElements"), Messages = NotificationMessages, ResponseStatus.success });
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult CostElements(int id, int adGroupId)
        {
            int campaignId = id;
            var model = LoadCostElementsData(null, campaignId, adGroupId);
            model.Elements = GetAdQueryResult(null, id, adGroupId);
            return PartialView("CostElement/CostElements", model);
        }

        // [DenyRole(Roles = "AppOps")]
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _CostElements(int id, int adGroupId)
        {
            int campaignId = id;
            var result = GetAdQueryResult(null, campaignId, adGroupId);
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        #endregion


        #region Ad Request
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public JsonResult SaveAdRequest(int typeId, int PlatformId, string Version, int campaignId, int AdGroupId)
        {
            var AdRequestTargeting = new AdRequestTargetingDto
            {
                campaignId = campaignId,
                AdGroupId = AdGroupId,
                AdRequestTypeId = typeId,
                AdRequestPlatformId = PlatformId,
                MinimumVersion = Version
            };
            try
            {
                _campaignService.SaveAdRequestTargeting(AdRequestTargeting);

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                //return Json(new { Success = false, ErrorMessage = str });
                AddErrorMsgs(ex);

                return Json(ResourcesUtilities.GetResource("AdRequest", "Targeting"), ResponseStatus.businessException);
            }
            AddSuccessfullyMsgMs(string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("AdRequest", "Targeting")));

            return Json(ResourcesUtilities.GetResource("AdRequest", "Targeting"), ResponseStatus.success);
            //return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("AdRequest", "Targeting")) });
        }


        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public JsonResult DeleteAdRequest(int id)
        {
            try
            {
                _campaignService.DeleteAdRequestTargeting(new ValueMessageWrapper<int> { Value = id });

            }
            catch (BusinessException ex)
            {
                var str = ex.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                return Json(new { Success = false, ErrorMessage = str });
            }
            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("DeleteSuccessfully", "Global"), ResourcesUtilities.GetResource("AdRequest", "Targeting")) });
        }


        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult GetAdRequests(int adGroupId)
        {
            var model = getAdrequestViewModel(adGroupId);
            return Json(model);
        }

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AdRequests(int adGroupId)
        {
            var model = getAdrequestViewModel(adGroupId);
            return PartialView("AdRequest/AdRequests", model);
        }
        private AdRequestViewModel getAdrequestViewModel(int adGroupId)
        {
            var AllItems = GetAdRequestQueryResult(null, adGroupId);
            var Types_Platforms_Versions = _campaignService.GetAdRequestTypes_Platforms_Versions();
            IList<SelectListItem> Types = new List<SelectListItem>();
            bool SelectedVar = true;
            var typeId = 0;
            foreach (AdRequestTypeDto Type in Types_Platforms_Versions.Types)
            {
                Types.Add(new SelectListItem { Value = Type.ID.ToString(), Text = Type.Name, Selected = SelectedVar });
                if (SelectedVar == true)
                {

                    typeId = Type.ID;
                }
                SelectedVar = false;

            }

            IList<SelectListItem> Platforms = new List<SelectListItem>();

            IList<SelectListItem> Versions = new List<SelectListItem>();
            var filteredByTypes = Types_Platforms_Versions.All.Where(M => M.AdRequestType.ID == typeId).ToList();
            SelectedVar = true;
            var SelectedVar2 = true;
            int PlatformId = 0;
            foreach (var Item in filteredByTypes)
            {
                if (Platforms.Where(M => M.Value == Item.AdRequestPlatform.ID.ToString()).SingleOrDefault() == null & Item.AdRequestType.ID == typeId)
                {
                    Platforms.Add(new SelectListItem { Value = Item.AdRequestPlatform.ID.ToString(), Text = Item.AdRequestPlatform.Name, Selected = SelectedVar });
                    PlatformId = Item.AdRequestPlatform.ID;
                    SelectedVar = false;
                }

                if (PlatformId > 0 & Versions.Where(M => M.Value == Item.Version).SingleOrDefault() == null & Item.AdRequestPlatform.ID == PlatformId)
                {
                    Versions.Add(new SelectListItem { Value = Item.Version, Text = Item.Version, Selected = SelectedVar2 });
                    SelectedVar2 = false;
                }
            }


            return new AdRequestViewModel
            {
                adGroupId = adGroupId,
                AdRequestDialog = new AdRequestDialogViewModel
                {
                    Types = Types,
                    Platforms = Platforms,
                    Versions = Versions
                },
                AllItems = AllItems
            };
        }

        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "typeId", "PlatformId" })]
        public JsonResult getAdrequestViewModelDropDown(int typeId, int PlatformId)
        {

            var Types_Platforms_Versions = _campaignService.GetAdRequestTypes_Platforms_Versions();
            IList<SelectListItem> Types = new List<SelectListItem>();
            bool SelectedVar = true;

            IList<SelectListItem> Versions = new List<SelectListItem>();
            IList<SelectListItem> Platforms = new List<SelectListItem>();
            var filteredByTypes = Types_Platforms_Versions.All.Where(M => M.AdRequestType.ID == typeId).ToList();
            int platformvin = 0;
            if (!(PlatformId > 0))
            {
                SelectedVar = true;

            }
            else
            {
                SelectedVar = false;
                platformvin = PlatformId;
            }

            var SelectedVar2 = true;

            foreach (var Item in filteredByTypes)
            {
                if (Platforms.Where(M => M.Value == Item.AdRequestPlatform.ID.ToString()).SingleOrDefault() == null & Item.AdRequestType.ID == typeId)
                {
                    if (PlatformId > 0 & PlatformId == Item.AdRequestPlatform.ID)
                    {

                        SelectedVar = true;
                    }


                    Platforms.Add(new SelectListItem { Value = Item.AdRequestPlatform.ID.ToString(), Text = Item.AdRequestPlatform.Name, Selected = SelectedVar });
                    if (!(PlatformId > 0) & SelectedVar)
                        platformvin = Item.AdRequestPlatform.ID;

                    SelectedVar = false;
                }
                if (platformvin > 0 && Versions.Where(M => M.Value == Item.Version).SingleOrDefault() == null & Item.AdRequestPlatform.ID == platformvin)
                {
                    Versions.Add(new SelectListItem { Value = Item.Version, Text = Item.Version, Selected = SelectedVar2 });
                    SelectedVar2 = false;
                }

            }
            var results =
                    new

               AdRequestDialogViewModel
                    {

                        Platforms = Platforms,
                        Versions = Versions
                    };

            return Json(results);



        }
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _AdRequest(int adGroupId)
        {
            var result = GetAdRequestQueryResult(null, adGroupId);

            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }



        #endregion



        #region Settings
        #region Helpers
        #endregion

        public ActionResult GetCampaignServerSettings(int id)
        {
            var settings = _campaignService.GetServerSettings(new ValueMessageWrapper<int> { Value = id });
            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id });
            var trackingEventsDto = _trackingEventService.GetCostModelEvents();
            var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();
            List<CampaignFrequencyCappingDto> FrequencyCappingListContainer = new List<CampaignFrequencyCappingDto>();

            for (int i = 0; i < trackingEventsList.Count(); i++)
            {

                CampaignFrequencyCappingDto campaignFrequencyCappingDto = settings.FrequencyCappingList.Where(p => p.EventId == trackingEventsList[i].ID).SingleOrDefault();
                if (campaignFrequencyCappingDto == null)
                {
                    campaignFrequencyCappingDto = new CampaignFrequencyCappingDto();
                    campaignFrequencyCappingDto.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Default;
                    campaignFrequencyCappingDto.EventDescription = trackingEventsList[i].EventDescription;
                    campaignFrequencyCappingDto.EventId = trackingEventsList[i].ID;
                    campaignFrequencyCappingDto.EventName = trackingEventsList[i].EventName;
                    campaignFrequencyCappingDto.IsCapping = false;
                    campaignFrequencyCappingDto.NumberName = "";
                    if (trackingEventsList[i].DefaultFrequencyCapping != null)
                    {
                        if (3600 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 3600 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                            if (86400 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 86400 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                if (604800 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 604800 / (int)trackingEventsList[i].DefaultFrequencyCapping;

                        else

                                    if (2592000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 2592000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                    if (7776000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 7776000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                    }
                    else
                    {
                        campaignFrequencyCappingDto.Number = 0;
                    }
                    campaignFrequencyCappingDto.IntervalName = "";
                    campaignFrequencyCappingDto.Interval = trackingEventsList[i].DefaultFrequencyCapping != null ? (int)trackingEventsList[i].DefaultFrequencyCapping : 0;
                    campaignFrequencyCappingDto.Type = 0;
                    campaignFrequencyCappingDto.TypeName = "";

                }
                else
                {

                    campaignFrequencyCappingDto.IsCapping = true;
                    campaignFrequencyCappingDto.CampignFrequencyCappingStatus = campaignFrequencyCappingDto.Interval != 0 ? CampignFrequencyCappingEnum.Capping : CampignFrequencyCappingEnum.NoCapping;

                }

                if (campaignFrequencyCappingDto.Interval > 0)
                {
                    if (3600 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Hour", "Global");
                    else

                        if (86400 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Day", "Global");
                    else

                            if (604800 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Week", "Global");
                    else
                                if (2592000 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Month", "Global");

                    else

                             if (7776000 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("CampLifeTime", "Global");
                }
                campaignFrequencyCappingDto.NumberName = campaignFrequencyCappingDto.Number.ToString();
                campaignFrequencyCappingDto.TypeName = campaignFrequencyCappingDto.Type != 1 ? ResourcesUtilities.GetResource("FastMode", "CampaignSettings") : ResourcesUtilities.GetResource("Evenly", "CampaignSettings");

                if (campaignFrequencyCappingDto.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.Capping)
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("Capping", "CampaignServerSetting");
                else
                    if (campaignFrequencyCappingDto.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.NoCapping)
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("NoCapping", "CampaignServerSetting");
                else
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("Default", "Campaign");

                FrequencyCappingListContainer.Add(campaignFrequencyCappingDto);
            }
            var agencyCommission = new List<object>();

            agencyCommission.Add(new { label = ResourcesUtilities.GetResource("Select", "Global"), value = 0 });
            agencyCommission.Add(new { label = ResourcesUtilities.GetResource("FixedCPM", "Global"), value = 1 });
            agencyCommission.Add(new { label = ResourcesUtilities.GetResource("NetCostMargin", "Global"), value = 2 });
            agencyCommission.Add(new { label = ResourcesUtilities.GetResource("BillableCostMargin", "Global"), value = 3 });
            agencyCommission.Add(new { lable = ResourcesUtilities.GetResource("GrossCostMargin", "Global"), value = 4 });
            settings.DefultFrequencyCappingList = FrequencyCappingListContainer;
            return Json(new { model = settings, accountAdvertiserId = advertiserAccountId?.Value, agencyCommissionTypes = agencyCommission, HasObjective = settings.HasObjective });
        }

        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]
        [HttpPost]
        public ActionResult SaveCampaignServerSettings([FromBody] CampaignServerSettingDto settingDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FixCampaignServerSettingDto(settingDto);
                    _campaignService.SaveServerSetting(settingDto);
                    foreach (var fcapping in settingDto.FrequencyCappingList)
                    {
                        SaveFrequencyCapping(new FreqCappingSaveModel { id = settingDto.ID, 
                            frequencyCappingSave= new CampaignFrequencyCappingSaveDto { 
                                CampignFrequencyCappingStatus = (int)fcapping.CampignFrequencyCappingStatus,
                                EventId = fcapping.EventId,
                                Id = fcapping.ID,
                                Interval = fcapping.Interval,
                                Number = fcapping.Number,
                                Type = fcapping.Type
                            } 
                        }
                        );
                    }
                    AddSuccessfullyMsg(formattedStrings: "Campaign Server Settings");
                    return Json("Campaign Server Settings", ResponseStatus.success);
                }
                else
                {
                    return Json("Campaign Server Settings", ResponseStatus.businessException);
                }
            }
            catch (BusinessException exception)
            {
                AddErrorMsgs(exception);
                return Json("Campaign Server Settings", ResponseStatus.businessException);
            }

        }
        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        public ActionResult ServerSetting(int id, bool isReact = false)
        {
            return View("Create/AdvancedSettings");
            var settings = _campaignService.GetServerSettings(new ValueMessageWrapper<int> { Value = id });
            var advertiserId = _campaignService.GetCampaignAdvertiser(new ValueMessageWrapper<int> { Value = id });
            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id });
            var AdvertiserName = _campaignService.GetAdvertiserString(advertiserId);

            var AdvertiserAccountName = _campaignService.GetAdvertiserAccountString(advertiserAccountId);
            var trackingEventsDto = _trackingEventService.GetCostModelEvents();
            var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();


            List<CampaignFrequencyCappingDto> FrequencyCappingListContainer = new List<CampaignFrequencyCappingDto>();

            for (int i = 0; i < trackingEventsList.Count(); i++)
            {

                CampaignFrequencyCappingDto campaignFrequencyCappingDto = settings.FrequencyCappingList.Where(p => p.EventId == trackingEventsList[i].ID).SingleOrDefault();


                if (campaignFrequencyCappingDto == null)
                {

                    campaignFrequencyCappingDto = new CampaignFrequencyCappingDto();
                    campaignFrequencyCappingDto.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Default;
                    campaignFrequencyCappingDto.EventDescription = trackingEventsList[i].EventDescription;
                    campaignFrequencyCappingDto.EventId = trackingEventsList[i].ID;
                    campaignFrequencyCappingDto.EventName = trackingEventsList[i].EventName;
                    //   container.ID = settings.ID;
                    campaignFrequencyCappingDto.IsCapping = false;
                    //campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("IsNotCapping", "Campaign");
                    campaignFrequencyCappingDto.NumberName = "";

                    if (trackingEventsList[i].DefaultFrequencyCapping != null)
                    {
                        if (3600 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 3600 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                            if (86400 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 86400 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                if (604800 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 604800 / (int)trackingEventsList[i].DefaultFrequencyCapping;

                        else

                                    if (2592000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 2592000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                    if (7776000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            campaignFrequencyCappingDto.Number = 7776000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                    }
                    else
                    {
                        campaignFrequencyCappingDto.Number = 0;
                    }
                    campaignFrequencyCappingDto.IntervalName = "";
                    campaignFrequencyCappingDto.Interval = trackingEventsList[i].DefaultFrequencyCapping != null ? (int)trackingEventsList[i].DefaultFrequencyCapping : 0;
                    campaignFrequencyCappingDto.Type = 0;
                    campaignFrequencyCappingDto.TypeName = "";

                }
                else
                {

                    //   campaignFrequencyCappingDto.ID=
                    campaignFrequencyCappingDto.IsCapping = true;
                    campaignFrequencyCappingDto.CampignFrequencyCappingStatus = campaignFrequencyCappingDto.Interval != 0 ? CampignFrequencyCappingEnum.Capping : CampignFrequencyCappingEnum.NoCapping;

                    //if (campaignFrequencyCappingDto.Interval>(int) FrequencyCappingInterval.Month)
                    //{
                    //    campaignFrequencyCappingDto.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.CappingLifeTime;
                    //}

                }

                if (campaignFrequencyCappingDto.Interval > 0)
                {
                    if (3600 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Hour", "Global");
                    else

                        if (86400 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Day", "Global");
                    else

                            if (604800 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Week", "Global");

                    else

                                if (2592000 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("Month", "Global");

                    else

                             if (7776000 >= campaignFrequencyCappingDto.Interval)
                        campaignFrequencyCappingDto.IntervalName = ResourcesUtilities.GetResource("LifeTime", "Global");




                }



                campaignFrequencyCappingDto.NumberName = campaignFrequencyCappingDto.Number.ToString();


                campaignFrequencyCappingDto.TypeName = campaignFrequencyCappingDto.Type != 1 ? ResourcesUtilities.GetResource("FastMode", "CampaignSettings") : ResourcesUtilities.GetResource("Evenly", "CampaignSettings");



                if (campaignFrequencyCappingDto.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.Capping)
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("Capping", "CampaignServerSetting");
                //else
                //    if (campaignFrequencyCappingDto.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.CappingLifeTime)
                //    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("CappingLifeTime", "Global");
                else
                    if (campaignFrequencyCappingDto.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.NoCapping)
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("NoCapping", "CampaignServerSetting");
                else
                    campaignFrequencyCappingDto.IsCappingValue = ResourcesUtilities.GetResource("Default", "Campaign");



                FrequencyCappingListContainer.Add(campaignFrequencyCappingDto);


            }

            settings.DefultFrequencyCappingList = FrequencyCappingListContainer;

            ViewData["CampaignId"] = id;

            ViewData["AdvertiserIdForTab"] = advertiserId;

            ViewData["AdvertiserAccountIdForTab"] = advertiserAccountId;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteSettings","SiteMapLocalizations"),
                                                  Order = 5
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =settings.Name,
                                                  Url = Url.Action("Create", "Campaign", new {AdvertiseraccId=advertiserAccountId,id = id}),
                                                  Order = 4
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignList", "SiteMapLocalizations"),
                                                  Order = 3,
                                                 Url = Url.Action("Index", new { AdvertiseraccId = advertiserAccountId })
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

            if (isReact)
                return View("Create/AdvancedSettings");
            return View(settings);
        }
        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult ServerSetting(int id, CampaignServerSettingDto settingDto, string returnUrl)
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
                                                  Text =settingDto.Name,
                                                  Url = Url.Action("Create", "Campaign", new {id = id,returnUrl=returnUrl}),
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
            if (ModelState.IsValid)
            {
                try
                {
                    FixCampaignServerSettingDto(settingDto);
                    _campaignService.SaveServerSetting(settingDto);
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("ServerSetting", new { id = id, returnUrl = returnUrl });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            // ViewData["CreateAdvertiserId"] = AdvertiserId;
            ViewData["CampaignId"] = id;
            var settings = _campaignService.GetServerSettings(new ValueMessageWrapper<int> { Value = id });
            settings.FrequencyCappingList = new List<CampaignFrequencyCappingDto>();
            return View();
        }

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [HttpGet]
        public ActionResult GetCampaignAssignedAppSites(int id)
        {

            var statuses = _appSiteStatusService.GetAll();
            var types = _appSiteTypeService.GetAll();
            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id });
            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();
            var campaignAssignAppsite = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = id });
            return Json(new { assignedAppSites = campaignAssignAppsite, appsitesStatuses = statuses, appsitesTypes = types });
        }

        [HttpPost]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCampaignAssignedAppsites([FromBody] CampaignAssignAppsitesModel campaignAssignAppsitesModel)
        {
            List<ResultMessage> rMessages = new List<ResultMessage>();
            try
            {
                CampaignAssignedAppsitesSaveDTo campaignAssignedAppsitesSaveDTo = new CampaignAssignedAppsitesSaveDTo();
                campaignAssignedAppsitesSaveDTo.CampaignId = campaignAssignAppsitesModel.CampaignId;
                campaignAssignedAppsitesSaveDTo.InsertedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignAssignedAppsitesDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.InsertedAssignedAppsites) ? "[]" : campaignAssignAppsitesModel.InsertedAssignedAppsites, _jsonOptions);
                campaignAssignedAppsitesSaveDTo.UpdatedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignAssignedAppsitesDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.UpdatedAssignedAppsites) ? "[]" : campaignAssignAppsitesModel.UpdatedAssignedAppsites, _jsonOptions);
                if (!string.IsNullOrEmpty(campaignAssignAppsitesModel.DeletedAssignedAppsites))
                {
                    var deletedAssignedAppsitesAr = campaignAssignAppsitesModel.DeletedAssignedAppsites.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    campaignAssignedAppsitesSaveDTo.DeletedAssignedAppsites = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
                }
                campaignAssignedAppsitesSaveDTo.NotCompatibleCampaignBidConfigs = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.NotCompatibleCampaignBidConfigs) ? "[]" : campaignAssignAppsitesModel.NotCompatibleCampaignBidConfigs, _jsonOptions);

                _campaignService.SaveCampaignAssignAppsites(campaignAssignedAppsitesSaveDTo);
                AddSuccessfullyMsg(rMessages);
            }
            catch (BusinessException ex)
            {
                AddErrorMsgs(rMessages, ex);
            }

            return Json(new { Messages = rMessages });

        }
        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [HttpGet]
        public ActionResult CampaignAssignAppsites(int id)
        {
            return View();
            var statues = _appSiteStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();
            var advertiserId = _campaignService.GetCampaignAdvertiser(new ValueMessageWrapper<int> { Value = id });
            var AdvertiserName = _campaignService.GetAdvertiserString(advertiserId);

            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id });
            var AdvertiserAccountName = _campaignService.GetAdvertiserAccountString(advertiserAccountId);
            //   var campaign= _campaignService.Get(id)
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            //load all types
            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();
            var campaignAssignAppsite = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = id });
            var model = new CampaignAssignAppsitesModel
            {
                CampaignId = id,
                AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() },
                Statuses = statuesDropDown,
                Types = typesDropDown,
                DateTo = serverDateTime,
                DateFrom = serverDateTime.AddDays(-30),
                SubPublishers = new List<SelectListItem>(),
                AssignedAppsitesList = campaignAssignAppsite.CampaignAssignedAppsitesList
            };
            ViewData["AdvertiserIdForTab"] = advertiserId;
            ViewData["AdvertiserAccountIdForTab"] = advertiserAccountId;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignAssignAppsites","SiteMapLocalizations"),
                                                  Order = 5
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =campaignAssignAppsite.CampignName,
                                                  Url = Url.Action("Create", "Campaign", new {AdvertiseraccId=advertiserAccountId,id = id}),
                                                  Order =4
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CampaignList", "SiteMapLocalizations"),
                                                  Order = 3,
                                                 Url = Url.Action("Index", new { AdvertiseraccId = advertiserAccountId })
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

            //     model.AppSites.Items = model.AppSites.Items.ToList();
            return View(model);
        }

        //[DenyRole(Roles = "AppOps")]
        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult CampaignAssignAppsites([FromBody] CampaignAssignAppsitesModel campaignAssignAppsitesModel)
        {
            //var ser = new JavaScriptSerializer();
            CampaignAssignedAppsitesSaveDTo campaignAssignedAppsitesSaveDTo = new CampaignAssignedAppsitesSaveDTo();
            campaignAssignedAppsitesSaveDTo.CampaignId = campaignAssignAppsitesModel.CampaignId;
            campaignAssignedAppsitesSaveDTo.InsertedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignAssignedAppsitesDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.InsertedAssignedAppsites) ? "[]" : campaignAssignAppsitesModel.InsertedAssignedAppsites, _jsonOptions);
            campaignAssignedAppsitesSaveDTo.UpdatedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignAssignedAppsitesDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.UpdatedAssignedAppsites) ? "[]" : campaignAssignAppsitesModel.UpdatedAssignedAppsites, _jsonOptions);
            if (!string.IsNullOrEmpty(campaignAssignAppsitesModel.DeletedAssignedAppsites))
            {
                var deletedAssignedAppsitesAr = campaignAssignAppsitesModel.DeletedAssignedAppsites.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                campaignAssignedAppsitesSaveDTo.DeletedAssignedAppsites = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
            }
            campaignAssignedAppsitesSaveDTo.NotCompatibleCampaignBidConfigs = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(campaignAssignAppsitesModel.NotCompatibleCampaignBidConfigs) ? "[]" : campaignAssignAppsitesModel.NotCompatibleCampaignBidConfigs, _jsonOptions);

            _campaignService.SaveCampaignAssignAppsites(campaignAssignedAppsitesSaveDTo);
            AddSuccessfullyMsg(formattedStrings: "Campaign Settings");
            return RedirectToAction("CampaignAssignAppsites", new { Id = campaignAssignAppsitesModel.CampaignId });
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [HttpGet]
        public ActionResult GetCampaignAssignAppsitesData(int id)
        {
            var statues = _appSiteStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();
            var advertiserId = _campaignService.GetCampaignAdvertiser(new ValueMessageWrapper<int> { Value = id });
            var AdvertiserName = _campaignService.GetAdvertiserString(advertiserId);

            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id });
            var AdvertiserAccountName = _campaignService.GetAdvertiserAccountString(advertiserAccountId);
            //   var campaign= _campaignService.Get(id)
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            //load all types
            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            DateTime serverDateTime = Framework.Utilities.Environment.GetServerTime();
            var campaignAssignAppsite = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = id });
            var model = new CampaignAssignAppsitesModel
            {
                CampaignId = id,
                AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() },
                Statuses = statuesDropDown,
                Types = typesDropDown,
                DateTo = serverDateTime,
                DateFrom = serverDateTime.AddDays(-30),
                SubPublishers = new List<SelectListItem>(),
                AssignedAppsitesList = campaignAssignAppsite.CampaignAssignedAppsitesList
            };
            ViewData["AdvertiserIdForTab"] = advertiserId;
            ViewData["AdvertiserAccountIdForTab"] = advertiserAccountId;
            
            //     model.AppSites.Items = model.AppSites.Items.ToList();
            return Json(model);
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,AppOps")]
        public JsonResult SearchSubAppsites([FromBody]AppSiteSearchModel appSiteSearchModel)
        {
            JsonResult josnResult;
            try
            {

                var appSiteSearchCriteria = new AllAppSiteCriteria()
                {
                    AppSiteId = appSiteSearchModel.AppSiteId,
                    IgnoreIsPrimaryUser = appSiteSearchModel.IgnoreIsPrimaryUser,
                    AccountId = appSiteSearchModel.AccountId,
                    AccountName = appSiteSearchModel.Name,
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
                //var result = _userService.GetPublisherUsers(appSiteSearchCriteria);
                var result = _appSiteService.QueryByCratiriaForSubAppsites(appSiteSearchCriteria);
                //var result = "";
                josnResult = new JsonResult(result);
                return josnResult;
            }
            catch (BusinessException exception)
            {
                josnResult = new JsonResult(null);
                foreach (var errorData in exception.Errors)
                {
                    josnResult.Value += errorData.Message;
                }
                return josnResult;
            }
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,AppOps")]
        public JsonResult SearchAppSites([FromBody]AppSiteSearchModel appSiteSearchModel)
        {
            JsonResult josnResult;
            try
            {

                var appSiteSearchCriteria = new AllAppSiteCriteria()
                {
                    AccountId = appSiteSearchModel.AccountId,
                    IgnoreIsPrimaryUser = appSiteSearchModel.IgnoreIsPrimaryUser,
                    AccountName = appSiteSearchModel.Name,
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
                //var result = _userService.GetPublisherUsers(appSiteSearchCriteria);
                var result = _appSiteService.GetAllActive(appSiteSearchCriteria);
                //var result = "";
                josnResult = new JsonResult(result);
                return josnResult;
            }
            catch (BusinessException exception)
            {
                josnResult = new JsonResult(null);
                foreach (var errorData in exception.Errors)
                {
                    josnResult.Value += errorData.Message;
                }
                return josnResult;
            }
        }


        [GridAction(EnableCustomBinding = true)]
        public ActionResult DummyAssignAppsitesSelect()
        {
            var result = new List<CampaignAssignAppsitesModel>();
            return View(new GridModel
            {
                Data = result,
                Total = 0
            });
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult GetCampaignAssignAppsites(int id)
        {
            var result = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = id });

            return Json(new GridModel
            {
                Data = result.CampaignAssignedAppsitesList,
                Total = result.CampaignAssignedAppsitesList.Count
            });
        }


        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult CampaignBidConfigPartial(int id, int adGroupId)
        {

            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            var model = new ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.CampaignBidConfigModel
            {
                AdGroupId = adGroupId,
                CampaignId = id,
                Types = typesDropDown,
                AppSites = new AppSiteListResultDto { Items = new List<AppSiteListDto>() },
                SubPublishers = new List<SelectListItem>(),
                Accounts = new List<AccountViewModel>(),
                CampaignBidConfigList = new List<CampaignBidConfigDto>()
            };

            return PartialView("Targeting/CampaignBidConfig", model);
        }

        public ActionResult CampaignBidConfigData(int id, int adGroupId)
        {
            var campaignBidConfigDto = _campaignService.GetCampaignBidConfigs(new CampaignIdAdgroupIdMessage { CampaignId = id, AdgroupId = adGroupId });
            var campaignAssignedAppsitesList = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = id }).CampaignAssignedAppsitesList;
            foreach (var bidConfig in campaignBidConfigDto.CampaignBidConfigDtos)
            {
                if (campaignAssignedAppsitesList.Where(x => (x.Appsite.ID == bidConfig.Appsite.ID) && (x.SubPublisherId == bidConfig.SubPublisherId)).Count() > 0)
                {
                    bidConfig.HideDeleteButton = true;

                }
            }

            return Json(new { CampaignBidConfigList = campaignBidConfigDto.CampaignBidConfigDtos.ToList() });
        }



        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult CampaignBidConfig(CampaignBidConfigModel campaignBidConfigModel)
        {
            //  var ser = new JavaScriptSerializer();
            CampaignBidConfigSaveDTo campaignBidConfigSaveDTo = new CampaignBidConfigSaveDTo();
            campaignBidConfigSaveDTo.CampaignId = campaignBidConfigModel.CampaignId;
            campaignBidConfigSaveDTo.InsertedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(campaignBidConfigModel.InserteCampaignBidConfigs) ? "[]" : campaignBidConfigModel.InserteCampaignBidConfigs, _jsonOptions);
            campaignBidConfigSaveDTo.UpdatedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(campaignBidConfigModel.UpdatedCampaignBidConfiges) ? "[]" : campaignBidConfigModel.UpdatedCampaignBidConfiges, _jsonOptions);

            if (!string.IsNullOrEmpty(campaignBidConfigModel.DeletedCampaignBidConfigs))
            {
                var deletedAssignedAppsitesAr = campaignBidConfigModel.DeletedCampaignBidConfigs.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                campaignBidConfigSaveDTo.DeletedCampaignBidConfigs = deletedAssignedAppsitesAr.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
            }
            _campaignService.SaveCampaignBidConfig(campaignBidConfigSaveDTo);
            return RedirectToAction("CampaignBidConfig", new { Id = campaignBidConfigModel.CampaignId });
        }

        // this action to render CampaignBidConfigDialog when a cost model or price model has benn changed 
        public ActionResult CampaignBidConfigDialogView(int? id, int adGroupId)
        {
            //  var campaignBidConfigDto = _campaignService.GetCampaignBidConfigs(id, adGroupId);

            var model = new ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.CampaignBidConfigModel
            {
                CampaignId = (int)id,
                AdGroupId = adGroupId,
                CampaignBidConfigList = new List<CampaignBidConfigDto>()
            };

            return PartialView("CampaignBidConfigDialog", model);
        }

        // Validation CampaignBidConfig
        public ActionResult CheckAppSiteCompatibleWithCampaign(int campaignId, int adGroupId, string InsertedItems, string UpdateItems, string DeletedItems, int PricingModel, int OldPriceModel)
        {
            //var ser = new JavaScriptSerializer();
            CampaignBidConfigSaveDTo campaignBidConfigSaveDTo = new CampaignBidConfigSaveDTo();
            IList<CampaignBidConfigDto> insertedItemsDto = null;
            var adgroupTargeting = _campaignService.GetTargeting(new GetTargetingRequest { CampaignId = campaignId, AdgroupId = adGroupId });
            var costModelchanged = adgroupTargeting.CostModelWrapper != PricingModel ? true : false;
            string[] deletedAssignedAppsites;
            int[] deletedIds = null;
            if (costModelchanged)
            {
                bool isValid = true;
                var groupBidConfigDtos = _campaignService.GetCampaignBidConfigs(new CampaignIdAdgroupIdMessage { CampaignId = campaignId, AdgroupId = adGroupId }).CampaignBidConfigDtos;

                if (!string.IsNullOrEmpty(InsertedItems) && InsertedItems.Length > 0)
                {
                    insertedItemsDto = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(InsertedItems) ? "[]" : InsertedItems, _jsonOptions);
                    groupBidConfigDtos = insertedItemsDto.Union(groupBidConfigDtos).ToList();
                }
                if (!string.IsNullOrEmpty(DeletedItems) && DeletedItems.Length > 0)
                {// remove deleted item fome the list
                    deletedAssignedAppsites = DeletedItems.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    deletedIds = deletedAssignedAppsites.Select(x => Convert.ToInt32(x)).Distinct().ToArray();
                }

                IList<CampaignBidConfigDto> notCompatableAppSites = null;
                CheckAppsitCostModelCompatableWitCampaignsResponse response = null;
                if (insertedItemsDto != null)
                    response = _campaignService.CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = campaignId, Appsites = insertedItemsDto.Select(x => x.Appsite.ID).ToList(), AdGroupId = adGroupId, GroupCostModelWrapperID = PricingModel, CheckExisting = true });
                else
                    response = _campaignService.CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = campaignId, Appsites = new List<int>(), AdGroupId = adGroupId, GroupCostModelWrapperID = PricingModel, CheckExisting = true });

                //  notCompatableAppSites = notCompatableAppSites.ToList();
                //  notCompatableAppSites = notCompatableAppSites.Where(x => !deletedIds.Contains(Convert.ToInt32(x.ID))).ToList();
                bool isExistingValid = response.Success;
                notCompatableAppSites = response.NotCompatableCampaigns;
                //if (notCompatableAppSites.Count <= 0)
                //{
                //    return Json(new { status = true, List = notCompatableAppSites });
                //}

                if (!string.IsNullOrEmpty(UpdateItems) && UpdateItems.Length > 0)
                {
                    IList<CampaignBidConfigDto> updateItemsDto = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(UpdateItems) ? "[]" : UpdateItems, _jsonOptions);
                    var updatedIDs = updateItemsDto.Select(x => Convert.ToInt32(x.ID)).Distinct().ToArray();
                    groupBidConfigDtos = groupBidConfigDtos.Where(x => !updatedIDs.Contains(Convert.ToInt32(x.ID))).ToList();
                    groupBidConfigDtos = groupBidConfigDtos.Union(updateItemsDto).ToList();
                }

                var campaignAssignedAppsitesList = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = campaignId }).CampaignAssignedAppsitesList;

                foreach (var notCompatable in notCompatableAppSites)
                {
                    if (groupBidConfigDtos.Where(x => (x.Appsite.ID == notCompatable.Appsite.ID) && (x.SubPublisherId == notCompatable.SubPublisherId)).Count() <= 0)
                    {
                        groupBidConfigDtos.Add(notCompatable);
                    }
                }

                foreach (var notCompatable in groupBidConfigDtos)
                {
                    if (campaignAssignedAppsitesList.Where(x => (x.Appsite.ID == notCompatable.Appsite.ID) && (x.SubPublisherId == notCompatable.SubPublisherId)).Count() > 0)
                    {
                        notCompatable.HideDeleteButton = true;
                    }
                }

                if (!string.IsNullOrEmpty(DeletedItems) && DeletedItems.Length > 0)
                {// remove deleted item fome the list
                    groupBidConfigDtos = groupBidConfigDtos.Where(x => !deletedIds.Contains(Convert.ToInt32(x.ID))).ToList();
                }
                if (groupBidConfigDtos.Count() > 0)
                {
                    isValid = false;
                }
                return Json(new { status = isValid, List = groupBidConfigDtos });
            }
            else return Json(new { status = true });

        }
        public ActionResult CheckAssignedAppSiteCompatibleWithCampaign(int CampaignId, string InsertedItems, string UpdateItems)
        {
            //var ser = new JavaScriptSerializer();

            CampaignBidConfigSaveDTo campaignBidConfigSaveDTo = new CampaignBidConfigSaveDTo();
            campaignBidConfigSaveDTo.InsertedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(InsertedItems) ? "[]" : InsertedItems, _jsonOptions);
            campaignBidConfigSaveDTo.UpdatedItems = System.Text.Json.JsonSerializer.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(UpdateItems) ? "[]" : UpdateItems, _jsonOptions);
            List<int> insertAppsitesIds = campaignBidConfigSaveDTo.InsertedItems.Select(x => x.Appsite.ID).ToList();
            IList<CampaignBidConfigDto> notCompatableAppSites = null;


            var result = _campaignService.CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = CampaignId, Appsites = insertAppsitesIds });
            bool isValid = result.Success;
            notCompatableAppSites = result.NotCompatableCampaigns;
            return Json(new { status = isValid, List = notCompatableAppSites });

        }

        public ActionResult CheckAppSiteCompatibleWithAdd(int CampaignId, int adGroupId, int adId, string AppsitesIds)
        {
            // var ser = new JavaScriptSerializer();
            CampaignBidConfigSaveDTo campaignBidConfigSaveDTo = new CampaignBidConfigSaveDTo();
            List<int> insertAppsitesIds = new List<int>();
            if (!string.IsNullOrEmpty(AppsitesIds))
                insertAppsitesIds = AppsitesIds.Split(',').Select(x => Convert.ToInt32(x)).ToList();


            List<int> appSiteAdQueuesIds = _campaignService.GetAppSiteAdQueues(new CampaignIdAdgroupIdAdIdMessage { CampaignId = CampaignId, AdgroupId = adGroupId, AdId = adId });
            List<int> newAppsiteIds = insertAppsitesIds.Except(appSiteAdQueuesIds).ToList<int>();

            IList<CampaignBidConfigDto> notCompatableAppSites = null;


            var result = _campaignService.CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = CampaignId, Appsites = newAppsiteIds, AdGroupId = adGroupId });
            bool isValid = result.Success;
            notCompatableAppSites = result.NotCompatableCampaigns;
            return Json(new { status = isValid, List = notCompatableAppSites });
        }

        public ActionResult CheckAppSiteCompatibleWithAddForTest(int CampaignId, int adGroupId, int adId, string AppsitesIds)
        {
            // var ser = new JavaScriptSerializer();
           bool isValid = false;
            var notCompatableAppSites = new  List<object>();
            for (int i = 0; i < 14; i++)
            {
                 notCompatableAppSites.Add( new
                {
                    CampaingName = "CampaingName_test_" + i,
                    AdGrouptName = "AdGrouptName_test_"+i,
                    AdGroupPricingModel = "AdGroupPricingModel_test_"+i,
                    AppsiteName = "AppsiteName_test_"+i,
                    AppsitePricingModelString = "i",
                    MinBid = "15",
                    Bid = i * 3 + "",
                    ID = CampaignId,
                    AdGroupId = adGroupId,
                    Appsite = "Appsite_test_"+i,
                    AccountId = adId,
                    AppsitePricingModelId = AppsitesIds,
                });

            }
            return Json(new { status = isValid, List = notCompatableAppSites });
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public JsonResult AccountSearch(AppSiteSearchModel appSiteSearchModel)
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
                var result = _userService.GetPublisherUsers(appSiteSearchCriteria);
                // var result = _appSiteService.GetAllActive(appSiteSearchCriteria);

                josnResult = new JsonResult(result);

                return josnResult;
            }
            catch (BusinessException exception)
            {
                josnResult = new JsonResult(null);
                foreach (var errorData in exception.Errors)
                {
                    josnResult.Value += errorData.Message;
                }
                return josnResult;
            }


        }

        // [DenyRole(Roles = "AppOps")]


        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _FrequencyCappping(int id)
        {
            var settings = _campaignService.GetServerSettings(new ValueMessageWrapper<int> { Value = id });

            foreach (var item in settings.FrequencyCappingList)
            {
                if (item.Interval == 0 && item.Type == 0 && item.Number == 1)
                {
                    item.IsCapping = false;
                }
                else
                {
                    item.IsCapping = true;
                    item.NumberName = item.Number.ToString();
                    item.IntervalName = GetIntervalName(item.Interval);
                    item.TypeName = item.Type == 1 ? ResourcesUtilities.GetResource("Evenly", "CampaignSettings") : ResourcesUtilities.GetResource("FastMode", "CampaignSettings");
                }
            }
            //return View(new GridModel
            //{
            //    Data = settings.FrequencyCappingList,
            //    Total = settings.FrequencyCappingList.Count()
            //});

            var trackingEventsDto = _trackingEventService.GetCostModelEvents();
            var trackingEventsList = trackingEventsDto.Select(p => new CampaignFrequencyCappingDto() { EventDescription = p.EventDescription, EventId = p.ID });
            //ViewData["CreateAdvertiserId"] = AdvertiserId;
            return View(new GridModel
            {
                Data = trackingEventsList,
                Total = trackingEventsList.Count()
            });

        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult DeleteFrequencyCapping(int id, int frequencyCappingId)
        {
            string deleteMessage = string.Format(ResourcesUtilities.GetResource("DeleteSuccessfully", "Global"), ResourcesUtilities.GetResource("Event", "Global"));
            try
            {

                _campaignService.DeleteFrequencyCapping(new DeleteFrequencyCappingRequest { CampaignId = id, FrequencyCappingId = frequencyCappingId });
            }
            catch (BusinessException x)
            {
                foreach (var item in x.Errors)
                {
                    deleteMessage = item.Message;
                }
                Response.StatusCode = 0;
            }

            return Content(deleteMessage);
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFrequencyCapping([FromBody] FreqCappingSaveModel saveModel)
        {

            int id = saveModel.id;
            CampaignFrequencyCappingSaveDto frequencyCappingSave = saveModel.frequencyCappingSave;
            string responseMessage = ResourcesUtilities.GetResource("SuccessAddFrequencyCapping", "CampaignServerSetting");
            try
            {
                switch (frequencyCappingSave.CampignFrequencyCappingStatus)
                {
                    case (int)CampignFrequencyCappingEnum.Capping:
                        {
                            _campaignService.SaveFrequencyCapping(new SaveFrequencyCappingRequest { CampaignId = id, FrequencyCapping = frequencyCappingSave });
                            break;
                        }
                    //case (int)CampignFrequencyCappingEnum.CappingLifeTime:
                    //    {
                    //        _campaignService.SaveFrequencyCapping(id, frequencyCappingSave);
                    //        break;
                    //    }
                    case (int)CampignFrequencyCappingEnum.NoCapping:
                        {
                            frequencyCappingSave.Interval = 0;
                            frequencyCappingSave.Type = 0;
                            frequencyCappingSave.Number = 1;
                            _campaignService.SaveFrequencyCapping(new SaveFrequencyCappingRequest { CampaignId = id, FrequencyCapping = frequencyCappingSave });

                            break;
                        }
                    case (int)CampignFrequencyCappingEnum.Default:
                        {
                            DeleteFrequencyCapping(id, frequencyCappingSave.Id);
                            break;
                        }
                }


            }
            catch (BusinessException x)
            {
                Response.StatusCode = 0;

                foreach (var item in x.Errors)
                {
                    responseMessage = item.Message;
                }
            }

            return Content(responseMessage);
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult FrequencyCapping()
        {
            var trackingEventsDto = _trackingEventService.GetCostModelEvents();
            var trackingEventsList = trackingEventsDto.Select(p => new SelectListItem() { Text = p.EventDescription, Value = p.ID.ToString() });

            // ViewData["TrackingEventsList"] = trackingEventsList;
            return View("Frequency Capping/FrequencyCapping");
        }

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult GetCampaignSettings(int? id)
        {
            if (!id.HasValue)
            {
                throw new Exceptions.Core.DataNotFoundException();
            }

            var campaignSettingsDto = _campaignService.GetSettings(new ValueMessageWrapper<int> { Value = id.Value });
            campaignSettingsDto.TrackConversionsUrlHttp = string.Format(Config.TrackConversionsHttpUrl, campaignSettingsDto.UniqueId);
            campaignSettingsDto.TrackConversionsUrlHttps = string.Format(Config.TrackConversionsHttpsUrl, campaignSettingsDto.UniqueId);
            var costModelWrappers = GetCostModelWrappers(campaignSettingsDto.ValidCostModelWrapper);
            var advertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id.Value });
            return Json(new { model = campaignSettingsDto, accountAdvertiserId = advertiserAccountId?.Value, costModelWrappers = costModelWrappers, HasObjective = campaignSettingsDto.HasObjective });

        }
        [HttpPost]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]
        public ActionResult SaveCampaignSettings([FromBody] CampaignSettingsDto settingsDto)
        {
            List<ResultMessage> rMessages = new List<ResultMessage>();

            try
            {
                if (ModelState.IsValid)
                {
                    _campaignService.SaveSettings(settingsDto);
                    FillCostModelWrappers(settingsDto.ValidCostModelWrapper, settingsDto.CostModelWrapper);
                    AddSuccessfullyMsg(formattedStrings: "Campaign Settings");
                    return Json(settingsDto, "Campaign Settings", ResponseStatus.success);
                }
                else
                {
                    AddModelStateErrorMsgs(ModelState);
                    return Json("Campaign Settings", ResponseStatus.businessException);
                }

            }
            catch (BusinessException exception)
            {
                AddErrorMsgs(exception);
                return Json("Campaign Settings", ResponseStatus.businessException);
            }
        }

        [HttpDelete]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]
        public ActionResult RemoveCampaignDiscount(int Id)
        {
            try
            {
                _campaignService.RemoveDiscount(new ValueMessageWrapper<int> { Value = Id });
                AddSuccessfullyMsgMs("Campaign discount removed successfully");
                return Json("Campaign Discount", ResponseStatus.success);
            }
            catch (BusinessException exception)
            {
                AddErrorMsgs(exception);
                return Json("Campaign Discount", ResponseStatus.businessException);
            }
        }

        //[DenyRole(Roles = "AppOps")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult Settings(int? id, bool isReact = false)
        {
            return View("Create/settings");
            if (!id.HasValue)
            {
                throw new ArabyAds.AdFalcon.Exceptions.Core.DataNotFoundException();
            }
            var campaignSettingsDto = _campaignService.GetSettings(new ValueMessageWrapper<int> { Value = id.Value });
            var AdvertiserId = _campaignService.GetCampaignAdvertiser(new ValueMessageWrapper<int> { Value = id.Value });
            var AdvertiserAccountId = _campaignService.GetCampaignAdvertiserAccount(new ValueMessageWrapper<int> { Value = id.Value });
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
                                                  Url = Url.Action("Create", "Campaign", new {AdvertiseraccId=AdvertiserAccountId,id = id}),
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

            FillCostModelWrappers(campaignSettingsDto.ValidCostModelWrapper, campaignSettingsDto.CostModelWrapper);
            //ViewData["CreateAdvertiserId"] = AdvertiserId;

            ViewBag.KeywordAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Kewords_Name",
                Name = "Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                IsRequired = true,
                ChangeCallBack = "KewordChanged",
                CurrentText = campaignSettingsDto.Keyword != null ? campaignSettingsDto.Keyword.Name.ToString() : string.Empty
            };
            ViewBag.AdvertiserAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
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
                CurrentText = campaignSettingsDto.Advertiser != null ? campaignSettingsDto.Advertiser.Name.ToString() : string.Empty
            };
            campaignSettingsDto.TrackConversionsUrlHttp = string.Format(Config.TrackConversionsHttpUrl, campaignSettingsDto.UniqueId);
            campaignSettingsDto.TrackConversionsUrlHttps = string.Format(Config.TrackConversionsHttpsUrl, campaignSettingsDto.UniqueId);
            //_AdvertiserService.GetAll();
            _keywordService.GetAll();
            ViewData["AdvertiserIdForTab"] = AdvertiserId;

            ViewData["AdvertiserAccountIdForTab"] = AdvertiserAccountId;
            if (isReact)
                return View("Create/settings");
            else
                return View("Settings", campaignSettingsDto);
        }

        //[DenyRole(Roles = "AppOps")]
        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Settings(CampaignSettingsDto settingsDto, int id, string returnUrl)
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
                                                  Url = Url.Action("Create", "Campaign", new {id = id,returnUrl=returnUrl}),
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
                    if (!string.IsNullOrWhiteSpace(Request.Form["Remove"]))
                    {
                        //clear discount
                        _campaignService.RemoveDiscount(new ValueMessageWrapper<int> { Value = settingsDtoData.ID });
                    }
                    else
                    {
                        _campaignService.SaveSettings(settingsDtoData);
                    }
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("Settings", new { id = id, returnUrl = returnUrl });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            ViewBag.KeywordAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Kewords_Name",
                Name = "Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged",
                CurrentText = settingsDto.Keyword != null && _keywordService.Get(new ValueMessageWrapper<int> { Value = settingsDto.Keyword.ID }) != null ? _keywordService.Get(new ValueMessageWrapper<int> { Value = settingsDto.Keyword.ID }).Name.ToString() : string.Empty
            };
            ViewBag.AdvertiserAutoComplete = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Advertisers_Name",
                Name = "Advertisers.Name",
                ActionName = "GetAdvertisers",
                ControllerName = "Advertiser",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "AdvertisersChanged",
                CurrentText = settingsDto.Advertiser != null && _AdvertiserService.Get(new ValueMessageWrapper<int> { Value = settingsDto.Advertiser.ID }) != null ? _AdvertiserService.Get(new ValueMessageWrapper<int> { Value = settingsDto.Advertiser.ID }).Name.ToString() : string.Empty
            };
            #region Cost Models

            FillCostModelWrappers(settingsDto.ValidCostModelWrapper, settingsDto.CostModelWrapper);

            #endregion
            //ViewData["CreateAdvertiserId"] = AdvertiserId;
            return View("Settings", settingsDto);
        }




        public ActionResult GetAppSitePriceModel(int id)
        {

            var Appsite = _appSiteService.GetServerSettings(new ValueMessageWrapper<int> { Value = id });
            if (Appsite.AppSiteServerSetting.CostModelWrapper == null)
            {
                return Json(new { PriceModel = ResourcesUtilities.GetResource("Default", "Campaign") });
            }

            return Json(new { PriceModel = Appsite.AppSiteServerSetting.CostModelWrapper.Name.Value });

        }

        public ActionResult getTrackingEvents()
        {
            var trackingEventsDto = _trackingEventService.GetCostModelEvents();
            var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();


            return new JsonResult(new { name = "amer" });
        }


        #endregion

        #region Private Methods


        private IEnumerable GetCostModelWrappers(int? validCostModel)
        {
            var optionalItem = new List<object>();
            optionalItem.Add(new { value = "", label = ResourcesUtilities.GetResource("All", "Global") });
            var costModelWrappersList = _CostModelWrapperService.GetAll();
            if (!validCostModel.HasValue)
            {
                optionalItem.AddRange(costModelWrappersList.Select(p => new { value = p.ID, label = p.Name.ToString() }));
            }
            else if (validCostModel != 0)
            {
                CostModelWrapperDto costModelWrapper = costModelWrappersList.Single(p => p.ID == validCostModel.Value);
                optionalItem.Add(new { value = costModelWrapper.ID, label = costModelWrapper.Name.ToString() });
            }
            return optionalItem;
        }
        private void FillCostModelWrappers(int? validCostModel, int? campaignCostModelWrapper)
        {
            #region Cost Models
            //Load Cost Models List
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("All", "Global") };
            var costModelWrappers = new List<SelectListItem> { optionalItem };
            var costModelWrappersList = _CostModelWrapperService.GetAll();

            if (!validCostModel.HasValue)
            {
                costModelWrappers.AddRange(costModelWrappersList.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name.ToString() }));
            }
            else
            {
                if (validCostModel != 0)
                {
                    CostModelWrapperDto costModelWrapper = costModelWrappersList.Where(p => p.ID == validCostModel.Value).Single();
                    costModelWrappers.Add(new SelectListItem() { Value = costModelWrapper.ID.ToString(), Text = costModelWrapper.Name.ToString() });
                }
            }


            var costmodelWrapper = costModelWrappers.FirstOrDefault(costModel => costModel.Value == (campaignCostModelWrapper.HasValue ? campaignCostModelWrapper.Value : 0).ToString());
            if (costmodelWrapper != null)
            {
                costmodelWrapper.Selected = true;
            }
            else
            {
                optionalItem.Selected = true;
            }
            ViewBag.CostModels = costModelWrappers;

            #endregion
        }

        private void FixCampaignServerSettingDto(CampaignServerSettingDto settingDto)
        {
            //if (!settingDto.AdImpressionIsCapping)
            //{
            //    settingDto.AdImpressionLifeTime = 1;
            //    settingDto.AdImpressionLifeTimeInterval = 0;
            //    settingDto.AdImpressionLifeTimeType = 0;
            //}
            //else
            //{
            //    if (!settingDto.AdImpressionLifeTime.HasValue)
            //    {
            //        settingDto.AdImpressionLifeTimeInterval = null;
            //        settingDto.AdImpressionLifeTimeType = null;
            //    }
            //}

            //if (!settingDto.AdClickIsCapping)
            //{
            //    settingDto.AdClickLifeTime = 1;
            //    settingDto.AdClickLifeTimeInterval = 0;
            //    settingDto.AdClickLifeTimeType = 0;
            //}
            //else
            //{
            //    if (!settingDto.AdClickLifeTime.HasValue)
            //    {
            //        settingDto.AdClickLifeTimeInterval = null;
            //        settingDto.AdClickLifeTimeType = null;
            //    }
            //}

            //if (!settingDto.AdConversionIsCapping)
            //{
            //    settingDto.AdConversionLifeTime = 1;
            //    settingDto.AdConversionLifeTimeInterval = 0;
            //    settingDto.AdConversionLifeTimeType = 0;
            //}
            //else
            //{
            //    if (!settingDto.AdConversionLifeTime.HasValue)
            //    {
            //        settingDto.AdConversionLifeTimeInterval = null;
            //        settingDto.AdConversionLifeTimeType = null;
            //    }
            //}
        }

        private void GetCreativesAttributes(ApproveAdDto approveAdDto, AdCreativeFullSummaryDto model)
        {
            approveAdDto.AdCreativesAttribues = new List<AdCreativeUnitDto>();

            foreach (var item in model.CreativeUnitsContent)
            {
                string attributeInputName = string.Format("Attributes_{0}", item.ID);
                string attributeInputValue = Request.Form[attributeInputName];
                if (!string.IsNullOrEmpty(attributeInputValue))
                {
                    List<int> attributesIds = attributeInputValue.Split(',').Select(p => int.Parse(p)).ToList();
                    AdCreativeUnitDto creativeDto = new AdCreativeUnitDto()
                    {
                        CreativeUnitId = item.CreativeUnitId,
                        ID = item.ID
                    };

                    List<AdCreativeAttributeDto> creativeAttributes = new List<AdCreativeAttributeDto>();
                    attributesIds = attributesIds.Distinct().ToList();
                    foreach (var attributeId in attributesIds)
                    {
                        AdCreativeAttributeDto attributeDto = new AdCreativeAttributeDto()
                        {
                            ID = attributeId
                        };

                        creativeAttributes.Add(attributeDto);
                    }

                    creativeDto.Attributes = creativeAttributes;
                    approveAdDto.AdCreativesAttribues.Add(creativeDto);
                }
            }
        }

        private void GetUploadedSnapshots(ApproveAdDto approveAdDto, AdCreativeFullSummaryDto model)
        {

            switch (model.TypeId)
            {
                case AdTypeIds.Banner:
                    break;
                case AdTypeIds.Text:
                case AdTypeIds.PlainHTML:
                case AdTypeIds.RichMedia:
                case AdTypeIds.NativeAd:
                    if (model.TypeId == AdTypeIds.RichMedia && model.AdSubType == AdSubTypes.ExpandableRichMedia)
                    {
                        return;
                    }

                    //if (model.TypeId == AdTypeIds.RichMedia && model.AdSubType == AdSubTypes.HTML5RichMedia)
                    //{
                    //    return;
                    //}
                    DeviceTypeEnum deviceType = model.AdBannerType.HasValue ? model.AdBannerType.Value : DeviceTypeEnum.Any;
                    approveAdDto.Snapshots = new List<AdCreativeUnitDto>();

                    foreach (var item in model.CreativeUnitsContent)
                    {
                        string creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.TypeId, "0", (int)deviceType, item.CreativeUnitId.ToString());

                        var adCreativeUnit = new AdCreativeUnitDto
                        {
                            CreativeUnitId = item.CreativeUnitId,
                            Content = Request.Form[creativeUnitName]
                        };

                        if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                        {
                            approveAdDto.Snapshots.Add(adCreativeUnit);
                        }
                    }

                    break;

                case AdTypeIds.InStreamVideo:
                    {

                        //DeviceTypeEnum deviceType = model.AdBannerType.HasValue ? model.AdBannerType.Value : DeviceTypeEnum.Any;
                        approveAdDto.Snapshots = new List<AdCreativeUnitDto>();

                        foreach (var item in model.CreativeUnitsContent)
                        {
                            string creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.TypeId, "0", (int)DeviceTypeEnum.Any, item.CreativeUnitId.ToString());

                            var adCreativeUnit = new AdCreativeUnitDto
                            {
                                CreativeUnitId = item.CreativeUnitId,
                                Content = item.InStreamVideoCreativeUnit.ThumbnailDocId.ToString()// Request.Form[creativeUnitName]
                            };

                            if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content) && adCreativeUnit.Content != "0")
                            {
                                approveAdDto.Snapshots.Add(adCreativeUnit);
                            }
                        }


                        deviceType = model.AdBannerType.HasValue ? model.AdBannerType.Value : DeviceTypeEnum.Any;
                        if (model.VideoEndCardCreativeUnitsContent != null)
                        {
                            foreach (var item in model.VideoEndCardCreativeUnitsContent)
                            {
                                string creativeUnitName = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)model.TypeId, "0", (int)DeviceTypeEnum.Any, item.CreativeUnitId);

                                var adCreativeUnit = new AdCreativeUnitDto
                                {
                                    CreativeUnitId = item.CreativeUnit.ID,
                                    Content = Request.Form[creativeUnitName]
                                };

                                if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                                {
                                    approveAdDto.Snapshots.Add(adCreativeUnit);
                                }
                            }
                        }

                        break;
                    }
                default:
                    break;
            }

        }

        private AdCreativeFullSummaryViewModel GetFullSummaryViewModel(AdCreativeFullSummaryDto adCreativeSummaryDto)
        {
            var viewModel = new AdCreativeFullSummaryViewModel()
            {
                ViewSummary = adCreativeSummaryDto
            };

            viewModel.SnapshotViewModel = new List<CreativeUnitViewModel>();

            DeviceTypeEnum deviceType = adCreativeSummaryDto.AdBannerType.HasValue ? adCreativeSummaryDto.AdBannerType.Value : DeviceTypeEnum.Any;

            foreach (var item in adCreativeSummaryDto.CreativeUnitsContent)
            {
                var creativeDeviceType = deviceType;

                var creativeUnit = new CreativeUnitViewModel()
                {
                    //alid
                    DocumentId = item == null ? (int?)null : item.SnapshotDocumentId,
                    Content = item != null ? item.Content : string.Empty,
                    DisplayText = item != null ? item.Name : string.Empty,
                    CreativeUnitDto = item != null ? item.CreativeUnit : null,
                    DeviceType = creativeDeviceType,
                    AdSubTypeId = adCreativeSummaryDto.AdSubType != null ? (int)adCreativeSummaryDto.AdSubType : 0,
                    AdTypeId = (int)adCreativeSummaryDto.TypeId,
                    Name = string.Format("CreativeUnit_{0}_{1}_{2}_{3}", (int)adCreativeSummaryDto.TypeId, "0", (int)creativeDeviceType, item.CreativeUnitId),
                };

                viewModel.SnapshotViewModel.Add(creativeUnit);
            }

            if (viewModel.SnapshotViewModel.Count > 1)
            {
                viewModel.SnapshotViewModel.First().ShowCopy = true;
            }

            return viewModel;
        }

        private string GetIntervalName(int interval)
        {
            switch (interval)
            {
                case 3600:
                    return ResourcesUtilities.GetResource("Hour", "Global");
                case 86400:
                    return ResourcesUtilities.GetResource("Day", "Global");
                case 604800:
                    return ResourcesUtilities.GetResource("Week", "Global");
                case 2592000:
                    return ResourcesUtilities.GetResource("Month", "Global");
                default:
                    return ResourcesUtilities.GetResource("Hour", "Global");
            }
        }

        #endregion


        #region CampaingSearch
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,AppOps")]
        public ActionResult CampaignSearch(int Id)
        {


            return PartialView(LoadDataCampaingSearch(null, Id));
        }

        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,AppOps")]
        public ActionResult _CampaignSearch(int Id)
        {
            var result = LoadDataCampaingSearch(null, Id);
            // return PartialView();

            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }

        protected ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter getDefualtFilterCampaign()
        {
            ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            //filter.StatusId = string.IsNullOrWhiteSpace(Request.Form["StatusId"]) ? (int?)null : Convert.ToInt32(Request.Form["StatusId"]);
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"].ToString();

            return filter;
        }
        protected CampaignCriteria GetCampainSearchCriteria(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int Id)
        {
            if (filter == null)
                filter = getDefualtFilterCampaign();
            var criteria = new CampaignCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = string.IsNullOrEmpty(filter.Name) ? "" : filter.Name,
                AccountId = Id,
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
        protected virtual CampaignListResultDto GetCampainSearchQueryResult(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int Id)
        {
            var criteria = GetCampainSearchCriteria(filter, Id);
            var result = _campaignService.QueryByCratiria(criteria);
            return result;
        }
        protected virtual CampaignListResultDto LoadDataCampaingSearch(ArabyAds.AdFalcon.Web.Controllers.Model.Campaign.Filter filter, int Id)
        {
            var result = GetCampainSearchQueryResult(filter, Id);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            // create the actions

            //var actions = GetCampaignAction();
            //List<Action> toolTips = GetCampaignTooltip();
            //load the statues 
            //var statues = _campaignStatusService.GetAll();
            //var statuesDropDown = GetSelectList();
            //foreach (var item in statues)
            //{
            //    var selectItem = new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() };
            //    statuesDropDown.Add(selectItem);
            //}
            return result;
            //return new ListViewModel()
            //{
            //    Items = items
            //    //TopActions = actions,
            //    //BelowAction = actions,
            //    //ToolTips = toolTips
            //    //, Statuses = statuesDropDown 
            //};
        }


        #endregion

        public ActionResult SaveGlobalMasterAppSites([FromBody] AdvertiserAccountMasterAppSiteDto dto)
        {
            try
            {


                dto.GlobalScope = true;
                dto.AccountId = ArabyAds.Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                dto.UserId = (int)ArabyAds.Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;


                _AdvertiserService.SaveAdvertiserAccountMasterAppSite(dto);
                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("ContentList", "Global"));
                return Json(ResourcesUtilities.GetResource("ContentList", "Global"), ResponseStatus.success);

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
        public virtual ActionResult GlobalMasterAppSites(int? Id)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MasterAppSiteLists","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;


            if (Id.HasValue)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(new ValueMessageWrapper<int> { Value = Id.Value });
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
                                                  Text =ResourcesUtilities.GetResource("MasterAppSiteLists","Global")  ,
                                                  Order = 3,
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks2;
            }
            #endregion

            ViewData["AllowDelete"] = breadCrumbLinks;

            var filter = getDefualtFilter();
            filter.showRoot = true;
            return View("GlobalMasterAppSites/Index", MasterAppSiteLoadData(filter, Id));
        }

        [AcceptVerbs("Post")]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult GlobalMasterAppSites(int? Id, int[] checkedRecords)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("MasterAppSite","Global")  ,
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            if (Id.HasValue)
            {
                var dto = _AdvertiserService.GetAccountAdvertiserById(new ValueMessageWrapper<int> { Value = Id.Value });
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
                                                  Text =ResourcesUtilities.GetResource("MasterAppSiteLists","Global")  ,
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
            if (Id.HasValue)
                return RedirectToAction("GlobalMasterAppSites", new { Id = Id });

            else
                return RedirectToAction("GlobalMasterAppSites");
        }
        
    }
}
