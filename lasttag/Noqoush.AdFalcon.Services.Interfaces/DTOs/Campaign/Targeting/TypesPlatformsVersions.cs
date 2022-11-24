using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    public class TypesPlatformsVersions
    {
        [DataMember]
        public IList<AdRequestTypePlatformVersionDto> All { get; set; }
        [DataMember]
        public IList<AdRequestTypeDto> Types { get; set; }
        [DataMember]
        public IList<AdRequestPlatformDto> Platforms { get; set; }

    }
}
