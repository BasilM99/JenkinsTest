using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class PlatformVersionMapping : ClassMap<PlatformVersion>
    {
        public PlatformVersionMapping()
        {
            Table("platformversions");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                          MappingSettings._nextHi,
                                          MappingSettings._maxLo,
                                          "TableKey = 'PlatformVersion'");
            
            Map(p => p.Version);
            Map(p => p.Code);
            References(p => p.Platform, "PlatformId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
