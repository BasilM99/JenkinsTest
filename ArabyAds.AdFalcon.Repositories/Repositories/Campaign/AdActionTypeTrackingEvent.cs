using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdActionTypeTrackingEventRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdActionTypeTrackingEvent, int>, IAdActionTypeTrackingEventRepository
    {
        public AdActionTypeTrackingEventRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdActionTypeTrackingEvent, int> repository)
            : base(repository)
        {
        }
    }
}
