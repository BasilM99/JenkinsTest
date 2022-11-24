using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    [CacheHeader("AppSiteStatusService", null, CacheStore = "MemoryCache")]
    public interface IAppSiteStatusService
    {
        /// <summary>
        /// use this service operation to get All  App Site Status Dtos 
        /// </summary>
        /// <returns>List App Site Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<AppSiteStatusDto> GetAll();

        ///// <summary>
        ///// use this service operation to get   App Site Status Dtos by id
        ///// </summary>
        ///// <returns> App Site Dtos  </returns>
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //[ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        //AppSiteStatusDto Get(int id);
    }
}
