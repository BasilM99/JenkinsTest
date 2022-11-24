using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteAdminConfigDto
    {
        [DataMember]
        public int AppSiteId { get; set; }

        [DataMember]
        public string AppSiteName { get; set; }

        [DataMember]
        public decimal? DefaultAccountRevenue { get; set; }

        [DataMember]
        public AppSiteServerSettingDto AppSiteServerSetting { get; set; }

        [DataMember]
        public AppSiteRevenueCalculationSettingDto CurrentRevenueCalculationSettings { get; set; }



        [DataMember]
        public IList<CampaignBidConfigDto> ModifiedNotCompatableBidConfigs { get; set; }

    }
}
