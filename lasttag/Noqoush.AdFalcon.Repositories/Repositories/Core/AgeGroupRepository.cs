using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class AgeGroupRepository : RepositoryBase<AgeGroup, int>, IAgeGroupRepository
    {
        public AgeGroupRepository(RepositoryImplBase<AgeGroup, int> repository)
            : base(repository)
        {


        }
    }
}
