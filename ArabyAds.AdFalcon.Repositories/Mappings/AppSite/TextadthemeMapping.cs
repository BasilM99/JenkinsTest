using System;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
	public class TextadthemeMapping : ClassMap<TextAdTheme>
	{
		public TextadthemeMapping()
		{
		    Table("textadthemes");
            Id(x => x.ID, "TextAdThemeId").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'TextAdTheme'");
		    References(x => x.Name, "NameId");
		    Map(x => x.BackgroundColor);
		    Map(x => x.IsCustom);
		    Map(x => x.TextColor);
            Cache.Transactional().ReadWrite().IncludeAll();
			BatchSize(500);
        }
	}
}