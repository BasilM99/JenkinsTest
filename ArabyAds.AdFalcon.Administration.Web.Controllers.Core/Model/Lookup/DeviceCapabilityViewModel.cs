
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class DeviceCapabilityViewModel : LookupViewModel
    {
        private DeviceCapabilityDto deviceCapability;
        public override LookupDto LookupDto
        {
            get { return deviceCapability; }
            set { deviceCapability = (DeviceCapabilityDto)value; }
        }
    }

    public class DeviceCapabilitySaveModel : LookupSaveModel
    {
        public DeviceCapabilityDto LookupDto { get; set; }
    }
}

