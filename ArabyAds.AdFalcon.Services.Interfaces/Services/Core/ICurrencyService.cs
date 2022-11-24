using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader(LookupNames.Currency, null, CacheStore = "MemoryCache", EnableLocalCacheInvalidation =  true)]
    public interface ICurrencyService
    {
        /// <summary>
        /// Get all Currencies
        /// </summary>
        /// <returns>all CurrencyDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<CurrencyDto> GetAll();
    }
}
