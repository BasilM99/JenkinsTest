using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class TargetingTypeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.TargetingType>
    {
        public TargetingTypeMapping()
        {
            Table("targetingtype");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'TargetingType'");
            References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
