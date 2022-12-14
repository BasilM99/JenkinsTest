using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class metriceColumnReportCriteriaMapping : ClassMap<metriceColumnReportCriteria>
    {
        public metriceColumnReportCriteriaMapping()
        {
            Table("metrices_columns_reportcriteria");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'metriceColumnReportCriteria'");
            References(x => x.metriceColumn, "metriceColumnId");
            References(x => x.ReportCriteria, "ReportCriteriaId");

            Map(X => X.IsDeleted);
        }
    }
}