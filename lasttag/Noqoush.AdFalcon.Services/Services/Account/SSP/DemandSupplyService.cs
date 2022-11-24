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
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Account.SSP
{
    public class SupplyService : ISupplyService
    {

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

        private ICostModelWrapperRepository costModelWrapperRepository;
        static readonly object LockObj = new object();

        public SupplyService(IAccountRepository accountRepository,
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


            , INativeAdCreativeBaseRepository NativeAdCreativeBaseRepository
            , ICostModelWrapperRepository CostModelWrapperRepository
            )
        {

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

            this.costModelWrapperRepository =CostModelWrapperRepository;
        }
        #region  Query Data
        public ResultPartnerDto QueryByCratiriaForPartner(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.PartnerCriteria wcriteria)
        {


            PartnerCriteria criteria = new PartnerCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);


            var result = new ResultPartnerDto();
            var businessType = this._businessPartnerTypeRepository.Query(M => M.Code == "DemandType").SingleOrDefault();
            if (businessType != null)
            {
                criteria.TypeId = businessType.ID;
            }
            IEnumerable<Domain.Model.Core.BusinessPartner> list = null;
            if (criteria.Page.HasValue)
            {
                list = _businessPartnerRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _businessPartnerRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<PartnerDto>(campaign)).ToList();

            result.Items = returnList;
            result.TotalCount = _businessPartnerRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public ResultPartnerSiteDto QueryByCratiriaForSitePartner(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.PartnerSiteCriteria wcriteria)
        { 


             PartnerSiteCriteria criteria = new PartnerSiteCriteria();
        criteria.CopyFromCommonToDomain(wcriteria);

            var result = new ResultPartnerSiteDto();
            var bizPartenr = _businessPartnerRepository.Get(criteria.PartnerId);
            IEnumerable<Domain.Model.Account.SSP.PartnerSite> list = null;
            if (criteria.Page.HasValue)
            {
                list = _partnerSiteRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _partnerSiteRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<PartnerSiteDto>(campaign)).ToList();
            result.BusinessId = criteria.PartnerId;
            result.BusinessName = bizPartenr.Name;
            result.Items = returnList;
            result.TotalCount = _partnerSiteRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public ResultSiteZoneDto QueryByCratiriaForSiteZone(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.SiteZoneCriteria wcriteria)
        {


            SiteZoneCriteria criteria = new SiteZoneCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new ResultSiteZoneDto();
            var bizPartenr = _businessPartnerRepository.Get(criteria.BusinessId);

            var site = _partnerSiteRepository.Get(criteria.SiteId);
            IEnumerable<Domain.Model.Account.SSP.SiteZone> list = null;
            if (criteria.Page.HasValue)
            {
                list = _siteZoneRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _siteZoneRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<SiteZoneDto>(campaign)).ToList();
            result.BusinessId = criteria.BusinessId;
            result.BusinessName = bizPartenr.Name;
            result.SiteId = criteria.SiteId;
            result.SiteName = site.SiteName;
            result.SiteIdStr = site.SiteID;
            result.Items = returnList;
            result.TotalCount = _siteZoneRepository.Query(criteria.GetExpression()).Count();
            return result;
        }
        public ResultFloorPriceConfigDto QueryByCratiriaForFloorPrice(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.FloorPriceCriteria wcriteria)
        {
            FloorPriceCriteria criteria = new FloorPriceCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);


            var result = new ResultFloorPriceConfigDto();
            var site = _partnerSiteRepository.Get(criteria.SiteId);
            var zone = _siteZoneRepository.Get(criteria.ZoneId);
            IEnumerable<Domain.Model.Account.SSP.FloorPrice> list = null;
            if (criteria.Page.HasValue)
            {
                list = _floorPriceRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                list = _floorPriceRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
            }
            var returnList = list.Select(campaign => MapperHelper.Map<FloorPriceConfigDto>(campaign)).ToList();
            result.BusinessId = site.Partner.ID;
            result.BusinessName = site.Partner.Name;
            result.SiteId = criteria.SiteId;
            result.SiteName = site.SiteName;
            result.ZoneName = zone.ZoneName;
            result.ZoneId = criteria.ZoneId;
            result.SiteIdStr = site.SiteID;
            result.ZoneIdStr = zone.ZoneID;

            if (criteria.Page.HasValue)
            {


                //if (criteria.Page < 2)
                //{
                //    FloorPrice fpPrice = new FloorPrice();
                //    var dtoFloorPrice = GetBaseFloorPrice(criteria.SiteId, criteria.ZoneId);

                //    if (dtoFloorPrice == null)
                //    {



                //        fpPrice.ConfigType = FloorPriceConfigType.Base;
                //        fpPrice.Price = 0;
                //        fpPrice.Zone = new SiteZone { ID = criteria.ZoneId };
                //        fpPrice.Site = new PartnerSite { ID = criteria.SiteId };
                //        _floorPriceRepository.Save(fpPrice);
                //    }
                //}
            }


            if (returnList != null && returnList.Count > 0)
            {
                foreach (var item in returnList)
                {

                    item.BidConfigType = GetBidConfigString(item.ConfigType);

                    //GetBidConfigString();

                }
                returnList = returnList.Where(M => M.ConfigType != FloorPriceConfigType.Base).ToList();
                result.Items = returnList.OrderBy(x => x.ConfigType).ToList();
                result.TotalCount = _floorPriceRepository.Query(criteria.GetExpression()).Count() - 1;
            }
            else
            {
                result.Items = new List<FloorPriceConfigDto>();

            }

            return result;
        }
        public ResultSiteZoneMapping QueryByCratiriaForSiteZoneMapping(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.SiteZoneMappingCriteria wcriteria)
        {


            SiteZoneMappingCriteria criteria = new SiteZoneMappingCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            var result = new ResultSiteZoneMapping();
            var site = _partnerSiteRepository.Get(criteria.SiteId);
            var zone = _siteZoneRepository.Get(criteria.ZoneId);
            IEnumerable<Domain.Model.Account.SSP.SiteZoneMapping> list = null;

            list = _siteZoneMappingRepository.QueryByCratiriaForSiteZoneMapping(criteria);


            //list = _siteZoneMappingRepository.QueryByCratiriaForSiteZoneMapping(criteria); // _siteZoneMappingRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);

            var returnList = list.Select(campaign => MapperHelper.Map<SiteZoneMappingDto>(campaign)).ToList();
            result.BusinessId = site.Partner.ID;
            result.BusinessName = site.Partner.Name;
            result.SiteId = criteria.SiteId;
            result.SiteName = site.SiteName;
            result.ZoneName = zone.ZoneName;
            result.ZoneId = criteria.ZoneId;
            result.SiteIdStr = site.SiteID;
            result.ZoneIdStr = zone.ZoneID;
            result.Items = returnList;
            criteria.Page = null;
            result.TotalCount = _siteZoneMappingRepository.QueryByCratiriaForSiteZoneMapping(criteria).Count();
            return result;
        }
        public ResultDealCampaignMappingDto QueryByCratiriaForDealCampaignMapping(Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP.DealCampaignMappingCriteria wcriteria)
        {

            DealCampaignMappingCriteria criteria = new DealCampaignMappingCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new ResultDealCampaignMappingDto();
            var bizPartenr = _businessPartnerRepository.Get(criteria.PartnerId);
            IEnumerable<Domain.Model.Account.SSP.DealCampaignMapping> list = null;

            list = _dealCampaignMappingRepository.QueryByCratiriaForDealCampaignMapping(criteria);


            var returnList = list.Select(campaign => MapperHelper.Map<DealCampaignMappingDto>(campaign)).ToList();
            result.BusinessId = criteria.PartnerId;
            result.BusinessName = bizPartenr.Name;
            result.Items = returnList;
            criteria.Page = null;

            result.TotalCount = _dealCampaignMappingRepository.QueryByCratiriaForDealCampaignMapping(criteria).Count();
            return result;
        }
        #endregion

        public void DeleteDealCampaignMapping(int[] Ids)
        {




            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _dealCampaignMappingRepository.Get(item)))
                {

                    item.Delete();
                    _dealCampaignMappingRepository.Save(item);

                }
            }

        }
        public void SaveDealCampaignMapping(DealCampaignMappingDto dto)
        {
            var item = new DealCampaignMapping();


            if (dto.ID > 0)
            {
                item = _dealCampaignMappingRepository.Get(dto.ID);

                item.DealId = dto.DealId;
                if (dto.AdFalconCampaignId > 0)
                    item.Campaign = _campaignRepository.Get(dto.AdFalconCampaignId);
                else
                    item.Campaign = null;
                //item.Description = dto.Description;
            }
            else
            {

                item = MapperHelper.Map<DealCampaignMapping>(dto);
                //item.Partner= new BusinessPartner{ID=dto.p}
                if (dto.AdFalconCampaignId > 0)
                    item.Campaign = _campaignRepository.Get(dto.AdFalconCampaignId);
                else
                    item.Campaign = null;
            }

            //item.SiteZoneValidation(item.ZoneName, item.ZoneID, item.Site.ID, item.ID);
            item.DealCampaignMappingValidation(item.ID);
            //For Audit trial-To Do should be improved

            if (dto.PartnerID > 0)
                item.Partner = _businessPartnerRepository.Get(dto.PartnerID);
            _dealCampaignMappingRepository.Save(item);
            if (item.Campaign != null)
                SetCampaingForDealing(item.Campaign);


        }


        private void SetCampaingForDealing(Noqoush.AdFalcon.Domain.Model.Campaign.Campaign camp)
        {
            if (camp.CampaignType!= CampaignType.ProgrammaticGuaranteed)
            {
                camp.EndDate = null;
                // camp.CampaignType = CampaignType.ProgrammaticGuaranteed;
                camp.RemoveDiscount();
                if (camp.GetGroups() != null)
                {
                    foreach (var adgrou in camp.GetGroups())
                    {
                        adgrou.Bid = 0;
                        adgrou.MinimumUnitPrice = 0.05M;
                        adgrou.AudianceDiscountPrice = 0;
                        adgrou.IsDefaultPrerequisitesSaved = true;
                        CostModelWrapper costModelWrapper = costModelWrapperRepository.Get((int)CostModelWrapperEnum.CPM);
                        adgrou.SetCostModelWrapper(costModelWrapper);
                        SetCampDealingAdGroupEvent(adgrou);
                        if (adgrou.GetAds() != null)
                        {
                            foreach (var ad in adgrou.GetAds())
                            {
                                ad.SetAdCreativeBid(0, 1);
                                if (AdTypeIds.NativeAd == ad.TypeId)
                                {
                                    var native = (NativeAdCreative)ad;
                                    //if (native.Images != null)
                                    //{ for (int i = 0; i < native.Images.Count; i++)
                                    //    {
                                    //        native.Images[i].AdCreative = null;
                                    //        this._NativeAdCreativeBaseRepository.Remove(native.Images[i]);
                                    //    }
                                    //}

                                    //if (native.Icons != null)
                                    //{
                                    //    for (int i = 0; i < native.Icons.Count; i++)
                                    //    {
                                    //        native.Icons[i].AdCreative = null;
                                    //        this._NativeAdCreativeBaseRepository.Remove(native.Icons[i]);
                                    //    }
                                    //}
                                    native.Images.Clear();
                                    native.Icons.Clear();
                                    native.AdText = null;
                                    native.ActionValue.Value = null;
                                    native.ActionValue.Value2 = null;

                                }

                                if (AdTypeIds.InStreamVideo == ad.TypeId && AdSubTypes.VideoLinear == ad.AdSubType)
                                {
                                    var stream = (InStreamVideoCreative)ad;
                                    stream.Description = null;
                                    stream.DurationInSeconds = 0;
                                    stream.AdText = null;
                                    stream.IsSecureCompliant = false;
                                    //stream.medi
                                }


                                if (ad.GetCreativeUnits() != null)
                                {
                                    foreach (var Creatad in ad.GetCreativeUnits())
                                    {
                                        Creatad.Document = null;
                                        Creatad.SnapshotDocument = null;
                                        Creatad.SnapshotUrl = null;
                                        Creatad.KeepShapshot = false;

                                        Creatad.Content = "<script></script>";
                                        if (Creatad.InStreamVideoCreativeUnit != null)
                                        {

                                            Creatad.InStreamVideoCreativeUnit.BitRate = 0;
                                            Creatad.InStreamVideoCreativeUnit.Width = 0;
                                            Creatad.InStreamVideoCreativeUnit.Height = 0;
                                            Creatad.InStreamVideoCreativeUnit.ThumbnailDoc = null;
                                            Creatad.RemoveMediaFiles();
                                            Creatad.Content = null;
                                        }
                                        if (Creatad.CreativeUnit.ID == 20)
                                        {

                                            Creatad.Content = null;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                _campaignRepository.Save(camp);
            }
      
        }


        public void SetCampDealingAdGroupEvent(AdGroup adgroup)
        {

            var trackingEvents = adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.IsBillable == true).ToList();

            foreach (var trackingEvent in trackingEvents)
            {
                trackingEvent.IsBillable = false;
            }

            var trackingEventimp= adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == "000imp").SingleOrDefault();
            if (trackingEventimp!=null)
            {
                trackingEventimp.IsBillable = true;

            }


        }
        public KeyValuePair<bool, string> IsDeleteTrackingEventAllowed(int campaignId, int adGroupId, List<string> adGroupTrackingEventCodes, bool checkStandards, int? newCostModelWrapperId)
        {
            var campaign = _campaignRepository.Get(campaignId);
       

            AdActionTypeBase adActionType = null;
            IEnumerable<AdActionTypeTrackingEvent> adActionTrackingEvents = null;

            var result = new KeyValuePair<bool, string>(true, string.Empty);

            if (campaign.IsValid)
            {
                var adgroup = campaign.AdGroups.Where(p => p.ID == adGroupId).SingleOrDefault();
                if (adgroup != null && !adgroup.IsDeleted)
                {
                    if (newCostModelWrapperId.HasValue)
                    {
                        adActionType = adgroup.Objective.AdAction;
                        adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapperEnum == (CostModelWrapperEnum)newCostModelWrapperId.Value);
                    }

                    if (adGroupTrackingEventCodes == null || adGroupTrackingEventCodes.Count == 0)
                    {
                        adGroupTrackingEventCodes = adgroup.TrackingEvents.Where(p => !p.IsDeleted).Select(p => p.Code).ToList();
                    }
                    bool exitLoop = false;
                    if (checkStandards)
                    {

                        foreach (var adGroupTrackingEventCode in adGroupTrackingEventCodes)
                        {
                            var trackingEvent = adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == adGroupTrackingEventCode).SingleOrDefault();

                            if (trackingEvent != null)
                            {
                                if (/*trackingEvent.IsBillable ||*/ !trackingEvent.IsCustom)
                                {
                                    result = new KeyValuePair<bool, string>(false, ResourceManager.Instance.GetResource("DeleteStandardEvent"));
                                    break;
                                }

                                foreach (var item in adgroup.GetAds())
                                {
                                    if (item.AdCreativeUnits.Any(p => p.Trackers.Any(x => !x.IsDeleted && x.AdGroupEvent.ID == trackingEvent.ID)))
                                    {
                                        bool raiseError = true;
                                        if (newCostModelWrapperId.HasValue)
                                        {
                                            if (adActionTrackingEvents.Any(p => p.Event.Code == trackingEvent.Code))
                                            {
                                                raiseError = false;
                                            }
                                        }

                                        if (raiseError)
                                        {
                                            result = new KeyValuePair<bool, string>(false, ResourceManager.Instance.GetResource("DeleteEventsWithTrackers"));
                                            exitLoop = true;
                                            break;
                                        }
                                    }
                                }

                                if (exitLoop)
                                    break;
                            }
                        }

                    }
                    else
                    {
                        foreach (var adGroupTrackingEventCode in adGroupTrackingEventCodes)
                        {
                            var trackingEvent = adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == adGroupTrackingEventCode).SingleOrDefault();

                            if (trackingEvent != null)
                            {
                                foreach (var item in adgroup.GetAds())
                                {
                                    if (item.AdCreativeUnits.Any(p => p.Trackers.Any(x => !x.IsDeleted && x.AdGroupEvent.ID == trackingEvent.ID)))
                                    {
                                        bool raiseError = true;
                                        if (newCostModelWrapperId.HasValue)
                                        {
                                            if (adActionTrackingEvents.Any(p => p.Event.Code == trackingEvent.Code))
                                            {
                                                raiseError = false;
                                            }
                                        }

                                        if (raiseError)
                                        {
                                            result = new KeyValuePair<bool, string>(false, ResourceManager.Instance.GetResource("DeleteEventsWithTrackers"));
                                            exitLoop = true;
                                            break;
                                        }
                                    }
                                }

                                if (exitLoop)
                                    break;
                            }
                        }
                    }

                }

            }

            return result;
        }

        private void AddDefaultAdGroupTrackingEvent(AdGroup adgroup, int costModelWrapperId, int? oldCostModelWrapper)
        {
            List<string> oldAdGroupTrackingEvents = new List<string>();

            if (oldCostModelWrapper.HasValue && oldCostModelWrapper != adgroup.CostModelWrapper.ID)
            {
                foreach (var trackingEvent in adgroup.TrackingEvents.Where(p => !p.IsDeleted))
                {
                    var result = IsDeleteTrackingEventAllowed(adgroup.Campaign.ID, adgroup.ID, new List<string>() { trackingEvent.Code }, false, costModelWrapperId);

                    if (result.Key)
                    {
                        oldAdGroupTrackingEvents.Add(trackingEvent.Code);
                        trackingEvent.IsDeleted = true;
                    }
                }
            }

            var adActionType = adgroup.Objective.AdAction;
            var adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapper == null || p.CostModelWrapperEnum == (CostModelWrapperEnum)costModelWrapperId).ToList();
            var costModelWrapperBill = costModelWrapperRepository.Get(costModelWrapperId);
            var billableEvent = 0;
            if (costModelWrapperBill != null && costModelWrapperBill.Event != null)
                billableEvent = costModelWrapperBill.Event.ID;

            if (adActionTrackingEvents != null)
            {
                bool checkBillable = true;
                var billableevents = adActionTrackingEvents.Where(M => M.IsBillable == true).ToList();
                if (billableevents != null && billableevents.Count > 0)
                {
                    checkBillable = false;
                }
                foreach (var item in adActionTrackingEvents)
                {
                    AdGroupTrackingEvent adGroupTrackingEvent = MapperHelper.Map<AdGroupTrackingEvent>(item);
                    if (billableEvent == item.Event.ID && checkBillable)
                    {
                        if (adgroup.CostModelWrapperEnum != CostModelWrapperEnum.CPA)
                            adGroupTrackingEvent.IsBillable = true;
                    }

                    // Move trackers from old adgrouptrackingevent to new one (for adgrouptrackingevent with same code)
                    // By: Malik Hassan
                    if (oldAdGroupTrackingEvents.Contains(adGroupTrackingEvent.Code))
                    {
                        foreach (var ad in adgroup.GetAds())
                        {
                            var creativeUnitsWithTrackers = ad.GetCreativeUnits().Where(p => p.Trackers.Any(x => !x.IsDeleted && x.AdGroupEvent.Code == adGroupTrackingEvent.Code));
                            foreach (var creativeUnit in creativeUnitsWithTrackers)
                            {
                                var trackers = creativeUnit.Trackers.Where(p => !p.IsDeleted && p.AdGroupEvent.Code == adGroupTrackingEvent.Code);
                                foreach (var tracker in trackers)
                                {
                                    tracker.AdGroupEvent = adGroupTrackingEvent;
                                }
                            }
                        }
                    }

                    adGroupTrackingEvent.AdGroup = adgroup;
                    adgroup.TrackingEvents.Add(adGroupTrackingEvent);
                }
            }
        }

        public DealCampaignMappingDto GetDealCampaignMapping(int Id)
        {
            var item = new DealCampaignMapping();
            if (Id > 0)
            {
                item = _dealCampaignMappingRepository.Get(Id);
                var dto = MapperHelper.Map<DealCampaignMappingDto>(item);

                return dto;

            }
            else
            {

                return null;
            }





        }

        public void DeleteSitePartner(int[] Ids)
        {

            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _partnerSiteRepository.Get(item)))
                {

                    item.Delete();
                    _partnerSiteRepository.Save(item);

                }
            }



        }
        public void DeleteBusinessPartner(int[] Ids)
        {


            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _businessPartnerRepository.Get(item)))
                {

                    item.Delete();
                    _businessPartnerRepository.Save(item);

                }
            }

        }


        public void DeleteSiteZoneMapping(int[] Ids)
        {


            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _siteZoneMappingRepository.Get(item)))
                {

                    item.Delete();
                    _siteZoneMappingRepository.Save(item);

                }
            }



        }
        public void SaveSitePartner(PartnerSiteDto dto)
        {
            var item = new PartnerSite();


            if (dto.ID > 0)
            {
                item = _partnerSiteRepository.Get(dto.ID);

                item.SiteName = dto.SiteName;
                item.SiteID = dto.SiteID;
                item.Description = dto.Description;
            }
            else
            {

                item = MapperHelper.Map<PartnerSite>(dto);
            }

            item.PartnerSiteValidation(item.SiteName, item.SiteID, item.Partner.ID, item.ID);
            //For Audit trial-To Do should be improved

            if (dto.PartnerID > 0)
                item.Partner = _businessPartnerRepository.Get(dto.PartnerID);
            _partnerSiteRepository.Save(item);


        }


        public PartnerSiteDto GetSitePartner(int Id)
        {

            var item = new PartnerSite();
            if (Id > 0)
            {
                item = _partnerSiteRepository.Get(Id);
                var dto = MapperHelper.Map<PartnerSiteDto>(item);

                return dto;
            }
            else
            {

                return null;
            }




        }
        public SiteZoneDto GetSiteZone(int Id)
        {
            var item = new SiteZone();
            if (Id > 0)
            {
                item = _siteZoneRepository.Get(Id);
                var dto = MapperHelper.Map<SiteZoneDto>(item);

                return dto;

            }
            else
            {

                return null;
            }





        }
        public FloorPriceConfigDto GetBaseFloorPrice(int SiteId, int ZoneId)
        {

            var item = _floorPriceRepository.Query(M => M.Zone.ID == ZoneId && M.Site.ID == SiteId && M.ConfigType == FloorPriceConfigType.Base).SingleOrDefault();
            if (item != null)
            {
                var dto = MapperHelper.Map<FloorPriceConfigDto>(item);
                dto.ConfigTypeId = (int)dto.ConfigType;
                return dto;
            }
            else
                return null;








        }
        public FloorPriceConfigDto GetFloorPrice(int Id)
        {
            var item = new FloorPrice();
            if (Id > 0)
            {
                item = _floorPriceRepository.Get(Id);
                var dto = MapperHelper.Map<FloorPriceConfigDto>(item);
                dto.ConfigTypeId = (int)dto.ConfigType;
                return dto;

            }
            else
            {

                return null;
            }





        }


        public void SaveFloorPrice(FloorPriceConfigDto dto)
        {
            var item = new FloorPrice();
            if (dto.ID > 0)
            {
                item = _floorPriceRepository.Get(dto.ID);
                if (dto.TargetingId > -1)

                    item.TargetingId = dto.TargetingId;
                else
                    item.TargetingId = null;
                item.Price = dto.Price;
                item.ConfigType = dto.ConfigType;
            }
            else
            {

                item = MapperHelper.Map<FloorPrice>(dto);
                if (dto.TargetingId > -1)

                    item.TargetingId = dto.TargetingId;
                else
                    item.TargetingId = null;

                if (item.ConfigType != FloorPriceConfigType.Base)
                {
                    bool isnew = false;
                    FloorPrice fpPrice = new FloorPrice();
                    var dtoFloorPrice = GetBaseFloorPrice(dto.SiteID, dto.ZoneID);

                    if (dtoFloorPrice == null)
                    {
                        isnew = true;

                    }


                    fpPrice.ConfigType = FloorPriceConfigType.Base;
                    fpPrice.Price = 0;
                    fpPrice.Zone = item.Zone;
                    fpPrice.Site = item.Site;
                    if (isnew)
                        _floorPriceRepository.Save(fpPrice);
                }
            }

            item.FloorPriceValidation(item.ID);
            //For Audit trial
            if (dto.SiteID > 0)
                item.Site = _partnerSiteRepository.Get(dto.SiteID);
            if (item.Site.Partner != null)
                item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);
            _floorPriceRepository.Save(item);


        }


        public void DeleteSiteZone(int[] Ids)
        {




            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _siteZoneRepository.Get(item)))
                {

                    item.Delete();
                    _siteZoneRepository.Save(item);

                }
            }

        }
        public void SaveSiteZone(SiteZoneDto dto)
        {
            var item = new SiteZone();



            if (dto.ID > 0)
            {
                item = _siteZoneRepository.Get(dto.ID);

                item.ZoneName = dto.ZoneName;
                item.ZoneID = dto.ZoneID;
                item.Description = dto.Description;

            }
            else
            {

                item = MapperHelper.Map<SiteZone>(dto);

            }


            item.SiteZoneValidation(item.ZoneName, item.ZoneID, item.Site.ID, item.ID);
            //For Audit trial-To Do should be improved
            if (dto.SiteID > 0)
                item.Site = _partnerSiteRepository.Get(dto.SiteID);
            if (item.Site.Partner != null)
                item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);
            _siteZoneRepository.Save(item);



        }

        public void SaveSiteZoneMapping(SiteZoneMappingDto dto, int[] appSiteIds)
        {
            SiteZoneMappingCriteria criteria = new SiteZoneMappingCriteria();
            criteria.SiteId = dto.SiteID;
            criteria.ZoneId = dto.ZoneID;
            var item = new SiteZoneMapping();
            if (dto.ID > 0)
            {
                item = _siteZoneMappingRepository.Get(dto.ID);
                criteria.ZoneId = item.Zone.ID;
                criteria.SiteId = item.Site.ID;
                if (dto.DeviceTypeID > 0)
                    item.DeviceType = new DeviceType { ID = dto.DeviceTypeID };
                else
                    item.DeviceType = null;

                if (dto.AdTypeID > 0)
                    item.AdType = new AdType { ID = dto.AdTypeID };
                else
                    item.AdType = null;

                item.IsInterstitial = dto.IsInterstitial;

                var enquiryId = item.IsUniqueMapping(criteria);
                if (enquiryId != 0 && enquiryId != item.ID)
                {
                    throw new Exception("Duplicated Mapping");
                }
                //item.Description = dto.Description;
                //if (dto.NativeLayoutId > 0)
                //{
                //    var appSite = _appSiteRepository.Get(dto.AppSiteID);
                //    appSite.AppSiteServerSetting.ChangeNativeAdLayout(new NativeAdLayout { ID = dto.NativeLayoutId });
                //    _appSiteRepository.Save(appSite);
                //    item.AppSite = appSite;

                //}
                //else
                //{
                //    item.AppSite = new Domain.Model.AppSite.AppSite { ID = dto.AppSiteID };
                //}

                ChangeNativeLayoutForMapping(dto, item, dto.AppSiteID);
            }
            else
            {

                foreach (var appSiteId in appSiteIds)
                {
                    item = MapperHelper.Map<SiteZoneMapping>(dto);
                    //if (dto.NativeLayoutId > 0)
                    //{
                    //    var appSite = _appSiteRepository.Get(appSiteId);
                    //    appSite.AppSiteServerSetting.ChangeNativeAdLayout(new NativeAdLayout { ID = dto.NativeLayoutId });
                    //    _appSiteRepository.Save(appSite);
                    //    item.AppSite = appSite;

                    //}
                    //else
                    //{
                    //    item.AppSite = new Domain.Model.AppSite.AppSite { ID = appSiteId };
                    //}

                    ChangeNativeLayoutForMapping(dto, item, appSiteId);
                    var enquiryId = item.IsUniqueMapping(criteria);
                    if (enquiryId == 0)
                    {
                        //For Audit trial-To Do should be improved
                        item.Site = _partnerSiteRepository.Get(dto.SiteID);
                        item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);

                        _siteZoneMappingRepository.Save(item);
                    }
                    else
                    {
                        var existItem = _siteZoneMappingRepository.Get(enquiryId);

                        ChangeNativeLayoutForMapping(dto, existItem, appSiteId);
                    }



                }
                return;
            }

            //For Audit trial-To Do should be improved
            if (dto.SiteID > 0)
                item.Site = _partnerSiteRepository.Get(dto.SiteID);
            if (item.Site.Partner != null)
                item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);
            _siteZoneMappingRepository.Save(item);


        }

        public void SaveSiteZoneMapping(SiteZoneMappingDto dto, IList<AssignedAppsitesDto> appSites)
        {

            SiteZoneMappingCriteria criteria = new SiteZoneMappingCriteria();
            criteria.SiteId = dto.SiteID;
            criteria.ZoneId = dto.ZoneID;
            var item = new SiteZoneMapping();
            if (dto.ID > 0)
            {
                item = _siteZoneMappingRepository.Get(dto.ID);
                criteria.ZoneId = item.Zone.ID;
                criteria.SiteId = item.Site.ID;
                if (dto.DeviceTypeID > 0)
                    item.DeviceType = new DeviceType { ID = dto.DeviceTypeID };
                else
                    item.DeviceType = null;

                if (dto.AdTypeID > 0)
                    item.AdType = new AdType { ID = dto.AdTypeID };
                else
                    item.AdType = null;

                item.IsInterstitial = dto.IsInterstitial;
                var enquiryId = item.IsUniqueMapping(criteria);
                if (enquiryId != 0 && enquiryId != item.ID)
                {
                    throw new Exception("Duplicated Mapping");
                }
                //item.Description = dto.Description;
                //if (dto.NativeLayoutId > 0)
                //{
                //    var appSite = _appSiteRepository.Get(dto.AppSiteID);
                //    appSite.AppSiteServerSetting.ChangeNativeAdLayout(new NativeAdLayout { ID = dto.NativeLayoutId });
                //    _appSiteRepository.Save(appSite);
                //    item.AppSite = appSite;

                //}
                //else
                //{
                //    item.AppSite = new Domain.Model.AppSite.AppSite { ID = dto.AppSiteID };
                //}

                //ChangeNativeLayoutForMapping(dto, item, dto.AppSiteID);
            }
            else
            {

                foreach (var appSite in appSites)
                {
                    item = MapperHelper.Map<SiteZoneMapping>(dto);
                    //if (dto.NativeLayoutId > 0)
                    //{
                    //    var appSite = _appSiteRepository.Get(appSiteId);
                    //    appSite.AppSiteServerSetting.ChangeNativeAdLayout(new NativeAdLayout { ID = dto.NativeLayoutId });
                    //    _appSiteRepository.Save(appSite);
                    //    item.AppSite = appSite;

                    //}
                    //else
                    //{
                    //    item.AppSite = new Domain.Model.AppSite.AppSite { ID = appSiteId };
                    //}
                    if (!string.IsNullOrEmpty(appSite.SubPublisherId))
                        item.AdFalconSubPublisherId = appSite.SubPublisherId;
                    else
                        item.AdFalconSubPublisherId = null;
                    if (item.AppSite == null)
                        item.AppSite = new Domain.Model.AppSite.AppSite { ID = appSite.Appsite.ID };

                    item.SubAppSite = appSite.SubAppsiteId.HasValue ? new SubAppsite { ID = (int)appSite.SubAppsiteId } : null;

                    //ChangeNativeLayoutForMapping(dto, item, appSite.Appsite.ID);
                    var enquiryId = item.IsUniqueMapping(criteria);
                    if (enquiryId == 0)
                    {
                        //For Audit trial-To Do should be improved
                        item.Site = _partnerSiteRepository.Get(dto.SiteID);
                        item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);
                        _siteZoneMappingRepository.Save(item);
                    }
                    else
                    {

                        throw new Exception("Duplicated Mapping");
                    }
                    //else
                    //{
                    //    var existItem = _siteZoneMappingRepository.Get(enquiryId);

                    //    //ChangeNativeLayoutForMapping(dto, existItem, appSite.Appsite.ID);
                    //}


                }
                return;
            }
            //For Audit trial-To Do should be improved
            if (item.Site == null && dto.SiteID > 0)
                item.Site = _partnerSiteRepository.Get(dto.SiteID);
            if (item.Site.Partner != null)
                item.Site.Partner = _businessPartnerRepository.Get(item.Site.Partner.ID);

            _siteZoneMappingRepository.Save(item);
        }
        public void ChangeNativeLayoutForMapping(SiteZoneMappingDto dto, SiteZoneMapping item, int appSiteId)
        {

            if (dto.NativeLayoutId > 0)
            {
                var appSite = _appSiteRepository.Get(appSiteId);
                appSite.AppSiteServerSetting.ChangeNativeAdLayout(new NativeAdLayout { ID = dto.NativeLayoutId });
                _appSiteRepository.Save(appSite);
                if (item.AppSite == null)
                    item.AppSite = appSite;

            }
            else
            {
                if (item.AppSite == null)
                    item.AppSite = new Domain.Model.AppSite.AppSite { ID = appSiteId };
            }
        }
        public SiteZoneMappingDto GetSiteZoneMapping(int Id)
        {
            var item = new SiteZoneMapping();
            if (Id > 0)
            {
                item = _siteZoneMappingRepository.Get(Id);

                var dto = MapperHelper.Map<SiteZoneMappingDto>(item);

                if (dto.AppSiteID > 0)
                {
                    var appSite = _appSiteRepository.Get(dto.AppSiteID);
                    if (appSite != null && appSite.AppSiteServerSetting != null)
                    {
                        if (appSite.AppSiteServerSetting.NativeAdLayout != null)
                        {
                            dto.NativeLayoutId = appSite.AppSiteServerSetting.NativeAdLayout.ID;
                            dto.IsNative = true;

                        }

                    }


                }
                return dto;

            }
            else
            {

                return null;
            }





        }
        public LookupListResultDto GetBidConfigTypeList()
        {



            var totalCount = 0;
            var items = new List<LookupDto>();
            LookupDto item;

            foreach (FloorPriceConfigType val in Enum.GetValues(typeof(FloorPriceConfigType)))
            {

                if (val != FloorPriceConfigType.Base)
                {
                    totalCount++;
                    item = new LookupDto();
                    item.ID = (int)val;
                    item.Name = new LocalizedStringDto { Value = GetBidConfigString(val), ID = item.ID };
                    items.Add(item);
                }
            }
            return new LookupListResultDto() { Items = items, TotalCount = totalCount };


        }
        public string GetBidConfigString(FloorPriceConfigType configType)
        {

            switch (configType)
            {

                case FloorPriceConfigType.ActionType:
                    return ResourceManager.Instance.GetResource("ActionType", "BidConfigType");
                case FloorPriceConfigType.Geographic:
                    return ResourceManager.Instance.GetResource("Geographic", "BidConfigType");

                case FloorPriceConfigType.Keyword:
                    return ResourceManager.Instance.GetResource("Keyword", "BidConfigType");

                case FloorPriceConfigType.Demographic:
                    return ResourceManager.Instance.GetResource("Demographic", "BidConfigType");

                case FloorPriceConfigType.Platform:
                    return ResourceManager.Instance.GetResource("Platform", "BidConfigType");

                case FloorPriceConfigType.Manufacturer:
                    return ResourceManager.Instance.GetResource("Manufacturer", "BidConfigType");

                case FloorPriceConfigType.DeviceType:
                    return ResourceManager.Instance.GetResource("DeviceType", "BidConfigType");

                /*case FloorPriceConfigType.Undefined:
                    return ResourceManager.Instance.GetResource("Undefined", "BidConfigType");*/

                case FloorPriceConfigType.Operator:
                    return ResourceManager.Instance.GetResource("Operator", "BidConfigType");

                case FloorPriceConfigType.Base:
                    return ResourceManager.Instance.GetResource("Base", "BidConfigType");

                default:
                    return string.Empty;



            }

        }
        public void DeleteFloorPrice(int[] Ids)
        {



            if (Ids != null)
            {
                foreach (var item in Ids.Select(item => _floorPriceRepository.Get(item)))
                {

                    item.Delete();
                    _floorPriceRepository.Save(item);

                }
            }



        }

    }
}

