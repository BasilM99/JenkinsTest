using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SearchByQueryTreeRequest
    {
        [ProtoMember(1)]
        public int DeviceTypeId { set; get; }
        [ProtoMember(2)]
        public string Query { set; get; }

        public override string ToString()
        {
            return $"{DeviceTypeId}_{Query ?? "Null"}";
        }
    }
}
