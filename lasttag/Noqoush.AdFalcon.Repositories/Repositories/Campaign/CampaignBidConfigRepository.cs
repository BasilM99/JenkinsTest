using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignBidConfigRepository : RepositoryBase<Domain.Model.Campaign.AdGroupBidConfig, int>, ICampaignBidConfigRepository
    {
        public CampaignBidConfigRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupBidConfig, int> repository)
            : base(repository)
        {
        }
    }
}
