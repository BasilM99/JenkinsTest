using System;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
	public class AppsitestatussMapping : ClassMap<AppSiteStatus>
	{
		public AppsitestatussMapping()
		{
			Table("appsitestatus");
            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSiteStatus'");
			References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
			BatchSize(500);
        }
	}
}