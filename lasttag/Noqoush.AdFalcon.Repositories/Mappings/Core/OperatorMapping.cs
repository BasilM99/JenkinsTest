using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class OperatorMapping : ClassMap<Operator>
    {
        public OperatorMapping()
        {
            Table("operators");
            Id(x => x.ID, "OperatorId").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Operator'");
            References(x => x.Location, "LocationId");
            References(x => x.Name, "OperatorNameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}