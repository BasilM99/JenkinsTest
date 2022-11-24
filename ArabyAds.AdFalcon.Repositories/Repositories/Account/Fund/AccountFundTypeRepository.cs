using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    class AccountFundTypeRepository : RepositoryBase<AccountFundType, int>, IAccountFundTypeRepository
    {
        public AccountFundTypeRepository(RepositoryImplBase<AccountFundType, int> repository)
            : base(repository)
        {
        }

    }
}