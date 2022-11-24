
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AccountInvitationRepository : RepositoryBase<AccountInvitation, int>, IAccountInvitationRepository
    {
        public AccountInvitationRepository(RepositoryImplBase<AccountInvitation, int> repository)
            : base(repository)
        {
        }
    }
}
