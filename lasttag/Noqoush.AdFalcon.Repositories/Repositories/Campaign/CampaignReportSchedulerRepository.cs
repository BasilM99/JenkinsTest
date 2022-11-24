using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignReportSchedulerRepository : RepositoryBase<CampaignReportScheduler, int>, ICampaignReportSchedulerRepository 
    {
        public CampaignReportSchedulerRepository(RepositoryImplBase<CampaignReportScheduler, int> repository)
            : base(repository)
        {
        }
    }
}
