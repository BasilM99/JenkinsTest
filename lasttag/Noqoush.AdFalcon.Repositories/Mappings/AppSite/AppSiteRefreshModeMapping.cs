using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppSiteRefreshModeMapping : ClassMap<AppSiteRefreshMode>
    {
        public AppSiteRefreshModeMapping()
        {
            Table("appsiterefreshmodes");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSiteRefreshMode'");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}