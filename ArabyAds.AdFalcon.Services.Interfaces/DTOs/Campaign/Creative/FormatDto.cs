using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class FormatDto : LookupDto
    {
       
       [ProtoMember(1)]
        public string Format { get; set; }

       [ProtoMember(2)]
        public virtual int MaxSize { get; set; }
    }
}
