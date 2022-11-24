using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Noqoush.AdFalcon.Business.Domain.Exceptions;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Exceptions.Account;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Payment;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.Pgw;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Security;
using Noqoush.Framework.Utilities;
using Noqoush.Framework.Utilities.EmailsSender;
using Noqoush.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;
using Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using System.Web.Routing;
using Noqoush.Framework.Logging;

using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System.Globalization;
using Noqoush.AdFalcon.Services;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using System.Configuration;
using System.IO;
using System.Net;
using GSSAPI;
using Noqoush.AdFalcon.Web.Controllers.Model.AccountManagement;
using Noqoush.AdFalcon.Domain.Common.Model.Account.DPP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Domain.Utilities;

//using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
//using WebMatrix.WebData;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    //  [DenyRole(Roles = "AppOps", DenyImpersonationOnly = true)]
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

        public UserController(IFundsService fundsService,
            ISecurityService securityService,
            IAccountService accountService,
            IUserService userService,
            IMailSender mailSender,
            IFundTransactionService fundTrns,
            ILanguageService languageService,
            IPartyService PartyService,
            IDocumentTypeService documentTypeService,
                     ICountryService CountryService,
            IAdvertiserService AdvertiserSearcher
           )
        {
            _securityProxy = new SecurityManager(securityService);
            _accountService = accountService;
            _userService = userService;
            _mailSender = mailSender;
            _FundsService = fundsService;
            _fundTrns = fundTrns;
            _languageService = languageService;
            _partyService = PartyService;
            _DocumentTypeService = documentTypeService;
            _countryService = CountryService;
            _AdvertiserSearcher = AdvertiserSearcher;
        }

        const string WEBHDFS_CONTEXT_ROOT = "/webhdfs/v1";
        //public ActionResult HelloAnas()
        //{



        //    getdeegaion("", @"D:\hadoob\OPENX3.PNG");



        //    return null;
        //}


        public static void getdeegaion(string sourcePath, string targetPath)
        {

            var dowloadPath = "http://hd01.iadfalcon.com:50070/" + WEBHDFS_CONTEXT_ROOT + sourcePath + "?op=GETDELEGATIONTOKEN";
            var downloadPath2 = "http://hd01.iadfalcon.com:50070/webhdfs/v1/dw/adfalcon/data-providers/headers/impressions-log-header.csv?op=OPEN";

            long startBytes = 0;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(downloadPath2);






            //Kerberos.NET.KerberosAuthenticator gg = new Kerberos.NET.KerberosAuthenticator(new Kerberos.NET.Crypto.KeyTable(bytes));


            //var c2 = gg.Authenticate(File.ReadAllBytes(@"d:\test\kk5cache"));
            //var c = gg.Authenticate(Encoding.ASCII.GetBytes("hadoopadmin@HDP.NET" + ":" + "P@ssw0rd112233")).Result;
            var cc = new CredentialCache();
            cc.Add(
                new Uri("http://hd01.iadfalcon.com:50070/"),
                "NEGOTIATE", //if we don't set it to "Kerberos" we get error 407 with ---> the function requested is not supported.
                new NetworkCredential("hdfs-reader-iadfalconcluster", "", ""));
            req.Credentials = cc;
            req.PreAuthenticate = true;
            req.UnsafeAuthenticatedConnectionSharing = true;

            req.AllowAutoRedirect = true;
            HttpWebResponse resp = null;
            try
            {
                Gss.InitializeAndOverrideApi();
                resp = (HttpWebResponse)req.GetResponse();
            }
            finally
            {
                Gss.TerminateAndRemoveOverride();

            }
            // BinaryReader sr = new BinaryReader(resp.GetResponseStream());
            using (Stream output = System.IO.File.OpenWrite(@"D:\hadoob\OPENX3.PNG"))
            using (Stream input = resp.GetResponseStream())
            {
                input.CopyTo(output);
            }

            int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);
            int i;


        }
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

            if (!string.IsNullOrWhiteSpace(Request.Form["Agree"]))
            {
                //user agreed on user agreement
                _userService.UpdateAgreement();
              
               
                //  OperationContext.Current.UserInfo<AdFalconUserInfo>(userInof)
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("index", "dashboard", new { charttype = "ad" });
            }
            return Redirect(returnUrl);
        }

        public ActionResult GetVATAmount(int Amount)
        {


            var result = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().VATValue * Amount;



            return Content(result.ToString("F2"));



        }
        [Authorize]
        public ActionResult DSPUserAgreement(string returnUrl)
        {
            if (!isDSP())
            {
                throw new UnauthorizedAccessException();
            }
           
            return View();
           // return null;
        }
        [Authorize]
        [HttpPost]
        public ActionResult DSPUserAgreement(string returnUrl, string dummy)
        {
            if (!isDSP())
            {
                 throw new UnauthorizedAccessException();
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Agree"]))
            {
                //user agreed on user agreement
                _userService.UpdateAgreement();
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("index", "dashboard", new { charttype = "ad" });
            }
            return Redirect(returnUrl);
        }

        #region Login
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("PublicInfo");
        }


        [RequireHttps(Order = 1)]
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult LogoutHttps()
        {
            FormsAuthentication.SignOut();
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
                FormsAuthentication.SignOut();
                return Redirect(Request.RawUrl);
            }
            if (Request.Cookies["rem"] != null)
            {
                ViewData["rememberMe"] = Request.Cookies["rem"].Value;
            }
            if (!string.IsNullOrEmpty(method) && method.ToLower() == "reset")
            {
                ModelState.AddModelError("NoUserNameError",
                                              ResourcesUtilities.GetResource("InvalidToken", "ForgetPassword"));
            }
            ViewBag.HideMenu = true;
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultAction, Config.DefaultController);
            }
            ViewData.Add("gsdfg", "gsdfg");
            return View();
        }


        [HttpPost]
        [RequireHttps(Order = 1)]
        [DenyRole(Roles = "", DenyImpersonationOnly = true)]
        public ActionResult Login(LoginInfo loginInfo, bool rememberMe, string returnUrl)
        {
            #region BreadCrumb

            ArrayList s = new ArrayList();
            s.Add(5);
            s.Add(6);

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Login", "SiteMapLocalizations"),
                Order = 1,
                Url = Url.Action("login", "user")
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (ModelState.IsValid)
            {
                AuthenticateResponse response;
                string userName = loginInfo.Username.Trim();

                response = _securityProxy.AuthenticateUser(userName, loginInfo.Password);

                if (response.Status == AuthenticateStatus.Success)
                {
                    var isUserBlocked = _userService.IsUserBlockedByEmail(userName);

                    if (isUserBlocked)
                    {
                        string errormessage1 = ResourcesUtilities.GetResource("UserNameBlocked", GlobalErrors);
                        ModelState.AddModelError("LogIn", errormessage1);
                        ViewBag.HideMenu = true;

                        return View(loginInfo);
                    }
                    LoginUser(userName, response.Principal.Token);

                    // UserDomainManager geteuser = new UserDomainManager(null);

                    UserDto user_info = _userService.GetUserByEmail(userName, true);

                    var lang = _languageService.GetAll().Where(x => x.ID == user_info.Language).FirstOrDefault();
                    RouteData.Values["language"] = lang.Code;

                    if (rememberMe)
                    {
                        HttpCookie rememberMeCookie = new HttpCookie("rem", loginInfo.Username)
                        {
                            Expires = Framework.Utilities.Environment.GetServerTime().AddDays(30)
                        };

                        if (!Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
                        {
                            rememberMeCookie.Domain = Config.CookieDomain;
                        }

                        Response.Cookies.Add(rememberMeCookie);
                    }
                    else
                    {
                        if (Request.Cookies["rem"] != null)
                        {
                            Response.Cookies["rem"].Expires = Framework.Utilities.Environment.GetServerTime().AddDays(-30);
                        }
                    }

                    if (_accountService.GetUserAccountsCount(user_info.Id) > 1)
                    {
                        var accId = _userService.getAccountUser();

                        if (accId > 0)
                        {
                            BuildAdFalconUser(accId, user_info.Id, true);
                            // return RedirectToAction("SwitchAccount", new { returnUrl = returnUrl });
                        }
                        //to be removed
                        else
                        {
                            //return RedirectToAction("SwitchAccount", new { returnUrl = returnUrl });
                            var accId2 = _accountService.GetFirstUserAccountId(user_info.Id);
                            BuildAdFalconUser(accId2, user_info.Id, true);
                        }

                        return ContinueLogin(returnUrl);
                    }
                    else
                    {

                        _userService.InsertLastLoginDateAuditTrial(loginInfo.Username);


                        return ContinueLogin(returnUrl);
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
                            errormessage = ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = loginInfo.Username }));
                        }
                        else
                        {
                            errormessage = ResourcesUtilities.GetResource("ChangedEmailIsntActivated", GlobalErrors).Replace("[pendingemail]", pendingEmailAddress);
                        }
                    }

                    ModelState.AddModelError("LogIn", errormessage);
                    ViewBag.HideMenu = true;

                    return View(loginInfo);
                }
            }

            return View(loginInfo);
        }

        public ActionResult ContinueLogin(string returnUrl)
        {
            //check user agreement version
            var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            if (user.UserAgreementVersion != Config.UserAgreementVersion && user.AccountRole != (int)AccountRole.DSP)
            {
                return RedirectToAction("UserAgreement", "User", new { returnUrl = returnUrl });
            }
            else if (user.UserAgreementVersion != Config.DSPUserAgreementVersion && user.AccountRole == (int)AccountRole.DSP)
            {
                return RedirectToAction("UserAgreement", "User", new { returnUrl = returnUrl });

            }
            else if (user.AccountRole == (int)AccountRole.DataProvider)
            {
                return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });

            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("index", "dashboard", new { charttype = "ad" });
                }
                else
                {
                    return Redirect(returnUrl);
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
            filter.EmailAddress = string.IsNullOrWhiteSpace(Request.Form["Email"]) ? null : Request.Form["Email"];
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



        [Authorize]
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

            #endregion
            return View(LoadData(null));
        }
        [GridAction(EnableCustomBinding = true)]

        [Authorize]
        [DenyNonPrimaryRole]
        public virtual ActionResult _Invitation()
        {

            var result = GetQueryResult(null);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [Authorize]
        [DenyNonPrimaryRole]
        public string GetInviteEmail(string invitationcode)
        {
            string emailTemplate = ResourcesUtilities.GetResource("AccountInvitation", "Emails");
            emailTemplate = emailTemplate.Replace("[url]","http://www.adfalcon.com/User/Register?id="+ invitationcode);
            emailTemplate = emailTemplate.Replace("[account]", OperationContext.Current.UserInfo<AdFalconUserInfo>().FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().LastName);
            emailTemplate = emailTemplate.Replace("[Year]", Noqoush.Framework.Utilities.Environment.GetServerTime().Year.ToString());
            return emailTemplate;
        }

        [Authorize]
        [DenyNonPrimaryRole]
        public ActionResult invite(string email,  string Ids, UserType? userType)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    throw new Exception("empty string");
                string invitationcode;
                bool result = _userService.invite(email, userType!=null? userType.Value: UserType.Normal, Ids, out invitationcode);
           

       

                if (result)
                {

                    string subject = ResourcesUtilities.GetResource("AccountInvitation", "Invite");
                    _mailSender.SendEmail("", "", "", email, subject, GetInviteEmail(invitationcode), true, "");


                }

                return Json(new { status = "", result = result });

            }
            catch (Exception e)
            {

                return Json(new { status = e.Message, result = false });

            }

        }
        [Authorize]
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
                , ToolTips = getMyUsersTips()
            };

            ViewBag.isAdmin = true;

            //ViewData["AccountId"] = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            return View(model);

        }

        protected virtual List<Model.Action> getMyUsersTips()
        {


            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
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
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("ResendEmailActivation", "SiteMapLocalizations"),
                Order = 1
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            UserDto userInfo = _userService.GetUserByEmail(email, false);

            if (userInfo.ActivationCode != "1")
            {
                _mailSender.SendEmail("", "", userInfo.EmailAddress, userInfo.EmailAddress, ResourcesUtilities.GetResource("Registration", "EmailHeader"), GetActivationEmail(userInfo.ActivationCode));
                AddMessages(ResourcesUtilities.GetResource("SuccessResend", "Resend"), MessagesType.Success);

            }
            else
            {
                AddMessages(ResourcesUtilities.GetResource("AlreadyActivated", "Resend"), MessagesType.Warning);
            }

            return View();
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
                return RedirectToAction(Config.DefaultAction, Config.DefaultController);
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


        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("ForgetPassword", "SiteMapLocalizations"),
                Order = 1,
                Url = Url.Action("forgetpassword", "user")
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultAction, Config.DefaultController);
            }

            if (ModelState.IsValid)
            {
                //string newPassword = Guid.NewGuid().ToString().Substring(0, 4) + (new Random(100).Next(1000));
                //newPassword = newPassword + GenerateCouponSpecialChracter(1);
                //newPassword = newPassword + GenerateCouponUPPER(1);

                int tokenExpirationInMinutesFromNow = 1440; // 24 hour to toke Expiration
                string token = "";// WebSecurity.GeneratePasswordResetToken(email,  tokenExpirationInMinutesFromNow );
                
                // generate unique token
                DateTime currentDate = Noqoush.Framework.Utilities.Environment.GetServerTime();
                Guid obj = Guid.NewGuid();
                string ticks = currentDate.Ticks.ToString();
                token = obj + "_" + ticks;

                var lnkHref = "<a href='" + Url.Action("ResetPassword", "User", new { token = token }, "http") + "'>" + ResourcesUtilities.GetResource( "ResetPassword", "ForgetPassword") + "</a>";
                string emailTemplate =  ResourcesUtilities.GetResource("ResetPasswordEmail", "ForgetPassword");
                emailTemplate = emailTemplate.Replace("[link]", lnkHref);

                try
                {
                    bool resetValue = _userService.SaveUserToken(email, token);
                    //bool resetValue = _userService.SaveUserToken(email, newPassword);
                    if (!resetValue)
                    {
                        ModelState.AddModelError("NoUserNameError",
                                                 ResourcesUtilities.GetResource("NoUserNameError", "ForgetPassword"));
                    }
                    else
                    {
                        _mailSender.SendEmail("", "", email, email,
                                              ResourcesUtilities.GetResource("ForgetPassword", "EmailHeader"),
                                              emailTemplate);
                        AddMessages(ResourcesUtilities.GetResource("SuccessForgetPassword2", "ForgetPassword"),
                                    MessagesType.Success);
                    }
                }
                catch (NotActivatedUserException exception)
                {
                    ModelState.AddModelError("LogIn", ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = email })));
                }


            }

            return View();
        }



        public ActionResult ResetPassword( string token)
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
                bool resetValue = _userService.CheckUserToken(token);
                //bool resetValue = _userService.SaveUserToken(email, newPassword);
                if (!resetValue)
                {
                    ModelState.AddModelError("NoUserNameError",
                                             ResourcesUtilities.GetResource("InvalidToken", "ForgetPassword"));
                    return RedirectToAction("Login", new {method="reset" });
                }
                else
                {
                    oResetPasswordInfo.Token = token ;
                }
            }
            catch (NotActivatedUserException exception)
            {
                ModelState.AddModelError("LogIn", ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user", new { email = "email" })));
            }

            return View(oResetPasswordInfo);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordInfo oResetPasswordInfo)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    bool resetValue = _userService.ResetUserPasswordByToken(oResetPasswordInfo.Token, oResetPasswordInfo.Password);
                    if (!resetValue)
                    {
                        ModelState.AddModelError("NoUserNameError",
                                                 ResourcesUtilities.GetResource("NoUserNameError", "ForgetPassword"));
                    }
                    else
                    {
                        AddMessages(ResourcesUtilities.GetResource("SuccessPasswordChange", "ForgetPassword"),
                                    MessagesType.Success);
                    }
                }
                catch (NotActivatedUserException exception)
                {
                    ModelState.AddModelError("LogIn", ResourcesUtilities.GetResource("UserIsntActivated", GlobalErrors).Replace("activationlink", Url.Action("resend", "user")));
                }


            }
            return RedirectToAction("Login");
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
        public ActionResult Register(string id = null, string requestCode = null)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Register", "SiteMapLocalizations"),
                Order = 1,
                Url = Url.Action("Register", "user")
            });
            #endregion

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            ViewData["Invitationcode"] = string.Empty;
            ViewData["AcceptTermsANdCondition"] = false;

            UserDto account = new UserDto();
            int? Country = GetCountryByIpAddres();
            account.Country = Country != null ? (int)Country : 0;

            string Title = ResourcesUtilities.GetResource("Register", "Titles");
            if (!string.IsNullOrEmpty(id))
            {
               var invitation= _userService.GetInvitation(id);
                string email = invitation.EmailAddress;
                string CompanyNName = invitation.CompanyName;
                account.Company = CompanyNName;
                ViewData["Invitationcode"] = id;
                if (!string.IsNullOrEmpty(email))
                {
                    ViewData["email"] = email;
                    ViewData["CompanyName"] = CompanyNName;
                    if (  _userService.InvitationAcceptedCountByCode(id) ==0&&   _userService.CheckInvitationAlreadyRegistred(email, id))
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
                    ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

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
                    ViewData["AcceptTermsANdCondition"] = null;
                    return View(account);
                }
                else
                {
                    ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));


                    ViewBag.HideMenu = true;
                    ViewData["AcceptTermsANdCondition"] = null;
                }

            }
            ViewData["Title"] = Title;
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultAction, Config.DefaultController);
            }

            return View(account);
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
                return Json(new { Success = true, Message = countryOb }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RequireHttps]
        public ActionResult Register(UserDto userDto)
        {
            #region BreadCrumb

            List<BreadCrumbModel> breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks.Add(new BreadCrumbModel()
            {
                Text = ResourcesUtilities.GetResource("Register", "SiteMapLocalizations"),
                Order = 1,
                Url = Url.Action("Register", "user")
            });

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return RedirectToAction(Config.DefaultAction, Config.DefaultController);
            }
            string Title = ResourcesUtilities.GetResource("CompleteRegistration", "UserInformation");
            if (!string.IsNullOrEmpty(userDto.requestCode))
            {
                ViewData["requestCode"] = userDto.requestCode;
                ViewData["email"] = userDto.EmailAddress;
            }
            if (ModelState.IsValid)
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
                            ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                            ViewData["Title"] = Title;
                            ViewBag.HideMenu = true;
                            ViewData["AcceptTermsANdCondition"] = true;
                            return View(userDto);
                        }


                    }
                    userDto.IPAddress = GetIPAddress();
                    userDto = _accountService.CreateUserAccount(userDto);

                    if (!string.IsNullOrEmpty(Invitationcode))
                    {
                        InvitationAccepted(Invitationcode);
                        return RedirectToAction("Activation", new { hashing = "Invited" });
                    }
                    if (!string.IsNullOrEmpty(userDto.ActivationCode))
                    {
                        _mailSender.SendEmail("", "", userDto.EmailAddress, userDto.EmailAddress, ResourcesUtilities.GetResource("Registration", "EmailHeader"), GetActivationEmail(userDto.ActivationCode));

                        return RedirectToAction("thankyou", "misc", new { resourceKeyName = "Registration", title = "SuccessRegistration", login = true });
                    }


                    return RedirectToAction("Activation", new { hashing = "Invited" });


                }
                catch (UserEmailAlreadyExistsException)
                {
                    ModelState.AddModelError("EmailAlreadyExists", ResourcesUtilities.GetResource("EmailAlreadyExists", "Errors"));

                    ViewData["Title"] = Title;
                    ViewBag.HideMenu = true;
                    ViewData["AcceptTermsANdCondition"] = true;
                }
            }

            return View(userDto);
        }

        [RequireHttps]

        public ActionResult generateBuyerId()
        {
            int? buyerid;

            try
            {
                buyerid = _userService.GetAccountBuyerCounter();

            }
            catch (Exception e)
            {

                throw e;
            }
            return Json(new { value = (int)buyerid });
        }

        private void InvitationAccepted(string Invitationcode)
        {
            try
            {
                var Invitation = _userService.GetInvitation(Invitationcode);
                string subject = ResourcesUtilities.GetResource("InvitationAcceptedSubject", "Emails");
                string email = _accountService.GetAccountEmailAddress(Invitation.accountid);
                _mailSender.SendEmail("", "", email, email, subject, GetInvitationAcceptedEmail(Invitation.EmailAddress), true, "");

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public ActionResult Activation(string activationCode, string hashing, string type)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == false && string.IsNullOrEmpty(type))
            {
                try
                {
                    UserDto userDtoInfo = null;
                    if (!string.IsNullOrEmpty(activationCode))
                        userDtoInfo = _userService.ActivateUser(activationCode);

                    if (userDtoInfo != null || (string.IsNullOrEmpty(activationCode) && hashing == "Invited"))
                    {

                        AddMessages(ResourcesUtilities.GetResource("Content", "Activation").Replace("[url]",
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
                                    MessagesType.Success);

                        return View();
                    }
                }
                catch (DataNotFoundException)
                {
                    RedirectToAction("login");
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

            return RedirectToAction(Config.DefaultAction, Config.DefaultController);
        }

        [Authorize]
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
                userInfo = _userService.GetUserById(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                ViewData.Model = userInfo;
            }
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true && id.HasValue)
            {
                UserDto userInfo = null;
                userInfo = _userService.GetUserById(id.Value);// _userService.GetUserByEmail(OperationContext.Current.CurrentPrincipal.Identity.Name);
                userInfo.MyUsers = true;
                ViewData.Model = userInfo;

                var userInfoOb = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                userInfoOb.SubUserId = id.Value;
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfoOb);
            }
            return View();
        }

        [Authorize]
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
                                                                             new { activationCode = changeEmail.ActivationCode, hashing = changeEmail.Hashing, type = "email" }, "http"));

                            _mailSender.SendEmail("", "", userInfo.EmailAddress, userInfo.EmailAddress, ResourcesUtilities.GetResource("ChangeEmail", "EmailHeader"), emailTemplate);
                        }

                    }

                    List<ModelState> modelErrors = ModelState.Values.Where(p => p.Errors.Count() == 1).ToList();

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

        [Authorize]
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

            UserDto userInfo = _userService.GetUserById(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);
            ViewBag.BuyerCode = userInfo.buyerCode;
            ViewBag.BuyerId = userInfo.buyerId;

            if (successful)
            {
                // AddMessages(string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("buyer", "account")), MessagesType.Success);
                AddSuccessfullyMsg();
            }

            return View();
        }
        [Authorize]
        [HttpPost]
        [RequireHttps]
        [AuthorizeRole(Roles = "Administrator")]

        public ActionResult Buyer(string buyerCode, int? buyerId)
        {

            _userService.SaveAccountBuyer(buyerCode, buyerId);


            return RedirectToAction("Buyer", "User", new { successful = true });
        }

   

        [Authorize]
        [RequireHttps]

        public ActionResult CheckduplicateBuyer(string buyerCode)
        {
            if (string.IsNullOrEmpty(buyerCode))
                return Json(new { result = false });

            bool result = _userService.CheckduplicateBuyer(buyerCode);

            return Json(new { result = result });
        }


        public ActionResult MD5Encryptiontest(string test)
        {
            return  Json( _userService.MD5Encryptiontest(test), JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Delete(FormCollection collection)
        {
            _userService.DeleteUser(OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);

            return RedirectToAction("login", "user", new { method = "logout" });

        }


        [RequireHttps(Order = 1)]
        public ActionResult CheckEmailAddress(string emailAddress)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                UserDto userInfo = _userService.GetUserByEmail(emailAddress, true);

                UserDto LogedinUserInfo = _userService.GetUserByAccount(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value, Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);

                if (userInfo == null || userInfo.Id == LogedinUserInfo.Id || userInfo.Id == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId.HasValue)
                    {
                        if (userInfo.Id == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId.Value && Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().SubUserId>0)
                        {
                            return Json(true, JsonRequestBehavior.AllowGet);

                        }
                    }
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = _userService.CheckUserEmail(emailAddress, true);
                //if (result)
                //{
                //    result= _userService.InvitationCount(emailAddress)>0 && _userService.InvitationAcceptedCount(emailAddress)==0;
                //    if (result)
                //    {//pass validation
                //        result = false;
                //    }
                //}
                return Json(!(result), JsonRequestBehavior.AllowGet);
            }

        }

        //[RequireHttps(Order = 1)]
        public ActionResult CheckAccountDSPEmailAddress(string emailAddress)
        {
            return Json(!_userService.CheckAcccountDSPEmail(emailAddress), JsonRequestBehavior.AllowGet);
        }


        [Authorize]
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
        [Authorize]
        [RequireHttps(Order = 1)]
        public ActionResult ChangePassword(ChangePasswordInfo changePasswordInfo)
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

            if (ModelState.IsValid && _userService.CheckUserPassword(changePasswordInfo.CurrentPassword))
            {
                try
                {
                    _userService.ChangePassword(changePasswordInfo.Password);
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



        [RequireHttps]
        public ActionResult CheckPassword(string currentpassword)
        {
            if (OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated == true)
            {
                return Json(_userService.CheckUserPassword(currentpassword), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserInfo(UserDto userInfo, string status)
        {
            ViewData["status"] = status;
            return View(userInfo);
        }

        [Authorize]
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



        [Authorize]
        [HttpPost]
        [DenyNonPrimaryRole]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult PaymentDetails(AccountPaymentDetailDto bankAccountDto)
        {


            if (ModelState.IsValid)
            {

                try
                {
                    var TaxDocument = Request.Files["TaxDocument"];
                    if (TaxDocument != null && TaxDocument.ContentLength != 0)
                    {

                        MemoryStream target = new MemoryStream();
                        TaxDocument.InputStream.CopyTo(target);

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


        [Authorize]
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
                Name = _accountService.GetAccountName(fund.AccountId),
                TransactionDate = fund.CreationDate.ToShortDateString(),
                Method = fund.FundTransType.Name.Value,
                Amount = fund.Amount,
                VATAmount = fund.VATAmount,
                NoqoushReceiptNumber = fund.NoqoushReceiptNumber
            };

            return View(model);

        }

        [Authorize]
        public ActionResult FundHistory()
        {
            FundResultDto fundsResultDto = new FundResultDto();

            ViewData.Model = fundsResultDto;
            return PartialView();
        }

        [Authorize]
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

            return View(new GridModel
            {
                Data = fundsResultDto.Items,
                Total = fundsResultDto.Total
            });
        }

        [Authorize]
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
                var userDto= _userService.GetUserByEmail(email,false);
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
        public ActionResult SwitchAccountUserFormData()
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

        [Authorize]
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

            return View(new GridModel
            {
                Data = paymentsResultDto.Items,
                Total = paymentsResultDto.Total
            });
        }



        //   [AuthorizeRole(Roles = "Administrator,AdOps")]

        [Authorize]
        // [AuthorizeRole(Roles = "Administrator,AdOps")]
        [RequireHttps]
        [DenyNonPrimaryRole]
        public ActionResult APIAccess()
        {
            AccountAPIAccessDto accountAPIAccess = _accountService.GetAPIAccessSetting();

            if (!accountAPIAccess.AllowAPIAccess)
            {
                AddMessages(ResourcesUtilities.GetResource("DenyAPIAccessWarning", "Global"), MessagesType.Warning);
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
            ViewBag.ShowMenu = true;

            return View(accountAPIAccess);
        }



        //  [AuthorizeRole(Roles = "Administrator,AdOps")]

        [Authorize]
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
                Role=saveModel.RoleId,
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
                        IsSecondPrimaryUser=item.IsSecondPrimaryUser,
                        UserType = item.UserType,
                        VATValue = item.VATValue,
                        Role = ((AccountRole)item.AccountRole).ToText(),
                        PermissionCodes = _userService.getAccountPermissionCode(item.AccountId)

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
                        PermissionCodes = _userService.getAccountPermissionCode(item.AccountId)

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
            //var sss = Noqoush.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat;

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
                    DateString = item.RequestDate != item.ActionDate ? item.ActionDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat) : "",
                    Date2String = item.RequestDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.LongDateFormat),

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

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        [Authorize]

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
            if (Noqoush.AdFalcon.Web.Controllers.Utilities.Config.IsAdministrationApp)
            {
                var result = GetAccounts(saveModel);
                IList<AccountViewModel> users = result.Users;

                //  getuserPermissions(ref users);
                return View(new GridModel
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
            return View(new GridModel
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
            return View(new GridModel
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
                _userService.BlockUser(id);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }


        }



        [DenyNonPrimaryRole]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult UpdateUserType(int id,  string Ids, UserType userType)
        {
            try
            {
                _userService.UpdateUserType(id, Ids, userType);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }


        }


        [DenyNonPrimaryRole]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult MakeUserSecondPrimaryUser(int id)
        {
            try
            {
                _userService.MakeUserSecondPrimaryUser(id);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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


            emailTemplate = emailTemplate.Replace("[url]", "http://" + Config.PublicHostName + "/User/Register?requestCode=" + requestCode);
            emailTemplate = emailTemplate.Replace("[Year]", Noqoush.Framework.Utilities.Environment.GetServerTime().Year.ToString());
            emailTemplate = emailTemplate.Replace("[CustomerEmailAddress]", userdto.EmailAddress);
            emailTemplate = emailTemplate.Replace("[CustomerName]", userdto.FirstName + " " + userdto.LastName);
            return emailTemplate;
        }

        [Authorize]
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
                        UserDto User = _userService.GetUserByEmail(email, true);
                        User.IsAccountDSP = true;
                        _accountService.CreateAccount(User);
                        var resources = ResourcesUtilities.GetResource("AccountDSPAcceptedAlreadyRegistered", "Emails");

                        resources = resources.Replace("[Year]", Noqoush.Framework.Utilities.Environment.GetServerTime().Year.ToString());
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


        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult _AccountDSPRequests(AccountSearchSaveModel saveModel)
        {
            var result = GetAccountDSPRequestSearchViewModel(GetAccountCriteriaBase(saveModel));
            IList<AccountViewModel> users = result.Users;

            //  getuserPermissions(ref users);
            return View(new GridModel
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
            var model = _accountService.GetAccountSetting(accountid);
            model.AccountId = accountid;
            ViewBag.ShowMenu = true;
            return View("AccountSettings", model);
        }

        [Authorize]
        //[AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult Settings(AccountSettingDto data)
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
                    return RedirectToAction("Settings", new { id = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value});
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

        #endregion

        #region Private Members

        private void LoginUser(string username, string token)
        {
            var ticket = new FormsAuthenticationTicket(1, username, Framework.Utilities.Environment.GetServerTime(),
                                                        Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                                                        true, token);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                Secure = FormsAuthentication.RequireSSL,
                HttpOnly = true
            };
            if (!Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
            {
                cookie.Domain = Config.CookieDomain;
            }
            Response.Cookies.Add(cookie);
            _securityProxy.BuildSecurityContext(token);
        }

        public string GetActivationEmail(string activationCode)
        {
            string emailTemplate = ResourcesUtilities.GetResource("RegisterCompletion", "Emails");

            emailTemplate = emailTemplate.Replace("[url]",
                                                  Url.Action("activation", "user",
                                                             new { activationCode = activationCode }, "http"));

            return emailTemplate;
        }
        public string GetInvitationAcceptedEmail(string accountName)
        {
            string emailTemplate = ResourcesUtilities.GetResource("InvitationAccepted", "Emails");

            emailTemplate = emailTemplate.Replace("[account]", accountName);
            emailTemplate = emailTemplate.Replace("[Year]", Noqoush.Framework.Utilities.Environment.GetServerTime().Year.ToString());
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
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            string res = context.Request.ServerVariables["REMOTE_ADDR"];

            return res;



        }

        #endregion


        #region Fund

        [Authorize]
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



        [Authorize]
        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AdFundPGW(AdFundDtoPGW fundDto)
        {
            if (ModelState.IsValid)
            {
                PaymentGatewayHelperFactory paymentFactory = new PaymentGatewayHelperFactory();
                IPaymentGatewayHelper paymentGatewayHelper = paymentFactory.CreatePaymentHelper(fundDto.PaymentType);
                fundDto.VatAmount = Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue * Convert.ToDecimal(fundDto.Amount);
                fundDto.VatAmount = decimal.Round(fundDto.VatAmount, 2, MidpointRounding.AwayFromZero); ;
                int transactionId = paymentGatewayHelper.InitiateTransaction(fundDto.Amount, fundDto.VatAmount);
                return paymentGatewayHelper.RedirectToGateWay(fundDto.Amount + fundDto.VatAmount, transactionId);

            }
            else
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
        }



        [Authorize]
        [RequireHttps]
        public ActionResult FundStatus(int id)
        {

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
            var tran = _fundTrns.GetFundTransactionById(id);
            var model = new ReceiptViewModel
            {
                Amount = FormatHelper.FormatMoney(tran.Amount),
                VATAmount = FormatHelper.FormatMoney(tran.VATAmount),
                ReceiptNo = tran.NoqoushReceiptNumber,
                TransactionId = tran.TransactionId,
                TransactionDate = tran.TransactionDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat + "  hh:mm")
            };
            return View(model);
        }


        [Authorize]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TransactionDone()
        {
            IPaymentGatewayHelperFactory paymentGatewayFactory = new PaymentGatewayHelperFactory();
            IPaymentGatewayHelper paymentGatewayHelper = null;
            if (Request.QueryString["transId"] != null)
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("paypal");
            }
            else
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("migs");
            }

            bool hashValidated = paymentGatewayHelper.ValidateTransaction(Request.QueryString);

            var model = new ReceiptViewModel();
            PaymentStatus status = null;
            if (hashValidated)
            {
                try
                {
                    status = paymentGatewayHelper.CompletePayment(Request.QueryString);
                }
                catch (BusinessException exception)
                {
                    string errorMessage = string.Empty;

                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                        errorMessage = errorMessage + errorData.Message + "<br />";
                    }

                    status = new PaymentStatus() { IsCompleted = false, Message = errorMessage };
                }

            }
            else
            {
                status = new PaymentStatus() { IsCompleted = false, Message = ResourcesUtilities.GetResource("inValidUrl", "PGW") };
            }

            if (!status.IsCompleted)
            {
                model.Message = status.Message;
                model.Title = ResourcesUtilities.GetResource("UnsuccessfulTransactionTitle", "PGW");

                #region BreadCrumb
                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("TransactionFailed", "SiteMapLocalizations"),
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
                return View("TransactionFailed", model);
            }
            else
            {
                AddMessages(status.Message, MessagesType.Success);
                MoveMessagesTempData();
                return RedirectToAction("FundStatus", new { id = status.TransationID });
            }
        }



        [Authorize]
        [RequireHttps]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TransactionCancel()
        {
            // This page is called now from paypal only
            IPaymentGatewayHelperFactory paymentGatewayFactory = new PaymentGatewayHelperFactory();
            IPaymentGatewayHelper paymentGatewayHelper = null;

            var model = new ReceiptViewModel();

            if (Request.QueryString["transId"] != null)
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("paypal");
            }
            else
            {
                paymentGatewayHelper = paymentGatewayFactory.CreatePaymentHelper("creditcard");
            }

            PaymentStatus status = paymentGatewayHelper.ClosePayment(Request.QueryString);

            model.Title = ResourcesUtilities.GetResource("TransactionCancel", "Titles");

            if (status == null)
            {
                model.Message = string.Format(ResourcesUtilities.GetResource("CanceledTransaction", "PGW"), Request.QueryString["transId"]);
            }
            else
            {
                model.Message = status.Message;
            }
            return View(model);
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
            filter.EmailAddress = string.IsNullOrWhiteSpace(Request.Form["Email"]) ? null : Request.Form["Email"];
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"];
            filter.Type = string.IsNullOrWhiteSpace(Request.Form["TypeLog"]) ? 0 : Convert.ToInt32(Request.Form["TypeLog"]);
            return filter;
        }
        protected ImpressionLogCriteria ImpressionLogGetCriteria(InvitationFilter filter)
        {
            if (filter == null)
                filter = getDefualtFilter();
            var DPPartner = _partyService.GetDPPartnerByAccount(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            var criteria = new ImpressionLogCriteria
            {
                DataFrom = filter.FromDate.HasValue ? filter.FromDate : null,
                DataTo = filter.ToDate.HasValue ? filter.ToDate : null,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                DataProviderId = DPPartner != null ? DPPartner.ID : null,
                Type = filter.Type>0 ? (ImpressionLogType)Enum.ToObject(typeof(ImpressionLogType), filter.Type) : ImpressionLogType.None
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

        [Authorize]
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

        [Authorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        [GridAction(EnableCustomBinding = true)]
        public virtual ActionResult _ImpressionLogs()
        {

            var result = ImpressionLogLoadData(null);
            ViewData["total"] = result.TotalCount;
            return View("~/Views/ImpressionLog/Index.cshtml", new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }


        [Authorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public async void DownloadImpLogOld(FormCollection collection, int id, string name)
        {
            var DPPartner = _partyService.GetDPPartnerByAccount(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            var impLogItem = _accountService.GetImpressionLogById(id);
            impLogItem.Path = impLogItem.Path.Replace(@"\", @"/");

        
            if (impLogItem.Type == Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
                name =  name + "_" + "AdMarkupLog" ;
            else
                name = name + "_" + "ImpressionLog";

            var pathimp = HttpUtility.UrlEncode(impLogItem.Path, Encoding.UTF8);
            if (impLogItem.Provider.ID != DPPartner.ID)
                return;

            bool IsPhysical = Convert.ToBoolean(ConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            if (IsPhysical)
            {

                dowloadPath = ConfigurationManager.AppSettings["LogImpPhysicalPath"];
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; ;filename=" + name + ".csv.gz", ""));
                Response.WriteFile(dowloadPath + impLogItem.Path);

            }
            else
            {
                dowloadPath = ConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
                var dowloadPath2 = ConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";
                var cc = new CredentialCache();
                cc.Add(
                    new Uri(ConfigurationManager.AppSettings["LogImpPath"]),
                    "NEGOTIATE",
                    new NetworkCredential("hdfs-reader-iadfalconcluster", "", ""));
                HttpWebResponse resp = null;
                HttpWebRequest req = null;
                try
                {
                    Gss.TerminateAndRemoveOverride();
                    Gss.InitializeAndOverrideApi();

                    req = (HttpWebRequest)WebRequest.Create(dowloadPath);
                    req.AllowAutoRedirect = true;
                    req.Credentials = cc;



                    try
                    {


                        resp = (HttpWebResponse)req.GetResponse();
                    }
                    catch (Exception ex)
                    {
                        req = (HttpWebRequest)WebRequest.Create(dowloadPath2);
                        req.AllowAutoRedirect = true;
                        req.Credentials = cc;
                        resp = (HttpWebResponse)req.GetResponse();
                    }
                }
                finally
                {
                    Gss.TerminateAndRemoveOverride();

                }
                BinaryReader sr = new BinaryReader(resp.GetResponseStream());

                try
                {
                    long startBytes = 0;
                    string _EncodedData = pathimp;

                    Response.Clear();
                    Response.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".csv.gz");
                    Response.AddHeader("Content-Length", (resp.ContentLength - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = Encoding.UTF8;

                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);

                    //Download in block of 1024 bytes
                    int i;
                    for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {
                        Response.BinaryWrite(sr.ReadBytes(1024));
                        Response.Flush();
                    }

                }
                finally
                {
                    Response.End();
                    sr.Close();
                }
            }


        }




        [Authorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]
        public  void DownloadImpLog(FormCollection collection, int id, string name)
        {
            var DPPartner = _partyService.GetDPPartnerByAccount(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            var impLogItem = _accountService.GetImpressionLogById(id);
            impLogItem.Path = impLogItem.Path.Replace(@"\", @"/");


            if (impLogItem.Type == Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
                name = name + "_" + "AdMarkupLog";
            else
                name = name + "_" + "ImpressionLog";

            var pathimp = impLogItem.Path /*HttpUtility.UrlEncode(impLogItem.Path, Encoding.UTF8)*/;
             pathimp = HttpUtility.UrlEncode(pathimp, Encoding.UTF8);
            if (impLogItem.Provider.ID != DPPartner.ID)
                return;

            bool IsPhysical = Convert.ToBoolean(ConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            if (IsPhysical)
            {

                dowloadPath = ConfigurationManager.AppSettings["LogImpPhysicalPath"];
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; ;filename=" + name + ".csv.gz", ""));
                Response.WriteFile(dowloadPath + impLogItem.Path);

            }
            else
            {
                dowloadPath = ConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
                var dowloadPath2 = ConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";
          

                    WebHDFSUtil hDFSUtil = new WebHDFSUtil(ConfigurationManager.AppSettings["LogImpPath"], ConfigurationManager.AppSettings["LogImpPath2"],"", new NetworkCredential(ConfigurationManager.AppSettings["WebHDFSUserName"], ConfigurationManager.AppSettings["WebHDFSPassword"], ConfigurationManager.AppSettings["WebHDFSDomain"]));

                Byte[] contentarr = null;
                
               // Stream fileOe = new  MemoryStream(myByteArray);
                try
                {
                    contentarr= hDFSUtil.ReadFileByResponse(pathimp);
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
                    Response.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".csv.gz");
                    Response.AddHeader("Content-Length", (contentarr.Length - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = Encoding.UTF8;

                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((contentarr.Length - startBytes + 0.0) / 1024);
                    Framework.ApplicationContext.Instance.Logger.Debug("maxCount = " + maxCount);
                    //Download in block of 1024 bytes
                    int i;
                    for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {
                        Response.BinaryWrite(sr.ReadBytes(1024));
                        Response.Flush();
                    }

                }
                finally
                {
                    Response.End();
                    sr.Close();
                }
            }


        }


        #endregion


        #region AduitTrial

        #region Index
        protected Noqoush.AdFalcon.Web.Controllers.Model.User.Filter getDefualtFilterForAuditTrial()
        {


            Noqoush.AdFalcon.Web.Controllers.Model.User.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.User.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);
            filter.type = string.IsNullOrWhiteSpace(Request.Form["ObjectType"]) ? 0 : Convert.ToInt32(Request.Form["ObjectType"]);
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? string.Empty : Request.Form["Name"];

            return filter;
        }

        protected AuditTrialCriteria GetAuditTrialCriteria(Noqoush.AdFalcon.Web.Controllers.Model.User.Filter filter)
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
        protected virtual TrialViewModel TrialMainRootGetQueryResult(Noqoush.AdFalcon.Web.Controllers.Model.User.Filter filter)
        {
            var criteria = GetAuditTrialCriteria(filter);
            var result = _accountService.MainRootTrialQueryByCratiria(criteria);


            return new TrialViewModel
            {
                Items = result.Items,
                TotalCount = result.TotalCount
            };


        }

        protected virtual TrialListViewModel LoadDataTrialMainRoot(Noqoush.AdFalcon.Web.Controllers.Model.User.Filter filter)
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


        [Authorize]
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

        [Authorize]

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrial()
        {
            var result = TrialMainRootGetQueryResult(null);
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [Authorize]
        public ActionResult AuditTrialSessions(int objectRootId, int objectRootTypeId, List<BreadCrumbModel> TraveledBreadCrumbLinks = null, string returnUrl = null)
        {

            //var list = _accountService.GetTrials();
            Noqoush.AdFalcon.Web.Controllers.Model.User.TrialListViewModel model = new TrialListViewModel();

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
            ViewData["AuditTrialSessionType"] = _accountService.GetRootObjectTypeNameValue(objectRootTypeId);
            ViewData["AuditTrialSession"] = _accountService.GetRootObjectName(objectRootTypeId, objectRootId, _accountService.GetRootObjectTypeName(objectRootTypeId));

            return View("AuditTrialSessions", model);
        }
        [Authorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSessions(int objectRootId, int objectRootTypeId)
        {
            //var list1 = LoadData(null);
            AuditTrialCriteria criteria = new AuditTrialCriteria();

            Noqoush.AdFalcon.Web.Controllers.Model.User.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.User.Filter();
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
            criteria.UserName = string.IsNullOrWhiteSpace(Request.Form["UserName"]) ? string.Empty : Request.Form["UserName"];
            //criteria.Name = name;
            IList<TrialDto> list = new List<TrialDto>();
            try
            {
                list = _accountService.GetTrialSessions(criteria, out total);
            }
            catch (UnauthorizedAccessException ex)
            {
                // to hanle it wieather show it or not
                total = 0;
            }
            return View(new GridModel
            {
                Data = list,
                Total = total
            });
        }

        [Authorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSession(string Id)
        {

            var list = _accountService.GetTrialSession(Id, Config.IsAdministrationApp || Config.IsAppOpsAdmin ||
    Config.IsAdOpsAdmin);

            return View(new GridModel
            {
                Data = list,
                Total = list.Count
            });
            //return PartialView("AuditTrialSessions", list);
        }
        [Authorize]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AuditTrialSessionDetails(long Id)
        {

            var list = _accountService.GetTrialDetailsSession(Id, Config.IsAdministrationApp || Config.IsAppOpsAdmin ||
    Config.IsAdOpsAdmin);

            return View(new GridModel
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
        [Authorize]
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
            model.Setting = _accountService.GetDSPAccountSettingReport(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
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
            List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
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

        [Authorize]
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
                List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
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


        [Authorize]
        [AllowRole(AccountRoles = new AccountRole[] { AccountRole.DSP })]
      
        public ActionResult ADMAccountSettings()
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
            model.Setting = _accountService.GetDSPAccountSettingReport(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
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
            List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
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

        [Authorize]
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
                List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = _countryService.GetAll().ToList().OrderBy(p => p.Name.Value).ToList();
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
        [Authorize]
        public ActionResult GetCitiesByCountry(int countryId)
        {

            List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.LocationDto> CitiesDtos = _countryService.GetAllStates(countryId).ToList().OrderBy(p => p.Name.Value).ToList();
            List<SelectListItem> Cities = new List<SelectListItem>();
            foreach (var item in CitiesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();

                Cities.Add(selectItem);
            }
            return Json(new { status = "1", result = Cities }, JsonRequestBehavior.AllowGet);

        }


        #endregion

        private void BuildAdFalconUser(int accountId, int userId, bool switchAccountSet = true)
        {
            var userInof = _accountService.BuildAdFalconUser(accountId, userId, OperationContext.Current.CurrentPrincipal.Identity.Name);
            if (userInof != null)
            {
                userInof.SwitchAccountSet = switchAccountSet;
                _userService.SetAccountUser(accountId);
                _userService.UpdateOperationContext(userInof);
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInof);
                _userService.InsertLastLoginDateAuditTrial(OperationContext.Current.CurrentPrincipal.Identity.Name);
            }
        }

        #region Feature
        public void SetFeature(int code)
        {
            _accountService.SetFeature(code);
        }

        public ActionResult HadAFeature(int code)
        {
            try
            {
                bool HadAFeature = _accountService.HadAFeature(code);
                return Json(new { Result = HadAFeature }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);

            }

        }

        [RequireHttps(Order = 1)]
        public ActionResult HadAFeatureHttps(int code)
        {
            try
            {
                bool HadAFeature = _accountService.HadAFeature(code);
                return Json(new { Result = HadAFeature }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);

            }

        }




        #endregion

       [RequireHttps]
        public ActionResult GetAdvertiserAccountReadOnlySettings(int userId)
        {
            AdvertiserAccountSettingsForReadOnly searcher = new AdvertiserAccountSettingsForReadOnly();


            searcher.UserId = userId;
            return Json(new { status = "1", result = _userService.GetAdvertiserAccountReadOnlySettings(searcher) }, JsonRequestBehavior.AllowGet);
        }
    }
}
