using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.AdServer.Integration.Services.Interfaces;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using NHibernate;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Persistence.ReportsGP.CardinalityEstimator;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Persistence.ReportsGP.MinHashEstimator;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Persistence.RepositoriesGP.Reports
{
    public class ReportGPRepository : BaseReportGPRepository, IReportGPRepository
    {
        private string AVReportingGPA = JsonConfigurationManager.AppSettings["ReportingGP"];

        private IAudienceSegmentRepository AudienceSegmentRepository = IoC.Instance.Resolve<IAudienceSegmentRepository>();

        private IBusinessPartnerRepository BusinessPartnerRepository = IoC.Instance.Resolve<IBusinessPartnerRepository>();
        private ICampaignRepository CampaignRepository = IoC.Instance.Resolve<ICampaignRepository>();
        public ReportGPRepository(RepositoryImplBase<ChartDto, int> repository, ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager)
            : base(repository, configurationManager)
        {

        }

        public List<string> GetResultAsString(string script, string optoionalDrop = "", string nameofMethod = "")
        {
            var items = GetResult<string>(script, optoionalDrop, nameofMethod).ToList();
            if (items != null && items.Count > 0)
                return items;
            else
                return new List<string>();

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
            var items = GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"), "GetAdGeoLocation");
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
            var items = GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"), "GetAdGeoLocation");
            return GetAdPerformanceResult(items);
        }

        #endregion

        #region Campaign Reports
        public List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.None);

            var result = GetReportCampaignResult(args, string.Empty, "GetCampaignReport");
            if (result != null && result.Count() > 0)
            {
                if ((criteriaDto.SummaryBy == 1 || criteriaDto.SummaryBy == 4 || criteriaDto.SummaryBy == 3) && criteriaDto.Layout == "detailed" && !criteriaDto.GroupByName)
                {
                    string Ids = String.Join(",", result.Select(x => x.Id));

                    AdvertisorEstimatorCalculation test = new AdvertisorEstimatorCalculation(criteriaDto.RFromDate, criteriaDto.RToDate, criteriaDto.SummaryBy == 4 ? EstimatorCalculationPeriodType.Accumulated : EstimatorCalculationPeriodType.Day, EstimatorCalculationType.Campaign, accountId);
                    IList<CampaignCardinalityEstimatorDto> res = test.GetCardinalityEsitimator(Ids);
                    if (res != null && res.Count() > 0)
                    {
                        foreach (CampaignCommonReportDto item in result)
                        {
                            item.UniqueClicks = res.Where(z => z.CampaignId == item.Id).Select(x => x.unique_clicks).FirstOrDefault();
                            item.UniqueImp = res.Where(z => z.CampaignId == item.Id).Select(x => x.unique_impressions).FirstOrDefault();

                        }
                    }
                }
            }

            return result;
        }

        public List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.AdGroup, SubType.None);
            var result = GetReportCampaignResult(args, string.Empty, "GetAdGroupReport");
            if (result != null && result.Count() > 0)
            {
                if ((criteriaDto.SummaryBy == 1 || criteriaDto.SummaryBy == 4 || criteriaDto.SummaryBy == 3) && criteriaDto.Layout == "detailed" && !criteriaDto.GroupByName)
                {
                    string Ids = String.Join(",", result.Select(x => x.Id));

                    AdvertisorEstimatorCalculation test = new AdvertisorEstimatorCalculation(criteriaDto.RFromDate, criteriaDto.RToDate, criteriaDto.SummaryBy == 4 ? EstimatorCalculationPeriodType.Accumulated : EstimatorCalculationPeriodType.Day, EstimatorCalculationType.AdGroup, accountId);
                    IList<CampaignCardinalityEstimatorDto> res = test.GetCardinalityEsitimator(Ids, true);
                    if (res != null && res.Count() > 0)
                    {
                        foreach (CampaignCommonReportDto item in result)
                        {
                            item.UniqueClicks = res.Where(z => z.AdGroupId == item.Id).Select(x => x.unique_clicks).FirstOrDefault();
                            item.UniqueImp = res.Where(z => z.AdGroupId == item.Id).Select(x => x.unique_impressions).FirstOrDefault();

                        }
                    }
                }
            }
            return result;


        }

        public List<CampaignCommonReportDto> GetAdReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Ad, SubType.None);
            return GetReportCampaignResult(args, string.Empty, "GetAdReport");

        }

        public List<CampaignCommonReportDto> GetCampaignPlatformReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Platform);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Platform", "Undefined"), "GetCampaignPlatformReport");
        }

        public List<CampaignCommonReportDto> GetCampaignManuFactorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Manufacturer);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Manufacturer", "Undefined"), "GetCampaignManuFactorReport");
        }

        public List<CampaignCommonReportDto> GetCampaignOperatorReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.Operator);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Operator", "Undefined"), "GetCampaignOperatorReport");
        }
        public List<CampaignCommonReportDto> GetCampaignAudianceSegmentReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.AudianceSegmentsForAdvertiser, SubType.Audiance);
            var results= GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("Operator", "Undefined"), "GetCampaignAudianceSegmentReport");

            if (results!=null  )
            {
                foreach (var item in results)
                {
                    if (item.SegmentId > 0)
                    {
                        var dpName = BusinessPartnerRepository.GetObjectName(item.ProviderId);

                        item.DataProvider = dpName ;
                    }
                }

            }
            return results;

        }

        public List<CampaignCommonReportDto> GetCampaignGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign, SubType.GeoLocation);
            return GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("GeoLocation", "Undefined"), "GetCampaignGeoLocationReport");
        }
        public List<CampaignCommonReportDto> GetCampaignSubAppSiteReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            if(!string.IsNullOrEmpty(criteriaDto.AdvancedCriteria))
                criteriaDto.SecondAdvancedCriteria = criteriaDto.AdvancedCriteria;
            criteriaDto.AdvancedCriteria = string.Empty;
               var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Campaign,  SubType.AppSite,SubType.SubSite);
            var Results=  GetReportCampaignResult(args, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignSubAppSiteReport");
            //if (Results!=null )
            //{
            //    foreach (var resul in Results)
            //    {
            //        if (!string.IsNullOrEmpty(resul.SecondSubName))
            //        {
            //            resul.Name = resul.SecondSubName;
            //        }

            //    }

            //}

            return Results;
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

        public List<ChartDto> GetCampaignAudianceSegmentReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.AudianceSegmentsForAdvertiser
                , SubType.Audiance);
            return GetChartResult(args, string.Empty);
        }
        public List<ChartDto> GetCampaignAppSiteReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {

            if (!string.IsNullOrEmpty(criteriaDto.AdvancedCriteria))
                criteriaDto.SecondAdvancedCriteria = criteriaDto.AdvancedCriteria;
            criteriaDto.AdvancedCriteria = string.Empty;
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Campaign, SubType.AppSite, SubType.SubSite);

         
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
        public List<DataQBDto> GetResultofDataQBDto(string script, string optoionalDrop, string nameofMethod )
            {   

             var items = GetResult<DataQBDto>(script, optoionalDrop, nameofMethod).ToList();

            return items;

    }

        public long GetResultofDataQBCount(string script, string optoionalDrop, string nameofMethod)
        {

            var items = GetResult<long>(script, optoionalDrop, nameofMethod).ToList();
            if (items != null && items.Count > 0)
                return items[0];
            else
                return 0;

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
            var items = GetResult<AppCommonReportDto>(script, args.DropStatements, "AppReport").ToList();
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


     

    private List<CampaignCommonReportDto> GetReportCampaignResult(ReportGeneratorArgs args, string undefinedText, string methodname)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            //ArabyAds.Framework.ApplicationContext.Instance.Logger.Debug(script);
            var items = GetResult<CampaignCommonReportDto>(script, args.DropStatements, "GetReportCampaignResult").ToList();
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
        private List<CampaignCommonReportDto> GetReportQueryResult(string script, string undefinedText, string methodname)
        {
       
            //ArabyAds.Framework.ApplicationContext.Instance.Logger.Debug(script);
            var items = GetResult<CampaignCommonReportDto>(script,string.Empty, "GetReportQueryResult").ToList();
            foreach (var appReportDto in items)
            {
                if (string.IsNullOrWhiteSpace(appReportDto.Name))
                {
                    appReportDto.Name = undefinedText;
                }
                
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
                AgencyRevenue = campaignCommonReportDto.AgencyRevenue,
                NetCost = campaignCommonReportDto.NetCost,
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
            var items = GetResult<AppSiteStatisticsReport>(script, args.DropStatements, "GetAppStatistcsReport").ToList();

            return items;
        }

        private List<AppSiteStatisticsGeoReport> GetAppStatistcsGeoReportResult(ReportGeneratorArgs args)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            var items = GetResult<AppSiteStatisticsGeoReport>(script, args.DropStatements, "GetAppStatistcsGeoReport").ToList();

            return items;
        }


        private static ReportCriteriaDto GetReportCriteriaDto(AppSiteStatisticsCriteriaDto criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType= CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                PageNumber = criteria.PageNumber,
                ItemsPerPage = criteria.ItemsPerPage,
                AccountAdvertiserId = criteria.AccountAdvertiserId,
                AdvertiserId = criteria.AdvertiserId,
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
                PageNumber = criteria.PageNumber,
                AccountAdvertiserId = criteria.AccountAdvertiserId,
                AdvertiserId = criteria.AdvertiserId,
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
        public ReportCriteriaDto GetReportCriteriaDto(DashboardPerformanceCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                PageNumber = criteria.PageNumber,
                AccountAdvertiserId = criteria.AccountAdvertiserId,
                AdvertiserId = criteria.AdvertiserId,
                ItemsPerPage = criteria.ItemsPerPage,
                OrderColumn = criteria.OrderColumn,
                OrderType = criteria.OrderType,
                CampName = criteria.CampName,
            
                CompanyName = criteria.CompanyName,
                ItemsList = criteria.IdFilter.HasValue ? criteria.IdFilter.ToString() : string.Empty,
                AdvancedCriteria = string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,
                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser
            };
            return criteriaDto;
        }
        public ReportCriteriaDto GetReportCriteriaDto(DashboardChartCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                AccountAdvertiserId = criteria.AccountAdvertiserId,
                AdvertiserId = criteria.AdvertiserId,
                CampName = criteria.CampName,
                CompanyName = criteria.CompanyName,
                MetricCode = criteria.MetricCode,
                ItemsList = criteria.IdFilter.HasValue ? criteria.IdFilter.Value.ToString() : string.Empty,
                AdvancedCriteria = string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,

                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser
            };
            return criteriaDto;
        }
        public ReportCriteriaDto GetReportCriteriaPPerfDto(DashboardPerformanceCriteria criteria)
        {
            var criteriaDto = new ReportCriteriaDto
            {
                CampaignType = CampaignType.Normal,
                NotInCampaignType = CampaignType.AdHouse,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                MetricCode = criteria.MetricCode,
               
                AccountAdvertiserId = criteria.AccountAdvertiserId,
                CampName = criteria.CampName,
                CompanyName = criteria.CompanyName,
                AdvertiserId = criteria.AdvertiserId,
                ItemsList = criteria.IdFilter.HasValue ? criteria.IdFilter.Value.ToString() : string.Empty,
                AdvancedCriteria = string.Empty,
                Layout = "Detailed",
                SummaryBy = 1,

                userId = criteria.userId,
                IsPrimaryUser = criteria.IsPrimaryUser
            };
            return criteriaDto;
        }
        private List<ChartDto> GetChartResult(ReportGeneratorArgs args, string undefinedText)
        {
            var script = ScriptGenerator.GenerateChartSelectScript(args);
            var items = GetResult<ChartDto>(script, args.DropStatements, "GetChartResult").ToList();
            //  var items = new List<ChartDto>();
            //if (args.Criteria.MetricCode.ToLower() == "adrequests")
            //{

            //    foreach (var item in items)
            //    {
            //        item.Yaxis =(int) item.Yaxis / 1000000;


            //    }


            //}

            //args.Criteria.SummaryBy==ReportType.Chart
            return items;
        }

        #endregion


        #region  Deals Report

        public List<DealPerformanceDto> GetDealReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Deal, SubType.None);
            return GetReportDealPerfResult(args, string.Empty);

        }
        public List<DealPerformanceDto> GetDealCampReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Deal, SubType.Campaign);
            var results= GetReportDealPerfResult(args, string.Empty, true);

            if (results != null)
            {
                criteriaDto.MetricCode = "ai";
                criteriaDto.AdvancedCriteria = string.Empty;
                criteriaDto.SecondAdvancedCriteria = string.Empty;
                var args2 = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.AdGroup);
                var avaiableImpression = GetChartResult(args2, string.Empty);
                long totalAvail = 0;
                if (avaiableImpression!=null)
                {
                    foreach (var item in avaiableImpression)
                    {
                        if (item.Yaxis != null)
                        {
                            var tot = Convert.ToInt64(item.Yaxis);

                            var Date = Convert.ToInt32(item.Xaxis.ToString("yyyyMMdd"));

                            var datsResult = results.Where(M => M.Date == Date).ToList();
                            if (datsResult != null)
                            {
                                foreach (var itemr in datsResult)
                                {
                                    itemr.TotalAvailableImpressions = tot;
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }

        public List<DealPerformanceDto> GetDealCampAdGroupReport(ReportCriteriaDto criteriaDto, int accountId)
        {


            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Deal, SubType.Campaign, SubType.AdGroup);
            var results = GetReportDealPerfResult(args, string.Empty, true);


            /*criteriaDto.ItemsPerPage = 1;
            criteriaDto.PageNumber = 0;
            criteriaDto.AdvancedCriteria = string.Empty;
            var args1 = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Deal, SubType.Campaign);
            var avaiableImpression = GetReportDealPerfResult(args1, string.Empty, true);
            */


            if (results!=null)
            {
               
                criteriaDto.MetricCode = "ai";
                criteriaDto.AdvancedCriteria = string.Empty;
                criteriaDto.SecondAdvancedCriteria = string.Empty;
                var args2 = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.AdGroup);
                var avaiableImpression = GetChartResult(args2, string.Empty);
                long totalAvail = 0;
                if (avaiableImpression != null)
                {
                    foreach (var item in avaiableImpression)
                    {
                        if (item.Yaxis != null)
                        {
                            var tot = Convert.ToInt64(item.Yaxis);

                            var Date = Convert.ToInt32(item.Xaxis.ToString("yyyyMMdd"));

                            var datsResult = results.Where(M => M.Date == Date).ToList();
                            if (datsResult != null)
                            {
                                foreach (var itemr in datsResult)
                                {
                                    itemr.TotalAvailableImpressions = tot;
                                }
                            }
                        }
                    }
                }
            }
           
            return results;

        }
        public List<ChartDto> GetDealReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {

            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.None);
            return GetChartResult(args, string.Empty);
        }
        public List<ChartDto> GetDealCampReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {

          
         


            ReportGeneratorArgs args = null;
            if (criteriaDto.MetricCode.ToLower() != "ai")
            { args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.Campaign);

            }
            else
            {
                criteriaDto.AdvancedCriteria = string.Empty;
                criteriaDto.SecondAdvancedCriteria = string.Empty;
                args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.Campaign);
            }
            return GetChartResult(args, string.Empty);
        }
        public List<ChartDto> GetDealCampAdGroupReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {
            ReportGeneratorArgs args = null;
            if (criteriaDto.MetricCode.ToLower() != "ai")
                args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.Campaign, SubType.AdGroup);
            else
            {
                criteriaDto.AdvancedCriteria = string.Empty;
                criteriaDto.SecondAdvancedCriteria = string.Empty;
                args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Deal, SubType.Campaign, SubType.None);
            }
            return GetChartResult(args, string.Empty);
        }

        private List<DealPerformanceDto> GetReportDealPerfResult(ReportGeneratorArgs args, string undefinedText, bool isGroupByCamp = false)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            //ArabyAds.Framework.ApplicationContext.Instance.Logger.Debug(script);
            var items = GetResult<DealPerformanceDto>(script, args.DropStatements, "GetReportDealPerfResult").ToList();
            foreach (var appReportDto in items)
            {
                appReportDto.GroupByCampId = isGroupByCamp;
                if (string.IsNullOrWhiteSpace(appReportDto.FinalSecondSubName))
                {
                    appReportDto.FinalSecondSubName = undefinedText;
                }
                if (!args.IsAccumulated)
                    appReportDto.DateRange = GetDateRange(getDate(appReportDto.Date, appReportDto.TimeId), args.Criteria);
            }

            return items;

        }

        #endregion


        #region  ImpressionLog Report

        public List<ImpressionLogPerformanceDto> GetImpressionLogReport(ReportCriteriaDto criteriaDto, int accountId)
        {
            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Report, EntityType.Audiances, SubType.None);
            return GetReportImpressionLogPerfResult(args, string.Empty);

        }

        public List<ChartDto> GetImpressionLogReportChart(ReportCriteriaDto criteriaDto, int accountId)
        {

            var args = ReportGeneratorArgs.GetInstance(criteriaDto, accountId, ReportType.Chart, EntityType.Audiances, SubType.None);
            return GetChartResult(args, string.Empty);
        }

        private List<ImpressionLogPerformanceDto> GetReportImpressionLogPerfResult(ReportGeneratorArgs args, string undefinedText, bool isGroupByCamp = false)
        {
            var script = ScriptGenerator.GenerateQueryScript(args);
            //ArabyAds.Framework.ApplicationContext.Instance.Logger.Debug(script);
            var items = GetResult<ImpressionLogPerformanceDto>(script, args.DropStatements, "GetReportImpressionLogPerfResult").ToList();
            foreach (var Log in items)
            {
                string usedSegment = Log.UsedSegments;

                if (!string.IsNullOrEmpty(usedSegment))
                {
                    Log.UsedSegments = string.Empty;
                    usedSegment = usedSegment.Remove(usedSegment.IndexOf('}'), 1);
                    usedSegment = usedSegment.Remove(usedSegment.IndexOf('{'), 1);
                    List<string> usedSegmentList = usedSegment.Split(',').ToList();
                    foreach (string seg in usedSegmentList)
                    {
                        var aud = AudienceSegmentRepository.GetSegmentById(Convert.ToInt32(seg));
                        if (aud != null)
                        {
                            Log.UsedSegments += aud.Name+", ";
                        }
                    }

                    if (!string.IsNullOrEmpty(Log.UsedSegments))
                    {
                        if(Log.UsedSegments.LastIndexOf(',')>1)
                        Log.UsedSegments= Log.UsedSegments.Substring(0, Log.UsedSegments.LastIndexOf(','));
                      }
                }
                if (Log.billedsegmentId > 0)
                {
                    var baud = AudienceSegmentRepository.GetSegmentById(Convert.ToInt32(Log.billedsegmentId));
                    if (baud != null)
                    {
                        Log.BilledSegment = baud.Name.Value;
                    }

                }
                //Log.AdvertiserName = CampaignRepository.GetAdvertiserName(Log.Id);
            }

            return items;

        }


        public List<CampaignCommonReportDto> getAdvertisersListForDP(int dataproviderId, string culture, string q, int dateFrom, int dateTo)
        {


            string culprev = culture.Contains("en") ? "en" : "ar";


            string mainQuery = string.Format(@"  select Distinct AdvertiserId as Id , dim_advertisers.name_{1} as Name from fact_data_providers_d inner join dim_advertisers on fact_data_providers_d.AdvertiserId = dim_advertisers.Id where dataproviderid = {0} and  dim_advertisers.name_{1} like '{2}%'  and fact_data_providers_d.dateid between {3} and  {4} ;", dataproviderId, culprev,q,dateFrom,dateTo);


            var Results = GetReportQueryResult(mainQuery, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "getAdvertiserList");






            return Results;
        }

        public List<CampaignCommonReportDto> getAgencyForDP(int dataproviderId, string q,int dateFrom, int dateTo)
        {


          


            string mainQuery = string.Format(@" Select  Distinct acc.Id as Id ,  displayName2 as Name  from dim_accounts_vw  acc  inner join fact_data_providers_d  fact on acc.Id=fact.AccountId  where fact.dataproviderid={0} and acc.displayName2 like '{1}%' and fact.dateid between {2} and  {3} ;", dataproviderId,  q,dateFrom,dateTo);


            var Results = GetReportQueryResult(mainQuery, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "getAgencyForDP");






            return Results;
        }

        public List<CampaignCommonReportDto> getCampaignForDP(int dataproviderId, string q, int dateFrom, int dateTo)
        {





            string mainQuery = string.Format(@" Select  Distinct acc.Id as Id ,  Name as Name  from dim_campaigns  acc  inner join fact_data_providers_d  fact on acc.Id=fact.campaignid  where fact.dataproviderid={0} and acc.Name like '{1}%'  and fact.dateid between {2} and  {3} ;", dataproviderId, q,dateFrom,dateTo);


            var Results = GetReportQueryResult(mainQuery, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "getAgencyForDP");






            return Results;
        }

        #endregion


        #region  Trafic Planner Report

        public List<CampaignCommonReportDto> GetCampaignSubAppSiteReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {


            string criteria = BuildTrafficPlannerCriteria(traficDto);
            string mainquery= string.Format( @"select sum(impressions) as Impressions, sum(requests ) as Requests,  0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(FSA.subappsiteid, 0) as subappId, COALESCE(FSA.appsiteid, 0) as appId   from fact_publisher_stat_w FSA    where (weekid = {0}) {1}
            group by appId,subappId Having( sum(requests )> 0);  ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {4} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      subappId integer DEFAULT '0',
      appId integer DEFAULT '0'
          )
 ; insert into {4} {7} ";
               
            string CountScript = string.Format(@"    Select Count(*) 


            ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size);
            string QueryScript = string.Format(newTemptable + @" CREATE temporary TABLE {4}_counts (
   relname  text PRIMARY KEY,
   reltuples   numeric,TotalAvailableImpressions bigint);  insert into {4}_counts SELECT  '{6}',( Select Count(*)  from {4}) as reltuples ;    Select (select  reltuples from {4}_counts where relname='{6}') as TotalCount , Impressions as Impress, Requests ,DataFee , BillableCost,  O.Name as Name ,  O2.SubPublisherName as SecondSubName,subappId,appId  from (

select * from {4}

    ) as newTable LEFT OUTER JOIN dim_sub_appsites O2 ON O2.Id = newTable.subappId LEFT OUTER JOIN dim_appsites O ON O.Id = newTable.appId order by Requests DESC LIMIT {2}
            OFFSET {3}    ;", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size,"ReportSubSite_"+DateTime.Now.Ticks, CountScript,"SubSite",  mainquery);


            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignSubAppSiteReport");


            //var countResult = GetResult<long>(CountScript);
            MinHashEstimatorProcessing minHashAlgortihtem1 = new MinHashEstimatorProcessing(CounterCode.AppSiteCode, traficDto);
            minHashAlgortihtem1.BuildTrafficPlannerCriteriaCahing();
            MinHashEstimatorProcessing minHashAlgortihtemSub = new MinHashEstimatorProcessing(CounterCode.SubAppSiteCode, traficDto);
            minHashAlgortihtemSub.BuildTrafficPlannerCriteriaCahing();
            if (Results != null)
            {
                foreach (var resul in Results)
                {
                    // resul.TotalCount = countResult[0];
                    if (!string.IsNullOrEmpty(resul.SecondSubName))
                    {
                        resul.Name = resul.SecondSubName;
                        resul.UniqueImp = minHashAlgortihtemSub.GetUniqueCountForByCounterId(resul.subappId);
                    }
                    else
                    {
                        resul.UniqueImp = minHashAlgortihtem1.GetUniqueCountForByCounterId(resul.appId);

                    }
                  
                }

            }

            return Results;
        }
        public List<CampaignCommonReportDto> GetCampaignCountryReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";

            string criteria = BuildTrafficPlannerCriteria(traficDto);
            string mainQuery= string.Format(@"select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(countryid, 0) as Id    from fact_publisher_stat_w where countryid>0 and (weekid = {0}) {1}
            group by Id Having( sum(requests )> 0 ) ; ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

select sum(impressions) as Impressions,    COALESCE(countryid, 0) as Id    from fact_publisher_stat_w where countryid>0 and (weekid = {0}) {1}
            group by Id   Having( sum(requests )>0 )  
          

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable+@"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress, Requests , DataFee,BillableCost,  crt.Id, crt.location_{4} As Name from(

      select * from {5}

    ) as newTable LEFT OUTER Join dim_locations crt on newTable.Id = crt.Id  order by Requests DESC LIMIT {2}
            OFFSET {3}; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportCountry_" + DateTime.Now.Ticks, CountScript, "Country", mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignCountryReportTraficPlannerDrillDown");
           
            if (Results != null)
            {

                List<long> countResult = new List<long>() { 0 };
                //if (!traficDto.isChartType)
                 //   countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.CountryCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                  //  resul.TotalCount = countResult[0];
                   
                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }




            return Results;
        }

        public List<CampaignCommonReportDto> GetCampaignOSReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";
            string criteria = BuildTrafficPlannerCriteria(traficDto);
     

            string mainQuery = string.Format(@"    select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(deviceosid, 0) as Id    from fact_publisher_stat_w where deviceosid>0 and (weekid = {0}) {1}
            group by Id   Having( sum(requests )>0)   ; ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

    select sum(impressions) as Impressions,   COALESCE(deviceosid, 0) as Id    from fact_publisher_stat_w where deviceosid>0 and (weekid = {0}) {1}
            group by Id  Having( sum(requests )>0)   
           

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable + @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress,Requests, DataFee, BillableCost,  crt.Id ,crt.platform_{4} As Name from (

select * from {5}

    ) as newTable LEFT OUTER Join dim_platforms crt on newTable.Id = crt.Id  order by Requests DESC LIMIT {2}
            OFFSET {3}  ; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportOS_" + DateTime.Now.Ticks, CountScript, "OS",mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignOSReportTraficPlannerDrillDown");

      
            if (Results != null)
            {
                List<long> countResult = new List<long>() { 0 };
               // if (!traficDto.isChartType)
                    //countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.PlatformCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                   // resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }
            return Results;
        }
        public List<CampaignCommonReportDto> GetCampaignAdTypeGroupReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";
            string criteria = BuildTrafficPlannerCriteria(traficDto);
            string mainQuery = string.Format(@"     select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(adtypegroupid, 0) as Id    from fact_publisher_stat_w where adtypegroupid>0 and (weekid = {0}) {1}
            group by Id Having( sum(requests )>0) ;  ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

    select sum(impressions) as Impressions,   COALESCE(adtypegroupid, 0) as Id    from fact_publisher_stat_w where adtypegroupid>0 and  (weekid = {0}) {1}
            group by Id Having(  sum(requests )>0)
           

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable + @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress , Requests, DataFee,BillableCost,  Id  from (

select * from {5}
 
    ) as newTable  order by Requests DESC  LIMIT {2}
            OFFSET {3} ; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportAdType_" + DateTime.Now.Ticks, CountScript, "AdType", mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignAdTypeGroupReportTraficPlannerDrillDown");

           
            if (Results != null)
            {
                List<long> countResult = new List<long>() { 0 };
               // if (!traficDto.isChartType)
                 //   countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.AdFormatCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                   // resul.TotalCount = countResult[0];
                    if (resul.Id == (int)AdTypeGroup.Undefined)
                      resul.CalculateTheName(AdTypeGroup.Undefined) ;
                    if (resul.Id == (int)AdTypeGroup.Banner)
                        resul.CalculateTheName(AdTypeGroup.Banner);
                   
                    if (resul.Id == (int)AdTypeGroup.Native)
                        resul.CalculateTheName(AdTypeGroup.Native);
                 
                    if (resul.Id == (int)AdTypeGroup.InStream)
                        resul.CalculateTheName(AdTypeGroup.InStream);
                   

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);
                }

            }
            return Results;
        }
        public List<CampaignCommonReportDto> GetCampaignDeviceTypeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";
            string criteria = BuildTrafficPlannerCriteria(traficDto);
            string mainQuery = string.Format(@" select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost,COALESCE(dimDv.devicetypeid, 0) as newId     from fact_publisher_stat_w factw LEFT OUTER Join dim_devices dimDv on factw.devicemodelid = dimDv.id where dimDv.devicetypeid>0 and (weekid = {0}) {1}
            group by newId Having(  sum(requests )>0) ;", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      newid integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

    select sum(impressions) as Impressions, COALESCE(dimDv.devicetypeid, 0) as newId     from fact_publisher_stat_w factw LEFT OUTER join  dim_devices dimDv on factw.devicemodelid = dimDv.id where dimDv.devicetypeid>0 and (weekid = {0}) {1}
            group by newId Having(  sum(requests )>0)
           

    ) as newTable ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable + @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress,  Requests,DataFee,BillableCost,  crt.Id as Id ,crt.devicetype_{4} As Name from (


select * from {5}
    ) as newTable LEFT OUTER JOIN dim_devicetypes crt on newTable.newId = crt.Id order by Requests DESC  LIMIT {2}
            OFFSET {3}  ; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportDeviceType_" + DateTime.Now.Ticks, CountScript, "DeviceType", mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignDeviceTypeReportTraficPlannerDrillDown");

            
            if (Results != null)
            {
                List<long> countResult = new List<long>() { 0 };
              //  if (!traficDto.isChartType)
                    //countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.DeviceTypeCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                    //resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }

            return Results;
        }

        public List<CampaignCommonReportDto> GetCampaignAdSizeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";
            string criteria = BuildTrafficPlannerCriteria(traficDto);


            string mainQuery = string.Format(@"     select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(dimDv.creativeunitid, 0) as Id from fact_publisher_stat_w factw

   LEFT OUTER  JOIN creativeunitgroups_creativeunitids dimDv on factw.creativeunitgroupid = dimDv.creativeunitgroupid
     where dimDv.creativeunitid>0 and (weekid = {0}) {1}
            group by Id Having(sum(requests )>0 ) ;  ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

    select sum(impressions) as Impressions,     COALESCE(dimDv.creativeunitid, 0) as Id from fact_publisher_stat_w factw

LEFT OUTER  JOIN creativeunitgroups_creativeunitids dimDv on factw.creativeunitgroupid = dimDv.creativeunitgroupid
     where dimDv.creativeunitid>0 and (weekid = {0}) {1}
            group by Id Having( sum(requests )>0)  

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format( newTemptable+  @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount ,  Impressions as Impress ,  Requests ,DataFee, BillableCost,  crt.Id ,crt.creativeunit_{4} As Name from (
select * from {5}

    ) as newTable LEFT OUTER JOIN dim_creativeunits crt on newTable.Id = crt.Id order by Requests DESC  LIMIT {2}   OFFSET {3}; "
                , traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportAdSize_" + DateTime.Now.Ticks, CountScript, "AdSize",mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignAdSizeReportTraficPlannerDrillDown");


            
            if (Results != null)
            {
                 List<long> countResult = new List<long>(){ 0 };
               // if (!traficDto.isChartType)
                 //countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.AdSizeCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                    //resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }
            return Results;
        }
        public List<CampaignCommonReportDto> GetCampaignGenderReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {
            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";

            string criteria = BuildTrafficPlannerCriteria(traficDto);

            string mainQuery = string.Format(@"select sum(impressions) as Impressions, sum(requests ) as Requests,   0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(genderid, 0) as Id    from fact_publisher_stat_w where   (weekid = {0}) {1}
            group by Id   Having(  sum(requests )>0)   ;  ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

select sum(impressions) as Impressions,   COALESCE(genderid, 0) as Id    from fact_publisher_stat_w where  (weekid = {0}) {1}
            group by Id  Having(  sum(requests )>0 )  
          

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);
            string QueryScript = string.Format(newTemptable+@"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress, Requests,DataFee, BillableCost,  crt.Id, crt.gender_{4} As Name from(


select * from {5}
    ) as newTable LEFT OUTER Join dim_genders crt on newTable.Id = crt.Id  order by Requests DESC LIMIT {2}
            OFFSET {3}; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportGender_" + DateTime.Now.Ticks, CountScript, "Gender",mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignCountryReportTraficPlannerDrillDown");

            if (Results != null)
            {

                List<long> countResult = new List<long>() { 0 };
               // if (!traficDto.isChartType)
                    //countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.GenderTypeCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                    //resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }




            return Results;
        }

      
        public string BuildTrafficPlannerCriteria(TrafficPlannerCriteriaDto TrafficPlannerCrt, int countAnd = 1)
        {

            string DrillDownDataFilter = string.Empty;
         
              //DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}   pubaccountid>0 ", string.Empty, countAnd > 0 ? " And " : string.Empty);
              //countAnd++;


            //if (TrafficPlannerCrt.Continents != null && TrafficPlannerCrt.Continents.Length > 0)
            //{
            //    DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} EXISTS (Select loc.id  from dim_locations loc  where parentid IN ({0})  and loc.id= countryid )", string.Join(",", TrafficPlannerCrt.Continents), countAnd > 0 ? " And " : string.Empty);
            //    countAnd++;
            //}

            if (TrafficPlannerCrt.Countries != null && TrafficPlannerCrt.Countries.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  countryid IN({0}) ", string.Join(",", TrafficPlannerCrt.Countries), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }


            if (TrafficPlannerCrt.Operators != null && TrafficPlannerCrt.Operators.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  mobileoperatorid IN({0}) ", string.Join(",", TrafficPlannerCrt.Operators), countAnd > 0 ? " And " : string.Empty);

                countAnd++;
            }

            if (TrafficPlannerCrt.Platforms != null && TrafficPlannerCrt.Platforms.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} deviceosid IN({0}) ", string.Join(",", TrafficPlannerCrt.Platforms), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }
            if (TrafficPlannerCrt.AdFormats != null && TrafficPlannerCrt.AdFormats.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} adtypegroupid IN({0}) ", string.Join(",", TrafficPlannerCrt.AdFormats), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }
            if (TrafficPlannerCrt.GenderType > 0)
            {

                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} genderid ={0} ", TrafficPlannerCrt.GenderType, countAnd > 0 ? " And " : string.Empty);


                countAnd++;
            }
            if (TrafficPlannerCrt.EnvironmentType > 0)
            {

                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} environmenttype ={0} ", TrafficPlannerCrt.EnvironmentType, countAnd > 0 ? " And " : string.Empty);


                countAnd++;
            }
            if (TrafficPlannerCrt.AgeGroup > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} agegroupid={0} ", TrafficPlannerCrt.AgeGroup, countAnd > 0 ? " And " : string.Empty);
                countAnd++;

            }

            if (TrafficPlannerCrt.languages != null && TrafficPlannerCrt.languages.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1}  languageid IN({0}) ", string.Join(",", TrafficPlannerCrt.languages), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }

            if (TrafficPlannerCrt.DeviceTypeId > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format("  {1} EXISTS(Select id from dim_devices where devicetypeId={0}  and id = devicemodelid	)  ", TrafficPlannerCrt.DeviceTypeId, countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }

            if (TrafficPlannerCrt.AdSizes != null && TrafficPlannerCrt.AdSizes.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format("  {1} EXISTS( Select crvGroup.creativeunitgroupid from creativeunitgroups_creativeunitids crvGroup  where creativeunitid IN ({0})  and crvGroup.creativeunitgroupid = creativeunitgroupid )  ", string.Join(",", TrafficPlannerCrt.AdSizes), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }
            if (TrafficPlannerCrt.AppSites != null && TrafficPlannerCrt.AppSites.Length > 0)
            {
                DrillDownDataFilter = DrillDownDataFilter + string.Format(" {1} appsiteid IN({0})  ", string.Join(",", TrafficPlannerCrt.AppSites), countAnd > 0 ? " And " : string.Empty);
                countAnd++;
            }


            return DrillDownDataFilter;


        }

        public List<CampaignCommonReportDto> GetEnvironmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";
            string criteria = BuildTrafficPlannerCriteria(traficDto);

            string mainQuery = string.Format(@"    select sum(impressions) as Impressions, sum(requests ) as Requests, 0 as DataFee, sum(adjustednetcost) as BillableCost, COALESCE(environmenttype, 0) as Id    from fact_publisher_stat_w where environmenttype>0 and (weekid = {0}) {1}
            group by Id   Having(  sum(requests )>0)   ; ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

    select sum(impressions) as Impressions,   COALESCE(environmenttype, 0) as Id    from fact_publisher_stat_w where environmenttype>0 and (weekid = {0}) {1}
            group by Id  Having( sum(requests )>0)   
           

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable + @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress,Requests, DataFee, BillableCost,  Id ,  (   CASE Id WHEN 1 THEN 'Web'
              WHEN 2 THEN 'App'
              ELSE 'All'
       END   ) As Name from (

select * from {5}

    ) as newTable  order by Requests DESC LIMIT {2}
            OFFSET {3}  ; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportOS_" + DateTime.Now.Ticks, CountScript, "EnvType", mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetEnvironmentReportTraficPlannerDrillDown");


            if (Results != null)
            {
                List<long> countResult = new List<long>() { 0 };
                // if (!traficDto.isChartType)
                //countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.environmenttypeCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                    // resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }
            return Results;
        }


        public List<CampaignCommonReportDto> GetCampaignSegmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto)
        {

            string culprev = traficDto.Culture.Contains("en") ? "en" : "ar";

            string criteria = BuildTrafficPlannerCriteria(traficDto);
            string mainQuery = string.Format(@"select 0 as Impressions, sum(unique_requests ) as Requests, 0 as DataFee, 0 as BillableCost, COALESCE(counter_value_id, 0) as Id    from fact_publisher_counters where counter_value_id >0 and  counter_code ='provider-segments' and (dateid = {0})  {1}
            group by Id Having( sum(unique_requests )> 0 ) ; ", traficDto.Weekid, criteria);
            string newTemptable = @"create temporary table {5} (

            Impressions bigint DEFAULT '0',
      Requests bigint DEFAULT '0',
DataFee decimal(21, 12) DEFAULT '0.00000',
BillableCost decimal(21, 12) DEFAULT '0.00000',
      Id integer DEFAULT '0'
   
          
) ; insert into {5} {8} ";
            string CountScript = string.Format(@"Select Count(*) from(

select 0 as Impressions,    COALESCE(counter_value_id, 0) as Id    from fact_publisher_counters where counter_value_id >0 and  counter_code ='provider-segments' and (dateid = {0}) {1}
            group by Id   Having( sum(unique_requests )>0 )  
          

    ) as newTable  ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev);

            string QueryScript = string.Format(newTemptable + @"CREATE temporary TABLE {5}_counts(
relname  text PRIMARY KEY,
reltuples   numeric, TotalAvailableImpressions bigint); insert into {5}_counts SELECT  '{7}',(select count(*) from {5}) as reltuples; Select(select  reltuples from {5}_counts where relname = '{7}') as TotalCount , Impressions as Impress, Requests , DataFee,BillableCost,  crt.Id, crt.name_{4} As Name from(

      select * from {5}

    ) as newTable LEFT OUTER Join dim_audience_segments crt on newTable.Id = crt.Id  order by Requests DESC LIMIT {2}
            OFFSET {3}; ", traficDto.Weekid, criteria, traficDto.Size, traficDto.PageIndex * traficDto.Size, culprev, "ReportSegments_" + DateTime.Now.Ticks, CountScript, "Segments", mainQuery);

            var Results = GetReportQueryResult(QueryScript, Framework.Resources.ResourceManager.Instance.GetResource("SubAppSite", "Undefined"), "GetCampaignSegmentReportTraficPlannerDrillDown");

            if (Results != null)
            {

                List<long> countResult = new List<long>() { 0 };
                //if (!traficDto.isChartType)
                //   countResult = GetResult<long>(CountScript);
                MinHashEstimatorProcessing minHashAlgortihtem = new MinHashEstimatorProcessing(CounterCode.providerSegmentsCode, traficDto);
                minHashAlgortihtem.BuildTrafficPlannerCriteriaCahing();
                foreach (var resul in Results)
                {
                    //  resul.TotalCount = countResult[0];

                    resul.UniqueImp = minHashAlgortihtem.GetUniqueCountForByCounterId(resul.Id);

                }

            }




            return Results;
        }



        #endregion
    }
}
