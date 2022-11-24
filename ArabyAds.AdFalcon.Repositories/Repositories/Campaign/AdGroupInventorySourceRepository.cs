using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class AdGroupInventorySourceRepository : RepositoryBase<Domain.Model.Campaign.AdGroupInventorySource, int>, IAdGroupInventorySourceRepository
    {
        public AdGroupInventorySourceRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupInventorySource, int> repository)
            : base(repository)
        {
        }
    }
}
