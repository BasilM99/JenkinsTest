using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IRefreshIntervalRepository : IKeyedRepository<AppSiteRefreshMode, int>
    {
    }
}
