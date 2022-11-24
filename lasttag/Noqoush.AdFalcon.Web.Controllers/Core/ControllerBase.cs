using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using System.Web;
using Noqoush.Framework;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] atts = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            if (atts.Any())
            {
                //check user agreement version
                var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            //    if (_accountService.GetUserAccountsCount(user.UserId.Value) > 1 && user.SwitchAccountSet == false && user.ImpersonatedAccount == null)
            //    {
            //        string returnUrl = HttpContext.Request.RawUrl;
            //        var URl = Url.Action("SwitchAccount", "user", new { returnUrl = !string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("switchaccount") ? returnUrl: ""  });
            //    filterContext.Result = new RedirectResult(URl);
            //    return;
            //}
            if (user.AccountRole != (int)Domain.Common.Model.Account.AccountRole.DSP)
            {
                if (user.UserAgreementVersion != Config.UserAgreementVersion)
                {
                    var date = Config.UserAgreementEffectiveDate;
                    if (Noqoush.Framework.Utilities.Environment.GetServerTime() > date)
                    {
                        var url = Url.Action("UserAgreement", "User", new { returnUrl = HttpContext.Request.RawUrl });
                        filterContext.Result = new RedirectResult(url);
                    }
                    return;
                }
            }

            else
            {
                if (user.UserAgreementVersion != Config.DSPUserAgreementVersion)
                {
                    var date = Config.DSPUserAgreementEffectiveDate;
                    if (Noqoush.Framework.Utilities.Environment.GetServerTime() > date)
                    {
                        var url = Url.Action("UserAgreement", "User", new { returnUrl = HttpContext.Request.RawUrl });
                        filterContext.Result = new RedirectResult(url);
                    }
                    return;
                }


            }
        }
    }

    protected override void OnAuthorization(AuthorizationContext filterContext)
    {
        var requireHttps = filterContext.ActionDescriptor
                       .GetCustomAttributes(
                          typeof(RequireHttpsAttribute), false)
                       .Count() >= 1;
        //Only check if we are already on a secure connection and   
        // we don't have a [RequireHttpsAttribute] defined  
        if (Request.IsSecureConnection)
        {
            //If we don't need SSL and we are not on a child action  
            if (!requireHttps && !filterContext.IsChildAction)
            {
                var portNo = 80;
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["HttpPortNo"], out portNo);
                var uriBuilder = new UriBuilder(Request.Url)
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
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["HttpsPortNo"], out portNo);
                var uriBuilder = new UriBuilder(Request.Url)
                {
                    Scheme = "https",
                    Port = portNo
                };
                filterContext.Result =
                     this.Redirect(uriBuilder.Uri.AbsoluteUri);
            }
        }
        base.OnAuthorization(filterContext);
    }
    protected override void Initialize(System.Web.Routing.RequestContext requestContext)
    {
        TempDataProvider = new CacheTempDataProvider();
        base.Initialize(requestContext);
    }
    #endregion
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
    protected void AddSuccessfullyMsg()
    {
        if (DispalyName == string.Empty)
        {
            _dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");
        }
        AddSuccessfullyMsg(DispalyName);
    }
    protected void AddSuccessfullyMsg(string displayName)
    {
        var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");
        AddMessages(string.Format(savedSuccessfully, displayName), MessagesType.Success);
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
            var result = GetExternalDataProviderQueryResult(null, "DataProviders");

            return result.Items;
        }


        protected PartyListResultDto GetExternalDataProviderQueryResult(Model.Core.Party.Filter filter, string type = "")
        {
            var criteria = GetPartyCriteria(filter, type);
            var result = partyDataProviderService.QueryByCriteriaForDPPartner(criteria);
            return result;
        }
        protected DPPartnerCriteria GetPartyCriteria(Model.Core.Party.Filter filter, string type = "")
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
        protected Model.Core.Party.Filter GetDefualtPartyFilter()
        {
            var filter = new Model.Core.Party.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int)1 : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int)Config.PageSize : Convert.ToInt32(Request.Form["size"]);

            filter.Name = string.IsNullOrWhiteSpace(Request.Form["PartyName"]) ? "" : Request.Form["PartyName"];

            return filter;
        }
        #endregion
    }

}
