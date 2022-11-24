using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SaveUserTokenRequest
    {
        [ProtoMember(1)]
        public string Email { get; set; }
        [ProtoMember(2)]
        public string Token { get; set; }
      
    }
}
