using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class TargetingSaveDto
    {

       [ProtoMember(1)]
        public bool ExcludeSensitiveCategories { get; set; }

       [ProtoMember(2)]
        public bool AllowInclude { get; set; }

       [ProtoMember(3)]
        public int? ViewabilityVendorId { get; set; }
       [ProtoMember(4)]
        public string changedAudiances { get; set; }
       [ProtoMember(5)]
        public string Runtype { get; set; }
       [ProtoMember(6)]
        public string groupAudianceString { get; set; }
       [ProtoMember(7)]
        public bool AllowOpenAuction { get; set; }

       [ProtoMember(8)]
        public int? TargetingConnectionType { get; set; }
        
       [ProtoMember(9)]
        public group group { get; set; }
       [ProtoMember(10)]
        public bool isFromHouseAd { get; set; }
       [ProtoMember(11)]
        public int AdGroupId { get; set; }
       [ProtoMember(12)]
        public int CampaignId { get; set; }
       [ProtoMember(13)]
        public decimal? AudianceDiscountPrice { get; set; }
       [ProtoMember(14)]
        public decimal Bid { get; set; }
       [ProtoMember(15)]
        public bool TrackInstalls { get; set; }
       [ProtoMember(16)]
        public bool OpenInExternalBrowser { get; set; }
       [ProtoMember(17)]
        public int CostModelWrapper { get; set; }
       [ProtoMember(18)]
        public string[] NewKeywords { get; set; }
       [ProtoMember(19)]
        public int[] DeletedKeywords { get; set; }
       [ProtoMember(20)]
        public int[] Operators { get; set; }
       [ProtoMember(21)]
        public int OperatorTargetingIsAll { get; set; }
       [ProtoMember(22)]
        public int[] Geographies { get; set; }
       [ProtoMember(23)]
        public IList<int> Manufacturers { get; set; } = new List<int>();
        [ProtoMember(24)]
        public IDictionary<int, string> Platforms { get; set; } = new Dictionary<int, string>();
       [ProtoMember(25)]
        public int[] Models { get; set; }
       [ProtoMember(26)]
        public int DeviceTargetingTypeId { get; set; }
       [ProtoMember(27)]
        public int AgeGroupId { get; set; }
       [ProtoMember(28)]
        public int Gender { get; set; }
       [ProtoMember(29)]
        public BidDto BinInfo { get; set; }
       [ProtoMember(30)]
        public int[] DeviceCapabilities { get; set; }
       [ProtoMember(31)]
        public int[] ExcludeDeviceCapability { get; set; }
       [ProtoMember(32)]
        public bool DisableProxyTraffic { get; set; }
       [ProtoMember(33)]
        public bool IsWifi { get; set; }
       [ProtoMember(34)]
        public bool IsCellular { get; set; }
       [ProtoMember(35)]
        public int[] DeviceTypeIds { get; set; }

       [ProtoMember(36)]
        public int[] LanguagesIds { get; set; }

       [ProtoMember(37)]
        public IList<IPTargetingDto> InsertedIPRanges { get; set; } = new List<IPTargetingDto>();
        [ProtoMember(38)]
        public IList<int> DeletedIPRanges { get; set; } = new List<int>();

        [ProtoMember(39)]
        public IList<GeoFencingTargetingDto> InsertedGeoFencings { get; set; } = new List<GeoFencingTargetingDto>();
        [ProtoMember(40)]
        public IList<int> DeletedGeoFencings { get; set; }


       [ProtoMember(41)]
        public IList<URLTargetingDto> InsertedURLTargeting { get; set; } = new List<URLTargetingDto>();
        [ProtoMember(42)]
        public IList<int> DeletedURLTargeting { get; set; } = new List<int>();


        [ProtoMember(43)]
        public IList<AdGroupTrackingEventSaveDto> InsertedTrackingEvents { get; set; } = new List<AdGroupTrackingEventSaveDto>();
        [ProtoMember(44)]
        public IList<int> DeletedTrackingEvents { get; set; } = new List<int>();
       [ProtoMember(45)]
        public IList<string> DeletedTrackingCodeEvents { get; set; } = new List<string>();
        [ProtoMember(46)]
        public IList<AdGroupCostElementSaveDto> InsertedCostElements { get; set; } = new List<AdGroupCostElementSaveDto>();
        [ProtoMember(47)]
        public IList<int> DeletedCostElements { get; set; }

        [ProtoMember(48)]
        public Dictionary<int, decimal> UpdatedCostElements { get; set; } = new Dictionary<int, decimal>();

        [ProtoMember(49)]
        public IEnumerable<CampaignBidConfigDto> InsertedBidConfigItems { get; set; } = new List<CampaignBidConfigDto>();

       [ProtoMember(50)]
        public IEnumerable<CampaignBidConfigDto> UpdatedBidConfigItems { get; set; } = new List<CampaignBidConfigDto>();

        [ProtoMember(51)]
        public IEnumerable<CampaignBidConfigDto> UpdatedNotCompatableCampaignBidConfiges { get; set; } = new List<CampaignBidConfigDto>();
        [ProtoMember(52)]
        public IEnumerable<AdGroupFeeDto> FeesAddList { get; set; } = new List<AdGroupFeeDto>();


        [ProtoMember(53)]
        public IList<int> DeletedCampaignBidConfigs { get; set; } = new List<int>();
        [ProtoMember(54)]
        public IList<PlatfromTree> platfromTree { get; set; } = new List<PlatfromTree>();

       [ProtoMember(55)]
        public string DeletedPMPDealConfigs { get; set; }
       [ProtoMember(56)]
        public string InsertePMPDealConfigs { get; set; }
       [ProtoMember(57)]
        public string DeletedMasterListConfigs { get; set; }
       [ProtoMember(58)]
        public string InserteMasterListConfigs { get; set; }

       [ProtoMember(59)]
        public IEnumerable<InventorySourceDto> InsertedInventorySourceItems { get; set; } = new List<InventorySourceDto>();

        [ProtoMember(60)]
        public IEnumerable<InventorySourceDto> UpdatedInventorySourceItems { get; set; } = new List<InventorySourceDto>();

        [ProtoMember(61)]
        public IEnumerable<InventorySourceDto> UpdatedNotCompatableCampaignInventorySource { get; set; } = new List<InventorySourceDto>();
        [ProtoMember(62)]
        public IList<int> DeletedInventoryItemsSources { get; set; } = new List<int>();

        [ProtoMember(63)]
        public IList<int> CheckedSSP { get; set; } = new List<int>();


        [ProtoMember(64)]
        public string SSPCheckedString { get; set; }
       [ProtoMember(65)]
        public string InsertedInventorySources { get; set; }
       [ProtoMember(66)]
        public string DeletedInventorySources { get; set; }

       [ProtoMember(67)]
        public string UpdatedInventorySources { get; set; }

        #region Video Targeting
       [ProtoMember(68)]
        public bool PlacementType_InStream { get; set; }
       [ProtoMember(69)]
        public bool PlacementType_OutStream { get; set; }
       [ProtoMember(70)]
        public bool PlacementType_Interstitial { get; set; }
       [ProtoMember(71)]
        public bool PlacementType_Undetermined { get; set; }


       [ProtoMember(72)]
        public bool InStreamPosition_PreRoll { get; set; }

       [ProtoMember(73)]
        public bool InStreamPosition_MidRoll { get; set; }
       [ProtoMember(74)]
        public bool InStreamPosition_PostRoll { get; set;
        }
       [ProtoMember(75)]
        public bool InStreamPosition_Undetermined { get; set; }

       [ProtoMember(76)]
        public bool SkippableAds_SkippableAdSpaces { get; set; }
       [ProtoMember(77)]
        public bool SkippableAds_NonSkippableAdSpaces { get; set; }
       [ProtoMember(78)]
        public bool SkippableAds_Undetermined { get; set; }
       [ProtoMember(79)]
        public bool Playback_AutoPlaySoundOn { get; set; }
       [ProtoMember(80)]
        public bool Playback_AutoPlaySoundOff { get; set; }
       [ProtoMember(81)]
        public bool Playback_ClickToPlay { get; set; }
       [ProtoMember(82)]
        public bool Playback_Undetermined { get; set; }
       [ProtoMember(83)]
        public bool Video_RewardedAds { get; set; }
       [ProtoMember(84)]
        public bool Video_MatchOrientation { get; set; }
       [ProtoMember(85)]
        public bool RewardedAdOnly { get; set; }

        #endregion

       [ProtoMember(86)]
        public decimal? DailyBudget { get; set; }

       [ProtoMember(87)]
        public decimal? Budget { get; set; }


       [ProtoMember(88)]
        public bool AdPosition_Unknown { get; set; }

       [ProtoMember(89)]
        public bool AdPosition_AboveTheFold { get; set; }

       [ProtoMember(90)]
        public bool AdPosition_BelowTheFold { get; set; }
       [ProtoMember(91)]
        public bool AdPosition_Enabled { get; set; }

       [ProtoMember(92)]
        public BiddingStrategy BiddingStrategy { get; set; }




       [ProtoMember(93)]
        public decimal BidOptimizationValue { get; set; }


       [ProtoMember(94)]
        public decimal MaxBidPrice { get; set; }



       [ProtoMember(95)]
        public bool KeepBiddingAtMinimum { get; set; }
       [ProtoMember(96)]

        public BidOptimizationType BidOptimizationType { get; set; }



       [ProtoMember(97)]
        public ConversionSetting ConversionSetting { get; set; }
       [ProtoMember(98)]
        public ConversionType ConversionType { get; set; }

       [ProtoMember(99)]
        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }


       [ProtoMember(100)]
        public int ViewAttribuation { get; set; }
       [ProtoMember(101)]
        public int ClickAttribuation { get; set; }
       [ProtoMember(102)]
        public int CountingAttribuation { get; set; }

        #region Conversion and events

       [ProtoMember(103)]
        public IList<AdGroupConversionEventDto> ConversionItems { get; set; } = new List<AdGroupConversionEventDto>();


        [ProtoMember(104)]
        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; } = new List<AdGroupTrackingEventDto>();

        #endregion



        [ProtoMember(106)]
        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; } = new List<AdGroupBidModifierDto>();


        [ProtoMember(107)]
        public IList<int> SelectedInventory { get; set; } = new List<int>();



        [ProtoMember(108)]
        public IList<int> IncludedInventory { get; set; } = new List<int>();



        [ProtoMember(109)]
        public IList<int> SelectedDeals { get; set; } = new List<int>();

        [ProtoMember(110)]
        public IList<int> SelectedMasterListConfigs { get; set; } = new List<int>();


        [ProtoMember(111)]
        public IList<IPTargetingDto> AllIPRanges { get; set; } = new List<IPTargetingDto>();

        [ProtoMember(112)]
        public IList<GeoFencingUITargeting> AllGeoFencing { get; set; } = new List<GeoFencingUITargeting>();


        [ProtoMember(113)]
        public IList<URLTargetingDto> AllURLs { get; set; } = new List<URLTargetingDto>();


        [ProtoMember(114)]
        public IList<int> AllKeywords { get; set; } = new List<int>();

        [ProtoMember(115)]
        public IEnumerable<CampaignBidConfigDto> AllBidConfigItems { get; set; } = new List<CampaignBidConfigDto>();


        [ProtoMember(116)]
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        [ProtoMember(117)]
        public int ForAppSite { get; set; }

        [ProtoMember(118)]
        public IList<int> TargetAppSites { get; set; } = new List<int>();

        [ProtoMember(119)]
        public string groupContextualString { get; set; }

        [ProtoMember(120)]
        public string changedContextuals { get; set; }

        [ProtoMember(121)]
        public string groupBrandSafetyString { get; set; }

        [ProtoMember(122)]
        public string changedBrandSafety { get; set; }

    }


}
