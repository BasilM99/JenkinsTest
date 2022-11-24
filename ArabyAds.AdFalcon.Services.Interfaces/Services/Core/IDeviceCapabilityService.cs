using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Linq.Expressions;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader(LookupNames.DeviceCapability, typeof(IDeviceCapabilityService), CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IDeviceCapabilityService
    {
        /// <summary>
        /// use this service operation to get All Device Capabilities
        /// </summary>
        /// <returns>List DeviceCapabilityDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<DeviceCapabilityDto> GetAll();
    }
}
