using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class FormattedContentDto
    {
       [ProtoMember(1)]
        public string Content { get; set; }

       [ProtoMember(2)]
        public bool IsValid { get; set; }
    }
}
