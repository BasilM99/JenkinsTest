using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
  
    [ServiceContract]
    [CacheHeader(LookupNames.impressionmetric, typeof(MetricSearcher), 1440, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IMetricTestService
    {
        /// <summary>
        /// Get all the metrics related to the appsite
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        List<MetricDto> GetAll();

        /// <summary>
        /// Get metric by Id
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = false)]
        MetricDto GetByCode(string code);


    }
}
