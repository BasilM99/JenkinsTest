using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;

using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.Framework.UserInfo;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        UserDto GetById(int id);

        /// <summary>
        /// Get Account Payment Details for the current account
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountPaymentDetailDto GetAccountPaymentDetails();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Update Bank Account Info")]
        void UpdateBankAccountInfo(AccountPaymentDetailDto bankAccountDto);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountSummaryDto GetAccountSummary();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Add Payment")]
        void AddPayment(NewPaymentDto paymentDto);


        /// <summary>
        /// Get Account Payment History
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PaymentDtoResult GetAccountPaymentsHistory(HistoryCriteriaDto fundsCriteria);


        /// <summary>
        /// Get Account Payment Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PaymentDetailDto> GetPaymentDetails(int accountId, PayemntAccountType accountType);

        /// <summary>
        /// Get Account Payment full Detail
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PaymentFullDetailDto> GetFullPaymentDetails(int accountId, PayemntAccountType accountType, PayemntAccountSubType subType);

        /// <summary>
        /// Get Account current Discount
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountDiscountDto GetAccountDiscount(int accountId);

        /// <summary>
        /// Get Account current Setting
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountSettingDto GetAccountSetting(int accountId);

        /// <summary>
        /// Save Account Setting
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveAccountSetting(AccountSettingDto setting);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        AccountAPIAccessDto GetAccountAPIAccessByAPIClientId(string clientId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountAPIAccessDto GetAPIAccessSetting();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountAPIAccessDto GenerateAPIAccess();

        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //IList<TrialDto> GetTrials();
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<TrialDto> GetTrialSessions(AuditTrialCriteria criteria, out int total);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        TrialResultDto MainRootTrialQueryByCratiria(AuditTrialCriteria criteria);

        //IList<TrialDto> GetTrialSessions(int objRootId, int userId, int objTypeId, int page);



        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<TrialDto> GetTrialSession(string id, bool isAdminApp);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //long GeAuditTrialForObjectRootCounts(int objRootId, int userId, int objTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<ObjectTypeDto> getObjectTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetRootObjectName(int RootObjectTypeID, int RootObjectId, string objectTypeName);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetRootObjectTypeName(int RootObjectTypeID);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<TrialDto> GetTrialDetailsSession(long Id, bool IsAdminApp, bool CollectInfo = true);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetRootObjectTypeNameValue(int RootObjectTypeID);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetObjectRootTypeId(string objectRoot);



        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void GetAuditTrialsMaxAndMin(int years, out long max, out long min);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void GetAuditTrialSessionStatAliasMaxAndMin(int years, out long max, out long min);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        void GetAuditTrialStatAliasMaxAndMin(int years, out long max, out long min);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrials(long max, long min);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrialSessionStat(long max, long min);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void DeleteAuditTrialStat(long max, long min);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetAccountEmailAddress(int accountId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        bool checkAdPermissions(PortalPermissionsCode Code);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        int GetAccountRole(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto CreateAccount(Interfaces.DTOs.Account.UserDto userDtoInfo);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto CreateUserAccount(Interfaces.DTOs.Account.UserDto userDtoInfo);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountCostElementResultDto QueryAccountCostElements(AccountCostElementCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveAccountCostElementBulk(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void EnableDisableAccountCostElement(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveAccountCostElement(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveAccountCostElement(AccountCostElementDto dto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ImpressionLogListResultDto ImpressionLogQueryByCratiria(ImpressionLogCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ImpressionLogDto GetImpressionLogById(int Id);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetVATTaxNoRegularExpression(int userID);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetAccountName(int AccountId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        UsersListResultDto QueryByCratiria(UserCriteriaBase criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        [NoAuthentication]

        AdFalconUserInfo BuildAdFalconUser(int accountId, int userid, string emil);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<UserDto> GetUserAccounts(int userid);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<UserDto> GetUserAccountsByEmail(string email);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        int GetUserAccountsCount(int userid);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        decimal GetVATValueByAccountId(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool CheckIfDocumentbelongToAccount(int documentId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetFirstUserAccountId(int userid);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int SaveDSPAccountSettingReport(AccountDSPsettingDTO itemDtoVar);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AccountDSPsettingDTO GetDSPAccountSettingReport(int accountId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SetFeature(int code);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool HadAFeature(int code);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        AccountFeeResultDto QueryAccountFees(AccountFeeCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveAccountFeeBulk(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void EnableDisableAccountFee(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void RemoveAccountFee(int Id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool SaveAccountFee(AccountFeeDto dto);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveCampAccountSetting(AccountSettingDto settings);
    }
}
