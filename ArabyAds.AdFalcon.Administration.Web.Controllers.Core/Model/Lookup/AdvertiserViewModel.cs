using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
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
