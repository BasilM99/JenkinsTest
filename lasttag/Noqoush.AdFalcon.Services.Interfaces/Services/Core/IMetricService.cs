using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System.ServiceModel;
using Noqoush.Framework.Caching;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    public static class MetricSearcher
    {
        public static MetricDto GetByCode(IEnumerable<MetricDto> items, string code)
        {
            return items.Where(p => p.Code.ToLower() == code.ToLower()).SingleOrDefault();
        }
    }

    [ServiceContract]
    [CacheHeader("MetricCache", typeof(MetricSearcher), 1440)]
    public interface IMetricService
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
