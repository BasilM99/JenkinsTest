using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    //[DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    public class ReactUiTestController : AuthorizedControllerBase
    {

        public ReactUiTestController()
        {
            
        }


        public ActionResult ReactUi()
        {
            return View();
        }


    }
}










