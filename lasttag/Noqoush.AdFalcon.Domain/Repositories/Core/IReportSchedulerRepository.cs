using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface IReportSchedulerRepository : IKeyedRepository<ReportScheduler, int>
    {
        string GetObjectName(int Id);
    }
}
