using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    


    public class CreativeVendorViewModel : LookupViewModel
    {

        private CreativeVendorDto Vendor;
        public override LookupDto LookupDto
        {
            get { return Vendor; }
            set { Vendor = (CreativeVendorDto)value; }
        }
       // public CreativeVendorDto Vendor { get; set; }
       public IList<CreativeVendorKeywordDto> VendorKeyWord { get; set; }

        public IList<string> InsertedKeyWords { get; set; }

        public IList<string> DeletedKeyWords { get; set; }
    }

    public class CreativeVendorSaveModel : LookupSaveModel
    {
        public CreativeVendorDto LookupDto { get; set; }

        public IList<string> InsertedKeyWords { get; set; }

        public IList<string> DeletedKeyWords { get; set; }
        //public CreativeVendorDto Vendor { get; set; }
        //public IList<CreativeVendorKeywordDto> VendorKeyWord { get; set; }
    }
}
