using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignPerformanceRepository : RepositoryBase<Domain.Model.Campaign.Performance.CampaignPerformance, int>, ICampaignPerformanceRepository
    {
        public CampaignPerformanceRepository(RepositoryImplBase<Domain.Model.Campaign.Performance.CampaignPerformance, int> repository)
            : base(repository)
        {
        }
    }
}
