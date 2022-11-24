using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.AppSite
{
    public class AppSiteStatusRepository : RepositoryBase<AppSiteStatus, int>, IAppSiteStatusRepository
    {
        public AppSiteStatusRepository(RepositoryImplBase<AppSiteStatus, int> repository)
            : base(repository)
        {
        }
    }
}
