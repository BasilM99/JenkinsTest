using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{

    public class CreativeViewModel
    {
        public CreativeViewModel()
        {
            AllCreativeUnits = new Dictionary<int, IEnumerable<CreativeUnitViewModel>>();
            PhoneCreativeUnits = new Dictionary<int, IEnumerable<CreativeUnitViewModel>>();
            TabletCreativeUnits = new Dictionary<int, IEnumerable<CreativeUnitViewModel>>();

            EnvironmentTypes = new List<SelectListItem>
                {
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("AllEnvironmentType", "Campaign"),
                            Value = "0",
                            Selected = true
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("WebEnvironmentType", "Campaign"),
                            Value = "1",
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("AppEnvironmentType", "Campaign"),
                            Value = "2",
                        },
                };

         

            ClickMethods = new List<SelectListItem>
                {
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("Select", "Global"),
                            Value = "0",
                            Selected = true
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("EntireAdClickable", "Global"),
                            Value = "1",
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("QueryStringParameter", "Global"),
                            Value = "2",
                        },
        
                    new SelectListItem()
        {
            Text = ResourcesUtilities.GetResource("QueryStringParameterRe", "Global"),
                            Value = "3",
                        },
                };
            OrientationTypes = new List<SelectListItem>
                {
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("AllOrientationType", "Campaign"),
                            Value = "0",
                            Selected = true
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("PortraitOrientationType", "Campaign"),
                            Value = "1",
                        },
                    new SelectListItem()
                        {
                            Text = ResourcesUtilities.GetResource("LandscapeOrientationType", "Campaign"),
                            Value = "2",
                        },
                };

        }

        public bool IsAllowedToSaveImpressionTracker { get; set; }
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; }
        public bool IsCreativeVendorChanged { get; set; }
        public IList<int> CreativeVendorIds { get; set; }
        public bool IsClientLocked { get; set; }

        public ClickMethod ClickMethod { get; set; }
        public string WrapperContent { get; set; }
        public string SelectedHTML5FileName { get; set; }
        public int SelectedHTML5DocumentId { get; set; }
        public int SelectedHTML5CreativeId { get; set; }
        public int SelectedHTML5AdCreativeId { get; set; }
        public int CreativeVendorId { get; set; }
        public int AdCreativeVendorId { get; set; }
        public string CreativeVendorString { get; set; }
        public string CampaignName { get; set; }
        public int AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }


        public int AdvertiserAccountId { get; set; }
        public string AdvertiserAccountName { get; set; }
        public string AdGroupName { get; set; }
        public decimal? DiscountedBid { get; set; }


        public DiscountDto DiscountDto { get; set; }
        public AdCreativeDto AdCreativeDto { get; set; }
        public bool IsClientReadOnly { get; set; }
        
        //CreativeUnitLisViewModel

        public IDictionary<int, IEnumerable<CreativeUnitViewModel>> AllCreativeUnits { get; set; }
        public IDictionary<int, IEnumerable<CreativeUnitViewModel>> PhoneCreativeUnits { get; set; }
        public IDictionary<int, IEnumerable<CreativeUnitViewModel>> TabletCreativeUnits { get; set; }
        public IList<CreativeUnitViewModel> AllCreatives { get; set; }
        public IList<CreativeUnitViewModel> VideoEndCardAdImages { get; set; }
        public TileImageViewModel TileImageViewModel { get; set; }
        public IList<CreativeUnitViewModel> NativeAdIcons { get; set; }
        public IList<CreativeUnitViewModel> NativeAdImages { get; set; }
        public IEnumerable<CreativeUnitViewModel> InStreamVideos { get; set; }
        public IEnumerable<RichMediaRequiredProtocolDto> RichMediaProtocols { get; set; }
        public bool IsMandatory { get; set; }

        public IList<html5ListModel> HTML5CreativesUpdated { get; set; }
        public IList<html5ListModel> HTML5Creatives { get; set; }
        public IEnumerable<SelectListItem> ClickMethods { get; set; }
        public IEnumerable<SelectListItem> EnvironmentTypes { get; set; }
        public IEnumerable<SelectListItem> OrientationTypes { get; set; }
        public IList<SelectListItem> AdTypes { get; set; }
        public IEnumerable<SelectListItem> AdBannerTypes { get; set; }
        public IEnumerable<SelectListItem> AdSubTypes { get; set; }
        public string AdSubTypeName { get; set; }
        public bool IsSecureCompliant { get; set; }
        public string AdBannerTypeName { get; set; }
        public IList<SelectListItem> RichMediaRequiredProtocolsList { get; set; }
        public bool IsSecureCompliantRich { get; set; }
        public IEnumerable<VideoTypeDto> VideoTypeDtoList { get; set; }
        //public IEnumerable<VideoDeliveryMethod> VideoDeliveryMethodDtoList { get; set; }
    
        public IList<SelectListItem> VideoTypes { get; set; }
        public IList<SelectListItem> AppMarketingPartners { get; set; }
        public IList<SelectListItem> VideoDeliveryMethods { get; set; }
        public IList<AdActionValueDto> VideoAdActionValueListDto { get; set; }
        public IEnumerable<int> CreativeUnitIds()
        {
            return AllCreativeUnits.Keys.Union(PhoneCreativeUnits.Keys.Union(TabletCreativeUnits.Keys));
        }
        public IList<AdCreativeDto> VideoEndCards { get; set; }

        public IList<ClickTagTrackerDto> ClickTags { get; set; }

        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; }
        public string impressionTrackerRedirect { get; set; }

        public string platformString { get; set; }



        
    }

    public class CreativeUnitLisViewModel
    {
        public bool IsAllowedToSaveImpressionTracker { get; set; }
        public IEnumerable<CreativeUnitViewModel> CreativeUnits { get; set; }
        public AdTypeIds TypeId { get; set; }
        public AdSubTypes? AdSubType { get; set; }
        public AdCreativeDto AdCreativeDto { get; set; }
        public bool isVideoEndCard { get; set; }
    }
    public class CreativeUnitViewModel
    {
        public int AdCreativeId { get; set; }
        public bool IsAllowedToSaveImpressionTracker { get; set; }
        public int? CreativeVendorId { get; set; }
        public string Content { get; set; }
        public int? DocumentId { get; set; }
        public string ImpressionTrackerRedirect { get; set; }
        public string ImpressionTrackerJSRedirect { get; set; }
        //Alid
        public InStreamVideoCreativeUnitDto InStreamVideoCreativeUnit { get; set; }
        public IEnumerable<VideoTypeDto> VideoTypeDtoList { get; set; }
        //public IEnumerable<VideoDeliveryMethod> VideoDeliveryMethodDtoList { get; set; }
        public IList<SelectListItem> VideoTypes { get; set; }
        public IList<SelectListItem> VideoDeliveryMethods { get; set; }
        public IList<AdCreativeUnitTrackerDto> ImpressionTrackerRedirectList { get; set; }


        public CreativeUnitDto CreativeUnitDto { get; set; }
        public TileImageDocumentDto TileImageDocumentDto { get; set; }
        public string Name { get; set; }
        public int AdTypeId { get; set; }
        public int AdSubTypeId { get; set; }
        
        public string DisplayText { get; set; }
        public int? OrientationReplacementId { get; set; }
        public bool ShowCopy { get; set; }
        public DeviceTypeEnum DeviceType { get; set; }
        public IList<SelectListItem> CreativeVendors { get; set; }
        public string UniqueId { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; } 
        //     public string 
    }
    public class TileImageViewModel
    {
        public string SelectedValue { get; set; }
        public bool IsAllowedToSaveImpressionTracker { get; set; }
        public IEnumerable<CreativeUnitViewModel> TileImages { get; set; }
        public IEnumerable<SelectListItem> TileImageList { get; set; }
    }
    public class CreativeSaveViewModel
    {
        #region TrackingAdd

        public int AppMarketingPartnerId { get; set; }
        public string ClickTrackerUrl { get; set; }
        public bool EnableEventsPostback { get; set; }
        public bool VerifyTargetingCriteria { get; set; }
        public bool UpdateEventsFrequency { get; set; }
        public bool VerifyPrerequisiteEvents { get; set; }
        public bool VerifyDailyBudget { get; set; }
        public bool VerifyCampaignStartAndEndDate { get; set; }
        public bool ValidateRequestDeviceAndLocationData { get; set; }
        
        public bool UpdateTags { get; set; }
        public bool VerifyEventsFrequency { get; set; }
        #endregion
        

             public IList<TrackersEvent> TrackersEvent { get; set; }
        public bool IsStatic { get; set; }
        public int id { get; set; }
        public int adGroupId { get; set; }
        public int? adId { get; set; }
        public string returnUrl { get; set; }

        public string WrapperContent { get; set; }
        public int TileImage { get; set; }
        public string[] docId { get; set; }
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; }
        public bool IsCreativeVendorChanged { get; set; }
        public IList<int> CreativeVendorIds { get; set; }
        public ClickMethod ClickMethod { get; set; }
        public AdCreativeDto AdCreativeDto { get; set; }
        public IEnumerable<UploadViewModel> PhoneBanners { get; set; }
        public IEnumerable<UploadViewModel> TabletBanners { get; set; }
        public IEnumerable<UploadViewModel> TileImages { get; set; }
        public AdTypeIds AdTypeId { get; set; }
        public AdSubTypes? AdSubType { get; set; }
        public EnvironmentType EnvironmentType { get; set; }
        public OrientationType OrientationType { get; set; }
        public bool IsSecureCompliant { get; set; }
        public bool IsSecureCompliantRich { get; set; }
        public DeviceTypeEnum? AdBannerTypeId { get; set; }
        public int? RichMediaRequiredProtocol { get; set; }

        public bool IsMandatory { get; set; }
        public int CreativeVendorId { get; set; }
        public int AdCreativeVendorId { get; set; }
        public string SelectedHTML5FileName { get; set; }
        public int SelectedHTML5DocumentId { get; set; }
        public int SelectedHTML5CreativeId { get; set; }
        public int SelectedHTML5AdCreativeId { get; set; }
        public IList<AdCreativeSaveDto> VideoEndCards { get; set; }

        public IList<ClickTagTrackerDto> ClickTags { get; set; }

        public IList<ThirdPartyTrackerDto> ThirdPartyTrackers { get; set; }
        public string ImpressionTrackerJSRedirect { get; set; }

        public IList<AdCreativeUnitDto> Creatives { get; set; }

        public IList<AdCreativeUnitDto> IconCreatives { get; set; }

        public string  impressionTrackerRedirect { get; set; }




        public int CreativeUnit_VidoeDocId { get; set; }
        public int CreativeUnit_ThumbnailDocId { get; set; }
        public int CreativeUnitId { get; set; }
        public int CreativeUnit_BitRate { get; set; }

        public int CreativeUnit_VideoWidth { get; set; }
        public int CreativeUnit_VideoHeight { get; set; }
        public int CreativeUnit_Duration { get; set; }
        public string CreativeUnit_VideoFormat { get; set; }

        public IList<string> ImpressionClickTracker { get; set; }
    }

    public class TrackersEvent
    {
        public string EventName { get; set; }
        public IList<string> URLs { get; set; }
        


    }
    public class html5ListModel {
        public AdSubTypes? AdSubType { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }


    }

}
