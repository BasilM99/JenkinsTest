using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.QueryBuilder
{

    public class DimensionMapping : ClassMap<Dimension>
    {
        public DimensionMapping()
        {
            Schema("adfalcon_report");
            Table("qb_dimensions");
            Id(x => x.Id);
            Map(x => x.IsDeleted);
            References(x => x.Name, "NameID");
            Map(x => x.Attributes);
            Map(x => x.FilterCol);
            Map(x => x.TableName);
            Map(x => x.Selector);
            Map(x => x.IsSql);
            Map(x => x.IsEnum);
            Map(x => x.SupportedByAdvertiser);
            Map(x => x.SupportedByPublisher);
            Map(x => x.Source);
            Map(x => x.IsGrouped);
            Map(x => x.CustomGet);
            Map(x => x.IsScoped);
            Map(x => x.ScopeTableName);
            Map(x => x.DimensionType);
            Map(x => x.DimensionTypeStr);
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
