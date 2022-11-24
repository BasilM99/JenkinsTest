using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupSearchDto
    {
        [DataMember]
        public string CampaignName { get; set; }
        [DataMember]
        public CampaignPerformanceDto Performance { get; set; }
        [DataMember]
        public IEnumerable<AdGroupListDto> Items { get; set; }

        [DataMember]
        public string AdvertiserName { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }


        [DataMember]
        public string AdvertiserAccountName { get; set; }
        [DataMember]
        public int AdvertiserAccountId { get; set; }


        [DataMember]
        public long TotalCount { get; set; }
    }

    //[DataContract]
    //public class PerformanceDto
    //{
    //    [DataMember]
    //    public Decimal Bid { get; set; }
    //    [DataMember]
    //    public string Objective { get; set; }
    //    [DataMember]
    //    public int Impressions { get; set; }
    //    [DataMember]
    //    public int Clicks { get; set; }
    //    [DataMember]
    //    public int CTR { get; set; }
    //    [DataMember]
    //    public float AvgCPC { get; set; }
    //    [DataMember]
    //    public Decimal Spend { get; set; }
    //}
}
