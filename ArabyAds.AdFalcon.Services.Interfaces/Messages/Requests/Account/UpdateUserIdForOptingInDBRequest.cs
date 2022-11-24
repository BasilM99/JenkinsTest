using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UpdateUserIdForOptingInDBRequest
    {
        [ProtoMember(1)]
        public string UserId { get; set; }
        [ProtoMember(2)]
        public bool TrackEnabled { get; set; }
      
    }
}
