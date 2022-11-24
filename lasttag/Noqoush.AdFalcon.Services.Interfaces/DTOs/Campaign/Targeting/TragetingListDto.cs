using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class TargetingListDto
    {
        [DataMember]
        public AdGroupDealAndSourceDTO MultiSources { get; set; }
        [DataMember]
        public bool AllowInclude { get; set; }

        [DataMember]
        public bool AllowGeofencing { get; set; }
        [DataMember]
        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }

        [DataMember]
        public string groupAudianceString { get; set; }

        [DataMember]
        public int? TargetingConnectionType { get; set; }
        [DataMember]
        public bool AllowOpenAuction { get; set; }

        [DataMember]
        public group group { get; set; }

        [DataMember]
        public string AdvertiserName { get; set; }


        [DataMember]
        public int? ViewabilityVendorId { get; set; }



        [DataMember]
        public string UniqueId { get; set; }
        



        [DataMember]
        public int AdvertiserId { get; set; }

        [DataMember]
        public string AdvertiserAccountName { get; set; }
        [DataMember]
        public bool ExcludeSensitiveCategories { get; set; }

        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public string CampaignName { get; set; }
        [DataMember]
        public bool DisableProxyTraffic {get;set;}
        [DataMember]
        public bool IsWifi { get; set; }
        [DataMember]
        public bool IsCellular { get; set; }
        [DataMember]
        public string AdGroupName { get; set; }
        [DataMember]
        public IEnumerable<TargetingBaseDto> Targeting { get; set; }
        [DataMember]
        public decimal Bid { get; set; }
        [DataMember]
        public decimal DiscountedBid { get; set; }
        [DataMember]
        public  int CostModelWrapper { get; set; }
        [DataMember]
        public AdActionTypeDto AdActionTypeDto { get; set; }
        [DataMember]
        public AdTypeDto AdType { get; set; }
        [DataMember]
        public DiscountDto DiscountDto { get; set; }
        [DataMember]
        public int? CampaignCostModelWrapper { get; set; }
        [DataMember]
        public bool IsClientLocked { get; set; }
        [DataMember]
        public bool IsClientReadOnly{ get; set; }
        
        [DataMember]
        public bool IsHasAds{ get; set; }
        [DataMember]
        public bool TrackInstalls { get; set; }
        [DataMember]
        public bool OpenInExternalBrowser { get; set; }
        [DataMember]
        public bool LoadDefaultsTrackingEvents { get; set; }
        [DataMember]
        public bool IsPricingModelChanged { get; set; }

        [DataMember]
        public bool RunAllExchanges { get; set; }
        [DataMember]

        public bool IsVideoActionType { get; set; }



        [DataMember]
        public decimal? AudianceDiscountPrice { get; set; }


        [DataMember]
        public decimal? DailyBudget { get; set; }

        [DataMember]
        public decimal? Budget { get; set; }
        [DataMember]
        public decimal? DataBid { get; set; }
        [DataMember]
        public decimal? MaxDataBid { get; set; }
        [DataMember]
        public int CountExternalAudienceList { get; set; }
        [DataMember]
        public string DataPriceAudienceSegment { get; set; }

        [DataMember]
        public IEnumerable<AdvertiserAccountMasterAppSiteDto> MasterList { get; set; }

        [DataMember]
        public IEnumerable<AdGroupFeeDto> FeesAdded { get; set; }


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
        public  ConversionSetting ConversionSetting { get; set; }
        [DataMember]
        public  ConversionType ConversionType { get; set; }
        [DataMember]
        public  int ViewAttribuation { get; set; }
        [DataMember]
        public  int ClickAttribuation { get; set; }
        [DataMember]
        public  int CountingAttribuation { get; set; }

        #region Conversion and events

        [DataMember]
        public IList<AdGroupConversionEventDto> ConversionItems { get; set; }


        [DataMember]
        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; }




        #endregion


    }
}
