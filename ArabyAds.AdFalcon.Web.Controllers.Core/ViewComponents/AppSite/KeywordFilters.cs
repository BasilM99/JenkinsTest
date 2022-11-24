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
    public class KeywordFilters : ViewComponent
    {
        public KeywordFilters()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            return View("KeywordFilters", keywordViewModel);
        }
    }
}
