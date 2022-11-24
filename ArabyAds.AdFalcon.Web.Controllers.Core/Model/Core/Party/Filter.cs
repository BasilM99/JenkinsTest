using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core.Party
{
    public class Filter
    {
        public IList<int> notInclud { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }
        public string id { get; set; }
        public string Name { get; set; }
        public bool ShowArchive { get; set; }
        public string IdPrefix { get; set; }
        public string Prefix { get; set; }
    }
}
