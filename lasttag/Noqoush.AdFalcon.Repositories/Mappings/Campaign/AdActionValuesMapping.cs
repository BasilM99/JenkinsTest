using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdActionValuesMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Objective.AdActionValue>
    {
        public AdActionValuesMapping()
        {
            Table("adactionvalues");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdActionValue'");
            Map(x => x.IsDeleted);
            Map(x => x.Value);
            Map(x => x.Value2);
            HasMany(p => p.Trackers).KeyColumn("AdActionValueId").Cascade.All();
            References(x => x.ActionType, "TypeId");
            References(x => x.AdCreative, "AdId").Cascade.All();
        }
    }


}
