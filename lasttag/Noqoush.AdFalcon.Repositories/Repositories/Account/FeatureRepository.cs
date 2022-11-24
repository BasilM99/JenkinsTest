using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class FeatureRepository : RepositoryBase<Feature, int>, IFeatureRepository
    {

        public FeatureRepository(RepositoryImplBase<Feature, int> repository)
            : base(repository)
        {


        }
    }
}
