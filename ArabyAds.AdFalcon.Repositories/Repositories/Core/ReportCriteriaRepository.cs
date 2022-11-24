using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class ReportCriteriaRepository : RepositoryBase<ReportCriteria, int>, IReportCriteriaRepository
    {
        public ReportCriteriaRepository(RepositoryImplBase<ReportCriteria, int> repository)
            : base(repository)
        {


        }
    }



    public class DashBoardCriteriaRepository : RepositoryBase<DashBoardCriteria, int>, IDashBoardCriteriaRepository
    {
        public DashBoardCriteriaRepository(RepositoryImplBase<DashBoardCriteria, int> repository)
            : base(repository)
        {


        }
    }
}
