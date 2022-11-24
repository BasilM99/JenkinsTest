using System.Collections.Generic;
using System.ServiceModel;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader(LookupNames.AgeGroup, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IAgeGroupService
    {
        /// <summary>
        /// Get all AgeGroups
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<AgeGroupDto> GetAll();
    }
}
