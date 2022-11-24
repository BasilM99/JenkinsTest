using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class ReportRecipientMapping : ClassMap<ReportRecipient>
    {
        public ReportRecipientMapping()
        {
            Table("reportrecipient");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                      MappingSettings._maxLo, "TableKey = 'ReportRecipient'");
            Map(p => p.Email);
            Map(p => p.IsDeleted);
            References(p => p.ReportScheduler, "ReportSchedulerId").LazyLoad().Cascade.None();
        }
    }
}
