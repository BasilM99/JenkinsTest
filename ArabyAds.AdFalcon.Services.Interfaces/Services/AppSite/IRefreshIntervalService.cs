using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    [CacheHeader("RefreshIntervalService", null, CacheStore = "MemoryCache")]
    public interface IRefreshIntervalService
    {
        /// <summary>
        /// use this service operation to get All Refresh Intervals 
        /// </summary>
        /// <returns>List of Refresh Intervals </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<LookupDto> GetAll();
    }
}
