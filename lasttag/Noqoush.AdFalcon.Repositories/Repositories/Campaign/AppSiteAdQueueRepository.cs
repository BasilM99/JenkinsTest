using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AppSiteAdQueueRepository: RepositoryBase<AppSiteAdQueue, int>, IAppSiteAdQueueRepository
    {
        public AppSiteAdQueueRepository(RepositoryImplBase<AppSiteAdQueue, int> repository)
            : base(repository)
        {
        }
    }
}
