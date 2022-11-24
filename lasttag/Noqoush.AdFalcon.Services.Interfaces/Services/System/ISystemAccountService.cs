using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.System
{
    [ServiceContract]
    public interface ISystemAccountService
    {
        /// <summary>
        /// Get System Payment Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PaymentDetailDto> GetSystemPaymentDetails(PayemntAccountType accountType);

    }
}
