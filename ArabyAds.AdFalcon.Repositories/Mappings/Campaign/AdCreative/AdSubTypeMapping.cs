using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdSubTypeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdSubType>
    {
        public AdSubTypeMapping()
        {
            Table("adsubtypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdSubType'");
            References(x => x.Name, "NameID");
            Map(x => x.Code).CustomType<AdSubTypes>();
            References(x => x.AdType, "AdTypeId").Cascade.None();
            References(x => x.Permission, "PermissionId");
            HasMany(x => x.AdTypeActions).AsSet().Table("SubtypeActionTypes").KeyColumn("SubTypeId");
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(100);
        }
    }
}
