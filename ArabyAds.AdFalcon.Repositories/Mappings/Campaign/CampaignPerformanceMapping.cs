using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignPerformanceMapping : ClassMap<CampaignPerformance>
    {
        public CampaignPerformanceMapping()
        {
            Table("campaignsperformance");
            Id(x => x.ID, "Id");
            Map(x => x.CampaignId);
            //References(x => x.Campaign, "CampaignId");
            Map(x => x.Spend, "BillableCost");
        }
    }
}
