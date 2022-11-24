using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class AccountFeaturesRepository : RepositoryBase<AccountFeatures, int>, IAccountFeaturesRepository
    {

        public AccountFeaturesRepository(RepositoryImplBase<AccountFeatures, int> repository)
            : base(repository)
        {


        }
    }
}
