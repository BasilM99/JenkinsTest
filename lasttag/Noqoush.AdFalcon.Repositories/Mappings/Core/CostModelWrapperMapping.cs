using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class CostModelWrapperMapping : ClassMap<CostModelWrapper>
    {
        public CostModelWrapperMapping()
        {
            Table("cost_model_wrappers");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CostModelWrapper'");
            Map(p => p.DefaultBidValue);
            Map(p => p.DefaultDSPBidValue);
            Map(p => p.Factor);
          

            References(x => x.Name, "NameId");
            References(x => x.CostModel, "CostModelId");
            References(x => x.Event, "ad_event_definition_id");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
