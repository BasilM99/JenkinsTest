using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetLookupTextByCodeRequest
    {
        [ProtoMember(1)]
        public string Code { set; get; }
        [ProtoMember(2)]
        public string LookupType { set; get; }

        public override string ToString()
        {
            return $"{Code ?? "Null"}_{LookupType ?? "Null"}";
        }
    }
}
