using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Business.Domain.Exceptions;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Exceptions.Account;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Payment;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.Account;
using Noqoush.AdFalcon.Web.Controllers.Model.Pgw;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Security;
using Noqoush.Framework.Utilities.EmailsSender;
using Noqoush.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace Noqoush.AdFalcon.Administration.Web.Controllers
{
    public class UserController : AuthorizedControllerBase
    {
        private IAccountService _accountService;
        private IUserService _userService;
        private IPaymentTypeService _paymentTypeService;
        public UserController(IAccountService accountService, IUserService userService, IPaymentTypeService paymentTypeService)
        {
            _accountService = accountService;
            _userService = userService;
            _paymentTypeService = paymentTypeService;
        }
        #region overriding
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var s = filterContext.RouteData.Values.Count;
            if (OperationContext.Current.CurrentPrincipal.IsInRole("AdOps"))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
            }
        }
        #endregion

        #region AccountSearch

        #region Helpers
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
                Page = page,
                Size = size
            };
        }
        private AccountSearchViewModel GetAccountSearchViewModel(UserCriteriaBase criteria)
        {
            var result = _userService.QueryByCratiria(criteria);
            var model = new AccountSearchViewModel
            {
                Name = criteria.Name,
                CompanyName = criteria.CompanyName,
                Email = criteria.Email,
                TotalCount = result.TotalCount,
                Users = result.Items.Select(item => new AccountViewModel()
                {
                    Id = item.AccountId,
                    Name = item.ToString(),
                    CompanyName = item.Company,
                    Email = item.EmailAddress
                }).ToList()
            };
            return model;
        }
        #endregion
        [RequireHttps(Order = 1)]
        public ActionResult AccountSearch()
        {
            var model = new AccountSearchViewModel
                            {
                                Name = string.Empty,
                                CompanyName = string.Empty,
                                TotalCount = 0,
                                Users = new List<AccountViewModel>()
                            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }


        [GridAction(EnableCustomBinding = true)]
        [RequireHttps(Order = 1)]
        public ActionResult _accountSearch(AccountSearchSaveModel saveModel)
        {
            var result = GetAccountSearchViewModel(GetUserCriteriaBase(saveModel));
            return View(new GridModel
                            {
                                Data = result.Users,
                                Total = Convert.ToInt32(result.TotalCount)
                            });
        }

        #endregion
        #region Impersonate


        [RequireHttps(Order = 1)]
        public ActionResult Impersonate(string returnUrl)
        {
            if (!OperationContext.Current.CurrentPrincipal.IsInRole("Administrator"))
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
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return View(model);
        }



        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult Impersonate(AccountSearchSaveModel saveModel)
        {

            if (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator"))
            {
                //TODO:Osaleh to check if the use don't select any account
                var userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                if (!string.IsNullOrWhiteSpace(Request.Form["Filter"]))
                {
                    ViewBag.isAdmin = true;
                    return View(GetAccountSearchViewModel(GetUserCriteriaBase(saveModel)));
                }
                if ((!string.IsNullOrWhiteSpace(Request.Form["Save"])) && (!saveModel.AccountId.HasValue))
                {
                    ViewBag.isAdmin = true;
                    //AddMessages(ResourcesUtilities.GetResource("ImpersonateMsg", "Impersonate"), MessagesType.Error);
                    return View(GetAccountSearchViewModel(GetUserCriteriaBase(saveModel)));
                }
                if (!string.IsNullOrWhiteSpace(Request.Form["Revert"]))
                {
                    saveModel.AccountId = userInfo.OriginalAccountId;
                }
                if (_userService.Impersonate(saveModel.AccountId))
                {
                    userInfo.AccountId = saveModel.AccountId;
                    OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);
                }
            }
            if (string.IsNullOrEmpty(saveModel.returnUrl))
            {
                return RedirectToAction("index", "dashboard", new { charttype = "ad" });
            }
            return Redirect(saveModel.returnUrl);
        }
        #endregion
        #region Payment
        [RequireHttps(Order = 1)]
        public ActionResult GetPaymentDetails(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetPaymentDetails(accountId, accountType);
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }
        private AddPaymentViewModel GetAddPaymentViewModel()
        {
            ViewBag.isAdmin = true;
            var model = new AddPaymentViewModel();
            var users = _userService.GetAllUser();

            //Load Accounts List
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };
            var accounts = new List<SelectListItem> { optionalItem };
            accounts.AddRange(users.Select(item => new SelectListItem
            {
                Value = item.AccountId.ToString(),
                Text =
                    item.FirstName + " " + item.LastName + "(" + item.Company + ")"
            }));
            model.Accounts = accounts;


            var paymentTypes = _paymentTypeService.GetAll();

            var paymentTypesList = new List<SelectListItem> { optionalItem };
            paymentTypesList.AddRange(paymentTypes.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            model.PaymentTypes = paymentTypesList;

            return model;
        }

        [Authorize]
        [RequireHttps(Order = 1)]
        public ActionResult AddPayment()
        {
            var model = GetAddPaymentViewModel();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult AddPayment(NewPaymentDto paymentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.AddPayment(paymentDto);
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("AddPayment", "Admin");
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            var model = GetAddPaymentViewModel();
            model.PaymentDto = paymentDto;
            return View(model);
        }

        #endregion

    }
}
