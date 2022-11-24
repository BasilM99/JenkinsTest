using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund
{
    [ServiceContract]
    [CacheHeader("FundTransTypeLookup", null, CacheStore = "MemoryCache")]
    public interface IFundTransTypeService
    {
        /// <summary>
        /// Get all Fund Trans Types
        /// </summary>
        /// <returns>List Fund Trans Type Dto</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<AccountFundTransTypeDto> GetAll();
    }
}
