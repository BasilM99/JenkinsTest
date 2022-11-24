using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class SubtypeActionMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.SubtypeAction>
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
