using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework.Persistence;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    [CacheHeader("AdCreativeStatusService", null, CacheStore = "MemoryCache")]
    public interface IAdCreativeStatusService
    {
        /// <summary>
        /// use this service operation to get All Ad Creative Status Dtos 
        /// </summary>
        /// <returns>List Ad Creative Status Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<LookupDto> GetAll();
    }
}
