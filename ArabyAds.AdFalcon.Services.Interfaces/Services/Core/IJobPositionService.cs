using System.Collections.Generic;
using System.ServiceModel;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader(LookupNames.JobPosition, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IJobPositionService
    {
        /// <summary>
        /// Get all JobPositions
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<JobPositionDto> GetAll();

        /// <summary>
        /// Get all JobPositions that start with prefix value
        /// </summary>
        /// <param name="prefix">string value to search with</param>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsSelfCachable = true)]
        IEnumerable<JobPositionDto> GetByName(string prefix);
    }
}
