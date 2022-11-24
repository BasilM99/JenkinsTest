using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdTypeMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdType>
    {
        public AdTypeMapping()
        {
            Table("adtypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdType'");
            HasManyToMany(p => p.AdActions).AsSet().Table("adtypeactions").ParentKeyColumn("AdTypeId").ChildKeyColumn("AdActionTypeId").Cascade.All().Fetch.Select();
         HasMany(x => x.SubTypes).AsSet().Table("AdSubTypes").KeyColumn("AdTypeId");
            References(x => x.Name, "NameID");

            Map(x => x.Group, "AdTypeGroup").CustomType(typeof(AdTypeGroup)); ;
            References(x => x.Permission, "PermissionId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
