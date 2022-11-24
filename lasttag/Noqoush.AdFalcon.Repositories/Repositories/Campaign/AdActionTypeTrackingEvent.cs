using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdActionTypeTrackingEventRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdActionTypeTrackingEvent, int>, IAdActionTypeTrackingEventRepository
    {
        public AdActionTypeTrackingEventRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdActionTypeTrackingEvent, int> repository)
            : base(repository)
        {
        }
    }
}
