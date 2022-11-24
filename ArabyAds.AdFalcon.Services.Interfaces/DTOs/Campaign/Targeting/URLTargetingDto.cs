using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class URLTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public string URL { get; set; }
    }
}
