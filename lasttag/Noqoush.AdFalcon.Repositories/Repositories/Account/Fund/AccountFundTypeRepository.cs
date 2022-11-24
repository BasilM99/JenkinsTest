using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    class AccountFundTypeRepository : RepositoryBase<AccountFundType, int>, IAccountFundTypeRepository
    {
        public AccountFundTypeRepository(RepositoryImplBase<AccountFundType, int> repository)
            : base(repository)
        {
        }

    }
}