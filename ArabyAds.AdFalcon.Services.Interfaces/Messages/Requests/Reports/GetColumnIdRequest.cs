using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetColumnIdRequest
    {
        [ProtoMember(1)]
        public string AppFieldName { set; get; }
        [ProtoMember(2)]
        public bool Publisher { set; get; }

        public override string ToString()
        {
            return $"{AppFieldName ?? "Null"}_{Publisher}";
        }

    }
}
