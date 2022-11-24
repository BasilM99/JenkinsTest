using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
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
