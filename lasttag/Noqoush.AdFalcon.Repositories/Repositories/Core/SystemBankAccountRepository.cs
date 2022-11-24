using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class SystemBankAccountRepository : RepositoryBase<SystemBankAccount, int>, ISystemBankAccountRepository
    {
        public SystemBankAccountRepository(RepositoryImplBase<SystemBankAccount, int> repository)
            : base(repository)
        {


        }
    }
}
