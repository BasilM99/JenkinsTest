using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using System.Web.UI;
using ArabyAds.Framework;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [RequireHttps(Order = 1)]

    public class ManufacturerController : ControllerBase
    {
        private IManufacturerService manufacturerService;
        public ManufacturerController( )
        {
            manufacturerService = IoC.Instance.Resolve<IManufacturerService>() ;
        }
        [IgnoreAntiforgeryToken]
        [OutputCache(Duration = 21600)]
        public JsonResult GetTreeData()
        {
            ApplicationContext.Instance.Logger.Info("GetTreeData-manu");
            var items = manufacturerService.GetAllManufacturerTree();
            var tree = TreeModel.GetTreeNodes(items);
        
            var result = new JsonResult (  tree);
            return result;
        }
        public ActionResult Tree()
        {
            return View();
        }
    }
}


