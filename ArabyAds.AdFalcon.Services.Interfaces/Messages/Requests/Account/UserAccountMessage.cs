using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UserAccountMessage
    {
        [ProtoMember(1)]
        public int AccountId { get; set; }
        [ProtoMember(2)]
        public int? UserId { get; set; }

        public override string ToString()
        {
            return $"{AccountId}_{UserId?.ToString() ?? "Null"}";
        }
    }
}
