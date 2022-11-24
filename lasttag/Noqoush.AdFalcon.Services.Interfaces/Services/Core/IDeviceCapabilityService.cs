using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Linq.Expressions;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader("DeviceCapabilityService", typeof(IDeviceCapabilityService))]
    public interface IDeviceCapabilityService
    {
        /// <summary>
        /// use this service operation to get All Device Capabilities
        /// </summary>
        /// <returns>List DeviceCapabilityDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<DeviceCapabilityDto> GetAll();
    }
}
