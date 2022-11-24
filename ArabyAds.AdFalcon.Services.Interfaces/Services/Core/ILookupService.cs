using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework.EventBroker;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
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
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllPageLookup(LookupCriteria criteria);

        /// <summary>
        /// Get paged Lookup for specific type
        /// </summary>
        /// <returns>all LookupDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetCostPageLookup(LookupCriteria criteria);
        
        /// <summary>
        /// Get All Lookup for specific type
        /// </summary>
        /// <returns>all LookupDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllLookup(LookupCriteriaBase criteria);

        /// <summary>
        /// Get  Lookup for specific type using id
        /// </summary>
        /// <returns>LookupDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupDto GetLookup(LookupGetCriteria criteria);

        /// <summary>
        /// Save Lookup
        /// </summary>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("LookUp Save")]
        void SaveLookup(SaveLookupRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetParentLocations(ValueMessageWrapper<int> parentId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAdTypes();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetNativeAdLayouts();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetLookupName(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CreativeVendorKeywordDto> GetVendorkeywords(ValueMessageWrapper<int> id);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetLookupTextByCode(GetLookupTextByCodeRequest request);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //bool ValidateLangCode(int? id, string code);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<CreativeFormatsDto> CreativeFormatsGetByQuery(CreativeFormatsCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetAllLookupByType(LookupCriteriaBase criteria);
    }
}
