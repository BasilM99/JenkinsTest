using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.Framework.Utilities.EmailsSender;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.Framework.Resources;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    //[RequireHttps(Order = 1)]
    public class MiscController : ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase
    {
        private IMailSender _mailSender;
        private IUserService _userService;
        private IAppSiteService _appSiteService;
        private ILanguageService _languageService;
        private ICountryService countryService;
        private IResourceManager resourceManager;
        private IAgeGroupService _ageGroupService;
        private IMetricService _MetricService;
        public MiscController(  )
        {
            this._mailSender = IoC.Instance.Resolve<IMailSender>();
            this._appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            this._userService = IoC.Instance.Resolve<IUserService>();
              this._languageService = IoC.Instance.Resolve<ILanguageService>();
            this.countryService = IoC.Instance.Resolve<ICountryService>();

            this.resourceManager = IoC.Instance.Resolve<IResourceManager>();

            this._ageGroupService = IoC.Instance.Resolve<IAgeGroupService>();

            this._MetricService = IoC.Instance.Resolve<IMetricService>();
        }
        [RequireHttps(Order = 1)]
        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "q","culture" })]
        
        public ActionResult GetLanguages(string q, string culture)
        {
            q ??= "";
            /*var results2 = _CampaignService.GetImpressionMetricTargetings(new Domain.Repositories.Campaign.Creative.ImpressionMetricCriteria {AdGroupId= 110899 });

               var results=_CampaignService.GetImpressionMetric();
            var gffgg=_CampaignService.GetImpressionMetricTargeting(11089999);*/
            return Json(ReturnLanguageResult(q, culture));
        }
        public PartialViewResult PublicInfo()
        {
            return PartialView("PublicUserInformation");
        }


        [RequireHttps(Order = 1)]
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


        [RequireHttps(Order = 1)]
        public PartialViewResult PublicInfoHttps()
        {
            return PartialView("PublicUserInformation");
        }

        [RequireHttps(Order = 1)]
        //[PermissionsAuthorize(Permission = Domain.Common.Model.Core.PortalPermissionsCode.TrafficPlanner, Roles = "Administrator,adops,AccountManager")]

        [OutputCache(Duration = 9200, VaryByQueryKeys = new string[] { })]

        public ActionResult GetTrafficPlannerVM()
        {
            var optionalItem = new SelectListItem { Value = "0", Text = ResourcesUtilities.GetResource("All", "Global") };
            var ageGroupDtos = _ageGroupService.GetAll().ToList();
            var ageGroups = new List<SelectListItem> { optionalItem };
            ageGroups.AddRange(ageGroupDtos.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));
            return Json(new
            {
                AgeGroups = ageGroups,
                AdFormatBanner = new { Value = (int)AdTypeGroup.Banner, Text = AdTypeGroup.Banner.ToText() },
                AdFormatInStream = new { Value = (int)AdTypeGroup.InStream, Text = AdTypeGroup.InStream.ToText() },
                AdFormatNative = new { Value = (int)AdTypeGroup.Native, Text = AdTypeGroup.Native.ToText() }
            });

        }

        private IEnumerable<LanguageDto> ReturnLanguageResult(string q, string culture)
        {


            var criteria = new LanguageCriteria() { Value = q, Culture = culture };
            var Advertisers = _languageService.GetByQuery(criteria);


            return Advertisers;
        }
        public ActionResult ThankYou(string resourceKeyName, string url, string title, string view, int? Id, bool login = false)
        {



            //#region BreadCrumb
            //var breadCrumbLinks = new List<BreadCrumbModel>();



            //if (!login)
            //{

            //    breadCrumbLinks.Add(new BreadCrumbModel()
            //    {
            //        Text = ResourcesUtilities.GetResource("AppSiteList", "SiteMapLocalizations"),
            //        Order = 1,
            //        Url = Url.Action("Index", "appsite")
            //    });

            //    breadCrumbLinks.Add(
            //        new BreadCrumbModel()
            //        {
            //            Text = Id == null ? "" : _appSiteService.Get(new ValueMessageWrapper<int> { Value = (int)Id }).Name,// ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
            //            Order = 2,
            //        });


            //}
            //else
            //{
            //    breadCrumbLinks.Add(new BreadCrumbModel()
            //    {
            //        Text = ResourcesUtilities.GetResource("Register", "UserInformation"),
            //        Order = 1

            //    });

            //    ViewBag.HideMenu = true;

            //}

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion

            //ViewBag.url = url;
            if (!string.IsNullOrWhiteSpace(title))
            {
                ViewBag.title = ResourcesUtilities.GetResource(title, "ThanksTitle");
            }
            if (!string.IsNullOrWhiteSpace(view))
            {
                ViewBag.view = view;
            }
            if (Id.HasValue)
            {
                ViewBag.Id = Id;
            }
         

            //ViewBag.ShowMenu = true;

            var values = (from key in Request.Query.Keys where !key.Equals("resourceKeyName", StringComparison.OrdinalIgnoreCase) select Request.Query[key]).ToList();
            IList<string> valuesStr = new List<string>();
            foreach (var val in  values)
            {
                if (val.Count >0)
                {
                    valuesStr.Add(val[0].ToString());
                }
            
            }
            
            string thanksMessage = string.Format(ResourcesUtilities.GetResource(resourceKeyName, "ThanksMessages"), valuesStr.ToArray());
            AddSuccessfullyMsgMs(thanksMessage);
            //AddMessages(thanksMessage, MessagesType.Success);
            //ViewBag.Mess = thanksMessage;
            return View();
        }

        [HttpPost]
        public ActionResult AgencyRequest(AgencyRequestDto agencyRequestDto)
        {
            if (ModelState.IsValid)
            {
                string emailTemplate = ResourcesUtilities.GetResource("AgencyRequest", "Emails");

                emailTemplate = emailTemplate.Replace("{address}", agencyRequestDto.Address).Replace("{email}", agencyRequestDto.Email).Replace("{phonenumber}", agencyRequestDto.PhoneNumber).Replace("{message}", agencyRequestDto.Message)
                    .Replace("{firstname}", agencyRequestDto.FirstName).Replace("{secondname}", agencyRequestDto.SecondName).Replace("{company}", agencyRequestDto.Company);

                string email = Config.ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");

                _mailSender.SendEmail("", "", email, email, ResourcesUtilities.GetResource("AgencyRequest", "EmailHeader"), emailTemplate);

                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }

        }

        [HttpPost]
        public ActionResult ContactUsRequest(ContactUsDto contactUsDto)
        {
            if (ModelState.IsValid)
            {
                string emailTemplate = ResourcesUtilities.GetResource("ContactUsRequest", "Emails");

                emailTemplate = emailTemplate.Replace("{Email}", contactUsDto.Email).Replace("{Message}", contactUsDto.Message)
                    .Replace("{Name}", contactUsDto.Name).Replace("{Subject}", contactUsDto.Subject);

                string email = Config.ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");

                _mailSender.SendEmail("", "", email, email, contactUsDto.Subject, emailTemplate);

                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }

        }
        /// <summary>
        /// This method is hot fix for https requests that comes from mediaagency page in the public website when the url is https and not http
        /// </summary>
        /// <param name="https"></param>
        /// <param name="agencyRequestDto"></param>
        /// <returns></returns>

        [RequireHttps]
        public ActionResult AgencyHttpsRequest(AgencyRequestDto agencyRequestDto)
        {
            JsonResult result = new JsonResult(null);
           
            if (ModelState.IsValid)
            {
                string emailTemplate = ResourcesUtilities.GetResource("AgencyRequest", "Emails");

                emailTemplate = emailTemplate.Replace("{address}", agencyRequestDto.Address).Replace("{email}", agencyRequestDto.Email).Replace("{phonenumber}", agencyRequestDto.PhoneNumber).Replace("{message}", agencyRequestDto.Message)
                    .Replace("{firstname}", agencyRequestDto.FirstName).Replace("{secondname}", agencyRequestDto.SecondName).Replace("{company}", agencyRequestDto.Company);

                string email = Config.ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");

                _mailSender.SendEmail("", "", email, email, ResourcesUtilities.GetResource("AgencyRequest", "EmailHeader"), emailTemplate);

                result.Value = new { status = "success" };
                return result;
            }
            else
            {
                result.Value = new { status = "error" };
                return result;
            }

        }

        [HttpPost]
        [RequireHttps]
        public ActionResult ContactUsHttpsRequest(ContactUsDto contactUsDto)
        {
            if (ModelState.IsValid)
            {
                string emailTemplate = ResourcesUtilities.GetResource("ContactUsRequest", "Emails");

                emailTemplate = emailTemplate.Replace("{Email}", contactUsDto.Email).Replace("{Message}", contactUsDto.Message)
                    .Replace("{Name}", contactUsDto.Name).Replace("{Subject}", contactUsDto.Subject);

                string email = Config.ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");

                _mailSender.SendEmail("", "", email, email, contactUsDto.Subject, emailTemplate);

                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }
        }


        [HttpPost]
        public ActionResult OptOutRequest(string optoutEmail)
        {
            if (ModelState.IsValid)
            {

                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }

        }
        [HttpPost]
        [RequireHttps]
        public ActionResult OptOutHttpsRequest(string optoutEmail)
        {
            if (ModelState.IsValid)
            {

                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "error" });
            }

        }



        public ActionResult ReadUserOpt()
        {

            string userId = string.Empty;
            // load the culture info from the cookie
            var cookie = Request.Cookies["ADFALCON-UUID"];
            var langHeader = string.Empty;
            if (cookie != null)
            {
                // set the culture by the cookie content
                userId = cookie;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                var result = this._userService.GetUserForOptingInDB(userId);

                return Json(new { status = result.ToString() });
            }
            else
            {

                userId = this._userService.CreateUserIdForOpting();
                //var newcookie = new System.Web.HttpCookie("ADFALCON-UUID", userId);
                ////{
                ////    Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                ////    Secure = FormsAuthentication.RequireSSL,
                ////    HttpOnly = true
                ////};
                //newcookie.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);
                //if (!Request.GetDisplayUrl().ToLower().Contains("localhost"))
                //{
                //    newcookie.Domain = Config.CookieDomain;
                //}

                CookieOptions option = new CookieOptions();

                option.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);
                if (!Request.GetDisplayUrl().ToLower().Contains("localhost"))
                {
                    option.Domain = Config.CookieDomain;
                }

                Response.Cookies.Append("ADFALCON-UUID", userId, option);

                return Json(new { status = "True" });
            }



        }
        [RequireHttps]
        public ActionResult ReadUserOptHttps()
        {

            string userId = string.Empty;
            // load the culture info from the cookie
            var cookie = Request.Cookies["ADFALCON-UUID"];
            var langHeader = string.Empty;
            if (cookie != null)
            {
                // set the culture by the cookie content
                userId = cookie;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                var result = this._userService.GetUserForOptingInDB(userId);

                return Json(new { status = result.ToString() });
            }
            else
            {

                userId = this._userService.CreateUserIdForOpting();
                //var newcookie = new System.Web.HttpCookie("ADFALCON-UUID", userId);
                ////{
                ////    Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                ////    Secure = FormsAuthentication.RequireSSL,
                ////    HttpOnly = true
                ////};
                //newcookie.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);



                CookieOptions option = new CookieOptions();

                option.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);
                if (!Request.GetDisplayUrl().ToLower().Contains("localhost"))
                {
                    option.Domain = Config.CookieDomain;
                }


                Response.Cookies.Append("ADFALCON-UUID", userId, option);

                return Json(new { status = "True" });
            }



        }
        [HttpPost]
        public void UpdateUserOpt(bool tracking)
        {

            string userId = string.Empty;
            // load the culture info from the cookie
            var cookie = Request.Cookies["ADFALCON-UUID"];

            if (cookie != null)
            {
                // set the culture by the cookie content
                userId = cookie;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                this._userService.UpdateUserIdForOptingInDB(new UpdateUserIdForOptingInDBRequest { UserId=userId, TrackEnabled= tracking });


            }
        }

        [HttpPost]
        public void UpdateUserOptHttps(bool tracking)
        {

            string userId = string.Empty;
            // load the culture info from the cookie
            var cookie = Request.Cookies["ADFALCON-UUID"];

            if (cookie != null)
            {
                // set the culture by the cookie content
                userId = cookie;

                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                this._userService.UpdateUserIdForOptingInDB(new UpdateUserIdForOptingInDBRequest { UserId=userId,  TrackEnabled=tracking});


            }
        }



       
        public ActionResult GetUserRandomScript()
        {
            Random r = new Random();

           int resultd = r.Next(10, 50);
            if(resultd%2 ==0 )
            return Redirect("http://cdn01.static.adfalcon.com/static/js/drawad.js");
            else

                return Redirect("http://cdn01.static.adfalcon.com/static/js/drawad2.js");
        }

        [RequireHttps]
        public ActionResult GetUserRandomScripts()
        {
            Random r = new Random();

            int resultd = r.Next(10, 50);
            if (resultd % 2 == 0)
                return Redirect("https://cdn01.static.adfalcon.com/static/js/drawad.js");
            else

                return Redirect("https://cdn01.static.adfalcon.com/static/js/drawad2.js");
        }

        public ActionResult GetUserRandomURL(string url)
        {
            Random r = new Random();

            int resultd = r.Next(10, 50);
            if (resultd % 2 == 0)
                return Redirect(url);
            else

                return Redirect("http://cdn01.static.adfalcon.com/static/js/drawad2.js");
        }

        [RequireHttps]
        public ActionResult GetUserRandomURLs(string url)
        {
            Random r = new Random();

            int resultd = r.Next(10, 50);
            if (resultd % 2 == 0)
                return Redirect(url);
            else

                return Redirect("https://cdn01.static.adfalcon.com/static/js/drawad2.js");
        }

        [OutputCache(Duration = 21600)]
        public ActionResult GetCountries()
        {
            SelectListItem optionalItem = new SelectListItem();
            optionalItem.Value = "";
            optionalItem.Text = ResourcesUtilities.GetResource("ByCountry", "Chart"); ;

            ArabyAds.AdFalcon.Services.Interfaces.Services.ICountryService countryService = this.countryService;
            List<SelectListItem> countriesList = new List<SelectListItem>();
            //countriesList.Add(optionalItem);

            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = countryService.GetAll().OrderBy(p => p.Name.Value).ToList();

            foreach (var item in countriesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();
                countriesList.Add(selectItem);
            }
            return Json(countriesList);
        }


        [RequireHttps]
        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "setName", "langName" })]
        public async Task<IActionResult> Resources(string setName, string langName)
        {
            List<string> setNames = setName.Split(' ').ToList();
            Dictionary<string, Dictionary<string, string>> allResources = new Dictionary<string, Dictionary<string, string>>();

            if (langName == "en")
            {
                //var result = await RetrieveValueAsync(setName, "en-US");
                //if (result["en-US"] != null)
                  //  return Json(result["en-US"]);

                foreach (var item in setNames)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var result = await RetrieveValueAsync(item, "en-US");
                        allResources.Add(item, result["en-US"]);
                    }
                }

                return Json(new { en = allResources });

            }
            else
            {
                foreach (var item in setNames)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var result = await RetrieveValueAsync(item, "ar-JO");
                        allResources.Add(item, result["ar-JO"]);
                    }
                }

                return Json(new { ar = allResources });
            }
            return Content("{}");
        }

        [RequireHttps]
        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] {  "langName" })]
        public async Task<IActionResult> GetAllResources( string langName)
        {
      
            Dictionary<string, Dictionary<string, string>> allResources = new Dictionary<string, Dictionary<string, string>>();
            List<string> arrayofResourceSet = new List<string>{
                  "Ad",
                  "AdChart",
                  "AddFund",
                  "AddPayment",
                  "AdFund",
                  "AdGroup",
                  "AdGroupObjective",
                  "AdGroupTrackingEvent",
                  "AdRequests",
                  "Advertiser",
                  "AdvSecurity",
                  "APIAccess",
                  "AppChart",
                  "AppReport",
                  "AppsDashboard",
                  "AppSite",
                  "AppSiteSettings",
                  "AppType",
                  "Audiances",
                  "AudianceSegment",
                  "Audience",
                  "AudienceList",
                  "audiences",
                  "AudienceSegment",
                  "AudienceSegments",
                  "AuditTrial",
                  "BankAccount",
                  "BidConfig",
                  "BidConfigType",
                  "Billing",
                  "Business",
                  "byCatgeo",
                  "CampaignAssignAppsites",
                  "CampaignBidConfig",
                  "CampaignReport",
                  "CampaignServerSetting",
                  "CampaignSettings",
                  "CampaignsReport",
                  "ChangePassword",
                  "Chart",
                  "Clone",
                  "Columns",
                  "ContextualTargeting",
                  "CostElement",
                  "CostElementCalculatedFrom",
                  "CostElements",
                  "CostItemCategroyFlags",
                  "CostItemType",
                  "CreateHouseAd",
                  "Creative",
                  "CreativesSettings",
                  "Dashboard",
                  "DataProviders",
                  "DeActivate",
                  "Deal",
                  "DeleteAccount",
                  "Descs",
                  "DeviceTypeTargeting",
                  "DPP",
                  "DPPartners",
                  "DSPPartners",
                  "DSPRequest",
                  "DynamicBidding",
                  "Edit",
                  "EmailHeader",
                  "Emails",
                  "Error",
                  "Errors",
                  "EventBroker_Emails",
                  "FeeCalculatedFrom",
                  "FeeElement",
                  "Filters",
                  "Footer",
                  "Forgetpassword",
                  "GlobalErrors",
                  "HouseAd",
                  "ImagesUploadType",
                  "Impersonate",
                  "ImpressionLog",
                  "InstreamVideo",
                  "InStreamVideoCreative",
                  "Invitation",
                  "Invite",
                  "JobGrid",
                  "Keywords",
                  "LanguageFilters",
                  "LocationAdmin",
                  "Login",
                  "Lookup",
                  "Mail",
                  "MasterAppSite",
                  "MasterAppSiteItem",
                  "NativeAd",
                  "Party",
                  "PGW",
                  "Pixel",
                  "PMPDeal",
                  "PMPDeals",
                  "PMPDealTargetings",
                  "Receipt",
                  "Register",
                  "ReplacementType",
                  "Report",
                  "ReportBuilder",
                  "Reports",
                  "ReportSchedule",
                  "Resend",
                  "ResourceSet",
                  "RichMedia",
                  "SiteMapLocalizations",
                  "SSPCommands",
                  "SSPDealCampaign",
                  "SSPDealCampaignMapping",
                  "SSPFloorPrices",
                  "SSPPartner",
                  "SSPPartners",
                  "SSPSites",
                  "SSPSiteZoneMappings",
                  "SSPSiteZones",
                  "SwitchAccount",
                  "Targeting",
                  "Tax",
                  "TextFilter",
                  "TextFilters",
                  "ThanksMessages",
                  "ThanksTitle",
                  "Time",
                  "TopAccounts",
                  "TrackingAd",
                  "Undefined",
                  "Upload",
                  "UrlFilters",
                  "User",
                  "UserAgreement",
                  "UserInformation",
                  "UserType",
                  "VAT",
                  "Video",
                  "VideosTag",
                  "WeekDays",
                  "XML"
                };

            if (langName == "en")
            {
                foreach(var setName in arrayofResourceSet)
                {    
                var result = await RetrieveValueAsync(setName, "en-US");
                    allResources.Add(setName, result["en-US"]);
                }
              
                    return Json( allResources);

            }
            else
            {
                //var result = await RetrieveValueAsync(setName, "ar-JO");
                foreach (var setName in arrayofResourceSet)
                {
                    var result = await RetrieveValueAsync(setName, "ar-JO");
                    allResources.Add(setName, result["ar-JO"]);
                }
                return Json( allResources );
            }
            return Content("{}");
        }


        [HttpGet]
        public ActionResult AccountDSPRequest()
        {
            //if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            //{
            //    return RedirectToAction(Config.DefaultAction, Config.DefaultController);
            //}

            AccountDSPRequestDto model = new AccountDSPRequestDto();
            //if (id.HasValue)
            //{
            //    try
            //    {
            //        model = _userService.GetAccountDSPRequest((int)id);
            //    }
            //    catch (Exception e)
            //    {

            //        throw e;
            //    }

            //}
            var CompanyTypes = _userService.GetCompanyTypes();
            model.CompanyTypes = new List<SelectListItem>();


            model.CompanyTypes.Add(new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") });


            foreach (var item in CompanyTypes)
            {
                model.CompanyTypes.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name.Value, Selected = model.CompanyType == item.ID });
            }


            return View("~/Views/AccountDSPRequest/AccountDSPRequest.cshtml", model);

        }

        [HttpGet]
        public ActionResult AccountDSPRequestSuccess()
        {


            AccountDSPRequestDto model = new AccountDSPRequestDto();

            var CompanyTypes = _userService.GetCompanyTypes();
            model.CompanyTypes = new List<SelectListItem>();


            model.CompanyTypes.Add(new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") });


            foreach (var item in CompanyTypes)
            {
                model.CompanyTypes.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name.Value, Selected = model.CompanyType == item.ID });
            }

            model.Result = true;
            return View("~/Views/AccountDSPRequest/AccountDSPRequest.cshtml", model);

        }


        [HttpPost]
        public ActionResult AccountDSPRequest(AccountDSPRequestDto AccountDSPRequestDto)
        {
            if (ModelState.IsValid)
            {

                _userService.UpdateAccountDSPReqest(AccountDSPRequestDto);



            }
            else
            {
                AddMessages(ResourcesUtilities.GetResource("Exception", "Global"), MessagesType.Error);

            }

            return RedirectToAction("AccountDSPRequestSuccess");
        }


        //[OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "setName", "langName" })]
        public async Task<Dictionary<string, Dictionary<string, string>>> RetrieveValueAsync(string set, string lang)
        {

            var result = await Task.Run(() =>
            {
                Dictionary<string, Dictionary<string, string>> dictionaries = resourceManager.GetResourceSet(set, lang);
                return dictionaries;
            });
            // await Task.Delay(50);
            //  return 42;
            return result;
        }
        public ActionResult ResourcesTest(string setName, string langName)
        {
            if (langName == "en")
            {
                var result = resourceManager.GetResourceSet(setName, "en-US");
                if (result["en-US"] != null)
                    return Json(result["en-US"]);

            }
            else
            {
                var result = resourceManager.GetResourceSet(setName, "ar-JO");

                if (result["ar-JO"] != null)
                    return Json(result["ar-JO"]);
            }
            return Content("{}");
        }
        public string DictionaryToString(Dictionary<string, string> dictionary)
        {
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> keyValues in dictionary)
            {
                dictionaryString += "\'" + keyValues.Key + "\' : \'" + keyValues.Value + "\', ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }


    }
}
