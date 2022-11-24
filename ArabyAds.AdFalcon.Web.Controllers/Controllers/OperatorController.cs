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
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class OperatorController : AuthorizedControllerBase
    {
        private const string _separator = "&";
        private const string _bidSeparator = ",";
        private IOperatorService _OperatorService;
        private ILocationService _LocationService;

        public OperatorController(IOperatorService operatorService,ILocationService locationService)
        {
            _OperatorService = operatorService;
            _LocationService = locationService;
        }
        [OutputCache(Duration = 21600, VaryByParam = "Geographies")]
        public JsonResult GetTreeData(string Geographies)
        {
            IEnumerable<TreeDto> items = null;
            if (string.IsNullOrWhiteSpace(Geographies) || (Geographies.Equals("null", StringComparison.OrdinalIgnoreCase)))
            {
                items = _OperatorService.GetAllCountryOperator();
            }
            else
            {

                var countryIds = GetCountryIds(Geographies);
                items = _OperatorService.GetAllOperatorByCountryIds(countryIds.ToArray());
            }
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        private List<int> GetCountryIds(string Geographies)
        {
            var locations = _LocationService.GetAll();
            var locationsTree = _LocationService.GetTree();

            string[] Ids = Geographies.Split(_bidSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var countryIds = new List<int>();

            foreach (var item in locations.Where(p => Ids.Contains(p.ID.ToString())))
            {
                if (item.Type == LocationType.Country)
                {
                    countryIds.Add(item.ID);
                }
                else
                {
                    if (item.Type != LocationType.Continent)
                    {
                        int? countryId = GetCountryId(locations, locationsTree, item);

                        if (countryId.HasValue)
                        {
                            countryIds.Add(countryId.Value);
                        }
                    }
                }
            }

            return countryIds;
        }

        private int? GetCountryId(IEnumerable<LocationDto> locations, IEnumerable<TreeDto> locationsTree, LocationDto item)
        {
            TreeDto parentItem = GetParentItem(locationsTree, item);

            if (parentItem != null)
            {
                var location = locations.Where(p => p.ID.ToString() == parentItem.Id).SingleOrDefault();
                if (location.Type == LocationType.Country)
                {
                    return location.ID;
                }
                else
                {
                    var parentLocation = locations.Where(p => p.ID == int.Parse(parentItem.Id)).SingleOrDefault();

                    return GetCountryId(locations, locationsTree, parentLocation);
                }
            }
            else
            {
                return null;
            }
        }

        private TreeDto GetParentItem(IEnumerable<TreeDto> locationsTree, LocationDto item)
        {
            var locationItem = locationsTree.Where(p => p.Childs != null && p.Childs.Where(x=>x.Id == item.ID.ToString()).Count() == 1).SingleOrDefault();

            if (locationItem == null)
            {
                IEnumerable<TreeDto> childs = locationsTree.Where(p => p.Childs != null).SelectMany(p => p.Childs);
                return GetParentItem(childs, item);
            }

            return locationItem;
        }

        public ActionResult Tree()
        {
            return View();
        }
    }
}


