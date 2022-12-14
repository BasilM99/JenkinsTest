using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class FeaturesMapping : ClassMap<Feature>
    {

        public FeaturesMapping()
        {
            Table("features");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Feature'");
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
            Map(x => x.Code).CustomType<FeaturesCode>();
        }
    }
}