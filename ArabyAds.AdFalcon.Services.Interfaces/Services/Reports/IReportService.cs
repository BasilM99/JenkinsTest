using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.Framework.EventBroker;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Reports
{
    [ServiceContract]
    [CacheHeader("ReportService", null)]
    public interface IReportService
    {

        #region Dashboard

        /// <summary>
        /// Return the chart points that will be drawn on the chart control
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountId"></param>
        /// <param name="metricCode"></param>
        /// <param name="appsiteId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppSiteChart(DashboardChartCriteria criteria);


        /// <summary>
        /// Get appsite performance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppSitePerformanceDto> GetAppSitePerformance(DashboardPerformanceCriteria criteria);


        /*   /// <summary>
           /// Get total records for appsite performance
           /// </summary>
           /// <param name="accountId"></param>
           /// <param name="fromDate"></param>
           /// <param name="toDate"></param>
           /// <returns></returns>
           [OperationContract]
           //[FaultContract(typeof(ServiceFault))]
          int GetTotalAppSitePerformance(DashboardPerformanceCriteria criteria);*/

        /// <summary>
        /// Get Appsite by geo location
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="countryId"></param>
        /// <param name="appSiteId"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppSiteGeoLocationDto> GetAppSiteGeoLocation(DashboardGeoLocationCriteria criteria);


        ///// <summary>
        ///// Get total records for appsite by geo location
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <param name="fromDate"></param>
        ///// <param name="toDate"></param>
        ///// <param name="countryId"></param>
        ///// <param name="appSiteId"></param>
        ///// <returns></returns>
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //int GetTotalAppSiteGeoLocation(DashboardGeoLocationCriteria criteria);

        /// <summary>
        /// Get Campaigns by geo location
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="countryId"></param>
        /// <param name="appSiteId"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AdGeoLocationDto> GetAdGeoLocation(DashboardGeoLocationCriteria criteria);

        /* /// <summary>
         /// Get total records for campaigns by geo location
         /// </summary>
         /// <param name="accountId"></param>
         /// <param name="fromDate"></param>
         /// <param name="toDate"></param>
         /// <param name="countryId"></param>
         /// <param name="appSiteId"></param>
         /// <returns></returns>
         [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAdGeoLocation(DashboardGeoLocationCriteria criteria);*/

        /// <summary>
        /// Return the chart points that will be drawn on the chart control
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="p"></param>
        /// <param name="metricCode"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAdChart(DashboardChartCriteria criteria);

        /// <summary>
        /// Get Campaign performance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AdPerformanceDto> GetAdPerformance(DashboardPerformanceCriteria criteria);

        /* /// <summary>
         /// Get total records for appsite performance
         /// </summary>
         /// <param name="accountId"></param>
         /// <param name="fromDate"></param>
         /// <param name="toDate"></param>
         /// <returns></returns>
         [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAdPerformance(DashboardPerformanceCriteria criteria);*/

        #endregion

        #region Campaign Reports

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignReport(ReportCriteriaDto criteriaDto);

        /*[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        int GetTotalCampaignReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetAdGroupReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAdGroupReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetAdReport(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignPlatformReport(ReportCriteriaDto criteriaDto);

        /*[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        int GetTotalCampaignPlatformReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignManuFactorReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalCampaignManuFactorReport(ReportCriteriaDto criteriaDto);*/

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAdReport(ReportCriteriaDto criteriaDto);*/


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignOperatorReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalCampaignOperatorReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignGeoLocationReport(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignSubAppSiteReport(ReportCriteriaDto criteriaDto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAdGroupReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAdReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignOperatorReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignGeoLocationReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignPlatformReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignManuFactorReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ServiceKnownType(typeof(ReportSchedulerDto))]


        [EventBroker("ReportScheduler Save")]
        ValueMessageWrapper<int> SaveSchadulingReport(ReportSchedulerDto dto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ServiceKnownType(typeof(testReportSchedulerDto))]
        [NoAuthentication]
        ValueMessageWrapper<int> TestSaveSchadulingReport(testReportSchedulerDto dto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteSchadulingReport(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
         void SendSchadulingReport(int[] Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ReportSchedulerDto GetSchadulingReport(ValueMessageWrapper<int> id);
        #endregion

        #region App Reports

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppCommonReportDto> GetAppReport(ReportCriteriaDto criteriaDto);

        /*[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        int GetTotalAppReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppCommonReportDto> GetAppOperatorReport(ReportCriteriaDto criteriaDto);

        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //int GetTotalAppOperatorReport(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppCommonReportDto> GetAppPlatformReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAppPlatformReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppCommonReportDto> GetAppManuFactorReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAppManuFactorReport(ReportCriteriaDto criteriaDto);*/

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppCommonReportDto> GetAppGeoLocationReport(ReportCriteriaDto criteriaDto);

        /* [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         int GetTotalAppGeoLocationReport(ReportCriteriaDto criteriaDto);*/


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppOperatorReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppGeoLocationReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppPlatformReportChart(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetAppManuFactorReportChart(ReportCriteriaDto criteriaDto);


        #endregion

        #region API

        /// <summary>
        /// Get AppSite reports based on the reportCriteria parameter
        /// </summary>
        /// <param name="reportCriteria"></param>
        /// <returns></returns>
        [NoAuthentication]
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppSiteStatisticsReport> GetAppSiteStatisticsReport(AppSiteStatisticsCriteriaDto reportCriteria);

        /// <summary>
        /// Get AppSite reports based on the reportCriteria parameter group by country
        /// </summary>
        /// <param name="reportCriteria"></param>
        /// <returns></returns>
        [NoAuthentication]
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<AppSiteStatisticsGeoReport> GetAppSiteStatisticsGeoReport(AppSiteStatisticsCriteriaDto reportCriteria);

        #endregion

        #region Jobs
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetCampaignName(ValueMessageWrapper<int> campaignId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAppSiteName(ValueMessageWrapper<int> appSiteId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> UpdateJobToSchduledAfterRun(UpdateJobToSchduledAfterRunRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateJobToSchduled(UpdateJobToSchduledRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<ReportSchedulerDto> GetAppsJobsToSchduled();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<ReportSchedulerDto> GetCampaignJobsToSchduled();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignGeoLocationReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignOperatorReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetAdReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignManuFactorReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignPlatformReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetAdGroupReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<AppCommonReportDto> GetAppGeoLocationReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<AppCommonReportDto> GetAppManuFactorReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<AppCommonReportDto> GetAppPlatformReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<AppCommonReportDto> GetAppOperatorReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<AppCommonReportDto> GetAppReportForEmailService(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetEmailAdminForReport();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultReportSchedulerDto QueryByCratiriaForReportSchaduling(ReportSchedulerCriteria criteria);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void PauseSchadulingReport(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteSchadulingReportBulk(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RunSchadulingReport(int[] Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAdGroupName(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAdName(ValueMessageWrapper<int> Id);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAdGroupNameForQ(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAdNameForQ(ValueMessageWrapper<int> Id);




        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateJobToSendNow(ValueMessageWrapper<int> jobID);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateJobToFinished(UpdateJobToFinishedRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateJobToNotFinished(ValueMessageWrapper<int> jobID);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateJobToFinishedNoFireTime(ValueMessageWrapper<int> jobID);
        #endregion

        #region Deals
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetDealChart(DashboardChartCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<DealPerformanceDto> GetDealPerformance(DashboardPerformanceCriteria criteria);

        #endregion
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignSubAppSiteReportForEmailService(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ResultReportSchedulerDto QueryByCratiriaForReportCriteria(ReportJsonCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteCriteriaReportBulk(int[] Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> SaveReportCriteria(ReportCriteriaSchedulerDto reportSchedulerDto);
        #region  ImpressionLog


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ImpressionLogPerformanceDto> GetImpressionLogPerformance(DashboardPerformanceCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetImpressionLogChart(DashboardChartCriteria criteria);


        #endregion


        [OperationContract]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        //[FaultContract(typeof(ServiceFault))]
        List<metriceColumnDto> GetmetriceColumns();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<metriceColumnDto> GetmetriceColumnsForAdvertiser();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<metriceColumnDto> GetmetriceColumnsForPublisher();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        metriceColumnDto GetColumn(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ValueMessageWrapper<int> GetColumnId(GetColumnIdRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<int> GetmetriceColumnsForTemplate(ValueMessageWrapper<int> TemplateId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignAppSiteReportChart(ReportCriteriaDto criteriaDto);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<metriceColumnDto> GetAllmetriceColumnsForAdvertiser();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<metriceColumnDto> GetAllmetriceColumnsForPublisher();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<ChartDto> GetCampaignAudianceSegmentReportChart(ReportCriteriaDto criteriaDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignAudianceSegmentReport(ReportCriteriaDto criteriaDto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<CampaignCommonReportDto> GetCampaignAudianceSegmentForAdvertiserReportForEmailService(ReportCriteriaDto criteriaDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignSubAppSiteReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignAdSizeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignDeviceTypeReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignOSReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignCountryReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignAdTypeGroupReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetCampaignGenderReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> GetEnvironmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        [OperationContract]
        List<CampaignCommonReportDto> GetCampaignSegmentReportTraficPlannerDrillDown(TrafficPlannerCriteriaDto traficDto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> getAdvertisersListForDP(QueryDataProviderRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> getAgencyForDP(QueryDataProviderRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<CampaignCommonReportDto> getCampaignForDP(QueryDataProviderRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ValueMessageWrapper<int> GetColumnCount(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ValueMessageWrapper<int> GetMeasureCount(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        DimensionDto GetDimensionById(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ColumnQBDto GetColumnById(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        MeasureDto GetMeasureById(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        MeasureDto GetMeasureByName(string Name);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ColumnQBDto GetColumnByName(string Name);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<MeasureDto> GetMeasuresByFactId(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<ColumnQBDto> GetColumnsByFactId(GetColumnsByFactIdRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<FactDto> GetAllFacts();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        FactDto GetFactById(ValueMessageWrapper<int> Id);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<DataQBDto> GetResultofDataQBDto(GetResultofDataQBDtoRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<string> Execute(ExecuteQueryRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultDataSetQBDto ExecuteWithPagination(ExecuteWithPaginationRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        List<DataQBDto> GetResultofDataQBDtoWithScoping(GetResultofDataQBDtoWithScopingRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultDataModelDto FilterResult(DataModelDto data);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        List<string> ExecuteAndFilterResult(DataModelDto data);


        [OperationContract]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<DimensionDto> GetDimensionsType();

        [OperationContract]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        DimensionDto GetDimensionIdByType(ValueMessageWrapper<int> Id);


        [OperationContract]
        [NoAuthentication]
        IList<ReportCriteriaDto> GetReportCriteriaForDashboard(ValueMessageWrapper<int> SectionType);
        [OperationContract]
        [NoAuthentication]
        ValueMessageWrapper<int> SaveReportCriteriaForDashboard(ReportCriteriaDto dto);

        [OperationContract]
        [NoAuthentication]
       // [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        ReportCriteriaDto GetReportCriteriaForDashboardById(ValueMessageWrapper<int> Id);


        [OperationContract]
        [NoAuthentication]
        List<KPIConfigDto> GetKPIConfigs();
    }
}
