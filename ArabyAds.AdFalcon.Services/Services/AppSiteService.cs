using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Noqoush.AdFalcon.Domain.Model.AppSite.Filtering;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Services;
using Noqoush.AdFalcon.Persistence.Reports.Repositories;
using Noqoush.AdFalcon.Persistence.Repositories.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework.DomainServices.Localization.Repositories;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.Framework;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
namespace Noqoush.AdFalcon.Services
{
    public class AppSiteService : IAppSiteService
    {
        private IAppSiteRepository appSiteRepository = null;
        private IKeyWordRepository keyWordRepository = null;
        private IReportRepository reportRepository = null;
        private ISummaryRepository summaryRepository = null;
        public AppSiteService(IAppSiteRepository appSiteRepository,
            IKeyWordRepository keyWordRepository,
            IReportRepository reportRepository, ISummaryRepository summaryRepository)
        {
            this.appSiteRepository = appSiteRepository;
            this.keyWordRepository = keyWordRepository;
            this.reportRepository = reportRepository;
            this.summaryRepository = summaryRepository;
        }

        #region AppSite
        public AppSiteListResultDto QueryByCratiria(AppSiteCriteriaBase criteria)
        {
            var result = new AppSiteListResultDto();
            IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression(), criteria.Page - 1, criteria.Size, item => item.ID, true);
            result.Items = list.Select(appSite => MapperHelper.Map<AppSiteListDto>(appSite)).ToList();
            var performance = new PerformanceCriteriaBase { Ids = result.Items.Select(obj => obj.Id).ToList() };
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
        public AppSiteListResultDtoBase GetAllActive(AllAppSiteCriteria criteria)
        {
            var result = new AppSiteListResultDtoBase();
            IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression(), criteria.Page - 1, criteria.Size, item => item.ID, true);
            result.Items = list.Select(appSite => MapperHelper.Map<AppSiteListDtoBase>(appSite)).ToList();
            result.TotalCount = appSiteRepository.Query(criteria.GetExpression()).Count();
            return result;
        }

        public bool Delete(IEnumerable<int> appSiteIds)
        {
            if (appSiteIds != null)
            {
                foreach (var item in appSiteIds.Select(appSiteId => appSiteRepository.Get(appSiteId)))
                {
                    item.Validate(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
                    if (item.IsValid)
                    {
                        item.IsDeleted = true;
                        appSiteRepository.Save(item);
                    }
                }
            }
            return true;
        }
        private AppSite setStatus(AppSiteDto appSiteDto, AppSite item)
        {
            if (appSiteDto.Type.IsApp)
            {
                var itemApp = (App)item;
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
        private AppSite getAppSite(AppSiteDto appSiteDto, AppSite item)
        {

            if ((appSiteDto.IsPublished) && (!string.IsNullOrWhiteSpace(appSiteDto.URL)))
            {
                // active or Submitted
                if (item.Type.IsApp)
                {
                    var itemApp = (App)item;
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
                    if ((appSiteDto.IsPublished != itemSite.IsPublished) || (!itemSite.SiteURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
                    {
                        //  URL changed then set it to submitted
                        itemSite.Status = AppSiteStatus.Submitted;
                    }
                }
            }
            else
            {
                //Incomplete
                item.Status = AppSiteStatus.Incomplete;
            }
            //if (item.Type.IsApp)
            //{
            //    var itemApp = (App)item;
            //    itemApp.MarketURL = appSiteDto.URL;
            //    itemApp.SubType = appSiteDto.SubType;
            //    if ((appSiteDto.IsPublished) && (!string.IsNullOrWhiteSpace(appSiteDto.URL)))
            //    {
            //        /*if ((string.IsNullOrWhiteSpace(itemApp.MarketURL)) || (!itemApp.MarketURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
            //            itemApp.Status = AppSiteStatus.Submitted;*/
            //        if()
            //        if ((itemApp.Status)((appSiteDto.IsPublished != itemApp.IsPublished) || (!itemApp.MarketURL.Equals(itemApp.MarketURL, StringComparison.OrdinalIgnoreCase))))
            //        {
            //            itemApp.Status = AppSiteStatus.Submitted;
            //        }
            //    }
            //    else
            //    {
            //        itemApp.Status = AppSiteStatus.Incomplete;
            //    }
            //}
            //else
            //{
            //    var itemSite = (Site)item;
            //    itemSite.SiteURL = appSiteDto.URL;
            //    if ((appSiteDto.IsPublished) && (!string.IsNullOrWhiteSpace(itemSite.SiteURL)))
            //    {
            //        /*if ((string.IsNullOrWhiteSpace(itemSite.SiteURL)) || (!itemSite.SiteURL.Equals(appSiteDto.URL, StringComparison.OrdinalIgnoreCase)))
            //            itemSite.Status = AppSiteStatus.Submitted;*/
            //        itemSite.Status = AppSiteStatus.Submitted;
            //    }
            //    else
            //    {
            //        itemSite.Status = AppSiteStatus.Incomplete;
            //    }
            //}
            if (item.Type.IsApp)
            {
                var itemApp = (App)item;
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
        public string RandomNumbersMethod(int length)
        {
            string result = string.Empty;
            var random = new Random();
            while (result.Length < length)
            {
                var r = random.Next(0, 122);
                if ((r >= 0) && (r <= 9))
                {
                    result += r.ToString();
                }
                else
                {
                    if ((r >= 97) && (r <= 122))
                    {
                        char ch = Convert.ToChar(r);
                        result += ch;
                    }
                }
            }

            return result.ToLower();
        }
        private string getPublisherId()
        {
            // return RandomNumbersMethod(10);
            return Guid.NewGuid().ToString("N");
        }
        public SaveAppSiteDtoResult Save(AppSiteDto appSite)
        {
            // trim spaces from app/site url
            if (!string.IsNullOrWhiteSpace(appSite.URL))
            {
                appSite.URL = appSite.URL.Trim();
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
                    item = MapperHelper.Map<App>(appSite);
                }
                else
                {
                    item = MapperHelper.Map<Site>(appSite);
                }
                //ToDo : Move this to the DB
                item.RegistrationDate = Framework.Utilities.Environment.GetServerTime();

                // Get AppSite Status
                setStatus(appSite, item);
            }
            if (string.IsNullOrWhiteSpace(item.PublisherId))
            {
                item.PublisherId = getPublisherId();
            }
            //Change Account 
            if (item.Account == null)
            {
                item.ChangeAccount(new Domain.Model.Account.Account() { ID = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value });
            }
            //handle the keywords
            if (appSite.NewKeywords != null)
            {
                foreach (var newKeyword in appSite.NewKeywords)
                {
                    int id;
                    Keyword newKeywordObj = null;
                    var found = false;
                    if (Int32.TryParse(newKeyword, out id))
                    {
                        newKeywordObj = keyWordRepository.Get(Convert.ToInt32(newKeyword));
                        newKeywordObj.Usage++;
                        found = true;
                    }
                    else
                    {
                        // check if we have same keyword
                        string keyword = newKeyword;
                        newKeywordObj = keyWordRepository.Query(c => c.Name.Values.Any(v => v.Value.Equals(keyword))).FirstOrDefault();
                        if (newKeywordObj != null)
                        {
                            newKeywordObj.Usage++;
                            found = true;
                        }
                        else
                        {
                            newKeywordObj = new Keyword();
                            //ToDO: Osaleh to find other way to insert the Keyword to all supported languages
                            newKeywordObj.Name =
                                new Framework.DomainServices.Localization.LocalizedString("AppSiteGroup");
                            newKeywordObj.Name.SetValue(newKeyword, "en-US");
                            newKeywordObj.Name.SetValue(newKeyword, "ar-JO");
                            newKeywordObj.Usage = 1;
                            keyWordRepository.Save(newKeywordObj);
                        }
                    }
                    if(found)
                    {
                        if(item.Keywords.FirstOrDefault(x => x.ID == newKeywordObj.ID)==null)
                        {
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

            item.Validate(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            if (item.IsValid)
            {
                appSiteRepository.Save(item);
            }
            return new SaveAppSiteDtoResult() { Id = item.ID, PublisherId = item.PublisherId };
        }
        public List<AppSiteDto> GetAppSitesByAccountId()
        {
            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            List<AppSite> appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId && appsite.IsDeleted == false).ToList();

            return appsites.Select(p => MapperHelper.Map<AppSiteDto>(p)).ToList();
        }
        public AppSiteDto Get(int appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId);
            item.Validate(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            if (item.IsValid)
            {
                return MapperHelper.Map<AppSiteDto>(item);
            }
            return null;
        }
        public SettingsDto GetSettings(int appSiteId)
        {
            var item = appSiteRepository.Get(appSiteId);
            item.Validate(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            if (item.IsValid)
            {
                SettingsDto returnValue = null;
                returnValue = item.AppSiteSetting != null
                                  ? MapperHelper.Map<SettingsDto>(item.AppSiteSetting)
                                  : new SettingsDto();
                returnValue.AppSiteId = appSiteId;
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
            //Load the AppSite Object
            var appSite = appSiteRepository.Get(settings.AppSiteId);
            // Create App Setting Object Using the DTO
            var newSetting = MapperHelper.Map<AppSiteSetting>(settings);
            appSite.ChangeSetting(newSetting);

            appSite.Validate(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            if (appSite.IsValid)
            {
                appSiteRepository.Save(appSite);
            }
        }
        public List<Interfaces.DTOs.Core.TreeDto> GetAppSitesTreeByAccountId()
        {
            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            List<AppSite> appsites = appSiteRepository.Query(appsite => appsite.Account.ID == accountId && appsite.IsDeleted == false).ToList();

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
        #endregion

        #region TextFilters

        public List<TextFilterDto> GetAppSiteTextFilters(int appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            if (appSite.IsValid)
            {
                List<TextFilter> list =
                    UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == appSiteId && p.IsDeleted == false).ToList();


                return list.Select(p => MapperHelper.Map<TextFilterDto>(p)).ToList();

            }
            else
            {
                return null;
            }
        }

        public void DeleteAppsiteFilter(int filterId)
        {
            AppSiteFilter filter =
                UnitOfWork.Current.EntitySet<AppSiteFilter>().Where(p => p.ID == filterId).
                    ToList().SingleOrDefault();

            if (filter != null)
            {
                filter.IsDeleted = true;
                UnitOfWork.Current.Save(filter);
            }

        }

        public bool UpdateAppSiteTextFilter(TextFilterDto textFilterDto, int appsiteId)
        {
            AppSite appSite = appSiteRepository.Get(appsiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            bool result = false;

            if (appSite.IsValid)
            {

                TextFilter filter = appSite.AppSiteFilters.Where(p => p.ID == textFilterDto.TextFilterId).SingleOrDefault() as TextFilter;

                List<TextFilter> textFilters = UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == appsiteId && p.MatchType.ID == textFilterDto.MatchTypeId && p.Text == textFilterDto.Text && p.IsDeleted == false).ToList();

                if (filter != null && (textFilters.Count == 0 || (textFilters.Count == 1 && textFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.Text = textFilterDto.Text;
                    filter.MatchType = new MatchType();
                    filter.MatchType.ID = textFilterDto.MatchTypeId;

                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return result;
        }

        public bool AddAppSiteTextFilter(int appSiteId, TextFilterDto textFilterDto)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            bool result = false;

            if (appSite.IsValid)
            {
                int duplicate = UnitOfWork.Current.EntitySet<TextFilter>().Where(p => p.AppSite.ID == appSiteId && p.MatchType.ID == textFilterDto.MatchTypeId && p.Text == textFilterDto.Text && !p.IsDeleted).Count();

                if (duplicate == 0)
                {
                    TextFilter filter = MapperHelper.Map<TextFilter>(textFilterDto);
                    filter.AppSite = appSite;
                    filter.CIsDeleted = false;
                    filter.CAppSiteId = appSite.ID;
                    appSite.AppSiteFilters.Add(filter);
                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region UrlFilters

        List<UrlFilterDto> IAppSiteService.GetAppSiteUrlFilters(int appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId);

            int? accountIdValue = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId;

            appSite.Validate(accountIdValue.Value);

            if (appSite.IsValid)
            {
                List<UrlFilter> list =
                  UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == appSiteId && p.IsDeleted == false).ToList();


                return list.Select(p => MapperHelper.Map<UrlFilterDto>(p)).ToList();
            }
            else
            {
                return null;
            }
        }

        public bool UpdateAppSiteUrlFilter(UrlFilterDto urlFilterDto, int appSiteId)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            bool result = false;

            if (appSite.IsValid)
            {
                UrlFilter filter = appSite.AppSiteFilters.Where(p => p.ID == urlFilterDto.UrlFilterId).SingleOrDefault() as UrlFilter;

                List<UrlFilter> urlFilters = UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == appSiteId && p.URL == urlFilterDto.Url && p.IsDeleted == false).ToList();

                if (filter != null && (urlFilters.Count == 0 || (urlFilters.Count == 1 && urlFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.URL = urlFilterDto.Url;
                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }

            return result;

        }

        public bool AddAppSiteUrlFilter(int appSiteId, UrlFilterDto urlFilterDto)
        {
            AppSite appSite = appSiteRepository.Get(appSiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            bool result = false;

            if (appSite.IsValid)
            {
                int duplicate = UnitOfWork.Current.EntitySet<UrlFilter>().Where(p => p.AppSite.ID == appSiteId && p.URL == urlFilterDto.Url && !p.IsDeleted).Count();

                if (duplicate == 0)
                {
                    var filter = MapperHelper.Map<UrlFilter>(urlFilterDto);
                    filter.AppSite = appSite;
                    filter.CAppSiteId = appSite.ID;
                    filter.CIsDeleted = false;

                    appSite.AppSiteFilters.Add(filter);
                    appSiteRepository.Save(appSite);
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region LanguageFilters

        public List<LanguageFilterDto> GetAppSiteLanguageFilters(int appSiteId)
        {
            List<LanguageFilter> list =
                UnitOfWork.Current.EntitySet<LanguageFilter>().Where(
                    p => p.AppSite.ID == appSiteId && p.IsDeleted == false).ToList();
            return list.Select(p => MapperHelper.Map<LanguageFilterDto>(p)).ToList();
        }

        public bool AddAppSiteLanguageFilter(int appSiteId, LanguageFilterDto languageFilterDto)
        {
            var appSite = appSiteRepository.Get(appSiteId);
            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            bool result = false;
            if (appSite.IsValid)
            {
                var duplicate = UnitOfWork.Current.EntitySet<LanguageFilter>().Where(p => p.AppSite.ID == appSiteId && p.Language.ID == languageFilterDto.LanguageId && p.IsDeleted == false).Count();
                if (duplicate == 0)
                {
                    var filter = MapperHelper.Map<LanguageFilter>(languageFilterDto);
                    filter.AppSite = appSite;
                    appSite.AppSiteFilters.Add(filter);
                    filter.CIsDeleted = false;
                    filter.CAppSiteId = appSite.ID;

                    appSiteRepository.Save(appSite);

                    result = true;
                }
            }
            return result;
        }

        public bool UpdateAppSiteLanguageFilter(LanguageFilterDto languageFilterDto, int appSiteId)
        {
            var appSite = appSiteRepository.Get(appSiteId);

            appSite.Validate(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            bool result = false;

            if (appSite.IsValid)
            {
                LanguageFilter filter = appSite.AppSiteFilters.Where(p => p.ID == languageFilterDto.languageFilterId).SingleOrDefault() as LanguageFilter;

                List<LanguageFilter> languageFilters = UnitOfWork.Current.EntitySet<LanguageFilter>().Where(p => p.AppSite.ID == appSiteId && p.Language.ID == languageFilterDto.LanguageId && p.IsDeleted == false).ToList();

                if (filter != null && (languageFilters.Count == 0 || (languageFilters.Count == 1 && languageFilters.Single().ID == filter.ID)))
                {
                    int Id = filter.ID;
                    filter.Language = new Language();
                    filter.Language.ID = languageFilterDto.LanguageId;

                    appSiteRepository.Save(appSite);

                    result = true;
                }

            }

            return result;
        }

        #endregion

    }
}
