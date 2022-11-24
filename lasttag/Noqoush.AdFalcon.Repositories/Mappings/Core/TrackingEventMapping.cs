using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class TrackingEventMapping : ClassMap<TrackingEvent>
    {
        public TrackingEventMapping()
        {
            Table("ad_events_definition");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                        MappingSettings._nextHi,
                                        MappingSettings._maxLo,
                                        "TableKey = 'TrackingEvent'");
            References(x => x.Name, "NameId").Cascade.All();
            Map(x => x.EventName, "Name");
            Map(x => x.Code);
           // Map(x => x.ValidFor);
            Map(x => x.DefaultFrequencyCapping);
            Map(x => x.IsConversion);
            HasOne(p => p.CostModelWrapper).PropertyRef(x => x.Event).Cascade.None();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
