using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestTypePlatformVersionRepository : RepositoryBase<AdRequestTypePlatformVersion, int>, IAdRequestTypePlatformVersionRepository
    {
        public AdRequestTypePlatformVersionRepository(RepositoryImplBase<AdRequestTypePlatformVersion, int> repository)
            : base(repository)
        {
        }
    }
}
