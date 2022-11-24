using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdCampaignStatusMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCampaignStatus>
    {
        public AdCampaignStatusMapping()
        {
            Table("campaignstatus");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdCampaignStatus'");
            References(x => x.Name, "NameID");
            Map(x => x.IsDeleted);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
