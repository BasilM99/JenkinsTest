using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdActionTypeRepository : RepositoryBase<Domain.Model.Campaign.Objective.AdActionTypeBase, int>, IAdActionTypeRepository
    {
        public AdActionTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Objective.AdActionTypeBase, int> repository)
            : base(repository)
        {
        }
    }
}
