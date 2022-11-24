using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class HistoryCriteriaDto
    {
        [DataMember]
        public int ItemsPerPage { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public string OrderBy { get; set; }

        [DataMember]
        public bool Ascending { get; set; }

        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }
    }
}
