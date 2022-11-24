using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using NHibernate;
using Noqoush.AdFalcon.Business.Domain.Exceptions;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Services;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Security;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.Framework.EventBroker.Context;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.SSP;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Resources;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.PMP;
using Noqoush.AdFalcon.Domain.Repositories.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.Framework.DomainServices;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Utilities;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;

namespace Noqoush.AdFalcon.Services.Services.Account.PMP
{

    public class PMPDealService : IPMPDealService
    {
        private readonly IAdSupportedCreativeUnitRepository _adSupportedCreativeUnitRepository = null;
        private IDealCampaignMappingRepository _dealCampaignMappingRepository;
        private INativeAdCreativeBaseRepository _NativeAdCreativeBaseRepository;
        private IAccountRepository _accountRepository;
        private ICampaignRepository _campaignRepository = null;
        private IUserRepository _userRepository;
        private IUserDomainManager _userDomainService;
        private ISecurityService _securityService;
        private IPaymentRepository _paymentRepository;
        private IPaymentTypeRepository _paymentTypeRepository;
        private IConfigurationManager _configurationManager;
        private IDocumentRepository _documentRepository;
        private IAccountDiscountRepository _accountDiscountRepository;
        private readonly IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        private IAppSiteRepository _appSiteRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IJobPositionRepository _jobPositionRepository;
        private readonly IPartnerSiteRepository _partnerSiteRepository;
        private readonly ISiteZoneRepository _siteZoneRepository;
        private readonly ISiteZoneMappingRepository _siteZoneMappingRepository;
        private readonly IFloorPriceRepository _floorPriceRepository;
        private readonly IAccountPartyDefineRepository _accountPartyDefineRepository;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IBusinessPartnerTypeRepository _businessPartnerTypeRepository;
        private readonly ICreativeUnitRepository _CreativeUnitRepository;
        private IAdvertiserAccountRepository AdvertiserAccountRepository;
        private readonly IPMPDealRepository _PMPDealRepository;
        private readonly IPMPDealTargetingRepository _PMPDealTargetingRepository;


        //static readonly object LockObj = new object();

        public PMPDealService(

             IAdSupportedCreativeUnitRepository AdSupportedCreativeUnitRepository,
               ICreativeUnitRepository CreativeUnitRepository,
                 IPMPDealRepository PMPDealRepository,
            IPMPDealTargetingRepository PMPDealTargetingRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            ISecurityService securityService,
            IPaymentRepository paymentRepository,
            IConfigurationManager configurationManager,
            IDocumentRepository documentRepository,
            IAccountPaymentDetailsRepository accountPaymentDetailsRepository,
            IAccountDiscountRepository accountDiscountRepository, IPaymentTypeRepository paymentTypeRepository,

       IPartyRepository partyRepository,
       IJobPositionRepository jobPositionRepository,
       IPartnerSiteRepository partnerSiteRepository,
       ISiteZoneRepository siteZoneRepository,
        ISiteZoneMappingRepository siteZoneMappingRepository,
      IFloorPriceRepository floorPriceRepository,
        IAccountPartyDefineRepository accountPartyDefineRepository, IBusinessPartnerRepository businessPartnerRepository, IBusinessPartnerTypeRepository businessPartnerTypeRepository, IAppSiteRepository appSiteRepository, IDealCampaignMappingRepository dealCampaignMappingRepository, ICampaignRepository campaignRepository


            , INativeAdCreativeBaseRepository NativeAdCreativeBaseRepository,
        IAdvertiserAccountRepository AdvertiserAccountRepository
            )
        {

            this._adSupportedCreativeUnitRepository = AdSupportedCreativeUnitRepository;
            this._CreativeUnitRepository = CreativeUnitRepository;
            this._PMPDealRepository = PMPDealRepository;
            this._PMPDealTargetingRepository = PMPDealTargetingRepository;

            this._appSiteRepository = appSiteRepository;
            this._accountRepository = accountRepository;
            this._userRepository = userRepository;
            this._userDomainService = new UserDomainManager(userRepository, accountRepository);
            this._securityService = securityService;
            this._paymentRepository = paymentRepository;
            this._configurationManager = configurationManager;
            this._documentRepository = documentRepository;
            this._accountPaymentDetailsRepository = accountPaymentDetailsRepository;
            this._accountDiscountRepository = accountDiscountRepository;
            _paymentTypeRepository = paymentTypeRepository;

            this._partyRepository = partyRepository;
            this._jobPositionRepository = jobPositionRepository;
            this._partnerSiteRepository = partnerSiteRepository;
            this._siteZoneRepository = siteZoneRepository;
            this._siteZoneMappingRepository = siteZoneMappingRepository;
            this._floorPriceRepository = floorPriceRepository;
            this._accountPartyDefineRepository = accountPartyDefineRepository;
            this._businessPartnerRepository = businessPartnerRepository;
            this._businessPartnerTypeRepository = businessPartnerTypeRepository;
            this._dealCampaignMappingRepository = dealCampaignMappingRepository;

            this._campaignRepository = campaignRepository;
            this._NativeAdCreativeBaseRepository = NativeAdCreativeBaseRepository;
            this.AdvertiserAccountRepository = AdvertiserAccountRepository;
        }

        #region  Query Data
        public ResultPMPDealDto QueryByCratiriaForPMPDeal(Noqoush.AdFalcon.Domain.Common.Repositories.Account.PMP.PMPDealCriteria wcriteria)
        {
            PMPDealCriteria criteria = new PMPDealCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            if (criteria.AdvertiserAccountId.HasValue)
            {
                ValidateAdvertiser(criteria.AdvertiserAccountId.Value);
            }

            var result = new ResultPMPDealDto();
            int totalCount = 0;
            IEnumerable<Domain.Model.Account.PMP.PMPDeal> list = null;

            list = _PMPDealRepository.GetPMPDeals(criteria, out totalCount);


            var returnList = list.Select(campaign => MapperHelper.Map<PMPDealDto>(campaign)).ToList();

            result.Items = returnList;
            result.TotalCount = totalCount;
            return result;
        }

        #endregion


        private void ValidateDeal(Domain.Model.Account.PMP.PMPDeal pmpDeal, bool statusCheck = false, bool validateDates = false)
        {
            bool isManager = IsDealManager();

            pmpDeal.Validate(!isManager, statusCheck, validateDates);
        }

        private void ValidateAdvertiser(Noqoush.AdFalcon.Domain.Model.Campaign.AdvertiserAccount advertiserAccount, bool statusCheck = false, bool validateDates = false)
        {
            bool isManager = IsDealManager();

            advertiserAccount.Validate(!isManager, statusCheck);
        }
        private void ValidateAdvertiser(int advertiseraccountId, bool statusCheck = false, bool validateDates = false)
        {
            AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.ID == advertiseraccountId && x.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();
            bool isManager = IsDealManager();
            if(advertiserAccount!=null)
            advertiserAccount.Validate(!isManager, statusCheck);
        }
        private bool IsDealManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager");
        }

       // private static Id64Generator _id64Generator = new Id64Generator(int.Parse(System.Configuration.ConfigurationManager.AppSettings["HostId"]));
        public PMPDealSaveDto SavePMPDeal(PMPDealDto dto)
        {
            var item = new PMPDeal();

            dto.EndDate = dto.EndDate.HasValue ? new DateTime(dto.EndDate.Value.Year, dto.EndDate.Value.Month, dto.EndDate.Value.Day, 23, 59, 59) : (DateTime?)null;
            dto.StartDate = dto.StartDate.HasValue ? new DateTime(dto.StartDate.Value.Year, dto.StartDate.Value.Month, dto.StartDate.Value.Day, 0, 0, 0) : (DateTime?)null;

            if (dto.ID > 0)
            {
                var result =_PMPDealRepository.Query(M => M.DealID == dto.DealID && M.ID!=dto.ID).ToList();
                if (result!=null && result.Count>0)
                {
                    //create business Exception to hold error data list 
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DealIDBR" });
                    throw error;
                }
                item = _PMPDealRepository.Get(dto.ID);

                ValidateDeal(item);
                //if (dto.PublisherId > 0)
                //    item.Publisher = new Domain.Model.Account.Account { ID = dto.PublisherId };
                //else
                //    item.Publisher = null;

                item.PublisherName = dto.PublisherName;
                item.IsDeleted = false;
                if (dto.AdvertiserAccountId > 0)
                {
                    if (dto.AdvertiserId>0)
                    item.Advertiser = new Advertiser { ID = dto.AdvertiserId };
                    item.AdvertiserAccount = new AdvertiserAccount { ID = dto.AdvertiserAccountId };
                }
                else
                {
                    item.Advertiser = null;
                    item.AdvertiserAccount = null;
                }


                if (dto.ExchangeId > 0)
                    item.Exchange = new SSPPartner { ID = dto.ExchangeId };
                else
                    item.Exchange = null;
                item.Description = dto.Description;
                item.Type = dto.Type;
                item.Price = dto.Price;
                item.StartDate = dto.StartDate;
                item.EndDate = dto.EndDate;
                item.DealID = dto.DealID;
                item.Note = dto.Note;
                item.Name = dto.Name;
                if (Domain.Configuration.IsAdminOrAdOps)
                {
                    item.IsGlobal = dto.IsGlobal;
                }
            }
            else
            {
                var result = _PMPDealRepository.Query(M => M.DealID == dto.DealID  ).ToList();
                if (result != null && result.Count > 0)
                {
                    //create business Exception to hold error data list 
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DealIDBR" });
                    throw error;
                }
                item = MapperHelper.Map<PMPDeal>(dto);
                if (!Domain.Configuration.IsAdminOrAdOps)
                {
                    dto.IsGlobal = false;
                }
                
                item.UniqueId = Noqoush.AdFalcon.Domain.Configuration.Id64Generator.GenerateId();
            }


            if (dto.AccountId > 0)
                item.Account = _accountRepository.Get(dto.AccountId);

            if (item.AdvertiserAccount != null)
                ValidateAdvertiser(item.AdvertiserAccount.ID);

            _PMPDealRepository.Save(item);
            if (item.Targetings == null)
            {
                item.Targetings = new List<PMPDealTargeting>();
            }
            List<int> typesListToSend = null;
            if (!string.IsNullOrEmpty(dto.PMPTargetingSaveDto.RawAdFormats))
            {

                var typesList = dto.PMPTargetingSaveDto.RawAdFormats.Split(',');

                if (typesList != null && typesList.Count() > 0)
                    typesListToSend = typesList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
            }
            dto.PMPTargetingSaveDto.AdFormats = typesListToSend;

            if (dto.PMPTargetingSaveDto.Geographies == null)
            {
                dto.PMPTargetingSaveDto.Geographies = new List<int>();

            }
            if (dto.PMPTargetingSaveDto.AdSizes == null)
            {
                dto.PMPTargetingSaveDto.AdSizes = new List<int>();

            }

            if (dto.PMPTargetingSaveDto.AdFormats == null)
            {
                dto.PMPTargetingSaveDto.AdFormats = new List<int>();

            }
            SaveTargeting(dto.PMPTargetingSaveDto, item);
            var SavePMPDeal = new PMPDealSaveDto();
            SavePMPDeal.ID = item.ID;
            return SavePMPDeal;

        }



        public PMPDealDto GetDealPMPDeal(int Id)
        {
            var item = new PMPDeal();
            if (Id > 0)
            {
                item = _PMPDealRepository.Get(Id);
                ValidateDeal(item);
                var dto = MapperHelper.Map<PMPDealDto>(item);
                dto.PMPTargetingSaveDto = GetTargeting(Id, item);
                return dto;

            }
            else
            {

                return null;
            }





        }


        public bool Delete(IEnumerable<int> Ids)
        {
            if (Ids != null)
            {
                foreach (var item in Ids.Select(campaignId => _PMPDealRepository.Get(campaignId)))
                {

                    ValidateDeal(item);
                    item.Delete();
                    _PMPDealRepository.Save(item);

                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public PMPTargetingSaveDto GetTargeting(int dealId, PMPDeal obj)
        {

            PMPTargetingSaveDto targetingGeDto = new PMPTargetingSaveDto();
            //var item = _PMPDealRepository.Get(dealId);
            var item = obj;
            #region Gegoraphic Targeting
            var GegoraphicTargetings = (item.Targetings.ToList().OfType<GeographicPMPDealTargeting>()).ToList();


            if (GegoraphicTargetings != null)
            {
                targetingGeDto.Geographies = new List<int>();
                foreach (var gt in GegoraphicTargetings)
                {

                    targetingGeDto.Geographies.Add(gt.Location.ID);


                }
            }
            #endregion

            #region AdSizePMPDealTargeting Targeting
            var AdSizePMPDealTargetings = (item.Targetings.ToList().OfType<AdSizePMPDealTargeting>()).ToList();


            if (AdSizePMPDealTargetings != null)
            {
                targetingGeDto.AdSizes = new List<int>();
                foreach (var gt in AdSizePMPDealTargetings)
                {

                    targetingGeDto.AdSizes.Add(gt.AdSize.ID);


                }
            }


            #endregion

            #region AdTypeGroupPMPDealTargeting Targeting
            var AdTypeGroupPMPDealTargetings = (item.Targetings.ToList().OfType<AdTypeGroupPMPDealTargeting>()).ToList();





            if (AdTypeGroupPMPDealTargetings != null)
            {
                targetingGeDto.AdFormats = new List<int>();
                foreach (var gt in AdTypeGroupPMPDealTargetings)
                {

                    targetingGeDto.AdFormats.Add((int)gt.AdTypeGroup);

                    targetingGeDto.RawAdFormats = targetingGeDto.RawAdFormats + "" + (int)gt.AdTypeGroup + ",";
                }
            }
            #endregion


            return targetingGeDto;
        }
        private bool IsManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AdOps")
                || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator")
                || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager");
        }
        public bool SaveTargeting(PMPTargetingSaveDto targetingSaveDto, PMPDeal obj)
        {
            //var item = _PMPDealRepository.Get(targetingSaveDto.PMPDealID);
            var item = obj;


            #region Geographies
            //load Geographic targeting
            //if (targetings.OfType<GeographicTargeting>().ToList().Count > 0)
            {
                foreach (var geographic in targetingSaveDto.Geographies)
                {
                    var targetingObj =
                        item.Targetings.ToList().OfType<GeographicPMPDealTargeting>().FirstOrDefault(
                            targeting => targeting.Location.ID == geographic);
                    if (targetingObj == null)
                    {
                        //if not found then add it
                        var locationItem = new LocationBase { ID = geographic };
                        if (locationItem != null)
                        {
                            //create targeting Object
                            targetingObj = new GeographicPMPDealTargeting
                            {
                                Type = PMPDealTargetingType.Location,
                                Location = locationItem
                            };

                        }
                        item.AddTargeting(targetingObj);
                    }
                }
                if (item.Targetings != null)
                {
                    //Load List after the Update
                    foreach (var currentGeographic in item.Targetings.ToList().OfType<GeographicPMPDealTargeting>())
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        var found = false;
                        foreach (var geographic in targetingSaveDto.Geographies)
                        {
                            if (currentGeographic.Location.ID == geographic)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            //we should remove the targeting
                            item.RemoveTargeting(currentGeographic);
                        }
                    }
                }
            }
            #endregion

            #region AdSize
            //load Geographic targeting
            //if (targetings.OfType<GeographicTargeting>().ToList().Count > 0)
            {
                foreach (var AdSize in targetingSaveDto.AdSizes)
                {
                    var targetingObj =
                        item.Targetings.ToList().OfType<AdSizePMPDealTargeting>().FirstOrDefault(
                            targeting => targeting.AdSize.ID == AdSize);
                    if (targetingObj == null)
                    {
                        //if not found then add it
                        var creativeUnitItem = new CreativeUnit { ID = AdSize };
                        if (creativeUnitItem != null)
                        {
                            //create targeting Object
                            targetingObj = new AdSizePMPDealTargeting
                            {
                                Type = PMPDealTargetingType.AdSize,
                                AdSize = creativeUnitItem
                            };

                        }
                        item.AddTargeting(targetingObj);
                    }
                }
                //Load List after the Update

                if (item.Targetings != null)
                {
                    foreach (var currentAdSize in item.Targetings.ToList().OfType<AdSizePMPDealTargeting>())
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        var found = false;
                        foreach (var AdSize in targetingSaveDto.AdSizes)
                        {
                            if (currentAdSize.AdSize.ID == AdSize)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            //we should remove the targeting
                            item.RemoveTargeting(currentAdSize);
                        }
                    }
                }
            }
            #endregion

            #region AdFormats
            //load Geographic targeting
            //if (targetings.OfType<GeographicTargeting>().ToList().Count > 0)
            {
                foreach (var AdFormat in targetingSaveDto.AdFormats)
                {
                    AdTypeGroup convAdFormat = (AdTypeGroup)Enum.ToObject(typeof(AdTypeGroup), AdFormat);
                    var targetingObj =
                        item.Targetings.ToList().OfType<AdTypeGroupPMPDealTargeting>().FirstOrDefault(
                            targeting => targeting.AdTypeGroup == convAdFormat);
                    if (targetingObj == null)
                    {
                        //if not found then add it
                        var cAdFormat = convAdFormat;
                        // if (cAdFormat != null)
                        // //{
                        //create targeting Object
                        targetingObj = new AdTypeGroupPMPDealTargeting
                        {
                            Type = PMPDealTargetingType.AdFormat,
                            AdTypeGroup = cAdFormat
                        };


                        item.AddTargeting(targetingObj);
                    }
                }
                if (item.Targetings != null)
                {
                    //Load List after the Update
                    foreach (var currentAdFormat in item.Targetings.ToList().OfType<AdTypeGroupPMPDealTargeting>())
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        var found = false;
                        foreach (var AdFormat in targetingSaveDto.AdFormats)
                        {
                            if ((int)currentAdFormat.AdTypeGroup == AdFormat)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            //we should remove the targeting
                            item.RemoveTargeting(currentAdFormat);
                        }
                    }
                }
            }
            #endregion  
            //}
            _PMPDealRepository.Save(item);

            return true;

        }
        public IList<PMPDealDto> GetAllPMPDealsByAccount(int AccountId)
        {

            var Deals = _PMPDealRepository.GetAllPMPDealsByAccount(AccountId);
            IList<PMPDeal> allDeals = new List<PMPDeal>();
            var dealManager = IsManager();
            foreach (var deal in Deals)
            {
                if (deal.IsGlobal && !dealManager)
                {
                    if (_PMPDealRepository.IsCampsBydeal(deal.ID))
                        allDeals.Add(deal); 
                }
                else if (deal.IsGlobal&& dealManager)
                {

                    allDeals.Add(deal);
                }
                else
                {
                    allDeals.Add(deal);
                }

            }
            var list = allDeals.Select(Deal => MapperHelper.Map<PMPDealDto>(Deal)).ToList();
            return list;
        }
        public IList<CampaignDto> getCampsBydeal(int dealid)
        {

            var Campaigns = _PMPDealRepository.getCampsBydeal(dealid);
            var list = Campaigns.Select(Campaign => new CampaignDto { ID = Campaign.ID, Name = Campaign.Name }).ToList();

            return list != null ? list : new List<CampaignDto>();
        }
        public CampaignListResultDto getCampsAdvertiserBydeal(int dealid, int advertiserId)
        {


            CampaignListResultDto results = new CampaignListResultDto();

            var Campaigns = _PMPDealRepository.getCampsBydeal(dealid);
            if (advertiserId>0)
            {
                Campaigns = Campaigns.Where(M => M.AdvertiserAccount.ID == advertiserId).ToList();
            }
            if (Campaigns!=null)
            {
                results.CampaignItems = new List<CampaignDto>();
                results.AdvertiserAccountItems= new List<AdvertiserAccountDto>();
                foreach (var camp in Campaigns)
                {

                    results.CampaignItems.Add(new CampaignDto { ID = camp.ID, Name = camp.Name });

                    if (results.AdvertiserAccountItems.Where(M=>M.Id==camp.AdvertiserAccount.ID ).SingleOrDefault()==null)
                    {
                        results.AdvertiserAccountItems.Add(new AdvertiserAccountDto { Id = camp.AdvertiserAccount.ID, Name = camp.AdvertiserAccount.Name });

                    }
                }

            }

            return results;


        }

        
        public IList<AdvertiserAccountDto> getAdvertiserAccountsBydeal(int dealid)
        {

            var Campaigns = _PMPDealRepository.getAdvertiserAccountsBydeal(dealid);
            var list = Campaigns.Select(Campaign => new AdvertiserAccountDto { Id = Campaign.ID, Name = Campaign.Name }).ToList();

            return list != null ? list : new List<AdvertiserAccountDto>();
        }

        
        public IList<AdGroupDto> getDealCampsAdgruops(int dealId, int campId)
        {

            var Adgruops = _PMPDealRepository.getDealCampsAdgruops(dealId, campId);
            var list = Adgruops.Select(Adgruop => MapperHelper.Map<AdGroupDto>(Adgruop)).ToList();
            return list != null ? list : new List<AdGroupDto>();
        }



        public IList<Interfaces.DTOs.Core.TreeDto> GetAdFormatsTree(IList<int> AdFormats)
        {


            List<CreativeUnit> AllCreativeUnit = _CreativeUnitRepository.GetAll().ToList();
            List<TreeDto> returnList = new List<TreeDto>();

            if (AdFormats != null && AdFormats.Count > 0)
            {
                foreach (var item in AdFormats)
                {
                    var treeDto = new TreeDto { Id = item.ToString() };
                    var childs = new List<TreeDto>();
                    AdTypeGroup convAdFormat = (AdTypeGroup)Enum.ToObject(typeof(AdTypeGroup), item);
                    var creativUnits = AllCreativeUnit.Where(M => M.SupportedTypes.Any(C => C.AdType.Group == convAdFormat)).ToList();
                    foreach (var item1 in creativUnits)
                    {
                        if (item1.Width > 0 && item1.Height > 0)
                        {
                            var childDto = new TreeDto
                            {
                                Id = item1.ID.ToString(),
                                Name = LocalizedStringDto.ConvertToLocalizedStringDto(item1.ToString())
                            };
                            childs.Add(childDto);
                        }
                    }
                    treeDto.Childs = childs;
                    treeDto.Name = LocalizedStringDto.ConvertToLocalizedStringDto(convAdFormat.ToText());
                    returnList.Add(treeDto);
                }
            }
            else
            {
                var treeDto = new TreeDto { Id = "0" };
                var childs = new List<TreeDto>();
                foreach (var item1 in AllCreativeUnit)
                {
                    var childDto = new TreeDto
                    {
                        Id = item1.ID.ToString(),
                        Name = LocalizedStringDto.ConvertToLocalizedStringDto(item1.GetDescription())
                    };
                    childs.Add(childDto);
                }
                treeDto.Childs = childs;
                treeDto.Name = LocalizedStringDto.ConvertToLocalizedStringDto(ResourceManager.Instance.GetResource("All", "Global"));
                returnList.Add(treeDto);


            }

            return returnList.ToList();
        }

    }
}
