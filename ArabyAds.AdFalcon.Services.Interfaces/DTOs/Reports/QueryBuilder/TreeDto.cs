using ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
 

    [ProtoContract]
    [ProtoInclude(100,typeof(ColumnQBDto))]
    [ProtoInclude(101,typeof(MeasureDto))]
    public class TreeQBDto : EntityQBDto, ITreeQBDto
    {
        [ProtoMember(1)]
        public string id { set; get; }
        [ProtoMember(2)]
        public string Name { set; get; }

     
        [ProtoMember(3)]
        public string data { set; get; }

        [ProtoMember(4)]
        public string text { set; get; }

        [ProtoMember(5)]
        public string Attribute { set; get; }
        [ProtoMember(6)]
        public string SubstituteAttribute { set; get; }

        [ProtoMember(7)]
        public string RawAttribute { set; get; }


        [ProtoMember(8)]
        public int OrderNumber { get; set; }

        [ProtoMember(9)]
        public int ParentId { get; set; }

        [ProtoMember(10)]
        public string DisplayName { set; get; }


        [ProtoMember(11)]
        public DataTypeQB DataType { set; get; }


        [ProtoMember(12)]
        public bool @checked { get; set; }


        [ProtoMember(13)]
        public bool hasChildren { get; set; }


        [ProtoMember(14)]
        public List<TreeQBDto> children { get; set; }


        [ProtoMember(15)]
        public int CustomValue { get; set; }

        [ProtoMember(16)]
        public string CustomValue1 { get; set; }

        [ProtoMember(17)]
        public string CustomValue2 { get; set; }


        [ProtoMember(18)]
        public string Key { get; set; }


        [ProtoMember(19)]
        public List<TreeQBDto> Childs { get; set; }


        [ProtoMember(20)]
        public string state { get; set; }


        [ProtoMember(21)]
        public string style { get; set; }


        [ProtoMember(22)]
        public string requestsmapping { get; set; }


        [ProtoMember(23)]
        public string dealsrequestsmapping { get; set; }

        [ProtoMember(24)]
        public bool SupportedByPublisher { set; get; }
        [ProtoMember(25)]
        public bool SupportedByAdvertiser { set; get; }


        [ProtoMember(26)]
        public int minWidth { set; get; }
    }
}