using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class ContinentRepository : RepositoryBase<Continent, int>, IContinentRepository
    {
        public ContinentRepository(RepositoryImplBase<Continent, int> repository)
            : base(repository)
        {


        }
    }
}
