using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.HouseAd
{
    public class HouseAdSaveModel
    {
        [Required(ResourceName = "GroupNameRequiredMsg", ResourceSet = "Msgs")]
        [StringLength(255, ResourceName = "GroupNamLengthMsg", ResourceSet = "Msgs")]
        public string Name { get; set; }
        public int CampaignId { get; set; }
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        [Required()]
        public int ForAppSite { get; set; }
        [Required(ResourceName = "DestinationAppSitesRequiredMsg", ResourceSet = "Msgs")]
        public string DestinationAppSites { get; set; }

        public IList<AppSiteBasicDto> TargetAppSites { get; set; } = new List<AppSiteBasicDto>();
        public AppSiteBasicDto InitiateAppSites { get; set; } 

    }
}
