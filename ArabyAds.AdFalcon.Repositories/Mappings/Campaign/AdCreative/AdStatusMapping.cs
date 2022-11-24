using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdCreativeStatusMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCreativeStatus>
    {
        public AdCreativeStatusMapping()
        {
            Table("adstatus");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdCreativeStatus'");
            References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(500);
        }
    }
}
