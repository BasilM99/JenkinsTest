using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class JobPositionMapping : ClassMap<JobPosition>
    {
        public JobPositionMapping()
        {
            Table("job_positions");
            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'Job Positions'");
            References(r => r.Name, "NameID").Cascade.All(); ;
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}