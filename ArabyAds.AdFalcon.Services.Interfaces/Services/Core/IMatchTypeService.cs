using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("MatchTypeList", null, CacheStore = "MemoryCache")]
    public interface IMatchTypeService
    {
        /// <summary>
        /// Get all match types
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        List<LookupDto> GetAll();
    }
}
