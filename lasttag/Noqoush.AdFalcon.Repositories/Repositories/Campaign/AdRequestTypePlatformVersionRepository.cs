using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestTypePlatformVersionRepository : RepositoryBase<AdRequestTypePlatformVersion, int>, IAdRequestTypePlatformVersionRepository
    {
        public AdRequestTypePlatformVersionRepository(RepositoryImplBase<AdRequestTypePlatformVersion, int> repository)
            : base(repository)
        {
        }
    }
}
