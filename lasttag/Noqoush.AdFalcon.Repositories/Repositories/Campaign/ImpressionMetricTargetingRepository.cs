
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class ImpressionMetricTargetingRepository : RepositoryBase<ImpressionMetricTargeting, int>, IImpressionMetricTargetingRepository
    {
        public ImpressionMetricTargetingRepository(RepositoryImplBase<ImpressionMetricTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
