using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
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
