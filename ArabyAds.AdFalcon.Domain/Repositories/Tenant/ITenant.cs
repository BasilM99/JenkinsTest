using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Tenant
{
    public interface ITenant<T>
    {
      
    }

    public interface ITenantRepository : IKeyedRepository<ArabyAds.Framework.Tenant, int>
    {
        int GetTenanyIdByDomain(string Domain);
        ArabyAds.Framework.Tenant GetTenantByDomain(string Domain);
    }
}
