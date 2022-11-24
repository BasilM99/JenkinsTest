using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdCreativeSaveDto : AdCreativeDtoBase
    {
      

       [ProtoMember(1)]
        public List<AdCreativeUnitDto> CreativeUnitsContent { get; set; }
       [ProtoMember(2)]
        public List<AdCreativeUnitDto> Banners { get; set; }
       [ProtoMember(3)]
        public List<TileImageInformationDto> TileImageInformationList { get; set; }
       [ProtoMember(4)]
        public List<AdCreativeUnitDto> NativeAdIcons { get; set; }
       [ProtoMember(5)]
        public List<AdCreativeUnitDto> NativeAdImages { get; set; }
       [ProtoMember(6)]
        public List<AdCreativeUnitDto> InStreamVideos { get; set; }

       [ProtoMember(7)]
        public VideoMediaFileDto VideoMediaFile { get; set; }
       [ProtoMember(8)]
        public IList<VideoMediaFileDto> MediaFilesSupported { get; set; }
       [ProtoMember(9)]
        public DeviceTypeEnum? AdBannerType { get; set; }
       [ProtoMember(10)]
        public bool IsAdPaused { get; set; }
       [ProtoMember(11)]
        public int? RichMediaRequiredProtocol { get; set; }

       [ProtoMember(12)]
        public string Description { get; set; }

       [ProtoMember(13)]
        public string ActionText { get; set; }

       [ProtoMember(14)]
        public float? StarRating { get; set; }

       [ProtoMember(15)]
        public string AppUrl { get; set; }
       [ProtoMember(16)]
        public bool IsSecureCompliant { get; set; }

       [ProtoMember(17)]
        public bool ShowIfInstalled { get; set; }


        #region TrackerAd




       [ProtoMember(18)]


        public int AppMarketingPartnerId { get; set; }

       [ProtoMember(19)]
        public string ClickTrackerUrl { get; set; }

       [ProtoMember(20)]
        public bool EnableEventsPostback { get; set; }

       [ProtoMember(21)]
        public bool VerifyTargetingCriteria { get; set; }
       [ProtoMember(22)]
        public bool UpdateEventsFrequency { get; set; }

       [ProtoMember(23)]
        public bool VerifyDailyBudget { get; set; }

       [ProtoMember(24)]
        public bool VerifyCampaignStartAndEndDate { get; set; }

       [ProtoMember(25)]
        public bool ValidateRequestDeviceAndLocationData { get; set; }


        

       [ProtoMember(26)]
        public bool VerifyPrerequisiteEvents { get; set; }



       [ProtoMember(27)]
        public bool UpdateTags { get; set; }

       [ProtoMember(28)]
        public bool VerifyEventsFrequency { get; set; }











        #endregion


       [ProtoMember(29)]
        public IList<AdCreativeSaveDto> VideoEndCards { get; set; }

       [ProtoMember(30)]
        public string SelectedHTML5FileName { get; set; }
       [ProtoMember(31)]
        public int SelectedHTML5DocumentId { get; set; }
       [ProtoMember(32)]
        public int SelectedHTML5CreativeId { get; set; }
       [ProtoMember(33)]
        public bool IsMandatory { get; set; }

        [ProtoMember(34)]
        public int? PlatformId { get; set; }
    }

    [ProtoContract]
    public class TileImageInformationDto
    {
       [ProtoMember(1)]
        public TileImageDocumentDto TileImage { get; set; }

       [ProtoMember(2)]
        public string ImpressionTrackerRedirect { get; set; }
       [ProtoMember(3)]
        public string ImpressionTrackerJSRedirect { get; set; }

    }
}
