using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Persistence.Repositories.Reports;
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
using Noqoush.AdFalcon.Persistence.RepositoriesGP.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using Noqoush.AdFalcon.Domain.Repositories.QueryBuilder;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using Noqoush.AdFalcon.Domain.Model.QueryBuilder;
using Npgsql;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Services;
using Newtonsoft.Json;
using Noqoush.AdFalcon.Persistence.ReportsGP.CardinalityEstimator;
using Noqoush.AdFalcon.Persistence.Reports.RepositoriesGP;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.Core;

namespace Noqoush.AdFalcon.Services.Services.Reports
{
    public class ReportService : IReportService
    {
        private dynamic _ReportRepository;
        private IAdCreativeRepository _AdCreativeRepository;
        private IAdGroupRepository _AdGroupRepository;
        private IReportRecipientRepository _ReportRecipientRepository;
        private IAccountRepository _AccountRepository;
        private ImetriceColumnRepository _metriceColumnRepository;
        private IConfigurationManager _ConfigurationManager;
        private IReportSchedulerRepository _ReportSchedulerRepository;
        private IDocumentRepository _DocumentRepository;
        private IDocumentTypeRepository _DocumentTypeRepository;
        private readonly ICampaignRepository _CampaignRepository;
        private IAppSiteRepository appSiteRepository = null;
        private IConfigurationManager _configurationManager;
        private ImetriceColumnReportCriteriaRepository _imetriceColumnReportCriteriaRepository;
        private IReportCriteriaRepository _reportCriteriaRepository;
        private IAdvertiserRepository _AdvertiserRepository;
        private IAdvertiserAccountRepository AdvertiserAccountRepository;


        private IFactRepository _FactRepository;
        private IDimensionRepository _DimensionRepository;
        private IColumnQBRepository _ColumnQBRepository;
        private IMeasureRepository _MeasureRepository;

        private IAccountStatistic _AccountStatistic;
        private ISummaryGPRepository _SummaryGPRepository;
        public ReportService(IAdvertiserAccountRepository advertiserAccountRepository, IAdCreativeRepository adCreativeRepository, IAdGroupRepository adGroupRepository, IConfigurationManager configurationManager, IReportSchedulerRepository campaignReportSchedulerRepository, ICampaignRepository CampaignReportSchedulerRepository, IAccountRepository AccountRepository, IDocumentRepository DocumentRepository, IDocumentTypeRepository DocumentTypeRepository, IAppSiteRepository appSiteRepository, IReportRecipientRepository ReportRecipientRepository, IReportCriteriaRepository _reportCriteriaRepository, ImetriceColumnRepository metriceColumnRepository, ImetriceColumnReportCriteriaRepository _imetriceColumnReportCriteriaRepository,
            IAdvertiserRepository AdvertiserRepository



                , IFactRepository FactRepository
               , IDimensionRepository DimensionRepository
                , IColumnQBRepository ColumnQBRepository
               , IMeasureRepository MeasureRepository
                        , IAccountStatistic AccountStatistic
                            , ISummaryGPRepository SummaryGPRepository
            )
        {
            string reporintGPFlag = System.Configuration.ConfigurationManager.AppSettings["ReportingGP"];


            if (!string.IsNullOrEmpty(reporintGPFlag) && reporintGPFlag.ToLower() == "true")
                this._ReportRepository = Framework.IoC.Instance.Resolve<IReportGPRepository>();

            else
                this._ReportRepository = Framework.IoC.Instance.Resolve<IReportRepository>();
            this.AdvertiserAccountRepository = advertiserAccountRepository;
            this._ReportSchedulerRepository = campaignReportSchedulerRepository;
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
            this._reportCriteriaRepository = _reportCriteriaRepository;
            _metriceColumnRepository = metriceColumnRepository;
            this._imetriceColumnReportCriteriaRepository = _imetriceColumnReportCriteriaRepository;
            this._AdvertiserRepository = AdvertiserRepository;

            this._FactRepository = FactRepository;
            this._DimensionRepository = DimensionRepository;
            this._ColumnQBRepository = ColumnQBRepository;
            this._MeasureRepository = MeasureRepository;
            this._AccountStatistic = AccountStatistic;

            this._SummaryGPRepository = SummaryGPRepository;
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

            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;

            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetAdGroupReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAdGroupReport(criteriaDto, criteriaDto.AccountId);
        }



        public List<CampaignCommonReportDto> GetCampaignPlatformReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;

            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignPlatformReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignManuFactorReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignManuFactorReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetAdReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetAdReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<CampaignCommonReportDto> GetCampaignSubAppSiteReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignSubAppSiteReport(criteriaDto, criteriaDto.AccountId);

        }

        public List<CampaignCommonReportDto> GetCampaignOperatorReportForEmailService(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
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


            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;

            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignGeoLocationReport(criteriaDto, criteriaDto.AccountId);
        }

        public List<CampaignCommonReportDto> GetCampaignAudianceSegmentForAdvertiserReportForEmailService(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);


            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;

            criteriaDto.IsPrimaryUser = criteriaDto.IsPrimaryUser;
            criteriaDto.userId = criteriaDto.userId;
            return _ReportRepository.GetCampaignAudianceSegmentReport(criteriaDto, criteriaDto.AccountId);
        }
        public List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto)
        {


            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);

            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto)
        {

            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.RFromDate = criteriaDto.FromDate;
            criteriaDto.RToDate = criteriaDto.ToDate;

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

        public List<CampaignCommonReportDto> GetCampaignAudianceSegmentReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignAudianceSegmentReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
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
        public List<CampaignCommonReportDto> GetCampaignSubAppSiteReport(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignSubAppSiteReport(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
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

        public List<ChartDto> GetCampaignAudianceSegmentReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignAudianceSegmentReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public List<ChartDto> GetCampaignAppSiteReportChart(ReportCriteriaDto criteriaDto)
        {
            criteriaDto.FromDate = FixReportStartDate(criteriaDto.FromDate);
            criteriaDto.ToDate = FixReportEndDate(criteriaDto.ToDate);
            criteriaDto.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteriaDto.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            return _ReportRepository.GetCampaignAppSiteReportChart(criteriaDto, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
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
                if (dateTime > Framework.Utilities.Environment.GetServerTime())
                {
                    dateTime = Framework.Utilities.Environment.GetServerTime().Date;
                }
            }
            return dateTime;
        }

        private DateTime FixReportEndDate(DateTime dateTime)
        {
            if (dateTime > Framework.Utilities.Environment.GetServerTime())
            {
                dateTime = Framework.Utilities.Environment.GetServerTime().Date;
            }

            return dateTime;
        }


        #endregion

        #region API

        public List<AppSiteStatisticsReport> GetAppSiteStatisticsReport(AppSiteStatisticsCriteriaDto reportCriteria)
        {

            //reportCriteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            //reportCriteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            reportCriteria.IsPrimaryUser = true;
            reportCriteria.userId = 0;
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

            var name = _CampaignRepository.GetObjectName(campaignId);
            return name;
        }
        public int GetCampaignAccountId(int campaignId)
        {

            var camp = _CampaignRepository.Get(campaignId);
            return camp.Account.ID;
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
                // itemDocu.IsWebHDFS = true;
                //itemDocu.Name = itemDocu.StructureTheName(itemDocu.Name+itemDocu.Extension);
                itemDocu.IsWebHDFS = true;
                itemDocu.Name = itemDocu.StructureTheName(itemDocu.Name + itemDocu.Extension);
                itemDocu.WriteContent(itemDocu.Content);
    

              
                //itemDocu.WriteContent(itemDocu.Content);

                _DocumentRepository.Save(itemDocu);
                item.LastDocumnetGenerated = itemDocu;
              
                _DocumentRepository.Save(itemDocu);
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

            //if (reportSchedulerDto.ReportDto.AdvertiserId>0)
            //{
            //    IList<AdvertiserAccount> accountsAdv = AdvertiserAccountRepository.Query(x => x.Advertiser.ID == reportSchedulerDto.ReportDto.AdvertiserId && x.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value).ToList() ;

            //    reportSchedulerDto.ReportDto.AccountAdvertiserId = advertiserAccount.ID;

            //}
            string json = new JavaScriptSerializer().Serialize(reportSchedulerDto.ReportDto);

            if (item.ID == 0)
            {
                ReportCriteria ReportCriteria = new ReportCriteria
                {
                    Account = new Noqoush.AdFalcon.Domain.Model.Account.Account
                    {
                        ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                    },
                    User = new Noqoush.AdFalcon.Domain.Model.Account.User
                    {
                        ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value
                    },
                    CreationDate = Framework.Utilities.Environment.GetServerTime(),
                    SectionType = item.ReportSectionType,
                    Criteria = json

                };
                ReportCriteria.Columns = new List<metriceColumnReportCriteria>();
                foreach (var metriceColumn in reportSchedulerDto.MatixColumns)
                {

                    ReportCriteria.Columns.Add(new metriceColumnReportCriteria { ReportCriteria = ReportCriteria, metriceColumn = new metriceColumn { Id = metriceColumn } });

                }

                _reportCriteriaRepository.Save(ReportCriteria);
                item.ReportCriteria = ReportCriteria;

            }
            else
            {
                ReportCriteria ReportCriteria = _reportCriteriaRepository.Get(item.ReportCriteria.ID);
                if (ReportCriteria != null)
                {
                    ReportCriteria.Criteria = json;
                }
                else
                {
                    ReportCriteria = new ReportCriteria
                    {
                        Account = new Noqoush.AdFalcon.Domain.Model.Account.Account
                        {
                            ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                        },
                        User = new Noqoush.AdFalcon.Domain.Model.Account.User
                        {
                            ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value
                        },
                        Name = reportSchedulerDto.Name,
                        CreationDate = Framework.Utilities.Environment.GetServerTime(),
                        SectionType = item.ReportSectionType,
                        Criteria = json

                    };
                    item.ReportCriteria = ReportCriteria;

                }

                if (ReportCriteria.Columns == null)
                    ReportCriteria.Columns = new List<metriceColumnReportCriteria>();
                else
                    ReportCriteria.Columns.Clear();
                foreach (var metriceColumn in reportSchedulerDto.MatixColumns)
                {

                    ReportCriteria.Columns.Add(new metriceColumnReportCriteria { ReportCriteria = ReportCriteria, metriceColumn = new metriceColumn { Id = metriceColumn } });

                }

                _reportCriteriaRepository.Save(ReportCriteria);

            }



            var itemDto = MapperHelper.Map<ReportSchedulerDto>(item);

            if (item.AllRecipient != null)
            {
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
        public ResultReportSchedulerDto QueryByCratiriaForReportSchaduling(Noqoush.AdFalcon.Domain.Common.Repositories.Core.ReportSchedulerCriteria wcriteria)
        {
            ReportSchedulerCriteria criteria = new ReportSchedulerCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
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
                var criteriaItem = _reportCriteriaRepository.Get(reportSchedulerDto.CriteriaSchedulerId);
                var jsonObj = new JavaScriptSerializer().Deserialize(criteriaItem != null ? criteriaItem.Criteria : reportSchedulerDto.ReportJsonCriteria, typeof(ReportCriteriaDto));
                reportSchedulerDto.ReportDto = (ReportCriteriaDto)jsonObj;
                var rep = list.Where(M => M.ID == reportSchedulerDto.ID).Single();
                reportSchedulerDto.CompositeName = rep.GetCompositeMame(reportSchedulerDto.ReportDto.ItemsList, reportSchedulerDto.ReportDto.TabId, false);
                reportSchedulerDto.ReportDto = null;
                //reportSchedulerDto.Name = criteriaItem.Name;
            }
            result.Items = returnList;
            result.TotalCount = _ReportSchedulerRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public ResultReportSchedulerDto QueryByCratiriaForReportCriteria(Noqoush.AdFalcon.Domain.Common.Repositories.Core.ReportJsonCriteria wcriteria)
        {

            ReportJsonCriteria criteria = new ReportJsonCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new ResultReportSchedulerDto();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            //if (!OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            //{
            criteria.UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId;

            //}
            IEnumerable<Domain.Model.Core.ReportCriteria> list = null;
            if (criteria.Page.HasValue)
            {
                list = _reportCriteriaRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _reportCriteriaRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<ReportCriteriaSchedulerDto>(campaign)).ToList();


            result.CriteriaItems = returnList;
            result.TotalCount = _reportCriteriaRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public int SaveReportCriteria(ReportCriteriaSchedulerDto reportSchedulerDto)
        {
            var item = _reportCriteriaRepository.Get(reportSchedulerDto.ID);

            if (item == null)
            {
                item = MapperHelper.Map<Domain.Model.Core.ReportCriteria>(reportSchedulerDto);
                item.CreationDate = Framework.Utilities.Environment.GetServerTime();
                item.Account = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value };

                item.User = new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value };
            }
            else
            {
                item.Criteria = reportSchedulerDto.Criteria;

                item.Name = reportSchedulerDto.Name;


            }
            _reportCriteriaRepository.Save(item);

            return item.ID;

        }
        public ReportSchedulerDto GetSchadulingReport(int id)
        {


            var item = _ReportSchedulerRepository.Get(id);

            if (item == null)
            {
                item = new Noqoush.AdFalcon.Domain.Model.Core.ReportScheduler();

            }

            var dto = MapperHelper.Map<ReportSchedulerDto>(item);

            if (item.ReportCriteria != null & item.ReportCriteria.Columns != null
                )
            {
                dto.MatixColumns = new List<int>();

                foreach (var column in item.ReportCriteria.Columns)
                {
                    dto.MatixColumns.Add(column.metriceColumn.Id);
                }
            }
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
                                if (item.User != null)
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
                    if (item.Account.PrimaryUser.ID == item.User.ID)
                    {
                        dto.IsPrimaryUser = true;

                    }
                    dto.UserId = item.User.ID;
                }
            }

            dto.MatixColumns = new List<int>();

            var jsonObj = new JavaScriptSerializer().Deserialize(item.ReportCriteria != null ? item.ReportCriteria.Criteria : item.ReportJsonCriteria, typeof(ReportCriteriaDto));
            dto.ReportDto = (ReportCriteriaDto)jsonObj;

            if (item.ReportCriteria != null && item.ReportCriteria.Columns != null && item.ReportCriteria.Columns.Count > 0)
            {
                foreach (var column in item.ReportCriteria.Columns)
                {
                    dto.MatixColumns.Add(column.metriceColumn.Id);
                }
            }
            else
            {
                dto.MatixColumns = GetmetriceColumnsForSchudling(item, dto.ReportDto);

            }
            CalculateDateCorrectlyForReportScheduler(dto.DateRecurrenceType, dto.ReportDto);


            if (dto.ReportDto != null && dto.ReportDto.AdvertiserId > 0)
            {



                dto.AdvertiserName = _AdvertiserRepository.Get(dto.ReportDto.AdvertiserId).Name.Value;

            }

            if (dto.ReportDto != null && dto.ReportDto.AccountAdvertiserId > 0)
            {



                dto.AdvertiserAccountName = AdvertiserAccountRepository.Get(dto.ReportDto.AccountAdvertiserId).Name;

            }
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

        public void DeleteCriteriaReportBulk(int[] Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _reportCriteriaRepository.Get(item)))
                {

                    // item.Delete();
                    _reportCriteriaRepository.Remove(item);

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


        #region  Deals



        public List<DealPerformanceDto> GetDealPerformance(DashboardPerformanceCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            ReportCriteriaDto reportCriteria = _ReportRepository.GetReportCriteriaDto(criteria);



            if (criteria.IdSubFilter.HasValue)
            {

                reportCriteria.AdvancedCriteria = "" + criteria.IdSubFilter.Value;
                return _ReportRepository.GetDealCampAdGroupReport(reportCriteria, GetCampaignAccountId(criteria.IdSubFilter.Value));

            }
            else if (criteria.IdSecondSubFilter.HasValue)
            {
                reportCriteria.SecondAdvancedCriteria = "" + criteria.IdSecondSubFilter.Value;
                return _ReportRepository.GetDealCampAdGroupReport(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            }
            else if (criteria.IdFilter.HasValue)
            {

                return _ReportRepository.GetDealCampReport(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            }
            else
            {
                return _ReportRepository.GetDealReport(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            }
        }

        public List<ChartDto> GetDealChart(DashboardChartCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;


            ReportCriteriaDto reportCriteria = _ReportRepository.GetReportCriteriaDto(criteria);



            if (criteria.IdSubFilter.HasValue)
            {
                reportCriteria.AdvancedCriteria = "" + criteria.IdSubFilter.Value;

                return _ReportRepository.GetDealCampReportChart(reportCriteria, GetCampaignAccountId(criteria.IdSubFilter.Value));
            }
            else if (criteria.IdSecondSubFilter.HasValue)
            {
                reportCriteria.SecondAdvancedCriteria = "" + criteria.IdSecondSubFilter.Value;

                return _ReportRepository.GetDealCampAdGroupReportChart(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            }

            else
            {
                return _ReportRepository.GetDealReportChart(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            }
        }

        #endregion


        #region  ImpressionLog



        public List<ImpressionLogPerformanceDto> GetImpressionLogPerformance(DashboardPerformanceCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            ReportCriteriaDto reportCriteria = _ReportRepository.GetReportCriteriaDto(criteria);
            reportCriteria.ItemsList = criteria.DPProviderId.ToString();

            return _ReportRepository.GetImpressionLogReport(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

        }

        public List<ChartDto> GetImpressionLogChart(DashboardChartCriteria criteria)
        {
            criteria.IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            criteria.userId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
            ReportCriteriaDto reportCriteria = _ReportRepository.GetReportCriteriaDto(criteria);

            reportCriteria.ItemsList = criteria.DPProviderId.ToString();

            return _ReportRepository.GetImpressionLogReportChart(reportCriteria, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

        }



        public List<CampaignCommonReportDto> getAdvertisersListForDP(int dataproviderId, string culture, string q, int dateFrom, int dateTo)
        {
            return _ReportRepository.getAdvertisersListForDP(dataproviderId, culture, q, dateFrom, dateTo);
        }

        public List<CampaignCommonReportDto> getAgencyForDP(int dataproviderId, string q, int dateFrom, int dateTo)
        {
            return _ReportRepository.getAgencyForDP(dataproviderId, q, dateFrom, dateTo);

        }

        public List<CampaignCommonReportDto> getCampaignForDP(int dataproviderId, string q, int dateFrom, int dateTo)
        {
            return _ReportRepository.getCampaignForDP(dataproviderId, q, dateFrom, dateTo);
        }
        #endregion

        #region Columns
        public List<metriceColumnDto> GetmetriceColumns()
        {
            var items = _metriceColumnRepository.GetAll();
            List<metriceColumnDto> mappeditems = items.Select(x => MapperHelper.Map<metriceColumnDto>(x)).ToList();
            return mappeditems;
        }
        public List<metriceColumnDto> GetmetriceColumnsForPublisher()
        {
            var ShowDSP = false;
            if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)Domain.Common.Model.Account.AccountRole.DSP || Domain.Configuration.IsAdmin)
            {
                ShowDSP = true;
            }


            var items = _metriceColumnRepository.Query(M => M.Publisher == true).OrderBy(M => M.Order).ToList();
            if (ShowDSP == false)
                items = items.Where(M => M.DSP == ShowDSP).ToList();
            List<metriceColumnDto> mappeditems = items.Select(x => MapperHelper.Map<metriceColumnDto>(x)).ToList();
            return mappeditems;
        }
        public List<metriceColumnDto> GetmetriceColumnsForAdvertiser()
        {
            var ShowDSP = false;
            if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)Domain.Common.Model.Account.AccountRole.DSP || Domain.Configuration.IsAdmin)
            {
                ShowDSP = true;
            }
            var items = _metriceColumnRepository.Query(M => M.Advertiser == true).OrderBy(M => M.Order).ToList();
            if (ShowDSP == false)
                items = items.Where(M => M.DSP == ShowDSP).ToList();

            items = items.Where(M => M.HeaderResourceKey != "MediaSpend" && M.HeaderResourceKey != "DataSpend" && M.HeaderResourceKey != "TotalSpend").ToList();
            List<metriceColumnDto> mappeditems = items.Select(x => MapperHelper.Map<metriceColumnDto>(x)).ToList();
            return mappeditems;
        }
        public List<metriceColumnDto> GetAllmetriceColumnsForPublisher()
        {
            var ShowDSP = false;
            //  if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)Domain.Model.Account.AccountRole.DSP || Domain.Configuration.IsAdmin)
            //{
            ShowDSP = true;
            // }


            var items = _metriceColumnRepository.Query(M => M.Publisher == true).OrderBy(M => M.Order).ToList();
            if (ShowDSP == false)
                items = items.Where(M => M.DSP == ShowDSP).ToList();
            List<metriceColumnDto> mappeditems = items.Select(x => MapperHelper.Map<metriceColumnDto>(x)).ToList();
            return mappeditems;
        }
        public List<metriceColumnDto> GetAllmetriceColumnsForAdvertiser()
        {
            var ShowDSP = false;
            // if (Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole == (int)Domain.Model.Account.AccountRole.DSP || Domain.Configuration.IsAdmin)
            //{
            ShowDSP = true;
            //}
            var items = _metriceColumnRepository.Query(M => M.Advertiser == true).OrderBy(M => M.Order).ToList();
            if (ShowDSP == false)
                items = items.Where(M => M.DSP == ShowDSP).ToList();
            List<metriceColumnDto> mappeditems = items.Select(x => MapperHelper.Map<metriceColumnDto>(x)).ToList();
            return mappeditems;
        }

        public metriceColumnDto GetColumn(int id)
        {
            metriceColumn Column = _metriceColumnRepository.Get(id);
            metriceColumnDto mappeditem = MapperHelper.Map<metriceColumnDto>(Column);
            return mappeditem;
        }


        private List<int> GetmetriceColumnsForSchudling(ReportScheduler item, ReportCriteriaDto repdto)
        {
            var ShowDSP = false;
            List<int> Columns = new List<int>();
            if (_AccountRepository.IsAccountDSP(item.Account.ID))
            {
                ShowDSP = true;
            }
            List<metriceColumn> items = null;
            if (item.ReportSectionType == ReportSectionType.Advertiser)
            {
                items = _metriceColumnRepository.Query(M => M.Advertiser == true).OrderBy(M => M.Order).ToList();
                bool showCI = false;

                if ((repdto.SummaryBy == 1 || repdto.SummaryBy == 4) && repdto.Layout == "detailed" && !repdto.GroupByName)
                {
                    showCI = true;
                }

                if (ShowDSP == false)
                    items = items.Where(M => M.DSP == ShowDSP).ToList();
                if (!showCI)
                {
                    items = items.Where(M => M.AppFieldName != "UniqueImp" && M.AppFieldName != "UniqueClicks").ToList();
                }
            }
            else
                items = _metriceColumnRepository.Query(M => M.Publisher == true).OrderBy(M => M.Order).ToList();

            if (items != null)
            {
                foreach (var itemC in items)
                {
                    if (itemC.IsSelected)
                        Columns.Add(itemC.Id);
                }
            }
            return Columns;
        }
        public int GetColumnId(string AppFieldName, bool Publisher)
        {

            return _metriceColumnRepository.GetColumnId(AppFieldName, Publisher);
        }
        public List<int> GetmetriceColumnsForTemplate(int TemplateId)
        {
            return _imetriceColumnReportCriteriaRepository.GetmetriceColumnsForTemplate(TemplateId);
        }

        #endregion


        #region TP


        public List<CampaignCommonReportDto> GetCampaignSubAppSiteReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignSubAppSiteReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetCampaignAdSizeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignAdSizeReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetCampaignDeviceTypeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignDeviceTypeReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetCampaignOSReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignOSReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetCampaignCountryReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignCountryReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetCampaignAdTypeGroupReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignAdTypeGroupReportTraficPlannerDrillDown(traficDto);
            return results;
        }
        public List<CampaignCommonReportDto> GetCampaignGenderReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetCampaignGenderReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        public List<CampaignCommonReportDto> GetEnvironmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            List<CampaignCommonReportDto> results = _ReportRepository.GetEnvironmentReportTraficPlannerDrillDown(traficDto);
            return results;
        }

        #endregion

        public List<DataQBDto> GetResultofDataQBDto(string script, string optionaldrop, string methdname, int page)
        {


            string CountQuery = string.Format("SELECT Count(*) From ({0}) AS CountQueryResultSet", script);
            string PaginationQuery = string.Format("SELECT * From ({0}) AS PaginationResultSet LIMIT {1}  OFFSET  {2}", script, 10, (page - 1) * 10);


            var TotalCount = this._ReportRepository.GetResultofDataQBCount(CountQuery, optionaldrop, methdname);

            var results = this._ReportRepository.GetResultofDataQBDto(PaginationQuery, optionaldrop, methdname);

            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Execute-{1}", "GetResultofDataQBDto", PaginationQuery);
            if (results != null)
            {

                foreach (var res in results)
                {
                    res.TotalCount = TotalCount;
                }
            }
            return results;
        }

        public List<DataQBDto> GetResultofDataQBDtoWithScoping(string script, string optionaldrop, string methdname, int page)
        {


            string CountQuery = string.Format("SELECT Count(*) From ({0}) AS CountQueryResultSet", script);
            string PaginationQuery = string.Format("SELECT * From ({0}) AS PaginationResultSet LIMIT {1}  OFFSET  {2}", script, 10, (page - 1) * 10);

            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Execute-{1}", "GetResultofDataQBDtoWithScoping", PaginationQuery);

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery queryCount = null;

            ISQLQuery queryResult = null;
            queryCount =
                  nhibernateSession.CreateSQLQuery(CountQuery);

            queryResult =
                 nhibernateSession.CreateSQLQuery(PaginationQuery);

            var TotalCount = queryCount.UniqueResult<long>();

            queryResult.SetResultTransformer(Transformers.AliasToBean<DataQBDto>());
            var results = queryResult.List<DataQBDto>().ToList() ?? new List<DataQBDto>();


            if (results != null)
            {

                foreach (var res in results)
                {
                    res.TotalCount = TotalCount;
                }
            }
            return results;
        }
        #region  Query Builder

        public List<FactDto> GetAllFacts()
        {
            var items = this._FactRepository.GetAll();
            items = items.Where(M => M.IsDeleted == false);
            List<FactDto> mappeditems = items.Select(x => MapperHelper.Map<FactDto>(x)).ToList();
            return mappeditems;
        }

        public FactDto GetFactById(int Id)
        {
            var item = this._FactRepository.Get(Id);
            FactDto Fact = MapperHelper.Map<FactDto>(item);
            return Fact;
        }


        public List<ColumnQBDto> GetColumnsByFactId(int Id, bool includeId)
        {
            List<ColumnQB> columns = _FactRepository.Get(Id).Columns.OrderBy(x => x.DisplayName).ToList();
            List<ColumnQBDto> mappeditems = columns.Where(M => M.IsDeleted == false && M.IsHidden == false).Select(x => MapperHelper.Map<ColumnQBDto>(x)).ToList();
            List<ColumnQBDto> Resultitems = new List<ColumnQBDto>();
            if (!Domain.Configuration.IsAdminOrAdOps )
            {
                 mappeditems = mappeditems.Where(M => M.ParentId != 24000 ).ToList();
                mappeditems = mappeditems.Where(M => M.Id != 24000).ToList();
               
            }

            if (!includeId)
            {
                foreach (var mappedItem in mappeditems)
                {
                    if (mappedItem.Name.ToLower() != "id" && mappedItem.Name.ToLower() != "name")
                        Resultitems.Add(mappedItem);
                }
            }
            else
            {
                Resultitems = mappeditems;


            }
            return Resultitems;
        }

        public List<MeasureDto> GetMeasuresByFactId(int Id)
        {
            List<Measure> columns = _FactRepository.Get(Id).Measures.Where(x => !x.IsHidden).ToList();
            if (!Configuration.IsAdmin)
            {
                //to be configured 
                columns = columns.Where(M => M.Id != 32).ToList();


            }
            List<MeasureDto> mappeditems = columns.Select(x => MapperHelper.Map<MeasureDto>(x)).ToList();
            return mappeditems;
        }
        public ColumnQBDto GetColumnByName(string Name)
        {
            var item = this._ColumnQBRepository.Query(x => x.Name == Name).SingleOrDefault();
            ColumnQBDto Fact = MapperHelper.Map<ColumnQBDto>(item);
            return Fact;
        }
        public MeasureDto GetMeasureByName(string Name)
        {
            var item = this._MeasureRepository.Query(x => x.Name == Name).SingleOrDefault();
            MeasureDto Fact = MapperHelper.Map<MeasureDto>(item);
            return Fact;
        }

        public MeasureDto GetMeasureByDealRequestMappings(string requestDealMap, int id)
        {
            MeasureDto Fact = null;

            var item = this._MeasureRepository.Query(x => x.requestsmapping == requestDealMap && x.dealsrequestsmapping == id.ToString()).SingleOrDefault();
            if (item != null)
            {
                Fact = MapperHelper.Map<MeasureDto>(item);
            }
            return Fact;
        }
        public string GetMeasureByDealRequestMappingsForRate(string requestDealMap)
        {
            MeasureDto Fact = null;

            var item = this._MeasureRepository.Query(x => x.requestsmapping == requestDealMap ).SingleOrDefault();
            if (item != null)
            {
                Fact = MapperHelper.Map<MeasureDto>(item);
                return Fact.Attribute;
            }
            return "sum(requests)";
        }
        public MeasureDto GetMeasureById(int Id)
        {
            var item = this._MeasureRepository.Get(Id);
            MeasureDto Fact = MapperHelper.Map<MeasureDto>(item);
            return Fact;
        }

        public ColumnQBDto GetColumnById(int Id)
        {
            var item = this._ColumnQBRepository.Get(Id);
            ColumnQBDto Fact = MapperHelper.Map<ColumnQBDto>(item);
            return Fact;
        }

        public DimensionDto GetDimensionById(int Id)
        {
            var item = this._DimensionRepository.Get(Id);
            DimensionDto Fact = MapperHelper.Map<DimensionDto>(item);


            return Fact;
        }


        public int GetMeasureCount(int Id)
        {
            var item = this._MeasureRepository.Query(x => x.ParentId == Id).Count();

            return item;
        }

        public int GetColumnCount(int Id)
        {
            var item = this._ColumnQBRepository.Query(x => x.ParentId == Id).Count();
            return item;
        }
        public List<ColumnQBDto> GetColumnsByParentId(int Id)
        {
            List<ColumnQB> columns = _ColumnQBRepository.Query(x => x.ParentId == Id).ToList();
            List<ColumnQBDto> mappeditems = columns.Select(x => MapperHelper.Map<ColumnQBDto>(x)).ToList();

            return mappeditems;
        }

        public List<StringBuilder> Execute(string query, ResultDataModelDto datamOdelDto)
        {

            query = query.Replace("TRUNC(", "(").Replace("trunc(", "(").Replace("ROUND(", "(").Replace("round(", "(").Replace("::float", "").Replace(",2R", "").Replace(" ,2R", "");

            IList<StringBuilder> rows = new List<StringBuilder>();
            if (string.IsNullOrEmpty(query))
            {
                Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Execute-{1}", "Query Builder", query);
                return rows.ToList();
            }
            try
            {
                Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Execute-{1}", "Report Builder-Export", query);
                string ConnectionName = "ReportingGPDB";
                string Connectionstring = "";
                if (System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName] != null)
                {
                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString))
                    {
                        Connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
                    }
                }
                using (NpgsqlConnection connection = new NpgsqlConnection(Connectionstring))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader dataReader = command.ExecuteReader())
                    {
                        var columns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();
                        rows.Add(new StringBuilder(string.Join("^", columns)));
                        while (dataReader.Read())
                        {
                            string[] row = new string[columns.Count()];
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                row[i] = dataReader.GetValue(i).ToString();
                            }
                            rows.Add(new StringBuilder(string.Join("^", row)));
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            
              var stResult = GetUniqueColumns(datamOdelDto, rows.ToList(), datamOdelDto.accountId);
            return stResult;

        }

        public ResultDataSetQBDto ExecuteWithPagination(string query, int pagenumber,  ResultDataModelDto dataCOnf, int pageSize = 100)
        {
            ResultDataSetQBDto resultSet = new ResultDataSetQBDto();
            IList<StringBuilder> rows = new List<StringBuilder>();

            try
            {
                query = query.Replace(",2R", ",2").Replace(" ,2R", " ,2");

                string ConnectionName = "ReportingGPDB";
                string Connectionstring = "";
                query = query.Replace("LIMIT 10;", "");
                string CountQuery = string.Format("SELECT Count(*) From ({0}) AS CountQueryResultSet", query);
                string PaginationQuery = string.Format("{0} LIMIT {1}  OFFSET  {2}", query, pageSize, pagenumber * pageSize);


                Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:ExecuteWithPagination-{1}", "Query Builder", PaginationQuery);
                if (System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName] != null)
                {
                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString))
                    {
                        Connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
                    }
                }
                using (NpgsqlConnection connection = new NpgsqlConnection(Connectionstring))
                {
                    connection.Open();

                    //if ((pagenumber + 1) == 1)
                    {
                        using (NpgsqlCommand commandCount = new NpgsqlCommand(CountQuery, connection))
                        {

                            resultSet.Count = Convert.ToInt64(commandCount.ExecuteScalar());
                        }
                    }

                    using (NpgsqlCommand command = new NpgsqlCommand(PaginationQuery, connection))
                    using (NpgsqlDataReader dataReader = command.ExecuteReader())
                    {
                        var columns = Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToList();

                        resultSet.Columns = columns;
                        rows.Add(new StringBuilder(string.Join("^", columns)));
                        resultSet.Rows = new List<IDictionary<string, object>>();

                        while (dataReader.Read())
                        {
                            string[] row = new string[columns.Count()];
                            IDictionary<string, object> rowDic = new Dictionary<string, object>();
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                row[i] = dataReader.GetValue(i).ToString();
                                rowDic.Add(columns[i], row[i]);
                            }
                            resultSet.Rows.Add(rowDic);
                            rows.Add(new StringBuilder(string.Join("^", row)));


                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            resultSet= GetUniqueColumns(dataCOnf, resultSet, dataCOnf.accountId);
            return resultSet;

        }

        public static string GenerateQueryScriptUnion(ReportGeneratorArgs args)
        {
            string selectStatmen = string.Empty;
            var oldToDate = args.Criteria.ToDate;
            var oldFromDate = args.Criteria.FromDate;
            var reportCommand = new StringBuilder();

            int countselect = 0;

            string groupByVar = string.Empty;

            List<DateTime> partialDates;
            switch (args.Criteria.SummaryBy)
            {


                case 2: //week
                    {
                        if ((args.Criteria.FromDate.DayOfWeek != DayOfWeek.Sunday) ||
                            (args.Criteria.ToDate.DayOfWeek != DayOfWeek.Saturday))
                        {
                            // Partial Dates
                            //get partial weeks dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                               out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}
                            countselect++;
                           string dateFilter= GenerateBetweenDates(tableInfo[args.DayFactStatTable],"dateid", SummaryBy.Day);
                            selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.DayFactStatTable).Replace("dateid", "dateid");




                            reportCommand.AppendLine(selectStatmen);
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }


                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Week);
                            List<int> weekno = new List<int>();
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Week, weekno);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.WeekFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 0)
                                    reportCommand.AppendLine(" union All ");
                                countselect++;
                                string dateFilter = GenerateBetweenDates(tableInfo[args.WeekFactStatTable], "weekid", SummaryBy.Week);
                                selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.WeekFactStatTable).Replace("fdateid", "fweekid").Replace("to_date(dateid", "to_date(weekid");


                               // selectStatmen = GenerateSelectForUnionCommand(tableInfo[args.WeekFactStatTable], SummaryBy.Week, args, ref groupByVar, null);
                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
                case 4:
                    {


                        if ((args.Criteria.FromDate.Day != 1) ||
                       (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}

                            countselect++;
                            string dateFilter = GenerateBetweenDates(tableInfo[args.DayFactStatTable], "dateid", SummaryBy.Day);

                            selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.DayFactStatTable).Replace("dateid", "dateid");


                            reportCommand.AppendLine(selectStatmen);
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;

                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;

                            //}

                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 0)
                                    reportCommand.AppendLine(" union All ");
                                string dateFilter = GenerateBetweenDates(tableInfo[args.MonthFactStatTable], "monthid", SummaryBy.Accumulated);

                                selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.MonthFactStatTable).Replace("fdateid", "fmonthid").Replace("to_date(dateid", "to_date(monthid");


                                countselect++;
                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
                case 3: //Month
                    {
                        if ((args.Criteria.FromDate.Day != 1) ||
                            (args.Criteria.ToDate.Month == (args.Criteria.ToDate.AddDays(1).Month)))
                        {
                            // Partial Dates
                            //get partial Month dates
                            DateTime newDateFrom, newDateTo;
                            partialDates = RepositoryScriptHelper.GetPartialDates(args.Criteria.FromDate, args.Criteria.ToDate, (SummaryBy)args.Criteria.SummaryBy,
                                                              out newDateFrom, out newDateTo);
                            if (!partialDates.Any())
                            {
                                // this should no happen
                                throw new Exception("should be partial dates");
                            }
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, partialDates, SummaryBy.Day);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.DayFactStatTable = tableName;

                            //}
                            countselect++;
                            string dateFilter = GenerateBetweenDates(tableInfo[args.DayFactStatTable], "dateid", SummaryBy.Day);
                            selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.DayFactStatTable).Replace("dateid", "dateid");

                            reportCommand.AppendLine(selectStatmen);
                            args.Criteria.FromDate = newDateFrom;
                            args.Criteria.ToDate = newDateTo;
                        }

                        if (args.Criteria.ToDate != args.Criteria.FromDate)
                        {
                            var dates = RepositoryScriptHelper.GetDates(args.Criteria.FromDate, args.Criteria.ToDate, SummaryBy.Month);
                            var tableInfo = RepositoryScriptHelper.GetDateTables(args, dates, SummaryBy.Month);
                            //foreach (var table in tableInfo)
                            //{
                            //    var tableName = table.Key;
                            //    var tableDays = table.Value;
                            //    args.MonthFactStatTable = tableName;

                            //}
                            if (tableInfo.Keys.Count > 0)
                            {
                                if (countselect > 0)
                                    reportCommand.AppendLine(" union All ");
                                countselect++;
                                string dateFilter = GenerateBetweenDates(tableInfo[args.MonthFactStatTable], "monthid", SummaryBy.Month);

                                selectStatmen = args.SubQueryStr.Replace("DATEREPL", dateFilter).Replace("fact_stat_d", args.MonthFactStatTable).Replace("fdateid", "fmonthid").Replace("to_date(dateid", "to_date(monthid");


                                reportCommand.AppendLine(selectStatmen);
                            }
                        }
                        break;
                    }
            }


            return reportCommand.ToString();

        }

        private static string GenerateBetweenDates(string dates, string dateField, SummaryBy summaryBy)
        {
            IList<int> datesNo = (dates ?? "").Split(',').Select<string, int>(int.Parse).ToList();
            datesNo = datesNo.OrderBy(M => M).ToList();
            IDictionary<int, int> dicgouping = new Dictionary<int, int>();
            int groupIndex = 0;
            int diffDay = 0;
            int IncreasedgroupIndex = 0;
            if (summaryBy == SummaryBy.Day)
            {
                groupIndex = 1;
                diffDay = 1;
                IncreasedgroupIndex = 1;
            }
            if (summaryBy == SummaryBy.Accumulated)
            {
                diffDay = 28;
                groupIndex = 100;
                IncreasedgroupIndex = 100;
            }
            if (summaryBy == SummaryBy.Week)
            {
                diffDay = 7;
                groupIndex = 7;
                IncreasedgroupIndex = 7;
            }
            if (summaryBy == SummaryBy.Month)
            {
                diffDay = 28;
                groupIndex = 100;
                IncreasedgroupIndex = 100;
            }

            dicgouping.Add(datesNo[0], IncreasedgroupIndex);

            DateTime firstDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime secondDate = DateTime.ParseExact(datesNo[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 1; i < datesNo.Count; i++)

            {
                firstDate = DateTime.ParseExact(datesNo[i - 1].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                secondDate = DateTime.ParseExact(datesNo[i].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                var diff = (secondDate - firstDate).TotalDays;

                if (diff == diffDay)

                {
                    dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                }
                else
                {
                    if (summaryBy == SummaryBy.Month || summaryBy == SummaryBy.Accumulated)
                    {
                        if (diffDay >= 28 && diffDay <= 31)
                        {

                            dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                        }
                        else
                        {
                            IncreasedgroupIndex = IncreasedgroupIndex + groupIndex;
                            dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                        }
                    }
                    else
                    {
                        IncreasedgroupIndex = IncreasedgroupIndex + groupIndex;
                        dicgouping.Add(datesNo[i], IncreasedgroupIndex);
                    }
                }

            }
            var results = dicgouping.GroupBy(n => n.Value).Select(g => g.Count() >= 2 ?
       dateField + " BETWEEN " + g.First().Key + " AND " + g.Last().Key
       :
          dateField + "=" + g.Select(x => x.Key).First()).ToList();


            //          List<string> items = datesNo
            //.Select((n, i) => new { number = n, group = n - i })
            //.GroupBy(n => n.group)
            //.Select(g =>
            //  g.Count() >= 2 ?
            //   "BETWEEN "+dateField + g.First().number + " AND " + g.Last().number
            //  :
            //    String.Join(", "+dateField+"=", g.Select(x => x.number))
            //)
            //   .ToList();

            return "(" + String.Join(" oR ", results) + ")";

            //  var  groupIndex = 1;
            //  foreach (var dateNo in datesNo)
            //  {

            //      dicgouping.Add(dateNo, groupIndex);



            //  }

            //return string.Empty;


        }


        #endregion


        #region  QueryBuilder Metod


        private List<int> FixTreeIds(List<int> ids, string type, bool includeId)
        {

            List<int> FixedIds = new List<int>();
            if (ids != null)
            {

                switch (type)
                {
                    case "1":
                        var CRoot = GetColumnByName("root");
                        foreach (var id in ids)
                        {
                            var node = GetColumnById(id);
                            int count = GetColumnCount(node.Id);

                            if (node.ParentId > 0 && (node.ParentId != CRoot.Id || count == 0) && count == 0)
                            {
                                FixedIds.Add(id);
                            }
                            else
                            {
                                var columns = GetColumnsByParentId(id);
                                var listOfIds = columns.Select(M => M.Id).ToList();
                                var intersction = listOfIds.Intersect(ids);
                                if (!(intersction.Count() > 0))
                                {

                                    var nameCol = columns.Where(M => M.Name.ToLower() == "name").SingleOrDefault();
                                    var idCol = columns.Where(M => M.Name.ToLower() == "id").SingleOrDefault();
                                    if (!includeId)
                                    {
                                        if (nameCol != null)
                                        {
                                            FixedIds.Add(nameCol.Id);
                                        }
                                    }
                                    else
                                    {
                                        if (nameCol != null)
                                        {
                                            FixedIds.Add(nameCol.Id);
                                        }
                                        if (idCol != null)
                                        {
                                            FixedIds.Add(idCol.Id);
                                        }
                                    }

                                    if ((nameCol == null) && node.Name.ToLower() == "date")
                                    {
                                        var IdCol = columns.Where(M => M.Name.ToLower() == "id").SingleOrDefault();

                                        if (IdCol != null)
                                        {
                                            FixedIds.Insert(0,IdCol.Id);
                                        }
                                    }

                                }
                            }
                        }

                        break;
                    case "2":
                        var MRoot = GetMeasureByName("Root");

                        foreach (var id in ids)
                        {
                            var node = GetMeasureById(id);
                            int childerCount = GetMeasureCount(node.Id);
                            if (node.ParentId > 0 && (node.ParentId != MRoot.Id || childerCount == 0) && childerCount == 0)
                            {
                                FixedIds.Add(id);
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
            return FixedIds;

        }
        public ResultDataModelDto FilterResult(DataModelDto data)
        {
            ResultDataModelDto result = new ResultDataModelDto();
            string Query = string.Empty;
            StringBuilder warnings = new StringBuilder();
            string queryFormat =
                "select\n{0}\nfrom({1})f\n{2} {3}";
            string subQuery = "\n select \n{0} \n  from {1} \n  where {2} \n  group by {3}\n";

            string subQueryNoGroup = "\n select \n {0} \n  from {1} \n  where {2} \n";
            string subQueryWithUnionAll = "\n select \n {0} \n  from ({1}) f1 \n  group by {2} \n";
            List<string> resultHeader = new List<string>();
            string orderBy = "order by \n  {0}";
            string subWhereQuery = "{0} in ({1})";
            string joinQuery = "LEFT JOIN {0} on f.{1} = {2}.{3} \n";
            string asjoinQuery = "LEFT JOIN {0} as {1} on f.{2} = {3}.{4} \n";
            string firstSelect = "";
            string secondSelect = "";
            List<string> secondSelectPartOne = new List<string>();
            List<string> secondSelectPartOneUnion = new List<string>();
            List<string> secondSelectPartTempOne = new List<string>();
            List<string> secondSelectPartTwo = new List<string>();


            List<string> secondSelectPartTempOneUnion = new List<string>();
            List<string> secondSelectPartTwoUnion = new List<string>(); 
            FactDto fact = GetFactById(data.fact);
            List<string> queryColumns = new List<string>();
            List<string> queryMeasures = new List<string>();
            StringBuilder subWhereQueries = new StringBuilder();
            StringBuilder CopysubWhereQueries = new StringBuilder();
            StringBuilder tempSubWhereQuery = new StringBuilder();
            StringBuilder joins = new StringBuilder();
            IList<int> notAllowedadv = new List<int>();
            bool customEvents = false;

            if (data.ColumnsIds.Contains(29020))
            {
                data.ColumnsIds.Remove(29019);
                data.ColumnsIds.Remove(29020);
                data.SummaryBy = (int)SummaryBy.Hour;
                if (data.to.Value > Framework.Utilities.Environment.GetServerTime().AddDays(7))
                {
                    data.to=Framework.Utilities.Environment.GetServerTime().AddDays(7);
                }
            }
            else if (data.ColumnsIds.Contains(29021))
            {
                data.ColumnsIds.Remove(29019);
                data.ColumnsIds.Remove(29021);
                data.SummaryBy = (int)SummaryBy.Day;

                if (data.to.Value > Framework.Utilities.Environment.GetServerTime().AddMonths(3))
                {
                    data.to = Framework.Utilities.Environment.GetServerTime().AddMonths(3);
                }
            }

            else if (data.ColumnsIds.Contains(29022))
            {
                data.ColumnsIds.Remove(29019);
                data.ColumnsIds.Remove(29022);
                data.SummaryBy = (int)SummaryBy.Week;

                if (data.to.Value > Framework.Utilities.Environment.GetServerTime().AddMonths(6))
                {
                    data.to = Framework.Utilities.Environment.GetServerTime().AddMonths(6);
                }
            }
            else if (data.ColumnsIds.Contains(29023))
            {
                data.ColumnsIds.Remove(29019);
                data.ColumnsIds.Remove(29023);
                data.SummaryBy = (int)SummaryBy.Month;
                if (data.to.Value > Framework.Utilities.Environment.GetServerTime().AddYears(1))
                {
                    data.to = Framework.Utilities.Environment.GetServerTime().AddYears(1);
                }
            }
            else if (data.SummaryBy == 0)
            {
                data.SummaryBy = (int)SummaryBy.Accumulated;
                if (data.to.Value > Framework.Utilities.Environment.GetServerTime().AddYears(1))
                {
                    data.to = Framework.Utilities.Environment.GetServerTime().AddYears(1);
                }
            }
            if (data.ColumnsIds.Contains(65) || data.ColumnsIds.Contains(66) || data.ColumnsIds.Contains(24000))
            {
                fact.Name = "fact_stat_fraud_d_vw";
            }
            if (data.ColumnsIds.Contains(29012) || data.ColumnsIds.Contains(29013) || data.ColumnsIds.Contains(29014) || data.ColumnsIds.Contains(29015) || data.ColumnsIds.Contains(29016))
            {
                fact.Name = "fact_stat_adv_seg_vw";
            }

            if (data.MeasuresIds.Contains(139))
            {
                customEvents = true;
                data.MeasuresIds.Remove(139);

            }
                if (Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().IsPrimaryUser)
            {

                notAllowedadv = this._AccountStatistic.GetNotAllowedAdvertiserAsscoiation(Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value, Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().UserId.Value);

            }
            if (data.SummaryBy != (int)SummaryBy.Accumulated && data.SummaryBy != (int)SummaryBy.Hour)
            {
                if (!data.ColumnsIds.Contains(63) && !data.ColumnsIds.Contains(24) && !data.ColumnsIds.Contains(64))
                {
                    //data.ColumnsIds.Add(24);
                    data.ColumnsIds.Insert(0, 24);
                }
            }
            else if (data.SummaryBy == (int)SummaryBy.Hour)
            {
                if (!data.ColumnsIds.Contains(63) && !data.ColumnsIds.Contains(24) && !data.ColumnsIds.Contains(64))
                {
                    data.ColumnsIds.Add(24);
                    if ((fact.Name != "fact_stat_fraud_d_vw" && fact.Name != "fact_stat_adv_seg_vw"))
                    {
                        data.ColumnsIds.Insert(0,64);
                    }
                    else
                    {
                        warnings.AppendLine("The selected Dimensions cannot provide Hour information");
                    }

                }

            }
     
            //Unique Clicks // Unique Impression
            if ((data.SummaryBy == 1 || data.SummaryBy == 4 || data.SummaryBy == 3) && (data.MeasuresIds.Contains(137) || data.MeasuresIds.Contains(138)))
            {
                result.isEnabledUniqueQuery = true;
                result.iscampUnique = true;
                //foreach (var datai in data.ColumnsIds)
                //{
                if (!(data.ColumnsIds.Where(M => M != 2000 && M != 1000 && M != 21000 && M != 29021 && M != 29023 && M != 29019 && M != 24).ToList().Count == 0))
                {
                    result.isEnabledUniqueQuery = false;
                }
                else
                {
                    if (data.SummaryBy == 1 && ((data.ColumnsIds.Where(M => M != 29021 && M != 24 && M != 29019).ToList().Count == 0)) && (data.MeasuresIds.Where(M => M != 137 && M != 138 && M != 132).ToList().Count == 0))
                    {
                        result.isEnabledUniqueQuery = false;
                        fact.Name = "fact_campaigns_unique_counters_d";
                    }

                    else if (data.SummaryBy == 3 && ((data.ColumnsIds.Where(M => M != 29023 && M != 24 && M != 29019).ToList().Count == 0)) && (data.MeasuresIds.Where(M => M != 137 && M != 138 && M != 132).ToList().Count == 0))
                    {
                        result.isEnabledUniqueQuery = false;
                        data.MeasuresIds.Remove(137);
                        data.MeasuresIds.Remove(138);

                    }
                   
                }
                    
                    //}

                




                if (result.isEnabledUniqueQuery)
                {
                    if (data.ColumnsIds.Contains(2000))
                    {
                        result.iscampUnique = false;

                    }

                    if ((data.MeasuresIds.Contains(137) && data.MeasuresIds.Contains(138)))
                    {
                        result.UniqueClicksImpressionNumb = 3;
                    }
                    else if (data.MeasuresIds.Contains(137))
                    {
                        result.UniqueClicksImpressionNumb =1;
                    }
                    else if (data.MeasuresIds.Contains(138))
                    {
                        result.UniqueClicksImpressionNumb = 2;
                    }
                    result.to = data.to;
                    result.from = data.from;
                    if (data.SummaryBy == 4)
                    {
                        result.uniquePeriod = 4;
                    }
                    else if(data.SummaryBy == 3)

                    {
                        result.uniquePeriod = 3;
                    }
                    else

                    {
                        result.uniquePeriod = 1;
                    }
                }

            }

            var originalColumn = data.ColumnsIds;


           


            #region Columns
            if (data.ColumnsIds != null)
            {
                data.ColumnsIds = FixTreeIds(data.ColumnsIds, "1", data.IncludeId);
                for (int i = 0; i < data.ColumnsIds.Count(); i++)
                {
                    var col = GetColumnById(data.ColumnsIds.ElementAt(i));
                    if (col.Id == 24 || col.Id == 63)
                    {
                        if (data.SummaryBy==(int)SummaryBy.Day)
                        {
                            var coltemp = GetColumnById(29021);
                            col.DisplayName = coltemp.DisplayName;
                        }
                        else if (data.SummaryBy == (int)SummaryBy.Week)
                        {
                            var coltemp = GetColumnById(29022);
                            col.DisplayName = coltemp.DisplayName;

                        }
                        else if (data.SummaryBy == (int)SummaryBy.Month)
                        {
                            var coltemp = GetColumnById(29023);
                            col.DisplayName = coltemp.DisplayName;

                        }
                    }
                    else if (col.Id==64)
                    {
                        var coltemp = GetColumnById(29020);
                        col.DisplayName = coltemp.DisplayName;

                    }


                    if (col != null)
                    {
                        string alias = col.IsSql && col.IsDuplicated ? "alias_" + col.TableName + "_" + col.FkSelector : col.TableName;
                        // queryColumns.Add("  " + alias + "." + col.Attribute + " as \"" + col.DisplayName + "\"");
                        resultHeader.Add(alias + "." + col.Attribute);
                        if (!string.IsNullOrEmpty(col.formatSQL))
                        {
                            queryColumns.Add(string.Format(col.formatSQL, "  " + alias + "." + col.Attribute) + " as \"" + col.DisplayName + "\"");
                        }
                        else
                        {
                            queryColumns.Add("  " + alias + "." + col.Attribute + " as \"" + col.DisplayName + "\"");

                        }
                        if (secondSelectPartOne.Where(x => x.ToLower().Trim() == col.FkSelector.ToLower().Trim()).Count() == 0)
                        {
                            secondSelectPartOne.Add("  " + col.FkSelector);

                            secondSelectPartOneUnion.Add("  " + col.FkSelector);

                            if (col.FkSelector == "dateid")
                            {


                                if (data.SummaryBy == (int)SummaryBy.Week)
                                {
                                    secondSelectPartTempOne.Add("   etl.date_get_weekid(to_date(dateid || '', 'YYYYMMDD'))" + "" + " as " + col.FkSelector);


                                    secondSelectPartTempOne.Add("   etl.date_get_weekid(to_date(dateid || '', 'YYYYMMDD'))" + "" + " as f" + col.FkSelector);
                                    secondSelectPartTempOneUnion.Add("   " + col.FkSelector);

                                  //  secondSelectPartTempOneUnion.Add("    f" + col.FkSelector);

                                    secondSelectPartOne.Remove("  " + col.FkSelector);
                                    secondSelectPartOne.Add("  f" + col.FkSelector);
                                }
                                else if (data.SummaryBy == (int)SummaryBy.Month)
                                {
                                    secondSelectPartTempOne.Add("   etl.date_get_monthid(to_date(dateid || '', 'YYYYMMDD'))" + "" + " as " + col.FkSelector);

                                    secondSelectPartTempOne.Add("   etl.date_get_monthid(to_date(dateid || '', 'YYYYMMDD'))" + "" + " as f" + col.FkSelector);

                                    secondSelectPartTempOneUnion.Add("   " + col.FkSelector);

                                   // secondSelectPartTempOneUnion.Add("    f" + col.FkSelector);


                                    secondSelectPartOne.Remove("  " + col.FkSelector);
                                    secondSelectPartOne.Add("  f" + col.FkSelector);
                                }
                                else if (data.SummaryBy == (int)SummaryBy.Hour && (fact.Name != "fact_stat_fraud_d_vw" && fact.Name != "fact_stat_adv_seg_vw"))
                                {
                                    secondSelectPartTempOne.Add(" " + col.FkSelector);
                                    secondSelectPartTempOne.Add("  HourId" + "" + " as Hour");
                                    secondSelectPartTempOneUnion.Add(" Hour  ");
                                    secondSelectPartTempOneUnion.Add("  " + col.FkSelector);
                                    if (fact.Name == "fact_stat_d")
                                        fact.Name = "fact_stat_h";
                                    else if (fact.Name == "fact_deals_stat_d")
                                        fact.Name = "fact_deals_stat_h";
                                }
                                else
                                {
                                    secondSelectPartTempOne.Add("  " + col.FkSelector);
                                    secondSelectPartTempOneUnion.Add("  " + col.FkSelector);
                                }
                            }
                            else if (col.FkSelector == "hourid")
                            {
                                if (fact.Name == "fact_stat_d")
                                    fact.Name = "fact_stat_h";
                                else if (fact.Name == "fact_deals_stat_d")
                                    fact.Name = "fact_deals_stat_h";
                                secondSelectPartTempOne.Add("  " + col.FkSelector);

                                secondSelectPartTempOneUnion.Add("  " + col.FkSelector);
                            }
                            else
                            {
                                /* if (col.FkSelector != "app_info_id_f")
                                 {
                                     secondSelectPartTempOne.Add("  " + col.FkSelector);
                                 }
                                 else
                                 {

                                     secondSelectPartTempOne.Add("  coalesce(sub.app_info_id, app.app_info_id) as  app_info_id_f  ");

                                     secondSelectPartOne.Remove("  " + col.FkSelector);
                                     secondSelectPartOne.Add("  app_info_id_f");

                                     subQuery = "\n select \n{0} \n  from {1} \n    LEFT JOIN sandbox.dim_appsites_t app ON (app.id = {1}.appsiteid) \n    LEFT JOIN sandbox.dim_sub_appsites_t sub ON(sub.id = {1}.subappsiteid)   \n  where {2}   \n group by {3}\n";
                                 } */

                                if (col.FkSelector != "subappsiteid")
                                {
                                    secondSelectPartTempOne.Add("  " + col.FkSelector);
                                    secondSelectPartTempOneUnion.Add("  " + col.FkSelector);
                                }
                                else
                                {

                                    secondSelectPartTempOne.Add("    CASE    WHEN  subappsiteid > 0 THEN  subappsiteid || '-subappsite'    ELSE appsiteid || '-appsite'  END as subappsiteid");

                                    secondSelectPartTempOneUnion.Add(" subappsiteid");

                                    secondSelectPartOne.Add("  appsiteid");

                                   // secondSelectPartOneUnion.Add("  appsiteid");
                                }
                            }


                        }
                        if (!joins.ToString().ToLower().Contains(alias))
                        {
                            if (col.IsSql)
                            {
                                if (col.IsDuplicated)
                                {
                                    joins.Append(string.Format(asjoinQuery, col.TableName, alias, col.FkSelector, alias, col.homeIdSelector));
                                }
                                else
                                {
                                    joins.Append(string.Format(joinQuery, string.IsNullOrEmpty(col.Source) ? col.TableName : col.Source, col.FkSelector, col.TableName, col.homeIdSelector));
                                }
                            }
                        }
                    }
                }
            }
            #endregion



            bool isQueryAccum = false;
            if (data.SummaryBy != (int)SummaryBy.Hour && data.SummaryBy != (int)SummaryBy.Day && fact.Name == "fact_stat_d" && data.SummaryBy==(int) SummaryBy.Accumulated)
            {
                isQueryAccum = true;
            }
                #region MeasuresSelect
                if (data.MeasuresIds != null)
            {
                data.MeasuresIds = FixTreeIds(data.MeasuresIds, "2", false);


                for (int i = 0; i < data.MeasuresIds.Count(); i++)
                {
                    var Measure = GetMeasureById(data.MeasuresIds.ElementAt(i));
                    if (Measure != null)
                    {
                        if (string.IsNullOrEmpty(Measure.requestsmapping))
                        {
                            

                            if ( !string.IsNullOrEmpty(Measure.RawAttribute))
                            {
                                queryMeasures.Add("  " + "" + Measure.SubstituteAttribute + " as \"" + Measure.DisplayName + "\"");


                                secondSelectPartTwo.Add("  " + Measure.RawAttribute + "  " );
                                secondSelectPartTwoUnion.Add("  " + Measure.Attribute + "  ");

                                getOrderByFromRawAttribute(Measure.Attribute, resultHeader);
                                //resultHeader.Add("f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));
                            

                            
                            }
                            else
                            {
                                queryMeasures.Add("  f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + Measure.DisplayName + "\"");


                                secondSelectPartTwo.Add("  " + Measure.Attribute + " as " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                secondSelectPartTwoUnion.Add("  sum(" + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                resultHeader.Add("f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));
                            }


                         
                        }
                        else
                        {
                            string filter = string.Empty;
                            List<int> colmnsAndCreativeDeal = new List<int> { 25000, 6000, 2000, 1000 };
                            List<int> colmnsDeal = new List<int> { 17000 };
                            foreach (var item in colmnsAndCreativeDeal)
                            {

                                if (originalColumn.Contains(item))
                                {

                                    filter = filter + "" + item + ",";
                                }
                                if (!string.IsNullOrEmpty(filter))
                                {
                                    break;
                                }
                            }

                            foreach (var item in colmnsDeal)
                            {

                                if (originalColumn.Contains(item))
                                {

                                    filter = filter + "" + item + ",";
                                }
                            }
                            if (Measure.Id != 105)
                            {
                                if (!string.IsNullOrEmpty(filter))
                                {

                                    filter = filter.Substring(0, filter.LastIndexOf(","));


                                    var messureS = GetMeasureByDealRequestMappings(filter, Measure.Id);

                                    if (messureS != null)
                                    {

                                    

                                        if ( !string.IsNullOrEmpty(messureS.RawAttribute))
                                        {
                                            queryMeasures.Add("  " + "" + messureS.SubstituteAttribute + " as \"" + messureS.DisplayName + "\"");


                                            secondSelectPartTwo.Add("  " + messureS.RawAttribute + "  ");
                                            secondSelectPartTwoUnion.Add("  " + messureS.Attribute + "  ");


                                            //resultHeader.Add("f" + "." + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                            getOrderByFromRawAttribute(messureS.Attribute, resultHeader);
                                        }
                                        else
                                        {
                                            queryMeasures.Add("  f" + "." + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + messureS.DisplayName + "\"");


                                            secondSelectPartTwo.Add("  " + messureS.Attribute + " as " + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));
                                            resultHeader.Add("f" + "." + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                            secondSelectPartTwoUnion.Add("  sum(" + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "")); 
                                        
                                        }



                                    }
                                    else
                                    {
                                        messureS = GetMeasureByDealRequestMappings(null, Measure.Id);
                                     

                                        if ( !string.IsNullOrEmpty(messureS.RawAttribute))
                                        {
                                            queryMeasures.Add("  " + "" + messureS.SubstituteAttribute + " as \"" + messureS.DisplayName + "\"");


                                            secondSelectPartTwo.Add("  " + messureS.RawAttribute + "  ");
                                            secondSelectPartTwoUnion.Add("  " + messureS.Attribute + "  ");
                                            getOrderByFromRawAttribute(messureS.Attribute, resultHeader);


                                        }
                                        else
                                        {
                                            queryMeasures.Add("  f" + "." + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + messureS.DisplayName + "\"");


                                            secondSelectPartTwo.Add("  " + messureS.Attribute + " as " + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                            resultHeader.Add("f" + "." + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                            secondSelectPartTwoUnion.Add("  sum(" + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + messureS.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        }



                                    }

                                }
                                else
                                {

                                   


                                    if ( !string.IsNullOrEmpty(Measure.RawAttribute))
                                    {
                                        queryMeasures.Add("  " + "" + Measure.SubstituteAttribute + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.RawAttribute + "  ");
                                        secondSelectPartTwoUnion.Add("  " + Measure.Attribute + "  ");

                                        getOrderByFromRawAttribute(Measure.Attribute, resultHeader);

                                    }
                                    else
                                    {
                                        queryMeasures.Add("  f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.Attribute + " as " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        resultHeader.Add("f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        secondSelectPartTwoUnion.Add("  sum(" + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                    }


                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(filter))
                                {

                                    filter = filter.Substring(0, filter.LastIndexOf(","));


                                    var messureS = GetMeasureByDealRequestMappingsForRate(filter);

                                    if ( !string.IsNullOrEmpty(Measure.RawAttribute))
                                    {
                                        queryMeasures.Add("  " + "" + Measure.SubstituteAttribute + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.RawAttribute.Replace("{ResponseByType}", messureS) + "  " );


                                        secondSelectPartTwoUnion.Add("  " + Measure.Attribute + "  ");
                                        getOrderByFromRawAttribute(Measure.Attribute, resultHeader);


                                    }
                                    else
                                    {
                                        queryMeasures.Add("  f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.Attribute.Replace("{ResponseByType}", messureS) + " as " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));
                                        resultHeader.Add("f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        secondSelectPartTwoUnion.Add("  sum(" + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));


                                    }


                                }
                                else

                                {
                                    if ( !string.IsNullOrEmpty(Measure.RawAttribute))
                                    {
                                        queryMeasures.Add("   " + "" + Measure.SubstituteAttribute + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.RawAttribute.Replace("{ResponseByType}", "sum(requests)") + "  "  ); 
                                        
                                        secondSelectPartTwoUnion.Add("  " + Measure.Attribute + "  ");


                                        getOrderByFromRawAttribute(Measure.Attribute, resultHeader);

                                    }
                                    else
                                    {
                                        queryMeasures.Add("  f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + Measure.DisplayName + "\"");


                                        secondSelectPartTwo.Add("  " + Measure.Attribute.Replace("{ResponseByType}", "sum(requests)") + " as " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        resultHeader.Add("f" + "." + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));

                                        secondSelectPartTwoUnion.Add("  sum(" + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", "") + ") as   " + Measure.DisplayName.Replace(" ", "").Replace("/", "").Replace("-", ""));




                                    }






                                }
                                   


                            }



                        }


                    }
                }
            }

            #endregion
           
            #region Querydata
            if (data.Querydata != null)
            {
                int QuerydataCount = data.Querydata.Count();
                int z = 0;
                foreach (KeyValuePair<string, string> entry in data.Querydata)
                {
                    var dim = GetDimensionById(Convert.ToInt32(entry.Key));
                    if (!string.IsNullOrEmpty(entry.Value))
                    {
                        if (dim != null)
                        {
                            string value = entry.Value;
                            if (dim.IsGrouped)
                            {
                                value = string.Format(dim.CustomGet, entry.Value);
                            }
                            if (z != QuerydataCount && z > 0)
                            {
                                tempSubWhereQuery.Append(" and ");
                            }
                            tempSubWhereQuery.Append(string.Format(subWhereQuery, dim.Selector, value));
                        }
                        z++;
                    }
                    else
                    {
                        warnings.Append(dim.Name + ", ");
                    }
                }
                if (tempSubWhereQuery.ToString() != "")
                {
                    subWhereQueries.Append(" ( ");
                    subWhereQueries.Append(tempSubWhereQuery);
                    subWhereQueries.Append(" ) ");

                    CopysubWhereQueries.Append(" ( ");
                    CopysubWhereQueries.Append(tempSubWhereQuery);
                    CopysubWhereQueries.Append(" ) ");
                    CopysubWhereQueries.Append(" and (DATEREPL) ");
                    subWhereQueries.Append(string.Format(" and (dateid  BETWEEN {1} AND {0}) ", long.Parse(data.to.Value.ToString("yyyyMMdd")), long.Parse(data.from.Value.ToString("yyyyMMdd"))));
                }
                else
                {
                    CopysubWhereQueries.Append("  (DATEREPL) ");
                    subWhereQueries.Append(string.Format(" (dateid  BETWEEN {1} AND {0}) ", long.Parse(data.to.Value.ToString("yyyyMMdd")), long.Parse(data.from.Value.ToString("yyyyMMdd"))));
                }
            }
            else
            {
                CopysubWhereQueries.Append("  (DATEREPL) ");
                subWhereQueries.Append(string.Format(" (dateid  BETWEEN {1} AND {0}) ", long.Parse(data.to.Value.ToString("yyyyMMdd")), long.Parse(data.from.Value.ToString("yyyyMMdd"))));

            }


            if (notAllowedadv != null && notAllowedadv.Count > 0)
            {
                var idsStr = notAllowedadv.Aggregate(string.Empty, (current, c) => current + ("," + c.ToString()));
                var resultids = idsStr.Trim(',');

                //instance.AdvertiserIdFieldName = "advassociationid";
                //instance.AdvertiserIdEqualFormat = " and " + instance.AdvertiserIdFieldName + " Not In (" + result + " ) ";




                if (!fact.Name.Contains("unique_counters_"))
                {

                    subWhereQueries.Append(string.Format(" and (advassociationid  Not In ({0})) ", resultids));
                    CopysubWhereQueries.Append(string.Format(" and (advassociationid  Not In ({0})) ", resultids));
                }

            }
          // data.accountId = 229472;
            result.accountId = data.accountId;
            if (data.accountId > 0 && (fact.Name.Contains("deal") || fact.Name.Contains("unique_counters_")  ))
               {
                   subWhereQueries.Append(string.Format(" and ( accountid  = {0} ) ", data.accountId));
                   CopysubWhereQueries.Append(string.Format(" and ( accountid  = {0} ) ", data.accountId));


               }
               
               else if (data.accountId > 0)
               {
                   subWhereQueries.Append(string.Format(" and ( advaccountid  = {0} ) ", data.accountId));

                   CopysubWhereQueries.Append(string.Format(" and ( advaccountid  = {0} ) ", data.accountId));

               }
            #endregion

            if (result.isEnabledUniqueQuery)
            {
                if (result.iscampUnique)
                {
                    queryMeasures.Add("  f" + ".campaignid" + " as \"UniqueKeyId" + "\"");
                }
                else
                {
                    queryMeasures.Add("  f" + ".adgroupid" + " as \"UniqueKeyId" + "\"");
                }

            }
            //Custom Events
            if (customEvents)
            {
               var QueryEvents = " select  distinct skeys(custom_events)  from " + fact.Name + "  where " + subWhereQueries;
                var customEventsCode = this._ReportRepository.GetResultAsString(QueryEvents,string.Empty,"Custom Events from fact");

                var EventsName = this._SummaryGPRepository.EventItemsList();
                if (customEventsCode!=null)
                {
                  var installIndex= customEventsCode.IndexOf("instll");
                    if(installIndex>-1)
                    customEventsCode.RemoveAt(installIndex);
                }
                foreach (var evCode in customEventsCode)
                {
                    if (EventsName.Where(M => M.Code == evCode).SingleOrDefault()!=null)
                    {
                        queryMeasures.Add("  f" + "." + EventsName.Where(M => M.Code == evCode).Single().Name.Replace(" ", "").Replace("/", "").Replace("-", "") + " as \"" + EventsName.Where(M => M.Code == evCode).Single().Name + "\"");

                        var eventName= EventsName.Where(M => M.Code == evCode).Single().Name.Replace(" ", "").Replace("/", "").Replace("-", "");
                        secondSelectPartTwo.Add("  sum(coalesce((custom_events->'" + evCode + "')::bigint, 0))" + " as " + eventName);
                     
                        secondSelectPartTwoUnion.Add("  sum("+ eventName  + ")" + " as " + eventName );
 
                        resultHeader.Add("f" + "." + EventsName.Where(M => M.Code == evCode).Single().Name.Replace(" ", "").Replace("/", "").Replace("-", ""));
                    }


                }
            }

            List<string> headers = queryColumns.Concat(queryMeasures).ToList();
            List<string> subHeaders = secondSelectPartTempOne.Concat(secondSelectPartTwo).ToList();
            List<string> subHeadersUnion = secondSelectPartTempOneUnion.Concat(secondSelectPartTwoUnion).ToList();

            firstSelect = string.Join(",\n", headers);
            secondSelect = string.Join(",\n", subHeaders);
            Query = string.Empty;
            if (data.ColumnsIds.Count() == 0 && data.MeasuresIds.Count() == 0)
            {
                Query = string.Format("select * \n from \n ( \n \t select * \n \t from {0} \n \t where{1} \n )f", "public." + fact.Name, subWhereQueries);
                /* if (data.function != Function.Query)
                 {
                     Execute(Query);
                 }*/

                result.Query = Query;
                result.Warnings = warnings;
                return result;
            }
            else
            {
                string CopysubQuery = string.Empty;
                string CopysubQueryUnion = string.Empty;
                string tempQu = string.Empty;
                if (secondSelectPartOne.Count() == 0)
                { tempQu = subQueryNoGroup;
                    subQuery = string.Format(subQueryNoGroup, secondSelect, fact.Name, subWhereQueries);
                    
                    CopysubQuery = string.Format(tempQu, secondSelect, fact.Name, CopysubWhereQueries);

                  //  CopysubQueryUnion = string.Format(tempQu, string.Join(",\n", subHeadersUnion), fact.Name, CopysubWhereQueries);
                }
                else
                {

                tempQu = subQuery;
                subQuery = string.Format(subQuery, secondSelect, fact.Name, subWhereQueries, string.Join(",", secondSelectPartOne));

                    CopysubQuery = string.Format(tempQu, secondSelect, fact.Name, CopysubWhereQueries, string.Join(",", secondSelectPartOne));
                  


                }
                if (!string.IsNullOrWhiteSpace(string.Join(",\n  ", resultHeader)))
                {
                    orderBy = string.Format(orderBy, string.Join(",\n  ", resultHeader));
                    orderBy= orderBy.Replace("f.dateid", "f.dateid  DESC"); 
                }
                else
                    orderBy = string.Empty;

                if (data.SummaryBy!=(int)SummaryBy.Hour && data.SummaryBy != (int)SummaryBy.Day && fact.Name== "fact_stat_d")
                {
                    ReportGeneratorArgs args = new ReportGeneratorArgs();
                    args.WeekFactStatTable = "fact_stat_w";
                    args.DayFactStatTable = "fact_stat_d";
                    args.MonthFactStatTable = "fact_stat_m";
                    args.SubQueryStr = "(" + CopysubQuery + ")";
                    args.Criteria = new ReportCriteriaDto();
                    args.Criteria.SummaryBy = data.SummaryBy;
                    args.Criteria.FromDate = data.from.Value;
                    args.Criteria.ToDate = data.to.Value;
                  ;
                    var resultsunionall = GenerateQueryScriptUnion(args);
                    CopysubQueryUnion = resultsunionall;
                    if (isQueryAccum && resultsunionall.Contains("union") )
                    {
                        CopysubQueryUnion = string.Format(subQueryWithUnionAll, string.Join(",\n", subHeadersUnion), resultsunionall, string.Join(",", secondSelectPartOneUnion));
 
                        
                    }

                    queryFormat = string.Format(queryFormat, firstSelect, CopysubQueryUnion, joins, orderBy);
                    Query = queryFormat;
                    result.Query = Query;
                    result.Warnings = warnings;
                    return result;
                }
                queryFormat = string.Format(queryFormat, firstSelect, subQuery, joins, orderBy);
                Query = queryFormat;
                result.Query = Query;
                result.Warnings = warnings;
                return result;
                /* if (data.function != Function.Query)
                 {
                     Execute(queryFormat);
                 }*/
            }



            // result.Warnings = warnings;
            // return result;
        }

 
        public List<StringBuilder> GetUniqueColumns(ResultDataModelDto dataConf , List<StringBuilder>  results,int accountid)
        {
            if (results.Count >1 && dataConf.isEnabledUniqueQuery)
            {
                var arrString= results[0].ToString().Split('^');
                var uniqueIndex = arrString.Length - 1;
                int indexforimpression = -1;
                int indexforClicks = -1;
               var accountId= accountid;
                for (var i=0; i< arrString.Length; i++)
                {
                    if (arrString[i] == "Unique Impressions")
                        indexforimpression = i;
                    if (arrString[i] == "Unique Clicks")
                        indexforClicks = i;
                }
                results[0] = results[0].Replace("^UniqueKeyId", string.Empty);
                string Ids = string.Empty;
                IList<int> idsInt = new List<int>();

                for (var c = 1; c < results.Count; c++)
                {
                    var arrStringtemp = results[c].ToString().Split('^');
                    //if (!Ids.Contains(arrStringtemp[uniqueIndex]))
                    //    Ids = arrStringtemp[uniqueIndex] + "," + Ids;

                    if (!idsInt.Contains(Convert.ToInt32(arrStringtemp[uniqueIndex])))
                    {
                        // Ids = arrStringtemp + "," + Ids;

                        idsInt.Add(Convert.ToInt32(arrStringtemp[uniqueIndex]));
                    }
                }
            

                AdvertisorEstimatorCalculation test = new AdvertisorEstimatorCalculation(dataConf.from.Value, dataConf.to.Value, dataConf.uniquePeriod==4 ? EstimatorCalculationPeriodType.Accumulated: dataConf.uniquePeriod == 3 ? EstimatorCalculationPeriodType.Month : EstimatorCalculationPeriodType.Day, dataConf.iscampUnique?EstimatorCalculationType.Campaign: EstimatorCalculationType.AdGroup, accountId);
                IList<CampaignCardinalityEstimatorDto> res = null;

                string Idtemps = string.Join(",", idsInt.Select(x => x).ToList());

                if (!string.IsNullOrEmpty(Idtemps))
                res = test.GetCardinalityEsitimator(Idtemps, true);

                if (res == null )
                {
                    res = new List<CampaignCardinalityEstimatorDto>();
                }


                for (var c = 1; c < results.Count; c++)
                {

                    var arrStringtemp = results[c].ToString().Split('^');

                    var uniqueId = arrStringtemp[uniqueIndex];


                        if ((dataConf.UniqueClicksImpressionNumb==3 || dataConf.UniqueClicksImpressionNumb==1)&& indexforimpression>-1)
                        {
                          arrStringtemp[indexforimpression] =res.Where(z => z.AdGroupId ==Convert.ToInt32(uniqueId)).Select(x => x.unique_impressions).FirstOrDefault().ToString();
                        }
                        if ((dataConf.UniqueClicksImpressionNumb == 3 || dataConf.UniqueClicksImpressionNumb == 2)&& indexforClicks>-1)
                        {
                            arrStringtemp[indexforClicks] = res.Where(z => z.AdGroupId == Convert.ToInt32(uniqueId)).Select(x => x.unique_clicks).FirstOrDefault().ToString(); ;
                        }
                          var temparrList=  arrStringtemp.ToList();

                   temparrList.RemoveAt(uniqueIndex);

                    results[c] =new StringBuilder(string.Join("^", temparrList.ToArray()));
                  

                    }
                
            }
            else
            {

                results[0] = results[0].Replace("^UniqueKeyId", string.Empty);
            }
            return results;

        }

        public ResultDataSetQBDto GetUniqueColumns(ResultDataModelDto dataConf, ResultDataSetQBDto results,int accountid)
        {
            if (results.Rows.Count >= 1 && dataConf.isEnabledUniqueQuery)
            {
             
                var uniqueIndex = results.Columns.IndexOf("UniqueKeyId");
                int indexforimpression = results.Columns.IndexOf("Unique Impressions"); ;
                int indexforClicks = results.Columns.IndexOf("Unique Clicks"); ;
                var accountId = accountid;
               
          
                string Ids = string.Empty;
                IList<int> idsInt = new List<int>();


                for (var c = 0; c < results.Rows.Count; c++)
                {
                    var arrStringtemp = results.Rows[c]["UniqueKeyId"].ToString();
                    if (!idsInt.Contains(Convert.ToInt32(arrStringtemp)))
                    {
                       // Ids = arrStringtemp + "," + Ids;

                        idsInt.Add(Convert.ToInt32(arrStringtemp));
                    }
                }


                AdvertisorEstimatorCalculation test = new AdvertisorEstimatorCalculation(dataConf.from.Value, dataConf.to.Value, dataConf.uniquePeriod == 4 ? EstimatorCalculationPeriodType.Accumulated : dataConf.uniquePeriod == 3? EstimatorCalculationPeriodType.Month:   EstimatorCalculationPeriodType.Day, dataConf.iscampUnique ? EstimatorCalculationType.Campaign : EstimatorCalculationType.AdGroup, accountId);
                IList<CampaignCardinalityEstimatorDto> res = null;
               string  Idtemps = string.Join(",", idsInt.Select(x => x).ToList());

                if (!string.IsNullOrEmpty(Idtemps))
                    res= test.GetCardinalityEsitimator(Idtemps, true);
                if (res == null)
                {
                    res = new List<CampaignCardinalityEstimatorDto>();
                }


                for (var c = 0; c < results.Rows.Count; c++)
                {

                    var uniqueId = results.Rows[c]["UniqueKeyId"].ToString();

               


                    if ((dataConf.UniqueClicksImpressionNumb == 3 || dataConf.UniqueClicksImpressionNumb == 1) && indexforimpression > -1)
                    {
                        results.Rows[c]["Unique Impressions"] = res.Where(z => z.AdGroupId == Convert.ToInt32(uniqueId)).Select(x => x.unique_impressions).FirstOrDefault().ToString();
                    }
                    if ((dataConf.UniqueClicksImpressionNumb == 3 || dataConf.UniqueClicksImpressionNumb == 2) && indexforClicks > -1)
                    {
                        results.Rows[c]["Unique Clicks"] = res.Where(z => z.AdGroupId == Convert.ToInt32(uniqueId)).Select(x => x.unique_clicks).FirstOrDefault().ToString(); ;
                    }
                  

                    results.Rows[c].Remove("UniqueKeyId");

                }
                results.Columns.Remove("UniqueKeyId");

            }
            else
            {
               results.Columns.Remove("UniqueKeyId");
              
            }
            return results;

        }



        
        public List<StringBuilder> ExecuteAndFilterResult(DataModelDto data)
        {
            data.Querydata = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.QueryJsonData);
            var queryString = FilterResult(data);

           var results=  Execute(queryString.Query, queryString);
            return results;
        }

        public void getOrderByFromRawAttribute(string rawAtribute, List<string> ordr)
        {

            string[] rawAtributeResults = { rawAtribute };
            if (rawAtribute.Contains(","))
            {
                 rawAtributeResults = rawAtribute.Split(new char[] { ',' });
              


            }
            foreach (var rawAtributeResult in rawAtributeResults)
            {
                if (!string.IsNullOrWhiteSpace(rawAtributeResult))
                {
                    var indexlast = rawAtributeResult.LastIndexOf("as ");

                    if(indexlast>0)
                    ordr.Add(" f."+rawAtributeResult.Substring(indexlast + 3).Replace(" ", ""));

                }
                }
        }
        #endregion

    }
}
