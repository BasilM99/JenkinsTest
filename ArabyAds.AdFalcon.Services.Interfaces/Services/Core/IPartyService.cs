using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("PartyService", null)]
    public interface IPartyService
    {
        /// <summary>
        /// Get all Parties
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<PartyDto> GetAll();


        /// <summary>
        /// Get all Parties by Criteria
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PartyListResultDto QueryByCriteria(PartyCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        EmployeeDto GetEmployee(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        BusinessPartnerDto GetBusinessPartner(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> SaveEmployee(EmployeeDto saveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> SaveBusinessPartner(BusinessPartnerDto saveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteParty(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        ValueMessageWrapper<bool> DeleteParties(int[] Ids);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //bool DeleteBusinessPartner(int Id);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //bool DeleteEmployee(int Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetSupplyBusinesPartner();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetDemandBusinesPartner();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        BusinessPartnerDto GetDPPartnerByAccount(ValueMessageWrapper<int> id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PartyListResultDto QueryByCriteriaForDPPartner(DPPartnerCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetDPBusinesPartner();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PartyDto> GetAllExternalDPPartner(ValueMessageWrapper<int> campId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PartyDto GetExternalDPPartner(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void SaveImpressionLogWrritten(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<ImpressionLogDto> GetImpressionLogsNotWrritten(ValueMessageWrapper<int> ProviderId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<BusinessPartnerDto> GetDPPartnersFTP();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PartyDto> GetAllInternalDPPartner(ValueMessageWrapper<int> campId);
    }
}
