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
    public class IndexDropDown : ViewComponent
    {

        private static IAppSiteService _appSiteService;
        private static IAppSiteTypeService _appSiteTypeService;
        private static IRefreshIntervalService _refreshIntervalService;
        protected static IAccountService _accountService;
        protected static JsonSerializerOptions _jsonOptions;

        private static IThemeService _themeService;


        static IndexDropDown()
        {
            //_jsonOptions = jsonOptions.Value.JsonSerializerOptions;
            // this.DispalyResourceName = "AppSiteDispalyName";
            _appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
            _themeService = IoC.Instance.Resolve<IThemeService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

        }

        public IndexDropDown()
        {

        }


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
        public async Task<IViewComponentResult> InvokeAsync(int appID)
        {
            AppSiteCriteriaModel appCriteria = new AppSiteCriteriaModel()
            {
                Size = int.MaxValue
            };

            var result = getQueryResult(appCriteria);

            var apps = result.Items.Select(appsite => new SelectListItem { Value = appsite.Id + "#" + appsite.TypeId, Text = appsite.Name, Selected = appsite.Id == appID }).ToList();

            return View("IndexDropDown", apps);
        }
    }
}
