using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdCreativeSaveDto : AdCreativeDtoBase
    {
      

        [DataMember]
        public List<AdCreativeUnitDto> CreativeUnitsContent { get; set; }
        [DataMember]
        public List<AdCreativeUnitDto> Banners { get; set; }
        [DataMember]
        public List<TileImageInformationDto> TileImageInformationList { get; set; }
        [DataMember]
        public List<AdCreativeUnitDto> NativeAdIcons { get; set; }
        [DataMember]
        public List<AdCreativeUnitDto> NativeAdImages { get; set; }
        [DataMember]
        public List<AdCreativeUnitDto> InStreamVideos { get; set; }

        [DataMember]
        public VideoMediaFileDto VideoMediaFile { get; set; }
        [DataMember]
        public IList<VideoMediaFileDto> MediaFilesSupported { get; set; }
        [DataMember]
        public DeviceTypeEnum? AdBannerType { get; set; }
        [DataMember]
        public bool IsAdPaused { get; set; }
        [DataMember]
        public int? RichMediaRequiredProtocol { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ActionText { get; set; }

        [DataMember]
        public float? StarRating { get; set; }

        [DataMember]
        public string AppUrl { get; set; }
        [DataMember]
        public bool IsSecureCompliant { get; set; }

        [DataMember]
        public bool ShowIfInstalled { get; set; }


        #region TrackerAd




        [DataMember]


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
        public bool VerifyDailyBudget { get; set; }

        [DataMember]
        public bool VerifyCampaignStartAndEndDate { get; set; }

        [DataMember]
        public bool ValidateRequestDeviceAndLocationData { get; set; }


        

        [DataMember]
        public bool VerifyPrerequisiteEvents { get; set; }



        [DataMember]
        public bool UpdateTags { get; set; }

        [DataMember]
        public bool VerifyEventsFrequency { get; set; }











        #endregion


        [DataMember]
        public IList<AdCreativeSaveDto> VideoEndCards { get; set; }

        [DataMember]
        public string SelectedHTML5FileName { get; set; }
        [DataMember]
        public int SelectedHTML5DocumentId { get; set; }
        [DataMember]
        public int SelectedHTML5CreativeId { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
       

    }

    [DataContract]
    public class TileImageInformationDto
    {
        [DataMember]
        public TileImageDocumentDto TileImage { get; set; }

        [DataMember]
        public string ImpressionTrackerRedirect { get; set; }
        [DataMember]
        public string ImpressionTrackerJSRedirect { get; set; }

    }
}
