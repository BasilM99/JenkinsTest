using ArabyAds.AdFalcon.Web.Controllers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.AppSite.Performance
{
    public class BaseAppSitePerformanceSearchInfo : BaseSearchInfo
    {
        public string OrderColumn { get; set; }
        public string OrderBy { get; set; }
        public string AppSiteName { get; set; }
        public string AccountName { get; set; }
        public int? AccountId { get; set; }
        public List<int> CountryIds { get; set; }
        public string MetricValue { get; set; }
    }
}
