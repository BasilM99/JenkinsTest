using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdGroupStatusMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdGroupStatus>
    {
        public AdGroupStatusMapping()
        {
            Table("adgroupstatus");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupStatus'");
            References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
