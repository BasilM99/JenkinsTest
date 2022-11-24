using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class TargetingSaveDto
    {

        [DataMember]
        public bool ExcludeSensitiveCategories { get; set; }

        [DataMember]
        public bool AllowInclude { get; set; }

        [DataMember]
        public int? ViewabilityVendorId { get; set; }
        [DataMember]
        public string changedAudiances { get; set; }
        [DataMember]
        public string Runtype { get; set; }
        [DataMember]
        public string groupAudianceString { get; set; }
        [DataMember]
        public bool AllowOpenAuction { get; set; }

        [DataMember]
        public int? TargetingConnectionType { get; set; }
        
        [DataMember]
        public group group { get; set; }
        [DataMember]
        public bool isFromHouseAd { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public decimal? AudianceDiscountPrice { get; set; }
        [DataMember]
        public decimal Bid { get; set; }
        [DataMember]
        public bool TrackInstalls { get; set; }
        [DataMember]
        public bool OpenInExternalBrowser { get; set; }
        [DataMember]
        public int CostModelWrapper { get; set; }
        [DataMember]
        public string[] NewKeywords { get; set; }
        [DataMember]
        public int[] DeletedKeywords { get; set; }
        [DataMember]
        public int[] Operators { get; set; }
        [DataMember]
        public int OperatorTargetingIsAll { get; set; }
        [DataMember]
        public int[] Geographies { get; set; }
        [DataMember]
        public IList<int> Manufacturers { get; set; }
        [DataMember]
        public IDictionary<int, string> Platforms { get; set; }
        [DataMember]
        public int[] Models { get; set; }
        [DataMember]
        public int DeviceTargetingTypeId { get; set; }
        [DataMember]
        public int AgeGroupId { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public BidDto BinInfo { get; set; }
        [DataMember]
        public int[] DeviceCapabilities { get; set; }
        [DataMember]
        public int[] ExcludeDeviceCapability { get; set; }
        [DataMember]
        public bool DisableProxyTraffic { get; set; }
        [DataMember]
        public bool IsWifi { get; set; }
        [DataMember]
        public bool IsCellular { get; set; }
        [DataMember]
        public int[] DeviceTypeIds { get; set; }

        [DataMember]
        public int[] LanguagesIds { get; set; }

        [DataMember]
        public IList<IPTargetingDto> InsertedIPRanges { get; set; }
        [DataMember]
        public IList<int> DeletedIPRanges { get; set; }

        [DataMember]
        public IList<GeoFencingTargetingDto> InsertedGeoFencings { get; set; }
        [DataMember]
        public IList<int> DeletedGeoFencings { get; set; }


        [DataMember]
        public IList<URLTargetingDto> InsertedURLTargeting { get; set; }
        [DataMember]
        public IList<int> DeletedURLTargeting { get; set; }


        [DataMember]
        public IList<AdGroupTrackingEventSaveDto> InsertedTrackingEvents { get; set; }
        [DataMember]
        public IList<int> DeletedTrackingEvents { get; set; }
        [DataMember]
        public IList<string> DeletedTrackingCodeEvents { get; set; }
        [DataMember]
        public IList<AdGroupCostElementSaveDto> InsertedCostElements { get; set; }
        [DataMember]
        public IList<int> DeletedCostElements { get; set; }

        [DataMember]
        public Dictionary<int, decimal> UpdatedCostElements { get; set; }

        [DataMember]
        public IEnumerable<CampaignBidConfigDto> InsertedBidConfigItems { get; set; }

        [DataMember]
        public IEnumerable<CampaignBidConfigDto> UpdatedBidConfigItems { get; set; }

        [DataMember]
        public IEnumerable<CampaignBidConfigDto> UpdatedNotCompatableCampaignBidConfiges { get; set; }
        [DataMember]
        public IEnumerable<AdGroupFeeDto> FeesAddList { get; set; }

        
        [DataMember]
        public IList<int> DeletedCampaignBidConfigs { get; set; }
        [DataMember]
        public IList<PlatfromTree> platfromTree { get; set; }

        [DataMember]
        public string DeletedPMPDealConfigs { get; set; }
        [DataMember]
        public string InsertePMPDealConfigs { get; set; }
        [DataMember]
        public string DeletedMasterListConfigs { get; set; }
        [DataMember]
        public string InserteMasterListConfigs { get; set; }

        [DataMember]
        public IEnumerable<InventorySourceDto> InsertedInventorySourceItems { get; set; }

        [DataMember]
        public IEnumerable<InventorySourceDto> UpdatedInventorySourceItems { get; set; }

        [DataMember]
        public IEnumerable<InventorySourceDto> UpdatedNotCompatableCampaignInventorySource { get; set; }
        [DataMember]
        public IList<int> DeletedInventoryItemsSources { get; set; }

        [DataMember]
        public IList<int> CheckedSSP { get; set; }

        
                [DataMember]
        public string SSPCheckedString { get; set; }
        [DataMember]
        public string InsertedInventorySources { get; set; }
        [DataMember]
        public string DeletedInventorySources { get; set; }

        [DataMember]
        public string UpdatedInventorySources { get; set; }

        #region Video Targeting
        [DataMember]
        public bool PlacementType_InStream { get; set; }
        [DataMember]
        public bool PlacementType_OutStream { get; set; }
        [DataMember]
        public bool PlacementType_Interstitial { get; set; }
        [DataMember]
        public bool PlacementType_Undetermined { get; set; }


        [DataMember]
        public bool InStreamPosition_PreRoll { get; set; }

        [DataMember]
        public bool InStreamPosition_MidRoll { get; set; }
        [DataMember]
        public bool InStreamPosition_PostRoll { get; set;
        }
        [DataMember]
        public bool InStreamPosition_Undetermined { get; set; }

        [DataMember]
        public bool SkippableAds_SkippableAdSpaces { get; set; }
        [DataMember]
        public bool SkippableAds_NonSkippableAdSpaces { get; set; }
        [DataMember]
        public bool SkippableAds_Undetermined { get; set; }
        [DataMember]
        public bool Playback_AutoPlaySoundOn { get; set; }
        [DataMember]
        public bool Playback_AutoPlaySoundOff { get; set; }
        [DataMember]
        public bool Playback_ClickToPlay { get; set; }
        [DataMember]
        public bool Playback_Undetermined { get; set; }
        [DataMember]
        public bool Video_RewardedAds { get; set; }
        [DataMember]
        public bool Video_MatchOrientation { get; set; }
        [DataMember]
        public bool RewardedAdOnly { get; set; }

        #endregion

        [DataMember]
        public decimal? DailyBudget { get; set; }

        [DataMember]
        public decimal? Budget { get; set; }


        [DataMember]
        public bool AdPosition_Unknown { get; set; }

        [DataMember]
        public bool AdPosition_AboveTheFold { get; set; }

        [DataMember]
        public bool AdPosition_BelowTheFold { get; set; }
        [DataMember]
        public bool AdPosition_Enabled { get; set; }

        [DataMember]
        public BiddingStrategy BiddingStrategy { get; set; }




        [DataMember]
        public decimal BidOptimizationValue { get; set; }


        [DataMember]
        public decimal MaxBidPrice { get; set; }



        [DataMember]
        public bool KeepBiddingAtMinimum { get; set; }
        [DataMember]

        public BidOptimizationType BidOptimizationType { get; set; }



        [DataMember]
        public ConversionSetting ConversionSetting { get; set; }
        [DataMember]
        public ConversionType ConversionType { get; set; }

        [DataMember]
        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }


        [DataMember]
        public int ViewAttribuation { get; set; }
        [DataMember]
        public int ClickAttribuation { get; set; }
        [DataMember]
        public int CountingAttribuation { get; set; }

        #region Conversion and events

        [DataMember]
        public IList<AdGroupConversionEventDto> ConversionItems { get; set; }


        [DataMember]
        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; }

        #endregion
    }


}
