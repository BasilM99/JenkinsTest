using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.Framework;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [RequireHttps(Order = 1)]


    public class PlatformController : ControllerBase
    {
        private IPlatformService platformService;
        public PlatformController( )
        {
            platformService = IoC.Instance.Resolve<IPlatformService>();
        }
        [OutputCache(Duration = 21600)]
        public JsonResult GetTreeData()
        {
            var items = platformService.GetAllPlatformTree();
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult (  tree);
            return result;
        }

        public JsonResult GetData()
        {
            var items = platformService.GetAllPlatformTree();
            var tree = TreeModel.GetTreeNodes(items);

            var list = tree.Select(x => new { id = x.attributes.id, name = x.data, additionalValue = x.attributes.isRoot }).ToList();
            var result = new JsonResult (  list);
            return result;
        }

        public ActionResult Tree()
        {
            return View();
        }
    }
}


