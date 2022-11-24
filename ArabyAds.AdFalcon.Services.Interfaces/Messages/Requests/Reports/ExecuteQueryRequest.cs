using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class ExecuteQueryRequest
    {
        [ProtoMember(1)]
        public string Query { set; get; }
        [ProtoMember(2)]
        public ResultDataModelDto DataModelDto { set; get; }

    }
}
