using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignStatusRepository  : RepositoryBase<Domain.Model.Campaign.AdCampaignStatus, int>, ICampaignStatusRepository 
    {
        public CampaignStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdCampaignStatus, int> repository)
            : base(repository)
        {
        }
    }
}
