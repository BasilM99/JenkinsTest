using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class ProtocolMapping : ClassMap<Protocol>
    {
        public ProtocolMapping()
        {
            Table("creative_protocols");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Protocol'");
            References(x => x.Name, "NameId").Cascade.All();
            Map(x => x.Code);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}