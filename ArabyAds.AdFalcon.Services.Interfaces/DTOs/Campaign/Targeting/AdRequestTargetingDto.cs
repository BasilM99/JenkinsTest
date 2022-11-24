using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class AdRequestTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public string MinimumVersion { get; set; }
       [ProtoMember(2)]
        public AdRequestTypeDto AdRequestType { get; set; }
       [ProtoMember(3)]
        public AdRequestPlatformDto AdRequestPlatform { get; set; }


       [ProtoMember(4)]
        public int AdRequestTypeId { get; set; }
       [ProtoMember(5)]
        public int AdRequestPlatformId { get; set; }
       [ProtoMember(6)]
        public int AdGroupId { get; set; }
       [ProtoMember(7)]
        public int campaignId { get; set; }
    }
    [ProtoContract]
    public class AdRequestTargetingDtoResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AdRequestTargetingDto> Items { get; set; } = new List<AdRequestTargetingDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}
