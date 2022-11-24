using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class BidDto
    {
       [ProtoMember(1)]
        public int[] Operators { get; set; }
       [ProtoMember(2)]
        public int[] Geographies { get; set; }
       [ProtoMember(3)]
        public int[] Manufacturers { get; set; }
       [ProtoMember(4)]
        public int[] Platforms { get; set; }
       [ProtoMember(5)]
        public int[] Keywords { get; set; }
       [ProtoMember(6)]
        public int[] Models { get; set; }
       [ProtoMember(7)]
        public int? Demographic { get; set; }
       [ProtoMember(8)]
        public int DeviceTargetingTypeId { get; set; }
       [ProtoMember(9)]
        public int ActionType { get; set; }
       [ProtoMember(10)]
        public int? AdTypeId { get; set; }
       [ProtoMember(11)]
        public int[] DeviceCapabilities { get; set; }

    }
}
