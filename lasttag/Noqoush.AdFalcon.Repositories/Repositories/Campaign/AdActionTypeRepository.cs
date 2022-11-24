using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdActionTypeRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdActionTypeBase, int>, IAdActionTypeRepository
    {
        public AdActionTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdActionTypeBase, int> repository)
            : base(repository)
        {
        }
    }
}
