using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignFrequencyCappingRepository : RepositoryBase<Domain.Model.Campaign.CampaignFrequencyCapping, int>, ICampaignFrequencyCappingRepository
    {
        public CampaignFrequencyCappingRepository(RepositoryImplBase<Domain.Model.Campaign.CampaignFrequencyCapping, int> repository)
            : base(repository)
        {
        }
    }
}
