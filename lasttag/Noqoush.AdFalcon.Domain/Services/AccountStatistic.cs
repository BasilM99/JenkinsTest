using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Account.PMP;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Domain.Services
{
    public class AccountStatistic : IAccountStatistic
    {
        private const string CACHE_KEY = "AccountStatistic-{0}-Count-{1}";
        private const string CACHE_KEY_IDS = "AccountStatistic-{0}-Ids-{1}";


        private const string USER_CACHE_KEY = "User_AccountStatistic-{0}-Count-{1}";
        private const string USER_CACHE_KEY_IDS = "User_AccountStatistic-{0}-Ids-{1}";
        private IAppSiteRepository appSiteRepository;
        private IAdRepository adRepository;



        private ICampaignRepository campaignRepository;
        private IAdGroupRepository adGroupRepository;

        private IPMPDealRepository PMPdealrepository;
        private IAudienceSegmentRepository AudienceSegmentRepository;
        private IAdvertiserAccountRepository AdvertiserAccountRepository;
        static readonly object LockObj = new object();

        public AccountStatistic(IAppSiteRepository appSiteRepository, IAdRepository adRepository, IAdGroupRepository adGroupRepository, ICampaignRepository campaignRepository, IPMPDealRepository PMPDealRepository, IAudienceSegmentRepository AudienceSegmentRepositoryvar, IAdvertiserAccountRepository advertiserAccountRepository)
        {
            this.appSiteRepository = appSiteRepository;
            this.adRepository = adRepository;


            this.campaignRepository = campaignRepository;
            this.adGroupRepository = adGroupRepository;

            this.PMPdealrepository = PMPDealRepository;
            this.AudienceSegmentRepository= AudienceSegmentRepositoryvar;
            this.AdvertiserAccountRepository = advertiserAccountRepository;
        }
        public int GetAppCount(Domain.Model.Account.Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY, "Apps", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
            if (!value.HasValue)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
                    if (!value.HasValue)
                    {
                        var apps = appSiteRepository.Query(x => x.Account.ID == account.ID).ToList();
                        value = apps.Count;
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value.Value);
                    }
                }
            }
            return value.Value;
        }
        public int GetDealCount(Domain.Model.Account.Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY, "Deals", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
            if (!value.HasValue)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
                    if (!value.HasValue)
                    {
                        var deals = PMPdealrepository.Query(x => x.Account.ID == account.ID).ToList();
                        value = deals.Count;
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value.Value);
                    }
                }
            }
            return value.Value;
        }
        public int GetAdCount(Domain.Model.Account.Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY, "Ads", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
            if (!value.HasValue)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
                    if (!value.HasValue)
                    {
                        var ads = adRepository.Query(x => x.Group.Campaign.Account.ID == account.ID).ToList();
                        value = ads.Count;
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value.Value);
                    }
                }
            }
            return value.Value;
        }
        public int GetAudienceSegmentCount(Domain.Model.Account.Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY, "AudienceSegments", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
            if (!value.HasValue)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<int?>(key);
                    if (!value.HasValue)
                    {
                        var deals = AudienceSegmentRepository.Query(x => x.Provider.Account.ID == account.ID).ToList();
                        value = deals.Count;
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value.Value);
                    }
                }
            }
            return value.Value;
        }
        public IList<int> GetAdIds(Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY_IDS, "Ads", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
            if (value == null)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
                    if (value == null)
                    {
                        var ads = adRepository.Query(x => x.Group.Campaign.Account.ID == account.ID).ToList();
                        value = ads.Select(a => a.ID).ToList();
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                    }
                }
            }
            return value;
        }

        public IList<int> GetAppIds(Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY_IDS, "Apps", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
            if (value == null)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
                    if (value == null)
                    {
                        var apps = appSiteRepository.Query(x => x.Account.ID == account.ID).ToList();
                        value = apps.Select(a => a.ID).ToList();
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                    }
                }
            }
            return value;
        }

        public IList<int> GeDealsIds(Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY_IDS, "Deals", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
            if (value == null)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
                    if (value == null)
                    {
                        var deals = PMPdealrepository.Query(x => x.Account.ID == account.ID).ToList();
                        value = deals.Select(a => a.ID).ToList();
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                    }
                }
            }
            return value;
        }
        public IList<int> GeAudienceSegmentsIds(Account account)
        {
            //check if data found in cache
            var key = string.Format(CACHE_KEY_IDS, "AudienceSegments", account.ID);
            var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
            if (value == null)
            {
                lock (LockObj)
                {
                    // get from account then cache value
                    value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<IList<int>>(key);
                    if (value == null)
                    {
                        var deals = AudienceSegmentRepository.Query(x => x.Provider.Account.ID == account.ID).ToList();
                        value = deals.Select(a => a.ID).ToList();
                        Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                    }
                }
            }
            return value;
        }

        public IList<int> GetAdIdsPerUser(int accountId, int userId)
        {
          
        var ads = adRepository.Query(x => x.Group.Campaign.Account.ID == accountId && x.Group.Campaign.User.ID==userId).ToList();
        var  value = ads.Select(a => a.ID).ToList();
                 
            return value;
        }
        public IList<int> GetCampaignIdsPerUser(int accountId, int userId)
        {
            var ads = campaignRepository.Query(x => x.Account.ID == accountId && x.User.ID == userId).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }
        public IList<int> GetNotAllowedAdvertiserAsscoiation(int accountId, int userId)
        {
            AdvertiserAccountCriteria cri = new AdvertiserAccountCriteria();
            cri.AccountId = accountId;
            cri.userId = userId; 

            var ads = AdvertiserAccountRepository.Query(cri.GetExpressionNoAccess()).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }
        public IList<int> GetAdGroupIdsPerUser(int accountId, int userId)
        {
            var ads = adGroupRepository.Query(x => x.Campaign.Account.ID == accountId && x.Campaign.User.ID == userId).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }

        public IList<int> GetAppIdsPerUser(int accountId, int userId)
        {
            var ads = appSiteRepository.Query(x => x.Account.ID == accountId && x.User.ID == userId).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }

        public IList<int> GetDealIdsPerUser(int accountId, int userId)
        {
            var ads = PMPdealrepository.Query(x => x.Account.ID == accountId && x.User.ID == userId).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }
        public IList<int> GetAudienceSegmentIdsPerUser(int accountId, int userId)
        {
            var ads = AudienceSegmentRepository.Query(x => x.Provider.Account.ID == accountId ).ToList();
            var value = ads.Select(a => a.ID).ToList();

            return value;
        }
    }
}
