using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CreativeUnitMapping : ClassMap<CreativeUnit>
    {
        public CreativeUnitMapping()
        {
            Table("creativeunits");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, 
                                           MappingSettings._nextHi, 
                                           MappingSettings._maxLo, 
                                           "TableKey = 'CreativeUnit'");
            Map(x => x.Description);
            References(x => x.Name, "NameId").Not.LazyLoad().Fetch.Select();
            References(x => x.DeviceType, "DeviceTypeId").Not.LazyLoad().Fetch.Select();
            HasMany(x => x.Formats).KeyColumn("CreativeUnitId").Not.LazyLoad().Fetch.Select().BatchSize(1000).Cache.Transactional().ReadWrite();
            Map(x => x.Width);
            Map(x => x.Height);
            Map(x => x.HD_Width);
            Map(x => x.HD_Height);
            Map(x => x.PreviewWidth);
            Map(x => x.PreviewHeight);
            Map(p => p.Code);
            Map(x => x.OrientationType, "OrientationTypeId").CustomType(typeof(OrientationType));
            HasManyToMany(p => p.Groups).Table("creativeunit_groups_mapping").ParentKeyColumn("CreativeUnitId").ChildKeyColumn("GroupId").Not.LazyLoad().Fetch.Select().BatchSize(1000).Cache.Transactional().ReadWrite();
            HasMany(p => p.SupportedTypes).KeyColumn("creativeunitid").BatchSize(1000).Cache.ReadWrite();
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(1000);
        }
    }
}