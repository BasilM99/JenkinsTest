using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100,typeof(BasePagingCriteriaDto))]
    [ProtoInclude(101,typeof(DashboardChartCriteria))]
    public class BaseCriteriaDto
    {
       [ProtoMember(1)]
        public DateTime FromDate { get; set; }

       [ProtoMember(2)]
        public DateTime ToDate { get; set; }

       [ProtoMember(3)]
        public DateTime RFromDate { get; set; }

       [ProtoMember(4)]
        public DateTime RToDate { get; set; }
       [ProtoMember(5)]
        public int AdvertiserId { get; set; }

       [ProtoMember(6)]
        public int AccountAdvertiserId { get; set; }
    }
}
