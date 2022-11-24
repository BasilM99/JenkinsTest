using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class HouseAdDto
    {
        [ProtoMember(1)]
        public int ID { get;set; }
        [ProtoMember(2)]
        public AdGroupDto AdGroup { get; set; }
        [ProtoMember(3)]
        public bool IsDeleted { get; set; }
        [ProtoMember(4)]
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        [ProtoMember(5)]
        public AppSiteBasicDto ForAppSite { get; set; }
        [ProtoMember(6)]
        public IList<AppSiteBasicDto> DestinationAppSites { get; set; } = new List<AppSiteBasicDto>();
    }

    [ProtoContract]
    public class HouseAdGroupDto
    {
        [ProtoMember(1)]
        public int ID { get;set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public int CampaignId { get; set; }
        [ProtoMember(4)]
        public bool IsDeleted { get; set; }
        [ProtoMember(5)]
        public HouseAdDeliveryMode DeliveryMode { get; set; }
        [ProtoMember(6)]
        public int ForAppSite { get; set; }
        [ProtoMember(7)]
        public IList<int> DestinationAppSites { get; set; }
    }
}
