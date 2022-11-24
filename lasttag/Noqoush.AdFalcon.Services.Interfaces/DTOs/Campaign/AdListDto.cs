using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdbIDListDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Bid { get; set; }
       
    }

    [DataContract]
    public class AdListDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Bid { get; set; }
        [DataMember]
        public decimal DiscountedBid { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public AdPerformance Performance { get; set; }

        [DataMember]
        public int StatusId { get; set; }
    }
}
