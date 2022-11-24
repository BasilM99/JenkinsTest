using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core.Payment
{
    class AccountPaymentDetailsRepository : RepositoryBase<AccountPaymentDetails, int>, IAccountPaymentDetailsRepository
    {
        public AccountPaymentDetailsRepository(RepositoryImplBase<AccountPaymentDetails, int> repository)
            : base(repository)
        {
        }
    }
}
