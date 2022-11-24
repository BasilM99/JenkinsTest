using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder;
using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.QueryBuilder
{

    public class ColumnQBMapping : ClassMap<ColumnQB>
    {
        public ColumnQBMapping()
        {

            Schema("adfalcon_report");
            Table("qb_columns");
            Id(x => x.Id);
            Map(x => x.IsDeleted);
            Map(x => x.Name);
            Map(x => x.Attribute);
            Map(x => x.FkSelector);
            Map(x => x.Source);

            Map(x => x.IsSql);
            Map(x => x.ParentId);
            Map(x => x.OrderNumber);
            Map(x => x.IsHidden);
            Map(x => x.IsDuplicated);
            Map(x => x.Alias);
            Map(x => x.homeIdSelector);
            References(x => x.DisplayName, "NameID");
            Map(x => x.TableName);
            Map(x => x.formatSQL);
            Map(x => x.SupportedByAdvertiser);
            Map(x => x.SupportedByPublisher);
            Map(x => x.minWidth);

            Map(x => x.DataType).CustomType(typeof(DataTypeQB));
            
            Cache.Transactional().ReadWrite().IncludeAll();


        }
    }
}
