using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdCreativeAttributeDto : LookupDto
    {
       [ProtoMember(1)]
        public virtual bool IsSupported { get; set; }

       [ProtoMember(2)]
        [Required]
        public virtual int Code  { get; set; }

       [ProtoMember(3)]
        [StringLength(200)]
        public virtual string Description { get; set; }

    }
}
