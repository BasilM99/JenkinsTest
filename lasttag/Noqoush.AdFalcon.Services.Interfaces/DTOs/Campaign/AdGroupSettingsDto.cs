using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    public class AdGroupSettingsDto
    {
        public decimal? DailyBudget { get; set; }
        public decimal? Budget { get; set; }
          public decimal CampaignBudget { get; set; }
        
        public int CampaignId { get; set; }
        public int AdGroupId { get; set; }

    }
}
