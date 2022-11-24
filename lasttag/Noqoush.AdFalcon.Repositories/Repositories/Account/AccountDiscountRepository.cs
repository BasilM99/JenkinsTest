using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class AccountDiscountRepository : RepositoryBase<AccountDiscount, int>, IAccountDiscountRepository
    {
        public AccountDiscountRepository(RepositoryImplBase<AccountDiscount, int> repository)
            : base(repository)
        {
        }
    }
}
