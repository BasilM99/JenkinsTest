using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class HistoryCriteriaDto
    {
       [ProtoMember(1)]
        public int ItemsPerPage { get; set; }

       [ProtoMember(2)]
        public int PageNumber { get; set; }

       [ProtoMember(3)]
        public string OrderBy { get; set; }

       [ProtoMember(4)]
        public bool Ascending { get; set; }

       [ProtoMember(5)]
        public DateTime FromDate { get; set; }

       [ProtoMember(6)]
        public DateTime ToDate { get; set; }
    }
}
