using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class FeatureRepository : RepositoryBase<Feature, int>, IFeatureRepository
    {

        public FeatureRepository(RepositoryImplBase<Feature, int> repository)
            : base(repository)
        {


        }
    }
}
