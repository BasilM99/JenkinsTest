using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignPerformanceRepository : RepositoryBase<Domain.Model.Campaign.Performance.CampaignPerformance, int>, ICampaignPerformanceRepository
    {
        public CampaignPerformanceRepository(RepositoryImplBase<Domain.Model.Campaign.Performance.CampaignPerformance, int> repository)
            : base(repository)
        {
        }
    }
}
