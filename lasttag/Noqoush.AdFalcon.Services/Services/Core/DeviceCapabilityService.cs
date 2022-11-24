using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Mapping;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class DeviceCapabilityService : IDeviceCapabilityService
    {

        private IDeviceCapabilityRepository deviceCapabilityRepository = null;
        public DeviceCapabilityService(IDeviceCapabilityRepository deviceCapabilityRepository)
        {
            this.deviceCapabilityRepository = deviceCapabilityRepository;
        }

        /// <summary>
        /// use this service operation to get All Device Capabilities
        /// </summary>
        /// <returns>List DeviceCapabilityDto </returns>
        IEnumerable<DeviceCapabilityDto> IDeviceCapabilityService.GetAll()
        {
            try
            {
                var list = deviceCapabilityRepository.GetAll();
                return list.Select(deviceCapability => MapperHelper.Map<DeviceCapabilityDto>(deviceCapability)).ToList();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
