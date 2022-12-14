using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CurrencyMapping : ClassMap<Currency>
    //public class CurrencyMapping : SubclassMap<Currency>
    {
       /* public CurrencyMapping()
        {
            Table("currencies");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Currency'");
            References(x => x.Name, "NameId");
        }*/
        public CurrencyMapping()
        {
            Table("currencies");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Currency'");
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}