using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;
using System.Collections.Generic;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdGroupRepository : IKeyedRepository<Model.Campaign.AdGroup, int>
    {
        string GetObjectName(int Id);
        bool AdGroupHasAds(int id);
        IList<AdGroup> GetAdGroupsByCampaign(int id);
    }
}
