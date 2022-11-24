using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class SystemPayPalAccountRepository : RepositoryBase<SystemPayPalAccount, int>, ISystemPayPalAccountRepository
    {
        public SystemPayPalAccountRepository(RepositoryImplBase<SystemPayPalAccount, int> repository)
            : base(repository)
        {


        }
    }
}
