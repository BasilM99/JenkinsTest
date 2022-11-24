using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class LanguageTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public LanguageDto Language { get; set; }

    }
    [DataContract]
    public class DeviceTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public bool IsAll { get; set; }
        [DataMember]
        public DeviceTargetingTypeDto TargetingType { get; set; }
        [DataMember]
        public IEnumerable<DeviceDto> Devices { get; set; }
        [DataMember]
        public IEnumerable<ManufacturerDto> Manufacturers { get; set; }
        [DataMember]
        public IEnumerable<PlatformDto> Platforms { get; set; }
        [DataMember]
        public IEnumerable<DeviceCapabilityDto> DeviceCapabilities { get; set; }
        [DataMember]
        public IEnumerable<DeviceTypeDto> DeviceTypes { get; set; }
    }
}
