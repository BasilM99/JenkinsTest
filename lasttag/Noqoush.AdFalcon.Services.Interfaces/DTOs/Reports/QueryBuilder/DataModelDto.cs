using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder
{

    [DataContract]

    public class DataModelDto
    {
        [DataMember]
        public int pageSize { get; set; }
        [DataMember]
        public int pageNumber { get; set; }
        [DataMember]
        public DateTime? from { get; set; }
        [DataMember]
        public DateTime? to { get; set; }
        [DataMember]
        public List<int> ColumnsIds { get; set; }
        [DataMember]
        public List<int> MeasuresIds { get; set; }
        [DataMember]
        public Dictionary<string, string> Querydata { get; set; }
        [DataMember]
        public string QueryJsonData { get; set; }
        [DataMember]
        public string ColumnsIdsString { get; set; }
        [DataMember]
        public string MeasuresIdsString { get; set; }
        [DataMember]
        public int SummaryBy { get; set; }
        [DataMember]
        public int fact { get; set; }
        [DataMember]
        public int accountId { get; set; }
        [DataMember]
        public bool IncludeId { get; set; }
        [DataMember]
        public bool isEnabledUniqueQuery { get; set; }

        [DataMember]
        public bool iscampUnique { get; set; }
        [DataMember]
        public int uniquePeriod { get; set; }


    }


    [DataContract]

    public class ResultDataModelDto
    {
 
       
        [DataMember]
        public string Query { get; set; }
        [DataMember]
        public StringBuilder Warnings { get; set; }

        [DataMember]
        public bool isEnabledUniqueQuery { get; set; }

        [DataMember]
        public bool iscampUnique { get; set; }
        [DataMember]
        public int uniquePeriod { get; set; }
        [DataMember]
        public int UniqueClicksImpressionNumb { get; set; }

        [DataMember]
        public DateTime? from { get; set; }
        [DataMember]
        public DateTime? to { get; set; }

        [DataMember]
        public int accountId { get; set; }
    }
}
