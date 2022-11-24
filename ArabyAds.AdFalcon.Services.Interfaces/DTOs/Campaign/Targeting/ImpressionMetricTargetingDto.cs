
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class ImpressionMetricTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        [Range(0, 1, ResourceName = "MaxPayment")]

        public float MinValue { get; set; }


       [ProtoMember(2)]
    

        public string MinValuePercantage { get {

                return String.Format("{0:0.##%}", MinValue);
                //return String.Format("{0:0.##\\%}", MinValue); 

            } set { } }
       [ProtoMember(3)]
        public ImpressionMetricDto ImpressionMetric { get; set; }

       [ProtoMember(4)]
        public MetricVendorDto MetricVendor { get; set; }

       [ProtoMember(5)]
        public bool Ignore { get; set; }


       [ProtoMember(6)]
        public int ImpressionMetricId { get; set; }
       [ProtoMember(7)]
        public int AdGroupId { get; set; }
       [ProtoMember(8)]
        public int campaignId { get; set; }

    }
    [ProtoContract]
    public class ImpressionMetricTargetingResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<ImpressionMetricTargetingDto> Items { get; set; } = new List<ImpressionMetricTargetingDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}