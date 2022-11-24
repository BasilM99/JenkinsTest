using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Common.UserInfo;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Web.Controllers.Model.AccountManagement;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{
    public class AccountManagementController : AuthorizedControllerBase
    {
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
        public AccountManagementController(IAccountService accountService,
            IUserService userService,
            IPaymentTypeService paymentTypeService,
            IFundTransTypeService fundTransTypeService,
            IFundTypeService fundTypeService,
            IFundTransactionService fundTransactionService,
            ICurrencyService currencyService, ILookupService lookupService, IFundsService FundsService, IPartyService PartyService)
        {
            _accountService = accountService;
            _userService = userService;
            _paymentTypeService = paymentTypeService;
            _fundTransTypeService = fundTransTypeService;
            _fundTransactionService = fundTransactionService;
            _currencyService = currencyService;

            _partyService = PartyService;
            _fundTypeService = fundTypeService;
            this.lookupService = lookupService;
            this._fundService = FundsService;
            _WriteReportHelper = new WriteReportDocumentsHelper();
        }

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
                return Json(results, JsonRequestBehavior.AllowGet);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();
           
            foreach (var id in TagIds)
            {
                var item = _accountService.GetById(id);
                results.Add(item);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
          
        }
        [OutputCache(Duration = 9200, VaryByParam = "q;page")]
        [RequireHttps]
        public ActionResult GetAccountSecure(string q, string culture,int page)
        {
            return Json(ReturnAccountResult(q, culture, page), JsonRequestBehavior.AllowGet);
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
        public ActionResult Impersonate(AccountSearchSaveModel saveModel)
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
            if (!string.IsNullOrEmpty(saveModel.AccountIdStr))
            {
                var Ids = saveModel.AccountIdStr.Split('_');
                saveModel.UserId = Convert.ToInt32(Ids[1]);
                saveModel.AccountId = Convert.ToInt32(Ids[0]);
            }
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
                saveModel.UserId = userInfo.OriginalUserId;
            }
            var impersonatedAccount = _userService.Impersonate(saveModel.AccountId, saveModel.UserId);
            userInfo.AccountId = saveModel.AccountId;
            userInfo.UserId = saveModel.UserId;
            userInfo.IsPrimaryUser = impersonatedAccount.IsPrimaryUser;
            userInfo.AllowAPIAccess = impersonatedAccount.AllowAPIAccess;
            userInfo.ImpersonatedAccount = impersonatedAccount;
            userInfo.AccountRole = impersonatedAccount.AccountRole;
            userInfo.VATValue = _accountService.GetVATValueByAccountId((int)userInfo.AccountId);

            var Permissions = _userService.GetAccountAdPermissions(userInfo.AccountId.Value).ToArray();
            userInfo.Permissions = Permissions;
            OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);

            if (impersonatedAccount.AccountRole == (int)AccountRole.DataProvider)
            {
                return RedirectToAction("index", "dashboard", new { charttype = "lmpressionlog" });

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
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        public ActionResult GetPaymentAccountsNames(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetPaymentDetails(accountId, accountType);
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }

        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,Finance Manager")]
        [DenyRole(Roles = "AppOps")]
        public ActionResult GetPaymentAccountDetails(int accountId, PayemntAccountType accountType)
        {
            //load the system Account 
            var accounts = _accountService.GetFullPaymentDetails(accountId, accountType, PayemntAccountSubType.Payment);
            return Json(accounts, JsonRequestBehavior.AllowGet);
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
        [DenyRole(Roles = "AppOps,AccountManager")]
        [HttpPost]
        [RequireHttps(Order = 1)]

        public ActionResult AddPayment(NewPaymentDto paymentDto)
        {
            var isValid = true;

            try
            {
              
                var result = paymentDto.Validate();
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
                var result = FundDto.Validate();
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
            var model = _accountService.GetAccountSetting(accountid);
            model.AccountId = accountid;
            return View("AccountSettings", model);
        }

        [Authorize]
        [AuthorizeRole(Roles = "Administrator,AppOps")]
        [HttpPost]
        [RequireHttps(Order = 1)]
        public ActionResult Settings(AccountSettingDto data)
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
                    return RedirectToAction("Settings", new { id = data.AccountId });
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


        //[AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountCostElementEnableDisable(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountCostElement(Id.Value);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements")), JsonRequestBehavior.AllowGet });
        }



        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult RemoveAccountCostElement(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.RemoveAccountCostElement(Id.Value);
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements")), JsonRequestBehavior.AllowGet });
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

        //[AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AccountCostElments()
        {
            var result = GetAccountCostElementsQueryResult();
            // var model = LoadAccountCostElementsData();
            ViewData["total"] = Convert.ToInt32(result.TotalCount);

            if (result.Items == null)
                result.Items = new List<AccountCostElementDto>();
            return View("AccountCostElmentsGrid", new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [AcceptVerbs(HttpVerbs.Post)]
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


            Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"];


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


        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetAccountCostElementsAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("CostElement", "Account"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
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
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetAccountCostElementTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
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

        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElement(AccountCostElementDto dto)
        {

            bool result = false;
            string massage = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("CostElement", "CostElements"));
            try
            {
                result = _accountService.SaveAccountCostElement(dto);

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

        #region Fee


        //[AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult AccountFeeEnableDisable(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.EnableDisableAccountFee(Id.Value);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account")), JsonRequestBehavior.AllowGet });
        }



        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult RemoveFee(int? Id)
        {

            {
                try
                {
                    if (Id.HasValue)
                        _accountService.RemoveAccountFee(Id.Value);
                    //_demandSupplyService.SaveAccountCostElementsg(floorPrice.SaveDto);

                }
                catch (BusinessException exception)
                {
                    var errors = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                    return Json(new { Success = false, Message = errors });
                }
            }

            return Json(new { Success = true, Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account")), JsonRequestBehavior.AllowGet });
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

        //[AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AccountFees()
        {
            var result = GetAccountFeesQueryResult();
            // var model = LoadAccountCostElementsData();
            ViewData["total"] = Convert.ToInt32(result.TotalCount);

            if (result.Items == null)
                result.Items = new List<AccountFeeDto>();
            return View("AccountFeesGrid", new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }



        [AcceptVerbs(HttpVerbs.Post)]
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


            Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"];


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


        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetAccountFeesAction()
        {
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
                        {
                            ActionName = "Delete",
                            ClassName = "delete-button",
                            Type = ActionType.Submit,
                            DisplayText = ResourcesUtilities.GetResource("Delete", "Commands"),
                            ExtraPrams=ResourcesUtilities.GetResource("CostElement", "Account"),
                            ExtraPrams2 = ResourcesUtilities.GetResource("Delete", "Confirmation") // like are u sure ?

                           
                        },

                    new Noqoush.AdFalcon.Web.Controllers.Model.Action()
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
        protected virtual List<Noqoush.AdFalcon.Web.Controllers.Model.Action> GetAccountFeeTooltip()
        {
            // Create the tool tip actions
            return new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                {

                       new Noqoush.AdFalcon.Web.Controllers.Model.Action()
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

        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,AdOps")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFee(AccountFeeDto dto)
        {
            bool result = false;
            string massage = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("FeeElement", "Account"));
            try
            {
                result = _accountService.SaveAccountFee(dto);
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

        //[AcceptVerbs(HttpVerbs.Post)]
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
            return View("AccountCostElmentsGrid", new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.Total)
            });

        }



        protected TransactionVATCriteria GetTransactionVATCriteria(bool IsReport = false)
        {


            Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter filter = new Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int?)null : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int?)null : Convert.ToInt32(Request.Form["size"]);
            filter.name = string.IsNullOrWhiteSpace(Request.Form["Name"]) ? null : Request.Form["Name"];
            filter.FromDate = string.IsNullOrWhiteSpace(Request.Form["FromDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["FromDate"], Config.ShortDateFormat, null);
            filter.ToDate = string.IsNullOrWhiteSpace(Request.Form["ToDate"]) ? (DateTime?)null : DateTime.ParseExact(Request.Form["ToDate"], Config.ShortDateFormat, null);

            var criteria = new TransactionVATCriteria
            {
                DataFrom = filter.FromDate,
                DataTo = filter.ToDate,
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : IsReport ? (int?)null : 1,
                AccountId = string.IsNullOrWhiteSpace(Request.Form["AccountId"]) ? null : (int?)Convert.ToInt32(Request.Form["AccountId"]),
                Details = string.IsNullOrWhiteSpace(Request.Form["Details"]) ? false : Request.Form["Details"].ToLower() == "false" ? false : true,
                Payments = string.IsNullOrWhiteSpace(Request.Form["FilterType"]) ? false : Request.Form["FilterType"].ToLower() == "fund" ? false : true,
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
        public ActionResult TransactionVATReportExport(FormCollection collection, string exportType, string fromDate, string toDate, string AccountId, string Details, string FilterType)
        {
            List<FundTransactionDto> reportingList;

            reportingList = GetTransactionVATQueryResult(true).Items.ToList();
            bool DetailsBool = string.IsNullOrWhiteSpace(Details) ? false : Details.ToLower() == "false" ? false : true;
            return _WriteReportHelper.BuildFundTransactionFile(reportingList, exportType, DetailsBool,new List<Services.Interfaces.DTOs.Reports.Dashboard.KeyValueDto>(), "VATLog");
        }
        #endregion
    }
}
