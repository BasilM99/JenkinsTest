using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ArabyAds.AdFalcon.Business.Domain.Exceptions;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Exceptions.Account;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.Security;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework.UserInfo;
using ArabyAds.Framework.Persistence;
using ArabyAds.Framework.Resources;
using ArabyAds.Framework.Utilities.EmailsSender;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;
using NHibernate;
using NHibernate.Criterion;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DomainServices.AuditTrial.Repositories;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using FluentNHibernate.Utils;

namespace ArabyAds.AdFalcon.Services.Services
{
    public class UserService : IUserService
    {
        private ILanguageRepository _languageRepository = null;
        private ICompanyTypeRepository _companyTypeRepository;
        private IUserDomainManager _userDomainService;
        private IUserRepository _userRepository;
        private IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository;
        private ISecurityService _securityService;
        private IAccountRepository _accountRepository;
        private IConfigurationManager _configurationManager;
        private IAccountInvitationRepository _accountInvitationRepository;
        private IAppSiteRepository _appSiteRepository;
        private IMailSender _mailSender;
        private IPortalPermisionRepository _PortalPermisionRepository;
        private IBuyerRepository _BuyerRepository;
        private ISSPPartnerRepository _SSPPartnerRepository;
        private IAccountDSPRequestRepository _AccountDSPRequestRepository;
        private IObjectTypeRepository _ObjectTypeRepository;
        private IObjectActionRepository _ObjectActionRepository;
        private IUserAccountsRepository _UserAccountsRepository;

        private IAdvertiserAccountUserRepository _AdvertiserAccountReadOnlyUserRepository;
        private IAdvertiserAccountRepository _AdvertiserAccountRepository;



        public UserService(IUserRepository userRepository,
                           ISecurityService securityService,
                            IAccountRepository accountRepository,
                            IMailSender mailSender,
                            IBuyerRepository BuyerRepository,
IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository,
IAccountInvitationRepository accountInvitationRepository,
                            IPortalPermisionRepository portalPermisionRepository,
        IConfigurationManager configurationManager,
            IAppSiteRepository appSiteRepository, ISSPPartnerRepository SSPPartnerRepository, IAccountDSPRequestRepository AccountDSPRequestRepository, ICompanyTypeRepository companyTypeRepository, ILanguageRepository languageRepository

            , IUserAccountsRepository UserAccountsRepository
                , IObjectTypeRepository objectTypeRepository
                , IObjectActionRepository objectActionRepository
            , IAdvertiserAccountUserRepository AdvertiserAccountReadOnlyUserRepository
            , IAdvertiserAccountRepository AdvertiserAccountRepository
           )
        {
            this._AdvertiserAccountRepository = AdvertiserAccountRepository;
            this._userRepository = userRepository;
            this._userDomainService = new UserDomainManager(userRepository, accountRepository);
            _accountInvitationRepository = accountInvitationRepository;
            _securityService = securityService;
            _accountRepository = accountRepository;
            _configurationManager = configurationManager;
            _appSiteRepository = appSiteRepository;
            _PortalPermisionRepository = portalPermisionRepository;
            this._AccountPortalPermissionsRepository = _AccountPortalPermissionsRepository;
            _mailSender = mailSender;
            _BuyerRepository = BuyerRepository;
            _SSPPartnerRepository = SSPPartnerRepository;
            _AccountDSPRequestRepository = AccountDSPRequestRepository;
            _companyTypeRepository = companyTypeRepository;
            _languageRepository = languageRepository;

            this._UserAccountsRepository = UserAccountsRepository;


            this._ObjectTypeRepository = objectTypeRepository;
            this._ObjectActionRepository = objectActionRepository;

            this._AdvertiserAccountReadOnlyUserRepository = AdvertiserAccountReadOnlyUserRepository;

        }

        public ValueMessageWrapper<bool> CheckUserEmail(CheckUserEmailRequest request)
        {
            return ValueMessageWrapper.Create(_userDomainService.GetUserByEmail(request.EmailAddress, request.CheckPendingEmail) != null ? true : false);
        }


        public UserDto ActivateUser(string activationCode)
        {
            User userInfo = _userRepository.Query(p => p.ActivationCode == activationCode).ToList().SingleOrDefault();
            if (userInfo == null)
                throw new DataNotFoundException();
            bool activateResult = _securityService.ActivateUser(ValueMessageWrapper<string>.Create(userInfo.EmailAddress)).Result.Value;
            //to do by mosab maps cannot be registered @ runtime 
            //Mapper.CreateMap<User, UserDto>().ForMember(p => p.Country, opt => opt.Ignore());
            //Mapper.CreateMap<User, UserDto>().ForMember(p => p.Language, opt => opt.Ignore());

            if (activateResult)
            {
                if (userInfo == null)
                    return null;

                userInfo.Activate();
                userInfo.Status = new UserStatus();
                userInfo.Status.SetActiveStatus();

                _userRepository.Save(userInfo);


                UserDto userDtoInfo = Mapping.MapperHelper.Map<UserDto>(userInfo);
                //userDtoInfo.AccountId = userInfo.Account.ID;

                return userDtoInfo;
            }
            else
            {
                throw new ActivateUserException();
            }
        }

        public UsersListResultDto QueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.UserCriteriaBase wcriteria)
        {


            UserCriteriaBase criteria = new UserCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);


            var result = new UsersListResultDto();
            int Count = 0;
            var list = _userRepository.QueryByCratiriaForUsers(criteria, out Count);

            var pageItems = list.ToList();
            if (pageItems != null)
            {
                foreach (var item in pageItems)
                {
                    var usert = item.UserAccounts.Where(M => M.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).FirstOrDefault();
                    if (usert != null)
                        item.UserType = usert.UserType;
                    else
                        item.UserType = UserType.Normal;
                }
            }
            result.Items = pageItems.Select(user => MapperHelper.Map<UserDto>(user)).ToList();
            result.TotalCount = Count;
            return result;
        }

        public IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAllForUI()
        {
            IEnumerable<Language> languageList = _languageRepository.Query(M => M.ForPortal == true);

            var items = languageList.Select(languageDto => MapperHelper.Map<LanguageDto>(languageDto)).ToList();

            return items;
        }
        public ValueMessageWrapper<bool> IsUserBlocked(ValueMessageWrapper<int> userId)
        {

            var useracc = this._UserAccountsRepository.Query(x => x.User.ID == userId.Value && x.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).SingleOrDefault();
            if (useracc.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value)
            {
                return ValueMessageWrapper.Create(useracc.User.Block);
            }
            else
            {

                throw new UnauthorizedAccessException("you are not authrized");
            }

        }

        public ValueMessageWrapper<bool> IsUserBlockedByEmail(string userEmail)
        {
            var user = _userRepository.Query(x => x.EmailAddress == userEmail).SingleOrDefault();
            //  if (user.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value)
            //{



            // _userRepository.Save(user);
            return ValueMessageWrapper.Create(user.Block);


        }

        public void InsertLastLoginDateAuditTrial(string userEmail)
        {
            var user = _userRepository.Query(x => x.EmailAddress == userEmail).SingleOrDefault();
            if (user == null)
            {
                return;
            }
            DateTime oldLogin = ArabyAds.Framework.Utilities.Environment.GetServerTime();
            try
            {
                oldLogin = _securityService.ValidateUser(ValueMessageWrapper<string>.Create(userEmail)).Result.Value;
            }
            //silent exce
            catch (Exception ex)
            {
                return;
            }
            user.LastLoginDate = Framework.Utilities.Environment.GetServerTime();
            var oldLoginstr = oldLogin.ToShortDateString() + " " + oldLogin.ToShortTimeString();

            var NewLoginstr = user.LastLoginDate.ToShortDateString() + " " + user.LastLoginDate.ToShortTimeString();
            string Details = @"<OA T=""ArabyAds.AdFalcon.Domain.Model.Account.User"" A=""Update"" E=""{0}""><PS><P N=""LastLoginDate""><OV>{1}</OV><NV>{2}</NV></P></PS></OA>";
            var actionTime = Framework.Utilities.Environment.GetServerTime();

            string ActionTimeStr = actionTime.ToString("yyyy-MM-dd HH:mm:ss");
            Details = string.Format(Details, user.ID, oldLoginstr, NewLoginstr);
            var objectId = _ObjectTypeRepository.GetByName("ArabyAds.AdFalcon.Domain.Model.Account.User");

            var rootobjectId = _ObjectTypeRepository.GetByName("ArabyAds.AdFalcon.Domain.Model.Account.Account");

            string insetSta = " INSERT INTO `audittrials` (`ObjectTypeID`, `ObjectActionId`, `ObjectId`, `UserID`, `AccountId`, `ActionTime`, `Details`, `RootObjectId`,`SessionId`) VALUES('" + objectId.ID + "', '2','" + user.ID + "','" + user.ID + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "','" + ActionTimeStr + "','" + Details + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "',:ByteArr);" + "INSERT INTO `audittrialsessionstat` (`ids`, `actiontime`, `user`, `RootObjectId`, `RootObjectTypeId`, `AccountId`,`sessionId`) VALUES( LAST_INSERT_ID(),'" + ActionTimeStr + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "','" + rootobjectId.ID + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "',:ByteArr);"; /*+ "INSERT INTO `audittrialstat` (`LastActionTime`, `RootObjectId`, `RootObjectTypeId`, `LastUser`, `AccountId`) VALUES('" + ActionTimeStr + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "','" + rootobjectId.ID + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value + "','" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value + "');";*/

            ;
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQuery query = nhibernateSession.CreateSQLQuery(insetSta);
            query.SetBinary("ByteArr", Guid.NewGuid().ToByteArray());
            query.UniqueResult();
            AuditTrialStat AuditTrialStatAlias = null;

            var auditStat = new AuditTrialStat
            {
                LastActionTime = actionTime,
                LastUser = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value,
                RootObjectTypeId = rootobjectId.ID,
                RootObjectId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value

            };

            IQueryOver<AuditTrialStat, AuditTrialStat> rootQuery = nhibernateSession.QueryOver<AuditTrialStat>(() => AuditTrialStatAlias);


            rootQuery.Where(M => M.RootObjectTypeId == rootobjectId.ID);
            rootQuery.Where(M => M.RootObjectId == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            //  rootQuery.Where(M => M.ObjectType.ID == objectTypeId);
            //  rootQuery.Where(M => M.ObjectId== objectId);
            var idStat = rootQuery.Select(M => M.ID).SingleOrDefault<int>();
            auditStat.ID = idStat;
            // session.Lock(auditStat, LockMode.None
            //);
            if (auditStat.ID == 0)
            {
                auditStat.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                nhibernateSession.Save(auditStat);

            }
            else
            {

                auditStat.LastActionTime = actionTime;
                auditStat.LastUser = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

                nhibernateSession.CreateQuery(@"update AuditTrialStat objectRootStat set objectRootStat.LastUser = " + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value + " , objectRootStat.LastActionTime='" + actionTime.ToString("yyyy-MM-dd HH:mm:ss") + "' where objectRootStat.id=" + auditStat.ID)/*.SetLockMode("objectRootStat", LockMode.None)*/
.ExecuteUpdate();
            }


            // nhibernateSession.Dispose();







        }
        public void BlockUser(ValueMessageWrapper<int> userId)
        {
            var useracc = _UserAccountsRepository.Query(x => x.User.ID == userId.Value && x.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).SingleOrDefault();
            if (useracc.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                if (useracc.User.Block)
                    useracc.User.UnBlockUser();
                else
                    useracc.User.BlockUser();

                _userRepository.Save(useracc.User);
            }
            else
            {

                throw new UnauthorizedAccessException("you are not authrized");
            }
        }

        public void MakeUserSecondPrimaryUser(ValueMessageWrapper<int> userId)
        {
            var useracc = _UserAccountsRepository.Query(x => x.User.ID == userId.Value && x.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).SingleOrDefault();
            if (useracc.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                if (useracc.UserType == UserType.Primary)
                    useracc.UserType = UserType.Normal;
                else
                    useracc.UserType = UserType.Primary;

                _UserAccountsRepository.Save(useracc);
            }
            else
            {

                throw new UnauthorizedAccessException("you are not authrized");
            }
        }


        public void UpdateUserType(UpdateUserTypeRequest request)
        {
            var useracc = _UserAccountsRepository.Query(x => x.User.ID == request.UserId && x.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).SingleOrDefault();
            if (useracc.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                useracc.UserType = request.UserType;

                _UserAccountsRepository.Save(useracc);

                if (!string.IsNullOrEmpty(request.Ids))
                {
                    AdvertiserAccountSettingsForReadOnly oAdvertiserAccountSettingsForReadOnly = new AdvertiserAccountSettingsForReadOnly()
                    {
                        UserId = useracc.User.ID,
                        UserType = (UserType)request.UserType,
                        LinkIds = request.Ids.Split(',').Select(Int32.Parse).ToList()
                    };

                    if (request.UserType == UserType.ReadOnly)
                        SaveAdvertiserAccountReadOnlySettings(oAdvertiserAccountSettingsForReadOnly);
                }
            }
            else
            {

                throw new UnauthorizedAccessException("you are not authrized");
            }
        }

        public ValueMessageWrapper<bool> IsUserSecondPrimaryUser(UserAccountMessage request)
        {
            var useracc = _UserAccountsRepository.Query(x => x.User.ID == request.UserId && x.Account.ID == request.AccountId).SingleOrDefault();

            return ValueMessageWrapper.Create(useracc.UserType == UserType.Primary);

        }
        public ValueMessageWrapper<bool> IsUserReadOnlyUser(UserAccountMessage request)
        {
            var useracc = _UserAccountsRepository.Query(x => x.User.ID == request.UserId && x.Account.ID == request.AccountId).SingleOrDefault();

            return ValueMessageWrapper.Create(useracc.UserType == UserType.ReadOnly);

        }
        public UsersListResultDto GetSSPUsers(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria wcriteria)
        {
            AllAppSiteCriteria criteria = new AllAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new UsersListResultDto();
            AllAppSiteCriteria appCriteria = new AllAppSiteCriteria();
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
                appCriteria.UserId = UserId;
            }

            if (criteria.IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
            {

                appCriteria.UserId = null;
                criteria.UserId = null;
            }
            //IEnumerable<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite> list1 = UnitOfWork.Current.EntitySet<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Where(
            //          p => string.IsNullOrEmpty(criteria.SubPublisherId) || p.SubAppsites.Any(v => v.SubPublisherId == criteria.SubPublisherId));
            //IEnumerable<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite> list1 = UnitOfWork.Current.EntitySet<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Where(
            //criteria.GetExpression());
            int count = 0;
            criteria.StatusId = AppSiteStatus.Active.ID;
            //var publisherAccountsIds = list1.Select(x => x.Account.ID).Distinct().ToList();
            //// var publisherAccountsIds = _appSiteRepository.GetAll().Where(criteria.GetWhere()).Select(x => x.Account.ID).Distinct().ToList();
            //var list = _userRepository.GetAll().Where(x => publisherAccountsIds.Contains(x.Account.ID));//.Where(x=>x .Account.ID in x. ;
            //var pageItems = list.OrderBy(c => c.FirstName).Skip((criteria.Page - 1) * criteria.Size).Take(criteria.Size).ToList();

            //var pageItems = _userRepository.GetSSPPartners(criteria, out count).ToList();
            //var publisherAccountsIds = pageItems.Select(x => x.Account.ID).Distinct().ToList();

            var sspPartnersAll = _SSPPartnerRepository.Query(M => M.IsDeleted == false && M.Visible == true).ToList();

            var sspPartners = sspPartnersAll.ToList();

            //var resultUser= pageItems.Select(user => MapperHelper.Map<UserDto>(user)).ToList();
            IList<UserDto> usersList = new List<UserDto>();
            result.Items = new List<UserDto>();
            foreach (var sspPartner in sspPartners)
            {

                UserDto dto = new UserDto();
                dto.Id = sspPartner.ID;
                dto.AccountId = sspPartner.Account.ID;
                dto.AccountName = sspPartner.Name;

                dto.TaggingAllowed = sspPartner.TaggingAllowed;
                dto.DisallowGeofenceLessThanRadius = sspPartner.DisallowGeofenceLessThanRadius;
                dto.DisallowGeofenceLessThanRadius = sspPartner.DisallowGeofenceLessThanRadius;
                dto.DisallowGeofenceLessThanRadius = sspPartner.DisallowGeofenceLessThanRadius;

                dto.AppSiteId = sspPartner.AppSite.ID;
                dto.FingerPrintAllowed = sspPartner.FingerPrintAllowed;
                dto.AllowExchangeCreativeFormat = sspPartner.AllowExchangeCreativeFormat;
                if (sspPartner.Icon != null)
                    dto.ImageId = sspPartner.Icon.ID;
                // if (!(usersList.Where(M => M.AccountId == item.AccountId).ToList().Count() > 0))
                // {

                usersList.Add(dto);
                //}


            }
            result.Items = usersList;
            result.TotalCount = sspPartnersAll.Count;
            //foreach (var sspPartner in resultUser)
            //{
            //    var item = sspPartnersAll.Where(M => M.Account.ID == sspPartner.AccountId).First();
            //    sspPartner.AccountName = item.Name + "-" + sspPartner.AccountName;



            //}

            //result.Items = resultUser;
            //result.TotalCount = count;


            return result;
        }
        public UsersListResultDto GetPublisherUsers(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria wcriteria)
        {


            AllAppSiteCriteria criteria = new AllAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new UsersListResultDto();
            AllAppSiteCriteria appCriteria = new AllAppSiteCriteria();
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.UserId = UserId;
                appCriteria.UserId = UserId;
            }

            if (criteria.IgnoreIsPrimaryUser.HasValue && !IsPrimaryUser)
            {

                appCriteria.UserId = null;
                criteria.UserId = null;
            }
            //IEnumerable<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite> list1 = UnitOfWork.Current.EntitySet<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Where(
            //          p => string.IsNullOrEmpty(criteria.SubPublisherId) || p.SubAppsites.Any(v => v.SubPublisherId == criteria.SubPublisherId));
            //IEnumerable<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite> list1 = UnitOfWork.Current.EntitySet<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Where(
            //criteria.GetExpression());
            int count = 0;
            criteria.StatusId = AppSiteStatus.Active.ID;
            //var publisherAccountsIds = list1.Select(x => x.Account.ID).Distinct().ToList();
            //// var publisherAccountsIds = _appSiteRepository.GetAll().Where(criteria.GetWhere()).Select(x => x.Account.ID).Distinct().ToList();
            //var list = _userRepository.GetAll().Where(x => publisherAccountsIds.Contains(x.Account.ID));//.Where(x=>x .Account.ID in x. ;
            //var pageItems = list.OrderBy(c => c.FirstName).Skip((criteria.Page - 1) * criteria.Size).Take(criteria.Size).ToList();

            var pageItems = _userRepository.GetPublishedUsers(criteria, out count).ToList();
            result.Items = pageItems.Select(user => MapperHelper.Map<UserDto>(user)).ToList();
            result.TotalCount = count; ;

            return result;
        }
        public UserDto GetUserByEmail(CheckUserEmailRequest request)
        {
            User userInfo = _userDomainService.GetUserByEmail(request.EmailAddress, request.CheckPendingEmail);

            if (userInfo == null)
                return null;

            var userDtoInfo = MapperHelper.Map<UserDto>(userInfo);
            //TODO:this is temp code , remove it and make sure that the country and language is been mapped correctly
            if (userInfo.Country != null)
                userDtoInfo.Country = userInfo.Country.ID;
            if (userInfo.Language != null)
                userDtoInfo.Language = userInfo.Language.ID;

            userDtoInfo.IsPrimaryUser = userInfo.Account.PrimaryUser.ID == userInfo.ID;
            userDtoInfo.AccountId = userInfo.Account.ID;
            userDtoInfo.AccountRole = (int)userInfo.Account.AccountRole;

            userDtoInfo.UserAgreementVersion = userInfo.Account.UserAgreementVersion;
            userDtoInfo.AllowAPIAccess = userInfo.Account.AllowAPIAccess;
            userDtoInfo.VATValue = userInfo.GetVATValue();
            //userDtoInfo.AdPermission = GetAccountAdPermissions(userInfo.Account.ID);
            return userDtoInfo;
        }
        #region Invitation

        public InvitationListDto InvitationQueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountInvitationCriteria wcriteria)
        {
            AccountInvitationCriteria criteria = new AccountInvitationCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            var result = new InvitationListDto();
            IEnumerable<Domain.Model.Account.AccountInvitation> list = null;
            if (criteria.invitationcode == null)
            {
                criteria.invitationcode = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _accountInvitationRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _accountInvitationRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(x => MapperHelper.Map<InvitationDto>(x)).ToList();

            result.Items = returnList;
            result.TotalCount = _accountInvitationRepository.Query(criteria.GetExpression()).Count();

            return result;
        }

        public ValueMessageWrapper<bool> CheckInvitationAlreadyRegistred(CheckInvitationAlreadyRegistredRequest request)
        {


            AccountInvitation invitatin = _accountInvitationRepository.Query(M => M.InvitationCode == request.Invitation).SingleOrDefault();


            var users = _userRepository.Query(x => x.EmailAddress.ToLower() == request.Email.ToLower()).ToList();
            int alReadyReg = 0;

            if (users != null)
                alReadyReg = users.Count;
            if (alReadyReg > 0)
            {
                var Ids = (users.Select(X => X.ID).ToArray());
                var count = _accountRepository.Query(M => Ids.Contains(M.PrimaryUser.ID) && M.AccountRole != invitatin.Account.AccountRole).Count();
                if (count > 0)
                {
                    return ValueMessageWrapper.Create(true);

                }


            }
            return ValueMessageWrapper.Create(false);


        }
        public InviteResponse invite(InviteRequest request)
        {
            var response = new InviteResponse { Invitationcode = string.Empty };
            try
            {
                int invitations = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == request.Email.ToLower() && x.Account.ID == (int)OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).Count();
                var users = _userRepository.Query(x => x.EmailAddress.ToLower() == request.Email.ToLower()).ToList();
                int alReadyReg = 0;
                int idInvitation = 0;
                if (users != null)
                    alReadyReg = users.Count;
                if (invitations + alReadyReg == 0)
                {
                    response.Invitationcode = DoInvitation(request.Email, request.UserType, out idInvitation);

                    if (!string.IsNullOrEmpty(request.IdAdvs))
                    {
                        AdvertiserAccountSettingsForReadOnly oAdvertiserAccountSettingsForReadOnly = new AdvertiserAccountSettingsForReadOnly()
                        {
                            InvitationId = idInvitation,
                            UserType = (UserType)request.UserType,
                            LinkIds = request.IdAdvs.Split(',').Select(Int32.Parse).ToList()
                        };

                        if (request.UserType == UserType.ReadOnly)
                            SaveAdvertiserAccountReadOnlySettings(oAdvertiserAccountSettingsForReadOnly);
                    }

                    //invitationcode = AccountInvitation.InvitationCode;
                    response.Success = true;
                    return response;
                }
                else if (alReadyReg > 0)
                {
                    var Ids = (users.Select(X => X.ID).ToArray());
                    var count = _accountRepository.Query(M => Ids.Contains(M.PrimaryUser.ID) && M.AccountRole == (AccountRole)OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole).Count();
                    if (count > 0)
                        throw new Exception(ResourceManager.Instance.GetResource("AlreadyRegistered", "Register"));
                    else
                    {
                        if (invitations == 0)
                        {
                            response.Invitationcode = DoInvitation(request.Email, request.UserType, out idInvitation);

                            if (!string.IsNullOrEmpty(request.IdAdvs))
                            {
                                AdvertiserAccountSettingsForReadOnly oAdvertiserAccountSettingsForReadOnly = new AdvertiserAccountSettingsForReadOnly()
                                {
                                    InvitationId = idInvitation,
                                    UserType = (UserType)request.UserType,
                                    LinkIds = request.IdAdvs.Split(',').Select(Int32.Parse).ToList()
                                };

                                if (request.UserType == UserType.ReadOnly)
                                    SaveAdvertiserAccountReadOnlySettings(oAdvertiserAccountSettingsForReadOnly);
                            }
                            response.Success = true;
                            return response;
                        }
                        else
                        {
                            throw new Exception(ResourceManager.Instance.GetResource("AlreadyInvited", "Invite"));

                        }
                    }

                }
                else if (invitations > 0)
                {
                    throw new Exception(ResourceManager.Instance.GetResource("AlreadyInvited", "Invite"));

                }


                return response;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public ValueMessageWrapper<int> getInvitationId(string email)
        {

            var itemOne = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == email.ToLower() && x.Account.ID == (int)OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).SingleOrDefault();

            return ValueMessageWrapper.Create(itemOne.ID);
        }
        public ValueMessageWrapper<int> InvitationCount(string email)
        {
            int invitations = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == email.ToLower() && x.IsAccepted == false).Count();

            return ValueMessageWrapper.Create(invitations);
        }
        public ValueMessageWrapper<int> InvitationAcceptedCount(string email)
        {
            int invitations = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == email.ToLower() && x.IsAccepted == true).Count();

            return ValueMessageWrapper.Create(invitations);
        }
        public ValueMessageWrapper<int> InvitationAcceptedCountByCode(string Id)
        {
            int invitations = _accountInvitationRepository.Query(x => x.IsAccepted == true && x.InvitationCode == Id).Count();

            return ValueMessageWrapper.Create(invitations);
        }
        private string DoInvitation(string email, UserType userType, out int IdInvite)
        {

            AccountInvitation AccountInvitation = new AccountInvitation();
            AccountInvitation.InvitationDate = Framework.Utilities.Environment.GetServerTime();
            AccountInvitation.InvitationCode = MD5Encryption(Guid.NewGuid().ToString());
            AccountInvitation.EmailAddress = email;
            AccountInvitation.Account = _accountRepository.Get((int)OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            AccountInvitation.UserType = userType;

            _accountInvitationRepository.Save(AccountInvitation);
            IdInvite = AccountInvitation.ID;
            return AccountInvitation.InvitationCode;
        }
        //public string getInvitedEmail(string InvitationCode)
        //{
        //    if (!string.IsNullOrEmpty(InvitationCode))
        //    {
        //        var Invitation = _accountInvitationRepository.Query(x => x.InvitationCode == InvitationCode).FirstOrDefault();
        //        return Invitation != null && Invitation.IsAccepted == false ? Invitation.EmailAddress : null;
        //    }
        //    return null;
        //}

        public InvitationDto GetInvitation(string InvitationCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(InvitationCode))
                {
                    var item = _accountInvitationRepository.Query(x => x.InvitationCode == InvitationCode).FirstOrDefault();

                    var result = MapperHelper.Map<InvitationDto>(item);
                    result.CompanyName = _accountRepository.Get(result.accountid).PrimaryUser.Company;
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        #endregion
        public UserDto GetUserByAccount(UserAccountMessage request)
        {
            IEnumerable<UserAccounts> list = null;
            if (request.UserId.HasValue)
                list = _UserAccountsRepository.Query(x => x.Account.ID == request.AccountId && x.User.ID == request.UserId).ToList();
            else
                list = _UserAccountsRepository.Query(x => x.Account.ID == request.AccountId).ToList();
            if (list.FirstOrDefault() == null)
                return null;
            else
            {
                User userInfo = list.First().User;
                var userDtoInfo = MapperHelper.Map<UserDto>(userInfo);
                //TODO:this is temp code , remove it and make sure that the country and language is been mapped correctly
                if (userInfo.Country != null)
                    userDtoInfo.Country = userInfo.Country.ID;
                if (userInfo.Language != null)
                    userDtoInfo.Language = userInfo.Language.ID;

                return userDtoInfo;
            }
        }
        public UserDto GetUserById(ValueMessageWrapper<int> userId)
        {
            var list = _userRepository.Query(x => x.ID == userId.Value).ToList();
            if (list.FirstOrDefault() == null)
                return null;
            else
            {
                User userInfo = list.First();
                var accountInfo = _accountRepository.Query(p => p.ID == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).FirstOrDefault();
                var userDtoInfo = MapperHelper.Map<UserDto>(userInfo);
                //TODO:this is temp code , remove it and make sure that the country and language is been mapped correctly
                if (userInfo.Country != null)
                    userDtoInfo.Country = userInfo.Country.ID;
                if (userInfo.Language != null)
                    userDtoInfo.Language = userInfo.Language.ID;
                userDtoInfo.buyerId = accountInfo.buyer != null ? accountInfo.buyer.ID : new Nullable<int>();
                userDtoInfo.buyerCode = accountInfo.buyer != null ? accountInfo.buyer.Code : string.Empty;
                /*if (string.IsNullOrEmpty(userDtoInfo.buyerCode))
                {
                    userDtoInfo.buyerCode =

                }*/
                return userDtoInfo;
            }
        }

        public string GetUserNameById(ValueMessageWrapper<int> userId)
        {
            var list = _userRepository.Query(x => x.ID == userId.Value).ToList();
            if (list.FirstOrDefault() == null)
                return null;
            else
            {
                User userInfo = list.First();
                return userInfo.FirstName + " " + userInfo.LastName;

            }
        }
        /// <summary>
        /// Get All users information
        /// </summary>
        /// <returns>List of UserDto</returns>
        public IEnumerable<UserDto> GetAllUser()
        {
            IEnumerable<User> list = _userDomainService.GetAllUser();
            return list.Select(userDto => MapperHelper.Map<UserDto>(userDto)).ToList();
        }

        public ChangeEmailDto UpdateUser(UserDto userDtoInfo)
        {
            ChangeEmailDto changeEmail = new ChangeEmailDto();
            //var userInfo =_userRepository.Query(p => p.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId).ToList().SingleOrDefault();
            var userInfo = _userRepository.Query(p => p.ID == userDtoInfo.Id).ToList().SingleOrDefault();
            string hashing = string.Empty;

            userDtoInfo.Id = userInfo.ID;
            userDtoInfo.ActivationCode = userInfo.ActivationCode;


            if (userInfo != null)
            {

                userDtoInfo.Password = userInfo.Password;

                if (userInfo.EmailAddress == userDtoInfo.EmailAddress && !string.IsNullOrEmpty(userInfo.PendingEmailAddress))
                {
                    userInfo.PendingEmailAddress = string.Empty;
                    userInfo.Activate();
                    userInfo.Status = new UserStatus();
                    userInfo.Status.SetActiveStatus();
                    _securityService.ActivateUser(ValueMessageWrapper<string>.Create(userInfo.EmailAddress));
                }
                else
                {
                    if ((userDtoInfo.EmailAddress != userInfo.EmailAddress && userDtoInfo.EmailAddress != userInfo.PendingEmailAddress))
                    {
                        if (!CheckUserEmail(new CheckUserEmailRequest { EmailAddress = userDtoInfo.EmailAddress, CheckPendingEmail = true }).Value)
                        {
                            userInfo.PendingEmailAddress = userDtoInfo.EmailAddress;
                            userDtoInfo.EmailAddress = userInfo.EmailAddress;
                            userInfo.SetActivationCode();
                            userInfo.Status = new UserStatus();
                            userInfo.Status.SetPendingStatus();
                            _securityService.DeactivateUser(ValueMessageWrapper<string>.Create(userInfo.EmailAddress));
                            hashing = MD5Encryption(userInfo.ActivationCode + userInfo.PendingEmailAddress);
                        }
                        else
                        {
                            throw new UserEmailAlreadyExistsException();
                        }
                    }
                    else
                    {
                        if ((userDtoInfo.EmailAddress != userInfo.EmailAddress && userDtoInfo.EmailAddress == userInfo.PendingEmailAddress))
                        {
                            userDtoInfo.EmailAddress = userInfo.EmailAddress;
                        }
                    }
                }

                userDtoInfo.ActivationCode = userInfo.ActivationCode;

                // MapperHelper.Map(userDtoInfo, userInfo);
                userInfo.LastName = userDtoInfo.LastName;
                userInfo.FirstName = userDtoInfo.FirstName;
                userInfo.Phone = userDtoInfo.Phone;
                userInfo.State = userDtoInfo.State;
                userInfo.Company = userDtoInfo.Company;
                userInfo.Address2 = userDtoInfo.Address2;
                userInfo.Address1 = userDtoInfo.Address1;
                userInfo.City = userDtoInfo.City;
                userInfo.Postal = userDtoInfo.Postal;
                userInfo.IsAllowNotifications = userDtoInfo.IsAllowNotifications;
                userInfo.Language = new Language { ID = userDtoInfo.Language };
                userInfo.Country = new Country { ID = userDtoInfo.Country };
                userInfo.RegistredIP = !string.IsNullOrEmpty(userDtoInfo.IPAddress) ? Encoding.ASCII.GetBytes(userDtoInfo.IPAddress) : null;
                var accountInfo = _accountRepository.Query(p => p.ID == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).FirstOrDefault();
                accountInfo.Name = userInfo.GetAccountName();

                _userRepository.Save(userInfo);
                _accountRepository.Save(accountInfo);
            }

            //ChangeEmailDto changeEmail = null;

            changeEmail.buyerId = userDtoInfo.buyerId;
            if (!string.IsNullOrEmpty(hashing))
            {
                changeEmail = new ChangeEmailDto();
                changeEmail.ActivationCode = userInfo.ActivationCode;
                changeEmail.Hashing = hashing;
            }

            return changeEmail;
        }
        public void SaveAccountBuyer(SaveAccountBuyerRequest request)
        {

            var userInfo = _accountRepository.Query(p => p.ID == Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value).FirstOrDefault();

            Buyer buyer = new Buyer();
            if (request.BuyerId.HasValue && request.BuyerId > 0 && !string.IsNullOrEmpty(request.BuyerCode))
            {

                buyer = _BuyerRepository.Get((int)request.BuyerId);
                if (buyer.Code != request.BuyerCode)
                {
                    buyer = new Buyer { ID = 0, Code = request.BuyerCode };
                    var resultBuyer = _BuyerRepository.Query(M => M.Code == request.BuyerCode).ToList();
                    if (resultBuyer.Count == 0)
                    {
                        _BuyerRepository.Save(buyer);

                        request.BuyerId = buyer.ID;
                    }
                    else
                    {
                        throw new Exception("Duplicated Buyer Code");
                    }


                }



            }
            else if (!string.IsNullOrEmpty(request.BuyerCode) && !string.IsNullOrWhiteSpace(request.BuyerCode) && !(request.BuyerId.HasValue || request.BuyerId > 0))
            {
                if (request.BuyerCode.Length < 7)
                {
                    buyer = new Buyer { ID = 0, Code = request.BuyerCode };
                    var resultBuyer = _BuyerRepository.Query(M => M.Code == request.BuyerCode).ToList();
                    if (resultBuyer.Count == 0)
                    {
                        _BuyerRepository.Save(buyer);

                        request.BuyerId = buyer.ID;
                    }
                    else
                    {
                        throw new Exception("Duplicated Buyer Code");
                    }
                }
                else
                {
                    throw new Exception("Invalid Buyer Code Length");
                }
            }

            if (request.BuyerId.HasValue && request.BuyerId > 0 && !string.IsNullOrEmpty(request.BuyerCode))
            {

                var resultBuyer = _BuyerRepository.Query(M => M.Code == request.BuyerCode).ToList();

                if (resultBuyer.Count == 0)

                    userInfo.buyer = buyer;
                else
                {
                    if (resultBuyer.Count > 1)
                        throw new Exception("Duplicated Buyer Code");
                    else
                    {
                        var buyerItem = resultBuyer[0];

                        if (buyerItem.ID != request.BuyerId)
                        {

                            throw new Exception("Duplicated Buyer Code");
                        }
                        else
                        {
                            userInfo.buyer = buyer;

                        }
                    }
                }
            }
            else
            {
                userInfo.buyer = null;
                request.BuyerId = null;
            }
            _accountRepository.Save(userInfo);


        }
        public int GetAccountMaxBuyerCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            //IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            //query.SetString("CounterName", "AccountBuyer");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            //var count = query.UniqueResult();

            int max = nhibernateSession.QueryOver<Buyer>()
                  .Select(Projections.ProjectionList().Add(Projections.Max<Buyer>(x => x.ID))).List<int>().First();

            //if (max != null)
            //{
            //    return (int)max + 1;

            //}
            //else
            //{
            //    throw new Exception("somthing went wrong");
            //}

            int count = _accountRepository.Query(x => x.buyer.ID == max).Count();
            if (count > 0)
            {
                Buyer buyer = new Buyer();
                buyer.Code = "0";
                _BuyerRepository.Save(buyer);
                buyer.Code = buyer.ID.ToString();
                _BuyerRepository.Save(buyer);
                return Convert.ToInt32(buyer.Code);
            }
            else
            {
                return max;
            }
        }

        public ValueMessageWrapper<int> GetAccountBuyerCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            //IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            //query.SetString("CounterName", "AccountBuyer");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            //var count = query.UniqueResult();

            int max = nhibernateSession.QueryOver<Buyer>()
                  .Select(Projections.ProjectionList().Add(Projections.Max<Buyer>(x => x.ID))).List<int>().First();

            //if (max != null)
            //{
            //    return (int)max + 1;

            //}
            //else
            //{
            //    throw new Exception("somthing went wrong");
            //}

            int count = _accountRepository.Query(x => x.buyer.ID == max).Count();
            if (count > 0)
            {
                Buyer buyer = new Buyer();
                buyer.Code = "0";
                _BuyerRepository.Save(buyer);
                buyer.Code = buyer.ID.ToString();
                _BuyerRepository.Save(buyer);
                return ValueMessageWrapper.Create(Convert.ToInt32(buyer.Code));
            }
            else
            {
                return ValueMessageWrapper.Create(max);
            }
        }


        public ValueMessageWrapper<bool> CheckduplicateBuyer(string buyerCode)
        {
            int count = _BuyerRepository.Query(x => x.Code == buyerCode).Count();
            var buyer = _accountRepository.Get((int)Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId).buyer;
            var buyersCodes = _accountRepository.Query(M => M.buyer.Code == buyerCode).FirstOrDefault();
            if (count == 0 || (buyer != null && buyer.Code == buyerCode) || (count > 0 &&(buyersCodes == null  )))
                return ValueMessageWrapper.Create(false);
            else
                return ValueMessageWrapper.Create(true);
        }

        public ValueMessageWrapper<bool> ChangeEmail(ChangeEmailDto changeEmailDto)
        {

            var userInfo = _userRepository.Query(p => p.ActivationCode == changeEmailDto.ActivationCode).ToList().SingleOrDefault();

            bool result = false;

            if (userInfo != null)
            {
                if (changeEmailDto.Hashing == MD5Encryption(changeEmailDto.ActivationCode + userInfo.PendingEmailAddress))
                {
                    if (!CheckUserEmail(new CheckUserEmailRequest { EmailAddress = userInfo.PendingEmailAddress, CheckPendingEmail = false }).Value)
                    {
                        if (_securityService.ChangeUsernameandEmail(new ChangeUserEmailRequest { OldEmail = userInfo.EmailAddress, NewEmail = userInfo.PendingEmailAddress }).Result.Value == true)
                        {
                            userInfo.Activate();
                            userInfo.Status = new UserStatus();
                            userInfo.Status.SetActiveStatus();
                            _securityService.ActivateUser(ValueMessageWrapper<string>.Create(userInfo.PendingEmailAddress));
                            userInfo.EmailAddress = userInfo.PendingEmailAddress;
                            userInfo.PendingEmailAddress = string.Empty;

                            _userRepository.Save(userInfo);
                            result = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Email Used By Other User");
                    }
                }
                else
                {
                    throw new Exception("Hash values not matched");
                }
            }

            return ValueMessageWrapper.Create(result);
        }

        public ValueMessageWrapper<bool> ResetUserPassword(ChangeUserPasswordRequest request)
        {
            bool resetPassword = false;

            var userInfo = _userRepository.Query(p => p.EmailAddress == request.UserName && p.Status.ID == UserStatus.ActiveUser).ToList().SingleOrDefault();

            if (userInfo != null)
            {

                resetPassword = _securityService.ChangeUserPassword(new ChangeUserPasswordRequest { UserName = request.UserName, NewPassword = request.NewPassword }).Result.Value;

                if (resetPassword)
                {
                    userInfo.ChangePassword(request.NewPassword);
                    _userRepository.Save(userInfo);
                    resetPassword = true;
                }
            }
            else
            {
                userInfo = _userRepository.Query(p => p.EmailAddress == request.UserName && p.Status.ID == UserStatus.PendingUser).ToList().SingleOrDefault();
                if (userInfo != null)
                {
                    throw new NotActivatedUserException();
                }
            }

            return ValueMessageWrapper.Create(resetPassword);
        }

        public ValueMessageWrapper<bool> ResetUserPasswordByToken(ResetUserPasswordByTokenRequest request)
        {
            bool resetPassword = false;

            var userInfo = _userRepository.Query(p => p.AccountResetToken == request.Token && p.Status.ID == UserStatus.ActiveUser).ToList().SingleOrDefault();

            if (userInfo != null)
            {
                //sstring oldPass= decrypt(userInfo.Password);
                resetPassword = _securityService.ResetPassword(new ChangeUserPasswordRequest { UserName = userInfo.EmailAddress, OldPassword = null, NewPassword = request.NewPassword }).Result.Value;

                if (resetPassword)
                {
                    userInfo.ChangePassword(request.NewPassword);
                    userInfo.AccountResetToken = null;
                    _userRepository.Save(userInfo);
                    resetPassword = true;

                }
            }
            else
            {
                userInfo = _userRepository.Query(p => p.AccountResetToken == request.Token && p.Status.ID == UserStatus.PendingUser).ToList().SingleOrDefault();
                if (userInfo != null)
                {
                    throw new NotActivatedUserException();
                }
            }

            return ValueMessageWrapper.Create(resetPassword);
        }

        public ValueMessageWrapper<bool> SaveUserToken(SaveUserTokenRequest request)
        {

            var userInfo = _userRepository.Query(p => p.EmailAddress == request.Email && p.Status.ID == UserStatus.ActiveUser).ToList().SingleOrDefault();

            if (userInfo != null)
            {
                userInfo.AccountResetToken = request.Token;
                userInfo.TokenCreationDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                _userRepository.Save(userInfo);

                return ValueMessageWrapper.Create(true);
            }
            else
            {
                userInfo = _userRepository.Query(p => p.EmailAddress == request.Email && p.Status.ID == UserStatus.PendingUser).ToList().SingleOrDefault();
                if (userInfo != null)
                {
                    throw new NotActivatedUserException();
                }
            }

            return ValueMessageWrapper.Create(false);
        }

        public ValueMessageWrapper<bool> CheckUserToken(string token)
        {

            var userInfo = _userRepository.Query(p => p.AccountResetToken == token
                                                        && p.Status.ID == UserStatus.ActiveUser).Where(p => ArabyAds.Framework.Utilities.Environment.GetServerTime() < p.TokenCreationDate.AddDays(1)).ToList().SingleOrDefault();

            if (userInfo != null)
            {
                return ValueMessageWrapper.Create(true);
            }
            else
            {
                return ValueMessageWrapper.Create(false);
            }

        }

        public ValueMessageWrapper<bool> DeleteUser(ValueMessageWrapper<int> userId)
        {
            var userInfo = _userRepository.Query(p => p.ID == userId.Value).ToList().SingleOrDefault();

            var result = false;
            if (userInfo != null)
            {
                if (_securityService.DeactivateUser(ValueMessageWrapper<string>.Create(userInfo.EmailAddress)).Result.Value)
                {

                    userInfo.Status = new UserStatus();
                    userInfo.Status.SetDeletingStatus();

                    _userRepository.Save(userInfo);

                    result = true;
                }
            }

            return ValueMessageWrapper.Create(result);
        }

        public void ChangePassword(ChangeUserPasswordRequest request)
        {
            var currentUser = _userRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);

            bool resetPassword = _securityService.ChangeUserPassword(new ChangeUserPasswordRequest { UserName = currentUser.EmailAddress, OldPassword = request.OldPassword, NewPassword = request.NewPassword }).Result.Value;

            if (resetPassword)
            {
                currentUser.ChangePassword(request.NewPassword);
                _userRepository.Save(currentUser);
            }
            else
            {
                throw new ChangePasswordException();
            }
        }

        /// <summary>
        /// Impersonate Account
        /// </summary>
        /// <returns>true if the Operation is successfully done</returns>
        /// <param name="accountId"></param>
        public ImpersonatedAccountInfo Impersonate(ImpersonateRequest request)
        {
            var userInfo = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>();
            userInfo.AccountId = request.AccountId;
            ImpersonatedAccountInfo impersonatedAccount = null;
            if (request.AccountId.HasValue)
            {
                var account = _accountRepository.Get(request.AccountId.Value);
                userInfo.AllowAPIAccess = account.AllowAPIAccess;
                userInfo.IsPrimaryUser = true;
                userInfo.AccountRole = (int)account.AccountRole;
                var Permissions = GetAccountAdPermissions(ValueMessageWrapper.Create(userInfo.AccountId.Value)).ToArray();
                userInfo.Permissions = Permissions;

                userInfo.VATValue = account.GetVATValue();
                if (request.UserId.HasValue)
                {
                    userInfo.UserId = request.UserId;
                    userInfo.IsPrimaryUser = account.PrimaryUser.ID == request.UserId;
                }
                if (userInfo.IsPrimaryUser)
                {
                    impersonatedAccount = new ImpersonatedAccountInfo
                    {
                        AccountId = account.ID,
                        FirstName = account.PrimaryUser.FirstName,
                        LastName = account.PrimaryUser.LastName,
                        IsPrimaryUser = userInfo.IsPrimaryUser,
                        AllowAPIAccess = account.AllowAPIAccess,
                        AccountRole = (int)account.AccountRole
                    };
                }
                else
                {
                    var user = _userRepository.Get(request.UserId.Value);
                    impersonatedAccount = new ImpersonatedAccountInfo
                    {
                        AccountId = account.ID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsPrimaryUser = userInfo.IsPrimaryUser,
                        AllowAPIAccess = account.AllowAPIAccess,
                        AccountRole = (int)account.AccountRole
                    };

                }

            }
            else
            {
                impersonatedAccount = new ImpersonatedAccountInfo
                {
                    AccountId = userInfo.AccountId,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    AccountRole = 0
                };
            }

            userInfo.ImpersonatedAccount = impersonatedAccount;
            OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);

            return impersonatedAccount;
        }

        public void UpdateAgreement()
        {

            var currentAccount =
                _accountRepository.Get(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            if (currentAccount.AccountRole != AccountRole.DSP)
                currentAccount.UserAgreementVersion = _configurationManager.GetConfigurationSetting(null, null,
                                                                                               "UserAgreementVersion");

            else
                currentAccount.UserAgreementVersion = _configurationManager.GetConfigurationSetting(null, null,
                                                                                                             "DSPUserAgreementVersion");

            //update cached version
            var userInfo = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>();
            userInfo.UserAgreementVersion = currentAccount.UserAgreementVersion;
            OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);
        }

        public string getCurrentUserAgreement()
        {
            var currentAccount =
                   _accountRepository.Get(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            return currentAccount.UserAgreementVersion;
        }



        public ValueMessageWrapper<bool> CheckUserPassword(string password)
        {
            User currentUser = _userRepository.Query(p => p.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId).SingleOrDefault();
            bool result = false;

            if (currentUser != null)
            {
                //Osaleh 15-01-2012

                //User userInfo = new User();
                //userInfo.ChangePassword(password);

                //if (currentUser.Password.ToLower() == userInfo.Password.ToLower())
                //{
                //    result = true;
                //}

                // try to login using password
                var response = _securityService.AuthenticateUser(new AuthenticationRequest { UserName = currentUser.EmailAddress, Password = password }).Result;
                return ValueMessageWrapper.Create(response.Status == AuthenticateStatus.Success);
            }

            return ValueMessageWrapper.Create(result);
        }

        public string GetPendingEmailAddress(string email)
        {
            var userInfo = _userRepository.Query(p => p.EmailAddress == email).SingleOrDefault();

            if (userInfo != null)
            {
                return userInfo.PendingEmailAddress;
            }

            return null;
        }

        public ValueMessageWrapper<bool> GiveTakePermission(AccountAdPermissionsDto details)
        {
            try
            {
                var Permissions = _AccountPortalPermissionsRepository.Query(x => x.Account.ID == details.AccountId).ToList();

                string[] GivenPermissionAdCodes = details.GivenPermissionAdCodes.Split(',');

                GivenPermissionAdCodes = GivenPermissionAdCodes.Take(GivenPermissionAdCodes.Count()).ToArray();

                bool found = false;
                foreach (AccountPortalPermissions Permission in Permissions)
                {
                    foreach (string code in GivenPermissionAdCodes)
                    {
                        if ((int)Permission.Permission.Code == Convert.ToInt32(code))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        _AccountPortalPermissionsRepository.Remove(Permission);

                    found = false;
                }

                foreach (string code in GivenPermissionAdCodes)
                {
                    foreach (AccountPortalPermissions Permission in Permissions)
                    {
                        if ((int)Permission.Permission.Code == Convert.ToInt32(code))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        _AccountPortalPermissionsRepository.Save(new AccountPortalPermissions { Account = new ArabyAds.AdFalcon.Domain.Model.Account.Account { ID = details.AccountId }, Permission = _PortalPermisionRepository.Query(x => (int)x.Code == Convert.ToInt32(code)).SingleOrDefault() });

                    found = false;
                }


            }
            catch (Exception e)
            {

                throw e;
            }

            return ValueMessageWrapper.Create(true);

        }

        public string getAccountPermissionCode(ValueMessageWrapper<int> AccountId)
        {
            IList<PortalPermision> list = _AccountPortalPermissionsRepository.GetAccountAdPermissions(AccountId.Value).ToList();
            string codes = string.Empty;
            foreach (PortalPermision item in list)
            {
                codes += ((int)item.Code).ToString() + ",";

            }

            return codes;
        }

        public IList<int> GetAccountAdPermissions(ValueMessageWrapper<int> accountId)
        {
            try
            {
                IList<int> list = _AccountPortalPermissionsRepository.GetAccountAdPermissions(accountId.Value).Select(x => (int)x.Code).ToList();
                return list;

            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public string MD5Encryptiontest(string originalText)
        {
            var enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(originalText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            var str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }

            return str.ToString();
        }
        #region Private Members

        private string MD5Encryption(string originalText)
        {
            var enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(originalText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            var str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }

            return str.ToString();
        }

        #endregion



        #region opting issue




        public string CreateUserIdForOpting()
        {

            Server.Integration.Services.Client.IntegrationServiceClient clientServer = new Server.Integration.Services.Client.IntegrationServiceClient(ArabyAds.AdFalcon.Domain.Configuration.WebAPIHostAdServer);
            var Request = new Server.Integration.Services.Model.UpdateTrackingRequest();

            Request.TrackEnabled = true;

            Request.UserId = null;

            string userId = clientServer.UpdateTracking(Request).Result;
            //  string userId = _id64Generator.GenerateId().ToString();

            //CreateUserIdForOptingInDB(userId);
            return userId;

        }



        public void UpdateUserIdForOptingInDB(UpdateUserIdForOptingInDBRequest request)
        {

            Server.Integration.Services.Client.IntegrationServiceClient clientServer = new Server.Integration.Services.Client.IntegrationServiceClient(ArabyAds.AdFalcon.Domain.Configuration.WebAPIHostAdServer);
            var Request = new Server.Integration.Services.Model.UpdateTrackingRequest();

            Request.TrackEnabled = request.TrackEnabled;

            Request.UserId = request.UserId;

            // string userId = clientServer.UpdateTracking(Request).Result;


            string userid = clientServer.UpdateTracking(Request).Result;




        }

        public ValueMessageWrapper<bool> GetUserForOptingInDB(string userId)
        {



            //writePolicy = new Aerospike.Client.WritePolicy();
            // writePolicy.recordExistsAction = Aerospike.Client.RecordExistsAction.UPDATE;
            //  userProfileLifeTime = new TimeSpan(int.Parse(_configurationManager.GetConfigurationSetting(1, null, "UserProfileLifeTimeInDays")), 0, 0, 0);
            //writePolicy.expiration = (int)userProfileLifeTime.TotalSeconds;

            Server.Integration.Services.Client.IntegrationServiceClient clientServer = new Server.Integration.Services.Client.IntegrationServiceClient(ArabyAds.AdFalcon.Domain.Configuration.WebAPIHostAdServer);
            var Request = new Server.Integration.Services.Model.UpdateTrackingRequest();

            // Request.TrackEnabled = TrackEnabled;

            Request.UserId = userId;
            return ValueMessageWrapper.Create(clientServer.IsTrackingEnabled(userId).Result);





        }


        #endregion

        public void SetAccountUser(ValueMessageWrapper<int> accountid)
        {

            var user = _userRepository.Get(ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().UserId.Value);
            if (user.UserAccounts != null && user.UserAccounts.Where(M => M.Account.ID == accountid.Value).SingleOrDefault() != null)
            {
                user.LastAccountLogin = new Domain.Model.Account.Account { ID = accountid.Value };

                _userRepository.Save(user);
            }
        }
        public void UpdateOperationContext(AdFalconUserInfo userInof)
        {
            if (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().UserId.Value == userInof.UserId)
            {
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInof);
            }

        }
        public ValueMessageWrapper<int> getAccountUser()
        {

            var user = _userRepository.Get(ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().UserId.Value);

            if (user.LastAccountLogin != null)
                return ValueMessageWrapper.Create(user.LastAccountLogin.ID);

            return ValueMessageWrapper.Create(0);
        }
        #region  AccountDSPRequest


        public AccountDSPRequestListResultDto QueryByCratiriaForAccountUsers(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.UserCriteriaBase wcriteria)
        {
            UserCriteriaBase criteria = new UserCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);

            var result = new AccountDSPRequestListResultDto();
            int Count = 0;

            var list = _AccountDSPRequestRepository.QueryByCratiriaForAccountDSPRequests(criteria, out Count);


            var pageItems = list.ToList();
            result.Items = pageItems.Select(user => MapperHelper.Map<AccountDSPRequestDto>(user)).ToList();

            foreach (var pageItem in pageItems)
            {
                var dto = result.Items.Where(M => M.Id == pageItem.ID).Single();
                if (pageItem.CompanyType != null)
                    dto.CompanyTypeNameValue = pageItem.CompanyType.Name.GetValue();
                if (pageItem.Country != null)
                    dto.CountryNameValue = pageItem.Country.Name.GetValue();
            }
            result.TotalCount = Count;
            return result;
        }
        public AccountDSPReqestResultDto UpdateAccountDSPReqest(AccountDSPRequestDto userDtoInfo)
        {
            try
            {
                ChangeEmailDto changeEmail = new ChangeEmailDto();



                var userInfo = _AccountDSPRequestRepository.Query(p => p.ID == userDtoInfo.Id).ToList().SingleOrDefault();
                string hashing = string.Empty;
                if (userInfo == null)
                {
                    userInfo = new AccountDSPRequest();
                }

                userDtoInfo.Id = userInfo.ID;

                if (userInfo.ID == 0)
                {
                    if (!CheckAcccountDSPEmail(userDtoInfo.EmailAddress).Value)
                    {

                        MapperHelper.Map(userDtoInfo, userInfo);
                        userInfo.RequestDate = Framework.Utilities.Environment.GetServerTime();
                        userInfo.ActionDate = Framework.Utilities.Environment.GetServerTime();
                        userInfo.Status = AccountDSPRequestStatus.New;
                        _AccountDSPRequestRepository.Save(userInfo);
                    }
                }
                else
                {
                    if (userDtoInfo.Status == AccountDSPRequestStatus.Approved)
                    {
                        userInfo.SetRequestCode();
                        userInfo.Approver = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value };
                    }
                    userInfo.ActionDate = Framework.Utilities.Environment.GetServerTime();
                    userInfo.ActionNote = userDtoInfo.ActionNote;
                    userInfo.Status = userDtoInfo.Status;
                    _AccountDSPRequestRepository.Save(userInfo);



                }
                var result = _userRepository.GetUserAccountIdByEmail(userDtoInfo.EmailAddress);
                int? accountId = null;
                if (result > 0)
                    accountId = result;


                return new AccountDSPReqestResultDto { RequestCode = userInfo.RequestCode, Success = true, accountId = accountId.HasValue ? accountId.Value : 0, IsAlreadyRegistered = accountId.HasValue };
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public UserDto GetAccountDSPRequestByRequestCode(string RequestCode)
        {
            var userInfo = _AccountDSPRequestRepository.GetByRequestCode(RequestCode);
            UserDto userDtoInfo = null;
            if (userInfo != null && userInfo.Status == AccountDSPRequestStatus.Approved)
                userDtoInfo = MapperHelper.Map<UserDto>(userInfo);


            return userDtoInfo;

        }
        public AccountDSPRequestDto GetAccountDSPRequest(ValueMessageWrapper<int> id)
        {
            var userInfo = _AccountDSPRequestRepository.Query(p => p.ID == id.Value).SingleOrDefault();
            if (userInfo == null)
                return null;
            AccountDSPRequestDto userDtoInfo = null;
            MapperHelper.Map(userInfo, userDtoInfo);


            return userDtoInfo;

        }

        public AccountDSPRequestDto GetAccountDSPRequestByEmail(string emailAddress)
        {
            var userInfo = _AccountDSPRequestRepository.GetByEmailAddress(emailAddress);
            if (userInfo == null)
                return null;
            AccountDSPRequestDto userDtoInfo = null;
            MapperHelper.Map(userInfo, userDtoInfo);


            return userDtoInfo;


        }
        public ValueMessageWrapper<bool> CheckAcccountDSPEmail(string emailAddress)
        {

            return ValueMessageWrapper.Create(_AccountDSPRequestRepository.CheckEmailAddress(emailAddress) || _AccountDSPRequestRepository.CheckEmailAddressInvited(emailAddress));


        }
        public List<CompanyTypeDto> GetCompanyTypes()
        {
            var CompanyTypes = _companyTypeRepository.GetAll().ToList();

            return CompanyTypes.Select(CompanyType => MapperHelper.Map<CompanyTypeDto>(CompanyType)).ToList();

        }
        #endregion
        public void SaveAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item)
        {
            if (item.LinkIds != null && item.LinkIds.Count() > 0)
            {
                AdvertiserAccountUser AdvertiserAccountUser = null;
                foreach (var Assignment in item.LinkIds)
                {
                    AdvertiserAccountUser = null;
                    if (item.UserId > 0)
                        AdvertiserAccountUser = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.User.ID == item.UserId && x.Link.ID == Assignment).FirstOrDefault();
                    else if (item.InvitationId > 0)
                        AdvertiserAccountUser = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Invitation.ID == item.InvitationId && x.Link.ID == Assignment).FirstOrDefault();
                    if (AdvertiserAccountUser == null)
                    {
                        AdvertiserAccountUser = new AdvertiserAccountUser();
                        if (item.UserId > 0)
                            AdvertiserAccountUser.User = new User { ID = item.UserId };
                        AdvertiserAccountUser.Link = new AdvertiserAccount { ID = Assignment };
                        if (item.InvitationId > 0)
                            AdvertiserAccountUser.Invitation = new AccountInvitation { ID = item.InvitationId };

                        AdvertiserAccountUser.Read = true;
                        var advObj = _AdvertiserAccountRepository.Get(Assignment);
                        advObj.IsRestricted = true;
                        _AdvertiserAccountRepository.Save(advObj);
                    }
                    else
                    {

                        AdvertiserAccountUser.IsDeleted = false;

                        if (item.UserId > 0)
                            AdvertiserAccountUser.User = new User { ID = item.UserId };

                        if (item.InvitationId > 0)
                            AdvertiserAccountUser.Invitation = new AccountInvitation { ID = item.InvitationId };
                    }
                    _AdvertiserAccountReadOnlyUserRepository.Save(AdvertiserAccountUser);

                }
            }
            else
            {
                item.Assignments = new List<AdvertiserAccountReadOnlyUserDto>();
            }
            IList<AdvertiserAccountUser> all = new List<AdvertiserAccountUser>();
            if (item.InvitationId > 0)
                all = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Link.ID == item.InvitationId).ToList();
            else if (item.UserId > 0)
                all = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.User.ID == item.UserId).ToList();
            foreach (var tempItem in all)
            {
                if (item.LinkIds.Where(x => x == tempItem.Link.ID).Count() == 0)
                {
                    tempItem.IsDeleted = true;

                    _AdvertiserAccountReadOnlyUserRepository.Save(tempItem);
                }
            }






        }


        public IList<AdvertiserAccountReadOnlyUserDto> GetAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item)
        {
            IList<AdvertiserAccountUser> items = new List<AdvertiserAccountUser>();
            IList<AdvertiserAccountReadOnlyUserDto> Resultitems = new List<AdvertiserAccountReadOnlyUserDto>();
            if (item.UserId > 0)
                items = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.User.ID == item.UserId && x.IsDeleted == false).ToList();
            else if (item.InvitationId > 0)
                items = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Invitation.ID == item.InvitationId && x.IsDeleted == false).ToList();

            foreach (var itemOn in items)
            {
                var AdvertiserAccountReadOnlyUserOn = new AdvertiserAccountReadOnlyUserDto
                {
                    ID = itemOn.ID,

                };
                if (itemOn.User != null)
                    AdvertiserAccountReadOnlyUserOn.User = new UserDto { Id = itemOn.User.ID };

                if (itemOn.Invitation != null)
                    AdvertiserAccountReadOnlyUserOn.Invitation = new InvitationDto { id = itemOn.Invitation.ID };

                if (itemOn.Link != null)
                    AdvertiserAccountReadOnlyUserOn.Link = new AdvertiserAccountDto { Id = itemOn.Link.ID, Name = itemOn.Link.Name };


                Resultitems.Add(AdvertiserAccountReadOnlyUserOn);
            }

            return Resultitems;
        }
    }
}
