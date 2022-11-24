using ArabyAds.Framework.Persistence;
using ArabyAds.Framework;
using System.Linq;
using System.Threading;
using ArabyAds.AdFalcon.Domain.Repositories.Tenant;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Tenant
{
 

    public class TenantRepository : RepositoryBase<ArabyAds.Framework.Tenant, int>, ITenantRepository
    {
        public TenantRepository(RepositoryImplBase<ArabyAds.Framework.Tenant, int> repository)
            : base(repository)
        {
        }

        public int GetTenanyIdByDomain(string Domain)
        {
            ArabyAds.Framework.Tenant currentTenant = this.Query(M => M.Domain == Domain).SingleOrDefault();
            return currentTenant!=null? currentTenant.ID : 0;

        }

        public ArabyAds.Framework.Tenant GetTenantByDomain(string Domain)
        {
            ArabyAds.Framework.Tenant currentTenant = this.Query(M => M.Domain == Domain).SingleOrDefault();
            return currentTenant ;

        }
    }
}
