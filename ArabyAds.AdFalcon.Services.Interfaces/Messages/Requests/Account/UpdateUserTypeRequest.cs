using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UpdateUserTypeRequest
    {
        [ProtoMember(1)]
        public int UserId { get; set; }
        [ProtoMember(2)]
        public string Ids { get; set; }
        [ProtoMember(3)]
        public UserType UserType { get; set; }

    }
}
