using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class PartyRepository : RepositoryBase<Party, int>, IPartyRepository
    {
        public PartyRepository(RepositoryImplBase<Party, int> repository)
            : base(repository)
        {


        }
    }
}
