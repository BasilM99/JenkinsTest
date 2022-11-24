using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetBaseFloorPriceRequest
    {
        [ProtoMember(1)]
        public int SiteId { get; set; }
        [ProtoMember(2)]
        public int ZoneId { get; set; }

        public override string ToString()
        {
            return $"{SiteId}_{ZoneId}";
        }
    }
}
