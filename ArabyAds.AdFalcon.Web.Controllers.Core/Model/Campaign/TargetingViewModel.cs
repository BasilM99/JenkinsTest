using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Web.Controllers.Model.PMPDeal;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ArabyAds.AdFalcon.Web.Controllers.Model.HouseAd;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class TargetingSaveDto
    {
        public string[] NewKeywords { get; set; }
        public string[] DeletedKeywords { get; set; }
    }

    public class TargetingViewModel
    {

        public IEnumerable<SelectListItem> BidOptimizationTypeList { get; set; }
        public string UniqueId { get; set; }
        public bool AllowInclude { get; set; }
        public bool ExcludeSensitiveCategories { get; set; }
        public AdGroupSettingsViewModel AdGroupSettings { get; set; }
        public decimal? AudianceDiscountPrice { get; set; }
        public int? ViewabilityVendorId { get; set; }
        public bool IsClientReadOnly { get; set; }
        public bool HasGeofencingTargeting { get; set; }
        public bool HasInternalDPPartner { get; set; }
        public string groupAudianceString { get; set; }
        public List<AudienceSegmentDto> BrandSafetySegments { get; set; }
        public List<AudienceSegmentDto> ContextualSegments { get; set; }
        public PMPDealListViewModel PMPDealListViewModel { get; set; }
        public IEnumerable<PMPDealDto> PMPDealConfigList { get; set; }
        public InventorySourceSaveDTo InventorySourceSaveDTo { get; set; }
        public HouseAdSaveModel HouseAdSave { get; set; }
        
        public bool AllowBidToBeZero { get; set; }
        public bool AdPosition_Unknown { get; set; }

   
        public bool AdPosition_AboveTheFold { get; set; }

        public bool AdPosition_BelowTheFold { get; set; }
      
        public bool AdPosition_Enabled { get; set; }
        public int CountExternalAudienceList { get; set; }
        public int ParentIdOfFirstParty { get; set; }
        public decimal MaxDataBidContextual { get; set; }
        public decimal MaxDataBidBrandSafety { get; set; }
        public int ParentIdOfFirstPartyContextual { get; set; }

        public string DataPriceAudienceSegment { get; set; }

        public int? TargetingConnectionType { get; set; }
        public string DeletedPMPDealConfigs { get; set; }
        public string InsertePMPDealConfigs { get; set; }

        public IEnumerable<AdvertiserAccountMasterAppSiteDto> MasterListConfigList { get; set; }

        public string DeletedMasterListConfigs { get; set; }
        public string InserteMasterListConfigs { get; set; }

        public CampaignBidConfigSaveDTo campaignBidConfigSaveDTo { get; set; }
        public int AdGroupId { get; set; }

        public int AdvertiserId { get; set; }
        
        public TreeViewModel Audiences { get; set; }
        public TreeViewModel Contextuals { get; set; }
        public TreeViewModel BrandSafety { get; set; }

        public int AdvertiserAccountId { get; set; }
        public int CampaignId { get; set; }
        public string AdvertiserAccountName { get; set; }
        public string AdvertiserName { get; set; }
        public string AdGroupName { get; set; }
        public string CampaignName { get; set; }
        public int AdActionTypeId { get; set; }
        public int? AdTypeId { get; set; }
        public group group { get; set; }
        public bool AllowOpenAuction { get; set; }
        public bool IsClientLocked { get; set; }
        public bool IsHasAds { get; set; }

        public string changedAudiances { get; set; }
        public string changedContextuals { get; set; }
        public string changedBrandSafety { get; set; }

        [RegularExpression(@"^\$?\d{1,5}(\.(\d{1,3}))?$", ResourceName = "CurrencyMsg")]
        [Range(0.01, 99999999, ResourceName = "MinBidMsg")]
        [Required()]
        public decimal Bid { get; set; }
        public decimal? DataBid { get; set; }
        public decimal? MaxDataBid { get; set; }
        public decimal? DailyBudget { get; set; }
        public decimal? Budget { get; set; }
     

        public decimal? DiscountedBid { get; set; }
        public DiscountDto DiscountDto { get; set; }


        public IEnumerable<SelectListItem> CostModels { get; set; }
        public KeywordTargetingViewModel KeywordTargetingViewModel { get; set; }

        public GeographicViewModel Geographics { get; set; }
        public DeviceTargetingViewModel DeviceTargetingView { get; set; }
        public OperaterViewModel OperaterTargetingView { get; set; }
        public IEnumerable<URLTargetingView> URLs { get; set; }
        public CampaignBidConfigModel CampaignBidConfigModel { get; set; }
        public InventorySourceModel InventorySourceModel { get; set; }
        public DemographicTargetingViewModel DemographicTargetingView { get; set; }
        public int BiddingStrategy { get; set; }
        public bool TrackInstalls { get; set; }

        public bool OpenInExternalBrowser { get; set; }
        
        public bool LoadDetaultTrackingEvents { get; set; }

        public int? CostModelWrapperId { get; set; }

        public bool IsPricingModelChanged { get; set; }

        public bool IsVideoActionType { get; set; }
        #region VideoTargeting

        public  bool PlacementType_InStream { get; set; }
        public  bool PlacementType_OutStream { get; set; }
        public  bool PlacementType_Interstitial { get; set; }
        public  bool PlacementType_Undetermined { get; set; }



        public  bool InStreamPosition_PreRoll { get; set; }
        public  bool InStreamPosition_MidRoll { get; set; }
        public  bool InStreamPosition_PostRoll { get; set; }

        public  bool InStreamPosition_Undetermined { get; set; }


        public  bool SkippableAds_SkippableAdSpaces { get; set; }
        public  bool SkippableAds_NonSkippableAdSpaces { get; set; }

        public  bool SkippableAds_Undetermined { get; set; }

        public  bool Playback_AutoPlaySoundOn { get; set; }

        public  bool Playback_AutoPlaySoundOff { get; set; }
        public  bool Playback_ClickToPlay { get; set; }
        public bool RewardedAdOnly { get; set; }
        public  bool Playback_Undetermined { get; set; }
        public  bool Video_RewardedAds { get; set; }
        public  bool Video_MatchOrientation { get; set; }
        public bool IsHouseAd { get; set; }


        #endregion

        public IEnumerable<SelectListItem> ExternalDataProvider { get; set; }
        public IEnumerable<SelectListItem> FeesList { get; set; }
        public IList<SelectListItem> MetricVendors { get; set; }

        public IList<AdGroupFeeDto> FeesAddList { get; set; }




   


        public decimal BidOptimizationValue { get; set; }
     
      
     
        public decimal MaxBidPrice { get; set; }
   

     
    
        public bool KeepBiddingAtMinimum { get; set; }
    
     
        public int BidOptimizationType { get; set; }

        public List<ushort> EpochValues { get; set; }



      
        public ConversionSetting ConversionSetting { get; set; }
     
        public ConversionType ConversionType { get; set; }
      
        public int ViewAttribuation { get; set; }
     
        public int ClickAttribuation { get; set; }
       
        public int CountingAttribuation { get; set; }
        public CountingTypeAttribuation CountingTypeAttribuation { get; set; }

        public AdGroupTrackingEventResultDto AdEvents { get; set; }

        public AdGroupConversionEventResultDto ConversionEvents { get; set; }
        public int MaxAdGroupTrackingEvents { get; set; }

        #region Conversion and events


        public IList<AdGroupConversionEventDto> ConversionItems { get; set; }


        public IList<AdGroupTrackingEventDto> AdEventItems { get; set; }


        public  int ConversionIndexItems { get; set; }


        public  int AdEventIndexItems { get; set; }

        #endregion


        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; }




        public bool AudienceAllowed { get; set; }
        public bool PMPDealAllowed { get; set; }
        public bool InventorySourcesAllowed { get; set; }

        public IList<SelectListItem> trackingEventsNamesIdsList { get; set; }
        public IList<SelectListItem> trackingEventsCodesIdsList { get; set; }

        public int ActionTypeCode { get; set; }
        public IList<DimensionType> dimentionTypes { get; set; }

        public IList<SelectListItem> ProvidersList { get; set; }
        public IList<SelectListItem> CostElementsItems { get; set; }

    }
    public class DimensionType {
        public string label { get; set; }
        public int value { get; set; }
        public int type { get; set; }

       // label = m.Name, value = m.Id, type = m.DimensionType

    }

    public class BidModifierModel {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; }
    }


    public class URLTargetingView
    {
        public int ID { get; set; }

       [Required(ResourceName = "UrlMsg")]
       
        public string URL { get; set; }
    }

    public class IPTargetingView
    {
        public int ID { get; set; }

        [Required(ResourceName = "IPNotValid")]
        public string StartRange { get; set; }

        // [Required(ResourceName = "IPNotValid")]
        public string EndRange { get; set; }

        public string Description { get; set; }
    }

    public class KeywordTargetingViewModel
    {
     
        public KeywordViewModel KeywordViewModel { get; set; }
        public IEnumerable<KeywordTargetingDto> KeywordsTargeting { get; set; }
    }
    public class DeviceTargetingViewModel
    {
        public int Type { get; set; }
        public List<PlatformDto> Platforms { get; set; }
        public TreeViewModel Manufacturers { get; set; }
        public TreeViewModel DevicesTree { get; set; }
        public TreeViewModel Devices { get; set; }
        public IEnumerable<DeviceTargetingDto> deviceTargeting { get; set; }
        public IEnumerable<DeviceCapabilityDto> DeviceCapabilities { get; set; }
        public DeviceTypeViewModel DeviceTypeModel { get; set; }
        public IEnumerable<LanguageTargetingDto> LanguagesTargeting { get; set; }
        public IEnumerable<SelectListItem> Languages { get; set; }
        public int[] LanguageType { get; set; }

    }


    public class DeviceTypeViewModel
    {
        public IEnumerable<SelectListItem> DeviceTypes { get; set; }
    }


    public class GeographicViewModel
    {
        public TreeViewModel GeographicalAreas { get; set; }

        public IList<GeoFencingTargetingDto> GeoFencings { get; set; }
    }
    public class AudienceViewModel {
        public TreeViewModel GeographicalAreas { get; set; }
        public TreeViewModel Audiences { get; set; }
    }
    public class OperaterViewModel
    {
        public TreeViewModel Operaters { get; set; }
        public IList<IPTargetingView> IPRanges { get; set; }
        [RegularExpression(@"^.*\.(txt|text)$", ResourceName = "InvalidIPRangeUploadFileType")]
        public IFormFile IPFile { get; set; }
        public bool DisableProxyTraffic { get; set; }
        public bool IsWifi { get; set; }

        public bool IsCellular { get; set; }

        public int? TargetingConnectionType { get; set; }



       

  
    }
    public class DemographicTargetingViewModel
    {
        public DemographicTargetingDto DemographicTargeting { get; set; }
        public IEnumerable<SelectListItem> AgeGroups { get; set; }

    }


    public class ImpressionMetricViewModel
    {
        public int AdGroupId { set; get; }

        public ImpressionMetricTargetingResultDto AllItems { set; get; }
        public ImpressionMetricViewDialogModel ImpressionMetricDialog { set; get; }

    }

    public class ImpressionMetricViewDialogModel
    {
        public IList<SelectListItem> ImpressionMetrics { set; get; }
        public IList<SelectListItem> MetricVendors { set; get; }


    }
}
