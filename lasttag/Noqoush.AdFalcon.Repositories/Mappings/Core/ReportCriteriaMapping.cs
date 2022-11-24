using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class ReportCriteriaMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Core.ReportCriteria>
    {
        public ReportCriteriaMapping()
        {
            Table("report_criteria");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ReportCriteria'");
            Map(x => x.Criteria);
            Map(x => x.CreationDate);
            Map(x => x.Name);
            Map(x => x.ReportScope).CustomType<ReportCriteriaScope>();
            Map(x => x.SectionType).CustomType<ReportSectionType>();
            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Account, "AccountId").LazyLoad();

            HasMany(d => d.Columns).KeyColumn("ReportCriteriaId").Cascade.AllDeleteOrphan().Where(x => !x.IsDeleted).Inverse();
        }
    }
}
