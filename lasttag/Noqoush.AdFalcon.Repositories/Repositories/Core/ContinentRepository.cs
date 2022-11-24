using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class ContinentRepository : RepositoryBase<Continent, int>, IContinentRepository
    {
        public ContinentRepository(RepositoryImplBase<Continent, int> repository)
            : base(repository)
        {


        }
    }
}
