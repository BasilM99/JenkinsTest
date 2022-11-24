using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using System.Web;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using ArabyAds.Framework.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core.Security;
using ArabyAds.AdFalcon.Web.Controllerss.Model;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{

    public class NamespaceConstraint : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var dataTokenNamespace = (string)routeContext.RouteData.DataTokens.FirstOrDefault(dt => dt.Key == "NameSpace").Value;
            var actionNamespace = ((ControllerActionDescriptor)action).MethodInfo.ReflectedType.Namespace;

            return dataTokenNamespace == actionNamespace;
        }
    }
    [DataContract()]
    public enum MessagesType
    {
        [EnumMember]
        Information,
        [EnumMember]
        Error,
        [EnumMember]
        Warning,
        [EnumMember]
        Success
    }

    [NotCacheable()]
 
    public class ControllerBase : Controller
    {
        private readonly IAccountService _accountService = IoC.Instance.Resolve<IAccountService>();
        private readonly IUserService _userOpService = IoC.Instance.Resolve<IUserService>();

        private readonly IPartyService partyDataProviderService = IoC.Instance.Resolve<IPartyService>();
        #region Private

        private string _dispalyName = string.Empty;
        private string _dispalyResourceName = "PageDispalyName";
        #endregion
        #region Public
        public string DispalyResourceName
        {
            get { return _dispalyResourceName; }
            set { _dispalyResourceName = value; }
        }
        public string DispalyName
        {
            get { return _dispalyName; }
        }
        #endregion
        #region overeide
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //TODO : wen need to review this approach to access the controller
            ViewData["Controller"] = this;
            base.OnActionExecuted(context);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.HasFormContentType)
            {
                Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
{
    { "CheckingCore", "True" },

});
            }
            object[] atts = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            if (atts.Any())
            {
                //check user agreement version
                var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                //    if (_accountService.GetUserAccountsCount(user.UserId.Value) > 1 && user.SwitchAccountSet == false && user.ImpersonatedAccount == null)
                //    {
                //        string returnUrl = HttpContextHelper.Request.RawUrl;
                //        var URl = Url.Action("SwitchAccount", "user", new { returnUrl = !string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("switchaccount") ? returnUrl: ""  });
                //    filterContext.Result = new RedirectResult(URl);
                //    return;
                //}
                if (user.AccountRole != (int)Domain.Common.Model.Account.AccountRole.DSP)
                {
                    if (_userOpService.getCurrentUserAgreement() != Config.UserAgreementVersion)
                    {
                        var date = Config.UserAgreementEffectiveDate;
                        if (ArabyAds.Framework.Utilities.Environment.GetServerTime() > date)
                        {
                            var url = Url.Action("UserAgreement", "User", new { returnUrl = HttpContext.Request.GetRawUrl() });
                            filterContext.Result = new RedirectResult(url);
                        }
                        return;
                    }
                }

                else
                {
                    if (_userOpService.getCurrentUserAgreement() != Config.DSPUserAgreementVersion)
                    {
                        var date = Config.DSPUserAgreementEffectiveDate;
                        if (ArabyAds.Framework.Utilities.Environment.GetServerTime() > date)
                        {
                            var url = Url.Action("UserAgreement", "User", new { returnUrl = HttpContext.Request.GetRawUrl() });
                            filterContext.Result = new RedirectResult(url);
                        }
                        return;
                    }


                }

            }
            CheckRedirection(filterContext);
        }
        public void CheckRedirection(ActionExecutingContext filterContext)
        {

            var requireHttps = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(
                           typeof(RequireHttpsAttribute), false)
                        .Count() >= 1;
            var requireHttps2 = (filterContext.Controller).GetType().GetCustomAttributes(
                          typeof(RequireHttpsAttribute), true)
                       .Count() >= 1;

            var requireNoHttps = (filterContext.Controller).GetType().GetCustomAttributes(
                   typeof(NoHttps), true)
                .Count() >= 1;
            requireHttps = requireHttps || requireHttps2;
            if (requireNoHttps)
                requireHttps = false;
            if ((filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.Name == "SwitchAccount")
                return;
            //Only check if we are already on a secure connection and   
            // we don't have a [RequireHttpsAttribute] defined  
            if (Request.IsHttps)
            {
                //If we don't need SSL and we are not on a child action  
                if (!requireHttps /*&& !filterContext.IsChildAction*/)
                {
                    var portNo = 80;
                    int.TryParse(JsonConfigurationManager.AppSettings["HttpPortNo"], out portNo);
                    var uriBuilder = new UriBuilder(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(Request)
)
                    {
                        Scheme = "http",
                        Port = portNo
                    };
                    filterContext.Result =
                         this.Redirect(uriBuilder.Uri.AbsoluteUri);
                }
            }
            else
            {
                if (requireHttps)
                {
                    var portNo = 443;
                    int.TryParse(JsonConfigurationManager.AppSettings["HttpsPortNo"], out portNo);
                    var uriBuilder = new UriBuilder(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(Request)
)
                    {
                        Scheme = "https",
                        Port = portNo
                    };
                    filterContext.Result =
                         this.Redirect(uriBuilder.Uri.AbsoluteUri);
                }
            }

        }
        // to be remove into module -Anas AH

        //protected override void Initialize(Microsoft.AspNetCore.Routing.RequestContext requestContext)
        //{
        //    this.TempDataProvider = new CacheTempDataProvider();
        //    base.Initialize(requestContext);
        //}
        #endregion

        protected IList<ResultMessage> NotificationMessages = new List<ResultMessage>();

      
        #region Messages
        List<string> SuccessMessages
        {
            get
            {
                if (ViewBag.SuccessMessages == null)
                {
                    ViewBag.SuccessMessages = new List<string>();
                }
                return ViewBag.SuccessMessages as List<string>;
            }
        }
        List<string> WarningMessages
        {
            get
            {
                if (ViewBag.WarningMessages == null)
                {
                    ViewBag.WarningMessages = new List<string>();
                }
                return ViewBag.WarningMessages as List<string>;
            }
        }
        List<string> InformationMessages
        {
            get
            {
                if (ViewBag.InformationMessages == null)
                {
                    ViewBag.InformationMessages = new List<string>();
                }
                return ViewBag.InformationMessages as List<string>;
            }
        }
        private List<string> ErrorMessages
        {
            get
            {
                if (ViewBag.ErrorMessages == null)
                {
                    ViewBag.ErrorMessages = new List<string>();
                }
                return ViewBag.ErrorMessages as List<string>;
            }
        }
        List<string> TempSuccessMessages
        {
            get
            {
                if (TempData["TempSuccessMessages"] == null)
                {
                    TempData["TempSuccessMessages"] = new List<string>();
                }
                return TempData["TempSuccessMessages"] as List<string>;
            }
        }
        List<string> TempWarningMessages
        {
            get
            {
                if (TempData["TempWarningMessages"] == null)
                {
                    TempData["TempWarningMessages"] = new List<string>();
                }
                return TempData["TempWarningMessages"] as List<string>;
            }
        }
        List<string> TempInformationMessages
        {
            get
            {
                if (TempData["TempInformationMessages"] == null)
                {
                    TempData["TempInformationMessages"] = new List<string>();
                }
                return TempData["TempInformationMessages"] as List<string>;
            }
        }
        private List<string> TempErrorMessages
        {
            get
            {
                if (TempData["TempErrorMessages"] == null)
                {
                    TempData["TempErrorMessages"] = new List<string>();
                }
                return TempData["TempErrorMessages"] as List<string>;
            }
        }
        public void AddMessages(string message, MessagesType messagesType)
        {
            switch (messagesType)
            {
                case MessagesType.Success:
                    SuccessMessages.Add(message);
                    break;
                case MessagesType.Warning:
                    WarningMessages.Add(message);
                    break;
                case MessagesType.Information:
                    InformationMessages.Add(message);
                    break;
                case MessagesType.Error:
                    //TODO:OSaleh to add the message to the error not Warnings
                    ErrorMessages.Add(message);
                    break;
            }
        }
        public void MoveMessagesTempData()
        {
            foreach (var warningMessage in WarningMessages)
            {
                TempWarningMessages.Add(warningMessage);
            }
            foreach (var successMessage in SuccessMessages)
            {
                TempSuccessMessages.Add(successMessage);
            }
            foreach (var informationMessage in InformationMessages)
            {
                TempInformationMessages.Add(informationMessage);
            }
            foreach (var errorMessage in ErrorMessages)
            {
                TempErrorMessages.Add(errorMessage);
            }
        }
        #endregion
        #region Functions
        [Obsolete]
        protected void AddSuccessfullyMsg()
        {
            if (DispalyName == string.Empty)
            {
                _dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");
            }
            AddSuccessfullyMsg(DispalyName);
        }

        [NonAction]
        public virtual JsonResult Json(object data, object serializerSettings, string clientNotificationTitle, ResponseStatus status = ResponseStatus.success)
        {
            var reponse = new { Result = data, MessageTitle = clientNotificationTitle, Messages = NotificationMessages, Status = status };
            return Json(reponse, serializerSettings);
        }

        [NonAction]
        public virtual JsonResult Json(object data, string clientNotificationTitle, ResponseStatus status = ResponseStatus.success)
        {
            var reponse = new { Result = data, MessageTitle = clientNotificationTitle, Messages = NotificationMessages, Status = status };
            return Json(reponse);
        }

        [NonAction]
        public virtual JsonResult Json(string clientNotificationTitle, ResponseStatus status = ResponseStatus.success)
        {
            var reponse = new { MessageTitle = clientNotificationTitle, Messages = NotificationMessages, Status = status };
            return Json(reponse);
        }
               

        [NonAction]
        protected void AddSuccessfullyMsg(string resourceKey = "savedSuccessfully", string resourceSet = "Global",  params string[] formattedStrings)
        {
            var savedSuccessfully = ResourcesUtilities.GetResource(resourceKey, resourceSet);
            if (formattedStrings != null && formattedStrings.Length > 0)
            {
                savedSuccessfully = string.Format(savedSuccessfully, formattedStrings);
            }
            NotificationMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = savedSuccessfully });
        }

        [NonAction]
        protected void AddModelStateErrorMsgs(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                foreach (var item in modelState.Values)
                {
                    if (item.Errors != null)
                    {
                        foreach (var error in item.Errors)
                        {
                            NotificationMessages.Add(new ResultMessage { Type = MessagesType.Error, Message = error.ErrorMessage });
                        }
                    }
                }
            }
        }

        [NonAction]
        protected void AddErrorMsgs(BusinessException businessException)
        {
            foreach (var errorData in businessException.Errors)
            {
                NotificationMessages.Add(new ResultMessage { Message = errorData.Message, Type = MessagesType.Error });
            }

        }
        [NonAction]
        protected void AddErrorMsgs(string Message)
        {
            
                NotificationMessages.Add(new ResultMessage { Message = Message, Type = MessagesType.Error });
          

        }
        [NonAction]
        protected void AddWarnningMsgs(IList<ErrorData> warnnings)
        {
            if (warnnings == null || warnnings.Count < 1) return;
            foreach (var errorData in warnnings)
            {
                NotificationMessages.Add(new ResultMessage { Message = errorData.Message, Type = MessagesType.Warning });
            }

        }

        [NonAction]
        protected void AddWarnningMsgs(string msg)
        {

            NotificationMessages.Add(new ResultMessage { Message = msg, Type = MessagesType.Warning });


        }
        [NonAction]
        protected void AddSuccessfullyMsgMs(string msg)
        {
            
                NotificationMessages.Add(new ResultMessage { Message = msg, Type = MessagesType.Success });
            

        }
        [Obsolete]
        protected void AddSuccessfullyMsg(string displayName)
        {
            var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");
            AddMessages(string.Format(savedSuccessfully, displayName), MessagesType.Success);
        }


        [Obsolete]
        protected void AddSuccessfullyMsg(IList<ResultMessage> msgs)
        {
            var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");
            msgs.Add(new ResultMessage { Type = MessagesType.Success, Message = savedSuccessfully });
        }

        [Obsolete]
        protected void AddModelStateErrorMsgs(IList<ResultMessage> msgs, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                foreach (var item in modelState.Values)
                {
                    if (item.Errors != null)
                    {
                        foreach (var error in item.Errors)
                        {
                            msgs.Add(new ResultMessage { Type = MessagesType.Error, Message = error.ErrorMessage });
                        }
                    }
                }
            }
        }

        [Obsolete]
        protected void AddErrorMsgs(IList<ResultMessage> msgs, BusinessException businessException)
        {

            foreach (var errorData in businessException.Errors)
            {
                msgs.Add(new ResultMessage { Message = errorData.Message, Type = MessagesType.Error });
            }

        }

        [Obsolete]
        protected void AddWarnningMsgs(IList<ResultMessage> msgs, IList<ErrorData> warnnings)
        {
            if (warnnings == null || warnnings.Count < 1) return;
            foreach (var errorData in warnnings)
            {
                msgs.Add(new ResultMessage { Message = errorData.Message, Type = MessagesType.Warning });
            }

        }

        protected void ChangeJavaScriptSet(string javascriptSetName)
        {
            ViewData["CustomScriptSet"] = javascriptSetName;
        }
        public string GetJavaScriptSet()
        {
            if (ViewData["CustomScriptSet"] != null)
            {
                return ViewData["CustomScriptSet"].ToString();
            }
            else
            {
                return "siteJs";
            }
        }
        public IEnumerable<PartyDto> GetExternalDataProviderQueryResultAllResult()
        {
            //no need for it as no external providers now
          //  var result = GetExternalDataProviderQueryResult(null, "DataProviders");

            //return result.Items;
            return new List<PartyDto>(); 
        }


        protected PartyListResultDto GetExternalDataProviderQueryResult(Web.Controllers.Model.Core.Party.Filter filter, string type = "")
        {
            var criteria = GetPartyCriteria(filter, type);
            var result = partyDataProviderService.QueryByCriteriaForDPPartner(criteria);
            return result;
        }
        protected DPPartnerCriteria GetPartyCriteria(Web.Controllers.Model.Core.Party.Filter filter, string type = "")
        {
            if (filter == null)
                filter = GetDefualtPartyFilter();
            var criteria = new DPPartnerCriteria
            {
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.Name,
                Visible = true,


                //StatusId = filter.StatusId
            };





            //criteria.Type = PartyType.BusinessPartner;
            return criteria;
        }
        protected Web.Controllers.Model.Core.Party.Filter GetDefualtPartyFilter()
        {
            var filter = new Web.Controllers.Model.Core.Party.Filter();
            filter.page = !Request.HasFormContentType || string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int)1 : Convert.ToInt32(Request.Form["page"]);
            filter.size = !Request.HasFormContentType || string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int)Config.PageSize : Convert.ToInt32(Request.Form["size"]);

            filter.Name = !Request.HasFormContentType || string.IsNullOrWhiteSpace(Request.Form["PartyName"]) ? "" : Request.Form["PartyName"].ToString();

            return filter;
        }


        public ActionResult GetSideBarData()
        {
          return  Json(GetSideBarDataValue());
        }
        public SideMenuItem GetSideBarDataValue()
        {
            {
                SideMenuItem a = null;
                var ReportScheduleCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
                    <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                    ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                    ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.ReportSchedule).Count() > 0);
                var hasSchedulingPermission = Config.IsAdmin || Config.IsAdOps || Config.IsAppOps || Config.IsAccountManager || _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = PortalPermissionsCode.ReportSchedule }).Value;

                var PMPDealCount = (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                    ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                    ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.PMPDeal).Count() > 0);

                var isDSP = (ArabyAds.Framework.OperationContext.Current.UserInfo
                            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                            ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP);
                var TrafficPlannerCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
                       <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                           ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                               ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.TrafficPlanner).Count() > 0);
                var externalList = GetExternalDataProviderQueryResultAllResult().ToList();

                if ((ArabyAds.Framework.OperationContext.Current.UserInfo
                    <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                    ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP))
                {
                    a = 
                        new SideMenuItem
                        {
                            Items = new List<SideMenuItem>() {

                               new SideMenuItem
                                {
                                    id = "1-AdvertisersTree",
                                    label = "Global:Advertisers",
                                          icon="ri-advertisement-line",
                                    href = "/campaign/AccountAdvertisers",
                                     Items = getAdvertiserSideMenuList(),
                                    classs = "",  showBranchesLine = true,
                                    active = false,

                                },
                                      new SideMenuItem{
                                            id= "3",
                                            label= "Global:ContentLists",
                                           icon="ri-file-list-line",
                                            href="/campaign/MasterAppSites",
                                            showBranchesLine= false,
                                            active= false
                                        },

                                      TrafficPlannerCount|| Config.IsAdOpsAdmin? new SideMenuItem{
                                id= "2-1",
                                label= "Global:TrafficPlanner",
                                icon="ri-traffic-light-line",
                                href="/Campaign/TrafficPlanner",
                                showBranchesLine= false,
                                active= false
                            } : null,
                               new SideMenuItem
                                {
                                    id = "2",
                                    label = "Global:Reporting",
                                    href =  (ReportScheduleCount && hasSchedulingPermission)||  Config.IsAdOpsAdmin? "/reports/IndexReportsJob?reportType=ad": "/Filter/Filter?reportType=ad",
                                    classs = "", icon="ri-file-chart-line",
                                    active = false,

                                },
                               PMPDealCount||  Config.IsAdOpsAdmin? new SideMenuItem{
                                            id= "1-1",
                                            label= "PMPDeal:DealManagement",
                                         icon="ri-money-dollar-circle-line",
                                            href="/deals?id",
                                            Items = new List<SideMenuItem>()
                                            {     new SideMenuItem{
                                                        id= "1-1",
                                                        label= "PMPDeal:DealMonitoringDashBoard",
                                                    icon="ri-home-line",
                                                        href="/dashboard/Index/deal/",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-2",
                                                        label= "PMPDeal:PMPDeals",
                                                   icon="ri-money-dollar-circle-line",
                                                        href="/deals?id",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },

                                            },
                                            showBranchesLine= false,
                                            active= false
                                        }
                                        : null,
                            (externalList != null && externalList.Count > 0)?

                                new SideMenuItem
                                {
                                    id = "2",
                                    label = "Providers:Provider",
                                    href =  "#",
                                    Items= externalList.Select(x => new SideMenuItem{
                                        label = x.Name,
                                        Items = new List<SideMenuItem>()
                                        {
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuForgetpassword"+x.ID,
                                                label = "Titles:Forgetpassword" ,
                                                href="/DataProvider/LandingDataProvider?Id ="+ x.ID+"&Source=Forgetpassword",
                                            },
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuChangePassword"+x.ID,
                                                label = "MenuChangePassword" ,
                                                href="/DataProvider/LandingDataProvider?Id ="+x.ID+"&Source=ChangePassword",
                                            },
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuLogin"+x.ID,
                                                label = "Titles:Login" ,
                                                href="/DataProvider/LandingDataProvider?Id ="+x.ID+"&Source=Login",
                                            },
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuRegister"+x.ID,
                                                label = "UserInformation:Register" ,
                                                href="/DataProvider/LandingDataProvider?Id ="+x.ID+"&Source=Register",
                                            },
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuAudiance"+x.ID,
                                                label = "Global:AudienceLists" ,
                                                href="/DataProvider/LandingDataProvider?Id ="+x.ID+"&Source=Audiance",
                                            },
                                            new SideMenuItem
                                            { icon="ri-store-3-line",
                                                id="DataProviderMenuLogOut"+x.ID,
                                                label = "UserInformation:Logout" ,
                                                href="#",
                                            }
                                        }
                                    }).ToList(),
                                    classs = "",icon="ri-store-3-line",
                                    active = false,

                                }
                             : null,
                             Config.IsAdministrationApp?
                               new SideMenuItem{
                                            id= "ListMenuAdmin",
                                            label= "Admin",
                                        icon="ri-admin-fill", IsExternal=true,
                                            href="/AdOps/Index?Id",
                                            Items = new List<SideMenuItem>()
                                            {
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:Impersonate",
                                                      icon="ri-admin-fill",
                                                        href="/AccountManagement/Impersonate",
                                                        showBranchesLine= false,
                                                        active= false ,IsExternal=false,
                                                    },
                                                   new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:permission",
                                                      icon="ri-admin-fill",
                                                        href="/user/Permissions",
                                                        showBranchesLine= false,
                                                        active= false ,IsExternal=false,
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AdOps",
                                                      icon="ri-admin-fill", IsExternal=false,
                                                        href="/AdOps/Index",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AppOps" ,IsExternal=false,
                                                      icon="ri-admin-fill",
                                                        href="/AppOps/AppSiteManagement",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },

                                                 new SideMenuItem
                                                            {
                                                                id= "1-1", IsExternal=false,
                                                                label= "account:Buyer",
                                                                 icon="ri-admin-fill",
                                                                href="/User/Buyer",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                             new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Audience:AudienceSegments",
                                                            icon="ri-admin-fill",IsExternal=true,
                                                                href="/Lookup/Providers",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:LookupManagement",
                                                              icon="ri-admin-fill",IsExternal=false,
                                                                href="/Lookup/Index/currency",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },


                                                 new SideMenuItem
                                                            {
                                                                id= "1-1", IsExternal=false,
                                                                label= "Account:CostElement",
                                                                 icon="ri-admin-fill",
                                                                href="/AccountManagement/AccountCostElements",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                             new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Global:AccountFeeElement",
                                                            icon="ri-admin-fill",IsExternal=false,
                                                                href="/AccountManagement/AccountFees",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },

                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Deal:GlobalDeals",
                                                      icon="ri-admin-fill",IsExternal=true,
                                                        href="/deals/index?AllowGlobalization=ture",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:MasterAppSiteLists",
                                                      icon="ri-admin-fill", IsExternal=false,
                                                        href="/campaign/GlobalMasterAppSites?id=",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AdFund",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/AccountManagement/AddFund",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },

                                                    new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AddPayment",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/AccountManagement/AddPayment",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                   new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:Employees",
                                                              icon="ri-admin-fill",IsExternal=true,
                                                                href="/Party/Employees",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:BusinessPartners",
                                                              icon="ri-admin-fill",IsExternal=true,
                                                                href="/Party/businesspartners",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:BusinessPartnerSupplyMenu",
                                                        icon="ri-admin-fill", IsExternal=true,
                                                        href="/Partner/Index",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "AccountDSPRequest:AccountDSPManagement",
                                                       icon="ri-admin-fill",IsExternal=false,
                                                        href="/User/AccountDSPRequests",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:TransactionVATHistory",
                                                 icon="ri-admin-fill",IsExternal=true,
                                                        href="/AccountManagement/TransactionVATHistory",
                                                        showBranchesLine= false,
                                                        active= false
                                                    }
                                            },
                                            showBranchesLine= false,
                                            active= false
                                        }

                      :null
                        }
                        }
                            ;
                }
                else if ((ArabyAds.Framework.OperationContext.Current.UserInfo
                <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider))
                {
                    a = 
                        new SideMenuItem
                        {
                            Items = new List<SideMenuItem>() {
                               new SideMenuItem {
                                    id = "1",
                                    label = "Menu:Dashboard",
                                 icon="ri-home-line",
                                    href = "/dashboard/Index/lmpressionlog/",
                                    classs = "",
                                    showBranchesLine = false,
                                    active = false
                                },
                               new SideMenuItem
                                {
                                    id = "2",
                                    label = "DPP:ImpressionLogs",
                                    href = "/User/ImpressionLogs",
                                    classs = "",
                                    active = false,
                                     icon="ri-archive-drawer-line",
                                }
                                   }
                        }
                            ;
                }
                else
                {

                    a = 
                                new SideMenuItem
                                {
                                    Items = new List<SideMenuItem>() {
                               new SideMenuItem {
                                    id = "1",
                                    label = "Global:Advertisers",
                                  icon="ri-advertisement-line",
                                    href = "/campaign/AccountAdvertisers",
                                    classs = "",
                                    Items =getAdvertiserSideMenuList()

                               } ,
                               TrafficPlannerCount || Config.IsAdOpsAdmin? new SideMenuItem{
                                id= "2-1",
                                label= "Global:TrafficPlanner",
                                icon="ri-traffic-light-line",
                                href="/Campaign/TrafficPlanner",
                                showBranchesLine= false,
                                active= false
                            } : null,
                                        new SideMenuItem{
                                            id= "3",
                                            label= "Global:ContentLists",
                                           icon="ri-file-list-line",
                                            href="/campaign/MasterAppSites",
                                            showBranchesLine= false,
                                            active= false
                                        },
                                     new SideMenuItem{
                                            id= "4",
                                            label= "Global:Reporting",
                                          icon="ri-file-chart-line",
                                            href =  (ReportScheduleCount && hasSchedulingPermission) ||  Config.IsAdOpsAdmin? "/reports/IndexReportsJob?reportType=ad": "/Filter/Filter?reportType=ad",
                                            showBranchesLine= false,
                                            active= false
                                        }
                                       ,
                                        PMPDealCount|| Config.IsAdOpsAdmin? new SideMenuItem{
                                            id= "6",
                                            label= "PMPDeal:DealManagement",
                                           icon="ri-money-dollar-circle-line",
                                          href="/deals?id",
                                            Items = new List<SideMenuItem>()
                                            {

                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "PMPDeal:DealMonitoringDashBoard",
                                                       icon="ri-home-line",
                                                        href="/dashboard/Index/deal/",
                                                        showBranchesLine= false,
                                                        active= false
                                                    }

                                                 ,      new SideMenuItem{
                                                        id= "1-2",
                                                        label= "PMPDeal:PMPDeals",
                                                       icon="ri-money-dollar-circle-line",
                                                        href="/deals?id",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                            },
                                            showBranchesLine= false,
                                            active= false
                                        }
                                        : null,



                                new SideMenuItem
                                {
                                    id = "2",
                                    label = "Menu:Publisher",
                                    href = "/appsite",
                                    Items = new List<SideMenuItem>()
                                            {
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:Dashboard",
                                                        icon="ri-home-line",
                                                        href="/dashboard/Index/app/",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "AppReport:App",
                                                       icon="ri-community-fill",
                                                        href="/appsite",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "AppSite:HouseAd",
                                                        icon="ri-home-8-line",
                                                        href="/HouseAd",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem
                                {
                                    id = "2",
                                    label ="Global:Reporting",
                                    href =  (ReportScheduleCount && hasSchedulingPermission)||  Config.IsAdOpsAdmin? "/reports/IndexReportsJob?reportType=app": "/Filter/Filter?reportType=app",
                                    classs = "", icon="ri-file-chart-line",
                                    active = false,

                                }
                                            },
                                    classs = "",
                                    active = false,icon= "ri-community-fill"

                                } ,     Config.IsAdministrationApp?
                               new SideMenuItem{
                                            id= "ListMenuAdmin",
                                            label= "SiteMapLocalizations:AccountManagement",
                                        icon="ri-admin-fill", IsExternal=false,
                                            href="/AdOps/Index?Id",
                                            Items = new List<SideMenuItem>()
                                            {
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:Impersonate",
                                                      icon="ri-admin-fill",
                                                        href="/AccountManagement/Impersonate",
                                                        showBranchesLine= false,
                                                        active= false ,IsExternal=false,
                                                    },

                                                   new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:permission",
                                                      icon="ri-admin-fill",
                                                        href="/user/Permissions",
                                                        showBranchesLine= false,
                                                        active= false ,IsExternal=false,
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AdOps",
                                                      icon="ri-admin-fill", IsExternal=false,
                                                        href="/AdOps/Index",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AppOps" ,IsExternal=false,
                                                      icon="ri-admin-fill",
                                                        href="/AppOps/AppSiteManagement",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },

                                                new SideMenuItem
                                                            {
                                                                id= "1-1", IsExternal=false,
                                                                label= "account:Buyer",
                                                                 icon="ri-admin-fill",
                                                                href="/User/Buyer",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },

                                                       new SideMenuItem
                                                            {
                                                                id= "1-1", IsExternal=false,
                                                                label= "Account:CostElement",
                                                                 icon="ri-admin-fill",
                                                                href="/AccountManagement/AccountCostElements",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                             new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Global:AccountFeeElement",
                                                            icon="ri-admin-fill",IsExternal=false,
                                                                href="/AccountManagement/AccountFees",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Audience:AudienceSegments",
                                                            icon="ri-admin-fill",IsExternal=true,
                                                                href="/Lookup/Providers",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:LookupManagement",
                                                            icon="ri-admin-fill",IsExternal=false,
                                                                href="/Lookup/Index/currency",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Deal:GlobalDeals",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/deals/index?AllowGlobalization=ture",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:MasterAppSiteLists",
                                                      icon="ri-admin-fill", IsExternal=false,
                                                        href="/campaign/GlobalMasterAppSites?id=",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AdFund",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/AccountManagement/AddFund",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                   new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:AddPayment",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/AccountManagement/AddPayment",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:Employees",
                                                           icon="ri-admin-fill",IsExternal=true,
                                                                href="/Party/Employees",
                                                                showBranchesLine= false,
                                                                active= false
                                                            },
                                                            new SideMenuItem
                                                            {
                                                                id= "1-1",
                                                                label= "Menu:BusinessPartners",
                                                             icon="ri-admin-fill",IsExternal=true,
                                                                href="/Party/businesspartners",
                                                                showBranchesLine= false,
                                                                active= false
                                                            }

                                                    ,
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:BusinessPartnerSupplyMenu",
                                                       icon="ri-admin-fill", IsExternal=true,
                                                        href="/Partner/Index",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "AccountDSPRequest:AccountDSPManagement",
                                                      icon="ri-admin-fill",IsExternal=false,
                                                        href="/User/AccountDSPRequests",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                                                 new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Global:TransactionVATHistory",
                                                 icon="ri-admin-fill",IsExternal=false,
                                                        href="/AccountManagement/TransactionVATHistory",
                                                        showBranchesLine= false,
                                                        active= false
                                                    }
                                            },
                                            showBranchesLine= false,
                                            active= false
                                        }:null


                                }
                                }
                            ;


                }
                return a;
            }
        }
        public ActionResult GetTopBarData()
        {
            return Json(GetTopBarDataValue());
        }
        public List<SideMenuItem>  GetTopBarDataValue()
        {
            List<SideMenuItem> Items = new List<SideMenuItem>();
            var ReportScheduleCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
               <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.ReportSchedule).Count() > 0);

            var PMPDealCount = (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.PMPDeal).Count() > 0);

            var isDSP = (ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().AccountRole != (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP);

            var TrafficPlannerCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
                   <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                       ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                           ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.TrafficPlanner).Count() > 0);
            var externalList = GetExternalDataProviderQueryResultAllResult().ToList();
            if ((ArabyAds.Framework.OperationContext.Current.UserInfo
              <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
              ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.NormalUser))
            {

                Items.Add(new SideMenuItem { title = "Global:Advertisers", href = "/campaign/AccountAdvertisers", icon = "ri-advertisement-line", moduleId = 1 });
                Items.Add(new SideMenuItem { title = "Menu:Publisher", href = "/dashboard/Index/app/", icon = "ri-community-fill", moduleId = 6 });
                if (PMPDealCount)
                    Items.Add(new SideMenuItem { title = "PMPDeal:PMPDeals", href = "/deals?id", icon = "ri-money-dollar-circle-line", moduleId = 5 });


            }
            else if ((ArabyAds.Framework.OperationContext.Current.UserInfo
                <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP))
            {





                Items.Add(new SideMenuItem { title = "Global:Advertisers", href = "/campaign/AccountAdvertisers", icon = "ri-advertisement-line", moduleId = 1 });
                if (PMPDealCount)
                    Items.Add(new SideMenuItem { title = "PMPDeal:PMPDeals", href = "/deals?id", icon = "ri-money-dollar-circle-line", moduleId = 5 });
                Items.Add(new SideMenuItem { title = "Global:Reporting", href = ReportScheduleCount || Config.IsAdOpsAdmin ? "/reports/IndexReportsJob?reportType=ad" : "/Filter/Filter?reportType=ad", icon = "ri-file-chart-line", moduleId = 4 });
                if (externalList != null && externalList.Count > 0)
                    Items.Add(new SideMenuItem { title = "Audience:Providers", icon = "ri-information-line" });



            }
            else if ((ArabyAds.Framework.OperationContext.Current.UserInfo
            <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
            ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider))
            {

                Items.Add(new SideMenuItem { title = "DPP:ImpressionLogs", href = "/User/ImpressionLogs", icon = "ri-chat-poll-fill", moduleId = 9 });

                // Items.Add(new SideMenuItem { title = "Menu:Reports", icon = "ri-folder-chart-line",href=ReportScheduleCount ? "/reports/IndexReportsJob?reportType=ad" : "/reports?reportType=ad" });
                //Items.Add(new SideMenuItem { title = "DataProviders", icon = "ri-information-line" });

            }

            return Items;
        }

        public ActionResult GetUserSettingsBarData()
        {
            return Json(GetUserSettingsBarDataValue());
        }

        public List<SideMenuItem> GetUserSettingsBarDataValue()
        {
            List<SideMenuItem> Items = new List<SideMenuItem>();
            var ReportScheduleCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
               <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.ReportSchedule).Count() > 0);

            var PMPDealCount = (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.PMPDeal).Count() > 0);

            var isDSP = (ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().AccountRole != (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP);

            var isDataProvider = (ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider);
            var TrafficPlannerCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
                   <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                       ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                           ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.TrafficPlanner).Count() > 0);
            var externalList = GetExternalDataProviderQueryResultAllResult().ToList();
            var AllowAPIAccess = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AllowAPIAccess;

            Items.Add(new SideMenuItem { title = "Menu:Profile", icon = "ri-information-line", href = "/user/edit", IsUserSettings = true });
            if (PMPDealCount)
                Items.Add(new SideMenuItem { title = "Global:ADMAccountSettings", icon = "ri-bar-chart-box-line", href = "/user/ADMAccountSettings", IsUserSettings = false });

            Items.Add(new SideMenuItem { title = "Menu:Transfer", icon = "ri-money-dollar-box-line", href = "/user/PaymentDetails", IsUserSettings = false });
            Items.Add(new SideMenuItem { title = "Menu:Payment", icon = "ri-exchange-dollar-line", href = "/user/AccountHistory", IsUserSettings = false });
            Items.Add(new SideMenuItem { title = "Menu:ChangePassword", icon = "ri-lock-line", href = "/user/changepassword", IsUserSettings = true });
            if (AllowAPIAccess)
                Items.Add(new SideMenuItem { title = "Menu:APIAccess", icon = "ri-information-line", href = "/user/apiaccess", IsUserSettings = false });
            if (!isDataProvider)
                Items.Add(new SideMenuItem { title = "Menu:AdFund", icon = "ri-money-dollar-circle-line", href = "/user/adfundpgw", IsUserSettings = false });


            if (ArabyAds.Framework.OperationContext.Current.UserInfo
                    <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                    ().IsPrimaryUser)
                Items.Add(new SideMenuItem { title = "Menu:UserManagement", icon = "ri-account-box-line", href = "/User/MyUsers", IsUserSettings = false });
            if (ArabyAds.Framework.OperationContext.Current.UserInfo
          <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
          ().IsPrimaryUser)
                Items.Add(new SideMenuItem { title = "Invite:Invitations", icon = "ri-chat-new-line", href = "/User/Invitation", IsUserSettings = false });
            //Items.Add(new SideMenuItem { title = "CostElement", icon = "ri-image-line", href = "#" });
            //Items.Add(new SideMenuItem { title = "AccountFeeElement", icon = "ri-image-line", href = "#" });
            Items.Add(new SideMenuItem { title = "AuditTrial:Header", icon = "ri-search-eye-line", href = "/User/AuditTrial", IsUserSettings = false });
            Items.Add(new SideMenuItem { Logout = true, title = "UserInformation:Logout", icon = "ri-logout-box-line", IsExternal = true, href = "/User/LogoutFromClient?method=logout", IsUserSettings = true });


            if (Config.IsAdmin)
            {

                Items.Add(new SideMenuItem { title = "Admin Account Settings", icon = "ri-equalizer-line", href = "/AccountManagement/Settings/", IsUserSettings = false });
            }
            else
            {
                if (!isDSP)
                    Items.Add(new SideMenuItem { title = "Menu:Settings", icon = "ri-equalizer-line", href = "/User/Settings", IsUserSettings = false });
            }



            return Items;
        }

        private List<SideMenuItem> getAdvertiserSideMenuList()
        {
            List<SideMenuItem> tempAdvList1 = new List<SideMenuItem>();
            List<SideMenuItem> tempAdvList2 = new List<SideMenuItem>();
            var ReportScheduleCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
               <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
               ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.ReportSchedule).Count() > 0);

            var PMPDealCount = (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.PMPDeal).Count() > 0);

            var isDSP = (ArabyAds.Framework.OperationContext.Current.UserInfo
                        <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                        ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP);
            var TrafficPlannerCount = (ArabyAds.Framework.OperationContext.Current.UserInfo
                   <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                       ().Permissions != null && ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                           ().Permissions.Where(x => x == (int)ArabyAds.AdFalcon.Domain.Common.Model.Core.PortalPermissionsCode.TrafficPlanner).Count() > 0);
            var externalList = GetExternalDataProviderQueryResultAllResult().ToList();
            var isDataProvider = ((ArabyAds.Framework.OperationContext.Current.UserInfo
                                <ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
                                ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider));

            if (PMPDealCount || Config.IsAdOpsAdmin)
            {
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Menu:Campains",
                    icon = "ri-briefcase-line",
                    href = "/Campaign/?AdvertiseraccId={{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Global:AudienceList",
                    icon = "ri-list-unordered",
                    href = "/Campaign/AudienceList/{{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Menu:Dashboard",
                    icon = "ri-home-line",
                    href = "/Dashboard/Index/ad/{{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "PMPDeal:PMPDeals",
                    icon = "ri-money-dollar-circle-line",
                    href = "/Deals/{{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                {

                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Global:ContentLists",
                        icon = "ri-file-list-line",
                        href = "/Campaign/MasterAppSites/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Targeting:Pixels",
                        icon = "ri-focus-3-line",
                        href = "/Campaign/TrackingPixel/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                
                }
                else
                {
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Global:ContentLists",
                        icon = "ri-file-list-line",
                        href = "/Campaign/MasterAppSites/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Targeting:Pixels",
                        icon = "ri-focus-3-line",
                        href = "/Campaign/TrackingPixel/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });

                }
                if (Config.IsAdOpsAdmin)
                {
                   
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Lookalike",
                        icon = "ri-eye-line",
                        href = "/Campaign/AudienceListForAdmin/{{id}}",
                        classs = "", IsExternal=true,
                        showBranchesLine = false,
                        active = false
                    });
                }
            }
            else
            {
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Menu:Campains",
                    icon = "ri-briefcase-line",
                    href = "/Campaign/?AdvertiseraccId={{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Global:AudienceList",
                    icon = "ri-list-unordered",
                    href = "/Campaign/AudienceList/{{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });
                tempAdvList2.Add(new SideMenuItem
                {
                    label = "Menu:Dashboard",
                    icon = "ri-home-line",
                    href = "/Dashboard/Index/ad/{{id}}",
                    classs = "",
                    showBranchesLine = false,
                    active = false
                });

                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                {

                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Global:ContentLists",
                        icon = "ri-file-list-line",
                        href = "/Campaign/MasterAppSites/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Targeting:Pixels",
                        icon = "ri-focus-3-line",
                        href = "/Campaign/TrackingPixel/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                    //tempAdvList2.Add(new SideMenuItem
                    //{
                    //    label = "AppSite:Settings",
                    //    icon = "smartphone",
                    //    href = "/Deals/{{id}}",
                    //    classs = "",
                    //    showBranchesLine = false,
                    //    active = false
                    //});
                }
                else
                {
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Global:ContentLists",
                        icon = "ri-file-list-line",
                        href = "/Campaign/MasterAppSites/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Targeting:Pixels",
                        icon = "ri-focus-3-line",
                        href = "/Campaign/TrackingPixel/{{id}}",
                        classs = "",
                        showBranchesLine = false,
                        active = false
                    });

                }
                if (Config.IsAdOpsAdmin)
                {
                    //tempAdvList2.Add(new SideMenuItem
                    //{
                    //    label = "PMPDeals:unArchive",
                    //    icon = "smartphone",
                    //    href = "/Campaign/unArchiveAdvertiser/",
                    //    classs = "",
                    //    showBranchesLine = false,
                    //    active = false
                    //});
                    tempAdvList2.Add(new SideMenuItem
                    {
                        label = "Lookalike",
                        icon = "ri-eye-line",
                        href = "/Campaign/AudienceListForAdmin/{{id}}",
                        classs = "",IsExternal=true,
                        showBranchesLine = false,
                        active = false
                    });
                }
            }


            if (isDSP)
            {
                tempAdvList1 = new List<SideMenuItem>()
                {

                        new SideMenuItem
                    {
                                                        id= "1-1",
                                                        label= "Menu:Dashboard",
                                                        icon="ri-home-line",
                                                        href="/dashboard/Index/ad/",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                    new SideMenuItem
                    {
                        id = "1-2",
                        label = "Global:AdvertisersList",
                                          icon="ri-advertisement-line",
                         href = "/campaign/AccountAdvertisers",
                        showBranchesLine = false,
                        active = false
                    },

                


                            new SideMenuItem() {
                                id= "1-3-1-currentAdvertiser",
                                label= "{{currentAdvertiser}}",
                                icon="",
                                href="",
                                showBranchesLine= true,
                                active= false,
                                classs = "sidebar-tree-structuer",
                                Items= tempAdvList2
                            }
                    };
            }else if(!isDataProvider)
            {
                tempAdvList1 = new List<SideMenuItem>()
                    {        new SideMenuItem{
                                                        id= "1-1",
                                                        label= "Menu:Dashboard",
                                                        icon="ri-home-line",
                                                        href="/dashboard/Index/ad/",
                                                        showBranchesLine= false,
                                                        active= false
                                                    },
                     new SideMenuItem
                    {
                        id = "1-2",
                    label = "Global:AdvertisersList",
                                          icon="ri-advertisement-line",
                         href = "/campaign/AccountAdvertisers",
                        showBranchesLine = false,
                        active = false
                    },
                        
                        new SideMenuItem() {
                            id= "1-3-1-currentAdvertiser",
                            label= "{{currentAdvertiser}}",
                            icon="",
                            href="",
                            showBranchesLine= true,
                            active= false,
                            classs = "sidebar-tree-structuer",
                            Items=tempAdvList2
                        }
                    };

            }



            return tempAdvList1;
        }
        #endregion
    }

}
