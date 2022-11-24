using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupDynamicBiddingConfigRepository : RepositoryBase<AdGroupDynamicBiddingConfig, int>, IAdGroupDynamicBiddingConfigRepository
    {
        public AdGroupDynamicBiddingConfigRepository(RepositoryImplBase<AdGroupDynamicBiddingConfig, int> repository)
            : base(repository)
        {
        }
    }
}
