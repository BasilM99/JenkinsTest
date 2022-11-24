
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{


    public class AdvertiserAccountMasterAppSiteRepository : RepositoryBase<AdvertiserAccountMasterAppSite, int>, IAdvertiserAccountMasterAppSiteRepository
    {

        public AdvertiserAccountMasterAppSiteRepository(RepositoryImplBase<AdvertiserAccountMasterAppSite, int> repository)
          : base(repository)
        {
        }
    }
}