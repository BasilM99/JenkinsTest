
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CompanyTypeMapping : ClassMap<CompanyType>
 
    {
       
        public CompanyTypeMapping()
        {
            Table("companytypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CompanyType'");
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}