using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{

    [ProtoContract]
    public class DeviceDto : LookupDto
    {
       [ProtoMember(1)]
        public PlatformDto Platform { get; set; }
       [ProtoMember(2)]

        public ManufacturerDto Manufacturer { get; set; }
       [ProtoMember(3)]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
       [ProtoMember(4)]
        public virtual int DeviceTypeId { get; set; }
    }
}
