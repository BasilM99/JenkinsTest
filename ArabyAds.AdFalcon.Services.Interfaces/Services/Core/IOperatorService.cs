using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Attributes;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader(LookupNames.Operator, null, CacheStore = "MemoryCache",  EnableLocalCacheInvalidation = true)]
    public interface IOperatorService
    {
        /// <summary>
        /// use this service operation to get All Operators
        /// </summary>
        /// <returns>List OperatorDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]
        IEnumerable<OperatorDto> GetAll();

        /// <summary>
        /// use this service operation to get All Countries Operators
        /// </summary>
        /// <returns>List CountryOperatorDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<TreeDto> GetAllCountryOperator();

        /// <summary>
        /// use this service operation to get All  Operators by country Ids
        /// </summary>
        /// <param name="countryIds">country Ids to filter by</param>
        /// <returns>List CountryOperatorDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllOperatorByCountryIds(int[] countryIds);
    }
}
