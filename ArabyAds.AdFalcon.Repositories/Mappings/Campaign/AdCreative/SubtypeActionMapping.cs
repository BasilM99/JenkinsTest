using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class SubtypeActionMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.SubtypeAction>
    {
        public SubtypeActionMapping()
        {
            Table("subtypeactiontypes");
            Id(x => x.ID, "Id");
            References(x => x.ActionType, "AdTypeActionId").Cascade.None();
            References(x => x.SubType, "SubTypeId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
