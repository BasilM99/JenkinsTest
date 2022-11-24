using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class LanguageTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public LanguageDto Language { get; set; }

    }
    [ProtoContract]
    public class DeviceTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public bool IsAll { get; set; }
       [ProtoMember(2)]
        public DeviceTargetingTypeDto TargetingType { get; set; }
       [ProtoMember(3)]
        public IEnumerable<DeviceDto> Devices { get; set; } = new List<DeviceDto>();
        [ProtoMember(4)]
        public IEnumerable<ManufacturerDto> Manufacturers { get; set; } = new List<ManufacturerDto>();
       [ProtoMember(5)]
        public IEnumerable<PlatformDto> Platforms { get; set; } = new List<PlatformDto>();
        [ProtoMember(6)]
        public IEnumerable<DeviceCapabilityDto> DeviceCapabilities { get; set; } = new List<DeviceCapabilityDto>();
        [ProtoMember(7)]
        public IEnumerable<DeviceTypeDto> DeviceTypes { get; set; } = new List<DeviceTypeDto>();
    }
}
