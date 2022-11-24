using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class RichMediaRequiredProtocolMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.RichMediaRequiredProtocol>
    {
        public RichMediaRequiredProtocolMapping()
        {
            Table("richmedia_protocols");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'RichMediaRequiredProtocol'");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
