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
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.User
{

    public class SwitchAccount : ViewComponent
    {
        private static SecurityManager _securityProxy;
        private const string GlobalErrors = "GlobalErrors";
        private static IAccountService _accountService;
        private static IUserService _userService;
        private static IMailSender _mailSender;
        private static IFundsService _FundsService;
        private static IPartyService _partyService;
        private static IDocumentTypeService _DocumentTypeService;

        private static IFundTransactionService _fundTrns;
        private static ILanguageService _languageService;
        private static ICountryService _countryService;
        private static IAdvertiserService _AdvertiserSearcher;


        static SwitchAccount()
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

        }
        public SwitchAccount()
        {
        }


        public async Task<IViewComponentResult> InvokeAsync(
       string returnUrl)
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
            return View("SwitchAccount");
        }

    }
}
