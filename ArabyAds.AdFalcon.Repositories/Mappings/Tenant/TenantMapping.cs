using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Tenant.Mapping
{
   
    public class TenantMapping : ClassMap<ArabyAds.Framework.Tenant>
    {
        public TenantMapping()
        {
            Table("tenants");
            Id(d => d.ID, "Id").GeneratedBy.Identity();
            Map(d => d.Name).Not.Nullable().Length(255);
            Map(d => d.Domain).Not.Nullable().Length(255);
            Map(d => d.Code);
            Cache.Transactional().ReadWrite().IncludeAll();
          
        }
    }
}
