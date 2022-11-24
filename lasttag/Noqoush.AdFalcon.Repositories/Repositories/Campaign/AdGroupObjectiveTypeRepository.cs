using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupObjectiveTypeRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdGroupObjectiveType, int>, IAdGroupObjectiveTypeRepository
    {
        public AdGroupObjectiveTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdGroupObjectiveType, int> repository)
            : base(repository)
        {
        }
    }
}
