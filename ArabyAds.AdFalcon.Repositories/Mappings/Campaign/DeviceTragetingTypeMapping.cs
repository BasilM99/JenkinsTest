using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class DeviceTargetingTypeMapping : ClassMap<DeviceTargetingType>
    {
        public DeviceTargetingTypeMapping()
        {
            Table("devicetargetingtypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'DeviceTargetingType'");
            References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
