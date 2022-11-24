using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    [ProtoInclude(100,typeof(GetCreativeUnitByCriteriaWithDimensionsRequest))]
    public class GetCreativeUnitByCriteriaRequest
    {
        [ProtoMember(1)]
        public int? CreativeUnitId { get; set; }
        [ProtoMember(2)]
        public int DeviceTypeId { get; set; }
        [ProtoMember(3)]
        public string Group { get; set; }
        [ProtoMember(4)]
        public int? AdTypeId { get; set; }
        [ProtoMember(5)]
        public int? AdSubTypeId { get; set; }

        public override string ToString()
        {
            return $"{CreativeUnitId?.ToString() ?? "Null"}_{DeviceTypeId}_{Group ?? "Null"}_{AdTypeId?.ToString() ?? "Null"}";
        }

    }
}
