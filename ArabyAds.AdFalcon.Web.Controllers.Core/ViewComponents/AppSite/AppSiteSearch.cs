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
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.AppSite
{
    public class AppSiteSearch : ViewComponent
    {
        private static IAppSiteService _appSiteService;
        private static IAppSiteTypeService _appSiteTypeService;
        private static IRefreshIntervalService _refreshIntervalService;
        protected static IAccountService _accountService;
        protected static JsonSerializerOptions _jsonOptions;

        private static IThemeService _themeService;


        static AppSiteSearch()
        {
            //_jsonOptions = jsonOptions.Value.JsonSerializerOptions;
            // this.DispalyResourceName = "AppSiteDispalyName";
            _appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
            _themeService = IoC.Instance.Resolve<IThemeService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

        }
        public AppSiteSearch()
        {

        }


        public async Task<IViewComponentResult> InvokeAsync(int? filterType, int? accountId)
        {
            var isAllFilterSelected = filterType.HasValue == false;



            ViewData["total"] = 0;
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
                          new Web.Controllers.Model.Action()
                        {
                            Code = "0",
                            DisplayText =ResourcesUtilities.GetResource("tooltpi","Audittrial"),
                            ClassName = "grid-tool-tip-trail",
                            ActionName = "RedirectToAuditTrial",

                        }
                               };
            var model= new ListViewModel()
            {
                //Items = items,
                TopActions = actions,
                BelowAction = actions,
                FilterBar = FilterBar,
                ToolTips = toolTips
            };
            return View("AppSiteSearch", model);
        }
    }
}
