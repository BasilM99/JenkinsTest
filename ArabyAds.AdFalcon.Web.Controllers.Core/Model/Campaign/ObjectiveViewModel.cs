using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class ObjectiveViewModel
    {
        public IEnumerable<AdActionTypeDto> AdActionTypes { get; set; }
        public IEnumerable<ObjectiveTypeDto> Items { get; set; }
        [Required(ResourceName = "GroupNameRequiredMsg", ResourceSet = "Msgs")]
        [StringLength(255, ResourceName = "GroupNamLengthMsg", ResourceSet = "Msgs")]
        public string Name { get; set; }
        [Required(ResourceName = "ActionTypeRequiredMsg", ResourceSet = "Msgs")]
        public int ActionTypeId { get; set; }
        public bool IsNativeAd { get; set; }
        [Required(ResourceName = "ObjectiveTypeRequiredMsg", ResourceSet = "Msgs")]
        public int ObjectiveTypeId { get; set; }
        public int CampaignId { get; set; }
        public int AdvertiserId { get; set; }
        public string AdvertiserAccountName { get; set; }
        public string AdvertiserName { get; set; }
        public int id { get; set; }
        public int AdvertiserAccountId { get; set; }
        public bool NativeShow { get; set; }
    }


}
