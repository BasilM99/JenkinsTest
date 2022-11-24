using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class TargetingTypeRepository : RepositoryBase<Domain.Model.Campaign.Targeting.TargetingType, int>, ITargetingTypeRepository
    {
        public TargetingTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.TargetingType, int> repository)
            : base(repository)
        {
        }
    }
}
