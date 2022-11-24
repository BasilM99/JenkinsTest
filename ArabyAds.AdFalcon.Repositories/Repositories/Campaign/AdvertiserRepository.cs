using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdvertiserRepository : RepositoryBase<Advertiser, int>, IAdvertiserRepository
    {
        public AdvertiserRepository(RepositoryImplBase<Advertiser, int> repository)
            : base(repository)
        {
        }
    }
}
