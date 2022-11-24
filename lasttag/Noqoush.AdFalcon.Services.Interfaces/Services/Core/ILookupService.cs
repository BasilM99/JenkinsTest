using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.EventBroker;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    //[CacheHeader("LookupService", null)]
    public interface ILookupService
    {
        /// <summary>
        /// Get paged Lookup for specific type
        /// </summary>
        /// <returns>all LookupDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllPageLookup(LookupCriteria criteria);

        /// <summary>
        /// Get paged Lookup for specific type
        /// </summary>
        /// <returns>all LookupDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetCostPageLookup(LookupCriteria criteria);
        
        /// <summary>
        /// Get All Lookup for specific type
        /// </summary>
        /// <returns>all LookupDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllLookup(LookupCriteriaBase criteria);

        /// <summary>
        /// Get  Lookup for specific type using id
        /// </summary>
        /// <returns>LookupDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupDto GetLookup(LookupGetCriteria criteria);

        /// <summary>
        /// Save Lookup
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("LookUp Save")]
        void SaveLookup(LookupDto data, string lookType);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetParentLocations(int parentId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAdTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetNativeAdLayouts();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetLookupName(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<CreativeVendorKeywordDto> GetVendorkeywords(int id);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetLookupTextByCode(string Code, string LookupType);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //bool ValidateLangCode(int? id, string code);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<CreativeFormatsDto> CreativeFormatsGetByQuery(CreativeFormatsCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllLookupByType(LookupCriteriaBase criteria);
    }
}
