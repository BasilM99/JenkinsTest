using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Persistence.RepositoriesGP.Reports;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.Framework.ConfigurationSetting;
using System.Globalization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using Noqoush.AdFalcon.Domain;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Domain.Repositories;
using System.IO;

using NHibernate;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Resources;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Reports
{
    public class ReportTestService : IReportTestService
    {
        private IReportGPRepository _ReportRepository;
        private IAdCreativeRepository _AdCreativeRepository;
        private IAdGroupRepository _AdGroupRepository;
        private IReportRecipientRepository _ReportRecipientRepository;
        private IAccountRepository _AccountRepository;
        private IConfigurationManager _ConfigurationManager;
        private IReportSchedulerRepository _ReportSchedulerRepository;
        private IDocumentRepository _DocumentRepository;
        private IDocumentTypeRepository _DocumentTypeRepository;
        private readonly ICampaignRepository _CampaignRepository;
        private IAppSiteRepository appSiteRepository = null;
        private IConfigurationManager _configurationManager;
        public ReportTestService(IAdCreativeRepository adCreativeRepository, IAdGroupRepository adGroupRepository, IReportGPRepository reportRepository, IConfigurationManager configurationManager, IReportSchedulerRepository campaignReportSchedulerRepository, ICampaignRepository CampaignReportSchedulerRepository, IAccountRepository AccountRepository, IDocumentRepository DocumentRepository, IDocumentTypeRepository DocumentTypeRepository, IAppSiteRepository appSiteRepository, IReportRecipientRepository ReportRecipientRepository)
        {
            this._ReportSchedulerRepository = campaignReportSchedulerRepository;
            this._ReportRepository = reportRepository;
            this._ConfigurationManager = configurationManager;
            this._CampaignRepository = CampaignReportSchedulerRepository;
            this._AccountRepository = AccountRepository;
            this._DocumentRepository = DocumentRepository;
            this._DocumentTypeRepository = DocumentTypeRepository;
            this.appSiteRepository = appSiteRepository;
            this._AdCreativeRepository = adCreativeRepository;
            this._AdGroupRepository = adGroupRepository;
            //this._configurationManager = configurationManager;
            this._ReportRecipientRepository = ReportRecipientRepository;
        }


  
        #region Dashboard

        public List<ChartDto> GetAppSiteChart(DashboardChartCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppSiteChart(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<AppSitePerformanceDto> GetAppSitePerformance(DashboardPerformanceCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppSitePerformance(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<AppSiteGeoLocationDto> GetAppSiteGeoLocation(DashboardGeoLocationCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppSiteGeoLocation(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<AdGeoLocationDto> GetAdGeoLocation(DashboardGeoLocationCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdGeoLocation(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAdChart(DashboardChartCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdChart(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<AdPerformanceDto> GetAdPerformance(DashboardPerformanceCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdPerformance(criteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        #endregion

        #region Campaign Reports

        public List<CampaignCommonReportDto> GetCampaignReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

        
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetAdGroupReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAdGroupReport(criteriaDto, criteriaDto.AccountId);
        }



        public List<CampaignCommonReportDto> GetCampaignPlatformReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignPlatformReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignManuFactorReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignManuFactorReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetAdReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAdReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignOperatorReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignOperatorReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignGeoLocationReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignGeoLocationReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdGroupReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }



        public List<CampaignCommonReportDto> GetCampaignPlatformReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignPlatformReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetCampaignManuFactorReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignManuFactorReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetAdReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetCampaignOperatorReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignOperatorReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetCampaignGeoLocationReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignGeoLocationReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetCampaignReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAdGroupReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdGroupReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAdReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAdReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetCampaignOperatorReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignOperatorReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetCampaignGeoLocationReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignGeoLocationReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetCampaignPlatformReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignPlatformReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetCampaignManuFactorReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignManuFactorReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }



        #endregion

        #region App Reports

        public List<AppCommonReportDto> GetAppReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;

            return _ReportRepository.GetAppReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<AppCommonReportDto> GetAppOperatorReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAppOperatorReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<AppCommonReportDto> GetAppPlatformReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAppPlatformReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<AppCommonReportDto> GetAppManuFactorReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAppManuFactorReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<AppCommonReportDto> GetAppGeoLocationReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAppGeoLocationReport(criteriaDto, criteriaDto.AccountId);
        }


        public List<AppCommonReportDto> GetAppReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public List<AppCommonReportDto> GetAppOperatorReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppOperatorReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public List<AppCommonReportDto> GetAppPlatformReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppPlatformReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public List<AppCommonReportDto> GetAppManuFactorReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            return _ReportRepository.GetAppManuFactorReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public List<AppCommonReportDto> GetAppGeoLocationReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppGeoLocationReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAppReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAppOperatorReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppOperatorReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAppGeoLocationReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppGeoLocationReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAppPlatformReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppPlatformReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<ChartDto> GetAppManuFactorReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetAppManuFactorReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        #endregion

        #region Private Members

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

        #region API

        public List<AppSiteStatisticsReport> GetAppSiteStatisticsReport(AppSiteStatisticsCriteriaDto reportCriteria)
        {

            //reportCriteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            //reportCriteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            reportCriteria.IsPrimaryUser =true;
            reportCriteria.userId =0;
            return _ReportRepository.GetAppSiteStatisticsReport(reportCriteria);
        }

        public List<AppSiteStatisticsGeoReport> GetAppSiteStatisticsGeoReport(AppSiteStatisticsCriteriaDto reportCriteria)
        {
            //reportCriteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            //reportCriteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            reportCriteria.IsPrimaryUser = true;
            reportCriteria.userId = 0;
            return _ReportRepository.GetAppSiteGeoStatisticsReport(reportCriteria);
        }

        #endregion


        #region schduler

        private int GetReportSchedulerCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            query.SetString("CounterName", "ReportScheduler");
            query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        public string GetCampaignName(int campaignId)
        {

            var name = _CampaignRepository.GetObjectName(campaignId) ;
            return name;
        }

        public string GetAdGroupName(int Id)
        {

            var name = _AdGroupRepository.Query(M => M.ID == Id).Select(M => M.Campaign.Name).SingleOrDefault<string>(); ;
            return name;
        }
        public string GetAdName(int Id)
        {

            var name = _AdCreativeRepository.Query(M => M.ID == Id).Select(M => M.Group.Campaign.Name).SingleOrDefault<string>(); ;
            return name;
        }
        public string GetAppSiteName(int appSiteId)
        {

            var name = appSiteRepository.GetObjectName(appSiteId);
            return name;
        }
        private bool validateCampaignScheduling(ReportSchedulerDto reportSchedulerDto)
        {

            return true;
        }
        public List<ReportSchedulerDto> GetCampaignJobsToSchduled()
        {
            var results = _ReportSchedulerRepository.Query(x => (x.ReportSectionType == ReportSectionType.Advertiser && (x.IsScheduled == false) && (!x.EndDate.HasValue || x.EndDate > Framework.Utilities.Environment.GetServerTime())) || (x.ReportSectionType == ReportSectionType.Advertiser && x.IsSendNow == true)).ToList();
            List<ReportSchedulerDto> dtos = new List<ReportSchedulerDto>();



            if (results != null)
            {
                dtos = results.Select(item => MapperHelper.Map<ReportSchedulerDto>(item)).ToList();

            }


            return dtos;

        }
        public List<ReportSchedulerDto> GetAppsJobsToSchduled()
        {

            var results = _ReportSchedulerRepository.Query(x => (x.ReportSectionType == ReportSectionType.Publisher && (x.IsScheduled == false) && (!x.EndDate.HasValue || x.EndDate > Framework.Utilities.Environment.GetServerTime())) || (x.ReportSectionType == ReportSectionType.Publisher && x.IsSendNow == true)).ToList();
            List<ReportSchedulerDto> dtos = new List<ReportSchedulerDto>();



            if (results != null)
            {
                dtos = results.Select(item => MapperHelper.Map<ReportSchedulerDto>(item)).ToList();

            }


            return dtos;

        }

        public void UpdateJobToSchduled(int jobID, string JobName, string TriggerName, string TriggerGroupName, DateTime? NextFireTime)
        {
            var item = _ReportSchedulerRepository.Get(jobID);

            if (item == null)
            {
                throw new Exception("object not found");

            }

            item.IsScheduled = true;

            item.JobName = JobName;
            item.TriggerName = TriggerName;
            item.TriggerGroupName = TriggerGroupName;
            _ReportSchedulerRepository.Save(item);


            //if (NextFireTime.HasValue)
            //{
            item.NextFireTime = NextFireTime;

            //}
        }

        public void UpdateJobToSendNow(int jobID)
        {
            var item = _ReportSchedulerRepository.Get(jobID);

            if (item == null)
            {
                throw new Exception("object not found");

            }

            //item.IsScheduled = true;
            item.IsSendNow = false;
            //item.JobName = JobName;
            //item.TriggerName = TriggerName;
            //item.TriggerGroupName = TriggerGroupName;
            _ReportSchedulerRepository.Save(item);



        }
        public void UpdateJobToNotFinished(int jobID)
        {
            var item = _ReportSchedulerRepository.Get(jobID);

            if (item == null)
            {
                throw new Exception("object not found");

            }

            item.IsFinished = false;

            _ReportSchedulerRepository.Save(item);



        }
        public void UpdateJobToFinishedNoFireTime(int jobID)
        {

            var item = _ReportSchedulerRepository.Get(jobID);

            if (item == null)
            {
                throw new Exception("object not found");

            }

            item.IsFinished = true;
            // if (NextFireTime.HasValue)
            //{
            //item.NextFireTime = NextFireTime;


            _ReportSchedulerRepository.Save(item);

            //}
        }
        public void UpdateJobToFinished(int jobID, DateTime? NextFireTime)
        {
            var item = _ReportSchedulerRepository.Get(jobID);

            if (item == null)
            {
                throw new Exception("object not found");

            }

            item.IsFinished = true;
            // if (NextFireTime.HasValue)
            //{
            item.NextFireTime = NextFireTime;


            _ReportSchedulerRepository.Save(item);

            //}

        }

        public bool UpdateJobToSchduledAfterRun(int jobID, string ReportFilePath)
        {

            var item = _ReportSchedulerRepository.Get(jobID);
            byte[] bytes;

            using (MemoryStream ms = new MemoryStream())
            using (FileStream file = new FileStream(ReportFilePath, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
            }


            Document doc = null;
            if (item == null)
            {
                throw new Exception("object not found");

            }


            item.LastRunningDate = Framework.Utilities.Environment.GetServerTime();

            Document itemDocu = new Document();
            if (item.LastDocumnetGenerated != null)
            {

                itemDocu = _DocumentRepository.Get(item.LastDocumnetGenerated.ID);
                if (itemDocu != null)
                {
                    itemDocu.Name = itemDocu.Name;
                    itemDocu.Content = bytes;
                    itemDocu.UploadedDate = Framework.Utilities.Environment.GetServerTime();

                    itemDocu.Size = bytes.Length;

                    itemDocu.WriteContent(itemDocu.Content);

                }
                _DocumentRepository.Save(itemDocu);



            }
            else
            {
                var documentTpye = _DocumentTypeRepository.Query(M => M.Code == ".csv").SingleOrDefault<DocumentType>();
                itemDocu.DocumentType = documentTpye;
                itemDocu.Name = Path.GetFileNameWithoutExtension(ReportFilePath);
                itemDocu.Content = bytes;
                itemDocu.UploadedDate = Framework.Utilities.Environment.GetServerTime();
                itemDocu.Extension = ".csv";
                itemDocu.Size = bytes.Length;
                itemDocu.IsWebHDFS = true;

                itemDocu.Name =itemDocu.StructureTheName(itemDocu.Name + itemDocu.Extension);
                itemDocu.WriteContent(itemDocu.Content);
                _DocumentRepository.Save(itemDocu);
                item.LastDocumnetGenerated = itemDocu;

            }

            _ReportSchedulerRepository.Save(item);


            return true;
        }
        public int TestSaveSchadulingReport(testReportSchedulerDto reportSchedulerDto)
        {


            //var item = _ReportSchedulerRepository.Get(reportSchedulerDto.ID);

            //if (item == null)
            //{
            //    item = new Noqoush.AdFalcon.Domain.Model.Core.ReportScheduler();

            //}
            //string json = new JavaScriptSerializer().Serialize(reportSchedulerDto.ReportDto);
            //item = MapperHelper.Map<Domain.Model.Core.ReportScheduler>(reportSchedulerDto);
            //item.ReportJsonCriteria = json;

            //_ReportSchedulerRepository.Save(item);

            return 0;

        }
        //private string SetCompositeMame(ReportSchedulerDto dto, bool isEmail)
        //{
        //    string Name = "";

        //    Name = isEmail ? dto.EmailSubject : dto.Name;

        //    if (dto.RecurrenceType == RecurrenceType.Day) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Daily", "Time"));
        //    if (dto.RecurrenceType == RecurrenceType.Week) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Weekly", "Time"));
        //    if (dto.RecurrenceType == RecurrenceType.Month) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Monthly", "Time"));
        //    string substitutionName = string.Empty;
        //    List<int> Ids = new List<int>();

        //    int tempId = 0;

        //    if (dto.ReportDto != null && !string.IsNullOrEmpty(dto.ReportDto.ItemsList))
        //    {
        //        string AdsList = dto.ReportDto.ItemsList.Trim(new char[] { ',' });
        //        var arrString = AdsList.Split(new char[] { ',' });
        //        foreach (var id in arrString)
        //        {
        //            tempId = 0;
        //            int.TryParse(id, out tempId);
        //            if (tempId > 0)
        //                Ids.Add(tempId);
        //        }
        //    }
        //    if (Ids.Count == 1 && string.IsNullOrEmpty(dto.PreferedName))
        //    {
        //        switch (dto.ReportDto.TabId.ToLower())
        //        {
        //            case "campaign":
        //                substitutionName = GetCampaignName(Ids[0]);
        //                Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("CampaignSubject", "Report") + substitutionName);
        //                break;
        //            case "adgroup":
        //                substitutionName = GetAdGroupName(Ids[0]);
        //                Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AdGroupSubject", "Report") + substitutionName);
        //                break;
        //            case "ad":
        //                substitutionName = GetAdName(Ids[0]);
        //                Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AdSubject", "Report") + substitutionName);
        //                break;
        //            case "app":
        //                substitutionName = GetAppSiteName(Ids[0]);
        //                Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AppSiteSubject", "Report") + substitutionName);
        //                break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(dto.PreferedName))
        //    {
        //        substitutionName = dto.PreferedName;
        //        Name = Name.Replace("{EntityName}", dto.PreferedName);

        //    }
        //    if (string.IsNullOrEmpty(substitutionName))
        //    {
        //        substitutionName = DateTime.Now.ToString("yyyyMMdd");

        //        Name = Name.Replace(" - {EntityName}", string.Empty);
        //        Name = Name.Replace("{EntityName}", string.Empty);
        //    }

        //    Name = Name.Replace("{Date}", String.Format("{0:yyyyMMdd}", dto.CreationDate));
        //    Name = Name.Replace("{ReportId}", dto.ReportId.ToString());
        //    return Name;

        //}

        public int SaveSchadulingReport(ReportSchedulerDto reportSchedulerDto)
        {


            var item = _ReportSchedulerRepository.Get(reportSchedulerDto.ID);

            if (item == null)
            {
                item = MapperHelper.Map<Domain.Model.Core.ReportScheduler>(reportSchedulerDto);
                item.CreationDate = Framework.Utilities.Environment.GetServerTime();
                item.ReportId = GetReportSchedulerCounter();

                item.User = new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value };
            }
            else
            {
                item.PreferedName = reportSchedulerDto.PreferedName;
                item.IsActive = reportSchedulerDto.IsActive;
                item.Name = reportSchedulerDto.Name;
                item.DateRecurrenceType = reportSchedulerDto.DateRecurrenceType;
                item.EndDate = reportSchedulerDto.EndDate;
                item.StartDate = reportSchedulerDto.StartDate;
                item.TimeSentAt = reportSchedulerDto.TimeSentAt;
                item.RecurrenceType = reportSchedulerDto.RecurrenceType;
                //item.ReportSectionType = reportSchedulerDto.ReportSectionType;
                item.WeekDay = reportSchedulerDto.WeekDay;
                item.MonthDay = reportSchedulerDto.MonthDay;
                item.EmailSubject = reportSchedulerDto.EmailSubject;
                item.EmailIntroduction = reportSchedulerDto.EmailIntroduction;
                item.DaysOfWeekParams = "";

            }

            item.IsScheduled = false;

            if (reportSchedulerDto.IsFriday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Friday;
            }
            if (reportSchedulerDto.IsSaturday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Saturday;
            }
            if (reportSchedulerDto.IsSunday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Sunday;
            }

            if (reportSchedulerDto.IsMonday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Monday;
            }
            if (reportSchedulerDto.IsTuesday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Tuesday;
            }
            if (reportSchedulerDto.IsWednesday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Wednesday;

            }
            if (reportSchedulerDto.IsThursday)
            {
                item.DaysOfWeekParams = item.DaysOfWeekParams + "," + (int)DayOfWeek.Thursday;
            }
            string json = new JavaScriptSerializer().Serialize(reportSchedulerDto.ReportDto);

            item.ReportJsonCriteria = json;
            var itemDto = MapperHelper.Map<ReportSchedulerDto>(item);
            //if (itemDto != null)
            //{
            //    item.CompositeName = SetCompositeMame(itemDto, false);
            //    item.CompositeEmail = SetCompositeMame(itemDto, true);
            //}

            //;
            if (item.AllRecipient != null)
            {
              
                //item.AllRecipient 
                if (item.ID == 0)
                {
                    var tempRecep = item.AllRecipient;
                    item.AllRecipient = new List<ReportRecipient>();
                    foreach (var recp in tempRecep)
                    {
                        if (!string.IsNullOrEmpty(recp.Email))
                        {
                            recp.ReportScheduler = item;
                        }
                    }
                    _ReportSchedulerRepository.Save(item);
                    _ReportRecipientRepository.Save(tempRecep);
                }
                else
                {
                    var adTrackers = item.AllRecipient;

                    foreach (var tracker in reportSchedulerDto.AllReportRecipient)
                    {
                        if (!string.IsNullOrEmpty(tracker.Email))
                        {
                            if (!adTrackers.Any(p => !p.IsDeleted && p.Email == tracker.Email))
                            {
                                adTrackers.Add(new Domain.Model.Core.ReportRecipient() { Email = tracker.Email, ReportScheduler = item });
                            }
                        }
                    }

                    foreach (var tracker in adTrackers.Where(p => !p.IsDeleted && !reportSchedulerDto.AllReportRecipient.Any(z => z.Email == p.Email)))
                    {
                        tracker.IsDeleted = true;
                    }
                    _ReportSchedulerRepository.Save(item);
                }
            }
            else
            {
                _ReportSchedulerRepository.Save(item);

            }

            return item.ID;

        }
        public string GetEmailAdminForReport()
        {
            return _ConfigurationManager.GetConfigurationSetting(null, null, "NoReplyEmail");

        }
        public ResultReportSchedulerDto QueryByCratiriaForReportSchaduling(ReportSchedulerCriteria criteria)
        {
            var result = new ResultReportSchedulerDto();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            if (!OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                criteria.UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;

            }
            IEnumerable<Domain.Model.Core.ReportScheduler> list = null;
            if (criteria.Page.HasValue)
            {
                list = _ReportSchedulerRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _ReportSchedulerRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<ReportSchedulerDto>(campaign)).ToList();

            foreach (ReportSchedulerDto reportSchedulerDto in returnList)
            {
                var jsonObj = new JavaScriptSerializer().Deserialize(reportSchedulerDto.ReportJsonCriteria, typeof(ReportCriteriaDto));
                reportSchedulerDto.ReportDto = (ReportCriteriaDto)jsonObj;
               var rep= list.Where(M => M.ID == reportSchedulerDto.ID).Single();
                reportSchedulerDto.CompositeName = rep.GetCompositeMame(reportSchedulerDto.ReportDto.ItemsList, reportSchedulerDto.ReportDto.TabId, false);
                reportSchedulerDto.ReportDto = null;
            }
            result.Items = returnList;
            result.TotalCount = _ReportSchedulerRepository.Query(criteria.GetExpression()).Count();
            return result;
        }

        public ReportSchedulerDto GetSchadulingReport(int id)
        {


            var item = _ReportSchedulerRepository.Get(id);

            if (item == null)
            {
                item = new Noqoush.AdFalcon.Domain.Model.Core.ReportScheduler();

            }

            var dto = MapperHelper.Map<ReportSchedulerDto>(item);
            if (dto.DaysOfWeekParams != null)
            {
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Friday))
                {
                    dto.IsFriday = true;

                }
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Saturday))
                {
                    dto.IsSaturday = true;

                }
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Sunday))
                {
                    dto.IsSunday = true;

                }

                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Monday))
                {
                    dto.IsMonday = true;

                }
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Tuesday))
                {
                    dto.IsTuesday = true;

                }
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Wednesday))
                {
                    dto.IsWednesday = true;


                }
                if (item.DaysOfWeekParams.Contains("" + (int)DayOfWeek.Thursday))
                {
                    dto.IsThursday = true;

                }
            }
            if (item.Account != null)
            {
                dto.AccountName = this._AccountRepository.Query(M => M.ID == item.Account.ID).Select(M => M.Name).SingleOrDefault<string>();
                if (OperationContext.Current != null)
                {
                    if (OperationContext.Current.UserInfo<AdFalconUserInfo>() != null)
                    {

                        if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != null)
                        {
                            if (dto.AccountId != OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value)
                            {
                                throw new UnauthorizedAccessException("you are not authrized");
                            }
                            if (!OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                            {
                                if (item.User!=null)
                                {
                                    if (item.User.ID != OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value)
                                    {
                                        throw new UnauthorizedAccessException("you are not authrized");
                                    }
                                }
                            }
                        }
                    }

                }
                dto.LanguageCode = item.Account.PrimaryUser.Language.Code;
                dto.IsPrimaryUser = true;
                if (item.User == null)
                {

                    dto.UserId = item.Account.PrimaryUser.ID;
                }
                else
                {
                    dto.IsPrimaryUser = false;
                    if (item.Account.PrimaryUser.ID== item.User.ID)
                    {
                        dto.IsPrimaryUser = true;

                    }
                    dto.UserId = item.User.ID;
                }
            }
            

            var jsonObj = new JavaScriptSerializer().Deserialize(item.ReportJsonCriteria, typeof(ReportCriteriaDto));
            dto.ReportDto = (ReportCriteriaDto)jsonObj;
            CalculateDateCorrectlyForReportScheduler(dto.DateRecurrenceType, dto.ReportDto);
            return dto;

        }
        private void CalculateDateCorrectlyForReportScheduler(DateRecurrenceType recDateType, ReportCriteriaDto dto)
        {
            var date = Framework.Utilities.Environment.GetServerTime();
            if (recDateType == DateRecurrenceType.Today)
            {
                dto.FromDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
                dto.ToDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
            }
            else if (recDateType == DateRecurrenceType.YearToDate)
            {
                dto.FromDate = new DateTime(date.Year, 1, 1, 0, 0, 0, 0);
                dto.ToDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
            }
            else if (recDateType == DateRecurrenceType.PreviousMonth)
            {
                var newdate = date.AddMonths(-1);
                var days = DateTime.DaysInMonth(newdate.Year, newdate.Month);
                dto.FromDate = new DateTime(newdate.Year, newdate.Month, 1, 0, 0, 0, 0);
                dto.ToDate = new DateTime(newdate.Year, newdate.Month, days, 23, 59, 59, 999);

                // dto.FromDate = StartOfDay(Framework.Utilities.Environment.GetServerTime());
                // dto.ToDate = EndOfDay(Framework.Utilities.Environment.GetServerTime());
            }
            else if (recDateType == DateRecurrenceType.MonthtoDate)
            {
                dto.FromDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
                dto.ToDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
            }
            else if (recDateType == DateRecurrenceType.Last7Days)
            {
                dto.ToDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
                var newdate = date.AddDays(-6);
                dto.FromDate = new DateTime(newdate.Year, newdate.Month, newdate.Day, 0, 0, 0, 0);
            }
        }
        private DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        private DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
        public bool DeleteSchadulingReport(int id)
        {

            var item = _ReportSchedulerRepository.Get(id);

            if (item != null)
            {

                _ReportSchedulerRepository.Remove(item);

                return true;
            }
            else
            {
                return false;

            }

        }

        public void RunSchadulingReport(int[] Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _ReportSchedulerRepository.Get(item)))
                {

                    item.Resume();
                    _ReportSchedulerRepository.Save(item);

                }
            }
        }
        public void DeleteSchadulingReportBulk(int[] Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _ReportSchedulerRepository.Get(item)))
                {

                    item.Delete();
                    _ReportSchedulerRepository.Save(item);

                }
            }
        }


        public void PauseSchadulingReport(int[] Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _ReportSchedulerRepository.Get(item)))
                {

                    item.Pause();
                    _ReportSchedulerRepository.Save(item);

                }
            }
        }
        public void SendSchadulingReport(int[] Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _ReportSchedulerRepository.Get(item)))
                {

                    item.SendNow();
                    _ReportSchedulerRepository.Save(item);

                }
            }
        }
        #endregion

    }
}
