using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class TypesPlatformsVersions
    {
       [ProtoMember(1)]
        public IList<AdRequestTypePlatformVersionDto> All { get; set; } = new List<AdRequestTypePlatformVersionDto>();
        [ProtoMember(2)]
        public IList<AdRequestTypeDto> Types { get; set; } = new List<AdRequestTypeDto>();
        [ProtoMember(3)]
        public IList<AdRequestPlatformDto> Platforms { get; set; } = new List<AdRequestPlatformDto>();

    }
}
