using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using Noqoush.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Reports
{
    [ServiceContract]
    public interface IPerformanceReportService
    {
         /// <summary>
        /// Return the top accounts per specific metric (requestts, clicks,...)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<BaseAppSitePerformanceDetailsDto> GetAccountsPerformanceDetails(string accountName, string appSiteName, BaseAppSitePerformanceDetailsCriteria criteria);


        /// <summary>
        /// Return the top appsites per specific metric (requestts, clicks,...)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<BaseAppSitePerformanceDetailsDto> GetAppSitesPerformanceDetails(string accountName,string appSiteName, BaseAppSitePerformanceDetailsCriteria criteria);


        /// <summary>
        /// Return the platforms ordered by specific metric (requestts, clicks,...)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<BaseAppSitePerformanceDetailsDto> GetPlatformsPerformanceDetails(BaseAppSitePerformanceDetailsCriteria criteria);
    }
}
