using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdActionValueDto
    {
       [ProtoMember(1)]

       [ArabyAds.Framework.DataAnnotations.Required()]
        public string Value { get; set; }

       [ProtoMember(2)]
        public string Value2 { get; set; }

       [ProtoMember(3)]
        public IList<AdActionValueTrackerDto> Trackers { get; set; } = new List<AdActionValueTrackerDto>();
    }

}
