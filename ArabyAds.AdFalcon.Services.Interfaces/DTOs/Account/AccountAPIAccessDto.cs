using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class AccountAPIAccessDto
    {
       [ProtoMember(1)]
        public string APISecretKey { get; set; }

       [ProtoMember(2)]
        public string APIClientId { get; set; }

       [ProtoMember(3)]
        public int AccountId { get; set; }

       [ProtoMember(4)]
        public bool AllowAPIAccess { get; set; }
    }
}
