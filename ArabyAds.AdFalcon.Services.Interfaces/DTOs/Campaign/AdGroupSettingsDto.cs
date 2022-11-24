using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupSettingsDto
    {
        [ProtoMember(1)]
        public decimal? DailyBudget { get; set; }
        [ProtoMember(2)]
        public decimal? Budget { get; set; }
        [ProtoMember(3)]
        public decimal CampaignBudget { get; set; }

        [ProtoMember(4)]
        public int CampaignId { get; set; }
        [ProtoMember(5)]
        public int AdGroupId { get; set; }

    }
}
