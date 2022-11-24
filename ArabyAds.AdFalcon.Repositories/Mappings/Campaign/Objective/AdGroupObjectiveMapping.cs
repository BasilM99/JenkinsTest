using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdGroupObjectiveMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdGroupObjective>
    {
        public AdGroupObjectiveMapping()
        {
            Table("adgroupobjectives");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupObjective'");
            References(x => x.AdGroup, "AdGroupId").Unique();
            References(x => x.AdType, "AdTypeId");
            References(x => x.AdAction, "ActionTypeId");
            References(x => x.Objective, "ObjectiveTypeId");
        }
    }
}
