using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class BidConfigMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.BidConfig>
    {
        public BidConfigMapping()
        {
            Table("bidconfig");
            Id(x => x.Id).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'BidConfig'");
            Map(x => x.Type, "TypeId").CustomType<BidConfigType>();
            Map(x => x.TargetingId);
            Map(x => x.AppScope).CustomType<AppScope>();
            Map(x => x.Value);
            References(x => x.CostModelWrapper, "CostModelWrapperId");
        }
    }
}
