using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    [CacheHeader("LocationLookup", null, CacheStore = "MemoryCache", TypeName = LookupNames.LocationBase, EnableLocalCacheInvalidation = true)]
    public interface ILocationService
    {
        /// <summary>
        /// Get all Locations
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]

        IEnumerable<LocationDto> GetAll();

        /// <summary>
        /// use this service operation to get All Locations Tree Node
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetTree();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetTestTree();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<Interfaces.DTOs.Core.LocationDto> GetContinentsByCountries(int[] Countries);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        Interfaces.DTOs.Core.LocationDto GetCountryById(ValueMessageWrapper<int> Id);
    }
}
