using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [RequireHttps(Order = 1)]
  
    public class CountryController : ControllerBase
    {
        private ICountryService countryService;
        private ILocationService locationService;
        public CountryController()
        {
            this.countryService = IoC.Instance.Resolve<ICountryService>() ;
            this.locationService =IoC.Instance.Resolve<ILocationService>() ;
        }
        //[OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "allowRegion" })]
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

            var result = new JsonResult (  tree);
            return result;
        }

        public ActionResult GetCountryById(string ids)
        {
            IList<LocationDto> results = new List<LocationDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();

            foreach (var id in TagIds)
            {
                var item = locationService.GetCountryById(new ArabyAds.Framework.ValueMessageWrapper<int> { Value = id });
                results.Add(item);
            }
            return Json(results);
        }
        public ActionResult Tree()
        {
            return View();
        }

        [OutputCache(Duration = 21600)]
        //[Produces("application/json")]
        public ActionResult GetCountries()
        {
            SelectListItem optionalItem = new SelectListItem();
            optionalItem.Value = "";
            optionalItem.Text = ResourcesUtilities.GetResource("ByCountry", "Chart"); ;

            ArabyAds.AdFalcon.Services.Interfaces.Services.ICountryService countryService = this.countryService;
            List<SelectListItem> countriesList = new List<SelectListItem>();
            //countriesList.Add(optionalItem);

            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.CountryDto> countriesDtos = countryService.GetAll().OrderBy(p => p.Name.Value).ToList();

            foreach (var item in countriesDtos)
            {
                var selectItem = new SelectListItem();
                selectItem.Value = item.ID.ToString();
                selectItem.Text = item.Name.ToString();
                countriesList.Add(selectItem);
            }
            return Json(countriesList);
        }
    }
}


