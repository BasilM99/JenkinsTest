using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    public class HouseAdDto
    {
        public int ID { get;set; }
        public AdGroupDto AdGroup { get; set; }
        public bool IsDeleted { get; set; }
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        public AppSiteBasicDto ForAppSite { get; set; }
        public IList<AppSiteBasicDto> DestinationAppSites { get; set; }
    }

    public class HouseAdGroupDto
    {
        public int ID { get;set; }
        public string Name { get; set; }
        public int CampaignId { get; set; }
        public bool IsDeleted { get; set; }
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        public int ForAppSite { get; set; }
        public IList<int> DestinationAppSites { get; set; }
    }
}
