using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Services
{
    public interface IAccountStatistic
    {
        int GetAppCount(Domain.Model.Account.Account account);
        int GetAdCount(Domain.Model.Account.Account account);
        IList<int> GetAdIds(Domain.Model.Account.Account account);
        IList<int> GetAppIds(Domain.Model.Account.Account account);


        IList<int> GetCampaignIdsPerUser(int accountId, int userId);
        IList<int> GetAdGroupIdsPerUser(int accountId, int userId);
        IList<int> GetAdIdsPerUser(int accountId, int userId);
        IList<int> GetAppIdsPerUser(int accountId, int userId);
        IList<int> GeDealsIds(Domain.Model.Account.Account account);

        int GetDealCount(Domain.Model.Account.Account account);
        IList<int> GetDealIdsPerUser(int accountId, int userId);


        IList<int> GeAudienceSegmentsIds(Domain.Model.Account.Account account);

        int GetAudienceSegmentCount(Domain.Model.Account.Account account);
        IList<int> GetAudienceSegmentIdsPerUser(int accountId, int userId);

        IList<int> GetNotAllowedAdvertiserAsscoiation(int accountId, int userId);

    }
}
