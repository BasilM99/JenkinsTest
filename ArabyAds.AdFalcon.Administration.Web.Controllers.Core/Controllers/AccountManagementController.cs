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
using System.IO;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class AccountManagementController : AuthorizedControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPartyService _partyService;
        private readonly IFundsService _fundService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IFundTransTypeService _fundTransTypeService;
        private IFundTransactionService _fundTransactionService;
        private ICurrencyService _currencyService;
        private IFundTypeService _fundTypeService;
        protected ILookupService lookupService;
        private WriteReportDocumentsHelper _WriteReportHelper;
        public AccountManagementController(
)
        {
            _systemAccountService = IoC.Instance.Resolve<ISystemAccountService>();
            _accountService = IoC.Instance.Resolve<IAccountService>(); ;
            _userService = IoC.Instance.Resolve<IUserService>(); ;
            _paymentTypeService = IoC.Instance.Resolve<IPaymentTypeService>(); ;
            _fundTransTypeService = IoC.Instance.Resolve<IFundTransTypeService>(); ;
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>(); ;
            _currencyService = IoC.Instance.Resolve<ICurrencyService>(); ;

            _partyService = IoC.Instance.Resolve<IPartyService>(); ;
            _fundTypeService = IoC.Instance.Resolve<IFundTypeService>(); ;
            this.lookupService = IoC.Instance.Resolve<ILookupService>(); ;
            this._fundService = IoC.Instance.Resolve<IFundsService>(); ;
            _WriteReportHelper = new WriteReportDocumentsHelper();
        }


       public  ActionResult Encrypt()
        {
            var value = _systemAccountService.Encrypt().Value;
            return  new JsonResult(value.ToString());
        }
        public ActionResult UnProtect()
        {
         var value   =_systemAccountService.Unprotect().Value;
            return new JsonResult(value.ToString());
        }

        public ActionResult Protect()
        {
            var value = _systemAccountService.protect().Value;
            return new JsonResult(value.ToString());
        }
        public ActionResult Decrypt()
        {
            var value = _systemAccountService.Decypt().Value;
            return new JsonResult(value.ToString());
        } /**/
        public virtual ActionResult Index()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("DemandPartnerSupplyMenu", "Menu"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View();
        }
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
                AccountId = saveModel.AccountId,
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
                AccountIdValue = criteria.AccountId,
                TotalCount = result.TotalCount,
                Users = result.Items.Select(item => new AccountViewModel()
                {
                    Id = item.Id,
                    AccountId = item.AccountId,
                    Name = item.ToString(),
                    CompanyName = item.Company,
                    Email = item.EmailAddress
                }).ToList()
            };
            return model;
        }
        #endregion
        [RequireHttps(Order = 1)]
       [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        public ActionResult AccountSearch(bool hideAdmin = false)
        {
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                hideAdmin = hideAdmin,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        public ActionResult nohttpsPublisherAccountSearch()
        {
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }


        
            

             [RequireHttps]
        public ActionResult GetAccountById(string ids)
        {
            IList<UserDto> results = new List<UserDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();
           
            foreach (var id in TagIds)
            {
                var item = _accountService.GetById(new ValueMessageWrapper<int> { Value = id });
                results.Add(item);
            }
            return Json(results);
          
        }
        [OutputCache(Duration = 9200, VaryByQueryKeys = new string[] { "q","page" })]
        [RequireHttps]
        public ActionResult GetAccountSecure(string q, string culture,int page)
        {
            return Json(ReturnAccountResult(q, culture, page));
        }
       
      


        private UsersListResultDto ReturnAccountResult(string q, string culture,int page)
        {
            var result = _accountService.QueryByCratiria( new UserCriteriaBase {Name=q , hideNonPrimary=true, Page= page-1 ,Size=10});
          
           


            return result;
        }



        //[GridAction(EnableCustomBinding = true)]
        //[AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
        //[RequireHttps(Order = 1)]
        //public ActionResult _accountSearch(AccountSearchSaveModel saveModel, bool isMyUsers = false)
        //{
        //    if (isMyUsers)
        //    {
        //        saveModel.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId;
        //    }
        //    var result = GetAccountSearchViewModel(GetUserCriteriaBase(saveModel));
        //    return View(new GridModel
        //    {
        //        Data = result.Users,
        //        Total = Convert.ToInt32(result.TotalCount)
        //    });
        //}



        //[Requ
        /*[GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
       [RequireHttps(Order = 1)]
        public ActionResult _accountSearchHttps(AccountSearchSaveModel saveModel)
        {
            var result = GetAccountSearchViewModel(GetUserCriteriaBase(saveModel));
            return View(new GridModel
                            {
                                Data = result.Users,
                                Total = Convert.ToInt32(result.TotalCount)
                            });
        }
        [AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
        public ActionResult AccountSearchNoHttps()
        {
            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return PartialView(model);
        }


        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AdOps,Finance Manager,AppOps")]
        //[RequireHttps(Order = 1)]
        public ActionResult _accountSearchNoHttps(AccountSearchSaveModel saveModel)
        {
            var result = GetAccountSearchViewModel(GetUserCriteriaBase(saveModel));
            return View(new GridModel
            {
                Data = result.Users,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }*/

        #endregion
        #region Impersonate


        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AppOps,AccountManager,Finance Manager,adops")]
        public ActionResult Impersonate(string returnUrl)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Impersonate","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            var model = new AccountSearchViewModel
            {
                Name = string.Empty,
                CompanyName = string.Empty,
                AccountIdValue = null,
                TotalCount = 0,
                Users = new List<AccountViewModel>()
            };
            ViewBag.isAdmin = true;
            return View(model);
        }



        [HttpPost]
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AppOps,AccountManager,Finance Manager,adops")]
        public ActionResult Impersonate([FromBody]AccountSearchSaveModel saveModel)
        {
            //#region BreadCrumb

            //var breadCrumbLinks = new List<BreadCrumbModel>
            //                                  {
            //                                      new BreadCrumbModel()
            //                                          {
            //                                              Text = ResourcesUtilities.GetResource("Impersonate","SiteMapLocalizations"),
            //                                              Order = 2,
            //                                          },
            //                                      new BreadCrumbModel()
            //                                          {
            //                                              Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
            //                                              Order = 1,
            //                                              Url = Url.Action("Index", "AdOps")
            //                                          }
            //                                  };

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion
            if (!string.IsNullOrEmpty(saveModel.AccountIdStr))
            {
                var Ids = saveModel.AccountIdStr.Split('_');
                saveModel.UserId = Convert.ToInt32(Ids[1]);
                saveModel.AccountId = Convert.ToInt32(Ids[0]);
            }
            //TODO:Osaleh to check if the use don't select any account
            var userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
            //if (!string.IsNullOrWhiteSpace(Request.Form["Filter"]))
            //{
            //    ViewBag.isAdmin = true;
            //    return View(GetAccountSearchViewModel(GetUserCriteriaBase(saveModel)));
            //}
            if (saveModel.Save != null && !string.IsNullOrWhiteSpace(saveModel.Save) && (!saveModel.AccountId.HasValue))
            {
                ViewBag.isAdmin = true;
                //AddMessages(ResourcesUtilities.GetResource("ImpersonateMsg", "Impersonate"), MessagesType.Error);
                return View(GetAccountSearchViewModel(GetUserCriteriaBase(saveModel)));
            }
            if (saveModel.Revert != null && !string.IsNullOrWhiteSpace(saveModel.Revert))
            {
                saveModel.AccountId = userInfo.OriginalAccountId;
                saveModel.UserId = userInfo.OriginalUserId;
                userInfo.AccountId = userInfo.OriginalAccountId;
                userInfo.UserId = userInfo.OriginalUserId;
                userInfo.IsPrimaryUser = true;
                userInfo.AllowAPIAccess =true;
                userInfo.ImpersonatedAccount = null;
                userInfo.AccountRole = userInfo.AccountRole;
                userInfo.VATValue = _accountService.GetVATValueByAccountId(new ValueMessageWrapper<int> { Value = (int)userInfo.AccountId }).Value;

                var PermissionsUser = _userService.GetAccountAdPermissions(new ValueMessageWrapper<int> { Value = userInfo.AccountId.Value }).ToArray();
                userInfo.Permissions = PermissionsUser;
                var impersonatedAccountOriginal = _userService.Impersonate(new ImpersonateRequest { AccountId = saveModel.AccountId, UserId = saveModel.UserId });

                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);

                if (userInfo.AccountRole == (int)AccountRole.DataProvider)
                {
                    //return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });
                    return Json(new { url = "/dashboard/index/lmpressionlog/" });

                }
                if (string.IsNullOrEmpty(saveModel.returnUrl))
                {
                    //return RedirectToAction("index", "dashboard", new { charttype = "ad" });
                    return Json(new { url = "/dashboard/index/ad/" });
                }
              

                return Redirect(saveModel.returnUrl);
            }
            var impersonatedAccount = _userService.Impersonate( new ImpersonateRequest {  AccountId= saveModel.AccountId, UserId= saveModel.UserId });
            userInfo.AccountId = saveModel.AccountId;
            userInfo.UserId = saveModel.UserId;
            userInfo.IsPrimaryUser = impersonatedAccount.IsPrimaryUser;
            userInfo.AllowAPIAccess = impersonatedAccount.AllowAPIAccess;
            userInfo.ImpersonatedAccount = impersonatedAccount;
            userInfo.AccountRole = impersonatedAccount.AccountRole;
            userInfo.VATValue = _accountService.GetVATValueByAccountId(new ValueMessageWrapper<int> { Value = (int)userInfo.AccountId }).Value;

            var Permissions = _userService.GetAccountAdPermissions(new ValueMessageWrapper<int> { Value = userInfo.AccountId.Value }).ToArray();
            userInfo.Permissions = Permissions;
            OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);

            if (impersonatedAccount.AccountRole == (int)AccountRole.DataProvider)
            {
                //return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });
                return Json(new { url = "/dashboard/index/lmpressionlog/" });

            }
            if (string.IsNullOrEmpty(saveModel.returnUrl))
            {
                //return RedirectToAction("index", "dashboard", new { charttype = "ad" });
                return Json(new { url = "/dashboard/index/ad/" });
            }

            return Redirect(saveModel.returnUrl);
        }
        #endregion
        #region Payment
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        public ActionResult GetPaymentAccountsNames(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetPaymentDetails( new GetPaymentDetailsRequest { AccountId= accountId, PaymentAccountType= accountType });
            return Json(accounts);
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        public ActionResult GetPaymentAccountDetails(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetFullPaymentDetails( new GetFullPaymentDetailsRequest {  AccountId=accountId, PaymentAccountType= accountType, PaymentAccountSubType= PayemntAccountSubType.Payment });
            return Json(accounts);
        }

        private AddPaymentViewModel GetAddPaymentViewModel()
        {
            ViewBag.isAdmin = true;
            var model = new AddPaymentViewModel();
            //var users = _userService.GetAllUser();

            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };

            // load payment types
            var paymentTypes = _paymentTypeService.GetAll();
            var paymentTypesList = new List<SelectListItem> { optionalItem };
            paymentTypesList.AddRange(paymentTypes.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            model.PaymentTypes = paymentTypesList;

            /*var currencies = _currencyService.GetAll();
            var currenciesList = new List<SelectListItem> { };
            currenciesList.AddRange(currencies.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            model.Currencies = currenciesList;*/

            return model;
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult AddPayment()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AddPayment","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var model = GetAddPaymentViewModel();
            model.PaymentDto = new NewPaymentDto();

            return View(model);
        }


        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult GetAddPaymentLookups()
        {
          
            var model = GetAddPaymentViewModel();
            model.PaymentDto = new NewPaymentDto();
            model.VATAmountPercentageValue = GetVatAmountPercentageValue();
            model.VATAmountPercentageString = GetVatAmountPercentage();
            return Json(model);
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps,AccountManager")]
        [HttpPost]
        [RequireHttps(Order = 1)]

        public ActionResult SaveAddPayment([FromBody]NewPaymentDto paymentDto)
        {
            var isValid = true;

            try
            {

                var result = ValidateNewPaymentDto(paymentDto);
                if (result.Errors.Count > 0)
                {
                    isValid = false;
                
                         AddErrorMsgs(result);
                    return Json(new { Status = true }, ResourcesUtilities.GetResource("AddPayment", "Titles"), ResponseStatus.businessException);
                }
                if (isValid)
                {

                    _accountService.AddPayment(paymentDto);


                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("Payments", "AccountHistory"));

                    return Json(new { Status = true }, ResourcesUtilities.GetResource("AddPayment", "Titles"), ResponseStatus.success);
                }
            }
            catch (BusinessException exception)
            {
                AddErrorMsgs(exception);
                return Json(new { Status = true }, ResourcesUtilities.GetResource("AddPayment", "Titles"), ResponseStatus.businessException);

            }


            var model = GetAddPaymentViewModel();
            model.PaymentDto = paymentDto;
            return Json(new { Status = true }, ResourcesUtilities.GetResource("AddPayment", "Titles"), ResponseStatus.success);
        }

        public BusinessException ValidateNewPaymentDto(NewPaymentDto dto)
        {
            var error = new BusinessException();
            if (!dto.AccountId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentAccount" });
            }
            if (!dto.TransactionDate.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentDate" });
            }
            if (!dto.PaymentType.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentType" });
            }
            if (!dto.Amount.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredAmount" });
            }
            else
            {
                if (dto.Amount <= 0 || dto.Amount > 99999999)
                {
                    error.Errors.Add(new ErrorData { ID = "MaxPayment" });
                }
            }



            return error;
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps,AccountManager")]
        [HttpPost]
        [RequireHttps(Order = 1)]

        public ActionResult AddPayment(NewPaymentDto paymentDto)
        {
            var isValid = true;

            try
            {
              
                var result = ValidateNewPaymentDto(paymentDto);  
                if (result.Errors.Count > 0)
                {
                    isValid = false;
                    foreach (var errorData in result.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }

                }
                if (isValid)
                {
                   
                        _accountService.AddPayment(paymentDto);

                  
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("AddPayment", "AccountManagement");
                }
            }
            catch (BusinessException exception)
            {
                foreach (var errorData in exception.Errors)
                {
                    AddMessages(errorData.Message, MessagesType.Error);
                }
            }

            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AddPayment","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var model = GetAddPaymentViewModel();
            model.PaymentDto = paymentDto;
            return View(model);
        }



        public string GetVatAmountPercentage()
        {
            string VatAmountPercentage = ArabyAds.Framework.Utilities.FormatHelper.FormatPercentage(ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue);
            return VatAmountPercentage;
        }
        public decimal GetVatAmountPercentageValue()
        {
            var VatAmountPercentage = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue;
            return VatAmountPercentage;
        }


        #endregion
        #region Fund
        private AddFundViewModel GetAddFundViewModel()
        {
            ViewBag.isAdmin = true;
            var model = new AddFundViewModel();

            //Load fund types List
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };
            var FundTypes = _fundTransTypeService.GetAll();
            var FundTypesList = new List<SelectListItem> { optionalItem };
            FundTypesList.AddRange(FundTypes.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            model.FundTypes = FundTypesList;


            var currencies = _currencyService.GetAll();
            var currenciesList = new List<SelectListItem> { optionalItem };
            currenciesList.AddRange(currencies.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            model.Currencies = currenciesList;


            var types = _fundTypeService.GetAll();
            var typesList = new List<SelectListItem> { };
            typesList.AddRange(types.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString()
            }));
            typesList.First().Selected = true;
            model.Types = typesList;
            return model;
        }
        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult AddFund()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AddFund","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var model = GetAddFundViewModel();
            model.FundDto = new NewFundDto();
            return View(model);
        }
        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult GetAddFundLookups()
        {
      
            var model = GetAddFundViewModel();
            model.FundDto = new NewFundDto();
            model.VATAmountPercentageValue = GetVatAmountPercentageValue();
            model.VATAmountPercentageString = GetVatAmountPercentage();
            return Json(model);
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps,AccountManager")]

        [HttpPost]
        [RequireHttps(Order = 1)]

        public ActionResult SaveAddFund([FromBody]NewFundDto FundDto)
        {
            var isValid = true;

            //if (ModelState.IsValid)
            //{
            try
            {
                var result = ValidateNewFundDto(FundDto);
                if (result.Errors.Count > 0)
                {
                    isValid = false;
                    AddErrorMsgs(result);
                    return Json(new { Status = true }, ResourcesUtilities.GetResource("AddFund", "Titles"), ResponseStatus.businessException);
                }
                if (isValid)
                {
                    _fundTransactionService.AddFund(FundDto);
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("Amount", "AccountHistory"));
                    return Json(new { Status = true }, ResourcesUtilities.GetResource("AddFund", "Titles") , ResponseStatus.success);

                }
            }
            catch (BusinessException exception)
            {
                AddErrorMsgs(exception);
                return Json(new { Status = true}, ResourcesUtilities.GetResource("AddFund", "Titles"), ResponseStatus.businessException);
            }
            //}
          
            var model = GetAddFundViewModel();
            model.FundDto = FundDto;
            return Json(model, ResourcesUtilities.GetResource("AddFund", "Titles"), ResponseStatus.success);
        }



        private BusinessException ValidateNewFundDto(NewFundDto fundDto)
        {
            var error = new BusinessException();
            if (!fundDto.AccountId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentAccount" });
            }

            if (!fundDto.TransactionDate.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredFundDate" });
            }
            if (!fundDto.FundType.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredFundType" });
            }

            if (!fundDto.Amount.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredAmount" });
            }
            else
            {
                if (fundDto.Amount <= 0 || fundDto.Amount > 99999999)
                {
                    error.Errors.Add(new ErrorData { ID = "MaxFund" });
                }
            }
            if (!fundDto.OriginalAmount.HasValue && fundDto.CurrencyId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredOriginalAmount" });
            }
            if (fundDto.OriginalAmount.HasValue && !fundDto.CurrencyId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredCurrency" });
            }
            if (fundDto.FundType.HasValue && (fundDto.TypeId == (int)AccountFundTypeIds.ServiceCharge) && ((AccountFundTransTypeIds)fundDto.FundType != AccountFundTransTypeIds.Cash))
            {
                error.Errors.Add(new ErrorData { ID = "ServiceChargeFundType" });
            }
            return error;
        }




        [Authorize]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps,AccountManager")]

        [HttpPost]
        [RequireHttps(Order = 1)]

        public ActionResult AddFund(NewFundDto FundDto)
        {
            var isValid = true;

            //if (ModelState.IsValid)
            //{
            try
            {
                var result = ValidateNewFundDto(FundDto);
                if (result.Errors.Count > 0)
                {
                    isValid = false;
                    foreach (var errorData in result.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }

                }
                if (isValid)
                {
                    _fundTransactionService.AddFund(FundDto);

                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("AddFund", "AccountManagement");
                }
            }
            catch (BusinessException exception)
            {
                foreach (var errorData in exception.Errors)
                {
                    AddMessages(errorData.Message, MessagesType.Error);
                }
            }
            //}
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AddFund","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var model = GetAddFundViewModel();
            model.FundDto = FundDto;
            return View(model);
        }

        #endregion
        #region Settings
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult Settings(string id)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Settings","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            var accountid = 0;
            if (!int.TryParse(id, out accountid))
            {
                //get current account id
                accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            }
            var model = _accountService.GetAccountSetting(new ValueMessageWrapper<int> { Value = accountid });
            model.AccountId = accountid;
            return View("AccountSettings", model);
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        public ActionResult GetSettings(string id)
        {
            

            var accountid = 0;
            if (!int.TryParse(id, out accountid))
            {
                //get current account id
                accountid = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            }
            var model = _accountService.GetAccountSetting(new ValueMessageWrapper<int> { Value = accountid });
            model.AccountId = accountid;
            return Json( model);
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult Settings([FromBody]AccountSettingDto data)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("Settings","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SaveAccountSetting(data);

                    #region Refresh UserInfo
                    AdFalconUserInfo userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                    userInfo.AllowAPIAccess = data.AllowAPIAccess;
                    OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);
                    #endregion

                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    NotificationMessages.Add(new ResultMessage { Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("PageDispalyName", "Global")), Type = MessagesType.Success });

                    return Json( new { id = data.AccountId, ResultSaved = true, MessageTitle = ResourcesUtilities.GetResource("AccountSettings", "Titles"), Messages = NotificationMessages, ResponseStatus.success });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                }
            }
            return View("AccountSettings", data);
        }

        #endregion


        #region Account Cost Elments


        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountCostElementEnableDisable(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountCostElement(new ValueMessageWrapper<int> { Value = Id.Value });

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements")) } );
        }



        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult RemoveAccountCostElement(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.RemoveAccountCostElement(new ValueMessageWrapper<int> { Value = Id.Value });
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements")) } );
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AccountCostElements()
        {
            CostElementListViewModel costElements = LoadAccountCostElementsData();
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount == null)
                costElements.BusinessName = OperationContext.Current.UserInfo<AdFalconUserInfo>().FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().LastName;
            else
                costElements.BusinessName = OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.LastName;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {


                                                      new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("CostElement","Account"),
                                                  Url=" ",
                                                  Order =1,
                                              }

                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View("IndexAccountCostElements", costElements);
        }

        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AccountCostElments()
        {
            var result = GetAccountCostElementsQueryResult();
            // var model = LoadAccountCostElementsData();
            ViewData["total"] = Convert.ToInt32(result.TotalCount);

            if (result.Items == null)
                result.Items = new List<AccountCostElementDto>();
            return Json( new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult AccountCostElements(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete AccounCostElements
                _accountService.RemoveAccountCostElementBulk(checkedRecords);
            }

            return RedirectToAction("AccountCostElements");
        }

        protected AccountCostElementCriteria GetAccountCostElementsCriteria()
        {


            ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"].ToString();


            var criteria = new AccountCostElementCriteria
            {

                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.name

                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected AccountCostElementResultDto GetAccountCostElementsQueryResult()
        {
            var criteria = GetAccountCostElementsCriteria();


            var result = _accountService.QueryAccountCostElements(criteria);
            return result;
        }

        protected CostElementListViewModel LoadAccountCostElementsData()
        {
            var result = GetAccountCostElementsQueryResult();
            var items = result.Items;
            ViewData["total"] = 0;
            if (items == null)
                items = new List<AccountCostElementDto>();
            #region Actions
            var action = GetAccountCostElementsAction();
            #endregion
            // var toolTip = GetDealCampaignTooltip();

            return new CostElementListViewModel()
            {

                Items = items,



                TopActions = action,
                BelowAction = null,
                ToolTips = null
            };
        }


        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetAccountCostElementsAction()
        {
            #region Actions

            var actions = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
                {

                    new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("CostElement", "Account"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },

                    new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"),
                                ExtraPrams3="AccountCostElement"
                        }
                };
            #endregion

            return actions;
        }
        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetAccountCostElementTooltip()
        {
            // Create the tool tip actions
            return new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
                {

                       new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


                     }


                };
        }
        private string GetCostValue(CostElementDto element)
        {
            IList<string> keyValueList = new List<string>();

            switch (element.TypeId)
            {
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Fixed:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value));
                        }
                        break;
                    }
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value * 100));
                        }
                        break;
                    }
            }

            return string.Join(",", keyValueList);
        }
        private List<SelectListItem> GetCostElementList(string lookupType, int selectedValue)
        {
            var items = lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = lookupType });
            items.Items = items.Items.OrderBy(x => x.Name.Value, StringComparer.InvariantCultureIgnoreCase).ToList();
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0#1#0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};
            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = string.Format("{0}#{1}#{2}", item.ID.ToString(), (item as CostElementDto).TypeId, GetCostValue(item as CostElementDto)),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }));
            return lookupsList;
        }
		
		
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]

        public ActionResult AcountCostElementProviders()
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
            //ProvidersList.Insert(0, lookupsList[0]);
            return Json(ProvidersList);
        }
		
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]

        public ActionResult AcountCostElement(int? id)
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
            return PartialView("AcountCostElement", viewModel);
        }


        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElement(AccountCostElementDto dto)
        {

            bool result = false;
            string massage = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements"));
            try
            {
                result = _accountService.SaveAccountCostElement(dto).Value;

            }
            catch (BusinessException ex)
            {
                var str = ex.Message;
                return Json(new { Success = false, Message = str });
            }
            if (!result)
            {
                massage = "Somthing Went Wrong";
            }
            return Json(new { Success = result, Message = massage });
        }

        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFeeJson([FromBody]AccountFeeDto dto)
        {
            bool result = false;



         



            // FillUserData(viewModel, model, campaignId, adGroupId);
            try
            {
                result = _accountService.SaveAccountFee(dto).Value;
                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("FeeElement", "Account"));

            }
            catch (BusinessException ex)
            {
                AddErrorMsgs(ex);
                return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.businessException);
            }
            
           
            return Json(new { Success = result}, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.success);

        }


        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElementJson([FromBody]AccountCostElementDto dto)
        {

            bool result = false;
            
            try
            {
                result = _accountService.SaveAccountCostElement(dto).Value;
                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("CostElement", "CostElements"));

            }
            catch (BusinessException ex)
            {
                AddErrorMsgs(ex);
                return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.businessException);
            }
          
            return Json(new { Success = result }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.success);

        }




        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountCostElementEnableDisableJson(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountCostElement(new ValueMessageWrapper<int> { Value = Id.Value });
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("CostElement", "CostElements"));

                }
                catch (BusinessException exception)
                {

                    AddErrorMsgs(exception);
                    return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(new { Success = true }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.success);
        }



        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]
        [HttpPost]
        public ActionResult RemoveAccountCostElementJson([FromBody]BulkIds ids)
        {

            {
                try
                {
                   
                        _accountService.RemoveAccountCostElementBulk(ids.Ids);
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("CostElement", "CostElements"));


                }
                catch (BusinessException exception)
                {
                    AddErrorMsgs(exception);
                    return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(new { Success = true }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.success);
        }


        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]
       
      
        [HttpPost]
        public ActionResult RemoveFeeJson([FromBody] BulkIds ids)
       
        {

            {
                try
                {
                   
                        _accountService.RemoveAccountFeeBulk(ids.Ids);
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("FeeElement", "Account"));


                }
                catch (BusinessException exception)
                {

                    AddErrorMsgs(exception);
                    return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(new { Success = true }, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.success);
        }


        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountFeeEnableDisableJson(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountFee(new ValueMessageWrapper<int> { Value = Id.Value });
                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("FeeElement", "Account"));

                }
                catch (BusinessException exception)
                {

                    AddErrorMsgs(exception);
                    return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(new { Success = true }, ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"), ResponseStatus.success);
        }
        #endregion

        #region Fee


        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountFeeEnableDisable(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountFee(new ValueMessageWrapper<int> { Value = Id.Value });

                }
                catch (BusinessException exception)
                {
                    AddErrorMsgs(exception);
                    return Json(new { Success = false }, ResourcesUtilities.GetResource("AddAccountCostElement", "Titles"), ResponseStatus.businessException);
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account")) } );
        }



        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult RemoveFee(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.RemoveAccountFee( new ValueMessageWrapper<int> { Value = Id.Value });
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account")) } );
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult AccountFees()
        {
            FeeListViewModel costElements = LoadAccountFeesData();
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount == null)
                costElements.BusinessName = OperationContext.Current.UserInfo<AdFalconUserInfo>().FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().LastName;
            else
                costElements.BusinessName = OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.FirstName + " " + OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.LastName;
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                                      new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("AccountFeeElement","Global"),
                                                  Url=" ",
                                                  Order =1,
                                              }
                                                      
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View("IndexAccountFees", costElements);
        }

        //[AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AccountFees()
        {
            var result = GetAccountFeesQueryResult();
            // var model = LoadAccountCostElementsData();
            ViewData["total"] = Convert.ToInt32(result.TotalCount);

            if (result.Items == null)
                result.Items = new List<AccountFeeDto>();
            return Json( new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult AccountFees(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                //Delete AccounCostElements
                _accountService.RemoveAccountFeeBulk(checkedRecords);
            }

            return RedirectToAction("AccountFees");
        }

        protected AccountFeeCriteria GetAccountFeesCriteria()
        {


            ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"].ToString();


            var criteria = new AccountFeeCriteria
            {

                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.name

                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected AccountFeeResultDto GetAccountFeesQueryResult()
        {
            var criteria = GetAccountFeesCriteria();


            var result = _accountService.QueryAccountFees(criteria);
            return result;
        }

        protected FeeListViewModel LoadAccountFeesData()
        {
            var result = GetAccountFeesQueryResult();
            var items = result.Items;
            ViewData["total"] = 0;
            if (items == null)
                items = new List<AccountFeeDto>();
            #region Actions
            var action = GetAccountFeesAction();
            #endregion
            // var toolTip = GetDealCampaignTooltip();

            return new FeeListViewModel()
            {

                Items = items,



                TopActions = action,
                BelowAction = null,
                ToolTips = null
            };
        }


        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetAccountFeesAction()
        {
            #region Actions

            var actions = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
                {

                    new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("CostElement", "Account"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },

                    new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Create",
                            ClassName = "primary-btn",
                            Type = ActionType.Link,
                            DisplayText = ResourcesUtilities.GetResource("AddAccountFeeElement", "Titles"),
                                ExtraPrams3="AccountFee"


                        }
                };
            #endregion

            return actions;
        }
        protected virtual List<ArabyAds.AdFalcon.Web.Controllers.Model.Action> GetAccountFeeTooltip()
        {
            // Create the tool tip actions
            return new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
                {

                       new ArabyAds.AdFalcon.Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",


                     }


                };
        }
        private string GetCostValue(FeeDto element)
        {
            IList<string> keyValueList = new List<string>();

            switch (element.TypeId)
            {
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Fixed:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value));
                        }
                        break;
                    }
                case (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage:
                    {
                        foreach (var item in element.Values)
                        {
                            keyValueList.Add(string.Format("{0}:{1}", item.CostModelWrapper.ID, item.Value * 100));
                        }
                        break;
                    }
            }

            return string.Join(",", keyValueList);
        }
        private List<SelectListItem> GetFeeList(string lookupType, int selectedValue)
        {
            var items = lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = lookupType });
            items.Items = items.Items.OrderBy(x => x.Name.Value, StringComparer.InvariantCultureIgnoreCase).ToList();
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0#1#0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};
            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = string.Format("{0}#{1}#{2}", item.ID.ToString(), (item as CostElementDto).TypeId, GetCostValue(item as CostElementDto)),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }));
            return lookupsList;
        }
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]

        public ActionResult AccountFee(int? id)
        {
            AccountFeeDto viewModel = new AccountFeeDto();
            
            return PartialView("AccountFee", viewModel);
        }

        [AcceptVerbs("Post")]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFee(AccountFeeDto dto)
        {
            bool result = false;
            string massage = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account"));
            try
            {
                result = _accountService.SaveAccountFee(dto).Value;
            }
            catch (BusinessException ex)
            {
                var str = ex.Message;
                return Json(new { Success = false, Message = str });
            }
            if (!result)
            {
                massage = "Somthing Went Wrong";
            }
            return Json(new { Success = result, Message = massage });
        }
        #endregion

        #region  Transaction VAT

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager")]
        public ActionResult TransactionVATHistory()
        {
            TransactionVATHistoryModel vatmodel = LoadTransactionVATData();

            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {


                                                      new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("TransactionVATHistory","Global"),
                                                  Url=" ",
                                                  Order =1,
                                              }

                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            #endregion

            return View(vatmodel);
        }

        //[AcceptVerbs("Post")]
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _TransactionVATHistory()
        {
            var result = GetTransactionVATQueryResult();
            // var model = LoadAccountCostElementsData();
            ViewData["total"] = Convert.ToInt32(result.Total);

            if (result.Items == null)
                result.Items = new List<FundTransactionDto>();
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.Total)
            });

        }



        protected TransactionVATCriteria GetTransactionVATCriteria(bool IsReport = false)
        {


            ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"].ToString();
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);

            var criteria = new TransactionVATCriteria
            {
                DataFrom = filter.FromDate,
                DataTo = filter.ToDate,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : IsReport ? (int?)null : 1,
                AccountId = string.IsNullOrWhiteSpace(Request.Form["AccountId"]) ? null : (int?)Convert.ToInt32(Request.Form["AccountId"]),
                Details = string.IsNullOrWhiteSpace(Request.Form["Details"]) ? false : Request.Form["Details"].ToString().ToLower() == "false" ? false : true,
                Payments = string.IsNullOrWhiteSpace(Request.Form["FilterType"]) ? false : Request.Form["FilterType"].ToString().ToLower() == "fund" ? false : true,
                //StatusId = filter.StatusId
            };
            return criteria;
        }
        protected FundResultDto GetTransactionVATQueryResult(bool IsReport = false)
        {
            var criteria = GetTransactionVATCriteria(IsReport);


            var result = _fundService.GetTransactionVATHistory(criteria);
            return result;
        }

        protected TransactionVATHistoryModel LoadTransactionVATData()
        {
            var result = GetTransactionVATQueryResult();
            var items = result.Items;
            ViewData["total"] = result.Total;
            if (items == null)
                items = new List<FundTransactionDto>();
            #region Actions

            #endregion

            return new TransactionVATHistoryModel()
            {
                Items = items
            };
        }
        [RequireHttps(Order = 1)]
        public ActionResult TransactionVATReportExport(IFormCollection collection, string exportType, string fromDate, string toDate, string AccountId, string Details, string FilterType)
        {
            List<FundTransactionDto> reportingList;

            reportingList = GetTransactionVATQueryResult(true).Items.ToList();
            bool DetailsBool = string.IsNullOrWhiteSpace(Details) ? false : Details.ToLower() == "false" ? false : true;
            return _WriteReportHelper.BuildFundTransactionFile(reportingList, exportType, DetailsBool,new List<Services.Interfaces.DTOs.Reports.Dashboard.KeyValueDto>(), "VATLog");
        }
        #endregion


       
        [HttpPost]
        [HttpPut]
        [DenyNonPrimaryRole]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SavePaymentFile(AccountPaymentDetailDto bankAccountDto)
        {

            List<ResultMessage> rMessages = new List<ResultMessage>();


            try
            {
                var TaxDocument = Request.Form.Files["PaymentsDocument"];
                if (TaxDocument != null && TaxDocument.Length != 0)
                {

                    MemoryStream target = new MemoryStream();
                    //TaxDocument.OpenReadStream().CopyTo(target);

                    using (var reader = new StreamReader(TaxDocument.OpenReadStream()))
                    {
                        int accountA =0;
                        decimal amountB = 0;
                        string fundType = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            accountA=Convert.ToInt32(values[0]);
                            amountB=Convert.ToDecimal(values[1]);
                            fundType = values[2];
                            NewFundDto fundDto = new NewFundDto();
                            NewPaymentDto PaymentDto = new NewPaymentDto();
                            if (fundType.ToLower() == "fund" || fundType.ToLower() == "refund")
                            {
                                fundDto.AccountId = accountA;
                                fundDto.Amount = amountB;
                                fundDto.TypeId = 1;
                                if (fundType.ToLower() == "refund")
                                    //refund
                                    fundDto.TypeId = 2;

                                fundDto.TransactionDate = Framework.Utilities.Environment.GetServerTime();
                                fundDto.FundType = 3;
                                fundDto.Comment = "ArabyAds account’s closure transaction";
                                _fundTransactionService.AddFund(fundDto);
                            }
                            else
                            {
                                PaymentDto.AccountId = accountA;
                                PaymentDto.Amount = amountB;
                                PaymentDto.PaymentType = 1;

                                PaymentDto.TransactionDate = Framework.Utilities.Environment.GetServerTime(); 
                                PaymentDto.ForMonth = Framework.Utilities.Environment.GetServerTime(); 
                                PaymentDto.Comment = "ArabyAds account’s closure transaction";

                                _accountService.AddPayment(PaymentDto);
                            }
                         

                          
                          

                            




         

                        }
                    }

                }

                return Json("Payment Details","Done" ,ResponseStatus.success);
            }
            catch (BusinessException exception)
            {
                
                AddErrorMsgs(exception);
             
                return Json("Payment Details", ResponseStatus.businessException);

            }



            
        }

    }
}
