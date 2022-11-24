
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class AccountInvitationRepository : RepositoryBase<AccountInvitation, int>, IAccountInvitationRepository
    {
        public AccountInvitationRepository(RepositoryImplBase<AccountInvitation, int> repository)
            : base(repository)
        {
        }
    }
}
