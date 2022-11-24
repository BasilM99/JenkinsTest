using Noqoush.Framework.Persistence;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface ICampaignAssignedAppsiteRepository : IKeyedRepository<Model.Campaign.CampaignAssignedAppsite, int>
    {
        IList<int> GetCampaignIdsByAppSiteId(int AppSiteId);
    }
}
