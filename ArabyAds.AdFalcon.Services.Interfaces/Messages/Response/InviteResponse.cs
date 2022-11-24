using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class InviteResponse
    {
        [ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoMember(2)]
        public string Invitationcode { get; set; }
    }
}
