using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class CityRepository : RepositoryBase<City, int>, ICityRepository
    {
        public CityRepository(RepositoryImplBase<City, int> repository)
            : base(repository)
        {


        }
    }
}
