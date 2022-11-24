using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{

    public class TrafficPlannerCriteria
    {
        public string Operators { get; set; }
        public string Countries { get; set; }
        public string Continents { get; set; }
        public string AdSizes { get; set; }
        public string Platforms { get; set; }
        public string AppSites { get; set; }
        public int AgeGroup { get; set; }
        public int DeviceTypeId { get; set; }
        public int GenderType { get; set; }
        public int EnvironmentType { get; set; }
        public string languages { get; set; }

        public string AdFormats { get; set; }

        public int Weekid { get; set; }
        public int? PageIndex { get; set; }

        public int Size { get; set; }

        public int Type { get; set; }
        public bool IsRun { get; set; }
    }
}
