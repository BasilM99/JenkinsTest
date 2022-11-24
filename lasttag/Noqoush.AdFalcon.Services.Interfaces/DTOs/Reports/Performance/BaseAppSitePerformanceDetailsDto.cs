using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [DataContract]
    public class BaseAppSitePerformanceDetailsDto: BaseAppSiteResultDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public long TotalMetricSum { get; set; }
    }
}
