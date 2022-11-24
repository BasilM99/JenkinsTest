using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class TargetingListDto
    {
       [ProtoMember(1)]
        public AdGroupDealAndSourceDTO MultiSources { get; set; }
       [ProtoMember(2)]
        public bool AllowInclude { get; set; }

       [ProtoMember(3)]
        public bool AllowGeofencing { get; set; }
       [ProtoMember(4)]
        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }

       [ProtoMember(5)]
        public string groupAudianceString { get; set; }

       [ProtoMember(6)]
        public int? TargetingConnectionType { get; set; }
       [ProtoMember(7)]
        public bool AllowOpenAuction { get; set; }

       [ProtoMember(8)]
        public group group { get; set; }

       [ProtoMember(9)]
        public string AdvertiserName { get; set; }


       [ProtoMember(10)]
        public int? ViewabilityVendorId { get; set; }



       [ProtoMember(11)]
        public string UniqueId { get; set; }
        



       [ProtoMember(12)]
        public int AdvertiserId { get; set; }

       [ProtoMember(13)]
        public string AdvertiserAccountName { get; set; }
       [ProtoMember(14)]
        public bool ExcludeSensitiveCategories { get; set; }

       [ProtoMember(15)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(16)]
        public string CampaignName { get; set; }
       [ProtoMember(17)]
        public bool DisableProxyTraffic {get;set;}
       [ProtoMember(18)]
        public bool IsWifi { get; set; }
       [ProtoMember(19)]
        public bool IsCellular { get; set; }
       [ProtoMember(20)]
        public string AdGroupName { get; set; }
       [ProtoMember(21)]
        public IEnumerable<TargetingBaseDto> Targeting { get; set; } = new List<TargetingBaseDto>();
        [ProtoMember(22)]
        public decimal Bid { get; set; }
       [ProtoMember(23)]
        public decimal DiscountedBid { get; set; }
       [ProtoMember(24)]
        public  int CostModelWrapper { get; set; }
       [ProtoMember(25)]
        public AdActionTypeDto AdActionTypeDto { get; set; }
       [ProtoMember(26)]
        public AdTypeDto AdType { get; set; }
       [ProtoMember(27)]
        public DiscountDto DiscountDto { get; set; }
       [ProtoMember(28)]
        public int? CampaignCostModelWrapper { get; set; }
       [ProtoMember(29)]
        public bool IsClientLocked { get; set; }
       [ProtoMember(30)]
        public bool IsClientReadOnly{ get; set; }
        
       [ProtoMember(31)]
        public bool IsHasAds{ get; set; }
       [ProtoMember(32)]
        public bool TrackInstalls { get; set; }
       [ProtoMember(33)]
        public bool OpenInExternalBrowser { get; set; }
       [ProtoMember(34)]
        public bool LoadDefaultsTrackingEvents { get; set; }
       [ProtoMember(35)]
        public bool IsPricingModelChanged { get; set; }

       [ProtoMember(36)]
        public bool RunAllExchanges { get; set; }
       [ProtoMember(37)]

        public bool IsVideoActionType { get; set; }



       [ProtoMember(38)]
        public decimal? AudianceDiscountPrice { get; set; }


       [ProtoMember(39)]
        public decimal? DailyBudget { get; set; }

       [ProtoMember(40)]
        public decimal? Budget { get; set; }
       [ProtoMember(41)]
        public decimal? DataBid { get; set; }
       [ProtoMember(42)]
        public decimal? MaxDataBid { get; set; }
       [ProtoMember(43)]
        public int CountExternalAudienceList { get; set; }
       [ProtoMember(44)]
        public string DataPriceAudienceSegment { get; set; }

       [ProtoMember(45)]
        public IEnumerable<AdvertiserAccountMasterAppSiteDto> MasterList { get; set; } = new List<AdvertiserAccountMasterAppSiteDto>();

        [ProtoMember(46)]
        public IEnumerable<AdGroupFeeDto> FeesAdded { get; set; } = new List<AdGroupFeeDto>();


        [ProtoMember(47)]
        public bool AdPosition_Unknown { get; set; }

       [ProtoMember(48)]
        public bool AdPosition_AboveTheFold { get; set; }

       [ProtoMember(49)]
        public bool AdPosition_BelowTheFold { get; set; }
       [ProtoMember(50)]
        public bool AdPosition_Enabled { get; set; }
       [ProtoMember(51)]
        public BiddingStrategy BiddingStrategy { get; set; }


       [ProtoMember(52)]
        public decimal BidOptimizationValue { get; set; }


       [ProtoMember(53)]
        public decimal MaxBidPrice { get; set; }



       [ProtoMember(54)]
        public bool KeepBiddingAtMinimum { get; set; }
       [ProtoMember(55)]

        public BidOptimizationType BidOptimizationType { get; set; }


       [ProtoMember(56)]
        public  ConversionSetting ConversionSetting { get; set; }
       [ProtoMember(57)]
        public  ConversionType ConversionType { get; set; }
       [ProtoMember(58)]
        public  int ViewAttribuation { get; set; }
       [ProtoMember(59)]
        public  int ClickAttribuation { get; set; }
       [ProtoMember(60)]
        public  int CountingAttribuation { get; set; }

        #region Conversion and events

       [ProtoMember(61)]
        public IList<AdGroupConversionEventDto> ConversionItems { get; set; } = new List<AdGroupConversionEventDto>();


        [ProtoMember(62)]
        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; } = new List<AdGroupTrackingEventDto>();




        #endregion
        [ProtoMember(63)]
        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; } = new List<AdGroupBidModifierDto>();



        [ProtoMember(64)]
        public CampaignType CampaignType { get; set; }

        [ProtoMember(65)]
        public bool isHouseAd { get; set; }

        [ProtoMember(66)]
        public List<AudienceSegmentDto> BrandSafetySegments { get; set; }

        [ProtoMember(67)]
        public List<AudienceSegmentDto> ContextualSegments { get; set; }

        [ProtoMember(68)]
        public string ContextualFirstPartyCode { get; set; }

        [ProtoMember(69)]
        public decimal MaxDataBidContextual { get; set; }

        [ProtoMember(70)]
        public decimal MaxDataBidBrandSafety { get; set; }

    }
}
