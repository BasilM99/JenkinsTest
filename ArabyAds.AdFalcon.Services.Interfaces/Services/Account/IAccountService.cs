using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.EventBroker;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        UserDto GetById(ValueMessageWrapper<int> id);

        /// <summary>
        /// Get Account Payment Details for the current account
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountPaymentDetailDto GetAccountPaymentDetails();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker("Update Bank Account Info")]
        void UpdateBankAccountInfo(AccountPaymentDetailDto bankAccountDto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountSummaryDto GetAccountSummary();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Add Payment")]
        void AddPayment(NewPaymentDto paymentDto);


        /// <summary>
        /// Get Account Payment History
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PaymentDtoResult GetAccountPaymentsHistory(HistoryCriteriaDto fundsCriteria);


        /// <summary>
        /// Get Account Payment Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PaymentDetailDto> GetPaymentDetails(GetPaymentDetailsRequest request);

        /// <summary>
        /// Get Account Payment full Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PaymentFullDetailDto> GetFullPaymentDetails(GetFullPaymentDetailsRequest request);

        /// <summary>
        /// Get Account current Discount
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountDiscountDto GetAccountDiscount(ValueMessageWrapper<int> accountId);

        /// <summary>
        /// Get Account current Setting
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountSettingDto GetAccountSetting(ValueMessageWrapper<int> accountId);

        /// <summary>
        /// Save Account Setting
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveAccountSetting(AccountSettingDto setting);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        AccountAPIAccessDto GetAccountAPIAccessByAPIClientId(string clientId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountAPIAccessDto GetAPIAccessSetting();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountAPIAccessDto GenerateAPIAccess();

        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //IList<TrialDto> GetTrials();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        GetTrialSessionsResponse GetTrialSessions(AuditTrialCriteria criteria);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        TrialResultDto MainRootTrialQueryByCratiria(AuditTrialCriteria criteria);

        //IList<TrialDto> GetTrialSessions(int objRootId, int userId, int objTypeId, int page);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<TrialDto> GetTrialSession(GetTrialSessionRequest request);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //long GeAuditTrialForObjectRootCounts(int objRootId, int userId, int objTypeId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<ObjectTypeDto> getObjectTypes();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetRootObjectName(GetRootObjectNameRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetRootObjectTypeName(ValueMessageWrapper<int> RootObjectTypeID);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<TrialDto> GetTrialDetailsSession(GetTrialDetailsSessionRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetRootObjectTypeNameValue(ValueMessageWrapper<int> RootObjectTypeID);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetObjectRootTypeId(string objectRoot);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        AuditTrialsMaxAndMinMessage GetAuditTrialsMaxAndMin(ValueMessageWrapper<int> years);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        AuditTrialsMaxAndMinMessage GetAuditTrialSessionStatAliasMaxAndMin(ValueMessageWrapper<int> years);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        AuditTrialsMaxAndMinMessage GetAuditTrialStatAliasMaxAndMin(ValueMessageWrapper<int> years);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrials(AuditTrialsMaxAndMinMessage request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrialSessionStat(AuditTrialsMaxAndMinMessage request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrialStat(AuditTrialsMaxAndMinMessage request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAccountEmailAddress(ValueMessageWrapper<int> accountId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        ValueMessageWrapper<bool> checkAdPermissions(ValueMessageWrapper<PortalPermissionsCode> Code);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> GetAccountRole(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto CreateAccount(Interfaces.DTOs.Account.UserDto userDtoInfo);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto CreateUserAccount(Interfaces.DTOs.Account.UserDto userDtoInfo);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountCostElementResultDto QueryAccountCostElements(AccountCostElementCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveAccountCostElementBulk(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void EnableDisableAccountCostElement(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveAccountCostElement(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveAccountCostElement(AccountCostElementDto dto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpressionLogListResultDto ImpressionLogQueryByCratiria(ImpressionLogCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpressionLogDto GetImpressionLogById(ValueMessageWrapper<int> Id);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetVATTaxNoRegularExpression(ValueMessageWrapper<int> userID);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetAccountName(ValueMessageWrapper<int> AccountId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        UsersListResultDto QueryByCratiria(UserCriteriaBase criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        [NoAuthentication]

        AdFalconUserInfo BuildAdFalconUser(BuildAdFalconUserRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<UserDto> GetUserAccounts(ValueMessageWrapper<int> userid);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<UserDto> GetUserAccountsByEmail(string email);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        ValueMessageWrapper<int> GetUserAccountsCount(ValueMessageWrapper<int> userid);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<decimal> GetVATValueByAccountId(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> CheckIfDocumentbelongToAccount(ValueMessageWrapper<int> documentId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetFirstUserAccountId(ValueMessageWrapper<int> userid);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> SaveDSPAccountSettingReport(AccountDSPsettingDTO itemDtoVar);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountDSPsettingDTO GetDSPAccountSettingReport(ValueMessageWrapper<int> accountId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SetFeature(ValueMessageWrapper<int> code);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> HadAFeature(ValueMessageWrapper<int> code);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        AccountFeeResultDto QueryAccountFees(AccountFeeCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveAccountFeeBulk(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void EnableDisableAccountFee(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void RemoveAccountFee(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> SaveAccountFee(AccountFeeDto dto);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveCampAccountSetting(AccountSettingDto settings);
    }
}
