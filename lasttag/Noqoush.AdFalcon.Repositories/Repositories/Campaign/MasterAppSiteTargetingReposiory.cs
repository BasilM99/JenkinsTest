using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;



namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
   
    public class MasterAppSiteTargetingRepository : RepositoryBase<Domain.Model.Campaign.Targeting.MasterAppSiteTargeting, int>, IMasterAppSiteTargetingRepository
    {
        public MasterAppSiteTargetingRepository(RepositoryImplBase<Domain.Model.Campaign.Targeting.MasterAppSiteTargeting, int> repository)
            : base(repository)
        {
        }
    }
}
