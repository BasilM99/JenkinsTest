using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class DeviceCapabilityDto : LookupDto
    {
       [ProtoMember(1)]
        public bool Selected { get; set; }

       [ProtoMember(2)]
        public bool IsInclude { get; set; }

       [ProtoMember(3)]
        [Required(ResourceName = "RequiredMessage")]
        public string WurflCapabilities { get; set; }

       [ProtoMember(4)]
        [Required(ResourceName = "RequiredMessage")]
        public string WurflValue { get; set; }

       [ProtoMember(5)]
        [Required(ResourceName = "RequiredMessage")]
        public int Type { get; set; }

        public DeviceCapabilityDto Clone()
        {
            return (DeviceCapabilityDto)this.MemberwiseClone();
        }
    }
}
