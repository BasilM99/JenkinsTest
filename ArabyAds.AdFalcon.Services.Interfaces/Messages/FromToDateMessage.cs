using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    [ProtoInclude(100,typeof(GetAdvertiserAccountTotalSpendRequest))]
    public class FromToDateMessage
    {
        [ProtoMember(1)]
        public DateTime FromDate { get; set; }
        [ProtoMember(2)]
        public DateTime ToDate { get; set; }
    }
}
