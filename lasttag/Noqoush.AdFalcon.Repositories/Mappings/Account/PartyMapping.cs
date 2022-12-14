using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class PartyMapping : ClassMap<Party>
    {
        public PartyMapping()
        {
            Table("`party`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Party'");
            Map(x => x.Name);
            Map(x => x.IsDeleted);
            Map(x => x.Visible);
            //  Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}