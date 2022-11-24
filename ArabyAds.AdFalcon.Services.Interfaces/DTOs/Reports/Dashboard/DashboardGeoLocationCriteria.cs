using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [ProtoContract]
    public class DashboardGeoLocationCriteria : BasePagingCriteriaDto
    {
       
       [ProtoMember(1)]
        public int? CountryId { get; set; }

       [ProtoMember(2)]
        public int? IdFilter { get; set; }


       [ProtoMember(3)]
        public int userId { get; set; }
       [ProtoMember(4)]
        public bool IsPrimaryUser { get; set; }

    }
}
