using System;
using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund
{
    /// <summary>
    /// this service is responsible for all fund's transactions operations, also it provides
    /// the needed information and configurations for related entities.
    /// </summary>
    [CacheHeader("FundTransactionService", null)]
    [ServiceContract]

    public interface IFundTransactionService
    {
        /// <summary>
        /// Get Transaction details for the given id.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        FundTransactionDto GetFundTransactionById(int id);

        /// <summary>
        /// Initiates fund's transaction, some fund types will be intiated completed as in check and banck transfer.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        int InitiateFundTransaction(FundTransactionDto data);

        /// <summary>
        /// Closes an already opened transaction, by committing or marking it as failed.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isTransactionComitted"></param>
        /// <returns>indicates if the transaction committed or not based on the server checking</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [EventBroker("Close Fund Transaction")]
        bool CloseFundTransaction(FundTransactionResponseDto data);


        /// <summary>
        /// get list of all active PGWs
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ICollection<PgwDto> GetRegistredPGWs();

        /// <summary>
        /// Get all pending transaction that happened after the given date and for the given PGW.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<FundTransactionResponseDto> GetPendingFundTransactions(int fundTransactionTypeId, DateTime dtFrom);

        ///// <summary>
        /// Get PGW details for the given id.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        PgwDto GetPgwInfo(int id);

        /// <summary>
        /// Get PGW details for the given code.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = false, IsSelfCachable = true)]
        PgwDto GetPgwInfoByCode(string code);

        /// <summary>
        /// Add fund
        /// </summary>
        /// <param name="fundDto"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Add Fund by Admin")]
        void AddFund(NewFundDto fundDto);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Add Fund by Admin")]
        [NoAuthentication]
        void AddOverBudgetReturnFundFromCampaign(int campaignId, decimal invoicedAmount);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Add Fund by Admin")]
        [NoAuthentication]
        void AddOverBudgetReturnFundFromAccount(int AccountId, decimal invoicedAmount );


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void SendAdGroupbillingInfoacknowledgment(int AdGroupId, string FieldToChange, decimal? RequestedAmount, decimal? CommittedAmount, DateTime ModOn);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void SendCampaignbillingInfoacknowledgment(int CampaignId, string FieldToChange, decimal RequestedAmount, decimal CommittedAmount, DateTime ModOn);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void testPublickEventKafka();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        FundTransactionDto GetFundTransactionByref(string refId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateFundTransactionByref(int Id, string refId);

    }
}
