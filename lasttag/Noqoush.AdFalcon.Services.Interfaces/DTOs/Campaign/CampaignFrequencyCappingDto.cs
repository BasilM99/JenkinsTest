using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignFrequencyCappingDto
    {
        public CampaignFrequencyCappingDto()
        {
            CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Default;
            IsCappingValue = CampignFrequencyCappingStatus.ToString();

        }
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public string EventDescription { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public string NumberName { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public string IntervalName { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public bool IsCapping { get; set; }

        [DataMember]
        public string IsCappingValue { get; set; }
        [DataMember]
        public CampignFrequencyCappingEnum CampignFrequencyCappingStatus { get; set; }
    }
}
