using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class CheckAppsitCostModelCompatableWitCampaignsResponse
    {
        [ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoMember(2)]
        public IList<CampaignBidConfigDto> NotCompatableCampaigns { get; set; } = new List<CampaignBidConfigDto>();
    }
}
