using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    [ProtoContract]
    [ProtoInclude(100, typeof(TreeQBDto))]
    [ProtoInclude(101, typeof(DimensionDto))]
    [ProtoInclude(102, typeof(FactDto))]
    public class EntityQBDto
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public bool IsDeleted { get; set; }
    }
}