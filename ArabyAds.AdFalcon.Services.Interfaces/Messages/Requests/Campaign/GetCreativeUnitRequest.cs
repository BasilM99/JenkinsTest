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
    public class GetCreativeUnitRequest
    {
        [ProtoMember(1)]
        public DeviceTypeEnum DeviceType { get; set; }
        [ProtoMember(2)]
        public AdTypeIds? AdType { get; set; }
        [ProtoMember(3)]
        public AdSubTypes? AdSubType { get; set; }
        [ProtoMember(4)]
        public string Group { get; set; }

        public override string ToString()
        {
            return $"{DeviceType}_{AdType?.ToString() ?? "Null"}_{AdSubType?.ToString() ?? "Null"}_{Group ?? null}";
        }
    }
}
