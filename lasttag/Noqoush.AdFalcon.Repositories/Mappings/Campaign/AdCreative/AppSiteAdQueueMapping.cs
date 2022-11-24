using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AppSiteAdQueueMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AppSiteAdQueue>
    {
        public AppSiteAdQueueMapping()
        {
            Table("appsite_ad_queue");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSiteAdQueue'");


         

            Map(x => x.Bid);
           References(x => x.AppSite, "AppsiteId").Not.Nullable();
           References(x => x.Ad, "AdId").Not.Nullable();
            Map(x => x.Include).CustomType(typeof(Include));

             //  CompositeId()
             //// .KeyProperty(x => x.ID)
             // .KeyReference(x => x.AppSite, "AppsiteId")
             // .KeyReference(x => x.Ad, "AdId");

        }
    }
}
