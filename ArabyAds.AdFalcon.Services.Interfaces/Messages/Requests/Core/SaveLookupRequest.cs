using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SaveLookupRequest
    {
        [ProtoMember(1)]
        public LookupDto Data { set; get; }
        [ProtoMember(2)]
        public string LookType { set; get; }
    }
}
