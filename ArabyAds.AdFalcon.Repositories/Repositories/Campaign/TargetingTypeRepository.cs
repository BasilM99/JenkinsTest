using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class TargetingTypeRepository : RepositoryBase<Domain.Model.Campaign.Targeting.TargetingType, int>, ITargetingTypeRepository
    {
        public TargetingTypeRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.TargetingType, int> repository)
            : base(repository)
        {
        }
    }
}
