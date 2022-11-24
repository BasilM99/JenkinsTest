using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Web.Controllers.Model.HouseAd
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
    }
}
