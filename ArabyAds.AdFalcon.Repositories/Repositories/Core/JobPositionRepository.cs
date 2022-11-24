using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class JobPositionRepository : RepositoryBase<JobPosition, int>, IJobPositionRepository
    {
        public JobPositionRepository(RepositoryImplBase<JobPosition, int> repository)
            : base(repository)
        {
        }
    }
}
