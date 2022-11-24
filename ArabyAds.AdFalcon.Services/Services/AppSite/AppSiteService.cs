using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using ArabyAds.AdFalcon.Domain.Model.AppSite.Filtering;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Exceptions.AppSite;
using ArabyAds.AdFalcon.Persistence.Reports.Repositories;
using ArabyAds.AdFalcon.Persistence.Repositories.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.DomainServices.Localization.Repositories;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using NHibernate.Criterion;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using NHibernate.Transform;

namespace ArabyAds.AdFalcon.Services
{
    public class AppSiteService : IAppSiteService
    {
        private const string splitter = "*##*";
        private IBusinessPartnerRepository _BusinessRepository = null;
        private ISSPPartnerRepository _SSPPartnerRepository = null;
        private ISubAppsiteRepository _subAppsiteRepository = null;
        private IAppSiteRepository appSiteRepository = null;
        private ICostModelWrapperRepository costModelWrapperRepository;
        private IAppSiteStatusRepository appSiteStatusRepository;
        private IAppSiteTypeRepository appSiteTypeRepository = null;
        private IKeyWordRepository keyWordRepository = null;
        private IReportRepository reportRepository = null;
        private ISummaryRepository summaryRepository = null;
        private IConfigurationManager configurationManager = null;
        private ITrackingEventRepository trackingEventRepository;
        private ICampaignRepository campaignRepository;
        private IAdGroupRepository adGroupRepository;
        private IAccountRepository accountRepository;
        private ICampaignAssignedAppsiteRepository campaignAssignedAppsiteRepository;
        private ICampaignBidConfigRepository campaignBidConfigRepository;


        public AppSiteService(IAppSiteRepository appSiteRepository,
            IKeyWordRepository keyWordRepository,
            IReportRepository reportRepository,
            ISummaryRepository summaryRepository,
            IAppSiteStatusRepository appSiteStatusRepository,
            IConfigurationManager configurationManager, ITrackingEventRepository trackingEventRepository, IAppSiteTypeRepository appSiteTypeRepository, ICampaignRepository campaignRepository, IAdGroupRepository adGroupRepository, IAccountRepository accountRepository
            , ICampaignAssignedAppsiteRepository campaignAssignedAppsiteRepository
            , ICampaignBidConfigRepository campaignBidConfigRepository, ICostModelWrapperRepository costModelWrapperRepository, ISubAppsiteRepository subAppsiteRepository, ISSPPartnerRepository SSPPartnerRepository, IBusinessPartnerRepository BusinessPartnerRepository)
        {
            this.configurationManager = configurationManager;
            this.appSiteRepository = appSiteRepository;
            this.keyWordRepository = keyWordRepository;
            this.reportRepository = reportRepository;
            this.summaryRepository = summaryRepository;
            this.appSiteStatusRepository = appSiteStatusRepository;
            this.trackingEventRepository = trackingEventRepository;
            this.appSiteTypeRepository = appSiteTypeRepository;
            this.campaignRepository = campaignRepository;
            this.costModelWrapperRepository = costModelWrapperRepository;
            this.adGroupRepository = adGroupRepository;
            this.accountRepository = accountRepository;
            this.campaignAssignedAppsiteRepository = campaignAssignedAppsiteRepository;
            this.campaignBidConfigRepository = campaignBidConfigRepository;
            this._subAppsiteRepository = subAppsiteRepository;
            this._SSPPartnerRepository = SSPPartnerRepository;
            this._BusinessRepository = BusinessPartnerRepository;
        }

        #region AppSite
        public AppSiteListResultDto QueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase wcriteria)
        {
            AppSiteCriteriaBase criteria = new AppSiteCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
                //appCriteria.UserId = UserId;
            }
            ValidateAccount(criteria.AccountId, criteria.UserId);

            var result = new AppSiteListResultDto();
            IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression(), criteria.Page - 1, criteria.Size, item => item.ID, false);
            result.Items = list.Select(appSite => MapperHelper.Map<AppSiteListDto>(appSite)).ToList();
            var performance = new PerformanceCriteria { Ids = result.Items.Select(obj => obj.Id).ToList(), CampaignType = CampaignType.Normal,OtherCampaignType= CampaignType.ProgrammaticGuaranteed };
            var performances = summaryRepository.GetAppSitesPerformance(performance);
            foreach (var appSite in result.Items)
            {
                //load App/Site Performance
                appSite.Performance = performances.FirstOrDefault(item => item.AppSiteID == appSite.Id);
            }
            result.TotalCount = appSiteRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public AppSiteListResultDto QueryByCratiriaForDashboard(ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase wcriteria)
        {
            AppSiteCriteriaBase criteria = new AppSiteCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
                //appCriteria.UserId = UserId;
            }
            ValidateAccount(criteria.AccountId, criteria.UserId);

            var result = new AppSiteListResultDto();
            IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression());
            result.Items = list.Select(appSite => MapperHelper.Map<AppSiteListDto>(appSite)).ToList();
         
            return result;
        }

        public AppSiteListResultDto QueryByAppOpsCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteria wcriteria)
        {

            AppSiteCriteria criteria = new AppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            ValidateAccount(criteria.AccountId, criteria.UserId);

            var result = new AppSiteListResultDto();
            IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression(), criteria.Page - 1, criteria.Size, item => item.ID, true);
            result.Items = list.Select(appSite => MapperHelper.Map<AppSiteListDto>(appSite)).ToList();
            var performance = new PerformanceCriteria { Ids = result.Items.Select(obj => obj.Id).ToList(), CampaignType = CampaignType.Normal, OtherCampaignType = CampaignType.ProgrammaticGuaranteed, FromDate = criteria.DateFrom, ToDate = criteria.DateTo };
            var performances = summaryRepository.GetAppSitesPerformance(performance);
            foreach (var appSite in result.Items)
            {
                //load App/Site Performance
                appSite.Performance = performances.FirstOrDefault(item => item.AppSiteID == appSite.Id);
            }
            result.TotalCount = appSiteRepository.Query(criteria.GetExpression()).Count();
            return result;
        }

        /// <summary>
        /// use this service operation to get all Active AppSites
        /// </summary>
        /// <returns>All Active AppSites</returns>
        public AppSiteListResultDtoBase GetAllActive(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria wcriteria)
        {

            AllAppSiteCriteria criteria = new AllAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AppSiteListResultDtoBase();
            //   _appSiteRepository.GetAll().Where(criteria.GetWhere())
            IEnumerable<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite> list = UnitOfWork.Current.EntitySet<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Where(
          criteria.GetExpression());
            //   IEnumerable<AppSite> list = appSiteRepository.GetAll().Where(criteria.GetWhere());

            /*
            var ListOfAppIds= _SSPPartnerRepository.CheckWeatherNotMeetRTBSettings();
            if(ListOfAppIds!=null && ListOfAppIds.Count>0)
            list= list.Where(M => M.ID.IsIn(ListOfAppIds.ToArray())==false).ToList();
          */

            var pageItems = list.OrderBy(c => c.Name).Skip((criteria.Page - 1) * criteria.Size).Take(criteria.Size).ToList();
            //result.Items = pageItems.Select(x => MapperHelper.Map<AppSiteListDtoBase>(x));
            //result.TotalCount = list.Count();

            result.Items = pageItems.Select(appSite => MapperHelper.Map<AppSiteListDtoBase>(appSite)).ToList();
            result.TotalCount = list.Count();
            return result;
        }

        public ValueMessageWrapper<bool> Delete(IEnumerable<int> appSiteIds)
        {
            if (appSiteIds != null)
            {
                foreach (var item in appSiteIds.Select(appSiteId => appSiteRepository.Get(appSiteId)))
                {
                    ValidateAppSite(item);
                    if (item.IsValid)
                    {
                        item.IsDeleted = true;
                        appSiteRepository.Save(item);
                    }
                }
            }
            return ValueMessageWrapper.Create(true);
        }

        public SaveAppSiteDtoResult Save(AppSiteDto appSite)
        {
            // trim spaces from app/site url
            if (!string.IsNullOrWhiteSpace(appSite.URL))
            {
                appSite.URL = appSite.URL.Trim();
            }
            if (appSite.Theme != null)
            {
                if (!(appSite.Theme.Id > 0) && !appSite.Theme.IsCustom)
                {
                    appSite.Theme.IsCustom = true;

                }

            }
            var item = appSiteRepository.Get(appSite.ID);
            if (item != null)
            {
                item = getAppSite(appSite, item);
            }
            else
            {
                if (appSite.Type.IsApp)
                {
                    item = MapperHelper.Map<ArabyAds.AdFalcon.Domain.Model.AppSite.App>(appSite);
                }
                else
                {
                    item = MapperHelper.Map<Site>(appSite);
                }
                //ToDo : Move this to the DB
                item.RegistrationDate = Framework.Utilities.Environment.GetServerTime();
                item.AddDefaultAppSiteSetting();
                // Get AppSite Status
                setStatus(appSite, item);
            }

            //if (!item.LastAdminComment.Equals(appSite.AdminComment, StringComparison.OrdinalIgnoreCase))
            //{

            //    item.AdminComments = appSite.AdminComment;

            //}
            item.ChangePlacementType(appSite.RewardedVideoItemName, appSite.RewardedVideoItemValue, appSite.PlacementType);

            AppSiteType appSiteType = appSiteTypeRepository.Get(appSite.Type.Id);

            if (item.ID > 0 && item.Type.IsApp == !appSiteType.IsApp)//Alid: if appSiteType changed from app to site or vise versa.
            {
                ChangeAppsiteType(item, appSiteType);
            }

            item.ChangeType(item, appSiteType);

            if (string.IsNullOrWhiteSpace(item.PublisherId))
            {
                item.PublisherId = getPublisherId();
            }
            //Change Account 
            if (item.Account == null)
            {
                item.ChangeAccount(new Domain.Model.Account.Account() { ID = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            }

            if (item.User == null)
            {
                item.ChangeUser(new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });


            }
            //handle the keywords
            if (appSite.NewKeywords != null)
            {
                foreach (var newKeyword in appSite.NewKeywords)
                {
                    int id;
                    Keyword newKeywordObj = null;
                    if (Int32.TryParse(newKeyword, out id))
                    {
                        if (item.Keywords.FirstOrDefault(x => x.Keyword.ID == id) == null)
                        {
                            newKeywordObj = keyWordRepository.Get(Convert.ToInt32(newKeyword));
                            newKeywordObj.Usage++;
                            item.AddKeyword(newKeywordObj);
                        }
                    }

                }
            }
            if (appSite.DeletedKeywords != null)
            {
                foreach (var deletedKeyword in appSite.DeletedKeywords)
                {
                    var deletedKeywordObj = keyWordRepository.Get(Convert.ToInt32(deletedKeyword));
                    item.RemoveKeyword(deletedKeywordObj);
                }
            }
            if (item.Keywords != null)
            {
                IList<AppSiteKeyword> allkeyword = new List<AppSiteKeyword>();
                foreach (var keyword in  item.Keywords)
                {
                    allkeyword.Add(keyword);
                }
                foreach (var Keyword in allkeyword)
                {
                    if (appSite.intKeywords !=null )
                    {
                        if (appSite.intKeywords.FirstOrDefault(x => x == Keyword.Keyword.ID) == 0)
                        {
                            item.RemoveAppKeyword(Keyword);
                        }
                    }
                    else
                    {

                        item.RemoveAppKeyword(Keyword);
                    }
                }
            }


            if (appSite.intKeywords != null)
            {
                foreach (var newKeyword in appSite.intKeywords)
                {
                    int id;
                    Keyword newKeywordObj = null;
                    
                        if (item.Keywords.FirstOrDefault(x => x.Keyword.ID == newKeyword) == null)
                        {
                            newKeywordObj = keyWordRepository.Get(newKeyword);
                            newKeywordObj.Usage++;
                            item.AddKeyword(newKeywordObj);
                        }
                    

                }

            }
            
            ValidateAppSite(item);


            if (item.IsValid)
            {
                appSiteRepository.Save(item);
            }
            return new SaveAppSiteDtoResult() { Id = item.ID, PublisherId = item.PublisherId };
        }
        public List<AppSiteDto> GetAppSitesByAccountId(ValueMessageWrapper<int> accountId)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ValidateAccount(accountId.Value, UserId);
            List<AppSite> appsites = new List<AppSite>();

            if (IsPrimaryUser)
                appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId.Value && appsite.IsDeleted == false).ToList();
            else
                appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId.Value && appsite.User.ID == UserId && appsite.IsDeleted == false).ToList();
            return appsites.Select(p => MapperHelper.Map<AppSiteDto>(p)).ToList();
        }
        public AppSiteDto Get(ValueMessageWrapper<int> appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId.Value);

            ValidateAppSite(item);
            if (item.IsValid)
            {
                var appsite = MapperHelper.Map<AppSiteDto>(item);
                appsite.AdminComment = item.LastAdminComment;
                if (item.User != null && item.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value)
                {
                    appsite.SupUserName = string.Empty;
                }
                appsite.PlacementType = (int)item.AppSiteServerSetting.AppSitePlacementType;
                appsite.RewardedVideoItemName= item.AppSiteServerSetting.RewardedVideoItemName;
                appsite.RewardedVideoItemValue = item.AppSiteServerSetting.RewardedVideoItemValue;
                return appsite;

            }
            return null;
        }

        public AppSiteBasicDto GetAppSiteByPublisherId(string publisherId)
        {
            if (!string.IsNullOrEmpty(publisherId))
            {
                AppSite appsite = appSiteRepository.Query(p => p.PublisherId == publisherId.ToLower() && p.IsDeleted == false).SingleOrDefault();

                if (appsite != null)
                {
                    return MapperHelper.Map<AppSiteBasicDto>(appsite);
                }
            }

            return null;
        }

        public AppSiteDtoBase GetBasicInfo(ValueMessageWrapper<int> appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId.Value);
            ValidateAppSite(item);
            if (item.IsValid)
            {
                var result = MapperHelper.Map<AppSiteDtoBase>(item);
                result.StatusId = item.Status.ID;
                return result;
            }
            return null;
        }

        public SettingsDto GetSettings(ValueMessageWrapper<int> appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId.Value);
            ValidateAppSite(item);
            if (item.IsValid)
            {
                SettingsDto returnValue = null;
                returnValue = item.AppSiteSetting != null
                                  ? MapperHelper.Map<SettingsDto>(item.AppSiteSetting)
                                  : new SettingsDto();
                returnValue.AppSiteId = appSiteId.Value;
                returnValue.AppSiteName = item.Name;
                return returnValue;
            }
            else
            {
                return null;
            }
        }
        public void SaveSettings(SettingsDto settings)
        {
            //TODO:Osaleh to remove this temp code, this code added after hiding the test mode setting from the UI
            if (settings.TestingModeId < 1)
                settings.TestingModeId = 1;

            if (settings.RefreshModeId != 3)
            {
                settings.RefreshInterval = 0;
            }
            //Load the AppSite Object
            var appSite = appSiteRepository.Get(settings.AppSiteId);
            // Create App Setting Object Using the DTO
            var newSetting = MapperHelper.Map<AppSiteSetting>(settings);
            appSite.ChangeSetting(newSetting);

            ValidateAppSite(appSite);
            if (appSite.IsValid)
            {
                appSiteRepository.Save(appSite);
            }
        }

        public AppSiteAdminConfigDto GetServerSettings(ValueMessageWrapper<int> appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId.Value);
            ValidateAppSiteForAllRoles(item);
            if (item.IsValid)
            {
                var currentRevenueCalculationSetting = item.CurrentRevenueCalculationSettingDiscount();
                var returnValue = new AppSiteAdminConfigDto
                {
                    AppSiteServerSetting = MapperHelper.Map<AppSiteServerSettingDto>(item.AppSiteServerSetting),
                    CurrentRevenueCalculationSettings = currentRevenueCalculationSetting == null ? null : MapperHelper.Map<AppSiteRevenueCalculationSettingDto>(currentRevenueCalculationSetting),
                    AppSiteId = appSiteId.Value,
                    AppSiteName = item.Name,
                    DefaultAccountRevenue = item.Account.DefaultRevenuePercentage.HasValue ? new decimal?(Convert.ToDecimal(item.Account.DefaultRevenuePercentage.Value * 100)) : null
                };
                return returnValue;
            }
            else
            {
                return null;
            }
        }



        public void SaveServerSettings(AppSiteAdminConfigDto siteAdminConfig)
        {
            if (siteAdminConfig == null)
                throw new ArgumentNullException("siteAdminConfig");
            int newCostModelWrapperId = -1;
            //Load the AppSite Object
            var appSite = appSiteRepository.Get(siteAdminConfig.AppSiteId);
            var currentcostmodel = appSite.AppSiteServerSetting.GetPricingModel();
            decimal oldfactor = currentcostmodel != null ? currentcostmodel.Factor : 1;
            bool wasDefualtCostModel = appSite.AppSiteServerSetting.GetPricingModel() == null;
            if (siteAdminConfig.AppSiteServerSetting != null)
            {
                // Create app server Setting Object Using the DTO
                var newSetting = MapperHelper.Map<AppSiteServerSetting>(siteAdminConfig.AppSiteServerSetting);
                appSite.ChangeServerSetting(newSetting);
            }
            else
            {
                appSite.ResetServerSetting();
            }




            #region AppSite Events

            var currentAppSiteEvents = appSite.AppSiteServerSetting.EventsClone();


            decimal newfactor = 1;


            if (siteAdminConfig.AppSiteServerSetting.Events != null && siteAdminConfig.AppSiteServerSetting.Events.Count != 0)
            {
                var appsiteEventsDto = siteAdminConfig.AppSiteServerSetting.Events;
                var newCostModel = siteAdminConfig.AppSiteServerSetting.Events.Where(x => x.IsBillable).FirstOrDefault();

                var currentCostModel = currentAppSiteEvents.Where(x => x.IsBillable).FirstOrDefault(); ;
                if (currentCostModel != null && newCostModel != null && newCostModel.EventId != currentCostModel.Event.ID)
                {
                    newCostModelWrapperId = newCostModel.EventId;
                }
                else if (currentCostModel == null && newCostModel != null)
                {
                    newCostModelWrapperId = newCostModel.EventId;
                }

                foreach (var item in currentAppSiteEvents.Where(p => !p.IsDeleted && !appsiteEventsDto.Any(x => x.EventId == p.Event.ID)))
                {
                    item.IsDeleted = true;
                }

                foreach (var item in currentAppSiteEvents.Where(p => !p.IsDeleted && appsiteEventsDto.Any(x => x.EventId == p.Event.ID)))
                {
                    var currentEvent = appsiteEventsDto.Where(p => p.EventId == item.Event.ID).SingleOrDefault();
                    var factor = item.Event.CostModelWrapper.Factor;

                    item.MinBid = currentEvent.MinBid / factor;
                    item.IsBillable = currentEvent.IsBillable;
                }

                foreach (var item in appsiteEventsDto.Where(p => !currentAppSiteEvents.Any(x => x.Event.ID == p.EventId)))
                {
                    AppSiteEvent newAppSiteEvent = new AppSiteEvent();
                    var trackingEvent = trackingEventRepository.Get(item.EventId);
                    var factor = trackingEvent.CostModelWrapper.Factor;

                    newAppSiteEvent.MinBid = item.MinBid / factor;
                    newAppSiteEvent.IsBillable = item.IsBillable;
                    newAppSiteEvent.Event = trackingEvent;
                    newAppSiteEvent.AppSiteServerSetting = appSite.AppSiteServerSetting;
                    appSite.AppSiteServerSetting.Events.Add(newAppSiteEvent);
                }

                newfactor = currentAppSiteEvents.Count > 0 ? currentAppSiteEvents.Last().Event.CostModelWrapper.Factor : oldfactor;

                if (newCostModelWrapperId != -1)
                {
                    var response = CehckAppsitCostModelCompatableWitCampaigns( new CheckAppsitCostModelCompatableWitCampaignsRequest { AppSiteId = siteAdminConfig.AppSiteId, AppSiteCostModel = newCostModelWrapperId });

                    if (response.NotCompatableCampaigns != null && response.NotCompatableCampaigns.Count > 0)
                    {
                        var adgroups = response.NotCompatableCampaigns.Select(x => x.AdGroupId);

                        if (siteAdminConfig.ModifiedNotCompatableBidConfigs == null || siteAdminConfig.ModifiedNotCompatableBidConfigs.Count() <= 0)
                        {
                            throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                        }

                        foreach (CampaignBidConfigDto notCompatableBidConfig in response.NotCompatableCampaigns)
                        {
                            var notCompatableBidConfigModified = siteAdminConfig.ModifiedNotCompatableBidConfigs.Where(x => x.Appsite.ID == notCompatableBidConfig.Appsite.ID && x.AdGroupId == notCompatableBidConfig.AdGroupId && x.Bid > 0).FirstOrDefault();
                            if (notCompatableBidConfigModified == null || notCompatableBidConfigModified.Bid <= 0)
                            {
                                throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                            }
                            var group = adGroupRepository.Get(notCompatableBidConfigModified.AdGroupId);

                            if (notCompatableBidConfigModified.Bid > group.GetReadableBid())
                            {
                                throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                            }

                            if (!string.IsNullOrEmpty(notCompatableBidConfigModified.ID))
                            {
                                var campaignBidConfig = group.GetCampaignBidConfigs().Where(x => x.ID == Convert.ToInt32(notCompatableBidConfigModified.ID)).FirstOrDefault();
                                campaignBidConfig.Bid = notCompatableBidConfigModified.Bid;
                            }
                            else
                            {
                                var campaignBidConfig = new AdGroupBidConfig() { AdGroup = group, SubPublisherId = notCompatableBidConfigModified.SubPublisherId };
                                campaignBidConfig.AppSite = appSiteRepository.Get(notCompatableBidConfigModified.Appsite.ID);
                                campaignBidConfig.Account = accountRepository.Get(notCompatableBidConfigModified.AccountId);

                                campaignBidConfig.Bid = notCompatableBidConfigModified.Bid;

                                group.AddCampaignBidConfig(campaignBidConfig);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in appSite.AppSiteServerSetting.Events)
                {
                    item.IsDeleted = true;
                }
            }
            for (int i = 0; i < currentAppSiteEvents.Count(); i++)
            {
                if (currentAppSiteEvents[i].IsDeleted)
                {
                    appSite.AppSiteServerSetting.Events.Remove(currentAppSiteEvents[i]);
                }
            }

            #endregion

            if (siteAdminConfig.CurrentRevenueCalculationSettings != null)
            {
                // Create app server Setting Object Using the DTO
                var newRevenueCalculationSetting = MapperHelper.Map<AppSiteRevenueCalculationSetting>(siteAdminConfig.CurrentRevenueCalculationSettings);
                appSite.AddRevenueCalculationSetting(newRevenueCalculationSetting);
            }
            else
            {
                appSite.RemoveRevenueCalculationSetting();
            }

      

            ValidateAppSite(appSite);

            // if the new cost model has the same factor as the previous one or the cost model hasnt even changed, then no need for recalculation
            if (appSite.AppSiteServerSetting.GetPricingModel() != currentcostmodel)
                ChangeBidConfigsAssignment(appSite.ID, wasDefualtCostModel, oldfactor, siteAdminConfig.ModifiedNotCompatableBidConfigs);

            if (appSite.IsValid)
            {
                appSiteRepository.Save(appSite);
            }

        }
        public void ChangeBidConfigsAssignment(int appSiteId, bool wasDefualtCostModel, decimal oldfactor, IList<CampaignBidConfigDto> ModifiedNotCompatableBidConfigs)
        {
            var appsite = appSiteRepository.Get(appSiteId);

            //Get All BidConfigs set in targeting campiangbidConfig tap
            var appSitesBidConfigs = campaignBidConfigRepository.Query(x => x.AppSite != null && x.AppSite.ID == appSiteId && !x.IsDeleted).ToList();

            foreach (AdGroupBidConfig adGroupBidConfig in appSitesBidConfigs)
            {

                var bidconfig = ModifiedNotCompatableBidConfigs.Where(x => x.Appsite.ID == adGroupBidConfig.AppSite.ID && x.AdGroupId == adGroupBidConfig.AdGroup.ID && x.Bid > 0).FirstOrDefault();

                if (bidconfig == null)
                {
                    //get it as user readalbe value before set it 
                    adGroupBidConfig.Bid *= wasDefualtCostModel ? adGroupBidConfig.AdGroup.CostModelWrapper.Factor : oldfactor;
                }
                else
                {
                    adGroupBidConfig.Bid = bidconfig.Bid; //we already got it's user readable value in the Dialog 
                }

                adGroupBidConfig.SetAdGroupBidConfigsBid(adGroupBidConfig.Bid);

            }

        }
        public void Approval(AppSiteApprovalDto appSiteApprovalDto)
        {

            var newStatus = appSiteStatusRepository.Get(appSiteApprovalDto.StatusId);
            if (newStatus == null)
            {
                throw new ArgumentException("Not Valid Status", "statusId");
            }
            var item = appSiteRepository.Get(appSiteApprovalDto.AppSiteId);

            //handle the keywords
            if (appSiteApprovalDto.NewKeywords != null)
            {
                foreach (var newKeyword in appSiteApprovalDto.NewKeywords)
                {
                    int id;
                    Keyword newKeywordObj = null;
                    if (Int32.TryParse(newKeyword, out id))
                    {
                        if (item.Keywords.FirstOrDefault(x => x.Keyword.ID == id) == null)
                        {
                            newKeywordObj = keyWordRepository.Get(Convert.ToInt32(newKeyword));
                            newKeywordObj.Usage++;
                            item.AddKeyword(newKeywordObj);
                        }
                    }

                }
            }
            if (appSiteApprovalDto.DeletedKeywords != null)
            {
                foreach (var deletedKeyword in appSiteApprovalDto.DeletedKeywords)
                {
                    var deletedKeywordObj = keyWordRepository.Get(Convert.ToInt32(deletedKeyword));
                    item.RemoveKeyword(deletedKeywordObj);
                }
            }
            if (item.Keywords != null)
            {
                IList<AppSiteKeyword> allkeyword = new List<AppSiteKeyword>();
                foreach (var keyword in item.Keywords)
                {
                    allkeyword.Add(keyword);
                }

                foreach (var Keyword in allkeyword)
                {
                    if (appSiteApprovalDto.intKeywords != null)
                    {
                        if (appSiteApprovalDto.intKeywords.FirstOrDefault(x => x == Keyword.Keyword.ID) == 0)
                        {
                            item.RemoveAppKeyword(Keyword);
                        }
                    }
                    else
                    {
                        item.RemoveAppKeyword(Keyword);

                    }
                }
            }


            if (appSiteApprovalDto.intKeywords != null)
            {
                foreach (var newKeyword in appSiteApprovalDto.intKeywords)
                {
                    int id;
                    Keyword newKeywordObj = null;

                    if (item.Keywords.FirstOrDefault(x => x.Keyword.ID == newKeyword) == null)
                    {
                        newKeywordObj = keyWordRepository.Get(newKeyword);
                        newKeywordObj.Usage++;
                        item.AddKeyword(newKeywordObj);
                    }


                }

            }
            ValidateAppSite(item);
            if (item.IsValid)
            {
                if (item.Status.ID == appSiteApprovalDto.StatusId)
                {
                    throw new AppAlreadyInThisStatus();
                }
                AppSiteType appSiteType = appSiteTypeRepository.Get(appSiteApprovalDto.Type.Id);// MapperHelper.Map<AppSiteType>(appSiteApprovalDto.Type);

                if (item.Type.IsApp == !appSiteType.IsApp)//Alid: if appSiteType changed from app to site or vise versa.
                {
                    ChangeAppsiteType(item, appSiteType);
                }

                item.ChangeType(item, appSiteType);

                item.Approve(newStatus, appSiteApprovalDto.Comments);
            }


            appSiteRepository.Save(item);

        }

        /// <summary>
        /// Alid
        /// Update AppSite.IsApp , Because "IsApp" is DiscriminateSubClassesOnColumn ,
        /// And flehntnhibernate dose not update the Discriminator column on traditional( Save or Update) statment
        /// </summary>
        /// <param name="appsite"></param>
        /// <param name="newAppsiteType"></param>
        private void ChangeAppsiteType(AppSite appsite, AppSiteType newAppsiteType)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateQuery("UPDATE AppSite SET IsApp=:IsApp WHERE Id=:AppSiteId");
            query.SetParameter("AppSiteId", appsite.ID);
            query.SetParameter("IsApp", newAppsiteType.IsApp);
            query.ExecuteUpdate();
        }

        public List<Interfaces.DTOs.Core.TreeDto> GetAppSitesTreeByAccountId(ValueMessageWrapper<int> accountId)
        {
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ValidateAccount(accountId.Value, UserId);

            List<AppSite> appsites = null;
            if (IsPrimaryUser)
                appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId.Value && appsite.IsDeleted == false).ToList();
            else
                appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId.Value && appsite.User.ID == UserId && appsite.IsDeleted == false).ToList();
            var returnList = new List<TreeDto>();
            foreach (var item in appsites)
            {
                var treeDto = new TreeDto
                {
                    Id = item.ID.ToString(),
                    Childs = new List<TreeDto>(),
                    Name = LocalizedStringDto.ConvertToLocalizedStringDto(item.Name)
                };
                returnList.Add(treeDto);
            }

            return returnList;

        }

        public BasicAppSiteInformation GetPrimaryUserBasicInformation(ValueMessageWrapper<int> appsiteId)
        {
            var appsite = appSiteRepository.Get(appsiteId.Value);

            if (appsite == null || appsite.IsDeleted)
            {
                return null;
            }

            BasicAppSiteInformation basicAppSiteInformation = MapperHelper.Map<BasicAppSiteInformation>(appsite);

            return basicAppSiteInformation;
        }

        public IEnumerable<AppSiteEventDto> GetAppSiteEvents(int appsiteId)
        {
            var appsite = appSiteRepository.Get(appsiteId);

            ValidateAppSite(appsite);

            var appsiteEvents = appsite.AppSiteServerSetting.Events;

            IEnumerable<AppSiteEventDto> appsiteEventsDto = appsiteEvents.Select(p => MapperHelper.Map<AppSiteEventDto>(p)).ToList();

            return appsiteEventsDto;
        }


        public void DeleteAppSiteEvent(int appsiteId, int appsiteEventId)
        {
            var appsite = appSiteRepository.Get(appsiteId);

            ValidateAppSite(appsite);

            var appsiteEvent = appsite.AppSiteServerSetting.Events.Where(p => p.ID == appsiteEventId).SingleOrDefault();

            if (appsiteEvent != null)
            {
                appsite.AppSiteServerSetting.Events.Remove(appsiteEvent);

                appSiteRepository.Save(appsite);
            }

        }


        public void AddAppSiteEvent(int appsiteId, AppSiteEventSaveDto saveDto)
        {
            var appsite = appSiteRepository.Get(appsiteId);

            ValidateAppSite(appsite);

            var appsiteEvent = MapperHelper.Map<AppSiteEvent>(saveDto);
            appsiteEvent.AppSiteServerSetting = appsite.AppSiteServerSetting;

            appsiteEvent.Event = trackingEventRepository.Get(saveDto.EventId);
            appsite.AppSiteServerSetting.Events.Add(appsiteEvent);

            appSiteRepository.Save(appsite);
        }

        #endregion

        #region TextFilters

        public List<TextFilterDto> GetAppSiteTextFilters(ValueMessageWrapper<int> appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId.Value);

            ValidateAppSite(appSite);

            if (appSite.IsValid)
            {
                List<TextFilter> list =
                    UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == appSiteId.Value && p.IsDeleted == false).ToList();


                return list.Select(p => MapperHelper.Map<TextFilterDto>(p)).ToList();

            }
            else
            {
                return null;
            }
        }

        public void DeleteAppsiteFilter(ValueMessageWrapper<int> filterId)
        {
            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            int UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ValidateAccount(accountId, UserId);

            AppSiteFilter filter =
                UnitOfWork.Current.EntitySet<AppSiteFilter>().Where(p => p.ID == filterId.Value).
                    ToList().SingleOrDefault();

            if (filter != null)
            {
                filter.IsDeleted = true;
                UnitOfWork.Current.Save(filter);
            }

        }

        public ValueMessageWrapper<bool> UpdateAppSiteTextFilter(AppSiteTextFilterMessage request)
        {
            AppSite appSite = appSiteRepository.Get(request.AppSiteId);

            ValidateAppSite(appSite);

            bool result = false;

            if (appSite.IsValid)
            {

                TextFilter filter = appSite.AppSiteFilters.Where(p => p.ID == request.TextFilterDto.TextFilterId).SingleOrDefault() as TextFilter;

                List<TextFilter> textFilters = UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.MatchType.ID == request.TextFilterDto.MatchTypeId && p.Text == request.TextFilterDto.Text && p.IsDeleted == false).ToList();

                if (filter != null && (textFilters.Count == 0 || (textFilters.Count == 1 && textFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.Text = request.TextFilterDto.Text;
                    filter.MatchType = new MatchType();
                    filter.MatchType.ID = request.TextFilterDto.MatchTypeId;

                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return ValueMessageWrapper.Create(result);
        }

        public ValueMessageWrapper<bool> AddAppSiteTextFilter(AppSiteTextFilterMessage request)
        {
            AppSite appSite = appSiteRepository.Get(request.AppSiteId);

            ValidateAppSite(appSite);

            bool result = false;

            if (appSite.IsValid)
            {
                int duplicate = UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.MatchType.ID == request.TextFilterDto.MatchTypeId && p.Text == request.TextFilterDto.Text && !p.IsDeleted).Count();

                if (duplicate == 0)
                {
                    TextFilter filter = MapperHelper.Map<TextFilter>(request.TextFilterDto);
                    filter.AppSite = appSite;
                    filter.CIsDeleted = false;
                    filter.CAppSiteId = appSite.ID;
                    appSite.AppSiteFilters.Add(filter);
                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return ValueMessageWrapper.Create(result);
        }

        #endregion

        #region UrlFilters

        List<UrlFilterDto> IAppSiteService.GetAppSiteUrlFilters(ValueMessageWrapper<int> appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId.Value);

            ValidateAppSite(appSite);

            if (appSite.IsValid)
            {
                List<UrlFilter> list =
                  UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == appSiteId.Value && p.IsDeleted == false).ToList();


                return list.Select(p => MapperHelper.Map<UrlFilterDto>(p)).ToList();
            }
            else
            {
                return null;
            }
        }

        public ValueMessageWrapper<bool> UpdateAppSiteUrlFilter(AppSiteUrlFilterMessage request)
        {
            AppSite appSite = appSiteRepository.Get(request.AppSiteId);

            ValidateAppSite(appSite);

            bool result = false;

            if (appSite.IsValid)
            {
                UrlFilter filter = appSite.AppSiteFilters.Where(p => p.ID == request.UrlFilterDto.UrlFilterId).SingleOrDefault() as UrlFilter;

                List<UrlFilter> urlFilters = UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.URL == request.UrlFilterDto.Url && p.IsDeleted == false).ToList();

                if (filter != null && (urlFilters.Count == 0 || (urlFilters.Count == 1 && urlFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.URL = request.UrlFilterDto.Url.Trim();
                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return ValueMessageWrapper.Create(result);

        }

        public ValueMessageWrapper<bool> AddAppSiteUrlFilter(AppSiteUrlFilterMessage request)
        {
            AppSite appSite = appSiteRepository.Get(request.AppSiteId);

            ValidateAppSite(appSite);

            bool result = false;

            if (appSite.IsValid)
            {
                int duplicate = UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.URL == request.UrlFilterDto.Url && !p.IsDeleted).Count();

                if (duplicate == 0)
                {
                    var filter = MapperHelper.Map<UrlFilter>(request.UrlFilterDto);
                    filter.AppSite = appSite;
                    filter.CAppSiteId = appSite.ID;
                    filter.CIsDeleted = false;
                    filter.URL = filter.URL.Trim();

                    appSite.AppSiteFilters.Add(filter);
                    appSiteRepository.Save(appSite);
                    result = true;
                }
            }

            return ValueMessageWrapper.Create(result);
        }

        #endregion

        #region LanguageFilters

        public List<LanguageFilterDto> GetAppSiteLanguageFilters(ValueMessageWrapper<int> appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId.Value);

            ValidateAppSite(appSite);

            if (appSite.IsValid)
            {

                List<LanguageFilter> list =
                    UnitOfWork.Current.EntitySet<LanguageFilter>().Where(
                        p => p.AppSite.ID == appSiteId.Value && p.IsDeleted == false).ToList();
                return list.Select(p => MapperHelper.Map<LanguageFilterDto>(p)).ToList();
            }
            else
            {
                return null;
            }
        }

        public ValueMessageWrapper<bool> AddAppSiteLanguageFilter(AppSiteLanguageFilterMessage request)
        {
            var appSite = appSiteRepository.Get(request.AppSiteId);
            ValidateAppSite(appSite);

            bool result = false;
            if (appSite.IsValid)
            {
                var duplicate = UnitOfWork.Current.EntitySet<LanguageFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.Language.ID == request.LanguageFilterDto.LanguageId && p.IsDeleted == false).Count();
                if (duplicate == 0)
                {
                    var filter = MapperHelper.Map<LanguageFilter>(request.LanguageFilterDto);
                    filter.AppSite = appSite;
                    appSite.AppSiteFilters.Add(filter);
                    filter.CIsDeleted = false;
                    filter.CAppSiteId = appSite.ID;

                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }
            return ValueMessageWrapper.Create(result);
        }

        public ValueMessageWrapper<bool> UpdateAppSiteLanguageFilter(AppSiteLanguageFilterMessage request)
        {
            var appSite = appSiteRepository.Get(request.AppSiteId);

            ValidateAppSite(appSite);

            bool result = false;

            if (appSite.IsValid)
            {
                LanguageFilter filter = appSite.AppSiteFilters.Where(p => p.ID == request.LanguageFilterDto.languageFilterId).SingleOrDefault() as LanguageFilter;

                List<LanguageFilter> languageFilters = UnitOfWork.Current.EntitySet<LanguageFilter>().Where(p => p.AppSite.ID == request.AppSiteId && p.Language.ID == request.LanguageFilterDto.LanguageId && p.IsDeleted == false).ToList();

                if (filter != null && (languageFilters.Count == 0 || (languageFilters.Count == 1 && languageFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.Language = new Language();
                    filter.Language.ID = request.LanguageFilterDto.LanguageId;

                    appSiteRepository.Save(appSite);

                    result = true;
                }

            }

            return ValueMessageWrapper.Create(result);
        }

        #endregion

        #region Private Members

        private void ValidateAppSite(AppSite appSite)
        {
            bool isManager = IsAppSiteManager();

            appSite.Validate(!isManager);
        }

        private void ValidateAppSiteForAllRoles(AppSite appSite)
        {
            bool isManager = OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager") || OperationContext.Current.CurrentPrincipal.IsInRole("AppOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator");

            appSite.Validate(!isManager);
        }

        private void ValidateAccount(int? accountId, int? userId)
        {
            bool isManager = IsAppSiteManager();

            if (!isManager)
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != accountId)
                {
                    throw new AccountNotValidException();
                }


                if (userId.HasValue && !Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().IsPrimaryUser)
                {
                    if ((userId != Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().UserId))
                    {
                        throw new AccountNotValidException();
                    }
                }
            }
        }

        private bool IsAppSiteManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AppOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator");
        }

        private AppSite setStatus(AppSiteDto appSiteDto, AppSite item)
        {
            if (appSiteDto.Type.IsApp)
            {
                var itemApp = (ArabyAds.AdFalcon.Domain.Model.AppSite.App)item;
                if ((itemApp.IsPublished) && (!string.IsNullOrWhiteSpace(itemApp.MarketURL)))
                {
                    itemApp.Status = AppSiteStatus.Submitted;
                }
                else
                {
                    itemApp.Status = AppSiteStatus.Incomplete;
                }
            }
            else
            {
                var itemSite = (Site)item;
                if ((itemSite.IsPublished) && (!string.IsNullOrWhiteSpace(itemSite.SiteURL)))
                {
                    /*if ((string.IsNullOrWhiteSpace(itemSite.SiteURL)) || (!itemSite.SiteURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
                        itemSite.Status = AppSiteStatus.Submitted;*/
                    itemSite.Status = AppSiteStatus.Submitted;
                }
                else
                {
                    itemSite.Status = AppSiteStatus.Incomplete;
                }
            }
            return item;
        }

        public List<SubAppsiteDto> GetSubAppsites(GetSubAppsitesRequest request)
        {
            AppSite appSite = appSiteRepository.Get(request.AppSiteId);
            appSite.Validate(false);

            //ValidateAppSiteForAllRoles(appSite);

            if (appSite.IsValid)
            {

                List<SubAppsite> list =
                    UnitOfWork.Current.EntitySet<SubAppsite>().Where(
                        p => p.AppSite.ID == request.AppSiteId && (string.IsNullOrEmpty(request.SubPublisherId) || p.SubPublisherId == request.SubPublisherId)).ToList();
                return list.Select(p => MapperHelper.Map<SubAppsiteDto>(p)).ToList();
            }
            else
            {
                return new List<SubAppsiteDto>();
            }
        }
        public SubAppSiteListResultDto GetAppsAndSubAppsites(GetSubAppsitesRequest request)
        {
            SubAppSiteListResultDto totalResult = new SubAppSiteListResultDto();
            string script = " SELECT(SELECT users.LastName   FROM users   inner join  adfalcon.account on   users.Id=account.PrimaryUserId where account.Id= app.AccountId) as AccountName, app.AccountId as AccountId   ,app.ID as AppSiteId  , subapp.SubPublisherId as SubPublisherId , subapp.Id as Id ,subapp.SubPublisherName as SubPublisherName,app.Name as AppSiteName FROM adfalcon.appsite app left outer join adfalcon.sub_appsites subapp on app.Id = AppsiteId";
            if (!string.IsNullOrEmpty(request.AppSiteName) || !string.IsNullOrEmpty(request.SubPublisherId) || request.AccountId > 0)
                script = script + " where  1=1  ";
                if (request.AccountId > 0)
                script = script + " and  AccountId=" + request.AccountId;


            if (!string.IsNullOrEmpty(request.AppSiteName))
                script = script + " and  app.Name like '" + request.AppSiteName+"%'";

            if (!string.IsNullOrEmpty(request.SubPublisherId))
                script = script + " and  SubPublisherId like '" + request.SubPublisherId + "%'"; 

            string CountQuery = string.Format("SET SQL_BIG_SELECTS=1;  SELECT Count(*) From ({0}) AS CountQueryResultSet", script);
            string PaginationQuery = string.Format("SET SQL_BIG_SELECTS=1;  SELECT * From ({0}) AS PaginationResultSet LIMIT {1}  OFFSET  {2}", script, 100, (request.Page - 1) * 100);

            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Execute-{1}", "GetResultofDataQBDtoWithScoping", PaginationQuery);

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery queryCount = null;

            ISQLQuery queryResult = null;
           // queryCount =nhibernateSession.CreateSQLQuery(CountQuery);

            queryResult =nhibernateSession.CreateSQLQuery(PaginationQuery);

            var TotalCount = 0;

            queryResult.SetResultTransformer(Transformers.AliasToBean<SubAppsiteDto>());
            var results = queryResult.List<SubAppsiteDto>().ToList() ?? new List<SubAppsiteDto>();
            totalResult.Items = results;

           totalResult.TotalCount = TotalCount;
            
            return totalResult;
        }



        public SubAppSiteListResultDto QueryByCratiriaForSubAppsites(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria wcriteria)
        {


            AllAppSiteCriteria criteria = new AllAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            AppSite appSite = appSiteRepository.Get(criteria.AppSiteId);
            SubAppSiteListResultDto result = new SubAppSiteListResultDto();

            ValidateAppSite(appSite);

            if (appSite.IsValid)
            {
                var results = appSiteRepository.QueryByCratiriaForSubAppSite(criteria);

                result.Items = results.Select(p => MapperHelper.Map<SubAppsiteDto>(p)).ToList();
                //criteria.Page = -2;
                result.TotalCount = appSiteRepository.QueryByCratiriaForSubAppSiteCount(criteria);
            }
            else
            {
                result.Items = new List<SubAppsiteDto>();
                result.TotalCount = 0;
            }
            return result;
        }

        public SubAppSiteListResultDto SearchSubAppsites(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria wcriteria)
        {
            AllAppSiteCriteria criteria = new AllAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            SubAppSiteListResultDto result = new SubAppSiteListResultDto();
            int count = 0;
            result.Items = new List<SubAppsiteDto>();
            List<BusinessPartner> searcbusinessParners = null;
            if (!(criteria.AccountIds != null && criteria.AccountIds.Length > 0))
            {
                return result;

            }
            if (criteria.QuickSearchField != null)
            {
                criteria.QuickSearchField = criteria.QuickSearchField.Trim();

            }

            if (criteria.QuickSearchExchangeNameField != null)
            {
                criteria.QuickSearchExchangeNameField = criteria.QuickSearchExchangeNameField.Trim();

            }
            List<BusinessPartner> businessParners = _BusinessRepository.GetBusinessPartnerByAccountIds(criteria.AccountIds).ToList();
            if (criteria.QuickSearchExchangeNameField != null && criteria.QuickSearchExchangeNameField.Trim() != string.Empty)
            {

                searcbusinessParners = businessParners.Where(M => M.Name.ToLower().Contains(criteria.QuickSearchExchangeNameField.ToLower())).ToList();

            }
            else
            {
                searcbusinessParners = businessParners;

            }
            businessParners = searcbusinessParners;
            //if (searcbusinessParners!=null && searcbusinessParners.Count > 0)
            //{
            //    businessParners = searcbusinessParners;
            //}
            if (!(businessParners != null && businessParners.Count > 0))
            {
                return result;
            }
            else
            {
                criteria.AccountIds = businessParners.Select(M => M.Account.ID).Distinct().ToList().ToArray();


            }
            var results = _subAppsiteRepository.GetSubAppSitesQuery(criteria, out count);

            //result.Items = results.Select(p => MapperHelper.Map<SubAppsiteDto>(p)).ToList();

            if (results != null)
            {
                BusinessPartner busReps = null;

                foreach (var ent in results)
                {

                    if (criteria.IsForSSP == true)
                    {
                        busReps = businessParners.Where(M => M.Account.ID == ent.AccountId && M is SSPPartner).FirstOrDefault();
                    }
                    else
                        busReps = businessParners.Where(M => M.Account.ID == ent.AccountId).FirstOrDefault();


                    ent.ExchangeName = busReps.Name;
                    ent.ExchangeId = busReps.ID;
                    result.Items.Add(new SubAppsiteDto { Id = ent.Id, SubPublisherMarketId = ent.SubPublisherMarketId, SubPublisherId = ent.SubPublisherId, SubPublisherName = ent.SubPublisherName, SubPublisherUrl = ent.SubPublisherUrl, AppSiteId = ent.AppSiteId, AppSiteName = ent.AppSiteName, ExchangeId = ent.ExchangeId, ExchangeName = ent.ExchangeName });

                }

            }

            result.TotalCount = count;

            return result;
        }
        private bool AppSiteChanged(AppSiteDto appSiteDto)
        {
            var appsite = Get(ValueMessageWrapper.Create(appSiteDto.ID));
            var kwwords = appsite.Keywords.Select(x => x.ID.ToString()).ToArray();

            if (appSiteDto.NewKeywords != null)
            {
                var newAddedKeywords = appSiteDto.NewKeywords.Except(kwwords).ToArray();

                if (newAddedKeywords.Length > 0 || appSiteDto.DeletedKeywords != null)
                {
                    return true;
                }
            }

            if (appSiteDto.DeletedKeywords != null)
            {
                return true;
            }
            if (appSiteDto.Type.Id != appsite.Type.Id)
            {
                return true;
            }

            return false;
        }


        private AppSite getAppSite(AppSiteDto appSiteDto, AppSite item)
        {

            if ((appSiteDto.IsPublished) && (!string.IsNullOrWhiteSpace(appSiteDto.URL)))
            {
                // active or Submitted
                if (item.Type.IsApp)
                {
                    var itemApp = (ArabyAds.AdFalcon.Domain.Model.AppSite.App)item;
                    // Get Status
                    if ((appSiteDto.IsPublished != itemApp.IsPublished) || (!itemApp.MarketURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
                    {
                        //  URL changed then set it to submitted
                        itemApp.Status = AppSiteStatus.Submitted;
                    }
                }
                else
                {
                    var itemSite = (Site)item;
                    // Get Status
                    if ((appSiteDto.IsPublished != itemSite.IsPublished) || itemSite.SiteURL == null || (!itemSite.SiteURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
                    {
                        //  URL changed then set it to submitted
                        itemSite.Status = AppSiteStatus.Submitted;
                    }
                }
            }
            else
            {
                if (item.Type.IsApp)
                {
                    var itemApp = (ArabyAds.AdFalcon.Domain.Model.AppSite.App)item;
                    if (appSiteDto.URL != itemApp.MarketURL || appSiteDto.IsPublished != item.IsPublished)
                        //Incomplete
                        item.Status = AppSiteStatus.Incomplete;

                }
                else
                {
                    var itemSite = (Site)item;

                    if (appSiteDto.URL != itemSite.SiteURL || appSiteDto.IsPublished != item.IsPublished)
                        //Incomplete
                        item.Status = AppSiteStatus.Incomplete;

                }

            }

            if (item.Type.IsApp)
            {
                var itemApp = (ArabyAds.AdFalcon.Domain.Model.AppSite.App)item;
                itemApp.MarketURL = appSiteDto.URL;
                itemApp.SubType = appSiteDto.SubType;
            }
            else
            {
                var itemSite = (Site)item;
                itemSite.SiteURL = appSiteDto.URL;
            }
            item.Name = appSiteDto.Name;
            item.IsPublished = appSiteDto.IsPublished;
            item.Theme = MappingRegister.MapAppSiteTheme(appSiteDto);
            return item;
        }

        private string getPublisherId()
        {
            // return RandomNumbersMethod(10);
            return Guid.NewGuid().ToString("N");
        }

        #endregion

        public CheckAppsitCostModelCompatableWitCampaignsResponse CehckAppsitCostModelCompatableWitCampaigns(CheckAppsitCostModelCompatableWitCampaignsRequest request)
        {
            var appsite = appSiteRepository.Get(request.AppSiteId);
            var currentCostModel = appsite.AppSiteServerSetting.GetPricingModel();

            List<int> campaignsIds = GetCampaignAllAssignedAppsites(request.AppSiteId);// get all Campaigns using this appsite
            // List<int> groupIds = GetAllCampaignGroupsUsingApp(appSiteId);
            var response = new CheckAppsitCostModelCompatableWitCampaignsResponse { NotCompatableCampaigns = new List<CampaignBidConfigDto>() };
            CampaignBidConfigDto campaignBidConfigDto = null;
            List<AdGroup> allAdGroup = GetAllCampaignGroupsUsingApp(request.AppSiteId);
            foreach (int id in campaignsIds)
            {
                var item = campaignRepository.Get(id);
                foreach (AdGroup adGroup in item.AdGroups.Where(x => x.CreationDate >= ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(-90)))
                {
                    allAdGroup.Add(adGroup);
                }
            }

            foreach (AdGroup adGroup in allAdGroup.Distinct())
            {
                if (adGroup.IsDeleted || adGroup.Status.ID == AdGroupStatus.Completed.ID)
                {
                    continue;
                }
                if (!isValidPricingModel(adGroup, request.AppSiteCostModel))
                {
                    var adGroupBidConfigs = adGroup.GetCampaignBidConfigs().Where(x => x.AppSite.ID == appsite.ID).ToList();
                    if (adGroupBidConfigs.Count > 0)
                    {
                        foreach (var adGroupBidConfig in adGroupBidConfigs)
                        {


                            campaignBidConfigDto = MapperHelper.Map<CampaignBidConfigDto>(adGroupBidConfig);
                            campaignBidConfigDto.Bid = adGroupBidConfig.GetUserReadableValue();
                            //   campaignBidConfigDto.Bid *= currentCostModel != null ? currentCostModel.Factor : adGroup.CostModelWrapper.Factor;

                            response.NotCompatableCampaigns.Add(campaignBidConfigDto);
                        }
                    }
                    else
                    {
                        campaignBidConfigDto = new CampaignBidConfigDto()
                        {
                            Appsite = MapperHelper.Map<AppSiteBasicDto>(appsite),
                            AccountId = appsite.Account.ID,
                            AccountName = appsite.Account.GetAccountName(),
                            AdGroupId = adGroup.ID,
                            AdGrouptName = adGroup.Name,
                            MinBid = adGroup.GetReadableBid(),
                            CampaingName = adGroup.Campaign.Name,
                            AdGroupPricingModel = adGroup.CostModelWrapper.CostModel.GetDescription()
                        };

                        response.NotCompatableCampaigns.Add(campaignBidConfigDto);
                    }
                }
            }

            // Get All BidConfigs set in targeting campiangbidConfig tap
            //var appSitesBidConfigs = campaignBidConfigRepository.GetAll().Where(x => x.AppSite != null && x.AppSite.ID == appSiteId && !x.IsDeleted).ToList();
            //foreach (CampaignBidConfig campaignBidConfig in appSitesBidConfigs)
            //{
            //    if (!isValidPricingModel(campaignBidConfig.AdGroup, appSiteCostModel))
            //    {
            //        if (notCompatibleAppSiteList.Where(x => Convert.ToInt32(x.ID) == campaignBidConfig.ID).Count() <= 0)
            //        {
            //            notCompatibleAppSiteList.Add(MapperHelper.Map<CampaignBidConfigDto>(campaignBidConfig));
            //        }
            //    }
            //}
            if (response.NotCompatableCampaigns.Count == 0)
                 response.Success = true;
            return response;
        }

        private bool isValidPricingModel(AdGroup adGroup, int appsitePricingModelId)
        {
            if (adGroup.CostModelWrapper != null && appsitePricingModelId != adGroup.CostModelWrapper.Event.ID)
            {
                return false;
            }
            return true;

        }

        public List<AdGroup> GetAllCampaignGroupsUsingApp(int appSiteId)
        {
            AdGroupCriteria criteria = new AdGroupCriteria() { AppSiteId = appSiteId, DateFrom = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(-90) };

            //     List<Domain.Model.Campaign.Campaign> campaigns = campaignRepository.Query(criteria.GetExpression()).ToList();
            var Permissions = OperationContext.Current.UserInfo<AdFalconUserInfo>().Permissions;
            if (!Domain.Configuration.IsAdmin)
                criteria.Permissions = Permissions != null ? Permissions.ToList() : new List<int>();

            List<Domain.Model.Campaign.AdGroup> adGroups = adGroupRepository.Query(criteria.GetExpression()).ToList();

            return adGroups;
            //  QueryGroupsByCratiria(criteria).
        }

        public List<int> GetCampaignAllAssignedAppsites(int appsiteId)
        {
            int[] campaingIds = null;
            List<int> allAssignedAppsites = new List<int>();
            //IList<int> GetCampaignIdsByAppSiteId(int AppSiteId)
            var results = campaignAssignedAppsiteRepository.GetCampaignIdsByAppSiteId(appsiteId);



            if (results != null && results.Count() > 0)
            {
                var resultsCamp = results;
                resultsCamp = resultsCamp.Distinct().ToList();
                campaingIds = resultsCamp.ToArray();

            }
            if (campaingIds != null)
                allAssignedAppsites.AddRange(campaingIds);
            return allAssignedAppsites;
        }


    }
}
