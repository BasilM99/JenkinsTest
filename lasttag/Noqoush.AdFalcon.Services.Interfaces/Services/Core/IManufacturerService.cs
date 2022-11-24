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
    [CacheHeader("ManufacturerService", null)]
    public interface IManufacturerService
    {
        /// <summary>
        /// use this service operation to get All Manufacturers
        /// </summary>
        /// <returns>List ManufacturerDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<ManufacturerDto> GetAll();

        /// <summary>
        /// use this service operation to get All Manufacturers
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllManufacturerTree();
    }
}
