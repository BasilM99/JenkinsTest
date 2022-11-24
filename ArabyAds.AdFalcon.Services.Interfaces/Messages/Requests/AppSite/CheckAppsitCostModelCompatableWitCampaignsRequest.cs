using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class CheckAppsitCostModelCompatableWitCampaignsRequest
    {
        [ProtoMember(1)]
        public int AppSiteId { get; set; }

        [ProtoMember(2)]
        public int AppSiteCostModel { get; set; }

        public override string ToString()
        {
            return $"{AppSiteId}_{AppSiteCostModel}";
        }

    }
}
