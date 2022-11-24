using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class HouseAdRepository  : RepositoryBase<Domain.Model.Campaign.HouseAd, int>, IHouseAdRepository 
    {
        public HouseAdRepository(RepositoryImplBase<Domain.Model.Campaign.HouseAd, int> repository)
            : base(repository)
        {
        }
    }
}
