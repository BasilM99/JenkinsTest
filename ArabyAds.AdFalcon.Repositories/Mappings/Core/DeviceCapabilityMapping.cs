using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class DeviceCapabilityMapping : ClassMap<DeviceCapability>
    {
        public DeviceCapabilityMapping()
        {
            Table("device_capabilities");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'DeviceCapability'");

            Where("IsDeleted=0");
            References(x => x.Name, "NameId").Cascade.All();
            Map(x => x.WurflCapabilities);
            Map(x => x.WurflValue);
            Map(x => x.Type,"type").CustomType<DeviceCapabilityType>();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}