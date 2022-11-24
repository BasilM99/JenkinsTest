using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdTypeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdType>
    {
        public AdTypeMapping()
        {
            Table("adtypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdType'");
            HasManyToMany(p => p.AdActions).AsSet().Table("adtypeactions").ParentKeyColumn("AdTypeId").ChildKeyColumn("AdActionTypeId").Cascade.All();
         HasMany(x => x.SubTypes).AsSet().Table("AdSubTypes").KeyColumn("AdTypeId").BatchSize(100).Cache.Transactional().ReadWrite();
            References(x => x.Name, "NameID").Not.LazyLoad().Fetch.Select();

            Map(x => x.Group, "AdTypeGroup").CustomType(typeof(AdTypeGroup)); ;
            References(x => x.Permission, "PermissionId");
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(1000);
        }
    }
}
