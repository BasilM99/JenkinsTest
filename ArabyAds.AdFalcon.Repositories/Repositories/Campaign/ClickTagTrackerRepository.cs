using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class ClickTagTrackerRepository : RepositoryBase<ClickTagTracker, int>, IClickTagTrackerRepository
    {
        public ClickTagTrackerRepository(RepositoryImplBase<ClickTagTracker, int> repository)
            : base(repository)
        {
        }
    }


    public class ThirdPartyTrackerRepository : RepositoryBase<ThirdPartyTracker, int>, IThirdPartyTrackerRepository
    {
        public ThirdPartyTrackerRepository(RepositoryImplBase<ThirdPartyTracker, int> repository)
            : base(repository)
        {
        }
    }


    
}
