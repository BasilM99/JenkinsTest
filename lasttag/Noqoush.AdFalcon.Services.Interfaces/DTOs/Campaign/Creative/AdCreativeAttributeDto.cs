using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdCreativeAttributeDto : LookupDto
    {
        [DataMember]
        public virtual bool IsSupported { get; set; }

        [DataMember]
        [Required]
        public virtual int Code  { get; set; }

        [DataMember]
        [StringLength(200)]
        public virtual string Description { get; set; }

    }
}
