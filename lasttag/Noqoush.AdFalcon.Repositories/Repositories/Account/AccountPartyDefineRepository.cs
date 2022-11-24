using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class AccountPartyDefineRepository : RepositoryBase<AccountPartyDefine, int>, IAccountPartyDefineRepository
    {
        public AccountPartyDefineRepository(RepositoryImplBase<AccountPartyDefine, int> repository)
            : base(repository)
        {
        }
    }
}
