using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class AdRequestTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public string MinimumVersion { get; set; }
        [DataMember]
        public AdRequestTypeDto AdRequestType { get; set; }
        [DataMember]
        public AdRequestPlatformDto AdRequestPlatform { get; set; }


        [DataMember]
        public int AdRequestTypeId { get; set; }
        [DataMember]
        public int AdRequestPlatformId { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public int campaignId { get; set; }
    }
    [DataContract]
    public class AdRequestTargetingDtoResultDto
    {
        [DataMember]
        public IEnumerable<AdRequestTargetingDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}
