using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.AppSite
{
    public class AppSiteStatusRepository : RepositoryBase<AppSiteStatus, int>, IAppSiteStatusRepository
    {
        public AppSiteStatusRepository(RepositoryImplBase<AppSiteStatus, int> repository)
            : base(repository)
        {
        }
    }
}
