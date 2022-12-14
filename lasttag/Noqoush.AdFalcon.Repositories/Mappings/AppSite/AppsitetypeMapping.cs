using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppsitetypeMapping : ClassMap<AppSiteType>
    {
        public AppsitetypeMapping()
        {
            Table("appsitetypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSiteType'");
            Map(x => x.IconURL, "IconURL");
            Map(x => x.ViewName);
            Map(x => x.IsThemeable);
            Map(x => x.IsApp);
            References(x => x.Name, "NameID");
            References(x => x.Platform, "PlatformId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}