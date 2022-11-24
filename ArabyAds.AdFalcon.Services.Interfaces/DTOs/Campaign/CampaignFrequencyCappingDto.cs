using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CampaignFrequencyCappingDto
    {
        public CampaignFrequencyCappingDto()
        {
            CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Default;
            IsCappingValue = CampignFrequencyCappingStatus.ToString();

        }
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public int EventId { get; set; }

       [ProtoMember(3)]
        public string EventName { get; set; }

       [ProtoMember(4)]
        public string EventDescription { get; set; }

       [ProtoMember(5)]
        public int Number { get; set; }

       [ProtoMember(6)]
        public string NumberName { get; set; }

       [ProtoMember(7)]
        public int Interval { get; set; }

       [ProtoMember(8)]
        public string IntervalName { get; set; }

       [ProtoMember(9)]
        public int Type { get; set; }

       [ProtoMember(10)]
        public string TypeName { get; set; }

       [ProtoMember(11)]
        public bool IsCapping { get; set; }

       [ProtoMember(12)]
        public string IsCappingValue { get; set; }
       [ProtoMember(13)]
        public CampignFrequencyCappingEnum CampignFrequencyCappingStatus { get; set; }

        public int CampignFrequencyCappingStatusInt 
        {
            get
            {
                return (int) CampignFrequencyCappingStatus;
            } 
        }

        [ProtoMember(14)]
        public string EventDescriptionStr { get {
                if (!string.IsNullOrEmpty(EventDescription))
                    return EventDescription.Replace(" ", "&nbsp;");
                else
                    return string.Empty;
            
            } set { } }
    }
}
