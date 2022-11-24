using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    public enum CampignFrequencyCappingEnum
    {
        Default = 1,
        Capping = 2,
        NoCapping = 3,
        CappingLifeTime = 4
    }
    [ProtoContract]
    public class CampaignFrequencyCappingSaveDto
    {

       [ProtoMember(1)]
        public int Id { get; set; }

       [ProtoMember(2)]
        public int EventId { get; set; }

       [ProtoMember(3)]
        public int Number { get; set; }

       [ProtoMember(4)]
        public int Interval { get; set; }

       [ProtoMember(5)]
        public int Type { get; set; }

      
       [ProtoMember(6)]
        public int CampignFrequencyCappingStatus { get; set; }

    }
}
