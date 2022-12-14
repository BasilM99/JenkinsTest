using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.AppSite;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class MatchtypeMapping : ClassMap<MatchType>
    {
        public MatchtypeMapping()
        {
            Table("matchtypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'MatchType'");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}