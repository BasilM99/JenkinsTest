
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class ImpressionMetricTargetingRepository : RepositoryBase<ImpressionMetricTargeting, int>, IImpressionMetricTargetingRepository
    {
        public ImpressionMetricTargetingRepository(RepositoryImplBase<ImpressionMetricTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
