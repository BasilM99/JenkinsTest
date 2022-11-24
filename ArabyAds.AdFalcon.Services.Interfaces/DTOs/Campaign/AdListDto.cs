using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdbIDListDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }

       [ProtoMember(2)]
        public string Name { get; set; }

       [ProtoMember(3)]
        public string Bid { get; set; }
       
    }

    [ProtoContract]
    public class AdListDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public int CampaignId { get; set; }
       [ProtoMember(3)]
        public int AdGroupId { get; set; }
       [ProtoMember(4)]
        public string Name { get; set; }
       [ProtoMember(5)]
        public decimal Bid { get; set; }
       [ProtoMember(6)]
        public decimal DiscountedBid { get; set; }
       [ProtoMember(7)]
        public string Status { get; set; }
       [ProtoMember(8)]
        public DateTime CreationDate { get; set; }
       [ProtoMember(9)]
        public AdPerformance Performance { get; set; }

       [ProtoMember(10)]
        public int StatusId { get; set; }
    }
}
