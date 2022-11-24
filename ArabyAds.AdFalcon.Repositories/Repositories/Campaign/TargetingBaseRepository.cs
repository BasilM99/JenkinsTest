using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class TargetingBaseRepository : RepositoryBase<Domain.Model.Campaign.Targeting.TargetingBase, int>, ITargetingBaseRepository
    {
        public TargetingBaseRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.TargetingBase, int> repository)
            : base(repository)
        {
        }
    }
}
