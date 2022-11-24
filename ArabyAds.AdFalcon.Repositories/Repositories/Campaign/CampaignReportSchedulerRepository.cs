using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignReportSchedulerRepository : RepositoryBase<CampaignReportScheduler, int>, ICampaignReportSchedulerRepository 
    {
        public CampaignReportSchedulerRepository(RepositoryImplBase<CampaignReportScheduler, int> repository)
            : base(repository)
        {
        }
    }
}
