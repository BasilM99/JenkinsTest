using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdSubTypeMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdSubType>
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
        }
    }
}
