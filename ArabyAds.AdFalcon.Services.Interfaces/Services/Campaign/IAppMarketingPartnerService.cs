
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract]
    [CacheHeader(LookupNames.AppMarketingPartner, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation =  true)]
    public interface IAppMarketingPartnerService
    {
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<AppMarketingPartnerDto> GetAll();
    }
}
