using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public enum CampignFrequencyCappingEnum
    {[EnumMember]
        Default = 1,
        [EnumMember]
        Capping = 2,
       
        [EnumMember]
        NoCapping = 3,
             [EnumMember]
        CappingLifeTime = 4
    }
    [DataContract]
    public class CampaignFrequencyCappingSaveDto
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public int Type { get; set; }

      
        [DataMember]
        public int CampignFrequencyCappingStatus { get; set; }

    }
}
