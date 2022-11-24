using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    [ProtoContract]
    public class DimensionDto : EntityQBDto
    {
        [ProtoMember(1)]
        public string Name { set; get; }
        [ProtoMember(2)]
        public string Source { set; get; }
        [ProtoMember(3)]
        public string Attributes { set; get; }
        [ProtoMember(4)]
        public string FilterCol { set; get; }
        [ProtoMember(5)]
        public string CustomGet { set; get; }

        [ProtoMember(6)]
        public bool IsGrouped { set; get; }


        [ProtoMember(7)]
        public bool IsSql { set; get; }
        [ProtoMember(8)]
        public IList<ColumnQBDto> Columns { set; get; } = new List<ColumnQBDto>();
        [ProtoMember(9)]
        public bool IsEnum { set; get; }

        [ProtoMember(10)]
        public string TableName { set; get; }
        [ProtoMember(11)]
        public string Selector { set; get; }

        [ProtoMember(12)]
        public bool IsScoped { set; get; }
        [ProtoMember(13)]
        public string ScopeTableName { set; get; }


        [ProtoMember(14)]
        public int DimensionType { set; get; }
        [ProtoMember(15)]
        public string DimensionTypeStr { set; get; }

        [ProtoMember(16)]
        public  bool SupportedByPublisher { set; get; }

        [ProtoMember(17)]
        public bool SupportedByAdvertiser { set; get; }
    }
}