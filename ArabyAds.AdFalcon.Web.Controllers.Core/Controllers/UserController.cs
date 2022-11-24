using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Security;
using ArabyAds.AdFalcon.Business.Domain.Exceptions;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;
using ArabyAds.AdFalcon.Exceptions.Account;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Payment;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Model.Pgw;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.Utilities.EmailsSender;
using ArabyAds.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;
using ArabyAds.AdFalcon.Web.Controllers.Utilities.PaymentGateways;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using Microsoft.AspNetCore.Routing;
using ArabyAds.Framework.Logging;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System.Globalization;
using ArabyAds.AdFalcon.Services;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using System.Configuration;
using System.IO;
using System.Net;

using ArabyAds.AdFalcon.Web.Controllers.Model.AccountManagement;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core.Security;
using System.Text.Json;
using ArabyAds.AdFalcon.Web.Controllers.Core.Model.User;
using System.Threading;
using ArabyAds.AdFalcon.Web.Core.Helper;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
//using WebMatrix.WebData;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    //  [DenyRole(Roles = "AppOps", DenyImpersonationOnly = true)]
    [RequireHttps(Order = 1)]
    public class UserController : ControllerBase
    {
        private SecurityManager _securityProxy;
        private const string GlobalErrors = "GlobalErrors";
        private IAccountService _accountService;
        private IUserService _userService;
        private IMailSender _mailSender;
        private IFundsService _FundsService;
        private IPartyService _partyService;
        private IDocumentTypeService _DocumentTypeService;

        private IFundTransactionService _fundTrns;
        private ILanguageService _languageService;
        private ICountryService _countryService;
        private IAdvertiserService _AdvertiserSearcher;
        private ICountryService countryService;
        public UserController(
           )
        {
            _securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
            _accountService = IoC.Instance.Resolve<IAccountService>();
            _userService = IoC.Instance.Resolve<IUserService>();
            _mailSender = IoC.Instance.Resolve<IMailSender>();
            _FundsService = IoC.Instance.Resolve<IFundsService>();
            _fundTrns = IoC.Instance.Resolve<IFundTransactionService>();
            _languageService = IoC.Instance.Resolve<ILanguageService>();
            _partyService = IoC.Instance.Resolve<IPartyService>();
            _DocumentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();
            _countryService = IoC.Instance.Resolve<ICountryService>();
            _AdvertiserSearcher = IoC.Instance.Resolve<IAdvertiserService>();

            countryService = IoC.Instance.Resolve<ICountryService>();


        }

        const string WEBHDFS_CONTEXT_ROOT = "/webhdfs/v1";

        [RequireHttps(Order = 1)]
        public ActionResult ChangeLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                var culture = CultureInfo.CreateSpecificCulture(getCulture(lang));
                culture.DateTimeFormat.ShortDatePattern = Config.ShortDateFormat;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Framework.OperationContext.Current.CultureCode = culture.Name;
                return Content(culture.Name);
            }
            return Content(string.Empty);
        }
        private string getCulture(string lang)
        {
            lang = lang.ToLower();
            if (lang.Contains("-"))//en-US,en-UK
            {
                lang = lang.Substring(0, 2);
            }
            var returnStr = "en-US";
            switch (lang)
            {
                case "ar":
                    returnStr = "ar-JO";
                    break;
                case "en":
                    returnStr = "en-US";
                    break;
            }
            return returnStr;
        }

        public ActionResult IsAuth()
        {
            if (Framework.OperationContext.Current.CurrentPrincipal != null && !string.IsNullOrEmpty(Framework.OperationContext.Current.CurrentPrincipal.Token))
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>() != null && OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.HasValue)
                {
                    var user = _userService.GetUserByAccount(new UserAccountMessage { AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value, UserId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });
                    //TODO we should set expiry for session and do it in the code level once the user status changed from UI
                    if (user.Block)
                        return StatusCode(600);
                    else
                        return new JsonResult(frontEndUserObject(user));

                }
                else
                    return StatusCode(600);
            }
            else
            {

                return StatusCode(600);
            }


        }

        public Object frontEndUserObject(UserDto userDto)
        {
            var impersonatedAccount = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().ImpersonatedAccount;
            string impersonatedAccountName = "";
            if (impersonatedAccount != null)
            {
                impersonatedAccountName = " (" + impersonatedAccount.FirstName + " " + impersonatedAccount.LastName + ")";

            }

            int NormalAccId = 0;
            int DSPAccId = 0;
            int DataProviderAccId = 0;

            var accounts = ArabyAds.Framework.IoC.Instance.Resolve<ArabyAds.AdFalcon.Services.Interfaces.Services.Account.IAccountService>().GetUserAccountsByEmail(ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().EmailAddress);
            if (accounts != null && accounts.Count() > 1)
            {
                foreach (var item in accounts)
                {
                    if (item.AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP)
                    {
                        DSPAccId = item.AccountId;
                    }
                    else if (item.AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider)
                    {
                        DataProviderAccId = item.AccountId;
                    }
                    else
                    {
                        NormalAccId = item.AccountId;
                    }
                    //ViewData["UserId"] = userDto.Id;

                }
            }

            List<string> SystemRoles = new List<string>();

            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAdmin)
            {
                SystemRoles.Add(@"Administrator");
            }
            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAppOps)
            {
                SystemRoles.Add(@"AppOps");
            }

            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAdOps)
            {
                SystemRoles.Add(@"AdOps");
            }
            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsFinanceManager)
            {
                SystemRoles.Add(@"FinanceManager");
            }
            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAccountManager)
            {
                SystemRoles.Add(@"AccountManager");
            }


            var SystemRolesArray = string.Join(",", SystemRoles.ToArray());
            var tempIsAuthenticated = ArabyAds.Framework.OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated;


            Object tempUserData = new
            {
                AdFalconUserLoggedInUserObject = new {
                    //IsAdmin = false,
                    IsAdminApp = true,
                    //IsPrimary = false, 
                    loadingLang = true,
                    Direction = ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.CurrentLanguage.ToLower() == "en" ? "ltr" : "rtl",
                    impersonatedAccountName = impersonatedAccountName,
                    Email = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().EmailAddress,
                    CurrentUserRole = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountRole,
                    UserRole = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountRole,
                    SystemRoles = SystemRolesArray,
                    themeClass = "theme-dark",
                    NormalAccId = NormalAccId,
                    DSPAccId = DSPAccId,
                    DataProviderAccId = DataProviderAccId,
                    PermissionsList = string.Join(",", ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().Permissions.Length > 0 ? ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().Permissions : new int[] { 0 }),

                    CurrentLanguage = ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.CurrentLanguage.ToLower(),
                    Name = (ArabyAds.Framework.OperationContext.Current.UserInfo
                            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                            ().FirstName + ArabyAds.Framework.OperationContext.Current.UserInfo
                            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                            ().LastName),
                    EmailAddress = (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().EmailAddress),
                    Company = ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().Company,

                    CurrentUserId = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().UserId,
                    UserId = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().UserId,
                    CurrentAccountId = ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().OriginalAccountId,
                    AccountId = ArabyAds.Framework.OperationContext.Current.UserInfo
                            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                            ().OriginalAccountId,
                    CurrentLoggedAccountId = ArabyAds.Framework.OperationContext.Current.UserInfo
                         <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                         ().AccountId,
                    DefaultRedirect = "/audience-list",
                    firstName = ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().FirstName,
                    LastName = ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().LastName,
                    ImpersonatedAccountId = ArabyAds.Framework.OperationContext.Current.UserInfo
                     <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                     ().ImpersonatedAccount != null ? ArabyAds.Framework.OperationContext.Current.UserInfo
                     <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                     ().ImpersonatedAccount.AccountId : 0,
                    IsPrimary = (ArabyAds.Framework.OperationContext.Current.UserInfo
                            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                            ().IsPrimaryUser == true ? true : false),
                    IsAdmin = (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAdOpsAdmin == true ? true : false),
                },
                IsAuthenticated = (tempIsAuthenticated == true ? true : false),
                TopMenu = (tempIsAuthenticated == true ? JsonSerializer.Serialize(GetTopBarDataValue()) : ""),
                SideMenu = (tempIsAuthenticated == true ? JsonSerializer.Serialize(GetSideBarDataValue()) : ""),
                UserSettingsMenu = (tempIsAuthenticated == true ? JsonSerializer.Serialize(GetUserSettingsBarDataValue()) : ""),
            };

            return tempUserData;
        }
        //public ActionResult HelloAnas()
        //{



        //    getdeegaion("", @"D:\hadoob\OPENX3.PNG");



        //    return null;
        //}


        //public static void getdeegaion(string sourcePath, string targetPath)
        //{

        //    var dowloadPath = "http://hd01.iadfalcon.com:50070/" + WEBHDFS_CONTEXT_ROOT + sourcePath + "?op=GETDELEGATIONTOKEN";
        //    var downloadPath2 = "http://hd01.iadfalcon.com:50070/webhdfs/v1/dw/adfalcon/data-providers/headers/impressions-log-header.csv?op=OPEN";

        //    long startBytes = 0;

        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(downloadPath2);






        //    //Kerberos.NET.KerberosAuthenticator gg = new Kerberos.NET.KerberosAuthenticator(new Kerberos.NET.Crypto.KeyTable(bytes));


        //    //var c2 = gg.Authenticate(File.ReadAllBytes(@"d:\test\kk5cache"));
        //    //var c = gg.Authenticate(Encoding.ASCII.GetBytes("hadoopadmin@HDP.NET" + ":" + "P@ssw0rd112233")).Result;
        //    var cc = new CredentialCache();
        //    cc.Add(
        //        new Uri("http://hd01.iadfalcon.com:50070/"),
        //        "NEGOTIATE", //if we don't set it to "Kerberos" we get error 407 with ---> the function requested is not supported.
        //        new NetworkCredential("hdfs-reader-iadfalconcluster", "", ""));
        //    req.Credentials = cc;
        //    req.PreAuthenticate = true;
        //    req.UnsafeAuthenticatedConnectionSharing = true;

        //    req.AllowAutoRedirect = true;
        //    HttpWebResponse resp = null;
        //    try
        //    {
        //        Gss.InitializeAndOverrideApi();
        //        resp = (HttpWebResponse)req.GetResponse();
        //    }
        //    finally
        //    {
        //        Gss.TerminateAndRemoveOverride();

        //    }
        //    // BinaryReader sr = new BinaryReader(resp.GetResponseStream());
        //    using (Stream output = System.IO.File.OpenWrite(@"D:\hadoob\OPENX3.PNG"))
        //    using (Stream input = resp.GetResponseStream())
        //    {
        //        input.CopyTo(output);
        //    }

        //    int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);
        //    int i;


        //}
        public PartialViewResult PublicInfo()
        {
            return PartialView("PublicUserInformation");
        }
        [RequireHttps(Order = 1)]
        public PartialViewResult PublicInfoHttps()
        {
            return PartialView("PublicUserInformation");
        }

        public ActionResult UserAgreement(string returnUrl)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("UserAgreement", "SiteMapLocalizations"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View();
        }
        [HttpPost]
        public ActionResult UserAgreement(string returnUrl, string dummy)
        {

            /*if (!string.IsNullOrWhiteSpace(Request.Form["Agree"]))
            {
                //user agreed on user agreement
              


                //  OperationContext.Current.UserInfo<AdFalconUserInfo>(userInof)
            }*/
            _userService.UpdateAgreement();
            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    return RedirectToAction("index", "dashboard", new { charttype = "ad" });
            //}
           return Json(true);
        }

        public ActionResult GetVATAmount(int Amount)
        {


            var result = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().VATValue * Amount;



            return Content(result.ToString("F2"));



        }
        [CustomAuthorize]
        public ActionResult DSPUserAgreement(string returnUrl)
        {
            if (!isDSP())
            {
                throw new UnauthorizedAccessException();
            }

            return View();
            // return null;
        }
        [CustomAuthorize]
        [HttpPost]
        public ActionResult DSPUserAgreement(string returnUrl, string dummy)
        {
            if (!isDSP())
            {
                throw new UnauthorizedAccessException();
            }
            
                //user agreed on user agreement
                _userService.UpdateAgreement();
            
            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    return RedirectToAction("index", "dashboard", new { charttype = "ad" });
            //}
            return Json(true);
        }

        #region Login
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            return RedirectToAction("PublicInfo");
        }

        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult LogoutFromClient()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false).GetAwaiter().GetResult();
            return Redirect("http://" + JsonConfigurationManager.AppSettings["AdFalconWebReactnonHttps"].ToString());

        }

        [RequireHttps(Order = 1)]
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult LogoutHttps()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false).GetAwaiter().GetResult();

            return RedirectToAction("PublicInfoHttps");
        }


        [RequireHttps(Order = 1)]
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult Login(string method)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Login", "SiteMapLocalizations"),
                Order = 1,
                Url = Url.Action("login", "user")
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (!string.IsNullOrEmpty(method) && method.ToLower() == "logout" && OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                HttpContext.SignOutAsync().ConfigureAwait(false);
                return Redirect(Request.GetDisplayUrl());
            }
            if (Request.Cookies["rem"] != null)
            {
                ViewData["rememberMe"] = Request.Cookies["rem"];
            }
            if (!string.IsNullOrEmpty(method) && method.ToLower() == "reset")
            {
                ModelState.AddModelError("NoUserNameError",
                                              ResourcesUtilities.GetResource("InvalidToken", "ForgetPassword"));
            }
            ViewBag.HideMenu = true;
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
            }
            // TempData.Add("fsdf","fdsff");
            return View();
        }


        [HttpPost]
        [RequireHttps(Order = 1)]
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult Login([FromBody] LoginInfo loginInfo)
        {


            if (ModelState.IsValid)
            {
                AuthenticateResponse response;
                string userName = loginInfo.Username.Trim();

                response = _securityProxy.AuthenticateUser(userName, loginInfo.Password).Result;

                if (response.Status == AuthenticateStatus.Success)
                {
                    var isUserBlocked = _userService.IsUserBlockedByEmail(userName).Value;

                    if (isUserBlocked)
                    {
                        string errormessage1 = ResourcesUtilities.GetResource("UserNameBlocked", GlobalErrors);
                        //ModelState.AddModelError("LogIn", errormessage1);
                        ViewBag.HideMenu = true;
                        AddErrorMsgs(errormessage1);
                        return Json(ResourcesUtilities.GetResource("Login", "Titles"), ResponseStatus.businessException);
                    }
                    LoginUser(userName, response.Principal.Token);

                    // UserDomainManager geteuser = new UserDomainManager(null);

                    UserDto user_info = _userService.GetUserByEmail(new CheckUserEmailRequest { EmailAddress = userName, CheckPendingEmail = true });

                    var lang = _languageService.GetAll().Where(x => x.ID == user_info.Language).FirstOrDefault();
                    RouteData.Values["language"] = lang.Code;

                    if (loginInfo.rememberMe)
                    {
                        CookieOptions rememberMeCookie = new CookieOptions
                        {
                            Expires = Framework.Utilities.Environment.GetServerTime().AddDays(30)
                        };

                        if (!Request.GetDisplayUrl().ToLower().Contains("localhost"))
                        {
                            rememberMeCookie.Domain = Config.CookieDomain;
                        }

                        Response.Cookies.Append("rem", loginInfo.Username, rememberMeCookie);
                    }
                    else
                    {
                        if (Request.Cookies["rem"] != null)
                        {
                            Response.Cookies.Delete("rem");/*.Expires = Framework.Utilities.Environment.GetServerTime().AddDays(-30);*/
                        }
                    }

                    if (_accountService.GetUserAccountsCount(new ValueMessageWrapper<int> { Value = user_info.Id }).Value > 1)
                    {
                        var accId = _userService.getAccountUser().Value;

                        if (accId > 0)
                        {
                            BuildAdFalconUser(accId, user_info.Id, true);
                            // return RedirectToAction("SwitchAccount", new { returnUrl = returnUrl });
                        }
                        //to be removed
                        else
                        {
                            //return RedirectToAction("SwitchAccount", new { returnUrl = returnUrl });
                            var accId2 = _accountService.GetFirstUserAccountId(new ValueMessageWrapper<int> { Value = user_info.Id }).Value;
                            BuildAdFalconUser(accId2, user_info.Id, true);
                        }

                        return ContinueLogin(loginInfo.returnUrl);
                    }
                    else
                    {

                        _userService.InsertLastLoginDateAuditTrial(loginInfo.Username);


                        return ContinueLogin(loginInfo.returnUrl);
                    }
                }
                else
                {
                    string errormessage = string.Empty;
                    if (response.Status == AuthenticateStatus.Error)
                    {
                        errormessage = ResourcesUtilities.GetResource("UsernamePasswordNotMatch", GlobalErrors);
                    }
                    else
                    {
                        string pendingEmailAddress = _userService.GetPendingEmailAddress(userName);

                        if (string.IsNullOrEmpty(pendingEmailAddress))
                        {
                            errormessage = ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = loginInfo.Username }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString()));
                        }
                        else
                        {
                            errormessage = ResourcesUtilities.GetResource("ChangedEmailIsntActivated", GlobalErrors).Replace("[pendingemail]", pendingEmailAddress);
                        }
                    }

                    // ModelState.AddModelError("LogIn", errormessage);
                    ViewBag.HideMenu = true;

                    AddErrorMsgs(errormessage);
                    return Json(ResourcesUtilities.GetResource("Login", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(loginInfo, ResourcesUtilities.GetResource("Login", "Titles"), ResponseStatus.success);
        }

        public ActionResult ContinueLogin(string returnUrl)
        {
            //check user agreement version
            var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            if (user.UserAgreementVersion != Config.UserAgreementVersion && user.AccountRole != (int)AccountRole.DSP)
            {

                return Json(new { Status = ResponseStatus.redirect, url = Url.Action("UserAgreement", "User", new { returnUrl = returnUrl }) });
                // return RedirectToAction("UserAgreement", "User", new { returnUrl = returnUrl });
            }
            else if (user.UserAgreementVersion != Config.DSPUserAgreementVersion && user.AccountRole == (int)AccountRole.DSP)
            {
                return Json(new { Status = ResponseStatus.redirect, url = Url.Action("UserAgreement", "User", new { returnUrl = returnUrl }) });

                //return RedirectToAction("UserAgreement", "User", new { returnUrl = returnUrl });

            }
            else if (user.AccountRole == (int)AccountRole.DataProvider)
            {
                //return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });
                return Json(new { Status = ResponseStatus.redirect, url = Url.Action("index", "dashboard", new { charttype = "lmpressionlog" }) });

            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    //return RedirectToAction("index", "dashboard", new { charttype = "ad" });
                    return Json(new { Status = ResponseStatus.redirect, url = Url.Action("index", "dashboard", new { charttype = "ad" }) });

                }
                else
                {
                    //return Redirect(returnUrl);
                    return Json(new { Status = ResponseStatus.redirect, url = returnUrl });

                }
            }

        }

        public ActionResult Signup()
        {
            return RedirectToAction("Register");
        }
        #endregion

        #region invitation
        protected InvitationFilter getDefualtFilter()
        {
            InvitationFilter filter = new InvitationFilter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            filter.EmailAddress = string.IsNullOrWhiteSpace(Request.Form["Email"]) ? null : Request.Form["Email"].ToString();
            filter.Type = string.IsNullOrWhiteSpace(Request.Form["TypeLog"]) ? 0 : Convert.ToInt32(Request.Form["TypeLog"]);
            return filter;
        }
        protected AccountInvitationCriteria GetCriteria(InvitationFilter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var criteria = new AccountInvitationCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                EmailAddress = filter.EmailAddress != "" ? filter.EmailAddress : null
            };
            return criteria;
        }



        protected virtual InvitationListDto GetQueryResult(InvitationFilter filter)
        {
            var criteria = GetCriteria(filter);
            var result = _userService.InvitationQueryByCratiria(criteria);
            return result;
        }
        protected virtual InvitationViewModel LoadData(InvitationFilter filter)
        {
            var result = GetQueryResult(filter);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            // create the actions

            //var actions = GetCampaignAction();
            //List<Action> toolTips = GetCampaignTooltip();
            //load the statues 
            //var statues = _campaignStatusService.GetAll();
            //var statuesDropDown = GetSelectList();
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.invitationcode = string.Empty;
                }
            }

            return new InvitationViewModel()
            {
                Items = items,
                //TopActions = actions,
                //BelowAction = actions,
                //ToolTips = toolTips
                //, Statuses = statuesDropDown 
            };
        }



        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public virtual ActionResult Invitation()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Invitations","Invite"),
                                                          Order = 1,
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            // ViewBag.ShowMenu = true;
            #endregion
            return View(LoadData(null));
        }
        [GridAction(EnableCustomBinding = true)]

        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public virtual ActionResult _Invitation()
        {

            var result = GetQueryResult(null);
            ViewData["total"] = result.TotalCount;
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public string GetInviteEmail(string invitationcode)
        {
            string emailTemplate = ResourcesUtilities.GetResource("AccountInvitation", "Emails");
            emailTemplate = emailTemplate.Replace("[url]", "https://" + Config.PublicHostName + "/en/User/Register?id=" + invitationcode);
            emailTemplate = emailTemplate.Replace("[account]", OperationContext.Current.UserInfo<AdFalconUserInfo>().FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().LastName);
            emailTemplate = emailTemplate.Replace("[Year]", ArabyAds.Framework.Utilities.Environment.GetServerTime().Year.ToString());
            return emailTemplate;
        }

        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public ActionResult invite(string email, string Ids, UserType? userType)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    throw new Exception("empty string");
                string invitationcode;
                var resultobj = _userService.invite(new InviteRequest { Email = email, UserType = userType != null ? userType.Value : UserType.Normal, IdAdvs = Ids });

                bool result = resultobj.Success;

                invitationcode = resultobj.Invitationcode;


                if (result)
                {

                    string subject = ResourcesUtilities.GetResource("AccountInvitation", "Invite");
                    _mailSender.SendEmail("", "", "", email, subject, GetInviteEmail(invitationcode), true, "");


                }


                AddSuccessfullyMsg("Invited", "Global");
                return Json(result, ResourcesUtilities.GetResource("UserManagement", "Menu"), ResponseStatus.success);

                //return Json(new { status = "success", result = result });

            }
            catch (Exception e)
            {

                return Json(new { status = "businessException", Message = e.Message, result = false });

            }

        }
        [CustomAuthorize]
        [RequireHttps(Order = 1)]
        [DenyNonPrimaryRole]

        public ActionResult MyUsers()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("MyUsers","User"),
                                                          Order = 1,
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
                ,
                ToolTips = getMyUsersTips()
            };

            ViewBag.isAdmin = true;

            //ViewData["AccountId"] = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            return View(model);

        }

        protected virtual List<Model.Action> getMyUsersTips()
        {


            var toolTips = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
            { };



            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                toolTips.Add(new Model.Action()
                {
                    Code = "3",
                    DisplayText = ResourcesUtilities.GetResource("Advertiser", "Menu"),
                    ClassName = "grid-tool-tip-trail",
                    ActionName = "AccountAdvertisers",
                    ControllerName = "Campaign"
                });
            }



            return toolTips;
        }
        #endregion

        public ActionResult Resend(string email)
        {
            //#region BreadCrumb

            //List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            //breadCrumbLinks.Add(new BreadCrumbModel()
            //{
            //    Text = ResourcesUtilities.GetResource("ResendEmailActivation", "SiteMapLocalizations"),
            //    Order = 1
            //});

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion

            UserDto userInfo = _userService.GetUserByEmail(new CheckUserEmailRequest { EmailAddress = email, CheckPendingEmail = false });

            if (userInfo.ActivationCode != "1")
            {
                _mailSender.SendEmail("", "", userInfo.EmailAddress, userInfo.EmailAddress, ResourcesUtilities.GetResource("Registration", "EmailHeader"), GetActivationEmail(userInfo.ActivationCode));
                AddMessages(ResourcesUtilities.GetResource("SuccessResend", "Resend"), MessagesType.Success);

            }
            else
            {
                AddMessages(ResourcesUtilities.GetResource("AlreadyActivated", "Resend"), MessagesType.Warning);
            }
            ViewBag.ActivationCode = userInfo.ActivationCode;
            return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/user/resend");
        }

        public ActionResult ForgetPassword()
        {
            ViewBag.HideMenu = true;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                                        {
                                                            new BreadCrumbModel()
                                                                {
                                                                    Text =ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"),
                                                                    Order = 1,
                                                                    Url = Url.Action("forgetpassword", "user")
                                                                }
                                                        };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
            }

            return View();
        }

        public virtual string GenerateCouponSpecialChracter(int length)
        {
            Random random = new Random();
            string characters = "!@#$%^&*";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
        public virtual string GenerateCouponUPPER(int length)
        {
            Random random = new Random();
            string characters = "QWERTYUIOPLKJHGFDSAZXCVBNM";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
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

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            //#region BreadCrumb

            //List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            //breadCrumbLinks.Add(new BreadCrumbModel()
            //{
            //    Text = ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"),
            //    Order = 1,
            //    Url = Url.Action("forgetpassword", "user")
            //});

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion

            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
            }

            if (ModelState.IsValid)
            {
                //string newPassword = Guid.NewGuid().ToString().Substring(0, 4) + (new Random(100).Next(1000));
                //newPassword = newPassword + GenerateCouponSpecialChracter(1);
                //newPassword = newPassword + GenerateCouponUPPER(1);

                int tokenExpirationInMinutesFromNow = 1440; // 24 hour to toke Expiration
                string token = "";// WebSecurity.GeneratePasswordResetToken(email,  tokenExpirationInMinutesFromNow );

                // generate unique token
                DateTime currentDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                Guid obj = Guid.NewGuid();
                string ticks = currentDate.Ticks.ToString();
                token = obj + "_" + ticks;

                var lnkHref = "<a href='https://"+JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/User/ResetPassword/" + token+""+ "'>" + ResourcesUtilities.GetResource("ResetPassword", "ForgetPassword") + "</a>";
                string emailTemplate = ResourcesUtilities.GetResource("ResetPasswordEmail", "ForgetPassword");
                emailTemplate = emailTemplate.Replace("[link]", lnkHref);

                try
                {
                    bool resetValue = _userService.SaveUserToken(new SaveUserTokenRequest { Email = email, Token = token }).Value;
                    //bool resetValue = _userService.SaveUserToken(email, newPassword);
                    if (!resetValue)
                    {


                        AddErrorMsgs(ResourcesUtilities.GetResource("NoUserNameError", "ForgetPassword"));
                        return Json(ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"), ResponseStatus.businessException);

                    }


                    else
                    {
                        _mailSender.SendEmail("", "", email, email,
                                              ResourcesUtilities.GetResource("ForgetPassword", "EmailHeader"),
                                              emailTemplate);

                        AddSuccessfullyMsgMs(ResourcesUtilities.GetResource("SuccessForgetPassword2", "ForgetPassword"));


                    }
                }
                catch (NotActivatedUserException exception)
                {
                    // ModelState.AddModelError("LogIn", ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = email })));
                    AddErrorMsgs(ResourcesUtilities.GetResource("UserIsntActivatedReset", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = email }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString())));
                    return Json(ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"), ResponseStatus.businessException);
                }


            }

            return Json(ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"), ResponseStatus.success);
        }



        public ActionResult ResetPassword(string token)
        {
            ViewBag.HideMenu = true;
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                                        {
                                                            new BreadCrumbModel()
                                                                {
                                                                    Text =ResourcesUtilities.GetResource("ResetPassword", "ForgetPassword"),
                                                                    Order = 1,
                                                                    Url = Url.Action("forgetpassword", "user")
                                                                }
                                                        };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            ResetPasswordInfo oResetPasswordInfo = new ResetPasswordInfo();

            try
            {
                bool resetValue = _userService.CheckUserToken(token).Value;
                //bool resetValue = _userService.SaveUserToken(email, newPassword);
                if (!resetValue)
                {
                    ModelState.AddModelError("NoUserNameError",
                                             ResourcesUtilities.GetResource("InvalidToken", "ForgetPassword"));
                    return RedirectToAction("Login", new { method = "reset" });
                }
                else
                {
                    oResetPasswordInfo.Token = token;
                }
            }
            catch (NotActivatedUserException exception)
            {
                ModelState.AddModelError("LogIn", ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = "email" }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString())));
                return RedirectToAction("Login", new { method = "reset" });
            }

            return View(oResetPasswordInfo);
        }

        [HttpPost]
        public ActionResult ResetPassword([FromBody] ResetPasswordInfo oResetPasswordInfo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    bool resetValue = _userService.ResetUserPasswordByToken(new ResetUserPasswordByTokenRequest { Token = oResetPasswordInfo.Token, NewPassword = oResetPasswordInfo.Password }).Value;
                    if (!resetValue)
                    {
                        AddErrorMsgs(ResourcesUtilities.GetResource("NoUserNameError", "ForgetPassword"));
                        return Json(ResourcesUtilities.GetResource("ResetPassword", "ForgetPassword"), ResponseStatus.businessException);
                    }
                    else
                    {
                        AddSuccessfullyMsgMs(ResourcesUtilities.GetResource("SuccessPasswordChange", "ForgetPassword"));

                    }
                }
                catch (NotActivatedUserException exception)
                {
                    AddErrorMsgs(ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user",null, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString())));
                    return Json(ResourcesUtilities.GetResource("ResetPassword", "ForgetPassword"), ResponseStatus.businessException);


                }


            }
            //return RedirectToAction("Login");

            return Json(ResourcesUtilities.GetResource("ResetPassword", "ForgetPassword"), ResponseStatus.success);

        }

        private int GetCountryByCode(string Code)
        {
            var cuntr = _countryService.GetAll().Where(M => M.Code == Code).SingleOrDefault();
            if (cuntr != null)
                return cuntr.ID;
            else
                return 0;
        }

        [RequireHttps]
        public ActionResult GetUserAgreement()
        {
            var UserAgreementEffectiveDateStr = @Config.UserAgreementEffectiveDate.Value.ToShortDateString();
            var DSPUserAgreementEffectiveDateStr = @Config.DSPUserAgreementEffectiveDate.Value.ToShortDateString();
            return Json(new { UserAgreementEffectiveDateStr = UserAgreementEffectiveDateStr, DSPUserAgreementEffectiveDateStr = DSPUserAgreementEffectiveDateStr});

        }
        [HttpPost]
        [RequireHttps]
        public ActionResult GetRegister([FromBody] RegisterUserRequestModel model)
        {
            var Invitationcode = string.Empty;
            var AcceptTermsANdCondition = false;
            var email = "";
            var Title = "";
            var CompanyName = "";
            var FirstName = string.Empty;
            var LastName = string.Empty;
            UserDto account = new UserDto();
            int? Country = GetCountryByIpAddres();
            account.Country = Country != null ? (int)Country : 0;
             Country = account.Country;
            string Title2 = ResourcesUtilities.GetResource("Register", "Titles");
            if (!string.IsNullOrEmpty(model.id))
            {
                var invitation = _userService.GetInvitation(model.id);
                string email2 = invitation.EmailAddress;
                string CompanyNName = invitation.CompanyName;
                account.Company = CompanyNName;
                Invitationcode = model.id;
                if (!string.IsNullOrEmpty(email))
                {
                    email= email2;
                    CompanyName = CompanyNName;
                    if (_userService.InvitationAcceptedCountByCode(model.id).Value == 0 && _userService.CheckInvitationAlreadyRegistred(new CheckInvitationAlreadyRegistredRequest { Email = email, Invitation = model.id }).Value)
                    {
                        UserDto userDto = new UserDto();
                        userDto.IPAddress = GetIPAddress();
                        userDto.EmailAddress = email;
                        userDto.Invitationcode = model.id;
                        // userDto.Company = CompanyNName;
                        userDto.AlreadyReg = true;
                        userDto = _accountService.CreateUserAccount(userDto);


                        InvitationAccepted(model.id);
                        //return RedirectToAction("Activation", new { hashing = "Invited" });

                    }
                }
                else
                {
                    //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                    AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                    return Json(Title, ResponseStatus.businessException);
                }

            }
            if (!string.IsNullOrEmpty(model.requestCode))
            {
                account = _userService.GetAccountDSPRequestByRequestCode(model.requestCode);
                Title2 = ResourcesUtilities.GetResource("CompleteRegistration", "UserInformation");
                var requestCode = model.requestCode;
                if (account != null && !string.IsNullOrEmpty(account.EmailAddress))
                {
                    account.IsAllowNotifications = true;

                    email = account.EmailAddress;

                    //ViewBag.HideMenu = true;
                    Title = Title2;
                    CompanyName = account.Company;
                    FirstName = account.FirstName;
                    LastName = account.LastName;
                    AcceptTermsANdCondition = false;
                    //return View(account);
                }
                else
                {
                    //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                    AcceptTermsANdCondition = false;
                    AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                    return Json(Title, ResponseStatus.businessException);
                    //ViewBag.HideMenu = true;
                    //
                }

            }
            Title = Title2;
            //if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            //{
            //    return RedirectToAction(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
            //}
           
            var urladv = "http://" + JsonConfigurationManager.AppSettings["AdFalconWebReactnonHttps"].ToString()+ "/en/terms.html";
            var urlpub = "http://" + JsonConfigurationManager.AppSettings["AdFalconWebReactnonHttps"].ToString() + "/en/pubterms.html";
            return Json(new { urladv = urladv, urlpub = urlpub, account = account , requestCode= model.requestCode, Country= Country, email= email, FirstName= FirstName, CompanyName= CompanyName, LastName = LastName, Invitationcode = Invitationcode, AcceptTermsANdCondition= AcceptTermsANdCondition, Title = Title });
        }

        [RequireHttps]
        public ActionResult Register(string id = null, string requestCode = null)
        {
            //return View();
            ViewData["Invitationcode"] = string.Empty;
            ViewData["AcceptTermsANdCondition"] = false;
            ViewData["email"] ="";
            ViewData["CompanyName"] = "";
            ViewData["FirstName"] = string.Empty;
            ViewData["LastName"] = string.Empty;
            UserDto account = new UserDto();
            int? Country = GetCountryByIpAddres();
            account.Country = Country != null ? (int)Country : 0;
            ViewData["Country"] = account.Country;
            string Title = ResourcesUtilities.GetResource("Register", "Titles");
            if (!string.IsNullOrEmpty(id))
            {
                var invitation = _userService.GetInvitation(id);
                string email = invitation.EmailAddress;
                string CompanyNName = invitation.CompanyName;
                account.Company = CompanyNName;
                ViewData["Invitationcode"] = id;
                if (!string.IsNullOrEmpty(email))
                {
                    ViewData["email"] = email;
                    ViewData["CompanyName"] = CompanyNName;
                    if (_userService.InvitationAcceptedCountByCode(id).Value == 0 && _userService.CheckInvitationAlreadyRegistred(new CheckInvitationAlreadyRegistredRequest { Email = email, Invitation = id }).Value)
                    {
                        UserDto userDto = new UserDto();
                        userDto.IPAddress = GetIPAddress();
                        userDto.EmailAddress = email;
                        userDto.Invitationcode = id;
                        // userDto.Company = CompanyNName;
                        userDto.AlreadyReg = true;
                        userDto = _accountService.CreateUserAccount(userDto);


                        InvitationAccepted(id);
                        return RedirectToAction("Activation", new { hashing = "Invited" });

                    }
                }
                else
                {
                    //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                    AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                    return Json(Title, ResponseStatus.businessException);
                }

            }
            if (!string.IsNullOrEmpty(requestCode))
            {
                account = _userService.GetAccountDSPRequestByRequestCode(requestCode);
                Title = ResourcesUtilities.GetResource("CompleteRegistration", "UserInformation");
                ViewData["requestCode"] = requestCode;
                if (account != null && !string.IsNullOrEmpty(account.EmailAddress))
                {
                    account.IsAllowNotifications = true;

                    ViewData["email"] = account.EmailAddress;

                    ViewBag.HideMenu = true;
                    ViewData["Title"] = Title;
                    ViewData["CompanyName"] = account.Company;
                    ViewData["FirstName"] = account.FirstName;
                    ViewData["LastName"] = account.LastName;
                    ViewData["AcceptTermsANdCondition"] = false;
                    return View(account);
                }
                else
                {
                    //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                    ViewData["AcceptTermsANdCondition"] = false;
                    AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                  
                    return Json( Title, ResponseStatus.businessException);
                    //ViewBag.HideMenu = true;
                    //
                }

            }
            ViewData["Title"] = Title;
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
            }

            return View(account);
        }

        private bool isDSP()
        {
            var impersonatedAccount = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().ImpersonatedAccount;
            var Account = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>();

            if (impersonatedAccount != null)
            {
                return _accountService.GetAccountRole(new ValueMessageWrapper<int> { Value = (int)impersonatedAccount.AccountId }).Value == (int)AccountRole.DSP;
            }
            else
            {
                return Account.AccountRole == (int)AccountRole.DSP;

            }
        }
        private int? GetCountryByIpAddres()
        {
            var ipaddres = GetIPAddress();

            if (!string.IsNullOrEmpty(ipaddres) && ipaddres != "::1")
            {
                IPDetectionHelper ipHelper = new IPDetectionHelper();
                var locInfo = ipHelper.Detect(ipaddres);

                var countryOb = GetCountryByCode(locInfo.CountryCode_Alpha2);
                return countryOb;
            }
            return null;
        }
        [RequireHttps]
        public ActionResult GetCountryByIpAddres(string Ip)
        {

            if (!string.IsNullOrEmpty(Ip) && Ip != "::1")
            {
                IPDetectionHelper ipHelper = new IPDetectionHelper();
                var locInfo = ipHelper.Detect(Ip);

                var countryOb = GetCountryByCode(locInfo.CountryCode_Alpha2);
                return Json(new { Success = true, Message = countryOb });
            }
            return Json(new { Success = false, Message = 0 });
        }

        [HttpPost]
        [RequireHttps]
        public ActionResult Register([FromBody]UserDto userDto)
        {
            string linkRedirect = string.Empty;

            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                 linkRedirect =Url.Action(Config.DefaultActionAccountAdvertiser, Config.DefaultController);
                return Json(linkRedirect, "", ResponseStatus.redirect);

            }
            string Title = ResourcesUtilities.GetResource("CompleteRegistration", "UserInformation");
            /*if (!string.IsNullOrEmpty(userDto.requestCode))
            {
                ViewData["requestCode"] = userDto.requestCode;
                ViewData["email"] = userDto.EmailAddress;
            }*/
          
            if (ModelState.IsValid )
            {
                try
                {
                    string Invitationcode = userDto.Invitationcode;
                    if (!string.IsNullOrEmpty(userDto.requestCode))
                    {

                        UserDto account = _userService.GetAccountDSPRequestByRequestCode(userDto.requestCode);

                        if (account != null && !string.IsNullOrEmpty(account.EmailAddress))
                        {
                            userDto.IsAccountDSP = true;
                        }
                        else
                        {
                            //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                            AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                            //ViewData["Title"] = Title;
                            // ViewBag.HideMenu = true;
                            // ViewData["AcceptTermsANdCondition"] = true;
                            return Json(userDto, Title, ResponseStatus.businessException);
                           /// return View(userDto);
                        }


                    }
                    userDto.IPAddress = GetIPAddress();
                    userDto = _accountService.CreateUserAccount(userDto);

                    if (!string.IsNullOrEmpty(Invitationcode))
                    {
                        InvitationAccepted(Invitationcode);
                         linkRedirect= Url.Action("Activation", new { hashing = "Invited" });
                        return Json(linkRedirect, "", ResponseStatus.redirect);

                    }
                    if (!string.IsNullOrEmpty(userDto.ActivationCode))
                    {
                        _mailSender.SendEmail("", "", userDto.EmailAddress, userDto.EmailAddress, ResourcesUtilities.GetResource("Registration", "EmailHeader"), GetActivationEmail(userDto.ActivationCode));

                         

                         linkRedirect = Url.Action("thankyou", "misc");
                        return Json(linkRedirect, "", ResponseStatus.redirect);
                    }


                     
                     linkRedirect = Url.Action("Activation", new { hashing = "Invited" });
                    return Json(linkRedirect, "", ResponseStatus.redirect);

                }
                catch (UserEmailAlreadyExistsException)
                {
                    //ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                    AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                    //ViewData["Title"] = Title;
                    //ViewBag.HideMenu = true;
                    //ViewData["AcceptTermsANdCondition"] = true;
                    return Json(userDto, Title, ResponseStatus.businessException);

                }
            }
           

            return Json(userDto, Title, ResponseStatus.success);
        }

        [RequireHttps]

        public ActionResult generateBuyerId()
        {
            int? buyerid;

            try
            {
                buyerid = _userService.GetAccountBuyerCounter().Value;

            }
            catch (Exception e)
            {

                throw e;
                return Json(ResourcesUtilities.GetResource("Generate", "Account"), ResponseStatus.businessException);
            }

            AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("Generate", "Account"));
            return Json(new { value = (int)buyerid }, "buyer", ResponseStatus.success);
        }

        private void InvitationAccepted(string Invitationcode)
        {
            try
            {
                var Invitation = _userService.GetInvitation(Invitationcode);
                string subject = ResourcesUtilities.GetResource("InvitationAcceptedSubject", "Emails");
                string email = _accountService.GetAccountEmailAddress(new ValueMessageWrapper<int> { Value = Invitation.accountid });
                _mailSender.SendEmail("", "", email, email, subject, GetInvitationAcceptedEmail(Invitation.EmailAddress), true, "");

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        [RequireHttps]
        public ActionResult RedirectRegisterDSP(string requestCode)
        {
           
            return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/en/User/Register?requestCode=" + requestCode + "");
        }
        [RequireHttps]
        public ActionResult RedirectRegisterInvitation(string id)
        {

            return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/en/User/Register?id=" + id + "");
        }


        [RequireHttps]
        public ActionResult Activation(string activationCode, string hashing, string type)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == false && string.IsNullOrEmpty(type))
            {
                try
                {
                    UserDto userDtoInfo = null;
                    if (!string.IsNullOrEmpty(activationCode))
                        userDtoInfo = _userService.ActivateUser(activationCode);

                    //if (userDtoInfo != null || (string.IsNullOrEmpty(activationCode) && hashing == "Invited"))
                    //{

                        /*AddMessages(ResourcesUtilities.GetResource("Content", "Activation").Replace("[url]",
                                                                                                    string.Format(
                                                                                                        "<a href='{0}'>{1}</a>",
                                                                                                        Url.Action(
                                                                                                            "login",
                                                                                                            "user"),
                                                                                                        ResourcesUtilities
                                                                                                            .GetResource
                                                                                                            (
                                                                                                                "ClickHere",
                                                                                                                "Global"))),
                                    MessagesType.Success);*/

                        //return View();
                    //}
                }
                catch (DataNotFoundException)
                {
                    return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/user/login");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(type))
                {
                    ChangeEmailDto changeEmailDto = new ChangeEmailDto();
                    changeEmailDto.ActivationCode = activationCode;
                    changeEmailDto.Hashing = hashing;

                    _userService.ChangeEmail(changeEmailDto);
                }
            }

            return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/" + Config.DefaultController + "/" + Config.DefaultActionAccountAdvertiser + "");
        }

        [CustomAuthorize]
        [RequireHttps]
        public ActionResult Edit(int? id)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                                        {
                                                            new BreadCrumbModel()
                                                                {
                                                                    Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                                    Order = 1
                                                                }
                                                        };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

                        


            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                UserDto userInfo = null;
                userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                ViewData.Model = userInfo;
            }
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true && id.HasValue)
            {
                UserDto userInfo = null;
                userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = id.Value });// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                userInfo.MyUsers = true;
                ViewData.Model = userInfo;

                var userInfoOb = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                userInfoOb.SubUserId = id.Value;
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfoOb);
            }
            return View();
        }

        [CustomAuthorize]
        [RequireHttps]
        public ActionResult GetEditData(int? id)
        {



            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                UserDto userInfo = null;
                userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                ViewData.Model = userInfo;
            }
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true && id.HasValue)
            {
                UserDto userInfo = null;
                userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = id.Value });// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                userInfo.MyUsers = true;
                ViewData.Model = userInfo;

                var userInfoOb = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                userInfoOb.SubUserId = id.Value;
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfoOb);
            }
            return Json(ViewData.Model);
        }
        [CustomAuthorize]
        [HttpPost]
        [RequireHttps]
        public ActionResult Edit(UserDto userInfo)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (ModelState.IsValid || (ModelState.IsValid == false && ModelState.Values.Where(p => p.Errors.Count() >= 1).Count() == 2))
            {
                try
                {
                    ChangeEmailDto changeEmail = _userService.UpdateUser(userInfo);

                    if (changeEmail != null)
                    {
                        if (!string.IsNullOrEmpty(changeEmail.Hashing))
                        {
                            string emailTemplate = ResourcesUtilities.GetResource("ChangeEmailCompletion", "Emails");

                            emailTemplate = emailTemplate.Replace("[url]",
                                                                  Url.Action("activation", "user",
                                                                             new { activationCode = changeEmail.ActivationCode, hashing = changeEmail.Hashing, type = "email" }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString()));

                            _mailSender.SendEmail("", "", userInfo.EmailAddress, userInfo.EmailAddress, ResourcesUtilities.GetResource("ChangeEmail", "EmailHeader"), emailTemplate);
                        }

                    }

                    var modelErrors = ModelState.Values.Where(p => p.Errors.Count() == 1).ToList();

                    if (modelErrors != null && modelErrors.Count != 0)
                    {
                        foreach (var item in modelErrors)
                        {
                            item.Errors.Clear();
                        }
                    }


                    modelErrors = ModelState.Values.Where(p => p.Errors.Count() == 2).ToList();

                    if (modelErrors != null && modelErrors.Count != 0)
                    {
                        foreach (var item in modelErrors)
                        {
                            item.Errors.Clear();
                        }
                    }
                    AddMessages(ResourcesUtilities.GetResource("SuccessEditPersonalInformation", "ThanksMessages"), MessagesType.Success);
                    if (changeEmail.duplicateBuyer)
                    {

                        AddMessages(ResourcesUtilities.GetResource("BuyerDuplicate", "ThanksMessages"), MessagesType.Warning);
                    }
                }
                catch (UserEmailAlreadyExistsException)
                {
                    ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                }


            }

            if (ModelState["confirmpassword"] != null)
            {
                ModelState["confirmpassword"].Errors.Clear();
            }

            ViewData.Model = userInfo;

            return View();
        }

        [CustomAuthorize]
        [HttpPost]
        [RequireHttps]
        public ActionResult SaveEditUser([FromBody]UserDto userInfo)
        {
          
                try
                {
                    ChangeEmailDto changeEmail = _userService.UpdateUser(userInfo);

                    if (changeEmail != null)
                    {
                        if (!string.IsNullOrEmpty(changeEmail.Hashing))
                        {
                            string emailTemplate = ResourcesUtilities.GetResource("ChangeEmailCompletion", "Emails");

                            emailTemplate = emailTemplate.Replace("[url]",
                                                                  Url.Action("activation", "user",
                                                                             new { activationCode = changeEmail.ActivationCode, hashing = changeEmail.Hashing, type = "email" }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString()));

                            _mailSender.SendEmail("", "", userInfo.EmailAddress, userInfo.EmailAddress, ResourcesUtilities.GetResource("ChangeEmail", "EmailHeader"), emailTemplate);
                        }

                    }





                //AddMessages(ResourcesUtilities.GetResource("SuccessEditPersonalInformation", "ThanksMessages"), MessagesType.Success);
                AddSuccessfullyMsgMs(ResourcesUtilities.GetResource("SuccessEditPersonalInformation", "ThanksMessages"));

                if (changeEmail.duplicateBuyer)
                    {

                   

                    AddWarnningMsgs(ResourcesUtilities.GetResource("BuyerDuplicate", "ThanksMessages"));
                }
                return Json(ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"), ResponseStatus.success);
            }
                catch (UserEmailAlreadyExistsException)
                {
                //    ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                AddErrorMsgs(ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));
                return Json(ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"), ResponseStatus.businessException);


            }


            

            

     
        }

        
        [CustomAuthorize]
        [RequireHttps]
        [AuthorizeRole(Roles = "Administrator")]

        public ActionResult Buyer(bool successful = false)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                                        {
                                                            new BreadCrumbModel()
                                                                {
                                                                    Text =ResourcesUtilities.GetResource("Buyer","account"),
                                                                    Order = 1
                                                                }
                                                        };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            UserDto userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });
            ViewBag.BuyerCode = userInfo.buyerCode;
            ViewBag.BuyerId = userInfo.buyerId;

            if (successful)
            {
                // AddMessages(string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("buyer", "account")), MessagesType.Success);
                AddSuccessfullyMsg();
            }

            return View();
        }

        [HttpGet]
        [CustomAuthorize]
        [RequireHttps]
        [AuthorizeRole(Roles = "Administrator")]

        public ActionResult GetBuyerData()
        {

            UserDto userInfo = _userService.GetUserById(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });

            return Json( new { BuyerCode = userInfo.buyerCode, BuyerId = userInfo.buyerId, Name = userInfo.AccountName } );

        }

        [CustomAuthorize]
        [HttpPost]
        [RequireHttps]
        [AuthorizeRole(Roles = "Administrator")]

        public ActionResult Buyer(string buyerCode, int? buyerId)
        {

            _userService.SaveAccountBuyer(new SaveAccountBuyerRequest { BuyerCode = buyerCode, BuyerId = buyerId });


            return RedirectToAction("Buyer", "User", new { successful = true });
        }

        [CustomAuthorize]
        [HttpPost]
        [RequireHttps]
        [AuthorizeRole(Roles = "Administrator")]

        public ActionResult SaveBuyerData([FromBody]BuyerData model)
        {
            try
            {
                _userService.SaveAccountBuyer(new SaveAccountBuyerRequest { BuyerCode = model.buyerCode, BuyerId = model.buyerId });
            }
            catch (Exception)
            {
                AddErrorMsgs(ResourcesUtilities.GetResource("FailedSaveBuyerData", "account"));
                return Json(ResourcesUtilities.GetResource("buyerCode", "account"), ResponseStatus.businessException);
            }


            AddSuccessfullyMsgMs(ResourcesUtilities.GetResource("SaveBuyerData", "account"));
            return Json("Buyer", ResponseStatus.success);

        }


        [CustomAuthorize]
        [RequireHttps]

        public ActionResult CheckduplicateBuyer(string buyerCode)
        {
            if (string.IsNullOrEmpty(buyerCode))
                return Json(new { result = false });

            bool result = _userService.CheckduplicateBuyer(buyerCode).Value;
            return Json(new { result = result });

        }


        public ActionResult MD5Encryptiontest(string test)
        {
            return Json(_userService.MD5Encryptiontest(test));

        }

        [CustomAuthorize]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Delete(FormCollection collection)
        {
            _userService.DeleteUser(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });

            return RedirectToAction("login", "user", new { method = "logout" });

        }


        [RequireHttps(Order = 1)]
        public ActionResult CheckEmailAddress(string emailAddress)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                UserDto userInfo = _userService.GetUserByEmail(new CheckUserEmailRequest { EmailAddress = emailAddress, CheckPendingEmail = true });

                UserDto LogedinUserInfo = _userService.GetUserByAccount(new UserAccountMessage { AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value, UserId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });

                if (userInfo == null || userInfo.Id == LogedinUserInfo.Id || userInfo.Id == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId)
                {
                    return Json(true);
                }
                else
                {
                    if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId.HasValue)
                    {
                        if (userInfo.Id == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId.Value && Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId > 0)
                        {
                            return Json(true);

                        }
                    }
                    return Json(false);
                }

            }
            else
            {
                var result = _userService.CheckUserEmail(new CheckUserEmailRequest { EmailAddress = emailAddress, CheckPendingEmail = true }).Value;
                //if (result)
                //{
                //    result= _userService.InvitationCount(emailAddress)>0 && _userService.InvitationAcceptedCount(emailAddress)==0;
                //    if (result)
                //    {//pass validation
                //        result = false;
                //    }
                //}
                return Json(!(result));
            }

        }

        //[RequireHttps(Order = 1)]
        public ActionResult CheckAccountDSPEmailAddress(string emailAddress)
        {
            return Json(!_userService.CheckAcccountDSPEmail(emailAddress).Value);
        }


        [CustomAuthorize]
        [RequireHttps(Order = 1)]
        public ActionResult ChangePassword()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ChangePassword","SiteMapLocalizations"),
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                               {
                                                  Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                                }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        [RequireHttps(Order = 1)]
        public ActionResult ChangePassword([FromBody]ChangePasswordInfo changePasswordInfo)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ChangePassword","SiteMapLocalizations"),
                                                  Order = 2,
                                              },
                                          new BreadCrumbModel()
                                               {
                                                  Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                                }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (ModelState.IsValid && _userService.CheckUserPassword(changePasswordInfo.CurrentPassword).Value)
            {
                try
                {
                    _userService.ChangePassword(new ChangeUserPasswordRequest { NewPassword = changePasswordInfo.Password, OldPassword = changePasswordInfo.CurrentPassword });                    //AddMessages(ResourcesUtilities.GetResource("SuccessfulChangePassword", "ChangePassword"), MessagesType.Success);
                    AddMessages(ResourcesUtilities.GetResource("SuccessfulChangePassword", "ChangePassword"), MessagesType.Success);
                }
                catch (ChangePasswordException)
                {
                    AddMessages(ResourcesUtilities.GetResource("FailedChangePassword", "ChangePassword"), MessagesType.Warning);
                }
            }
            else
            {
                ModelState.AddModelError("ModelInValid", ResourcesUtilities.GetResource("InvalidModel", "ChangePassword"));
            }

            return View();
        }


        [HttpPost]
        [CustomAuthorize]
        [RequireHttps(Order = 1)]
        public ActionResult SaveChangePassword([FromBody] ChangePasswordInfo changePasswordInfo)
        {


            if (_userService.CheckUserPassword(changePasswordInfo.CurrentPassword).Value)
            {
                try
                {
                    _userService.ChangePassword(new ChangeUserPasswordRequest { NewPassword = changePasswordInfo.Password, OldPassword = changePasswordInfo.CurrentPassword });                    //AddMessages(ResourcesUtilities.GetResource("SuccessfulChangePassword", "ChangePassword"), MessagesType.Success);
                    AddSuccessfullyMsg("SuccessfulChangePassword", "ChangePassword");
                    return Json(ResourcesUtilities.GetResource("ChangePassword", "Titles"), ResponseStatus.success);

                }
                catch (ChangePasswordException)
                {
                    //AddMessages(ResourcesUtilities.GetResource("FailedChangePassword", "ChangePassword"), MessagesType.Warning);
                    AddErrorMsgs(ResourcesUtilities.GetResource("FailedChangePassword", "ChangePassword"));
                    return Json(ResourcesUtilities.GetResource("ChangePassword", "Titles") , ResponseStatus.businessException);

                }
            }
            else
            {
                //ModelState.AddModelError("ModelInValid", ResourcesUtilities.GetResource("InvalidModel", "ChangePassword"));
                AddErrorMsgs(ResourcesUtilities.GetResource("InvalidModel", "ChangePassword"));
                return Json(ResourcesUtilities.GetResource("ChangePassword", "Titles"), ResponseStatus.businessException);
            }
            //return Json("ChangePassword", ResponseStatus.success);
        }

        [RequireHttps]
        public ActionResult CheckPassword(string currentpassword)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return Json(_userService.CheckUserPassword(currentpassword).Value);
            }
            else
            {
                return Json(true);
            }
        }

        public ActionResult UserInfo(UserDto userInfo, string status)
        {
            ViewData["status"] = status;
            return View(userInfo);
        }

        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public ActionResult PaymentDetails()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("BankAccount","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                               {
                                                  Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                                }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewData.Model = _accountService.GetAccountPaymentDetails();
            return View();
        }
        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public ActionResult PaymentUploader()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("BankAccount","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                               {
                                                  Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                                }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewData.Model = _accountService.GetAccountPaymentDetails();
            return View();
        }
        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public ActionResult GetPaymentDetails()
        {
            var Model = _accountService.GetAccountPaymentDetails();
            return Json(Model);

        }

        [CustomAuthorize]
        [HttpPost]
        [DenyNonPrimaryRole]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult PaymentDetails([FromBody]AccountPaymentDetailDto bankAccountDto)
        {


            if (ModelState.IsValid)
            {

                try
                {
                    var TaxDocument = Request.Form.Files["TaxDocument"];
                    if (TaxDocument != null && TaxDocument.Length != 0)
                    {

                        MemoryStream target = new MemoryStream();
                        TaxDocument.OpenReadStream().CopyTo(target);

                        DocumentDto DocumentDto = new DocumentDto
                        {
                            Content = target.ToArray(),
                            Name = TaxDocument.FileName,
                            UploadedDate = Framework.Utilities.Environment.GetServerTime(),
                            Size = target.ToArray().Length,
                            Extension = TaxDocument.ContentType,
                            DocumentTypeId = _DocumentTypeService.GetByCode(".jpg").ID,
                        };

                        bankAccountDto.Document = DocumentDto;

                    }

                    _accountService.UpdateBankAccountInfo(bankAccountDto);
                    AddMessages(ResourcesUtilities.GetResource("SuccessSave", "BankAccount"), MessagesType.Success);
                    MoveMessagesTempData();
                    return RedirectToAction("PaymentDetails");
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                    return View(bankAccountDto);
                }
            }

            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("BankAccount","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                               {
                                                  Text =ResourcesUtilities.GetResource("Edit","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                                }
                                      };

            #endregion
            return View(bankAccountDto);
        }
        [CustomAuthorize]
        [HttpPost]
        [HttpPut]
        [DenyNonPrimaryRole]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SavePaymentDetails( AccountPaymentDetailDto bankAccountDto)
        {

            List<ResultMessage> rMessages = new List<ResultMessage>();


            try
                {
                    var TaxDocument = Request.Form.Files["TaxDocument"];
                    if (TaxDocument != null && TaxDocument.Length != 0)
                    {

                        MemoryStream target = new MemoryStream();
                        TaxDocument.OpenReadStream().CopyTo(target);

                        DocumentDto DocumentDto = new DocumentDto
                        {
                            Content = target.ToArray(),
                            Name = TaxDocument.FileName,
                            UploadedDate = Framework.Utilities.Environment.GetServerTime(),
                            Size = target.ToArray().Length,
                            Extension = TaxDocument.ContentType,
                            DocumentTypeId = _DocumentTypeService.GetByCode(".jpg").ID,
                        };

                        bankAccountDto.Document = DocumentDto;

                    }

                    _accountService.UpdateBankAccountInfo(bankAccountDto);
               // rMessages.Add( new ResultMessage { Message= ResourcesUtilities.GetResource("SuccessSave", "BankAccount"),Type= MessagesType.Success });

                AddSuccessfullyMsg(resourceKey: "SuccessSave", resourceSet: "BankAccount");

                // AddSuccessfullyMsg(rMessages);
                //MoveMessagesTempData();
                //return RedirectToAction("PaymentDetails");
                return Json(bankAccountDto, ResourcesUtilities.GetResource("BankAccount", "BankAccount"), ResponseStatus.success);

            }
                catch (BusinessException exception)
                {
                    //foreach (var errorData in exception.Errors)
                  //  {
                    //AddMessages(errorData.Message, MessagesType.Error);
                    AddErrorMsgs(exception);
                //}
                return Json(ResourcesUtilities.GetResource("BankAccount", "BankAccount"), ResponseStatus.businessException);

                //return Json(bankAccountDto, "Bank Account", ResponseStatus.success);

            }



            // return Json(new { Messages = rMessages });
        }

        [CustomAuthorize]
        [DenyNonPrimaryRole]
        public ActionResult AccountHistory()
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AccountHistory", "SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            AccountSummaryDto accountSummaryDto = _accountService.GetAccountSummary();
            ViewData["Earnings"] = accountSummaryDto.RoundedEarning.ToString("F2");
            ViewData["Funds"] = accountSummaryDto.RoundedFunds.ToString("F2");
            return View();
        }
        public ActionResult GetAccountHistoryResult()
        {
            AccountSummaryDto accountSummaryDto = _accountService.GetAccountSummary();
            var earnings = accountSummaryDto.RoundedEarning.ToString("F2");
            var funds = accountSummaryDto.RoundedFunds.ToString("F2");
            return Json(new { earnings = earnings, funds = funds }) ;
        }
        public ActionResult Receipt(int id)
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ReceiptHeader", "Receipt"),
                                                  Order = 3
                                              },
                                              new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("AccountHistory", "Titles"),
                                                  Order = 2,
                                                  Url = Url.Action("AccountHistory", "User")
                                              },
                                               new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            HistoryCriteriaDto criteria = new HistoryCriteriaDto();
            criteria.FromDate = Framework.Utilities.Environment.GetServerTime().AddYears(-10).ToUniversalTime();
            criteria.ToDate = Framework.Utilities.Environment.GetServerTime();
            criteria.ItemsPerPage = Config.PageSize;
            criteria.Ascending = false;

            FundTransactionDto fund = _FundsService.GetAccountFundsHistory(criteria).Items.Where(x => x.ID == id).FirstOrDefault();


            Receipt model = new Receipt
            {
                Name = _accountService.GetAccountName(new ValueMessageWrapper<int> { Value = fund.AccountId }),
                TransactionDate = fund.CreationDate.ToShortDateString(),
                Method = fund.FundTransType.Name.Value,
                Amount = fund.Amount,
                VATAmount = fund.VATAmount,
                NoqoushReceiptNumber = fund.NoqoushReceiptNumber
            };

            return View(model);

        }

        public ActionResult GetReceipt(int id)
        {
                

            HistoryCriteriaDto criteria = new HistoryCriteriaDto();
            criteria.FromDate = Framework.Utilities.Environment.GetServerTime().AddYears(-10).ToUniversalTime();
            criteria.ToDate = Framework.Utilities.Environment.GetServerTime();
            criteria.ItemsPerPage = Config.PageSize;
            criteria.Ascending = false;

            FundTransactionDto fund = _FundsService.GetAccountFundsHistory(criteria).Items.Where(x => x.ID == id).FirstOrDefault();


            Receipt model = new Receipt
            {
                Name = _accountService.GetAccountName(new ValueMessageWrapper<int> { Value = fund.AccountId }),
                TransactionDate = fund.CreationDate.ToShortDateString(),
                Method = fund.FundTransType.Name.Value,
                Amount = fund.Amount,
                VATAmount = fund.VATAmount,
                NoqoushReceiptNumber = fund.NoqoushReceiptNumber
            };

            return Json(model);

        }
        [CustomAuthorize]
        public ActionResult FundHistory()
        {
            FundResultDto fundsResultDto = new FundResultDto();

            ViewData.Model = fundsResultDto;
            return PartialView();
        }

        [CustomAuthorize]
        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult FundHistory(string orderBy, int? page)
        {
            HistoryCriteriaDto criteria = new HistoryCriteriaDto();
            //TODO:osaleh to use GMT
            criteria.FromDate = Framework.Utilities.Environment.GetServerTime().AddYears(-10).ToUniversalTime();
            criteria.ToDate = Framework.Utilities.Environment.GetServerTime();
            criteria.PageNumber = (page.HasValue ? page.Value - 1 : 0); ;
            criteria.ItemsPerPage = Config.PageSize;
            criteria.Ascending = false;

            /*TransactionVATCriteria criteriadd = new TransactionVATCriteria();
            //TODO:osaleh to use GMT
            criteriadd.DataFrom = Framework.Utilities.Environment.GetServerTime().AddYears(-10).ToUniversalTime();
            criteriadd.DataTo = Framework.Utilities.Environment.GetServerTime();
            criteriadd.Page = (page.HasValue ? page: null); ;
            criteriadd.Size = Config.PageSize;

             var resultgdg=    _FundsService.GetTransactionVATHistory(criteriadd);*/
            FundResultDto fundsResultDto = _FundsService.GetAccountFundsHistory(criteria);

            return Json(new GridModel
            {
                Data = fundsResultDto.Items,
                Total = fundsResultDto.Total
            });
        }

        [CustomAuthorize]
        public ActionResult PaymentHistory()
        {
            PaymentDtoResult fundsResultDto = new PaymentDtoResult();

            ViewData.Model = fundsResultDto;
            return PartialView();
        }


        public PartialViewResult SwitchAccount(string returnUrl)
        {
            string email = OperationContext.Current.CurrentPrincipal.Identity.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var userDto = _userService.GetUserByEmail(new CheckUserEmailRequest { EmailAddress = email, CheckPendingEmail = false });
                var accounts = _accountService.GetUserAccountsByEmail(email);
                if (accounts != null && accounts.Count() > 1)
                {
                    foreach (var item in accounts)
                    {
                        if (item.AccountRole == (int)AccountRole.DSP)
                        {
                            ViewData["DSPId"] = item.AccountId;
                        }
                        else if (item.AccountRole == (int)AccountRole.DataProvider)
                        {
                            ViewData["DataProviderId"] = item.AccountId;

                        }
                        else
                        {
                            ViewData["NormalId"] = item.AccountId;
                        }


                        ViewData["UserId"] = userDto.Id;

                    }
                    ViewData["Email"] = email;

                }
                //else if (accounts != null && accounts.Count() == 1)
                //{
                //    return RedirectToAction("index", "dashboard", new { charttype = "ad" });

                //}
            }
            ViewData["returnUrl"] = returnUrl;


            return PartialView();
        }

        [HttpPost]
        public ActionResult SwitchAccountUserFormData([FromBody]SwitchAccountUserModel model)
        {
            if (!string.IsNullOrEmpty(OperationContext.Current.CurrentPrincipal.Identity.Name) && OperationContext.Current.CurrentPrincipal.Identity.Name != "notauthunticated")
            {
                int ChosenId =  model.AccountId ;
                int UserId = model.UserId ;
                //string returnUrl = Request.Form["SwitchAccountUserReturnUrl"];
                BuildAdFalconUser(ChosenId, UserId, true);

                //return RedirectToAction("ContinueLogin", new { returnUrl = returnUrl });
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole != (int)AccountRole.DataProvider)
                {
                    string linkRedirect = Url.Action("index", "dashboard", new { charttype = "ad" });

                    return Json(linkRedirect,"",ResponseStatus.redirect);


                }
                else
                {
                    string returnUrl = Url.Action("index", "dashboard", new { charttype = "lmpressionlog" });

                    return Json(returnUrl, "", ResponseStatus.redirect);
                }
                //return null;
            }
            else
            {
                return null;
            }

        }
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult SwitchAccountUserFormDataHttps()
        {
            if (!string.IsNullOrEmpty(OperationContext.Current.CurrentPrincipal.Identity.Name) && OperationContext.Current.CurrentPrincipal.Identity.Name != "notauthunticated")
            {
                int ChosenId = !string.IsNullOrEmpty(Request.Form["SwitchAccountUserChosenId"]) ? Convert.ToInt32(Request.Form["SwitchAccountUserChosenId"]) : 0;
                int UserId = !string.IsNullOrEmpty(Request.Form["SwitchAccountUserUserId"]) ? Convert.ToInt32(Request.Form["SwitchAccountUserUserId"]) : 0;
                string returnUrl = Request.Form["SwitchAccountUserReturnUrl"];
                BuildAdFalconUser(ChosenId, UserId, true);

                //return RedirectToAction("ContinueLogin", new { returnUrl = returnUrl });
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole != (int)AccountRole.DataProvider)
                {
                    return RedirectToAction("index", "dashboard", new { charttype = "ad" });
                }
                else
                {
                    return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });
                }
                //return null;
            }
            else
            {
                return null;
            }

        }

        [CustomAuthorize]
        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult PaymentHistory(string orderBy, int? page)
        {
            var criteria = new HistoryCriteriaDto
            {
                FromDate = Framework.Utilities.Environment.GetServerTime().AddYears(-10).ToUniversalTime(),
                ToDate = Framework.Utilities.Environment.GetServerTime(),
                PageNumber = (page.HasValue ? page.Value - 1 : 0)
            };
            //TODO:osaleh to use GMT
            //.ToUniversalTime();
            ;
            criteria.ItemsPerPage = Config.PageSize;
            criteria.Ascending = false;

            PaymentDtoResult paymentsResultDto = _accountService.GetAccountPaymentsHistory(criteria);

            return Json(new GridModel
            {
                Data = paymentsResultDto.Items,
                Total = paymentsResultDto.Total
            });
        }



        //   [AuthorizeRole(Roles = "Administrator,AdOps")]

        [CustomAuthorize]
        // [AuthorizeRole(Roles = "Administrator,AdOps")]
        [RequireHttps]
        [DenyNonPrimaryRole]
        public ActionResult APIAccess()
        {
            AccountAPIAccessDto accountAPIAccess = _accountService.GetAPIAccessSetting();

        
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("APIAccess", "Titles"),
                                                  Order = 2
                                              },
                                              new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            ViewBag.ShowMenu = true;

            return View(accountAPIAccess);
        }

        [CustomAuthorize]
        // [AuthorizeRole(Roles = "Administrator,AdOps")]
        [RequireHttps]
        [DenyNonPrimaryRole]
        public ActionResult GetAPIAccess()
        {
            AccountAPIAccessDto accountAPIAccess = _accountService.GetAPIAccessSetting();

            if (!accountAPIAccess.AllowAPIAccess)
            {
                AddWarnningMsgs(ResourcesUtilities.GetResource("DenyAPIAccessWarning", "Global"));
            }


            return Json(accountAPIAccess, ResourcesUtilities.GetResource("APIAccess", "Menu"), ResponseStatus.success);
        }

        //  [AuthorizeRole(Roles = "Administrator,AdOps")]

        [CustomAuthorize]
        //    [AuthorizeRole(Roles = "Administrator,AdOps")]
        [HttpPost]
        [RequireHttps]
        public ActionResult APIAccess(string generateAPIAccess)
        {
            AccountAPIAccessDto accountAPIAccess = null;

            try
            {
                accountAPIAccess = _accountService.GenerateAPIAccess();
            }
            catch (BusinessException x)
            {
                foreach (var item in x.Errors)
                {
                    AddMessages(item.Message, MessagesType.Warning);
                }

                accountAPIAccess = new AccountAPIAccessDto();
                accountAPIAccess.AllowAPIAccess = false;
            }

            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("APIAccess", "Titles"),
                                                  Order = 2
                                              },
                                              new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(accountAPIAccess);
        }

        [CustomAuthorize]
        //    [AuthorizeRole(Roles = "Administrator,AdOps")]
        [HttpPost]
        [RequireHttps]
        public ActionResult SetAPIAccess(string generateAPIAccess)
        {
            AccountAPIAccessDto accountAPIAccess = null;

            try
            {
                accountAPIAccess = _accountService.GenerateAPIAccess();
            }
            catch (BusinessException x)
            {
                foreach (var item in x.Errors)
                {
                    AddMessages(item.Message, MessagesType.Warning);
                }

                accountAPIAccess = new AccountAPIAccessDto();
                accountAPIAccess.AllowAPIAccess = false;
            }


            return Json(accountAPIAccess);
        }

        private UserCriteriaBase GetAccountCriteriaBase(AccountSearchSaveModel saveModel)
        {
            int page = 1;
            int size = 10;
            if (!int.TryParse(Request.Form["page"], out page))
                page = 1;
            if (!int.TryParse(Request.Form["size"], out size))
                size = 10;
            page--;
            return new UserCriteriaBase
            {
                CompanyName = saveModel.CompanyName,
                Name = saveModel.Name,
                Email = saveModel.Email,
                AccountId = saveModel.AccountId,
                StatusId = saveModel.StatusId,
                Page = page,
                Size = size,
                Role = saveModel.RoleId,
                hideCurrentUser = saveModel.hideCurrentUser,
                hideNonPrimary = saveModel.hideNonPrimary,
                publisherUsers = saveModel.publisherUsers,
            };
        }
        private UserCriteriaBase GetUserCriteriaBase(AccountSearchSaveModel saveModel)
        {
            int page = 1;
            int size = 10;
            if (!int.TryParse(Request.Form["page"], out page))
                page = 1;
            if (!int.TryParse(Request.Form["size"], out size))
                size = 10;
            page--;
            return new UserCriteriaBase
            {
                CompanyName = saveModel.CompanyName,
                Name = saveModel.Name,
                Email = saveModel.Email,
                AccountId = saveModel.AccountId,
                StatusId = saveModel.StatusId,
                Page = page,
                Size = size,
                hideCurrentUser = saveModel.hideCurrentUser,
                hideNonPrimary = saveModel.hideNonPrimary,
                publisherUsers = saveModel.publisherUsers
            };
        }

        private AccountSearchViewModel GetAccountSearchViewModel(UserCriteriaBase criteria)
        {
            var result = _accountService.QueryByCratiria(criteria);
            var model = new AccountSearchViewModel();
            if (criteria.hideCurrentUser && criteria.hideNonPrimary)
            {
                model = new AccountSearchViewModel
                {
                    Name = criteria.Name,
                    CompanyName = criteria.CompanyName,
                    Email = criteria.Email,
                    AccountIdValue = criteria.AccountId,
                    TotalCount = result.TotalCount,

                    Users = result.Items.Select(item => new AccountViewModel()
                    {
                        Id = item.Id,
                        AccountId = item.AccountId,
                        Name = item.ToString(),
                        CompanyName = item.Company,
                        Email = item.EmailAddress,
                        IsBlocked = item.Block,
                        IsSecondPrimaryUser = item.IsSecondPrimaryUser,
                        UserType = item.UserType,
                        VATValue = item.VATValue,
                        Role = ((AccountRole)item.AccountRole).ToText(),
                        PermissionCodes = _userService.getAccountPermissionCode(new ValueMessageWrapper<int> { Value = item.AccountId })

                    }).ToList()
                };
            }
            else
            {
                model = new AccountSearchViewModel
                {
                    Name = criteria.Name,
                    CompanyName = criteria.CompanyName,
                    Email = criteria.Email,
                    AccountIdValue = criteria.AccountId,
                    TotalCount = result.TotalCount,
                    Users = result.Items.Select(item => new AccountViewModel()
                    {
                        Id = item.Id,
                        AccountId = item.AccountId,
                        Name = item.ToString(),
                        CompanyName = item.Company,
                        Email = item.EmailAddress,
                        IsSecondPrimaryUser = item.IsSecondPrimaryUser,
                        UserType = item.UserType,
                        IsBlocked = item.Block,
                        VATValue = item.VATValue,
                        Role = ((AccountRole)item.AccountRole).ToText(),



                    }).ToList()
                };


            }

            return model;
        }
        private AccountSearchViewModel GetUserSearchViewModel(UserCriteriaBase criteria)
        {
            var result = _userService.QueryByCratiria(criteria);
            var model = new AccountSearchViewModel();
            if (criteria.hideCurrentUser && criteria.hideNonPrimary)
            {
                model = new AccountSearchViewModel
                {
                    Name = criteria.Name,
                    CompanyName = criteria.CompanyName,
                    Email = criteria.Email,
                    AccountIdValue = criteria.AccountId,
                    TotalCount = result.TotalCount,
                    Users = result.Items.Select(item => new AccountViewModel()
                    {
                        Id = item.Id,
                        AccountId = item.AccountId,
                        Name = item.ToString(),
                        CompanyName = item.Company,
                        Email = item.EmailAddress,
                        IsSecondPrimaryUser = item.IsSecondPrimaryUser,

                        UserType = item.UserType,

                        IsBlocked = item.Block,
                        PermissionCodes = _userService.getAccountPermissionCode(new ValueMessageWrapper<int> { Value = item.AccountId })

                    }).ToList()
                };
            }
            else
            {
                model = new AccountSearchViewModel
                {
                    Name = criteria.Name,
                    CompanyName = criteria.CompanyName,
                    Email = criteria.Email,
                    AccountIdValue = criteria.AccountId,
                    TotalCount = result.TotalCount,
                    Users = result.Items.Select(item => new AccountViewModel()
                    {
                        Id = item.Id,
                        AccountId = item.AccountId,
                        Name = item.ToString(),
                        IsSecondPrimaryUser = item.IsSecondPrimaryUser,

                        UserType = item.UserType,
                        CompanyName = item.Company,
                        Email = item.EmailAddress,
                        IsBlocked = item.Block


                    }).ToList()
                };


            }

            return model;
        }
        private AccountSearchViewModel GetAccountDSPRequestSearchViewModel(UserCriteriaBase criteria)
        {
            //var sss = ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat;

            var result = _userService.QueryByCratiriaForAccountUsers(criteria);
            var model = new AccountSearchViewModel
            {
                Name = criteria.Name,
                CompanyName = criteria.CompanyName,
                Email = criteria.Email,
                AccountIdValue = criteria.AccountId,
                StatusId = criteria.StatusId,
                TotalCount = result.TotalCount,

                Users = result.Items.Select(item => new AccountViewModel()
                {
                    Id = item.Id,
                    AccountId = item.AccountId,
                    Name = item.ToString(),
                    CompanyName = item.Company,
                    Email = item.EmailAddress,
                    Address = item.Address1,
                    StatusName = item.StatusName,
                    ApprovalNote = item.ActionNote,
                    Note = item.Note,
                    DateString = item.RequestDate != item.ActionDate ? item.ActionDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat) : "",
                    Date2String = item.RequestDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat),

                    CountryName = item.CountryNameValue,
                    CompanyTypeName = item.CompanyTypeNameValue,
                    Phone = item.Phone
                }).ToList()
            };
            return model;
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        public ActionResult AccountSearch()
        {
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }

        [AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult GiveTakePermission(string GivenPermissionAdCodes, int? accountId)
        {
            try
            {
                if (accountId.HasValue)
                {
                    AccountAdPermissionsDto PermissionDetails = new AccountAdPermissionsDto
                    {
                        GivenPermissionAdCodes = GivenPermissionAdCodes,
                        AccountId = (int)accountId
                    };

                    _userService.GiveTakePermission(PermissionDetails);

                    return Json(new { status = true });
                }
                else
                {
                    throw new Exception("GivenPermissionAdCodes or accountId are empty");
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]
        [HttpPost]
        public ActionResult SaveGiveTakePermission([FromBody]UserPermissionSave model)
        {
           
                if (model.AccountId > 0)
                {
                    AccountAdPermissionsDto PermissionDetails = new AccountAdPermissionsDto
                    {
                        GivenPermissionAdCodes = model.GivenPermissionAdCodes,
                        AccountId = model.AccountId
                    };

                    _userService.GiveTakePermission(PermissionDetails);
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("Permissions", "Global"));
                    return Json(new { status = true }, ResourcesUtilities.GetResource("Permissions", "Global"), ResponseStatus.success);
                }
                else
                {
                    AddErrorMsgs("GivenPermissionAdCodes or accountId are empty");
                    return Json(new { status = false }, ResourcesUtilities.GetResource("Permissions", "Global"), ResponseStatus.businessException);
               }
       
        }
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        [CustomAuthorize]

        public ActionResult Permissions(AccountSearchSaveModel saveModel)
        {

            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                hideCurrentUser = true,
                hideNonPrimary = true,
                hideAdmin = true,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult _accountSearch(AccountSearchSaveModel saveModel)
        {
            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp)
            {
                var result = GetAccounts(saveModel);
                IList<AccountViewModel> users = result.Users;

                //  getuserPermissions(ref users);
                return Json(new GridModel
                {
                    Data = users,
                    Total = Convert.ToInt32(result.TotalCount)
                });
            }
            return null;
        }
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        //[RequireHttps(Order = 1)]
        public ActionResult _accountSearchNoHttps(AccountSearchSaveModel saveModel)
        {
            if (ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp)
            {
                var result = GetAccounts(saveModel);
                IList<AccountViewModel> users = result.Users;

                //  getuserPermissions(ref users);
                return Json(new GridModel
                {
                    Data = users,
                    Total = Convert.ToInt32(result.TotalCount)
                });
            }
            return null;
        }

        [GridAction(EnableCustomBinding = true)]
        //[RequireHttps(Order = 1)]
        public ActionResult _nohttpsPublisherAccountSearch(AccountSearchSaveModel saveModel)
        {

            saveModel.publisherUsers = true;
            var result = GetAccounts(saveModel);
            IList<AccountViewModel> users = result.Users;

            //  getuserPermissions(ref users);
            return Json(new GridModel
            {
                Data = users,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        //private void getuserPermissions(ref IList<AccountViewModel> users)
        //{

        //    foreach (AccountViewModel user in users)
        //    {
        //        user.Id
        //    }

        //}

        [GridAction(EnableCustomBinding = true)]
        [RequireHttps(Order = 1)]
        [DenyNonPrimaryRole]
        public ActionResult _MyUsers(AccountSearchSaveModel saveModel)
        {
            saveModel.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId;
            saveModel.hideCurrentUser = true;
            var result = GetUsers(saveModel);
            if (result.Users != null && result.Users.Count > 0)
                result.Users = result.Users.Where(x => x.Id != OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId).ToList();
            return Json(new GridModel
            {
                Data = result.Users,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        public ActionResult nohttpsPublisherAccountSearch()
        {
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }

        [DenyNonPrimaryRole]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Block(int id)
        {
            try
            {
                _userService.BlockUser(new ValueMessageWrapper<int> { Value = id });
                return Json(new { status = true });
            }
            catch (Exception e)
            {

                throw e;
            }


        }



        [DenyNonPrimaryRole]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult UpdateUserType(int id, string Ids, UserType userType)
        {
            try
            {
                _userService.UpdateUserType(new UpdateUserTypeRequest { UserId = id, Ids = Ids, UserType = userType });

                //let successMessg = props.t("Global:savedSuccessfully");
                // let userManagement = props.t("Menu:UserManagement");
                // successMessg = successMessg.replace("{0}", userManagement)
                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("UserManagement", "Menu"));
                return Json( ResourcesUtilities.GetResource("UserManagement", "Menu"), ResponseStatus.success );
            }
            catch (Exception e)
            {
                return Json(new { status = "businessException", Message=e.Message });
                //throw e;
            }


        }


        [DenyNonPrimaryRole]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult MakeUserSecondPrimaryUser(int id)
        {
            try
            {
                _userService.MakeUserSecondPrimaryUser(new ValueMessageWrapper<int> { Value = id });
                return Json(new { status = true });
            }
            catch (Exception e)
            {

                throw e;
            }


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


        private string GetAprroveEmail(string requestCode)
        {
            string emailTemplate = ResourcesUtilities.GetResource("AccountDSPAccepted", "Emails");

            var userdto = _userService.GetAccountDSPRequestByRequestCode(requestCode);


            emailTemplate = emailTemplate.Replace("[url]", "https://" + Config.PublicHostName + "/en/User/Register?requestCode=" + requestCode);
            emailTemplate = emailTemplate.Replace("[Year]", ArabyAds.Framework.Utilities.Environment.GetServerTime().Year.ToString());
            emailTemplate = emailTemplate.Replace("[CustomerEmailAddress]", userdto.EmailAddress);
            emailTemplate = emailTemplate.Replace("[CustomerName]", userdto.FirstName + " " + userdto.LastName);
            return emailTemplate;
        }

        [CustomAuthorize]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult ApproveAccountDSP(int? id, string note, string email)
        {


            if (id.HasValue)
            {
                AccountDSPReqestResultDto result = _userService.UpdateAccountDSPReqest(new AccountDSPRequestDto
                {
                    Id = (int)id,
                    ActionNote = note,
                    IsApproved = true,
                    EmailAddress = email,
                    Status = AccountDSPRequestStatus.Approved
                });
                if (result != null && result.Success)
                {
                    string subject = ResourcesUtilities.GetResource("AccountDSPAccepted", "DSPRequest");

                    if (!result.IsAlreadyRegistered)
                    {
                        _mailSender.SendEmail("", string.Empty, "", email, subject, GetAprroveEmail(result.RequestCode), true, "");
                    }
                    else
                    {
                        UserDto User = _userService.GetUserByEmail(new CheckUserEmailRequest { EmailAddress = email, CheckPendingEmail = true });
                        User.IsAccountDSP = true;
                        _accountService.CreateAccount(User);
                        var resources = ResourcesUtilities.GetResource("AccountDSPAcceptedAlreadyRegistered", "Emails");

                        resources = resources.Replace("[Year]", ArabyAds.Framework.Utilities.Environment.GetServerTime().Year.ToString());
                        _mailSender.SendEmail("", string.Empty, "", email, subject, resources, true, "");

                    }
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });

                }

            }
            else
            {
                return Json(new { status = false });


            }


        }


        [CustomAuthorize]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult IgnoreAccountDSP(int? id, string note, string email)
        {


            if (id.HasValue)
            {
                AccountDSPReqestResultDto result = _userService.UpdateAccountDSPReqest(new AccountDSPRequestDto
                {
                    Id = (int)id,
                    ActionNote = note,
                    IsApproved = false,
                    EmailAddress = email,
                    Status = AccountDSPRequestStatus.Ignored
                });
                if (result != null && result.Success)
                {

                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });

                }

            }
            else
            {
                return Json(new { status = false });


            }


        }
        [CustomAuthorize]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AccountDSPRequests(AccountSearchSaveModel saveModel)
        {

            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Requests", "AccountDSPRequest"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            ViewBag.AccountDSPStatus = new List<SelectListItem>();
            ViewBag.AccountDSPStatus.Add(new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") });

            ViewBag.AccountDSPStatus.Add(new SelectListItem { Value = ((int)AccountDSPRequestStatus.New).ToString(), Text = AccountDSPRequestStatus.New.ToText() });
            ViewBag.AccountDSPStatus.Add(new SelectListItem { Value = ((int)AccountDSPRequestStatus.Ignored).ToString(), Text = AccountDSPRequestStatus.Ignored.ToText() });
            ViewBag.AccountDSPStatus.Add(new SelectListItem { Value = ((int)AccountDSPRequestStatus.Approved).ToString(), Text = AccountDSPRequestStatus.Approved.ToText() });

            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                hideCurrentUser = true,
                hideNonPrimary = true,
                hideAdmin = true,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return View("~/Views/AccountDSPRequest/AccountDSPRequests.cshtml", model);
        }
        [GridAction(EnableCustomBinding = true)]
        [CustomAuthorize]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _AccountDSPRequests(AccountSearchSaveModel saveModel)
        {
            var result = GetAccountDSPRequestSearchViewModel(GetAccountCriteriaBase(saveModel));
            IList<AccountViewModel> users = result.Users;

            //  getuserPermissions(ref users);
            return Json(new GridModel
            {
                Data = users,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }
        #region Settings
        [RequireHttps(Order = 1)]
        // [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult Settings(string id)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Settings","SiteMapLocalizations"),
                                                          Order = 2,
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            int accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            //if (!int.TryParse(id, out accountid))
            //{
            //    //get current account id
            //    accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            //}
            var model = _accountService.GetAccountSetting(new ValueMessageWrapper<int> { Value = accountid });
            model.AccountId = accountid;
            ViewBag.ShowMenu = true;
            return View("AccountSettings", model);
        }
        [RequireHttps(Order = 1)]
        // [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult GetSettings(string id)
        {
            int accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
          
            var model = _accountService.GetAccountSetting(new ValueMessageWrapper<int> { Value = accountid });
            model.AccountId = accountid;
            
            return Json(model);
        }
        [CustomAuthorize]
        //[AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult Settings([FromBody]AccountSettingDto data)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Settings","SiteMapLocalizations"),
                                                          Order = 2,
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SaveCampAccountSetting(data);

                    #region Refresh UserInfo
                    AdFalconUserInfo userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();

                    #endregion

                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("Settings", new { id = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            ViewBag.ShowMenu = true;
            return View("AccountSettings", data);
        }



        [CustomAuthorize]
        //[AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult SaveUserSettings([FromBody] AccountSettingDto data)
        {
           
                try
                {
                    _accountService.SaveCampAccountSetting(data);

                    #region Refresh UserInfo
                    AdFalconUserInfo userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();

                #endregion

                //AddSuccessfullyMsg();
                //MoveMessagesTempData();
                //return RedirectToAction("Settings", new { id = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("Settings", "SiteMapLocalizations"));
                return Json(ResourcesUtilities.GetResource("Settings", "SiteMapLocalizations"), ResponseStatus.success);

            }
                catch (BusinessException exception)
                {
                AddErrorMsgs(exception);
                return Json(ResourcesUtilities.GetResource("Settings", "SiteMapLocalizations"), ResponseStatus.businessException);
            }
            
            
            //return View("AccountSettings", data);
        }

        #endregion

        #region Private Members

        private void LoginUser(string username, string token)
        {
            //var ticket = new FormsAuthenticationTicket(1, username, Framework.Utilities.Environment.GetServerTime(),
            //                                            Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
            //                                            true, token);

            //string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            //var cookie = new CookieOptions()
            //{
            //    Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
            //    Secure = FormsAuthentication.RequireSSL,
            //    HttpOnly = true
            //};
            //if (!Request.GetDisplayUrl().ToLower().Contains("localhost"))
            //{
            //    cookie.Domain = Config.CookieDomain;
            //}

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim("UserToken", token));
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), new AuthenticationProperties
            {
                ExpiresUtc = Framework.Utilities.Environment.GetServerTime().AddHours(24*7),
               
                IsPersistent = true,
                AllowRefresh = true,  
                
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            //Response.Cookies.Append(FormsAuthentication.FormsCookieName, encryptedTicket, cookie);
            _securityProxy.BuildSecurityContext(token).ConfigureAwait(false).GetAwaiter().GetResult(); ;
        }
        //      private static string GetAccessToken(string userId)
        //      {
        //          const string issuer = "localhost";
        //          const string audience = "localhost";

        //          var identity = new ClaimsIdentity(new List<Claim>
        //{
        //  new Claim("sub", userId)
        //});

        //          var bytes = Encoding.UTF8.GetBytes(userId);
        //          var key = new SymmetricSecurityKey(bytes);
        //          var signingCredentials = new SigningCredentials(
        //            key, SecurityAlgorithms.HmacSha256);

        //          var now = DateTime.UtcNow;
        //          var handler = new JwtSecurityTokenHandler();

        //          var token = handler.CreateJwtSecurityToken(
        //            issuer, audience, identity,
        //            now, now.Add(TimeSpan.FromHours(1)),
        //            now, signingCredentials);

        //          return handler.WriteToken(token);
        //      }
        public string GetActivationEmail(string activationCode)
        {
            string emailTemplate = ResourcesUtilities.GetResource("RegisterCompletion", "Emails");

            emailTemplate = emailTemplate.Replace("[url]",
                                                  Url.Action("activation", "user",
                                                             new { activationCode = activationCode }, "https", JsonConfigurationManager.AppSettings["AdFalconAPI"].ToString()));

            return emailTemplate;
        }
        public string GetInvitationAcceptedEmail(string accountName)
        {
            string emailTemplate = ResourcesUtilities.GetResource("InvitationAccepted", "Emails");

            emailTemplate = emailTemplate.Replace("[account]", accountName);
            emailTemplate = emailTemplate.Replace("[Year]", ArabyAds.Framework.Utilities.Environment.GetServerTime().Year.ToString());
            return emailTemplate;
        }

        private AccountSearchViewModel GetAccounts(AccountSearchSaveModel saveModel)
        {
            return GetAccountSearchViewModel(GetAccountCriteriaBase(saveModel));
        }
        private AccountSearchViewModel GetUsers(AccountSearchSaveModel saveModel)
        {
            return GetUserSearchViewModel(GetUserCriteriaBase(saveModel));
        }

        private string GetIPAddress()
        {
            var context = HttpContext;
            //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (!string.IsNullOrEmpty(ipAddress))
            //{
            //    string[] addresses = ipAddress.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}

            // string res = context.Request.ServerVariables["REMOTE_ADDR"];
            return HttpContext.Connection.RemoteIpAddress.ToString();
            //return res;



        }

        #endregion


        #region Fund

        [CustomAuthorize]
        public ActionResult AdFundPGW()
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AdFund", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  //Url = Url.Action("AdFundPGW", "user")
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            return View();
        }



        [CustomAuthorize]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AdFundPGW([FromBody]AdFundDtoPGW fundDto)
        {
            if (ModelState.IsValid)
            {
                PaymentGatewayHelperFactory paymentFactory = new PaymentGatewayHelperFactory();
                IPaymentGatewayHelper paymentGatewayHelper = paymentFactory.CreatePaymentHelper(fundDto.PaymentType);
                fundDto.VatAmount = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue * Convert.ToDecimal(fundDto.Amount);
                fundDto.VatAmount = decimal.Round(fundDto.VatAmount, 2, MidpointRounding.AwayFromZero); ;
                int transactionId = paymentGatewayHelper.InitiateTransaction(fundDto.Amount, fundDto.VatAmount);
                return Json( new { redirectURL= paymentGatewayHelper.RedirectToGateWayString(fundDto.Amount + fundDto.VatAmount, transactionId) }, ResourcesUtilities.GetResource("Funds", "AccountHistory"), ResponseStatus.redirect);

            }
            else
            {
           
                return Json("AdFund", ResponseStatus.businessException);
            }
        }
        public  ActionResult GetVatAmountPercentage()
        {
            string VatAmountPercentage = ArabyAds.Framework.Utilities.FormatHelper.FormatPercentage(ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue);
            return Json(new { VatAmountPercentage = VatAmountPercentage });
        }
        public ActionResult GetVatAmountPercentageValue()
        {
            var VatAmountPercentage =ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue;
            return Json(new { VatAmountPercentage = VatAmountPercentage });
        }

        [CustomAuthorize]
        [RequireHttps]
        public ActionResult FundStatus(int id)
        {

            return View();
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("TransactionSuccess", "SiteMapLocalizations"),
                                                  Order = 3
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AdFund", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url = Url.Action("AdFundPGW", "user")
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Edit", "SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Edit", "user")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var tran = _fundTrns.GetFundTransactionById(new ValueMessageWrapper<int> { Value = id });
            var model = new ReceiptViewModel
            {
                Amount = FormatHelper.FormatMoney(tran.Amount),
                VATAmount = FormatHelper.FormatMoney(tran.VATAmount),
                ReceiptNo = tran.NoqoushReceiptNumber,
                TransactionId = tran.TransactionId,
                TransactionDate = tran.TransactionDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat + "  hh:mm")
            };
            return View(model);
        }

        [CustomAuthorize]
        [RequireHttps]
        public ActionResult GetFundStatus(int id)
        {

            
            var tran = _fundTrns.GetFundTransactionById(new ValueMessageWrapper<int> { Value = id });
            var model = new ReceiptViewModel
            {
                Amount = FormatHelper.FormatMoney(tran.Amount),
                VATAmount = FormatHelper.FormatMoney(tran.VATAmount),
                ReceiptNo = tran.NoqoushReceiptNumber,
                TransactionId = tran.TransactionId,
                TransactionDate = tran.TransactionDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat + "  hh:mm")
            };
            return Json(model);
        }


        [CustomAuthorize]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TransactionDone()
        {
            IPaymentGatewayHelperFactory paymentGatewayFactory = new PaymentGatewayHelperFactory();
            IPaymentGatewayHelper paymentGatewayHelper = null;
            if (Request.Query.ContainsKey("transId"))
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("paypal");
            }
            else
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("migs");
            }

            bool hashValidated = paymentGatewayHelper.ValidateTransaction(Request.Query.ToNameValueCollection());

            var model = new ReceiptViewModel();
            PaymentStatus status = null;
            if (hashValidated)
            {
                try
                {
                    status = paymentGatewayHelper.CompletePayment(Request.Query.ToNameValueCollection(), this.ControllerContext);
                }
                catch (BusinessException exception)
                {
                    string errorMessage = string.Empty;

                    foreach (var errorData in exception.Errors)
                    {
                        AddErrorMsgs(errorData.Message);
                        errorMessage = errorMessage + errorData.Message + "<br />";
                    }

                    status = new PaymentStatus() { IsCompleted = false, Message = errorMessage };
                }

            }
            else
            {
                status = new PaymentStatus() { IsCompleted = false, Message = ResourcesUtilities.GetResource("inValidUrl", "PGW") };

                AddErrorMsgs(ResourcesUtilities.GetResource("inValidUrl", "PGW"));
            }

            if (!status.IsCompleted)
            {
                ViewData["TransactionIsCompleted"] = status.IsCompleted;
                ViewData["TransactionFailedMessage"] = status.Message;
                return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/en/user/TransactionDone/?msg=" + status.Message);

            }
            else
            {
                return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/en/user/FundStatus/" + status.TransationID);
            }
        }



        [CustomAuthorize]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TransactionCancel()
        {
            // This page is called now from paypal only
            IPaymentGatewayHelperFactory paymentGatewayFactory = new PaymentGatewayHelperFactory();
            IPaymentGatewayHelper paymentGatewayHelper = null;

            var model = new ReceiptViewModel();

            if (Request.Query.ContainsKey("transId"))
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("paypal");
            }
            else
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("migs");
            }

            PaymentStatus status = paymentGatewayHelper.ClosePayment(Request.Query.ToNameValueCollection());

            model.Title = ResourcesUtilities.GetResource("TransactionCancel", "Titles");

            if (status == null)
            {
                model.Message = string.Format(ResourcesUtilities.GetResource("CanceledTransaction", "PGW"), Request.Query["transId"]);
            }
            else
            {
                model.Message = status.Message;
            }
            return Redirect("https://" + JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString() + "/en/user/TransactionCancel/?msg=" + model.Message);

        }


        #endregion

        #region ImpressionLog
        protected InvitationFilter ImpressionLoggetDefualtFilter()
        {
            InvitationFilter filter = new InvitationFilter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            filter.EmailAddress = string.IsNullOrWhiteSpace(Request.Form["Email"]) ? null : Request.Form["Email"].ToString();
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"].ToString();
            filter.Type = string.IsNullOrWhiteSpace(Request.Form["TypeLog"]) ? 0 : Convert.ToInt32(Request.Form["TypeLog"]);
            return filter;
        }
        protected ImpressionLogCriteria ImpressionLogGetCriteria(InvitationFilter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var DPPartner = _partyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            var criteria = new ImpressionLogCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                DataProviderId = DPPartner != null ? DPPartner.ID : null,
                Type = filter.Type > 0 ? (ImpressionLogType)Enum.ToObject(typeof(ImpressionLogType), filter.Type) : ImpressionLogType.None
            };
            return criteria;
        }


        protected virtual ImpressionLogListResultDto ImpressionLogGetQueryResult(InvitationFilter filter)
        {
            var criteria = ImpressionLogGetCriteria(filter);
            var result = _accountService.ImpressionLogQueryByCratiria(criteria);
            return result;
        }

        protected ImpressionLogListResultDto ImpressionLogLoadData(InvitationFilter filter)
        {

            var result = ImpressionLogGetQueryResult(filter);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            return new ImpressionLogListResultDto()
            {
                Items = items,
                TotalCount = result.TotalCount
            };
        }

        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]

        public virtual ActionResult ImpressionLogs()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("ImpressionLogs","DPP"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View("~/Views/ImpressionLog/Index.cshtml", ImpressionLogLoadData(null));
        }

        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _ImpressionLogs()
        {

            var result = ImpressionLogLoadData(null);
            ViewData["total"] = result.TotalCount;
            return Json( new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        //[CustomAuthorize]
        //[AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        //public async void DownloadImpLogOld(FormCollection collection, int id, string name)
        //{
        //    var DPPartner = _partyService.GetDPPartnerByAccount(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

        //    var impLogItem = _accountService.GetImpressionLogById(id);
        //    impLogItem.Path = impLogItem.Path.Replace(@"\", @"/");


        //    if (impLogItem.Type == Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
        //        name =  name + "_" + "AdMarkupLog" ;
        //    else
        //        name = name + "_" + "ImpressionLog";

        //    var pathimp = HttpUtility.UrlEncode(impLogItem.Path, Encoding.UTF8);
        //    if (impLogItem.Provider.ID != DPPartner.ID)
        //        return;

        //    bool IsPhysical = Convert.ToBoolean(ConfigurationManager.AppSettings["LogImpIsPhysical"]);
        //    string dowloadPath = "";

        //    if (IsPhysical)
        //    {

        //        dowloadPath = ConfigurationManager.AppSettings["LogImpPhysicalPath"];
        //        Response.ContentType = "application/octet-stream";
        //        Response.AddHeader("Content-Disposition", string.Format("attachment; ;filename=" + name + ".csv.gz", ""));
        //        Response.WriteFile(dowloadPath + impLogItem.Path);

        //    }
        //    else
        //    {
        //        dowloadPath = ConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
        //        var dowloadPath2 = ConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";
        //        var cc = new CredentialCache();
        //        cc.Add(
        //            new Uri(ConfigurationManager.AppSettings["LogImpPath"]),
        //            "NEGOTIATE",
        //            new NetworkCredential("hdfs-reader-iadfalconcluster", "", ""));
        //        HttpWebResponse resp = null;
        //        HttpWebRequest req = null;
        //        try
        //        {
        //            Gss.TerminateAndRemoveOverride();
        //            Gss.InitializeAndOverrideApi();

        //            req = (HttpWebRequest)WebRequest.Create(dowloadPath);
        //            req.AllowAutoRedirect = true;
        //            req.Credentials = cc;



        //            try
        //            {


        //                resp = (HttpWebResponse)req.GetResponse();
        //            }
        //            catch (Exception ex)
        //            {
        //                req = (HttpWebRequest)WebRequest.Create(dowloadPath2);
        //                req.AllowAutoRedirect = true;
        //                req.Credentials = cc;
        //                resp = (HttpWebResponse)req.GetResponse();
        //            }
        //        }
        //        finally
        //        {
        //            Gss.TerminateAndRemoveOverride();

        //        }
        //        BinaryReader sr = new BinaryReader(resp.GetResponseStream());

        //        try
        //        {
        //            long startBytes = 0;
        //            string _EncodedData = pathimp;

        //            Response.Clear();
        //            Response.Buffer = false;
        //            Response.AddHeader("Accept-Ranges", "bytes");
        //            Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
        //            Response.ContentType = "application/octet-stream";
        //            Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".csv.gz");
        //            Response.AddHeader("Content-Length", (resp.ContentLength - startBytes).ToString());
        //            Response.AddHeader("Connection", "Keep-Alive");
        //            Response.ContentEncoding = Encoding.UTF8;

        //            //Dividing the data in 1024 bytes package
        //            int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);

        //            //Download in block of 1024 bytes
        //            int i;
        //            for (i = 0; i < maxCount && Response.IsClientConnected; i++)
        //            {
        //                Response.BinaryWrite(sr.ReadBytes(1024));
        //                Response.Flush();
        //            }

        //        }
        //        finally
        //        {
        //            Response.End();
        //            sr.Close();
        //        }
        //    }


        //}




        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public void DownloadImpLog(IFormCollection collection, int id, string name)
        {
            var DPPartner = _partyService.GetDPPartnerByAccount(new ValueMessageWrapper<int> { Value = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });

            var impLogItem = _accountService.GetImpressionLogById(new ValueMessageWrapper<int> { Value = id });
            impLogItem.Path = impLogItem.Path.Replace(@"\", @"/");


            if (impLogItem.Type == Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
                name = name + "_" + "AdMarkupLog";
            else
                name = name + "_" + "ImpressionLog";

            var pathimp = impLogItem.Path /*HttpUtility.UrlEncode(impLogItem.Path, Encoding.UTF8)*/;
            pathimp = HttpUtility.UrlEncode(pathimp, Encoding.UTF8);
            if (impLogItem.Provider.ID != DPPartner.ID)
                return;

            bool IsPhysical = Convert.ToBoolean(JsonConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            if (IsPhysical)
            {

                dowloadPath = JsonConfigurationManager.AppSettings["LogImpPhysicalPath"];
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; ;filename=" + name + ".csv.gz", ""));
                //FileStreamReader strem = new FileStream(;
                //;
                Response.Body.Write(System.IO.File.ReadAllBytes(dowloadPath + impLogItem.Path));

            }
            else
            {
                dowloadPath = JsonConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
                var dowloadPath2 = JsonConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";


                WebHDFSUtil hDFSUtil = new WebHDFSUtil(JsonConfigurationManager.AppSettings["LogImpPath"], JsonConfigurationManager.AppSettings["LogImpPath2"], "", new NetworkCredential(JsonConfigurationManager.AppSettings["WebHDFSUserName"], JsonConfigurationManager.AppSettings["WebHDFSPassword"], JsonConfigurationManager.AppSettings["WebHDFSDomain"]));

                Byte[] contentarr = null;

                // Stream fileOe = new  MemoryStream(myByteArray);
                try
                {
                    contentarr = hDFSUtil.ReadFileByResponse(pathimp);
                }
                finally
                {


                }
                BinaryReader sr = new BinaryReader(new MemoryStream(contentarr));

                try
                {
                    long startBytes = 0;
                    string _EncodedData = pathimp;

                    Response.Clear();
                    //Response.bu.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.Headers.Append("ETag", "\"" + _EncodedData + "\"");
                    Response.ContentType = "application/octet-stream; charset=utf-8";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".csv.gz");
                    Response.AddHeader("Content-Length", (contentarr.Length - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    // Response.ContentEncoding = Encoding.UTF8;

                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((contentarr.Length - startBytes + 0.0) / 1024);
                    Framework.ApplicationContext.Instance.Logger.Debug("maxCount = " + maxCount);
                    //Download in block of 1024 bytes
                    int i;
                    for (i = 0; i < maxCount && !HttpContext.RequestAborted.IsCancellationRequested; i++)
                    {
                        Response.Body.Write(sr.ReadBytes(1024));
                        Response.Flush();
                    }

                }
                finally
                {
                   // Response.End();
                    sr.Close();
                }
            }


        }


        #endregion


        #region AduitTrial

        #region Index
        protected ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter getDefualtFilterForAuditTrial()
        {


            ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            filter.type = string.IsNullOrWhiteSpace(Request.Form["ObjectType"]) ? 0 : Convert.ToInt32(Request.Form["ObjectType"]);
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? string.Empty : Request.Form["Name"].ToString();

            return filter;
        }

        protected AuditTrialCriteria GetAuditTrialCriteria(ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter filter)
        {
            if (filter == null)
                filter = getDefualtFilterForAuditTrial();
            var criteria = new AuditTrialCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Type = filter.type,
                Name = filter.Name,


            };

            criteria.UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            return criteria;
        }
        protected virtual TrialViewModel TrialMainRootGetQueryResult(ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter filter)
        {
            var criteria = GetAuditTrialCriteria(filter);
            var result = _accountService.MainRootTrialQueryByCratiria(criteria);


            return new TrialViewModel
            {
                Items = result.Items,
                TotalCount = result.TotalCount
            };


        }

        protected virtual TrialListViewModel LoadDataTrialMainRoot(ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter filter)
        {
            var result = TrialMainRootGetQueryResult(filter);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            var ObjectTypes = getObjectTypes();
            return new TrialListViewModel()
            {
                ObjectTypes = ObjectTypes,
                Items = items
            };
        }






        #region Actions


        [CustomAuthorize]
        public ActionResult AuditTrial()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Header","Audittrial"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var model = LoadDataTrialMainRoot(null);
            model.ViewName = "AuditTrial";
            return View("Audittrial", model);
        }
        [CustomAuthorize]
        public ActionResult GetAuditTrial()
        {
            var model = LoadDataTrialMainRoot(null);
            model.ViewName = "AuditTrial";
            return Json( model);
        }
        [CustomAuthorize]

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrial()
        {
            var result = TrialMainRootGetQueryResult(null);
            ViewData["total"] = result.TotalCount;
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [CustomAuthorize]
        public ActionResult AuditTrialSessions(int objectRootId, int objectRootTypeId, List<BreadCrumbModel> TraveledBreadCrumbLinks = null, string returnUrl = null)
        {

            //var list = _accountService.GetTrials();
            ArabyAds.AdFalcon.Web.Controllers.Model.User.TrialListViewModel model = new TrialListViewModel();

            model.RootId = objectRootId;
            model.ObjectRootTypeId = objectRootTypeId;
            model.UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            model.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            model.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            // var list = _accountService.GetTrialSessions(objectRootId, userId, objectRootTypeId);
            model.ViewName = "AuditTrialSessions";
            model.Items = new List<TrialDto>();
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            if (TraveledBreadCrumbLinks == null)
            {
                breadCrumbLinks = new List<BreadCrumbModel>
                                      {

                                       new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Header", "Audittrial")/*+": "+data.BusinessName*/,
                                                  Url=Url.Action("AuditTrial"),
                                                  Order = 1,
                                              },
                                                    new BreadCrumbModel()
                                              {
                                                  Text ="Sessions",
                                                  Url=" ",
                                                  Order = 2,
                                              }


                                      };
            }
            else
            {
                TraveledBreadCrumbLinks.Add(new BreadCrumbModel()
                {
                    Text = "Sessions",
                    Url = " ",
                    Order = 2,
                });
                breadCrumbLinks = TraveledBreadCrumbLinks;


            }



            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            ViewData["returnUrl"] = string.IsNullOrEmpty(returnUrl) ? null : returnUrl;

            #endregion
            ViewData["total"] = 0;
            ViewData["AuditTrialSessionType"] = _accountService.GetRootObjectTypeNameValue(new ValueMessageWrapper<int> { Value = objectRootTypeId });
            ViewData["AuditTrialSession"] = _accountService.GetRootObjectName(new GetRootObjectNameRequest { RootObjectTypeID = objectRootTypeId, RootObjectId = objectRootId, ObjectTypeName = _accountService.GetRootObjectTypeName(new ValueMessageWrapper<int> { Value = objectRootTypeId }) });

            return View("AuditTrialSessions", model);
        }
        [CustomAuthorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSessions(int objectRootId, int objectRootTypeId)
        {
            //var list1 = LoadData(null);
            AuditTrialCriteria criteria = new AuditTrialCriteria();

            ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.User.Filter();
            int page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? 0 : Convert.ToInt32(Request.Form["page"]);

            DateTime? FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            DateTime? ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            int total;
            criteria.Page = page;
            criteria.DataFrom = FromDate;
            criteria.DataTo = ToDate;
            criteria.UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.ObjectRootId = objectRootId;
            criteria.Type = objectRootTypeId;
            //criteria.Type = objectRootTypeId;
            criteria.UserName = string.IsNullOrWhiteSpace(Request.Form["UserName"]) ? string.Empty : Request.Form["UserName"].ToString();
            //criteria.Name = name;
            IList<TrialDto> list = new List<TrialDto>();
            GetTrialSessionsResponse res = null;
            try
            {
                res = _accountService.GetTrialSessions(criteria);
            }
            catch (UnauthorizedAccessException ex)
            {
                // to hanle it wieather show it or not
                total = 0;
            }
            return Json(new GridModel
            {
                Data = res.Trials,
                Total = res.Total
            });
        }

        [CustomAuthorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSession(string Id)
        {

            var list = _accountService.GetTrialSession(new GetTrialSessionRequest
            {
                Id = Id,
                IsAdminApp = Config.IsAdministrationApp || Config.IsAppOpsAdmin ||
    Config.IsAdOpsAdmin
            });

            return Json(new GridModel
            {
                Data = list,
                Total = list.Count
            });
            //return PartialView("AuditTrialSessions", list);
        }
        [CustomAuthorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSessionDetails(long Id)
        {

            var list = _accountService.GetTrialDetailsSession(new GetTrialDetailsSessionRequest
            {
                Id = Id,
                IsAdminApp = Config.IsAdministrationApp || Config.IsAppOpsAdmin ||
    Config.IsAdOpsAdmin
            });

            return Json(new GridModel
            {
                Data = list,
                Total = list.Count
            });
            //return PartialView("AuditTrialSessions", list);
        }

        public IEnumerable<SelectListItem> getObjectTypes()
        {
            var list = _accountService.getObjectTypes();
            List<SelectListItem> types = new List<SelectListItem>();

            types.Add(new SelectListItem { Value = "0", Text = "select one" });
            foreach (var item in list)
            {
                types.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.ObjectTypeName });
            }

            return types;

        }
        #endregion
        #endregion
        #endregion


        #region DSPAccountSetting
        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
        public ActionResult DSPAccountSetting()
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("ADMAccountSettings", "Global"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            DSPAccountSettingsModel model = new DSPAccountSettingsModel();
            model.Recipients = new Model.Core.RecipientEmailModel();
            model.Recipients.RecipientEmail = new List<string>();
            model.Setting = _accountService.GetDSPAccountSettingReport(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            if (model.Setting == null)
            {
                model.Setting = new AccountDSPsettingDTO();
                model.Setting.AllContacts = new List<AccountDSPsettingContactDTO>();

            }
            else
            {
                if (model.Setting.AllContacts != null && model.Setting.AllContacts.Count() > 0)
                {
                    foreach (var item in model.Setting.AllContacts)
                    {
                        model.Recipients.RecipientEmail.Add(item.Email);
                    }
                }
            }
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
            model.Countries = new List<SelectListItem>();

            model.Countries.Add(new SelectListItem
            {
                Value = "",
                Text = ResourcesUtilities.GetResource("Select")
            });
            model.Cities = new List<SelectListItem>();

            foreach (var item in countriesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();

                if (model.Setting.CountryId == item.ID)
                {
                    selectItem.Selected = true;
                }

                model.Countries.Add(selectItem);
            }


            return View("~/Views/User/DSPAccount/Settings.cshtml", model);


        }

        [CustomAuthorize]
        [HttpPost]
        [AuthorizeRole(Roles = "Administrator")]
        public ActionResult DSPAccountSetting(DSPAccountSettingsModel model)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("AccountSettings", "Titles"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            try
            {
                model.Setting.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                if (!string.IsNullOrEmpty(model.RecipientsString))
                {
                    model.Recipients = new Model.Core.RecipientEmailModel();
                    model.Recipients.RecipientEmail = model.RecipientsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToList();
                    if (model.Recipients.RecipientEmail != null && model.Recipients.RecipientEmail.Count > 0)
                    {
                        var AllContacts = model.Setting.AllContacts;
                        model.Setting.AllContacts = new List<AccountDSPsettingContactDTO>();
                        if (AllContacts != null && AllContacts.Count > 0)
                        {
                            foreach (var item in AllContacts)
                            {
                                var email = model.Recipients.RecipientEmail.Where(p => p == item.Email).FirstOrDefault();
                                if (email != null)
                                {
                                    model.Setting.AllContacts.Add(item);
                                    model.Recipients.RecipientEmail.Remove(email);
                                }
                            }
                        }
                        foreach (var item in model.Recipients.RecipientEmail)
                        {
                            model.Setting.AllContacts.Add(new AccountDSPsettingContactDTO
                            {
                                AccountSettingId = model.Setting.ID,
                                Email = item,
                            });

                        }
                    }
                }
                _accountService.SaveDSPAccountSettingReport(model.Setting);
                AddSuccessfullyMsg();

            }
            catch (BusinessException exception)
            {
                foreach (var errorData in exception.Errors)
                {
                    AddMessages(errorData.Message, MessagesType.Error);
                }
            }
            finally
            {

                #region countries
                List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
                model.Countries = new List<SelectListItem>();

                model.Countries.Add(new SelectListItem
                {
                    Value = "-1",
                    Text = ResourcesUtilities.GetResource("Select")
                });
                model.Cities = new List<SelectListItem>();

                foreach (var item in countriesDtos)
                {
                    var selectItem = new SelectListItem();
                    selectItem.Value = item.ID.ToString();
                    selectItem.Text = item.Name.ToString();

                    if (model.Setting.CountryId == item.ID)
                    {
                        selectItem.Selected = true;
                    }

                    model.Countries.Add(selectItem);
                }
                #endregion

            }

            return View("~/Views/User/DSPAccount/Settings.cshtml", model);
        }

        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]

        public ActionResult GetADMAccountSettings()
        {


            DSPAccountSettingsModel model = new DSPAccountSettingsModel();
            model.Recipients = new Model.Core.RecipientEmailModel();
            model.Recipients.RecipientEmail = new List<string>();
            model.Setting = _accountService.GetDSPAccountSettingReport(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            if (model.Setting == null)
            {
                model.Setting = new AccountDSPsettingDTO();
                model.Setting.AllContacts = new List<AccountDSPsettingContactDTO>();

            }
            else
            {
                if (model.Setting.AllContacts != null && model.Setting.AllContacts.Count() > 0)
                {
                    foreach (var item in model.Setting.AllContacts)
                    {
                        model.Recipients.RecipientEmail.Add(item.Email);
                    }
                }
            }
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
            model.Countries = new List<SelectListItem>();

            model.Countries.Add(new SelectListItem
            {
                Value = "",
                Text = ResourcesUtilities.GetResource("Select")
            });
            model.Cities = new List<SelectListItem>();

            foreach (var item in countriesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();

                if (model.Setting.CountryId == item.ID)
                {
                    selectItem.Selected = true;
                }

                model.Countries.Add(selectItem);
            }


            return Json( model);


        }

        [CustomAuthorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]

        public ActionResult ADMAccountSettings()
        {

            //return View("~/Views/User/DSPAccount/Settings.cshtml");
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("ADMAccountSettings", "Global"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            DSPAccountSettingsModel model = new DSPAccountSettingsModel();
            model.Recipients = new Model.Core.RecipientEmailModel();
            model.Recipients.RecipientEmail = new List<string>();
            model.Setting = _accountService.GetDSPAccountSettingReport(new ValueMessageWrapper<int> { Value = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            if (model.Setting == null)
            {
                model.Setting = new AccountDSPsettingDTO();
                model.Setting.AllContacts = new List<AccountDSPsettingContactDTO>();

            }
            else
            {
                if (model.Setting.AllContacts != null && model.Setting.AllContacts.Count() > 0)
                {
                    foreach (var item in model.Setting.AllContacts)
                    {
                        model.Recipients.RecipientEmail.Add(item.Email);
                    }
                }
            }
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
            model.Countries = new List<SelectListItem>();

            model.Countries.Add(new SelectListItem
            {
                Value = "",
                Text = ResourcesUtilities.GetResource("Select")
            });
            model.Cities = new List<SelectListItem>();

            foreach (var item in countriesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();

                if (model.Setting.CountryId == item.ID)
                {
                    selectItem.Selected = true;
                }

                model.Countries.Add(selectItem);
            }


            return View("~/Views/User/DSPAccount/Settings.cshtml", model);


        }

        [CustomAuthorize]
        [HttpPost]
        [AuthorizeRole(Roles = "Administrator")]
        public ActionResult ADMAccountSettings(DSPAccountSettingsModel model)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("AccountSettings", "Titles"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            try
            {
                model.Setting.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                if (!string.IsNullOrEmpty(model.RecipientsString))
                {
                    model.Recipients = new Model.Core.RecipientEmailModel();
                    model.Recipients.RecipientEmail = model.RecipientsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToList();
                    if (model.Recipients.RecipientEmail != null && model.Recipients.RecipientEmail.Count > 0)
                    {
                        var AllContacts = model.Setting.AllContacts;
                        model.Setting.AllContacts = new List<AccountDSPsettingContactDTO>();
                        if (AllContacts != null && AllContacts.Count > 0)
                        {
                            foreach (var item in AllContacts)
                            {
                                var email = model.Recipients.RecipientEmail.Where(p => p == item.Email).FirstOrDefault();
                                if (email != null)
                                {
                                    model.Setting.AllContacts.Add(item);
                                    model.Recipients.RecipientEmail.Remove(email);
                                }
                            }
                        }
                        foreach (var item in model.Recipients.RecipientEmail)
                        {
                            model.Setting.AllContacts.Add(new AccountDSPsettingContactDTO
                            {
                                AccountSettingId = model.Setting.ID,
                                Email = item,
                            });

                        }
                    }
                }
                _accountService.SaveDSPAccountSettingReport(model.Setting);
                AddSuccessfullyMsg();

            }
            catch (BusinessException exception)
            {
                foreach (var errorData in exception.Errors)
                {
                    AddMessages(errorData.Message, MessagesType.Error);
                }
            }
            finally
            {

                #region countries
                List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
                model.Countries = new List<SelectListItem>();

                model.Countries.Add(new SelectListItem
                {
                    Value = "-1",
                    Text = ResourcesUtilities.GetResource("Select")
                });
                model.Cities = new List<SelectListItem>();

                foreach (var item in countriesDtos)
                {
                    var selectItem = new SelectListItem();
                    selectItem.Value = item.ID.ToString();
                    selectItem.Text = item.Name.ToString();

                    if (model.Setting.CountryId == item.ID)
                    {
                        selectItem.Selected = true;
                    }

                    model.Countries.Add(selectItem);
                }
                #endregion

            }

            return View("~/Views/User/DSPAccount/Settings.cshtml", model);
        }
        [CustomAuthorize]
        public ActionResult GetCitiesByCountry(int countryId)
        {

            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.LocationDto> CitiesDtos = _countryService.GetAllStates(new ValueMessageWrapper<int> { Value = countryId }).ToList().OrderBy(p => p.Name.Value).ToList();
            List<SelectListItem> Cities = new List<SelectListItem>();
            foreach (var item in CitiesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();

                Cities.Add(selectItem);
            }
            return Json(new { status = "1", result = Cities });

        }


        #endregion

        private void BuildAdFalconUser(int accountId, int userId, bool switchAccountSet = true)
        {
            var userInof = _accountService.BuildAdFalconUser(new BuildAdFalconUserRequest { AccountId = accountId, UserId = userId, Email = OperationContext.Current.CurrentPrincipal.Identity.Name });
            if (userInof != null)
            {
                userInof.SwitchAccountSet = switchAccountSet;
                _userService.SetAccountUser(new ValueMessageWrapper<int> { Value = accountId });
                _userService.UpdateOperationContext(userInof);
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInof);
                _userService.InsertLastLoginDateAuditTrial(OperationContext.Current.CurrentPrincipal.Identity.Name);
            }
        }

        #region Feature
        public void SetFeature(int code)
        {
            _accountService.SetFeature(new ValueMessageWrapper<int> { Value = code });
        }

        public ActionResult HadAFeature(int code)
        {
            try
            {
                bool HadAFeature = _accountService.HadAFeature(new ValueMessageWrapper<int> { Value = code }).Value;
                return Json(new { Result = HadAFeature });

            }
            catch (Exception)
            {

                return Json(new { Result = false });

            }

        }

        [RequireHttps(Order = 1)]
        public ActionResult HadAFeatureHttps(int code)
        {
            try
            {
                bool HadAFeature = _accountService.HadAFeature(new ValueMessageWrapper<int> { Value = code }).Value;
                return Json(new { Result = HadAFeature });

            }
            catch (Exception)
            {

                return Json(new { Result = false });

            }

        }




        #endregion

        [RequireHttps]
        public ActionResult GetAdvertiserAccountReadOnlySettings(int userId)
        {
            AdvertiserAccountSettingsForReadOnly searcher = new AdvertiserAccountSettingsForReadOnly();


            searcher.UserId = userId;
            return Json(new { status = "1", result = _userService.GetAdvertiserAccountReadOnlySettings(searcher) });
        }

        public ActionResult GetExternalDataProviderQueryResultAllResultActionResult()
        {
           var oPartyDtoList = GetExternalDataProviderQueryResultAllResult();
            return Json(new { status = "1", result = oPartyDtoList });
        }
        public ActionResult GetUserTypes()
        {
            var selectItems = Enum.GetValues(typeof(ArabyAds.AdFalcon.Domain.Common.Model.Account.UserType)).
                Cast<ArabyAds.AdFalcon.Domain.Common.Model.Account.UserType>().Select(p => new SelectListItem()
                { Text = p.ToText(), Value = ((int)p).ToString() }).ToList();
            selectItems.RemoveAt(0);

            return Json(selectItems);
        }

        public ActionResult GetUserAgreementEffectiveDate()
        {
            bool _UserAgreementEffectiveDate ;
            if ((ArabyAds.Framework.OperationContext.Current.UserInfo
        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
        ().AccountRole != (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP))
            {
                _UserAgreementEffectiveDate = DateTime.Now < ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.DSPUserAgreementEffectiveDate;
            }
            else
            {
                _UserAgreementEffectiveDate = DateTime.Now < ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.UserAgreementEffectiveDate;
            }

                return Json(_UserAgreementEffectiveDate);
        }


    }
}
