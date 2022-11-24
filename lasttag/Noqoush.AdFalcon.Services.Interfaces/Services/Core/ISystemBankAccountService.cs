using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    [CacheHeader("SystemBankAccounts", null)]
    public interface ISystemBankAccountService
    {
        /// <summary>
        /// Get all System Bank Accounts
        /// </summary>
        /// <returns>SystemBankAccountDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(typeof(ISystemBankAccountService), IsGetAll = true)]
        IEnumerable<SystemBankAccountDto> GetAll();
    }
}
