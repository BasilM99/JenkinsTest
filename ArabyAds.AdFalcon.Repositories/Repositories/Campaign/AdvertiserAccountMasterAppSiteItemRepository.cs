using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    

    public class AdvertiserAccountMasterAppSiteItemRepository : RepositoryBase<AdvertiserAccountMasterAppSiteItem, int>, IAdvertiserAccountMasterAppSiteItemRepository
    {

        public AdvertiserAccountMasterAppSiteItemRepository(RepositoryImplBase<AdvertiserAccountMasterAppSiteItem, int> repository)
          : base(repository)
        {
        }
    }
}