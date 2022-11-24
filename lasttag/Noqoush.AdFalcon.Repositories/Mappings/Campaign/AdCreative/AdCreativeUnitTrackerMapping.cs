using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdCreativeUnitTrackerMapping : ClassMap<AdCreativeUnitTracker>
    {
        public AdCreativeUnitTrackerMapping()
        {
            Table("ad_creative_unit_trackers");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.TrackingUrl, "Url");
            Map(p => p.TrackingJS, "TrackingJS");
            Map(p => p.IsDeleted);
            Map(p => p.AdCreativeUnitTrackerType,"Type").CustomType<AdCreativeUnitTrackerType>(); ;
            
            References(p => p.AdGroupEvent, "AdGroupEventId");
            References(p => p.CreativeUnit, "AdCreativeUnitId");
        }
    }
}
