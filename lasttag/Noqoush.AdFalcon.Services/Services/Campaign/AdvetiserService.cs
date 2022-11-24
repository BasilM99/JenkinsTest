using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.AdFalcon.Persistence.Reports.Repositories;
using Noqoush.Framework;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using NHibernate;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.UserInfo;
using Noqoush.AdFalcon.Base;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Services.Campaign
{
    public class AdvertiserService : IAdvertiserService
    {
        private string codeForFirstParty = "fpaud";
        private IAccountRepository _AccountRepository = null;
        private IAdvertiserRepository AdvertiserRepository = null;
        private IAdvertiserAccountRepository _advertiserAccountRepository = null;
        private IMasterAppSiteTargetingRepository _MasterAppSiteTargetingReposiory = null;

        private IPixelRepository _pixelRepository = null;


        private IAdvertiserAccountMasterAppSiteRepository _advertiserAccountMasterAppSiteRepository = null;
        private IAdvertiserAccountMasterAppSiteItemRepository _advertiserAccountMasterAppSiteItemRepository = null;
        private ICampaignRepository CampaignRepository = null;
        private ISummaryRepository _summaryRepository;
        private IAdvertiserAccountUserRepository _advertiserAccountUserRepository = null;
        private IDSPAccountSettingRepository _DSPAccountSettingRepository = null;

        private IConfigurationManager configurationManager = null;
        private IAudienceSegmentRepository _AudienceSegmentRepository = null;

        private IAudienceSegmentOccupationRepository _AudienceSegmentOccupationRepository = null;
        private IDPPartnerRepository _DPPartnerRepository = null;
        private IlookalikejobRepository _IlookalikejobRepository = null;
        private IAdvertiserAccountUserRepository _AdvertiserAccountReadOnlyUserRepository = null;
        public AdvertiserService(IAdvertiserRepository AdvertiserRepository, IConfigurationManager configurationManager, IAdvertiserAccountRepository AdvertiserAccountRepository, ICampaignRepository CampaignRepository, ISummaryRepository summaryRepository
            , IAdvertiserAccountUserRepository advertiserAccountUserRepository
            , IAdvertiserAccountMasterAppSiteRepository AdvertiserAccountMasterAppSiteRepository
            , IAdvertiserAccountMasterAppSiteItemRepository AdvertiserAccountMasterAppSiteItemRepository
                 , IMasterAppSiteTargetingRepository MasterAppSiteTargetingReposiory,
            IDSPAccountSettingRepository DSPAccountSettingRepository,

                   IAccountRepository AccountFRepository,
                   IAudienceSegmentOccupationRepository AudienceSegmentOccupationRepository,
                     IAudienceSegmentRepository AudienceSegmentRepository,

                     IDPPartnerRepository DPPartnerRepository,


                     IPixelRepository PixelRepository,

                     IAdvertiserAccountUserRepository AdvertiserAccountReadOnlyUserRepositor,

                    IlookalikejobRepository lookalikejobRepository
            )
        {
            _advertiserAccountRepository = AdvertiserAccountRepository;
            this.CampaignRepository = CampaignRepository;
            this._AdvertiserAccountReadOnlyUserRepository = AdvertiserAccountReadOnlyUserRepositor;
            this.AdvertiserRepository = AdvertiserRepository;
            this.configurationManager = configurationManager;
            _summaryRepository = summaryRepository;
            _advertiserAccountUserRepository = advertiserAccountUserRepository;

            _advertiserAccountMasterAppSiteRepository = AdvertiserAccountMasterAppSiteRepository;
            _advertiserAccountMasterAppSiteItemRepository = AdvertiserAccountMasterAppSiteItemRepository;
            _MasterAppSiteTargetingReposiory = MasterAppSiteTargetingReposiory;
            _DSPAccountSettingRepository = DSPAccountSettingRepository;
            _AccountRepository = AccountFRepository;
            this._AudienceSegmentOccupationRepository = AudienceSegmentOccupationRepository;
            this._AudienceSegmentRepository = AudienceSegmentRepository;
            this._DPPartnerRepository = DPPartnerRepository;

            this._pixelRepository = PixelRepository;
            _IlookalikejobRepository = lookalikejobRepository;
            if (System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"] != null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"]))
            {

                codeForFirstParty = System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"];

            }
        }

        public AdvertiserDto Get(int id)
        {
            var Advertiser = AdvertiserRepository.Get(id);
            if (Advertiser != null)
            {
                return MapperHelper.Map<AdvertiserDto>(Advertiser);
            }
            return null;
        }

        public IEnumerable<AdvertiserDto> GetAll()
        {
            IEnumerable<Advertiser> list = AdvertiserRepository.GetAll();
            return list.Select(AdvertiserDto => MapperHelper.Map<AdvertiserDto>(AdvertiserDto)).ToList();
        }
        public AdvertiserAccountListResultDto GetAccountAdvertiser(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountCriteria wcriteria)
        {
            AdvertiserAccountCriteria criteria = new AdvertiserAccountCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AdvertiserAccountListResultDto();
            criteria.IsReadOnly = Framework.OperationContext.Current.UserInfo<IUserInfo>().IsReadOnly;
            IEnumerable<AdvertiserAccount> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _advertiserAccountRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _advertiserAccountRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<AdvertiserAccountListDto>(AdvertiserAccount)).ToList();
            if (criteria.Page.HasValue)
            {
                #region Performance
                var performance = new PerformanceCriteria
                {
                    FromDate = criteria.DataFrom,
                    ToDate = criteria.DataTo,
                    Ids = returnList.Select(obj => obj.Id).ToList()
                };

                var performances = _summaryRepository.GetAdvertisersPerformance(performance);


                var idStatus = _summaryRepository.GetAdsByAdvertiser(performance);
                //   var idStatus = _summaryRepository.GetAdsByCampaign(performance);

                foreach (var advertiserListDto in returnList)
                {
                    advertiserListDto.Performance = performances.FirstOrDefault(item => item.DimAdvID == advertiserListDto.Id);

                    if (advertiserListDto.Performance == null)
                    {
                        advertiserListDto.Performance = new Interfaces.DTOs.Reports.AdvertiserPerformanceDto();

                    }

                    if (!advertiserListDto.IsDeleted)
                    {
                        //load Ad Status
                        var advAds = idStatus.Where(ad => ad.AdvertiserId == advertiserListDto.AdvertiserItem.ID).ToList();

                        advertiserListDto.Status = _summaryRepository.CalculateAdvertiserStatus(advAds);

                    }
                    else
                    {
                        advertiserListDto.Status = Framework.Resources.ResourceManager.Instance.GetResource("StatusNotActive", "PMPDeals");
                    }
                }


                #endregion

                result.TotalCount = _advertiserAccountRepository.Query(criteria.GetExpression()).Count();
            }

            result.Items = returnList;

            return result;

        }
        public AdvertiserAccountListDto GetAccountAdvertiserById(int Id)
        {

            var result = new AdvertiserAccountListResultDto();

            IEnumerable<AdvertiserAccount> list = null;

            var item = _advertiserAccountRepository.Get(Id);

            var returnItem = MapperHelper.Map<AdvertiserAccountListDto>(item);

            ValidateAdvertiser(Id);
            return returnItem;

        }
        public void SaveAdvertiserAccount(AdvertiserAccountDto item)
        {
            AdvertiserAccount AdvertiserAccount = MapperHelper.Map<AdvertiserAccount>(item);
            if (_advertiserAccountRepository.Query(x => x.Account.ID == item.AccountId && x.Advertiser.ID == item.Advertiser.ID && x.Name.ToLower().Contains(item.Name.ToLower()) && x.ID != item.Id && !x.IsDeleted).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicatedAdvertiser") });

            }
            else
            {
                var obj = _advertiserAccountRepository.Query(x => x.Account.ID == item.AccountId && x.Name.ToLower().Contains(item.Name.ToLower()) && x.Advertiser.ID == item.Advertiser.ID && x.ID != item.Id).FirstOrDefault();
                if (obj == null)
                {
                    //AdvertiserAccount.IsRestricted = false;

                    AdvertiserAccount.Account = new Domain.Model.Account.Account { ID = item.AccountId };
                    AdvertiserAccount.User = new Domain.Model.Account.User { ID = item.UserId };
                    AdvertiserAccount.Name = item.Name;
                    var accDsPSetting = _AccountRepository.Get(item.AccountId);
                    if (accDsPSetting != null/* && accDsPSetting.AccountRole==AccountRole.DSP*/)
                    {
                        AdvertiserAccount.AgencyCommission = accDsPSetting.AgencyCommission;
                        AdvertiserAccount.AgencyCommissionValue = accDsPSetting.AgencyCommissionValue;


                    }
                    //else
                    //{
                    //    AdvertiserAccount.AgencyCommission = AgencyCommission.FixedCPM;

                    //}

                    _advertiserAccountRepository.Save(AdvertiserAccount);
                }
                else
                {
                    obj.IsDeleted = false;
                    _advertiserAccountRepository.Save(obj);

                }
            }
        }

        public void SaveAdvertiserAccountSettings(AdvertiserAccountSettings item)
        {
            if (item.Assignments != null && item.Assignments.Count() > 0)
            {
                foreach (AdvertiserAccountUserDto Assignment in item.Assignments)
                {
                    AdvertiserAccountUser AdvertiserAccountUser = _advertiserAccountUserRepository.Query(x => x.User.ID == Assignment.User.Id && x.Link.ID == Assignment.Link.Id).FirstOrDefault();

                    if (AdvertiserAccountUser == null)
                    {
                        AdvertiserAccountUser = MapperHelper.Map<AdvertiserAccountUser>(Assignment);

                    }
                    else
                    {
                        AdvertiserAccountUser.Read = Assignment.Read;
                        AdvertiserAccountUser.Write = Assignment.Write;
                        AdvertiserAccountUser.IsDeleted = false;
                    }
                    _advertiserAccountUserRepository.Save(AdvertiserAccountUser);

                }
            }
            else
            {
                item.Assignments = new List<AdvertiserAccountUserDto>();
            }

            var all = _advertiserAccountUserRepository.Query(x => x.Link.ID == item.AccountAdvertiserId).ToList();
            foreach (var tempItem in all)
            {
                if (item.Assignments.Where(x => x.User.Id == tempItem.User.ID).Count() == 0)
                {
                    tempItem.IsDeleted = true;
                    tempItem.Read = false;
                    tempItem.Write = false;
                    _advertiserAccountUserRepository.Save(tempItem);
                }
            }




            AdvertiserAccount AdvertiserAccountObj = _advertiserAccountRepository.Get(item.AccountAdvertiserId);
            AdvertiserAccountObj.IsRestricted = item.IsRestricted;
            AdvertiserAccountObj.setAgencyCommission(item.AgencyCommission);

            if (AdvertiserAccountObj.AgencyCommission == AgencyCommission.FixedCPM)
                AdvertiserAccountObj.AgencyCommissionValue = item.AgencyCommissionValue / 1000;
            else
                AdvertiserAccountObj.AgencyCommissionValue = item.AgencyCommissionValue / 100;
            // AdvertiserAccountObj.AgencyCommissionValue = item.AgencyCommissionValue;
            _advertiserAccountRepository.Save(AdvertiserAccountObj);

        }

        public void SaveAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item)
        {
            if (item.LinkIds != null && item.LinkIds.Count() > 0)
            {
                AdvertiserAccountUser AdvertiserAccountUser = null;
                foreach (var Assignment in item.LinkIds)
                {
                    AdvertiserAccountUser = null;
                    if(item.UserId>0)
                        AdvertiserAccountUser = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.User.ID == item.UserId && x.Link.ID == Assignment).FirstOrDefault();
                        else if(item.InvitationId>0)
                        AdvertiserAccountUser = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Invitation.ID == item.InvitationId && x.Link.ID == Assignment).FirstOrDefault();
                    if (AdvertiserAccountUser == null)
                    {
                        AdvertiserAccountUser = new AdvertiserAccountUser();
                        if (item.UserId > 0)
                            AdvertiserAccountUser.User = new User { ID = item.UserId };
                        AdvertiserAccountUser.Link = new AdvertiserAccount { ID = Assignment };
                        if(item.InvitationId>0)
                        AdvertiserAccountUser.Invitation = new AccountInvitation { ID = item.InvitationId};
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
            if(item.InvitationId>0)
             all = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Link.ID == item.InvitationId).ToList();
            else if(item.UserId > 0)
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
                items = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.User.ID == item.UserId&& x.IsDeleted==false ).ToList();
                    else if (item.InvitationId > 0)
                items = _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Invitation.ID == item.InvitationId && x.IsDeleted == false).ToList();

            foreach (var itemOn in items)
            {
                var AdvertiserAccountReadOnlyUserOn =  new AdvertiserAccountReadOnlyUserDto
                {
                    ID = itemOn.ID,
                    
                };
                if (itemOn.User!=null)
                AdvertiserAccountReadOnlyUserOn.User = new UserDto { Id = itemOn.User.ID };

                if (itemOn.Invitation != null)
                    AdvertiserAccountReadOnlyUserOn.Invitation = new InvitationDto { id = itemOn.Invitation.ID };

                if (itemOn.Link != null)
                    AdvertiserAccountReadOnlyUserOn.Link = new AdvertiserAccountDto { Id = itemOn.Link.ID, Name=itemOn.Link.Name };


                Resultitems.Add(AdvertiserAccountReadOnlyUserOn);
            }

            return Resultitems;
        }


        public AdvertiserAccountSettings GetAdvertiserAccountSettings(int Id)
        {
            AdvertiserAccountSettings Settings = new AdvertiserAccountSettings();
            List<AdvertiserAccountUserDto> list = _advertiserAccountUserRepository.Query(x => x.Link.ID == Id && !x.IsDeleted).Select(x => MapperHelper.Map<AdvertiserAccountUserDto>(x)).ToList();
            AdvertiserAccount AdvertiserAccountObj = _advertiserAccountRepository.Get(Id);

            Settings.Assignments = list;
            Settings.AccountAdvertiserId = AdvertiserAccountObj.ID;
            Settings.IsRestricted = AdvertiserAccountObj.IsRestricted;
            Settings.AgencyCommission = AdvertiserAccountObj.getAgencyCommission();

            if (Settings.AgencyCommission == AgencyCommission.FixedCPM)
                Settings.AgencyCommissionValue = AdvertiserAccountObj.AgencyCommissionValue * 1000;
            else
                Settings.AgencyCommissionValue = AdvertiserAccountObj.AgencyCommissionValue * 100;
            // Settings.AgencyCommissionValue = AdvertiserAccountObj.AgencyCommissionValue;
            return Settings;
        }
        public bool Delete(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _advertiserAccountRepository.Get(id)))
                {

                    var performance = new PerformanceCriteria
                    {
                        FromDate = null,
                        ToDate = null,
                        Ids = new List<int>() { item.ID }
                    };

                    var performances = _summaryRepository.GetAdvertisersPerformance(performance);


                    var idStatus = _summaryRepository.GetAdsByAdvertiser(performance);
                    var advAds = idStatus.Where(ad => ad.AdvertiserId == item.Advertiser.ID).ToList();

                    var Status = _summaryRepository.CalculateAdvertiserStatus(advAds);
                    if (!item.IsDeleted && Status == Framework.Resources.ResourceManager.Instance.GetResource("InActiveAdvertisers", "Global"))
                    {
                        item.Delete();
                        _advertiserAccountRepository.Save(item);
                        DeleteAccountAdvertiserCampaigns(item.Account.ID, item.ID);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void unArchive(int id)
        {
            AdvertiserAccount advertiserAccount = _advertiserAccountRepository.Query(x => x.ID == id && x.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();
            if (advertiserAccount != null)
            {
                advertiserAccount.IsDeleted = false;
                _advertiserAccountRepository.Save(advertiserAccount);


            }
        }

        public void ValidateAdvertiser(int advertiserId, bool statusCheck = false)
        {
            AdvertiserAccount advertiserAccount = new AdvertiserAccount();
            bool isManager = IsManager();

            if (!isManager)
            {
                advertiserAccount = _advertiserAccountRepository.Query(x => x.ID == advertiserId && x.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();
                if(advertiserAccount!=null)
                advertiserAccount.Validate(Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().IsPrimaryUser == false, statusCheck);
                else
                    throw new DataNotFoundException();
            }
            else
            {
                bool isLinked = _advertiserAccountRepository.Query(x => x.ID == advertiserId && !x.IsDeleted).Count() > 0;
                if (!isLinked)
                {

                    throw new DataNotFoundException();
                }
            }
        }
        public bool IsSubUserHasWriteMode(int advertiseraccId)
        {

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return true;
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            { return false; }
                var advAss = _advertiserAccountRepository.Query(x => x.ID == advertiseraccId).Single();
            if (!advAss.IsRestricted)
                return true;
            var obj = _advertiserAccountUserRepository.Query(x => x.Link.ID == advertiseraccId && x.IsDeleted == false && x.User.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().UserId.Value).FirstOrDefault();
            if (obj != null)
                return obj.Write;




            return false;
        }
        private bool IsManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AdOps")
                || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator")
                || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager");
        }
        private void DeleteAccountAdvertiserCampaigns(int accountId, int advertiserId)
        {
            List<Noqoush.AdFalcon.Domain.Model.Campaign.Campaign> Campaigns = CampaignRepository.Query(x => x.Account.ID == accountId && x.AdvertiserAccount.ID == advertiserId && x.IsDeleted == false).ToList();
            foreach (var Campaign in Campaigns)
            {
                Campaign.Delete();
                CampaignRepository.Save(Campaign);
            }
        }

        private int GetRankForTag(double Usage, double TotalCount)
        {
            if (TotalCount == 0)
                return 1;

            var result = (Usage / TotalCount) * 100;
            if (result <= 5)
                return 1;
            if (result <= 15)
                return 2;
            if (result <= 30)
                return 3;
            if (result <= 50)
                return 4;
            if (result <= 70)
                return 5;
            if (result <= 90)
                return 6;
            return 7;
        }

        //public IEnumerable<AdvertiserDto> GetTop(int? count)
        //{
        //    //first get top 1000 order by usage then get top count order by random Guid
        //    //toDO:OSaleh to change the maxTop from configuration not static
        //    var maxTop = 30;
        //    //TODO: Malik to add cache to Configuration Setting Service
        //    //configurationManager.GetConfigurationSetting(null, null, "MaxTagCount");
        //    if (!count.HasValue)
        //    {
        //        count = maxTop;
        //    }
        //    if (count > maxTop)
        //        count = maxTop;
        //    IEnumerable<Advertiser> list = AdvertiserRepository.GetTop(maxTop).OrderBy(x => new Guid()).Take(count.Value).ToList();
        //    var dtosList = list.Select(AdvertiserDto => MapperHelper.Map<AdvertiserDto>(AdvertiserDto)).ToList();
        //    int totalCount = dtosList.Sum(item => item.Usage);
        //    foreach (var AdvertiserDto in dtosList)
        //    {
        //        AdvertiserDto.Rank = GetRankForTag(AdvertiserDto.Usage, totalCount);
        //    }
        //    return dtosList;
        //}

        public IEnumerable<AdvertiserDto> GetByQuery(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserCriteria wcriteria)
        {

            AdvertiserCriteria criteria = new AdvertiserCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            IEnumerable<Advertiser> list = null;

            if (criteria.Page.HasValue)
            {
                list = AdvertiserRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = AdvertiserRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }

            // var list = AdvertiserRepository.Query(criteria.GetExpression());
            return list.Select(AdvertiserDto => MapperHelper.Map<AdvertiserDto>(AdvertiserDto)).ToList();
        }

        public AdvertiserResult GetByQueryPagination(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserCriteria wcriteria)
        {
            AdvertiserCriteria criteria = new AdvertiserCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            AdvertiserResult result = new AdvertiserResult();

            IEnumerable<Advertiser> list = null;

            if (criteria.Page.HasValue)
            {
                list = AdvertiserRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = AdvertiserRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }

            // var list = AdvertiserRepository.Query(criteria.GetExpression());
            result.Items = list.Select(AdvertiserDto => MapperHelper.Map<AdvertiserDto>(AdvertiserDto)).ToList();
            result.TotalCount = AdvertiserRepository.Query(criteria.GetExpression()).Count();
            return result;


        }


        public bool IsReadOrWriteAdvertiserAccount(int AdvertiserAccountId)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return true;

            var AdvertiserAccount = _advertiserAccountRepository.Get(AdvertiserAccountId);
            int count = 0;
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {

                _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted ).Count();
                return count > 0;
            }
                if (!AdvertiserAccount.IsRestricted)
                return true;

             count = _advertiserAccountUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted && (x.Read || x.Write)).Count();

            return count > 0;
        }
        public bool IsWriteAdvertiserAccount(int AdvertiserAccountId)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return true;

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {
                return false;

            }
            var AdvertiserAccount = _advertiserAccountRepository.Get(AdvertiserAccountId);
            if (!AdvertiserAccount.IsRestricted)
                return true;

            int count = _advertiserAccountUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted && x.Write).Count();

            return count > 0;
        }

        #region Master AppSite 

        public AdvertiserAccountMasterAppSiteDto GetAdvertiserAccountMasterAppSiteById(int Id)
        {
            var advItem = _advertiserAccountMasterAppSiteRepository.Get(Id);

            AdvertiserAccountMasterAppSiteDto AdvertiserAccount = MapperHelper.Map<AdvertiserAccountMasterAppSiteDto>(advItem);
            var itemsList = _advertiserAccountMasterAppSiteItemRepository.Query(M => M.Link.ID == advItem.ID && M.IsDeleted == false).ToList();

            foreach (var itL in itemsList)
            {
                AdvertiserAccount.ContentListItems = AdvertiserAccount.ContentListItems + itL.AppSiteName + "\n";

            }

            return AdvertiserAccount;
        }
        public AdvertiserAccountMasterAppSiteResultDto GetAdvertiserAccountMasterAppSite(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountMasterAppSiteCriteria wcriteria)
        {
            AdvertiserAccountMasterAppSiteCriteria criteria = new AdvertiserAccountMasterAppSiteCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AdvertiserAccountMasterAppSiteResultDto();

            IEnumerable<AdvertiserAccountMasterAppSite> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _advertiserAccountMasterAppSiteRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _advertiserAccountMasterAppSiteRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<AdvertiserAccountMasterAppSiteDto>(AdvertiserAccount)).ToList();


            result.Items = returnList;
            result.TotalCount = _advertiserAccountMasterAppSiteRepository.Query(criteria.GetExpression()).Count();
            return result;

        }

        public void SaveAdvertiserAccountMasterAppSite(AdvertiserAccountMasterAppSiteDto item)
        {
            AdvertiserAccountMasterAppSite AdvertiserAccount = MapperHelper.Map<AdvertiserAccountMasterAppSite>(item);
            if(AdvertiserAccount.Link!=null)
            ValidateAdvertiser(AdvertiserAccount.Link.ID);
            if (!item.GlobalScope && item.LinkId <= 0 && _advertiserAccountMasterAppSiteRepository.Query(x => x.Name.ToLower().Equals(item.Name.Trim().ToLower()) && x.ID != item.Id && !x.IsDeleted && x.Account.ID == item.AccountId).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("AdvertiserAccountMasterAppSiteDuplicated") });

            }
            else if (!item.GlobalScope && item.LinkId > 0 && _advertiserAccountMasterAppSiteRepository.Query(x => x.Name.ToLower().Equals(item.Name.Trim().ToLower()) && x.ID != item.Id && !x.IsDeleted && x.Account.ID == item.AccountId && x.Link.ID == item.LinkId).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("AdvertiserAccountMasterAppSiteDuplicated") });

            }
            else if (item.GlobalScope && _advertiserAccountMasterAppSiteRepository.Query(x => x.Name.ToLower().Equals(item.Name.Trim().ToLower()) && x.ID != item.Id && !x.IsDeleted && x.GlobalScope == true).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("AdvertiserAccountMasterAppSiteDuplicated") });
            }
            else
            {
                var obj = _advertiserAccountMasterAppSiteRepository.Query(x => x.ID == item.Id).FirstOrDefault();
                string PatternURL = @"^(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|af|ax|al|dz|as|ad|ao|ai|aq|ag|ar|am|aw|ac|au|at|az|bs|bh|bd|bb|eus|by|be|bz|bj|bm|bt|bo|bq|an|nl|ba|bw|bv|br|io|vg|bn|bg|bf|mm|bi|kh|cm|ca|cv|cat|ky|cf|td|cl|cn|cx|cc|co|km|cd|cg|ck|cr|ci|hr|cu|cw|cy|cz|dk|dj|dm|do|tl|ec|eg|sv|gq|er|ee|et|eu|fk|fo|fm|fj|fi|fr|gf|pf|tf|ga|gal|gm|ge|de|gi|gr|gl|gd|gp|gu|gt|gg|gn|gw|gy|ht|hm|hn|hk|hu|is|in|id|ir|iq|ie|im|il|it|jm|jp|je|jo|kz|ke|ki|kw|kg|la|lv|lb|ls|lr|ly|li|lt|lu|mo|mk|mg|mw|my|mv|ml|mt|mh|mq|mr|mu|yt|mx|md|mc|mn|me|ms|mz|mm|na|nr|np|nl|nc|nz|ni|ne|ng|nu|nf|nc|kp|mk|mp|no|om|pk|pw|ps|pa|pg|py|pe|ph|pn|pl|pt|pr|qa|ro|ru|rw|re|bq|an|bl|gp|fr|sh|kn|lc|mf|gp|fr|pm|vc|ws|sm|st|sa|sn|rs|sc|sl|sg|bq|an|nl|sx|an|sk|si|sb|so|za|gs|kr|ss|es|lk|sd|sr|sj|sz|se|ch|sy|tw|tj|tz|th|tg|tk|to|tt|tn|tr|tm|tc|tv|ug|ua|ae|uk|us|vi|uy|uz|vu|va|ve|vn|wf|eh|ye|zm|zw)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";
                Regex RgxURL = new Regex(PatternURL, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                string pattern = @"^([A-Za-z]{1}[A-Za-z\d_]*\.)*[A-Za-z][A-Za-z\d_]*$";

                Regex RgxAppDomain = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (obj == null)
                {
              
                    string ItemF = string.Empty;
                  
                    //AdvertiserAccount.IsRestricted = false;
                    AdvertiserAccount.Account = new Domain.Model.Account.Account { ID = item.AccountId };
                    AdvertiserAccount.User = new Domain.Model.Account.User { ID = item.UserId };
                    if (item.LinkId > 0)
                        AdvertiserAccount.Link = new AdvertiserAccount { ID = item.LinkId };
                    else
                        AdvertiserAccount.GlobalScope = item.GlobalScope;
                    AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                    AdvertiserAccount.Name = item.Name.Trim();
                    long x;
                    if (!string.IsNullOrWhiteSpace(item.ContentListItems))
                    {
                        AdvertiserAccount.Items = new List<AdvertiserAccountMasterAppSiteItem>();

                        var listOfNames = item.ContentListItems.Split(new char[] { '\n' });

                        foreach (var Name in listOfNames)
                        {
                            ItemF = Name.Trim();
                            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 255)
                                continue;

                            

                            var iteminList = AdvertiserAccount.Items.Where(M => M.AppSiteName.ToLower().Equals(ItemF.ToLower()) && M.IsDeleted == false).SingleOrDefault();
                            if (iteminList != null)
                                continue;
                            x = 0;
                                
                           // bool isUri = Uri.IsWellFormedUriString(Name.Trim(), UriKind.RelativeOrAbsolute);
                            if (RgxURL.IsMatch(ItemF) && ItemF.ToLower().IndexOf("com.")!=0)
                            {
                                if(ItemF.IndexOf("www.")==0)
                                ItemF = ItemF.Replace("www.", "");
                                AdvertiserAccount.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = AdvertiserAccount, AppSiteName = ItemF });
                            }
                            else if (RgxAppDomain.IsMatch(ItemF))
                            {

                               /* if ( IsValidURL(ItemF))
                                {
                                    AdvertiserAccount.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = AdvertiserAccount, AppSiteName = ItemF });
                                }
                               else*/
                                    AdvertiserAccount.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = AdvertiserAccount, AppSiteName = ItemF });

                            }
                           else 
                            {
                                long.TryParse(ItemF, out x);
                                if (  x > 0)
                                AdvertiserAccount.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = AdvertiserAccount, AppSiteName = ItemF });
                            }



                        }
                    }

                    AdvertiserAccount.LastModifiedDate = Framework.Utilities.Environment.GetServerTime();
                    _advertiserAccountMasterAppSiteRepository.Save(AdvertiserAccount);
                }
                else
                {
                    string ItemF = string.Empty;
                    obj.Name = item.Name.Trim();
                    obj.Type = item.Type;
                    // AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                    obj.IsDeleted = false;
                    long x;
                    if (string.IsNullOrWhiteSpace(item.ContentListItems))
                    {
                        if (obj.Items != null)
                        {
                            foreach (var addItems in obj.Items)
                            {

                                addItems.IsDeleted = true;

                            }
                        }
                        //obj.Items.Clear();





                    }
                    else
                    {
                       
                        var listOfNames = item.ContentListItems.Split(new char[] { '\n' });
                        var addedItems = new List<AdvertiserAccountMasterAppSiteItem>();
                        if (obj.Items == null)
                            obj.Items = new List<AdvertiserAccountMasterAppSiteItem>();
                        foreach (var Name in listOfNames)
                        {
                            ItemF = Name.Trim();
                            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 255)
                                continue;

                        
                         
                            x = 0;
                        
                            //bool isUri = Uri.IsWellFormedUriString(Name.Trim(), UriKind.RelativeOrAbsolute);
                          if (RgxURL.IsMatch(ItemF) && ItemF.ToLower().IndexOf("com.") != 0)
                            {
                                if (ItemF.IndexOf("www.") == 0)
                                    ItemF = ItemF.Replace("www.","");
                                var iteminList = obj.Items.Where(M => M.AppSiteName.ToLower().Equals(ItemF.ToLower()) && M.IsDeleted == false).SingleOrDefault();
                                if (iteminList == null)
                                {
                                    obj.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = obj, AppSiteName = ItemF });

                                    addedItems.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = obj, AppSiteName = ItemF });
                                }
                                else
                                {
                                    addedItems.Add(iteminList);

                                }
                            }
                            else if (RgxAppDomain.IsMatch(ItemF))
                            {
                                var iteminList = obj.Items.Where(M => M.AppSiteName.ToLower().Equals(ItemF.ToLower()) && M.IsDeleted == false).SingleOrDefault();
                                if (iteminList == null)
                                {
                                    /*
                                    if ( IsValidURL(ItemF))
                                    {
                                        obj.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = obj, AppSiteName = ItemF });

                                        addedItems.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.Site, Domain = ItemF, Link = obj, AppSiteName = ItemF });
                                    }
                                    else
                                    {*/
                                        obj.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = obj, AppSiteName = ItemF });

                                        addedItems.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = obj, AppSiteName = ItemF });
                                    /*}*/
                                }
                                else
                                {
                                    addedItems.Add(iteminList);

                                }

                            }
                            
                            else 
                            {
                                long.TryParse(ItemF, out x);
                                if (x>0)
                                {
                                    var iteminList = obj.Items.Where(M => M.AppSiteName.ToLower().Equals(ItemF.ToLower()) && M.IsDeleted == false).SingleOrDefault();
                                    if (iteminList == null)
                                    {
                                        obj.Items.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = obj, AppSiteName = ItemF });

                                        addedItems.Add(new AdvertiserAccountMasterAppSiteItem { Type = MasterAppSiteItemType.App, BundleID = ItemF, Link = obj, AppSiteName = ItemF });
                                    }
                                    else
                                    {
                                        addedItems.Add(iteminList);

                                    }
                                }
                            }
                        }

                        foreach (var addItems in obj.Items.Where(M => M.IsDeleted == false))
                        {
                            var iteminList = addedItems.Where(M => M.AppSiteName.ToLower().Equals(addItems.AppSiteName.ToLower())).FirstOrDefault();
                            if (iteminList == null)
                            {
                                addItems.IsDeleted = true;
                            }
                        }





                    }
                    obj.LastModifiedDate = Framework.Utilities.Environment.GetServerTime();
                    _advertiserAccountMasterAppSiteRepository.Save(obj);

                }
            }
        }


        public bool IsValidURL(string URL)
        {
            string Pattern = @"^(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|af|ax|al|dz|as|ad|ao|ai|aq|ag|ar|am|aw|ac|au|at|az|bs|bh|bd|bb|eus|by|be|bz|bj|bm|bt|bo|bq|an|nl|ba|bw|bv|br|io|vg|bn|bg|bf|mm|bi|kh|cm|ca|cv|cat|ky|cf|td|cl|cn|cx|cc|co|km|cd|cg|ck|cr|ci|hr|cu|cw|cy|cz|dk|dj|dm|do|tl|ec|eg|sv|gq|er|ee|et|eu|fk|fo|fm|fj|fi|fr|gf|pf|tf|ga|gal|gm|ge|de|gi|gr|gl|gd|gp|gu|gt|gg|gn|gw|gy|ht|hm|hn|hk|hu|is|in|id|ir|iq|ie|im|il|it|jm|jp|je|jo|kz|ke|ki|kw|kg|la|lv|lb|ls|lr|ly|li|lt|lu|mo|mk|mg|mw|my|mv|ml|mt|mh|mq|mr|mu|yt|mx|md|mc|mn|me|ms|mz|mm|na|nr|np|nl|nc|nz|ni|ne|ng|nu|nf|nc|kp|mk|mp|no|om|pk|pw|ps|pa|pg|py|pe|ph|pn|pl|pt|pr|qa|ro|ru|rw|re|bq|an|bl|gp|fr|sh|kn|lc|mf|gp|fr|pm|vc|ws|sm|st|sa|sn|rs|sc|sl|sg|bq|an|nl|sx|an|sk|si|sb|so|za|gs|kr|ss|es|lk|sd|sr|sj|sz|se|ch|sy|tw|tj|tz|th|tg|tk|to|tt|tn|tr|tm|tc|tv|ug|ua|ae|uk|us|vi|uy|uz|vu|va|ve|vn|wf|eh|ye|zm|zw)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(URL);
        }
        public bool DeleteAdvertiserAccountMasterAppSite(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _advertiserAccountMasterAppSiteRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        item.Delete();
                        _advertiserAccountMasterAppSiteRepository.Save(item);
                        var listTargeting = _MasterAppSiteTargetingReposiory.Query(M => M.List.ID == item.ID).ToList();
                        if (listTargeting != null)
                        {
                            for (int itemIndex = 0; itemIndex < listTargeting.Count; itemIndex++)
                            {
                                _MasterAppSiteTargetingReposiory.Remove(listTargeting[itemIndex]);
                            }
                        }

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _advertiserAccountMasterAppSiteRepository.Get(id)))
                {


                    if (!(item.Status == MasterAppSiteStatus.Active))
                    {
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        item.Activate();
                        _advertiserAccountMasterAppSiteRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _advertiserAccountMasterAppSiteRepository.Get(id)))
                {


                    if (!(item.Status == MasterAppSiteStatus.InActive))
                    {
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        item.DeActivate();
                        _advertiserAccountMasterAppSiteRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        #endregion

        #region Master AppSite Item
        public AdvertiserAccountMasterAppSiteItemResultDto GetAdvertiserAccountMasterAppSiteItem(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountMasterAppSiteItemCriteria wcriteria)
        {
            AdvertiserAccountMasterAppSiteItemCriteria criteria = new AdvertiserAccountMasterAppSiteItemCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AdvertiserAccountMasterAppSiteItemResultDto();

            IEnumerable<AdvertiserAccountMasterAppSiteItem> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _advertiserAccountMasterAppSiteItemRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _advertiserAccountMasterAppSiteItemRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<AdvertiserAccountMasterAppSiteItemDto>(AdvertiserAccount)).ToList();


            result.Items = returnList;
            result.TotalCount = _advertiserAccountMasterAppSiteItemRepository.Query(criteria.GetExpression()).Count();
            return result;

        }

        public void SaveAdvertiserAccountMasterAppSiteItem(AdvertiserAccountMasterAppSiteItemDto item)
        {
            AdvertiserAccountMasterAppSiteItem AdvertiserAccount = MapperHelper.Map<AdvertiserAccountMasterAppSiteItem>(item);

            if (_advertiserAccountMasterAppSiteItemRepository.Query(x => x.AppSiteName.ToLower().Equals(item.AppSiteName.Trim().ToLower()) && x.ID != item.Id && x.Link.ID == item.LinkId && !x.IsDeleted).Count() > 0)
            {
                // throw new BusinessException(new List<ErrorData>() { new ErrorData("AdvertiserAccountMasterAppSiteItemDuplicated") });
                return;

            }

            else
            {
                var obj = _advertiserAccountMasterAppSiteItemRepository.Query(x => x.AppSiteName.ToLower().Equals(item.AppSiteName.Trim().ToLower()) && x.ID != item.Id && x.Link.ID == item.LinkId).FirstOrDefault();
                if (obj == null)
                {
                    //AdvertiserAccount.IsRestricted = false;
                    AdvertiserAccount.Account = new Domain.Model.Account.Account { ID = item.AccountId };
                    AdvertiserAccount.User = new Domain.Model.Account.User { ID = item.UserId };
                    if (item.LinkId > 0)
                        AdvertiserAccount.Link = new AdvertiserAccountMasterAppSite { ID = item.LinkId };

                    AdvertiserAccount.AppSiteName = item.AppSiteName.Trim();
                    AdvertiserAccount.BundleID = item.BundleID;
                    AdvertiserAccount.Domain = item.Domain;
                    AdvertiserAccount.Type = item.Type;
                    _advertiserAccountMasterAppSiteItemRepository.Save(AdvertiserAccount);
                }
                else
                {
                    obj.AppSiteName = item.AppSiteName.Trim();
                    obj.BundleID = item.BundleID;
                    obj.Domain = item.Domain;
                    // AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                    obj.IsDeleted = false;
                    _advertiserAccountMasterAppSiteItemRepository.Save(obj);

                }
            }
        }

        public bool DeleteAdvertiserAccountMasterAppSiteItem(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _advertiserAccountMasterAppSiteItemRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        item.Delete();
                        _advertiserAccountMasterAppSiteItemRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region AudienceList
        public AudienceSegmentResultResultDto GetAudienceSegmentsPerAdvertiser(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AudienceSegmentCriteria wcriteria)
        {
            AudienceSegmentCriteria criteria = new AudienceSegmentCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AudienceSegmentResultResultDto();

            IEnumerable<AudienceSegment> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _AudienceSegmentRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _AudienceSegmentRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<AudienceSegmentDto>(AdvertiserAccount)).ToList();
            if (returnList != null)
            {
                foreach (var item in returnList)
                {

                    item.en = item.Name.GetValue();


                }


            }
            #region Performance
            var performance = new PerformanceCriteria
            {
                FromDate = criteria.DataFrom,
                ToDate = criteria.DataTo,
                Ids = returnList.Select(obj => obj.ID).ToList()
            };

             var performances = _summaryRepository.GetAudienceListsPerformance(performance);

          //  var performances = new List<AudienceListPerformanceDto>();
          //  var idStatus = _summaryRepository.GetAdsByAdvertiser(performance);
            //   var idStatus = _summaryRepository.GetAdsByCampaign(performance);

            foreach (var advertiserListDto in returnList)
            {
                advertiserListDto.Performance = performances.FirstOrDefault(item => item.Id == advertiserListDto.ID);

                if (advertiserListDto.Performance == null)
                {
                    advertiserListDto.Performance = new Interfaces.DTOs.Reports.AudienceListPerformanceDto();

                }

               
            }


            #endregion


            result.Items = returnList;
            result.TotalCount = _AudienceSegmentRepository.Query(criteria.GetExpression()).Count();
            return result;

        }


    
        public int GetRootIdofFirstParty()
           {

            var DpPartner = _DPPartnerRepository.Query(M => M.Code == codeForFirstParty).SingleOrDefault();

           var root= _AudienceSegmentRepository.Query(M => M.Provider.ID == DpPartner.ID && M.Parent == null).SingleOrDefault();
            return root.ID;

        }
        public void SaveAudienceSegmentPerAdvertiser(AudienceSegmentDto item)
        {
            item.Name = new LocalizedStringDto
            {
                GroupKey = "AudienceSegment",
                Value = item.en,
                Values = new List<LocalizedValueDto>()

            };
            lookalikejob looklikeobj = new lookalikejob();
            item.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = item.en });
            item.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = item.en });


            AudienceSegment audienceSegment = MapperHelper.Map<AudienceSegment>(item);
            if (audienceSegment.Advertiser != null)
                ValidateAdvertiser(audienceSegment.Advertiser.ID);
            //Code should be configured
            var DpPartner= _DPPartnerRepository.Query(M => M.Code == codeForFirstParty).SingleOrDefault();
            if (_AudienceSegmentRepository.Query(x => x.Name.Values.Any(v => v.Value.ToLower().Equals(item.Name.Value.Trim().ToLower())) && x.ID != item.ID && x.Advertiser.ID == item.AdvertiserId && !x.IsDeleted).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("Duplicated") });
                //return;

            }

            else
            { var obj = audienceSegment;

                var audianceseg = this._AudienceSegmentRepository.Query(M => M.Provider.ID == DpPartner.ID && M.Parent == null).SingleOrDefault();
                if (item.ID > 0)
                { obj = _AudienceSegmentRepository.Get(item.ID); }
                if (!(item.ID > 0))
                {
                    obj.Selectable = true;
                    obj.Parent = audianceseg;
                    obj.Code = GeSegmentsCounter();
                    obj.Provider = DpPartner;
                    obj.OperatorSegmentCode = DpPartner.Code + ":" + obj.Code;
                    obj.Activated = true;
                    obj.BinIndex = obj.CalculateBinIndex(obj.Advertiser.ID);
                    obj.Account = new Domain.Model.Account.Account { ID = Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value };
                    obj.User = new Domain.Model.Account.User { ID = Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().UserId.Value };


                    foreach (var localizedValue in obj.Name.Values)
                    {
                        if (!string.IsNullOrEmpty(obj.Name.Value))
                        {
                            obj.Name.Value = obj.Name.Value.Trim();
                        }
                        localizedValue.LocalizedString = obj.Name;
                    }
                }
                if (item.ID > 0)
                {
                    obj.Name.Values[0].Value = item.Name.Values[0].Value;
                    obj.Name.Values[1].Value = item.Name.Values[1].Value;

                    obj.Description = item.Description;
                }

                // AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                obj.IsDeleted = false;
               

              
              
               

                _AudienceSegmentRepository.Save(obj);
               
            }
        }
        public void SaveAudienceSegmentPerAdvertiserForAdmin(AudienceSegmentDto item)
        {
            item.Name = new LocalizedStringDto
            {
                GroupKey = "AudienceSegment",
                Value = item.en,
                Values = new List<LocalizedValueDto>()

            };
            lookalikejob looklikeobj = new lookalikejob();
            item.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = item.en });
            item.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = item.en });


            AudienceSegment audienceSegment = MapperHelper.Map<AudienceSegment>(item);
            if (audienceSegment.Advertiser != null)
                ValidateAdvertiser(audienceSegment.Advertiser.ID);
            //Code should be configured
            var DpPartner = _DPPartnerRepository.Query(M => M.Code == codeForFirstParty).SingleOrDefault();
            if (_AudienceSegmentRepository.Query(x => x.Name.Values.Any(v => v.Value.ToLower().Equals(item.Name.Value.Trim().ToLower())) && x.ID != item.ID && x.Advertiser.ID == item.AdvertiserId && !x.IsDeleted).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("Duplicated") });
                //return;

            }

            else
            {
                var obj = audienceSegment;

                var audianceseg = this._AudienceSegmentRepository.Query(M => M.Provider.ID == DpPartner.ID && M.Parent == null).SingleOrDefault();
                if (item.ID > 0)
                { obj = _AudienceSegmentRepository.Get(item.ID); }
                if (!(item.ID > 0))
                {
                    obj.Selectable = true;
                    obj.Parent = audianceseg;
                    obj.Code = GeSegmentsCounter();
                    obj.Provider = DpPartner;
                    obj.OperatorSegmentCode = DpPartner.Code + ":" + obj.Code;
                    obj.Activated = true;
                    obj.BinIndex = obj.CalculateBinIndex(obj.Advertiser.ID);
                    obj.Account = new Domain.Model.Account.Account { ID = Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value };
                    obj.User = new Domain.Model.Account.User { ID = Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().UserId.Value };


                    foreach (var localizedValue in obj.Name.Values)
                    {
                        if (!string.IsNullOrEmpty(obj.Name.Value))
                        {
                            obj.Name.Value = obj.Name.Value.Trim();
                        }
                        localizedValue.LocalizedString = obj.Name;
                    }
                }
                if (item.ID > 0)
                {
                    obj.Name.Values[0].Value = item.Name.Values[0].Value;
                    obj.Name.Values[1].Value = item.Name.Values[1].Value;

                    obj.Description = item.Description;
                }

                // AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                obj.IsDeleted = false;


                if (obj.Advertiser != null)
                {
                    looklikeobj = _IlookalikejobRepository.Query(M => M.LookalikeAudienceListCode == obj.Code).SingleOrDefault();
                    if (looklikeobj == null)
                    {
                        looklikeobj = new lookalikejob();
                        obj.Activated = false;

                    }
                    looklikeobj.SeedAudienceListCode = item.SeedAudienceListCode;
                    looklikeobj.LookalikePercentage =(float) Math.Round(item.LookalikePercentage / 100, 2); ;

                    looklikeobj.LookalikeAudienceListCode = obj.Code;

                    looklikeobj.PopulationCountryFilter = item.PopulationCountryFilter;


                }



                _AudienceSegmentRepository.Save(obj);
                if (Domain.Configuration.IsAdminOrAdOps && !string.IsNullOrWhiteSpace(looklikeobj.PopulationCountryFilter))
                {
                    _IlookalikejobRepository.Save(looklikeobj);
                }
            }
        }
        private int GeSegmentsCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "My Audience Segments");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        private int GePixelCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "My Pixel");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }

        public AudienceSegmentDto GetAudienceSegmentDto(int Id)
        {
            var item = _AudienceSegmentRepository.Get(Id);
            AudienceSegmentDto audienceSegment = MapperHelper.Map<AudienceSegmentDto>(item);

            lookalikejob looklikeobj = _IlookalikejobRepository.Query(M => M.LookalikeAudienceListCode == item.Code).SingleOrDefault();
            if (looklikeobj != null)
            {


                audienceSegment.LookalikePercentage =(float) Math.Round(looklikeobj.LookalikePercentage*100,2);

                audienceSegment.SeedAudienceListCode = looklikeobj.SeedAudienceListCode;

                audienceSegment.PopulationCountryFilter = looklikeobj.PopulationCountryFilter;

            }
            return audienceSegment;
        }
        public bool DeleteAudienceSegmentPerAdvertiser
            (IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _AudienceSegmentRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        if (item.Advertiser != null)
                            ValidateAdvertiser(item.Advertiser.ID);
                        item.IsDeleted = true ;
                        _AudienceSegmentRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DeleteAudienceSegmentPerAdvertiserForAdmin
        (IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _AudienceSegmentRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        if (item.Advertiser != null)
                            ValidateAdvertiser(item.Advertiser.ID);
                        item.IsDeleted = true;
                        _AudienceSegmentRepository.Save(item);
                     var itemlook=   _IlookalikejobRepository.Query(M=>M.LookalikeAudienceListCode == item.Code).SingleOrDefault();
                        if(itemlook!=null)
                        _IlookalikejobRepository.Remove(itemlook);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UnDeleteAudienceSegmentPerAdvertiser(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _AudienceSegmentRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        if (item.Advertiser != null)
                            ValidateAdvertiser(item.Advertiser.ID);
                        item.IsDeleted = false;
                        _AudienceSegmentRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion





        #region Pixel

        public PixelDto GetPixelById(int Id)
        {
            var advItem = _pixelRepository.Get(Id);

            PixelDto pixel = MapperHelper.Map<PixelDto>(advItem);
          

            return pixel;
        }
        public PixelResultDto GetPixel(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.PixelCriteria wcriteria)
        {
            PixelCriteria criteria = new PixelCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new PixelResultDto();

            IEnumerable<Pixel> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _pixelRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _pixelRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<PixelDto>(AdvertiserAccount)).ToList();


            result.Items = returnList;
            result.TotalCount = _pixelRepository.Query(criteria.GetExpression()).Count();
            return result;

        }

        public void SavePixel(PixelDto item)
        {
            Pixel Pixel = MapperHelper.Map<Pixel>(item);
            IList<int> intList = new List<int>();


            // One Is Billable tracking event is allowed for each adgroup
            if (Pixel.Link != null)
                ValidateAdvertiser(Pixel.Link.ID);

            int mos = 0;

            if (item.SegmentsId != null)
            {
                intList = item.SegmentsId.Split(',')
                    .Where(m => int.TryParse(m, out mos))
                    .Select(m => int.Parse(m))
                    .ToList();
            }


            if (item.LinkId > 0 && _pixelRepository.Query(x => x.Name.ToLower().Equals(item.Name.Trim().ToLower()) && x.ID != item.Id && !x.IsDeleted && x.Account.ID == item.AccountId && x.Link.ID == item.LinkId).Count() > 0)
            {
                throw new BusinessException(new List<ErrorData>() { new ErrorData("Duplicated") });

            }
           
            else
            {
                var obj = _pixelRepository.Query(x => x.ID == item.Id).FirstOrDefault();
                if (obj == null)
                {
                    //AdvertiserAccount.IsRestricted = false;
                    Pixel.Account = new Domain.Model.Account.Account { ID = item.AccountId };
                    Pixel.User = new Domain.Model.Account.User { ID = item.UserId };
                    if (item.LinkId > 0)
                        Pixel.Link = new AdvertiserAccount { ID = item.LinkId };
                    //else
                    //    Pixel.GlobalScope = item.GlobalScope;
                    Pixel.Status = PixelStatus.Active;
                    Pixel.Name = item.Name.Trim();

                    Pixel.Code = GePixelCounter();
                    Pixel.LastModifiedDate = Framework.Utilities.Environment.GetServerTime();


                   
                    if (intList != null && intList.Count > 0)
                    {
                        Pixel.AudienceSegmentListsMap = new List<AudienceSegmentPixelMap>();

                        foreach (var item1 in intList)
                        {

                            Pixel.AudienceSegmentListsMap.Add(new AudienceSegmentPixelMap { AudienceSegment = new AudienceSegment { ID = item1 }, Pixel  = Pixel });
                        }

                    }

                    //now if we are adding any already exits event and it's associated with a the current adgroup  , then we only need to update it not add it again !


                        _pixelRepository.Save(Pixel);
                }
                else
                {
                    obj.Name = item.Name.Trim();
                    //obj.Type = item.Type;
                    // AdvertiserAccount.Status = MasterAppSiteStatus.Active;
                    obj.IsDeleted = false;
                  
                    obj.LastModifiedDate = Framework.Utilities.Environment.GetServerTime();




                    if (obj.AudienceSegmentListsMap == null)
                    {

                        obj.AudienceSegmentListsMap = new List<AudienceSegmentPixelMap>();
                    }

                    if (obj.AudienceSegmentListsMap.Count!=0)
                    {
                    foreach (var item1 in intList)
                            {

                        if (obj.AudienceSegmentListsMap.Where(M => M.AudienceSegment.ID == item1).SingleOrDefault() == null)
                        {
                                obj.AudienceSegmentListsMap.Add(new AudienceSegmentPixelMap { AudienceSegment = new AudienceSegment { ID = item1 }, Pixel = obj });
                        }

                    }

                    for (var i = 0; i < obj.AudienceSegmentListsMap.Count; i++)
                    {

                        if (!intList.Contains(obj.AudienceSegmentListsMap[i].AudienceSegment.ID))
                        {
                                obj.AudienceSegmentListsMap.Remove(obj.AudienceSegmentListsMap[i]);
                            if (i != 0)
                            {
                                i--;
                            }

                        }
                    }
                }
                        else
                        {
                        if (obj.AudienceSegmentListsMap == null)
                        {

                            obj.AudienceSegmentListsMap = new List<AudienceSegmentPixelMap>();
                        }


                        foreach (var item1 in intList)
                    {

                            obj.AudienceSegmentListsMap.Add(new AudienceSegmentPixelMap { AudienceSegment = new AudienceSegment { ID = item1 }, Pixel = obj });
                    }

                }
                _pixelRepository.Save(obj);

                }
            }
        }



        public bool DeletePixel(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _pixelRepository.Get(id)))
                {


                    if (!item.IsDeleted)
                    {
                        item.Delete();
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        _pixelRepository.Save(item);
                        //var listTargeting = _pixelRepository.Query(M => M.List.ID == item.ID).ToList();
                        //if (listTargeting != null)
                        //{
                        //    for (int itemIndex = 0; itemIndex < listTargeting.Count; itemIndex++)
                        //    {
                        //        _pixelRepository.Remove(listTargeting[itemIndex]);
                        //    }
                        //}

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActivatePixel(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _pixelRepository.Get(id)))
                {


                    if (!(item.Status == PixelStatus.Active))
                    {
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        item.Activate();
                        _pixelRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeActivatePixel(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _pixelRepository.Get(id)))
                {


                    if (!(item.Status == PixelStatus.InActive))
                    {
                        if (item.Link != null)
                            ValidateAdvertiser(item.Link.ID);
                        item.DeActivate();
                        _pixelRepository.Save(item);

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public PixelResultDto GetPixelsPerAdvertiser(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.PixelCriteria wcriteria)
        {
            PixelCriteria criteria = new PixelCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            var result = new PixelResultDto();

            IEnumerable<Pixel> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = _pixelRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _pixelRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(AdvertiserAccount => MapperHelper.Map<PixelDto>(AdvertiserAccount)).ToList();
        


            result.Items = returnList;
            result.TotalCount = _pixelRepository.Query(criteria.GetExpression()).Count();
            return result;

        }

        #endregion
    }
}
