using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader("PlatformService", null)]
    public interface IPlatformService
    {
        /// <summary>
        /// use this service operation to get All Platforms
        /// </summary>
        /// <returns>List PlatformDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<PlatformDto> GetAll();

        /// <summary>
        /// use this service operation to get All Platforms
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllPlatformTree();
    }
}
