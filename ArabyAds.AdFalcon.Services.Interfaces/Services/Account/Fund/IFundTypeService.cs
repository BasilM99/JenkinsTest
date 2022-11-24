using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund
{
    [ServiceContract]
    [CacheHeader("FundTypeLookup", null, CacheStore = "MemoryCache")]
    public interface IFundTypeService
    {
        /// <summary>
        /// Get all Fund Types
        /// </summary>
        /// <returns>List Fund Type Dto</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<AccountFundTypeDto> GetAll();
    }
}