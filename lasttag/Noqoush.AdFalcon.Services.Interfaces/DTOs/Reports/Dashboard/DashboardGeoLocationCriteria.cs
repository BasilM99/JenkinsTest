using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [DataContract]
    public class DashboardGeoLocationCriteria : BasePagingCriteriaDto
    {
       
        [DataMember]
        public int? CountryId { get; set; }

        [DataMember]
        public int? IdFilter { get; set; }


        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public bool IsPrimaryUser { get; set; }

    }
}
