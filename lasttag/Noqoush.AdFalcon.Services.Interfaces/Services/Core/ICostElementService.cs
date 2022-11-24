using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("CostElementLookup", null)]
    public interface ICostElementService
    {
        /// <summary>
        /// Get all Cost Elements
        /// </summary>
        /// <returns>all CostElementDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<CostElementDto> GetAll();
    }
}
