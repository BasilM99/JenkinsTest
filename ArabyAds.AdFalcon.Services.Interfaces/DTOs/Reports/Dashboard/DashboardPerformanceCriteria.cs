using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [ProtoContract]
    public class DashboardPerformanceCriteria : BasePagingCriteriaDto
    {
       [ProtoMember(1)]
        public string MetricCode { get; set; }
       [ProtoMember(2)]
        public int CompanyName { get; set; }
       [ProtoMember(3)]
        public int CampName { get; set; }
      
        
       [ProtoMember(4)]
        public int userId { get; set; }
       [ProtoMember(5)]
        public bool IsPrimaryUser { get; set; }
       [ProtoMember(6)]
        public int? IdFilter { get; set; }
   
        

     [ProtoMember(7)]
        public int? DPProviderId { get; set; }
       [ProtoMember(8)]
        public int? IdSubFilter { get; set; }


       [ProtoMember(9)]
        public int? IdSecondSubFilter { get; set; }

    }
}
