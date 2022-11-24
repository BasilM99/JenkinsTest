using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class ObjectiveTypeRepository : RepositoryBase<AdGroupObjectiveType, int>, IObjectiveTypeRepository
    {
        public ObjectiveTypeRepository(RepositoryImplBase<AdGroupObjectiveType, int> repository)
            : base(repository)
        {
        }
    }
}
