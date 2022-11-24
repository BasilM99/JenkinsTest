using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Runtime.Serialization;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    [CacheHeader("CostModelWrapperService", null)]
    public interface ICostModelWrapperService
    {
        /// <summary>
        /// use this service operation to get All costmodelwrappers
        /// </summary>
        /// <returns>List DeviceDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<CostModelWrapperDto> GetAll();
    }
}
