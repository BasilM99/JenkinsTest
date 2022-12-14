using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
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
            References(x => x.Name, "NameId");
            References(x => x.DeviceType, "DeviceTypeId");
            HasMany(x => x.Formats).KeyColumn("CreativeUnitId");
            Map(x => x.Width);
            Map(x => x.Height);
            Map(x => x.HD_Width);
            Map(x => x.HD_Height);
            Map(x => x.PreviewWidth);
            Map(x => x.PreviewHeight);
            Map(p => p.Code);
            Map(x => x.OrientationType, "OrientationTypeId").CustomType(typeof(OrientationType));
            HasManyToMany(p => p.Groups).Table("creativeunit_groups_mapping").ParentKeyColumn("CreativeUnitId").ChildKeyColumn("GroupId").Fetch.Select();
            HasMany(p => p.SupportedTypes).KeyColumn("creativeunitid");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}