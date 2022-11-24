
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class CompanyTypeRepository : RepositoryBase<CompanyType, int>, ICompanyTypeRepository
    {
        public CompanyTypeRepository(RepositoryImplBase<CompanyType, int> repository)
            : base(repository)
        {


        }
    }
}
