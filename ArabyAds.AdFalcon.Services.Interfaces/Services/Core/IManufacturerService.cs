using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader(LookupNames.Manufacturer, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IManufacturerService
    {
        /// <summary>
        /// use this service operation to get All Manufacturers
        /// </summary>
        /// <returns>List ManufacturerDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<ManufacturerDto> GetAll();

        /// <summary>
        /// use this service operation to get All Manufacturers
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllManufacturerTree();
    }
}
