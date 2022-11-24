using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative

{
    public class AppMarketingPartnerTrackerMapping : ClassMap<AppMarketingPartnerTracker>
    {
        public AppMarketingPartnerTrackerMapping()
        {
            Table("app_marketing_partner_trackers");
            Id(x => x.ID).GeneratedBy.Identity();
            // Map(p => p.ClickTrackerUrlTemplate);
            // Map(p => p.EventPostbackUrlTemplate);

            Map(p => p.TrackerUrlTemplate);
            Map(p => p.AdGroupID).Nullable();
            Map(p => p.TypeID,"Type");
            // HasOne(x => x.AppMarketingPartner).ForeignKey("Id").Cascade.All();
            References(p => p.Platform, "PlatformId");
            References(p => p.AppMarketingPartner, "AppMarketingPartnerId");
        }
    }
}
