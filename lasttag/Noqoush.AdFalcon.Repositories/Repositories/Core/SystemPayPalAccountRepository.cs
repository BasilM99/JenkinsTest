using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class SystemPayPalAccountRepository : RepositoryBase<SystemPayPalAccount, int>, ISystemPayPalAccountRepository
    {
        public SystemPayPalAccountRepository(RepositoryImplBase<SystemPayPalAccount, int> repository)
            : base(repository)
        {


        }
    }
}
