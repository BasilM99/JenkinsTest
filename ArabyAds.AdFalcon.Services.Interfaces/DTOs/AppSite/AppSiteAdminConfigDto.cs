using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteAdminConfigDto
    {
       [ProtoMember(1)]
        public int AppSiteId { get; set; }

       [ProtoMember(2)]
        public string AppSiteName { get; set; }

       [ProtoMember(3)]
        public decimal? DefaultAccountRevenue { get; set; }

       [ProtoMember(4)]
        public AppSiteServerSettingDto AppSiteServerSetting { get; set; }

       [ProtoMember(5)]
        public AppSiteRevenueCalculationSettingDto CurrentRevenueCalculationSettings { get; set; }



       [ProtoMember(6)]
        public IList<CampaignBidConfigDto> ModifiedNotCompatableBidConfigs { get; set; } = new List<CampaignBidConfigDto>();

    }
}
