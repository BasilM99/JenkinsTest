using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class TargetingBaseRepository : RepositoryBase<Domain.Model.Campaign.Targeting.TargetingBase, int>, ITargetingBaseRepository
    {
        public TargetingBaseRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.TargetingBase, int> repository)
            : base(repository)
        {
        }
    }
}
