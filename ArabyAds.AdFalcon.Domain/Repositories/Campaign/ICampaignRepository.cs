using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;
using System.Collections.Generic;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public interface ICampaignRepository : IKeyedRepository<Model.Campaign.Campaign, int>
    {
        string GetObjectName(int Id);
        bool IsAllowedGroup(AdGroup group);
        bool IsAllowedAd(AdCreative ad);

        bool IsAllowedGroup(int id);
         bool IsAllowedAd(int id);
        int GetAdvertiserId(int Id);
        string GetAdvertiserName(int Id);
        IList<AdGroup> GetAllAdGroupByAccount(int AccountId);
        IList<Model.Campaign.Campaign> Query(CampaignCriteria criteria);


    }
}
