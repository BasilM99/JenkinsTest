using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Runtime.Serialization;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    [CacheHeader("CountriesLookup", null)]
    public interface ICountryService
    {


        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        Interfaces.DTOs.Core.CountryDto GetByCode(string Code);
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<CountryDto> GetAll();

        [OperationContract]
        
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.LocationDto> GetAllStates(int parentCountryId);
    }
}
