using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.System
{
    [ServiceContract]
    public interface ISystemAccountService
    {
        /// <summary>
        /// Get System Payment Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PaymentDetailDto> GetSystemPaymentDetails(ValueMessageWrapper<PayemntAccountType> accountType);
        [OperationContract]
        ValueMessageWrapper<bool> Decypt();

        [OperationContract]
        ValueMessageWrapper<bool> Encrypt();
        [OperationContract]
        ValueMessageWrapper<bool> Unprotect();
        [OperationContract]
        ValueMessageWrapper<bool> protect();
    }
}
