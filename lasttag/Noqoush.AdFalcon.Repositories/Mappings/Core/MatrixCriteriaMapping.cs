using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
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