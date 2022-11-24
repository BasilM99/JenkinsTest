using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    //campaigns
    public class ReportSchedulerMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>
    {
        public ReportSchedulerMapping()
        {
            Table("reportscheduler");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ReportScheduler'");
            HasMany(p => p.AllRecipient).KeyColumn("ReportSchedulerId").Cascade.All().Inverse();
            Map(x => x.Name);
            References(x => x.Account, "AccountId").LazyLoad().Cascade.None();

            References(x => x.User, "UserId").LazyLoad().Cascade.None().Not.Update();
            Map(p => p.CreationDate);
            Map(x => x.StartDate);
            Map(x => x.Description).Nullable();
            Map(x => x.EmailSubject).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.IsScheduled).Nullable();
            Map(x => x.JobName).Nullable();
            References(x => x.ReportCriteria, "ReportCriteriaId").Cascade.None();

            References(x => x.LastDocumnetGenerated, "LastDocumnetGeneratedId").LazyLoad().Cascade.Delete().Nullable();
            Map(x => x.LastRunningDate).Nullable();
            Map(x => x.IsActive);
            Map(x => x.IsDeleted);
            Map(x => x.PreferedName).Nullable();
            //Map(x => x.ReportJsonCriteria).Nullable();
           // Map(x => x.StartDate);
            Map(x => x.TimeSentAt);
            Map(x => x.WeekDay, "WeekDay").CustomType<WeekDay>().Nullable();
            Map(x => x.MonthDay).Nullable();

            Map(x => x.RecurrenceType, "RecurrenceType").CustomType<RecurrenceType>();
            Map(x => x.ReportSectionType, "ReportSectionType").CustomType<ReportSectionType>();
            Map(x => x.DateRecurrenceType, "DateRecurrenceType").CustomType<DateRecurrenceType>();
            //Map(x => x.JobName).Nullable();
            Map(x => x.TriggerName).Nullable();
            Map(x => x.TriggerGroupName).Nullable();
            Map(x => x.DaysOfWeekParams).Nullable();
            Map(x => x.IsSendNow).Nullable();
            Map(x => x.IsFinished).Nullable();
            Map(x => x.NextFireTime).Nullable();
            Map(x => x.ReportId);
            Map(x => x.UpdateDate);
            Map(x => x.IsForQueryBuilder);
            Map(x => x.EmailIntroduction).Nullable();
            
            

        }
    }
}
