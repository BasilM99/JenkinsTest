using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Repositories.Account.PMP
{
    

    public interface IPMPDealRepository : IKeyedRepository<PMPDeal, int>
    {
        IEnumerable<PMPDeal> GetPMPDeals(PMPDealCriteria filter, out int TotalCount);
        IList<PMPDeal> GetAllPMPDealsByAccount(int AccountId);

        IList<Domain.Model.Campaign.Campaign> getCampsBydeal(int DealId);
        IList<AdGroup> getDealCampsAdgruops(int dealId, int campId);
        IList<Domain.Model.Campaign.AdvertiserAccount> getAdvertiserAccountsBydeal(int DealId);
        bool IsCampsBydeal(int DealId);


    }
}
