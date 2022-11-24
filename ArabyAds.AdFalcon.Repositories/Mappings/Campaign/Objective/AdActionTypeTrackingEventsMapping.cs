using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdActionTypeTrackingEventsMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.Objective.AdActionTypeTrackingEvent>
    {
        public AdActionTypeTrackingEventsMapping()
        {
            Table("adactiontype_tracking_events");
            Id(x => x.ID).GeneratedBy.Identity();
            Map(x => x.StatisticsColumnName, "MappedStatColumnName");
            Map(x => x.Prerequisites, "Prerequisites");
            Map(x => x.AllowDuplicate, "AllowDuplicate");
            Map(x => x.IsBillable, "IsBillable");
            References(x => x.CostModelWrapper, "CostModelWrapperId");
            References(x => x.Event, "TrackingEventId");
         
           
        }
    }
}
