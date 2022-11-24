using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
   public class SetAdsBidModel
    {
        public int campaignId { get; set; }
        public int adGroupId { get; set; }
        public List<string> adIds { get; set; }
        public  decimal bid { get; set; }
    }
}
