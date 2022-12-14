using System.Collections.Generic;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class ListViewModel 
    {
        public IList<SelectListItem> LookupTypes { get; set; }
        public IEnumerable<LookupDto> Items { get; set; }
        public string EntityType { get; set; }
        public string FilterView { get; set; }
        public string SearchAction { get; set; }
        public int Type { get; set; }

        public string Name { get; set; }
    }

    public class DeviceListViewModel : ListViewModel
    {
        public IList<SelectListItem> Manufacturers { get; set; }
        public IList<SelectListItem> Platforms { get; set; }
    }
}
