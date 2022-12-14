using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
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