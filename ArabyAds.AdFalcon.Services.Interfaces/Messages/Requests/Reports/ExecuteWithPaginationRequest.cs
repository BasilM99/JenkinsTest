using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class ExecuteWithPaginationRequest
    {
        [ProtoMember(1)]
        public string Query { set; get; }
        [ProtoMember(2)]
        public int PageNumber { set; get; }

        [ProtoMember(3)]
        public ResultDataModelDto DataModelDto { set; get; }

        [ProtoMember(4)]
        [DefaultValue(100)]
        public int PageSize { set; get; } = 100;


        [ProtoMember(5)]
        public string CountQuery { set; get; }

    }
}
