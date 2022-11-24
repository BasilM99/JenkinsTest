using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class ObjectiveTypeRepository : RepositoryBase<AdGroupObjectiveType, int>, IObjectiveTypeRepository
    {
        public ObjectiveTypeRepository(RepositoryImplBase<AdGroupObjectiveType, int> repository)
            : base(repository)
        {
        }
    }
}
