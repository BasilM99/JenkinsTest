using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class CityRepository : RepositoryBase<City, int>, ICityRepository
    {
        public CityRepository(RepositoryImplBase<City, int> repository)
            : base(repository)
        {


        }
    }
}
