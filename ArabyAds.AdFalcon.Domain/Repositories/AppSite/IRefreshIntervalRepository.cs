using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IRefreshIntervalRepository : IKeyedRepository<AppSiteRefreshMode, int>
    {
    }
}
