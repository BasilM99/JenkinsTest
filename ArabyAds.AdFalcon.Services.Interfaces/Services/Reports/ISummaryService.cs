using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Services
{
    [ServiceContract]
    public interface ISummaryService
    {
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<decimal> GetAccountTotalRevenue(FromToDateMessage request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<decimal> GetAccountTotalSpend(FromToDateMessage request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<decimal> GetAdvertiserAccountTotalSpend(GetAdvertiserAccountTotalSpendRequest request);
    }
}
