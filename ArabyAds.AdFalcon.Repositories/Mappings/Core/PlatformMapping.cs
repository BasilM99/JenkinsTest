using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class PlatformMapping : ClassMap<Platform>
    {
        public PlatformMapping()
        {
            Table("platforms");
            Id(x => x.ID, "PlatformId").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Platform'");
            References(x => x.Name, "PlatformNameId").Cascade.All(); ;
            HasMany(x => x.Devices).KeyColumn("PlatformId").Inverse().Fetch.Select().AsSet().Cache.ReadWrite(); 
            HasMany(x => x.Versions).KeyColumn("PlatformId").Inverse().BatchSize(100).AsSet().Cache.ReadWrite(); 
            Map(x => x.IsVisible);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}