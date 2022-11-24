using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using ArabyAds.AdFalcon.Business.Domain.Exceptions;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework.EventBroker.Context;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.DomainServices.AuditTrial.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using ArabyAds.Framework.DomainServices.Localization.Repositories;
using ArabyAds.Framework.DomainServices;
using System.Reflection;
using System.Globalization;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
//using System.Web.Script.Serialization;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Domain.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using AccountN = ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using FluentNHibernate.Utils;

namespace ArabyAds.AdFalcon.Services.Services.Account
{
    public class AccountService : IAccountService
    {
        private IFeatureRepository _featureRepository;
        private IAccountFeaturesRepository _accountFeaturesRepository;
        private IAccountRepository _accountRepository;
        private IUserRepository _userRepository;
        private IUserDomainManager _userDomainService;
        private ISecurityService _securityService;
        private IPaymentRepository _paymentRepository;
        private IPaymentTypeRepository _paymentTypeRepository;
        private IConfigurationManager _configurationManager;
        private IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository;
        private IDocumentTypeRepository _documentTypeRepository;
        private IDocumentRepository _documentRepository;
        private IAccountDiscountRepository _accountDiscountRepository;
        private IAuditTrialRepository _AuditTrialRepository;
        private IAccountInvitationRepository _accountInvitationRepository;
        private IPortalPermisionRepository _PortalPermisionRepository;
        private IObjectTypeRepository _ObjectTypeRepository;
        private IObjectActionRepository _ObjectActionRepository;
        private IAppSiteRepository _AppSiteRepository;
        private IReportSchedulerRepository _ReportSchedulerRepository;
        private ILocalizedStringRepository _LocalizedStringRepository;
        private ICampaignRepository _CampaignRepository;
        private IAdGroupRepository _adGroupRepositroy;
        private IAdCreativeRepository _adCreativeRepository;
        private IAccountSummaryRepository _AccountSummaryRepository;
        private IAccountCostElementRepository _AccountCostElementRepository;
        private IAccountFeeRepository _AccountFeeRepository;
        private readonly IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        private IAccountDSPRequestRepository _AccountDSPRequestRepository;
        private IImpressionLogRepository _impressionLogRepository;
        private IUserAccountsRepository _UserAccountsRepositor;

        private IDSPAccountSettingRepository _DSPAccountSettingRepositor;
        private ICostElementRepository _CostElementRepository;
        private IDSPAccountSettingContactRepository _DSPAccountSettingContactRepositor;

        private IAdvertiserAccountUserRepository _AdvertiserAccountReadOnlyUserRepository = null;
        static readonly object LockObj = new object();

        public AccountService(IDSPAccountSettingRepository DSPAccountSettingRepositor, IDSPAccountSettingContactRepository DSPAccountSettingContactRepositor, IAccountRepository accountRepository,
                                        IAccountInvitationRepository accountInvitationRepository,

            IUserRepository userRepository,
            ISecurityService securityService,
            IPaymentRepository paymentRepository,
            IConfigurationManager configurationManager,
            IDocumentRepository documentRepository,
            ICampaignRepository CampaignRepository,
               IReportSchedulerRepository ReportSchedulerRepository,
                 IObjectTypeRepository ObjectTypeRepository,
        IAppSiteRepository appSiteRepository,
        ILocalizedStringRepository LocalizedStringRepository,

        IAccountPaymentDetailsRepository accountPaymentDetailsRepository,
            IAccountDiscountRepository accountDiscountRepository, IPaymentTypeRepository paymentTypeRepository, IAuditTrialRepository AuditTrialRepository, IObjectActionRepository objectActionRepository, IAdGroupRepository adGroupRep, IAdCreativeRepository adCreativeRepository, IAccountSummaryRepository accountSummeruRe, IAccountPortalPermissionsRepository AccountPortalPermissionsRepository, IAccountDSPRequestRepository AccountDSPRequestRepository, IPortalPermisionRepository PortalPermisionRepository, IAccountCostElementRepository AccountCostElementRe, IImpressionLogRepository ImpressionLogRepository,
            IDocumentTypeRepository _documentTypeRepository, IUserAccountsRepository UserAccountsRepositor, 
            IAccountFeaturesRepository _accountFeaturesRepository, IFeatureRepository _featureRepository,


            IAccountFeeRepository AccountFeeRepository,

            ICostElementRepository CostElementRep,
                 IAdvertiserAccountUserRepository AdvertiserAccountReadOnlyUserRepository
            )
        {
            this._DSPAccountSettingRepositor = DSPAccountSettingRepositor;
            this._DSPAccountSettingContactRepositor = DSPAccountSettingContactRepositor;
            _AdvertiserAccountReadOnlyUserRepository = AdvertiserAccountReadOnlyUserRepository;

            this._accountRepository = accountRepository;
            this._userRepository = userRepository;
            this._userDomainService = new UserDomainManager(userRepository, accountRepository);
            this._securityService = securityService;
            this._paymentRepository = paymentRepository;
            this._configurationManager = configurationManager;
            this._documentRepository = documentRepository;
            this._accountPaymentDetailsRepository = accountPaymentDetailsRepository;
            this._accountDiscountRepository = accountDiscountRepository;
            this._AuditTrialRepository = AuditTrialRepository;
            this._CampaignRepository = CampaignRepository;
            this._AppSiteRepository = appSiteRepository;
            this._ReportSchedulerRepository = ReportSchedulerRepository;
            this._ObjectTypeRepository = ObjectTypeRepository;
            this._documentTypeRepository = _documentTypeRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _LocalizedStringRepository = LocalizedStringRepository;
            _accountInvitationRepository = accountInvitationRepository;
            _AccountPortalPermissionsRepository = AccountPortalPermissionsRepository;

            this._ObjectActionRepository = objectActionRepository;
            this._adGroupRepositroy = adGroupRep;
            this._adCreativeRepository = adCreativeRepository;
            this._AccountSummaryRepository = accountSummeruRe;
            this._AccountDSPRequestRepository = AccountDSPRequestRepository;
            this._PortalPermisionRepository = PortalPermisionRepository;
            this._AccountCostElementRepository = AccountCostElementRe;

            this._AccountFeeRepository = AccountFeeRepository;
            _impressionLogRepository = ImpressionLogRepository;
            this._accountFeaturesRepository = _accountFeaturesRepository;
            _UserAccountsRepositor = UserAccountsRepositor;
            this._featureRepository = _featureRepository;
            this._CostElementRepository = CostElementRep;
        }
        #region Account Cost Elements


        public ValueMessageWrapper<bool> SaveAccountCostElement(AccountCostElementDto dto)
        {
            Party party = null;
            AccountCostElement costAcc;
            bool existParty = false;
            if (dto.partyId > 0)
            {
                party = new Party { ID = dto.partyId };
            }
            if (dto.ID.HasValue && dto.ID > 0)
            {
                costAcc = this._AccountCostElementRepository.Get((int)dto.ID);
            }
            else
            {
                costAcc = new AccountCostElement();

            }

            costAcc.CostElement = this._CostElementRepository.Get(dto.CostElmentId) ;
            costAcc.Account = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value };
            costAcc.Enabled = dto.Enabled;
            costAcc.Beneficiary = party;
            if (dto.DataProviderId > 0)
            {
                costAcc.DataProvider = new DPPartner { ID = dto.DataProviderId };
            }
            else
            {
                costAcc.DataProvider = null;

            }
            var itemsexist = this._AccountCostElementRepository.Query(M => M.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && M.CostElement.ID == dto.CostElmentId).ToList();

            if (costAcc.CostElement.Scope== 2 && costAcc.DataProvider == null)
            {
                throw new AccountCostElementNoDataProvider(null, null, Framework.Resources.ResourceManager.Instance.GetResource("ProviderVal", "CostElements"));
            }
   
            if (dto.ID.HasValue && itemsexist.Count() > 0 && itemsexist.Where(x => x.ID == dto.ID).Count() > 0 && (party == null || itemsexist.Where(x => x.Beneficiary != null && x.Beneficiary.ID == party.ID && x.ID != dto.ID).Count() == 0))
            {
                this._AccountCostElementRepository.Save(costAcc);
                return ValueMessageWrapper.Create(true);
            }


            if (itemsexist.Count() == 0)
            {
                this._AccountCostElementRepository.Save(costAcc);

            }
            else
            {

                if ((party != null && itemsexist.Where(x => x.Beneficiary != null && x.Beneficiary.ID == party.ID).Count() == 0) || (party == null && itemsexist.Where(x => x.Beneficiary == null).Count() == 0))
                {
                    this._AccountCostElementRepository.Save(costAcc);
                    return ValueMessageWrapper.Create(true);

                }

                throw new AccountCostElementAlreadyExist(null, null, Framework.Resources.ResourceManager.Instance.GetResource("Duplicated", "Global"));
            }
            return ValueMessageWrapper.Create(true);
        }
        public void RemoveAccountCostElement(ValueMessageWrapper<int> Id)
        {


            var item = this._AccountCostElementRepository.Get(Id.Value);
            if (item != null)
                this._AccountCostElementRepository.Remove(item);

        }
        public void EnableDisableAccountCostElement(ValueMessageWrapper<int> Id)
        {


            var item = this._AccountCostElementRepository.Get(Id.Value);


            if (item != null)
            {
                item.Enabled = !item.Enabled;
                this._AccountCostElementRepository.Save(item);
            }

        }

        public void RemoveAccountCostElementBulk(int[] Ids)
        {




            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _AccountCostElementRepository.Get(item)))
                {

                    //item.Delete();
                    _AccountCostElementRepository.Remove(item);

                }
            }

        }

        public AccountCostElementResultDto QueryAccountCostElements(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountCostElementCriteria wcriteria)
        {
            AccountCostElementCriteria criteria = new AccountCostElementCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            /*[DataMember]
      public string Value { get; set; }
      [DataMember]
      public bool Enabled { get; set; }
      [DataMember]
      public int CostElmentId { get; set; }

      [DataMember]
      public int AccountId { get; set; }*/

            criteria.AccountId= Framework.OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value;
            AccountCostElementResultDto result = new AccountCostElementResultDto();
            IList<AccountCostElementDto> itemsAll = new List<AccountCostElementDto>();
            var Items = _AccountCostElementRepository.Query(criteria.GetExpression());

            //, criteria.Size, item => item.ID, false
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                Items = Items.Where(M => M.CostElement.Name.Value.Contains(criteria.Name)).ToList();

            }
            Items = Items.Skip(criteria.Size * (criteria.Page.Value - 1)).Take(criteria.Size).OrderByDescending(M => M.ID).ToList();
            foreach (var item in Items)
            {

                // ItemsAll.Add
                var itemsAdd = new AccountCostElementDto { Value = item.CostElement.GetDescription(), CostElmentId = item.CostElement.ID, ID = item.ID, AccountId = item.Account.ID, Enabled = item.Enabled };
                itemsAdd.partyId = item.Beneficiary != null ? item.Beneficiary.ID : 0;
                itemsAdd.partyName = item.Beneficiary != null ? item.Beneficiary.Name : string.Empty;
                itemsAdd.DataProviderId= item.DataProvider != null ? item.DataProvider.ID : 0;
                itemsAdd.DataProviderName = item.DataProvider != null ? item.DataProvider.Name : string.Empty;
                //itemsAdd.CostValue = item.CostValue;
                itemsAll.Add(itemsAdd);

            }
            result.Items = itemsAll;
            result.TotalCount = _AccountCostElementRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        #endregion




        #region Account Fees


        public ValueMessageWrapper<bool> SaveAccountFee(AccountFeeDto dto)
        {
            Party party = null;
            AccountFee costAcc;
            bool existParty = false;
            if (dto.partyId > 0)
            {
                party = new Party { ID = dto.partyId };
            }
            if (dto.ID.HasValue && dto.ID > 0)
            {
                costAcc = this._AccountFeeRepository.Get((int)dto.ID);
            }
            else
            {
                costAcc = new AccountFee();

            }

            costAcc.Fee = new Fee{ ID = dto.FeeId};
            costAcc.Account = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value };
            costAcc.Enabled = dto.Enabled;
            costAcc.Beneficiary = party;


            var itemsexist = this._AccountFeeRepository.Query(M => M.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && M.Fee.ID == dto.FeeId).ToList();



            if (dto.ID.HasValue && itemsexist.Count() > 0 && itemsexist.Where(x => x.ID == dto.ID).Count() > 0 && (party == null || itemsexist.Where(x => x.Beneficiary != null && x.Beneficiary.ID == party.ID && x.ID != dto.ID).Count() == 0))
            {
                this._AccountFeeRepository.Save(costAcc);
                return ValueMessageWrapper.Create(true);
            }


            if (itemsexist.Count() == 0)
            {
                this._AccountFeeRepository.Save(costAcc);

            }
            else
            {

                if ((party != null && itemsexist.Where(x => x.Beneficiary != null && x.Beneficiary.ID == party.ID).Count() == 0) || (party == null && itemsexist.Where(x => x.Beneficiary == null).Count() == 0))
                {
                    this._AccountFeeRepository.Save(costAcc);
                    return ValueMessageWrapper.Create(true);

                }

                throw new AccountFeeAlreadyExist(null, null, Framework.Resources.ResourceManager.Instance.GetResource("Duplicated", "Global"));
            }
            return ValueMessageWrapper.Create(true);
        }
        public void RemoveAccountFee(ValueMessageWrapper<int> Id)
        {


            var item = this._AccountFeeRepository.Get(Id.Value);
            if (item != null)
                this._AccountFeeRepository.Remove(item);

        }
        public void EnableDisableAccountFee(ValueMessageWrapper<int> Id)
        {


            var item = this._AccountFeeRepository.Get(Id.Value);


            if (item != null)
            {
                item.Enabled = !item.Enabled;
                this._AccountFeeRepository.Save(item);
            }

        }

        public void RemoveAccountFeeBulk(int[] Ids)
        {




            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _AccountFeeRepository.Get(item)))
                {

                    //item.Delete();
                    _AccountFeeRepository.Remove(item);

                }
            }

        }

        public AccountFeeResultDto QueryAccountFees(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountFeeCriteria wcriteria)
        {
            AccountFeeCriteria criteria = new AccountFeeCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            /*[DataMember]
      public string Value { get; set; }
      [DataMember]
      public bool Enabled { get; set; }
      [DataMember]
      public int CostElmentId { get; set; }

      [DataMember]
      public int AccountId { get; set; }*/

            criteria.AccountId = Framework.OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value;
            AccountFeeResultDto result = new AccountFeeResultDto();
            IList<AccountFeeDto> itemsAll = new List<AccountFeeDto>();
            var Items = _AccountFeeRepository.Query(criteria.GetExpression());

            //, criteria.Size, item => item.ID, false
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                Items = Items.Where(M => M.Fee.Name.Value.Contains(criteria.Name)).ToList();

            }
            Items = Items.Skip(criteria.Size * (criteria.Page.Value - 1)).Take(criteria.Size).OrderByDescending(M => M.ID).ToList();
            foreach (var item in Items)
            {

                // ItemsAll.Add
                var itemsAdd = new AccountFeeDto { Value = item.Fee.GetDescription(), FeeId = item.Fee.ID, ID = item.ID, AccountId = item.Account.ID, Enabled = item.Enabled };
                itemsAdd.partyId = item.Beneficiary != null ? item.Beneficiary.ID : 0;
                itemsAdd.partyName = item.Beneficiary != null ? item.Beneficiary.Name : string.Empty;
                //itemsAdd.CostValue = item.CostValue;
                itemsAll.Add(itemsAdd);

            }
            result.Items = itemsAll;
            result.TotalCount = _AccountFeeRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        #endregion


        #region AuditTrial

        private void ValidateAccount(int? accountId, int? userId)
        {

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != accountId)
            {
                throw new AccountNotValidException();
            }

            if (userId.HasValue && !Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().IsPrimaryUser)
            {
                if ((userId != Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().UserId))
                {
                    throw new AccountNotValidException();
                }
            }

        }
        public TrialResultDto MainRootTrialQueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AuditTrialCriteria wcriteria)

        { 

            AuditTrialCriteria criteria = new AuditTrialCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            IList<TrialDto> list = new List<TrialDto>();
            IEnumerable<AuditTrialDto> Items;
            var result = new TrialResultDto();
            var GMT = Framework.Resources.ResourceManager.Instance.GetResource("UTC", "Global");
            var formatDate = _configurationManager.GetConfigurationSetting(1, null, "ShortDateFormat");
            var formatTime = _configurationManager.GetConfigurationSetting(1, null, "TimeFormat");
            int TotalCount;
            try
            {
                var filter = new AuditTrialFilter { PageSize = criteria.Size, ObjectTypeID = criteria.Type, ObjectActionID = 0, UserId = criteria.UserId.Value, AccountId = criteria.AccountId.Value, IsPrimaryUser = criteria.IsPrimaryUser, ActionTimeFrom = criteria.DataFrom, ActionTimeTo = criteria.DataTo, PageIndex = (int)criteria.Page, objectTypeName = criteria.Name };
                if (criteria.Type > 0 && !string.IsNullOrEmpty(criteria.Name))
                {
                    var objectType = _ObjectTypeRepository.Get(criteria.Type);
                    filter.objectshortType = objectType.ObjectTypeName.Split('.').Last();
                    Items = _accountRepository.GeAuditTrialMainRootsUsingStat(filter, out TotalCount);



                }
                else
                    Items = _accountRepository.GeAuditTrialMainRootsUsingStat(filter, out TotalCount);


                foreach (AuditTrialDto item in Items)
                {

                    var objectType = _ObjectTypeRepository.Get((int)item.RootObjectTypeID);
                    TrialDto Trial = new TrialDto();
                    Trial.RootId = item.RootObjectId;
                    Trial.ObjectRootTypeId = item.RootObjectTypeID;
                    Trial.UserId = item.User;
                    //item.ActionTime = _accountRepository.GeMaxActionTime(item.RootObjectId, item.RootObjectTypeID);
                    Trial.ActionTime = item.ActionTime;
                    Trial.ActionTimeString = item.ActionTime.ToString(formatDate) + " " + item.ActionTime.ToString(formatTime) + " " + GMT;

                    string objectTypeName = objectType.ObjectTypeName.Split('.').Last();
                    Trial.Name = GetRootObjectName(new GetRootObjectNameRequest { RootObjectTypeID = Trial.ObjectRootTypeId, RootObjectId = item.RootObjectId, ObjectTypeName = objectTypeName });
                    //Get object Type
                    if (objectType != null)
                    {
                        if (objectType.Name != null)
                            Trial.Type = _LocalizedStringRepository.GetLocalizedStringById(objectType.Name.ID);
                    }
                    else
                    {
                        throw new Exception("objectType cant be null");
                    }



                    Trial.UserId = item.User;

                    list.Add(Trial);

                }

            }
            catch (Exception)
            {

                throw;
            }

            result.Items = list/*.OrderByDescending(M=>M.ActionTime).ToList()*/;
            result.TotalCount = TotalCount;
            return result;


        }


        public IList<Interfaces.DTOs.Account.ObjectTypeDto> getObjectTypes()
        {

            IList<ObjectType> types = _ObjectTypeRepository.GetRootObjects();
            IList<Interfaces.DTOs.Account.ObjectTypeDto> dtos = new List<Interfaces.DTOs.Account.ObjectTypeDto>();
            foreach (ObjectType type in types)
            {
                Interfaces.DTOs.Account.ObjectTypeDto dto = new Interfaces.DTOs.Account.ObjectTypeDto();


                var typeModefied = type.ObjectTypeName;

                dto.ObjectTypeName = _LocalizedStringRepository.GetLocalizedStringById(type.Name.ID);
                dto.ID = type.ID;
                dtos.Add(dto);


            }
            return dtos;



        }

        #region Audit max and min 
        public AuditTrialsMaxAndMinMessage GetAuditTrialsMaxAndMin(ValueMessageWrapper<int> years)
        {

            try
            {
                _AuditTrialRepository.GetAuditTrialsMaxAndMin(years.Value, out var max, out var min);
                return new AuditTrialsMaxAndMinMessage { Max = max, Min = min };
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public AuditTrialsMaxAndMinMessage GetAuditTrialSessionStatAliasMaxAndMin(ValueMessageWrapper<int> years)
        {

            try
            {
                _AuditTrialRepository.GetAuditTrialSessionStatAliasMaxAndMin(years.Value, out var max, out var min);
                return new AuditTrialsMaxAndMinMessage { Max = max, Min = min};
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public AuditTrialsMaxAndMinMessage GetAuditTrialStatAliasMaxAndMin(ValueMessageWrapper<int> years)
        {

            try
            {
                _AuditTrialRepository.GetAuditTrialStatAliasMaxAndMin(years.Value, out var max, out var min);
                return new AuditTrialsMaxAndMinMessage { Max =max, Min= min };
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region delete Audit
        public void DeleteAuditTrials(AuditTrialsMaxAndMinMessage request)
        {

            try
            {
                _AuditTrialRepository.DeleteAuditTrials(request.Max, request.Min);

            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public void DeleteAuditTrialSessionStat(AuditTrialsMaxAndMinMessage request)
        {

            try
            {
                _AuditTrialRepository.DeleteAuditTrialSessionStat(request.Max, request.Min);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public void DeleteAuditTrialStat(AuditTrialsMaxAndMinMessage request)
        {

            try
            {
                _AuditTrialRepository.DeleteAuditTrialStat(request.Max, request.Min);

            }
            catch (Exception e)
            {

                throw e;
            }

        }
        #endregion

        public IList<TrialDto> GetTrialSession(GetTrialSessionRequest request)
        {
            Guid guid = new Guid(request.Id);
            ObjectAction objectAction = null;
            ObjectType objectType = null;
            IList<TrialDto> list = new List<TrialDto>();
            var childrenItems = _AuditTrialRepository.GeAuditTrialForSessionRoot(guid.ToByteArray(), 0);
            foreach (var child in childrenItems)
            {


                TrialDto dtoChild = new TrialDto();
                dtoChild.ActionTime = child.ActionTime;
                dtoChild.Details = child.Details;
                dtoChild.ObjectTypeId = child.ObjectTypeID;
                dtoChild.ObjectId = child.ObjectId;
                dtoChild.SessionId = request.Id;
                dtoChild.ID = child.ID;
                dtoChild.RootId = child.RootObjectId;
                dtoChild.ObjectActionID = child.ObjectActionID;



                if (child.ObjectTypeID > 0)
                {
                    objectType = _ObjectTypeRepository.Get(child.ObjectTypeID);
                    //var objectType = _ObjectTypeRepository.Get((int)child.ObjectTypeID);

                    //if (child.TypeNameId > 0)
                    dtoChild.Type = objectType.Name.Value;/* _LocalizedStringRepository.GetLocalizedStringById(child.TypeNameId);*/

                    child.Visibility = objectType.Visibility;
                    child.Type = objectType.ObjectTypeName;
                    dtoChild.ObjectName = GetObjectName(child.Type, dtoChild.ObjectId, null, child.Details);

                }
                if (child.ObjectActionID > 0)
                {
                    objectAction = _ObjectActionRepository.Get(child.ObjectActionID);
                    // var objectAction = _ObjectActionRepository.Get((int)child.ObjectActionID);
                    // var action = _ObjectActionRepository.Get(child.ObjectActionID);
                    //if (child.ActionNameId > 0)
                    //{  
                    dtoChild.ObjectActionString = objectAction.Name.Value /*_LocalizedStringRepository.GetLocalizedStringById(child.ActionNameId)*/;
                    dtoChild.ObjectActionConstantString = objectAction.ObjectActionName;


                    //}

                }

                if (child.Visibility == TrialVisibility.Unknowen)
                {

                    continue;
                }
                if (!request.IsAdminApp)
                {
                    if (child.Visibility == TrialVisibility.Admin)
                    {

                        continue;
                    }

                }


                if (dtoChild.ObjectActionConstantString != "Delete")
                {
                    var childReords = GetTrialDetailsSession(new GetTrialDetailsSessionRequest { Id = child.ID, IsAdminApp = request.IsAdminApp, CollectInfo = false });
                    if (childReords != null && childReords.Count > 0)
                        list.Add(dtoChild);
                }
                else
                {

                    list.Add(dtoChild);

                }
            }
            return list;
        }

        public IList<TrialDto> GetTrialDetailsSession(GetTrialDetailsSessionRequest request)

        {
            //Guid guid = new Guid(id);

            IList<TrialDto> list = new List<TrialDto>();
            var details = _AuditTrialRepository.GetTrialDetailsSession(request.Id);


            XmlReader reader = XmlReader.Create(new StringReader(details));

            XmlSerializer ser = new XmlSerializer(typeof(AuditTrialModelXml));

            var listDes = (AuditTrialModelXml)ser.Deserialize(reader);
            var ObjectTypeName = listDes.Type;
            System.Reflection.Assembly assembly = typeof(ArabyAds.AdFalcon.Domain.Model.Account.Account).Assembly;
            Type type = assembly.GetType(ObjectTypeName);


            if (listDes != null && listDes.PropertyList != null)

            {
                var objectDataSet = Framework.DomainServices.AuditTrial.ObjectMetaDataManagement.ObjectMetaDataManager.Instance.GetobjectSet(ObjectTypeName);

                ObjectMetaDataDto proprtyMetaData = null;
                foreach (var child in listDes.PropertyList.props)
                {
                    TrialDto dtoChild = new TrialDto();

                    // dtoChild.Details = child.Details;
                    dtoChild.PropertyName = child.Name;
                    dtoChild.NewValue = child.NewValue;
                    dtoChild.OldValue = child.OldValue;
                    if (objectDataSet != null)
                    {
                        if (objectDataSet.ContainsKey(dtoChild.PropertyName))
                            proprtyMetaData = objectDataSet[dtoChild.PropertyName];
                        else
                            proprtyMetaData = null;
                    }
                    else
                        proprtyMetaData = null;
                    if (proprtyMetaData != null)
                    {
                        var PrortyInfor = type.GetProperty(dtoChild.PropertyName);

                        var show = ObjectToShowOrNot(proprtyMetaData, PrortyInfor != null ? PrortyInfor.PropertyType : null, request.IsAdminApp);
                        if (show)
                        {
                            if (request.CollectInfo)
                            {
                                if (!string.IsNullOrEmpty(dtoChild.NewValue))
                                    dtoChild.NewValue = GetPropertyString(proprtyMetaData, PrortyInfor != null ? PrortyInfor.PropertyType : null, type, dtoChild.NewValue);
                                if (!string.IsNullOrEmpty(dtoChild.OldValue))
                                    dtoChild.OldValue = GetPropertyString(proprtyMetaData, PrortyInfor != null ? PrortyInfor.PropertyType : null, type, dtoChild.OldValue);

                                if (!string.IsNullOrEmpty(proprtyMetaData.ResourceKey) && !string.IsNullOrEmpty(proprtyMetaData.ResourceSet))
                                    dtoChild.PropertyName = Framework.Resources.ResourceManager.Instance.GetResource(proprtyMetaData.ResourceKey, proprtyMetaData.ResourceSet);
                            }
                            if (dtoChild.OldValue == null)
                            {
                                dtoChild.OldValue = string.Empty;
                            }
                            if (dtoChild.NewValue == null)
                            {
                                dtoChild.NewValue = string.Empty;
                            }
                            if (dtoChild.OldValue != dtoChild.NewValue)
                                list.Add(dtoChild);
                        }

                        continue;
                    }
                }
            }
            return list;
        }


        public GetTrialSessionsResponse GetTrialSessions(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AuditTrialCriteria wcriteria)
        {


            AuditTrialCriteria criteria = new AuditTrialCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            IList<TrialDto> list = new List<TrialDto>();


            AuditTrialFilter filter = new AuditTrialFilter();
            filter.RootID = criteria.ObjectRootId;
            filter.UserId = criteria.UserId.Value;
            filter.AccountId = criteria.AccountId.Value;
            filter.IsPrimaryUser = criteria.IsPrimaryUser;
            filter.ObjectTypeID = criteria.Type;
            filter.PageIndex = criteria.Page.Value;
            filter.ActionTimeFrom = criteria.DataFrom;
            filter.ActionTimeTo = criteria.DataTo;
            filter.UserName = criteria.UserName;
            var RootOjecttype = _ObjectTypeRepository.Get(filter.ObjectTypeID);
            string objectNameRoot = RootOjecttype.ObjectTypeName.Split('.').Last();

            if (!_accountRepository.IsRootObjectRelatedToAccount(filter.RootID, filter.AccountId, filter.IsPrimaryUser ? null : (int?)filter.UserId, objectNameRoot))
                throw new UnauthorizedAccessException("you are not authrized");
            //bool IsRootObjectRelatedToAccount(int rootobjecid, int AccountId, string TypeName)
            filter.UserId = 0;
            filter.PageSize = 10;
            var Items = _accountRepository.GeAuditTrialForObjectRootUsingStat(filter, out var total).ToList();
            var GMT = Framework.Resources.ResourceManager.Instance.GetResource("UTC", "Global");
            var formatDate = _configurationManager.GetConfigurationSetting(1, null, "ShortDateFormat");
            var formatTime = _configurationManager.GetConfigurationSetting(1, null, "TimeFormat");


            foreach (AuditTrialDto item in Items)
            {
                TrialDto dto = new TrialDto();
                Guid guid = new Guid(item.SessionId);
                dto.SessionId = guid.ToString();
                dto.ActionTime = item.ActionTime;
                dto.ActionTimeString = item.ActionTime.ToString(formatDate) + " " + item.ActionTime.ToString(formatTime) + " " + GMT;

                dto.UserName = _accountRepository.GetObjectNameForUserName(item.User);
                dto.Childs = new List<TrialDto>();
                //var childrenItems=_AuditTrialRepository.GeAuditTrialForSessionRoot(item.SessionId, 0);
                // foreach (var child in childrenItems)
                // {
                //     TrialDto dtoChild = new TrialDto();
                //     dtoChild.ActionTime = child.ActionTime;
                //     dtoChild.Details = child.Details;
                //     dtoChild.ObjectTypeId = child.ObjectTypeID;
                //     dtoChild.ObjectActionID = child.ObjectActionID;

                //     dto.Childs.Add(dtoChild);
                // }
                list.Add(dto);
            }
            return new GetTrialSessionsResponse { Trials = list, Total = total };
        }




        public string GetRootObjectName(GetRootObjectNameRequest request)
        {
            //var objectType = _ObjectTypeRepository.Get(RootObjectTypeID);
            //string objectTypeName = "";

            //if (objectType != null)
            //{
            //    string typeModefied = objectType.ObjectTypeName;

            //}
            //else
            //{
            //    throw new Exception("objectType cant be null");
            //}

            string objectTypeName = request.ObjectTypeName.Split('.').Last();
            if (!string.IsNullOrEmpty(objectTypeName))
            {
                switch (objectTypeName)
                {
                    case "ReportScheduler":
                        {
                            var rep = _ReportSchedulerRepository.Get(request.RootObjectId);

                            var jsonObj = JsonConvert.DeserializeObject<ReportCriteriaDto>(rep.ReportCriteria != null ? rep.ReportCriteria.Criteria : rep.ReportJsonCriteria);
                            var ReportCriteriaDto = (ReportCriteriaDto)jsonObj;

                            return rep.GetCompositeMame(ReportCriteriaDto.ItemsList, ReportCriteriaDto.TabId, false, string.Empty);


                        }


                    case "AppSite":

                        return _AppSiteRepository.GetObjectName(request.RootObjectId);


                    case "Campaign":
                        return _CampaignRepository.GetObjectName(request.RootObjectId);



                    case "Account":

                        return _accountRepository.GetObjectName(request.RootObjectId);


                    default:
                        throw new Exception("inValid objectType Name");

                }



            }
            else
            {
                throw new Exception("objectType Must has a name");

            }
        }

        public string GetObjectName(string objectTypeName, int ObjectId, Type ObjectT = null, string details = null
            )
        {


            if (!string.IsNullOrEmpty(objectTypeName))
            {
                switch (objectTypeName)
                {
                    case "ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler":
                        {
                            //_ReportSchedulerRepository.GetObjectName(ObjectId);

                            var rep = _ReportSchedulerRepository.Get(ObjectId);

                            var jsonObj = JsonConvert.DeserializeObject(rep.ReportCriteria != null ? rep.ReportCriteria.Criteria : rep.ReportJsonCriteria);
                            var ReportCriteriaDto = (ReportCriteriaDto)jsonObj;

                            return rep.GetCompositeMame(ReportCriteriaDto.ItemsList, ReportCriteriaDto.TabId, false, string.Empty);


                        }


                    case "ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite":

                        return _AppSiteRepository.GetObjectName(ObjectId);


                    case "ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign":
                        return _CampaignRepository.GetObjectName(ObjectId);



                    case "ArabyAds.AdFalcon.Domain.Model.Account.Account":

                        return _accountRepository.GetObjectName(ObjectId);
                    case "ArabyAds.AdFalcon.Domain.Model.Campaign.AdGroup":

                        return this._adGroupRepositroy.GetObjectName(ObjectId);



                    default:

                        {
                            //ArabyAds.AdFalcon.Domain.Model.AppSite.AppSiteSetting gffggf;
                            if (ObjectT != null)
                            {
                                if (ObjectT.IsSubclassOf(typeof(Domain.Model.Campaign.AdCreative)))
                                {
                                    this._adCreativeRepository.GetObjectName(ObjectId);

                                }

                            }
                            System.Reflection.Assembly assembly = typeof(ArabyAds.AdFalcon.Domain.Model.Account.Account).Assembly;
                            Type type = assembly.GetType(objectTypeName);

                            if (ObjectId > 0)
                            {
                                var obj = _ObjectTypeRepository.GetObjectByType(type, ObjectId);
                                if (obj != null)
                                    return obj.GetDescription();
                                else
                                {
                                    if (!string.IsNullOrEmpty(details))
                                    {

                                        XmlReader reader = XmlReader.Create(new StringReader(details));

                                        XmlSerializer ser = new XmlSerializer(typeof(AuditTrialModelXml));

                                        var listDes = (AuditTrialModelXml)ser.Deserialize(reader);
                                        if (listDes != null)
                                        {
                                            return listDes.Name;
                                        }
                                    }

                                }
                            }
                            return string.Empty;
                        }

                }



            }
            else
            {
                throw new Exception("objectType Must has a name");

            }
        }
        public bool CheckAuditTrialVisiblity(TrialVisibility vsB, bool isAdminApp = false)
        {

            if (!isAdminApp)
            {

                if (vsB == TrialVisibility.Admin)
                {

                    return false;
                }
            }
            if (vsB == TrialVisibility.Unknowen)
            {

                return false;
            }

            return true;
        }
        public bool ObjectToShowOrNot(ObjectMetaDataDto propertyMetaData, Type ObjectT = null, bool isAdminApp = false)
        {
            //if (ObjectT != null)
            //{
            //    if (ObjectT.IsSubclassOf(typeof(Domain.Model.Campaign.AdCreative)))
            //    {
            //        return true;


            //    }

            //}

            //if (PropertyName=="IsDeleted" || PropertyName == "ReportJsonCriteria" || PropertyName == "Usage")
            //{
            //    return false;

            //}
            //if (ObjectT == null)
            //{
            //  return CheckAuditTrialVisiblity(propertyMetaData.Visibility, isAdminApp);




            //}
            //else
            //{

            return CheckAuditTrialVisiblity(propertyMetaData.Visibility, isAdminApp);
            //}
            //if (!string.IsNullOrEmpty(ObjectT.FullName))
            //{
            //    switch (ObjectT.FullName)
            //    {
            //        case "ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler":
            //            return false;


            //        case "ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite":

            //            return false;


            //        case "ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign":
            //            return false;



            //        case "ArabyAds.AdFalcon.Domain.Model.Account.Account":

            //            return false;
            //        case "ArabyAds.AdFalcon.Domain.Model.Campaign.AdGroup":

            //            return true;



            //        default:


            //            return CheckAuditTrialVisiblity(propertyMetaData.Visibility, isAdminApp);



            //    }



            //}
            //else
            //{
            //    throw new Exception("objectType Must has a name");

            //}
        }
        public string GetRootObjectTypeNameValue(ValueMessageWrapper<int> RootObjectTypeID)
        {
            var objectType = _ObjectTypeRepository.Get(RootObjectTypeID.Value);
            string objectTypeName = "";

            if (objectType != null)
            {


                //string typeModefied = objectType.ObjectTypeName;
                objectTypeName = _LocalizedStringRepository.GetLocalizedStringById(objectType.Name.ID);
            }
            else
            {
                throw new Exception("objectType cant be null");
            }
            return objectTypeName;
        }

        public string GetRootObjectTypeName(ValueMessageWrapper<int> RootObjectTypeID)
        {
            var objectType = _ObjectTypeRepository.Get(RootObjectTypeID.Value);
            string objectTypeName = "";

            if (objectType != null)
            {


                string typeModefied = objectType.ObjectTypeName;
                objectTypeName = typeModefied.Split('.').Last();
            }
            else
            {
                throw new Exception("objectType cant be null");
            }
            return objectTypeName;
        }

        private string GetPropertyString(ObjectMetaDataDto propertyMetaData, Type property, Type ContainerObjectType, string objectValue)
        {


            if (!string.IsNullOrEmpty(propertyMetaData.MethodDescriper))
            {
                dynamic result = null;
                MethodInfo methodInfo = ContainerObjectType.GetMethod(propertyMetaData.MethodDescriper);
                if (methodInfo != null)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(ContainerObjectType, null);
                    List<string> listString = new List<string>();
                    listString.Add(objectValue);
                    if (parameters.Length > 1)
                        listString = objectValue.Split(',').ToList();

                    var objectsparam = new List<Object>();
                    foreach (var paramstring in listString)
                    {
                        objectsparam.Add(paramstring);
                    }
                    result = methodInfo.Invoke(classInstance, parameters.Length == 0 ? null : objectsparam.ToArray());

                    return result.ToString();
                }

            }
            if (property == typeof(DateTime) || property == typeof(DateTime?) || propertyMetaData.Format == DataFormat.DataDate || propertyMetaData.Format == DataFormat.DataDateTime || propertyMetaData.Format == DataFormat.DataTime)
            {
                var GMT = Framework.Resources.ResourceManager.Instance.GetResource("UTC", "Global");
                var formatDate = _configurationManager.GetConfigurationSetting(1, null, "ShortDateFormat");
                var formatTime = _configurationManager.GetConfigurationSetting(1, null, "TimeFormat");



                CultureInfo us = new CultureInfo("en-US");
                var dt = Convert.ToDateTime(objectValue, us);
                if (propertyMetaData.Format == DataFormat.DataDateTime)

                    return dt.ToString(formatDate) + " " + dt.ToString(formatTime) + " " + GMT;//+ " " + dt.ToShortDateString();
                else if (propertyMetaData.Format == DataFormat.DataTime)
                    return dt.ToString(formatTime) + " " + GMT;
                else
                    return dt.ToString(formatDate);


            }

            if (property == typeof(bool) || property == typeof(bool?) || propertyMetaData.Format == DataFormat.DataBool)
            {

                return Framework.Resources.ResourceManager.Instance.GetResource(objectValue, "Global");
            }
            if (property == typeof(double) || property == typeof(decimal) || property == typeof(double?) || property == typeof(decimal?) || property == typeof(float?) || property == typeof(float) || propertyMetaData.Format == DataFormat.DataDecimalRaw || propertyMetaData.Format == DataFormat.DataDecimal)
            {
                var value = Convert.ToDecimal(objectValue);
                if (propertyMetaData.Format == DataFormat.DataDecimalRaw)
                    return FormatDecimal(value, "");
                else
                    return FormatDecimal(value, "$");
            }
            if ((property != null && property.IsEnum) || propertyMetaData.Format == DataFormat.DataEnum)
            {

                Enum enumTobe = (Enum)Enum.Parse(property, objectValue);
                return enumTobe.ToText();
            }
            if ((property != null && property.GetInterface(typeof(IEntity<>).Name) != null /*typeof(IEntity<>).IsAssignableFrom(property)*/) || propertyMetaData.Format == DataFormat.DataCustom)
            {

                var intV = Convert.ToInt32(objectValue);
                return GetObjectName(property.FullName, intV, property);//+ " " + dt.ToShortDateString();
            }


            return objectValue;
        }
        protected string FormatDecimal(Decimal value, string extra = "")
        {
            if (value == 0)
            {
                return string.Format("0{0}", extra);
            }
            if (value < 1)
            {
                return string.Format("{0}{1}", value.ToString("0.##"), extra);
            }
            return string.Format("{0}{1}", value.ToString("#####################.##"), extra);
        }
        //private string GetName(IEntity<int> entity)
        //{
        //    if (entity.GetType().GetGenericTypeDefinition() == typeof(Domain.Model.Core.LookupBase<,>) )
        //    {
        //        var item = GetName(entity.ID);
        //        //return item.GetDescription();
        //        return item;

        //    }
        //    if (entity is ManagedLookupBase)
        //    {
        //        var lookupBase = entity as ManagedLookupBase;
        //        try
        //        {
        //            return lookupBase.GetDescription();
        //        }
        //        catch (Exception)
        //        {
        //            return GetName(lookupBase.ID);
        //        }
        //    }

        //    return string.Empty;
        //}

        public ValueMessageWrapper<int> GetObjectRootTypeId(string objectRoot)
        {
            int id;
            try
            {
                id = _ObjectTypeRepository.Query(x => x.ObjectTypeName == objectRoot).FirstOrDefault().RootID;
            }
            catch (Exception e)
            {

                throw e;
            }


            return ValueMessageWrapper.Create(id);
        }

        //public string DeleteLessThanThreeYearsAuditTrials()
        //{
        //    string time = _AuditTrialRepository.DeleteLessThanThreeYearsAuditTrials();
        //    return time;
        //}
        #endregion



        public AccountPaymentDetailDto GetAccountPaymentDetails()
        {
            Domain.Model.Account.Account currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            var bank_paymentinfo = currentAccount.PaymentDetails.FirstOrDefault(x => x.IsActive && x.AccountType == PayemntAccountType.Bank);
            var paypal_paymentinfo = currentAccount.PaymentDetails.Where(x => x.IsActive && x.AccountType == PayemntAccountType.PayPal && (x as PayPalAccountPaymentDetails).IsPrimary).FirstOrDefault();

            var paymentinfo = new AccountPaymentDetailDto { TypeId = 1 };

            //TODO:Osaleh to use MapperHelper , this is temp solution for inheritance issue
            if (bank_paymentinfo != null)
            //MapperHelper.Map<AccountPaymentDetailDto>(bank_paymentinfo,paymentinfo);
            {
                var obj = bank_paymentinfo as BankAccountPaymentDetails;
                paymentinfo.BankName = obj.BankName;
                paymentinfo.BankAddress = obj.BankAddress;
                paymentinfo.BeneficiaryName = obj.BeneficiaryName;
                paymentinfo.RecipientAccountNumber = obj.RecipientAccountNumber;
                paymentinfo.SWIFT = obj.SWIFT;
            }
            if (paypal_paymentinfo != null)
            //MapperHelper.Map<AccountPaymentDetailDto>(paypal_paymentinfo,paymentinfo);
            {
                var obj = paypal_paymentinfo as PayPalAccountPaymentDetails;
                paymentinfo.UserName = obj.UserName;
                if (obj.IsDefault)
                {
                    paymentinfo.TypeId = 2; //paypal
                }
            }
            paymentinfo.TaxNumber = currentAccount.TaxNo;
            if (currentAccount.TaxRegistration != null)
            {
                paymentinfo.Document = MapperHelper.Map<DocumentDto>(currentAccount.TaxRegistration);
            }
            paymentinfo.TaxNumberRegex = GetVATTaxNoRegularExpression(ValueMessageWrapper.Create((int)OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId));
            return paymentinfo;
        }

        public ValueMessageWrapper<bool> CheckIfDocumentbelongToAccount(ValueMessageWrapper<int> documentId)
        {
            var currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            if (currentAccount.TaxRegistration != null && currentAccount.TaxRegistration.ID == documentId.Value)
            {
                return ValueMessageWrapper.Create(true);

            }

            throw new UnauthorizedAccessException();

        }

        public void UpdateBankAccountInfo(AccountPaymentDetailDto paymentAccountDto)
        {
            bool isDefaultBank = false, isDefaultPayPal = false;
            var currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);

            AccountPaymentDetails newBankPaymentInfo = MapperHelper.Map<BankAccountPaymentDetails>(paymentAccountDto); //CreateNewBankAccount();
            newBankPaymentInfo.AccountType = PayemntAccountType.Bank;
            newBankPaymentInfo.SubType = PayemntAccountSubType.Both;

            AccountPaymentDetails newPaypalPaymentInfo = MapperHelper.Map<PayPalAccountPaymentDetails>(paymentAccountDto); //CreateNewPayPalAccount();
            newPaypalPaymentInfo.AccountType = PayemntAccountType.PayPal;
            newPaypalPaymentInfo.SubType = PayemntAccountSubType.Both;

            switch (paymentAccountDto.TypeId)
            {
                case 1://bank account
                    {
                        isDefaultBank = true;
                        break;
                    }
                case 2://paypal account
                    {
                        isDefaultPayPal = true;
                        break;
                    }
            }

            newPaypalPaymentInfo.Validate();
            newBankPaymentInfo.Validate();
            Document Document = null;
            var taxDoc = currentAccount.TaxRegistration;
            string taxno = currentAccount.TaxNo;
            if (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue > 0)
            {
                currentAccount.TaxNo = paymentAccountDto.TaxNumber;

                if (paymentAccountDto.Document != null && paymentAccountDto.Document.Content != null && paymentAccountDto.Document.Content.Length > 0)
                {
                    if (paymentAccountDto.Document.ID == 0)
                    {
                        Document = MapperHelper.Map<Document>(paymentAccountDto.Document);
                        _documentRepository.Save(Document);
                        currentAccount.TaxRegistration = Document;
                    }
                    else
                    {
                        Document = _documentRepository.Get(paymentAccountDto.Document.ID);

                    }

                }
                else
                {
                    if (paymentAccountDto.Document != null && paymentAccountDto.Document.ID == 0)
                        currentAccount.TaxRegistration = null;
                }
            }
            if ((!isDefaultBank) && (!isDefaultPayPal))
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "NoDefaultAccountBR" });
                throw error;
            }
            if (((isDefaultBank) && (!newBankPaymentInfo.IsValid)) ||
                ((isDefaultPayPal) && (!newPaypalPaymentInfo.IsValid)))
            {
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "NoDataBR" });
                throw error;
            }

            AddPaymentResponse bankPaymentResponse = AddPaymentResponse.NoChanges;
            AddPaymentResponse paypalPaymentResponse = AddPaymentResponse.NoChanges;


            if (!newBankPaymentInfo.IsHasValue)
            {
                bankPaymentResponse = currentAccount.ResetAccountPaymentDetails(PayemntAccountType.Bank);
            }
            if (!newPaypalPaymentInfo.IsHasValue)
            {
                paypalPaymentResponse = currentAccount.ResetAccountPaymentDetails(PayemntAccountType.PayPal);
            }

            if (newBankPaymentInfo.IsValid)
            {
                bankPaymentResponse = currentAccount.AddAccountPaymentDetail(newBankPaymentInfo, isDefaultBank);
            }
            if (newPaypalPaymentInfo.IsValid)
            {
                paypalPaymentResponse = currentAccount.AddAccountPaymentDetail(newPaypalPaymentInfo, isDefaultPayPal);
            }
            if (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue > 0)
            {
                if ((bankPaymentResponse == AddPaymentResponse.NoChanges) && (paypalPaymentResponse == AddPaymentResponse.NoChanges) && (taxno == currentAccount.TaxNo && (taxDoc == currentAccount.TaxRegistration)))
                {
                    throw new NoChangesException();
                }
            }
            else
            {
                if ((bankPaymentResponse == AddPaymentResponse.NoChanges) && (paypalPaymentResponse == AddPaymentResponse.NoChanges))
                {
                    throw new NoChangesException();
                }

            }
            _accountRepository.Save(currentAccount);
        }
        public string GetAccountEmailAddress(ValueMessageWrapper<int> accountId)
        {
            var currentAccount = _accountRepository.Get(accountId.Value);



            return currentAccount.PrimaryUser.EmailAddress;
        }

        public string GetVATTaxNoRegularExpression(ValueMessageWrapper<int> accountId)
        {
            var currentacc = _accountRepository.Get(accountId.Value);



            return currentacc.GetTaxRegistrationExpression();
        }
        public AccountSummaryDto GetAccountSummary()
        {
            var currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
            currentAccount.AccountSummary = _AccountSummaryRepository.Get(currentAccount.ID);
            AccountSummaryDto accountAccountDto = new AccountSummaryDto(); ;
            if (currentAccount.AccountSummary != null)
                accountAccountDto = MapperHelper.Map<AccountSummaryDto>(currentAccount.AccountSummary);


            return accountAccountDto;
        }
        private string GetReceiptNumber()
        {
            //TODO:Osaleh to rewrite this code
            // generate Receipt Number
            return string.Format("{0}/{1}/{2}", Framework.Utilities.Environment.GetServerTime().Year, ApplicationPrefix, GetCounter());
        }
        private int GetCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            query.SetString("CounterName", "Payment");
            query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        private int GetAccountCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            query.SetString("CounterName", "Account");
            query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        private string ApplicationPrefix
        {
            get
            {
                const string key = "ApplicationPrefix-CacheKey";
                var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    lock (LockObj)
                    {
                        value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = _configurationManager.GetConfigurationSetting(null, null, "ApplicationPrefix");
                            Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                        }
                    }
                }
                return value;
            }
        }


        public  BusinessException ValidateNewPaymentDto(NewPaymentDto dto)
        {
            var error = new BusinessException();
            if (!dto.AccountId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentAccount" });
            }
            if (!dto.TransactionDate.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentDate" });
            }
            if (!dto.PaymentType.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentType" });
            }
            if (!dto.Amount.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredAmount" });
            }
            else
            {
                if (dto.Amount <= 0 || dto.Amount > 99999999)
                {
                    error.Errors.Add(new ErrorData { ID = "MaxPayment" });
                }
            }



            return error;
        }
        public void AddPayment(NewPaymentDto paymentDto)
        {
            var error = ValidateNewPaymentDto(paymentDto);

            Domain.Model.Account.Account currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId.Value);

            if ((!paymentDto.AccountId.HasValue) || (!paymentDto.PaymentType.HasValue))
                throw error;

            Domain.Model.Account.Account account = _accountRepository.Get(paymentDto.AccountId.Value);

            Payment payment = null;
            var valid = true;
            switch ((PaymentTypeIds)paymentDto.PaymentType)
            {
                case PaymentTypeIds.Cash:
                    {
                        payment = new Payment
                        {
                            Type = Domain.Model.Account.Payment.PaymentType.Cash,
                            AdFalconReceiptNo = GetReceiptNumber(),
                        };
                        break;
                    }
                case PaymentTypeIds.WireTransfer:
                    {
                        if (!paymentDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }
                        if (!paymentDto.AccountPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "AccountPaymentDetailBR" });
                            valid = false;
                        }
                        if (valid)
                        {
                            payment = new PaymentWire()
                            {
                                Type = Domain.Model.Account.Payment.PaymentType.WireTransfer,
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(paymentDto.SystemPaymentDetailId.Value) as BankAccountPaymentDetails,
                                AccountPaymentDetail = _accountPaymentDetailsRepository.Get(paymentDto.AccountPaymentDetailId.Value) as BankAccountPaymentDetails
                            };
                        }
                        break;
                    }
                case PaymentTypeIds.Check:
                    {
                        if (!paymentDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }
                        if (string.IsNullOrWhiteSpace(paymentDto.BeneficiaryName))
                        {
                            error.Errors.Add(new ErrorData { ID = "BeneficiaryNameBR" });
                            valid = false;
                        }
                        if (string.IsNullOrWhiteSpace(paymentDto.CheckNo))
                        {
                            error.Errors.Add(new ErrorData { ID = "CheckNoBR" });
                            valid = false;
                        }
                        if (!paymentDto.DueDate.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "DueDateBR" });
                            valid = false;
                        }

                        if (valid)
                        {
                            payment = new PaymentCheck()
                            {
                                Type = Domain.Model.Account.Payment.PaymentType.Check,
                                BeneficiaryName = paymentDto.BeneficiaryName,
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(paymentDto.SystemPaymentDetailId.Value) as BankAccountPaymentDetails,
                                CheckNo = paymentDto.CheckNo,
                                DueDate = paymentDto.DueDate.Value,
                                TransactionId = paymentDto.CheckNo
                            };
                        }
                        break;
                    }
                case PaymentTypeIds.PayPal:
                    {

                        if (!paymentDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }
                        if (!paymentDto.AccountPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "AccountPaymentDetailBR" });
                            valid = false;
                        }
                        if (valid)
                        {
                            payment = new PaymentPaypal()
                            {
                                Type = Domain.Model.Account.Payment.PaymentType.PayPal,
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(paymentDto.SystemPaymentDetailId.Value) as PayPalAccountPaymentDetails,
                                AccountPaymentDetail = _accountPaymentDetailsRepository.Get(paymentDto.AccountPaymentDetailId.Value) as PayPalAccountPaymentDetails
                            };
                        }
                        break;
                    }
                default:
                    {
                        var type = _paymentTypeRepository.Get((int)paymentDto.PaymentType);
                        if (type == null)
                        {
                            error.Errors.Add(new ErrorData { ID = "PaymentTypeBR" });
                            break;
                        }
                        else
                        {
                            payment = new Payment
                            {
                                Type = type,
                                AdFalconReceiptNo = GetReceiptNumber(),
                            };
                        }
                        break;
                    }
            }
            if (error.Errors.Count > 0)
            {
                throw (error);
            }

            payment.Amount = paymentDto.Amount.Value;
            payment.VATAmount = paymentDto.VATAmount.HasValue ? paymentDto.VATAmount.Value : 0;
            payment.User = currentAccount.PrimaryUser;
            payment.TransactionId = paymentDto.TransactionId;
            if ((PaymentTypeIds)paymentDto.PaymentType == PaymentTypeIds.Check)
            {
                payment.TransactionId = paymentDto.CheckNo;
            }
            payment.Comment = paymentDto.Comment;
            payment.TransactionDate = paymentDto.TransactionDate.Value;
            payment.AdFalconReceiptNo = GetReceiptNumber();
            payment.Currency = Currency.USD;
            payment.OriginalAmount = payment.Amount;
            payment.ForMonth = paymentDto.ForMonth.HasValue ? new DateTime?(paymentDto.ForMonth.Value) : null;

            int attachmentId;
            if (int.TryParse(paymentDto.AttachmentId, out attachmentId))
            {
                payment.Attachment = _documentRepository.Get(attachmentId);
                payment.Attachment.UpdateUsage();
            }

            account.AddPayment(payment);
            _paymentRepository.Save(payment);
            _accountRepository.Save(account);
            //account.PublishAccountAmountForKafka();
            #region EventBroker

            Dictionary<string, object> extraParameter = new Dictionary<string, object>();
            extraParameter.Add("Id", payment.ID);
            extraParameter.Add("NotifyUser", paymentDto.NotifyUser);

            EventBrokerContext.Current.ExtraParameters.Add(extraParameter);

            #endregion
        }

        /// <summary>
        /// Get Account Payment History
        /// </summary>
        /// <returns></returns>
        public PaymentDtoResult GetAccountPaymentsHistory(HistoryCriteriaDto fundsCriteria)
        {
            var result = new PaymentDtoResult();
            var list = _paymentRepository.Query((p => p.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && (p.TransactionDate >= fundsCriteria.FromDate && p.TransactionDate <= fundsCriteria.ToDate)), fundsCriteria.PageNumber, fundsCriteria.ItemsPerPage, (p => p.TransactionDate), fundsCriteria.Ascending);
            result.Items = list.Select(p => MapperHelper.Map<PaymentDto>(p)).ToList();

            result.Total = _paymentRepository.Query((p => p.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && (p.TransactionDate >= fundsCriteria.FromDate && p.TransactionDate <= fundsCriteria.ToDate))).Count();

            return result;
        }

        public IList<PaymentDetailDto> GetPaymentDetails(GetPaymentDetailsRequest request)
        {
            var list = _accountPaymentDetailsRepository.Query(x => x.Account.ID == request.AccountId && x.IsActive && x.AccountType == request.PaymentAccountType);
            return list.Select(x => MapperHelper.Map<PaymentDetailDto>(x)).ToList();
        }

        public IList<PaymentFullDetailDto> GetFullPaymentDetails(GetFullPaymentDetailsRequest request)
        {
            var list = _accountPaymentDetailsRepository.Query(x => x.Account.ID == request.AccountId && x.IsActive && x.AccountType == request.PaymentAccountType && (x.SubType == PayemntAccountSubType.Both || x.SubType == request.PaymentAccountSubType));
            return list.Select(x => MapperHelper.Map<PaymentFullDetailDto>(x)).ToList();
        }

        /// <summary>
        /// Get Account current Discount
        /// </summary>
        /// <returns></returns>
        public AccountDiscountDto GetAccountDiscount(ValueMessageWrapper<int> accountId)
        {
            var item = _accountDiscountRepository.Query(x =>
                 x.Account.ID == accountId.Value && x.Discount.FromDate <= Framework.Utilities.Environment.GetServerTime() &&
                 (!x.Discount.ToDate.HasValue || x.Discount.ToDate >= Framework.Utilities.Environment.GetServerTime())).FirstOrDefault();
            if (item != null)
            {
                return MapperHelper.Map<AccountDiscountDto>(item);
            }
            else
            {
                return null;
            }

        }

        public AccountSettingDto GetAccountSetting(ValueMessageWrapper<int> accountId)
        {
            var result = new AccountSettingDto();
            var account = _accountRepository.Get(accountId.Value);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }
            var currentTime = Framework.Utilities.Environment.GetServerTime().AddSeconds(1);
            var item = _accountDiscountRepository.Query(x =>
                x.Account.ID == accountId.Value && x.Discount.FromDate <= currentTime &&
                (!x.Discount.ToDate.HasValue || x.Discount.ToDate >= currentTime)).FirstOrDefault();
            if (item != null)
            {
                result.Discount = (float?)(item.Discount.Value * 100);
            }
            result.RevenuePercentage = account.DefaultRevenuePercentage * 100;
            result.OverDraft = account.AccountSummary.Credit;
            result.AllowAPIAccess = account.AllowAPIAccess;

            result.AgencyCommission = account.getAgencyCommission();

            if (account.AgencyCommission == AgencyCommission.FixedCPM)
                result.AgencyCommissionValue =  account.AgencyCommissionValue * 1000;
            else
                result.AgencyCommissionValue =  account.AgencyCommissionValue *100;

           // result.AgencyCommissionValue = account.AgencyCommissionValue;
            return result;
        }

        public void SaveAccountSetting(AccountSettingDto settings)
        {
            //TODO:Osaleh to check user permission
            var account = _accountRepository.Get(settings.AccountId);
            bool originalAllowAPIAccessValue = account.AllowAPIAccess;

            if (account == null)
            {
                throw new AccountNotFoundException();
            }
            //Discount 
            if ((settings.Discount.HasValue) && (settings.Discount > 0))
            {
                // create discount object
                var discount = new Discount { Value = (decimal)(settings.Discount.Value / 100) };
                // update discount
                account.AddDiscount(discount);
            }
            else
            {
                // remove discount
                account.RemoveDiscount();
            }
            //Revenue Percentage
            if (settings.RevenuePercentage.HasValue)
            {
                account.DefaultRevenuePercentage = settings.RevenuePercentage / 100;
            }
            else
            {
                account.DefaultRevenuePercentage = null;
            }
            var previousCredit=account.AccountSummary.Credit;
            account.AccountSummary.Credit = settings.OverDraft;
            account.AccountSummary.FundsDelta = settings.OverDraft- previousCredit;
            account.AllowAPIAccess = settings.AllowAPIAccess;
            if (settings.AgencyCommission != null)
            {
                account.setAgencyCommission(settings.AgencyCommission);
                if (account.AgencyCommission == AgencyCommission.FixedCPM)
                    account.AgencyCommissionValue = settings.AgencyCommissionValue / 1000;
                else
                    account.AgencyCommissionValue = settings.AgencyCommissionValue / 100;
            }
            _accountRepository.Save(account);

            if (originalAllowAPIAccessValue != settings.AllowAPIAccess)
            {
                AdFalconUserInfo userInfo = OperationContext.Current.UserInfo<AdFalconUserInfo>();
                userInfo.AllowAPIAccess = settings.AllowAPIAccess;
                OperationContext.Current.UserInfo<AdFalconUserInfo>(userInfo);
            }
        }
        public void SaveCampAccountSetting(AccountSettingDto settings)
        {
            //TODO:Osaleh to check user permission
            var account = _accountRepository.Get(settings.AccountId);
       

            if (account == null)
            {
                throw new AccountNotFoundException();
            }
            //Discount 
          
            account.setAgencyCommission( settings.AgencyCommission);
            if(account.AgencyCommission==AgencyCommission.FixedCPM)
                account.AgencyCommissionValue = settings.AgencyCommissionValue/1000;
            else
                account.AgencyCommissionValue = settings.AgencyCommissionValue / 100;
            _accountRepository.Save(account);

          
        }
        public AccountAPIAccessDto GetAccountAPIAccessByAPIClientId(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var account = _accountRepository.Query(p => p.APIAccess.APIClientId == clientId).SingleOrDefault();

                if (account != null)
                {
                    if (!account.AllowAPIAccess)
                    {
                        var error = new BusinessException();
                        throw error;
                    }

                    return MapperHelper.Map<AccountAPIAccessDto>(account);
                }
            }

            return null;
        }

        public AccountAPIAccessDto GetAPIAccessSetting()
        {
            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            var accountInfo = _accountRepository.Get(accountId);

            return MapperHelper.Map<AccountAPIAccessDto>(accountInfo);

        }

        public AccountAPIAccessDto GenerateAPIAccess()
        {
            int accountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;

            var accountInfo = _accountRepository.Get(accountId);

            if (accountInfo.AllowAPIAccess)
            {
                if (accountInfo.APIAccess == null)
                {
                    AccountAPIAccess apiAccess = new AccountAPIAccess(accountInfo);

                    accountInfo.APIAccess = apiAccess;
                    apiAccess.APIClientId = Guid.NewGuid().ToString("N");
                    apiAccess.APISecretKey = Guid.NewGuid().ToString("N");

                    _accountRepository.Save(accountInfo);
                }
            }
            else
            {

                var notAllowedError = new ErrorData();
                notAllowedError.ID = "DenyAPIAccessWarning";
                List<ErrorData> errors = new List<ErrorData>();
                errors.Add(notAllowedError);

                throw new BusinessException(errors);
            }

            return MapperHelper.Map<AccountAPIAccessDto>(accountInfo);
        }

        //public bool GoDSP(int accountid, bool IsNew = false)
        //{
        //    try
        //    {
        //        var account = _accountRepository.Get(accountid);
        //        account.GoDSP();
        //        if (IsNew)
        //        {
        //            _securityService.ActivateUser(account.PrimaryUser.EmailAddress);

        //            account.PrimaryUser.Activate();
        //            account.PrimaryUser.Status = new UserStatus();
        //            account.PrimaryUser.Status.SetActiveStatus();
        //            _accountRepository.Save(account);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }


        //    return true;
        //}

        public ValueMessageWrapper<bool> checkAdPermissions(ValueMessageWrapper<PortalPermissionsCode> Code)
        {

            bool result = _AccountPortalPermissionsRepository.checkAdPermissions(Code.Value);

            return ValueMessageWrapper.Create(result);
        }

        public ValueMessageWrapper<int> GetAccountRole(ValueMessageWrapper<int> id)
        {
            int AccountRole = 0;
            if (_accountRepository.Get(id.Value) != null)
                AccountRole = (int)_accountRepository.Get(id.Value).AccountRole;
            else
                throw new Exception("No Such Account with the Given Id");

            return ValueMessageWrapper.Create(AccountRole);
        }

        public ImpressionLogListResultDto ImpressionLogQueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.ImpressionLogCriteria wcriteria)
        {

            ImpressionLogCriteria criteria = new ImpressionLogCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            if (criteria.DataFrom.HasValue)
            {
                criteria.DataFromInt = Convert.ToInt32(criteria.DataFrom.Value.ToString("yyyyMMdd"));
            }
            if (criteria.DataTo.HasValue)
            {

                criteria.DataToInt = Convert.ToInt32(criteria.DataTo.Value.ToString("yyyyMMdd"));

            }
            var result = new ImpressionLogListResultDto();
            IEnumerable<ImpressionLog> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _impressionLogRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _impressionLogRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(ImpressionLog => MapperHelper.Map<ImpressionLogDto>(ImpressionLog)).ToList();


            result.Items = returnList;
            result.TotalCount = _impressionLogRepository.Query(criteria.GetExpression()).Count();
            return result;
        }

        public ImpressionLogDto GetImpressionLogById(ValueMessageWrapper<int> Id)
        {
            var impLog = _impressionLogRepository.Query(M => M.ID == Id.Value).SingleOrDefault();
            return MapperHelper.Map<ImpressionLogDto>(impLog);


        }


        #region Create User/Account

        //creates a second account for already exists user, or creates both if the user doesn't exist
        public UserDto CreateAccount(Interfaces.DTOs.Account.UserDto userDtoInfo)
        {
            var Users = _userRepository.Query(p => p.EmailAddress == userDtoInfo.EmailAddress.ToLower() || p.PendingEmailAddress == userDtoInfo.EmailAddress.ToLower()).ToList();
            int accountCount = _accountRepository.Query(x => x.PrimaryUser.EmailAddress.ToLower() == userDtoInfo.EmailAddress.ToLower()).Count();

            if (Users.Count() == 0)
            {
                return CreateUserAccount(userDtoInfo);
            }
            else
            {
                if (accountCount > 1)
                {
                    throw new UserEmailAlreadyExistsException();
                }
                // AccountN.Account ParentAccount = Users[0].UserAccounts!=null && Users[0].UserAccounts.Count> 0 ? Users[0].UserAccounts[0].Account:null;
                //Users[0].Account = null;
                AccountN.Account SecondAccount = CreateAccountHelper(Users[0], null);
                //Users[0].Account = ParentAccount;
                // SecondAccount.Parent = ParentAccount;
                _accountRepository.Save(SecondAccount);
                if (userDtoInfo.IsAccountDSP)
                {
                    SecondAccount.GoDSP();
                }
                Users[0].AttachAccount(SecondAccount.ID, UserType.Normal);
                UserDto FinalUser = MapperHelper.Map<UserDto>(Users[0]);

                return FinalUser;

            }

        }
        public int InvitationCount(string email)
        {
            int invitations = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == email.ToLower() && x.IsAccepted == false).Count();

            return invitations;
        }
        public int InvitationAcceptedCount(string email)
        {
            int invitations = _accountInvitationRepository.Query(x => x.EmailAddress.ToLower() == email.ToLower() && x.IsAccepted == true).Count();

            return invitations;
        }
        public UserDto CreateUserAccount(Interfaces.DTOs.Account.UserDto userDtoInfo)
        {
            if (_userDomainService.GetUserByEmail(userDtoInfo.EmailAddress, false) != null)
            {if (string.IsNullOrEmpty(userDtoInfo.Invitationcode))
                    throw new UserEmailAlreadyExistsException();
                else
                {
                  // var  result = InvitationCount(userDtoInfo.EmailAddress) > 0 && InvitationAcceptedCount(userDtoInfo.EmailAddress) == 0;
                    if(!userDtoInfo.AlreadyReg)
                        throw new UserEmailAlreadyExistsException();
                }
            }
          
            var addUserResult = false;
            if (!userDtoInfo.AlreadyReg && !_securityService.CheckUserExists(new ValueMessageWrapper<string> { Value = userDtoInfo.EmailAddress }).Result.Value)
                addUserResult = _securityService.CreateUser(new CreateUserRequest { Email = userDtoInfo.EmailAddress, Password = userDtoInfo.Password, UserName = userDtoInfo.EmailAddress }).Result.Value;
            if (userDtoInfo.AlreadyReg)
                addUserResult = true;
            if (addUserResult)
            {

                User userInfo = CreateUserHepler(userDtoInfo);
                var invitation = _accountInvitationRepository.Query(x => x.EmailAddress == userDtoInfo.EmailAddress && x.InvitationCode == userDtoInfo.Invitationcode).FirstOrDefault();
                ArabyAds.AdFalcon.Domain.Model.Account.Account Account = null;
                if (invitation != null)
                {
                    Account = CreateAccountHelper(userInfo, invitation.Account);


                }
                else
                    Account = CreateAccountHelper(userInfo, userInfo.UserAccounts != null && userInfo.UserAccounts.Count > 0 ? userInfo.UserAccounts[0].Account : null);

                if (userDtoInfo.IsAccountDSP)
                {
                    Account.GoDSP(false);
                }
                userInfo.AttachAccount(Account.ID, UserType.Normal);

                _userRepository.Save(userInfo);

                UserDto FinalUser = MapperHelper.Map<UserDto>(userInfo);
                return FinalUser;
            }
            else
            {
                throw new UserEmailAlreadyExistsException();
            }

        }


        #region Helers

        private User CreateUserHepler(UserDto userDtoInfo)
        {
            User userInfo = MapperHelper.Map<User>(userDtoInfo);
            if (!userDtoInfo.AlreadyReg)
            {
                userInfo.RegistrationDate = Framework.Utilities.Environment.GetServerTime();
                userInfo.SetActivationCode();
                userInfo.Country = new Country { ID = userDtoInfo.Country };
                userInfo.ChangePassword(userDtoInfo.Password);
                userInfo.Status = new UserStatus();
                userInfo.Status.SetPendingStatus();
                userInfo.Language = new Language { ID = userDtoInfo.Language };
            }
            else
            {
                userInfo = _userRepository.Query(M => M.EmailAddress == userDtoInfo.EmailAddress).SingleOrDefault();
            }
            var invitation = _accountInvitationRepository.Query(x => x.EmailAddress == userDtoInfo.EmailAddress && x.InvitationCode == userDtoInfo.Invitationcode).FirstOrDefault();
            if (invitation != null)
            {
                invitation.IsAccepted = true;
                _accountInvitationRepository.Save(invitation);
                _securityService.ActivateUser(ValueMessageWrapper<string>.Create(userInfo.EmailAddress));

                userInfo.AttachAccount(invitation.Account.ID,invitation.UserType);
                userInfo.Activate();

            }

            _userRepository.Save(userInfo);

            if (invitation != null)
            {
                var listOfAdv= _AdvertiserAccountReadOnlyUserRepository.Query(M=>M.Invitation.ID==invitation.ID&& M.IsDeleted==false);
                if (listOfAdv!=null)
                {
                    foreach (var item in listOfAdv)
                    {
                        item.User = userInfo;
                        _AdvertiserAccountReadOnlyUserRepository.Save(item);
                    }
                }
            }
            return userInfo;
        }
        private AccountN.Account CreateAccountHelper(User userInfo, AccountN.Account Account)
        {
            if (Account == null)
            {
                var newAccount = new Domain.Model.Account.Account();

                var accountSummary = new AccountSummary(newAccount);
                newAccount.AccountSummary = accountSummary;
                newAccount.PrimaryUser = new User { ID = userInfo.ID, EmailAddress = userInfo.EmailAddress };
                newAccount.Name = userInfo.GetAccountName();
                newAccount.AccountBusinessId = GetAccountCounter();
                newAccount.UserAgreementVersion = _configurationManager.GetConfigurationSetting(null, null, "UserAgreementVersion");
                newAccount.AccountRole = AccountRole.NormalUser;
                _accountRepository.Save(newAccount);
                return newAccount;

            }
            else
            {
                return Account;
            }
        }
        #endregion


        #endregion


        public string GetAccountName(ValueMessageWrapper<int> AccountId)
        {

            var account = _accountRepository.Get(AccountId.Value);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            return account.GetName();

        }


        public UsersListResultDto QueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.UserCriteriaBase wcriteria)
        {
            //var test = OperationContext.Current.UserInfo<AdFalconUserInfo>().AdPermission;
            UserCriteriaBase criteria = new UserCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new UsersListResultDto();
            int Count = 0;
            //IEnumerable<AppSite> list = appSiteRepository.Query(criteria.GetExpression(), criteria.Page - 1, criteria.Size, item => item.ID, true);

            var list = _accountRepository.QueryByCratiriaForUsers(criteria, out Count);
            //    var list2 = _accountRepository.QueryByCratiriaForUsers(criteria, out Count);

            //var listCount = _userRepository.QueryByCratiriaForUsers(criteria, out Count);
            var pageItems = list/*.OrderBy(c => c.FirstName)*//*.Skip(criteria.Page * criteria.Size).Take(criteria.Size)*/.ToList();
            result.Items = pageItems.Select(user => MapperHelper.Map<UserDto>(user)).ToList();
            result.TotalCount = Count;
            return result;
        }

        public UserDto GetById(ValueMessageWrapper<int> id)
        {
            //var test = OperationContext.Current.UserInfo<AdFalconUserInfo>().AdPermission;


         
            var item = _accountRepository.Get(id.Value);

            return MapperHelper.Map<UserDto>(item);
          
          
        }

        public AdFalconUserInfo BuildAdFalconUser(BuildAdFalconUserRequest request)
        {
            var account = _accountRepository.Get(request.AccountId);
            var userac= _UserAccountsRepositor.Query(X=>X.User.ID== request.UserId && X.Account.ID == request.AccountId).SingleOrDefault();
         var userobj=   _userRepository.Get(request.UserId);
            if (account != null && (userac!=null))
            {
                var Permissions = _AccountPortalPermissionsRepository.GetAccountAdPermissions(request.AccountId).Select(x => (int)x.Code).ToArray(); ;

                return new AdFalconUserInfo(userobj.FirstName, userobj.LastName, userobj.ID, request.AccountId, account.UserAgreementVersion, account.AllowAPIAccess, account.PrimaryUser.ID == request.UserId || (userac.UserType==UserType.Primary), Permissions, (int)account.AccountRole, account.GetVATValue(), userobj.EmailAddress, userobj.Company, (userac.UserType == UserType.ReadOnly));
            }
            else
                return null;
        }

        public IList<UserDto> GetUserAccounts(ValueMessageWrapper<int> userid)
        {
            var accounts = _UserAccountsRepositor.Query(M => M.User.ID == userid.Value).Select(M => M.Account).ToList();

            return accounts.Select(acc => MapperHelper.Map<UserDto>(acc)).ToList();

        }
        public ValueMessageWrapper<int> GetUserAccountsCount(ValueMessageWrapper<int> userid)
        {
            int accounts = _UserAccountsRepositor.Query(M => M.User.ID == userid.Value).Select(M => M.Account).Count();

            return ValueMessageWrapper.Create( accounts);

        }

        public ValueMessageWrapper<int> GetFirstUserAccountId(ValueMessageWrapper<int> userid)
        {
            var accounts = _UserAccountsRepositor.Query(M => M.User.ID == userid.Value).Select(M => M.Account).FirstOrDefault();
            if (accounts != null)
                return ValueMessageWrapper.Create(accounts.ID);
            else
                return ValueMessageWrapper.Create(0);
        }
        public IList<UserDto> GetUserAccountsByEmail(string email)
        {
            var accounts = _UserAccountsRepositor.Query(M => M.User.EmailAddress.ToLower() == email.ToLower()).Select(M => M.Account).ToList();

            return accounts.Select(acc => MapperHelper.Map<UserDto>(acc)).ToList();

        }

        public ValueMessageWrapper<int> SaveDSPAccountSettingReport(AccountDSPsettingDTO itemDtoVar)
        {


            var item = _DSPAccountSettingRepositor.Get(itemDtoVar.ID);

            if (item == null)
            {
                item = MapperHelper.Map<Domain.Model.Account.DSPAccountSetting>(itemDtoVar);

            }
            else
            {

                item.BillToAddressPersonName = itemDtoVar.BillToAddressPersonName;
                item.BillToAddress1 = itemDtoVar.BillToAddress1;
                item.BillToAddress2 = itemDtoVar.BillToAddress2;
                item.BillingContactName = itemDtoVar.BillingContactName;
                item.AgencyCommission = itemDtoVar.AgencyCommission;
                item.BusinessName = itemDtoVar.BusinessName;

            }


            if (itemDtoVar.AllContacts != null && itemDtoVar.AllContacts.Count() > 0)
            {
                var Contacts = item.Contacts;

                foreach (var Contact in itemDtoVar.AllContacts)
                {
                    if (!string.IsNullOrEmpty(Contact.Email))
                    {
                        if (!Contacts.Any(p => !p.IsDeleted && p.Email == Contact.Email))
                        {
                            Contacts.Add(new Domain.Model.Account.DSPAccountSettingContact() { Email = Contact.Email, DSPAccountSetting = item });
                        }
                    }
                }

                foreach (var Contact in Contacts.Where(p => !p.IsDeleted && !itemDtoVar.AllContacts.Any(z => z.Email == p.Email)))
                {
                    Contact.IsDeleted = true;
                }

            }
            else
            {
                if (item.Contacts != null && item.Contacts.Count() > 0)
                {
                    foreach (var Contact in item.Contacts)
                    {
                        Contact.IsDeleted = true;
                        _DSPAccountSettingContactRepositor.Save(Contact);
                    }

                }
            }

            if (item.Contacts != null && item.Contacts.Count() > 0)
            {
                foreach (var Contact in item.Contacts)
                {
                    Contact.DSPAccountSetting = item;
                }

            }

            _DSPAccountSettingRepositor.Save(item);

            return ValueMessageWrapper.Create(item.ID);

        }

        public AccountDSPsettingDTO GetDSPAccountSettingReport(ValueMessageWrapper<int> accountId)
        {
            var item = _DSPAccountSettingRepositor.Query(x => x.Account.ID == accountId.Value).FirstOrDefault();
            AccountDSPsettingDTO settings = new AccountDSPsettingDTO();
            settings.AgencyCommission = AgencyCommission.FixedCPM;
            if (item != null)
            {
                settings = MapperHelper.Map<AccountDSPsettingDTO>(item);

            }
            return settings;

        }
        public ValueMessageWrapper<decimal> GetVATValueByAccountId(ValueMessageWrapper<int> id)
        {
            ArabyAds.AdFalcon.Domain.Model.Account.Account account = _accountRepository.Get(id.Value);
            if (account != null)
            {
                return ValueMessageWrapper.Create(account.GetVATValue());
            }
            else
            {
                return ValueMessageWrapper.Create(0M);
            }

        }

        #region Features
        public void SetFeature(ValueMessageWrapper<int> code)
        {
            Feature feature = _featureRepository.Query(x => (int)x.Code == code.Value).FirstOrDefault();

            AccountFeatures AccountFeatures = new AccountFeatures
            {
                Account = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value },
                User = new User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value },
                Feature = new Feature { ID = feature.ID },
                DateNotify= Framework.Utilities.Environment.GetServerTime()
            };

            _accountFeaturesRepository.Save(AccountFeatures);
        }

        public ValueMessageWrapper<bool> HadAFeature(ValueMessageWrapper<int> code)
        {
            int count = _accountFeaturesRepository.Query(x =>
            x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value &&
            (int)x.Feature.Code == code.Value
            ).Count();

            return ValueMessageWrapper.Create(count > 0);
        }
        #endregion
    }
}
