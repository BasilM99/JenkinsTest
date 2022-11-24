using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class SystemBankAccountRepository : RepositoryBase<SystemBankAccount, int>, ISystemBankAccountRepository
    {
        public SystemBankAccountRepository(RepositoryImplBase<SystemBankAccount, int> repository)
            : base(repository)
        {


        }
    }
}
