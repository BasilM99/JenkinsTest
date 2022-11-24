using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetColumnsByFactIdRequest
    {
        [ProtoMember(1)]
        public int Id { set; get; }
        [ProtoMember(2)]
        public bool IncludeId { set; get; }
        public override string ToString()
        {
            return $"{Id}_{IncludeId}";
        }

    }
}
