using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("AgeGroupService", null)]
    public interface IAgeGroupService
    {
        /// <summary>
        /// Get all AgeGroups
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<AgeGroupDto> GetAll();
    }
}
