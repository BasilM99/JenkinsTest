using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Persistence.Reports.Repositories;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using NHibernate;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Repositories.Reports
{
    public class ReportRepository : BaseReportRepository, IReportRepository
    {
        
        public ReportRepository(RepositoryImplBase<ChartDto, int> repository, Noqoush.Framework.ConfigurationSetting.IConfigurationManager configurationManager)
            : base(repository, configurationManager)
        {

        }

       
        #region Dashboard

        public List<ChartDto> GetAppSiteChart(DashboardChartCriteria criteria, int accountId)
        {
            var criteriaDto = GetReportCriteriaDto(criteria);
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        public List<AppSitePerformanceDto> GetAppSitePerformance(DashboardPerformanceCriteria criteria, int accountId)
        {
            var criteriaDto = GetReportCriteriaDto(criteria);
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.None);
            var items = GetAppReportResult(args, string.Empty);
            return GetAppPerformanceResult(items);

        }

        public int GetTotalAppSitePerformance(DashboardPerformanceCriteria criteria, int accountId)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query = nhibernateSession.CreateSQLQuery("call sp_GetAppStatPerformanceCount(:AccountID,:FromDate,:ToDate)");
            query.SetTimeout(120);
            query.SetDateTime("FromDate", criteria.FromDate);
            query.SetDateTime("ToDate", criteria.ToDate);
            query.SetInt32("AccountID", accountId);
            return (int)query.UniqueResult<long>();

        }

        public List<AppSiteGeoLocationDto> GetAppSiteGeoLocation(DashboardGeoLocationCriteria criteria, int accountId)
        {
            var criteriaDto = GetReportCriteriaDto(criteria);
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.GeoLocation);
            var items = GetAppReportResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"));
            return GetReportAppSiteLocationResult(items);
        }


        public List<AdGeoLocationDto> GetAdGeoLocation(DashboardGeoLocationCriteria criteria, int accountId)
        {
            var criteriaDto = GetReportCriteriaDto(criteria);

            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.GeoLocation);
            var items = GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"));
            return GetReportCampaignLocationResult(items);

        }

        public List<ChartDto> GetAdChart(DashboardChartCriteria criteria, int accountId)
        {

            var criteriaDto = GetReportCriteriaDto(criteria);
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.None);
            return GetChartResult(args, string.Empty);
        }



        public List<AdPerformanceDto> GetAdPerformance(DashboardPerformanceCriteria criteria, int accountId)
        {
            var criteriaDto = GetReportCriteriaDto(criteria);
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.None);
            var items = GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"));
            return GetAdPerformanceResult(items);
        }

        #endregion

        #region Campaign Reports
        public List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.None);
            return GetReportCampaignResult(args, string.Empty);

        }

        public List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.AdGroup, SubType.None);
            return GetReportCampaignResult(args, string.Empty);

        }

        public List<CampaignCommonReportDto> GetAdReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Ad, SubType.None);
            return GetReportCampaignResult(args, string.Empty);

        }

        public List<CampaignCommonReportDto> GetCampaignPlatformReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Platform);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Platform", "Undefined"));
        }

        public List<CampaignCommonReportDto> GetCampaignManuFactorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Manufacturer);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Manufacturer", "Undefined"));
        }

        public List<CampaignCommonReportDto> GetCampaignOperatorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Operator);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Operator", "Undefined"));
        }

        public List<CampaignCommonReportDto> GetCampaignGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.GeoLocation);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"));
        }

        #region Chart
        public List<ChartDto> GetCampaignReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAdGroupReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.AdGroup, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAdReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Ad, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetCampaignOperatorReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.Operator);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetCampaignGeoLocationReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.GeoLocation);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetCampaignPlatformReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.Platform);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetCampaignManuFactorReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {

            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.Manufacturer);
            return GetChartResult(args, string.Empty);

        }
        #endregion
        #endregion

        #region App Reports

        public List<AppEmailReportDto> GetAppEmailReport(EmailReportCriteriaDto criteriaDto)
        {
            throw new NotImplementedException();
        }

        public List<AppCommonReportDto> GetAppReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.None);
            return GetAppReportResult(args, string.Empty);
        }

        public List<AppCommonReportDto> GetAppOperatorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.Operator);
            return GetAppReportResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Operator", "Undefined"));
        }

        public List<AppCommonReportDto> GetAppPlatformReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.Platform);
            return GetAppReportResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Platform", "Undefined"));
        }

        public List<AppCommonReportDto> GetAppManuFactorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.Manufacturer);
            return GetAppReportResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Manufacturer", "Undefined"));
        }

        public List<AppCommonReportDto> GetAppGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.App, SubType.GeoLocation);
            return GetAppReportResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"));
        }


        public List<ChartDto> GetAppReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
       
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAppOperatorReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.Operator);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAppPlatformReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.Platform);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAppManuFactorReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.Manufacturer);
            return GetChartResult(args, string.Empty);
        }

        public List<ChartDto> GetAppGeoLocationReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.App, SubType.GeoLocation);
            return GetChartResult(args, string.Empty);

        }

        #endregion

        #region API

        public List<AppSiteStatisticsReport> GetAppSiteStatisticsReport(AppSiteStatisticsCriteriaDto criteriaDto)
        {
            var reportCriteria = GetReportCriteriaDto(criteriaDto);
            var args = ReportGeneratorArgs.GetInstance(reportCriteria, criteriaDto.AccountId, ReportType.Report, EntityType.API, SubType.None);

            var result = GetAppStatistcsReportResult(args);

            return result;
        }

        public List<AppSiteStatisticsGeoReport> GetAppSiteGeoStatisticsReport(AppSiteStatisticsCriteriaDto criteriaDto)
        {
            var reportCriteria = GetReportCriteriaDto(criteriaDto);
            var args = ReportGeneratorArgs.GetInstance(reportCriteria, criteriaDto.AccountId, ReportType.Report, EntityType.API, SubType.GeoLocation);

            var result = GetAppStatistcsGeoReportResult(args);

            return result;
        }

        #endregion

        #region Helpers

        private List<AppCommonReportDto> GetAppReportResult(ReportGeneratorArgs args, string undefinedText)
        {
          
            var script = ScriptGenerator.GenerateQueryScript(args);
            var items = GetResult<AppCommonReportDto>(script).ToList();
            foreach (var appReportDto in items)
            {
                if (string.IsNullOrWhiteSpace(appReportDto.Name))
                {
                    appReportDto.Name = undefinedText;
                }
                if (!args.IsAccumulated)
                appReportDto.DateRange = GetDateRange(getDate(appReportDto.Date, appReportDto.TimeId), args.Criteria);
            }
            return items;
        }

        private List<CampaignCommonReportDto> GetReportCampaignResult(ReportGeneratorArgs args, string undefinedText)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            //Noqoush.Framework.ApplicationContext.Instance.Logger.Debug(script);
            var items = GetResult<CampaignCommonReportDto>(script).ToList();
            foreach (var appReportDto in items)
            {
                if (string.IsNullOrWhiteSpace(appReportDto.Name))
                {
                    appReportDto.Name = undefinedText;
                }
                if(!args.IsAccumulated)
                appReportDto.DateRange = GetDateRange(getDate(appReportDto.Date, appReportDto.TimeId), args.Criteria);
            }

            return items;

        }

        private static List<AdGeoLocationDto> GetReportCampaignLocationResult(List<CampaignCommonReportDto> items)
        {
            return items.Select(campaignCommonReportDto => new AdGeoLocationDto
            {
                CampaignName = campaignCommonReportDto.SubName,
                CountryName = campaignCommonReportDto.Name,
                AvgCPC = campaignCommonReportDto.AvgCPC,
                CTR = campaignCommonReportDto.CTR,
                Clicks = campaignCommonReportDto.Clicks,
                Impress = campaignCommonReportDto.Impress,
                Spend = campaignCommonReportDto.Spend,
                BillableCost = campaignCommonReportDto.BillableCost,
                AdjustedNetCost = campaignCommonReportDto.AdjustedNetCost,
                DataFee = campaignCommonReportDto.DataFee,
                NetCost= campaignCommonReportDto.NetCost,
                AgencyRevenue = campaignCommonReportDto.AgencyRevenue,
                GrossCost = campaignCommonReportDto.GrossCost,
                TotalCount = campaignCommonReportDto.TotalCount
            }).ToList();
        }
        private static List<AppSiteGeoLocationDto> GetReportAppSiteLocationResult(List<AppCommonReportDto> items)
        {
            return items.Select(appSiteCommonReportDto => new AppSiteGeoLocationDto
            {
                AppSiteName = appSiteCommonReportDto.SubName,
                CountryName = appSiteCommonReportDto.Name,
                AdRequests = appSiteCommonReportDto.AdRequests,
                CTR = appSiteCommonReportDto.CTR,
                AdClicks = appSiteCommonReportDto.Clicks,
                AdImpress = appSiteCommonReportDto.AdImpress,
                FillRate = appSiteCommonReportDto.FillRate,
                Revenue = appSiteCommonReportDto.Revenue,
                eCPM = appSiteCommonReportDto.eCPM,
                TotalCount = (long)appSiteCommonReportDto.TotalCount
            }).ToList();
        }
        private static List<AdPerformanceDto> GetAdPerformanceResult(List<CampaignCommonReportDto> items)
        {
            return items.Select(campaignCommonReportDto => new AdPerformanceDto
            {
                CampaignName = campaignCommonReportDto.SubName,
                AvgCPC = campaignCommonReportDto.AvgCPC,
                CTR = campaignCommonReportDto.CTR,
                Clicks = campaignCommonReportDto.Clicks,
                Impress = campaignCommonReportDto.Impress,
                Spend = campaignCommonReportDto.Spend,
                BillableCost = campaignCommonReportDto.BillableCost,
                AdjustedNetCost = campaignCommonReportDto.AdjustedNetCost,
                DataFee = campaignCommonReportDto.DataFee,
                NetCost = campaignCommonReportDto.NetCost,
                AgencyRevenue = campaignCommonReportDto.AgencyRevenue,
                GrossCost = campaignCommonReportDto.GrossCost,
                TotalCount = campaignCommonReportDto.TotalCount
            }).ToList();
        }
        private static List<AppSitePerformanceDto> GetAppPerformanceResult(List<AppCommonReportDto> items)
        {
            return items.Select(appReportDto => new AppSitePerformanceDto
            {
                AppSiteName = appReportDto.SubName ?? appReportDto.Name,
                AdClicks = appReportDto.Clicks,
                AdImpress = appReportDto.AdImpress,
                AdRequests = appReportDto.AdRequests,
                CTR = appReportDto.CTR,
                FillRate = appReportDto.FillRate,
                Revenue = appReportDto.Revenue,
                eCPM = appReportDto.eCPM,
                TotalCount = (long)appReportDto.TotalCount
            }).ToList();
        }


        private List<AppSiteStatisticsReport> GetAppStatistcsReportResult(ReportGeneratorArgs args)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            var items = GetResult<AppSiteStatisticsReport>(script).ToList();

            return items;
        }

        private List<AppSiteStatisticsGeoReport> GetAppStatistcsGeoReportResult(ReportGeneratorArgs args)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            var items = GetResult<AppSiteStatisticsGeoReport>(script).ToList();

            return items;
        }


        private static ReportCriteriaDto GetReportCriteriaDto(AppSiteStatisticsCriteriaDto criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                PageNumber = criteria.PageNumber,
                ItemsPerPage = criteria.ItemsPerPage,
                ItemsList = criteria.ItemsList,
                AdvancedCriteria = criteria.AdvancedCriteria,
                Layout = "Detailed",
                SummaryBy = criteria.SummaryBy,
                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser

            };
            return criteriaDto;
        }

        private static ReportCriteriaDto GetReportCriteriaDto(DashboardGeoLocationCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                AdvertiserId = criteria.AdvertiserId,
                PageNumber = criteria.PageNumber,
                ItemsPerPage = criteria.ItemsPerPage,
                OrderColumn = criteria.OrderColumn,
                OrderType = criteria.OrderType,
                ItemsList = criteria.IdFilter.HasValue ? criteria.IdFilter.Value.ToString() : string.Empty,
                AdvancedCriteria = criteria.CountryId.HasValue ? criteria.CountryId.Value.ToString() : string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,
                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser
            };
            return criteriaDto;
        }
        private static ReportCriteriaDto GetReportCriteriaDto(DashboardPerformanceCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                AdvertiserId = criteria.AdvertiserId,
                PageNumber = criteria.PageNumber,
                ItemsPerPage = criteria.ItemsPerPage,
                OrderColumn = criteria.OrderColumn,
                OrderType = criteria.OrderType,
                ItemsList = string.Empty,
                AdvancedCriteria = string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,
                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser
            };
            return criteriaDto;
        }
        private static ReportCriteriaDto GetReportCriteriaDto(DashboardChartCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                AdvertiserId = criteria.AdvertiserId,
                MetricCode = criteria.MetricCode,
                ItemsList = criteria.IdFilter.HasValue ? criteria.IdFilter.Value.ToString() : string.Empty,
                AdvancedCriteria = string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,

                    userId = criteria.userId,
                IsPrimaryUser =criteria.IsPrimaryUser
            };
            return criteriaDto;
        }

        private List<ChartDto> GetChartResult(ReportGeneratorArgs args, string undefinedText)
        {
            var script = ScriptGenerator.GenerateChartSelectScript(args);
            var items = GetResult<ChartDto>(script).ToList();
            return items;
        }
        
        #endregion
    }
}
