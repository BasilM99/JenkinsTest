using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [ProtoContract]
    public class BaseAppSitePerformanceDetailsDto: BaseAppSiteResultDto
    {
      
       [ProtoMember(1)]
        public long TotalMetricSum { get; set; }
    }
}
