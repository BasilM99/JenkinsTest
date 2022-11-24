using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    public interface IHouseAdService
    {
        /// <summary>
        /// use this service operation to get list of House Ads Campaigns depend on the criteria
        /// </summary>
        /// <param name="criteria">criteria to Query By</param>
        /// <returns>House Ad Campaigns that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignListResultDto QueryHouseAdsCampaignsCratiria(CampaignCriteria criteria);

         
        /// <summary>
        /// use this service operation to get House Ad Campaign depend on the id
        /// </summary>
        /// <param name="id">id to Query By</param>
        /// <returns>House Ad Campaign that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignDto GetHouseAdCampaign(int id);

        /// <summary>
        /// use this service operation to get House Ad Object depend on the id
        /// </summary>
        /// <param name="id">id to Query By</param>
        /// <returns>House Ads that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        HouseAdDto Get(int id);


        /*

        /// <summary>
        /// use this service operation to Delete List of House Ads using Ids
        /// </summary>
        /// <param name="houseAdIds">Ids to Get By</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [EventBroker("Delete House Ad")]
        bool Delete(int[] houseAdIds);

        /// <summary>
        /// use this service operation to run list of House Ads depend on the Ids
        /// </summary>
        /// <param name="houseAdIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Run House Ad")]
        void Run(int[] houseAdIds);

        /// <summary>
        /// use this service operation to run list of House Ads  depend on the Id
        /// </summary>
        /// <param name="houseAdIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Pause House Ad")]
        void Pause(int[] houseAdIds);

        */
        #region House Ad Group
        /// <summary>
        /// use this service operation to Insert/Update house Ad Group Object
        /// </summary>
        /// <param name="houseAdGroup">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int SaveAdGroup(HouseAdGroupDto houseAdGroup);

        #endregion



    }
}
