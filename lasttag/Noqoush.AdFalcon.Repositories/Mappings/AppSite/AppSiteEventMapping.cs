using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.AppSite
{
    public class AppSiteEventMapping : ClassMap<AppSiteEvent>
    {
        public AppSiteEventMapping()
        {
            Table("appsite_events");
            Id(p => p.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSiteEvents'");
            References(p => p.AppSiteServerSetting, "AppSiteId").Not.Nullable();
            References(p => p.Event, "AdEventDefinitionId");
            Map(p => p.IsBillable);
            Map(p => p.MinBid);
         
        }
    }
}
