using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    [ProtoContract]
    public class FactDto: EntityQBDto
    {
        [ProtoMember(1)]
        public string DisplayName { set; get; }
        [ProtoMember(2)]
        public string Name { set; get; }
        [ProtoMember(3)]
        public bool IsForWeb { set; get; }

        [ProtoMember(4)]
        public string WebDisplayName { set; get; }
        [ProtoMember(5)]
        public ICollection<DimensionDto> Dimensions { set; get; } = new List<DimensionDto>();
        [ProtoMember(6)]
        public ICollection<MeasureDto> Measures { set; get; } = new List<MeasureDto>();


    }
}