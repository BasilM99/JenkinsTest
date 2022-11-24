using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdActionValueTrackerMapping : ClassMap<AdActionValueTracker>
    {
        public AdActionValueTrackerMapping()
        {
            Table("ad_action_value_trackers");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                      MappingSettings._maxLo, "TableKey = 'AdActionValueTracker'");
            Map(p => p.Url);
            Map(p => p.IsDeleted);
            References(p => p.AdActionValue, "AdActionValueId");
        }
    }
}
