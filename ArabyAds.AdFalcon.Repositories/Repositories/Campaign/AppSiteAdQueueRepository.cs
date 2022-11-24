using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AppSiteAdQueueRepository: RepositoryBase<AppSiteAdQueue, int>, IAppSiteAdQueueRepository
    {
        public AppSiteAdQueueRepository(RepositoryImplBase<AppSiteAdQueue, int> repository)
            : base(repository)
        {
        }
    }
}
