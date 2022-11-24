using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.Framework;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class KeywordController : AuthorizedControllerBase
    {
        private IKeywordService _KeywordService;
        public KeywordController(IKeywordService KeywordService)
        {
            _KeywordService = KeywordService;
        }
        public ActionResult Index()
        {
            var service = IoC.Instance.Resolve<IKeywordService>();

            return View();
        }
        [OutputCache(Duration = 21600, VaryByParam = "q")]
        public ActionResult GetKeywords(string q)
        {
            //_KeywordService.GetAll();
            var criteria = new KeywordCriteria() { Value = q };
            var keywords = _KeywordService.GetByQuery(criteria);
            return Json(keywords, JsonRequestBehavior.AllowGet);
        }
    }
}
