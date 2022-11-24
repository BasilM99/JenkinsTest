using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private IDeviceTypeRepository _DeviceTypeRepository = null;
        public DeviceTypeService(IDeviceTypeRepository deviceRepository)
        {
            this._DeviceTypeRepository = deviceRepository;
        }

        IEnumerable<DeviceTypeDto> IDeviceTypeService.GetAll()
        {
            var list = _DeviceTypeRepository.GetAll();
            return list.Select(deviceType => MapperHelper.Map<DeviceTypeDto>(deviceType)).ToList();
        }
    }
}
