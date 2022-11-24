using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class AdLessQueryBidModel
    {
        public decimal bid { get; set; }
        public int campaignId { get; set; }
        public int adGroupId { get; set; }
    }
}
