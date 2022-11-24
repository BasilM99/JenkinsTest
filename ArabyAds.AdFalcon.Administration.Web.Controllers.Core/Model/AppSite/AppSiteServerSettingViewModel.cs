using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.AppSite
{
    public class AppSiteServerSettingViewModel : AppSiteUpdateBase
    {
        public AppSiteAdminConfigDto SettingsDto { get; set; }
        public bool IsSupportBannerAd { get; set; }
        public bool IsSupportTextAd { get; set; }
        public IEnumerable<SelectListItem> ImageFormats { get; set; }
        public string[] SupportedBannerImageTypeIds { get; set; }
        public IEnumerable<TrackingEventDto> CostModelEvents { get; set; }
        public IList<SelectListItem> NativeAdLayouts { get; set; }
        public IList<CampaignBidConfigDto> ModifiedNotCompatableBidConfigs { get; set; }
        public int id { get; set; }
        public string CalculationMode{ get; set; }
        public int? pricingModel { get; set; }
        public string[] FloorPrice { get; set; }

        public string ModifiedNotCompatableBidConfigsStr { get; set; }

    }
}
