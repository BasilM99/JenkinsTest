
using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdActionCostModelWrapperMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Objective.AdActionCostModelWrapper>
    {
        public AdActionCostModelWrapperMapping()
        {
            Table("adactioncostmodelwrappers");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdActionCostModelWrapper'");
            References(x => x.AdAction, "AdActionTypeId");
            References(x => x.CostModelWrapper, "CostModelWrapperId");
            Map(M => M.Scope).CustomType< Noqoush.AdFalcon.Domain.Common.Model.Core.AppScope>();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}