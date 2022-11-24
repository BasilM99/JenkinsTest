using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
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


    }
}
