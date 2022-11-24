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
    [CacheHeader("LocationLookup", null)]
    public interface ILocationService
    {
        /// <summary>
        /// Get all Locations
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]

        IEnumerable<LocationDto> GetAll();

        /// <summary>
        /// use this service operation to get All Locations Tree Node
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetTree();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetTestTree();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<Interfaces.DTOs.Core.LocationDto> GetContinentsByCountries(int[] Countries);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        Interfaces.DTOs.Core.LocationDto GetCountryById(int Id);
    }
}
