using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;
using Noqoush.Framework.UserInfo;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Business.Domain.Exceptions;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DSP, AccountRole.DataProvider } )]
    public class AppSiteController : AuthorizedControllerBase
    {

        private IAppSiteService _appSiteService;
        private IAppSiteTypeService _appSiteTypeService;
        private IRefreshIntervalService _refreshIntervalService;
        protected IAccountService _accountService;

        private IThemeService _themeService;
        public AppSiteController(IAppSiteService appSiteService,
                                 IAppSiteTypeService appSiteTypeService,
                                             IAccountService accountService,

                                 IThemeService themeService)
        {
            this.DispalyResourceName = "AppSiteDispalyName";
            _appSiteService = appSiteService;
            _appSiteTypeService = appSiteTypeService;
            _themeService = themeService;
            _accountService = accountService;
        }


        #region Helpers


        private AppSiteListResultDto getQueryResult(AppSiteCriteriaModel appCriteria)
        {

            string name = string.IsNullOrWhiteSpace(Request.Form["page"]) ? null : Request.Form["Name"];

            var cratiria = new Noqoush.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase
            {
                Size = !appCriteria.Size.HasValue ? Config.PageSize : appCriteria.Size.Value,
                AccountId = appCriteria.AccountId.HasValue ? appCriteria.AccountId : Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                TypeId = appCriteria.FilterType,
                Page = appCriteria.Page.HasValue ? appCriteria.Page.Value : 1,
                ExecludedAppId = appCriteria.AppId.HasValue ? appCriteria.AppId.Value : new int?(),
                DateFrom = appCriteria.FromDate.HasValue ? appCriteria.FromDate.Value : new DateTime?(),
                DateTo = appCriteria.ToDate.HasValue ? appCriteria.ToDate.Value : new DateTime?(),

                Name = name
            };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                cratiria.UserId = UserId;
                //appCriteria.UserId = UserId;
            }
            var result = _appSiteService.QueryByCratiria(cratiria);
            return result;
        }

        private ListViewModel LoadData(int? filterType, int? page = null, int? size = null, int? accountId = null)
        {
            var isAllFilterSelected = filterType.HasValue == false;
            AppSiteCriteriaModel appCriteria = new AppSiteCriteriaModel()
            {
                FilterType = filterType,
                Page = page,
                Size = size,
                AccountId = accountId
            };

            var result = getQueryResult(appCriteria);
            var items = result.Items;
            ViewData["total"] = result.TotalCount;
            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                              {
                                  new Action()
                                      {
                                          ActionName = "Delete",
                                          ClassName = "delete-button",
                                          Type = ActionType.Submit,
                                          DisplayText = ResourcesUtilities.GetResource("Delete", "Commands")
                                      },
                                  new Action()
                                      {
                                          ActionName = "Create",
                                          ClassName = "primary-btn",
                                          Type = ActionType.Link,
                                          DisplayText = ResourcesUtilities.GetResource("AddNewAppSite", "Commands")
                                      }
                              };


            //create the filter bar items
            var FilterBar = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                                {
                                    new Action()
                                        {
                                            Code = "",
                                            DisplayText = ResourcesUtilities.GetResource("All", "Global"),
                                            ActionName = "Index",
                                            ControllerName = "AppSite",
                                            IsSelected = isAllFilterSelected
                                        }
                                };


            var types = _appSiteTypeService.GetAll();
            FilterBar.AddRange(
                types.Select(
                    appSiteTypeDto =>
                    new Action()
                    {
                        Code = appSiteTypeDto.Id.ToString(),
                        DisplayText = appSiteTypeDto.Name.ToString(),
                        ActionName = "Index",
                        ControllerName = "AppSite",
                        IsSelected = (isAllFilterSelected == false && appSiteTypeDto.Id == filterType.Value)
                    }));

            // Create the tool tip actions
            var toolTips = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                               {
                                   new Action()
                                       {
                                           Code = "0",
                                           DisplayText = ResourcesUtilities.GetResource("Edit", "Commands"),
                                           ClassName = "grid-tool-tip-edit",
                                           ActionName = "Create"
                                       },
                                   new Action()
                                       {
                                           Code = "1",
                                           DisplayText = ResourcesUtilities.GetResource("Reports", "Commands"),
                                           ClassName = "grid-tool-tip-reports",
                                           URL = Url.Action("index", "Reports", new {reportType = "app"})
                                       },
                          new Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",

                        }
                               };
            return new ListViewModel()
            {
                Items = items,
                TopActions = actions,
                BelowAction = actions,
                FilterBar = FilterBar,
                ToolTips = toolTips
            };
        }

        private void setThemeSelected(IEnumerable<SelectListItem> themes, AppSiteDto appSiteDto)
        {
            var value = appSiteDto.Theme.Id.ToString() + "!" + appSiteDto.Theme.BackgroundColor + "!" +
                        appSiteDto.Theme.TextColor;
            var themeSelectItem = themes.FirstOrDefault(item => item.Value == value);
            if (themeSelectItem != null)
            {
                themeSelectItem.Selected = true;
            }
        }

        private CreateViewModel InitCreate()
        {
            //Load Themes List
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };
            List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.ThemeDto> ThemesDtos = _themeService.GetAll().ToList();
            var themes = new List<SelectListItem> { optionalItem };
            foreach (var item in ThemesDtos)
            {
                var selectItem = new SelectListItem
                {
                    Value = item.Id.ToString() + "!" + item.BackgroundColor + "!" + item.TextColor,
                    Text = item.Name.ToString()
                };
                themes.Add(selectItem);
            }
            //Load keyword List
            var keywordauto = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "AppSiteDto_Kewords_Name",
                Name = "AppSiteDto.Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged"
            };

            //get the Keyword Tag Cloud
            var keywordservice = Noqoush.Framework.IoC.Instance.Resolve<IKeywordService>();
            //TODo: Osaleh to get item count from Configuration setting
            var keywords = keywordservice.GetTop(null);
            var keywordTags =
                keywords.Select(
                    keywordDto =>
                    new TagCloud() { Id = keywordDto.ID, DispalValue = keywordDto.Name.ToString(), Rank = keywordDto.Rank }).ToList();


            var types = _appSiteTypeService.GetAll();

            return new CreateViewModel()
            {
                AppSiteViewName = types.First().ViewName,
                AppSiteTypes = types,
                Themes = themes,

                KeywordViewModel =
                    new KeywordViewModel()
                    {
                        Prefix = "AppSiteDto.",
                        KewordAuto = keywordauto,
                        KeywordTags = keywordTags,
                        Keywords = new List<KeywordDto>(),
                        AllowInsert = false
                    }
            };
        }

        protected IEnumerable<Tab> GetTabs(int tabId, string extraPrams)
        {
            var tabs = new List<Tab>
                           {
                               new Tab
                                   {
                                       IsSelected = false,
                                       Action =new Action()
                                               {
                                                   DisplayText =ResourcesUtilities.GetResource("ApplicationInformation","AppSite"),
                                                   ActionName = "Create",
                                                   ExtraPrams = extraPrams
                                               }
                                   },
                               new Tab
                                   {
                                       IsSelected = false,
                                       Action =new Action()
                                               {
                                                   DisplayText = ResourcesUtilities.GetResource("Filters", "AppSite"),
                                                   ActionName = "Filters",
                                                   ExtraPrams = extraPrams
                                               }
                                   },
                               new Tab
                                   {
                                       IsSelected = false,
                                       Action =
                                           new Action()
                                               {
                                                   DisplayText = ResourcesUtilities.GetResource("Settings", "AppSite"),
                                                   ActionName = "Settings",
                                                   ExtraPrams = extraPrams
                                               }
                                   }
                           };

            if (Config.IsAdministrationApp)
            {
                tabs.Add(new Tab
                {
                    IsSelected = false,
                    Action = new Action()
                    {
                        DisplayText = ResourcesUtilities.GetResource("Approval", "AppSite"),
                        ActionName = "Approval",
                        ExtraPrams = extraPrams
                    }
                });
                tabs.Add(new Tab
                {
                    IsSelected = false,
                    Action = new Action()
                    {
                        DisplayText = ResourcesUtilities.GetResource("ServerSettings", "AppSite"),
                        ActionName = "ServerSetting",
                        ExtraPrams = extraPrams
                    }
                });

            }
            tabs[tabId].IsSelected = true;

            return tabs;

        }

        #endregion

        #region General
        public virtual ActionResult RedirectToAuditTrial(int id)
        {
            string originalPath = new Uri(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri).OriginalString;

            try
            {
                int objectRootTypeId = _accountService.GetObjectRootTypeId("Noqoush.AdFalcon.Domain.Common.Model.AppSite.AppSite");

                return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = id, objectRootTypeId = objectRootTypeId, returnUrl = originalPath });
            }
            catch (Exception e)
            {

                throw e;
            }


        }


        public ActionResult Index(int? filterType)
        {
            #region BreadCrumb

            var falg = false;
            if (filterType.HasValue)
            {
                var item = _appSiteTypeService.Get(filterType.Value);
                if (item != null)
                {
                    var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = item.Name.ToString(),
                                                          Order = 2
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =
                                                              ResourcesUtilities.GetResource("AppSiteList",
                                                                                             "SiteMapLocalizations"),
                                                          Url = Url.Action("Index", "appSite"),
                                                          Order = 1,
                                                      }
                                              };
                    ViewData["BreadCrumbLinks"] = breadCrumbLinks;
                    falg = true;
                }
            }
            if (!falg)
            {
                var breadCrumbLinks = new List<BreadCrumbModel>
                                          {
                                              new BreadCrumbModel()
                                                  {
                                                      Text =
                                                          ResourcesUtilities.GetResource("AppSiteList",
                                                                                         "SiteMapLocalizations"),
                                                      Order = 1,
                                                  }
                                          };
                ViewData["BreadCrumbLinks"] = breadCrumbLinks;
            }


            #endregion
            return View(LoadData(filterType));
        }
        [RequireHttps(Order = 1)]
        public ActionResult AppSiteSearch(int? filterType, int? accountId)
        {
            var model = LoadData(filterType, null, null, accountId);
            return PartialView(model);
        }
        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps,Finance Manager,AppOps")]
        [RequireHttps(Order = 1)]
        public ActionResult _AppSiteSearch(int? filterType, int? accountId)
        {
            var model = LoadData(filterType, null, null, accountId);
            return View(new GridModel
            {
                Data = model.Items,
                Total = Convert.ToInt32(model.Items.Count())
            });
        }

        public PartialViewResult IndexPartial()
        {
            return PartialView("IndexPartial", new AppSiteListResultDto() { Items = new List<AppSiteListDto>(), TotalCount = 0 });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _IndexPartial(int? filterType, int? appId)
        {
            //if website then return all app/sites
            if (filterType != null)
            {
                if (filterType == 3)
                {
                    filterType = null;
                }

                int page = Convert.ToInt32(Request.Form["page"]);
                int size = Convert.ToInt32(Request.Form["size"]);


                if (appId.HasValue)
                {
                    AppSiteCriteriaModel appCriteria = new AppSiteCriteriaModel()
                    {
                        FilterType = filterType,
                        Page = page,
                        Size = size,
                        AppId = appId
                    };

                    var result = getQueryResult(appCriteria);
                    result.Items = result.Items;


                    return View(new GridModel
                    {
                        Data = result.Items,
                        Total = Convert.ToInt32(result.TotalCount)
                    });
                }
                else
                {
                    return View(new GridModel
                    {
                        Data = new List<AppSiteListDto>(),
                        Total = 0
                    });
                }
            }
            else
            {
                return View(new GridModel
                {
                    Data = new List<AppSiteListDto>(),
                    Total = 0
                });
            }
        }

        public PartialViewResult IndexDropDown(int appID)
        {
            AppSiteCriteriaModel appCriteria = new AppSiteCriteriaModel()
            {
                Size = int.MaxValue
            };

            var result = getQueryResult(appCriteria);

            var apps = result.Items.Select(appsite => new SelectListItem { Value = appsite.Id + "#" + appsite.TypeId, Text = appsite.Name, Selected = appsite.Id == appID }).ToList();

            return PartialView("IndexDropDown", apps);
        }

        public ActionResult AppSites(bool? IgnoreIsPrimaryUser)
        {
            GetAppSitesTypesList();
            var criteria = new AllAppSiteCriteria
            {
                Size = Config.PageSize,
                Page = 1
            };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
            }
            if (IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
            {


                criteria.UserId = null;
            }

            var result = _appSiteService.GetAllActive(criteria);
            ViewData["total"] = result.TotalCount;
            return PartialView(result);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AppSites(string appName, string accountName, int? TypeId, bool? IgnoreIsPrimaryUser)
        {
            int page = Convert.ToInt32(Request.Form["page"]);
            int size = Convert.ToInt32(Request.Form["size"]);
            var AppName = appName;

            var criteria = new AllAppSiteCriteria
            {
                Size = size,
                Page = page,
                AccountName = accountName,
                Name = AppName.Trim(),
                TypeId = TypeId
            };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
            }
            if (IgnoreIsPrimaryUser.HasValue&& !IsPrimaryUser)
            {


                criteria.UserId = null;
            }


              
            var result = _appSiteService.GetAllActive(criteria);

            foreach (var item in result.Items)
            {
                item.AccountName = item.AccountName == null ? null : item.AccountName.Replace("'", "").Replace("\"", "");
            }
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Index(AppSiteCriteriaModel appCriteria)
        {
            //int page = Convert.ToInt32(Request.Form["page"]);
            //int size = Convert.ToInt32(Request.Form["size"]);

            //AppSietCriteria appCriteria = new AppSietCriteria()
            //{
            //    FilterType = filterType,
            //    Page = page,
            //    Size = size
            //};

            var result = getQueryResult(appCriteria);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Index(int[] checkedRecords, int? filterType)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            ViewData["checkedRecords"] = checkedRecords;
            if (checkedRecords.Any())
            {
                _appSiteService.Delete(checkedRecords);
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult ApppSiteBasicInformation(int appsiteId)
        {
            var basicInformation = _appSiteService.GetPrimaryUserBasicInformation(appsiteId);
            ViewData.Model = basicInformation;

            return PartialView();
        }

        private void GetAppSitesTypesList()
        {
            //load all types
            var types = _appSiteTypeService.GetAll();
            var typesDropDown = Utility.GetSelectList();
            typesDropDown.AddRange(types.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() }));

            ViewData["TypeId"] = typesDropDown;

        }
        #endregion

        #region Create/Update
        public ActionResult Create(int? id)
        {
            var model = InitCreate();
            if (id.HasValue)
            {
                model.AppSiteDto = _appSiteService.Get(Convert.ToInt32(id));

                #region BreadCrumb
                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =model.AppSiteDto.Name,// ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
                                                  Order = 2,
                                              },
                                                new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index", "appsite")
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                #endregion
                if (model.AppSiteDto.Type != null)
                {
                    model.AppSiteViewName = model.AppSiteDto.Type.ViewName;

                }
                model.KeywordViewModel.Keywords = model.AppSiteDto.Keywords;
                model.Tabs = GetTabs(0, id.Value.ToString());
                setThemeSelected(model.Themes, model.AppSiteDto);
                return View("Update", model);
            }
            else
            {
                #region BreadCrumb

                var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("NewAppSite","SiteMapLocalizations"),
                                                  Order = 2
                                              },
                                              new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index", "appsite")
                                              }
                                      };

                ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                #endregion
                return View(model);
            }
        }

        [DenyRole(Roles = "AccountManager")]

        public ActionResult Save(CreateViewModel createViewModel, int id)
        {


            string successfulMassage = "", errorMassge = "";

            var appSite = _appSiteService.Get(id);
            appSite.Type = createViewModel.AppSiteDto.Type;
            appSite.NewKeywords = createViewModel.AppSiteDto.NewKeywords;
            appSite.DeletedKeywords = createViewModel.AppSiteDto.DeletedKeywords;
            appSite.AdminComment = createViewModel.AppSiteDto.AdminComment;
            appSite.Keywords = createViewModel.AppSiteDto.Keywords;



            try
            {
                _appSiteService.Save(appSite);

                var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");


                successfulMassage = string.Format(savedSuccessfully, dispalyName);

                return Json(new { ErrorMassge = "", SuccessfulMassage = successfulMassage, status = true });

            }
            catch (BusinessException exception)
            {

                errorMassge = exception.Errors.Aggregate(string.Empty, (current, errorData) => current + errorData.Message);
                successfulMassage = string.Empty;

                return Json(new { ErrorMassge = errorMassge, SuccessfulMassage = "", status = false });


            }

        }


        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Create(CreateViewModel createViewModel, int? id)
        {



            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    //update
                    var appSite = createViewModel.AppSiteDto;

                    #region BreadCrumb

                    var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = appSite.Name,
                                                          //ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =
                                                              ResourcesUtilities.GetResource("AppSiteList",
                                                                                             "SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "appsite")
                                                      }
                                              };

                    ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                    #endregion

                    try
                    {
                        _appSiteService.Save(appSite);
                        AddSuccessfullyMsg();
                        MoveMessagesTempData();
                        return RedirectToAction("Create", new { id = id.Value });
                    }
                    catch (BusinessException exception)
                    {
                        foreach (var errorData in exception.Errors)
                        {
                            AddMessages(errorData.Message, MessagesType.Error);
                        }

                        #region BreadCrumb

                        breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = appSite.Name,
                                                          //ResourcesUtilities.GetResource("NewAppSite","SiteMapLocalizations"),
                                                          Order = 2
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =
                                                              ResourcesUtilities.GetResource("AppSiteList",
                                                                                             "SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "appsite")
                                                      }
                                              };

                        ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                        #endregion

                        var model = InitCreate();
                        model.AppSiteDto = appSite;
                        if (string.IsNullOrWhiteSpace(model.AppSiteDto.Type.ViewName))
                        {
                            model.AppSiteViewName = model.AppSiteDto.Type.ViewName;
                        }
                        return View(model);
                    }
                    //var item = _appSiteService.Get(Convert.ToInt32(id));
                    //var model = InitCreate();
                    //model.AppSiteDto = appSite;
                    //if (!string.IsNullOrWhiteSpace(model.AppSiteDto.Type.ViewName))
                    //{
                    //    model.AppSiteViewName = model.AppSiteDto.Type.ViewName;
                    //}
                    //model.KeywordViewModel.Keywords = item.Keywords;
                    //model.Tabs = GetTabs(0, id.Value.ToString());
                    //setThemeSelected(model.Themes, model.AppSiteDto);
                    //return View("Update", model);
                }
                else
                {
                    //new
                    var appSite = createViewModel.AppSiteDto;
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var saveResult = _appSiteService.Save(appSite);
                            string PublisherId = saveResult.PublisherId;
                            int newid = saveResult.Id;
                            return RedirectToAction("ThankYou", "Misc",
                                                    new
                                                    {
                                                        resourceKeyName = "PublisherIdMsg",
                                                        pid = PublisherId,
                                                        url = Url.Action("Index"),
                                                        title = "AppSite",
                                                        view = "AppSiteLinks",
                                                        Id = newid
                                                    });
                        }
                        catch (BusinessException exception)
                        {
                            foreach (var errorData in exception.Errors)
                            {
                                AddMessages(errorData.Message, MessagesType.Error);
                            }
                        }
                    }

                    #region BreadCrumb

                    var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =
                                                              ResourcesUtilities.GetResource("NewAppSite",
                                                                                             "SiteMapLocalizations"),
                                                          Order = 2
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =
                                                              ResourcesUtilities.GetResource("AppSiteList",
                                                                                             "SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "appsite")
                                                      }
                                              };

                    ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                    #endregion

                    var model = InitCreate();
                    model.AppSiteDto = appSite;
                    if (string.IsNullOrWhiteSpace(model.AppSiteDto.Type.ViewName))
                    {
                        model.AppSiteViewName = model.AppSiteDto.Type.ViewName;
                    }
                    return View(model);
                }
                //}
            }
            return null;
        }

        public PartialViewResult AppSiteView(string Id)
        {
            var model = InitCreate();

            return PartialView(Id, model);
        }



        #endregion
        #region Settings
        public ActionResult Settings(int Id)
        {
            var appsiteSettings = _appSiteService.GetSettings(Id);
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteSettings","SiteMapLocalizations"),
                                                  Order = 3
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =appsiteSettings.AppSiteName,//ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url = Url.Action("create", "appsite", new {Id = Id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index", "appsite")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            var appSiteSettingUpdate = appsiteSettings;
            ViewData["Tabs"] = GetTabs(2, Id.ToString());

            return View("AppSiteSettings", appSiteSettingUpdate);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Settings(SettingsDto settingsDto)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    _appSiteService.SaveSettings(settingsDto);
                    AddSuccessfullyMsg();
                    MoveMessagesTempData();
                    return RedirectToAction("Settings", new { id = settingsDto.AppSiteId });
                }
                catch (BusinessException exception)
                {
                    foreach (var errorData in exception.Errors)
                    {
                        AddMessages(errorData.Message, MessagesType.Error);
                    }
                    #region BreadCrumb

                    var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteSettings","SiteMapLocalizations"),
                                                  Order = 3
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =settingsDto.AppSiteName,//ResourcesUtilities.GetResource("AppSite", "SiteMapLocalizations"),
                                                  Order = 2,
                                                  Url = Url.Action("create", "appsite", new {Id = settingsDto.AppSiteId})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index", "appsite")
                                              }
                                      };

                    ViewData["BreadCrumbLinks"] = breadCrumbLinks;

                    #endregion
                    var appSiteSettingUpdate = settingsDto;
                    ViewData["Tabs"] = GetTabs(2, settingsDto.AppSiteId.ToString());

                    return View("AppSiteSettings", appSiteSettingUpdate);
                }
            }
            /*var appSiteSettingUpdate = new AppSiteSettingUpdate()
                                           {
                                               SettingsDto = settingsDto,
                                               Tabs = GetTabs(2, settingsDto.AppSiteId.ToString())
                                           };
            return View("AppSiteSettings", appSiteSettingUpdate);*/
        }
        #endregion
        #region Filters

        public ActionResult Filters(int Id, string type, string Message, string Kind)
        {
            #region BreadCrumb

            var item = _appSiteService.Get(Id);
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("AppSiteFilter", "SiteMapLocalizations"),
                                                  Order = 3
                                              },
                                          new BreadCrumbModel()
                                              {
                                                  Text =item.Name,
                                                  Order = 2,
                                                  Url = Url.Action("create", "appsite", new {Id = Id})
                                              },
                                         new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
                                                  Order = 1,
                                                  Url = Url.Action("Index", "appsite")
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            if (string.IsNullOrEmpty(type))
            {
                type = "1";
            }

            ViewData["type"] = type;
            ViewData["message"] = Message;
            ViewData["kind"] = Kind;
            ViewData["Tabs"] = GetTabs(1, Id.ToString());
            ViewData["AppSiteId"] = Id;
            return View();
        }

        [HttpPost]
        [GridAction]
        public ActionResult Filters(int Id, string type, FormCollection collection)
        {
            //#region BreadCrumb
            //var item = _appSiteService.Get(Id);
            //var breadCrumbLinks = new List<BreadCrumbModel>
            //                          {
            //                              new BreadCrumbModel()
            //                                  {
            //                                      Text = ResourcesUtilities.GetResource("AppSiteFilter", "SiteMapLocalizations"),
            //                                      Order = 2
            //                                  },
            //                              new BreadCrumbModel()
            //                                  {
            //                                      Text =item.Name,
            //                                      Order = 2,
            //                                      Url = Url.Action("create", "appsite", new {Id = Id})
            //                                  },
            //                             new BreadCrumbModel()
            //                                  {
            //                                      Text =ResourcesUtilities.GetResource("AppSiteList","SiteMapLocalizations"),
            //                                      Order = 1,
            //                                      Url = Url.Action("Index", "appsite")
            //                                  }
            //                          };

            //ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            //#endregion

            var result = new JsonResult { Data = new GridModel() };
            return result;
        }

        public ActionResult TextFilters(string appsiteId, string Message, string Kind)
        {

            var filters = _appSiteService.GetAppSiteTextFilters(int.Parse(appsiteId));


            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;

            return PartialView();

        }
        public ActionResult GetTextFilter(string appsiteId, int filterID)
        {

            var filters = _appSiteService.GetAppSiteTextFilters(int.Parse(appsiteId));


            var filter = filters.Where(x => x.TextFilterId == filterID).FirstOrDefault();
            if (filter != null)
            {
                return Json(new { text = filter.Text });
            }

            return Json(string.Empty);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TextFiltersActions(string appsiteId, string actionName, TextFilterDto textFilterDto)
        {
            bool result = true;

            try
            {
                switch (actionName)
                {
                    case "delete":
                        _appSiteService.DeleteAppsiteFilter(textFilterDto.TextFilterId);
                        break;
                    case "update":

                        result = _appSiteService.UpdateAppSiteTextFilter(textFilterDto, int.Parse(appsiteId));

                        break;
                    case "insert":
                        result = _appSiteService.AddAppSiteTextFilter(int.Parse(appsiteId), textFilterDto);
                        break;
                    default:
                        break;
                }
            }
            catch (AccountNotValidException)
            {

            }

            if (result)
            {

                var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");


                var successfulMassage = string.Format(savedSuccessfully, dispalyName);

                return Json(new { Message = successfulMassage, Kind = "Successful" });
            }
            else
            {

                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateTextFilter", "Errors"), Kind = "Fail" });
            }
        }

        public ActionResult UrlFilters(string appsiteId, string Message, string Kind)
        {
            var filters = _appSiteService.GetAppSiteUrlFilters(int.Parse(appsiteId));
            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;
            return PartialView();

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult UrlFiltersActions(string appsiteId, string actionName, UrlFilterDto urlFilterDto = null)
        {
            var result = true;
            if (urlFilterDto != null)
            {
                try
                {
                    switch (actionName)
                    {
                        case "delete":
                            _appSiteService.DeleteAppsiteFilter(urlFilterDto.UrlFilterId);
                            break;
                        case "update":
                            if (ModelState.IsValid)
                            {
                                result = _appSiteService.UpdateAppSiteUrlFilter(urlFilterDto, int.Parse(appsiteId));
                            }
                            break;
                        case "insert":
                            result = _appSiteService.AddAppSiteUrlFilter(int.Parse(appsiteId), urlFilterDto);
                            break;
                        default:
                            break;
                    }
                }
                catch (AccountNotValidException)
                {

                }
            }

            if (result)
            {

                var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");


                var successfulMassage = string.Format(savedSuccessfully, dispalyName);

                return Json(new { Message = successfulMassage, Kind = "Successful" });
            }
            else
            {

                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateUrlFilters", "Errors"), Kind = "Fail" });

            }
        }

        public ActionResult LanguageFilters(string appsiteId, string Message, string Kind)
        {
            var filters = _appSiteService.GetAppSiteLanguageFilters(int.Parse(appsiteId));
            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;
            return PartialView();

        }

        public ActionResult KeywordFilters()
        {
            var Keywordauto = new Noqoush.AdFalcon.Web.Controllers.Model.AutoComplete()
            {
                Id = "Kewords_Name",
                Name = "Kewords.Name",
                ActionName = "GetKeywords",
                ControllerName = "Keyword",
                LabelExpression = "item.Name",
                ValueExpression = "item.Id",
                IsAjax = true,
                ChangeCallBack = "KewordChanged"
            };

            //get the Keyword Tag Cloud
            var keywordservice = Noqoush.Framework.IoC.Instance.Resolve<IKeywordService>();

            var keywords = keywordservice.GetTop(null);
            var keywordTags = keywords.Select(keywordDto => new TagCloud() { Id = keywordDto.ID, DispalValue = keywordDto.Name.ToString(), Rank = keywordDto.Rank }).ToList();

            var keywordViewModel = new KeywordViewModel() { Prefix = "", KewordAuto = Keywordauto, KeywordTags = keywordTags, Keywords = new List<KeywordDto>(), AllowInsert = true };

            return PartialView(keywordViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult LanguagefiltersActions(string appsiteId, string actionName, LanguageFilterDto languageFilterDto)
        {
            bool result = true;

            switch (actionName)
            {
                case "delete":
                    _appSiteService.DeleteAppsiteFilter(languageFilterDto.languageFilterId);
                    break;
                case "update":
                    if (ModelState.IsValid)
                    {
                        //languageFilterDto.LanguageId = int.Parse(languageFilterDto.LanguageName);
                        result = _appSiteService.UpdateAppSiteLanguageFilter(languageFilterDto, int.Parse(appsiteId));
                    }
                    break;
                case "insert":
                    //languageFilterDto.LanguageId = int.Parse(languageFilterDto.LanguageName);
                    result = _appSiteService.AddAppSiteLanguageFilter(int.Parse(appsiteId), languageFilterDto);
                    break;
                default:
                    break;
            }

            if (result)
            {
                List<LanguageFilterDto> filters = _appSiteService.GetAppSiteLanguageFilters(int.Parse(appsiteId));
                var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");


                var successfulMassage = string.Format(savedSuccessfully, dispalyName);

                return Json(new { Message = successfulMassage, Kind = "Successful" });
            }
            else
            {

                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateLanguageFilters", "Errors"), Kind = "Fail" });
            }

        }
        #endregion

    }
}
