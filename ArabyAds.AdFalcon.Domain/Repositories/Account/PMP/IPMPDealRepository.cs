using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account.PMP
{
    

    public interface IPMPDealRepository : IKeyedRepository<PMPDeal, int>
    {
        IEnumerable<PMPDeal> GetPMPDeals(PMPDealCriteria filter, out int TotalCount);
        IList<PMPDeal> GetAllPMPDealsByAccount(int AccountId);
        IList<Domain.Model.Campaign.Campaign> getCampsBydeal(int DealId);
        IList<AdGroup> getDealCampsAdgruops(int dealId, int campId);
        IList<Domain.Model.Campaign.AdvertiserAccount> getAdvertiserAccountsBydeal(int DealId);
        IList<PMPDeal> GetAllPMPDeals(int AccountId, int AdvertiserAccountId);
        bool IsCampsBydeal(int DealId);


    }
}
