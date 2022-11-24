using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdvertiserAccountRepository : RepositoryBase<AdvertiserAccount, int>, IAdvertiserAccountRepository
    {
        public AdvertiserAccountRepository(RepositoryImplBase<AdvertiserAccount, int> repository)
            : base(repository)
        {
        }
    }
}
