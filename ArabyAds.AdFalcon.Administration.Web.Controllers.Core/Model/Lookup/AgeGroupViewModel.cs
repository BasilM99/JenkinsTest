using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class AgeGroupViewModel : LookupViewModel
    {
        private AgeGroupDto AgeGroupDto;
        public override LookupDto LookupDto
        {
            get { return AgeGroupDto; }
            set { AgeGroupDto = (AgeGroupDto)value; }
        }

        public IList<SelectListItem> AgeGroups { get; set; }
    }

    public class AgeGroupSaveModel : LookupSaveModel
    {
        public AgeGroupDto LookupDto { get; set; }
    }
}
