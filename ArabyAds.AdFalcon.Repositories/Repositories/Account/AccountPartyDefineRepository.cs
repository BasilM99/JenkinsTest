using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AccountPartyDefineRepository : RepositoryBase<AccountPartyDefine, int>, IAccountPartyDefineRepository
    {
        public AccountPartyDefineRepository(RepositoryImplBase<AccountPartyDefine, int> repository)
            : base(repository)
        {
        }
    }
}
