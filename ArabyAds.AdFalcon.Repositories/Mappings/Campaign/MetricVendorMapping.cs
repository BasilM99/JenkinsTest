using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class MetricVendorMapping : ClassMap<MetricVendor>
    {
        public MetricVendorMapping()
        {
            Table("metric_vendors");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'MetricVendor'");
            References(x => x.Name, "NameId").Cascade.All();

            Map(p => p.IsDeleted);
            Map(p => p.Description);
            Map(p => p.Code);
            Cache.Transactional().ReadWrite().IncludeAll();

        }

    }
}
