using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestPlatformRepository : RepositoryBase<AdRequestPlatform, int>, IAdRequestPlatformRepository
    {
        public AdRequestPlatformRepository(RepositoryImplBase<AdRequestPlatform, int> repository)
            : base(repository)
        {

        }
    }
}
