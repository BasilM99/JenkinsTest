using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class AdCreativeAttributeMapping : ClassMap<AdCreativeAttribute>
    {
        public AdCreativeAttributeMapping()
        {
            Table("creative_attributes");
            Id(p => p.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CreativeAttribute'");
            Map(p => p.Code);
            Map(p => p.IsSupported);
            Map(p => p.Description);
            References(p => p.Name, "NameID").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
