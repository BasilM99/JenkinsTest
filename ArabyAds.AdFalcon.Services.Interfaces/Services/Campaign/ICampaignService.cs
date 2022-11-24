using System.Collections.Generic;
using ProtoBuf;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.Framework.EventBroker;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using System.Xml.Linq;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Targeting;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]

    public interface ICampaignService
    {
        #region Campaign
        /// <summary>
        /// use this service operation to get list of Campaigns Object depend on the criteria
        /// </summary>
        /// <param name="criteria">criteria to Query By</param>
        /// <returns>CampaignListResultDto that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignListResultDto QueryByCratiria(CampaignCriteria criteria);

        [OperationContract]
        List<CampaignTroubleshootingDto> GetCampaignTroubleshootingDetails(CampaignTroubleshootingCriteria criteria);

        /// <summary>
        /// use this service operation to Delete List of Campaigns using Ids
        /// </summary>
        /// <param name="campaignIds">List of Campaign Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete Campaign")]
        ValueMessageWrapper<bool> Delete(IEnumerable<int> campaignIds);

        /// <summary>
        /// use this service operation to Insert/Update Campaign Object
        /// </summary>
        /// <param name="campaign">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Campaign Save")]
        CampaignSaveDto Save(CampaignDto campaign);

        /// <summary>
        /// use this service operation to get list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Id to Get By</param>
        /// <param name="type">Campaign type to search by , throw exception if the returns campaign has different type than the requested</param>
        /// <returns>CampaignListDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
      //[EventBroker("CampaignStarted")]
        
        CampaignDto Get(GetCampaignRequest request);

        /// <summary>
        /// use this service operation to get Campaign Settings Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Id to Query By</param>
        /// <returns>CampaignSettingsDto that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignSettingsDto GetSettings(ValueMessageWrapper<int> campaignId);

        /// <summary>
        /// use this service operation to save Campaign Settings 
        /// </summary>
        /// <param name="settingsDto">settings to save</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveSettings(CampaignSettingsDto settingsDto);

        /// <summary>
        /// use this service operation to remove Campaign Discount
        /// </summary>
        /// <param name="campaignId">campaign Id</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> RemoveDiscount(ValueMessageWrapper<int> campaignId);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Run Campaign")]
        void Run(int[] campaignIds);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause Campaign")]
        void Pause(int[] campaignIds);

        /// <summary>
        /// Get all account campaigns.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetCampaignsTree();

        /// <summary>
        /// use this service operation to get Clone Campaign
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy Campaign")]
        ResponseDto CloneCampaign(CloneCampaignRequest request);

        /// <summary>
        /// Get Campaign Server Settings
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignServerSettingDto GetServerSettings(ValueMessageWrapper<int> Id);

        /// <summary>
        /// use this service operation to save Campaign Server Settings 
        /// </summary>
        /// <param name="settingDto"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveServerSetting(CampaignServerSettingDto settingDto);

        /// <summary>
        /// Delete frequencycapping event from this campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="frequencyCappingId"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteFrequencyCapping(DeleteFrequencyCappingRequest request);

        /// <summary>
        /// Add frequencycapping event for this campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="frequencyCappingId"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveFrequencyCapping(SaveFrequencyCappingRequest request);

        /// <summary>
        /// Save Campaign Assign Appsites
        /// </summary>
        /// <param name="campaignAssignedAppsites"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Campaign Assign Appsites")]
        void SaveCampaignAssignAppsites(CampaignAssignedAppsitesSaveDTo campaignAssignedAppsites);

        /// <summary>
        /// Save Campaign Bid Config
        /// </summary>
        /// <param name="campaignBidConfigSaveDTo"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Campaign Bid Config")]
        void SaveCampaignBidConfig(CampaignBidConfigSaveDTo campaignBidConfigSaveDTo);
        /// <summary>
        /// Get Campaign Assign Appsites
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignAssignedAppsitesModelDto GetCampaignAssignAppsites(ValueMessageWrapper<int> campaignId);
        /// <summary>
        /// Get Campaign Bid Configs
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignBidConfigModelDto GetCampaignBidConfigs(CampaignIdAdgroupIdMessage request);

        /// <summary>
        /// Rename a gruop 
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="gruipId"></param>
        /// <param name="name"></param>
        /// <returns>string</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string RenameGroup(RenameGroupRequest request);

        /// <summary>
        /// CehckAppsitesCostModelCompatableWitCampaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="adGoupId"></param>
        /// <param name="appsites"></param>
        /// <param name="notCompatibleAppSiteList"></param>
        /// <param name="checkExisting"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CheckAppsitCostModelCompatableWitCampaignsResponse CheckAppsitesCostModelCompatableWitCampaign(CheckAppsitesCostModelCompatableWitCampaignRequest request);


        /// <summary>
        /// use this service operation to Insert/Update Campaign Object
        /// </summary>
        /// <param name="campaign">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Campaign Info Settings Save")]
        CampaignSaveDto SaveCampInfoSettings(CampaignAllDto oCampaignAllDto);
        #endregion

        #region Ad Groups
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PMPDealDto> GetPMPDealConfigConfigs(CampaignIdAdgroupIdMessage request);
        /// <summary>
        /// use this service operation to get list of AdGroups Object depend on the criteria
        /// </summary>
        /// <returns>AdGroupSearchDto that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupSearchDto QueryGroupsByCratiria(AdGroupCriteria criteria);

        /// <summary>
        /// use this service operation to Delete List of AdGroups using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupIds">List of AdGroup Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete AdGroup")]
        ValueMessageWrapper<bool> DeleteGroups(CampaignIdAdgroupIdsMessage request);

        /// <summary>
        /// use this service operation to run list of Ad Groups Object depend on the Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Run AdGroup")]
        void RunGroups(CampaignIdAdgroupIdsMessage request);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause AdGroup")]
        void PauseGroups(CampaignIdAdgroupIdsMessage request);

        /// <summary>
        /// use this service operation to Insert/Update AdGroup Object
        /// </summary>
        /// <param name="adGroup">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> SaveAdGroup(SaveAdGroupRequest request);

        /// <summary>
        /// use this service operation to get list of AdGroups Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupId">Id to Get By</param>
        /// <returns>AdGroupListDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupDto GetAdGroup(CampaignIdAdgroupIdMessage request);

        /// <summary>
        /// use this service operation to get list of AdGroups  Targeting Objects depend on the AdGroupId
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupId">AdGroup Id to Query By</param>
        /// <returns>TargetingListDto  that Hold List of TargetingBaseDto match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ServiceKnownType(typeof(DeviceTargetingDto))]
        [ServiceKnownType(typeof(KeywordTargetingDto))]
        [ServiceKnownType(typeof(GeographicTargetingDto))]
        [ServiceKnownType(typeof(OperatorTargetingDto))]
        [ServiceKnownType(typeof(IPTargetingDto))]
        [ServiceKnownType(typeof(DemographicTargetingDto))]
        [ServiceKnownType(typeof(GeoFencingTargetingDto))]
        [ServiceKnownType(typeof(AdRequestTargetingDto))]
        [ServiceKnownType(typeof(ImpressionMetricTargetingDto))]
        [ServiceKnownType(typeof(LanguageTargetingDto))]

        [ServiceKnownType(typeof(VideoTargetingDto))]
        TargetingListDto GetTargeting(GetTargetingRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Targeting")]
        TargetingResultDto SaveTargeting(TargetingSaveDto targetingSaveDto);


        /// <summary>
        /// use this service operation to get the minimum Bid
        /// </summary>
        /// <returns>minimum Bid depend on the info</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ReturnBidDto GetMinBid(BidDto info);

        /// <summary>
        /// Get all account adgroups per campaign
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdGroupsTree();

        /// <summary>
        /// use this service operation to get Clone AdGroup
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy AdGroup")]
        ResponseDto CloneAdGroup(CloneAdgroupRequest request);


        #region Cost Elements
        /// <summary>
        /// use this service operation to get AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupCostElementResultDto GetAdGroupCostElements(AdGroupCostElementCriteria criteria);

        /// <summary>
        /// use this service operation to get AdGrouyp Cost Element by Id
        /// </summary>
        /// <returns>adGroup Cost Element</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupCostElementDto GetAdGroupCostElement(GetAdGroupCostElementRequest request);

        /// <summary>
        /// use this service operation to add AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void AddCostElements(AdGroupCostElementSaveDto saveDto);

        /// <summary>
        /// use this service operation to update AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateCostElements(AdGroupCostElementSaveDto saveDto);

        /// <summary>
        /// use this service operation to remove Cost Elements from AdGrouyp
        /// </summary>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveCostElements(AdGroupCostElementSaveDto saveDto);
        #endregion

        /// <summary>
        /// use this service operation to get AdGroup Settings Object depend on the Id
        /// </summary>
        /// <param name="adGroupId">Id to Query By</param>
        /// <param name="campaignId">campaign Id to Query By</param>
        /// <returns>CampaignSettingsDto that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupSettingsDto GetAdGroupSettings(CampaignIdAdgroupIdMessage request);

        /// <summary>
        /// use this service operation to save AdGroup Settings 
        /// </summary>
        /// <param name="settingsDto">settings to save</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveAdGroupSettings(AdGroupSettingsDto settingsDto);

        /// <summary>
        /// Get TrackingEvents for the adgroup, get the default TrackingEvents if there is no trackingevents
        /// </summary>
        /// <param name="adGroupId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupTrackingEventResultDto GetAdGroupTrackingEvents(AdGroupTrackingEventCriteriaDto criteria);


        /// <summary>
        /// Get TrackingEvents for the Account, in union with all the system events 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupTrackingEventResultDto GetAccountTrackingEvents();


        /// <summary>
        /// Delete tracking event for this adgroup
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="adGroupId">adGroupId</param>
        /// <param name="adGroupTrackingEventId">adgroup tracking event id</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteTrackingEvent(DeleteTrackingEventRequest request);

        /// <summary>
        /// Get AdGroup Tracking Events prerequisites List
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="adGroupId">adGroupId</param>
        /// <param name="costModelWrapperId">costModelWrapperId</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdGroupTrackingEventDto> GetCostModelWrapperTrackingEvents(GetCostModelWrapperTrackingEventsRequest request);

        /// <summary>
        /// Check if this tracking event could be deleted or not
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="adGroupId"></param>
        /// <param name="adGroupTrackingEventCodes"></param>
        /// <param name="checkStandards"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<KeyValuePair<bool, string>> IsDeleteTrackingEventAllowed(IsDeleteTrackingEventAllowedRequest request);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adGroupId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> CheckEventUniqueByCode(string code);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> checkSystemEventFraud(CheckSystemEventFraudRequest request);

  

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsAllowedGroupById(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsAllowedAdById(ValueMessageWrapper<int> id);

        #endregion

        #region Ads

        /// <summary>
        /// use this service operation to get list of Ads Object depend on the criteria
        /// </summary>
        /// <returns>AdsSearchDto that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdsSearchDto QueryAdsByCratiria(AdsCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<AdTypeIds> GetAddTypesByAddGroupAction(ValueMessageWrapper<int> adGroupId);
        /// <summary>
        /// use this service operation to get list of Ads Object that has bid less than certain value
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="bid">the bid value to check</param>
        /// <returns>List of AdbIDListDto that has bid less than</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<AdbIDListDto> QueryAdsLessBid(QueryAdsBidRequest request);

        /// <summary>
        /// use this service operation to update Bid for List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <param name="bid">new Bid value </param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SetAdsBid(SetAdsBidRequest request);

        /// <summary>
        /// use this service operation to get Ad Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreativeId">Id to Get By</param>
        /// <returns>AdCreativeDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdCreativeDto GetAdCreative(GetAdCreativeRequest requests);

        /// <summary>
        /// use this service operation to Insert/Update Ad Creative Object
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreative">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Ad")]
        ValueMessageWrapper<int> SaveAd(SaveAdRequest request);

        /// <summary>
        /// use this service operation to Delete List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete Ad")]
        ValueMessageWrapper<bool> DeleteAds(CampaignIdAdgroupIdAdIdsMessage request);

        /// <summary>
        /// use this service operation to run list of Ad Groups Object depend on the Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Run Ad")]
        void RunAds(CampaignIdAdgroupIdAdIdsMessage request);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause Ad")]
        void PauseAds(CampaignIdAdgroupIdAdIdsMessage request);

        /// <summary>
        /// use this service operation to Reject Ad
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adId">Id to Get Ad  By</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RejectAd(CampaignIdAdgroupIdAdIdMessage request);

        /// <summary>
        /// use this service operation to Approve Ad
        /// </summary>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void ApproveAd(ApproveAdDto approveAdDto);


        /// <summary>
        /// Get all account ads per campaign
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdsTree();

        /// <summary>
        /// Get all account ads per campaign and execlude specific AdId if it's no null
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAdsTreeByAccount")]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdsTree(GetAdsTreeRequest request);

        /// <summary>
        /// Get all unapproved ads from the adgroup of this ad
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<AdCreativeDtoBase> GetUnApprovedAdsFromAdGroupOfAd(ValueMessageWrapper<int> adId);

        /// <summary>
        /// Get Ad Creative Summary
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdCreativeSummaryDto GetAdSummary(CampaignIdAdgroupIdAdIdMessage request);

        /// <summary>
        /// Get Ad Creative Full Summary
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdCreativeFullSummaryDto GetAdFullSummary(CampaignIdAdgroupIdAdIdMessage request);


        /// <summary>
        /// use this service operation to get list of Campaign Summary Objects depend on the criteria
        /// </summary>
        /// <returns>CampaignSummaryDtos that match the criteria</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<CampaignSummaryDto> GetAdsSummary(AdsSummaryCriteria criteria);


        /// <summary>
        /// use this service operation to get Clone Ad
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy Ad")]
        ResponseDto CloneAd(CloneAdRequest request);

        /// <summary>
        /// use this service operation to Format Ad Creative Content
        /// </summary>
        /// <returns>Formatted Ad Creative Content</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        FormattedContentDto FormatAdCreativeContent(FormatAdCreativeContentRequest request);

        /// <summary>
        /// use this service operation to check if Ad Creative Content is formatted
        /// </summary>
        /// <returns>true if Ad Creative Content is Formatted </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsFormattedAdCreativeContent(string content);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<int> GetAppSiteAdQueues(CampaignIdAdgroupIdAdIdMessage request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        InventorySourceModelDto GetInventorySources(CampaignIdAdgroupIdMessage request);
        #endregion
        #region AdRequest

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        TypesPlatformsVersions GetAdRequestTypes_Platforms_Versions();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdRequestTargetingDtoResultDto GetAdRequestTargetings(AdRequestCriteria Criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveAdRequestTargeting(AdRequestTargetingDto dto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteAdRequestTargeting(ValueMessageWrapper<int> Id);

        #endregion


        #region ImpressionMetricTargetings

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpressionMetricTargetingDto GetImpressionMetricTargeting(ValueMessageWrapper<int> TargetingId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpressionMetricTargetingResultDto GetImpressionMetricTargetings(ImpressionMetricCriteria Criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveImpressionMetricTargeting(ImpressionMetricTargetingDto dto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteImpressionMetricTargeting(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<ImpressionMetricDto> GetImpressionMetrics();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpressionMetricDto GetImpressionMetric(ValueMessageWrapper<int> Id);

        #endregion


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        ValueMessageWrapper<int> GetCampaignAdvertiserAccount(ValueMessageWrapper<int> campaignId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetCreativeVendorForAdCreativeUnit(ValueMessageWrapper<int> AdCreativeUnitId);

        /*[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        XDocument downloadXml(string url);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        bool IsValidXml(string xmlString, string XsdsFolderPath);*/


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<VideoMediaFileDto> GetVideoMediaFiles(ValueMessageWrapper<int> videoAdId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void AddUpdateVideoMediaFile(VideoMediaFileDto dto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<AdCreativeDto> GetDraftVideoAd();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void PublishVideoAd(ValueMessageWrapper<int> videoAdint);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetMIMEType(string code);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<VideoConversionCreativeUnitDto> GetVideoConversionCreativeUnits(string code);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AudicanceBillSummary DeserializeRule(string groupJson);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdGroupDto> GetAllAdGroupByAccount(ValueMessageWrapper<int> AccountId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UploadTest(UploadTestRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetDirectory(ValueMessageWrapper<int> index);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<MetricVendorDto> getMetricVendors();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetAdvertiserString(ValueMessageWrapper<int> AdvertiserId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetCampaignAdvertiser(ValueMessageWrapper<int> campaignId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetCampaignsAdvTree(ValueMessageWrapper<int?> AdvertiserId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdsAdvTree(ValueMessageWrapper<int?> AdvertiserId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdGroupsAdvTree(ValueMessageWrapper<int?> AdvertiserId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetAdvertiserIdByCampaignId(ValueMessageWrapper<int> id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetAccountAdvertiserId(ValueMessageWrapper<int> advertiserId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetAdvertiserAccountString(ValueMessageWrapper<int> AdvertiserId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetAdvertiserIdFromAccount(ValueMessageWrapper<int> AdvertiserId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetAdvertiserAccountIdByCampaignId(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsReadOrWriteCampaign(ValueMessageWrapper<int> CampaignId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsWriteCampaign(ValueMessageWrapper<int> CampaignId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetPublisherCounterCurrentWeek();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupDealAndSourceDTO GetDealsAndSources(CampaignIdAdgroupIdMessage request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        TargetingResultDto AddExternalAudSegmentTargeting(AddExternalAudSegmentTargetingRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string SaveSegmentsForTargeting(SaveAudSegmentTargetingRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string getAudiancesUsedInIntegration(AudienceIdsMessages request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountMasterAppSiteDto> GetMasterListConfigConfigs(CampaignIdAdgroupIdAdIdMessage request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetAudienceListCounter(string dpName);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignSettingsDto GetCampSettings(ValueMessageWrapper<int> campaignId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveCampSettings(CampaignSettingsDto settingsDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> getCountAudiancesUsedInIntegration(AudienceIdsMessages request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string getDataBidAudiancesUsedInIntegration(AudienceIdsMessages request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> getCountAudiancesUsedInIntegrationAll(ValueMessageWrapper<int> adgroupId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string getDataBidAudiancesUsedInIntegrationAll(ValueMessageWrapper<int> adgroupId);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void AddDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupDynamicBiddingConfigDto GetAdGroupDynamicBiddingConfig(GetAdGroupDynamicBiddingConfigRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupDynamicBiddingConfigResultDto GetAdGroupDynamicBiddingConfigs(AdGroupCostElementCriteria criteria);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateAdGroupTrackingEvent(AdGroupTrackingEventSaveDto trackingEventSaveDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdGroupConversionEventResultDto GetAccountConversionEvents();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void AddDefaultAdGroupTrackingEventById(AddDefaultAdGroupTrackingEventByIdRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void PublishTargetingEvent(TargetingResultDto eventino);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string SaveSegmentsForTargetingForDel(SaveAudSegmentTargetingRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string getAudiancesUsedInIntegrationActive(AudienceIdsMessages request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DoesContainDataProviderAllowImpressionTracker(CampaignIdAdgroupIdMessage request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetActionTypeByadGroup(ValueMessageWrapper<int> adGroupId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<AdbIDListDto> QueryAdsMoreBid(QueryAdsBidRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsFormatedAdCreativUnit(string content);

        [OperationContract]
        List<AdGroupBidModifierDto> GetCampBidModifiers(CampaignIdAdgroupIdMessage request);
        [OperationContract]
        List<AdGroupBidModifierDto> GetAdGroupBidModifiers(CampaignIdAdgroupIdMessage request);
        [OperationContract]
        void SaveCampBidModifiers(SaveBidModifierRequest request);
        [OperationContract]
        AdGroupDto GetAdGroupInfo(CampaignIdAdgroupIdMessage request);

        [OperationContract]
        IList<DropDownDto> GetCampaignAdGroups(ValueMessageWrapper<int> CampaignId);

        [OperationContract]
        CampaignDto GetCampInfo(GetCampaignRequest request);

        [OperationContract]
        void SaveTargetingHouseAd(TargetingSaveDto targetingSaveDto);
        [OperationContract]
        void ApproveAdForNewUI(ApproveAdDto approveAdDto);


        [OperationContract]
        AudicanceBillSummary DeserializeContextualRule(string groupJson);
    }
}
