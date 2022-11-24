using System.Runtime.Serialization;
using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdsSearchDto
    {
        [DataMember]
        public int AdvertiserId { get; set; }
        [DataMember]
        public string AdvertiserName { get; set; }

        
        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public string AdvertiserAccountName { get; set; }


        [DataMember]
        public string CampaignName { get; set; }
        [DataMember]
        public AdGroupDto AdGroup { get; set; }
        [DataMember]
        public AdGroupPerformanceDto Performance { get; set; }
        [DataMember]
        public IEnumerable<AdListDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
        [DataMember]
        public  bool IsClientLocked { get; set; }


        [DataMember]
        public bool IsClientReadOnly { get; set; }

    }
}
