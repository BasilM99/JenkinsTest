using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    [CacheHeader("RefreshIntervalService", null)]
    public interface IRefreshIntervalService
    {
        /// <summary>
        /// use this service operation to get All Refresh Intervals 
        /// </summary>
        /// <returns>List of Refresh Intervals </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<LookupDto> GetAll();
    }
}
