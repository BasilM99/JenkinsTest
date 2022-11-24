using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria
{
    [DataContract]
    public class AppSiteStatisticsCriteriaDto : BasePagingCriteriaDto
    {
        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public bool IsPrimaryUser { get; set; }


        [DataMember]
        public int SummaryBy { get; set; }

        [DataMember]
        public string Layout { get; set; }

        [DataMember]
        public string ItemsList { get; set; }

        [DataMember]
        public string AdvancedCriteria { get; set; }

        [DataMember]
        public string GroupBy { get; set; }

        [DataMember]
        public int AccountId { get; set; }

    }
}
