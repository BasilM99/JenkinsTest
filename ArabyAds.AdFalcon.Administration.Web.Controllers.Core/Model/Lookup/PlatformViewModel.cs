using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class PlatformViewModel : LookupViewModel
    {
        private PlatformDto costPlatform;
        public override LookupDto LookupDto
        {
            get { return costPlatform; }
            set { costPlatform = (PlatformDto)value; }
        }
    }

    public class PlatformSaveModel : LookupSaveModel
    {
        public PlatformDto LookupDto { get; set; }
      }
}
