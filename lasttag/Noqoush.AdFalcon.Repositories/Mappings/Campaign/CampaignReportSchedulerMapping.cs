using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
  

    public class CampaignReportSchedulerMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.CampaignReportScheduler>
    {
        public CampaignReportSchedulerMapping()
        {
            Table("campaignreportscheduler");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'CampaignReportScheduler'");
            References(x => x.Account, "AccountId").LazyLoad();
            HasManyToMany(x => x.Campaigns)
                .ChildKeyColumn("CampaignId")
                .ParentKeyColumn("CampaignReportSchedulerId")
                .Table("campaign_campaignreportscheduler")
                .Fetch.Select()
                .AsSet().Cascade.None();

            Map(x => x.PreferedName);
            //HasMany(d => d.Campaigns).LazyLoad().KeyColumn("CampaignReportSchedulerId").Cascade.None();
            Map(x => x.ReportSummaryBy, "ReportSummaryBy").CustomType(typeof(ReportSummaryBy));
            HasMany(p => p.AllRecipient).KeyColumn("CampaignReportSchedulerId").Cascade.All();
            Map(x => x.GroupByType, "GroupByType").CustomType(typeof(GroupByType));
            References(x => x.ReportScheduler, "ReportSchedulerId");

        }
    }
}
