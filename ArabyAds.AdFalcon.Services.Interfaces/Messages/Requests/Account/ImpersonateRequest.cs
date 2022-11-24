using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class ImpersonateRequest
    {
        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public int? UserId { get; set; }
        public override string ToString()
        {
            return $"{AccountId?.ToString() ?? "Null"}_{UserId?.ToString() ?? "Null"}";
        }

    }
}
