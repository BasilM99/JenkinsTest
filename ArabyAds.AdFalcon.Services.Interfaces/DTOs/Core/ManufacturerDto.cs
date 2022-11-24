using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class ManufacturerDto : LookupDto
    {
       [ProtoMember(1)]
        [ArabyAds.Framework.DataAnnotations.Required()]
        public int Order { get; set; }
    }
}
