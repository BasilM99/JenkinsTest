using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using Microsoft.AspNetCore.Authentication.Cookies;
using ArabyAds.Framework.Security;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [RequireHttps(Order = 1)]

    public class AuthorizedControllerBase : ControllerBase
    {
        private readonly ICampaignService _campaignService = IoC.Instance.Resolve<ICampaignService>();

        static SecurityManager securityProxy;



        private static void RegisterSecurityProxy()
        {
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }
        private readonly IAccountService _accountService = IoC.Instance.Resolve<IAccountService>();

        private readonly IUserService _userOpService = IoC.Instance.Resolve<IUserService>();
        [AuthorizeRole(Roles = "Administrator,AppOps,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]

        public virtual ActionResult GetCampaignBidConfigs(int? id, int? adGroupId)
        {
            if (!id.HasValue || !adGroupId.HasValue)
            {
                return Json(new GridModel
                {
                    Data = new List<CampaignBidConfigDto>(),
                    Total = 0
                });
            }

            var campaignBidConfigDto = _campaignService.GetCampaignBidConfigs(new CampaignIdAdgroupIdMessage { CampaignId = (int)id, AdgroupId = (int)adGroupId });
            var campaignAssignedAppsitesList = _campaignService.GetCampaignAssignAppsites(new ValueMessageWrapper<int> { Value = (int)id }).CampaignAssignedAppsitesList;
            foreach (var bidConfig in campaignBidConfigDto.CampaignBidConfigDtos)
            {
                if (campaignAssignedAppsitesList.Where(x => (x.Appsite.ID == bidConfig.Appsite.ID) && (x.SubPublisherId == bidConfig.SubPublisherId)).Count() > 0)
                {
                    bidConfig.HideDeleteButton = true;

                }
            }

            return Json(new GridModel
            {
                Data = campaignBidConfigDto.CampaignBidConfigDtos.ToList(),
                Total = campaignBidConfigDto.CampaignBidConfigDtos.Count
            });
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           // if (filterContext.ActionDescriptor.Parameters.Count == 1 && filterContext.HttpContext.Request.ContentType != null && filterContext.HttpContext.Request.ContentType.Contains("application/json"))
            //{
            //    //filterContext.ModelState.Values.First().
            //    filterContext.ActionDescriptor.Parameters[0].BindingInfo.BindingSource = BindingInfo.GetBindingInfo(;
            //    TryUpdateModelAsync()
            //}
            if (!Request.HasFormContentType)
            {
                Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
{
    { "CheckingCore", "True" },

});
            }
            if (securityProxy == null)
            {
                RegisterSecurityProxy();
            }

            if (HttpContextHelper.Current.User != null && !(HttpContextHelper.Current.User is ArabyAdsPrincipal))
            {
                if (HttpContextHelper.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContextHelper.Current.User.Identity is IIdentity)
                    {
                        ClaimsIdentity identity = (ClaimsIdentity)HttpContextHelper.Current.User.Identity;

                        //FormsAuthenticationTicket ticket = identity.;

                        //userSession = identity.;
                        //ClaimsIdentity user = User.Identity as ClaimsIdentity

                        string userSession = identity.FindFirst("UserToken").Value;
                        securityProxy.BuildSecurityContext(userSession).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
            }



            //check user agreement version
            var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();

            //if (_accountService.GetUserAccountsCount(user.UserId.Value) > 1&& user.SwitchAccountSet== false && user.ImpersonatedAccount==null)
            //{
            //    string returnUrl = HttpContextHelper.Request.RawUrl;

            //    var URl = Url.Action("SwitchAccount","user", new { returnUrl = !string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("switchaccount") ? returnUrl : "" });
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



            CheckRedirection(filterContext);
        }
    }
}
