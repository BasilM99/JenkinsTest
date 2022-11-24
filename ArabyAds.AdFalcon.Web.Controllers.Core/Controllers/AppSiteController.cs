using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using Action = ArabyAds.AdFalcon.Web.Controllers.Model.Action;
using ArabyAds.Framework.UserInfo;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Business.Domain.Exceptions;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite.Save;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] {  AccountRole.DataProvider })]
    public class AppSiteController : AuthorizedControllerBase
    {

        private IAppSiteService _appSiteService;
        private IAppSiteTypeService _appSiteTypeService;
        private IRefreshIntervalService _refreshIntervalService;
        protected IAccountService _accountService;
        protected readonly JsonSerializerOptions _jsonOptions;

        private IThemeService _themeService;
        public AppSiteController(IOptions<JsonOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
            this.DispalyResourceName = "AppSiteDispalyName";
            _appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
            _themeService = IoC.Instance.Resolve<IThemeService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();
        }


        #region Helpers


        private AppSiteListResultDto getQueryResult(AppSiteCriteriaModel appCriteria)
        {

            string name = (!Request.HasFormContentType || string.IsNullOrWhiteSpace(Request.Form["page"])) ? null : Request.Form["Name"].ToString();
            var cratiria = new ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase
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
            var actions = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
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
            var FilterBar = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
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
            var toolTips = new List<ArabyAds.AdFalcon.Web.Controllers.Model.Action>
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

        public void setThemeSelected(IEnumerable<SelectListItem> themes, AppSiteDto appSiteDto)
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
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.ThemeDto> ThemesDtos = _themeService.GetAll().ToList();
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
            var keywordauto = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
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
            var keywordservice = ArabyAds.Framework.IoC.Instance.Resolve<IKeywordService>();
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
                             
                           };

            if (Config.IsAdministrationApp &&( Config.IsAdOps || Config.IsAppOps || Config.IsAdmin))
            {
                tabs.Add(new Tab
                {
                    IsSelected = false,
                    IsExternal=false,
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
                    IsExternal = false,
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
            //string originalPath = new Uri(HttpContextHelper.Current.Request.UrlReferrer.AbsoluteUri).OriginalString;
            
                int objectRootTypeId = _accountService.GetObjectRootTypeId("ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite").Value;

            // return RedirectToAction("AuditTrialSessions", "User", new { objectRootId = id, objectRootTypeId = objectRootTypeId, returnUrl = originalPath });
            var url = Url.Action("AuditTrialSessions", "User",
                                                                         new { objectRootId = id, objectRootTypeId = objectRootTypeId }, "https", JsonConfigurationManager.AppSettings["AdFalconWebReact"].ToString());
            return Redirect(url);


        }


        public ActionResult Index(int? filterType)
        {
            #region BreadCrumb

            var falg = false;
            if (filterType.HasValue)
            {
                var item = _appSiteTypeService.Get(new ValueMessageWrapper<int> { Value = filterType.Value });
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
            return Json(new GridModel
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
                int size = Convert.ToInt32(Request.Form["pageSize"]);


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


                    return Json(new GridModel
                    {
                        Data = result.Items,
                        Total = Convert.ToInt32(result.TotalCount)
                    });
                }
                else
                {
                    return Json(new GridModel
                    {
                        Data = new List<AppSiteListDto>(),
                        Total = 0
                    });
                }
            }
            else
            {
                return Json(new GridModel
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
            if (size == 0)
                size = 10;
            var AppName = appName;
            if (string.IsNullOrEmpty(accountName))
            {
                accountName = string.Empty;
            }
            if (string.IsNullOrEmpty(AppName))
            {
                AppName = string.Empty;
            }
            
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
            if (IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
            {


                criteria.UserId = null;
            }



            var result = _appSiteService.GetAllActive(criteria);

            foreach (var item in result.Items)
            {
                item.AccountName = item.AccountName == null ? null : item.AccountName.Replace("'", "").Replace("\"", "");
            }
            return Json(new GridModel
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
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [AcceptVerbs("Post")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Index(int[] checkedRecords, int? filterType)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            ViewData["checkedRecords"] = checkedRecords;
            if (checkedRecords.Any())
            {
                _appSiteService.Delete(checkedRecords);
            }
            //return RedirectToAction("Index");

            AddSuccessfullyMsgMs(string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("AppSite", "AppChart")));
            return Json(true, ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.success);

        }

        [AuthorizeRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult ApppSiteBasicInformation(int appsiteId)
        {
            var basicInformation = _appSiteService.GetPrimaryUserBasicInformation(new ValueMessageWrapper<int> { Value = appsiteId });
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

        [AcceptVerbs("Post")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveApp([FromBody]CreateViewModel createViewModel)
        {



            if (ModelState.IsValid)
            {
                if (createViewModel.id>0)
                {
                    //update
                    var appSite = createViewModel.AppSiteDto;
                    try
                    {
                        _appSiteService.Save(appSite);
                        _appSiteService.SaveSettings(createViewModel.SettingsDto);
                        AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("AppSite", "AppChart"));
                        return Json(ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.success);
                    }
                    catch (BusinessException exception)
                    {
                        AddErrorMsgs(exception);
                        return Json(ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.businessException);
                    }
               
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
                            string  url = Url.Action("ThankYou", "Misc",
                                                    new
                                                    {
                                                        resourceKeyName = "PublisherIdMsg",
                                                        pid = PublisherId,
                                                        url = Url.Action("Index"),
                                                        title = "AppSite",
                                                        view = "AppSiteLinks",
                                                        Id = newid
                                                    });


                            AddSuccessfullyMsgMs(string.Format(ResourcesUtilities.GetResource("PublisherIdMsg", "ThanksMessages"), PublisherId));
                            return Json(newid, ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.success);
                        }
                        catch (BusinessException exception)
                        {
                            AddErrorMsgs(exception);
                            return Json(ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.businessException);

                        }
                    }


                    var model = InitCreate();
                    model.AppSiteDto = appSite;
                    if (string.IsNullOrWhiteSpace(model.AppSiteDto.Type.ViewName))
                    {
                        model.AppSiteViewName = model.AppSiteDto.Type.ViewName;
                    }

                    AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("AppSite", "AppChart"));
                    return Json(model,ResourcesUtilities.GetResource("AppSite", "AppChart"), ResponseStatus.success);
                    //return Json(model);
                }
                //}
            }
            return null;
        }

        public ActionResult Create(int? id)
        {
            var model = new CreateViewModel();
          
           
                return View(model);
        }


        public CreateViewModel InitJsonCreate()
        {
            //Load Themes List
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", "Global") };
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.ThemeDto> ThemesDtos = _themeService.GetAll().ToList();
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
           
          

            var types = _appSiteTypeService.GetAll();

            return new CreateViewModel()
            {
                AppSiteViewName = types.First().ViewName,
                AppSiteTypes = types,
                Themes = themes,
                SettingsDto=new SettingsDto()
            };
        }
        public ActionResult GetAppInfo(int? id)
        {
            var model = InitJsonCreate();
            if (id.HasValue)
            {
                model.AppSiteDto = _appSiteService.Get(new ValueMessageWrapper<int> { Value = Convert.ToInt32(id) });


                if (model.AppSiteDto.Type != null)
                {
                    model.AppSiteViewName = model.AppSiteDto.Type.ViewName;

                }
                model.Keywords = model.AppSiteDto.Keywords;
                model.Tabs = GetTabs(0, id.Value.ToString());
                var appsiteSettings = _appSiteService.GetSettings(new ValueMessageWrapper<int> { Value = id.Value });
           
                var appSiteSettingUpdate = appsiteSettings;
                model.SettingsDto = appSiteSettingUpdate;
                setThemeSelected(model.Themes, model.AppSiteDto);
                return Json(model);
            }
            else
            {

                return Json(model);
            }
        }

        [DenyRole(Roles = "AccountManager")]
        public ActionResult Save(CreateViewModel createViewModel, int id)
        {


            string successfulMassage = "", errorMassge = "";

            var appSite = _appSiteService.Get(new ValueMessageWrapper<int> { Value = id });
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


        [AcceptVerbs("Post")]
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
            var appsiteSettings = _appSiteService.GetSettings(new ValueMessageWrapper<int> { Value = Id });
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
        [AcceptVerbs("Post")]
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

            var item = _appSiteService.Get(new ValueMessageWrapper<int> { Value = Id });
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

            var result = new JsonResult(new GridModel());
            return result;
        }

        public ActionResult TextFilters(string appsiteId, string Message, string Kind)
        {

            var filters = _appSiteService.GetAppSiteTextFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });


            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;

            return PartialView();

        }
        public ActionResult GetTextFilter(string appsiteId, int filterID)
        {

            var filters = _appSiteService.GetAppSiteTextFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });


            var filter = filters.Where(x => x.TextFilterId == filterID).FirstOrDefault();
            if (filter != null)
            {
                return Json(new { text = filter.Text });
            }

            return Json(string.Empty);

        }

        [AcceptVerbs("Post")]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult TextFiltersActions([FromBody] SaveAppSiteFilterModel modelsave)
        {
            bool result = true;
            string appsiteId = modelsave.appsiteId;
            string actionName = modelsave.actionName;
            TextFilterDto textFilterDto = modelsave.textFilterDto;
            try
            {
                switch (actionName)
                {
                    case "delete":
                        _appSiteService.DeleteAppsiteFilter(new ValueMessageWrapper<int> { Value = textFilterDto.TextFilterId });
                        break;
                    case "update":

                        result = _appSiteService.UpdateAppSiteTextFilter(new AppSiteTextFilterMessage { TextFilterDto = textFilterDto, AppSiteId = int.Parse(appsiteId) }).Value;

                        break;
                    case "insert":
                        result = _appSiteService.AddAppSiteTextFilter(new AppSiteTextFilterMessage { AppSiteId = int.Parse(appsiteId), TextFilterDto = textFilterDto }).Value;
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
                

                AddSuccessfullyMsg(formattedStrings: ResourcesUtilities.GetResource("AppSite", "ThanksTitle"));
                return Json(new { Message = successfulMassage, status = "success" });
            }
            else
            {
                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateTextFilter", "Errors"), status = "businessException" });
            }
        }

        public ActionResult UrlFilters(string appsiteId, string Message, string Kind)
        {
            var filters = _appSiteService.GetAppSiteUrlFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });
            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;
            return PartialView();

        }

        [HttpPost]

        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult UrlFiltersActions([FromBody] SaveAppSiteFilterModel modelsave)
        {

            string appsiteId = modelsave.appsiteId;
               string actionName = modelsave.actionName;
            UrlFilterDto urlFilterDto = modelsave.urlFilterDto;
            var result = true;
            if (urlFilterDto != null)
            {
                try
                {
                    switch (actionName)
                    {
                        case "delete":
                            _appSiteService.DeleteAppsiteFilter(new ValueMessageWrapper<int> { Value = urlFilterDto.UrlFilterId });
                            break;
                        case "update":
                            if (ModelState.IsValid)
                            {
                                result = _appSiteService.UpdateAppSiteUrlFilter(new AppSiteUrlFilterMessage { UrlFilterDto = urlFilterDto, AppSiteId = int.Parse(appsiteId) }).Value;
                            }
                            break;
                        case "insert":
                            result = _appSiteService.AddAppSiteUrlFilter(new AppSiteUrlFilterMessage { AppSiteId = int.Parse(appsiteId), UrlFilterDto = urlFilterDto }).Value;
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

                return Json(new { Message = successfulMassage, status = "success" });
            }
            else
            {

                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateUrlFilters", "Errors"), status = "businessException" });

            }
        }

        public ActionResult LanguageFilters(string appsiteId, string Message, string Kind)
        {
            var filters = _appSiteService.GetAppSiteLanguageFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });
            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;
            return PartialView();

        }

        public ActionResult KeywordFilters()
        {
            var Keywordauto = new ArabyAds.AdFalcon.Web.Controllers.Model.AutoComplete()
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
            var keywordservice = ArabyAds.Framework.IoC.Instance.Resolve<IKeywordService>();

            var keywords = keywordservice.GetTop(null);
            var keywordTags = keywords.Select(keywordDto => new TagCloud() { Id = keywordDto.ID, DispalValue = keywordDto.Name.ToString(), Rank = keywordDto.Rank }).ToList();

            var keywordViewModel = new KeywordViewModel() { Prefix = "", KewordAuto = Keywordauto, KeywordTags = keywordTags, Keywords = new List<KeywordDto>(), AllowInsert = true };

            return PartialView(keywordViewModel);
        }

        [AcceptVerbs("Post")]
        [GridAction]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult LanguagefiltersActions([FromBody] SaveAppSiteFilterModel modelsave)
        {
            string appsiteId = modelsave.appsiteId;
            string actionName = modelsave.actionName;
            LanguageFilterDto languageFilterDto = modelsave.languageFilterDto;
            bool result = true;

            switch (actionName)
            {
                case "delete":
                    _appSiteService.DeleteAppsiteFilter(new ValueMessageWrapper<int> { Value = languageFilterDto.languageFilterId });
                    break;
                case "update":
                    if (ModelState.IsValid)
                    {
                        //languageFilterDto.LanguageId = int.Parse(languageFilterDto.LanguageName);
                        result = _appSiteService.UpdateAppSiteLanguageFilter(new AppSiteLanguageFilterMessage { LanguageFilterDto = languageFilterDto, AppSiteId = int.Parse(appsiteId) }).Value;
                    }
                    break;
                case "insert":
                    //languageFilterDto.LanguageId = int.Parse(languageFilterDto.LanguageName);
                    result = _appSiteService.AddAppSiteLanguageFilter(new AppSiteLanguageFilterMessage { AppSiteId = int.Parse(appsiteId), LanguageFilterDto = languageFilterDto }).Value;
                    break;
                default:
                    break;
            }

            if (result)
            {
                List<LanguageFilterDto> filters = _appSiteService.GetAppSiteLanguageFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });
                var dispalyName = ResourcesUtilities.GetResource(DispalyResourceName, "Global");

                var savedSuccessfully = ResourcesUtilities.GetResource("savedSuccessfully", "Global");


                var successfulMassage = string.Format(savedSuccessfully, dispalyName);

                return Json(new { Message = successfulMassage, status = "success" });
            }
            else
            {

                return Json(new { Message = ResourcesUtilities.GetResource("DuplicateLanguageFilters", "Errors"), status = "businessException" });
            }

        }
        #endregion


        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AppSiteTextFilters(int appsiteId)
        {

            var filters = _appSiteService.GetAppSiteTextFilters(new ValueMessageWrapper<int> { Value = appsiteId });
            return Json(new GridModel
            {
                Data = filters,
                Total = filters.Count
            });
        }



        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AppSiteLanguageFilters(int appsiteId)
        {

            var filters = _appSiteService.GetAppSiteLanguageFilters(new ValueMessageWrapper<int> { Value = appsiteId });
            
            return Json(new GridModel
            {
                Data = filters,
                Total = filters.Count
            });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _AppSiteUrlFilters(int appsiteId)
        {

            var filters = _appSiteService.GetAppSiteUrlFilters(new ValueMessageWrapper<int> { Value =appsiteId});
            return Json(new GridModel
            {
                Data = filters,
                Total = filters.Count
            });
        }

        public ActionResult _GetTextFilterCondition()
        {
            ArabyAds.AdFalcon.Services.Interfaces.Services.Core.IMatchTypeService matchTypeService = ArabyAds.Framework.IoC.Instance.Resolve<ArabyAds.AdFalcon.Services.Interfaces.Services.Core.IMatchTypeService>();
            SelectList selecList = new SelectList(matchTypeService.GetAll(), "ID", "Name");
            return Json(selecList);
        }
        public ActionResult _GetAppSiteFilterLanguages()
        {
            ArabyAds.AdFalcon.Services.Interfaces.Services.ILanguageService languageService = ArabyAds.Framework.IoC.Instance.Resolve<ArabyAds.AdFalcon.Services.Interfaces.Services.ILanguageService>();

            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.LanguageDto> languagesDtos = languageService.GetAll().ToList();
            SelectList items = new SelectList(languagesDtos, "ID", "Name");
            return Json(items);
        }
    }
}
