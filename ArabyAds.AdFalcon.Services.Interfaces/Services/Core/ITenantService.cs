using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader("Tenants", null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface ITenantService
    {
        [OperationContract]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        TenantDto GetTenantByDomain(string domain);



    }
}

