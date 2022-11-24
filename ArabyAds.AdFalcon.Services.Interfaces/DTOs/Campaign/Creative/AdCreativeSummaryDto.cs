using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AppSiteAdQueueDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public string Name { get; set; }

       [ProtoMember(3)]
        public int AccountId { get; set; }
       [ProtoMember(4)]
        public string AccountName { get; set; }

        [ProtoMember(5)]
        public string Type { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100,typeof(AdCreativeSummaryDto))]
    public class AdCreativeSummaryDtoBase
    {
       [ProtoMember(1)]
        public ClickMethod ClickMethod { get; set; }

       [ProtoMember(2)]
        public string uId { get; set; }

       [ProtoMember(3)]
        public virtual int ID { get; set; }
       [ProtoMember(4)]
        public string AdvertiserName { get; set; }
       [ProtoMember(5)]
        public int AdvertiserId{ get; set; }

       [ProtoMember(6)]
        public string AdvertiserAccountName { get; set; }

       [ProtoMember(7)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(8)]
        public string Name { get; set; }

       [ProtoMember(9)]
        public string AdText { get; set; }

       [ProtoMember(10)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Bid { get; set; }

       [ProtoMember(11)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal DiscountedBid { get; set; }

       [ProtoMember(12)]
        public string Status { get; set; }

       [ProtoMember(13)]
        public string CreativeVendorText { get; set; }

       [ProtoMember(14)]
        public string ViewName { get; set; }

       [ProtoMember(15)]
        public AdActionValueDto ActionValue { get; set; }

       [ProtoMember(16)]
        public AdTypeIds TypeId { get; set; }

       [ProtoMember(17)]
        public AdSubTypes? AdSubType { get; set; }

       [ProtoMember(18)]
        public int ActionId { get; set; }

       [ProtoMember(19)]
        public EnvironmentType EnvironmentType { get; set; }

       [ProtoMember(20)]
        public OrientationType OrientationType { get; set; }

       [ProtoMember(21)]
        public RichMediaRequiredProtocolDto RichMediaRequiredProtocol { get; set; }

       [ProtoMember(22)]
        public IList<AdCreativeUnitDto> CreativeUnitsContent { get; set; } = new List<AdCreativeUnitDto>();


        [ProtoMember(23)]
        public IList<string> ImpressionTrackingURL { get; set; } = new List<string>();
        [ProtoMember(24)]
        public IList<string> ImpressionTrackingJS { get; set; } = new List<string>();

        [ProtoMember(25)]
        public virtual string ActionText { get; set; }

       [ProtoMember(26)]
        public virtual string AppUrl { get; set; }

       [ProtoMember(27)]
        public virtual bool ShowIfInstalled { get; set; }

       [ProtoMember(28)]
        public virtual bool IsSecureCompliant { get; set; }
       [ProtoMember(29)]
        public virtual float? StarRating { get; set; }

       [ProtoMember(30)]
        public virtual string Description { get; set; }

       [ProtoMember(31)]
        public IList<AdCreativeUnitDto> NativeAdIcons { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(32)]
        public IList<AdCreativeUnitDto> NativeAdImages { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(33)]
        public DeviceTypeEnum? AdBannerType { get; set; }

       [ProtoMember(34)]
        public virtual string ClickTrackerUrl { get; set; }

       [ProtoMember(35)]
        public virtual string AppMarketingPartnerName { get; set; }


       [ProtoMember(36)]
        public virtual bool EnableEventsPostback { get; set; }

       [ProtoMember(37)]
        public virtual bool VerifyTargetingCriteria { get; set; }

       [ProtoMember(38)]
        public virtual bool UpdateEventsFrequency { get; set; }

       [ProtoMember(39)]
        public virtual bool VerifyDailyBudget { get; set; }
       [ProtoMember(40)]
        public virtual bool VerifyCampaignStartAndEndDate { get; set; }

       [ProtoMember(41)]
        public virtual bool ValidateRequestDeviceAndLocationData { get; set; }


        

       [ProtoMember(42)]
        public virtual bool UpdateTags { get; set; }
       [ProtoMember(43)]
        public virtual bool VerifyEventsFrequency { get; set; }

       [ProtoMember(44)]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; } = new List<AdCreativeUnitVendorDto>();
        [ProtoMember(45)]
        public IList<int> CreativeVendorIds { get; set; }

       [ProtoMember(46)]
        public virtual bool IsVideo { get; set; }

       [ProtoMember(47)]
        public virtual bool IsXML { get; set; }

        #region  Video End Card

       [ProtoMember(48)]
        public virtual bool IsVpaid { get; set; }
       [ProtoMember(49)]
        public virtual bool Vpaid_1 { get; set; }
       [ProtoMember(50)]
        public virtual bool Vpaid_2 { get; set; }

       [ProtoMember(51)]
        public virtual string AdActionValueVideoEndCardURL { get; set; }
       [ProtoMember(52)]
        public VideoEndCardType CardType { get; set; }

       [ProtoMember(53)]
        public double AutoCloseWaitInSeconds { get; set; }

       [ProtoMember(54)]
        public bool EnableAutoClose { get; set; }

       [ProtoMember(55)]
        public bool VideoEndCardFluid { get; set; }
       [ProtoMember(56)]
        public string  VideoEndCardFluidURL { get; set; }

       [ProtoMember(57)]
        public bool IsStatic { get; set; }
       [ProtoMember(58)]
        public AdActionValueDto AdActionValueVideoEndCard { get; set; } 

       [ProtoMember(59)]
        public IList<string> VideoEndCardsTrackingURL { get; set; } = new List<string>();


        [ProtoMember(60)]
        public IList<AdCreativeUnitDto> VideoEndCardAdImages { get; set; } = new List<AdCreativeUnitDto>();

        [ProtoMember(61)]
        public IList<CreativeUnitDto> ImageUrls { get; set; } = new List<CreativeUnitDto>();

        [ProtoMember(62)]
        public IList<AdCreativeUnitDto> VideoEndCardCreativeUnitsContent { get; set; } = new List<AdCreativeUnitDto>();

        #endregion

        [ProtoMember(63)]
        public IList<ClickTagTrackerDto> ClickTags { get; set; } = new List<ClickTagTrackerDto>();
        [ProtoMember(64)]
        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; } = new List<ThirdPartyTrackerDto>();
        [ProtoMember(65)]
        //public IList<InStreamVideoCreativeUnitDto> InStreamVideoCreativeUnits { get; set; }
        public bool IsMandatory { get; set; }

       [ProtoMember(66)]
       
        public string WrapperContent { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(AdCreativeFullSummaryDto))]
    public class AdCreativeSummaryDto : AdCreativeSummaryDtoBase
    {

       [ProtoMember(1)]
        public bool VerifyPrerequisiteEvents { get; set; }
       [ProtoMember(2)]
        public AdGroupSummaryDtoBase Group { get; set; }
       [ProtoMember(3)]
        public CampaignsSummaryDtoBase Campaign { get; set; }


       [ProtoMember(4)]
        public IList<AdCreativeSummaryDtoBase> VideoEndCards { get; set; } = new List<AdCreativeSummaryDtoBase>();

        [ProtoMember(5)]
        public virtual IList<ErrorData> Warnings { get; set; } = new List<ErrorData>();

        public bool isSummary { get; set; }
    }


    [ProtoContract]
    public class AdCreativeFullSummaryDto : AdCreativeSummaryDto
    {
       [ProtoMember(1)]
        public IList<AppSiteAdQueueDto> AppSiteAdQueues { get; set; } = new List<AppSiteAdQueueDto>();

        [ProtoMember(2)]
        public bool Include { get; set; }

       [ProtoMember(3)]
        public int AdAccountId { get; set; }

       [ProtoMember(4)]
        public string DomainURL { get; set; }

       [ProtoMember(5)]
        public KeywordDto Keyword { get; set; }


       [ProtoMember(6)]
        public LanguageDto Language { get; set; }

        [ProtoMember(7)]
        public int? PlatformId { get; set; }


    }



}
