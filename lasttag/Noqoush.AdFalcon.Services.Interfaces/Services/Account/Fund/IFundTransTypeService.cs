using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund
{
    [ServiceContract]
    [CacheHeader("FundTransTypeLookup", null)]
    public interface IFundTransTypeService
    {
        /// <summary>
        /// Get all Fund Trans Types
        /// </summary>
        /// <returns>List Fund Trans Type Dto</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<AccountFundTransTypeDto> GetAll();
    }
}
