using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignFrequencyCappingMapping : ClassMap<CampaignFrequencyCapping>
    {
        public CampaignFrequencyCappingMapping()
        {
            Table("campaign_frequency_capping_settings");
            Id(p => p.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'CampaignFrequencyCapping'");
            References(p => p.CampaignServerSetting, "CampaignId").Not.Nullable();
            References(p => p.Event, "AdEventDefinitionId");
            Map(p => p.Number, "FrequencyCappingNumber");
            Map(p => p.Interval, "FrequencyCappingInterval");
            Map(p => p.Type, "FrequencyCappingType");

        }
    }
}
