using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Persistence.Reports.RepositoriesGP
{
    public class AdStatus
    {
        
        public int CampaignId{get;set;}
        public int AdgroupId{get;set;}
        public int AdId{get;set;}
        public int StatusId { get; set; }
    }
    public interface ISummaryGPRepository : IRepository<ChartDto>
    {
        decimal GetAccountTotalRevenue(DateTime fromDate, DateTime toDate, int accountId);
        decimal GetAdvertiserAccountTotalSpend(DateTime fromDate, DateTime toDate, int AdvertiserAssociationId);
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

        List<EventItemDto> EventItemsList();
    }
}
