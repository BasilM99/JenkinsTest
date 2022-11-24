using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder
{

    [ProtoContract]

    public class DataModelDto
    {
       [ProtoMember(1)]
        public int pageSize { get; set; }
       [ProtoMember(2)]
        public int pageNumber { get; set; }
       [ProtoMember(3)]
        public DateTime? from { get; set; }
       [ProtoMember(4)]
        public DateTime? to { get; set; }
       [ProtoMember(5)]
        public List<int> ColumnsIds { get; set; }
       [ProtoMember(6)]
        public List<int> MeasuresIds { get; set; }
       [ProtoMember(7)]
        public Dictionary<string, string> Querydata { get; set; }
       [ProtoMember(8)]
        public string QueryJsonData { get; set; }
       [ProtoMember(9)]
        public string ColumnsIdsString { get; set; }
       [ProtoMember(10)]
        public string MeasuresIdsString { get; set; }
       [ProtoMember(11)]
        public int SummaryBy { get; set; }
       [ProtoMember(12)]
        public int fact { get; set; }
       [ProtoMember(13)]
        public int accountId { get; set; }
       [ProtoMember(14)]
        public bool IncludeId { get; set; }
       [ProtoMember(15)]
        public bool isEnabledUniqueQuery { get; set; }

       [ProtoMember(16)]
        public bool iscampUnique { get; set; }
       [ProtoMember(17)]
        public int uniquePeriod { get; set; }
        [ProtoMember(18)]
        public bool ForPublisher { get; set; }

        
    }


    [ProtoContract]

    public class ResultDataModelDto
    {
 
       
       [ProtoMember(1)]
        public string Query { get; set; }
       [ProtoMember(2)]
        public string Warnings { get; set; }

       [ProtoMember(3)]
        public bool isEnabledUniqueQuery { get; set; }

       [ProtoMember(4)]
        public bool iscampUnique { get; set; }
       [ProtoMember(5)]
        public int uniquePeriod { get; set; }
       [ProtoMember(6)]
        public int UniqueClicksImpressionNumb { get; set; }

       [ProtoMember(7)]
        public DateTime? from { get; set; }
       [ProtoMember(8)]
        public DateTime? to { get; set; }

       [ProtoMember(9)]
        public int accountId { get; set; }


        [ProtoMember(10)]
        public string CountQuery { get; set; }
    }
}
