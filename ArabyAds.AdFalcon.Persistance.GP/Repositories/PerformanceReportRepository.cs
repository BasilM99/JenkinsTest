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
    public class PerformanceGPReportRepository : BaseReportGPRepository, IPerformanceGPReportRepository
    {
        public PerformanceGPReportRepository(RepositoryImplBase<ChartDto, int> repository, ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager)
            : base(repository, configurationManager)
        {
        }

        public List<BaseAppSitePerformanceDetailsDto> GetAccountsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteriaDto)
        {
            PerformanceReportGeneratorArgs args = PerformanceReportGeneratorArgs.GetInstance(criteriaDto, ReportType.Report, EntityType.App, GroupBy.Account);
            args.GetTotalMetric = true;
            return GetResult(args);

        }

        public List<BaseAppSitePerformanceDetailsDto> GetAppSitesPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteriaDto)
        {
            PerformanceReportGeneratorArgs args = PerformanceReportGeneratorArgs.GetInstance(criteriaDto, ReportType.Report, EntityType.App, GroupBy.Default);
            args.GetTotalMetric = true;
            return GetResult(args);
        }

        public List<BaseAppSitePerformanceDetailsDto> GetPlatformsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteriaDto)
        {
            PerformanceReportGeneratorArgs args = PerformanceReportGeneratorArgs.GetInstance(criteriaDto, ReportType.Report, EntityType.App, GroupBy.Platform);
            args.GetTotalMetric = true;
            return GetResult(args);
        }

        #region Private Members

        /// <summary>
        /// Build the script and return report result
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<BaseAppSitePerformanceDetailsDto> GetResult(PerformanceReportGeneratorArgs args)
        {
            var script = PerformanceReportsScriptGenerator.GenerateQueryScript(args);
            var items = GetResult<BaseAppSitePerformanceDetailsDto>(script, args.DropStatements,"Get AppSitePerformanceDetails").ToList();
           
            return items;
        }

        #endregion

    }
}
