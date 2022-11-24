using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AgeGroupRepository : RepositoryBase<AgeGroup, int>, IAgeGroupRepository
    {
        public AgeGroupRepository(RepositoryImplBase<AgeGroup, int> repository)
            : base(repository)
        {


        }
    }
}
