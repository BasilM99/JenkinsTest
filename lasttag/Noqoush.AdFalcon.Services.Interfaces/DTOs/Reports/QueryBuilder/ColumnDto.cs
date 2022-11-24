
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    public class ColumnQBDto : TreeQBDto
    {

        public string homeIdSelector { set; get; }

        public bool IsSql { set; get; }
        public string Source { set; get; }

        public bool IsDuplicated { set; get; }
        public string TableName { set; get; }

        public string FkSelector { set; get; }

        public string formatSQL { set; get; }

    }
    [DataContract]
    public class DataQBDto 
    {
        [DataMember]
        public long TotalCount { set; get; }
        
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string ParentName { set; get; }
        [DataMember]
        public string SuperParentName { set; get; }
    }
}