using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Tenant;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
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
            References(x => x.Tenant, "TenantId").Not.Update();
            //ApplyFilter<TenantFilter>();
          
            //  Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}