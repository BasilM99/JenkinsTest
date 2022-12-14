using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    //public class DeviceMapping : SubclassMap<Device>
    public class DeviceMapping : ClassMap<Device>
    {
        public DeviceMapping()
        {
            Table("devices");
            //Table("devices_copy");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Device'");
           // Map(c => c.Code);
            References(x => x.Name, "NameId").Cascade.SaveUpdate();
            References(x => x.Manufacturer).Column("ManufacturerId");
            References(x => x.Platform).Column("PlatformId");
            References(x => x.DeviceType, "DeviceTypeId");

           

            HasMany(p => p.Codes).KeyColumn("DeviceId").Cascade.All().Cache.ReadWrite(); ;

            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }




    public class DeviceCodeMapping : ClassMap<DeviceCode>
    {
        public DeviceCodeMapping()
        {
            Table("device_codes");
            //Table("devices_copy");
            Id(p => p.ID,"Id").GeneratedBy.Identity();
            Map(c => c.Code);
  
            References(x => x.Device, "DeviceId");
 
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}

