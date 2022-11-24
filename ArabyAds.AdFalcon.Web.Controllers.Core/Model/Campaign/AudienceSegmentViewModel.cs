using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class AudienceSegmentViewModel
    {
        public int ParentId { get; set; }
        public int id { get; set; }
        public string Path { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public bool showrecency { get; set; }
        public int recency { get; set; }
        public bool Positive { get; set; }
    }
}
