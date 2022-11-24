using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ImageDocumentNDto))]
    public class ImageDocumentNBaseDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int ParentId { get; set; }
    }

    [ProtoContract]
    public class CreativeUnitContentDto
    {
       [ProtoMember(1)]
        public string Content { get; set; }
       [ProtoMember(2)]
        public int CreativeUnitId { get; set; }
    }

    [ProtoContract]
    public class ImageDocumentNDto : ImageDocumentNBaseDto
    {
       [ProtoMember(1)]
        public string Name { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100,typeof(AdCreativeDto))]
    [ProtoInclude(101,typeof(AdCreativeSaveDto))]
    public class AdCreativeDtoBase
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        [Required(ResourceName = "AdNameErrMsg")]
        [StringLength(255)]
        public string Name { get; set; }
        
       [ProtoMember(3)]
        //[Required(ResourceName = "AdTextErrMsg")]
        [StringLength(40, ResourceName = "AdTextLengthErrMsg")]
        public string AdText { get; set; }

       [ProtoMember(4)]
        [Required(ResourceName = "BidErrMsg")]
        [RegularExpression(@"^\d{1,5}(\.\d{1,3})?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Bid { get; set; }

       [ProtoMember(5 )]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal MinBid { get; set; }

       [ProtoMember(6)]
        public decimal? DiscountedBid { get; set; }
       [ProtoMember(7)]
        public DiscountDto DiscountDto { get; set; }

       [ProtoMember(8)]
        public string AdvertiserName { get; set; }
       [ProtoMember(9)]
        public string AdvertiserAccountName { get; set; }
       [ProtoMember(10)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(11)]
        public int AdvertiserId { get; set; }
       [ProtoMember(12 )]
        public string ViewName { get; set; }
       [ProtoMember(13 )]
        public ClickMethod ClickMethod { get; set; }
       [ProtoMember(14)]
        [Required()]
        public AdTypeIds TypeId { get; set; }

       [ProtoMember(15)]
        public AdSubTypes? AdSubType { get; set; }

       [ProtoMember(16)]
        [Required()]
        public EnvironmentType EnvironmentType { get; set; }

       [ProtoMember(17)]
        [Required()]
        public OrientationType OrientationType { get; set; }

       [ProtoMember(18)]
        public int TileImageId { get; set; }

       [ProtoMember(19)]
        public AdActionValueDto AdActionValue { get; set; }

       [ProtoMember(20)]
        public int AdActionId { get; set; }

       [ProtoMember(21)]
        public bool IsAdChanged { get; set; }

       [ProtoMember(22)]
        public VASTProtocolsVersion VASTProtocol { get; set; }

       [ProtoMember(23 )]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; } = new List<AdCreativeUnitVendorDto>();
        [ProtoMember(24 )]
        public IList<int> CreativeVendorIds { get; set; }
       [ProtoMember(25)]
        public bool IsCreativeVendorChanged { get; set; }

       [ProtoMember(26)]
        public bool IsVideo { get; set; }
       [ProtoMember(27)]
        public string VideoEndCardFluidURL { get; set; }
       [ProtoMember(28)]
        public bool VideoEndCardFluid { get; set; }

        #region  Compnion Ads

       [ProtoMember(29)]
        public IList<AdCreativeUnitDto> VideoEndCardCreativeUnitsContent { get; set; } = new List<AdCreativeUnitDto>();
        [ProtoMember(30)]
        public string AdActionValueVideoEndCardURL { get; set; }
       [ProtoMember(31)]
        public AdActionValueDto AdActionValueImpressionTracker { get; set; }

       [ProtoMember(32)]
        public AdActionValueDto AdActionValueVideoEndCard { get; set; }

       [ProtoMember(33)]
        public IList<string> ImpressionTrackingURL { get; set; } = new List<string>();
        [ProtoMember(34)]
        public IList<string> ImpressionTrackingJS { get; set; } = new List<string>();


        [ProtoMember(35 )]
        public IList<string> VideoEndCardsTrackingURL { get; set; }

       [ProtoMember(36)]
        public VideoEndCardType CardType { get; set; }

       [ProtoMember(37 )]
        public double AutoCloseWaitInSeconds { get; set; }

       [ProtoMember(38 )]
        public bool EnableAutoClose { get; set; }

        #endregion

       [ProtoMember(39)]
        public IList<ClickTagTrackerDto> ClickTags { get; set; } = new List<ClickTagTrackerDto>();

        [ProtoMember(40 )]
        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; } = new List<ThirdPartyTrackerDto>();

        [ProtoMember(41 )]
        public string WrapperContent { get; set; }

        [ProtoMember(42)]
        public CampaignType CampaignType { get; set; }
    }
    [ProtoContract]
    public class AdCreativeDto : AdCreativeDtoBase
    {
       [ProtoMember(1)]
        public string UniqueId { get; set; }
       [ProtoMember(2)]
        public bool IsClientLocked { get; set; }

       [ProtoMember(3)]
        public bool IsClientReadOnly { get; set; }
       [ProtoMember(4)]
        [StringLength(255)]
        public string CampaignName { get; set; }
       [ProtoMember(5)]
        public string AdGroupName { get; set; }
       [ProtoMember(6)]
        public bool IsSecureCompliant { get; set; }

       [ProtoMember(7)]
        public AdTypeDto AdType { get; set; }

       [ProtoMember(8)]
        public DeviceTypeDto AdGroupDeviceTypeTargeting { get; set; }

       [ProtoMember(9)]
        public string AppMarketingPartnerName { get; set; }
       [ProtoMember(10)]
        public IList<AdCreativeUnitDto> CreativeUnitsContent { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(11)]
        public IList<AdCreativeUnitDto> NativeAdIcons { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(12)]
        public IList<AdCreativeUnitDto> NativeAdImages { get; set; } = new List<AdCreativeUnitDto>();
        [ProtoMember(13)]
        public IList<AdCreativeUnitDto> VideoEndCardAdImages { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(14)]
        public IList<CreativeUnitDto> ImageUrls { get; set; } = new List<CreativeUnitDto>();

        //[DataMember]
        //public InStreamVideoCreativeUnitDto InStreamVideoCreativeUnit { get; set; }

        [ProtoMember(15)]
        public DeviceTypeEnum? AdBannerType { get; set; }
       [ProtoMember(16)]
        public bool IsAllAdsPaused { get; set; }

       [ProtoMember(17)]
        public bool IsAdPaused { get; set; }

       [ProtoMember(18)]
        public AdGroupDto Group { get; set; }

       [ProtoMember(19)]
        public RichMediaRequiredProtocolDto RichMediaRequiredProtocol { get; set; }

       [ProtoMember(20)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [StringLength(400, ResourceName = "NativeAdDescriptionLengthErrMsg")]
        public string Description { get; set; }

       [ProtoMember(21)]
        [StringLength(50, ResourceName = "ActionTextLengthErrMsg")]
        public string ActionText { get; set; }

       [ProtoMember(22)]
        [Range(0d, 5d, ResourceName = "RangeMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        public float? StarRating { get; set; }

       [ProtoMember(23)]
        public string AppUrl { get; set; }

       [ProtoMember(24)]
        public bool ShowIfInstalled { get; set; }

       [ProtoMember(25)]
        public bool IsDownloadAction { get; set; }

       [ProtoMember(26)]
        public string XMlUrl { get; set; }

       [ProtoMember(27)]
        public string Xml { get; set; }

       [ProtoMember(28)]
        public bool IsXmlUrl { get; set; }


     
       [ProtoMember(29)]
        public bool IsStatic { get; set; }

       [ProtoMember(30)]
        public bool IsVpaid { get; set; }
       [ProtoMember(31)]
        public bool Vpaid_1 { get; set; }
       [ProtoMember(32)]
        public bool Vpaid_2 { get; set; }

        #region TrackerAd




       [ProtoMember(33)]
        [Required()]

        public int AppMarketingPartnerId { get; set; }

       [ProtoMember(34)]
        public string ClickTrackerUrl { get; set; }

       [ProtoMember(35)]
        public bool EnableEventsPostback { get; set; }

       [ProtoMember(36)]
        public bool VerifyTargetingCriteria { get; set; }
       [ProtoMember(37)]
        public bool UpdateEventsFrequency { get; set; }

       [ProtoMember(38)]
        public bool VerifyPrerequisiteEvents { get; set; }
        
       [ProtoMember(39)]
        public bool VerifyDailyBudget { get; set; }

       [ProtoMember(40)]
        public bool VerifyCampaignStartAndEndDate { get; set; }

       [ProtoMember(41)]
        public bool ValidateRequestDeviceAndLocationData { get; set; }


        



       [ProtoMember(42)]
        public bool UpdateTags { get; set; }

       [ProtoMember(43)]
        public bool VerifyEventsFrequency { get; set; }











        #endregion
       [ProtoMember(44)]
        public IList<AdActionValueDto> VideoAdActionValueListDto { get; set; } = new List<AdActionValueDto>();

        [ProtoMember(45)]
        public IList<AdCreativeDto> VideoEndCards { get; set; } = new List<AdCreativeDto>();
        [ProtoMember(46)]
        public bool IsMandatory { get; set; }

        [ProtoMember(47)]
        public int? PlatformId { get; set; }

        [ProtoMember(48)]
        public int? DeliveryMethod { get; set; }

        

    }



}
