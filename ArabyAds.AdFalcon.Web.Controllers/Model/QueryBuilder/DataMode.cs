using Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder;

using System;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Web.Controllers.Model.QueryBuilder
{

    public class DataModel
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }

        public int size { get; set; }
        public int page { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public List<int> ColumnsIds { get; set; }
        public List<int> MeasuresIds { get; set; }
        public Dictionary<string, string> Querydata { get; set; }
        public string QueryJsonData { get; set; }
        public string ColumnsIdsString { get; set; }
        public string MeasuresIdsString { get; set; }
        public Function function { get; set; }
        public int SummaryBy { get; set; }
        public int fact { get; set; }
        public bool IncludeId { get; set; }
    }
}