using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework.EventBroker;
using ArabyAds.Framework.UserInfo;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework.Security;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account
{
    [ServiceContract]
    public interface IUserService
    {
        /// <summary>
        /// Check if this email is reserved or not
        /// </summary>
        /// <param name="emailAddress">The email that will be checked</param>
        /// <param name="checkPendingEmail">true to check the pending email</param>
        /// <returns>true if the email is exist , false if not</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> CheckUserEmail(CheckUserEmailRequest request);

        /// <summary>
        /// Activate user by activation code and change his status to be activated
        /// </summary>
        /// <param name="activationCode">activation code for this user</param>
        /// <returns>The UserDto object for the user who has this activation code</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [EventBroker("Register User")]
        UserDto ActivateUser(string activationCode);



        [OperationContract]
        string getCurrentUserAgreement();
        /// <summary>
        /// Get users by criteria
        /// </summary>
        /// <param name="criteria">criteria to search by</param>
        /// <returns>UsersListResultDto</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UsersListResultDto QueryByCratiria(UserCriteriaBase criteria);

        /// <summary>
        /// Get Publisher Users
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UsersListResultDto GetPublisherUsers(AllAppSiteCriteria criteria);

        /// <summary>
        /// Get user information by email address
        /// </summary>
        /// <param name="emailAddress">EmailAddress for this user</param>
        /// <param name="checkPendingEmail">true to get user by pending email</param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto GetUserByEmail(CheckUserEmailRequest request);

        /// <summary>
        /// Get user information by account
        /// </summary>
        /// <param name="accountId">account Id for this user</param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        UserDto GetUserByAccount(UserAccountMessage request);

        /// <summary>
        /// Get All users information
        /// </summary>
        /// <returns>List of UserDto</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IEnumerable<UserDto> GetAllUser();

        /// <summary>
        /// Update user information 
        /// </summary>
        /// <param name="userDtoInfo">The dto for the object that will be updated</param>
        /// <returns>true if the email has been changed , return the activation code , otherwsie , empty string</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ChangeEmailDto UpdateUser(UserDto userDtoInfo);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="email">The email for the user that his password will be changed</param>
        /// <returns>true if reset password process works fine , otherwise false</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> ResetUserPassword(ChangeUserPasswordRequest request);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="email">The email for the user that his password will be changed</param>
        /// <returns>true if reset password process works fine , otherwise false</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> ResetUserPasswordByToken(ResetUserPasswordByTokenRequest request);

        /// <summary>
        /// Save User created Token
        /// </summary>
        /// <param name="email">The email for the user that his password will be changed</param>
        /// <returns>true if save token process works fine , otherwise false</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> SaveUserToken(SaveUserTokenRequest request);

        /// <summary>
        /// Check User Token
        /// </summary>
        /// <param name="email">The email for the user that his password will be changed</param>
        /// <returns>true if the token is exist and not outdated then the process will be works fine , otherwise false</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> CheckUserToken(string token);

        /// <summary>
        /// Change user email address
        /// </summary>
        /// <param name="activationCode"></param>
        /// <param name="hashing"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> ChangeEmail(ChangeEmailDto changeEmailDto);

        /// <summary>
        /// Delete user account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteUser(ValueMessageWrapper<int> userId);

        /// <summary>
        /// Check user password 
        /// </summary>
        /// <param name="password">user password</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> CheckUserPassword(string password);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="password"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void ChangePassword(ChangeUserPasswordRequest request);

        /// <summary>
        /// Impersonate Account
        /// </summary>
        /// <returns>return Impersonated Account Info</returns>
        /// <param name="accountId"></param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ImpersonatedAccountInfo Impersonate(ImpersonateRequest request);

        /// <summary>
        /// Update current user Agreement
        /// </summary>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateAgreement();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string GetPendingEmailAddress(string email);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        ValueMessageWrapper<bool> GetUserForOptingInDB(string userId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        void UpdateUserIdForOptingInDB(UpdateUserIdForOptingInDBRequest request);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //[NoAuthentication]
        //void CreateUserIdForOptingInDB(string userId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string CreateUserIdForOpting();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        InviteResponse invite(InviteRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        InvitationListDto InvitationQueryByCratiria(AccountInvitationCriteria criteria);


        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //[NoAuthentication]

        //string getInvitedEmail(string InvitationCode);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void BlockUser(ValueMessageWrapper<int> userId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsUserBlocked(ValueMessageWrapper<int> userId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> IsUserBlockedByEmail(string userEmail);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        InvitationDto GetInvitation(string InvitationCode);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        UserDto GetUserById(ValueMessageWrapper<int> userId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> GiveTakePermission(AccountAdPermissionsDto details);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string getAccountPermissionCode(ValueMessageWrapper<int> AccountId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        IList<int> GetAccountAdPermissions(ValueMessageWrapper<int> accountId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> GetAccountBuyerCounter();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> CheckduplicateBuyer(string buyerCode);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveAccountBuyer(SaveAccountBuyerRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        UsersListResultDto GetSSPUsers(AllAppSiteCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        ValueMessageWrapper<bool> CheckAcccountDSPEmail(string emailAddress);

        [OperationContract]
        [NoAuthentication]
        //[FaultContract(typeof(ServiceFault))]
        [EventBroker(eventName: "Account DSP Request")]
        AccountDSPReqestResultDto UpdateAccountDSPReqest(AccountDSPRequestDto userDtoInfo);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountDSPRequestListResultDto QueryByCratiriaForAccountUsers(UserCriteriaBase criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        UserDto GetAccountDSPRequestByRequestCode(string RequestCode);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        AccountDSPRequestDto GetAccountDSPRequest(ValueMessageWrapper<int> id);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AccountDSPRequestDto GetAccountDSPRequestByEmail(string emailAddress);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]

        List<CompanyTypeDto> GetCompanyTypes();
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAllForUI();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SetAccountUser(ValueMessageWrapper<int> accountid);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateOperationContext(AdFalconUserInfo userInof);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> getAccountUser();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void InsertLastLoginDateAuditTrial(string userEmail);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        string GetUserNameById(ValueMessageWrapper<int> userId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> InvitationAcceptedCount(string email);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> InvitationCount(string email);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<bool> CheckInvitationAlreadyRegistred(CheckInvitationAlreadyRegistredRequest request);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> InvitationAcceptedCountByCode(string Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        string MD5Encryptiontest(string originalText);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void MakeUserSecondPrimaryUser(ValueMessageWrapper<int> userId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        [NoAuthentication]
        ValueMessageWrapper<bool> IsUserSecondPrimaryUser(UserAccountMessage request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        [NoAuthentication]
        ValueMessageWrapper<bool> IsUserReadOnlyUser(UserAccountMessage request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateUserType(UpdateUserTypeRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> getInvitationId(string email);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountReadOnlyUserDto> GetAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item);
    }
}
