using ProtoBuf;
using System.Collections.Generic;
using System;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupSearchDto
    {
       [ProtoMember(1)]
        public string CampaignName { get; set; }
       [ProtoMember(2)]
        public CampaignPerformanceDto Performance { get; set; }
       [ProtoMember(3)]
        public IEnumerable<AdGroupListDto> Items { get; set; } = new List<AdGroupListDto>();

        [ProtoMember(4)]
        public string AdvertiserName { get; set; }
       [ProtoMember(5)]
        public int AdvertiserId { get; set; }


       [ProtoMember(6)]
        public string AdvertiserAccountName { get; set; }
       [ProtoMember(7)]
        public int AdvertiserAccountId { get; set; }


       [ProtoMember(8)]
        public long TotalCount { get; set; }
    }

    //[ProtoContract]
    //public class PerformanceDto
    //{
    //   [ProtoMember()]
    //    public Decimal Bid { get; set; }
    //   [ProtoMember()]
    //    public string Objective { get; set; }
    //   [ProtoMember()]
    //    public int Impressions { get; set; }
    //   [ProtoMember()]
    //    public int Clicks { get; set; }
    //   [ProtoMember()]
    //    public int CTR { get; set; }
    //   [ProtoMember()]
    //    public float AvgCPC { get; set; }
    //   [ProtoMember()]
    //    public Decimal Spend { get; set; }
    //}
}
