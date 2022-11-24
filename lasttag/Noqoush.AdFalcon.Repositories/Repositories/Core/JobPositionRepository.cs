using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class JobPositionRepository : RepositoryBase<JobPosition, int>, IJobPositionRepository
    {
        public JobPositionRepository(RepositoryImplBase<JobPosition, int> repository)
            : base(repository)
        {
        }
    }
}
