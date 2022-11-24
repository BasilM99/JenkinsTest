using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class BusinessPartnerTypeRepository : RepositoryBase<BusinessPartnerType, int>, IBusinessPartnerTypeRepository
    {
        public BusinessPartnerTypeRepository(RepositoryImplBase<BusinessPartnerType, int> repository)
            : base(repository)
        {


        }
    }
}
