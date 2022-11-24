using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.AppSite
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
        public string ModifiedNotCompatableBidConfigs { get; set; }

    }
}
