using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using System.Web.UI;
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class ManufacturerController : AuthorizedControllerBase
    {
        private IManufacturerService manufacturerService;
        public ManufacturerController(IManufacturerService _manufacturerService)
        {
            manufacturerService = _manufacturerService;
        }
        [OutputCache(Duration = 21600,VaryByParam = "none")]
        public JsonResult GetTreeData()
        {
            var items = manufacturerService.GetAllManufacturerTree();
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        public ActionResult Tree()
        {
            return View();
        }
    }
}


