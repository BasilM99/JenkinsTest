using ArabyAds.AdFalcon.Domain.Repositories.Tenant;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.Framework;


namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class TenantService : ITenantService
    {
        private ITenantRepository tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        public TenantDto GetTenantByDomain(string domain)
        {

           Tenant tenant =  tenantRepository.GetTenantByDomain(domain);

            return Mapping.MapperHelper.Map<TenantDto>(tenant);
        }

        public TenantDto GetTenantById(ValueMessageWrapper<int> Id)
        {

            Tenant tenant = tenantRepository.Get(Id.Value);

            return Mapping.MapperHelper.Map<TenantDto>(tenant);
        }
    }
}
