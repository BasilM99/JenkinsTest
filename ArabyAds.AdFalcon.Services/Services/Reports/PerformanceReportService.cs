using ArabyAds.AdFalcon.Domain;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Persistence.Reports.Repositories;
using ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Services.Reports
{
    public class PerformanceReportService : IPerformanceReportService
    {
        private dynamic _PerformanceReportRepository;
        private IConfigurationManager _ConfigurationManager;
        private IAppSiteRepository _AppSiteRepository;
        private IUserRepository _UserRepository;
        private IAccountRepository _AccountRepository;

        private IUserAccountsRepository _UserAccountsRepository;
        public PerformanceReportService( IConfigurationManager configurationManager, IAppSiteRepository appsiteRepository, IUserRepository userRepository, IAccountRepository accountRepository, IUserAccountsRepository UserAccountsRepository)
        {

           
                string reporintGPFlag = JsonConfigurationManager.AppSettings["ReportingGP"];


                if (!string.IsNullOrEmpty(reporintGPFlag) && reporintGPFlag.ToLower() == "true")
                    this._PerformanceReportRepository = Framework.IoC.Instance.Resolve<IPerformanceGPReportRepository>();
                else
                    this._PerformanceReportRepository = Framework.IoC.Instance.Resolve<IPerformanceReportRepository>();
              //  this._PerformanceReportRepository = reportRepository;
            this._ConfigurationManager = configurationManager;
            this._AppSiteRepository = appsiteRepository;
            this._AccountRepository = accountRepository;

            this._UserAccountsRepository = UserAccountsRepository;
        }


        public List<BaseAppSitePerformanceDetailsDto> GetAccountsPerformanceDetails(GetPerformanceDetailsRequest request)
        {
            if (!string.IsNullOrEmpty(request.AccountName) || !string.IsNullOrEmpty(request.AppSiteName))
            {
                SetAccountsIds(request.AccountName, request.Criteria);

                SetAppSiteIds(request.AppSiteName, request.Criteria);
            }

            request.Criteria.FromDate = FixReportStartDate(request.Criteria.FromDate);
            request.Criteria.ToDate = FixReportEndDate(request.Criteria.ToDate);

            return _PerformanceReportRepository.GetAccountsPerformanceDetails(request.Criteria);
        }


        public List<BaseAppSitePerformanceDetailsDto> GetPlatformsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteria)
        {
            criteria.FromDate = FixReportStartDate(criteria.FromDate);
            criteria.ToDate = FixReportEndDate(criteria.ToDate);

            return _PerformanceReportRepository.GetPlatformsPerformanceDetails(criteria);
        }


        public List<BaseAppSitePerformanceDetailsDto> GetAppSitesPerformanceDetails(GetPerformanceDetailsRequest request)
        {
            if (!string.IsNullOrEmpty(request.AccountName) || !string.IsNullOrEmpty(request.AppSiteName))
            {
                SetAccountsIds(request.AccountName, request.Criteria);

                SetAppSiteIds(request.AppSiteName, request.Criteria);
            }

            request.Criteria.FromDate = FixReportStartDate(request.Criteria.FromDate);
            request.Criteria.ToDate = FixReportEndDate(request.Criteria.ToDate);

            return _PerformanceReportRepository.GetAppSitesPerformanceDetails(request.Criteria);
        }


        #region Private Members

      
        private void SetAccountsIds(string accountName, BaseAppSitePerformanceDetailsCriteria criteria)
        {
            if (!string.IsNullOrEmpty(accountName))
            {
                string trimmedAccountName = accountName.Trim();

                IEnumerable<int> accountsIds = this._UserAccountsRepository.Query(p => (p.User.FirstName.Contains(trimmedAccountName) || 
                                                                            p.User.LastName.Contains(trimmedAccountName) || 
                                                                            p.User.EmailAddress.Contains(trimmedAccountName) || 
                                                                            p.User.Company.Contains(trimmedAccountName)
                                                                            )).Select(p => p.Account.ID);
                if (accountsIds.Count() != 0)
                {
                    criteria.AccountIds = accountsIds;
                }
                else
                {
                    criteria.AccountIds = new int[] { -1 };
                }
            }
        }


        private void SetAppSiteIds(string appSiteName, BaseAppSitePerformanceDetailsCriteria criteria)
        {
            if (!string.IsNullOrEmpty(appSiteName))
            {
                IEnumerable<int> appsiteIds = _AppSiteRepository.Query(p => p.Name.Contains(appSiteName.Trim())).Select(p => p.ID);

                if (appsiteIds.Count() != 0)
                {
                    criteria.AppSiteIds = appsiteIds;
                }
                else
                {
                    criteria.AppSiteIds = new int[] { -1 };
                }
               
            }
        }

        private DateTime FixReportStartDate(DateTime dateTime)
        {
            if (dateTime < Configuration.ApplicationReleaseDate)
            {
                dateTime = Configuration.ApplicationReleaseDate;
            }
            else
            {
                if (dateTime > DateTime.Now)
                {
                    dateTime = DateTime.Now.Date;
                }
            }
            return dateTime;
        }

        private DateTime FixReportEndDate(DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
            {
                dateTime = DateTime.Now.Date;
            }

            return dateTime;
        }

        #endregion
    }
}
