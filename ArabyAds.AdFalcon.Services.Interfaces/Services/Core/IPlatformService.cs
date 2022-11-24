using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader(LookupNames.Platform, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IPlatformService
    {
        /// <summary>
        /// use this service operation to get All Platforms
        /// </summary>
        /// <returns>List PlatformDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<PlatformDto> GetAll();

        /// <summary>
        /// use this service operation to get All Platforms
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllPlatformTree();


        [OperationContract]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        PlatformDto GetById(ValueMessageWrapper<int> Id);
    }
}
