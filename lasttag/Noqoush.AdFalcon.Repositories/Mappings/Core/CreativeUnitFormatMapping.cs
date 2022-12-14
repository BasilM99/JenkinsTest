using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class CreativeUnitFormatMapping : ClassMap<CreativeUnitFormat>
    {
        public CreativeUnitFormatMapping()
        {
            Table("creativeunitformats");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, 
                                           MappingSettings._nextHi, 
                                           MappingSettings._maxLo,
                                           "TableKey = 'CreativeUnitFormat'");
            Map(x => x.Format);
            Map(x => x.MaxSize);
            Map(x => x.IsDeleted);
            References(x => x.CreativeUnit, "CreativeUnitId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}