using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using System.Xml.Linq;
using Noqoush.Framework.Attributes;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Targeting;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
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
        [FaultContract(typeof(ServiceFault))]
        CampaignListResultDto QueryByCratiria(CampaignCriteria criteria);



        /// <summary>
        /// use this service operation to Delete List of Campaigns using Ids
        /// </summary>
        /// <param name="campaignIds">List of Campaign Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete Campaign")]
        bool Delete(IEnumerable<int> campaignIds);

        /// <summary>
        /// use this service operation to Insert/Update Campaign Object
        /// </summary>
        /// <param name="campaign">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Campaign Save")]
        CampaignSaveDto Save(CampaignDto campaign);

        /// <summary>
        /// use this service operation to get list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Id to Get By</param>
        /// <param name="type">Campaign type to search by , throw exception if the returns campaign has different type than the requested</param>
        /// <returns>CampaignListDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
      //[EventBroker("CampaignStarted")]
        
        CampaignDto Get(int campaignId, CampaignType type, CampaignType Othertype);

        /// <summary>
        /// use this service operation to get Campaign Settings Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Id to Query By</param>
        /// <returns>CampaignSettingsDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignSettingsDto GetSettings(int campaignId);

        /// <summary>
        /// use this service operation to save Campaign Settings 
        /// </summary>
        /// <param name="settingsDto">settings to save</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveSettings(CampaignSettingsDto settingsDto);

        /// <summary>
        /// use this service operation to remove Campaign Discount
        /// </summary>
        /// <param name="campaignId">campaign Id</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool RemoveDiscount(int campaignId);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Run Campaign")]
        void Run(int[] campaignIds);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause Campaign")]
        void Pause(int[] campaignIds);

        /// <summary>
        /// Get all account campaigns.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetCampaignsTree();

        /// <summary>
        /// use this service operation to get Clone Campaign
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy Campaign")]
        ResponseDto CloneCampaign(int campaignId, string name);

        /// <summary>
        /// Get Campaign Server Settings
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignServerSettingDto GetServerSettings(int Id);

        /// <summary>
        /// use this service operation to save Campaign Server Settings 
        /// </summary>
        /// <param name="settingDto"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveServerSetting(CampaignServerSettingDto settingDto);

        /// <summary>
        /// Delete frequencycapping event from this campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="frequencyCappingId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteFrequencyCapping(int campaignId, int frequencyCappingId);

        /// <summary>
        /// Add frequencycapping event for this campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="frequencyCappingId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveFrequencyCapping(int campaignId, CampaignFrequencyCappingSaveDto frequencyCappingSave);

        /// <summary>
        /// Save Campaign Assign Appsites
        /// </summary>
        /// <param name="campaignAssignedAppsites"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Campaign Assign Appsites")]
        void SaveCampaignAssignAppsites(CampaignAssignedAppsitesSaveDTo campaignAssignedAppsites);

        /// <summary>
        /// Save Campaign Bid Config
        /// </summary>
        /// <param name="campaignBidConfigSaveDTo"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Campaign Bid Config")]
        void SaveCampaignBidConfig(CampaignBidConfigSaveDTo campaignBidConfigSaveDTo);
        /// <summary>
        /// Get Campaign Assign Appsites
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignAssignedAppsitesModelDto GetCampaignAssignAppsites(int campaignId);
        /// <summary>
        /// Get Campaign Bid Configs
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignBidConfigModelDto GetCampaignBidConfigs(int campaignId, int adGroupID);

        /// <summary>
        /// Rename a gruop 
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="gruipId"></param>
        /// <param name="name"></param>
        /// <returns>string</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string RenameGroup(int campaignId, int GroupId, string name);

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
        [FaultContract(typeof(ServiceFault))]
        bool CheckAppsitesCostModelCompatableWitCampaign(int campaignId, out IList<CampaignBidConfigDto> notCompatibleAppSiteList, List<int> appsites = null, int? goupId = null, int? groupCostModelWrapperID = null, bool checkExisting = false);


        /// <summary>
        /// use this service operation to Insert/Update Campaign Object
        /// </summary>
        /// <param name="campaign">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Campaign Info Settings Save")]
        CampaignSaveDto SaveCampInfoSettings(CampaignAllDto oCampaignAllDto);
        #endregion

        #region Ad Groups
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PMPDealDto> GetPMPDealConfigConfigs(int campaignId, int adGroupID);
        /// <summary>
        /// use this service operation to get list of AdGroups Object depend on the criteria
        /// </summary>
        /// <returns>AdGroupSearchDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupSearchDto QueryGroupsByCratiria(AdGroupCriteria criteria);

        /// <summary>
        /// use this service operation to Delete List of AdGroups using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupIds">List of AdGroup Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete AdGroup")]
        bool DeleteGroups(int campaignId, int[] adGroupIds);

        /// <summary>
        /// use this service operation to run list of Ad Groups Object depend on the Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Run AdGroup")]
        void RunGroups(int campaignId, int[] adGroupIds);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause AdGroup")]
        void PauseGroups(int campaignId, int[] adGroupIds);

        /// <summary>
        /// use this service operation to Insert/Update AdGroup Object
        /// </summary>
        /// <param name="adGroup">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int SaveAdGroup(AdGroupDto adGroup, bool returnId);

        /// <summary>
        /// use this service operation to get list of AdGroups Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupId">Id to Get By</param>
        /// <returns>AdGroupListDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupDto GetAdGroup(int campaignId, int adGroupId);

        /// <summary>
        /// use this service operation to get list of AdGroups  Targeting Objects depend on the AdGroupId
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <param name="adGroupId">AdGroup Id to Query By</param>
        /// <returns>TargetingListDto  that Hold List of TargetingBaseDto match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
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
        TargetingListDto GetTargeting(int campaignId, int adGroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Targeting")]
        TargetingResultDto SaveTargeting(TargetingSaveDto targetingSaveDto);


        /// <summary>
        /// use this service operation to get the minimum Bid
        /// </summary>
        /// <returns>minimum Bid depend on the info</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ReturnBidDto GetMinBid(BidDto info);

        /// <summary>
        /// Get all account adgroups per campaign
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdGroupsTree();

        /// <summary>
        /// use this service operation to get Clone AdGroup
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy AdGroup")]
        ResponseDto CloneAdGroup(int campaignId, int groupId, string name);


        #region Cost Elements
        /// <summary>
        /// use this service operation to get AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupCostElementResultDto GetAdGroupCostElements(AdGroupCostElementCriteria criteria);

        /// <summary>
        /// use this service operation to get AdGrouyp Cost Element by Id
        /// </summary>
        /// <returns>adGroup Cost Element</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupCostElementDto GetAdGroupCostElement(int campaignId, int groupId, int costElemtnId);

        /// <summary>
        /// use this service operation to add AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void AddCostElements(AdGroupCostElementSaveDto saveDto);

        /// <summary>
        /// use this service operation to update AdGrouyp Cost Elements
        /// </summary>
        /// <returns>adGroup Cost Elements</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void UpdateCostElements(AdGroupCostElementSaveDto saveDto);

        /// <summary>
        /// use this service operation to remove Cost Elements from AdGrouyp
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveCostElements(AdGroupCostElementSaveDto saveDto);
        #endregion

        /// <summary>
        /// use this service operation to get AdGroup Settings Object depend on the Id
        /// </summary>
        /// <param name="adGroupId">Id to Query By</param>
        /// <param name="campaignId">campaign Id to Query By</param>
        /// <returns>CampaignSettingsDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupSettingsDto GetAdGroupSettings(int campaignId, int adGroupId);

        /// <summary>
        /// use this service operation to save AdGroup Settings 
        /// </summary>
        /// <param name="settingsDto">settings to save</param>
        /// <returns>true is the save operation is successfully</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveAdGroupSettings(AdGroupSettingsDto settingsDto);

        /// <summary>
        /// Get TrackingEvents for the adgroup, get the default TrackingEvents if there is no trackingevents
        /// </summary>
        /// <param name="adGroupId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupTrackingEventResultDto GetAdGroupTrackingEvents(AdGroupTrackingEventCriteriaDto criteria);


        /// <summary>
        /// Get TrackingEvents for the Account, in union with all the system events 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupTrackingEventResultDto GetAccountTrackingEvents();


        /// <summary>
        /// Delete tracking event for this adgroup
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="adGroupId">adGroupId</param>
        /// <param name="adGroupTrackingEventId">adgroup tracking event id</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteTrackingEvent(int campaignId, int adGroupId, int adGroupTrackingEventId);

        /// <summary>
        /// Get AdGroup Tracking Events prerequisites List
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="adGroupId">adGroupId</param>
        /// <param name="costModelWrapperId">costModelWrapperId</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdGroupTrackingEventDto> GetCostModelWrapperTrackingEvents(int campaignId, int adGroupId, int costModelWrapperId);

        /// <summary>
        /// Check if this tracking event could be deleted or not
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="adGroupId"></param>
        /// <param name="adGroupTrackingEventCodes"></param>
        /// <param name="checkStandards"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        KeyValuePair<bool, string> IsDeleteTrackingEventAllowed(int campaignId, int adGroupId, List<string> adGroupTrackingEventCodes, bool checkStandards, int? newCostModelWrapperId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adGroupId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool CheckEventUniqueByCode(string code);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool checkSystemEventFraud(string code, string name);

  

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsAllowedGroupById(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsAllowedAdById(int id);

        #endregion

        #region Ads

        /// <summary>
        /// use this service operation to get list of Ads Object depend on the criteria
        /// </summary>
        /// <returns>AdsSearchDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdsSearchDto QueryAdsByCratiria(AdsCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdTypeIds GetAddTypesByAddGroupAction(int adGroupId);
        /// <summary>
        /// use this service operation to get list of Ads Object that has bid less than certain value
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="bid">the bid value to check</param>
        /// <returns>List of AdbIDListDto that has bid less than</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<AdbIDListDto> QueryAdsLessBid(int campaignId, int adGroupId, decimal bid);

        /// <summary>
        /// use this service operation to update Bid for List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <param name="bid">new Bid value </param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SetAdsBid(int campaignId, int groupId, int[] adIds, decimal bid);

        /// <summary>
        /// use this service operation to get Ad Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreativeId">Id to Get By</param>
        /// <returns>AdCreativeDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdCreativeDto GetAdCreative(int campaignId, int adGroupId, int? adCreativeId, int? adType);

        /// <summary>
        /// use this service operation to Insert/Update Ad Creative Object
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreative">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Save Ad")]
        int SaveAd(int campaignId, int adGroupId, AdCreativeSaveDto adCreative);

        /// <summary>
        /// use this service operation to Delete List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete Ad")]
        bool DeleteAds(int campaignId, int groupId, int[] adIds);

        /// <summary>
        /// use this service operation to run list of Ad Groups Object depend on the Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Run Ad")]
        void RunAds(int campaignId, int groupId, int[] adIds);

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Pause Ad")]
        void PauseAds(int campaignId, int groupId, int[] adIds);

        /// <summary>
        /// use this service operation to Reject Ad
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adId">Id to Get Ad  By</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RejectAd(int campaignId, int groupId, int adId);

        /// <summary>
        /// use this service operation to Approve Ad
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void ApproveAd(ApproveAdDto approveAdDto);


        /// <summary>
        /// Get all account ads per campaign
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdsTree();

        /// <summary>
        /// Get all account ads per campaign and execlude specific AdId if it's no null
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAdsTreeByAccount")]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<TreeDto> GetAdsTree(int accountId, int? adId);

        /// <summary>
        /// Get all unapproved ads from the adgroup of this ad
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<AdCreativeDtoBase> GetUnApprovedAdsFromAdGroupOfAd(int adId);

        /// <summary>
        /// Get Ad Creative Summary
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdCreativeSummaryDto GetAdSummary(int campaignId, int groupId, int adId);

        /// <summary>
        /// Get Ad Creative Full Summary
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdCreativeFullSummaryDto GetAdFullSummary(int campaignId, int groupId, int adId);


        /// <summary>
        /// use this service operation to get list of Campaign Summary Objects depend on the criteria
        /// </summary>
        /// <returns>CampaignSummaryDtos that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<CampaignSummaryDto> GetAdsSummary(AdsSummaryCriteria criteria);


        /// <summary>
        /// use this service operation to get Clone Ad
        /// </summary>
        /// <returns>message if Clone Operation is successfully , else string.Empty</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Copy Ad")]
        ResponseDto CloneAd(int campaignId, int groupId, int adId, string name);

        /// <summary>
        /// use this service operation to Format Ad Creative Content
        /// </summary>
        /// <returns>Formatted Ad Creative Content</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        FormattedContentDto FormatAdCreativeContent(string content, int creativeId =0 );

        /// <summary>
        /// use this service operation to check if Ad Creative Content is formatted
        /// </summary>
        /// <returns>true if Ad Creative Content is Formatted </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsFormattedAdCreativeContent(string content);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<int> GetAppSiteAdQueues(int campaignId, int adGoupId, int adId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        InventorySourceModelDto GetInventorySources(int campaignId, int adGroupID);
        #endregion
        #region AdRequest

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        TypesPlatformsVersions GetAdRequestTypes_Platforms_Versions();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdRequestTargetingDtoResultDto GetAdRequestTargetings(AdRequestCriteria Criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveAdRequestTargeting(AdRequestTargetingDto dto);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteAdRequestTargeting(int Id);

        #endregion


        #region ImpressionMetricTargetings

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ImpressionMetricTargetingDto GetImpressionMetricTargeting(int TargetingId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ImpressionMetricTargetingResultDto GetImpressionMetricTargetings(ImpressionMetricCriteria Criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveImpressionMetricTargeting(ImpressionMetricTargetingDto dto);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteImpressionMetricTargeting(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<ImpressionMetricDto> GetImpressionMetrics();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ImpressionMetricDto GetImpressionMetric(int Id);

        #endregion


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        int GetCampaignAdvertiserAccount(int campaignId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetCreativeVendorForAdCreativeUnit(int AdCreativeUnitId);

        /*[OperationContract]
        [FaultContract(typeof(ServiceFault))]
        XDocument downloadXml(string url);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsValidXml(string xmlString, string XsdsFolderPath);*/


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<VideoMediaFileDto> GetVideoMediaFiles(int videoAdId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void AddUpdateVideoMediaFile(VideoMediaFileDto dto);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<AdCreativeDto> GetDraftVideoAd();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void PublishVideoAd(int videoAdint);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetMIMEType(string code);



        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<VideoConversionCreativeUnitDto> GetVideoConversionCreativeUnits(string code);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AudicanceBillSummary DeserializeRule(string groupJson);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdGroupDto> GetAllAdGroupByAccount(int AccountId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UploadTest(string fullDirectory, byte[] content);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetDirectory(int index);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<MetricVendorDto> getMetricVendors();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetAdvertiserString(int AdvertiserId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetCampaignAdvertiser(int campaignId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetCampaignsAdvTree(int? AdvertiserId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdsAdvTree(int? AdvertiserId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdGroupsAdvTree(int? AdvertiserId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetAdvertiserIdByCampaignId(int id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetAccountAdvertiserId(int advertiserId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetAdvertiserAccountString(int AdvertiserId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetAdvertiserIdFromAccount(int AdvertiserId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetAdvertiserAccountIdByCampaignId(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsReadOrWriteCampaign(int CampaignId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsWriteCampaign(int CampaignId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetPublisherCounterCurrentWeek();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupDealAndSourceDTO GetDealsAndSources(int campaignId, int adGroupID);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        TargetingResultDto AddExternalAudSegmentTargeting(int adgroupId, int IdAccAdv, int dpId, List<AudienceSegmentDto> Segments, string group);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string SaveSegmentsForTargeting(int adgroupId, int IdAccAdv, int dpId, List<AudienceSegmentDto> Segments);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string getAudiancesUsedInIntegration(int adgroupId, int IdAccAdv, int dpId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountMasterAppSiteDto> GetMasterListConfigConfigs(int campaignId, int adGroupID);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetAudienceListCounter(string dpName);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignSettingsDto GetCampSettings(int campaignId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveCampSettings(CampaignSettingsDto settingsDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int getCountAudiancesUsedInIntegration(int adgroupId, int IdAccAdv, int dpId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string getDataBidAudiancesUsedInIntegration(int adgroupId, int IdAccAdv, int dpId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int getCountAudiancesUsedInIntegrationAll(int adgroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string getDataBidAudiancesUsedInIntegrationAll(int adgroupId);



        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void UpdateDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void AddDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupDynamicBiddingConfigDto GetAdGroupDynamicBiddingConfig(int campaignId, int groupId, int configId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupDynamicBiddingConfigResultDto GetAdGroupDynamicBiddingConfigs(AdGroupCostElementCriteria criteria);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void UpdateAdGroupTrackingEvent(AdGroupTrackingEventSaveDto trackingEventSaveDto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdGroupConversionEventResultDto GetAccountConversionEvents();


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void AddDefaultAdGroupTrackingEventById(int adGroupId, int costModelWrapperId, int? oldCostModelWrapper);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void PublishTargetingEvent(TargetingResultDto eventino);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string SaveSegmentsForTargetingForDel(int adgroupId, int IdAccAdv, int dpId, List<AudienceSegmentDto> Segments);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string getAudiancesUsedInIntegrationActive(int adgroupId, int IdAccAdv, int dpId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DoesContainDataProviderAllowImpressionTracker(int campaignId, int groupId);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetActionTypeByadGroup(int adGroupId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<AdbIDListDto> QueryAdsMoreBid(int campaignId, int adGroupId, decimal bid);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsFormatedAdCreativUnit(string content);


    }
}
