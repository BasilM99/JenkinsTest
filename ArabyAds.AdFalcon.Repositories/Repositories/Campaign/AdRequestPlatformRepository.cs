using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestPlatformRepository : RepositoryBase<AdRequestPlatform, int>, IAdRequestPlatformRepository
    {
        public AdRequestPlatformRepository(RepositoryImplBase<AdRequestPlatform, int> repository)
            : base(repository)
        {

        }
    }
}
