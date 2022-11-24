using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class HouseAdListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<HouseAdBaseDto> Items { get; set; }
       [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}
