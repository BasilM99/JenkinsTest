using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;


namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class AdGroupInventorySourceRepository : RepositoryBase<Domain.Model.Campaign.AdGroupInventorySource, int>, IAdGroupInventorySourceRepository
    {
        public AdGroupInventorySourceRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupInventorySource, int> repository)
            : base(repository)
        {
        }
    }
}
