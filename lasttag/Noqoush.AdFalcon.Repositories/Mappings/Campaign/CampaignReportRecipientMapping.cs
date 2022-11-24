
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignReportRecipientMapping : ClassMap<CampaignReportRecipient>
    {
        public CampaignReportRecipientMapping()
        {
            Table("campaignreportrecipient");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                      MappingSettings._maxLo, "TableKey = 'CampaignReportRecipient'");
            Map(p => p.Email);
            Map(p => p.IsDeleted);
            References(p => p.CampaignReportScheduler, "CampaignReportSchedulerId");
        }
    }
}
