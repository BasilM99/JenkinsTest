using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{

    [DataContract]
    public class DeviceDto : LookupDto
    {
        [DataMember]
        public PlatformDto Platform { get; set; }
        [DataMember]

        public ManufacturerDto Manufacturer { get; set; }
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [DataMember]
        public virtual int DeviceTypeId { get; set; }
    }
}
