using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    [CacheHeader(LookupNames.Attributes, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IAdCreativeAttributeService
    {
        /// <summary>
        /// Get All AdCreativeAttribute
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        List<AdCreativeAttributeDto> GetAll();
    }
}
