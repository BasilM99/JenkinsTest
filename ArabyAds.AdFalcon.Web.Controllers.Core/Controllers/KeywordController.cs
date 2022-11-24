using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.Framework;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [RequireHttps(Order = 1)]

    public class KeywordController : ControllerBase
    {
        private IKeywordService _KeywordService;
        public KeywordController( )
        {
            _KeywordService = IoC.Instance.Resolve<IKeywordService>();
        }
        public ActionResult Index()
        {
            var service = IoC.Instance.Resolve<IKeywordService>();

            return View();
        }
        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "q" })]
        public ActionResult GetKeywords(string q)
        {
            //_KeywordService.GetAll();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var criteria = new KeywordCriteria() { Value = q };
                var keywords = _KeywordService.GetByQuery(criteria);
                return Json(keywords);
            }
            else
            {
                var keywords = _KeywordService.GetAll();
                return Json(keywords);
            }
        }
    }
}
