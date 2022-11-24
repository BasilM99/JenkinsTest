using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.Payment
{
    [ServiceContract]
    [CacheHeader("PaymentTypeLookup", null)]
    public interface IPaymentTypeService
    {
        /// <summary>
        /// Get all Payment Types
        /// </summary>
        /// <returns>List Payment Type Dto</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<PaymentTypeDto> GetAll();
    }
}
