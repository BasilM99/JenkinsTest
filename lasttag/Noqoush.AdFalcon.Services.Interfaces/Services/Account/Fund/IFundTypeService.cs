using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund
{
    [ServiceContract]
    [CacheHeader("FundTypeLookup", null)]
    public interface IFundTypeService
    {
        /// <summary>
        /// Get all Fund Types
        /// </summary>
        /// <returns>List Fund Type Dto</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<AccountFundTypeDto> GetAll();
    }
}