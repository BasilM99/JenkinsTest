using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.QueryBuilder
{

    public class DimensionMapping : ClassMap<Dimension>
    {
        public DimensionMapping()
        {
            Schema("adfalcon_report");
            Table("qb_dimensions");
            Id(x => x.Id);
            Map(x => x.IsDeleted);
            Map(x => x.Name);
            Map(x => x.Attributes);
            Map(x => x.FilterCol);
            Map(x => x.TableName);
            Map(x => x.Selector);
            Map(x => x.IsSql);
            Map(x => x.IsEnum);

            Map(x => x.Source);
            Map(x => x.IsGrouped);
            Map(x => x.CustomGet);
            Map(x => x.IsScoped);
            Map(x => x.ScopeTableName);



    }
    }
}
