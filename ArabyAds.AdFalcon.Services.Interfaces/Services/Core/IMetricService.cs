using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Attributes;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    public static class MetricSearcher
    {
        public static MetricDto GetByCode(IEnumerable<MetricDto> items, string code)
        {
            return items.Where(p => p.Code.ToLower() == code.ToLower()).SingleOrDefault();
        }
    }

    [ServiceContract]
    [CacheHeader(LookupNames.impressionmetric, typeof(MetricSearcher), 1440, CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IMetricService
    {
        /// <summary>
        /// Get all the metrics related to the appsite
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        [NoAuthentication]
        List<MetricDto> GetAll();

        /// <summary>
        /// Get metric by Id
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = false)]
        [NoAuthentication]
        MetricDto GetByCode(string code);

        [OperationContract]
        [Cachable(IsGetAll = false)]
        [NoAuthentication]
        List<MetricResultDto> GetMetricResultsAll();


    }
}
