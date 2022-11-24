using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    public class AdStatus
    {
        
        public int CampaignId{get;set;}
        public int AdgroupId{get;set;}
        public int AdId{get;set;}
        public int AdvertiserId { get; set; }
        public int StatusId { get; set; }
    }
    public interface ISummaryRepository : IRepository<ChartDto>
    {
        string CalculateAdvertiserStatus(IList<AdStatus> localAds);
        string CalculateAdvertiserStatusString(IList<AdStatus> localAds);
        decimal GetAdvertiserAccountTotalSpend(DateTime fromDate, DateTime toDate, int AdvertiserAssociationId);
        decimal GetAccountTotalRevenue(DateTime fromDate, DateTime toDate, int accountId);

        decimal GetAccountTotalSpend(DateTime fromDate, DateTime toDate, int accountId);
        #region Performance
        List<AppSitePerformance> GetAppSitesPerformance(PerformanceCriteria criteria);
        List<CampaignPerformanceDto> GetCampaignsPerformance(PerformanceCriteria criteria);
        CampaignPerformanceDto GetCampaignPerformance(PerformanceCriteriaBase criteria);
        List<AdGroupPerformanceDto> GetAdGroupsPerformance(PerformanceCriteria criteria);
        AdGroupPerformanceDto GetAdGroupPerformance(PerformanceCriteriaBase criteria);
        List<AdPerformance> GetAdsPerformance(PerformanceCriteria criteria);
        #endregion
        IList<AdStatus> GetAdsByCampaign(PerformanceCriteriaBase criteria);
        IList<AdStatus> GetAdsByAdGroups(PerformanceCriteriaBase criteria);
        string CalculateStatus(IList<AdStatus> adStatus);
        List<AdvertiserPerformanceDto> GetAdvertisersPerformance(PerformanceCriteria criteria);
        AdvertiserPerformanceDto GetAdvertiserPerformance(PerformanceCriteriaBase criteria);

        IList<AdStatus> GetAdsByAdvertiser(PerformanceCriteriaBase criteria);


        List<AudienceListPerformanceDto> GetAudienceListsPerformance(PerformanceCriteria criteria);


    }
}
