using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class AdCreativeAttributeViewModel : LookupViewModel
    {
        private AdCreativeAttributeDto adCreativeAttribute;
        public override LookupDto LookupDto
        {
            get { return adCreativeAttribute; }
            set { adCreativeAttribute = (AdCreativeAttributeDto)value; }
        }
    }

    public class AdCreativeAttributeSaveModel : LookupSaveModel
    {
        public AdCreativeAttributeDto LookupDto { get; set; }
    }
}
