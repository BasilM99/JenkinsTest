using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.Framework.Utilities.EmailsSender;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class MiscController : Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase
    {
        private IMailSender _mailSender;
        private IUserService _userService;
        private IAppSiteService _appSiteService;
        private ILanguageService _languageService;
        public MiscController(IMailSender mailSender, IAppSiteService appSiteService, IUserService userService, ILanguageService langServ )
        {
            this._mailSender = mailSender;
            this._appSiteService = appSiteService;
            this._userService = userService;
              this._languageService = langServ;
        }
        [OutputCache(Duration = 21600, VaryByParam = "q;culture")]
        public ActionResult GetLanguages(string q, string culture)
        {

            /*var results2 = _CampaignService.GetImpressionMetricTargetings(new Domain.Repositories.Campaign.Creative.ImpressionMetricCriteria {AdGroupId= 110899 });

               var results=_CampaignService.GetImpressionMetric();
            var gffgg=_CampaignService.GetImpressionMetricTargeting(11089999);*/
            return Json(ReturnLanguageResult(q, culture), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<LanguageDto> ReturnLanguageResult(string q, string culture)
        {


            var criteria = new LanguageCriteria() { Value = q, Culture = culture };
            var Advertisers = _languageService.GetByQuery(criteria);


            return Advertisers;
        }
        public System.Web.Mvc.ActionResult ThankYou(string resourceKeyName, string url, string title, string view, int? Id, bool login = false)
        {



            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();



            if (!login)
            {

                breadCrumbLinks.Add(new BreadCrumbModel()
                {
                    Text = ResourcesUtilities.GetResource("AppSiteList", "SiteMapLocalizations"),
                    Order = 1,
                    Url = Url.Action("Index", "appsite")
                });

                breadCrumbLinks.Add(
                    new BreadCrumbModel()
                    {
                        Text = Id == null ? "" : _appSiteService.Get((int)Id).Name,// ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
                        Order = 2,
                    });


            }
            else
            {
                breadCrumbLinks.Add(new BreadCrumbModel()
                {
                    Text = ResourcesUtilities.GetResource("Register", "UserInformation"),
                    Order = 1

                });

                ViewBag.HideMenu = true;

            }

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.url = url;
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
         

            ViewBag.ShowMenu = true;

            var values = (from key in Request.QueryString.AllKeys where !key.Equals("resourceKeyName", StringComparison.OrdinalIgnoreCase) select Request.QueryString[key]).ToList();
            string thanksMessage = string.Format(ResourcesUtilities.GetResource(resourceKeyName, "ThanksMessages"), values.ToArray());
            //AddMessages(thanksMessage, MessagesType.Success);
            ViewBag.Mess = thanksMessage;
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
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (ModelState.IsValid)
            {
                string emailTemplate = ResourcesUtilities.GetResource("AgencyRequest", "Emails");

                emailTemplate = emailTemplate.Replace("{address}", agencyRequestDto.Address).Replace("{email}", agencyRequestDto.Email).Replace("{phonenumber}", agencyRequestDto.PhoneNumber).Replace("{message}", agencyRequestDto.Message)
                    .Replace("{firstname}", agencyRequestDto.FirstName).Replace("{secondname}", agencyRequestDto.SecondName).Replace("{company}", agencyRequestDto.Company);

                string email = Config.ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");

                _mailSender.SendEmail("", "", email, email, ResourcesUtilities.GetResource("AgencyRequest", "EmailHeader"), emailTemplate);

                result.Data = new { status = "success" };
                return result;
            }
            else
            {
                result.Data = new { status = "error" };
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
                userId = cookie.Value;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                var result = this._userService.GetUserForOptingInDB(userId);

                return Json(new { status = result.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                userId = this._userService.CreateUserIdForOpting();
                var newcookie = new System.Web.HttpCookie("ADFALCON-UUID", userId);
                //{
                //    Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                //    Secure = FormsAuthentication.RequireSSL,
                //    HttpOnly = true
                //};
                newcookie.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);
                if (!Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
                {
                    newcookie.Domain = Config.CookieDomain;
                }


                Response.Cookies.Add(newcookie);

                return Json(new { status = "True" }, JsonRequestBehavior.AllowGet);
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
                userId = cookie.Value;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                var result = this._userService.GetUserForOptingInDB(userId);

                return Json(new { status = result.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                userId = this._userService.CreateUserIdForOpting();
                var newcookie = new System.Web.HttpCookie("ADFALCON-UUID", userId);
                //{
                //    Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                //    Secure = FormsAuthentication.RequireSSL,
                //    HttpOnly = true
                //};
                newcookie.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(60);
                if (!Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
                {
                    newcookie.Domain = Config.CookieDomain;
                }


                Response.Cookies.Add(newcookie);

                return Json(new { status = "True" }, JsonRequestBehavior.AllowGet);
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
                userId = cookie.Value;
                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                this._userService.UpdateUserIdForOptingInDB(userId, tracking);


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
                userId = cookie.Value;

                var iIndex = userId.IndexOf("-");
                if (iIndex != -1)
                {
                    userId = userId.Substring(0, iIndex);

                }
                this._userService.UpdateUserIdForOptingInDB(userId, tracking);


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
    }
}
