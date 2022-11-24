using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignFrequencyCappingRepository : RepositoryBase<Domain.Model.Campaign.CampaignFrequencyCapping, int>, ICampaignFrequencyCappingRepository
    {
        public CampaignFrequencyCappingRepository(RepositoryImplBase<Domain.Model.Campaign.CampaignFrequencyCapping, int> repository)
            : base(repository)
        {
        }
    }
}
