using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.System;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using System;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using Telerik.Web.Mvc;

using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;


using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Account;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Controllers
{

    public class SystemAccountController : AuthorizedControllerBase
    {
        private ISystemAccountService _systemAccountService;
        private IAccountService _accountService;
        public SystemAccountController()
        {
            _systemAccountService = IoC.Instance.Resolve<ISystemAccountService>(); ;
            _accountService = IoC.Instance.Resolve<IAccountService>(); ;
        }
        #region Index
    


        #region Actions

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult GetByType(PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _systemAccountService.GetSystemPaymentDetails( new Framework.ValueMessageWrapper<PayemntAccountType> { Value= accountType });
            return Json(accounts);
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        public ActionResult GetPaymentAccountDetails(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetFullPaymentDetails(new GetFullPaymentDetailsRequest { AccountId = accountId, PaymentAccountType = accountType, PaymentAccountSubType = PayemntAccountSubType.Payment });
            return Json(accounts);
        }


        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [HttpPost]
        //[DenyRole(Roles = "AppOps")]
        public ActionResult GetByTypeAccount([FromBody] AddFundViewModel model)
        {
            //load the system Account 
            var accounts = _systemAccountService.GetSystemPaymentDetails(new Framework.ValueMessageWrapper<PayemntAccountType> { Value = model.accountType });
            return Json(accounts);
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        [HttpPost]
        public ActionResult GetPaymentAccountDetailsAccount([FromBody] AddFundViewModel model)
        {
            //load the system Account 
            var accounts = _accountService.GetFullPaymentDetails(new GetFullPaymentDetailsRequest { AccountId = model.accountId, PaymentAccountType = model.accountType, PaymentAccountSubType = PayemntAccountSubType.Payment });
            return Json(accounts);
        }

        #endregion
        #endregion
    }
}
