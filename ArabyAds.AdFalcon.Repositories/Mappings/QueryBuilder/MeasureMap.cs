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

    public class MeasureMapping : ClassMap<Measure>
    {
        public MeasureMapping()
        {
           Schema("adfalcon_report");
            Table("qb_measures");
            Id(x => x.Id);
            Map(x => x.IsDeleted);
            Map(x => x.IsHidden);

            Map(x => x.Name);
            Map(x => x.Attribute);
            Map(x => x.ParentId);
            Map(x => x.OrderNumber);
           // Map(x => x.DisplayName);
            References(x => x.DisplayName, "NameID");
            Map(x => x.SubstituteAttribute);
            Map(x => x.RawAttribute);
            Map(x => x.SupportedByAdvertiser);
            Map(x => x.SupportedByPublisher);



            Map(x => x.requestsmapping);
            Map(x => x.dealsrequestsmapping);

            Map(x => x.minWidth);
            Map(x => x.DataType).CustomType(typeof(DataTypeQB));

            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
