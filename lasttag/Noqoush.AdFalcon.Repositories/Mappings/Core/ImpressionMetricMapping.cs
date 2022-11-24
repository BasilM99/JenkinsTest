using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class ImpressionMetricMapping : ClassMap<ImpressionMetric>
    {
        public ImpressionMetricMapping()
        {
            Table("impression_metrics");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'ImpressionMetric'");
            References(x => x.Name, "NameId").Cascade.All();

            Cache.Transactional().ReadWrite().IncludeAll();

            HasManyToMany(x => x.MetricVendors)
          .ChildKeyColumn("MetricVendorId")
          .ParentKeyColumn("ImpressionMetricId")
          .Table("bridge_impression_vendors")
          .Fetch.Select()
          .AsSet().LazyLoad().Cascade.All();
        }
    }
}