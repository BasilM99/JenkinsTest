using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface IReportSchedulerRepository : IKeyedRepository<ReportScheduler, int>
    {
        string GetObjectName(int Id);
    }
}
