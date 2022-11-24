using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class PlatformController : AuthorizedControllerBase
    {
        private IPlatformService platformService;
        public PlatformController(IPlatformService _platformService)
        {
            platformService = _platformService;
        }
        [OutputCache(Duration = 21600, VaryByParam = "none")]
        public JsonResult GetTreeData()
        {
            var items = platformService.GetAllPlatformTree();
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public JsonResult GetData()
        {
            var items = platformService.GetAllPlatformTree();
            var tree = TreeModel.GetTreeNodes(items);

            var list = tree.Select(x => new { id = x.attributes.id, name = x.data, additionalValue = x.attributes.isRoot }).ToList();
            var result = new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public ActionResult Tree()
        {
            return View();
        }
    }
}


