using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestTargetingRepository : RepositoryBase<AdRequestTargeting, int>, IAdRequestTargetingRepository
    {
        public AdRequestTargetingRepository(RepositoryImplBase<AdRequestTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
