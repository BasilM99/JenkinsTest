using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class QueryDataProviderRequest
    {
        [ProtoMember(1)]
        public int DataProviderId { set; get; }
        [ProtoMember(2)]
        public string q { set; get; }

        [ProtoMember(3)]
        public int DateFrom { set; get; }

        [ProtoMember(4)]
        public int DateTo { set; get; }

        [ProtoMember(5)]
        public string Culture { set; get; }

        public override string ToString()
        {
            return $"{DataProviderId}_{q ?? "Null"}_{DateFrom}_{DateTo}_{Culture ?? "Null"}";
        }

    }
}
