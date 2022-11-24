using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetResultofDataQBDtoRequest
    {
        [ProtoMember(1)]
        public string Script { set; get; }
        [ProtoMember(2)]
        public string OptionalDrop { set; get; }

        [ProtoMember(3)]
        public string MethodName { set; get; }
        [ProtoMember(4)]
        public int Page { set; get; }
        [ProtoMember(5)]
        public string LookupName { set; get; }
        [ProtoMember(6)]
        public string Ids { set; get; }
        public override string ToString()
        {
            return $"{Script ?? "Null"}_{OptionalDrop ?? "Null"}_{MethodName ?? "Null"}_{Page}";
        }

    }
}
