using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestTargetingRepository : RepositoryBase<AdRequestTargeting, int>, IAdRequestTargetingRepository
    {
        public AdRequestTargetingRepository(RepositoryImplBase<AdRequestTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
