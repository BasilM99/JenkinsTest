using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core.Payment
{
    class AccountPaymentDetailsRepository : RepositoryBase<AccountPaymentDetails, int>, IAccountPaymentDetailsRepository
    {
        public AccountPaymentDetailsRepository(RepositoryImplBase<AccountPaymentDetails, int> repository)
            : base(repository)
        {
        }
    }
}
