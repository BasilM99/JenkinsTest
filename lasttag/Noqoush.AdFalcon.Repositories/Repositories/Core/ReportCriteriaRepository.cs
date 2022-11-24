using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class ReportCriteriaRepository : RepositoryBase<ReportCriteria, int>, IReportCriteriaRepository
    {
        public ReportCriteriaRepository(RepositoryImplBase<ReportCriteria, int> repository)
            : base(repository)
        {


        }
    }
}
