using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
  

    public class AdGroupEventRepository : RepositoryBase<Domain.Model.Campaign.AdGroupEvent, int>, IAdGroupEventRepository
    {
        public AdGroupEventRepository(RepositoryImplBase<AdGroupEvent, int> repository)
            : base(repository)
        {
        }
    }
}
