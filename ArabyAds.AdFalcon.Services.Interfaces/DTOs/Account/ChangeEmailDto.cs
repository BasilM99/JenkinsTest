using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class ChangeEmailDto
    {
        [ProtoMember(1)]
        public string Hashing { get; set; }
        [ProtoMember(2)]
        public bool duplicateBuyer { get; set; }
        [ProtoMember(3)]
        public string ActivationCode { get; set; }
        [ProtoMember(4)]

        public int? buyerId { get; set; }
    }
}
