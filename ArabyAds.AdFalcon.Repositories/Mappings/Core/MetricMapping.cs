using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    [DataContract()]
    public enum MetricTarget
    {
        [EnumMember]
        AppSite = 0,
        [EnumMember]
        Campain = 1
    }

    public class MetricMapping : ClassMap<Metric>
    {
        public MetricMapping()
        {
            Table("`metrics`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Metric'");

            Map(x => x.MetricTarget);
            Map(x => x.Code);
            Map(x => x.Color);
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }


}
