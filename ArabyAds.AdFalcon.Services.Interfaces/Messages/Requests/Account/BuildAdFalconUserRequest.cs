using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class BuildAdFalconUserRequest
    {
        [ProtoMember(1)]
        public int AccountId { get; set; }
        [ProtoMember(2)]
        public int UserId { get; set; }
        [ProtoMember(3)]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{AccountId}_{UserId}_{Email??"Null"}";
        }
    }
}
