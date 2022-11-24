using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CreativeUnitGroupMapping : ClassMap<CreativeUnitGroup>
    {
        public CreativeUnitGroupMapping()
        {
            Table("creativeunit_groups");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.Description);
            Map(p => p.Code);
            HasManyToMany(p => p.CreativeUnits).Table("creativeunit_groups_mapping").ParentKeyColumn("GroupId").ChildKeyColumn("CreativeUnitId").Fetch.Select().Cache.ReadWrite(); 

            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(1000);
        }
    }
}
