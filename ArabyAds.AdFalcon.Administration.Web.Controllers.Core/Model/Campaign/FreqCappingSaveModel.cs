using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign
{
    public class FreqCappingSaveModel
    {

        public int id { get; set; }
        public CampaignFrequencyCappingSaveDto frequencyCappingSave { get; set; }
    }
}
