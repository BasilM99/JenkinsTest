using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Reports.Repositories
{
    /// <summary>
   /// Provides methods to implement performance reports for different entities in the system (Account,AppSite,Campaign,...)
    /// </summary>
    public interface IPerformanceReportRepository : IRepository<ChartDto>
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
