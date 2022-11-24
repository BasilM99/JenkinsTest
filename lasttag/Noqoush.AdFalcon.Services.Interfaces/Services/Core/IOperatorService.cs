using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Attributes;
namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]
    [CacheHeader("OperatorService", null)]
    public interface IOperatorService
    {
        /// <summary>
        /// use this service operation to get All Operators
        /// </summary>
        /// <returns>List OperatorDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]
        IEnumerable<OperatorDto> GetAll();

        /// <summary>
        /// use this service operation to get All Countries Operators
        /// </summary>
        /// <returns>List CountryOperatorDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<TreeDto> GetAllCountryOperator();

        /// <summary>
        /// use this service operation to get All  Operators by country Ids
        /// </summary>
        /// <param name="countryIds">country Ids to filter by</param>
        /// <returns>List CountryOperatorDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetAllOperatorByCountryIds(int[] countryIds);
    }
}
