using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    public class VideoDeliveryMethodDto : LookupDto
    {
       [ProtoMember(1)]
        public string Code { get; set; }
    }
}
