using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Web.Controllers.Model.AccountManagement;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Web.Controllers;
using Microsoft.AspNetCore.Http;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces.Services.System;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Core.ViewComponents.AccountManagement
{

    public class AcountCostElement : ViewComponent
    {
        private static ISystemAccountService _systemAccountService;
        private static IAccountService _accountService;
        private static IUserService _userService;
        private static IPartyService _partyService;
        private static IFundsService _fundService;
        private static IPaymentTypeService _paymentTypeService;
        private static IFundTransTypeService _fundTransTypeService;
        private static IFundTransactionService _fundTransactionService;
        private static ICurrencyService _currencyService;
        private static IFundTypeService _fundTypeService;
        protected static ILookupService lookupService;
        //private WriteReportDocumentsHelper _WriteReportHelper;
        static AcountCostElement()
        {
            _accountService = IoC.Instance.Resolve<IAccountService>(); ;
            _userService = IoC.Instance.Resolve<IUserService>(); ;
            _paymentTypeService = IoC.Instance.Resolve<IPaymentTypeService>(); ;
            _fundTransTypeService = IoC.Instance.Resolve<IFundTransTypeService>(); ;
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>(); ;
            _currencyService = IoC.Instance.Resolve<ICurrencyService>(); ;

            _partyService = IoC.Instance.Resolve<IPartyService>(); ;
            _fundTypeService = IoC.Instance.Resolve<IFundTypeService>(); ;
            lookupService = IoC.Instance.Resolve<ILookupService>(); ;
            _fundService = IoC.Instance.Resolve<IFundsService>(); ;
        }
        public AcountCostElement()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(
       int? id)
        {
            AccountCostElementDto viewModel = new AccountCostElementDto();

            var Providers = _partyService.QueryByCriteria(new PartyCriteria { Visible = true, Type = PartyType.DP });
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected =viewModel.DataProviderId==0
                                              }};
            List<SelectListItem> ProvidersList = Providers.Items.Select(
                item => new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name.ToString(),
                    Selected = viewModel != null && viewModel.DataProviderId == item.ID
                }).ToList();
            ProvidersList.Insert(0, lookupsList[0]);
            ViewData["Providers"] = ProvidersList;
            return View("AcountCostElement", viewModel);
        }

    }
}
