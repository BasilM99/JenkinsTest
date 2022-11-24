using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
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
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<PartyDto> GetAll();


        /// <summary>
        /// Get all Parties by Criteria
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PartyListResultDto QueryByCriteria(PartyCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        EmployeeDto GetEmployee(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        BusinessPartnerDto GetBusinessPartner(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int SaveEmployee(EmployeeDto saveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int SaveBusinessPartner(BusinessPartnerDto saveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteParty(int Id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        bool DeleteParties(int[] Ids);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //bool DeleteBusinessPartner(int Id);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //bool DeleteEmployee(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetSupplyBusinesPartner();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetDemandBusinesPartner();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        BusinessPartnerDto GetDPPartnerByAccount(int id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PartyListResultDto QueryByCriteriaForDPPartner(DPPartnerCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetDPBusinesPartner();


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PartyDto> GetAllExternalDPPartner(int campId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PartyDto GetExternalDPPartner(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void SaveImpressionLogWrritten(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<ImpressionLogDto> GetImpressionLogsNotWrritten(int ProviderId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<BusinessPartnerDto> GetDPPartnersFTP();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PartyDto> GetAllInternalDPPartner(int campId);
    }
}
