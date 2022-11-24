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
    [CacheHeader("CountriesLookup", null, CacheStore = "MemoryCache", TypeName = LookupNames.LocationBase, EnableLocalCacheInvalidation = true)]
    public interface ICountryService
    {


        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        Interfaces.DTOs.Core.CountryDto GetByCode(string Code);
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<CountryDto> GetAll();

        [OperationContract]
        
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.LocationDto> GetAllStates(ValueMessageWrapper<int> parentCountryId);
    }
}
