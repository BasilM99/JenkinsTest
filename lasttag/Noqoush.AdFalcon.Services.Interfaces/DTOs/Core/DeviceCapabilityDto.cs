using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class DeviceCapabilityDto : LookupDto
    {
        [DataMember]
        public bool Selected { get; set; }

        [DataMember]
        public bool IsInclude { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string WurflCapabilities { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string WurflValue { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public int Type { get; set; }

        public DeviceCapabilityDto Clone()
        {
            return (DeviceCapabilityDto)this.MemberwiseClone();
        }
    }
}
