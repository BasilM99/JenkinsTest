using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("JobPositionService", null)]
    public interface IJobPositionService
    {
        /// <summary>
        /// Get all JobPositions
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<JobPositionDto> GetAll();

        /// <summary>
        /// Get all JobPositions that start with prefix value
        /// </summary>
        /// <param name="prefix">string value to search with</param>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsSelfCachable = true)]
        IEnumerable<JobPositionDto> GetByName(string prefix);
    }
}
