using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.HouseAd;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class TargetingSaveModel
    {
        private IEnumerable<AdGroupTrackingEventSaveDto> addedTrackingEvents;

        public BidGetModel bidGetModel { get; set; }
        public bool AllowInclude { get; set; }
        public bool AdPosition_Unknown { get; set; }

        public int? ViewabilityVendorId { get; set; }
        public bool AdPosition_AboveTheFold { get; set; }

        public bool AdPosition_BelowTheFold { get; set; }
        public bool AllowBidToBeZero { get; set; }
        public bool AdPosition_Enabled { get; set; }
        public string changedAudiances { get; set; }
        public string changedContextuals { get; set; }
        public string changedBrandSafety { get; set; }
        public int AdGroupId { get; set; }
        public int CampaignId { get; set; }
        public int AgeGroupId { get; set; }
        public int Gender { get; set; }
        public int CostModelWrapper { get; set; }
        public decimal Bid { get; set; }
        public decimal DataBid { get; set; }
        public decimal? AudianceDiscountPrice { get; set; }
        public bool TrackInstalls { get; set; }
        public int? TargetingConnectionType { get; set; }
        public bool OpenInExternalBrowser { get; set; }
        public bool AllowOpenAuction { get; set; }
        public string[] NewKeywords { get; set; }
        public bool ExcludeSensitiveCategories { get; set; }

        public int[] DeletedKeywords { get; set; }
        public string Operators { get; set; }
        public string Geographies { get; set; }
        public int[] ModelId { get; set; }
        public string Manufacturers { get; set; }
        public string Platforms { get; set; }
        public string[] PlatformVersions { get; set; }
        public string Devices { get; set; }
        public string Models { get; set; }
        public int DeviceTargetingTypeId { get; set; }
        public int GeographicTargetingTypeId { get; set; }
        public int OperaterTargetingTypeId { get; set; }
        public int GeographicTargetingIsAll { get; set; }
        public int OperatorTargetingIsAll { get; set; }
        public bool DisableProxyTraffic { get; set; }
        public bool IsWifi { get; set; }
        public bool IsCellular { get; set; }
        public int[] DeviceType { get; set; }
        public int[] LanguageType { get; set; }
        public string DeviceCapabilities { get; set; }
        public string ExcludeDeviceCapability { get; set; }
        public decimal? Budget { get; set; }
        public decimal? DailyBudget { get; set; }

        public string DeletedIPRanges { get; set; }
        public string InsertedIPRanges { get; set; }

        public string DeletedGeoFencing { get; set; }
        public string InsertedGeoFencing { get; set; }

        public string DeletedURLTargeting { get; set; }
        public string InsertedURLTargeting { get; set; }

        public string InsertedTrackingEvents { get; set; }
        public string DeletedTrackingEvents { get; set; }
        public string DeletedTrackingCodeEvents { get; set; }
        public string InsertedCostElements { get; set; }
        public string DeletedCostElements { get; set; }
        public string UpdatedCostElements { get; set; }
        public IEnumerable<CampaignBidConfigDto> AllBidConfigItems { get; set; }
        public string InserteCampaignBidConfigs { get; set; }
        public string SSPCheckedString { get; set; }

        public string UpdatedCampaignBidConfiges { get; set; }

        public string UpdatedNotCompatableCampaignBidConfiges { get; set; }


        public string DeletedCampaignBidConfigs { get; set; }
        /*
        public IList<IPTargetingDto> DeletedIPRanges { get; set; }
        public IList<IPTargetingDto> InsertedIPRanges { get; set; }
        */
        public string InsertePMPDealConfigs { get; set; }
        public string DeletedPMPDealConfigs { get; set; }

        public IEnumerable<AdvertiserAccountMasterAppSiteDto> MasterListConfigList { get; set; }
        public IEnumerable<AdGroupTrackingEventSaveDto> AddedTrackingEvents { get => addedTrackingEvents; set => addedTrackingEvents = value; }
        public string DeletedMasterListConfigs { get; set; }
        public string InserteMasterListConfigs { get; set; }
        public string platfromTree { get; set; }

        public group group { get; set; }


        public string groupAudianceString { get; set; }
        public string groupContextualString { get; set; }
        public string groupBrandSafetyString { get; set; }


        public string InsertedInventorySources { get; set; }

        public string DeletedInventorySources { get; set; }


        public string UpdatedInventorySources { get; set; }


        public int[] CheckedSSP { get; set; }
        public bool RunTypeInventory { get; set; }

        public int[] SelectedInventory { get; set; }
        public int[] IncludedInventory { get; set; }
        public int[] SelectedDeals { get; set; }


        public string Runtype { get; set; }




        public int BiddingStrategy { get; set; }
        #region video targeting




        public bool PlacementType_InStream { get; set; }
        public bool PlacementType_OutStream { get; set; }
        public bool PlacementType_Interstitial { get; set; }
        public bool PlacementType_Undetermined { get; set; }



        public bool InStreamPosition_PreRoll { get; set; }
        public bool InStreamPosition_MidRoll { get; set; }
        public bool InStreamPosition_PostRoll { get; set; }

        public bool InStreamPosition_Undetermined { get; set; }


        public bool SkippableAds_SkippableAdSpaces { get; set; }
        public bool SkippableAds_NonSkippableAdSpaces { get; set; }

        public bool SkippableAds_Undetermined { get; set; }

        public bool Playback_AutoPlaySoundOn { get; set; }

        public bool Playback_AutoPlaySoundOff { get; set; }
        public bool Playback_ClickToPlay { get; set; }
        public bool RewardedAdOnly { get; set; }
        public bool Playback_Undetermined { get; set; }
        public bool Video_RewardedAds { get; set; }
        public bool Video_MatchOrientation { get; set; }
        #endregion
        public IEnumerable<AdGroupFeeDto> FeesAddList { get; set; }



        public decimal BidOptimizationValue { get; set; }



        public decimal MaxBidPrice { get; set; }




        public bool KeepBiddingAtMinimum { get; set; }


        public BidOptimizationType BidOptimizationType { get; set; }

        public ConversionSetting ConversionSetting { get; set; }

        public ConversionType ConversionType { get; set; }

        public int ViewAttribuation { get; set; }

        public int ClickAttribuation { get; set; }

        public int CountingAttribuation { get; set; }

        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }


        public int ConversionSettingInt { get; set; }

        public int ConversionTypeInt { get; set; }
        public int CountingTypeAttribuationInt { get; set; }


        #region Conversion and events


        public IList<AdGroupConversionEventDto> ConversionItems { get; set; }


        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; }

        #endregion



        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; }



        public int[] SelectedMasterListConfigs { get; set; }
        public IList<IPTargetingDto> AllIPRanges { get; set; }
        public IList<GeoFencingUITargeting> AllGeoFencing { get; set; }


        public IList<URLTargetingDto> AllURLs { get; set; }
        public IList<int> AllKeywords { get; set; }
        public bool? IsHouseAd { get; set; } = false;
        public HouseAdSaveModel HouseAdSave { get; set; }
    }
    public class BidGetModel
    {
        public int ActionTypeId { get; set; }
        public int? AdTypeId { get; set; }
        public string[] Keywords { get; set; }
        public string Operators { get; set; }
        public string Geographies { get; set; }
        public string Models { get; set; }
        public string Manufacturers { get; set; }
        public string Platforms { get; set;}
        public string Devices { get; set;}
        public int DeviceTargetingTypeId { get; set;}
        public int? Demographic { get; set; }
        public string DeviceCapabilities { get; set;}
        public string ExcludeDeviceCapability { get; set; }
      


    }

    public class ExternalAudianceSegmentTargetingModel {

        public int AdGroupId { get; set; }
        public int DataProviderId { get; set; }
        public int AccAdvertiserId { get; set;  }
        public List<AudienceSegmentDto> Segments { get; set; }


       



    }
}


