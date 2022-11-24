using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using System.Web.UI;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.Framework;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.Framework;
using ArabyAds.Framework.Resources;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class CommonController : AuthorizedControllerBase
    {

        private ILanguageService _languageService;
        private IUserService _userService;
        private IResourceManager resourceManager;
        public CommonController()
        {
            this._languageService = IoC.Instance.Resolve<ILanguageService>();
            _userService = IoC.Instance.Resolve<IUserService>();
            this.resourceManager = IoC.Instance.Resolve<IResourceManager>();

        }
        [OutputCache(Duration = 21600)]
        public JsonResult GetLanguages()
        {
            var items = _languageService.GetAll();

            var list = items.Select(x => new { id = x.ID, name = x.Name.Value, additionalValue = x.Code }).ToList();
            var result = new JsonResult (  list);
            return result;
        }

        [OutputCache(Duration = 21600)]

        public JsonResult GetSSPPartners()
        {
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria { });
            var list = UsersListResult.Items.Select(x => new { id = x.Id, name = x.AccountName, additionalValue = x.AppSiteId }).ToList();

            return new JsonResult (  list);
        }


        [OutputCache(Duration = 21600)]

        public JsonResult GetSSPPartnersAsAppsites()
        {
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria { });
            var list = UsersListResult.Items.Select(x => new { id = x.AppSiteId, name = x.AccountName, additionalValue = x.Id }).ToList();

            return new JsonResult (  list);
        }
       
    
    }
}


