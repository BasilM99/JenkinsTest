using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AccountFeaturesRepository : RepositoryBase<AccountFeatures, int>, IAccountFeaturesRepository
    {

        public AccountFeaturesRepository(RepositoryImplBase<AccountFeatures, int> repository)
            : base(repository)
        {


        }
    }
}
