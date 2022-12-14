using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
	public class LanguageMapping : ClassMap<Language>
	{
        public LanguageMapping()
        {
            Table("languages");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Language'");
            Map(x => x.Code);
            Map(x => x.ForPortal);
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
	}

    /*
    public class ViewAbilityVendorMapping : ClassMap<ViewAbilityVendor>
    {
        public ViewAbilityVendorMapping()
        {
            Table("viewability_vendors");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'ViewAbilityVendor'");
            Map(x => x.Code);
            Map(x => x.IsDeleted);
            Map(x => x.Description);
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }*/
}