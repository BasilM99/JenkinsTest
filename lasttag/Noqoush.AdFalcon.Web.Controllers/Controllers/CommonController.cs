using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using System.Web.UI;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class CommonController : AuthorizedControllerBase
    {

        private ILanguageService _languageService;
        private IUserService _userService;

        public CommonController(ILanguageService _languageService, IUserService userService)
        {
            this._languageService = _languageService;
            _userService = userService;
        }
        [OutputCache(Duration = 21600)]
        public JsonResult GetLanguages()
        {
            var items = _languageService.GetAll();

            var list = items.Select(x => new { id = x.ID, name = x.Name.Value, additionalValue = x.Code }).ToList();
            var result = new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [OutputCache(Duration = 21600)]

        public JsonResult GetSSPPartners()
        {
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria { });
            var list = UsersListResult.Items.Select(x => new { id = x.Id, name = x.AccountName, additionalValue = x.AppSiteId }).ToList();

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [OutputCache(Duration = 21600)]

        public JsonResult GetSSPPartnersAsAppsites()
        {
            UsersListResultDto UsersListResult = _userService.GetSSPUsers(new AllAppSiteCriteria { });
            var list = UsersListResult.Items.Select(x => new { id = x.AppSiteId, name = x.AccountName, additionalValue = x.Id }).ToList();

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


    }
}


