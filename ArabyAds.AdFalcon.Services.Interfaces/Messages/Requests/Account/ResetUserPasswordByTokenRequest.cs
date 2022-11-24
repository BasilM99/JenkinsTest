using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class ResetUserPasswordByTokenRequest
    {
        [ProtoMember(1)]
        public string Token { get; set; }
        [ProtoMember(2)]
        public string NewPassword { get; set; }

        public override string ToString()
        {
            return $"{Token ?? "Null"}_{NewPassword ?? "Null"}";
        }

    }
}
