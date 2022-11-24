using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("CurrenciesLookup", null)]
    public interface ICurrencyService
    {
        /// <summary>
        /// Get all Currencies
        /// </summary>
        /// <returns>all CurrencyDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<CurrencyDto> GetAll();
    }
}
