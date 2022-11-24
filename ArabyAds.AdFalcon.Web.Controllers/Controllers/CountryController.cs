using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class CountryController : AuthorizedControllerBase
    {
        private ICountryService countryService;
        private ILocationService locationService;
        public CountryController(ICountryService countryService, ILocationService locationService)
        {
            this.countryService = countryService;
            this.locationService = locationService;
        }
        [OutputCache(Duration = 21600, VaryByParam = "allowRegion")]
        public JsonResult GetTreeData(bool? allowRegion)
        {
            var items = locationService.GetTree();
            var tree = TreeModel.GetTreeNodes(items);

            // This block of code to remove regions in some cases like reports for now
            // This code should be revamped
            if (allowRegion.HasValue && !allowRegion.Value)
            {
                foreach (var countries in tree.Select(p=>p.children))
                {
                    foreach (var item in countries)
                    {
                        if (item.children != null)
                        {
                            item.children = null;
                        }
                    }
                }
            }

            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public ActionResult GetCountryById(string ids)
        {
            IList<LocationDto> results = new List<LocationDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results, JsonRequestBehavior.AllowGet);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();

            foreach (var id in TagIds)
            {
                var item = locationService.GetCountryById(id);
                results.Add(item);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Tree()
        {
            return View();
        }
      
    }
}


