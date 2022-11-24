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
    public class LanguageFilters : ViewComponent
    {

        private static IAppSiteService _appSiteService;
        private static IAppSiteTypeService _appSiteTypeService;
        private static IRefreshIntervalService _refreshIntervalService;
        protected static IAccountService _accountService;
        protected static JsonSerializerOptions _jsonOptions;

        private static IThemeService _themeService;


        static LanguageFilters()
        {
            //_jsonOptions = jsonOptions.Value.JsonSerializerOptions;
            // this.DispalyResourceName = "AppSiteDispalyName";
            _appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
            _themeService = IoC.Instance.Resolve<IThemeService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

        }
        public LanguageFilters()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(string appsiteId, string Message, string Kind)
        {
            var filters = _appSiteService.GetAppSiteLanguageFilters(new ValueMessageWrapper<int> { Value = int.Parse(appsiteId) });
            ViewData.Model = filters;
            ViewBag.Message = Message;
            ViewBag.kind = Kind;
            return View("LanguageFilters", filters);
        }
    }
}
