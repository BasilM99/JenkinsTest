using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Services
{
    [ServiceContract]
    public interface ISummaryService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        decimal GetAccountTotalRevenue(DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        decimal GetAccountTotalSpend(DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        decimal GetAdvertiserAccountTotalSpend(DateTime fromDate, DateTime toDate, int Id);
    }
}
