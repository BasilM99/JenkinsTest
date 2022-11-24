using System.Collections.Generic;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class DeviceViewModel : LookupViewModel
    {
        private DeviceDto deviceDto;
        public override LookupDto LookupDto
        {
            get { return deviceDto; }
            set { deviceDto = (DeviceDto)value; }
        }
        public IList<SelectListItem> Manufacturers { get; set; }
        public IList<SelectListItem> Platforms { get; set; }
        public IList<SelectListItem> DeviceTypes { get; set; }
    }

    public class DeviceSaveModel : LookupSaveModel
    {
        public DeviceDto LookupDto { get; set; }
        public IList<SelectListItem> Manufacturers { get; set; }
        public IList<SelectListItem> Platforms { get; set; }
        public IList<SelectListItem> DeviceTypes { get; set; }
    }
}
