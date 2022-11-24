using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class InviteRequest
    {
        [ProtoMember(1)]
        public string Email { get; set; }
        [ProtoMember(2)]
        public UserType UserType { get; set; }
        [ProtoMember(3)]
        public string IdAdvs { get; set; }

        public override string ToString()
        {
            return $"{Email ?? "Null"}_{UserType}_{IdAdvs ?? "Null"}";
        }

    }
}
