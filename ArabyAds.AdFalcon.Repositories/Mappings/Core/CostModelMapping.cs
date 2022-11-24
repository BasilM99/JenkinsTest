using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CostModelMapping : ClassMap<CostModel>
    {
        public CostModelMapping()
        {
            Table("costmodels");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CostModels'");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
