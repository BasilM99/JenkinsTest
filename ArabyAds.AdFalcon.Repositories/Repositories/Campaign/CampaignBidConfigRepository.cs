using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignBidConfigRepository : RepositoryBase<Domain.Model.Campaign.AdGroupBidConfig, int>, ICampaignBidConfigRepository
    {
        public CampaignBidConfigRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupBidConfig, int> repository)
            : base(repository)
        {
        }
    }
}
