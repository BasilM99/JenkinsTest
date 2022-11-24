using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class StateRepository : RepositoryBase<State, int>, IStateRepository
    {
        public StateRepository(RepositoryImplBase<State, int> repository)
            : base(repository)
        {


        }
    }
}
