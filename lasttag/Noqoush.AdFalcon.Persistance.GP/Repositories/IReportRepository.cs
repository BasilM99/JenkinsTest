using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ReportCriteriaDto = Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.ReportCriteriaDto;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB;

namespace Noqoush.AdFalcon.Persistence.RepositoriesGP.Reports
{
    public interface IReportGPRepository : IRepository<ChartDto>
    {
        #region Dashboard
        List<ChartDto> GetCampaignAppSiteReportChart(ReportCriteriaDto criteriaDto, int accountId);
        List<ChartDto> GetAppSiteChart(DashboardChartCriteria criteria, int accountId);

        List<AppSitePerformanceDto> GetAppSitePerformance(DashboardPerformanceCriteria criteria, int accountId);

        int GetTotalAppSitePerformance(DashboardPerformanceCriteria criteria, int accountId);

        List<AppSiteGeoLocationDto> GetAppSiteGeoLocation(DashboardGeoLocationCriteria criteria, int accountId);

        //int GetTotalAppSiteGeoLocation(DashboardGeoLocationCriteria criteria, int accountId);

        List<AdGeoLocationDto> GetAdGeoLocation(DashboardGeoLocationCriteria criteria, int accountId);

        /* int GetTotalAdGeoLocation(DashboardGeoLocationCriteria criteria, int accountId);*/

        List<ChartDto> GetAdChart(DashboardChartCriteria criteria, int accountId);

        List<AdPerformanceDto> GetAdPerformance(DashboardPerformanceCriteria criteria, int accountId);

        // int GetTotalAdPerformance(DashboardPerformanceCriteria criteria,int accountId);

        #endregion

        #region Campaign Reports

        /// <summary>
        /// Get the campaign report based on the criteria on the criteriaDto object and accountId
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto, int accountId);

        /* /// <summary>
         /// Get the total records for campaign report 
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <param name="accountId"></param>
         /// <returns></returns>
         int GetTotalCampaignReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// Get the ad group report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto, int accountId);


        /* /// <summary>
         /// Get the total records for ad group report
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <returns></returns>
         int GetTotalAdGroupReport(ReportCriteriaDto criteriaDto, int accountId);*/


        /// <summary>
        /// Get ads report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetAdReport(ReportCriteriaDto criteriaDto, int accountId);


        /*/// <summary>
        /// Get the total records for ads report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        int GetTotalAdReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignPlatformReport(ReportCriteriaDto criteriaDto, int accountId);

        /* /// <summary>
         /// 
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <param name="accountId"></param>
         /// <returns></returns>
         int GetTotalCampaignPlatformReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignManuFactorReport(ReportCriteriaDto criteriaDto, int accountId);

        /* /// <summary>
         /// 
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <param name="accountId"></param>
         /// <returns></returns>
         int GetTotalCampaignManuFactorReport(ReportCriteriaDto criteriaDto, int accountId);*/


        /// <summary>
        /// Get ads for operator report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignOperatorReport(ReportCriteriaDto criteriaDto, int accountId);


        /* /// <summary>
         /// Get the total records for ad group report
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <returns></returns>
         int GetTotalCampaignOperatorReport(ReportCriteriaDto criteriaDto, int accountId);*/


        /// <summary>
        /// Get ads for geo location report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId);


        /// <summary>
        /// Get the total records for geo location report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<CampaignCommonReportDto> GetCampaignSubAppSiteReport(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// Get the chart for campaign depends on the metric code
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetCampaignReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// Get the chart for ad group depends on the metric code
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAdGroupReportChart(ReportCriteriaDto criteriaDto, int accountId);


        /// <summary>
        /// Get the chart for ad depends on the metric code
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAdReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// Get the chart for operators depends on the metric code
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetCampaignOperatorReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// Get the chart for geolocation depends on the metric code
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetCampaignGeoLocationReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetCampaignPlatformReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetCampaignManuFactorReportChart(ReportCriteriaDto criteriaDto, int accountId);


        #endregion

        #region App Reports

        /// <summary>
        /// Get the app/site email report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<AppEmailReportDto> GetAppEmailReport(EmailReportCriteriaDto criteriaDto);

        /// <summary>
        /// Get the app/site report
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<AppCommonReportDto> GetAppReport(ReportCriteriaDto criteriaDto, int accountId);

        /*/// <summary>
        /// Get the total app/site records
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        int GetTotalAppReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<AppCommonReportDto> GetAppOperatorReport(ReportCriteriaDto criteriaDto, int accountId);

        /* /// <summary>
         /// 
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <param name="accountId"></param>
         /// <returns></returns>
         int GetTotalAppOperatorReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<AppCommonReportDto> GetAppPlatformReport(ReportCriteriaDto criteriaDto, int accountId);

        /* /// <summary>
         /// 
         /// </summary>
         /// <param name="criteriaDto"></param>
         /// <param name="accountId"></param>
         /// <returns></returns>
         int GetTotalAppPlatformReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<AppCommonReportDto> GetAppManuFactorReport(ReportCriteriaDto criteriaDto, int accountId);

        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        int GetTotalAppManuFactorReport(ReportCriteriaDto criteriaDto, int accountId);*/


        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<AppCommonReportDto> GetAppGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId);

        /*  /// <summary>
          /// 
          /// </summary>
          /// <param name="criteriaDto"></param>
          /// <param name="accountId"></param>
          /// <returns></returns>
          int GetTotalAppGeoLocationReport(ReportCriteriaDto criteriaDto, int accountId);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAppReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAppOperatorReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAppGeoLocationReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAppPlatformReportChart(ReportCriteriaDto criteriaDto, int accountId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ChartDto> GetAppManuFactorReportChart(ReportCriteriaDto criteriaDto, int accountId);

        #endregion

        #region API

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<AppSiteStatisticsReport> GetAppSiteStatisticsReport(AppSiteStatisticsCriteriaDto criteriaDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<AppSiteStatisticsGeoReport> GetAppSiteGeoStatisticsReport(AppSiteStatisticsCriteriaDto criteriaDto);

        #endregion
        ReportCriteriaDto GetReportCriteriaDto(DashboardChartCriteria criteria);
        #region Deals

        List<DealPerformanceDto> GetDealCampAdGroupReport(ReportCriteriaDto criteriaDto, int accountId);
        List<DealPerformanceDto> GetDealCampReport(ReportCriteriaDto criteriaDto, int accountId);
        List<DealPerformanceDto> GetDealReport(ReportCriteriaDto criteriaDto, int accountId);

        List<ChartDto> GetDealCampAdGroupReportChart(ReportCriteriaDto criteriaDto, int accountId);
        List<ChartDto> GetDealCampReportChart(ReportCriteriaDto criteriaDto, int accountId);
        List<ChartDto> GetDealReportChart(ReportCriteriaDto criteriaDto, int accountId);


        #endregion
        ReportCriteriaDto GetReportCriteriaPPerfDto(DashboardPerformanceCriteria criteria);

        #region ImpressionLog Report


        List<ImpressionLogPerformanceDto> GetImpressionLogReport(ReportCriteriaDto criteriaDto, int accountId);

        List<ChartDto> GetImpressionLogReportChart(ReportCriteriaDto criteriaDto, int accountId);


        #endregion

        List<CampaignCommonReportDto> GetCampaignAudianceSegmentReport(ReportCriteriaDto criteriaDto, int accountId);
        List<ChartDto> GetCampaignAudianceSegmentReportChart(ReportCriteriaDto criteriaDto, int accountId);

        List<CampaignCommonReportDto> GetCampaignSubAppSiteReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        List<CampaignCommonReportDto> GetCampaignAdSizeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
        List<CampaignCommonReportDto> GetCampaignDeviceTypeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
        List<CampaignCommonReportDto> GetCampaignOSReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);



        List<CampaignCommonReportDto> GetCampaignCountryReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        List<CampaignCommonReportDto> GetCampaignAdTypeGroupReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
        List<CampaignCommonReportDto> GetCampaignGenderReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        List<CampaignCommonReportDto> GetEnvironmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
         List<CampaignCommonReportDto> getAdvertisersListForDP(int dataproviderId, string culture, string q, int dateFrom, int dateTo);


        List<CampaignCommonReportDto> getAgencyForDP(int dataproviderId, string q,int dateFrom, int dateTo);
        List<CampaignCommonReportDto> getCampaignForDP(int dataproviderId, string q, int dateFrom, int dateToq);

        List<DataQBDto> GetResultofDataQBDto(string script, string optoionalDrop, string nameofMethod);
        long GetResultofDataQBCount(string script, string optoionalDrop, string nameofMethod);

        List<string> GetResultAsString(string script, string optoionalDrop = "", string nameofMethod = "");
    }
}
