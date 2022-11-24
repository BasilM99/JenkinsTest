using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.Framework.Attributes;
//using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("SystemPayPalAccounts", null)]
    public interface ISystemPayPalAccountService
    {
        /// <summary>
        /// Get all System PayPal Accounts
        /// </summary>
        /// <returns>SystemPayPalAccountDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(typeof(ISystemPayPalAccountService), IsGetAll = true)]
        IEnumerable<SystemPayPalAccountDto> GetAll();
    }
}
