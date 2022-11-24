
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader(LookupNames.DeviceType, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IDeviceTypeService
    {
        /// <summary>
        /// Get all Currencies
        /// </summary>
        /// <returns>all CurrencyDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<DeviceTypeDto> GetAll();
    }
}
