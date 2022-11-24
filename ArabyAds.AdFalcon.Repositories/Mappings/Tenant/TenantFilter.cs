using FluentNHibernate.Mapping;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Tenant
{
 
    public class TenantFilter : FilterDefinition
    {
        public TenantFilter()
        {
            WithName("TenantFilter").AddParameter("TenantId", NHibernateUtil.Int32).WithCondition("TenantId = :TenantId");
        }
    }
}
