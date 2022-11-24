using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.System;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using System;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using System.Collections.Generic;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Telerik.Web.Mvc;

using Noqoush.AdFalcon.Domain.Common.Repositories.Account;


using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;


namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{

    public class SystemAccountController : AuthorizedControllerBase
    {
        private ISystemAccountService _systemAccountService;
        private IAccountService _accountService;
        public SystemAccountController(ISystemAccountService systemAccountService, IAccountService AccountService)
        {
            _systemAccountService = systemAccountService;
            _accountService = AccountService;
        }
        #region Index
    


        #region Actions

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult GetByType(PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _systemAccountService.GetSystemPaymentDetails(accountType);
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #endregion
    }
}
