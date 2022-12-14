using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.RepositoriesGP
{
    /// <summary>
   /// Provides methods to implement performance reports for different entities in the system (Account,AppSite,Campaign,...)
    /// </summary>
    public interface IPerformanceGPReportRepository : IRepository<ChartDto>
    {
        /// <summary>
        /// Get Accounts Performance Reports, group data per account
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<BaseAppSitePerformanceDetailsDto> GetAccountsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteriaDto);

        /// <summary>
        /// Get AppSites Performance Reports, group data per appsite
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<BaseAppSitePerformanceDetailsDto> GetAppSitesPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteriaDto);

        /// <summary>
        /// Get Platforms Performance Reports, group data per platform
        /// </summary>
        /// <param name="criteriaDto"></param>
        /// <returns></returns>
        List<BaseAppSitePerformanceDetailsDto> GetPlatformsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteria);
    }
}
