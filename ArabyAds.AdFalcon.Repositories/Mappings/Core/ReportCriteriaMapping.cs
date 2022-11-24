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
    public class ReportCriteriaMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Core.ReportCriteria>
    {
        public ReportCriteriaMapping()
        {
            Table("report_criteria");
           

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ReportCriteria'");
            Where("ForDashBoard IS NULL");
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

    public class DashBoardCriteriaMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Core.DashBoardCriteria>
    {
        public DashBoardCriteriaMapping()
        {
            Table("report_criteria");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ReportCriteria'");

            Where("ForDashBoard IS NOT NULL");
            Map(x => x.Criteria);
            //Map(x => x.CriteriaName);
            Map(x => x.CreationDate);
            Map(x => x.Name);
            Map(x => x.ForDashBoard);
            
            Map(x => x.ReportScope).CustomType<ReportCriteriaScope>();
            Map(x => x.SectionType).CustomType<ReportSectionType>();
            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Account, "AccountId").LazyLoad();

            HasMany(d => d.Columns).KeyColumn("ReportCriteriaId").Cascade.AllDeleteOrphan().Where(x => !x.IsDeleted).Inverse();
        
        
        
        }
    }
}
