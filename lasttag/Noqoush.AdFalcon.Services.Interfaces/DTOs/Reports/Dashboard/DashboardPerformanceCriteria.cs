using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [DataContract]
    public class DashboardPerformanceCriteria : BasePagingCriteriaDto
    {
        [DataMember]
        public string MetricCode { get; set; }
        [DataMember]
        public int CompanyName { get; set; }
        [DataMember]
        public int CampName { get; set; }
      
        
        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public bool IsPrimaryUser { get; set; }
        [DataMember]
        public int? IdFilter { get; set; }
   
        

      [DataMember]
        public int? DPProviderId { get; set; }
        [DataMember]
        public int? IdSubFilter { get; set; }


        [DataMember]
        public int? IdSecondSubFilter { get; set; }

    }
}
