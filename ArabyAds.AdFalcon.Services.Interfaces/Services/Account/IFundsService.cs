using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    public interface IFundsService
    {
        /// <summary>
        /// Get Account Funds History
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        FundResultDto GetAccountFundsHistory(HistoryCriteriaDto fundsCriteria);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        FundResultDto GetTransactionVATHistory(TransactionVATCriteria criteria);
    }
}
