using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AppSiteAdQueueDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
    }

    [DataContract]
    public class AdCreativeSummaryDtoBase
    {
        [DataMember]
        public ClickMethod ClickMethod { get; set; }

        [DataMember]
        public string uId { get; set; }

        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public string AdvertiserName { get; set; }
        [DataMember]
        public int AdvertiserId{ get; set; }

        [DataMember]
        public string AdvertiserAccountName { get; set; }

        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AdText { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Bid { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal DiscountedBid { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string CreativeVendorText { get; set; }

        [DataMember]
        public string ViewName { get; set; }

        [DataMember]
        public AdActionValueDto ActionValue { get; set; }

        [DataMember]
        public AdTypeIds TypeId { get; set; }

        [DataMember]
        public AdSubTypes? AdSubType { get; set; }

        [DataMember]
        public int ActionId { get; set; }

        [DataMember]
        public EnvironmentType EnvironmentType { get; set; }

        [DataMember]
        public OrientationType OrientationType { get; set; }

        [DataMember]
        public RichMediaRequiredProtocolDto RichMediaRequiredProtocol { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> CreativeUnitsContent { get; set; }


        [DataMember]
        public IList<string> ImpressionTrackingURL { get; set; }
        [DataMember]
        public IList<string> ImpressionTrackingJS { get; set; }

        [DataMember]
        public virtual string ActionText { get; set; }

        [DataMember]
        public virtual string AppUrl { get; set; }

        [DataMember]
        public virtual bool ShowIfInstalled { get; set; }

        [DataMember]
        public virtual bool IsSecureCompliant { get; set; }
        [DataMember]
        public virtual float? StarRating { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> NativeAdIcons { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> NativeAdImages { get; set; }

        [DataMember]
        public DeviceTypeEnum? AdBannerType { get; set; }

        [DataMember]
        public virtual string ClickTrackerUrl { get; set; }

        [DataMember]
        public virtual string AppMarketingPartnerName { get; set; }


        [DataMember]
        public virtual bool EnableEventsPostback { get; set; }

        [DataMember]
        public virtual bool VerifyTargetingCriteria { get; set; }

        [DataMember]
        public virtual bool UpdateEventsFrequency { get; set; }

        [DataMember]
        public virtual bool VerifyDailyBudget { get; set; }
        [DataMember]
        public virtual bool VerifyCampaignStartAndEndDate { get; set; }

        [DataMember]
        public virtual bool ValidateRequestDeviceAndLocationData { get; set; }


        

        [DataMember]
        public virtual bool UpdateTags { get; set; }
        [DataMember]
        public virtual bool VerifyEventsFrequency { get; set; }

        [DataMember]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; }
        [DataMember]
        public IList<int> CreativeVendorIds { get; set; }

        [DataMember]
        public virtual bool IsVideo { get; set; }

        [DataMember]
        public virtual bool IsXML { get; set; }

        #region  Video End Card

        [DataMember]
        public virtual bool IsVpaid { get; set; }
        [DataMember]
        public virtual bool Vpaid_1 { get; set; }
        [DataMember]
        public virtual bool Vpaid_2 { get; set; }

        [DataMember]
        public virtual string AdActionValueVideoEndCardURL { get; set; }
        [DataMember]
        public VideoEndCardType CardType { get; set; }

        [DataMember]
        public double AutoCloseWaitInSeconds { get; set; }

        [DataMember]
        public bool EnableAutoClose { get; set; }

        [DataMember]
        public bool VideoEndCardFluid { get; set; }
        [DataMember]
        public string  VideoEndCardFluidURL { get; set; }

        [DataMember]
        public bool IsStatic { get; set; }
        [DataMember]
        public AdActionValueDto AdActionValueVideoEndCard { get; set; }

        [DataMember]
        public IList<string> VideoEndCardsTrackingURL { get; set; }


        [DataMember]
        public IList<AdCreativeUnitDto> VideoEndCardAdImages { get; set; }

        [DataMember]
        public IList<CreativeUnitDto> ImageUrls { get; set; }

        [DataMember]
        public IList<AdCreativeUnitDto> VideoEndCardCreativeUnitsContent { get; set; }

        #endregion

        [DataMember]
        public IList<ClickTagTrackerDto> ClickTags { get; set; }
        [DataMember]
        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; }
        [DataMember]
        //public IList<InStreamVideoCreativeUnitDto> InStreamVideoCreativeUnits { get; set; }
        public bool IsMandatory { get; set; }

        [DataMember]
       
        public string WrapperContent { get; set; }
    }

    [DataContract]
    public class AdCreativeSummaryDto : AdCreativeSummaryDtoBase
    {

        [DataMember]
        public bool VerifyPrerequisiteEvents { get; set; }
        [DataMember]
        public AdGroupSummaryDtoBase Group { get; set; }
        [DataMember]
        public CampaignsSummaryDtoBase Campaign { get; set; }


        [DataMember]
        public IList<AdCreativeSummaryDtoBase> VideoEndCards { get; set; }

        [DataMember]
        public virtual IList<ErrorData> Warnings { get; set; }

        public bool isSummary { get; set; }
    }


    [DataContract()]
    public class AdCreativeFullSummaryDto : AdCreativeSummaryDto
    {
        [DataMember]
        public IList<AppSiteAdQueueDto> AppSiteAdQueues { get; set; }

        [DataMember]
        public bool Include { get; set; }

        [DataMember]
        public int AdAccountId { get; set; }

        [DataMember]
        public string DomainURL { get; set; }

        [DataMember]
        public KeywordDto Keyword { get; set; }


        [DataMember]
        public LanguageDto Language { get; set; }
        



    }



}
