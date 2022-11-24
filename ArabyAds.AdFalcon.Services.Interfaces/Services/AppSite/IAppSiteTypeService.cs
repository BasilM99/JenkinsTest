using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    [CacheHeader("AppSiteTypeService", null, CacheStore = "MemoryCache")]
    public interface IAppSiteTypeService
    {
        /// <summary>
        /// use this service operation to get All  App Site Type Dtos 
        /// </summary>
        /// <returns>List App Site Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<AppSiteTypeDto> GetAll();

        /// <summary>
        /// use this service operation to get   App Site Type Dtos by id
        /// </summary>
        /// <returns> App Site Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        AppSiteTypeDto Get(ValueMessageWrapper<int> id);
    }
}
