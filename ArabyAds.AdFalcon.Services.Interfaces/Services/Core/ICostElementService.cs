using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader(LookupNames.CostElement, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface ICostElementService
    {
        /// <summary>
        /// Get all Cost Elements
        /// </summary>
        /// <returns>all CostElementDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<CostElementDto> GetAll();
    }
}
