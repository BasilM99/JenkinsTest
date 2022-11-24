
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class ImpressionMetricTargetingDto : TargetingBaseDto
    {
        [DataMember]
        [Range(0, 1, ResourceName = "MaxPayment")]

        public float MinValue { get; set; }


        [DataMember]
    

        public string MinValuePercantage { get {

                return String.Format("{0:0.##%}", MinValue);
                //return String.Format("{0:0.##\\%}", MinValue); 

            } set { } }
        [DataMember]
        public ImpressionMetricDto ImpressionMetric { get; set; }

        [DataMember]
        public MetricVendorDto MetricVendor { get; set; }

        [DataMember]
        public bool Ignore { get; set; }


        [DataMember]
        public int ImpressionMetricId { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public int campaignId { get; set; }

    }
    [DataContract]
    public class ImpressionMetricTargetingResultDto
    {
        [DataMember]
        public IEnumerable<ImpressionMetricTargetingDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}