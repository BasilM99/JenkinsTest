using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class StateRepository : RepositoryBase<State, int>, IStateRepository
    {
        public StateRepository(RepositoryImplBase<State, int> repository)
            : base(repository)
        {


        }
    }
}
