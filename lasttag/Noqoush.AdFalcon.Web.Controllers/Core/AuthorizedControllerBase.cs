using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    [Authorize]
    public class AuthorizedControllerBase : ControllerBase
    {
        private readonly ICampaignService _campaignService = IoC.Instance.Resolve<ICampaignService>();

        private readonly IAccountService _accountService = IoC.Instance.Resolve<IAccountService>();
        [AuthorizeRole(Roles = "Administrator,AppOps,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]

        public virtual ActionResult GetCampaignBidConfigs(int? id, int? adGroupId)
        {
            if (!id.HasValue || !adGroupId.HasValue)
            {
                return View(new GridModel
                {
                    Data = new List<CampaignBidConfigDto>(),
                    Total = 0
                });
            }

            var campaignBidConfigDto = _campaignService.GetCampaignBidConfigs((int)id, (int)adGroupId);
            var campaignAssignedAppsitesList = _campaignService.GetCampaignAssignAppsites((int)id).CampaignAssignedAppsitesList;
            foreach (var bidConfig in campaignBidConfigDto.CampaignBidConfigDtos)
            {
                if (campaignAssignedAppsitesList.Where(x => (x.Appsite.ID == bidConfig.Appsite.ID) && (x.SubPublisherId == bidConfig.SubPublisherId)).Count() > 0)
                {
                    bidConfig.HideDeleteButton = true;

                }
            }

            return View(new GridModel
            {
                Data = campaignBidConfigDto.CampaignBidConfigDtos.ToList(),
                Total = campaignBidConfigDto.CampaignBidConfigDtos.Count
            });
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //check user agreement version
            var user = OperationContext.Current.UserInfo<AdFalconUserInfo>();

            //if (_accountService.GetUserAccountsCount(user.UserId.Value) > 1&& user.SwitchAccountSet== false && user.ImpersonatedAccount==null)
            //{
            //    string returnUrl = HttpContext.Request.RawUrl;

            //    var URl = Url.Action("SwitchAccount","user", new { returnUrl = !string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("switchaccount") ? returnUrl : "" });
            //    filterContext.Result = new RedirectResult(URl);
            //    return;
            //}
            if (user.AccountRole !=(int) Domain.Common.Model.Account.AccountRole.DSP)
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
}
