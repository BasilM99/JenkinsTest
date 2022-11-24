using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;



namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
   
    public class MasterAppSiteTargetingRepository : RepositoryBase<Domain.Model.Campaign.Targeting.MasterAppSiteTargeting, int>, IMasterAppSiteTargetingRepository
    {
        public MasterAppSiteTargetingRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.MasterAppSiteTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
