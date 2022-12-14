using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupObjectiveTypeRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdGroupObjectiveType, int>, IAdGroupObjectiveTypeRepository
    {
        public AdGroupObjectiveTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdGroupObjectiveType, int> repository)
            : base(repository)
        {
        }
    }
}
