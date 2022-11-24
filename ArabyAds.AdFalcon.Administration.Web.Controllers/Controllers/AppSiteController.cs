using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Action = System.Action;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class AppSiteController : Noqoush.AdFalcon.Web.Controllers.Controllers.AppSiteController
    {
        private ILookupService _lookupService;
        private IAppSiteService _appSiteService;
        private IAppSiteTypeService _appSiteTypeService;
        private IThemeService _themeService;
        protected ICreativeUnitService _creativeUnitService;
        protected ICostModelService _costModelService;
        protected ITrackingEventService _trackingEventService;
        protected ICampaignService _campaignService;
        protected IAccountService _accountService;


        private const string _BannerTypeId = "1";
        private const string _TextTypeId = "2";

        public AppSiteController(ICampaignService campaignService, IAppSiteService appSiteService,
                                 IAppSiteTypeService appSiteTypeService,
                                                IAccountService accountService,
                                 IThemeService themeService, ICreativeUnitService creativeUnitService, ICostModelService costModelService, ITrackingEventService trackingEventService, ILookupService lookupService)
            : base(appSiteService, appSiteTypeService, accountService, themeService)
        {
            this.DispalyResourceName = "AppSiteDispalyName";
            _campaignService = campaignService;
            _appSiteService = appSiteService;
            _appSiteTypeService = appSiteTypeService;
            _themeService = themeService;
            _creativeUnitService = creativeUnitService;
            _costModelService = costModelService;
            _trackingEventService = trackingEventService;
            _lookupService = lookupService;

        }


        #region AppSite

        #region AppSite Approval
        public PartialViewResult ApprovalView(string ViewName, int Id)
        {

            var model = GetAppSiteApprovalViewModel(Id);



            return PartialView(ViewName, model);
        }


        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult Approval(int id, string successfulMassage, bool status = false)
        {
            var appSite = _appSiteService.Get(id);



            var model = GetAppSiteApprovalViewModel(id);
            if (appSite.Type != null)
            {
                model.AppSiteViewName = appSite.Type.ViewName;

            }

            ViewData["successfulMassage"] = successfulMassage;

            ViewData["status"] = status;

            return View(model);
        }



        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Approval(AppSiteApprovalDto model, int approveStatus)
        {
            string successfulMassage = "", errorMassge = "";

            if (ModelState.IsValid)
            {


                try
                {

                    model.StatusId = approveStatus;
                    AppSiteApprovalDto dto = new AppSiteApprovalDto() { AppSiteId = model.AppSiteId, Comments = model.Comments, StatusId = model.StatusId, Type = model.Type, NewKeywords = model.NewKeywords, DeletedKeywords = model.DeletedKeywords };

                    _appSiteService.Approval(dto);


                    var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                    var savedSuccessfully = approveStatus == 1 ? ResourcesUtilities.GetResource("Approved", "AppSite") : ResourcesUtilities.GetResource("Rejected", "AppSite");


                    successfulMassage = string.Format(savedSuccessfully, dispalyName);


                }
                catch (BusinessException exception)
                {
                    errorMassge = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    successfulMassage = string.Empty;

                    return Json(new { ErrorMassge = errorMassge, SuccessfulMassage = "", status = false });

                }
            }


            return Json(new { ErrorMassge = "", SuccessfulMassage = successfulMassage, status = true });
        }

        #endregion

        #region Server Setting

        private List<SelectListItem> GetList(string lookupType, int? selectedValue)
        {
            LookupListResultDto items = null;

            if (lookupType != LookupNames.AdType && lookupType != LookupNames.NativeAdLayout && lookupType != LookupNames.BidConfigType)
                items = _lookupService.GetAllLookup(new LookupCriteriaBase { LookType = lookupType });
            else
            {
                if (lookupType == LookupNames.AdType)
                {
                    items = _lookupService.GetAdTypes();
                }
                if (lookupType == LookupNames.NativeAdLayout)
                {
                    items = _lookupService.GetNativeAdLayouts();
                }


            }
            var lookupsList = new List<SelectListItem> { new SelectListItem
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
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult ServerSetting(int id)
        {
            return View(GetServerSettingViewModel(id));
        }

        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult ServerSetting(int id, AppSiteServerSettingViewModel model, string CalculationMode, int? pricingModel)
        {
            //set the supported ad types

            if (!ModelState.IsValid)
            {
                if (ModelState["SettingsDto.AppSiteServerSetting.NativeLayoutId"] != null && ModelState["SettingsDto.AppSiteServerSetting.NativeLayoutId"].Errors != null)
                    ModelState["SettingsDto.AppSiteServerSetting.NativeLayoutId"].Errors.Clear();
                if (!ModelState.IsValid)
                    return View(GetServerSettingViewModel(id));
            }

            if (pricingModel.HasValue || Request.Form.AllKeys.Any(p => p.StartsWith("FloorPrice")))
            {
                List<AppSiteEventDto> eventsList = new List<AppSiteEventDto>();
                AppSiteEventDto pricingModelEvent = null;

                if (pricingModel.HasValue)
                {
                    pricingModelEvent = new AppSiteEventDto();
                    pricingModelEvent.EventId = pricingModel.Value;
                    pricingModelEvent.IsBillable = true;

                    eventsList.Add(pricingModelEvent);
                }

                foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("FloorPrice")))
                {
                    string floorPrice = Request.Form[item];

                    if (!string.IsNullOrEmpty(floorPrice))
                    {
                        decimal floorPriceValue = decimal.Parse(floorPrice);
                        int eventId = int.Parse(item.Split('-')[1]);

                        if (pricingModelEvent != null && pricingModelEvent.EventId == eventId)
                        {
                            pricingModelEvent.MinBid = floorPriceValue;
                        }
                        else
                        {
                            AppSiteEventDto eventDto = new AppSiteEventDto();
                            eventDto.EventId = eventId;
                            eventDto.MinBid = floorPriceValue;

                            eventsList.Add(eventDto);
                        }
                    }
                }

                model.SettingsDto.AppSiteServerSetting.Events = eventsList;
            }

            if (CalculationMode != "0")
            {
                model.SettingsDto.CurrentRevenueCalculationSettings = new AppSiteRevenueCalculationSettingDto() { CalculationMode = (Domain.Common.Model.AppSite.CalculationMode)int.Parse(CalculationMode), Value = model.SettingsDto.CurrentRevenueCalculationSettings.Value };
            }
            else
            {
                model.SettingsDto.CurrentRevenueCalculationSettings = null;
            }
    
            model.SettingsDto.AppSiteServerSetting.SupportedAdTypes = string.Empty;
            model.SettingsDto.AppSiteServerSetting.AllowBlindAds = !model.SettingsDto.AppSiteServerSetting.AllowBlindAds;
            if (model.IsSupportBannerAd || model.IsSupportTextAd)
            {
                if (model.IsSupportBannerAd && model.IsSupportTextAd)
                {
                    model.SettingsDto.AppSiteServerSetting.SupportedAdTypes = null;
                }
                else
                {
                    model.SettingsDto.AppSiteServerSetting.SupportedAdTypes += model.IsSupportBannerAd ? _BannerTypeId : string.Empty;
                    model.SettingsDto.AppSiteServerSetting.SupportedAdTypes += model.IsSupportTextAd ? "," + _TextTypeId : string.Empty;
                    model.SettingsDto.AppSiteServerSetting.SupportedAdTypes = model.SettingsDto.AppSiteServerSetting.SupportedAdTypes.Trim(',');
                }
            }
            else
            {
                AddMessages(ResourcesUtilities.GetResource("ErrorSupportedAdTypes", "AppSite"), MessagesType.Error);

                return View(GetServerSettingViewModel(id));
            }

            if (model.SupportedBannerImageTypeIds != null && !(model.SupportedBannerImageTypeIds.Count() == 1 && model.SupportedBannerImageTypeIds.Single().Trim() == ""))
            {
                //fill supported images types
                model.SettingsDto.AppSiteServerSetting.SupportedBannerImageTypes = string.Empty;

                foreach (var imageType in model.SupportedBannerImageTypeIds)
                {
                    model.SettingsDto.AppSiteServerSetting.SupportedBannerImageTypes += (imageType + ",");
                }

                model.SettingsDto.AppSiteServerSetting.SupportedBannerImageTypes =
              model.SettingsDto.AppSiteServerSetting.SupportedBannerImageTypes.Trim(',');
            }
            var ser = new JavaScriptSerializer();
            model.SettingsDto.ModifiedNotCompatableBidConfigs = ser.Deserialize<IList<CampaignBidConfigDto>>(string.IsNullOrWhiteSpace(model.ModifiedNotCompatableBidConfigs) ? "[]" : model.ModifiedNotCompatableBidConfigs);

            _appSiteService.SaveServerSettings(model.SettingsDto);
            AddSuccessfullyMsg();
            MoveMessagesTempData();
            return RedirectToAction("ServerSetting", new { id = model.SettingsDto.AppSiteId });
        }

        private AppSiteServerSettingViewModel GetServerSettingViewModel(int id)
        {
            var settingsDto = _appSiteService.GetServerSettings(id);
            var events = _trackingEventService.GetCostModelEvents();

            ViewData["AppSIteId"] = id;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                {
                    new BreadCrumbModel()
                        {
                            Text = ResourcesUtilities.GetResource("AppSiteServerSettings", "SiteMapLocalizations"),
                            Order = 3
                        },
                    new BreadCrumbModel()
                        {
                            Text = settingsDto.AppSiteName,
                            Order = 2,
                            Url = Url.Action("create", "appsite", new {Id = settingsDto.AppSiteId})
                        },
                    new BreadCrumbModel()
                        {
                            Text = ResourcesUtilities.GetResource("AppSiteList", "SiteMapLocalizations"),
                            Order = 1,
                            Url = Url.Action("Index", "appsite")
                        }
                };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            var model = new AppSiteServerSettingViewModel
            {
                SettingsDto = settingsDto,
                NativeAdLayouts = GetList(LookupNames.NativeAdLayout, null),
                Tabs = GetTabs(4, id.ToString()),
                CostModelEvents = events,

            };

            if (settingsDto.DefaultAccountRevenue.HasValue)
            {
                ViewData["DefaultAccountRevenue"] = settingsDto.DefaultAccountRevenue.Value;
            }

            //get the supported ads
            if (string.IsNullOrWhiteSpace(settingsDto.AppSiteServerSetting.SupportedAdTypes))
            {
                model.IsSupportBannerAd = model.IsSupportTextAd = true;
            }
            else
            {
                var supportedAdTypes = model.SettingsDto.AppSiteServerSetting.SupportedAdTypes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                model.IsSupportBannerAd = supportedAdTypes.Any(x => x == _BannerTypeId);
                model.IsSupportTextAd = supportedAdTypes.Any(x => x == _TextTypeId);
            }
            //Load the Creative Types
            var creativeFormats = _creativeUnitService.GetAllSupportedFormat();
            var creativeFormatDropDown = Utility.GetSelectList();

            if (!string.IsNullOrWhiteSpace(settingsDto.AppSiteServerSetting.SupportedBannerImageTypes))
            {
                var supportedImageTypes = model.SettingsDto.AppSiteServerSetting.SupportedBannerImageTypes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var creativeFormat in creativeFormats)
                {
                    var val = creativeFormat.Trim('.');
                    var selected = supportedImageTypes.Any(x => x.Equals(val));

                    creativeFormatDropDown.Add(new SelectListItem { Value = val, Text = val, Selected = selected });
                }
            }
            else
            {
                creativeFormatDropDown[0].Selected = true;
                foreach (var creativeFormat in creativeFormats)
                {
                    var val = creativeFormat.Trim('.');
                    creativeFormatDropDown.Add(new SelectListItem { Value = val, Text = val, Selected = false });
                }
            }

            model.ImageFormats = creativeFormatDropDown;

            return model;
        }
        public ActionResult CampaignBidConfigDialogView(bool showAppSitePricingModel = true)
        {
            var campaignBidConfigDto = new CampaignBidConfigModelDto();

            var model = new Noqoush.AdFalcon.Web.Controllers.Model.Campaign.CampaignBidConfigModel
            {

                CampaignBidConfigList = new List<CampaignBidConfigDto>()
            };
            ViewBag.showAppSitePricingModel = showAppSitePricingModel;

            return PartialView("AppSiteBidConfigDialog", model);
        }
        public ActionResult CheckAppsiteCompatableWithCampaigns(int appsiteID, int pricingModel)
        {
            IList<CampaignBidConfigDto> notCompatableAppSites = null;
            bool isValid = _appSiteService.CehckAppsitCostModelCompatableWitCampaigns(appsiteID, pricingModel, out notCompatableAppSites);

            return Json(new { status = isValid, List = notCompatableAppSites }, JsonRequestBehavior.AllowGet);
        }

        #region Helpers



        private AppSiteApprovalViewModel GetAppSiteApprovalViewModel(int id)
        {
            var appSiteInfo = _appSiteService.GetBasicInfo(id);

            /////
            //Load keyword List
            var keywordauto = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "AppSiteDto_Kewords_Name",
                Name = "AppSiteDto.Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged"
            };

            //get the Keyword Tag Cloud
            var keywordservice = Noqoush.Framework.IoC.Instance.Resolve<IKeywordService>();
            //TODo: Osaleh to get item count from Configuration setting
            var keywords = keywordservice.GetTop(null);
            var keywordTags =
                keywords.Select(
                    keywordDto =>
                    new TagCloud() { Id = keywordDto.ID, DispalValue = keywordDto.Name.ToString(), Rank = keywordDto.Rank }).ToList();
            /////


            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                {
                    new BreadCrumbModel()
                        {
                            Text = ResourcesUtilities.GetResource("Approval", "SiteMapLocalizations"),
                            Order = 3
                        },
                    new BreadCrumbModel()
                        {
                            Text = appSiteInfo.Name,
                            Order = 2,
                            Url = Url.Action("create", "appsite", new {Id = appSiteInfo.ID})
                        },
                    new BreadCrumbModel()
                        {
                            Text = ResourcesUtilities.GetResource("AppSiteList", "SiteMapLocalizations"),
                            Order = 1,
                            Url = Url.Action("Index", "appsite")
                        }
                };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var type = _appSiteTypeService.GetAll();


            var model = new AppSiteApprovalViewModel
            {
                AppSite = appSiteInfo,
                AppSiteTypes = type,
                Comments = appSiteInfo.AdminComment,
                Tabs = GetTabs(3, id.ToString()),

                KeywordViewModel =
                    new KeywordViewModel()
                    {
                        Prefix = "AppSiteDto.",
                        KewordAuto = keywordauto,
                        KeywordTags = keywordTags,
                        Keywords = appSiteInfo.Keywords,
                        AllowInsert = false
                    }

            };
            return model;
        }




        #endregion

        #endregion

        #endregion

    }
}
