using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class ImageDocumentNBaseDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ParentId { get; set; }
    }

    [DataContract]
    public class CreativeUnitContentDto
    {
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int CreativeUnitId { get; set; }
    }

    [DataContract]
    public class ImageDocumentNDto : ImageDocumentNBaseDto
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class AdCreativeDtoBase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        [Required(ResourceName = "AdNameErrMsg")]
        [StringLength(255)]
        public string Name { get; set; }

        [DataMember]
        //[Required(ResourceName = "AdTextErrMsg")]
        [StringLength(40, ResourceName = "AdTextLengthErrMsg")]
        public string AdText { get; set; }

        [DataMember]
        [Required(ResourceName = "BidErrMsg")]
        [RegularExpression(@"^\d{1,5}(\.\d{1,3})?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Bid { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal MinBid { get; set; }

        [DataMember]
        public decimal? DiscountedBid { get; set; }
        [DataMember]
        public DiscountDto DiscountDto { get; set; }

        [DataMember]
        public string AdvertiserName { get; set; }
        [DataMember]
        public string AdvertiserAccountName { get; set; }
        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }
        [DataMember]
        public string ViewName { get; set; }
        [DataMember]
        public ClickMethod ClickMethod { get; set; }
        [DataMember]
        [Required()]
        public AdTypeIds TypeId { get; set; }

        [DataMember]
        public AdSubTypes? AdSubType { get; set; }

        [DataMember]
        [Required()]
        public EnvironmentType EnvironmentType { get; set; }

        [DataMember]
        [Required()]
        public OrientationType OrientationType { get; set; }

        [DataMember]
        public int TileImageId { get; set; }

        [DataMember]
        public AdActionValueDto AdActionValue { get; set; }

        [DataMember]
        public int AdActionId { get; set; }

        [DataMember]
        public bool IsAdChanged { get; set; }

        [DataMember]
        public VASTProtocolsVersion VASTProtocol { get; set; }

        [DataMember]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; }
        [DataMember]
        public IList<int> CreativeVendorIds { get; set; }
        [DataMember]
        public bool IsCreativeVendorChanged { get; set; }

        [DataMember]
        public bool IsVideo { get; set; }
        [DataMember]
        public string VideoEndCardFluidURL { get; set; }
        [DataMember]
        public bool VideoEndCardFluid { get; set; }

        #region  Compnion Ads

        [DataMember]
        public IList<AdCreativeUnitDto> VideoEndCardCreativeUnitsContent { get; set; }
        [DataMember]
        public string AdActionValueVideoEndCardURL { get; set; }
        [DataMember]
        public AdActionValueDto AdActionValueImpressionTracker { get; set; }

        [DataMember]
        public AdActionValueDto AdActionValueVideoEndCard { get; set; }

        [DataMember]
        public IList<string> ImpressionTrackingURL { get; set; }
        [DataMember]
        public IList<string> ImpressionTrackingJS { get; set; }
        

        [DataMember]
        public IList<string> VideoEndCardsTrackingURL { get; set; }

        [DataMember]
        public VideoEndCardType CardType { get; set; }

        [DataMember]
        public double AutoCloseWaitInSeconds { get; set; }

        [DataMember]
        public bool EnableAutoClose { get; set; }

        #endregion

        [DataMember]
        public IList<ClickTagTrackerDto> ClickTags { get; set; }

        [DataMember]
        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; }

        [DataMember]
        public string WrapperContent { get; set; }


    }
    [DataContract]
    public class AdCreativeDto : AdCreativeDtoBase
    {
        [DataMember]
        public string UniqueId { get; set; }
        [DataMember]
        public bool IsClientLocked { get; set; }

        [DataMember]
        public bool IsClientReadOnly { get; set; }
        [DataMember]
        [StringLength(255)]
        public string CampaignName { get; set; }
        [DataMember]
        public string AdGroupName { get; set; }
        [DataMember]
        public bool IsSecureCompliant { get; set; }

        [DataMember]
        public AdTypeDto AdType { get; set; }

        [DataMember]
        public DeviceTypeDto AdGroupDeviceTypeTargeting { get; set; }

        [DataMember]
        public string AppMarketingPartnerName { get; set; }
        [DataMember]
        public IList<AdCreativeUnitDto> CreativeUnitsContent { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> NativeAdIcons { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> NativeAdImages { get; set; }
        [DataMember]
        public IList<AdCreativeUnitDto> VideoEndCardAdImages { get; set; }

        [DataMember]
        public IList<CreativeUnitDto> ImageUrls { get; set; }

        //[DataMember]
        //public InStreamVideoCreativeUnitDto InStreamVideoCreativeUnit { get; set; }

        [DataMember]
        public DeviceTypeEnum? AdBannerType { get; set; }
        [DataMember]
        public bool IsAllAdsPaused { get; set; }

        [DataMember]
        public bool IsAdPaused { get; set; }

        [DataMember]
        public AdGroupDto Group { get; set; }

        [DataMember]
        public RichMediaRequiredProtocolDto RichMediaRequiredProtocol { get; set; }

        [DataMember]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [StringLength(400, ResourceName = "NativeAdDescriptionLengthErrMsg")]
        public string Description { get; set; }

        [DataMember]
        [StringLength(50, ResourceName = "ActionTextLengthErrMsg")]
        public string ActionText { get; set; }

        [DataMember]
        [Range(0d, 5d, ResourceName = "RangeMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        public float? StarRating { get; set; }

        [DataMember]
        public string AppUrl { get; set; }

        [DataMember]
        public bool ShowIfInstalled { get; set; }

        [DataMember]
        public bool IsDownloadAction { get; set; }

        [DataMember]
        public string XMlUrl { get; set; }

        [DataMember]
        public string Xml { get; set; }

        [DataMember]
        public bool IsXmlUrl { get; set; }


     
        [DataMember]
        public bool IsStatic { get; set; }

        [DataMember]
        public bool IsVpaid { get; set; }
        [DataMember]
        public bool Vpaid_1 { get; set; }
        [DataMember]
        public bool Vpaid_2 { get; set; }

        #region TrackerAd




        [DataMember]
        [Required()]

        public int AppMarketingPartnerId { get; set; }

        [DataMember]
        public string ClickTrackerUrl { get; set; }

        [DataMember]
        public bool EnableEventsPostback { get; set; }

        [DataMember]
        public bool VerifyTargetingCriteria { get; set; }
        [DataMember]
        public bool UpdateEventsFrequency { get; set; }

        [DataMember]
        public bool VerifyPrerequisiteEvents { get; set; }
        
        [DataMember]
        public bool VerifyDailyBudget { get; set; }

        [DataMember]
        public bool VerifyCampaignStartAndEndDate { get; set; }

        [DataMember]
        public bool ValidateRequestDeviceAndLocationData { get; set; }


        



        [DataMember]
        public bool UpdateTags { get; set; }

        [DataMember]
        public bool VerifyEventsFrequency { get; set; }











        #endregion
        [DataMember]
        public IList<AdActionValueDto> VideoAdActionValueListDto { get; set; }

        [DataMember]
        public IList<AdCreativeDto> VideoEndCards { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
    }



}
