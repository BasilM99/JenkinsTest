using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance
{
    public class AppOpsAppSitePerformanceViewModel
    {
        public IList<BaseAppSitePerformanceDetailsDto> Result { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<SelectListItem> MetricsList { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
    }
}
