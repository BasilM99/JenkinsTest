using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100, typeof(AppSiteStatisticsCriteriaDto))]
    [ProtoInclude(101, typeof(DashboardGeoLocationCriteria))]
    [ProtoInclude(102, typeof(DashboardPerformanceCriteria))]
    [ProtoInclude(103, typeof(BaseAppSitePerformanceDetailsCriteria))]
    [ProtoInclude(104, typeof(ReportCriteriaDto))]
    public class BasePagingCriteriaDto : BaseCriteriaDto
    {
    


       [ProtoMember(1)]
        public int PageNumber { get; set; }

       [ProtoMember(2)]
        public int ItemsPerPage { get; set; }

       [ProtoMember(3)]
        public string OrderColumn { get; set; }

       [ProtoMember(4)]
        public string OrderType { get; set; }

        public string Culture
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture.Name; }
            set { ; }
        }
    }
}
