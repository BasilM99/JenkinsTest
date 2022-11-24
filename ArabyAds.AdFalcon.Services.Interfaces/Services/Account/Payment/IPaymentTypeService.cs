using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Payment
{
    [ServiceContract]
    [CacheHeader("PaymentTypeLookup", null, CacheStore = "MemoryCache")]
    public interface IPaymentTypeService
    {
        /// <summary>
        /// Get all Payment Types
        /// </summary>
        /// <returns>List Payment Type Dto</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<PaymentTypeDto> GetAll();
    }
}
