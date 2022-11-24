using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class AdvertiserViewModel : LookupViewModel
    {
        private AdvertiserDto advertiser;
        public override LookupDto LookupDto
        {
            get { return advertiser; }
            set { advertiser = (AdvertiserDto)value; }
        }
    }

    public class AdvertiserSaveModel : LookupSaveModel
    {
        public AdvertiserDto LookupDto { get; set; }
    }
}
