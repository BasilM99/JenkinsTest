using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdvertiserAccountRepository : RepositoryBase<AdvertiserAccount, int>, IAdvertiserAccountRepository
    {
        public AdvertiserAccountRepository(RepositoryImplBase<AdvertiserAccount, int> repository)
            : base(repository)
        {
        }
    }
}
