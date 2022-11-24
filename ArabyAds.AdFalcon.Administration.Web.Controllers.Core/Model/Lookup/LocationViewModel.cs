using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class LocationViewModel : LookupViewModel
    {
        private LocationDto locationDto;
        public override LookupDto LookupDto
        {
            get { return locationDto; }
            set { locationDto = (LocationDto)value; }
        }

        public IList<SelectListItem> Locations { get; set; }
    }

    public class LocationSaveModel : LookupSaveModel
    {
        public LocationDto LookupDto { get; set; }
    }
}
