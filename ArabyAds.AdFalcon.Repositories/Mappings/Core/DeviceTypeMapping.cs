using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class DeviceTypeMapping : ClassMap<DeviceType>
    {
        public DeviceTypeMapping()
        {
            Table("devicetypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'DeviceType'");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(1000);
        }
    }
}