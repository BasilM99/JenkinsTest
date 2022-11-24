using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Report
{
    public class GChartControlModel
    {
        public int periodOption { get; set; }
        public string metricCode { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string subId { get; set; }
        public string secondsubId { get; set; }
        public int? CompanyName { get; set; }
        public int? CampName { get; set; }
        public int? AdvertiserId { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }

    public class GridCommonFilter
    {
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public int? size { get; set; }
        public int? skip { get; set; }
        public int? take { get; set; }
        public string sort { get; set; }
        public string group { get; set; }
        public string filter { get; set; }
        public string orderBy { get; set; }
    }
    public class AdGeoLocationGridFilter : GridCommonFilter
    {
        public int? AdvertiserId { get; set; }
        public int? AdvertiserAccountId { get; set; }
        public int period { get; set; }
        public string list { get; set; }
        public string country { get; set; }
    }

  
}
