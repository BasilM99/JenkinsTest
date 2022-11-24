using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    public class AgeGroupDto : LookupDto
    {
       [ProtoMember(1)]
        public int MinValue { get; set; }
       [ProtoMember(2)]
        public int MaxValue { get; set; }
    }
}
