using ProtoBuf;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdsSearchDto
    {
       [ProtoMember(1)]
        public int AdvertiserId { get; set; }
       [ProtoMember(2)]
        public string AdvertiserName { get; set; }

        
       [ProtoMember(3)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(4)]
        public string AdvertiserAccountName { get; set; }


       [ProtoMember(5)]
        public string CampaignName { get; set; }
       [ProtoMember(6)]
        public AdGroupDto AdGroup { get; set; }
       [ProtoMember(7)]
        public AdGroupPerformanceDto Performance { get; set; }
       [ProtoMember(8)]
        public IEnumerable<AdListDto> Items { get; set; } = new List<AdListDto>();
        [ProtoMember(9)]
        public long TotalCount { get; set; }
       [ProtoMember(10)]
        public  bool IsClientLocked { get; set; }


       [ProtoMember(11)]
        public bool IsClientReadOnly { get; set; }

    }
}
