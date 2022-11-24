using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class CheckInvitationAlreadyRegistredRequest
    {
        [ProtoMember(1)]
        public string Email { get; set; }
        [ProtoMember(2)]
        public string Invitation { get; set; }

        public override string ToString()
        {
            return $"{Email ?? "Null"}_{Invitation ?? "Null"}";
        }

    }
}
