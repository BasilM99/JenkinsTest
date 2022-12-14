using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.QueryBuilder
{

    public class FactMapping : ClassMap<Fact>
    {
        public FactMapping()
        {
           Schema("adfalcon_report");
            Table("qb_facts");
            Id(x => x.Id);
            Map(x => x.IsDeleted);
            Map(x => x.Name);
            Map(x => x.DisplayName);
            Map(x => x.WebDisplayName);
            Map(x => x.IsForWeb);

        HasManyToMany(x => x.Dimensions)
                .ChildKeyColumn("dimId")
                .ParentKeyColumn("factId")
                .Table("adfalcon_report.qb_bridge_fact_dimension")
                .Fetch.Select().BatchSize(200)
                .AsSet().Cascade.All().Cache.ReadWrite(); 

            HasManyToMany(x => x.Measures)
          .ChildKeyColumn("measurid")
          .ParentKeyColumn("factid")
          .Table("adfalcon_report.qb_bridge_fact_measure")
          .Fetch.Select().BatchSize(200)
          .AsSet().Cascade.All().Cache.ReadWrite(); 

            HasManyToMany(x => x.Columns)
        .ChildKeyColumn("colid")
          .ChildWhere(x=>!x.IsDeleted)

        .ParentKeyColumn("factId")
        .Table("adfalcon_report.qb_bridge_fact_column")
        .Fetch.Select()
        .AsSet().Cascade.All().Cache.ReadWrite(); 

            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
