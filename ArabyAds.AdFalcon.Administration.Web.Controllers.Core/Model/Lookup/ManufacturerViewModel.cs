using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class ManufacturerViewModel : LookupViewModel
    {
        private ManufacturerDto costManufacturer;
        public override LookupDto LookupDto
        {
            get { return costManufacturer; }
            set { costManufacturer = (ManufacturerDto)value; }
        }
    }

    public class ManufacturerSaveModel : LookupSaveModel
    {
        public ManufacturerDto LookupDto { get; set; }
      }
}
