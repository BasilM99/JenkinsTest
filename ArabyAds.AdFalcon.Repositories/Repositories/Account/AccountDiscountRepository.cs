using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AccountDiscountRepository : RepositoryBase<AccountDiscount, int>, IAccountDiscountRepository
    {
        public AccountDiscountRepository(RepositoryImplBase<AccountDiscount, int> repository)
            : base(repository)
        {
        }
    }
}
