
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Creative;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Exceptions.Campaign;
using ArabyAds.AdFalcon.Persistence.Reports.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.Resources;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Utilities;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Domain;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.UserInfo;
using ArabyAds.AdFalcon.Domain.Repositories.Core.Video;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Persistence.ReportsGP.CardinalityEstimator;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
//using ArabyAds.AdFalcon.Persistence.ReportsGP.CardinalityEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ArabyAds.Framework.Persistence;
using NHibernate;
using System.Linq.Expressions;
using Newtonsoft.Json;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign;
using ArabyAds.AdFalcon.Common;
using ArabyAds.AdFalcon.Persistence.ReportsGP.Repositories;

namespace ArabyAds.AdFalcon.Services.Services.Campaign
{
    public class CampaignService : ICampaignService
    {

        private const string IMPRESSIONEVENT = "000imp";
        private const string CREATVIEWEVENT = "creatv";
        private readonly string nativeAdCraetiveUnitCode;
        private IBusinessPartnerRepository _BusinessPartnerRepository;
        private readonly ICampaignRepository CampaignRepository = null;
        private IKeyWordRepository keyWordRepository = null;
        private IAdvertiserRepository AdvertiserRepository = null;
        private IAccountTrackingEventsRepository AccountTrackingEventRepository = null;
        private IPlatformRepository platformRepository = null;
        private IOperatorRepository operatorRepository = null;
        private ITargetingTypeRepository targetingTypeRepository = null;
        private ILocationRepository locationRepository = null;
        private IManufacturerRepository manufacturerRepository = null;
        private IDeviceRepository deviceRepository = null;
        private IAdActionTypeRepository adActionTypeRepository = null;
        private IAdGroupObjectiveTypeRepository adGroupObjectiveTypeRepository = null;
        private IBidManager bidManager = null;
        private IAgeGroupRepository ageGroupRepository;
        private ISubAppsiteRepository _subAppsiteRepository;
        private IGenderRepository genderRepository;
        private IDocumentRepository documentRepository;
        private ICreativeUnitRepository creativeUnitRepository;
        private ITileImageRepository tileImageRepository;
        private ITileImageSizeRepository tileImageSizeRepository;
        private ISummaryRepository summaryRepository;
        private IAccountRepository accountRepository;
        private IDeviceTargetingTypeRepository deviceTargetingTypeRepository;
        private IAdRepository adRepository;
        private Framework.ConfigurationSetting.IConfigurationManager configurationManager;
        private IAppSiteRepository appSiteRepository = null;
        private IDeviceCapabilityRepository deviceCapabilityRepository = null;
        private ICampaignPerformanceRepository campaignPerformanceRepository;
        private IPartyRepository partyRepository;
        private ICostElementRepository costElementRepository;
        private IDeviceTypeRepository deviceTypeRepository;
        private IAdTypeRepository adTypeRepository;
        private ICostModelWrapperRepository costModelWrapperRepository;
        private ITrackingEventRepository trackingEventRepository;
        private IAdGroupTrackingEventRepository adGroupTrackingEventRepository;
        private IMIMETypeRepository mimeTypeRepository;
        private IAdActionTypeTrackingEventRepository adActionTypeTrackingEvent;
        private IInStreamVideoCreativeUnitRepository inStreamVideoCreativeUnitRepository;
        private IRichMediaRequiredProtocolRepository requiredProtocolRepository = null;
        private IAppMarketingPartnerRepository appMarketingPartnerRepository = null;
        private readonly IAdSupportedCreativeUnitRepository adSupportedCreativeUnitRepository = null;
        private IVideoDeliveryMethodRepository videoDeliveryMethodRepository;
        private IVideoTypeRepository videoTypeRepository;
        private IAdRequestTypePlatformVersionRepository _AdRequestTypePlatformVersionRepository;
        private IAdGroupRepository adGroupRepository;
        private IAdCreativeRepository adCreativeRepository;
        private IAdRequestTargetingRepository _AdRequestTargetingRepository;
        private IAppSiteAdQueueRepository _AppSiteAdQueueRepository;
        private IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository;
        private IAdTypeRepository _AdTypeRepository;
        private IReportSchedulerRepository reportSchedulerRepository;
        private ISSPPartnerRepository _SSPPartnerRepository;
        private IPMPDealRepository PMPDealRepository;
        private IImpressionMetricRepository _IImpressionMetricRepository;
        private IImpressionMetricTargetingRepository _ImpressionMetricTargetingRepository;
        private IAdCreativeUnitVendorRepository _AdCreativeUnitVendorRepository;
        private ICreativeVendorKeywordRepository _CreativeVendorKeywordRepository;
        private IDocumentTypeRepository _DocumentTypeRepository;
        private ICreativeVendorRepository _CreativeVedorRepository;
        private IPlacementTypeRepository _PlacementTypeRepository;
        private IPlaybackMethodsRepository _PlaybackMethodsRepository;
        private IInStreamPositionRepository _InStreamPositionRepository;
        private ISkippableAdsRepository _SkippableAdsRepository;
        private IVideoMediaFileRepository _VideoMediaFileRepository;
        private IInStreamVideoCreativeRepository _InStreamVideoCreativeRepository;
        private IVideoConversionCreativeUnitRepository _VideoConversionCreativeUnitRepository;
        private ICreativeUnitRepository _CreativeUnitRepository;
        private IProtocolRepository _ProtocolRepository;
        private IMetricVendorRepository _metricVendorRepository;
        private IAdvertiserAccountRepository AdvertiserAccountRepository;
        private IAdvertiserAccountUserRepository _advertiserAccountUserRepository;
        private IDPPartnerRepository _DPPartnerRepository;
        private IAdvertiserAccountMasterAppSiteRepository _AdvertiserAccountMasterAppSiteRepository;
        private IFeeRepository _FeeRepository;
        private IAdGroupDynamicBiddingConfigRepository _AdGroupDynamicBiddingConfigRepository;
        private readonly IAudienceSegmentRepository audianSegRep = null;
        private IAudienceSegmentOccupationRepository _AudienceSegmentOccupationRepository;
        //private IAdSupportedCreativeUnitRepository _adSupportedCreativeUnitRepository;
        private IAdGroupConversionEventRepository _adGroupConversionEventRepository;
        private ICampaignFrequencyCappingRepository _campaignFrequencyCappingRepository;
        private IAdGroupEventRepository _adGroupEventRepository;
        private IAdvertiserAccountUserRepository  _AdvertiserAccountReadOnlyUserRepository;
        private ICampaignTroubleshootingRepository _campaignTroubleshootingRepository;


        private IAdGroupBidModifierRepository _AdGroupBidModifierRepository;
        private ICampaignBidModifierRepository _CampaignBidModifierRepository;
        private IHouseAdRepository houseAdRepository;
        private IContextualSegmentRepository _contextualSegmentRepository;
        //private IAdCreativeUnitVendorRepository AdCreativeUnitVendorRepository = null;
        public CampaignService(IAdGroupBidModifierRepository AdGroupBidModifierRepository, ICampaignBidModifierRepository CampaignBidModifierRepository, IAdGroupEventRepository adGroupEventRepository,  ICampaignFrequencyCappingRepository CampaignFrequencyCappingRepository, ICampaignRepository campaignRepository,
            IAccountPortalPermissionsRepository AccountPortalPermissionsRepository,
        IPlatformRepository platformRepository,
        IAdTypeRepository AdTypeRepository,
                                IOperatorRepository operatorRepository,
                                ITargetingTypeRepository targetingTypeRepository,
                                ILocationRepository locationRepository,
                                IManufacturerRepository manufacturerRepository,
                                IKeyWordRepository keyWordRepository,
                                IAdActionTypeRepository adActionTypeRepository,
                                IAdGroupObjectiveTypeRepository adGroupObjectiveTypeRepository,
                                IBidManager bidManager,
                                IDeviceRepository deviceRepository,
                                IAgeGroupRepository ageGroupRepository,
                                IGenderRepository genderRepository,
                                ICreativeUnitRepository creativeUnitRepository,
                                IDocumentRepository documentRepository,
                                ITileImageRepository tileImageRepository,
                                ITileImageSizeRepository tileImageSizeRepository,
                                ISummaryRepository summaryRepository,
                                IAccountRepository accountRepository,
                                IDeviceTargetingTypeRepository deviceTargetingTypeRepository,
                                IAdRepository adRepository,
                                IConfigurationManager configurationManager,
                                IAppSiteRepository appSiteRepository,
                                IDeviceCapabilityRepository deviceCapabilityRepository,
                                ICampaignPerformanceRepository campaignPerformanceRepository,
                                IPartyRepository partyRepository,
                                ICostElementRepository costElementRepository,
                                IRichMediaRequiredProtocolRepository requiredProtocolRepository,
                                IAdSupportedCreativeUnitRepository adSupportedCreativeUnitRepository,
                                IDeviceTypeRepository deviceTypeRepository,
                                IAdTypeRepository adTypeRepository,
                                ICostModelWrapperRepository costModelWrapperRepository,
                                ITrackingEventRepository trackingEventRepository,
                                IAdGroupTrackingEventRepository adGroupTrackingEventRepository,

                                           IAdGroupConversionEventRepository adGroupConversionEventRepository,
                                IMIMETypeRepository mimeTypeRepository,
                                 IAdActionTypeTrackingEventRepository adActionTypeTrackingEvent,
            IInStreamVideoCreativeUnitRepository inStreamVideoCreativeUnitRepository,
          IAccountTrackingEventsRepository AccountTrackingEventRepository,
            IVideoDeliveryMethodRepository videoDeliveryMethodRepository,
             IVideoTypeRepository videoTypeRepository, IAdRequestTypePlatformVersionRepository AdRequestTypePlatformVersionRepository,

             IAdGroupRepository adGroupRepository, IAppMarketingPartnerRepository appMarketingPartnerRepository, IAdCreativeRepository adCreativeRepository, IAdRequestTargetingRepository Targeting, IReportSchedulerRepository reportSchedulerRepository, IAdvertiserRepository advertiserRepository, IAppSiteAdQueueRepository AppSiteAdQueueRepository, IPMPDealRepository dealRep, ISSPPartnerRepository SSPPartnerRepository, ISubAppsiteRepository subAppsiteRepository, IBusinessPartnerRepository businessPartnerRepository, IImpressionMetricTargetingRepository ImpressionMetricTargetingRepository, IImpressionMetricRepository IImpressionMetricRepository, IAdCreativeUnitVendorRepository AdCreativeUnitVendorRepository, IAdCreativeUnitVendorRepository adCreativeUnitVendorRepository, ICreativeVendorKeywordRepository CreativeVendorKeywordRepository, ICreativeVendorRepository CreativeVedorRepository,


             IDocumentTypeRepository _DocumentTypeRepository,
            IPlacementTypeRepository PlacementTypeRepository,
IPlaybackMethodsRepository PlaybackMethodsRepository,
IInStreamPositionRepository InStreamPositionRepository,
ISkippableAdsRepository SkippableAdsRepository,
IVideoMediaFileRepository VideoMediaFileRepository,

IInStreamVideoCreativeRepository InStreamVideoCreativeRepository,

ICreativeUnitRepository CreativeUnitRepository,

IVideoConversionCreativeUnitRepository VideoConversionCreativeUnitRepository,
IProtocolRepository ProtocolRepository,
IMetricVendorRepository MetricVendorRepository,
IAdvertiserAccountRepository AdvertiserAccountRepository,

IAdvertiserAccountUserRepository advertiserAccountUserRepository, IAudienceSegmentRepository sugAudianceRe, IDPPartnerRepository PPartnerRepository,
IAdvertiserAccountMasterAppSiteRepository AdvertiserAccountMasterAppSiteRepository,
IFeeRepository FeeRepository,
IAdGroupDynamicBiddingConfigRepository AdGroupDynamicBiddingConfigRepository,
IAudienceSegmentOccupationRepository AudienceSegmentOccupationRepository,
IAdvertiserAccountUserRepository AdvertiserAccountReadOnlyUserRepository
          ,  IHouseAdRepository houseAdRepositoryas, ICampaignTroubleshootingRepository campaignTroubleshootingRepository
           ,IContextualSegmentRepository ContextualSegmentRepository )

        {
            this._AdGroupBidModifierRepository = AdGroupBidModifierRepository;
            this._CampaignBidModifierRepository= CampaignBidModifierRepository;
            this._AdvertiserAccountReadOnlyUserRepository = AdvertiserAccountReadOnlyUserRepository;
            this._adGroupEventRepository = adGroupEventRepository;
            this._campaignFrequencyCappingRepository = CampaignFrequencyCappingRepository;
            this._adGroupConversionEventRepository = adGroupConversionEventRepository;
            this._AdGroupDynamicBiddingConfigRepository = AdGroupDynamicBiddingConfigRepository;
            this.audianSegRep = sugAudianceRe;
            this._InStreamVideoCreativeRepository = InStreamVideoCreativeRepository;
            this._PlacementTypeRepository = PlacementTypeRepository;
            this._PlaybackMethodsRepository = PlaybackMethodsRepository;
            this._InStreamPositionRepository = InStreamPositionRepository;
            this._SkippableAdsRepository = SkippableAdsRepository;
            this._advertiserAccountUserRepository = advertiserAccountUserRepository;

            this.PMPDealRepository = dealRep;
            _AccountPortalPermissionsRepository = AccountPortalPermissionsRepository;
            this.CampaignRepository = campaignRepository;
            this._AdRequestTypePlatformVersionRepository = AdRequestTypePlatformVersionRepository;
            _AdTypeRepository = AdTypeRepository;
            this.platformRepository = platformRepository;
            this.operatorRepository = operatorRepository;
            this.targetingTypeRepository = targetingTypeRepository;
            this.locationRepository = locationRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.keyWordRepository = keyWordRepository;
            this.AccountTrackingEventRepository = AccountTrackingEventRepository;
            this.adActionTypeRepository = adActionTypeRepository;
            this.adGroupObjectiveTypeRepository = adGroupObjectiveTypeRepository;
            this.bidManager = bidManager;
            this.deviceRepository = deviceRepository;
            this.ageGroupRepository = ageGroupRepository;
            this.genderRepository = genderRepository;
            this.creativeUnitRepository = creativeUnitRepository;
            this.documentRepository = documentRepository;
            this.tileImageRepository = tileImageRepository;
            this.tileImageSizeRepository = tileImageSizeRepository;
            this.summaryRepository = summaryRepository;
            this.accountRepository = accountRepository;
            this.deviceTargetingTypeRepository = deviceTargetingTypeRepository;
            this.adRepository = adRepository;
            this.configurationManager = configurationManager;
            this.appSiteRepository = appSiteRepository;
            this.deviceCapabilityRepository = deviceCapabilityRepository;
            this.campaignPerformanceRepository = campaignPerformanceRepository;
            this.partyRepository = partyRepository;
            this.costElementRepository = costElementRepository;
            this.requiredProtocolRepository = requiredProtocolRepository;
            this.adSupportedCreativeUnitRepository = adSupportedCreativeUnitRepository;
            this.deviceTypeRepository = deviceTypeRepository;
            this.adTypeRepository = adTypeRepository;
            this.costModelWrapperRepository = costModelWrapperRepository;
            this.trackingEventRepository = trackingEventRepository;
            this.adGroupTrackingEventRepository = adGroupTrackingEventRepository;
            this.mimeTypeRepository = mimeTypeRepository;
            this.adActionTypeTrackingEvent = adActionTypeTrackingEvent;
            nativeAdCraetiveUnitCode = Configuration.NativeAdCreativeUnitCode;
            this.inStreamVideoCreativeUnitRepository = inStreamVideoCreativeUnitRepository;
            this.videoDeliveryMethodRepository = videoDeliveryMethodRepository;
            this.videoTypeRepository = videoTypeRepository;
            this.adGroupRepository = adGroupRepository;
            this.appMarketingPartnerRepository = appMarketingPartnerRepository;
            this.adCreativeRepository = adCreativeRepository;
            this._AdRequestTargetingRepository = Targeting;
            this.reportSchedulerRepository = reportSchedulerRepository;
            this.AdvertiserRepository = advertiserRepository;
            this._AppSiteAdQueueRepository = AppSiteAdQueueRepository;
            this._DocumentTypeRepository = _DocumentTypeRepository;
            this._SSPPartnerRepository = SSPPartnerRepository;
            this._subAppsiteRepository = subAppsiteRepository;
            _BusinessPartnerRepository = businessPartnerRepository;
            _IImpressionMetricRepository = IImpressionMetricRepository;
            _ImpressionMetricTargetingRepository = ImpressionMetricTargetingRepository;
            _AdCreativeUnitVendorRepository = adCreativeUnitVendorRepository;
            _CreativeVendorKeywordRepository = CreativeVendorKeywordRepository;
            _CreativeVedorRepository = CreativeVedorRepository;
            _VideoMediaFileRepository = VideoMediaFileRepository;
            _CreativeUnitRepository = CreativeUnitRepository;
            this._VideoConversionCreativeUnitRepository = VideoConversionCreativeUnitRepository;
            this._ProtocolRepository = ProtocolRepository;
            this._metricVendorRepository = MetricVendorRepository;
            this.AdvertiserAccountRepository = AdvertiserAccountRepository;
            this._DPPartnerRepository = PPartnerRepository;
            this._AdvertiserAccountMasterAppSiteRepository = AdvertiserAccountMasterAppSiteRepository;

            this._FeeRepository = FeeRepository;
            this._campaignTroubleshootingRepository = campaignTroubleshootingRepository;

            this._AudienceSegmentOccupationRepository = AudienceSegmentOccupationRepository;
            //AdCreativeUnitVendorRepository = adCreativeUnitVendorRepository;
            this.houseAdRepository = houseAdRepositoryas;
            this._contextualSegmentRepository = ContextualSegmentRepository;
        }
        private void CheckCampaign(Domain.Model.Campaign.Campaign item)
        {
            if (item == null)
            {
                throw new DataNotFoundException();
            }
        }

        private IEnumerable<CreativeUnitDto> GetCreativeUnitBy(DeviceTypeEnum deviceType, AdTypeIds? adType, AdSubTypes? adSubType, string group)
        {
            var result = new List<CreativeUnitDto>();
            var creativeUnits = adSupportedCreativeUnitRepository.Query(x => (deviceType == (int)DeviceTypeEnum.Any || x.CreativeUnit.DeviceType.ID == (int)deviceType) &&
                      (!adType.HasValue || x.AdType == null || x.AdType.ID == (int)adType.Value) &&
                      (!adSubType.HasValue || !x.AdSubType.HasValue || x.AdSubType.Value == adSubType.Value) &&
                      (string.IsNullOrEmpty(group) || x.CreativeUnit.Groups.Any(p => p.Code == group)))
                      .Select(x => new { x.CreativeUnit, x.EnvironmentType, x.RequiredType, x.OrientationReplacement, x.ID }).ToList();

            foreach (var creativeUnit in creativeUnits)
            {
                var creativeUnitDto = MapperHelper.Map<CreativeUnitDto>(creativeUnit.CreativeUnit);
                creativeUnitDto.EnvironmentType = creativeUnit.EnvironmentType;
                creativeUnitDto.RequiredType = (int)creativeUnit.RequiredType;
                creativeUnitDto.AdSupportedId = creativeUnit.ID;
                creativeUnitDto.OrientationReplacementId = creativeUnit.OrientationReplacement == null ? null : new int?(creativeUnit.OrientationReplacement.ID);
                result.Add(creativeUnitDto);
            }
            return result;
        }
        #region Campaign
        private void ValidateAdvertiser(ArabyAds.AdFalcon.Domain.Model.Campaign.AdvertiserAccount advertiserAccount, bool statusCheck = false, bool validateDates = false)
        {
            bool isManager = IsCampaignManager();

            advertiserAccount.Validate(!isManager, statusCheck);
        }
        private void ValidateAdvertiser(int advertiseraccountId, bool statusCheck = false, bool validateDates = false)
        {
            AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.ID == advertiseraccountId && x.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();

            if (advertiserAccount == null)
            {
                throw new AccountNotValidException();

            }
            bool isManager = IsCampaignManager();
            advertiserAccount.Validate(!isManager, statusCheck);
        }

        public ValueMessageWrapper<int> GetAccountAdvertiserId(ValueMessageWrapper<int> advertiserId)
        {

            AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.Advertiser.ID == advertiserId.Value && x.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();

            return ValueMessageWrapper.Create(advertiserAccount.ID);

        }
        public CampaignListResultDto QueryByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CampaignCriteria wcriteria)
        {
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new CampaignListResultDto();

            if (criteria.AdvertiserAccountId.HasValue)
            {

                AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.ID == criteria.AdvertiserAccountId.Value && x.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();
                if (advertiserAccount != null)
                {
                    ValidateAdvertiser(criteria.AdvertiserAccountId.Value);

                    result.AdvertiserId = advertiserAccount.Advertiser.ID;
                    result.AdvertiserName = advertiserAccount.Advertiser.Name.Value;


                    result.AdvertiserAccountId = advertiserAccount.ID;
                    result.AdvertiserAccountName = advertiserAccount.Name;
                }
                //else if (Domain.Configuration.IsAdmin)
                //{
                //    //   Advertiser advertiser = AdvertiserRepository.Query(x => x.ID == criteria.AdvertiserId.Value).FirstOrDefault();
                //    // AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.ID == criteria.AdvertiserAccountId.Value && x.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();

                //    Advertiser advertiser = advertiserAccount.Advertiser;
                //    if (advertiser == null)
                //    {
                //        throw new DataNotFoundException();
                //    }

                //    bool isLinked = AdvertiserAccountRepository.Query(x => x.Advertiser.ID == advertiser.ID).Count() > 0;
                //    if (!isLinked)
                //    {

                //        throw new DataNotFoundException();
                //    }

                //    result.AdvertiserId = advertiser.ID;
                //    result.AdvertiserName = advertiser.Name.Value;

                //    result.AdvertiserAccountId = advertiserAccount.ID;
                //    result.AdvertiserAccountName = advertiserAccount.Name;
                //}
                else
                {
                    throw new DataNotFoundException();

                }


            }

            IEnumerable<Domain.Model.Campaign.Campaign> list = null;
            if (criteria.Name == null)
            {
                criteria.Name = string.Empty;
            }
            if (criteria.Page.HasValue)
            {
                list = CampaignRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, false);
            }
            else
            {
                var list2 = CampaignRepository.Query(criteria.GetExpression()).OrderByDescending(x => x.ID);
                var list3 = new List<Domain.Model.Campaign.Campaign>();

                foreach (var item in list2)
                {
                    if (!IsllowedAdvertiserForCamp(item))
                        continue;
                    list3.Add(item);

                }
                list = list3.ToList();
            }
            var returnList = list.Select(campaign => MapperHelper.Map<CampaignListDto>(campaign)).ToList();

            #region Performance

            var performance = new PerformanceCriteria
            {
                FromDate = criteria.DataFrom,
                ToDate = criteria.DataTo,
                Ids = returnList.Select(obj => obj.Id).ToList()
            };
            var performances = summaryRepository.GetCampaignsPerformance(performance);
            var idStatus = summaryRepository.GetAdsByCampaign(performance);
            List<CampaignCardinalityEstimatorDto> res = new List<CampaignCardinalityEstimatorDto>();
            if (criteria.Page.HasValue)
            {
                DateTime from = criteria.DataFrom == null ? new DateTime(2009, 1, 1) : (DateTime)criteria.DataFrom;
                DateTime to = criteria.DataTo == null ? new DateTime(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month, DateTime.DaysInMonth(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month)) : (DateTime)criteria.DataTo;
                AdvertisorEstimatorCalculation AdvertisorEstimatorCalculationtemp = new AdvertisorEstimatorCalculation(from, to, EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.Campaign, Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value);
                AdvertisorEstimatorCalculationtemp.IdPerRange = new Dictionary<int, DateEstimatorRange>();
                string Idtemps = string.Empty;
                DateTime fromInti = DateTime.MaxValue;
                DateTime toInti = DateTime.MinValue;

                if (criteria.DataFrom == null)
                {
                    foreach (var resultCamp in list)
                    {

                        if (!resultCamp.EndDate.HasValue)
                        {
                            var firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                            var lastDayOfMonth = new DateTime(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month, DateTime.DaysInMonth(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month));
                            AdvertisorEstimatorCalculationtemp.IdPerRange.Add(resultCamp.ID, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });



                        }
                        else
                        {

                            if (resultCamp.EndDate.Value.Date <= to.Date)
                            {
                                var firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                                var lastDayOfMonth = new DateTime(resultCamp.EndDate.Value.Year, resultCamp.EndDate.Value.Month, DateTime.DaysInMonth(resultCamp.EndDate.Value.Year, resultCamp.EndDate.Value.Month));
                                AdvertisorEstimatorCalculationtemp.IdPerRange.Add(resultCamp.ID, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });


                            }
                            else
                            {
                                var firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                                var lastDayOfMonth = to.Date;
                                AdvertisorEstimatorCalculationtemp.IdPerRange.Add(resultCamp.ID, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });


                            }
                        }

                    }

                    Idtemps = string.Join(",", list.Select(x => x.ID).ToList());
                }
                else
                {
                    foreach (var resultCamp in list)
                    {
                        if (!resultCamp.EndDate.HasValue)
                        {
                            var lastDayOfMonth = to.Date;
                            var firstDayOfMonth = from.Date;
                            if (resultCamp.StartDate.Date >= from.Date)
                            {
                                firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                            }
                            else if (Framework.Utilities.Environment.GetServerDate().Date <= to.Date)
                            {
                                lastDayOfMonth = new DateTime(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month, DateTime.DaysInMonth(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month));

                            }

                            AdvertisorEstimatorCalculationtemp.IdPerRange.Add(resultCamp.ID, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });

                        }
                        else
                        {
                            var lastDayOfMonth = to.Date;
                            var firstDayOfMonth = from.Date;
                            if (resultCamp.StartDate.Date >= from.Date)
                            {
                                firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                            }
                            else if (resultCamp.EndDate.Value.Date <= to.Date)
                            {
                                lastDayOfMonth = new DateTime(resultCamp.EndDate.Value.Year, resultCamp.EndDate.Value.Month, DateTime.DaysInMonth(resultCamp.EndDate.Value.Year, resultCamp.EndDate.Value.Month));

                            }
                            AdvertisorEstimatorCalculationtemp.IdPerRange.Add(resultCamp.ID, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });



                        }

                    }
                    Idtemps = string.Join(",", list.Select(x => x.ID).ToList());

                    // AdvertisorEstimatorCalculation AdvertisorEstimatorCalculation = new AdvertisorEstimatorCalculation(from, to, EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.Campaign);
                    //string Ids = string.Join(",", returnList.Select(x => x.Id).ToList());


                }
                IList<DateTime> datesFrom = new List<DateTime>();
                IList<DateTime> datesTo = new List<DateTime>();
                foreach (var key in AdvertisorEstimatorCalculationtemp.IdPerRange)
                {

                    datesFrom.Add(key.Value.DateFrom);
                    datesTo.Add(key.Value.DateTo);
                }
                datesFrom = datesFrom.OrderBy(M => M).ToList();
                datesTo = datesTo.OrderByDescending(M => M).ToList();

                if (datesTo != null && datesTo.Count > 0)
                    AdvertisorEstimatorCalculationtemp.DateTo = datesTo[0];
                if (datesFrom != null && datesFrom.Count > 0)
                    AdvertisorEstimatorCalculationtemp.DateFrom = datesFrom[0];
                if (!string.IsNullOrEmpty(Idtemps))
                    res = AdvertisorEstimatorCalculationtemp.GetCardinalityEsitimator(Idtemps).ToList();

            }

            foreach (var campaignListDto in returnList)
            {
                campaignListDto.Performance = performances.FirstOrDefault(item => item.DimCampaignID == campaignListDto.Id);

                if (criteria.Page.HasValue && res != null && res.Count() > 0)
                {
                    var resItem = res.FirstOrDefault(M => M.CampaignId == campaignListDto.Id);

                    if (resItem != null)
                    {
                        campaignListDto.Performance.UniqueClicks = resItem.unique_clicks;

                        campaignListDto.Performance.UniqueImp = resItem.unique_impressions;
                    }
                }

                //load Ad Status
                var camAds = idStatus.Where(ad => ad.CampaignId == campaignListDto.Id).ToList();
                campaignListDto.Status = summaryRepository.CalculateStatus(camAds);
            }

            #endregion


            if (!criteria.ActiveCampaigns)
            {
                result.Items = returnList;
                result.TotalCount = CampaignRepository.Query(criteria.GetExpression()).Count();

                if (criteria.AdvertiserAccountId.HasValue)
                {
                    AdvertiserAccount advertiserAccount = AdvertiserAccountRepository.Query(x => x.ID == criteria.AdvertiserAccountId.Value && x.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).FirstOrDefault();
                    PerformanceCriteriaBase PerformanceCriteriaBase = new PerformanceCriteriaBase
                    {
                        Ids = new List<int>() { advertiserAccount.ID }
                    };
                    result.Performance = summaryRepository.GetAdvertiserPerformance(PerformanceCriteriaBase);
                }
            }
            else
            {

                for (int i = 0; i < returnList.Count; i++)
                {
                    if (returnList[i].Status == AdGroupStatus.Empty.Name.ToString())
                    {
                        returnList.RemoveAt(i);
                        //if (i != 0)
                        i--;

                    }


                }
                result.Items = returnList;
            }
            return result;
        }
        public string GetAdvertiserString(ValueMessageWrapper<int> AdvertiserId)
        {

            return AdvertiserRepository.Get(AdvertiserId.Value).Name.ToString();
        }

        public string GetAdvertiserAccountString(ValueMessageWrapper<int> AdvertiserId)
        {

            return AdvertiserAccountRepository.Get(AdvertiserId.Value).Name.ToString();
        }


        public ValueMessageWrapper<bool> IsReadOrWriteCampaign(ValueMessageWrapper<int> CampaignId)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return ValueMessageWrapper.Create(true);

            var Campaign = CampaignRepository.Get(CampaignId.Value);
            int AdvertiserAccountId = Campaign.AdvertiserAccount.ID;

            int count = 0;
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {

                _AdvertiserAccountReadOnlyUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted).Count();
                return ValueMessageWrapper.Create(count > 0);
            }

            if (!Campaign.AdvertiserAccount.IsRestricted)
                return ValueMessageWrapper.Create(true);

             count = _advertiserAccountUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted && (x.Read || x.Write)).Count();

            return ValueMessageWrapper.Create(count > 0);
        }
        public ValueMessageWrapper<bool> IsWriteCampaign(ValueMessageWrapper<int> CampaignId)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return ValueMessageWrapper.Create(true);

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {
                return ValueMessageWrapper.Create(false);

            }

            var Campaign = CampaignRepository.Get(CampaignId.Value);
            int AdvertiserAccountId = Campaign.AdvertiserAccount.ID;
            if (!Campaign.AdvertiserAccount.IsRestricted)
                return ValueMessageWrapper.Create(true);

            int count = _advertiserAccountUserRepository.Query(x => x.Link.ID == AdvertiserAccountId && x.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId && !x.IsDeleted && x.Write).Count();
            return ValueMessageWrapper.Create(count > 0);
        }

        public ValueMessageWrapper<int> GetAdvertiserIdFromAccount(ValueMessageWrapper<int> AdvertiserId)
        {

            return ValueMessageWrapper.Create(AdvertiserAccountRepository.Get(AdvertiserId.Value).Advertiser.ID);
        }
        public ValueMessageWrapper<bool> Delete(IEnumerable<int> campaignIds)
        {
            if (campaignIds != null)
            {
                foreach (var item in campaignIds.Select(campaignId => CampaignRepository.Get(campaignId)))
                {
                    ValidateCampaign(item);
                    if (item.IsValid)
                    {
                        if (item.AdvertiserAccount != null)
                            ValidateAdvertiser(item.AdvertiserAccount.ID);
                        item.Delete();
                        CampaignRepository.Save(item);
                    }
                }
                return ValueMessageWrapper.Create(true);
            }
            else
            {
                return ValueMessageWrapper.Create(false);
            }
        }

        public List<AdGroupBidModifierDto> GetAdGroupBidModifiers(CampaignIdAdgroupIdMessage request)
        {
            var items = this._AdGroupBidModifierRepository.Query(M => M.Campaign.ID == request.CampaignId && M.AdGroup.ID == request.AdgroupId && M.IsDeleted == false);
            List<AdGroupBidModifierDto> mappeditems = items.Select(x => MapperHelper.Map<AdGroupBidModifierDto>(x)).ToList();


            return mappeditems;
        }

        public List<AdGroupBidModifierDto> GetCampBidModifiers(CampaignIdAdgroupIdMessage request)
        {
            var items = this._CampaignBidModifierRepository.Query(M => M.Campaign.ID==request.CampaignId && M.IsDeleted==false);
            List<AdGroupBidModifierDto> mappeditems = items.Where(M=>M.GetType()== typeof(CampaignBidModifier)).Select(x =>
            {
               var dto =  MapperHelper.Map<AdGroupBidModifierDto>(x);
                return dto;
            }).ToList();
      

            return mappeditems;
        }

        public void SaveCampBidModifiers(SaveBidModifierRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            if (request.AdGroupBidModifiersDto != null)
            {
                foreach (var adGroupMod in request.AdGroupBidModifiersDto)
                {
                    var adgroupModifier = MapperHelper.Map<CampaignBidModifier>(adGroupMod);
                    if (!adgroupModifier.IsDeleted && adgroupModifier.DimentionType != DimentionType.Any)
                        item.AddCampaignBidModifiers(adgroupModifier);
                    else
                        item.DeleteBidModifier(adgroupModifier.ID);
                }


            }
        }

        public CampaignSaveDto Save(CampaignDto campaign)
        {
            var result = new CampaignSaveDto();
            var item = CampaignRepository.Get(campaign.ID);
            campaign.EndDate = campaign.EndDate.HasValue ? new DateTime(campaign.EndDate.Value.Year, campaign.EndDate.Value.Month, campaign.EndDate.Value.Day, 23, 59, 59) : (DateTime?)null;
            var dateChnaged = false;
            bool enddatechanged = false;
            if (item != null)
            {
                //Update Old Item

                if (campaign.CampaignType != CampaignType.AdHouse)
                {
                    //check if the campaign is started 
                    if ((item.IsRuntime))
                    // && (item.StartDate.CompareTo(Framework.Utilities.Environment.GetServerTime().Date) <= 0))
                    {
                        //ad is served on this campaign
                        //you cant's change the start data after this
                        if (item.StartDate.Date != campaign.StartDate.Date)
                        {
                            //create business Exception to hold error data list 
                            var error = new BusinessException();
                            error.Errors.Add(new ErrorData { ID = "CampaignStartDateBR" });
                            throw error;
                        }
                    }
                }
                result.Warnings = item.GetWarnings(campaign.Budget, campaign.DailyBudget, campaign.EndDate);

                item.Name = campaign.Name;
                item.Note = campaign.Note;
                if (item.StartDate.Date != campaign.StartDate.Date)
                {
                    dateChnaged = true;
                }

                if (item.EndDate != campaign.EndDate || item.StartDate.Date != campaign.StartDate.Date)
                {
                    if (item.EndDate != campaign.EndDate)
                    {
                        enddatechanged = true;


                    }
                    var frquencyLists = item.CampaignServerSetting.GetFrequencyCappingList();

                    if (frquencyLists != null && frquencyLists.Count > 0)
                    {
                        foreach (var frquencyList in frquencyLists)
                        {
                            if (frquencyList.Interval >= (int)FrequencyCappingInterval.LifeTime)
                            {



                                // Buinsess of Capping Life Time
                                var noOfMonth = 3 * 30;

                                // if (item.LifeTime == CampaignLifeTime.Default)
                                //{
                                if (campaign.EndDate.HasValue)
                                {
                                    var DateDiff = campaign.EndDate.Value - campaign.StartDate;

                                    var month = DateDiff.TotalDays;

                                    if (month == 0)
                                    {
                                        month = 1;
                                    }
                                    noOfMonth = noOfMonth + (int)month;



                                }

                                //}

                                frquencyList.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;
                            }
                        }
                    }
                }


                item.StartDate = campaign.StartDate;





                item.EndDate = campaign.EndDate;
                item.StartTime = campaign.StartTime;
                item.EndTime = campaign.EndTime;
                item.Budget = campaign.Budget;
                item.DailyBudget = campaign.DailyBudget;
               /* 
                 
                 if (campaign.AdvertiserAccountId > 0)
                    item.AdvertiserAccount = new AdvertiserAccount { ID = campaign.AdvertiserAccountId };
                else
                    item.AdvertiserAccount = null;
                item.Advertiser = campaign.CampaignType == CampaignType.Normal || campaign.CampaignType == CampaignType.ProgrammaticGuaranteed ? AdvertiserRepository.Get(campaign.Advertiser.ID) : null;
                
                 
                 */

                ValidateCampaign(item, true, dateChnaged);


            }
            else
            {
                //New Item
                item = MapperHelper.Map<Domain.Model.Campaign.Campaign>(campaign);
                item.PriceMode = PriceMode.Fixed;
                item.LifeTime = CampaignLifeTime.Default;
                if (campaign.Advertiser != null)
                    item.Advertiser = AdvertiserRepository.Get(campaign.Advertiser.ID);
                if (campaign.AdvertiserAccountId > 0)
                    item.AdvertiserAccount = AdvertiserAccountRepository.Get(campaign.AdvertiserAccountId);
                else
                    item.AdvertiserAccount = null;
                item.CreationDate = Framework.Utilities.Environment.GetServerTime();
                item.UniqueId = Guid.NewGuid().ToString();

                item.AddDefaultCampaignServerSetting();
                if (item.AdvertiserAccount != null)
                { item.CampaignServerSetting.AgencyCommission = item.AdvertiserAccount.AgencyCommission;
                    item.CampaignServerSetting.AgencyCommissionValue = item.AdvertiserAccount.AgencyCommissionValue;


                }
                //item.Status = AdCampaignStatus.Empty;
                //Change Account 
                if (item.Account == null)
                {
                    item.ChangeAccount(
                        accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value));
                }

                if (item.User == null)
                {
                    item.ChangeUser(new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });


                }
                result.Warnings = item.GetWarnings(campaign.Budget, campaign.DailyBudget, campaign.EndDate, true);
                item.ValidateNew();

            }
            if (campaign.AdGroupBidModifiersDto!=null)
            { 
                foreach (var adGroupMod in campaign.AdGroupBidModifiersDto)
                {
                    var adgroupModifier = MapperHelper.Map<CampaignBidModifier>(adGroupMod);
                   if(!adgroupModifier.IsDeleted && adgroupModifier.DimentionType != DimentionType.Any)
                    item.AddCampaignBidModifiers(adgroupModifier);
                   else
                        item.DeleteBidModifier(adgroupModifier.ID);
                }
            
            
            }
            if (item.IsValid)
            {
                if (item.AdvertiserAccount != null)
                    ValidateAdvertiser(item.AdvertiserAccount.ID);
                CampaignRepository.Save(item);
            }
            result.ID = item.ID;
            List<int> SegmentsIdRunning = new List<int>();
            if (enddatechanged)
            {
                if (item.Status == AdCampaignStatus.RunningWithAttentionActionNeeded || item.Status == AdCampaignStatus.Running)
                {
                    var tempres = item.GetAudienceSegmentsForExternal();
                    if (tempres != null && tempres.Count > 0)
                        SegmentsIdRunning.AddRange(tempres);

                    if (SegmentsIdRunning.Count>0)
                    {


                        AudienceSegmentTargeting tar = new AudienceSegmentTargeting();
                                tar.PublishkafkaforCheck(SegmentsIdRunning, item.ID, "CampaignEndDateUpdateSegment");
                       


                    }
                }


            }


            return result;
        }


        public ValueMessageWrapper<int> GetCampaignAdvertiser(ValueMessageWrapper<int> campaignId)
        {
            var item = CampaignRepository.Get(campaignId.Value);
            if (item.Advertiser != null)
            {
                return ValueMessageWrapper.Create(item.Advertiser.ID);

            }
            return ValueMessageWrapper.Create(0);
        }

        public ValueMessageWrapper<int> GetCampaignAdvertiserAccount(ValueMessageWrapper<int> campaignId)
        {
            var item = CampaignRepository.Get(campaignId.Value);
            if (item.AdvertiserAccount != null)
            {
                return ValueMessageWrapper.Create( item.AdvertiserAccount.ID);

            }
            return ValueMessageWrapper.Create(0);
        }
        public CampaignDto Get(GetCampaignRequest request)
        {
            //Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase("CampaignStarted", campaignId.ToString(), null,null));

            //Framework.EventBroker.EventBroker.Instance.Flush();

            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            if (item.CampaignType != request.Type && item.CampaignType != request.Othertype)
            {
                throw new DataNotFoundException();
            }
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var campaign = MapperHelper.Map<CampaignDto>(item);
                campaign.HasObjective = item.HasGroups();
                var val = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                if (item.User != null && item.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value)
                {
                    campaign.SupUserName = string.Empty;
                }
                if (item.Advertiser != null)
                {
                    campaign.Advertiser = MapperHelper.Map<AdvertiserDto>(item.Advertiser);

                }
                if (item.AdvertiserAccount != null)
                {
                    campaign.AdvertiserAccountName = item.AdvertiserAccount.Name;
                    campaign.AdvertiserAccountId = item.AdvertiserAccount.ID;
                }
                return campaign;
            }
            return null;
        }
        public CampaignDto GetCampInfo(GetCampaignRequest request)
        {
            
            var item = CampaignRepository.Get(request.CampaignId);
         
                var campaign = MapperHelper.Map<CampaignDto>(item);
                var val = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                if (item.User != null && item.User.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value)
                {
                    campaign.SupUserName = string.Empty;
                }
                /*if (item.Advertiser != null)
                {
                    campaign.Advertiser = MapperHelper.Map<AdvertiserDto>(item.Advertiser);

                }*/
                if (item.AdvertiserAccount != null)
                {
                    campaign.AdvertiserAccountName = item.AdvertiserAccount.Name;
                    campaign.AdvertiserAccountId = item.AdvertiserAccount.ID;
                }
                return campaign;
            
            //return null;
        }

        public List<CampaignTroubleshootingDto> GetCampaignTroubleshootingDetails(CampaignTroubleshootingCriteria criteria)
        {
            return _campaignTroubleshootingRepository.GetResult(criteria);
        }

        public AdGroupDto GetAdGroupInfo(CampaignIdAdgroupIdMessage request)
        {

            var item = adGroupRepository.Get(request.AdgroupId);

            var adgroupdto = MapperHelper.Map<AdGroupDto>(item);
            if((int)adgroupdto.ActionTypeId>0)
           adgroupdto.AdActionTypeCode= adActionTypeRepository.Get((int)adgroupdto.ActionTypeId).Code;
            if (item.Objective.AdType != null && item.Objective.AdType.ID == (int)AdTypeIds.NativeAd)
                adgroupdto.AdActionTypeCode = 24;
                
            
            return adgroupdto;

            //return null;
        }

        public IList<DropDownDto> GetCampaignAdGroups(ValueMessageWrapper<int> campaignId)
        {
            var adGroups = adGroupRepository.GetAdGroupsByCampaign(campaignId.Value).ToList();
            return adGroups.Select(adGroup => new DropDownDto { Id = adGroup.ID, Name = adGroup.Name }).ToList();
        }

        public CampaignSettingsDto GetSettings(ValueMessageWrapper<int> campaignId)
        {
            var item = CampaignRepository.Get(campaignId.Value);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var settingsDto = MapperHelper.Map<CampaignSettingsDto>(item);
                if (settingsDto.Discount != null)
                {
                    var discount = item.GetActiveDiscount();
                    if (discount != null)
                    {
                        if (discount.Type == DiscountType.Percentage)
                        {
                            discount.Value = discount.Value * 100;
                        }
                        settingsDto.Discount = MapperHelper.Map<DiscountDto>(discount);
                    }
                }

                settingsDto.HasObjective = item.HasGroups();
                if (item.CampaignType==CampaignType.ProgrammaticGuaranteed)
                {
                    settingsDto.IsProgrammaticGuaranteed = true;
                }
                settingsDto.LogAdMarkup = item.LogAdMarkup;

                if (ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>
 ().AccountRole == (int)ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP)
                {
                    item.PriceMode = PriceMode.Dynamic;
                }
                else
                {
                    settingsDto.PriceMode = item.PriceMode;
                }
                settingsDto.AgencyCommission = item.AgencyCommission;
                settingsDto.PacingPoliciesValue = item.GetPacingValue();
                settingsDto.DomainURL = item.DomainURL;

                settingsDto.LifeTime = item.LifeTime;
                if (item.Keyword != null)
                {
                    settingsDto.Keyword = MapperHelper.Map<KeywordDto>(item.Keyword);
                }
                if (item.Advertiser != null)
                {
                    settingsDto.Advertiser = MapperHelper.Map<AdvertiserDto>(item.Advertiser);
                }
                return settingsDto;
            }
            return null;
        }

        public ValueMessageWrapper<bool> SaveSettings(CampaignSettingsDto settingsDto)
        {
            var item = CampaignRepository.Get(settingsDto.ID);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                // Save Basic Settings
                item.IsClientLocked = settingsDto.IsClientLocked;
                if (settingsDto.IsProgrammaticGuaranteed)
                {
                    item.CampaignType = CampaignType.ProgrammaticGuaranteed;
                }
                item.CostModelWrapper = settingsDto.CostModelWrapper.HasValue ? (CostModelWrapperEnum)settingsDto.CostModelWrapper : (CostModelWrapperEnum?)null;
                item.DomainURL = settingsDto.DomainURL;
                item.Keyword = keyWordRepository.Get(settingsDto.Keyword.ID);
               // item.Advertiser = AdvertiserRepository.Get(settingsDto.Advertiser.ID);
                item.CPMValue = settingsDto.CPMValue;
                item.LogAdMarkup = settingsDto.LogAdMarkup;
                item.PriceMode = settingsDto.PriceMode;

                item.LifeTime = settingsDto.LifeTime;
                item.AgencyCommission = settingsDto.AgencyCommission;
                item.TrackConversions = settingsDto.TrackConversions;
                item.SetPacingValue(settingsDto.PacingPoliciesValue);
                if (item.LogAdMarkup)
                {
                    item.SetAdGroupMarkup();
                }
                // if discount is null then remove it
                if ((settingsDto.Discount == null) || (!settingsDto.Discount.Value.HasValue))
                {
                    item.RemoveDiscount();
                }
                else
                {
                    var discount = new Discount
                    {
                        Value = settingsDto.Discount.Value.Value,
                        FromDate = Framework.Utilities.Environment.GetServerTime(),
                        ToDate = settingsDto.Discount.ToDate,
                        Type = (DiscountType)settingsDto.Discount.TypeId
                    };
                    // update discount
                    item.AddDiscount(discount);
                }
                return ValueMessageWrapper.Create(true);
            }

      
            return ValueMessageWrapper.Create(false);
        }

        public ValueMessageWrapper<bool> RemoveDiscount(ValueMessageWrapper<int> campaignId)
        {
            var item = CampaignRepository.Get(campaignId.Value);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                item.RemoveDiscount();
                item.CostModelWrapper = null;
                return ValueMessageWrapper.Create(true);
            }
            return ValueMessageWrapper.Create(false);
        }

        public void Run(int[] campaignIds)
        {
            if (campaignIds != null)
            {
                foreach (var item in campaignIds.Select(campaignId => CampaignRepository.Get(campaignId)))
                {
                    ValidateCampaign(item);
                    if (item.IsValid)
                    {
                        if (item.AdvertiserAccount != null)
                            ValidateAdvertiser(item.AdvertiserAccount.ID);
                        item.Resume();
                        CampaignRepository.Save(item);
                    }
                }
            }
        }

        public void Pause(int[] campaignIds)
        {
            if (campaignIds != null)
            {
                foreach (var item in campaignIds.Select(campaignId => CampaignRepository.Get(campaignId)))
                {
                    ValidateCampaign(item);
                    if (item.IsValid)
                    {
                        if (item.AdvertiserAccount != null)
                            ValidateAdvertiser(item.AdvertiserAccount.ID);
                        item.Pause();
                        CampaignRepository.Save(item);
                    }
                }
            }
        }

        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetCampaignsTree()
        {
            return GetCampaignTreePrep(null);
        }
        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetCampaignsAdvTree(ValueMessageWrapper<int?> AdvertiserId)
        {
            return GetCampaignTreePrep(AdvertiserId.Value);
        }

        private IEnumerable<Interfaces.DTOs.Core.TreeDto> GetCampaignTreePrep(int? AdvertiserId)
        {

            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }
            criteria.AdvertiserAccountId = AdvertiserId;
            ValidateAccount(criteria.AccountId, criteria.userId);

            List<Domain.Model.Campaign.Campaign> campaigns = CampaignRepository.Query(criteria.GetExpression()).ToList();
            var returnList = new List<TreeDto>();
            foreach (var item in campaigns)
            {
                if (!IsllowedAdvertiserForCamp(item))
                    continue;
                var treeDto = new TreeDto
                {
                    Id = item.ID.ToString(),
                    Childs = new List<TreeDto>(),
                    Name = LocalizedStringDto.ConvertToLocalizedStringDto(item.Name)
                };
                returnList.Add(treeDto);
            }

            return returnList;
        }
        public ResponseDto CloneCampaign(CloneCampaignRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                if (!item.IsClientLocked || Domain.Configuration.IsAdmin)
                {
                    var clone = item.Clone();
                    clone.StartDate = Framework.Utilities.Environment.GetServerTime().Date;
                    clone.EndDate = null;
                    clone.UniqueId = Guid.NewGuid().ToString();
                    if (!string.IsNullOrWhiteSpace(request.Name))
                    {
                        clone.Name = request.Name;
                    }
                    CampaignRepository.Save(clone);
                    string dateFormat = Domain.Configuration.ShortDateFormat;
                    return new ResponseDto { Massage = string.Format(ResourceManager.Instance.GetResource("Campaign", "Clone"), clone.Name, clone.StartDate.ToString(dateFormat)), success = true };
                }
            }
            else
            {
                return new ResponseDto { Massage = ResourceManager.Instance.GetResource("LockedWarning", "Campaign"), success = false };
            }
            return new ResponseDto { Massage = ResourceManager.Instance.GetResource("CloneCampaignError", "Errors"), success = true };
        }
        public string RenameGroup(RenameGroupRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    var group = item.AdGroups.Where(x => x.ID == request.AdgroupId).FirstOrDefault();
                    if (group != null)
                    {
                        group.Name = request.Name;
                    }
                }
                CampaignRepository.Save(item);
                return ResourceManager.Instance.GetResource("RenamedSuccessfully", "Campaign");
            }
            return string.Empty;
        }

        public CampaignServerSettingDto GetServerSettings(ValueMessageWrapper<int> id)
        {
            var campaign = CampaignRepository.Get(id.Value);

            CheckCampaign(campaign);
            ValidateCampaign(campaign);

            if (campaign.IsValid)
            {
                if (campaign.CampaignServerSetting != null)
                {
                    var dto = MapperHelper.Map<CampaignServerSettingDto>(campaign.CampaignServerSetting);
                    dto.HasObjective = campaign.HasGroups();
                    if ((AgencyCommission)dto.AgencyCommission == AgencyCommission.FixedCPM)
                        dto.AgencyCommissionValue = campaign.CampaignServerSetting.AgencyCommissionValue * 1000;
                    else
                        dto.AgencyCommissionValue = campaign.CampaignServerSetting.AgencyCommissionValue * 100;

                    return dto;
                }

                var settingDto = new CampaignServerSettingDto();
                settingDto.Name = campaign.Name;
                settingDto.ID = campaign.ID;

                return settingDto;
            }

            return null;
        }

        public void SaveServerSetting(CampaignServerSettingDto settingDto)
        {
            var item = CampaignRepository.Get(settingDto.ID);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var campaignServerSetting = MapperHelper.Map<CampaignServerSetting>(settingDto);
                if (campaignServerSetting.AgencyCommission == AgencyCommission.Undefined)
                {
                    campaignServerSetting.AgencyCommission = null;

                }

                if ((AgencyCommission)settingDto.AgencyCommission == AgencyCommission.FixedCPM)
                    campaignServerSetting.AgencyCommissionValue = settingDto.AgencyCommissionValue / 1000;
                else
                    campaignServerSetting.AgencyCommissionValue = settingDto.AgencyCommissionValue / 100;
                item.ChangeServerSetting(campaignServerSetting);
               
            }
        }

        public void DeleteFrequencyCapping(DeleteFrequencyCappingRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                CampaignFrequencyCapping frequencyCappignItem = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.ID == request.FrequencyCappingId).SingleOrDefault();

                if (frequencyCappignItem != null)
                {
                    item.CampaignServerSetting.FrequencyCappingList.Remove(frequencyCappignItem);
                }

                CampaignRepository.Save(item);
            }
        }

        public void SaveFrequencyCapping(SaveFrequencyCappingRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                CampaignFrequencyCapping frequencyCapping = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.ID == request.FrequencyCapping.EventId).SingleOrDefault();

                if (frequencyCapping != null)
                {
                    frequencyCapping.Interval = request.FrequencyCapping.Interval;
                    frequencyCapping.Number = request.FrequencyCapping.Number;
                    frequencyCapping.Type = request.FrequencyCapping.Type;

                    // frequencyCapping. = frequencyCappingSave.IsCapping;
                    //var error = new BusinessException();
                    //error.Errors.Add(new ErrorData { ID = "DuplicateFrequencyCapping" });
                    //   throw error;
                    if (frequencyCapping.Interval >= (int)FrequencyCappingInterval.LifeTime)
                    {
                        //your business
                        frequencyCapping.Number = request.FrequencyCapping.Number;
                        frequencyCapping.Type = request.FrequencyCapping.Type;


                        // Buinsess of Capping Life Time
                        var noOfMonth = 3 * 30;

                        // if (item.LifeTime == CampaignLifeTime.Default)
                        //{
                        if (item.EndDate.HasValue)
                        {
                            var DateDiff = item.EndDate.Value - item.StartDate;

                            var month = DateDiff.TotalDays;

                            if (month == 0)
                            {
                                month = 1;
                            }
                            noOfMonth = noOfMonth + (int)month;



                        }

                        //}

                        frequencyCapping.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;
                    }
                }
                else
                {
                    frequencyCapping = MapperHelper.Map<CampaignFrequencyCapping>(request.FrequencyCapping);
                    frequencyCapping.Event = trackingEventRepository.Get(request.FrequencyCapping.EventId);
                    frequencyCapping.CampaignServerSetting = item.CampaignServerSetting;


                    if (frequencyCapping.Interval >= (int)FrequencyCappingInterval.LifeTime)
                    {
                        //your business



                        // Buinsess of Capping Life Time
                        var noOfMonth = 3 * 30;

                        //if (item.LifeTime == CampaignLifeTime.Default)
                        //{
                        if (item.EndDate.HasValue)
                        {
                            var DateDiff = item.EndDate.Value - item.StartDate;

                            var month = DateDiff.TotalDays;
                            if (month == 0)
                            {
                                month = 1;
                            }
                            noOfMonth = noOfMonth + (int)month;



                        }

                        //}




                        frequencyCapping.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;
                    }

                    item.CampaignServerSetting.FrequencyCappingList.Add(frequencyCapping);

                }
                CampaignRepository.Save(item);
            }
        }
        public void SaveCampaignAssignAppsites(CampaignAssignedAppsitesSaveDTo campaignAssignedAppsitesSaveDTo)
        {
            var item = CampaignRepository.Get(campaignAssignedAppsitesSaveDTo.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                foreach (CampaignAssignedAppsitesDto campaignAssignedAppsitesDto in campaignAssignedAppsitesSaveDTo.InsertedItems)
                {
                    SubAppsite subsite = null;
                    if (campaignAssignedAppsitesDto.SubAppsiteId.HasValue)
                    {
                        subsite = new SubAppsite { ID = campaignAssignedAppsitesDto.SubAppsiteId.Value };
                    }
                    var campaignAssignedAppsite = new CampaignAssignedAppsite() { Campaign = item, Include = !campaignAssignedAppsitesDto.Include, SubPublisherId = campaignAssignedAppsitesDto.SubPublisherId };
                    campaignAssignedAppsite.AppSite = new ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite { ID = campaignAssignedAppsitesDto.Appsite.ID };
                    campaignAssignedAppsite.AppSite.Name = appSiteRepository.GetObjectName(campaignAssignedAppsitesDto.Appsite.ID);
                    campaignAssignedAppsite.SubAppsite = subsite;
                    campaignAssignedAppsite.Account = new Domain.Model.Account.Account { ID = appSiteRepository.getAccountId(campaignAssignedAppsitesDto.Appsite.ID) };
                    item.AddAssignedAppsites(campaignAssignedAppsite);
                }
                foreach (CampaignAssignedAppsitesDto dto in campaignAssignedAppsitesSaveDTo.UpdatedItems)
                {
                    var campaignUpdatedAssignedAppsite = item.GetCampaignAssignedAppsite().Where(x => x.ID.ToString() == dto.ID).FirstOrDefault();
                    if (campaignUpdatedAssignedAppsite == null)
                    {
                        continue;
                    }
                    campaignUpdatedAssignedAppsite.Include = !dto.Include;
                }
                if (campaignAssignedAppsitesSaveDTo.DeletedAssignedAppsites != null)
                {
                    foreach (int deletedId in campaignAssignedAppsitesSaveDTo.DeletedAssignedAppsites)
                    {
                        if (deletedId != 0)
                        {
                            item.DeleteAssignedAppsites(deletedId);
                        }
                    }
                }

                #region remove extra data ,remove assigned appsites  when user select All with (include,not include)
                var assignList = item.CampaignAssignedAppsites;
                var includeAll = item.GetCampaignAssignedAppsite().Where(x => (x.SubPublisherId == string.Empty || x.SubPublisherId == null) && x.Include);
                if (includeAll != null)
                {
                    foreach (var includeItem in includeAll)
                    {
                        var inclededItems = item.GetCampaignAssignedAppsite().Where(x => x.Include && x.SubPublisherId != string.Empty && x.SubPublisherId != null && x.AppSite.ID == includeItem.AppSite.ID).ToList();
                        foreach (var campaingAssignApp in inclededItems)
                        {
                            if (campaingAssignApp.ID == 0)
                            {
                                assignList.Remove(campaingAssignApp);
                            }
                            else
                            {
                                CampaignAssignedAppsite assignedAppsite = assignList.Where(x => x.ID == campaingAssignApp.ID).FirstOrDefault();
                                if (assignedAppsite != null)
                                {
                                    assignedAppsite.IsDeleted = true;
                                }
                                // item.DeleteAssignedAppsites(campaingAssignApp.ID);
                            }
                        }
                    }
                }

                var notIncludeAll = item.GetCampaignAssignedAppsite().Where(x => (x.SubPublisherId == string.Empty || x.SubPublisherId == null) && !x.Include);
                if (notIncludeAll != null)
                {
                    foreach (var nIncludeItem in notIncludeAll)
                    {

                        var notInclededItems = item.CampaignAssignedAppsites.Where(x => !x.Include && x.SubPublisherId != string.Empty && x.SubPublisherId != null && x.AppSite.ID == nIncludeItem.AppSite.ID).ToList();
                        foreach (var campaingAssignApp in notInclededItems)
                        {
                            if (campaingAssignApp.ID == 0)
                            {
                                assignList.Remove(campaingAssignApp);
                            }
                            else
                            {
                                CampaignAssignedAppsite assignedAppsite = assignList.Where(x => x.ID == campaingAssignApp.ID).FirstOrDefault();
                                if (assignedAppsite != null)
                                {
                                    assignedAppsite.IsDeleted = true;
                                }
                                // item.DeleteAssignedAppsites(campaingAssignApp.ID);
                            }
                        }
                    }

                }

                item.CampaignAssignedAppsites = assignList;
                #endregion

                IList<CampaignBidConfigDto> notCompatableCampaigns = null;
                List<int> assignedAppsiteseIds = campaignAssignedAppsitesSaveDTo.InsertedItems.Select(x => x.Appsite.ID).ToList();
                var validGefencing = CheckAppsitesCompatibleWithSSPPartnerRTBSetings(campaignAssignedAppsitesSaveDTo.CampaignId, assignedAppsiteseIds);
                if (!validGefencing)
                {
                    throw new BusinessException(new List<ErrorData> { new ErrorData("InventroySourceAllowGeofencing") });
                }
                #region BidConfig


                var response = CheckAppsitesCostModelCompatableWitCampaign( new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = campaignAssignedAppsitesSaveDTo.CampaignId, Appsites = assignedAppsiteseIds });

                if (response.NotCompatableCampaigns != null && response.NotCompatableCampaigns.Count > 0)
                {
                    var adgroups = notCompatableCampaigns.Select(x => x.AdGroupId);
                    if (campaignAssignedAppsitesSaveDTo.NotCompatibleCampaignBidConfigs == null || campaignAssignedAppsitesSaveDTo.NotCompatibleCampaignBidConfigs.Count() <= 0)
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                    }
                    foreach (CampaignBidConfigDto notCompatableBidConfig in notCompatableCampaigns)
                    {
                        var notCompatableBidConfigModified = campaignAssignedAppsitesSaveDTo.NotCompatibleCampaignBidConfigs.Where(x => x.Appsite.ID == notCompatableBidConfig.Appsite.ID && x.AdGroupId == notCompatableBidConfig.AdGroupId && x.SubPublisherId == notCompatableBidConfig.SubPublisherId && x.Bid > 0).FirstOrDefault();
                        if (notCompatableBidConfigModified == null || notCompatableBidConfigModified.Bid <= 0)
                        {
                            throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                        }
                        var group = adGroupRepository.Get(notCompatableBidConfigModified.AdGroupId);

                        if (notCompatableBidConfigModified.Bid > group.GetReadableBid())
                        {
                            throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                        }

                        if (!string.IsNullOrEmpty(notCompatableBidConfigModified.ID))
                        {
                            var campaignBidConfig = group.GetCampaignBidConfigs().Where(x => x.ID == Convert.ToInt32(notCompatableBidConfigModified.ID)).FirstOrDefault();

                            campaignBidConfig.SetAdGroupBidConfigsBid(notCompatableBidConfigModified.Bid);


                        }
                        else
                        {
                            var campaignBidConfig = new AdGroupBidConfig() { AdGroup = group, SubPublisherId = notCompatableBidConfigModified.SubPublisherId };
                            // campaignBidConfig.AppSite = appSiteRepository.Get(notCompatableBidConfigModified.Appsite.ID);

                            campaignBidConfig.AppSite = new Domain.Model.AppSite.AppSite { ID = notCompatableBidConfigModified.Appsite.ID }; /*appSiteRepository.Get(campaignBidConfigDto.Appsite.ID);*/
                            campaignBidConfig.AppSite.Name = appSiteRepository.GetObjectName(notCompatableBidConfigModified.Appsite.ID);
                            campaignBidConfig.AppSite.AppSiteServerSetting = appSiteRepository.getServerSetting(notCompatableBidConfigModified.Appsite.ID);


                            // campaignBidConfig.Account = accountRepository.Get(notCompatableBidConfigModified.AccountId);
                            campaignBidConfig.Account = new Domain.Model.Account.Account { ID = appSiteRepository.getAccountId(notCompatableBidConfigModified.Appsite.ID) };

                            campaignBidConfig.SetAdGroupBidConfigsBid(notCompatableBidConfigModified.Bid);

                            group.AddCampaignBidConfig(campaignBidConfig);
                        }
                    }
                }

                #endregion

                CampaignRepository.Save(item);
            }

        }

        public CampaignSettingsDto GetCampSettings(ValueMessageWrapper<int> campaignId)
        {
            var item = CampaignRepository.Get(campaignId.Value);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {

                var trackingEventsDto = GetCostModelEvents().Where(M => M.ID == 1);
                var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();
                var i = 0;

                var settingsDto = MapperHelper.Map<CampaignSettingsDto>(item);

                var frequencyCapp = item.CampaignServerSetting.GetFrequencyCappingList().Where(M => M.Event.ID == 1).SingleOrDefault();
                if (frequencyCapp == null)
                {
                    if (trackingEventsList[i].DefaultFrequencyCapping != null)
                    {
                        settingsDto.FrequencyCapping = new CampaignFrequencyCappingDto();
                        settingsDto.FrequencyCapping.EventId = 1;
                        settingsDto.FrequencyCapping.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Default;

                        if (3600 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 3600 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                            if (86400 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 86400 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                if (604800 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 604800 / (int)trackingEventsList[i].DefaultFrequencyCapping;

                        else

                                    if (2592000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 2592000 / (int)trackingEventsList[i].DefaultFrequencyCapping;


                        settingsDto.FrequencyCapping.Interval = trackingEventsList[i].DefaultFrequencyCapping != null ? (int)trackingEventsList[i].DefaultFrequencyCapping : 0;

                    }
                    else
                    {
                        settingsDto.FrequencyCapping = new CampaignFrequencyCappingDto();
                        settingsDto.FrequencyCapping.EventId = 1;
                        settingsDto.FrequencyCapping.Interval = 0;
                        settingsDto.FrequencyCapping.Type = 0;
                        settingsDto.FrequencyCapping.Number = 1;
                        settingsDto.FrequencyCapping.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.NoCapping;

                    }
                }
                else
                {
                    settingsDto.FrequencyCapping = MapperHelper.Map<CampaignFrequencyCappingDto>(frequencyCapp);
                    if (frequencyCapp.Interval == 0 && frequencyCapp.Type == 0 && frequencyCapp.Number == 1)
                    {
                        settingsDto.FrequencyCapping.Interval = 0;
                        settingsDto.FrequencyCapping.Type = 0;
                        settingsDto.FrequencyCapping.Number = 1;
                        settingsDto.FrequencyCapping.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.NoCapping;

                    }
                    else
                    {

                        if (frequencyCapp.Interval > 0)
                        {
                            if (3600 >= frequencyCapp.Interval)
                                settingsDto.FrequencyCapping.Interval = 3600;
                            else

                                if (86400 >= frequencyCapp.Interval)
                                settingsDto.FrequencyCapping.Interval = 86400;
                            else

                                    if (604800 >= frequencyCapp.Interval)
                                settingsDto.FrequencyCapping.Interval = 604800;

                            else

                                        if (2592000 >= frequencyCapp.Interval)
                                settingsDto.FrequencyCapping.Interval = 2592000;
                            else
                                settingsDto.FrequencyCapping.Interval = 7776000;


                        }



                        settingsDto.FrequencyCapping.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.Capping;

                        //if (settingsDto.FrequencyCapping.Interval>(int) FrequencyCappingInterval.Month)
                        //{
                        //    settingsDto.FrequencyCapping.CampignFrequencyCappingStatus = CampignFrequencyCappingEnum.CappingLifeTime;
                        //}
                    }

                }




                settingsDto.PacingPoliciesValue = item.GetPacingValue();
                settingsDto.AgencyCommission = item.CampaignServerSetting.getAgencyCommission();
                //settingsDto.AgencyCommissionValue = item.CampaignServerSetting.AgencyCommissionValue;


                if (settingsDto.AgencyCommission == AgencyCommission.FixedCPM)
                    settingsDto.AgencyCommissionValue = item.CampaignServerSetting.AgencyCommissionValue * 1000;
                else
                    settingsDto.AgencyCommissionValue = item.CampaignServerSetting.AgencyCommissionValue * 100;
                return settingsDto;
            }
            return null;
        }
        public IEnumerable<TrackingEventDto> GetCostModelEvents()
        {

            var costModelWrappers = costModelWrapperRepository.GetAll();

            var costModelTrackingEvents = costModelWrappers.Where(M => M.Event != null).Select(p => p.Event);

            if (costModelTrackingEvents != null)
            {
                var trackingEventDtoList = costModelTrackingEvents.Select(p => MapperHelper.Map<TrackingEventDto>(p)).ToList();
                return trackingEventDtoList;
            }

            return null;
        }
        public ValueMessageWrapper<bool> SaveCampSettings(CampaignSettingsDto settingsDto)
        {
            var item = CampaignRepository.Get(settingsDto.ID);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                // Save Basic Settings
                var trackingEventsDto = GetCostModelEvents().Where(M => M.ID == 1);
                var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();
                var i = 0;

                item.SetPacingValue(settingsDto.PacingPoliciesValue);
                item.CampaignServerSetting.setAgencyCommission(settingsDto.AgencyCommission);

                if (item.CampaignServerSetting.AgencyCommission == AgencyCommission.FixedCPM)
                    item.CampaignServerSetting.AgencyCommissionValue = settingsDto.AgencyCommissionValue / 1000;
                else
                    item.CampaignServerSetting.AgencyCommissionValue = settingsDto.AgencyCommissionValue / 100;
                if (settingsDto.FrequencyCapping.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.Default)
                {

                    /*

                    if (trackingEventsList[i].DefaultFrequencyCapping != null)
                    {
                        if (3600 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 3600 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                            if (86400 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 86400 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                if (604800 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 604800 / (int)trackingEventsList[i].DefaultFrequencyCapping;

                        else

                                    if (2592000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            settingsDto.FrequencyCapping.Number = 2592000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                    }
                    else
                    {
                        settingsDto.FrequencyCapping.Number = 0;
                    }

                    settingsDto.FrequencyCapping.Interval = trackingEventsList[i].DefaultFrequencyCapping != null ? (int)trackingEventsList[i].DefaultFrequencyCapping : 0;
                    */

                    CampaignFrequencyCapping frequencyCappignItem = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.ID == settingsDto.FrequencyCapping.ID).SingleOrDefault();

                    if (frequencyCappignItem != null)
                    {
                        item.CampaignServerSetting.FrequencyCappingList.Remove(frequencyCappignItem);
                    }
                    return  ValueMessageWrapper.Create(true);
                }
                else if (settingsDto.FrequencyCapping.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.NoCapping)
                {

                    settingsDto.FrequencyCapping.Interval = 0;
                    settingsDto.FrequencyCapping.Type = 0;
                    settingsDto.FrequencyCapping.Number = 1;
                }

                else if (settingsDto.FrequencyCapping.Interval >= (int)FrequencyCappingInterval.LifeTime)
                {
                    // Buinsess of Capping Life Time
                    // Buinsess of Capping Life Time
                    var noOfMonth = 3 * 30;

                    // if (item.LifeTime == CampaignLifeTime.Default)
                    //{
                    if (item.EndDate.HasValue)
                    {
                        var DateDiff = item.EndDate.Value - item.StartDate;

                        var month = DateDiff.TotalDays;
                        if (month == 0)
                        { month = 1; }
                        noOfMonth = noOfMonth + (int)month;



                    }

                    // }

                    settingsDto.FrequencyCapping.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;




                    //settingsDto.FrequencyCapping.Interval = noOfMonth*(int)FrequencyCappingInterval.Month;

                }


                CampaignFrequencyCapping frequencyCapping = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.ID == settingsDto.FrequencyCapping.EventId).SingleOrDefault();

                if (frequencyCapping != null)
                {
                    frequencyCapping.Interval = settingsDto.FrequencyCapping.Interval;
                    frequencyCapping.Number = settingsDto.FrequencyCapping.Number;
                    frequencyCapping.Type = settingsDto.FrequencyCapping.Type;

                    // frequencyCapping. = frequencyCappingSave.IsCapping;
                    //var error = new BusinessException();
                    //error.Errors.Add(new ErrorData { ID = "DuplicateFrequencyCapping" });
                    //   throw error;
                }
                else
                {
                    frequencyCapping = MapperHelper.Map<CampaignFrequencyCapping>(settingsDto.FrequencyCapping);
                    if (settingsDto.FrequencyCapping.EventId == 0)
                        settingsDto.FrequencyCapping.EventId = 1;

                    frequencyCapping.Event = trackingEventRepository.Get(settingsDto.FrequencyCapping.EventId);
                    frequencyCapping.CampaignServerSetting = item.CampaignServerSetting;



                    item.CampaignServerSetting.FrequencyCappingList.Add(frequencyCapping);

                }


                return ValueMessageWrapper.Create(true);
            }

            if (item.AdvertiserAccount != null)
                ValidateAdvertiser(item.AdvertiserAccount.ID);
            return ValueMessageWrapper.Create(false);
        }

        public CampaignAssignedAppsitesModelDto GetCampaignAssignAppsites(ValueMessageWrapper<int> campaignId)
        {

            var item = CampaignRepository.Get(campaignId.Value);
            CheckCampaign(item);
            ValidateCampaign(item);
            var returnValue = new CampaignAssignedAppsitesModelDto();
            returnValue.CampignName = item.Name;
            if (item.IsValid)
            {
                var models = item.GetCampaignAssignedAppsite();
                returnValue.CampaignAssignedAppsitesList = models.Select(x => MapperHelper.Map<CampaignAssignedAppsitesDto>(x)).ToList();
                if (models != null)
                {
                    foreach (var obj in models)
                    {
                        var dto = returnValue.CampaignAssignedAppsitesList.Where(M => M.ID == obj.ID.ToString()).FirstOrDefault();
                        if (obj.SubAppsite != null)
                            dto.SubAppsiteId = obj.SubAppsite.ID;
                    }
                }
            }
            return returnValue;
        }

        public CampaignBidConfigModelDto GetCampaignBidConfigs(CampaignIdAdgroupIdMessage request)
        {
            //var item = CampaignRepository.Get(campaignId);
            //CheckCampaign(item);
            //ValidateCampaign(item);
            ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign item = null;
            AdGroup group = null;
            if (request.AdgroupId > 0) {
                group = adGroupRepository.Get(request.AdgroupId);
                if (group != null)
                {
                    item = group.Campaign;
                    ValidateCampaign(item);
                }
                else
                {
                    item = CampaignRepository.Get(request.CampaignId);
                }
            }

            var returnValue = new CampaignBidConfigModelDto();
            returnValue.CampignName = item.Name;
            if (item.IsValid)
            {
                //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
                if (group != null)
                {
                    returnValue.CampaignBidConfigDtos = group.GetCampaignBidConfigs().Select(x => MapperHelper.Map<CampaignBidConfigDto>(x)).ToList();
                }

                if (returnValue.CampaignBidConfigDtos != null)
                {
                    foreach (CampaignBidConfigDto campaignBidConfigDto in returnValue.CampaignBidConfigDtos)
                    {
                        if (campaignBidConfigDto.Appsite != null)
                        {
                            CostModelWrapper costModelWrapper = null;

                            if (campaignBidConfigDto.AppsitePricingModelId == -1)
                                costModelWrapper = group.CostModelWrapper;
                            else
                                costModelWrapper = costModelWrapperRepository.Get(campaignBidConfigDto.AppsitePricingModelId);

                            if (costModelWrapper != null)
                                campaignBidConfigDto.Bid *= costModelWrapper.Factor;
                            else
                                throw new InvalidOperationException("CostModelWrapper should not be null");

                        }

                    }

                }

            }

            return returnValue;
        }

        public InventorySourceModelDto GetInventorySources(CampaignIdAdgroupIdMessage request)
        {
            //var item = CampaignRepository.Get(campaignId);
            // CheckCampaign(item);
            // ValidateCampaign(item);
            var returnValue = new InventorySourceModelDto();
            // returnValue.CampignName = item.Name;
            // if (item.IsValid)
            // {
            //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
            var group = adGroupRepository.Get(request.AdgroupId);
            if (group != null)
            {
                returnValue.InventorySourceDtos = group.GetAdGroupInventorySources().Select(x => MapperHelper.Map<InventorySourceDto>(x)).ToList();
            }

            //if (returnValue.InventorySourceDtos != null)
            //{
            //    foreach (InventorySourceDto campaignBidConfigDto in returnValue.InventorySourceDtos)
            //    {


            //           var partner= this._SSPPartnerRepository.Query(M=>M.Account.ID== campaignBidConfigDto.AccountId).First();
            //        campaignBidConfigDto.AccountName = partner.Name + "-" + campaignBidConfigDto.AccountName;  



            //    }

            //}

            //}

            return returnValue;
        }
        public IList<PMPDealDto> GetPMPDealConfigConfigs(CampaignIdAdgroupIdMessage request)
        {
            var item = adGroupRepository.Get(request.AdgroupId);

            var returnValue = new List<PMPDealDto>();

            // returnValueSource.CampignName = item.Name;


            //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
            if (item != null)
            {
                returnValue = item.GetPMPDeal().Select(x => MapperHelper.Map<PMPDealDto>(x)).ToList();
            }





            return returnValue;
        }

        public IList<AdvertiserAccountMasterAppSiteDto> GetMasterListConfigConfigs(CampaignIdAdgroupIdAdIdMessage request)
        {
            var item = adGroupRepository.Get(request.AdgroupId);

            var returnValue = new List<AdvertiserAccountMasterAppSiteDto>();

            // returnValueSource.CampignName = item.Name;


            //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
            if (item != null)
            {
                returnValue = item.GetAccountMasterAppSiteLists().Select(x => MapperHelper.Map<AdvertiserAccountMasterAppSiteDto>(x)).ToList();
            }





            return returnValue;
        }
        public AdGroupDealAndSourceDTO GetDealsAndSources(CampaignIdAdgroupIdMessage request)
        {
            var result = new AdGroupDealAndSourceDTO();
            var returnValue = new List<PMPDealDto>();
            var item = adGroupRepository.Get(request.AdgroupId);
            var returnValueSource = new InventorySourceModelDto();
            //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
            if (item != null)
            {
                returnValue = item.GetPMPDeal().Select(x => MapperHelper.Map<PMPDealDto>(x)).ToList();
            }

            result.Deals = returnValue;

            if (item != null)
            {
                returnValueSource.InventorySourceDtos = item.GetAdGroupInventorySources().Select(x => MapperHelper.Map<InventorySourceDto>(x)).ToList();
            }
            result.Sources = returnValueSource;


            var group = item;
            var camp = item.Campaign;
            var returnValue2 = new CampaignBidConfigModelDto();
            returnValue2.CampignName = camp.Name;
            if (camp.IsValid)
            {
                //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
                if (group != null)
                {
                    returnValue2.CampaignBidConfigDtos = group.GetCampaignBidConfigs().Select(x => MapperHelper.Map<CampaignBidConfigDto>(x)).ToList();
                }

                if (returnValue2.CampaignBidConfigDtos != null)
                {
                    foreach (CampaignBidConfigDto campaignBidConfigDto in returnValue2.CampaignBidConfigDtos)
                    {
                        if (campaignBidConfigDto.Appsite != null)
                        {
                            CostModelWrapper costModelWrapper = null;

                            if (campaignBidConfigDto.AppsitePricingModelId == -1)
                                costModelWrapper = group.CostModelWrapper;
                            else
                                costModelWrapper = costModelWrapperRepository.Get(campaignBidConfigDto.AppsitePricingModelId);

                            if (costModelWrapper != null)
                                campaignBidConfigDto.Bid *= costModelWrapper.Factor;
                            else
                                throw new InvalidOperationException("CostModelWrapper should not be null");

                        }

                    }

                }

            }
            result.AdGroupSettings = new AdGroupSettingsDto();
            result.AdGroupSettings.DailyBudget = group.DailyBudget;
            result.AdGroupSettings.Budget = group.Budget;
            result.AdGroupSettings.CampaignBudget = camp.Budget;
            result.AdGroupSettings.CampaignId = camp.ID;
            result.AdGroupSettings.AdGroupId = group.ID;

            result.BidConfigs = returnValue2;
            return result;

        }
        private AdGroupDealAndSourceDTO GetDealsAndSourcesForGroup(AdGroup item)
        {
            var result = new AdGroupDealAndSourceDTO();
            var returnValue = new List<PMPDealDto>();
            var returnValueSource = new InventorySourceModelDto();
            //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
            if (item != null)
            {
                returnValue = item.GetPMPDeal().Select(x => MapperHelper.Map<PMPDealDto>(x)).ToList();
            }

            result.Deals = returnValue;

            if (item != null)
            {
                returnValueSource.InventorySourceDtos = item.GetAdGroupInventorySources().Select(x => MapperHelper.Map<InventorySourceDto>(x)).ToList();
            }
            result.Sources = returnValueSource;


            var group = item;
            var camp = item.Campaign;
            var returnValue2 = new CampaignBidConfigModelDto();
            returnValue2.CampignName = camp.Name;
            if (camp.IsValid)
            {
                //var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupID);
                if (group != null)
                {
                    returnValue2.CampaignBidConfigDtos = group.GetCampaignBidConfigs().Select(x => MapperHelper.Map<CampaignBidConfigDto>(x)).ToList();
                }

                if (returnValue2.CampaignBidConfigDtos != null)
                {
                    foreach (CampaignBidConfigDto campaignBidConfigDto in returnValue2.CampaignBidConfigDtos)
                    {
                        if (campaignBidConfigDto.Appsite != null)
                        {
                            CostModelWrapper costModelWrapper = null;

                            if (campaignBidConfigDto.AppsitePricingModelId == -1)
                                costModelWrapper = group.CostModelWrapper;
                            else
                                costModelWrapper = costModelWrapperRepository.Get(campaignBidConfigDto.AppsitePricingModelId);

                            if (costModelWrapper != null)
                                campaignBidConfigDto.Bid *= costModelWrapper.Factor;
                            else
                                throw new InvalidOperationException("CostModelWrapper should not be null");

                        }

                    }

                }

            }
            result.AdGroupSettings = new AdGroupSettingsDto();
            result.AdGroupSettings.DailyBudget = group.DailyBudget;
            result.AdGroupSettings.Budget = group.Budget;
            result.AdGroupSettings.CampaignBudget = camp.Budget;
            result.AdGroupSettings.CampaignId = camp.ID;
            result.AdGroupSettings.AdGroupId = group.ID;

            result.BidConfigs = returnValue2;
            return result;
        }
        public void SaveCampaignBidConfig(CampaignBidConfigSaveDTo campaignBidConfigSaveDTo)
        {
            var item = CampaignRepository.Get(campaignBidConfigSaveDTo.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == campaignBidConfigSaveDTo.AdGroupId);
                if (group != null)
                {
                    // IList<CampaignBidConfigDto> list = null;
                    //  CheckAppsitesCostModelCompatableWitCampaign(campaignBidConfigSaveDTo.CampaignId, campaignBidConfigSaveDTo.AdGroupId, campaignBidConfigSaveDTo., campaignBidConfigSaveDTo.InsertedItems.Select(x => x.Appsite.ID).ToList(), out list, true);
                    foreach (CampaignBidConfigDto campaignBidConfigDto in campaignBidConfigSaveDTo.InsertedItems)
                    {
                        var campaignBidConfig = new AdGroupBidConfig() { AdGroup = group, SubPublisherId = campaignBidConfigDto.SubPublisherId };
                        // campaignBidConfig.AppSite = appSiteRepository.Get(campaignBidConfigDto.Appsite.ID);
                        //campaignBidConfig.Account = accountRepository.Get(campaignBidConfigDto.AccountId);


                        // campaignBidConfig.AppSite = appSiteRepository.Get(notCompatableBidConfigModified.Appsite.ID);

                        campaignBidConfig.AppSite = new Domain.Model.AppSite.AppSite { ID = campaignBidConfigDto.Appsite.ID }; /*appSiteRepository.Get(campaignBidConfigDto.Appsite.ID);*/
                        campaignBidConfig.AppSite.Name = appSiteRepository.GetObjectName(campaignBidConfigDto.Appsite.ID);
                        campaignBidConfig.AppSite.AppSiteServerSetting = appSiteRepository.getServerSetting(campaignBidConfigDto.Appsite.ID);


                        // campaignBidConfig.Account = accountRepository.Get(notCompatableBidConfigModified.AccountId);
                        campaignBidConfig.Account = new Domain.Model.Account.Account { ID = appSiteRepository.getAccountId(campaignBidConfigDto.Appsite.ID) };


                        campaignBidConfig.Bid = campaignBidConfigDto.Bid;
                        //  var campaignBidConfig = MapperHelper.Map<CampaignBidConfig>(campaignBidConfigDto);
                        group.AddCampaignBidConfig(campaignBidConfig);
                    }
                    foreach (CampaignBidConfigDto dto in campaignBidConfigSaveDTo.UpdatedItems)
                    {
                        var campaignBidConfig = group.GetCampaignBidConfigs().Where(x => x.ID.ToString() == dto.ID).FirstOrDefault();
                        if (campaignBidConfig == null)
                        {
                            continue;
                        }
                        campaignBidConfig.Bid = dto.Bid;
                    }
                    if (campaignBidConfigSaveDTo.DeletedCampaignBidConfigs != null)
                    {
                        foreach (int deletedId in campaignBidConfigSaveDTo.DeletedCampaignBidConfigs)
                        {
                            if (deletedId != 0)
                            {
                                group.DeleteCampaignBidConfig(deletedId);
                            }
                        }
                    }



                }
            }
            CampaignRepository.Save(item);
        }

        public CampaignSaveDto SaveCampInfoSettings(CampaignAllDto oCampaignAllDto)
        {
            var result = new CampaignSaveDto();
            var item = CampaignRepository.Get(oCampaignAllDto.oCampaignDto.ID);
            oCampaignAllDto.oCampaignDto.EndDate = oCampaignAllDto.oCampaignDto.EndDate.HasValue ? new DateTime(oCampaignAllDto.oCampaignDto.EndDate.Value.Year, oCampaignAllDto.oCampaignDto.EndDate.Value.Month, oCampaignAllDto.oCampaignDto.EndDate.Value.Day, 23, 59, 59) : (DateTime?)null;
            var dateChnaged = false;
            if (item != null)
            {
                //Update Old Item

                if (oCampaignAllDto.oCampaignDto.CampaignType != CampaignType.AdHouse)
                {
                    //check if the Campaign is started 
                    if ((item.IsRuntime))
                    // && (item.StartDate.CompareTo(Framework.Utilities.Environment.GetServerTime().Date) <= 0))
                    {
                        //ad is served on this oCampaignAllDto.oCampaignDto
                        //you cant's change the start data after this
                        if (item.StartDate != oCampaignAllDto.oCampaignDto.StartDate)
                        {
                            //create business Exception to hold error data list 
                            var error = new BusinessException();
                            error.Errors.Add(new ErrorData { ID = "CampaignStartDateBR" });
                            throw error;
                        }
                    }
                }
                result.Warnings = item.GetWarnings(oCampaignAllDto.oCampaignDto.Budget, oCampaignAllDto.oCampaignDto.DailyBudget, oCampaignAllDto.oCampaignDto.EndDate);

                item.Name = oCampaignAllDto.oCampaignDto.Name;
                item.Note = oCampaignAllDto.oCampaignDto.Note;
                if (item.StartDate != oCampaignAllDto.oCampaignDto.StartDate)
                {
                    dateChnaged = true;
                }

                if (item.EndDate != oCampaignAllDto.oCampaignDto.EndDate || item.StartDate != oCampaignAllDto.oCampaignDto.StartDate)
                {
                    var frquencyLists = item.CampaignServerSetting.GetFrequencyCappingList();

                    if (frquencyLists != null && frquencyLists.Count > 0)
                    {
                        foreach (var frquencyList in frquencyLists)
                        {
                            if (frquencyList.Interval >= (int)FrequencyCappingInterval.LifeTime)
                            {



                                // Buinsess of Capping Life Time
                                var noOfMonth = 3 * 30;

                                // if (item.LifeTime == CampaignLifeTime.Default)
                                //{
                                if (oCampaignAllDto.oCampaignDto.EndDate.HasValue)
                                {
                                    var DateDiff = oCampaignAllDto.oCampaignDto.EndDate.Value - oCampaignAllDto.oCampaignDto.StartDate;

                                    var month = DateDiff.TotalDays;

                                    if (month == 0)
                                    {
                                        month = 1;
                                    }
                                    noOfMonth = noOfMonth + (int)month;



                                }

                                //}

                                frquencyList.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;
                            }
                        }
                    }
                }


                item.StartDate = oCampaignAllDto.oCampaignDto.StartDate;





                item.EndDate = oCampaignAllDto.oCampaignDto.EndDate;
                item.StartTime = oCampaignAllDto.oCampaignDto.StartTime;
                item.EndTime = oCampaignAllDto.oCampaignDto.EndTime;
                item.Budget = oCampaignAllDto.oCampaignDto.Budget;
                item.DailyBudget = oCampaignAllDto.oCampaignDto.DailyBudget;
                if (oCampaignAllDto.oCampaignDto.AdvertiserAccountId > 0)
                    item.AdvertiserAccount = new AdvertiserAccount { ID = oCampaignAllDto.oCampaignDto.AdvertiserAccountId };
                else
                    item.AdvertiserAccount = null;
                item.Advertiser = oCampaignAllDto.oCampaignDto.CampaignType == CampaignType.Normal  || oCampaignAllDto.oCampaignDto.CampaignType == CampaignType.ProgrammaticGuaranteed ? AdvertiserRepository.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID) : null;


                ValidateCampaign(item, true, dateChnaged);


            }
            else
            {
                //New Item
                item = MapperHelper.Map<Domain.Model.Campaign.Campaign>(oCampaignAllDto.oCampaignDto);
                item.PriceMode = PriceMode.Fixed;
                item.LifeTime = CampaignLifeTime.Default;
                if (oCampaignAllDto.oCampaignDto.Advertiser != null)
                    item.Advertiser = AdvertiserRepository.Get(oCampaignAllDto.oCampaignDto.Advertiser.ID);
                if (oCampaignAllDto.oCampaignDto.AdvertiserAccountId > 0)
                    item.AdvertiserAccount = AdvertiserAccountRepository.Get(oCampaignAllDto.oCampaignDto.AdvertiserAccountId);
                else
                    item.AdvertiserAccount = null;
                item.CreationDate = Framework.Utilities.Environment.GetServerTime();
                item.UniqueId = Guid.NewGuid().ToString();

                item.AddDefaultCampaignServerSetting();
                if (item.AdvertiserAccount != null)
                {
                    item.CampaignServerSetting.AgencyCommission = item.AdvertiserAccount.AgencyCommission;
                    item.CampaignServerSetting.AgencyCommissionValue = item.AdvertiserAccount.AgencyCommissionValue;


                }
                //item.Status = AdCampaignStatus.Empty;
                //Change Account 
                if (item.Account == null)
                {
                    item.ChangeAccount(
                        accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value));
                }

                if (item.User == null)
                {
                    item.ChangeUser(new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value });


                }
                result.Warnings = item.GetWarnings(oCampaignAllDto.oCampaignDto.Budget, oCampaignAllDto.oCampaignDto.DailyBudget, oCampaignAllDto.oCampaignDto.EndDate, true);
                item.ValidateNew();

            }
            if (item.IsValid)
            {
                
                CampaignRepository.Save(item);

                #region Save Settings
                // Save Basic Settings 
                var trackingEventsDto = GetCostModelEvents().Where(M => M.ID == 1);
                var trackingEventsList = trackingEventsDto.Select(p => new { p.EventDescription, p.ID, p.EventName, p.DefaultFrequencyCapping }).ToArray();
                var i = 0;

                item.SetPacingValue(oCampaignAllDto.oCampaignSettingsDto.PacingPoliciesValue);
                item.CampaignServerSetting.setAgencyCommission(oCampaignAllDto.oCampaignSettingsDto.AgencyCommission);

                if (item.CampaignServerSetting.AgencyCommission == AgencyCommission.FixedCPM)
                    item.CampaignServerSetting.AgencyCommissionValue = oCampaignAllDto.oCampaignSettingsDto.AgencyCommissionValue / 1000;
                else
                    item.CampaignServerSetting.AgencyCommissionValue = oCampaignAllDto.oCampaignSettingsDto.AgencyCommissionValue / 100;
                if (oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.Default)
                {

                    /*

                    if (trackingEventsList[i].DefaultFrequencyCapping != null)
                    {
                        if (3600 >= trackingEventsList[i].DefaultFrequencyCapping)
                            oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 3600 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                            if (86400 >= trackingEventsList[i].DefaultFrequencyCapping)
                            oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 86400 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                        else

                                if (604800 >= trackingEventsList[i].DefaultFrequencyCapping)
                            oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 604800 / (int)trackingEventsList[i].DefaultFrequencyCapping;

                        else

                                    if (2592000 >= trackingEventsList[i].DefaultFrequencyCapping)
                            oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 2592000 / (int)trackingEventsList[i].DefaultFrequencyCapping;
                    }
                    else
                    {
                        oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 0;
                    }

                    oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval = trackingEventsList[i].DefaultFrequencyCapping != null ? (int)trackingEventsList[i].DefaultFrequencyCapping : 0;
                    */

                    CampaignFrequencyCapping frequencyCappignItem = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.ID == oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.ID).SingleOrDefault();

                    if (frequencyCappignItem != null)
                    {
                        item.CampaignServerSetting.FrequencyCappingList.Remove(frequencyCappignItem);
                    }
                }
                else if (oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.CampignFrequencyCappingStatus == CampignFrequencyCappingEnum.NoCapping)
                {

                    oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval = 0;
                    oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Type = 0;
                    oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number = 1;
                }

                else if (oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval >= (int)FrequencyCappingInterval.LifeTime)
                {
                    // Buinsess of Capping Life Time
                    // Buinsess of Capping Life Time
                    var noOfMonth = 3 * 30;

                    // if (item.LifeTime == CampaignLifeTime.Default)
                    //{
                    if (item.EndDate.HasValue)
                    {
                        var DateDiff = item.EndDate.Value - item.StartDate;

                        var month = DateDiff.TotalDays;
                        if (month == 0)
                        { month = 1; }
                        noOfMonth = noOfMonth + (int)month;



                    }

                    // }

                    oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval = noOfMonth * (int)FrequencyCappingInterval.Day;




                    //oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval = noOfMonth*(int)FrequencyCappingInterval.Month;

                }


                CampaignFrequencyCapping frequencyCapping = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.ID == oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.EventId).SingleOrDefault();

                if (frequencyCapping != null)
                {
                    frequencyCapping.Interval = oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Interval;
                    frequencyCapping.Number = oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Number;
                    frequencyCapping.Type = oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.Type;

                    // frequencyCapping. = frequencyCappingSave.IsCapping;
                    //var error = new BusinessException();
                    //error.Errors.Add(new ErrorData { ID = "DuplicateFrequencyCapping" });
                    //   throw error;
                }
                else
                {
                    frequencyCapping = MapperHelper.Map<CampaignFrequencyCapping>(oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping);
                    frequencyCapping.Event = trackingEventRepository.Get(oCampaignAllDto.oCampaignSettingsDto.FrequencyCapping.EventId);
                    frequencyCapping.CampaignServerSetting = item.CampaignServerSetting;



                    item.CampaignServerSetting.FrequencyCappingList.Add(frequencyCapping);

                }
                CampaignRepository.Save(item);
                #endregion
            }
            result.ID = item.ID;


            return result;
        }
        #endregion

        #region Ad Groups
        public IList<AdGroupDto> GetAllAdGroupByAccount(ValueMessageWrapper<int> AccountId)
        {
            var adGroups = CampaignRepository.GetAllAdGroupByAccount(AccountId.Value);
            var list = adGroups.Select(adGroup => MapperHelper.Map<AdGroupDto>(adGroup)).ToList();

            return list;

        }


        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdGroupsTree()
        {
            return GetAdGroupAdsPrep(null);

        }
        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdGroupsAdvTree(ValueMessageWrapper<int?> AdvertiserId)
        {
            return GetAdGroupAdsPrep(AdvertiserId.Value);
        }
        private IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdGroupAdsPrep(int? AdvertiserId)
        {
            CampaignCriteria criteria = new CampaignCriteria();
            criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }
            criteria.AdvertiserAccountId = AdvertiserId;
            ValidateAccount(criteria.AccountId, criteria.userId);

            List<Domain.Model.Campaign.Campaign> campaigns = CampaignRepository.Query(criteria.GetExpression()).ToList();
            List<TreeDto> returnList = new List<TreeDto>();
            foreach (var item in campaigns.Where(p => p.GetGroups().Count != 0))
            {
                if (!IsllowedAdvertiserForCamp(item))
                    continue;
                var treeDto = new TreeDto { Id = item.ID.ToString() };
                var childs = new List<TreeDto>();
                foreach (var item1 in item.GetGroups())
                {
                    var childDto = new TreeDto
                    {
                        Id = item1.ID.ToString(),
                        Name = LocalizedStringDto.ConvertToLocalizedStringDto(item1.Name)
                    };
                    childs.Add(childDto);
                }
                treeDto.Childs = childs;
                treeDto.Name = LocalizedStringDto.ConvertToLocalizedStringDto(item.Name);
                returnList.Add(treeDto);
            }

            return returnList.ToList();

        }
        public ResponseDto CloneAdGroup(CloneAdgroupRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    if (IsAllowedGroup(group) && (!item.IsClientLocked || Domain.Configuration.IsAdmin))
                    {
                        var clone = group.Clone();
                        if (!string.IsNullOrWhiteSpace(request.Name))
                        {
                            clone.Name = request.Name;
                        }
                        return new ResponseDto { Massage = string.Format(ResourceManager.Instance.GetResource("AdGroup", "Clone"), clone.Name), success = true };

                    }
                    else
                    {
                        return new ResponseDto { Massage = ResourceManager.Instance.GetResource("LockedWarning", "Campaign"), success = false };

                    }
                }
            }
            return new ResponseDto { Massage = ResourceManager.Instance.GetResource("CloneAdGroupError", "Errors"), success = false };
        }

        #region Cost Elements

        public AdGroupCostElementResultDto GetAdGroupCostElements(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdGroupCostElementCriteria wcriteria)
        {
            AdGroupCostElementCriteria criteria = new AdGroupCostElementCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new AdGroupCostElementResultDto { TotalCount = 0, Items = new List<AdGroupCostElementDto>() };
            var item = CampaignRepository.Get(criteria.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == criteria.AdGroupId);
                if (group != null)
                {
                    // get ad group cost elements and apply the filter on it 
                    var costelemts = item.GetGroupCostElements(group);
                    if (costelemts.Count > 0)
                    {
                        var items = item.GetGroupCostElements(group).Where(
                            x =>
                            (criteria.DataFrom == null || x.FromDate > criteria.DataFrom) &&
                            //(!x.ToDate.HasValue)
                            (criteria.DataTo == null || x.ToDate < criteria.DataTo)
                            ).ToList();

                        var pageItems = items.OrderBy(x => x.ToDate).ThenByDescending(x => x.ID)
                            .Skip(criteria.Page * criteria.Size)
                            .Take(criteria.Size)
                            .ToList();
                        //return data
                        result.TotalCount = items.Count;
                        result.Items = pageItems.Select(x => MapperHelper.Map<AdGroupCostElementDto>(x)).ToList();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// use this service operation to get AdGrouyp Cost Element by Id
        /// </summary>
        /// <returns>adGroup Cost Element</returns>
        public AdGroupCostElementDto GetAdGroupCostElement(GetAdGroupCostElementRequest request)
        {
            var result = new AdGroupCostElementResultDto();
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var costelemts = item.GetGroupCostElements(group);
                    if (costelemts.Count > 0)
                    {
                        var element = costelemts.FirstOrDefault(x => x.ID == request.CostElementId);
                        return MapperHelper.Map<AdGroupCostElementDto>(element);
                    }
                }
            }
            return null;
        }

        public void AddCostElements(AdGroupCostElementSaveDto saveDto)
        {
            var result = new AdGroupCostElementResultDto();
            var item = CampaignRepository.Get(saveDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == saveDto.AdGroupId);
                if (group != null)
                {
                    var costElement = new AdGroupCostElement
                    {
                        Beneficiary = saveDto.BeneficiaryId.HasValue && saveDto.BeneficiaryId.Value > 0 ? partyRepository.Get(saveDto.BeneficiaryId.Value) : null,
                        CostElement = costElementRepository.Get(saveDto.CostElementId),
                        FromDate = saveDto.FromDate,
                        ToDate = saveDto.ToDate,
                        AdGroup = group,
                        Provider = saveDto.ProviderId.HasValue && saveDto.ProviderId.Value > 0 ? new DPPartner { ID = (int)saveDto.ProviderId } : null,
                        CostModelWrapper = saveDto.CostModelWrapperId.HasValue ? costModelWrapperRepository.Get(saveDto.CostModelWrapperId.Value) : group.CostModelWrapper
                    };
                    costElement.Scope = (AdGroupCostElementScope)costElement.CostElement.Scope; ;
                    if (costElement.Provider != null)
                    {
                        costElement.Scope = AdGroupCostElementScope.DataProvider;

                    }
                    costElement.SetCostElementValue(saveDto.Value, costElement.CostModelWrapper);
                    item.AddGroupCostElement(costElement);
                }
            }
            CampaignRepository.Save(item);
         
        }
        public void UpdateCostElements(AdGroupCostElementSaveDto saveDto)
        {
            var result = new AdGroupCostElementResultDto();
            var item = CampaignRepository.Get(saveDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == saveDto.AdGroupId);
                if (group != null)
                {
                    var costElement = new AdGroupCostElement
                    {
                        ID = (int)saveDto.ID,
                        Beneficiary = saveDto.BeneficiaryId.HasValue && saveDto.BeneficiaryId.Value > 0 ? partyRepository.Get(saveDto.BeneficiaryId.Value) : null,
                        CostElement = costElementRepository.Get(saveDto.CostElementId),
                        FromDate = saveDto.FromDate,
                        ToDate = saveDto.ToDate,
                        AdGroup = group,
                        Provider = saveDto.ProviderId.HasValue && saveDto.ProviderId.Value > 0 ? new DPPartner { ID = (int)saveDto.ProviderId } : null,
                        CostModelWrapper = saveDto.CostModelWrapperId.HasValue ? costModelWrapperRepository.Get(saveDto.CostModelWrapperId.Value) : group.CostModelWrapper,
                    };
                    costElement.Scope = (AdGroupCostElementScope)costElement.CostElement.Scope; ;
                    if (costElement.Provider != null)
                    {
                        costElement.Scope = AdGroupCostElementScope.DataProvider;

                    }
                    item.UpdateGroupCostElement(costElement, saveDto.Value);
                }
            }
            CampaignRepository.Save(item);
           
        }
        public void RemoveCostElements(AdGroupCostElementSaveDto saveDto)
        {
            var result = new AdGroupCostElementResultDto();
            var item = CampaignRepository.Get(saveDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == saveDto.AdGroupId);
                if (group != null)
                {
                    var costelemts = item.GetGroupCostElements(group);
                    if (costelemts.Count > 0)
                    {
                        var element = costelemts.FirstOrDefault(x => x.ID == saveDto.ID);
                        item.RemoveGroupCostElement(element);
                    }
                }
            }
        }

        #endregion

        #region Dynamic Bidding

        public AdGroupDynamicBiddingConfigResultDto GetAdGroupDynamicBiddingConfigs(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdGroupCostElementCriteria criteria)
        {
            var result = new AdGroupDynamicBiddingConfigResultDto { TotalCount = 0, Items = new List<AdGroupDynamicBiddingConfigDto>() };

            var group = adGroupRepository.Get(criteria.AdGroupId);



            var AdGroupDynamicBiddingConfigs = group.AdGroupDynamicBiddingConfigs.ToList();
            if (AdGroupDynamicBiddingConfigs.Count > 0)
            {

                var pageItems = AdGroupDynamicBiddingConfigs.OrderByDescending(x => x.ID)
                    .Skip(criteria.Page * criteria.Size)
                    .Take(criteria.Size)
                    .ToList();
                //return data
                result.TotalCount = AdGroupDynamicBiddingConfigs.Count;
                result.Items = pageItems.Select(x => MapperHelper.Map<AdGroupDynamicBiddingConfigDto>(x)).ToList();
            }
            return result;
        }

        /// <summary>
        /// use this service operation to get AdGrouyp Cost Element by Id
        /// </summary>
        /// <returns>GetAdGroupDynamicBiddingConfig</returns>
        public AdGroupDynamicBiddingConfigDto GetAdGroupDynamicBiddingConfig(GetAdGroupDynamicBiddingConfigRequest request)
        {
            var group = adGroupRepository.Get(request.AdgroupId);


            if (group != null)
            {

                var element = group.AdGroupDynamicBiddingConfigs.FirstOrDefault(x => x.ID == request.ConfigId);
                return MapperHelper.Map<AdGroupDynamicBiddingConfigDto>(element);
            }

            return null;
        }

        public void AddDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto)
        {
            var group = adGroupRepository.Get(saveDto.AdGroupId);


            if (group != null)
            {
                var costElement = new AdGroupDynamicBiddingConfig
                {
                    ID = 0,

                    BidOptimizationValue = saveDto.BidOptimizationValue,
                    DefaultBidPrice = saveDto.DefaultBidPrice,
                    MaxBidPrice = saveDto.MaxBidPrice,
                    MinBidPrice = saveDto.MinBidPrice,
                    BidStep = saveDto.BidStep,
                    KeepBiddingAtMinimum = saveDto.KeepBiddingAtMinimum,
                    Type = saveDto.Type,
                    AdGroup = group,

                };

                group.AddGroupDynamicBidding(costElement);
                adGroupRepository.Save(group);

                //_AdGroupDynamicBiddingConfigRepository.Save(costElement);
            }


            //  return null;
        }
        public void UpdateDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto)
        {
            var group = adGroupRepository.Get(saveDto.AdGroupId);


            if (group != null)
            {
                var costElement = new AdGroupDynamicBiddingConfig
                {
                    ID = (int)saveDto.ID,

                    BidOptimizationValue = saveDto.BidOptimizationValue,
                    DefaultBidPrice = saveDto.DefaultBidPrice,
                    MaxBidPrice = saveDto.MaxBidPrice,
                    MinBidPrice = saveDto.MinBidPrice,
                    BidStep = saveDto.BidStep,
                    KeepBiddingAtMinimum = saveDto.KeepBiddingAtMinimum,
                    Type = saveDto.Type,
                    AdGroup = group,

                };


                group.UpdateGroupDynamicBidding(costElement);
                adGroupRepository.Save(group);
            }


            //  return null;
        }
        public void RemoveDynamicBiddingConfig(AdGroupDynamicBiddingConfigSaveDto saveDto)
        {

            var group = adGroupRepository.Get(saveDto.AdGroupId);


            if (group != null)
            {
                var configs = group.AdGroupDynamicBiddingConfigs;
                if (configs != null && configs.Count > 0)
                {
                    var element = configs.FirstOrDefault(x => x.ID == saveDto.ID);
                    group.RemoveGroupDynamicBidding(element);
                    adGroupRepository.Save(group);

                }
            }

        }

        #endregion
        #region Fees

        public AdGroupFeeResultDto GetAdGroupFees(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdGroupCostElementCriteria criteria)
        {
            var result = new AdGroupFeeResultDto { TotalCount = 0, Items = new List<AdGroupFeeDto>() };
            var item = CampaignRepository.Get(criteria.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == criteria.AdGroupId);
                if (group != null)
                {
                    // get ad group cost elements and apply the filter on it 
                    var costelemts = item.GetGroupFees(group);
                    if (costelemts.Count > 0)
                    {
                        var items = item.GetGroupFees(group).Where(
                            x =>
                            (criteria.DataFrom == null || x.FromDate > criteria.DataFrom) &&
                            //(!x.ToDate.HasValue)
                            (criteria.DataTo == null || x.ToDate < criteria.DataTo)
                            ).ToList();

                        var pageItems = items.OrderBy(x => x.ToDate).ThenByDescending(x => x.ID)
                            .Skip(criteria.Page * criteria.Size)
                            .Take(criteria.Size)
                            .ToList();
                        //return data
                        result.TotalCount = items.Count;
                        result.Items = pageItems.Select(x => MapperHelper.Map<AdGroupFeeDto>(x)).ToList();
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// use this service operation to get AdGrouyp Cost Element by Id
        /// </summary>
        /// <returns>adGroup Cost Element</returns>
        public AdGroupFeeDto GetAdGroupFee(int campaignId, int groupId, int costElemtnId)
        {
            var result = new AdGroupFeeResultDto();
            var item = CampaignRepository.Get(campaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == groupId);
                if (group != null)
                {
                    var costelemts = item.GetGroupCostElements(group);
                    if (costelemts.Count > 0)
                    {
                        var element = costelemts.FirstOrDefault(x => x.ID == costElemtnId);
                        return MapperHelper.Map<AdGroupFeeDto>(element);
                    }
                }
            }
            return null;
        }

        public AdGroupFee AddFees(AdGroupFeeDto saveDto, AdGroup group, ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign item)
        {


            if (group != null)
            {
                var costElement = new AdGroupFee
                {
                    Beneficiary = saveDto.BeneficiaryId.HasValue && saveDto.BeneficiaryId.Value > 0 ? partyRepository.Get(saveDto.BeneficiaryId.Value) : null,
                    Fee = _FeeRepository.Get(saveDto.FeeId),
                    FromDate = saveDto.FromDate,
                    //ToDate = saveDto.ToDate,
                    AdGroup = group,
                    Provider = saveDto.ProviderId > 0 ? new DPPartner { ID = (int)saveDto.ProviderId } : null,
                    CostModelWrapper = group.CostModelWrapper
                };
                costElement.Scope = AdGroupCostElementScope.Inventory;
                if (costElement.Provider != null)
                {
                    costElement.Scope = AdGroupCostElementScope.DataProvider;

                }
                costElement.SetCostElementValue(saveDto.Value, costElement.CostModelWrapper);
                item.AddGroupFee(costElement);
            }

            return null;
        }
        public AdGroupFee UpdateFees(AdGroupFeeDto saveDto, AdGroup group, ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign item)
        {


            if (group != null)
            {
                var costElement = new AdGroupFee
                {
                    ID = (int)saveDto.ID,
                    Beneficiary = saveDto.BeneficiaryId.HasValue && saveDto.BeneficiaryId.Value > 0 ? partyRepository.Get(saveDto.BeneficiaryId.Value) : null,
                    Fee = _FeeRepository.Get(saveDto.FeeId),
                    FromDate = saveDto.FromDate,
                    // ToDate = saveDto.ToDate,
                    AdGroup = group,
                    Provider = saveDto.ProviderId > 0 ? new DPPartner { ID = (int)saveDto.ProviderId } : null,
                    CostModelWrapper = group.CostModelWrapper,
                };
                costElement.Scope = AdGroupCostElementScope.Inventory;
                if (costElement.Provider != null)
                {
                    costElement.Scope = AdGroupCostElementScope.DataProvider;

                }
                costElement.SetCostElementValue(saveDto.Value, costElement.CostModelWrapper);
                item.UpdateGroupFee(costElement, saveDto.Value);
            }


            return null;
        }
        public void RemoveFees(AdGroupFeeDto saveDto, AdGroup group, ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign item)
        {


            if (group != null)
            {
                var costelemts = item.GetGroupFees(group);
                if (costelemts.Count > 0)
                {
                    var element = costelemts.FirstOrDefault(x => x.ID == saveDto.ID);
                    item.RemoveGroupFee(element);
                }
            }

        }

        #endregion
        #region Adrequest
        public TypesPlatformsVersions GetAdRequestTypes_Platforms_Versions()
        {
            var AdRequestTypePlatformVersions = _AdRequestTypePlatformVersionRepository.GetAll().ToList();
            IList<AdRequestTypePlatformVersionDto> AdRequest_TypePlatform_Versions = new List<AdRequestTypePlatformVersionDto>();
            IList<AdRequestTypeDto> AdRequestType = new List<AdRequestTypeDto>();
            IList<AdRequestPlatformDto> AdRequestPlatform = new List<AdRequestPlatformDto>();

            AdRequest_TypePlatform_Versions = AdRequestTypePlatformVersions.Select(M => MapperHelper.Map<AdRequestTypePlatformVersionDto>(M)).ToList();
            foreach (var item in AdRequest_TypePlatform_Versions)
            {

                //item.AdRequestType.ID

                if (AdRequestType.Where(M => M.ID == item.AdRequestType.ID).SingleOrDefault() == null)
                {
                    AdRequestType.Add(item.AdRequestType);
                }

                if (AdRequestPlatform.Where(M => M.ID == item.AdRequestPlatform.ID).SingleOrDefault() == null)
                {
                    AdRequestPlatform.Add(item.AdRequestPlatform);
                }

            }

            return new TypesPlatformsVersions { All = AdRequest_TypePlatform_Versions, Types = AdRequestType, Platforms = AdRequestPlatform };
        }

        //public List<AdRequestTargetingDto> GetAdRequestTargetings(int campaignId, int adGroupId)
        //{
        //   var item = CampaignRepository.Get(campaignId);
        //    CheckCampaign(item);
        //    ValidateCampaign(item);

        //    if (item.IsValid)
        //    {


        //        var objs = this._AdRequestTargetingRepository.Query(M => M.AdGroup.ID == adGroupId).ToList();
        //        if (objs != null)
        //        {


        //              var   returnValue = objs.Select(obj => MapperHelper.Map<AdRequestTargetingDto>(obj)).ToList();
        //                //TODO:Osaleh to refractor this code and use auto Mapper

        //            return returnValue;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    return null;
        //}

        public ValueMessageWrapper<bool> SaveAdRequestTargeting(AdRequestTargetingDto dto)
        {
            var item = CampaignRepository.Get(dto.campaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            AdRequestTargeting targeting = new AdRequestTargeting();
            if (item.IsValid)
            {
                if (dto.ID > 0)
                {
                    targeting = this._AdRequestTargetingRepository.Get(dto.ID);
                }
                else
                {

                    targeting = new AdRequestTargeting();
                    targeting.Type = targetingTypeRepository.Get(10);

                }
                targeting.AdRequestPlatform = new AdRequestPlatform { ID = dto.AdRequestPlatformId };
                targeting.AdRequestType = new AdRequestType { ID = dto.AdRequestTypeId };
                targeting.AdGroup = item.AdGroups.Where(M => M.ID == dto.AdGroupId).SingleOrDefault<AdGroup>();
                targeting.MinimumVersion = dto.MinimumVersion;
                this._AdRequestTargetingRepository.Save(targeting);
                return  ValueMessageWrapper.Create(true);
            }
            return ValueMessageWrapper.Create(false);
        }
        public ValueMessageWrapper<bool> DeleteAdRequestTargeting(ValueMessageWrapper<int> Id)
        {
            if (Id.Value > 0)
            {
                var item = _AdRequestTargetingRepository.Get(Id.Value);
                if (item != null & item.AdGroup.ID > 0)
                {
                    this._AdRequestTargetingRepository.Remove(item);
                }
                return ValueMessageWrapper.Create(true);
            }
            return ValueMessageWrapper.Create(false);


        }
        public AdRequestTargetingDtoResultDto GetAdRequestTargetings(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdRequestCriteria criteria)
        {
            var result = new AdRequestTargetingDtoResultDto { Items = new List<AdRequestTargetingDto>(), TotalCount = 0 };
            var items = _AdRequestTargetingRepository.Query(x => x.AdGroup.ID == criteria.AdGroupId);
            var pageItems = items.OrderBy(x => x.ID)
                .Skip(criteria.Page * criteria.Size)
                .Take(criteria.Size)
                .ToList();
            result.Items = pageItems.Select(x => MapperHelper.Map<AdRequestTargetingDto>(x)).ToList();
            result.TotalCount = items.Count();
            return result;
        }
        #endregion


        #region  ImpressionMetricTargetings

        public IList<ImpressionMetricDto> GetImpressionMetrics()
        {
            var impMetrics = _IImpressionMetricRepository.GetAll().ToList();
            IList<ImpressionMetricDto> results = new List<ImpressionMetricDto>();


            results = impMetrics.Select(M => MapperHelper.Map<ImpressionMetricDto>(M)).ToList();

            return results;
        }
        public ValueMessageWrapper<bool> SaveImpressionMetricTargeting(ImpressionMetricTargetingDto dto)
        {
            var item = CampaignRepository.Get(dto.campaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            ImpressionMetricTargeting targeting = new ImpressionMetricTargeting();
            if (item.IsValid)
            {
                if (dto.ID > 0)
                {
                    targeting = this._ImpressionMetricTargetingRepository.Get(dto.ID);
                }
                else
                {

                    targeting = new ImpressionMetricTargeting();
                    targeting.Type = targetingTypeRepository.Get(13);

                }
                targeting.ImpressionMetric = new ImpressionMetric { ID = dto.ImpressionMetric.ID };
                if (dto.MetricVendor.ID > 0)
                {
                    targeting.MetricVendor = new MetricVendor { ID = dto.MetricVendor.ID };
                }
                else
                {
                    targeting.MetricVendor = null;

                }
                targeting.MinValue = dto.MinValue;
                targeting.Ignore = dto.Ignore;
                targeting.AdGroup = item.AdGroups.Where(M => M.ID == dto.AdGroupId).SingleOrDefault<AdGroup>();

                this._ImpressionMetricTargetingRepository.Save(targeting);
                return  ValueMessageWrapper.Create(true);
            }
            return ValueMessageWrapper.Create(false);
        }
        public ValueMessageWrapper<bool> DeleteImpressionMetricTargeting(ValueMessageWrapper<int> Id)
        {
            if (Id.Value > 0)
            {
                var item = _ImpressionMetricTargetingRepository.Get(Id.Value);
                if (item != null & item.AdGroup.ID > 0)
                {
                    this._ImpressionMetricTargetingRepository.Remove(item);
                }
                return ValueMessageWrapper.Create(true);
            }
            return ValueMessageWrapper.Create(false);


        }
        public ImpressionMetricDto GetImpressionMetric(ValueMessageWrapper<int> Id)
        {
            ImpressionMetric ImpressionMetric = _IImpressionMetricRepository.Get(Id.Value);
            if (ImpressionMetric != null)
            {
                ImpressionMetricDto item = MapperHelper.Map<ImpressionMetricDto>(ImpressionMetric);
                return item;
            }
            return null;

        }
        public ImpressionMetricTargetingResultDto GetImpressionMetricTargetings(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.ImpressionMetricCriteria criteria)
        {
            var result = new ImpressionMetricTargetingResultDto { Items = new List<ImpressionMetricTargetingDto>(), TotalCount = 0 };
            var items = _ImpressionMetricTargetingRepository.Query(x => x.AdGroup.ID == criteria.AdGroupId);
            var pageItems = items.OrderBy(x => x.ID)
                .Skip(criteria.Page * criteria.Size)
                .Take(criteria.Size)
                .ToList();
            result.Items = pageItems.Select(x => MapperHelper.Map<ImpressionMetricTargetingDto>(x)).ToList();
            result.TotalCount = items.Count();
            return result;
        }
        public ImpressionMetricTargetingDto GetImpressionMetricTargeting(ValueMessageWrapper<int> TargetingId)
        {
            var result = new ImpressionMetricTargetingResultDto { Items = new List<ImpressionMetricTargetingDto>(), TotalCount = 0 };
            var item = _ImpressionMetricTargetingRepository.Query(x => x.ID == TargetingId.Value).SingleOrDefault();

            var dto = MapperHelper.Map<ImpressionMetricTargetingDto>(item);



            return dto;
        }

        public List<MetricVendorDto> getMetricVendors()
        {
            List<MetricVendorDto> MetricVendors =
                _metricVendorRepository.GetAll().Select(x => MapperHelper.Map<MetricVendorDto>(x)).ToList();

            return MetricVendors;
        }

        #endregion
        public AdGroupSettingsDto GetAdGroupSettings(CampaignIdAdgroupIdMessage request)
        {
            var result = new AdGroupSettingsDto();
            // var item = CampaignRepository.Get(campaignId);
            // CheckCampaign(item);
            //ValidateCampaign(item);
            var group = adGroupRepository.Get(request.AdgroupId);
            var item = group.Campaign;
            ValidateCampaign(item);
            // if (item.IsValid)
            //{
            //  var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupId);
            if (group != null)
            {
                result.DailyBudget = group.DailyBudget;
                result.Budget = group.Budget;
                result.CampaignBudget = item.Budget;
                result.CampaignId = group.Campaign.ID;
                result.AdGroupId = group.ID;
            }
            //}
            return result;
        }

        public ValueMessageWrapper<bool> SaveAdGroupSettings(AdGroupSettingsDto settingsDto)
        {
            var item = CampaignRepository.Get(settingsDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == settingsDto.AdGroupId);
                if (group != null)
                {
                    if (settingsDto.DailyBudget.HasValue)
                    {
                        if (settingsDto.DailyBudget != -1)
                        {
                            group.DailyBudget = settingsDto.DailyBudget;
                        }

                    }
                    else
                    {
                        group.DailyBudget = null;
                    }

                    if (settingsDto.Budget.HasValue)
                    {
                        if (settingsDto.Budget != -1)
                        {
                            group.Budget = settingsDto.Budget;
                        }

                    }
                    else
                    {
                        group.Budget = null;
                    }


                }
            }
            CampaignRepository.Save(item);
            return ValueMessageWrapper.Create(true);
        }

        public AdGroupSearchDto QueryGroupsByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.AdGroupCriteria wcriteria)
        {
            
            AdGroupCriteria criteria = new AdGroupCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var item = CampaignRepository.Get(criteria.CampaignId);
            CheckCampaign(item);
            if (item.CampaignType != criteria.CampaignType && item.CampaignType != criteria.CampaignOtherType)
            {
                throw new DataNotFoundException();
            }
            ValidateCampaign(item);
            //criteria.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            var Permissions = OperationContext.Current.UserInfo<AdFalconUserInfo>().Permissions;
            if (!Domain.Configuration.IsAdmin)
                criteria.Permissions = Permissions != null ? Permissions.ToList() : new List<int>();

            if (item.IsValid)
            {
                var groups = item.GetGroups().ToList().OrderByDescending(x => x.ID);
                var list = groups.Where(criteria.GetWhere());
                if (list != null)
                {
                    var returnvalue = new AdGroupSearchDto();
                    returnvalue.Items = list.Skip((criteria.Page - 1) * criteria.Size).Take(criteria.Size).ToList().Select(adGroup => MapperHelper.Map<AdGroupListDto>(adGroup)).ToList();
                    returnvalue.TotalCount = list.Count();
                    returnvalue.CampaignName = item.Name;
                    returnvalue.AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty;

                    returnvalue.AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0;


                    returnvalue.AdvertiserAccountName = item.AdvertiserAccount != null ? item.AdvertiserAccount.Name.ToString() : string.Empty;

                    returnvalue.AdvertiserAccountId = item.AdvertiserAccount != null ? item.AdvertiserAccount.ID : 0;
                    #region Performance
                    var performance = new PerformanceCriteria()
                    {
                        FromDate = criteria.DateFrom,
                        ToDate = criteria.DateTo
                    };
                    performance.Ids = returnvalue.Items.Select(obj => obj.Id).ToList();
                    var performances = summaryRepository.GetAdGroupsPerformance(performance);
                    var idStatus = summaryRepository.GetAdsByAdGroups(performance);
                    //AdvertisorEstimatorCalculation test = new AdvertisorEstimatorCalculation(new DateTime(2010, 4, 30), Framework.Utilities.Environment.GetServerTime(), EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.AdGroup);
                    //IList<CampaignCardinalityEstimatorDto> res = test.GetCardinalityEsitimator(returnvalue.Items.Select(x => x.Id).ToList());

                    //DateTime from = criteria.DateFrom == null ? new DateTime(2009, 1, 1) : (DateTime)criteria.DateFrom;
                    //DateTime to = criteria.DateTo == null ? Framework.Utilities.Environment.GetServerDate() : (DateTime)criteria.DateTo;
                    //if (criteria.DateFrom == null)
                    //{
                    //    foreach (var adGroupListDto in returnvalue.Items)
                    //    {
                    //        if (adGroupListDto.CreationDate.Date<=)
                    //        {
                    //            var firstDayOfMonth = new DateTime(resultCamp.StartDate.Year, resultCamp.StartDate.Month, 1);
                    //            var lastDayOfMonth = Framework.Utilities.Environment.GetServerDate().AddMonths(1).AddDays(-1);
                    //            AdvertisorEstimatorCalculation AdvertisorEstimatorCalculationtemp = new AdvertisorEstimatorCalculation(firstDayOfMonth, lastDayOfMonth, EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.Campaign);
                    //            string Idtemps = "" + resultCamp.ID;
                    //            var restemo = AdvertisorEstimatorCalculationtemp.GetCardinalityEsitimator(Idtemps);
                    //            res.AddRange(restemo);

                    //        }


                    //    }
                    //}
                    //else
                    //{


                    //    AdvertisorEstimatorCalculation AdvertisorEstimatorCalculation = new AdvertisorEstimatorCalculation(from, to, EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.Campaign);
                    //    string Ids = string.Join(",", returnList.Select(x => x.Id).ToList());

                    //    res = AdvertisorEstimatorCalculation.GetCardinalityEsitimator(Ids).ToList();
                    //}

                    DateTime from = criteria.DateFrom == null ? DateTime.MinValue : (DateTime)criteria.DateFrom;
                    DateTime to = criteria.DateTo == null ? Framework.Utilities.Environment.GetServerDate() : (DateTime)criteria.DateTo;
                    AdvertisorEstimatorCalculation advertisorCalc = new AdvertisorEstimatorCalculation(from, to, EstimatorCalculationPeriodType.Accumulated, EstimatorCalculationType.AdGroup, Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value);
                    advertisorCalc.IdPerRange = new Dictionary<int, DateEstimatorRange>();
                    string Idstr = string.Empty;
                    if (criteria.DateFrom == null && criteria.Page > 0)

                    {
                        foreach (var adGroupListDto in returnvalue.Items)
                        {

                            //load Campaign Performance
                            //adGroupListDto.Performance = performances.FirstOrDefault(performanceItem => performanceItem.AdsGroupID == adGroupListDto.Id);



                            var firstDayOfMonth = new DateTime(adGroupListDto.CreationDate.Year, adGroupListDto.CreationDate.Month, 1);
                            var lastDayOfMonth = new DateTime(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month, DateTime.DaysInMonth(Framework.Utilities.Environment.GetServerDate().Year, Framework.Utilities.Environment.GetServerDate().Month));
                            advertisorCalc.IdPerRange.Add(adGroupListDto.Id, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });



                            //string Ids = string.Join(",", returnList.Select(x => x.Id).ToList());
                            // Idstr = "" + adGroupListDto.Id+",";
                            //IList<CampaignCardinalityEstimatorDto> res = test.GetCardinalityEsitimator(Idstr, true);
                            ////  load Campaign Performance
                            //adGroupListDto.Performance.UniqueClicks = res.Select(x => x.unique_clicks).FirstOrDefault();

                            //adGroupListDto.Performance.UniqueImp = res.Select(x => x.unique_impressions).FirstOrDefault();


                            ////load Ad Status
                            //var camAds = idStatus.Where(ad => ad.AdgroupId == adGroupListDto.Id).ToList();
                            //adGroupListDto.Status = summaryRepository.CalculateStatus(camAds);
                        }
                        Idstr = string.Join(",", returnvalue.Items.Select(x => x.Id).ToList());
                    }
                    else if (criteria.Page > 0)
                    {

                        foreach (var adGroupListDto in returnvalue.Items)
                        {
                            var firstDayOfMonth = new DateTime(adGroupListDto.CreationDate.Year, adGroupListDto.CreationDate.Month, 1);
                            var lastDayOfMonth = to;
                            if (adGroupListDto.CreationDate.Date < from.Date)
                            {
                                firstDayOfMonth = from;
                            }


                            advertisorCalc.IdPerRange.Add(adGroupListDto.Id, new DateEstimatorRange { DateFrom = firstDayOfMonth, DateTo = lastDayOfMonth });


                        }
                        Idstr = string.Join(",", returnvalue.Items.Select(x => x.Id).ToList());

                    }
                    if (criteria.Page > 0)
                    {
                        IList<DateTime> datesFrom = new List<DateTime>();
                        IList<DateTime> datesTo = new List<DateTime>();
                        foreach (var key in advertisorCalc.IdPerRange)
                        {

                            datesFrom.Add(key.Value.DateFrom);
                            datesTo.Add(key.Value.DateTo);
                        }
                        datesFrom = datesFrom.OrderBy(M => M).ToList();
                        datesTo = datesTo.OrderByDescending(M => M).ToList();

                        if (datesTo != null && datesTo.Count > 0)
                            advertisorCalc.DateTo = datesTo[0];
                        if (datesFrom != null && datesFrom.Count > 0)
                            advertisorCalc.DateFrom = datesFrom[0];
                        IList<CampaignCardinalityEstimatorDto> res = new List<CampaignCardinalityEstimatorDto>();

                        if (!string.IsNullOrEmpty(Idstr))
                            res = advertisorCalc.GetCardinalityEsitimator(Idstr, true);
                        foreach (var adGroupListDto in returnvalue.Items)
                        {
                            adGroupListDto.Performance = performances.FirstOrDefault(performanceItem => performanceItem.AdsGroupID == adGroupListDto.Id);
                            var resItem = res.FirstOrDefault(M => M.AdGroupId == adGroupListDto.Id);

                            if (resItem != null)
                            {
                                adGroupListDto.Performance.UniqueClicks = resItem.unique_clicks;

                                adGroupListDto.Performance.UniqueImp = resItem.unique_impressions;

                            }
                            //load Ad Status
                            var camAds = idStatus.Where(ad => ad.AdgroupId == adGroupListDto.Id).ToList();
                            adGroupListDto.Status = summaryRepository.CalculateStatus(camAds);

                        }
                    }
                    //load Campaign Performance
                    var campaignPerformanceCriteria = new PerformanceCriteria()
                    {
                        Ids = new List<int>() { criteria.CampaignId }
                    };
                    performance.Ids = returnvalue.Items.Select(obj => obj.Id).ToList();
                    var campaignPrformance = summaryRepository.GetCampaignPerformance(campaignPerformanceCriteria);
                    returnvalue.Performance = campaignPrformance;
                    #endregion
                    return returnvalue;
                }
            }
            return null;
        }

        public ValueMessageWrapper<bool> DeleteGroups(CampaignIdAdgroupIdsMessage request)
        {
            if (request.AdgroupIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    foreach (var adGroupId in request.AdgroupIds)
                    {
                        //get Ad Group Object
                        var adGroupObj = item.GetGroups().Where(adGroup => adGroup.ID == adGroupId).ToList();
                        if (adGroupObj.Count > 0)
                        {
                            item.RemoveGroup(adGroupObj.First());
                        }
                    }

                }
            }
            return ValueMessageWrapper.Create(true);
        }

        public void RunGroups(CampaignIdAdgroupIdsMessage request)
        {
            if (request.AdgroupIds!= null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    foreach (var adGroupId in request.AdgroupIds)
                    {
                        //get Ad Group Object
                        var adGroupObj = item.GetGroups().Where(adGroup => adGroup.ID == adGroupId).ToList();
                        if (adGroupObj.Count > 0)
                        {
                            item.ResumeGroup(adGroupObj.First());
                        }
                    }
                }
            }
        }

        public void PauseGroups(CampaignIdAdgroupIdsMessage request)
        {
            if (request.AdgroupIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    foreach (var adGroupId in request.AdgroupIds)
                    {
                        //get Ad Group Object
                        var adGroupObj = item.GetGroups().Where(adGroup => adGroup.ID == adGroupId).ToList();
                        if (adGroupObj.Count > 0)
                        {
                            item.PauseGroup(adGroupObj.First());
                        }
                    }
                }
            }
        }

        public ValueMessageWrapper<int> SaveAdGroup(SaveAdGroupRequest request)
        {
            var item = CampaignRepository.Get(request.AdGroup.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = MapperHelper.Map<AdGroup>(request.AdGroup);
                group.Campaign = item;
                group.RunAllExchanges = true;
                group.LogAdMarkup = item.LogAdMarkup;
                group.AllowOpenAuction = true;
                group.UniqueId = Guid.NewGuid().ToString();
                group.CreationDate = Framework.Utilities.Environment.GetServerTime();
                group.BiddingStrategy = BiddingStrategy.Fixed;


                //Default for  Conversion

                group.ConversionSetting = ConversionSetting.CountingFirst;
                group.ConversionType = ConversionType.Click;
                group.ClickAttribuation = 14;
                group.ViewAttribuation = 14;
                group.CountingAttribuation = 5;
                group.CountingTypeAttribuation = CountingTypeAttribuation.Minutes;

                //group.Status = AdGroupStatus.Empty;
                group.Objective = new AdGroupObjective
                {
                    AdGroup = group,
                    AdAction = adActionTypeRepository.Get((int)request.AdGroup.ActionTypeId),
                    AdType = request.AdGroup.TypeId.HasValue ? adTypeRepository.Get((int)request.AdGroup.TypeId.Value) : null,
                    Objective = adGroupObjectiveTypeRepository.Get((int)request.AdGroup.ObjectiveTypeId),
                };
                //if (!IsAllowedGroup(group))
                //{
                //    throw new NotAuthorizedException();
                //}
                item.AddGroup(group);
                //Add Default Cost Elments
                // group.SetAccountCostElmentsSaved();
                if (request.ReturnId)
                {
                    CampaignRepository.Save(item);
                }

                group.SetAccountFeesSaved(OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value);
               // var contextualSegmentTargetinglist = group.Targetings.OfType<ContextualSegmentTargeting>().Where(x => x.IsBrandSafty == true).ToList();
                bool gv_safe_found = false;
                int gv_safe_code = 7419;
                var segment_gv_safe = _contextualSegmentRepository.GetSegmentByCode(gv_safe_code);
             
                if (group.Targetings==null)
                {
                    group.Targetings = new List<TargetingBase>();
                }

                if (!gv_safe_found && segment_gv_safe != null)
                {
                    group.Targetings.Add(new ContextualSegmentTargeting { Type = new TargetingType { ID = 18 }, IsBrandSafty = true, Include = true, ContextualSegment = new ContextualSegment { ID = segment_gv_safe.ID } });
                }
                return ValueMessageWrapper.Create(group.ID);
            }
            return ValueMessageWrapper.Create(0);
        }


        public ValueMessageWrapper<int> SaveAdGroupForWeb(SaveAdGroupRequest request)
        {
            var item = CampaignRepository.Get(request.AdGroup.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = MapperHelper.Map<AdGroup>(request.AdGroup);
                group.Campaign = item;
                group.RunAllExchanges = true;
                group.LogAdMarkup = item.LogAdMarkup;
                group.AllowOpenAuction = true;
                group.UniqueId = Guid.NewGuid().ToString();
                group.CreationDate = Framework.Utilities.Environment.GetServerTime();
                group.BiddingStrategy = BiddingStrategy.Fixed;


                //Default for  Conversion

                group.ConversionSetting = ConversionSetting.CountingFirst;
                group.ConversionType = ConversionType.Click;
                group.ClickAttribuation = 14;
                group.ViewAttribuation = 14;
                group.CountingAttribuation = 5;
                group.CountingTypeAttribuation = CountingTypeAttribuation.Minutes;

                //group.Status = AdGroupStatus.Empty;
                group.Objective = new AdGroupObjective
                {
                    AdGroup = group,
                    AdAction = adActionTypeRepository.Get((int)request.AdGroup.ActionTypeId),
                    AdType = request.AdGroup.TypeId.HasValue ? adTypeRepository.Get((int)request.AdGroup.TypeId.Value) : null,
                    Objective = adGroupObjectiveTypeRepository.Get((int)request.AdGroup.ObjectiveTypeId),
                };
                //if (!IsAllowedGroup(group))
                //{
                //    throw new NotAuthorizedException();
                //}
                item.AddGroup(group);
                //Add Default Cost Elments
                // group.SetAccountCostElmentsSaved();
                if (request.ReturnId)
                {
                    CampaignRepository.Save(item);
                }

                group.SetAccountFeesSaved(OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value);
                return ValueMessageWrapper.Create(group.ID);
            }
            return ValueMessageWrapper.Create(0);
        }
        public AdGroupDto GetAdGroup(CampaignIdAdgroupIdMessage request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var adGroupObj = item.GetGroups().Where(adGroup => adGroup.ID == request.AdgroupId).ToList();
                if (adGroupObj.Count > 0)
                {
                    return MapperHelper.Map<AdGroupDto>(adGroupObj.First());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<AudienceSegmentDto> GetContextualSegmentslist(List<ContextualSegmentTargeting> contextualSegmentTargeting)
        {
            List<AudienceSegmentDto> contextualSegmentsList = new List<AudienceSegmentDto>();
            foreach (var segement in contextualSegmentTargeting)
            {
                var contextual = segement.ContextualSegment;
                var dto = MapperHelper.Map<AudienceSegmentDto>(segement.ContextualSegment);
                if (contextual.Parent != null)
                    dto.ParentName = contextual.Name.ToString();
                ContextualSegmentTargeting targ = new ContextualSegmentTargeting();
                dto.Path = targ.AudiencePath(contextual.ID);
                if (contextual.Parent != null && contextual.Parent.Provider != null && contextual.Parent.Provider.Code == targ.getFirstPartyCode())
                {
                    dto.recency = 65535;
                    dto.showrecency = true;

                }
                var target = segement.Include;
                if (target)
                    dto.Condition = "Target";
                else
                    dto.Condition = "Exclude";
                contextualSegmentsList.Add(dto);
            }
            return contextualSegmentsList;
        }

        public TargetingListDto GetTargeting(GetTargetingRequest request)
        {
            WatchingUtil.StartWatch("GetTargetingService");
            // var item = CampaignRepository.Get(campaignId);
            // CheckCampaign(item);
            // ValidateCampaign(item);
            WatchingUtil.StartWatch("adGroupRepository.Get(request.AdgroupId);");
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);
            WatchingUtil.EndWatch();
            var item = adGroupObj.Campaign;
            if (item.CampaignType != request.CampaignType && item.CampaignType != request.CampaignOtherType)
            {
                throw new DataNotFoundException();
            }
            ValidateCampaign(item);
            if (item.IsValid)
            {
                // var adGroupObj = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGroupId);
                WatchingUtil.StartWatch("adGroupObj.Targetings.ToList()");
                var targetings = adGroupObj.Targetings.ToList();
                WatchingUtil.EndWatch();

                WatchingUtil.StartWatch("adGroupObj.GetAudienceSegmentsForExternal");
                var items = adGroupObj.GetAudienceSegmentsForExternal();
                WatchingUtil.EndWatch();
                if (adGroupObj != null)
                {
                    WatchingUtil.StartWatch(" item.GetActiveDiscount()");
                    // get active discount
                    var discount = item.GetActiveDiscount();
                    WatchingUtil.EndWatch();
                    // get is the logged in user is admin
                    var isAdmin = Domain.Configuration.IsAdmin;
                    WatchingUtil.StartWatch(" audienceSegmentTargeting");
                    var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == false).FirstOrDefault();
                    string jsonObj = string.Empty;
                    if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
                    {

                        jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);

                    }
                    WatchingUtil.EndWatch();

                    //ContextualSegment
                    var contextualSegmentTargeting = adGroupObj.Targetings.OfType<ContextualSegmentTargeting>().Where(M => M.IsBrandSafty == false).ToList();
                    List<AudienceSegmentDto> contextualSegmentsList = new List<AudienceSegmentDto>();
                    decimal maxDataBidContextual = 0;
                    if (contextualSegmentTargeting.Count > 0)
                    {
                        contextualSegmentsList = GetContextualSegmentslist(contextualSegmentTargeting);
                        maxDataBidContextual = contextualSegmentTargeting.Max(x => x.ContextualSegment.Price);
                    }

                    //BrandSafetySegment
                    List<AudienceSegmentDto> brandSafetySegmentsList = new List<AudienceSegmentDto>();
                    var BrandSafetySegmentTargeting = adGroupObj.Targetings.OfType<ContextualSegmentTargeting>().Where(M => M.IsBrandSafty == true).ToList();
                    decimal maxDataBidBrandSafety = 0;
                    if (BrandSafetySegmentTargeting.Count > 0)
                    {
                        brandSafetySegmentsList = GetContextualSegmentslist(BrandSafetySegmentTargeting);
                        maxDataBidBrandSafety = BrandSafetySegmentTargeting.Max(x => x.ContextualSegment.Price);
                    }

                    var contextualSegment = new ContextualSegmentTargeting();
                    var contextualFirstPartyCode = contextualSegment.getFirstPartyCode();
                    //if (jsonObj == null)
                    //{
                    //    jsonObj = new group();
                    //}
                    int? targetingConnection = null;
                    targetingConnection = adGroupObj.GetConnectionValue();
                    WatchingUtil.StartWatch(" new TargetingListDto");
                    var returnValue = new TargetingListDto
                    {
                        AdActionTypeDto = MapperHelper.Map<AdActionTypeDto>(adGroupObj.Objective.AdAction),
                        AdType = adGroupObj.Objective.AdType != null ? MapperHelper.Map<AdTypeDto>(adGroupObj.Objective.AdType) : null,
                        Bid = adGroupObj.GetReadableBid(),
                        CampaignType = item.CampaignType,
                        AudianceDiscountPrice = adGroupObj.AudianceDiscountPrice * 1000,
                        DiscountedBid = adGroupObj.DiscountedBid,
                        CostModelWrapper = (int)adGroupObj.CostModelWrapperEnum,
                        CampaignName = item.Name,
                        //switch off external audience list
                        //CountExternalAudienceList = getCountAudiancesUsedInIntegrationAll(ValueMessageWrapper.Create(adGroupObj.ID)).Value,
                        DataPriceAudienceSegment = getDataBidAudiancesUsedInIntegrationAll(ValueMessageWrapper.Create(adGroupObj.ID)),
                        AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty,
                        AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0,
                        ViewabilityVendorId = adGroupObj.ViewabilityVendorId,
                        TargetingConnectionType = targetingConnection,
                        AdvertiserAccountName = item.Advertiser != null ? item.AdvertiserAccount.Name.ToString() : string.Empty,
                        AdvertiserAccountId = item.Advertiser != null ? item.AdvertiserAccount.ID : 0,

                        ContextualFirstPartyCode= contextualFirstPartyCode,
                        ContextualSegments = contextualSegmentsList,
                        BrandSafetySegments= brandSafetySegmentsList,
                        RunAllExchanges = adGroupObj.RunAllExchanges,
                        AllowOpenAuction = adGroupObj.AllowOpenAuction,
                        AdGroupName = adGroupObj.Name,
                        TrackInstalls = adGroupObj.TrackInstalls,
                        OpenInExternalBrowser = adGroupObj.OpenInExternalBrowser,

                        Budget = adGroupObj.Budget,
                        DataBid = adGroupObj.DataBid,
                        MaxDataBid = adGroupObj.MaxDataBid,
                        BiddingStrategy = adGroupObj.BiddingStrategy,
                        DailyBudget = adGroupObj.DailyBudget,
                        groupAudianceString = jsonObj,
                        DisableProxyTraffic = adGroupObj.DisableProxyTraffic,
                        IsWifi = adGroupObj.IsWifi,
                        IsCellular = adGroupObj.IsCellular,

                        DiscountDto = discount == null ? null : MapperHelper.Map<DiscountDto>(discount),
                        CampaignCostModelWrapper = item.CostModelWrapper.HasValue ? (int?)item.CostModelWrapper : (int?)null,
                        IsClientLocked = item.IsClientLocked && !isAdmin,
                        
                        IsPricingModelChanged = adGroupObj.IsCostModelChanged,
                        ConversionSetting = adGroupObj.ConversionSetting,
                        ConversionType = adGroupObj.ConversionType,
                        ViewAttribuation = adGroupObj.ViewAttribuation,
                        ClickAttribuation = adGroupObj.ClickAttribuation,
                        CountingAttribuation = adGroupObj.CountingAttribuation,
                        CountingTypeAttribuation = adGroupObj.CountingTypeAttribuation,
                        MaxDataBidContextual= maxDataBidContextual,
                        MaxDataBidBrandSafety=maxDataBidBrandSafety

                    };
                    WatchingUtil.EndWatch();
                    returnValue.IsHasAds = adGroupRepository.AdGroupHasAds(request.AdgroupId);
                    List<AdGroupBidModifierDto> mappeditems = new List<AdGroupBidModifierDto>();
                    WatchingUtil.StartWatch("AdGroupBidModifiers");
                    if (adGroupObj.AdGroupBidModifiers!=null)
                    mappeditems = adGroupObj.AdGroupBidModifiers.Where(x => !x.IsDeleted).Select(x => MapperHelper.Map<AdGroupBidModifierDto>(x)).ToList();
                    WatchingUtil.EndWatch();

                    returnValue.AdGroupBidModifiersDto = mappeditems;
//removed to speed up the process
                    /*if (!IsllowedAdvertiserForCamp(item))
                    {
                        returnValue.IsClientReadOnly = true;
                    }*/

                    if (adGroupObj.Objective.AdAction != null && adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.VideoStreaming)
                    {
                        returnValue.IsVideoActionType = true;

                    }
                    RefereseCalculateAdPosition(adGroupObj, returnValue);
                    WatchingUtil.StartWatch("GeoFencing");
                    if (adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().FirstOrDefault() != null)

                    {
                        var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > 1000).ToList();
                        if (results != null && results.Count > 0)
                            returnValue.AllowGeofencing = true;

                    }
                    WatchingUtil.EndWatch();

                    //removed to speed up the process

                    /* if (!IsAllowedGroup(adGroupObj))
                     {
                         returnValue.IsClientLocked = true;
                     }
                     if (!IsllowedAdvertiserForCamp(item) )
                     {
                         returnValue.IsClientReadOnly = true;
                     }*/

                    if (adGroupObj.CostModelWrapper == null)
                    {
                        returnValue.LoadDefaultsTrackingEvents = true;
                    }

                     returnValue.UniqueId = adGroupObj.UniqueId;

                    if ((targetings != null) && (targetings.Count == 0))
                    {
                        returnValue.Targeting = new List<TargetingBaseDto>();
                    }
                    else
                    {
                        WatchingUtil.StartWatch("MasterAppSiteTargeting");
                        returnValue.Targeting = targetings.Select(targeting => MapperHelper.Map<TargetingBaseDto>(targeting)).ToList();
                        if (adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>() != null)
                            returnValue.MasterList = adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>().Select(targeting => MapperHelper.Map<AdvertiserAccountMasterAppSiteDto>(targeting.List)).ToList();
                        WatchingUtil.EndWatch();
                        //TODO:Osaleh to refractor this code and use auto Mapper
                        var deviceTargetingDto = returnValue.Targeting.FirstOrDefault(x => x.Type.ID == 2);


                        WatchingUtil.StartWatch("VideoTargetingDto");
                        if (returnValue.IsVideoActionType)
                        {
                            var VideoTargeting =
                                         adGroupObj.Targetings.ToList().OfType<VideoTargeting>().FirstOrDefault();
                            if (VideoTargeting != null)
                            {
                                var VideoDto = returnValue.Targeting.ToList().OfType<VideoTargetingDto>().FirstOrDefault();
                                GetVideoTargeting(VideoTargeting, VideoDto);
                            }
                        }
                        WatchingUtil.EndWatch();


                        WatchingUtil.StartWatch("DeviceTargetingDto");
                        var deviceTargeting = targetings.FirstOrDefault(x => x.Type.ID == 2);
                        if ((deviceTargetingDto != null) && (deviceTargeting != null))
                        {
                            var deviceCapabilitiesDto = (deviceTargetingDto as DeviceTargetingDto).DeviceCapabilities;
                            var deviceTargetingObj = (deviceTargeting as DeviceTargeting);

                            foreach (var deviceCapabilityDto in deviceCapabilitiesDto)
                            {
                                deviceCapabilityDto.IsInclude = deviceTargetingObj.DeviceCapabilitiesTargeting.FirstOrDefault(
                                    x => x.Capability.ID == deviceCapabilityDto.ID).IsInclude;
                            }
                        }
                        WatchingUtil.EndWatch();

                        var KeyWordT = adGroupObj.Targetings.ToList().OfType<KeywordTargeting>().Where(M=>M.Keyword.Code!= "sensit").FirstOrDefault();
                        if(KeyWordT != null && KeyWordT.Include)
                        returnValue.AllowInclude = true;
                        else if(KeyWordT != null && !KeyWordT.Include)
                            returnValue.AllowInclude = false;
                        else
                            returnValue.AllowInclude = true;

                        WatchingUtil.StartWatch("KeywordTargeting");
                        var KeyWordST = adGroupObj.Targetings.ToList().OfType<KeywordTargeting>().Where(M => M.Keyword.Code == "sensit").FirstOrDefault();
                        if (KeyWordST != null && !KeyWordST.Include)
                            returnValue.ExcludeSensitiveCategories = true;
                        else if (KeyWordST != null && KeyWordST.Include)
                            returnValue.ExcludeSensitiveCategories = false;
                        else
                            returnValue.ExcludeSensitiveCategories = false;

                        WatchingUtil.EndWatch();
                    }

                    var costelemts = adGroupObj.GetCurrentFees();

                    List<AdGroupFeeDto> totalItems = null;
                    if (costelemts != null)
                    {
                        WatchingUtil.StartWatch("AdGroupFeeDto mapping");
                        totalItems = costelemts.Select(M => MapperHelper.Map<AdGroupFeeDto>(M)).ToList();
                        WatchingUtil.EndWatch();
                    }

                    WatchingUtil.StartWatch("Athis._FeeRepository.GetAll()");
                    var FeesList = this._FeeRepository.GetAll();
                    WatchingUtil.EndWatch();
                    if (totalItems == null)
                    {
                        totalItems = new List<AdGroupFeeDto>();

                    }
                    WatchingUtil.StartWatch("foreach (var feeItem in FeesList)");
                    
                    foreach (var feeItem in FeesList)
                    {
                        var feeaddItem = totalItems.Where(M => M.FeeId == feeItem.ID).ToList();
                        if (feeaddItem != null && feeaddItem.Count > 0)
                        {
                            for (int c = 0; c < feeaddItem.Count; c++)
                            {

                                if (feeItem.IsAutoAdded)
                                {
                                    feeaddItem[c].IsSystem = true;
                                    if (!string.IsNullOrEmpty(feeaddItem[c].Beneficiary))
                                    {
                                        feeaddItem[c].Fee = feeaddItem[c].Fee + ":" + feeaddItem[c].Beneficiary;
                                    }
                                    // feeaddItem.Value = adGroupObj.CostModelWrapper.Factor * feeaddItem.Value;
                                }
                                //feeaddItem[c].IsAdded = true;
                            }
                        }
                        else
                        {
                            totalItems.Add(new AdGroupFeeDto { FeeId = feeItem.ID, Fee = feeItem.GetDescription() });

                        }


                    }
                    WatchingUtil.EndWatch();

                    returnValue.FeesAdded = totalItems;

                    WatchingUtil.StartWatch("GetDealsAndSourcesForGroup(adGroupObj)");
                    returnValue.MultiSources = GetDealsAndSourcesForGroup(adGroupObj);
                    WatchingUtil.EndWatch();
                    //adGroupObj.CostModelWrapper.fa
                    if (adGroupObj.AdGroupDynamicBiddingConfig != null && adGroupObj.BiddingStrategy == BiddingStrategy.Dynamic)
                    {
                        if (adGroupObj.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeCTR || adGroupObj.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeVCVR)
                        {
                            returnValue.BidOptimizationValue = adGroupObj.AdGroupDynamicBiddingConfig.BidOptimizationValue * 100;
                        }
                        else
                        {

                            returnValue.BidOptimizationValue = adGroupObj.AdGroupDynamicBiddingConfig.BidOptimizationValue;
                        }
                        returnValue.MaxBidPrice = adGroupObj.AdGroupDynamicBiddingConfig.MaxBidPrice * adGroupObj.CostModelWrapper.Factor;

                        returnValue.KeepBiddingAtMinimum = adGroupObj.AdGroupDynamicBiddingConfig.KeepBiddingAtMinimum;

                        returnValue.BidOptimizationType = adGroupObj.AdGroupDynamicBiddingConfig.Type;

                    }


                    

                    #region  Conversions And Events




                    if (returnValue.ConversionSetting == ConversionSetting.Unknown)

                    {
                        returnValue.ConversionSetting = ConversionSetting.CountingEvery;
                        returnValue.CountingAttribuation = 5;
                    }

                    if (returnValue.ClickAttribuation == 0)

                    {
                        //returnValue.ConversionSetting = ConversionSetting.CountingEvery;
                        returnValue.ClickAttribuation = 14;
                    }
                    if (returnValue.ViewAttribuation == 0)

                    {
                        //returnValue.ConversionSetting = ConversionSetting.CountingEvery;
                        returnValue.ViewAttribuation = 14;
                    }
                    returnValue.AdEventItems = new List<AdGroupTrackingEventDto>();
                    var adGroupsTrackings = adGroupObj.TrackingEvents.Where(p => !p.IsDeleted)
                                          .OrderBy(p => p.ID).ToList();


                    foreach (var iteme in adGroupsTrackings)
                    {
                        WatchingUtil.StartWatch("AdGroupTrackingEventDto mapping");
                        AdGroupTrackingEventDto trackingEvent = MapperHelper.Map<AdGroupTrackingEventDto>(iteme);
                        WatchingUtil.EndWatch();

                        if (string.IsNullOrEmpty(trackingEvent.Name))
                        {
                            WatchingUtil.StartWatch("trackingEventRepository.Query(x => x.Code == trackingEvent.Code)");
                            var eventvar = trackingEventRepository.Query(x => x.Code == trackingEvent.Code).SingleOrDefault();
                            WatchingUtil.EndWatch();
                            if (eventvar != null)
                                trackingEvent.Description = eventvar.Name.ToString();
                           

                        }
                        trackingEvent.IsNotChanged = true;
                        if (iteme.AudienceSegmentListsMap != null && iteme.AudienceSegmentListsMap.Count > 0)
                            returnValue.AdEventItems.Add(trackingEvent);
                    }



                    returnValue.ConversionItems = new List<AdGroupConversionEventDto>();
                    var adGroupsConversions = adGroupObj.ConversionEvents.Where(p => !p.IsDeleted)
                                       .OrderBy(p => p.ID).ToList();



                    foreach (var itemCe in adGroupsConversions)
                    {
                        WatchingUtil.StartWatch("AdGroupConversionEventDto mapping");
                        AdGroupConversionEventDto ConversioEvent = MapperHelper.Map<AdGroupConversionEventDto>(itemCe);
                        WatchingUtil.EndWatch();
                        WatchingUtil.StartWatch("trackingEventRepository.Query(x => x.Code == ConversioEvent.Code)");
                        var eventvar = trackingEventRepository.Query(x => x.Code == ConversioEvent.Code).SingleOrDefault();
                        WatchingUtil.EndWatch();
                        if (eventvar != null)
                            ConversioEvent.Description = eventvar.Name.ToString();
                        else
                            ConversioEvent.Description = ConversioEvent.Description;
                        ConversioEvent.IsNotChanged = true;
                        if (itemCe.PixelListsMap != null && itemCe.PixelListsMap.Count > 0)
                            returnValue.ConversionItems.Add(ConversioEvent);
                    }







                    #endregion

                    WatchingUtil.EndWatch();
                    return returnValue;
                }
                else
                {
                    WatchingUtil.EndWatch();
                    return null;
                }
            }
            WatchingUtil.EndWatch();
            return null;
        }
        public void CalculateAdPostion(AdGroup adGro, TargetingSaveDto dto)
        {

            AdPositionEnum catEnum = AdPositionEnum.Undefined;


            if (!dto.AdPosition_Enabled)
            {
                adGro.AdPosition = null;
                return;

            }
            if (dto.AdPosition_AboveTheFold)
            {
                catEnum = AdPositionEnum.AboveTheFold | catEnum;


            }
            if (dto.AdPosition_BelowTheFold)
            {
                catEnum = AdPositionEnum.BelowTheFold | catEnum;


            }
            if (dto.AdPosition_Unknown)
            {
                catEnum = AdPositionEnum.Unknown | catEnum;


            }




            //dto.CalculatedFromFeeCategory = Convert.ToInt64(catFeeEnum);

            adGro.AdPosition = Convert.ToInt32(catEnum);
            //  costElem.CalculatedFromFeeCategory = dto.CalculatedFromFeeCategory;
        }
        public void RefereseCalculateAdPosition(AdGroup adGroup, TargetingListDto dto)
        {
            if (adGroup.AdPosition.HasValue)
            {
                dto.AdPosition_Enabled = true;

            }
            else
            {
                return;
            }
            AdPositionEnum catEnum = (AdPositionEnum)adGroup.AdPosition;





            dto.AdPosition_Unknown = catEnum.HasFlag(AdPositionEnum.Unknown);

            dto.AdPosition_AboveTheFold = catEnum.HasFlag(AdPositionEnum.AboveTheFold);

            dto.AdPosition_BelowTheFold = catEnum.HasFlag(AdPositionEnum.BelowTheFold);




        }
        public void GetVideoTargeting(VideoTargeting targeting, VideoTargetingDto dto)
        {

            if (targeting.InStreamPositions != null)
            {
                foreach (var targ in targeting.InStreamPositions)
                {


                    if (targ.Code == "1")
                    {

                        dto.InStreamPosition_PreRoll = true;
                    }


                    if (targ.Code == "2")
                    {
                        dto.InStreamPosition_MidRoll = true;

                    }

                    if (targ.Code == "3")
                    {

                        dto.InStreamPosition_PostRoll = true;
                    }
                    if (targ.Code == "0")
                    {
                        dto.InStreamPosition_Undetermined = true;

                    }
                }
            }
            if (targeting.PlacementTypes != null)
            {

                foreach (var targ in targeting.PlacementTypes)
                {

                    if (targ.Code == "1")
                    {

                        dto.PlacementType_InStream = true;
                    }


                    if (targ.Code == "2")
                    {
                        dto.PlacementType_OutStream = true;

                    }

                    if (targ.Code == "3")
                    {

                        dto.PlacementType_Interstitial = true;
                    }
                    if (targ.Code == "0")
                    {
                        dto.PlacementType_Undetermined = true;

                    }

                }
            }
            if (targeting.PlayBackMethods != null)
            {
                foreach (var targ in targeting.PlayBackMethods)
                {
                    if (targ.Code == "1")
                    {

                        dto.Playback_AutoPlaySoundOn = true;
                    }


                    if (targ.Code == "2")
                    {
                        dto.Playback_AutoPlaySoundOff = true;

                    }

                    if (targ.Code == "3")
                    {

                        dto.Playback_ClickToPlay = true;
                    }
                    if (targ.Code == "0")
                    {
                        dto.Playback_Undetermined = true;

                    }

                }
            }

            if (targeting.SkippableAds != null)
            {
                foreach (var targ in targeting.SkippableAds)
                {
                    if (targ.Code == "1")
                    {

                        dto.SkippableAds_SkippableAdSpaces = true;
                    }


                    if (targ.Code == "2")
                    {
                        dto.SkippableAds_NonSkippableAdSpaces = true;

                    }


                    if (targ.Code == "0")
                    {
                        dto.SkippableAds_Undetermined = true;

                    }

                }
            }
        }

        public void SaveVideoTargeting(VideoTargeting targeting, TargetingSaveDto dto)
        {


            var PlacementTypes = _PlacementTypeRepository.GetAll();
            var PlaybackMethods = _PlaybackMethodsRepository.GetAll();
            var InStreamPositions = _InStreamPositionRepository.GetAll();
            var SkippableAds = _SkippableAdsRepository.GetAll();




            var InStreamPosition_PreRoll = targeting.InStreamPositions != null ? targeting.InStreamPositions.Where(M => M.Code == "1").SingleOrDefault() : null;

            if (dto.InStreamPosition_PreRoll)
            {
                if (InStreamPosition_PreRoll == null)
                {
                    if (targeting.InStreamPositions != null)
                    {
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "1").SingleOrDefault());
                    }
                    else
                    {
                        targeting.InStreamPositions = new List<Domain.Model.Core.Video.InStreamPosition>();
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "1").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(InStreamPosition_PreRoll == null))
                {

                    targeting.InStreamPositions.Remove(InStreamPosition_PreRoll);

                }


            }

            var InStreamPosition_MidRoll = targeting.InStreamPositions != null ? targeting.InStreamPositions.Where(M => M.Code == "2").SingleOrDefault() : null;

            if (dto.InStreamPosition_MidRoll)
            {
                if (InStreamPosition_MidRoll == null)
                {




                    if (targeting.InStreamPositions != null)
                    {
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "2").SingleOrDefault());
                    }
                    else
                    {
                        targeting.InStreamPositions = new List<Domain.Model.Core.Video.InStreamPosition>();
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "2").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(InStreamPosition_MidRoll == null))
                {
                    targeting.InStreamPositions.Remove(InStreamPosition_MidRoll);

                }


            }




            var InStreamPosition_PostRoll = targeting.InStreamPositions != null ? targeting.InStreamPositions.Where(M => M.Code == "3").SingleOrDefault() : null;

            if (dto.InStreamPosition_PostRoll)
            {
                if (InStreamPosition_PostRoll == null)
                {




                    if (targeting.InStreamPositions != null)
                    {
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "3").SingleOrDefault());
                    }
                    else
                    {
                        targeting.InStreamPositions = new List<Domain.Model.Core.Video.InStreamPosition>();
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "3").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(InStreamPosition_PostRoll == null))
                {
                    targeting.InStreamPositions.Remove(InStreamPosition_PostRoll);

                }


            }


            var InStreamPosition_Undetermined = targeting.InStreamPositions != null ? targeting.InStreamPositions.Where(M => M.Code == "0").SingleOrDefault() : null;

            if (dto.InStreamPosition_Undetermined)
            {
                if (InStreamPosition_Undetermined == null)
                {





                    if (targeting.InStreamPositions != null)
                    {
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "0").SingleOrDefault());
                    }
                    else
                    {
                        targeting.InStreamPositions = new List<Domain.Model.Core.Video.InStreamPosition>();
                        targeting.InStreamPositions.Add(InStreamPositions.Where(M => M.Code == "0").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(InStreamPosition_Undetermined == null))
                {
                    targeting.InStreamPositions.Remove(InStreamPosition_Undetermined);

                }


            }



            var PlacementType_InStream = targeting.PlacementTypes != null ? targeting.PlacementTypes.Where(M => M.Code == "1").SingleOrDefault() : null;

            if (dto.PlacementType_InStream)
            {
                if (PlacementType_InStream == null)
                {






                    if (targeting.PlacementTypes != null)
                    {
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "1").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlacementTypes = new List<Domain.Model.Core.Video.PlacementType>();
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "1").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(PlacementType_InStream == null))
                {
                    targeting.PlacementTypes.Remove(PlacementType_InStream);

                }


            }


            var PlacementType_OutStream = targeting.PlacementTypes != null ? targeting.PlacementTypes.Where(M => M.Code == "2").SingleOrDefault() : null;

            if (dto.PlacementType_OutStream)
            {
                if (PlacementType_OutStream == null)
                {






                    if (targeting.PlacementTypes != null)
                    {
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "2").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlacementTypes = new List<Domain.Model.Core.Video.PlacementType>();
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "2").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(PlacementType_OutStream == null))
                {
                    targeting.PlacementTypes.Remove(PlacementType_OutStream);

                }


            }


            var PlacementType_Interstitial = targeting.PlacementTypes != null ? targeting.PlacementTypes.Where(M => M.Code == "3").SingleOrDefault() : null;

            if (dto.PlacementType_Interstitial)
            {
                if (PlacementType_Interstitial == null)
                {




                    if (targeting.PlacementTypes != null)
                    {
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "3").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlacementTypes = new List<Domain.Model.Core.Video.PlacementType>();
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "3").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(PlacementType_Interstitial == null))
                {
                    targeting.PlacementTypes.Remove(PlacementType_Interstitial);

                }


            }



            var PlacementType_Undetermined = targeting.PlacementTypes != null ? targeting.PlacementTypes.Where(M => M.Code == "0").SingleOrDefault() : null;

            if (dto.PlacementType_Undetermined)
            {
                if (PlacementType_Undetermined == null)
                {





                    if (targeting.PlacementTypes != null)
                    {
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "0").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlacementTypes = new List<Domain.Model.Core.Video.PlacementType>();
                        targeting.PlacementTypes.Add(PlacementTypes.Where(M => M.Code == "0").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(PlacementType_Undetermined == null))
                {
                    targeting.PlacementTypes.Remove(PlacementType_Undetermined);

                }


            }






            var Playback_AutoPlaySoundOn = targeting.PlayBackMethods != null ? targeting.PlayBackMethods.Where(M => M.Code == "1").SingleOrDefault() : null;

            if (dto.Playback_AutoPlaySoundOn)
            {
                if (Playback_AutoPlaySoundOn == null)
                {

                    //targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "1").SingleOrDefault());



                    if (targeting.PlayBackMethods != null)
                    {
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "1").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlayBackMethods = new List<Domain.Model.Core.Video.PlaybackMethods>();
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "1").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(Playback_AutoPlaySoundOn == null))
                {
                    targeting.PlayBackMethods.Remove(Playback_AutoPlaySoundOn);

                }


            }



            var Playback_AutoPlaySoundOff = targeting.PlayBackMethods != null ? targeting.PlayBackMethods.Where(M => M.Code == "2").SingleOrDefault() : null;

            if (dto.Playback_AutoPlaySoundOff)
            {
                if (Playback_AutoPlaySoundOff == null)
                {






                    if (targeting.PlayBackMethods != null)
                    {
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "2").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlayBackMethods = new List<Domain.Model.Core.Video.PlaybackMethods>();
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "2").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(Playback_AutoPlaySoundOff == null))
                {
                    targeting.PlayBackMethods.Remove(Playback_AutoPlaySoundOff);

                }


            }


            var Playback_ClickToPlay = targeting.PlayBackMethods != null ? targeting.PlayBackMethods.Where(M => M.Code == "3").SingleOrDefault() : null;

            if (dto.Playback_ClickToPlay)
            {
                if (Playback_ClickToPlay == null)
                {





                    if (targeting.PlayBackMethods != null)
                    {
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "3").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlayBackMethods = new List<Domain.Model.Core.Video.PlaybackMethods>();
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "3").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(Playback_ClickToPlay == null))
                {
                    targeting.PlayBackMethods.Remove(Playback_ClickToPlay);

                }


            }

            var Playback_Undetermined = targeting.PlayBackMethods != null ? targeting.PlayBackMethods.Where(M => M.Code == "0").SingleOrDefault() : null;

            if (dto.Playback_Undetermined)
            {
                if (Playback_Undetermined == null)
                {




                    if (targeting.PlayBackMethods != null)
                    {
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "0").SingleOrDefault());
                    }
                    else
                    {
                        targeting.PlayBackMethods = new List<Domain.Model.Core.Video.PlaybackMethods>();
                        targeting.PlayBackMethods.Add(PlaybackMethods.Where(M => M.Code == "0").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(Playback_Undetermined == null))
                {
                    targeting.PlayBackMethods.Remove(Playback_Undetermined);

                }


            }



            var SkippableAds_SkippableAdSpaces = targeting.SkippableAds != null ? targeting.SkippableAds.Where(M => M.Code == "1").SingleOrDefault() : null;

            if (dto.SkippableAds_SkippableAdSpaces)
            {
                if (SkippableAds_SkippableAdSpaces == null)
                {




                    if (targeting.SkippableAds != null)
                    {
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "1").SingleOrDefault());
                    }
                    else
                    {
                        targeting.SkippableAds = new List<Domain.Model.Core.Video.SkippableAds>();
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "1").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(SkippableAds_SkippableAdSpaces == null))
                {
                    targeting.SkippableAds.Remove(SkippableAds_SkippableAdSpaces);

                }


            }

            var SkippableAds_NonSkippableAdSpaces = targeting.SkippableAds != null ? targeting.SkippableAds.Where(M => M.Code == "2").SingleOrDefault() : null;

            if (dto.SkippableAds_NonSkippableAdSpaces)
            {
                if (SkippableAds_NonSkippableAdSpaces == null)
                {

                    // targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "2").SingleOrDefault());



                    if (targeting.SkippableAds != null)
                    {
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "2").SingleOrDefault());
                    }
                    else
                    {
                        targeting.SkippableAds = new List<Domain.Model.Core.Video.SkippableAds>();
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "2").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(SkippableAds_NonSkippableAdSpaces == null))
                {
                    targeting.SkippableAds.Remove(SkippableAds_NonSkippableAdSpaces);

                }


            }


            var SkippableAds_Undetermined = targeting.SkippableAds != null ? targeting.SkippableAds.Where(M => M.Code == "0").SingleOrDefault() : null;

            if (dto.SkippableAds_Undetermined)
            {
                if (SkippableAds_Undetermined == null)
                {





                    if (targeting.SkippableAds != null)
                    {
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "0").SingleOrDefault());
                    }
                    else
                    {
                        targeting.SkippableAds = new List<Domain.Model.Core.Video.SkippableAds>();
                        targeting.SkippableAds.Add(SkippableAds.Where(M => M.Code == "0").SingleOrDefault());

                    }
                }


            }
            else
            {
                if (!(SkippableAds_Undetermined == null))
                {
                    targeting.SkippableAds.Remove(SkippableAds_Undetermined);

                }


            }





        }
        private IList<string> SearchForUrls(AdCreative adCreative)
        {

            List<string> UrlsListToCheck = new List<string>();
            var AdSubtype = adCreative.AdSubType.GetValueOrDefault();
            var isExternalIntersessial = false;
            if (AdSubtype == AdSubTypes.ExternalUrlInterstitial)
            {
                isExternalIntersessial = true;
            }
            #region ActionArea
            if (adCreative.ActionValue != null)
            {
                if (adCreative.ActionValue.Trackers != null)
                {
                    foreach (AdActionValueTracker Tracker in adCreative.ActionValue.Trackers)
                    {
                        if (!Tracker.IsDeleted)
                            UrlsListToCheck.Add(Tracker.Url);
                    }
                }

                if (!string.IsNullOrEmpty(adCreative.ActionValue.Value))
                {
                    UrlsListToCheck.Add(adCreative.ActionValue.Value);
                }
                if (!string.IsNullOrEmpty(adCreative.ActionValue.Value2))
                {
                    UrlsListToCheck.Add(adCreative.ActionValue.Value2);
                }
            }

            #endregion
            // and SnapshotUrl
            #region AdCreativeUnits

            if (adCreative.AdCreativeUnits != null)
            {
                foreach (AdCreativeUnit adCreativeUnit in adCreative.AdCreativeUnits)
                {

                    if (adCreativeUnit.Trackers != null)
                    {
                        foreach (AdCreativeUnitTracker Tracker in adCreativeUnit.GetTrackers())
                        {
                            UrlsListToCheck.Add(Tracker.TrackingUrl);
                        }
                    }
                    if (isExternalIntersessial)
                    {
                        if (!string.IsNullOrEmpty(adCreativeUnit.Content))
                        {
                            UrlsListToCheck.Add(adCreativeUnit.Content);
                        }
                    }
                }
            }
            #endregion

            return UrlsListToCheck;


        }
        private void CheckAdvertiserBlock(AdGroup adGroupObj, AdCreative adCreative)
        {
            var obj = adGroupObj.Campaign;


            var audienceSegmentTargetingS = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>();
            List<string> blockedList = new List<string>();

            if (audienceSegmentTargetingS != null)
            {
                foreach (var audienceSegmentTargeting in audienceSegmentTargetingS)
                {

                    if (audienceSegmentTargeting is AudienceSegmentTargeting)
                    {


                        // if (audienceSegmentTargeting.IsExternal)
                        // {
                        var ist = audienceSegmentTargeting.GetDomainURLForAdvertiserBlocker(obj.Advertiser.ID, audienceSegmentTargeting.GetRulesJsonForGroup(), obj.Account.ID);
                        blockedList.AddRange(ist);
                        // }
                    }
                }

                if (blockedList != null && blockedList.Count > 0)
                {
                    var ListToCheck = SearchForUrls(adCreative);
                    if (ListToCheck != null && ListToCheck.Count > 0)
                    {
                        foreach (var itemBlock in blockedList)
                        {
                            // var URL2 = new Uri(itemBlock);
                            Uri URL2 = null;
                            var copyitemBlock = itemBlock;
                            if (copyitemBlock.Contains("www."))
                            {
                                copyitemBlock = copyitemBlock.Replace("www.", string.Empty);
                            }
                            if (!itemBlock.Contains("http"))
                            {
                                UriBuilder ub1 = new UriBuilder("http", copyitemBlock);
                                URL2 = ub1.Uri;
                            }
                            else
                            {
                                URL2 = new Uri(copyitemBlock);

                            }
                            foreach (var itemToCheck in ListToCheck)
                            {
                                Uri URL1 = null;


                                var copyitemToCheck = itemToCheck;
                                if (copyitemToCheck.Contains("www."))
                                {
                                    copyitemToCheck = copyitemToCheck.Replace("www.", string.Empty);
                                }
                                if (!itemToCheck.Contains("http"))
                                {


                                    UriBuilder ub2 = new UriBuilder("http", copyitemToCheck);
                                    URL1 = ub2.Uri;
                                }
                                else
                                {
                                    URL1 = new Uri(copyitemToCheck);

                                }
                                if (string.Equals(URL1.Host.ToLower(), URL2.Host.ToLower()))
                                {
                                    var error = new BusinessException();
                                    error.Errors.Add(new ErrorData { ID = "AdvertiserBlockViolate" });
                                    throw error;
                                }
                                string ResultURL = GetRealUrl(copyitemToCheck);
                                if (ResultURL.Contains("www."))
                                {
                                    ResultURL = ResultURL.Replace("www.", string.Empty);
                                }
                                if (string.IsNullOrEmpty(ResultURL))
                                {
                                    continue;
                                }
                                Uri uriResult;
                                bool result = Uri.TryCreate(ResultURL, UriKind.Absolute, out uriResult)
                                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                                if (result)
                                {
                                    var URL3 = uriResult;
                                    if (string.Equals(URL3.Host.ToLower(), URL2.Host.ToLower()))
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "AdvertiserBlockViolate" });
                                        throw error;
                                    }
                                }

                                string ResultURL2 = GetRealUrl(ResultURL);
                                if (ResultURL2.Contains("www."))
                                {
                                    ResultURL2 = ResultURL2.Replace("www.", string.Empty);
                                }
                                if (string.IsNullOrEmpty(ResultURL2))
                                {
                                    continue;
                                }
                                Uri uriResult2;
                                bool result2 = Uri.TryCreate(ResultURL2, UriKind.Absolute, out uriResult2)
                                    && (uriResult2.Scheme == Uri.UriSchemeHttp || uriResult2.Scheme == Uri.UriSchemeHttps);
                                if (result2)
                                {
                                    var URL4 = uriResult2;
                                    if (string.Equals(URL4.Host.ToLower(), URL2.Host.ToLower()))
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "AdvertiserBlockViolate" });
                                        throw error;
                                    }
                                }

                                string ResultURL3 = GetRealUrl(ResultURL2);
                                if (ResultURL3.Contains("www."))
                                {
                                    ResultURL3 = ResultURL3.Replace("www.", string.Empty);
                                }
                                if (string.IsNullOrEmpty(ResultURL3))
                                {
                                    continue;
                                }
                                Uri uriResult3;
                                bool result3 = Uri.TryCreate(ResultURL3, UriKind.Absolute, out uriResult3)
                                    && (uriResult3.Scheme == Uri.UriSchemeHttp || uriResult3.Scheme == Uri.UriSchemeHttps);
                                if (result3)
                                {
                                    var URL5 = uriResult3;
                                    if (string.Equals(URL5.Host.ToLower(), URL2.Host.ToLower()))
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "AdvertiserBlockViolate" });
                                        throw error;
                                    }
                                }


                            }

                        }
                    }

                }
            }
        }

        private string GetRealUrl(string url)
        {
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = "https://" + url;
                }
                WebRequest request = WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;
                WebResponse response = request.GetResponse();
                return response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                return url;
            }
        }
        public TargetingResultDto SaveTargeting(TargetingSaveDto targetingSaveDto)
        {

            TargetingResultDto dtoResult = new TargetingResultDto();
            var adGroupObj = adGroupRepository.Get(targetingSaveDto.AdGroupId);
            var item = adGroupObj.Campaign;
            ValidateCampaign(item);
            //var item = CampaignRepository.Get(targetingSaveDto.CampaignId);
            //CheckCampaign(item);
            // ValidateCampaign(item);
            var isPricingModelChanged = false;
            var isGeoFencingTargeting = false;
            var isSetDefaulAccountCostModel = false;

            if (item.IsValid)
            {
                // check if the campaign is locked
                // get if the logged in user is admin
                var isAdmin = Domain.Configuration.IsAdmin;
                if (!isAdmin && item.IsClientLocked)
                {
                    throw new CampaignLockedException();
                }
                if ((!isAdmin && !IsllowedAdvertiserForCamp(item)))
                    {
                    throw new CampaignReadOnlyException();
                }

                if (item.CostModelWrapper.HasValue && ((int)item.CostModelWrapper.Value != targetingSaveDto.CostModelWrapper))
                {
                    throw new NotValidCostModelException();
                }

                const int keywordTargetingTypeId = 1;
                const int deviceTargetingTypeId = 2;
                const int geographicTargetingTypeId = 3;
                const int operatorTargetingTypeId = 4;
                const int demographicTargtingTypeId = 5;
                const int ipRangeTargetingTypeId = 6;
                const int urlTargetingTypeId = 7;
                const int geoFencingTargetingTypeId = 8;
                const int pmpTargtingTypeId = 11;
                const int AudianceTargtingTypeId = 12;
                const int LanguageTargtingTypeId = 14;
                const int MasterAppsiteTargtingTypeId = 16;
                const int ContextualSegmentTargetingTypeId = 18;
                const int VideoTargetingTypeId = 15;
                if (targetingSaveDto.Operators == null)
                {
                    targetingSaveDto.Operators = new int[0];
                }

                if (targetingSaveDto.Geographies == null)
                {
                    targetingSaveDto.Geographies = new int[0];
                }

                if (targetingSaveDto.Manufacturers == null)
                {
                    targetingSaveDto.Manufacturers = new List<int>();
                }

                if (targetingSaveDto.Platforms == null)
                {
                    targetingSaveDto.Platforms = new Dictionary<int, string>();
                }
                if (targetingSaveDto.Models == null)
                {
                    targetingSaveDto.Models = new int[0];
                }
                if (targetingSaveDto.DeviceCapabilities == null)
                {
                    targetingSaveDto.DeviceCapabilities = new int[0];
                }
                if (targetingSaveDto.ExcludeDeviceCapability == null)
                {
                    targetingSaveDto.ExcludeDeviceCapability = new int[0];
                }
                CalculateAdPostion(adGroupObj, targetingSaveDto);
                //TODO:Osaleh to remove this code
                targetingSaveDto.Manufacturers = targetingSaveDto.Manufacturers.ToList();
                targetingSaveDto.Platforms = new Dictionary<int, string>(targetingSaveDto.Platforms);

                //var adGroupObj = (item.GetGroups().Where(adGroup => adGroup.ID == targetingSaveDto.AdGroupId).FirstOrDefault());
                if (adGroupObj != null)
                {
                    if (!IsAllowedGroup(adGroupObj))
                    {
                        throw new CampaignLockedException();
                    }

                    if ((!isAdmin && !IsllowedAdvertiserForCamp(item)))
                    {
                        throw new CampaignReadOnlyException();
                    }
                    if (adGroupObj.CostModelWrapper == null)
                    {
                        // isSetDe
                        isSetDefaulAccountCostModel = true;

                    }
                    int? oldPricingModel = adGroupObj.CostModelWrapper == null ? new int?() : new int?(adGroupObj.CostModelWrapper.CostModel.ID); ;
                    int? oldCostModelWrapper = adGroupObj.CostModelWrapper == null ? new int?() : new int?(adGroupObj.CostModelWrapper.ID);

                    if (oldPricingModel != null && (oldCostModelWrapper != targetingSaveDto.CostModelWrapper))
                    {
                        isPricingModelChanged = true;
                    }
                    //TODO:Osaleh to remove this temp code
                    var paltformIsAll = true;
                    var manufacturerIsAll = true;
                    if (targetingSaveDto.DailyBudget.HasValue)
                    {
                      


                        if (targetingSaveDto.DailyBudget.Value != 0)
                            adGroupObj.DailyBudget = targetingSaveDto.DailyBudget.Value;
                        else
                            adGroupObj.DailyBudget = null;
                    }
                    else
                    {
                        adGroupObj.DailyBudget = null;
                    }
                    if (adGroupObj.Objective.AdAction != null && (adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingIOS || adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingAndroid))
                    {
                        adGroupObj.DailyBudget = null;
                        adGroupObj.IgnoreDailyBudget = true;
                    }

                    if (adGroupObj.Objective.AdAction != null && (adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingIOSForLead || adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingAndroidForLead || adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.AdTracking))
                    {
                        adGroupObj.DailyBudget = null;
                        adGroupObj.IgnoreDailyBudget = true;
                    }
                    if (targetingSaveDto.Budget.HasValue)
                    {
                        

                        if (targetingSaveDto.Budget.Value != 0)
                            adGroupObj.Budget = targetingSaveDto.Budget.Value;
                        else
                            adGroupObj.Budget = null;
                    }
                    else
                    {

                        adGroupObj.Budget = null;
                    }

                    if (Domain.Configuration.IsAdminOnly)
                    {

                        if (targetingSaveDto.ViewabilityVendorId.HasValue)
                            adGroupObj.ViewabilityVendorId = targetingSaveDto.ViewabilityVendorId;
                        else
                            adGroupObj.ViewabilityVendorId = null;
                    }
                  /*  else
                    {

                        adGroupObj.ViewabilityVendorId = adGroupObj.ViewabilityVendorId;
                    }*/
                    //this condition been added to allow the house ad with bid 0 , 
                    //TODO:to use more generic way to handle this
                    if (targetingSaveDto.BinInfo != null)
                    {
                        //check if the bid in less than the min bid
                        targetingSaveDto.BinInfo.ActionType = adGroupObj.Objective.AdAction.ID;
                        targetingSaveDto.BinInfo.AdTypeId = adGroupObj.Objective.AdType != null ? adGroupObj.Objective.AdType.ID : new int?();

                        var minbid = GetMinBid(targetingSaveDto, adGroupObj);

                        decimal minBidValue = minbid.CostModelsWrappersBidValues.Where(p => p.Key == targetingSaveDto.CostModelWrapper).SingleOrDefault().Value;

                        if (targetingSaveDto.BiddingStrategy == BiddingStrategy.Fixed)
                        {
                            if (!Domain.Configuration.IsAdminOnly)

                            {
                                if (targetingSaveDto.Bid < minBidValue)
                                {
                                    var error = new BusinessException();
                                    error.Errors.Add(new ErrorData { ID = "MinBidErrorLess" });
                                    throw error;
                                }
                            }
                            else
                            {
                                if (targetingSaveDto.Bid < minBidValue)
                                {
                                    dtoResult.AdminLessThanMinBid = true;
                                }

                            }
                        }
                        //if (targetingSaveDto.Bid < minBidValue)
                        //{
                        //    var error = new BusinessException();
                        //    error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                        //    throw error;
                        //}



                        if (targetingSaveDto.BiddingStrategy == BiddingStrategy.Fixed)
                        {
                            if (!Domain.Configuration.IsAdminOnly)
                            {
                                //check is any ad for this group has bid less than the new min bid
                                var ads = item.GetGroupAds(adGroupObj).Where(ad => ad.GetReadableBid() < targetingSaveDto.Bid && ad.AdSubType != AdSubTypes.VideoEndCard);
                                if (ads != null && ads.Count() > 0)
                                {
                                    //create business Exception to hold error data list 
                                    var error = new BusinessException();
                                    error.Errors.Add(new ErrorData { ID = "adsMoreThanMinBidD" });
                                    throw error;
                                }

                            }
                        }

                        CostModelWrapper costModelWrapper = costModelWrapperRepository.Get(targetingSaveDto.CostModelWrapper);
                        adGroupObj.SetCostModelWrapper(costModelWrapper);
                    }
                    else
                    {
                        CostModelWrapper costModelWrapper = costModelWrapperRepository.Get((int)CostModelWrapperEnum.CPC);
                        adGroupObj.SetCostModelWrapper(costModelWrapper);
                    }
                    adGroupObj.AudianceDiscountPrice = targetingSaveDto.AudianceDiscountPrice / 1000;


                    adGroupObj.BiddingStrategy = targetingSaveDto.BiddingStrategy;
                    if (adGroupObj.BiddingStrategy == BiddingStrategy.Dynamic)
                    {
                        if (adGroupObj.AdGroupDynamicBiddingConfig == null)
                        {
                            adGroupObj.AdGroupDynamicBiddingConfig = new AdGroupDynamicBiddingConfig();
                        }
                        adGroupObj.AdGroupDynamicBiddingConfig.Type = targetingSaveDto.BidOptimizationType;
                        if (adGroupObj.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeCTR || adGroupObj.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeVCVR)
                            adGroupObj.AdGroupDynamicBiddingConfig.BidOptimizationValue = targetingSaveDto.BidOptimizationValue / 100;
                        else
                            adGroupObj.AdGroupDynamicBiddingConfig.BidOptimizationValue = targetingSaveDto.BidOptimizationValue;
                        //ArabyAds.AdFalcon.Domain.Configuration.DynamicBiddingDefaultBidPricePerc

                        adGroupObj.AdGroupDynamicBiddingConfig.MaxBidPrice = targetingSaveDto.MaxBidPrice / adGroupObj.CostModelWrapper.Factor;
                        adGroupObj.AdGroupDynamicBiddingConfig.MinBidPrice = adGroupObj.AdGroupDynamicBiddingConfig.MaxBidPrice * ArabyAds.AdFalcon.Domain.Configuration.DynamicBiddingMinBidPricePerc;
                        adGroupObj.AdGroupDynamicBiddingConfig.DefaultBidPrice = adGroupObj.AdGroupDynamicBiddingConfig.MaxBidPrice * ArabyAds.AdFalcon.Domain.Configuration.DynamicBiddingDefaultBidPricePerc;

                        adGroupObj.AdGroupDynamicBiddingConfig.BidStep = ArabyAds.AdFalcon.Domain.Configuration.DynamicBiddingDefaultBidStep / adGroupObj.CostModelWrapper.Factor;
                        adGroupObj.AdGroupDynamicBiddingConfig.KeepBiddingAtMinimum = targetingSaveDto.KeepBiddingAtMinimum;
                        adGroupObj.AdGroupDynamicBiddingConfig.AdGroup = adGroupObj;

                    }
                    else
                    { if (adGroupObj.AdGroupDynamicBiddingConfig != null)
                            _AdGroupDynamicBiddingConfigRepository.Remove(adGroupObj.AdGroupDynamicBiddingConfig);
                        adGroupObj.AdGroupDynamicBiddingConfig = null;

                    }






                    #region TrackingEvents

                    // if (adGroupObj.Campaign.CampaignType == CampaignType.Normal)
                    //{
                    if (adGroupObj.Campaign.CampaignType != CampaignType.Normal && adGroupObj.Campaign.CampaignType != CampaignType.ProgrammaticGuaranteed)
                    {
                        targetingSaveDto.CostModelWrapper = (int)adGroupObj.CostModelWrapperEnum;

                        if (oldCostModelWrapper != null)
                        {
                            var eventsTrack = adGroupObj.GetTrackingEvents();
                            if (eventsTrack == null || eventsTrack.Count == 0)
                            {
                                oldCostModelWrapper = null;
                            }
                        }
                    }
                    //else
                    //{



                    //}
                    if (oldCostModelWrapper == null || (oldCostModelWrapper != adGroupObj.CostModelWrapper.ID))
                    {
                        AddDefaultAdGroupTrackingEvent(adGroupObj, targetingSaveDto.CostModelWrapper, oldCostModelWrapper);
                        adGroupObj.IsDefaultPrerequisitesSaved = false;
                    }

                    var allTrackingEventObj =  adGroupObj.TrackingEvents.Where(c=> c.IsDeleted == false).ToList();
                    if (allTrackingEventObj != null && allTrackingEventObj.Count != 0)
                    {

                        if (targetingSaveDto.InsertedTrackingEvents == null)
                            targetingSaveDto.InsertedTrackingEvents = new List<AdGroupTrackingEventSaveDto>();
                        foreach (var itemEv in allTrackingEventObj)
                        {

                            var deletedTrackingEventObj =
                              targetingSaveDto.InsertedTrackingEvents.FirstOrDefault(targeting => targeting.Code == itemEv.Code && itemEv.IsCustom==true);
                            if (deletedTrackingEventObj == null && itemEv.IsCustom)
                            {
                                var result = IsDeleteTrackingEventAllowed( 
                                    new IsDeleteTrackingEventAllowedRequest { 
                                        CampaignId = item.ID, 
                                        AdgroupId = targetingSaveDto.AdGroupId, AdGroupTrackingEventCodes = new List<string>() { itemEv.Code }, 
                                        CheckStandards = true, NewCostModelWrapperId = null });

                                if (result.Value.Key)
                                {
                                    if (!CheckIfTrackingEventPrerequisiteDeleteIsAllowed(adGroupObj, new string[] { itemEv.Code}))
                                    {
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("DeletePrerequisite") });
                                    }
                                    itemEv.IsDeleted = true;
                                }
                            }
                        }
                    }

                    if (targetingSaveDto.InsertedTrackingEvents != null && targetingSaveDto.InsertedTrackingEvents.Count != 0)
                    {
                        foreach (var insertedTrackingEvent in targetingSaveDto.InsertedTrackingEvents)
                        {
                            if (adGroupObj.TrackingEvents != null && adGroupObj.TrackingEvents.Where(p => !p.IsDeleted).Count() == Configuration.MaxAdGroupTrackingEvents)
                            {
                                throw new BusinessException(new List<ErrorData>() { new ErrorData("ReachTrackingEventsLimit") });
                            }
                            /* if (insertedTrackingEvent.ValidFor > Domain.Configuration.TrackingEvent_ValidForSecondMax || insertedTrackingEvent.ValidFor < Domain.Configuration.TrackingEvent_ValidForSecondMin)
                             {
                                 throw new BusinessException(new List<ErrorData>() { new ErrorData("TrackingEventInvalidValidForSecond") });
                             }
                             */

                            //removed you can add events with out check uniq
                            if (CheckEventUniqueByCodeToSave(insertedTrackingEvent.Code).Value)
                            {
                                AddAdGroupTrackingEvent(adGroupObj, insertedTrackingEvent);
                            }
                        }
                    }

                    if (adGroupObj.CostModelWrapperEnum == CostModelWrapperEnum.CPA)
                    {
                        var itemBilling = adGroupObj.TrackingEvents.Where(M => M.IsBillable == true && M.IsDeleted == false).SingleOrDefault();
                        if (itemBilling == null)
                        {
                            var error = new BusinessException();
                            error.Errors.Add(new ErrorData { ID = "MustHaveBilling" });
                            throw error;
                        }
                    }
                    //}

                    #endregion

                    #region CostElemets

                    if (targetingSaveDto.InsertedCostElements != null)
                    {
                        //foreach (var insertedCostElement in targetingSaveDto.InsertedCostElements)
                        //{
                        //    var costElement = new AdGroupCostElement
                        //    {
                        //        Beneficiary = insertedCostElement.BeneficiaryId.HasValue ? partyRepository.Get(insertedCostElement.BeneficiaryId.Value) : null,
                        //        CostElement = costElementRepository.Get(insertedCostElement.CostElementId),
                        //        Value = insertedCostElement.Value
                        //    };
                        //    //TODO:Osaleh to review this and move it to more suitable place
                        //    if (costElement.CostElement.Type == CostElementType.Percentage)
                        //    {
                        //        costElement.Value = costElement.Value / 100.0M;
                        //    }
                        //    else
                        //    {
                        //        if (!costElement.CostElement.IsOneTime)
                        //        {
                        //            var costModelWrapper = costModelWrapperRepository.Get(insertedCostElement.CostModelWrapperId);
                        //            costElement.Value = costElement.Value / costModelWrapper.Factor;
                        //        }
                        //    }

                        //    item.AddGroupCostElement(adGroupObj, costElement);
                        //}
                    }

                    if (targetingSaveDto.DeletedCostElements != null)
                    {
                        //var costElements = item.GetGroupCostElements(adGroupObj);

                        //foreach (var deletedCostElement in targetingSaveDto.DeletedCostElements)
                        //{
                        //    var element = costElements.FirstOrDefault(x => x.ID == deletedCostElement);
                        //    if (element != null)
                        //    {
                        //        item.RemoveGroupCostElement(adGroupObj, element);
                        //    }
                        //}
                    }

                    if (!oldCostModelWrapper.HasValue || oldCostModelWrapper.Value != adGroupObj.CostModelWrapper.ID || adGroupObj.IsCostModelChanged)
                    {
                        if (targetingSaveDto.UpdatedCostElements != null && targetingSaveDto.UpdatedCostElements.Count != 0)
                        {
                            foreach (var updatedCostElement in targetingSaveDto.UpdatedCostElements)
                            {
                                var costElement = adGroupObj.GetCurrentCostElements().Where(p => p.ID == updatedCostElement.Key).SingleOrDefault();

                                if (costElement != null)
                                {
                                    costElement.SetCostElementValue(updatedCostElement.Value, adGroupObj.CostModelWrapper);
                                }
                            }

                            adGroupObj.IsCostModelChanged = false;
                        }
                        else
                        {
                            if (adGroupObj.CostModelWrapper != null)
                            {
                                adGroupObj.IsCostModelChanged = true;

                                foreach (var adCreative in adGroupObj.GetAds())
                                {
                                    item.SetCreativeStatus(adGroupObj, adCreative, false);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Fees
                    if (targetingSaveDto.FeesAddList != null)
                    {
                        var feesItem = this._FeeRepository.GetAll();
                        foreach (var feeItem in targetingSaveDto.FeesAddList)
                        {
                            var feeLook = feesItem.Where(M => M.ID == feeItem.FeeId).SingleOrDefault();
                            if (feeLook != null && feeItem.IsAdded)
                            {
                                if (feeItem.ID > 0 && !feeLook.IsAutoAdded)
                                {
                                    var itemfeeWxists = adGroupObj.GetCurrentFees().Where(M => M.ID == feeItem.ID).SingleOrDefault();



                                    if (itemfeeWxists != null && Math.Round(itemfeeWxists.GetReadableValue(), 2) != feeItem.Value || (itemfeeWxists != null && ((oldCostModelWrapper.HasValue && oldCostModelWrapper.Value != adGroupObj.CostModelWrapper.ID) || adGroupObj.IsCostModelChanged)))
                                    {
                                        if (feeLook.Type == CalculationType.Fixed)
                                        {
                                            feeItem.FromDate = Framework.Utilities.Environment.GetServerTime();

                                            adGroupObj.RemoveGroupFee(itemfeeWxists);
                                            feeItem.ID = 0;
                                            feeItem.BeneficiaryId = itemfeeWxists.Beneficiary != null ? (int?)itemfeeWxists.Beneficiary.ID : null;
                                            AddFees(feeItem, adGroupObj, item);
                                        }
                                        else
                                        {

                                            UpdateFees(feeItem, adGroupObj, item);
                                        }
                                        //UpdateFees(feeItem, adGroupObj, item);

                                    }
                                }
                                else if (!feeLook.IsAutoAdded)
                                {
                                    feeItem.FromDate = Framework.Utilities.Environment.GetServerTime();
                                    AddFees(feeItem, adGroupObj, item);
                                }

                            }
                            else if (feeLook != null && !feeLook.IsAutoAdded)
                            {
                                if (feeItem.ID > 0)
                                {
                                    var itemfeeWxists = adGroupObj.GetCurrentFees().Where(M => M.ID == feeItem.ID).SingleOrDefault();
                                    if (itemfeeWxists != null)
                                    {
                                        adGroupObj.RemoveGroupFee(itemfeeWxists);
                                    }
                                }

                            }
                            else if (feeLook != null && feeLook.IsAutoAdded)
                            {
                                if ((oldCostModelWrapper.HasValue && oldCostModelWrapper.Value != adGroupObj.CostModelWrapper.ID))// || adGroupObj.IsCostModelChanged
                                {
                                    if (feeItem.ID > 0)
                                    {
                                        var itemfeeWxists = adGroupObj.GetCurrentFees().Where(M => M.ID == feeItem.ID).SingleOrDefault();
                                        if (itemfeeWxists != null)
                                        {
                                            if (feeLook.Type == CalculationType.Percentage)
                                            {
                                                UpdateFees(feeItem, adGroupObj, item);
                                            }
                                            else
                                            {
                                                feeItem.FromDate = Framework.Utilities.Environment.GetServerTime();

                                                adGroupObj.RemoveGroupFee(itemfeeWxists);
                                                feeItem.ID = 0;
                                                feeItem.BeneficiaryId = itemfeeWxists.Beneficiary != null ? (int?)itemfeeWxists.Beneficiary.ID : null;
                                                AddFees(feeItem, adGroupObj, item);

                                            }
                                        }
                                    }
                                }
                                //to be discussed with the business
                                else
                                {
                                    if (feeItem.ID > 0)
                                    {
                                        var itemfeeWxists = adGroupObj.GetCurrentFees().Where(M => M.ID == feeItem.ID).SingleOrDefault();
                                        if (itemfeeWxists != null)
                                        {
                                            adGroupObj.RemoveGroupFee(itemfeeWxists);
                                        }
                                    }


                                }

                            }


                        }

                    }
                    else
                    {
                        var feesItem = this._FeeRepository.GetAll();
                        foreach (var feeItem in feesItem)
                        {

                            if (adGroupObj.GetCurrentFees()!=null)
                            {
                                var itemfeeWxists = adGroupObj.GetCurrentFees().Where(M => M.Fee.ID == feeItem.ID).SingleOrDefault();
                                if (itemfeeWxists != null)
                                {
                                    adGroupObj.RemoveGroupFee(itemfeeWxists);
                                }
                            }
                           
                        }
                    }

                    #endregion
                    #region Defaul CostElment
                    if (isSetDefaulAccountCostModel)
                        dtoResult.AddDefaultCostElement = adGroupObj.SetAccountCostElmentsSaved(OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value);

                    if (isSetDefaulAccountCostModel)
                        dtoResult.AddDefaultFee = adGroupObj.SetAccountFeesSaved(OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value);
                    #endregion

                    #region URL Targeting
                    var urlTargetings = (adGroupObj.Targetings.ToList().OfType<URLTargeting>()).ToList();
                    //old imp
                    if (targetingSaveDto.InsertedURLTargeting != null)
                    {
                        foreach (var insertedURLTargeting in targetingSaveDto.InsertedURLTargeting)
                        {
                            URLTargeting urlTargeting = new URLTargeting()
                            {
                                URL = insertedURLTargeting.URL,
                                Type = targetingTypeRepository.Get(urlTargetingTypeId)
                            };

                            item.AddGroupTargeting(adGroupObj, urlTargeting);
                        }

                    }

                    if (targetingSaveDto.DeletedURLTargeting != null)
                    {
                        foreach (var deletedURLTarget in targetingSaveDto.DeletedURLTargeting)
                        {
                            var urlTargeting =
                                urlTargetings.FirstOrDefault(targeting => targeting.ID == deletedURLTarget);
                            if (urlTargeting != null)
                            {
                                item.RemoveGroupTargeting(adGroupObj, urlTargeting);
                            }
                        }
                    }
                    //end old imp

                    if (targetingSaveDto.AllURLs == null)
                    { targetingSaveDto.AllURLs = new List<URLTargetingDto>(); }
                    if (urlTargetings != null)
                    {
                        foreach (var url in urlTargetings)
                        {



                            if (targetingSaveDto.AllURLs.Where(m=>m.ID== url.ID).FirstOrDefault()==null)
                            {
                                var deletedKeywordTargeting =
                               urlTargetings.FirstOrDefault(
                                   targeting => targeting.ID == url.ID);
                                if (deletedKeywordTargeting != null)
                                    item.RemoveGroupTargeting(adGroupObj, deletedKeywordTargeting);
                            }
                        }
                    }
                    urlTargetings = (adGroupObj.Targetings.ToList().OfType<URLTargeting>()).ToList();
                    foreach (var URL in targetingSaveDto.AllURLs)
                    {
                        if (urlTargetings.Where(M=>M.ID== URL.ID).FirstOrDefault()==null)
                        {
                            URLTargeting urlTargeting = new URLTargeting()
                            {
                                URL = URL.URL,
                                Type = targetingTypeRepository.Get(urlTargetingTypeId)
                            };

                            item.AddGroupTargeting(adGroupObj, urlTargeting);
                        }
                    
                    }
                    
                    #endregion

                    #region Operators

                    #region IP Range
                    var ipRangesTargetings = (adGroupObj.Targetings.ToList().OfType<IPTargeting>()).ToList();
                    if (targetingSaveDto.OperatorTargetingIsAll != 4)//IP Ranges
                    {
                        //clear all IP targeting
                        foreach (var ipRangesTargeting in ipRangesTargetings)
                        {
                            item.RemoveGroupTargeting(adGroupObj, ipRangesTargeting);
                        }
                    }
                    else
                    {//old imp
                        if (targetingSaveDto.DeletedIPRanges != null)
                        {
                            foreach (var deletedIPRange in targetingSaveDto.DeletedIPRanges)
                            {
                                var deletedIPRangeTargeting =
                                    ipRangesTargetings.FirstOrDefault(targeting => targeting.ID == deletedIPRange);
                                if (deletedIPRangeTargeting != null)
                                {
                                    item.RemoveGroupTargeting(adGroupObj, deletedIPRangeTargeting);
                                }
                            }
                        }

                        if (targetingSaveDto.InsertedIPRanges != null)
                        {
                            foreach (var insertedIPRange in targetingSaveDto.InsertedIPRanges)
                            {
                                if (string.IsNullOrWhiteSpace(insertedIPRange.StartRange) ||
                                    string.IsNullOrWhiteSpace(insertedIPRange.EndRange))
                                {
                                    if (insertedIPRange.StartRange != null && !insertedIPRange.StartRange.Contains("/"))
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                }
                                ipRangesTargetings = (adGroupObj.Targetings.ToList().OfType<IPTargeting>()).ToList();
                                byte[] startRange, endRange;
                                if (insertedIPRange.StartRange.Contains("/") && IpHelper.ParseCidrIP(insertedIPRange.StartRange) != null) //CIDR format
                                {
                                    IpRange ipRange = IpHelper.ParseCidrIP(insertedIPRange.StartRange);
                                    startRange = ipRange.StartRange;
                                    endRange = ipRange.EndRange;
                                }
                                else
                                {
                                    IPAddress ipAddress;
                                    if (!IPAddress.TryParse(insertedIPRange.StartRange, out ipAddress))
                                    {
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                    }
                                    if (!IPAddress.TryParse(insertedIPRange.EndRange, out ipAddress))
                                    {
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                    }


                                    startRange = IpHelper.ConvertIPToBytes(insertedIPRange.StartRange);
                                    endRange = IpHelper.ConvertIPToBytes(insertedIPRange.EndRange);
                                }
                                IPTargeting ipTargeting = null;

                                //TODO:Osaleh to validate the new IP range
                                //get lower ip

                                IList<IpRange> lst = ipRangesTargetings.Select(x => new IpRange() { StartRangeBigInt = IpHelper.ToBigInteger(x.StartRange).Value, EndRangeBigInt = IpHelper.ToBigInteger(x.EndRange).Value }).ToList();
                                //var lowerTargeting = lst.FirstOrDefault(x => x.StartRangeBigInt >= IpHelper.ToBigInteger(startRange) && x.StartRangeBigInt <= IpHelper.ToBigInteger(endRange));
                                //if (lowerTargeting != null)
                                //{
                                //    // move the range start to the lower value
                                //    lowerTargeting.StartRange = startRange;
                                //    continue;
                                //}

                                //var upperTargeting = lst.FirstOrDefault(x => x.EndRangeBigInt >= IpHelper.ToBigInteger(startRange) && x.EndRangeBigInt <= IpHelper.ToBigInteger(endRange));
                                //if (upperTargeting != null)
                                //{
                                //    // move the range end to the upper value
                                //    upperTargeting.EndRange = endRange;
                                //    continue;
                                //}

                                if (lst.Any(x => IpHelper.ToBigInteger(startRange) >= x.StartRangeBigInt && IpHelper.ToBigInteger(endRange) <= x.EndRangeBigInt))
                                {
                                    //new targeting is already on one of the save ranges
                                    continue;
                                }

                                //create targeting Object
                                ipTargeting = new IPTargeting
                                {
                                    Type = targetingTypeRepository.Get(ipRangeTargetingTypeId),
                                    Description = insertedIPRange.Description,
                                    StartRange = startRange,
                                    EndRange = endRange
                                };
                                item.AddGroupTargeting(adGroupObj, ipTargeting);
                            }
                        }
                        //end old imp



                        if (targetingSaveDto.AllIPRanges == null)
                        { targetingSaveDto.AllIPRanges = new List<IPTargetingDto>(); }
                        if (ipRangesTargetings != null)
                        {
                            foreach (var ip in ipRangesTargetings)
                            {



                                if (targetingSaveDto.AllIPRanges.Where(m => m.ID == ip.ID).FirstOrDefault() == null)
                                {
                                    var deletedIPTargeting =
                                   ipRangesTargetings.FirstOrDefault(
                                       targeting => targeting.ID == ip.ID);
                                    if (deletedIPTargeting != null)
                                        item.RemoveGroupTargeting(adGroupObj, deletedIPTargeting);
                                }
                            }
                        }
                        ipRangesTargetings = (adGroupObj.Targetings.ToList().OfType<IPTargeting>()).ToList();
                        foreach (var insertedIPRange in targetingSaveDto.AllIPRanges)
                        {
                            if (ipRangesTargetings.Where(M => M.ID == insertedIPRange.ID).FirstOrDefault() == null)
                            {
                                if (string.IsNullOrWhiteSpace(insertedIPRange.StartRange) ||
                                    string.IsNullOrWhiteSpace(insertedIPRange.EndRange))
                                {
                                    if (insertedIPRange.StartRange != null && !insertedIPRange.StartRange.Contains("/"))
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                }
                                ipRangesTargetings = (adGroupObj.Targetings.ToList().OfType<IPTargeting>()).ToList();
                                byte[] startRange, endRange;
                                if (insertedIPRange.StartRange.Contains("/") && IpHelper.ParseCidrIP(insertedIPRange.StartRange) != null) //CIDR format
                                {
                                    IpRange ipRange = IpHelper.ParseCidrIP(insertedIPRange.StartRange);
                                    startRange = ipRange.StartRange;
                                    endRange = ipRange.EndRange;
                                }
                                else
                                {
                                    IPAddress ipAddress;
                                    if (!IPAddress.TryParse(insertedIPRange.StartRange, out ipAddress))
                                    {
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                    }
                                    if (!IPAddress.TryParse(insertedIPRange.EndRange, out ipAddress))
                                    {
                                        throw new BusinessException(new List<ErrorData>() { new ErrorData("InvalidIPEntry") });
                                    }


                                    startRange = IpHelper.ConvertIPToBytes(insertedIPRange.StartRange);
                                    endRange = IpHelper.ConvertIPToBytes(insertedIPRange.EndRange);
                                }
                                IPTargeting ipTargeting = null;

                                //TODO:Osaleh to validate the new IP range
                                //get lower ip

                                IList<IpRange> lst = ipRangesTargetings.Select(x => new IpRange() { StartRangeBigInt = IpHelper.ToBigInteger(x.StartRange).Value, EndRangeBigInt = IpHelper.ToBigInteger(x.EndRange).Value }).ToList();
                                

                                if (lst.Any(x => IpHelper.ToBigInteger(startRange) >= x.StartRangeBigInt && IpHelper.ToBigInteger(endRange) <= x.EndRangeBigInt))
                                {
                                    //new targeting is already on one of the save ranges
                                    continue;
                                }

                                //create targeting Object
                                ipTargeting = new IPTargeting
                                {
                                    Type = targetingTypeRepository.Get(ipRangeTargetingTypeId),
                                    Description = insertedIPRange.Description,
                                    StartRange = startRange,
                                    EndRange = endRange
                                };
                                item.AddGroupTargeting(adGroupObj, ipTargeting);


                            }

                        }
                    }

                    #endregion
                    //load Operators targeting
                    foreach (var @operator in targetingSaveDto.Operators)
                    {
                        var targetingObj = (adGroupObj.Targetings.ToList().OfType<OperatorTargeting>().FirstOrDefault(targeting => targeting.Operator.ID == @operator));
                        if (targetingObj == null)
                        {
                            //if not found then add it
                            var operatorItem = operatorRepository.Get(@operator);
                            if (operatorItem != null)
                            {
                                //create targeting Object
                                targetingObj = new OperatorTargeting();
                                targetingObj.Type = targetingTypeRepository.Get(operatorTargetingTypeId);
                                targetingObj.Operator = operatorItem;
                                item.AddGroupTargeting(adGroupObj, targetingObj);
                            }
                           
                        }
                    }

                    //Load List after the Update
                    foreach (var currentOperator in adGroupObj.Targetings.ToList().OfType<OperatorTargeting>())
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        var found = false;
                        foreach (var @operator in targetingSaveDto.Operators)
                        {
                            if (currentOperator.Operator.ID == @operator)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            //we should remove the targeting
                            item.RemoveGroupTargeting(adGroupObj, currentOperator);
                        }
                    }
                    // set Disable Proxy Traffic flag
                    adGroupObj.DisableProxyTraffic = targetingSaveDto.DisableProxyTraffic;
                    adGroupObj.IsWifi = targetingSaveDto.IsWifi;
                    adGroupObj.IsCellular = targetingSaveDto.IsCellular;
                    adGroupObj.SetConnectionValue(targetingSaveDto.TargetingConnectionType);

                    adGroupObj.AllowOpenAuction = targetingSaveDto.AllowOpenAuction;
                    #endregion

                    #region Geographies
                    //load Geographic targeting
                    //if (targetings.OfType<GeographicTargeting>().ToList().Count > 0)
                    {
                        foreach (var geographic in targetingSaveDto.Geographies)
                        {
                            var targetingObj =
                                adGroupObj.Targetings.ToList().OfType<GeographicTargeting>().FirstOrDefault(
                                    targeting => targeting.Location.ID == geographic);
                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var locationItem = locationRepository.Get(geographic);
                                if (locationItem != null)
                                {
                                    //create targeting Object
                                    targetingObj = new GeographicTargeting
                                    {
                                        Type = targetingTypeRepository.Get(geographicTargetingTypeId),
                                        Location = locationItem
                                    };

                                }
                                item.AddGroupTargeting(adGroupObj, targetingObj);
                            }
                        }
                        //Load List after the Update
                        foreach (var currentGeographic in adGroupObj.Targetings.ToList().OfType<GeographicTargeting>())
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
                                item.RemoveGroupTargeting(adGroupObj, currentGeographic);
                            }
                        }
                    }
                    #endregion

                    #region GeoFencing

                    var geoFencingTargetings = (adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>()).ToList();

                    //old imp
                    if (targetingSaveDto.InsertedGeoFencings != null)
                    {
                        foreach (var insertedGeoFencing in targetingSaveDto.InsertedGeoFencings)
                        {
                            GeoFencingTargeting geoFencingTargeting = new GeoFencingTargeting()
                            {
                                Type = targetingTypeRepository.Get(geoFencingTargetingTypeId),
                                Longitude = insertedGeoFencing.Longitude,
                                Latitude = insertedGeoFencing.Latitude,
                                Radius = insertedGeoFencing.Radius
                            };

                            item.AddGroupTargeting(adGroupObj, geoFencingTargeting);
                        }
                    }
                    if (targetingSaveDto.DeletedGeoFencings != null)
                    {
                        foreach (var deletedGeoFencing in targetingSaveDto.DeletedGeoFencings)
                        {
                            var deletedGeoFencingTargeting =
                                geoFencingTargetings.FirstOrDefault(targeting => targeting.ID == deletedGeoFencing);
                            if (deletedGeoFencingTargeting != null)
                            {
                                item.RemoveGroupTargeting(adGroupObj, deletedGeoFencingTargeting);
                            }
                        }
                    }
                    //end imp 



                    if (targetingSaveDto.AllGeoFencing == null)
                    { targetingSaveDto.AllGeoFencing = new List<GeoFencingUITargeting>(); }
                    if (geoFencingTargetings != null)
                    {
                        foreach (var geofencing in geoFencingTargetings)
                        {



                            if (targetingSaveDto.AllGeoFencing.Where(m => m.ID == geofencing.ID).FirstOrDefault() == null)
                            {
                                var deletedIPTargeting =
                               geoFencingTargetings.FirstOrDefault(
                                   targeting => targeting.ID == geofencing.ID);
                                if (deletedIPTargeting != null)
                                    item.RemoveGroupTargeting(adGroupObj, deletedIPTargeting);
                            }
                        }
                    }
                    geoFencingTargetings = (adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>()).ToList();
                    foreach (var insertedGeo in targetingSaveDto.AllGeoFencing)
                    {
                        if (geoFencingTargetings.Where(M => M.ID == insertedGeo.ID).FirstOrDefault() == null)
                        {
                            GeoFencingTargeting geoFencingTargeting = new GeoFencingTargeting()
                            {
                                Type = targetingTypeRepository.Get(geoFencingTargetingTypeId),
                                Longitude = insertedGeo.Longitude,
                                Latitude = insertedGeo.Latitude,
                                Radius = insertedGeo.Radius
                            };

                            item.AddGroupTargeting(adGroupObj, geoFencingTargeting);


                        }

                    }

                    if (adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().FirstOrDefault() != null)
                    {
                        var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > 0).ToList();
                        if (results != null && results.Count > 0)
                            isGeoFencingTargeting = true;
                    }

                    #endregion

                    #region Device
                    //load Device targeting
                    {
                        var devicetargting = adGroupObj.Targetings.ToList().OfType<DeviceTargeting>().FirstOrDefault();
                        if (devicetargting == null)
                        {
                            //create new 
                            devicetargting = new DeviceTargeting { Type = targetingTypeRepository.Get(deviceTargetingTypeId), AdGroup = adGroupObj };
                        }
                        if (targetingSaveDto.DeviceTargetingTypeId > 0)
                        {
                            devicetargting.TargetingType = deviceTargetingTypeRepository.Get(targetingSaveDto.DeviceTargetingTypeId);
                        }
                        else
                        {
                            devicetargting.TargetingType = null;
                        }
                        if (devicetargting.TargetingType == null)
                            devicetargting.IsAll = true;

                        #region Handle DeviceType Targeting

                        // fix DeviceTypeId value
                        if (targetingSaveDto.DeviceTypeIds == null)
                        {
                            // ALID
                            if (adGroupObj.Objective.AdAction.Constraints != null && adGroupObj.Objective.AdAction.Constraints.Count > 0 && adGroupObj.Objective.AdAction.Constraints[0].DeviceConstraint != -1)
                            {
                                targetingSaveDto.DeviceTypeIds = adGroupObj.Objective.AdAction.Constraints.Select(x => x.DeviceConstraint).ToArray();
                            }
                        }

                        if (targetingSaveDto.DeviceTypeIds != null && targetingSaveDto.DeviceTypeIds.Count() > 0 && targetingSaveDto.DeviceTypeIds.Where(x => x != (int)DeviceTypeEnum.Any).Count() > 0)
                        {
                            devicetargting.IsAll = false;
                        }

                        if (targetingSaveDto.DeviceTypeIds != null && targetingSaveDto.DeviceTypeIds.Count() > 0)
                        {
                            var deletedDeviceTypeIds = devicetargting.DeviceTypeTargetings.Select(x => x.DeviceType.ID).Except(targetingSaveDto.DeviceTypeIds).ToList();
                            foreach (int deviceTypeId in deletedDeviceTypeIds)
                            {
                                DeviceType deviceType = deviceTypeRepository.Get(deviceTypeId);
                                devicetargting.RemoveDeviceType(deviceType);
                            }

                            foreach (int deviceTypeId in targetingSaveDto.DeviceTypeIds)
                            {

                                if (devicetargting.DeviceTypeTargetings.Count == 0)
                                {
                                    DeviceType deviceType = deviceTypeRepository.Get(deviceTypeId);
                                    devicetargting.AddDeviceType(deviceType);
                                }
                                else
                                {
                                    // ALID
                                    //var deviceTypeTargeting = devicetargting.DeviceTypeTargetings.SingleOrDefault();
                                    var deviceTypeTargeting = devicetargting.DeviceTypeTargetings.Where(x => x.DeviceType.ID == deviceTypeId).SingleOrDefault();

                                    if (deviceTypeTargeting != null && deviceTypeTargeting.DeviceType.ID != deviceTypeId)
                                    {
                                        deviceTypeTargeting.DeviceType = deviceTypeRepository.Get(deviceTypeId);
                                        devicetargting.ChangeDeviceType(deviceTypeTargeting);
                                    }
                                    else if (deviceTypeTargeting == null)
                                    {
                                        DeviceType deviceType = deviceTypeRepository.Get(deviceTypeId);
                                        devicetargting.AddDeviceType(deviceType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            devicetargting.ClearDeviceTypeTargeting();
                        }

                        #endregion

                        switch (targetingSaveDto.DeviceTargetingTypeId)
                        {
                            case 1://Platform
                                {
                                    devicetargting.IsAll = false;
                                    if (devicetargting.Devices != null)
                                        devicetargting.ClearDevices();
                                    if (devicetargting.Manufacturers != null)
                                        devicetargting.ClearManufacturers();
                                    if (devicetargting.DeviceCapabilities != null)
                                        devicetargting.ClearDeviceCapabilities();
                                    break;
                                }
                            case 2://Manufacturers
                                {
                                    devicetargting.IsAll = false;
                                    if (devicetargting.Devices != null)
                                        devicetargting.ClearDevices();
                                    if (devicetargting.Platforms != null)
                                        devicetargting.ClearPlatforms();
                                    if (devicetargting.DeviceCapabilities != null)
                                        devicetargting.ClearDeviceCapabilities();
                                    break;
                                }
                            case 3://Devices
                                {
                                    devicetargting.IsAll = false;
                                    if (devicetargting.DeviceCapabilities != null)
                                        devicetargting.ClearDeviceCapabilities();
                                    break;
                                }
                            case 4://Devices,Manufacturers or Platforms
                                {
                                    devicetargting.IsAll = false;
                                    if (devicetargting.DeviceCapabilities != null)
                                        devicetargting.ClearDeviceCapabilities();
                                    if (targetingSaveDto.isFromHouseAd == false)
                                        FixDeviceTargetingTree(ref targetingSaveDto);
                                    break;
                                }
                            case 5://Device Capabilities
                                {
                                    devicetargting.IsAll = false;
                                    if (devicetargting.Manufacturers != null)
                                        devicetargting.ClearManufacturers();
                                    if (devicetargting.Platforms != null)
                                        devicetargting.ClearPlatforms();
                                    if (devicetargting.Devices != null)
                                        devicetargting.ClearDevices();
                                    break;
                                }
                        }
                        #region Platforms




                        foreach (var platformsTargeting in devicetargting.PlatformsTargeting)
                        {
                            platformsTargeting.IsAll = false;
                        }
                        //devicetargting.ResetIsAll(targetingSaveDto.Platforms, DeviceTargetingTypeEnum.Platform);



                        foreach (var platform in targetingSaveDto.Platforms)
                        {
                            if (targetingSaveDto.DeviceTargetingTypeId == 4)
                            {
                                paltformIsAll = targetingSaveDto.platfromTree.Where(x => x.Id == platform.Key).Select(x => x.IsAll).SingleOrDefault();
                            }

                            var targetingObj = devicetargting.PlatformsTargeting.FirstOrDefault(targtingPaltform => targtingPaltform.Platform.ID == platform.Key);
                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var platformItem = platformRepository.Get(platform.Key);
                                if (platformItem != null)
                                {
                                    //create targeting Object
                                    devicetargting.AddPatform(platformItem, platform.Value, paltformIsAll);
                                }
                            }
                            else
                            {
                                if (targetingObj.MinimumVersion != platform.Value)
                                {
                                    devicetargting.ChangePlatformTargeting(targetingObj.Platform.ID, platform.Value, paltformIsAll);
                                }
                                else
                                {
                                    targetingObj.IsAll = paltformIsAll;
                                }
                            }
                        }



                        //TODO:Osaleh to change this code 
                        //Load platform from Models
                        foreach (var model in targetingSaveDto.Models)
                        {
                            var deviceObj = deviceRepository.Get(model);
                            if (deviceObj != null)
                            {
                                if (!targetingSaveDto.Platforms.Keys.Contains(deviceObj.Platform.ID))
                                {
                                    targetingSaveDto.Platforms.Add(deviceObj.Platform.ID, null);
                                }
                            }
                        }

                        //Load List after the Update
                        var deviceTargtingPlatforms = devicetargting.Platforms.ToList();
                        foreach (var targtingPaltform in from targtingPaltform in deviceTargtingPlatforms
                                                         let found = targetingSaveDto.Platforms.Any(paltform => paltform.Key == targtingPaltform.ID)
                                                         where found == false
                                                         select targtingPaltform)
                        {
                            //we should remove the targeting
                            devicetargting.RemovePatform(targtingPaltform);
                        }
                        #endregion

                        #region Manufacturers

                        foreach (var manufacturerTargeting in devicetargting.ManufacturersTargeting)
                        {
                            manufacturerTargeting.IsAll = false;
                        }
                        //devicetargting.ResetIsAll(targetingSaveDto.Manufacturers, DeviceTargetingTypeEnum.Manufacturer);

                        foreach (var manufacturers in targetingSaveDto.Manufacturers)
                        {
                            if (targetingSaveDto.DeviceTargetingTypeId == 4)
                            {
                                var manufacturerslist = targetingSaveDto.platfromTree.Select(x => x.Manu).ToList();
                                List<ManuTree> manufacturerslistcombined = new List<ManuTree>();
                                foreach (var index in manufacturerslist)
                                {
                                    foreach (ManuTree tree in index)
                                        manufacturerslistcombined.Add(tree);

                                }

                                manufacturerIsAll = manufacturerslistcombined.Where(x => x.Id == manufacturers).Select(x => x.IsAll).SingleOrDefault();
                            }

                            var targetingObj = devicetargting.ManufacturersTargeting.FirstOrDefault(targtingManufacturer => targtingManufacturer.Manufacturer.ID == manufacturers);

                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var manufacturerItem = manufacturerRepository.Get(manufacturers);
                                if (manufacturerItem != null)
                                {
                                    //create targeting Object
                                    devicetargting.AddManufacturer(manufacturerItem, manufacturerIsAll);
                                }
                            }
                            else
                            {
                                targetingObj.IsAll = manufacturerIsAll;
                            }
                        }

                        //TODO:Osaleh to change this code 
                        //Load manufacturers from Models
                        foreach (var model in targetingSaveDto.Models)
                        {
                            var deviceObj = deviceRepository.Get(model);
                            if (deviceObj != null)
                            {
                                if (!targetingSaveDto.Manufacturers.Contains(deviceObj.Manufacturer.ID))
                                {
                                    targetingSaveDto.Manufacturers.Add(deviceObj.Manufacturer.ID);
                                }
                            }
                        }
                        //Load List after the Update
                        var deviceTargtingManufacturers = devicetargting.Manufacturers.ToList();
                        foreach (var targtingManufacturer in from targtingManufacturer in deviceTargtingManufacturers
                                                             let found = targetingSaveDto.Manufacturers.Any(paltform => paltform == targtingManufacturer.ID)
                                                             where found == false
                                                             select targtingManufacturer)
                        {
                            //we should remove the targeting
                            devicetargting.RemoveManufacturer(targtingManufacturer);
                        }
                        #endregion

                        #region Models
                        //Models
                        foreach (var model in targetingSaveDto.Models)
                        {

                            var targetingObj = devicetargting.Devices.FirstOrDefault(
                                    targtingDevice => targtingDevice.ID == model);
                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var deviceItem = deviceRepository.Get(model);
                                if (deviceItem != null)
                                {
                                    //create targeting Object
                                    devicetargting.AddDevice(deviceItem);
                                }
                            }
                        }

                        //Load List after the Update
                        var deviceTargtingModels = devicetargting.Devices.ToList();
                        foreach (var targtingDevice in from targtingDevice in deviceTargtingModels
                                                       let found = targetingSaveDto.Models.Any(paltform => paltform == targtingDevice.ID)
                                                       where found == false
                                                       select targtingDevice)
                        {
                            //we should remove the targeting
                            devicetargting.RemoveDevice(targtingDevice);
                        }
                        #endregion


                        #region Device Capabilities
                        foreach (var deviceCapability in targetingSaveDto.DeviceCapabilities)
                        {

                            var targetingObj = devicetargting.DeviceCapabilities.FirstOrDefault(targtingCapability => targtingCapability.ID == deviceCapability);
                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var deviceCapabilityItem = deviceCapabilityRepository.Get(deviceCapability);
                                if (deviceCapabilityItem != null)
                                {
                                    //create targeting Object
                                    devicetargting.AddDeviceCapability(deviceCapabilityItem);
                                }
                            }
                            else
                            {
                                devicetargting.ChangeDeviceCapabilityIncludeStatus(targetingObj);
                            }
                        }
                        //Exclude Device Capabilities
                        foreach (var deviceCapability in targetingSaveDto.ExcludeDeviceCapability)
                        {

                            var targetingObj = devicetargting.DeviceCapabilities.FirstOrDefault(targtingCapability => targtingCapability.ID == deviceCapability);
                            if (targetingObj == null)
                            {
                                //if not found then add it
                                var deviceCapabilityItem = deviceCapabilityRepository.Get(deviceCapability);
                                if (deviceCapabilityItem != null)
                                {
                                    //create targeting Object
                                    devicetargting.AddDeviceCapability(deviceCapabilityItem, isInclude: false);
                                }
                            }
                            else
                            {
                                devicetargting.ChangeDeviceCapabilityIncludeStatus(targetingObj, isInclude: false);
                            }
                        }

                        //Load List after the Update
                        //merge all device capabilities
                        var allDeviceCapabilities = targetingSaveDto.DeviceCapabilities.ToList();
                        allDeviceCapabilities.AddRange(targetingSaveDto.ExcludeDeviceCapability.ToList());

                        var deviceTargtingDeviceCapabilities = devicetargting.DeviceCapabilities.ToList();
                        foreach (var targtingDeviceCapability in from targtingDeviceCapability in deviceTargtingDeviceCapabilities
                                                                 let found = allDeviceCapabilities.Any(deviceCapability => deviceCapability == targtingDeviceCapability.ID)
                                                                 where found == false
                                                                 select targtingDeviceCapability)
                        {
                            //we should remove the targeting
                            devicetargting.RemoveDeviceCapability(targtingDeviceCapability);
                        }

                        #endregion
                        devicetargting.AdGroup = adGroupObj;
                        //test 
                        //devicetargting.GroupId = adGroupObj.ID;
                        item.AddGroupTargeting(adGroupObj, devicetargting);

                    }
                    #endregion

                    #region Keywords
                    //load Keyword targeting
                    //handle the keywords
                    var keywordsTargetings = (adGroupObj.Targetings.ToList().OfType<KeywordTargeting>()).ToList();
                    if(keywordsTargetings!=null)
                    {
                        foreach (var allKeyword in keywordsTargetings)
                        {
                            allKeyword.Include = targetingSaveDto.AllowInclude;



                        }
                    }

                    //old imp
                    if (targetingSaveDto.NewKeywords != null)
                    {
                        foreach (var newKeyword in targetingSaveDto.NewKeywords)
                        {
                            KeywordTargeting keywordTargeting = null;
                            int id;
                            Keyword newKeywordObj = null;
                            if (Int32.TryParse(newKeyword, out id))
                            {
                                keywordTargeting = keywordsTargetings.FirstOrDefault(targeting => targeting.Keyword.ID == id);
                                if (keywordTargeting == null)
                                {

                                    newKeywordObj = keyWordRepository.Get(Convert.ToInt32(newKeyword));
                                    newKeywordObj.Usage++;
                                    //create targeting Object
                                    keywordTargeting = new KeywordTargeting();
                                    keywordTargeting.Type = targetingTypeRepository.Get(keywordTargetingTypeId);
                                    keywordTargeting.Keyword = newKeywordObj;
                                    keywordTargeting.Include = targetingSaveDto.AllowInclude;
                                }
                               
                                if (keywordTargeting != null)
                                {
                                    item.AddGroupTargeting(adGroupObj, keywordTargeting);
                                }
                            }
                            /*else
                            {
                                newKeywordObj = new Keyword();
                                //ToDO: Osaleh to find other way to insert the Keyword to all supported languages
                                newKeywordObj.Name =
                                    new Framework.DomainServices.Localization.LocalizedString("AppSiteGroup");
                                newKeywordObj.Name.SetValue(newKeyword, "en-US");
                                newKeywordObj.Name.SetValue(newKeyword, "ar-JO");
                                newKeywordObj.Usage = 1;
                                keyWordRepository.Save(newKeywordObj);
                                //create targeting Object
                                keywordTargeting = new KeywordTargeting();
                                keywordTargeting.Type = targetingTypeRepository.Get(keywordTargetingTypeId);
                                keywordTargeting.Keyword = newKeywordObj;

                            }*/

                        }
                    }
                    if (targetingSaveDto.DeletedKeywords != null)
                    {
                        foreach (var deletedKeyword in targetingSaveDto.DeletedKeywords)
                        {
                            var deletedKeywordTargeting =
                                keywordsTargetings.FirstOrDefault(
                                    targeting => targeting.Keyword.ID == deletedKeyword);
                            if (deletedKeywordTargeting != null)
                            {
                                item.RemoveGroupTargeting(adGroupObj, deletedKeywordTargeting);
                            }
                        }
                    }

                    //end old imp
                    if (targetingSaveDto.AllKeywords==null)
                    { targetingSaveDto.AllKeywords = new List<int>(); }
                    if (keywordsTargetings != null)
                    {
                        foreach (var Keyword in keywordsTargetings)
                        {
                            
                            
                            
                            if (!targetingSaveDto.AllKeywords.Contains(Keyword.Keyword.ID))
                            {
                                var deletedKeywordTargeting =
                               keywordsTargetings.FirstOrDefault(
                                   targeting =>   targeting.Keyword.ID == Keyword.Keyword.ID);
                               if(deletedKeywordTargeting!=null)
                                item.RemoveGroupTargeting(adGroupObj, deletedKeywordTargeting);
                            }
                        }
                    }
                    keywordsTargetings = (adGroupObj.Targetings.ToList().OfType<KeywordTargeting>()).ToList();
                    foreach (var Keyword in targetingSaveDto.AllKeywords)
                    {


                        KeywordTargeting keywordTargeting = null;
                        int id;
                        Keyword newKeywordObj = null;
                       
                            keywordTargeting = keywordsTargetings.FirstOrDefault(targeting => targeting.Keyword.ID == Keyword);
                            if (keywordTargeting == null)
                            {

                                newKeywordObj = keyWordRepository.Get(Keyword);
                                newKeywordObj.Usage++;
                                //create targeting Object
                                keywordTargeting = new KeywordTargeting();
                                keywordTargeting.Type = targetingTypeRepository.Get(keywordTargetingTypeId);
                                keywordTargeting.Keyword = newKeywordObj;
                                keywordTargeting.Include = targetingSaveDto.AllowInclude;
                            }

                            if (keywordTargeting != null)
                            {
                                item.AddGroupTargeting(adGroupObj, keywordTargeting);
                            }
                        

                       
                    }

                    if (targetingSaveDto.ExcludeSensitiveCategories)
                    {
                        var keywordTargeting = keywordsTargetings.FirstOrDefault(targeting => targeting.Keyword.Code == "sensit");
                        if (keywordTargeting == null)
                        {

                            var newKeywordObj = keyWordRepository.Query(M => M.Code == "sensit").SingleOrDefault();
                            newKeywordObj.Usage++;
                            //create targeting Object
                            keywordTargeting = new KeywordTargeting();
                            keywordTargeting.Type = targetingTypeRepository.Get(keywordTargetingTypeId);
                            keywordTargeting.Keyword = newKeywordObj;
                            keywordTargeting.Include = false;
                        }

                        if (keywordTargeting != null)
                        {
                            item.AddGroupTargeting(adGroupObj, keywordTargeting);
                        }

                    }
                    else
                    {
                        var deletedKeywordTargeting =
                               keywordsTargetings.FirstOrDefault(
                                   targeting => targeting.Keyword.Code == "sensit");
                        if (deletedKeywordTargeting != null)
                        {
                            item.RemoveGroupTargeting(adGroupObj, deletedKeywordTargeting);
                        }


                    }

                   // var keywordsAllTargetings = (adGroupObj.Targetings.ToList().OfType<KeywordTargeting>()).ToList();

                   
                        #endregion

                        #region Demographic
                        //load Demographic targeting
                        {
                        var demographicTargting = adGroupObj.Targetings.ToList().OfType<DemographicTargeting>().FirstOrDefault();
                        if (targetingSaveDto.AgeGroupId > 0 || targetingSaveDto.Gender > 0)
                        {
                            if (demographicTargting == null)
                            {
                                //create new 
                                demographicTargting = new DemographicTargeting
                                {
                                    Type = targetingTypeRepository.Get(demographicTargtingTypeId),
                                    Demographic = new Demographic()
                                };
                            }
                            demographicTargting.Demographic.AgeGroup = ageGroupRepository.Get(targetingSaveDto.AgeGroupId);
                            demographicTargting.Demographic.Gender = genderRepository.Get(targetingSaveDto.Gender);
                            item.AddGroupTargeting(adGroupObj, demographicTargting);
                        }
                        else
                        {
                            if (demographicTargting != null)
                            {
                                item.RemoveGroupTargeting(adGroupObj, demographicTargting);
                            }
                        }
                    }

                    #endregion

                    #region BidConfig

                    foreach (CampaignBidConfigDto campaignBidConfigDto in targetingSaveDto.InsertedBidConfigItems)
                    {
                        var campaignBidConfig = new AdGroupBidConfig() { AdGroup = adGroupObj, SubPublisherId = campaignBidConfigDto.SubPublisherId };
                        campaignBidConfig.AppSite = new Domain.Model.AppSite.AppSite { ID = campaignBidConfigDto.Appsite.ID }; /*appSiteRepository.Get(campaignBidConfigDto.Appsite.ID);*/
                        campaignBidConfig.AppSite.Name = appSiteRepository.GetObjectName(campaignBidConfigDto.Appsite.ID);
                        campaignBidConfig.AppSite.AppSiteServerSetting = appSiteRepository.getServerSetting(campaignBidConfigDto.Appsite.ID);
                        campaignBidConfig.Account = new Domain.Model.Account.Account { ID = appSiteRepository.getAccountId(campaignBidConfigDto.Appsite.ID) };
                        campaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfigDto.Bid);
                        campaignBidConfig.SubAppSite = !string.IsNullOrEmpty(campaignBidConfigDto.ID) ? new SubAppsite { ID = Convert.ToInt32(campaignBidConfigDto.ID) } : null;
                        adGroupObj.AddCampaignBidConfig(campaignBidConfig);
                    }

                    foreach (CampaignBidConfigDto dto in targetingSaveDto.UpdatedBidConfigItems)
                    {
                        var campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.ID.ToString() == dto.ID).FirstOrDefault();
                        if (campaignBidConfig == null)
                        {
                            continue;
                        }

                        campaignBidConfig.SetAdGroupBidConfigsBid(dto.Bid);



                    }

                    foreach (CampaignBidConfigDto campaignBidConfigDto in targetingSaveDto.UpdatedNotCompatableCampaignBidConfiges)
                    {
                        var campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.ID.ToString() == campaignBidConfigDto.ID).FirstOrDefault();
                        if (campaignBidConfig == null)
                        {
                            campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.AppSite.ID == campaignBidConfigDto.Appsite.ID && x.SubPublisherId == campaignBidConfigDto.SubPublisherId).FirstOrDefault();
                        }
                        if (campaignBidConfig == null)
                        {
                            var newCampaignBidConfig = new AdGroupBidConfig() { AdGroup = adGroupObj, SubPublisherId = campaignBidConfigDto.SubPublisherId };
                            newCampaignBidConfig.AppSite = appSiteRepository.Get(campaignBidConfigDto.Appsite.ID);
                            newCampaignBidConfig.Account = accountRepository.Get(campaignBidConfigDto.AccountId);
                            newCampaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfigDto.Bid);

                            adGroupObj.AddCampaignBidConfig(newCampaignBidConfig);
                        }
                        else
                        {
                            campaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfigDto.Bid);
                        }
                    }

                    if (targetingSaveDto.DeletedCampaignBidConfigs != null)
                    {
                        foreach (int deletedId in targetingSaveDto.DeletedCampaignBidConfigs)
                        {
                            if (deletedId != 0)
                            {
                                adGroupObj.DeleteCampaignBidConfig(deletedId);
                            }
                        }
                    }

                    //new Impl

                    foreach (CampaignBidConfigDto campaignBidConfigDto in targetingSaveDto.AllBidConfigItems)
                    {
                        var campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.ID.ToString() == campaignBidConfigDto.ID).FirstOrDefault();
                        if (campaignBidConfig == null)
                        {
                            campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.AppSite.ID == campaignBidConfigDto.AppSiteId&& x.SubPublisherId == campaignBidConfigDto.SubPublisherId).FirstOrDefault();
                        }
                        if (campaignBidConfig == null)
                        {
                            var newCampaignBidConfig = new AdGroupBidConfig() { AdGroup = adGroupObj, SubPublisherId = campaignBidConfigDto.SubPublisherId };
                            newCampaignBidConfig.AppSite = appSiteRepository.Get(campaignBidConfigDto.AppSiteId);
                            newCampaignBidConfig.Account = accountRepository.Get(campaignBidConfigDto.AccountId);
                            newCampaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfigDto.Bid);

                            adGroupObj.AddCampaignBidConfig(newCampaignBidConfig);
                        }
                        else
                        {
                            campaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfigDto.Bid);
                        }
                    }
                    var campaignBidConfigtotal = adGroupObj.GetCampaignBidConfigs();
                    foreach (var campaignBidConfigDto in campaignBidConfigtotal)
                    {
                        var campaignBidConfigI = targetingSaveDto.AllBidConfigItems.Where(x => x.ID == campaignBidConfigDto.ID.ToString()).FirstOrDefault();
                        if (campaignBidConfigI ==null && Convert.ToInt32(campaignBidConfigDto.ID) != 0)
                        {
                            adGroupObj.DeleteCampaignBidConfig(campaignBidConfigDto.ID);
                        }
                    }
                     






                    var response = CheckAppsitesCostModelCompatableWitCampaign(
                        new CheckAppsitesCostModelCompatableWitCampaignRequest
                        {
                            CampaignId = item.ID ,//targetingSaveDto.CampaignId,
                            Appsites = targetingSaveDto.InsertedBidConfigItems.Select(x => x.Appsite.ID).ToList(),
                            AdGroupId = targetingSaveDto.AdGroupId,
                            GroupCostModelWrapperID = targetingSaveDto.CostModelWrapper,
                            CheckExisting = isPricingModelChanged
                        });

                    if (response.NotCompatableCampaigns != null && response.NotCompatableCampaigns.Count > 0)
                    {
                        if (adGroupObj.GetCampaignBidConfigs() == null || adGroupObj.GetCampaignBidConfigs().Count() <= 0)
                        {
                            throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                        }

                        foreach (CampaignBidConfigDto bidConfig in response.NotCompatableCampaigns)
                        {

                            if (bidConfig.Bid > targetingSaveDto.Bid)
                            {
                                throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                            }

                            if (adGroupObj.GetCampaignBidConfigs().Where(x => x.AppSite.ID == bidConfig.Appsite.ID && x.SubPublisherId == bidConfig.SubPublisherId && x.Bid > 0).Count() <= 0)
                            {
                                throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                            }
                        }
                    }


                    ////if the cost model has changed , then every config connected with an app site which is on default price model must redivided with the new cost model factor.
                    //if (isPricingModelChanged)
                    //{
                    //    var campaignBidConfigs = adGroupObj.GetCampaignBidConfigs().ToList();
                    //    if (campaignBidConfigs.Count > 0)
                    //    {
                    //        foreach (var campaignBidConfig in campaignBidConfigs)
                    //        {
                    //            // if it's appsite is on default cost model
                    //            if (campaignBidConfig.AppSite.AppSiteServerSetting.IsUsingCampaignPricingModel)
                    //            {

                    //                var updatedcampaignBidConfig = targetingSaveDto.UpdatedNotCompatableCampaignBidConfiges.Where(x => x.ID == campaignBidConfig.ID.ToString()).FirstOrDefault();

                    //                if (updatedcampaignBidConfig == null)
                    //                    // get the bid as readable value by multiplying it with the old factor
                    //                    campaignBidConfig.Bid *= costModelWrapperRepository.Get((int)oldPricingModel).Factor;
                    //                else
                    //                    campaignBidConfig.Bid = updatedcampaignBidConfig.Bid;

                    //                // divide it again with the new factor
                    //                campaignBidConfig.SetAdGroupBidConfigsBid(campaignBidConfig.Bid);
                    //            }
                    //        }
                    //    }

                    //}




                    #endregion


                    #region InventorySource
                    if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.InventorySource))
                    {




                        if (targetingSaveDto.Runtype == "false")
                        {
                            if (adGroupObj.RunAllExchanges)
                            {
                                adGroupObj.RunAllExchanges = false;
                            }
                            //old imp
                            foreach (InventorySourceDto InventorSourceDto in targetingSaveDto.InsertedInventorySourceItems)
                            {
                                var InventorySourceObj = new AdGroupInventorySource() { AdGroup = adGroupObj, Campaign = item };

                                BusinessPartner partner = _BusinessPartnerRepository.Get(InventorSourceDto.SSPId);
                                if (InventorSourceDto.Appsite != null)
                                {
                                    InventorySourceObj.AppSite = new Domain.Model.AppSite.AppSite { ID = InventorSourceDto.Appsite.ID };

                                    InventorySourceObj.AppSite.Name = appSiteRepository.GetObjectName(InventorSourceDto.Appsite.ID);
                                }
                                else
                                {


                                    InventorySourceObj.AppSite = new Domain.Model.AppSite.AppSite { ID = partner.AppSite.ID };

                                    InventorySourceObj.AppSite.Name = appSiteRepository.GetObjectName(partner.AppSite.ID);
                                }
                                if (InventorSourceDto.SubAppSiteId > 0)
                                    InventorySourceObj.SubAppsite = new Domain.Model.AppSite.SubAppsite { ID = InventorSourceDto.SubAppSiteId };
                                InventorySourceObj.Partner = new SSPPartner { ID = InventorSourceDto.SSPId };
                                InventorySourceObj.Partner.Name = partner.Name;
                                InventorySourceObj.Include = InventorSourceDto.Include;
                                InventorySourceObj.SubPublisherId = InventorSourceDto.subPublisherId;
                                InventorySourceObj.Campaign = item;
                                InventorySourceObj.Account = new Domain.Model.Account.Account { ID = _BusinessPartnerRepository.getAccount(InventorSourceDto.SSPId) };
                                adGroupObj.AddInventorySource(InventorySourceObj);
                            }
                            //old imp
                            foreach (InventorySourceDto dto in targetingSaveDto.UpdatedInventorySourceItems)
                            {
                                var InventorySourceobj = adGroupObj.GetAdGroupInventorySources().Where(x => x.ID == dto.ID).FirstOrDefault();
                                if (InventorySourceobj == null)
                                {
                                    continue;
                                }


                                InventorySourceobj.Include = dto.Include;


                            }


                            //old imp
                            if (targetingSaveDto.DeletedInventoryItemsSources != null)
                            {
                                foreach (int deletedId in targetingSaveDto.DeletedInventoryItemsSources)
                                {
                                    if (deletedId != 0)
                                    {
                                        adGroupObj.DeleteInventorySource(deletedId);
                                    }
                                }
                            }

                            //end old imp
                            foreach (int idSubappSite in targetingSaveDto.SelectedInventory)
                            {
                                var InventorySourceObj = new AdGroupInventorySource() { AdGroup = adGroupObj, Campaign = item };
                             var subappSite=   _subAppsiteRepository.Get(idSubappSite);
                              var appSiteAccountId = appSiteRepository.GetAccountId(subappSite.AppSite.ID);
                           
                                BusinessPartner partner = _SSPPartnerRepository.Query(M => M.IsDeleted==false  && M.AppSite.ID == subappSite.AppSite.ID && M.Account.ID == appSiteAccountId).SingleOrDefault();
                                //BusinessPartner partner = _BusinessPartnerRepository.Get(InventorSourceDto.SSPId);
                                if (subappSite.AppSite != null)
                                {
                                    InventorySourceObj.AppSite = new Domain.Model.AppSite.AppSite { ID = subappSite.AppSite.ID };

                                    InventorySourceObj.AppSite.Name = appSiteRepository.GetObjectName(subappSite.AppSite.ID);
                                }
                                else
                                {


                                    InventorySourceObj.AppSite = new Domain.Model.AppSite.AppSite { ID = subappSite.AppSite.ID };

                                    InventorySourceObj.AppSite.Name = appSiteRepository.GetObjectName(subappSite.AppSite.ID);
                                }
                                //if (InventorSourceDto.SubAppSiteId > 0)
                                    InventorySourceObj.SubAppsite = new Domain.Model.AppSite.SubAppsite { ID = idSubappSite };
                                InventorySourceObj.Partner = new SSPPartner { ID = partner.ID };
                                InventorySourceObj.Partner.Name = partner.Name;
                                InventorySourceObj.Include = targetingSaveDto.IncludedInventory.Contains(idSubappSite) ;
                                InventorySourceObj.SubPublisherId = subappSite.SubPublisherId;
                                InventorySourceObj.Campaign = item;
                                InventorySourceObj.Account = new Domain.Model.Account.Account { ID = _BusinessPartnerRepository.getAccount(partner.ID) };
                                adGroupObj.AddUpdateInventorySourceForSubSites(InventorySourceObj);
                            }

                            var invetorySourcesExist = adGroupObj.GetAdGroupInventorySources();
                            for (int ci = 0; ci < invetorySourcesExist.Count; ci++)
                            {
                                var itemInventory = invetorySourcesExist[ci];

                                if (itemInventory.SubAppsite!=null && ! targetingSaveDto.SelectedInventory.Contains(itemInventory.SubAppsite.ID))
                                {
                                    adGroupObj.DeleteInventorySourceForSubSites(itemInventory.SubAppsite.ID);
                                }

                            }

                            invetorySourcesExist = adGroupObj.GetAdGroupInventorySources();
                            for (int ci = 0; ci < invetorySourcesExist.Count; ci++)
                            {
                                var itemInventory = invetorySourcesExist[ci];


                                if ((targetingSaveDto.CheckedSSP.Where(M => M == itemInventory.Partner.ID).FirstOrDefault() == 0))
                                {

                                    adGroupObj.DeleteInventorySource(itemInventory.ID);
                                    // ci--;

                                }
                            }

                            /* invetorySourcesExist = adGroupObj.GetAdGroupInventorySources();
                             if (invetorySourcesExist != null)
                             {
                                 for (int ci = 0; ci < invetorySourcesExist.Count; ci++)
                                 {
                                     var itemInventory = invetorySourcesExist[ci];


                                     if ((targetingSaveDto.CheckedSSP.Where(M => M == itemInventory.Partner.ID).FirstOrDefault() == itemInventory.Partner.ID) && itemInventory.SubAppsite == null)
                                     {

                                         adGroupObj.DeleteInventorySource(itemInventory.ID);
                                         ci--;
                                     }
                                 }
                             }*/


                            invetorySourcesExist = adGroupObj.GetAdGroupInventorySources();
                            if (targetingSaveDto.CheckedSSP != null && targetingSaveDto.CheckedSSP.Count > 0)
                            {
                                foreach (var SSPid in targetingSaveDto.CheckedSSP)
                                {

                                    var itemInv = adGroupObj.GetAdGroupInventorySources().Where(M => M.Partner.ID == SSPid && M.SubAppsite == null).FirstOrDefault();
                                    if (itemInv == null)
                                    {
                                        var existSubSite = adGroupObj.GetAdGroupInventorySources().Where(M => M.Partner.ID == SSPid && M.SubAppsite != null).FirstOrDefault();
                                        if (existSubSite == null)
                                        {
                                            var newInv = new AdGroupInventorySource
                                            {
                                                Partner = new SSPPartner { ID = SSPid },
                                                Include = true
                                            };

                                            BusinessPartner partnerobj = _BusinessPartnerRepository.Get(SSPid);

                                            newInv.AppSite = new Domain.Model.AppSite.AppSite { ID = partnerobj.AppSite.ID };

                                            newInv.AppSite.Name = appSiteRepository.GetObjectName(partnerobj.AppSite.ID);
                                            adGroupObj.AddInventorySource(newInv);

                                            newInv.Partner.Name = partnerobj.Name;
                                        }
                                    }
                                    else
                                    {
                                        var existSubSite = adGroupObj.GetAdGroupInventorySources().Where(M => M.Partner.ID == SSPid && M.SubAppsite != null).FirstOrDefault();
                                        if (existSubSite != null)
                                            adGroupObj.DeleteInventorySource(itemInv.ID);


                                    }
                                }


                            }
                            else
                            {

                                if (invetorySourcesExist != null)
                                {
                                    for (int ci = 0; ci < invetorySourcesExist.Count; ci++)
                                    {
                                        var itemInventory = invetorySourcesExist[ci];




                                        adGroupObj.DeleteInventorySource(itemInventory.ID);

                                    }
                                }


                            }

                            /*IList<CampaignBidConfigDto> notCompatabileAppsitesInventorySource = null;

                            bool isValidInv = CheckAppsitesCostModelCompatableWitCampaign(targetingSaveDto.CampaignId, out notCompatabileAppsitesInventorySource, targetingSaveDto.InsertedInventorySourceItems.Select(x => x.Appsite.ID).ToList(), targetingSaveDto.AdGroupId, targetingSaveDto.CostModelWrapper, isPricingModelChanged);

                            if (notCompatabileAppsitesInventorySource != null && notCompatabileAppsitesInventorySource.Count > 0)
                            {
                                if (adGroupObj.GetAdGroupInventorySources() == null || adGroupObj.GetAdGroupInventorySources().Count() <= 0)
                                {
                                    throw new BusinessException(new List<ErrorData> { new ErrorData("SSPPartnerBidConfigsNotValid") });
                                }

                                foreach (CampaignBidConfigDto bidConfig in notCompatabileAppsitesInventorySource)
                                {


                                    if (adGroupObj.GetAdGroupInventorySources().Where(x => (x.AppSite!=null  &&  x.AppSite.ID == bidConfig.Appsite.ID ) ).Count() >0)
                                    {
                                        throw new BusinessException(new List<ErrorData> { new ErrorData("SSPPartnerBidConfigsNotValid") });
                                    }
                                }
                            }*/

                            if (isGeoFencingTargeting)
                            {
                                var largestRadious = 0;
                                var InvetrorySourceList = adGroupObj.GetAdGroupInventorySources();
                                if (InvetrorySourceList != null && InvetrorySourceList.Count > 0)
                                {
                                    var parnters = InvetrorySourceList.Select(M => M.Partner.ID).ToList().Distinct();
                                    var allowGeofenceAboveRadius = true;
                                    foreach (var partner in parnters)
                                    {
                                        var partnerObj = _SSPPartnerRepository.Get(partner);
                                        if (partnerObj.DisallowGeofenceLessThanRadius)
                                        {
                                            allowGeofenceAboveRadius = false;
                                            if (partnerObj.GeofenceRadius > largestRadious)
                                            {
                                                largestRadious = partnerObj.GeofenceRadius;

                                            }

                                        }
                                        else

                                        {
                                            allowGeofenceAboveRadius = true;
                                            break;

                                        }

                                    }

                                    if (!allowGeofenceAboveRadius)
                                    {
                                        //for (int ci = 0; ci < InvetrorySourceList.Count; ci++)
                                        //{
                                        //    var itemInventory = InvetrorySourceList[ci];




                                        //    adGroupObj.DeleteInventorySource(itemInventory.ID);

                                        //}
                                        //dtoResult.InventroySourceAllowGeofencing = true;

                                        var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > largestRadious).ToList();
                                        if (results != null && results.Count > 0)
                                        {
                                            var error = new BusinessException();
                                            error.Errors.Add(new ErrorData { ID = "InventroySourceAllowGeofencing" });
                                            throw error;
                                        }
                                    }
                                }
                            }


                        }
                        else


                        {
                            if (adGroupObj.GetAdGroupInventorySources() != null && adGroupObj.GetAdGroupInventorySources().Count > 0)
                            {
                                var listIds = adGroupObj.GetAdGroupInventorySources().Select(M => M.ID).ToList();
                                foreach (var idItem in listIds)
                                {
                                    adGroupObj.DeleteInventorySource(idItem);
                                }
                            }
                            adGroupObj.RunAllExchanges = true;


                            if (item.Account.AccountRole == AccountRole.DSP)
                            {
                                var sspPartnersAll = _SSPPartnerRepository.Query(M => M.IsDeleted == false && M.Visible == true).ToList();

                                var sspPartners = sspPartnersAll.ToList();
                                if (sspPartners != null && sspPartners.Count > 0)
                                {
                                    foreach (var SSPid in sspPartners)
                                    {
                                        var itemInv = adGroupObj.GetAdGroupInventorySources().Where(M => M.Partner.ID == SSPid.ID && M.SubAppsite == null).FirstOrDefault();
                                        if (itemInv == null)
                                        {
                                            var newInv = new AdGroupInventorySource
                                            {
                                                Partner = new SSPPartner { ID = SSPid.ID },
                                                Include = true
                                            };

                                            BusinessPartner partnerobj = _BusinessPartnerRepository.Get(SSPid.ID);

                                            newInv.AppSite = new Domain.Model.AppSite.AppSite { ID = partnerobj.AppSite.ID };

                                            newInv.AppSite.Name = appSiteRepository.GetObjectName(partnerobj.AppSite.ID);
                                            adGroupObj.AddInventorySource(newInv);

                                            newInv.Partner.Name = partnerobj.Name;
                                        }
                                    }
                                }
                                //var invetorySourcesExist = adGroupObj.GetAdGroupInventorySources();



                                //for (int ci = 0; ci < invetorySourcesExist.Count; ci++)
                                //{
                                //    var itemInventory = invetorySourcesExist[ci];



                                //    adGroupObj.DeleteInventorySource(itemInventory.ID);


                                //}

                            }
                        }



                    }
                    #endregion

                    #region PMPDeal
                    if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.PMPDeal))
                    {

                        string[] typesList = null;
                        List<int> typesListToSend = null;



                        //old imp
                        if (!string.IsNullOrEmpty(targetingSaveDto.DeletedPMPDealConfigs))
                            typesList = targetingSaveDto.DeletedPMPDealConfigs.Split(',');

                        if (typesList != null && typesList.Count() > 0)
                            typesListToSend = typesList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                        if (typesListToSend != null)
                        {




                            foreach (var currentPMPDeal in adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>())
                            {
                                //check if the targeting is found on the List that we get form the user
                                //if not then  remove it
                                var found = false;
                                foreach (var itemdeal in typesListToSend)
                                {
                                    if (currentPMPDeal.Deal.ID == itemdeal)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (found == true)
                                {
                                    //we should remove the targeting
                                    item.RemoveGroupTargeting(adGroupObj, currentPMPDeal);
                                }
                            }
                        }

                        //End old imp

                        if (targetingSaveDto.SelectedDeals != null && targetingSaveDto.SelectedDeals.Count() > 0)
                            typesListToSend = targetingSaveDto.SelectedDeals.ToList();
                        if (typesListToSend==null)
                        { typesListToSend = new List<int>(); }
                        if (typesListToSend != null)
                        {



                           var pmpDeals= adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>();
                            foreach (var currentPMPDeal in pmpDeals)
                            {
                                //check if the targeting is found on the List that we get form the user
                                //if not then  remove it
                                var found = false;
                                //foreach (var itemdeal in typesListToSend)
                               // {
                                    if ( typesListToSend.Contains(currentPMPDeal.Deal.ID))
                                    {
                                        found = true;
                                        
                                    }
                               // }
                                if (found == false)
                                {
                                    //we should remove the targeting
                                    item.RemoveGroupTargeting(adGroupObj, currentPMPDeal);
                                }
                            }
                        }

                        typesList = null;
                        typesListToSend = null;

                        //Old Imp
                        if (!string.IsNullOrEmpty(targetingSaveDto.InsertePMPDealConfigs))
                            typesList = targetingSaveDto.InsertePMPDealConfigs.Split(',');

                        if (typesList != null && typesList.Count() > 0)
                            typesListToSend = typesList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();


                        if (typesListToSend != null)
                        {
                            foreach (int dealinsert in typesListToSend)
                            {

                                var targetingObj =
                                    adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>().FirstOrDefault(
                                        targeting => targeting.Deal.ID == dealinsert);

                                if (targetingObj == null)
                                {
                                    var AdPMPDealTargetingVa = new AdPMPDealTargeting
                                    {
                                        Type = targetingTypeRepository.Get(pmpTargtingTypeId),
                                        Deal = PMPDealRepository.Get(dealinsert) /* new Domain.Model.Account.PMP.PMPDeal() { ID = dealinsert }*/
                                    };
                                    item.AddGroupTargeting(adGroupObj, AdPMPDealTargetingVa);
                                }
                            }
                        }
                        //End Old Imp



                        if (targetingSaveDto.SelectedDeals != null && targetingSaveDto.SelectedDeals.Count() > 0)
                            typesListToSend = targetingSaveDto.SelectedDeals.ToList();


                        if (typesListToSend != null)
                        {
                            foreach (int dealinsert in typesListToSend)
                            {

                                var targetingObj =
                                    adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>().FirstOrDefault(
                                        targeting => targeting.Deal.ID == dealinsert);

                                if (targetingObj == null)
                                {
                                    var AdPMPDealTargetingVa = new AdPMPDealTargeting
                                    {
                                        Type = targetingTypeRepository.Get(pmpTargtingTypeId),
                                        Deal = PMPDealRepository.Get(dealinsert) /* new Domain.Model.Account.PMP.PMPDeal() { ID = dealinsert }*/
                                    };
                                    item.AddGroupTargeting(adGroupObj, AdPMPDealTargetingVa);
                                }
                            }
                        }
                        if (isGeoFencingTargeting)
                        {

                            var largestRadious = 0;
                            var DealsTargetingList = adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>().ToList();
                            if (DealsTargetingList != null && DealsTargetingList.Count > 0)
                            {
                                var parnters = DealsTargetingList.Select(M => M.Deal.Exchange.ID).ToList().Distinct();
                                var AllowGeofenceCampaignsDeal = true;
                                foreach (var partner in parnters)
                                {
                                    var partnerObj = _SSPPartnerRepository.Get(partner);
                                    if (partnerObj.DisallowGeofenceLessThanRadius)
                                    {
                                        AllowGeofenceCampaignsDeal = false;
                                        if (partnerObj.GeofenceRadius < largestRadious)
                                        {

                                            largestRadious = partnerObj.GeofenceRadius;
                                        }

                                        // break;
                                    }
                                    else

                                    {
                                        AllowGeofenceCampaignsDeal = true;
                                        break;

                                    }

                                }
                                if (!AllowGeofenceCampaignsDeal)
                                {
                                    //for (int ci = 0; ci < DealsTargetingList.Count; ci++)
                                    //{
                                    //    var itemDealTar = DealsTargetingList[ci];




                                    //    item.RemoveGroupTargeting(adGroupObj, itemDealTar);

                                    //}
                                    //dtoResult.DealAllowGeofencing = true;
                                    var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > largestRadious).ToList();
                                    if (results != null && results.Count > 0)
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "DealAllowGeofencingLess" });
                                        throw error;
                                    }
                                }

                            }
                        }

                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        var foundConflictGeograpic = false;
                        bool foundConflictGAdType = false;
                        var foundConflictPrice = false;
                        IList<int> locationCampain = new List<int>();
                        IList<int> adTypeCampain = new List<int>();

                        foreach (var currentGeographic in adGroupObj.Targetings.ToList().OfType<GeographicTargeting>())
                        {
                            locationCampain.Add(currentGeographic.Location.ID);
                        }
                        //var adGroupObjectiveType;

                        //AdTypeIds.NativeAd


                        var adgroupAdTypes = adGroupObj.Objective.AdAction.AdTypes;
                        foreach (var adgroupType in adgroupAdTypes)
                        {
                            if (!adTypeCampain.Contains((int)adgroupType.Group))
                            {

                                adTypeCampain.Add((int)adgroupType.Group);
                            }

                        }

                        if (adGroupObj.Objective.AdType != null)
                        {

                            if (adGroupObj.Objective.AdType.ID == (int)AdTypeIds.NativeAd)
                            {
                                if (!adTypeCampain.Contains((int)AdTypeGroup.Native))
                                {

                                    adTypeCampain.Add((int)AdTypeGroup.Native);
                                }

                            }

                        }
                        else
                        {

                            if (adTypeCampain.Contains((int)AdTypeGroup.Native))
                            {

                                adTypeCampain.Remove((int)AdTypeGroup.Native);
                            }


                        }
                        foreach (var pmptargeting in adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>())
                        {

                            var dealegraphicTargetings = pmptargeting.Deal.Targetings.ToList().OfType<GeographicPMPDealTargeting>();
                            var adTypeTargetings = pmptargeting.Deal.Targetings.ToList().OfType<AdTypeGroupPMPDealTargeting>();
                            var dealPrice = pmptargeting.Deal.Price;


                            IList<int> locationdeal = new List<int>();
                            IList<int> locationintersect = new List<int>();
                            IList<int> AdTypedealintersect = new List<int>();
                            IList<int> AdTypedeal = new List<int>();

                            foreach (var currentdealGeographic in dealegraphicTargetings)
                            {
                                locationdeal.Add(currentdealGeographic.Location.ID);


                            }

                            foreach (var currentdealAdType in adTypeTargetings)
                            {
                                AdTypedeal.Add((int)currentdealAdType.AdTypeGroup);

                            }


                            if (dealPrice > targetingSaveDto.Bid)
                            { foundConflictPrice = true; }
                            if (AdTypedeal.Count > 0)
                            {
                                if (adTypeCampain.Count > AdTypedeal.Count)
                                {
                                    AdTypedealintersect = adTypeCampain.Intersect(AdTypedeal).ToList();

                                    if (AdTypedealintersect.Count < AdTypedeal.Count)
                                    {
                                        foundConflictGAdType = true;


                                    }
                                }
                                else
                                {
                                    AdTypedealintersect = AdTypedeal.Intersect(adTypeCampain).ToList();

                                    if (AdTypedealintersect.Count < adTypeCampain.Count)
                                    {

                                        foundConflictGAdType = true;

                                        //break;
                                    }
                                    else if (adTypeCampain.Count == 0)
                                    {
                                        foundConflictGAdType = true;

                                    }
                                }
                            }
                            //else
                            //{
                            //    foundConflictGAdType = true;

                            //}

                            if (locationdeal.Count > 0)
                            {
                                if (locationCampain.Count > locationdeal.Count)
                                {
                                    locationintersect = locationCampain.Intersect(locationdeal).ToList();
                                    if (locationintersect.Count < locationdeal.Count)
                                    {
                                        foundConflictGeograpic = true;
                                        //break;

                                    }
                                }
                                else
                                {
                                    locationintersect = locationdeal.Intersect(locationCampain).ToList();

                                    if (locationintersect.Count < locationCampain.Count)
                                    {

                                        foundConflictGeograpic = true;
                                        //break;
                                    }
                                    else if (locationCampain.Count == 0)
                                    {
                                        foundConflictGeograpic = true;
                                    }
                                }
                            }
                            //else
                            //{
                            //    if (locationCampain.Count > 0)
                            //    {
                            //        foundConflictGeograpic = true;
                            //    }

                            //}


                        }
                        if (foundConflictGeograpic == true)
                        {
                            //we should remove the targeting
                            dtoResult.PMPDealConfictCountries = true;
                        }
                        if (foundConflictPrice == true)
                        {
                            //we should remove the targeting
                            dtoResult.PMPDealConfictPrice = true;
                        }


                        if (foundConflictGAdType == true)
                        {
                            //we should remove the targeting
                            dtoResult.PMPDealConfictAdType = true;


                        }

                        //  }
                        /*var targetingObjPMPDeals =
                                      adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>();
                        if (targetingObjPMPDeals != null)
                        {
                            var DealsSSP = targetingObjPMPDeals.Select(M => M.Deal.Exchange.ID).ToList();
                            if ((targetingSaveDto.CheckedSSP != null && targetingSaveDto.CheckedSSP.Count > 0) && (DealsSSP != null && DealsSSP.Count > 0))
                            {
                                var intersecresult = targetingSaveDto.CheckedSSP.Intersect(DealsSSP);
                                if (intersecresult != null && intersecresult.ToList().Count > 0)
                                {
                                    dtoResult.PMPDealConfictWithInventorySource = true;
                                }
                            }
                        }*/


                        var DealsTargetingobs =
                                      adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>().ToList();
                        var InventorySourceobjs = adGroupObj.GetAdGroupInventorySources();
                        if (targetingSaveDto.Runtype == "false" && DealsTargetingobs != null && DealsTargetingobs.Count > 0)
                        {


                            if (InventorySourceobjs != null && InventorySourceobjs.Count > 0)
                            {

                                foreach (var itemdeal in DealsTargetingobs)
                                {
                                    var dealFound = InventorySourceobjs.Where(M => M.Partner.ID == itemdeal.Deal.Exchange.ID).FirstOrDefault();
                                    if (dealFound == null)
                                    {
                                        dtoResult.PMPDealInventorySourceConflicts = true;
                                    }

                                }
                            }
                        }
                    }

                    #endregion


                    #region MasterList
                    //if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.PMPDeal))

                    //old imp
                    string[] typesMasterList = null;
                    List<int> typesMasterListToSend = null;




                    if (!string.IsNullOrEmpty(targetingSaveDto.DeletedMasterListConfigs))
                        typesMasterList = targetingSaveDto.DeletedMasterListConfigs.Split(',');

                    if (typesMasterList != null && typesMasterList.Count() > 0)
                        typesMasterListToSend = typesMasterList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                    if (typesMasterListToSend != null)
                    {




                        foreach (var currentMasterAppSite in adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>())
                        {
                            //check if the targeting is found on the List that we get form the user
                            //if not then  remove it
                            var found = false;
                            foreach (var itemList in typesMasterListToSend)
                            {
                                if (currentMasterAppSite.List.ID == itemList)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found == true)
                            {
                                //we should remove the targeting
                                item.RemoveGroupTargeting(adGroupObj, currentMasterAppSite);
                            }
                        }
                    }


                    typesMasterList = null;
                    typesMasterListToSend = null;
                    if (!string.IsNullOrEmpty(targetingSaveDto.InserteMasterListConfigs))
                        typesMasterList = targetingSaveDto.InserteMasterListConfigs.Split(',');

                    if (typesMasterList != null && typesMasterList.Count() > 0)
                        typesMasterListToSend = typesMasterList.Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();


                    if (typesMasterListToSend != null)
                    {
                        foreach (int listinsert in typesMasterListToSend)
                        {

                            var targetingObj =
                                adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>().FirstOrDefault(
                                    targeting => targeting.List.ID == listinsert);

                            if (targetingObj == null)
                            {
                                var MasterAppSiteTargetingVa = new MasterAppSiteTargeting
                                {
                                    Type = targetingTypeRepository.Get(MasterAppsiteTargtingTypeId),
                                    List = _AdvertiserAccountMasterAppSiteRepository.Get(listinsert) /* new Domain.Model.Account.PMP.PMPDeal() { ID = dealinsert }*/
                                };
                                item.AddGroupTargeting(adGroupObj, MasterAppSiteTargetingVa);
                            }
                        }
                    }

                    //end old imp

                    if (targetingSaveDto.SelectedMasterListConfigs ==null)
                    { targetingSaveDto.SelectedMasterListConfigs = new List<int>(); }


                   var MasterListOfTargeting= adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>();
                    foreach (var currentMasterAppSite in MasterListOfTargeting)
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it
                        
                        if (!targetingSaveDto.SelectedMasterListConfigs.Contains(currentMasterAppSite.List.ID))
                        {
                            //we should remove the targeting

                            var deletedTargeting =
                              MasterListOfTargeting.FirstOrDefault(
                                  targeting => targeting.List.ID == currentMasterAppSite.List.ID);
                            if (deletedTargeting != null)
                                item.RemoveGroupTargeting(adGroupObj, deletedTargeting);
                        }
                    }

                    MasterListOfTargeting = adGroupObj.Targetings.ToList().OfType<MasterAppSiteTargeting>();
                    foreach (var currentMasterAppSite in targetingSaveDto.SelectedMasterListConfigs)
                    {
                        //check if the targeting is found on the List that we get form the user
                        //if not then  remove it

                        if (MasterListOfTargeting.Where(M=>M.List.ID== currentMasterAppSite).FirstOrDefault()==null)
                        {
                            //we should remove the targeting
                           //item.RemoveGroupTargeting(adGroupObj, currentMasterAppSite);

                            var MasterAppSiteTargetingVa = new MasterAppSiteTargeting
                            {
                                Type = targetingTypeRepository.Get(MasterAppsiteTargtingTypeId),
                                List =  new AdvertiserAccountMasterAppSite {ID= currentMasterAppSite }
                                //_AdvertiserAccountMasterAppSiteRepository.Get(currentMasterAppSite) /* new Domain.Model.Account.PMP.PMPDeal() { ID = dealinsert }*/
                            };
                            item.AddGroupTargeting(adGroupObj, MasterAppSiteTargetingVa);
                        }
                    
                }

                #endregion
                #region Audiances
                if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.Audience))
                    //{
                    {
                        var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == false).FirstOrDefault();

                        var AudianceSegmentCostModelTypeAllowed = configurationManager.GetConfigurationSetting(null, null, "AudianceSegmentCostModelTypeAllowed");
                        IList<int> costmodelvalues = AudianceSegmentCostModelTypeAllowed.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        if (costmodelvalues.Contains(targetingSaveDto.CostModelWrapper))
                        {

                            if (audienceSegmentTargeting == null)
                            {

                                //create new 
                                audienceSegmentTargeting = new AudienceSegmentTargeting
                                {
                                    Type = targetingTypeRepository.Get(AudianceTargtingTypeId)

                                };
                                string json = string.Empty;
                                if (!string.IsNullOrEmpty(targetingSaveDto.groupAudianceString))
                                {
                                    //  json = new JavaScriptSerializer().Serialize(targetingSaveDto.group);

                                    if (targetingSaveDto.changedAudiances == "true")
                                        audienceSegmentTargeting.RulesJson = audienceSegmentTargeting.GetRulesJsonForExpression(targetingSaveDto.groupAudianceString);
                                    if (!string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
                                    item.AddGroupTargeting(adGroupObj, audienceSegmentTargeting);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(targetingSaveDto.groupAudianceString))
                                {
                                    //string json = new JavaScriptSerializer().Serialize(targetingSaveDto.group);

                                    if (targetingSaveDto.changedAudiances == "true")
                                    {
                                        int lastVersion = audienceSegmentTargeting.GetLastVersionForRuleJson(audienceSegmentTargeting.RulesJson);
                                        audienceSegmentTargeting.RulesJson = audienceSegmentTargeting.GetRulesJsonForExpression(targetingSaveDto.groupAudianceString, lastVersion);
                                        if (string.IsNullOrWhiteSpace(audienceSegmentTargeting.RulesJson))
                                        {
                                            item.RemoveGroupTargeting(adGroupObj, audienceSegmentTargeting);
                                        }
                                    }
                                }
                                else

                                {

                                    audienceSegmentTargeting.RulesJson = string.Empty;

                                    item.RemoveGroupTargeting(adGroupObj, audienceSegmentTargeting);
                                }
                            }


                            var audienceSegmentTargetingF = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == false).FirstOrDefault();
                            // }

                            if (audienceSegmentTargetingF != null)
                            {
                                if (item.Advertiser != null)
                                {
                                    var resultsTargetingAudic = audienceSegmentTargetingF.CheckIfRulesHaveAdvertiserBlocker(item.Advertiser.ID, targetingSaveDto.groupAudianceString);
                                    if (resultsTargetingAudic)
                                    {
                                        throw new BusinessException(new List<ErrorData> { new ErrorData("audienceSegmentTargetingNotValid") });
                                    }

                                    if (targetingSaveDto.changedAudiances == "true")
                                    {
                                        var changedCostElemtn = audienceSegmentTargetingF.CheckIfDataProviderCostElementAdded(adGroupObj, targetingSaveDto.groupAudianceString);
                                        if (!dtoResult.AddDefaultCostElement)
                                        {
                                            dtoResult.AddDefaultCostElement = changedCostElemtn;
                                        }

                                        var changedFeen = audienceSegmentTargetingF.CheckIfDataProviderFeeAdded(adGroupObj, targetingSaveDto.groupAudianceString);
                                        if (!dtoResult.AddDefaultFee)
                                        {
                                            dtoResult.AddDefaultFee = changedFeen;
                                        }
                                    }
                                    if (audienceSegmentTargetingF.LogAdMarkup)
                                        adGroupObj.LogAdMarkup = audienceSegmentTargetingF.LogAdMarkup;
                                }
                            }
                        }
                        else
                        {
                            if (audienceSegmentTargeting != null)
                            {
                                audienceSegmentTargeting.RulesJson = string.Empty;
                                item.RemoveGroupTargeting(adGroupObj, audienceSegmentTargeting);
                            }
                        }
                    }


                    #endregion

                    var audienceSegmentTargetingS = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>();
                    adGroupObj.DataBid = null;
                    adGroupObj.MaxDataBid = null;
                    if (audienceSegmentTargetingS != null)
                    {
                        foreach (var audienceSegmentTargeting in audienceSegmentTargetingS)
                        {

                            if (audienceSegmentTargeting is AudienceSegmentTargeting)
                            {
                                adGroupObj.SetAdsMaxDataBid(audienceSegmentTargeting as AudienceSegmentTargeting);
                                if (adGroupObj.DataBid.HasValue)
                                    adGroupObj.DataBid = adGroupObj.DataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.Value : 0);
                                else
                                    adGroupObj.DataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid;
                                if (adGroupObj.MaxDataBid.HasValue)
                                    adGroupObj.MaxDataBid = adGroupObj.MaxDataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.Value : 0);
                                else
                                    adGroupObj.MaxDataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid;


                                if (audienceSegmentTargeting.IsExternal)
                                {
                                    var changedCostElemtn = audienceSegmentTargeting.CheckIfDataProviderCostElementAdded(adGroupObj, audienceSegmentTargeting.GetRulesJsonForGroup());
                                    if (!dtoResult.AddDefaultCostElement)
                                    {
                                        dtoResult.AddDefaultCostElement = changedCostElemtn;
                                    }
                                    var changedFeen = audienceSegmentTargeting.CheckIfDataProviderFeeAdded(adGroupObj, audienceSegmentTargeting.GetRulesJsonForGroup());
                                    if (!dtoResult.AddDefaultFee)
                                    {
                                        dtoResult.AddDefaultFee = changedFeen;
                                    }
                                }
                                if (audienceSegmentTargeting.LogAdMarkup)
                                    adGroupObj.LogAdMarkup = audienceSegmentTargeting.LogAdMarkup;
                            }
                        }
                    }

                    #region Contextual
                    if (targetingSaveDto.changedContextuals == "true")
                    {
                        var contextualSegmentTargetinglist = adGroupObj.Targetings.OfType<ContextualSegmentTargeting>().Where(x => x.IsBrandSafty == false).ToList();
                        var Type = targetingTypeRepository.Get(ContextualSegmentTargetingTypeId);

                        if (contextualSegmentTargetinglist.Count <= 0)
                        {
                            //create new 
                            var contextualSegmentTargeting = new ContextualSegmentTargeting();
                            contextualSegmentTargeting.SaveContextualSegmentTargeting(adGroupObj, targetingSaveDto.groupContextualString, Type);
                        }
                        else
                        {
                            foreach (var contextualSegmentTargetingItem in contextualSegmentTargetinglist)
                            {
                                item.RemoveGroupTargeting(adGroupObj, contextualSegmentTargetingItem);
                            }

                            if (!string.IsNullOrEmpty(targetingSaveDto.groupContextualString))
                            {
                                var contextualSegmentTargeting = new ContextualSegmentTargeting();
                                contextualSegmentTargeting.SaveContextualSegmentTargeting(adGroupObj, targetingSaveDto.groupContextualString, Type);
                            }
                        }
                    }

                    #endregion
                    #region Contextual BrandSafty
                    if (targetingSaveDto.changedBrandSafety == "true")
                    {
                        var contextualSegmentTargetinglist = adGroupObj.Targetings.OfType<ContextualSegmentTargeting>().Where(x => x.IsBrandSafty == true).ToList();
                        var Type = targetingTypeRepository.Get(ContextualSegmentTargetingTypeId);

                        if (contextualSegmentTargetinglist.Count <= 0)
                        {
                            //create new 
                            var contextualSegmentTargeting = new ContextualSegmentTargeting();
                            contextualSegmentTargeting.SaveContextualSegmentTargeting(adGroupObj, targetingSaveDto.groupBrandSafetyString, Type, true);
                        }
                        else
                        {
                            foreach (var contextualSegmentTargetingItem in contextualSegmentTargetinglist)
                            {
                                item.RemoveGroupTargeting(adGroupObj, contextualSegmentTargetingItem);
                            }

                            if (!string.IsNullOrEmpty(targetingSaveDto.groupBrandSafetyString))
                            {
                                var contextualSegmentTargeting = new ContextualSegmentTargeting();
                                contextualSegmentTargeting.SaveContextualSegmentTargeting(adGroupObj, targetingSaveDto.groupBrandSafetyString, Type, true);
                            }
                        }


                    }
                    
                    #endregion
                    #region Language Targeting

                    {
                        if (targetingSaveDto.LanguagesIds != null)
                        {
                            foreach (var language in targetingSaveDto.LanguagesIds)
                            {
                                var targetingObj =
                                    adGroupObj.Targetings.ToList().OfType<LanguageTargeting>().FirstOrDefault(
                                        targeting => targeting.Language.ID == language);
                                if (targetingObj == null)
                                {
                                    //if not found then add it
                                    // var locationItem = langu.Get(language);
                                    // if (locationItem != null)
                                    //{
                                    //create targeting Object
                                    targetingObj = new LanguageTargeting
                                    {
                                        Type = targetingTypeRepository.Get(LanguageTargtingTypeId),
                                        Language = new Language { ID = language }
                                    };

                                    //}
                                    item.AddGroupTargeting(adGroupObj, targetingObj);
                                }
                            }
                        }
                        //Load List after the Update
                        foreach (var currentLanguage in adGroupObj.Targetings.ToList().OfType<LanguageTargeting>())
                        {
                            //check if the targeting is found on the List that we get form the user
                            //if not then  remove it
                            var found = false;
                            if (targetingSaveDto.LanguagesIds != null)
                            {
                                foreach (var language in targetingSaveDto.LanguagesIds)
                                {
                                    if (currentLanguage.Language.ID == language)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                            }
                            if (found == false)
                            {
                                //we should remove the targeting
                                item.RemoveGroupTargeting(adGroupObj, currentLanguage);
                            }
                        }
                    }
                    #endregion
                    #region VideoTargeting




                    if (adGroupObj.Objective.AdAction != null && adGroupObj.Objective.AdAction.ID == (int)AdActionTypeIds.VideoStreaming)
                    {
                        var VideoTargeting =
                                             adGroupObj.Targetings.ToList().OfType<VideoTargeting>().FirstOrDefault();

                        if (VideoTargeting != null)
                        {



                            VideoTargeting.PlacementType_InStream = targetingSaveDto.PlacementType_InStream;
                            VideoTargeting.PlacementType_OutStream = targetingSaveDto.PlacementType_OutStream;
                            VideoTargeting.PlacementType_Interstitial = targetingSaveDto.PlacementType_Interstitial;
                            VideoTargeting.PlacementType_Undetermined = targetingSaveDto.PlacementType_Undetermined;
                            VideoTargeting.InStreamPosition_PreRoll = targetingSaveDto.InStreamPosition_PreRoll;
                            VideoTargeting.InStreamPosition_MidRoll = targetingSaveDto.InStreamPosition_MidRoll;
                            VideoTargeting.InStreamPosition_PostRoll = targetingSaveDto.InStreamPosition_PostRoll;
                            VideoTargeting.InStreamPosition_Undetermined = targetingSaveDto.InStreamPosition_Undetermined;
                            VideoTargeting.SkippableAds_SkippableAdSpaces = targetingSaveDto.SkippableAds_SkippableAdSpaces;
                            VideoTargeting.SkippableAds_NonSkippableAdSpaces = targetingSaveDto.SkippableAds_NonSkippableAdSpaces;
                            VideoTargeting.SkippableAds_Undetermined = targetingSaveDto.SkippableAds_Undetermined;
                            VideoTargeting.Playback_AutoPlaySoundOn = targetingSaveDto.Playback_AutoPlaySoundOn;
                            VideoTargeting.Playback_AutoPlaySoundOff = targetingSaveDto.Playback_AutoPlaySoundOff;
                            VideoTargeting.Playback_ClickToPlay = targetingSaveDto.Playback_ClickToPlay;
                            VideoTargeting.Playback_Undetermined = targetingSaveDto.Playback_Undetermined;
                            VideoTargeting.RewardedAds = targetingSaveDto.Video_RewardedAds;
                            VideoTargeting.MatchOrientation = targetingSaveDto.Video_MatchOrientation;
                            VideoTargeting.RewardedAdOnly = targetingSaveDto.RewardedAdOnly;
                            SaveVideoTargeting(VideoTargeting, targetingSaveDto);

                        }
                        else
                        {

                            var targetingObj = new VideoTargeting
                            {
                                Type = targetingTypeRepository.Get(VideoTargetingTypeId),
                                PlacementType_InStream = targetingSaveDto.PlacementType_InStream,
                                PlacementType_OutStream = targetingSaveDto.PlacementType_OutStream,
                                PlacementType_Interstitial = targetingSaveDto.PlacementType_Interstitial,
                                PlacementType_Undetermined = targetingSaveDto.PlacementType_Undetermined,
                                InStreamPosition_PreRoll = targetingSaveDto.InStreamPosition_PreRoll,
                                InStreamPosition_MidRoll = targetingSaveDto.InStreamPosition_MidRoll,
                                InStreamPosition_PostRoll = targetingSaveDto.InStreamPosition_PostRoll,
                                InStreamPosition_Undetermined = targetingSaveDto.InStreamPosition_Undetermined,
                                SkippableAds_SkippableAdSpaces = targetingSaveDto.SkippableAds_SkippableAdSpaces,
                                SkippableAds_NonSkippableAdSpaces = targetingSaveDto.SkippableAds_NonSkippableAdSpaces,
                                SkippableAds_Undetermined = targetingSaveDto.SkippableAds_Undetermined,
                                Playback_AutoPlaySoundOn = targetingSaveDto.Playback_AutoPlaySoundOn,
                                Playback_AutoPlaySoundOff = targetingSaveDto.Playback_AutoPlaySoundOff,
                                Playback_ClickToPlay = targetingSaveDto.Playback_ClickToPlay,
                                Playback_Undetermined = targetingSaveDto.Playback_Undetermined,
                                RewardedAds = targetingSaveDto.Video_RewardedAds,
                                RewardedAdOnly = targetingSaveDto.RewardedAdOnly,
                                MatchOrientation = targetingSaveDto.Video_MatchOrientation,
                            };
                            SaveVideoTargeting(targetingObj, targetingSaveDto);
                            //}
                            item.AddGroupTargeting(adGroupObj, targetingObj);

                        }
                    }
                    #endregion



                    adGroupObj.TrackInstalls = targetingSaveDto.TrackInstalls;
                    adGroupObj.OpenInExternalBrowser = targetingSaveDto.OpenInExternalBrowser;

                    adGroupObj.SetAdGroupBid(targetingSaveDto.Bid, adGroupObj.CostModelWrapper.Factor
                       );
                    if (adGroupObj.GetAds() != null && adGroupObj.GetAds().Count() <= 1)

                    {
                        if (adGroupObj.GetAds().Count() < 1)
                        {
                            adGroupObj.MinimumUnitPrice = adGroupObj.Bid;
                        }
                        else
                        {
                            if (adGroupObj.GetAds().OrderBy(M => M.GetReadableBid()).ToList().First().GetBid() < adGroupObj.Bid)
                                adGroupObj.MinimumUnitPrice = adGroupObj.Bid;
                            else
                            {

                                #region AdGroup Minumum Unit Price
                                var groupAdsafterUpdate = adGroupObj.GetAds();
                                if (groupAdsafterUpdate != null && groupAdsafterUpdate.Count > 0)
                                {

                                    var orderList = groupAdsafterUpdate.OrderBy(M => M.GetBid()).ToList();
                                    adGroupObj.MinimumUnitPrice = orderList[0].GetBid();
                                }

                                #endregion
                            }

                        }
                    }
                    else if (adGroupObj.GetAds() == null)
                    {

                        adGroupObj.MinimumUnitPrice = adGroupObj.Bid;
                    }
                    //else {
                    //    adGroupObj.GetAds();


                    //}
                    if (adGroupObj.CostModelWrapperEnum == CostModelWrapperEnum.CPC)
                    {
                        var adGroupSources = adGroupObj.GetAdGroupInventorySources();

                        if (adGroupSources != null && adGroupSources.Count > 0 /*&&  adGroupObj.RunAllExchanges*/)
                        {
                            var campConfigs = adGroupObj.GetCampaignBidConfigs();
                            if (!(campConfigs != null && campConfigs.Count > 0))
                            {
                                var error = new BusinessException();
                                error.Errors.Add(new ErrorData { ID = "CampaignBidConfigsNotValid" });
                                throw error;
                            }
                        }
                    }
                    adGroupObj.SetAdsDataBid();


                    CampaignRepository.Save(item);
                    // if (item.CampaignType == CampaignType.Normal)
                    //{
                    SaveAdGroupTrackingEventsPrerequisites(item, adGroupObj, targetingSaveDto.InsertedTrackingEvents.ToDictionary(p => p.Code, p => p.PreRequisites));
                    //}



                    #region Conversions And Events

                    #region Conversions



                    adGroupObj.ConversionSetting = targetingSaveDto.ConversionSetting;
                    adGroupObj.ConversionType = targetingSaveDto.ConversionType;
                    adGroupObj.ViewAttribuation = targetingSaveDto.ViewAttribuation;
                    adGroupObj.ClickAttribuation = targetingSaveDto.ClickAttribuation;
                    adGroupObj.CountingTypeAttribuation = targetingSaveDto.CountingTypeAttribuation;

                    adGroupObj.CountingAttribuation = targetingSaveDto.CountingAttribuation;

                    #endregion
                    string columnPrefixName = Configuration.StatisticsColumnPrefixName;
                    IList<int> intList = new List<int>();
                    foreach (var trackingEventSaveDto in targetingSaveDto.AdEventItems)
                    {
                        if (trackingEventSaveDto.IsNotChanged)

                        { continue; }
                        var adGroupTrackingEvent = MapperHelper.Map<AdGroupTrackingEvent>(trackingEventSaveDto);

                        //adGroupObj.TrackingEvents
                        int mos = 0;

                        if (trackingEventSaveDto.SegmentsId != null)
                        {
                            intList = trackingEventSaveDto.SegmentsId.Split(',')
                                .Where(m => int.TryParse(m, out mos))
                                .Select(m => int.Parse(m))
                                .ToList();
                        }


                        var orginalEvent = adGroupObj.GetTrackingEvents().Where(p => p.Code == trackingEventSaveDto.Code).FirstOrDefault();
                        if (orginalEvent != null)
                        {

                            if (orginalEvent.AudienceSegmentListsMap == null)
                            {

                                orginalEvent.AudienceSegmentListsMap = new List<AudienceSegmentEventMap>();
                            }
                            foreach (var item1 in intList)
                            {

                                if (orginalEvent.AudienceSegmentListsMap.Where(M => M.AudienceSegment.ID == item1).SingleOrDefault() == null)
                                {
                                    orginalEvent.AudienceSegmentListsMap.Add(new AudienceSegmentEventMap { AudienceSegment = new AudienceSegment { ID = item1 }, Event = orginalEvent });
                                }

                            }

                            for (var i = 0; i < orginalEvent.AudienceSegmentListsMap.Count; i++)
                            {

                                if (!intList.Contains(orginalEvent.AudienceSegmentListsMap[i].AudienceSegment.ID))
                                {
                                    orginalEvent.AudienceSegmentListsMap.Remove(orginalEvent.AudienceSegmentListsMap[i]);
                                    if (i != 0)
                                    {
                                        i--;
                                    }

                                }
                            }
                        }

                    }

                    var listOfTrackings = adGroupObj.GetTrackingEvents();
                    if (listOfTrackings != null && listOfTrackings.Count > 0)
                    {
                        for (int i3 = 0; i3 < listOfTrackings.Count; i3++)
                        {
                            var Item = targetingSaveDto.AdEventItems.Where(M => M.Code == listOfTrackings[i3].Code).SingleOrDefault();

                            if (Item == null)
                            {
                                if (listOfTrackings[i3].AudienceSegmentListsMap != null && listOfTrackings[i3].AudienceSegmentListsMap.Count > 0)
                                    listOfTrackings[i3].AudienceSegmentListsMap.Clear();
                            }
                           

                        }
                    }
                    intList = new List<int>();

                    foreach (var conversionSaveDto in targetingSaveDto.ConversionItems)
                    {
                        if (conversionSaveDto.IsNotChanged)
                        { continue; }
                        var adGroupTrackingEvent = MapperHelper.Map<AdGroupConversionEvent>(conversionSaveDto);

                 
                        int mos = 0;

                        if (conversionSaveDto.PixelsId != null)
                        {
                            intList = conversionSaveDto.PixelsId.Split(',')
                                .Where(m => int.TryParse(m, out mos))
                                .Select(m => int.Parse(m))
                                .ToList();
                        }


                      
                        var orginalEvent = adGroupObj.GetConversionEvents().Where(p => p.Code == conversionSaveDto.Code).FirstOrDefault();
                        var orginalTracEvent = adGroupObj.GetTrackingEvents().Where(p => p.Code == conversionSaveDto.Code).FirstOrDefault();
                        if (orginalEvent != null)
                        {

                            if (orginalEvent.PixelListsMap == null)
                            {

                                orginalEvent.PixelListsMap = new List<PixelEventMap>();
                            }
                            foreach (var item1 in intList)
                            {

                                if (orginalEvent.PixelListsMap.Where(M => M.Pixel.ID == item1).SingleOrDefault() == null)

                                    orginalEvent.PixelListsMap.Add(new PixelEventMap { Pixel = new Pixel { ID = item1 }, Event = orginalEvent });
                            }



                            for (var i = 0; i < orginalEvent.PixelListsMap.Count; i++)
                            {

                                if (!intList.Contains(orginalEvent.PixelListsMap[i].Pixel.ID))
                                {
                                    orginalEvent.PixelListsMap.Remove(orginalEvent.PixelListsMap[i]);
                                    if (i != 0)
                                    {
                                        i--;
                                    }

                                }
                            }


                            orginalEvent.IsPrimary = conversionSaveDto.IsPrimary;
                            orginalEvent.Revenue = conversionSaveDto.Revenue;
                        }
                       else if (orginalTracEvent != null)
                        {
                            AdGroupConversionEvent convEvent = new AdGroupConversionEvent();
                            convEvent.ID = orginalTracEvent.ID;
                            if (orginalTracEvent.PixelListsMap == null)
                            {

                                orginalTracEvent.PixelListsMap = new List<PixelEventMap>();
                            }
                            foreach (var item1 in intList)
                            {

                                if (orginalTracEvent.PixelListsMap.Where(M => M.Pixel.ID == item1).SingleOrDefault() == null)

                                    orginalTracEvent.PixelListsMap.Add(new PixelEventMap { Pixel = new Pixel { ID = item1 }, Event = orginalTracEvent });
                            }



                            for (var i = 0; i < orginalTracEvent.PixelListsMap.Count; i++)
                            {

                                if (!intList.Contains(orginalTracEvent.PixelListsMap[i].Pixel.ID))
                                {
                                    orginalTracEvent.PixelListsMap.Remove(orginalTracEvent.PixelListsMap[i]);
                                    if (i != 0)
                                    {
                                        i--;
                                    }

                                }
                            }
                           
                            orginalTracEvent.Revenue = conversionSaveDto.Revenue;
                            orginalTracEvent.IsPrimary = conversionSaveDto.IsPrimary;
                        }
                        else
                        {




                            var adGroupConversionEvent = MapperHelper.Map<AdGroupConversionEvent>(conversionSaveDto);
                            adGroupConversionEvent.Description = conversionSaveDto.Name;
                            if (string.IsNullOrEmpty(conversionSaveDto.Code))
                            {
                                conversionSaveDto.Code = "Conv" + GetConversionCounter();
                                updateAccountTrackingEventsSupportConversion(adGroupObj, adGroupConversionEvent);
                            }
                            adGroupConversionEvent = MapperHelper.Map<AdGroupConversionEvent>(conversionSaveDto);
                            adGroupConversionEvent.IsConversion = true;
                            adGroupConversionEvent.IsCustom = true;
                            adGroupConversionEvent.AdGroup = adGroupObj;
                            adGroupConversionEvent.Description = conversionSaveDto.Name;
                            var listOfTrackingsAll2 = adGroupObj.GetTrackingEvents();
                            for (int i = 1; i <= Configuration.MaxAdGroupConversionEvents; i++)
                            {
                                string statisticsColumnName = string.Format("{0}{1}", columnPrefixName, i);
                                if (!adGroupObj.ConversionEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName)  && !listOfTrackingsAll2.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName))
                                {
                                    adGroupConversionEvent.StatisticsColumnName = statisticsColumnName;
                                    break;
                                }
                            }


                            mos = 0;

                            if (conversionSaveDto.PixelsId != null)
                            {
                                intList = conversionSaveDto.PixelsId.Split(',')
                                    .Where(m => int.TryParse(m, out mos))
                                    .Select(m => int.Parse(m))
                                    .ToList();
                            }



                            if (intList != null && intList.Count > 0)
                            {
                                adGroupConversionEvent.PixelListsMap = new List<PixelEventMap>();

                                foreach (var item2 in intList)
                                {

                                    adGroupConversionEvent.PixelListsMap.Add(new PixelEventMap { Pixel = new Pixel { ID = item2 }, Event = adGroupConversionEvent });
                                }
                                adGroupObj.ConversionEvents.Add(adGroupConversionEvent);

                            }







                        }

                    }
                    var listOfConversions = adGroupObj.GetConversionEvents();

                    var listOfTrackingsAll = adGroupObj.GetTrackingEvents();
                    var defaultClick = adGroupObj.GetTrackingEvents().Where(p => p.Code == "pagein").FirstOrDefault();

                    if (defaultClick == null)
                    {
                        var eventClick = adGroupObj.GetTrackingEvents().Where(p => p.Code == "000clk").FirstOrDefault();
                        if (eventClick != null)
                        {
                            var pagIntiEvent = new AdGroupTrackingEvent { AdGroup = adGroupObj, Code = "pagein", Description = "pageinit" };
                            pagIntiEvent.IsTracking = true;

                            adGroupObj.TrackingEvents.Add(pagIntiEvent);


                            for (int i = 1; i <= Configuration.MaxAdGroupTrackingEvents; i++)
                            {
                                string statisticsColumnName = string.Format("{0}{1}", columnPrefixName, i);
                                if (!adGroupObj.TrackingEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName) && !adGroupObj.ConversionEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName))
                                {
                                    pagIntiEvent.StatisticsColumnName = statisticsColumnName;
                                    break;
                                }
                            }

                        }
                    }

                    var viewImpress = adGroupObj.GetTrackingEvents().Where(p => p.Code == "00vimp").FirstOrDefault();

                    if (viewImpress == null)
                    {
                        var impClick = adGroupObj.GetTrackingEvents().Where(p => p.Code == "000imp").FirstOrDefault();
                        if (impClick != null)
                        {
                            var viewImpEvent = new AdGroupTrackingEvent { AdGroup = adGroupObj, Code = "00vimp", Description = "viewableimpression" };
                            viewImpEvent.IsTracking = true;

                            adGroupObj.TrackingEvents.Add(viewImpEvent);


                            for (int i = 1; i <= Configuration.MaxAdGroupTrackingEvents; i++)
                            {
                                string statisticsColumnName = string.Format("{0}{1}", columnPrefixName, i);
                                if (!adGroupObj.TrackingEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName) && !adGroupObj.ConversionEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName))
                                {
                                    viewImpEvent.StatisticsColumnName = statisticsColumnName;
                                    break;
                                }
                            }

                        }
                    }
                    if (listOfConversions != null && listOfConversions.Count > 0)
                    {
                        for (int i2 = 0; i2 < listOfConversions.Count; i2++)
                        {
                            var Item = targetingSaveDto.ConversionItems.Where(M => M.Code == listOfConversions[i2].Code).SingleOrDefault();

                            if (Item == null)
                            {
                                if (listOfTrackingsAll.Where(M => M.Code == listOfConversions[i2].Code).SingleOrDefault() == null)
                                {
                                    listOfConversions[i2].IsDeleted = true;
                                }
                                else
                                {

                                    listOfConversions[i2].PixelListsMap.Clear();
                                }
                                //adGroupObj.ConversionEvents.Remove(adGroupObj.ConversionEvents[i2]);
                            }
                            //if (i2 != 0)
                            //{
                            //    i2 = i2 - 1; ;

                            //}

                        }
                    }
                    var FrequencyCappingNumber = adGroupObj.CountingAttribuation;
                    var FrequencyCappingInterval = 60;
                    var FrequencyCappingType = 1;
                    var noOfMonth = 3 * 30;

                    if (adGroupObj.ConversionSetting == ConversionSetting.CountingFirst)
                    {
                        //if (item.LifeTime != CampaignLifeTime.Default)
                        //{
                        //    if (item.EndDate.HasValue)
                        //    {
                        //        var DateDiff = item.EndDate.Value - item.StartDate;

                        //        var month = DateDiff.TotalDays / 30;

                        //        noOfMonth = noOfMonth + (int)month;



                        //    }

                        //}

                        //var noOfMonth = 3 * 30;

                        //if (item.LifeTime == CampaignLifeTime.Default)
                        //{
                        if (item.EndDate.HasValue)
                        {
                            var DateDiff = item.EndDate.Value - item.StartDate;

                            var month = DateDiff.TotalDays;

                            if (month == 0)
                            {
                                month = 1;
                            }
                            noOfMonth = noOfMonth + (int)month;



                        }

                        //}


                        FrequencyCappingInterval = noOfMonth * (int)(int)Domain.Common.Model.Campaign.FrequencyCappingInterval.Day;



                        // FrequencyCappingInterval = noOfMonth * (int)Domain.Model.Campaign.FrequencyCappingInterval.Month;
                        FrequencyCappingType = 2;
                        FrequencyCappingNumber = 1;
                    }
                    else if (adGroupObj.ConversionSetting == ConversionSetting.CountingAll)
                    {
                        FrequencyCappingInterval = 0;
                        FrequencyCappingType = 0;
                        FrequencyCappingNumber = 1;

                    }
                    else if (adGroupObj.ConversionSetting == ConversionSetting.CountingEvery)
                    {
                        int numbrofMinutes = adGroupObj.CountingAttribuation;
                        if (adGroupObj.CountingTypeAttribuation == CountingTypeAttribuation.Hours)
                        {
                            numbrofMinutes = adGroupObj.CountingAttribuation * 60;

                        }
                        if (adGroupObj.CountingTypeAttribuation == CountingTypeAttribuation.Days)
                        {
                            numbrofMinutes = adGroupObj.CountingAttribuation * 24 * 60;

                        }
                        if (adGroupObj.CountingTypeAttribuation == CountingTypeAttribuation.Months)
                        {
                            numbrofMinutes = adGroupObj.CountingAttribuation * 30 * 24 * 60;

                        }
                        FrequencyCappingInterval = 60 * numbrofMinutes;

                    }
                    var listofConf=   adGroupObj.GetConversionEvents();
                    List<AdGroupEvent> groupevents = new List<AdGroupEvent>();
                    if (listofConf!=null)
                    {
                        foreach (var item2 in listofConf)
                        {
                            groupevents.Add(item2);

                        }
                    }
                    var listofTr = adGroupObj.GetTrackingEvents().Where(m=>m.IsConversion==true);
                    if (listofTr!=null)
                    {
                        foreach (var item2 in listofTr)
                        {
                            if(listofConf.Where(M=>M.Code==item2.Code).SingleOrDefault()==null)
                            groupevents.Add(item2);

                        }
                    }

                    if (groupevents != null && groupevents.Count > 0)
                    {

                        var convFreqList = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.IsConversion == true).ToList();

                        if (convFreqList != null)
                        {
                            for (int i4 = 0; i4 < convFreqList.Count; i4++)
                            {
                                var itemtoBeRemoved = groupevents.Where(M => M.Code == convFreqList[i4].Event.Code).SingleOrDefault();
                                if (itemtoBeRemoved == null)
                                {
                                    item.CampaignServerSetting.FrequencyCappingList.Remove(convFreqList[i4]);
                                    convFreqList = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.IsConversion == true).ToList();
                                    if (i4 != 0)
                                    {
                                        i4 = i4 - 1;
                                    }
                                }
                            }
                        }
                        var ConversionsItem = groupevents;
                        foreach (var convItem in ConversionsItem)
                        {
                            CampaignFrequencyCapping frequencyCapping = item.CampaignServerSetting.GetFrequencyCappingList().Where(p => p.Event.Code == convItem.Code).SingleOrDefault();

                            if (frequencyCapping != null)
                            {
                                frequencyCapping.Interval = FrequencyCappingInterval;
                                frequencyCapping.Number = FrequencyCappingNumber;
                                frequencyCapping.Type = FrequencyCappingType;

                                // frequencyCapping. = frequencyCappingSave.IsCapping;
                                //var error = new BusinessException();
                                //error.Errors.Add(new ErrorData { ID = "DuplicateFrequencyCapping" });
                                //   throw error;
                            }
                            else
                            {
                                frequencyCapping = new CampaignFrequencyCapping();
                                frequencyCapping.Interval = FrequencyCappingInterval;
                                frequencyCapping.Number = FrequencyCappingNumber;
                                frequencyCapping.Type = FrequencyCappingType;

                                frequencyCapping.Event = trackingEventRepository.Query(M => M.Code == convItem.Code).SingleOrDefault();


                                frequencyCapping.CampaignServerSetting = item.CampaignServerSetting;



                                item.CampaignServerSetting.FrequencyCappingList.Add(frequencyCapping);

                            }
                        }
                    }
                    //else
                    //{

                    //    var convFreqList=_campaignFrequencyCappingRepository.Query(M => M.Event.IsConversion == true && M.CampaignServerSetting.ID== item.ID).ToList() ;
                    //    foreach (var confFreqitem in convFreqList)
                    //    {
                    //        _campaignFrequencyCappingRepository.Remove(confFreqitem);


                    //    }
                    //}

                    #endregion

                    #region  BidModifier

                    if (targetingSaveDto.AdGroupBidModifiersDto != null)
                    {
                        foreach (var adGroupMod in targetingSaveDto.AdGroupBidModifiersDto)
                        {
                            var adgroupModifier = MapperHelper.Map<AdGroupBidModifier>(adGroupMod);
                            if (!adgroupModifier.IsDeleted && adGroupObj.BiddingStrategy  == BiddingStrategy.Fixed && adgroupModifier.DimentionType !=DimentionType.Any)
                                adGroupObj.AddAdGroupBidModifier(adgroupModifier);
                            else
                                adGroupObj.DeleteAdGroupBidModifier(adgroupModifier.ID);
                        }


                    }
                    #endregion

                
                    CampaignRepository.Save(item);


                    #region "PMPDealsUpdat Ads"
                    //if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.InventorySource))

                    //{
                    if (item.Account.AccountRole == AccountRole.DSP)
                    {
                        var ads = adGroupObj.GetAds();
                        var approvedads = new List<AdCreative>();
                        if (ads != null && ads.Count > 0)
                            approvedads = ads.ToList();
                        if (adGroupObj.AdGroupInventorySources != null && adGroupObj.AdGroupInventorySources.Where(x => !x.IsDeleted).Count() > 0)
                        {
                            var allSources = adGroupObj.AdGroupInventorySources.Where(x => !x.IsDeleted).Distinct();
                            if (allSources != null)
                            {
                                foreach (var inventory in allSources)
                                {


                                    foreach (var adObj in approvedads)
                                    {

                                        adObj.AddAppSiteAdQueue(inventory.AppSite, inventory.Include == true);
                                    }

                                }
                            }
                            foreach (var adObj in approvedads)
                            {
                                //if (adObj.Status.ID == AdCreativeStatus.Active.ID || (adObj.PausedStatus != null && adObj.PausedStatus.ID == AdCreativeStatus.Active.ID))
                                //{
                                var appSitesList = adObj.AppSiteAdQueues;
                                if (appSitesList != null)
                                {
                                    for (int ic = 0; ic < appSitesList.Count; ic++)
                                    {
                                        if (allSources.Where(M => M.AppSite.ID == appSitesList[ic].AppSite.ID).FirstOrDefault() == null)
                                        {
                                            appSitesList.RemoveAt(ic);
                                            if (ic > 0)
                                                ic = ic - 1;
                                        }

                                    }
                                }

                                //}
                            }
                        }
                        else
                        {
                            var sspPartnters = _SSPPartnerRepository.Query(x => !x.IsDeleted).ToList();
                            foreach (var sspPartnter in sspPartnters)
                            {

                                foreach (var adObj in approvedads)
                                {
                                    //if (adObj.Status.ID == AdCreativeStatus.Active.ID || (adObj.PausedStatus != null && adObj.PausedStatus.ID == AdCreativeStatus.Active.ID))
                                    adObj.AddAppSiteAdQueue(sspPartnter.AppSite);
                                }
                            }

                            foreach (var adObj in approvedads)
                            {
                                //if (adObj.Status.ID == AdCreativeStatus.Active.ID || (adObj.PausedStatus != null && adObj.PausedStatus.ID == AdCreativeStatus.Active.ID))
                                //{
                                var appSitesList = adObj.AppSiteAdQueues;
                                if (appSitesList != null)
                                {
                                    for (int ic = 0; ic < appSitesList.Count; ic++)
                                    {
                                        if (sspPartnters.Where(M => M.AppSite.ID == appSitesList[ic].AppSite.ID).FirstOrDefault() == null)
                                        {
                                            appSitesList.RemoveAt(ic);
                                            if (ic > 0)
                                                ic = ic - 1;
                                        }

                                    }
                                }


                                //}
                            }

                        }
                    }
                    //}

                    #endregion




                    if (!IsCampaignManager() && SearchForMyAudience(adGroupObj))
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSaveMyAud") });

                    }
                    dtoResult.Result = true;
                    return dtoResult;
                }
                else
                {
                    return dtoResult;
                }
            }
            return dtoResult;
        }

        public void SaveTargetingHouseAd(TargetingSaveDto targetingSaveDto)
        {
            var adGroupObj = adGroupRepository.Get(targetingSaveDto.AdGroupId);
            
            #region HouseAd
            if (targetingSaveDto.isFromHouseAd)
            {
                if (adGroupObj.HouseAd == null)
                {
                    adGroupObj.HouseAd = new HouseAd(adGroupObj)
                    {
                        Account = adGroupObj.Campaign.Account,
                        User = adGroupObj.Campaign.User,
                        DeliveryMode = targetingSaveDto.DeliveryMode,
                        ForAppSite = appSiteRepository.Get(targetingSaveDto.ForAppSite) 
                    };
                    foreach (var destinationAppSite in targetingSaveDto.TargetAppSites)
                    {
                        adGroupObj.HouseAd.AddDestinationAppSite(appSiteRepository.Get(destinationAppSite)  );
                    }
                    adGroupObj.HouseAd.ID = adGroupObj.ID;
                }
                else
                {

                    //update

                    adGroupObj.HouseAd.ClearDestinationAppSite();

                    adGroupObj.HouseAd.DeliveryMode = targetingSaveDto.DeliveryMode;
                    adGroupObj.HouseAd.ForAppSite = appSiteRepository.Get(targetingSaveDto.ForAppSite);
                    foreach (var destinationAppSite in targetingSaveDto.TargetAppSites)
                    {
                        adGroupObj.HouseAd.AddDestinationAppSite(appSiteRepository.Get(destinationAppSite));
                    }
                }


                switch (adGroupObj.HouseAd.DeliveryMode)
                {
                    case HouseAdDeliveryMode.FullyAllocate:
                        {
                            adGroupObj.CPMValue = Int16.MaxValue;
                            adGroupObj.Bid = 0;
                            adGroupObj.MinimumUnitPrice = 0;
                            break;
                        }
                    case HouseAdDeliveryMode.WhenNoAds:
                        {
                            adGroupObj.CPMValue = null;
                            adGroupObj.Bid = 0;
                            adGroupObj.MinimumUnitPrice = 0;
                            break;
                        }
                }
                houseAdRepository.Save(adGroupObj.HouseAd);
            }
            #endregion

        }
        private int GeSegmentsCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "Custom Audience Segments");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        public string SaveSegmentsForTargeting(SaveAudSegmentTargetingRequest request)
        {
            TargetingResultDto dtoResult = new TargetingResultDto();
            string groupAudianceString = string.Empty;
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);
            var ProviderData = _DPPartnerRepository.Get(request.DpId);

            //#region Audiances
            if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.Audience))
            //{
            {

                var item = adGroupObj.Campaign;

                Expression<Func<AudienceSegment, bool>> filter = c => true;


                filter = c => c.Advertiser != null && c.Advertiser.ID == request.IdAccAdv && c.Provider.ID == request.DpId && c.IsDeleted == false;

                List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

                List<AudienceSegment> deletedSeg = new List<AudienceSegment>();
                List<AudienceSegment> UpdateSeg = new List<AudienceSegment>();

                foreach (var seg in request.Segments)
                {
                    seg.OperatorSegmentCode = (ProviderData.Code + ":" + seg.IntegrationId);
                }
                foreach (var audItem in list)
                {
                    var searchitem = request.Segments.Where(M => M.OperatorSegmentCode == audItem.OperatorSegmentCode).SingleOrDefault();
                    if (searchitem != null)
                    {
                        //audItem.Price = searchitem.Price;

                        searchitem.ID = audItem.ID;
                        UpdateSeg.Add(audItem);
                    }
                    else
                    {
                        request.Segments.Remove(searchitem);
                        deletedSeg.Add(audItem);
                    }

                }


                /*for (var i = 0; i < deletedSeg.Count; i++)
                  {
                      deletedSeg[i].IsDeleted = true;
                      audianSegRep.Save(deletedSeg[i]);
                  }*/
                if (request.Segments != null && request.Segments.Count > 0)
                {
                    filter = c => c.Parent == null && c.Provider.ID == request.DpId && c.IsDeleted == false;
                    var maxCode = audianSegRep.GeMaxCode();
                    var parentseg = audianSegRep.Query(filter).FirstOrDefault();
                    ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.group maingrp = new Domain.Model.Campaign.Targeting.group();
                    ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.group grp = new Domain.Model.Campaign.Targeting.group();
                    grp.Operator = "OR";
                    maingrp.Operator = "OR";
                    maingrp.rules = new child[1];
                    // grp.rules[1] = new child { condition="Target"};
                    grp.rules = new child[request.Segments.Count];
                    for (var i = 0; i < request.Segments.Count; i++)
                    {
                        request.Segments[i].Name = new LocalizedStringDto
                        {
                            GroupKey = "AudienceSegment",
                            Value = request.Segments[i].en,
                            Values = new List<LocalizedValueDto>()

                        };
                        request.Segments[i].Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = ProviderData.Code + ":" + request.Segments[i].IntegrationId });
                        request.Segments[i].Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ProviderData.Code + ":" + request.Segments[i].IntegrationId });
                        var segmiteem = MapperHelper.Map<AudienceSegment>(request.Segments[i]);
                        segmiteem.Account = new Domain.Model.Account.Account { ID = item.Account.ID };
                        segmiteem.User = new Domain.Model.Account.User { ID = item.User.ID };
                        segmiteem.CostModel = new CostModel { ID = (int)CostModelEnum.CPM };

                        foreach (var localizedValue in segmiteem.Name.Values)
                        {
                            if (!string.IsNullOrEmpty(segmiteem.Name.Value))
                            {
                                segmiteem.Name.Value = segmiteem.Name.Value.Trim();
                            }
                            localizedValue.LocalizedString = segmiteem.Name;
                        }
                        segmiteem.OperatorSegmentCode = ProviderData.Code + ":" + request.Segments[i].IntegrationId;
                        segmiteem.Parent = new AudienceSegment { ID = parentseg.ID };




                        if (segmiteem.ID == 0)
                        {
                            segmiteem.Code = GeSegmentsCounter();
                            if (segmiteem.Price > 0)
                            {
                                segmiteem.Price = segmiteem.Price / 1000;
                            }
                            audianSegRep.Save(segmiteem);

                        }
                        else
                        {
                            var updatedItem = audianSegRep.Get(segmiteem.ID);
                            if (segmiteem.Price > 0)
                            {
                                segmiteem.Price = segmiteem.Price / 1000;
                            }
                            updatedItem.Price = segmiteem.Price;
                            updatedItem.Name.Values[0].Value = segmiteem.Name.Values[0].Value;
                            updatedItem.Name.Values[1].Value = segmiteem.Name.Values[1].Value;
                            segmiteem.Code = updatedItem.Code;
                            audianSegRep.Save(updatedItem);
                        }
                        grp.rules[i] = new child()  { recency= "65535", ParentId = segmiteem.Parent.ID.ToString(), id = segmiteem.ID.ToString(), Price = segmiteem.Price.ToString(), Name = segmiteem.Name.ToString(), condition = "Target" };
                    }
                    maingrp.rules[0] = new child()
                    {


                        group = grp
                    };
                    groupAudianceString = JsonConvert.SerializeObject(maingrp);
                }


            }
            return groupAudianceString;
        }

        public string SaveSegmentsForTargetingForDel(SaveAudSegmentTargetingRequest request)
        {
            TargetingResultDto dtoResult = new TargetingResultDto();
            string groupAudianceString = string.Empty;
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);
            var ProviderData = _DPPartnerRepository.Get(request.DpId);

            //#region Audiances
            if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.Audience))
            //{
            {

                var item = adGroupObj.Campaign;

                Expression<Func<AudienceSegment, bool>> filter = c => true;


                filter = c => c.Advertiser != null && c.Advertiser.ID == request.IdAccAdv && c.Provider.ID == request.DpId && c.IsDeleted == false;

                List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

                List<AudienceSegment> deletedSeg = new List<AudienceSegment>();
                List<AudienceSegment> UpdateSeg = new List<AudienceSegment>();

                foreach (var seg in request.Segments)
                {
                    seg.OperatorSegmentCode = (ProviderData.Code + ":" + seg.IntegrationId);
                }
                foreach (var audItem in list)
                {
                    var searchitem = request.Segments.Where(M => M.OperatorSegmentCode == audItem.OperatorSegmentCode).SingleOrDefault();
                    if (searchitem != null)
                    {
                        //audItem.Price = searchitem.Price;

                        searchitem.ID = audItem.ID;
                       
                    }
                    else
                    {
                        request.Segments.Remove(searchitem);
                      
                    }

                }


                /*for (var i = 0; i < deletedSeg.Count; i++)
                  {
                      deletedSeg[i].IsDeleted = true;
                      audianSegRep.Save(deletedSeg[i]);
                  }*/
                if (request.Segments != null && request.Segments.Count > 0)
                {
                    filter = c => c.Parent == null && c.Provider.ID == request.DpId && c.IsDeleted == false;
                    var maxCode = audianSegRep.GeMaxCode();
                    var parentseg = audianSegRep.Query(filter).FirstOrDefault();
                 
                    for (var i = 0; i < request.Segments.Count; i++)
                    {
                        request.Segments[i].Name = new LocalizedStringDto
                        {
                            GroupKey = "AudienceSegment",
                            Value = request.Segments[i].en,
                            Values = new List<LocalizedValueDto>()

                        };
                        request.Segments[i].Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = ProviderData.Code + ":" + request.Segments[i].IntegrationId });
                        request.Segments[i].Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ProviderData.Code + ":" + request.Segments[i].IntegrationId });
                        var segmiteem = MapperHelper.Map<AudienceSegment>(request.Segments[i]);
                        segmiteem.Account = new Domain.Model.Account.Account { ID = item.Account.ID };
                        segmiteem.User = new Domain.Model.Account.User { ID = item.User.ID };
                        segmiteem.CostModel = new CostModel { ID = (int)CostModelEnum.CPM };

                        foreach (var localizedValue in segmiteem.Name.Values)
                        {
                            if (!string.IsNullOrEmpty(segmiteem.Name.Value))
                            {
                                segmiteem.Name.Value = segmiteem.Name.Value.Trim();
                            }
                            localizedValue.LocalizedString = segmiteem.Name;
                        }
                        segmiteem.OperatorSegmentCode = ProviderData.Code + ":" + request.Segments[i].IntegrationId;
                        segmiteem.Parent = new AudienceSegment { ID = parentseg.ID };




                        if (segmiteem.ID != 0)
                          {
                            var updatedItem = audianSegRep.Get(segmiteem.ID);
                         
                            updatedItem.Name.Values[0].Value = segmiteem.Name.Values[0].Value;
                            updatedItem.Name.Values[1].Value = segmiteem.Name.Values[1].Value;
                            segmiteem.Code = updatedItem.Code;
                            updatedItem.IsDeleted = true;
                           // updatedItem.Activated = false;
                            audianSegRep.Save(updatedItem);
                        }

                    }
                   
                }


            }
            return string.Empty;
        }


        public TargetingResultDto AddExternalAudSegmentTargeting(AddExternalAudSegmentTargetingRequest request)
        {
            TargetingResultDto dtoResult = new TargetingResultDto();
            // string groupAudianceString = string.Empty;
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);
            #region Audiances
            if (_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.Audience))
            //{
            {

                var item = adGroupObj.Campaign;
                dtoResult.CampId = item.ID;
                // var returnList = list.Select(campaign => MapperHelper.Map<AudienceSegmentDto>(campaign)).ToList();

                ValidateCampaign(item);


                var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();

                //var AudianceSegmentCostModelTypeAllowed = configurationManager.GetConfigurationSetting(null, null, "AudianceSegmentCostModelTypeAllowed");
                //IList<int> costmodelvalues = AudianceSegmentCostModelTypeAllowed.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                if (audienceSegmentTargeting == null)
                {

                    //create new 
                    audienceSegmentTargeting = new AudienceSegmentTargeting
                    {
                        Type = targetingTypeRepository.Get(12)

                    };
                    string json = string.Empty;
                    if (!string.IsNullOrEmpty(request.Group))
                    {
                        //  json = new JavaScriptSerializer().Serialize(targetingSaveDto.group);

                        // if (targetingSaveDto.changedAudiances == "true")
                        audienceSegmentTargeting.RulesJson = audienceSegmentTargeting.GetRulesJsonForExpression(request.Group);
                        audienceSegmentTargeting.DataProvider = new DPPartner { ID = request.DpId };
                        audienceSegmentTargeting.IsExternal = true;
                        adGroupObj.SetAdsMaxDataBid(audienceSegmentTargeting);
                        item.AddGroupTargeting(adGroupObj, audienceSegmentTargeting);
                        audienceSegmentTargeting.DataBid = audienceSegmentTargeting.MaxDataBid;


                        var beforeids = audienceSegmentTargeting.GetAudienceSegmentIds();
                        if (dtoResult.IdsAdd == null)
                            dtoResult.IdsAdd = new List<int>();
                        dtoResult.FireEvents = true;
                        foreach (var singitem in beforeids)
                        {
                            // if (!afterids.Contains(singitem))
                            {
                                dtoResult.IdsAdd.Add(singitem);
                            }
                        }
                      


                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.Group))
                    {
                        //string json = new JavaScriptSerializer().Serialize(targetingSaveDto.group);

                        //if (targetingSaveDto.changedAudiances == "true")

                        int lastVersion = audienceSegmentTargeting.GetLastVersionForRuleJson(audienceSegmentTargeting.RulesJson);
                      var beforeids=  audienceSegmentTargeting.GetAudienceSegmentIds();

                        audienceSegmentTargeting.RulesJson = audienceSegmentTargeting.GetRulesJsonForExpression(request.Group, lastVersion);
                        var afterids = audienceSegmentTargeting.GetAudienceSegmentIds();
                        dtoResult.IdsDiffrent = new List<int>();
                        dtoResult.IdsAdd = new List<int>(); 
                        dtoResult.FireEvents= true;
                        foreach (var singitem in beforeids)
                        {
                            if (!afterids.Contains(singitem))
                            {
                                dtoResult.IdsDiffrent.Add(singitem);
                            }
                        }
                       
                        foreach (var singitem in afterids)
                        {
                            if (!beforeids.Contains(singitem))
                            {
                                dtoResult.IdsAdd.Add(singitem);
                            }
                        } 
                        //adGroupObj.SetAdsMaxDataBid(audienceSegmentTargeting);
                        audienceSegmentTargeting.DataBid = audienceSegmentTargeting.MaxDataBid;
                    }
                    else

                    {
                        var beforeids = audienceSegmentTargeting.GetAudienceSegmentIds();
                        if(dtoResult.IdsDiffrent==null)
                        dtoResult.IdsDiffrent = new List<int>();

                        foreach (var singitem in beforeids)
                        {
                           // if (!afterids.Contains(singitem))
                            {
                                dtoResult.IdsDiffrent.Add(singitem);
                            }
                        }
                        dtoResult.FireEvents = true;
                        audienceSegmentTargeting.RulesJson = string.Empty;

                        item.RemoveGroupTargeting(adGroupObj, audienceSegmentTargeting);
                    }
                }


                var audienceSegmentTargetingF = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();
                // }

                if (audienceSegmentTargetingF != null)
                {
                    if (item.Advertiser != null)
                    {
                        var resultsTargetingAudic = audienceSegmentTargetingF.CheckIfRulesHaveAdvertiserBlocker(item.Advertiser.ID, request.Group);
                        if (resultsTargetingAudic)
                        {
                            throw new BusinessException(new List<ErrorData> { new ErrorData("audienceSegmentTargetingNotValid") });
                        }

                        // if (targetingSaveDto.changedAudiances == "true")
                        //{
                        // var changedCostElemtn = audienceSegmentTargetingF.CheckIfDataProviderCostElementAdded(adGroupObj, groupAudianceString);
                        // if (!dtoResult.AddDefaultCostElement)
                        // {
                        //   dtoResult.AddDefaultCostElement = changedCostElemtn;
                        // }
                        //}
                        //  adGroupObj.LogAdMarkup = audienceSegmentTargetingF.LogAdMarkup;
                    }
                }


            }


            #endregion


            var audienceSegmentTargetingS = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>();
            adGroupObj.DataBid = null;
            adGroupObj.MaxDataBid = null;
            if (audienceSegmentTargetingS != null)
            {
                foreach (var audienceSegmentTargeting in audienceSegmentTargetingS)
                {

                    if (audienceSegmentTargeting is AudienceSegmentTargeting)
                    {
                        adGroupObj.SetAdsMaxDataBid(audienceSegmentTargeting as AudienceSegmentTargeting);
                        if (adGroupObj.DataBid.HasValue)
                            adGroupObj.DataBid = adGroupObj.DataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.Value : 0);
                        else
                            adGroupObj.DataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid;
                        if (adGroupObj.MaxDataBid.HasValue)
                            adGroupObj.MaxDataBid = adGroupObj.MaxDataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.Value : 0);
                        else
                            adGroupObj.MaxDataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid;

                        /*
                        if (audienceSegmentTargeting.IsExternal)
                        {
                            var changedCostElemtn = audienceSegmentTargeting.CheckIfDataProviderCostElementAdded(adGroupObj, audienceSegmentTargeting.GetRulesJson());
                            if (!dtoResult.AddDefaultCostElement)
                            {
                                dtoResult.AddDefaultCostElement = changedCostElemtn;
                            }
                        }*/

                    }
                }
            }

            //var audienceSegmentTargetingS = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>();
            //adGroupObj.DataBid = null;
            //adGroupObj.MaxDataBid = null;
            //if (audienceSegmentTargetingS != null)
            //{
            //    foreach (var audienceSegmentTargeting in audienceSegmentTargetingS)
            //    {

            //        if (audienceSegmentTargeting is AudienceSegmentTargeting)
            //        {
            //            adGroupObj.SetAdsMaxDataBid(audienceSegmentTargeting as AudienceSegmentTargeting);
            //            if (adGroupObj.DataBid.HasValue)
            //                adGroupObj.DataBid = adGroupObj.DataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid.Value : 0);
            //            else
            //                adGroupObj.DataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).DataBid;
            //            if (adGroupObj.MaxDataBid.HasValue)
            //                adGroupObj.MaxDataBid = adGroupObj.MaxDataBid + ((audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.HasValue ? (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid.Value : 0);
            //            else
            //                adGroupObj.MaxDataBid = (audienceSegmentTargeting as AudienceSegmentTargeting).MaxDataBid;
            //        }
            //    }
            //}
            dtoResult.DataPriceAudienceSegment = getDataBidAudiancesUsedInIntegrationAll(ValueMessageWrapper.Create(request.AdgroupId));
            dtoResult.CountExternalAudienceList = getCountAudiancesUsedInIntegrationAll(ValueMessageWrapper.Create(request.AdgroupId)).Value;
   


            return dtoResult;
        }

        public void PublishTargetingEvent(TargetingResultDto eventino)
        {

            AudienceSegmentTargeting tar = new AudienceSegmentTargeting();
            if(eventino.IdsDiffrent!=null && eventino.IdsDiffrent.Count>0)
            tar.PublishkafkaforCheck( eventino.IdsDiffrent, eventino.CampId,"Delete");

            if (eventino.IdsAdd != null && eventino.IdsAdd .Count > 0)
                tar.PublishkafkaforCheck(eventino.IdsAdd, eventino.CampId,"Add");
        }

        public string getAudiancesUsedInIntegration(AudienceIdsMessages request)
        {
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);

            var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();

            if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
            {

                string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                return audienceSegmentTargeting.getAudienceIntegrationList(jsonObj);
            }

            return string.Empty;

        }
        public string getAudiancesUsedInIntegrationActive(AudienceIdsMessages request)
        {
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);

            var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();

            if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
            {

                string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                return audienceSegmentTargeting.getAudienceIntegrationListActive(jsonObj);
            }

            return string.Empty;

        }

        public ValueMessageWrapper<int> getCountAudiancesUsedInIntegration(AudienceIdsMessages request)
        {
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);

            var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();

            if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
            {

                string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                return ValueMessageWrapper.Create(audienceSegmentTargeting.getCountAudienceIntegrationList(jsonObj));
            }

            return ValueMessageWrapper.Create(0);

        }

        public string getDataBidAudiancesUsedInIntegration(AudienceIdsMessages request)
        {
            var adGroupObj = adGroupRepository.Get(request.AdgroupId);

            var audienceSegmentTargeting = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.DataProvider.ID == request.DpId).FirstOrDefault();

            if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
            {

                //string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                if (audienceSegmentTargeting.DataBid.HasValue)
                    return (audienceSegmentTargeting.DataBid.Value * 1000).ToString("F2");
            }

            return 0.ToString("F2");

        }



        public ValueMessageWrapper<int> getCountAudiancesUsedInIntegrationAll(ValueMessageWrapper<int> adgroupId)
        {
            var adGroupObj = adGroupRepository.Get(adgroupId.Value);

            var audienceSegmentTargetings = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.IsDeleted == false).ToList();

            var count = 0;
            if (audienceSegmentTargetings != null)
            {
                foreach (var audienceSegmentTargeting in audienceSegmentTargetings)
                {
                    if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
                    {

                        string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                        count = count + audienceSegmentTargeting.getCountAudienceIntegrationList(jsonObj);
                    }
                }
            }
            return ValueMessageWrapper.Create(count);

        }

        public string getDataBidAudiancesUsedInIntegrationAll(ValueMessageWrapper<int> adgroupId)
        {
            var adGroupObj = adGroupRepository.Get(adgroupId.Value);

            var audienceSegmentTargetings = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().Where(M => M.IsExternal == true && M.IsDeleted == false).ToList();
            decimal dataprice = 0;
            if (audienceSegmentTargetings != null)
            {
                foreach (var audienceSegmentTargeting in audienceSegmentTargetings)
                {
                    if (audienceSegmentTargeting != null && !string.IsNullOrEmpty(audienceSegmentTargeting.RulesJson))
                    {

                        //string jsonObj = audienceSegmentTargeting.GetRulesJsonForGroup(audienceSegmentTargeting.RulesJson);
                        if (audienceSegmentTargeting.DataBid.HasValue)
                            dataprice = dataprice + (audienceSegmentTargeting.DataBid.Value * 1000);
                        else if(audienceSegmentTargeting.MaxDataBid.HasValue)
                            dataprice = dataprice + (audienceSegmentTargeting.MaxDataBid.Value * 1000);
                    }
                }
            }

            return dataprice.ToString("F2");

        }
        public void FixDeviceTargetingTree(ref TargetingSaveDto targetingSaveDto)
        {

            targetingSaveDto.Models = targetingSaveDto.Models.Except(targetingSaveDto.Models).ToArray();
            List<int> Models = new List<int>();

            foreach (var platform in targetingSaveDto.Platforms)
            {
                PlatfromTree platFormtree = targetingSaveDto.platfromTree.Where(x => x.Id == platform.Key).SingleOrDefault();

                if (!platFormtree.IsAll)
                {
                    foreach (ManuTree manu in platFormtree.Manu)
                    {
                        targetingSaveDto.Manufacturers.Add(manu.Id);
                        if (!manu.IsAll)
                        {
                            Models.AddRange(manu.Devices);
                        }
                    }

                }
            }
            if (Models.Count > 0)
            {
                targetingSaveDto.Models = Models.ToArray();

            }
        }


        public ReturnBidDto GetMinBid(BidDto info)
        {
            //try
            //{
            var parameters = MapperHelper.Map<BidParameter>(info);
            var returnBid = bidManager.GetBid(parameters);
            ReturnBidDto returnedBidValue = MapperHelper.Map<ReturnBidDto>(returnBid);
            return returnedBidValue;

            //}
            //catch (Exception e)
            //{

            //throw e;
            //}

        }


        #region Conversion


        public AdGroupConversionEventResultDto GetAccountConversionEvents()
        {

            var AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            AdGroupConversionEventResultDto result = new AdGroupConversionEventResultDto { TotalCount = 0, Items = new List<AdGroupConversionEventDto>() };

            var AccountTrackingEvents = AccountTrackingEventRepository.Query(x => x.AccountId == AccountId && x.IsConversion == true).ToList();
            if (AccountTrackingEvents != null && AccountTrackingEvents.Count != 0)
            {
                foreach (var item in AccountTrackingEvents)
                {
                    AdGroupConversionEventDto trackingEvent = MapperHelper.Map<AdGroupConversionEventDto>(item);
                    trackingEvent.IsCustom = true; // here isCustom is missused , i used it just to fix issues on the dialog , but it will not effect the grid.
                   
                    result.Items.Add(trackingEvent);
                }
            }

            var allSystemTrackingEvent = trackingEventRepository.Query(x => x.IsConversion == true).ToList();
            foreach (var item in allSystemTrackingEvent)
            {

                if (result.Items.Where(x => x.Code == item.Code).FirstOrDefault() == null)
                {
                    var trackingEvent = new AdGroupConversionEventDto()
                    {
                        Code = item.Code,
                        Description = item.Name.ToString(),
                        Name= item.EventName,
                        IsCustom = false // here isCustom is missused , i used it just to fix issues on the dialog , but it will not effect the grid.
                    };

                    result.Items.Add(trackingEvent);
                }
            }




            result.TotalCount = result.Items.Count;
            return result;

        }

        #endregion

        #region Tracking Events
        public AdGroupTrackingEventResultDto GetAccountTrackingEvents()
        {

            var AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            AdGroupTrackingEventResultDto result = new AdGroupTrackingEventResultDto { TotalCount = 0, Items = new List<AdGroupTrackingEventDto>() };

            var AccountTrackingEvents = AccountTrackingEventRepository.Query(x => x.AccountId == AccountId).ToList();
            if (AccountTrackingEvents != null && AccountTrackingEvents.Count != 0)
            {
                foreach (var item in AccountTrackingEvents)
                {
                    AdGroupTrackingEventDto trackingEvent = MapperHelper.Map<AdGroupTrackingEventDto>(item);
                    trackingEvent.IsCustom = true; // here isCustom is missused , i used it just to fix issues on the dialog , but it will not effect the grid.
                    result.Items.Add(trackingEvent);
                }
            }

            var allSystemTrackingEvent = trackingEventRepository.GetAll().ToList();
            foreach (var item in allSystemTrackingEvent)
            {

                if (result.Items.Where(x => x.Code == item.Code).FirstOrDefault() == null)
                {
                    var trackingEvent = new AdGroupTrackingEventDto()
                    {
                        Id = item.ID,
                        Code = item.Code,
                        Description = item.EventName,
                        IsCustom = false // here isCustom is missused , i used it just to fix issues on the dialog , but it will not effect the grid.
                    };

                    result.Items.Add(trackingEvent);
                }
            }




            result.TotalCount = result.Items.Count;
            return result;

        }
        public AdGroupTrackingEventResultDto GetAdGroupTrackingEvents(AdGroupTrackingEventCriteriaDto criteria)
        {
            var campaign = CampaignRepository.Get(criteria.CampaignId);
            CheckCampaign(campaign);
            ValidateCampaign(campaign);

            if (campaign.IsValid)
            {
                var adgroup = campaign.AdGroups.Where(p => p.ID == criteria.AdGroupId).SingleOrDefault();
                if (adgroup != null && !adgroup.IsDeleted)
                {
                    AdGroupTrackingEventResultDto result = null;

                    if (criteria.LoadDetaultTrackingEvents)
                    {
                        result = GetDefaultAdGroupTrackingEvents(adgroup, criteria.CostModelWrapperId);
                    }
                    else
                    {
                        result = new AdGroupTrackingEventResultDto { TotalCount = 0, Items = new List<AdGroupTrackingEventDto>() };

                        var adGroupsTrackings = adgroup.TrackingEvents.Where(p => !p.IsDeleted)
                                               .OrderBy(p => p.ID).ToList();



                        foreach (var item in adGroupsTrackings)
                        {
                            AdGroupTrackingEventDto trackingEvent = MapperHelper.Map<AdGroupTrackingEventDto>(item);

                            var ss = item.PreRequisitesList.Select(p => adgroup.TrackingEvents.Where(z => z.ID == p).SingleOrDefault());
                            trackingEvent.PreRequisites = string.Join(",",
                                                            item.PreRequisitesList
                                                            .Select(p => adgroup.TrackingEvents.Where(z => z.ID == p).SingleOrDefault())
                                                            .Where(p => p != null)
                                                            .Select(p => p.Code));

                            if (string.IsNullOrEmpty(trackingEvent.Name))
                            {
                                trackingEvent.Name = trackingEvent.Description;
                            }
                            if (trackingEvent.Code != "pagein" && trackingEvent.Code != "00vimp")
                                result.Items.Add(trackingEvent);
                        }

                        result.TotalCount = adgroup.TrackingEvents.Where(M => M.Code != "pagein" && M.Code!= "00vimp"
                          ).Count();
                    }

                    return result;
                }
            }

            return null;
        }

        public void DeleteTrackingEvent(DeleteTrackingEventRequest request)
        {
            var campaign = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(campaign);
            ValidateCampaign(campaign);

            if (campaign.IsValid)
            {
                var adGroup = campaign.AdGroups.Where(p => p.ID == request.AdgroupId).SingleOrDefault();
                if (adGroup != null && !adGroup.IsDeleted)
                {
                    var trackingEvent = adGroup.TrackingEvents.Where(p => p.ID == request.AdGroupTrackingEventId && !p.IsDeleted).Single();

                    if (adGroup.TrackingEvents.Any(p => !p.IsDeleted && p.PreRequisitesList.Contains(request.AdGroupTrackingEventId)))
                    {
                        throw new BusinessException(new List<ErrorData>() { new ErrorData("DeletePrerequisite") });
                    }

                    if (trackingEvent != null)
                    {
                        trackingEvent.IsDeleted = true;
                        CampaignRepository.Save(campaign);
                    }
                }

            }
        }
        public AudicanceBillSummary DeserializeRule(string groupJson)
        {
            AudicanceBillSummary result = null;

             AudienceSegmentTargeting AudienceSegment = new AudienceSegmentTargeting(groupJson);
            var Expression = AudienceSegment.GetRulesForExpression();
            if (Expression != null)
                 result = AudienceSegment.CalculateBillInternal(Expression.Version_1);
            else
                return new AudicanceBillSummary();
            return result;
        }


        public AudicanceBillSummary DeserializeContextualRule(string groupJson)
        {
            AudicanceBillSummary result = null;

            ContextualSegmentTargeting AudienceSegment = new ContextualSegmentTargeting(groupJson);
            var Expression = AudienceSegment.GetRulesForExpression();
            if (Expression != null)
                result = AudienceSegment.CalculateBillInternal(Expression.Version_1);
            else
                return new AudicanceBillSummary();
            return result;
        }

        public IList<AdGroupTrackingEventDto> GetCostModelWrapperTrackingEvents(GetCostModelWrapperTrackingEventsRequest request)
        {
            //var campaign = CampaignRepository.Get(campaignId);
            //CheckCampaign(campaign);
            //ValidateCampaign(campaign);

            //if (campaign.IsValid)
            //{
            //    var adgroup = campaign.AdGroups.Where(p => p.ID == adGroupId).SingleOrDefault();
            //    if (adgroup != null && !adgroup.IsDeleted)
            //    {
            //        var adActionType = adgroup.Objective.AdAction;
            //        var adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapperEnum == (CostModelWrapperEnum)costModelWrapperId);


            //    }

            //}

            throw new NotImplementedException();
        }

        public ValueMessageWrapper<KeyValuePair<bool, string>> IsDeleteTrackingEventAllowed(IsDeleteTrackingEventAllowedRequest request)
        {
            var campaign = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(campaign);
            ValidateCampaign(campaign);

            AdActionTypeBase adActionType = null;
            IEnumerable<AdActionTypeTrackingEvent> adActionTrackingEvents = null;

            var result = new KeyValuePair<bool, string>(true, string.Empty);

            if (campaign.IsValid)
            {
                var adgroup = campaign.AdGroups.Where(p => p.ID == request.AdgroupId).SingleOrDefault();
                if (adgroup != null && !adgroup.IsDeleted)
                {
                    if (request.NewCostModelWrapperId.HasValue)
                    {
                        adActionType = adgroup.Objective.AdAction;
                        adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapperEnum == (CostModelWrapperEnum)request.NewCostModelWrapperId.Value);
                    }

                    if (request.AdGroupTrackingEventCodes == null || request.AdGroupTrackingEventCodes.Count == 0)
                    {
                        request.AdGroupTrackingEventCodes = adgroup.TrackingEvents.Where(p => !p.IsDeleted).Select(p => p.Code).ToList();
                    }
                    bool exitLoop = false;
                    if (request.CheckStandards)
                    {

                        foreach (var adGroupTrackingEventCode in request.AdGroupTrackingEventCodes)
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
                                        if (request.NewCostModelWrapperId.HasValue)
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
                        foreach (var adGroupTrackingEventCode in request.AdGroupTrackingEventCodes)
                        {
                            var trackingEvent = adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == adGroupTrackingEventCode).SingleOrDefault();

                            if (trackingEvent != null)
                            {
                                foreach (var item in adgroup.GetAds())
                                {
                                    if (item.AdCreativeUnits.Any(p => p.Trackers.Any(x => !x.IsDeleted && x.AdGroupEvent.ID == trackingEvent.ID)))
                                    {
                                        bool raiseError = true;
                                        if (request.NewCostModelWrapperId.HasValue)
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

            return ValueMessageWrapper.Create(result);
        }



        public ValueMessageWrapper<bool> CheckEventUniqueByCodeToSave(string code)
        {
            var accountID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            // var allSystemTrackingEvent = trackingEventRepository.GetAll();


            var eventsList = AccountTrackingEventRepository.Query(p => p.Code == code && p.AccountId != accountID).ToList();
            var eventsSystemList = trackingEventRepository.Query(p => p.Code == code).ToList();

            if ((eventsList == null || eventsList.Count() == 0 || eventsList.Count() == 1 ) && (eventsSystemList == null || eventsSystemList.Count() == 0 || eventsSystemList.Count() == 1))
            {
                return ValueMessageWrapper.Create(true);
            }

            return ValueMessageWrapper.Create(false);
        }

        public ValueMessageWrapper<bool> CheckEventUniqueByCode(string code)
        {
            var accountID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
           // var allSystemTrackingEvent = trackingEventRepository.GetAll();


            var eventsList = AccountTrackingEventRepository.Query(p => p.Code == code && p.AccountId != accountID).ToList();
            var eventsSystemList = trackingEventRepository.Query(p => p.Code == code ).ToList();

            if ((eventsList == null || eventsList.Count() == 0)&& (eventsSystemList == null || eventsSystemList.Count() == 0))
            {
                return ValueMessageWrapper.Create(true);
            }

            return ValueMessageWrapper.Create(false);
        }

        #endregion

        #endregion

        #region Ads

        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdsTree()
        {
            return GetAdsTree(new GetAdsTreeRequest { AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value, AdId = null });
        }
        public IEnumerable<Interfaces.DTOs.Core.TreeDto> GetAdsAdvTree(ValueMessageWrapper<int?> AdvertiserId)
        {
            return GetAdsAdvTree(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value, null, AdvertiserId.Value);
        }
        public IEnumerable<TreeDto> GetAdsTree(GetAdsTreeRequest request)
        {
            return GetAdsTreePre(request.AccountId, request.AdId);
        }
        private IEnumerable<TreeDto> GetAdsTreePre(int accountId, int? adId = null, int? AdvertiserId = null)
        {
            var criteria = new CampaignCriteria { AccountId = accountId };
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;
                //appCriteria.UserId = UserId;

            }
            criteria.AdvertiserAccountId = AdvertiserId;
            // criteria.AdvertiserId = AdvertiserId;
            List<Domain.Model.Campaign.Campaign> campaigns = CampaignRepository.Query(criteria.GetExpression()).ToList();
            var returnList = new List<TreeDto>();
            foreach (var item in campaigns.Where(p => p.GetGroups().Where(x => x.Ads.Where(z => z.IsDeleted == false && (!adId.HasValue || z.ID != adId.Value)).Count() != 0 && x.IsDeleted == false).Count() != 0 && p.IsDeleted == false))
            {
                if (!IsllowedAdvertiserForCamp(item))
                    continue;
                var treeDto = new TreeDto { Id = item.ID.ToString() };
                var childs = new List<TreeDto>();
                foreach (var adGroup in item.GetGroups().Where(x => x.Ads.Where(z => z.IsDeleted == false && (!adId.HasValue || z.ID != adId.Value)).Count() != 0))
                {
                    var childDto = new TreeDto
                    {
                        Id = adGroup.ID.ToString(),
                        Name = LocalizedStringDto.ConvertToLocalizedStringDto(adGroup.Name)
                    };

                    List<TreeDto> ads = new List<TreeDto>();

                    foreach (var ad in adGroup.Ads.Where(p => p.IsDeleted == false && p.Parent == null && (!adId.HasValue || p.ID != adId.Value)))
                    {
                        var adDto = new TreeDto
                        {
                            Id = ad.ID.ToString(),
                            Name = LocalizedStringDto.ConvertToLocalizedStringDto(ad.Name),
                            Key = "Ads"
                        };
                        ads.Add(adDto);
                    }
                    childDto.Childs = ads;
                    childs.Add(childDto);

                }
                treeDto.Childs = childs;
                treeDto.Name = LocalizedStringDto.ConvertToLocalizedStringDto(item.Name);
                returnList.Add(treeDto);
            }

            return returnList;
        }
        public IEnumerable<TreeDto> GetAdsAdvTree(int accountId, int? adId = null, int? AdvertiserId = null)
        {
            return GetAdsTreePre(accountId, adId, AdvertiserId);
        }

        public ValueMessageWrapper<int> GetMIMEType(string code)
        {

            var mimeType = this.mimeTypeRepository.Query(M => M.MIME == code.ToLower()).SingleOrDefault();
            if (mimeType != null)
            {
                return ValueMessageWrapper.Create(mimeType.ID);
            }
            return ValueMessageWrapper.Create(0);
        }
        public IEnumerable<AdCreativeDtoBase> GetUnApprovedAdsFromAdGroupOfAd(ValueMessageWrapper<int> adId)
        {
            IEnumerable<AdCreativeDtoBase> adCreativeDtoList = new List<AdCreativeDtoBase>();

            var item = adRepository.Get(adId.Value);

            if (item != null && !item.IsDeleted)
            {
                IEnumerable<AdCreative> adsList = item.Group.Ads.Where(p => p.ID != adId.Value && p.Parent == null && !p.IsDeleted && (p.Status.ID != AdCreativeStatus.Active.ID && p.Status.ID != AdCreativeStatus.ActiveAdServer.ID));
                adCreativeDtoList = adsList.Select(p => MapperHelper.Map<AdCreativeDtoBase>(p)).ToList();
            }

            return adCreativeDtoList;
        }

        /// <summary>
        /// use this service operation to get list of Ads Object depend on the criteria
        /// </summary>
        /// <returns>AdsSearchDto that match the criteria</returns>
        public AdsSearchDto QueryAdsByCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdsCriteria wcriteria)
        {
            AdsCriteria criteria = new AdsCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var item = CampaignRepository.Get(criteria.CampaignId);
            CheckCampaign(item);
            if (item.CampaignType != wcriteria.CampaignType && item.CampaignType != wcriteria.CampaignOtherType)
            {
                throw new DataNotFoundException();
            }
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var returnvalue = new AdsSearchDto();
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == criteria.GroupId);
                if (group != null)
                {
                    if ( !IsAllowedGroup(group) || (item.IsClientLocked && !Domain.Configuration.IsAdmin))
                    {
                        returnvalue.IsClientLocked = true;
                    }
                    if (!IsllowedAdvertiserForCamp(item) )
                    {
                        returnvalue.IsClientReadOnly = true;
                    }
                    var groupAds = item.GetGroupAds(group).OrderByDescending(x => x.ID);
                    if (groupAds != null)
                    {

                        try
                        {
                            var Permissions = OperationContext.Current.UserInfo<AdFalconUserInfo>().Permissions;
                            if (!Domain.Configuration.IsAdmin)
                                criteria.Permissions = Permissions != null ? Permissions.ToList() : new List<int>();

                            var list = groupAds.Where(criteria.GetWhere());
                            if (list != null)
                            {
                                returnvalue.Items = list.Skip((criteria.Page - 1) * criteria.Size).Take(criteria.Size).ToList().Select(ad => MapperHelper.Map<AdListDto>(ad)).ToList();
                                if (returnvalue.Items != null)
                                {
                                    foreach (var itemAdsDto in returnvalue.Items)
                                    {
                                        if (itemAdsDto.StatusId == AdCreativeStatus.ActiveAdServer.ID)
                                            itemAdsDto.Status = AdCreativeStatus.Active.GetDescription();
                                    }
                                }
                                returnvalue.TotalCount = list.Count();
                                #region Performance
                                var performance = new PerformanceCriteria()
                                {
                                    FromDate = criteria.DataFrom,
                                    ToDate = criteria.DataTo
                                };
                                performance.Ids = returnvalue.Items.Select(obj => obj.Id).ToList();
                                var performances = summaryRepository.GetAdsPerformance(performance);
                                foreach (var adListDto in returnvalue.Items)
                                {
                                    //load Ads Performance
                                    adListDto.Performance = performances.FirstOrDefault(performanceItem => performanceItem.AdsID == adListDto.Id);
                                }
                                //load Ad Group Performance
                                var adGroupPerformanceCriteria = new PerformanceCriteria()
                                {
                                    Ids = new List<int>() { criteria.GroupId }
                                };
                                var adGroupPrformance = summaryRepository.GetAdGroupPerformance(adGroupPerformanceCriteria);
                                adGroupPrformance.Objective = group.Objective.Objective.Name.ToString();
                                adGroupPrformance.Bid = group.GetReadableBid();
                                returnvalue.Performance = adGroupPrformance;
                                returnvalue.CampaignName = item.Name;
                                returnvalue.AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty;

                                returnvalue.AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0;


                                returnvalue.AdvertiserAccountName = item.AdvertiserAccount != null ? item.AdvertiserAccount.Name.ToString() : string.Empty;

                                returnvalue.AdvertiserAccountId = item.AdvertiserAccount != null ? item.AdvertiserAccount.ID : 0;
                                returnvalue.AdGroup = MapperHelper.Map<AdGroupDto>(group);
                                #endregion
                                return returnvalue;
                            }
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }

                    }
                }
            }
            return null;
        }

        /// <summary>
        /// use this service operation to get list of Ads Object that has bid less than certain value
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="bid">the bid value to check</param>
        /// <returns>List of AdsSearchDto that has bid less than</returns>
        public IEnumerable<AdbIDListDto> QueryAdsLessBid(QueryAdsBidRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            if (item.IsValid)
            {
                var returnvalue = new List<AdbIDListDto>();
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        var list = groupAds.Where(ad => ad.GetReadableBid() < request.Bid && ad.Parent == null);
                        if (list != null)
                        {
                            returnvalue = list.Select(ad => MapperHelper.Map<AdbIDListDto>(ad)).ToList();
                        }
                    }
                }
                return returnvalue;
            }
            return null;
        }
        public IEnumerable<AdbIDListDto> QueryAdsMoreBid(QueryAdsBidRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            if (item.IsValid)
            {
                var returnvalue = new List<AdbIDListDto>();
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        var list = groupAds.Where(ad => ad.GetReadableBid() > request.Bid && ad.Parent == null);
                        if (list != null)
                        {
                            returnvalue = list.Select(ad => MapperHelper.Map<AdbIDListDto>(ad)).ToList();
                        }
                    }
                }
                return returnvalue;
            }
            return null;
        }
        /// <summary>
        /// use this service operation to update Bid for List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <param name="bid">new Bid value </param>
        public void SetAdsBid(SetAdsBidRequest request)
        {
            if (request.AdIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                    if (group != null)
                    {
                        var groupAds = item.GetGroupAds(group);
                        if (groupAds != null)
                        {
                            foreach (var adId in request.AdIds)
                            {
                                //get Ad  Object
                                var adObj = groupAds.FirstOrDefault(ad => ad.ID == adId);
                                if (adObj != null)
                                {
                                    item.UpdateAdBid(group, adObj, request.Bid);
                                    adObj.UpdatedbyPortal = true;
                                }
                            }
                            #region AdGroup Minumum Unit Price
                            var groupAdsafterUpdate = item.GetGroupAds(group);
                            if (groupAdsafterUpdate != null && groupAdsafterUpdate.Count > 0)
                            {

                                var orderList = groupAdsafterUpdate.OrderBy(M => M.GetBid()).ToList();
                                group.MinimumUnitPrice = orderList[0].GetBid();
                            }

                            #endregion


                        }
                    }
                }
            }
        }

        #region Get Ad Creative Helpers
        private AdCreativeDto getDefaultAdCreative(AdGroup group)
        {
            var discount = group.Campaign.GetActiveDiscount();
            var adCreativeDto = new AdCreativeDto
            {
                ViewName = group.Objective.AdAction.ViewName,
                //AdActionValue = new AdActionValueDto(),
                //AdActionRichMediaValue = new AdActionValueRichMediaDto(),
                AdActionId = group.Objective.AdAction.ID,
                TypeId = AdTypeIds.Text,
                DiscountDto = discount == null ? null : MapperHelper.Map<DiscountDto>(discount),
                DiscountedBid = group.DiscountedBid,
                Bid = group.GetReadableBid(),
                MinBid = group.GetReadableBid(),
                TileImageId = -1,
                Group = MapperHelper.Map<AdGroupDto>(group),
                EnableEventsPostback = true,
                VerifyTargetingCriteria = true,
                VerifyDailyBudget = true,
                VerifyCampaignStartAndEndDate = true,
                ValidateRequestDeviceAndLocationData = true,
                VerifyPrerequisiteEvents = true,
                UpdateEventsFrequency = true,
                VerifyEventsFrequency = true,
                UpdateTags = true,
            };

            if (group.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingIOS || group.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingAndroid)
            { adCreativeDto.TypeId = AdTypeIds.TrackingAd; }


            if (group.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingIOSForLead || group.Objective.AdAction.ID == (int)AdActionTypeIds.AdTrackingAndroidForLead || group.Objective.AdAction.ID == (int)AdActionTypeIds.AdTracking)
            { adCreativeDto.TypeId = AdTypeIds.TrackingAd; }
            adCreativeDto.Group.AdActionTypeCode = group.Objective.AdAction.Code;
            adCreativeDto.Group.BiddingStrategy = group.BiddingStrategy;
            if (group.AdGroupDynamicBiddingConfig != null && group.BiddingStrategy == BiddingStrategy.Dynamic)
            {
                if (group.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeCTR || group.AdGroupDynamicBiddingConfig.Type == BidOptimizationType.MaximizeVCVR)
                {
                    adCreativeDto.Group.BidOptimizationValue = group.AdGroupDynamicBiddingConfig.BidOptimizationValue * 100;
                }
                else
                {

                    adCreativeDto.Group.BidOptimizationValue = group.AdGroupDynamicBiddingConfig.BidOptimizationValue;
                }
                adCreativeDto.Group.MaxBidPrice = group.AdGroupDynamicBiddingConfig.MaxBidPrice * group.CostModelWrapper.Factor;

                adCreativeDto.Group.KeepBiddingAtMinimum = group.AdGroupDynamicBiddingConfig.KeepBiddingAtMinimum;

                adCreativeDto.Group.BidOptimizationType = group.AdGroupDynamicBiddingConfig.Type;

            }
            return adCreativeDto;
        }
        private AdCreativeSummaryDto getAdCreativeSummaryDto(AdCreative item)
        {
            var result = MapperHelper.Map<AdCreativeSummaryDto>(item);
            result.CreativeUnitsContent = new List<AdCreativeUnitDto>();
            result.ActionId = item.Group.Objective.AdAction.ID;
            switch (item.TypeId)
            {
                case AdTypeIds.Text:
                    {
                        var texgtCreative = (TextCreative)item;

                        var phoneImageDocument = texgtCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).SingleOrDefault();
                        var phoneCreativeUnit = MapperHelper.Map<AdCreativeUnitDto>(phoneImageDocument);


                        var phoneImpressionTracker = item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).First().GetTrackers().FirstOrDefault();
                        phoneCreativeUnit.ImpressionTrackerRedirect = phoneImpressionTracker != null ? phoneImpressionTracker.TrackingUrl : string.Empty;

                        phoneCreativeUnit.ImpressionTrackerJSRedirect = phoneImpressionTracker != null ? phoneImpressionTracker.TrackingJS : string.Empty;
                        var CreativeUnit = item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).First();
                        if (CreativeUnit.AdCreativeUnitVendorList != null)
                        {
                            //phoneCreativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            phoneCreativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            phoneCreativeUnit.CreativeVendorIds = new List<int>();
                            foreach (var itemVendor in CreativeUnit.AdCreativeUnitVendorList)
                            {
                                phoneCreativeUnit.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                phoneCreativeUnit.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }
                        result.CreativeUnitsContent.Add(phoneCreativeUnit);

                        var tabletImageDocument = texgtCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.Tablet).SingleOrDefault();
                        var tabletCreativeUnit = MapperHelper.Map<AdCreativeUnitDto>(tabletImageDocument);
                        var tabletImpressionTracker = item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet).First().GetTrackers().FirstOrDefault();
                        tabletCreativeUnit.ImpressionTrackerRedirect = tabletImpressionTracker != null ? tabletImpressionTracker.TrackingUrl : string.Empty;


                        tabletCreativeUnit.ImpressionTrackerJSRedirect = tabletImpressionTracker != null ? tabletImpressionTracker.TrackingJS : string.Empty;


                        var TableCreativeUnit = item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet).First();

                        if (TableCreativeUnit.AdCreativeUnitVendorList != null)
                        {
                            //tabletCreativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            tabletCreativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            tabletCreativeUnit.CreativeVendorIds = new List<int>();
                            foreach (var itemVendor in TableCreativeUnit.AdCreativeUnitVendorList)
                            {
                                tabletCreativeUnit.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                tabletCreativeUnit.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }
                        result.CreativeUnitsContent.Add(tabletCreativeUnit);

                        break;
                    }
                case AdTypeIds.Banner:
                    {
                        var bannerCreative = (BannerCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }
                        break;
                    }
                case AdTypeIds.TrackingAd:
                    {
                        var adTrackerCreative = (AdTrackerCreative)item;
                        //result.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Description;
                        result.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Name.Value;

                        result.EnableEventsPostback = adTrackerCreative.EnableEventsPostback;
                        result.VerifyTargetingCriteria = adTrackerCreative.VerifyTargetingCriteria;
                        result.UpdateEventsFrequency = adTrackerCreative.UpdateEventsFrequency;
                        result.VerifyDailyBudget = adTrackerCreative.VerifyDailyBudget;
                        result.VerifyCampaignStartAndEndDate = adTrackerCreative.VerifyCampaignStartAndEndDate;
                        result.ValidateRequestDeviceAndLocationData = adTrackerCreative.ValidateRequestDeviceAndLocationData;

                        result.VerifyPrerequisiteEvents = adTrackerCreative.VerifyPrerequisiteEvents;
                        result.UpdateTags = adTrackerCreative.UpdateTags;
                        result.VerifyEventsFrequency = adTrackerCreative.VerifyEventsFrequency;


                        result.ClickTrackerUrl = adTrackerCreative.ClickTrackerUrl;
                        foreach (var adCreativeUnit in adTrackerCreative.AdCreativeUnits)
                        {
                            //if (adCreativeUnit.AdCreativeUnitVendor != null)
                            //{
                            //        result.AdCreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.ID;
                            //        result.CreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.Vendor.ID;
                            //}

                            if (adCreativeUnit.AdCreativeUnitVendorList != null)
                            {
                                //result.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                result.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                result.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    result.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    result.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }

                        }


                        // result.ClickTrackerUrl = adTrackerCreative.ClickTrackerUrl;

                        /*if (adTrackerCreative.AdCreativeUnits!=null)
                        {
                            foreach (var adCreativeUnit in adTrackerCreative.AdCreativeUnits)
                            {
                                result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                            }
                        }*/

                        break;
                    }
                case AdTypeIds.NativeAd:
                    {
                        getNativeAdSummaryDto(result, item);

                    }
                    break;
                case AdTypeIds.PlainHTML:
                    {
                        var bannerCreative = (PlainHtmlCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }
                        break;
                    }
                case AdTypeIds.RichMedia:
                    {
                        var bannerCreative = (RichMediaCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }

                        if (bannerCreative.ClickTags != null && bannerCreative.ClickTags.Count > 0)
                        {

                            result.ClickTags = new List<ClickTagTrackerDto>();
                            foreach (var tracker in bannerCreative.ClickTags)
                            {
                                result.ClickTags.Add(MapperHelper.Map<ClickTagTrackerDto>(tracker));

                            }
                        }
                        result.IsMandatory = bannerCreative.GetisMandRichMediaProtocol();
                        break;
                    }
                case AdTypeIds.InStreamVideo:
                    {
                        var instreamVideoCreative = (InStreamVideoCreative)item;

                        //  result.InStreamVideoCreativeUnits = new List<InStreamVideoCreativeUnitDto>();
                        foreach (AdCreativeUnit adCreativeUnit in instreamVideoCreative.AdCreativeUnits)
                        {
                            
                               var adCreativeUnitDto = MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit);
                            var Protocolm = adCreativeUnit.Protocol;

                            adCreativeUnitDto.InStreamVideoCreativeUnit = MapperHelper.Map<InStreamVideoCreativeUnitDto>(adCreativeUnit.InStreamVideoCreativeUnit);
                            adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration = instreamVideoCreative.DurationInSeconds;
                     
                            result.IsVpaid = instreamVideoCreative.AdCustomParameters != null && instreamVideoCreative.AdCustomParameters.Where(x => !x.IsDeleted).Count() > 0;
                            result.Vpaid_1 = result.IsVpaid && instreamVideoCreative.AdCustomParameters.Where(x => x.Name == "vpaid1" && !x.IsDeleted).Count() > 0;
                            result.Vpaid_2 = result.IsVpaid && !result.Vpaid_1 && instreamVideoCreative.AdCustomParameters.Where(x => x.Name == "vpaid2" && !x.IsDeleted).Count() > 0;
                            adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid = result.IsVpaid;
                            adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_1 = result.Vpaid_1;
                            adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_2 = result.Vpaid_2;
                            
                            if (Protocolm != null)
                            {
                                if (Protocolm.Code == (int)VASTProtocolsVersion.VAST2)
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.VASTProtocol = VASTProtocolsVersion.VAST2;
                                else if (Protocolm.Code == (int)VASTProtocolsVersion.VAST3)
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.VASTProtocol = VASTProtocolsVersion.VAST3;
                                else if (Protocolm.Code == (int)VASTProtocolsVersion.VAST4)
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.VASTProtocol = VASTProtocolsVersion.VAST4;

                                else if (Protocolm.Code == (int)VASTProtocolsVersion.VAST41)
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.VASTProtocol = VASTProtocolsVersion.VAST41;

                                else if (Protocolm.Code == (int)VASTProtocolsVersion.VAST42)
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.VASTProtocol = VASTProtocolsVersion.VAST42;
                            }

                            adCreativeUnitDto.InStreamVideoCreativeUnit.IsVideo = !instreamVideoCreative.IsXml;
                            result.IsXML = instreamVideoCreative.IsXml;
                            result.IsVideo = !instreamVideoCreative.IsXml;
                            if (instreamVideoCreative.IsXml)
                            {
                                if (adCreativeUnit.Document == null)
                                {
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl = adCreativeUnit.Content;
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl = true;
                                }
                                else if (adCreativeUnit.Document != null)
                                {

                                    adCreativeUnitDto.InStreamVideoCreativeUnit.Xml = Encoding.Default.GetString(adCreativeUnit.Document.ReadContent());

                                }

                            }
                            result.CreativeUnitsContent.Add(adCreativeUnitDto);


                            if (adCreativeUnit.AdCreativeUnitVendorList != null && adCreativeUnit.AdCreativeUnitVendorList.Count > 0)
                            {
                                adCreativeUnitDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                adCreativeUnitDto.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    adCreativeUnitDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    adCreativeUnitDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }

                            if (adCreativeUnit.Trackers != null && adCreativeUnit.Trackers.Count > 0)
                            {
                                var impress = adCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code == IMPRESSIONEVENT).ToList();

                                if (impress != null)
                                {
                                    result.ImpressionTrackingURL = new List<string>();
                                    result.ImpressionTrackingJS = new List<string>();
                                    foreach (var tracker in impress)
                                    {
                                        if (!string.IsNullOrEmpty(tracker.TrackingUrl))
                                            result.ImpressionTrackingURL.Add(tracker.TrackingUrl);
                                        if(!string.IsNullOrEmpty(tracker.TrackingJS))
                                        result.ImpressionTrackingJS.Add(tracker.TrackingJS);
                                    }
                                }
                            }
                        }
                        result.Description = instreamVideoCreative.Description;
                        result.ImageUrls = new List<CreativeUnitDto>();
                        result.VideoEndCardAdImages = new List<AdCreativeUnitDto>();


                        if (instreamVideoCreative.VideoEndCards != null && instreamVideoCreative.VideoEndCards.Count() > 0)
                        {
                            result.VideoEndCards = new List<AdCreativeSummaryDtoBase>();
                            foreach (VideoEndCardCreative videoEndCardCreative in instreamVideoCreative.VideoEndCards)
                            {

                                var resultVideoEndCard = MapperHelper.Map<AdCreativeSummaryDtoBase>(videoEndCardCreative);

                                if (videoEndCardCreative.ActionValue != null)
                                {
                                    resultVideoEndCard.ActionValue = new AdActionValueDto
                                    {
                                        Value = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value,
                                        Value2 = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value2,

                                        Trackers = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Trackers.Where(p => !p.IsDeleted).Select(p => MapperHelper.Map<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto>(p)).ToList()
                                    };

                                }


                                result.VideoEndCards.Add(resultVideoEndCard);
                                resultVideoEndCard.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                                foreach (var adCreativeUnit in videoEndCardCreative.AdCreativeUnits)
                                {
                                    resultVideoEndCard.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                                }
                            }
                            result.VideoEndCardFluid = instreamVideoCreative.VideoEndCardFluid;
                            result.VideoEndCardFluidURL = instreamVideoCreative.VideoEndCards[0].AdCreativeUnits[0].Content;
                            result.CardType = instreamVideoCreative.VideoEndCards[0].CardType;
                            result.EnableAutoClose = instreamVideoCreative.VideoEndCards[0].EnableAutoClose;
                            if (instreamVideoCreative.VideoEndCards[0].AutoCloseWaitInSeconds.HasValue)
                                result.AutoCloseWaitInSeconds = instreamVideoCreative.VideoEndCards[0].AutoCloseWaitInSeconds.Value;
                            else
                                result.AutoCloseWaitInSeconds = 0;
                            result.AdActionValueVideoEndCard = new AdActionValueDto { Trackers = new List<AdActionValueTrackerDto>(), Value = instreamVideoCreative.VideoEndCards[0].ActionValue != null ? instreamVideoCreative.VideoEndCards[0].ActionValue.Value : "" };

                            foreach (var adCreativeUnit in instreamVideoCreative.VideoEndCards[0].AdCreativeUnits)
                            {

                                if (adCreativeUnit.Trackers != null && adCreativeUnit.Trackers.Count > 0)
                                {
                                    foreach (var tracker in adCreativeUnit.Trackers)
                                    {
                                        if (!string.IsNullOrEmpty(tracker.TrackingUrl))

                                            result.AdActionValueVideoEndCard.Trackers.Add(new AdActionValueTrackerDto { URL = tracker.TrackingUrl });

                                    }
                                }
                            }


                            GetVideoEndCardSummary(result);
                        }


                        if (instreamVideoCreative.ClickTags != null && instreamVideoCreative.ClickTags.Count > 0)
                        {

                            result.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                            foreach (var tracker in instreamVideoCreative.ThirdPartyTrackers)
                            {
                                result.ThirdPartyTrackers.Add(MapperHelper.Map<ThirdPartyTrackerDto>(tracker));

                            }
                        }

                        break;
                    }


            }

            //if (result.CreativeUnitsContent != null)
            //{
            //    var creativeUnit = result.CreativeUnitsContent.FirstOrDefault();
            //    if (creativeUnit != null && creativeUnit.AdCreativeVendorId.HasValue)
            //    {
            //        result.CreativeVendorId = _AdCreativeUnitVendorRepository.Get(creativeUnit.AdCreativeVendorId.Value).Vendor.ID;
            //    }
            //}

            if (result.CreativeUnitsContent != null)
            {
                var creativeUnit = result.CreativeUnitsContent.FirstOrDefault();
                if (creativeUnit != null && creativeUnit.AdCreativeVendorIds != null)
                {
                    //creativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                    // creativeUnit.CreativeVendorIds = new List<int>();
                    result.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                    result.CreativeVendorIds = new List<int>();
                    foreach (var itemVendor in creativeUnit.AdCreativeVendorIds)
                    {
                        result.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.VendorId, UnitId = itemVendor.UnitId, VendorText = itemVendor.VendorText });

                        result.CreativeVendorIds.Add(itemVendor.VendorId);
                    }


                }
            }
            result.Campaign = MapperHelper.Map<CampaignsSummaryDtoBase>(item.Group.Campaign);
            result.Campaign.AdvertiserAccountId = result.AdvertiserAccountId;
            result.Warnings = item.Group.Campaign.GetWarnings();

            if (result.AdCreativeVendorIds != null && result.AdCreativeVendorIds.Count > 0)
            {

                result.CreativeVendorText = string.Join(",", result.AdCreativeVendorIds.Select(X => X.VendorText).ToList());

                //_CreativeVedorRepository.Get(result.CreativeVendorIds).Name.GetValue();
                //fdfdf
            }
            result.WrapperContent = item.GetWrapperContent();

            return result;
        }


        private AdCreativeFullSummaryDto getAdCreativeFullSummaryDto(AdCreative item)
        {
            var result = MapperHelper.Map<AdCreativeFullSummaryDto>(item);
            result.CreativeUnitsContent = new List<AdCreativeUnitDto>();
            result.ActionId = item.Group.Objective.AdAction.ID;
            switch (item.TypeId)
            {
                case AdTypeIds.Text:
                    {
                        var texgtCreative = (TextCreative)item;

                        foreach (var phoneCreativeUnit in item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone))
                        {
                            var phoneTile = texgtCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).Single();

                            var creativeUnit = MapperHelper.Map<AdCreativeUnitDto>(phoneTile);
                            creativeUnit.Attributes = phoneCreativeUnit.AttributesMapping.Select(p => MapperHelper.Map<AdCreativeAttributeDto>(p.Attribute)).ToList();
                            creativeUnit.ID = phoneCreativeUnit.ID;
                            creativeUnit.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(phoneCreativeUnit.CreativeUnit);
                            creativeUnit.CreativeUnitId = phoneCreativeUnit.CreativeUnit.ID;
                            creativeUnit.Name = string.Format("{0}x{1}", phoneCreativeUnit.CreativeUnit.Width, phoneCreativeUnit.CreativeUnit.Height);
                            creativeUnit.SnapshotDocumentId = phoneCreativeUnit.SnapshotDocument == null ? new int?() : phoneCreativeUnit.SnapshotDocument.ID;
                            creativeUnit.SnapshotUrl = phoneCreativeUnit.SnapshotUrl;
                            var impressionEvent = phoneCreativeUnit.GetTrackers().FirstOrDefault();
                            creativeUnit.ImpressionTrackerRedirect = impressionEvent != null ? impressionEvent.TrackingUrl : string.Empty; ;

                            creativeUnit.ImpressionTrackerJSRedirect = impressionEvent != null ? impressionEvent.TrackingJS : string.Empty; ;
                            //if (phoneCreativeUnit.AdCreativeUnitVendor != null)
                            //{
                            //    creativeUnit.AdCreativeVendorId = phoneCreativeUnit.AdCreativeUnitVendor.ID;
                            //    creativeUnit.CreativeVendorId = phoneCreativeUnit.AdCreativeUnitVendor.Vendor.ID;
                            //}
                            if (phoneCreativeUnit.AdCreativeUnitVendorList != null)
                            {
                                creativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                creativeUnit.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in phoneCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    creativeUnit.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    creativeUnit.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }
                            result.CreativeUnitsContent.Add(creativeUnit);
                        }

                        foreach (var tableCreativeUnit in item.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet))
                        {
                            var tabletTile = texgtCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.Tablet).Single();

                            var creativeUnit = MapperHelper.Map<AdCreativeUnitDto>(tabletTile);
                            creativeUnit.ID = tableCreativeUnit.ID;
                            creativeUnit.Attributes = tableCreativeUnit.AttributesMapping.Select(p => MapperHelper.Map<AdCreativeAttributeDto>(p.Attribute)).ToList();
                            creativeUnit.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(tableCreativeUnit.CreativeUnit);
                            creativeUnit.CreativeUnitId = tableCreativeUnit.CreativeUnit.ID;
                            creativeUnit.Name = string.Format("{0}x{1}", tableCreativeUnit.CreativeUnit.Width, tableCreativeUnit.CreativeUnit.Height);
                            creativeUnit.SnapshotDocumentId = tableCreativeUnit.SnapshotDocument == null ? new int?() : tableCreativeUnit.SnapshotDocument.ID;
                            creativeUnit.SnapshotUrl = tableCreativeUnit.SnapshotUrl;
                            var impressionEvent = tableCreativeUnit.GetTrackers().FirstOrDefault();
                            creativeUnit.ImpressionTrackerRedirect = impressionEvent != null ? impressionEvent.TrackingUrl : string.Empty; ;
                            creativeUnit.ImpressionTrackerJSRedirect = impressionEvent != null ? impressionEvent.TrackingJS : string.Empty; ;
                            //if (tableCreativeUnit.AdCreativeUnitVendor != null)
                            //{
                            //    creativeUnit.AdCreativeVendorId = tableCreativeUnit.AdCreativeUnitVendor.ID;
                            //    creativeUnit.CreativeVendorId = tableCreativeUnit.AdCreativeUnitVendor.Vendor.ID;
                            //}
                            if (tableCreativeUnit.AdCreativeUnitVendorList != null)
                            {
                                creativeUnit.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                creativeUnit.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in tableCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    creativeUnit.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    creativeUnit.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }

                            result.CreativeUnitsContent.Add(creativeUnit);
                        }
                        break;
                    }
                case AdTypeIds.Banner:
                    {
                        var bannerCreative = (BannerCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            var crtiv = MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit);
                            result.CreativeUnitsContent.Add(crtiv);

                            crtiv.SnapshotDocumentId = adCreativeUnit.SnapshotDocument == null ? new int?() : adCreativeUnit.SnapshotDocument.ID;
                            crtiv.SnapshotUrl = adCreativeUnit.SnapshotUrl;
                        }
                        break;
                    }
                case AdTypeIds.TrackingAd:
                    {
                        var adTrackerCreative = (AdTrackerCreative)item;
                        if (adTrackerCreative.AppMarketingPartner != null)
                        {
                            // result.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Description;
                            result.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Name.Value;

                        }
                        result.ClickTrackerUrl = adTrackerCreative.ClickTrackerUrl;

                        result.PlatformId = adTrackerCreative.Platform?.ID;

                        result.EnableEventsPostback = adTrackerCreative.EnableEventsPostback;
                        result.VerifyTargetingCriteria = adTrackerCreative.VerifyTargetingCriteria;
                        result.UpdateEventsFrequency = adTrackerCreative.UpdateEventsFrequency;
                        result.VerifyDailyBudget = adTrackerCreative.VerifyDailyBudget;
                        result.VerifyCampaignStartAndEndDate = adTrackerCreative.VerifyCampaignStartAndEndDate;
                        result.ValidateRequestDeviceAndLocationData = adTrackerCreative.ValidateRequestDeviceAndLocationData;

                        result.UpdateTags = adTrackerCreative.UpdateTags;
                        result.VerifyEventsFrequency = adTrackerCreative.VerifyEventsFrequency;

                        foreach (AdCreativeUnit adCreativeUnit in adTrackerCreative.AdCreativeUnits)
                        {
                            if (adCreativeUnit.AdCreativeUnitVendorList != null)
                            {
                                result.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                result.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    result.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    result.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }

                            //if (adCreativeUnit.AdCreativeUnitVendor!=null)
                            //{
                            //    result.AdCreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.ID;
                            //    result.CreativeVendorId = _AdCreativeUnitVendorRepository.Get(adCreativeUnit.AdCreativeUnitVendor.ID).Vendor.ID;
                            //}
                            //result.InStreamVideoCreativeUnits.Add();
                        }
                        //if (adTrackerCreative.AdCreativeUnits!=null)
                        //{
                        //    foreach (var adCreativeUnit in adTrackerCreative.AdCreativeUnits)
                        //    {
                        //        result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        //    }
                        //}
                        break;
                    }
                case AdTypeIds.NativeAd:
                    {
                        getNativeAdSummaryDto(result, item);
                        //foreach (AdCreativeUnit adCreativeUnit in item.AdCreativeUnits)
                        //{

                        //    if (adCreativeUnit.AdCreativeUnitVendor != null)
                        //    {
                        //        result.AdCreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.ID;
                        //        result.CreativeVendorId = _AdCreativeUnitVendorRepository.Get(adCreativeUnit.AdCreativeUnitVendor.ID).Vendor.ID;
                        //    }
                        //    //result.InStreamVideoCreativeUnits.Add();
                        //}
                    }
                    break;
                case AdTypeIds.PlainHTML:
                    {
                        var bannerCreative = (PlainHtmlCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }
                        break;
                    }
                case AdTypeIds.RichMedia:
                    {
                        var bannerCreative = (RichMediaCreative)item;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            result.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }

                        if (bannerCreative.ClickTags != null && bannerCreative.ClickTags.Count > 0)
                        {

                            result.ClickTags = new List<ClickTagTrackerDto>();
                            foreach (var tracker in bannerCreative.ClickTags)
                            {
                                result.ClickTags.Add(MapperHelper.Map<ClickTagTrackerDto>(tracker));

                            }
                        }
                        break;
                    }
                case AdTypeIds.InStreamVideo:
                    {
                        var instreamVideoCreative = (InStreamVideoCreative)item;
                        result.Description = instreamVideoCreative.Description;

                        result.IsVpaid = instreamVideoCreative.AdCustomParameters != null && instreamVideoCreative.AdCustomParameters.Count() > 0;
                        result.Vpaid_1 = result.IsVpaid && instreamVideoCreative.AdCustomParameters.Where(x => x.Name == "Vpaid1" && !x.IsDeleted).Count() > 0;
                        result.Vpaid_2 = result.IsVpaid && !result.Vpaid_1 && instreamVideoCreative.AdCustomParameters.Where(x => x.Name == "Vpaid2" && !x.IsDeleted).Count() > 0;
                        result.VideoEndCardAdImages = new List<AdCreativeUnitDto>();
                        result.ImageUrls = new List<CreativeUnitDto>();
                        result.VideoEndCards = new List<AdCreativeSummaryDtoBase>();

                        foreach (AdCreativeUnit adCreativeUnit in instreamVideoCreative.AdCreativeUnits)
                        {
                            var adCreativeUnitDto = MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit);
                            adCreativeUnitDto.InStreamVideoCreativeUnit = MapperHelper.Map<InStreamVideoCreativeUnitDto>(adCreativeUnit.InStreamVideoCreativeUnit);
                            adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration = instreamVideoCreative.DurationInSeconds;

                            adCreativeUnitDto.InStreamVideoCreativeUnit.IsVideo = !instreamVideoCreative.IsXml;



                            //adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid = instreamVideoCreative.Vpaid;
                            result.IsXML = instreamVideoCreative.IsXml;
                            if (instreamVideoCreative.IsXml)
                            {
                                if (adCreativeUnit.Document == null)
                                {

                                    adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl = adCreativeUnit.Content;
                                    adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl = true;
                                }
                                else if (adCreativeUnit.Document != null)
                                {

                                    adCreativeUnitDto.InStreamVideoCreativeUnit.Xml = Encoding.Default.GetString(adCreativeUnit.Document.ReadContent());

                                }

                            }
                            //if (adCreativeUnit.AdCreativeUnitVendor != null)
                            //{
                            //    adCreativeUnitDto.AdCreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.ID;
                            //    adCreativeUnitDto.CreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.Vendor.ID;
                            //}
                            if (adCreativeUnit.AdCreativeUnitVendorList != null)
                            {
                                adCreativeUnitDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                adCreativeUnitDto.CreativeVendorIds = new List<int>();
                                foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                                {
                                    adCreativeUnitDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                    adCreativeUnitDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                }
                            }
                            result.CreativeUnitsContent.Add(adCreativeUnitDto);

                            if (adCreativeUnit.Trackers != null && adCreativeUnit.Trackers.Count > 0)
                            {
                                var impress = adCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code == IMPRESSIONEVENT).ToList();

                                if (impress != null)
                                {
                                    result.ImpressionTrackingURL = new List<string>();
                                    result.ImpressionTrackingJS = new List<string>();
                                    foreach (var tracker in impress)
                                    {
                                        if (!string.IsNullOrEmpty(tracker.TrackingUrl))
                                            result.ImpressionTrackingURL.Add(tracker.TrackingUrl);
                                        if(!string.IsNullOrEmpty(tracker.TrackingJS))
                                        result.ImpressionTrackingJS.Add(tracker.TrackingJS);

                                    }
                                }
                            }
                            //result.InStreamVideoCreativeUnits.Add();
                        }

                        if (instreamVideoCreative.VideoEndCards != null)
                        {
                            foreach (VideoEndCardCreative videoEndCardCreative in instreamVideoCreative.VideoEndCards)
                            {

                                var resultVideoEndCard = MapperHelper.Map<AdCreativeSummaryDtoBase>(videoEndCardCreative);
                                if (videoEndCardCreative.ActionValue != null)
                                {
                                    resultVideoEndCard.ActionValue = new AdActionValueDto
                                    {
                                        Value = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value,
                                        Value2 = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value2,
                                        Trackers = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Trackers.Where(p => !p.IsDeleted).Select(p => MapperHelper.Map<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto>(p)).ToList()
                                    };

                                }

                                resultVideoEndCard.CardType = videoEndCardCreative.CardType;
                                resultVideoEndCard.EnableAutoClose = videoEndCardCreative.EnableAutoClose;
                                if (videoEndCardCreative.AutoCloseWaitInSeconds.HasValue)
                                    resultVideoEndCard.AutoCloseWaitInSeconds = videoEndCardCreative.AutoCloseWaitInSeconds.Value;
                                else
                                    resultVideoEndCard.AutoCloseWaitInSeconds = 0;

                                resultVideoEndCard.ImageUrls = new List<CreativeUnitDto>();
                                CreativeUnitDto CreativeUnit = MapperHelper.Map<CreativeUnitDto>(videoEndCardCreative.AdCreativeUnits.FirstOrDefault().CreativeUnit);
                                ;

                                resultVideoEndCard.ImageUrls.Add(CreativeUnit);
                                result.VideoEndCards.Add(resultVideoEndCard);
                                resultVideoEndCard.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                                foreach (var adCreativeUnit in videoEndCardCreative.AdCreativeUnits)
                                {
                                    resultVideoEndCard.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                                }
                            }
                            GetVideoEndCardSummary(result);
                        }



                        if (instreamVideoCreative.ThirdPartyTrackers != null && instreamVideoCreative.ThirdPartyTrackers.Count > 0)
                        {

                            result.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                            foreach (var tracker in instreamVideoCreative.ThirdPartyTrackers)
                            {
                                result.ThirdPartyTrackers.Add(MapperHelper.Map<ThirdPartyTrackerDto>(tracker));

                            }
                        }
                        break;
                    }
            }
            result.Campaign = MapperHelper.Map<CampaignsSummaryDtoBase>(item.Group.Campaign);
            result.Campaign.AdvertiserAccountId = result.AdvertiserAccountId;

            result.AdAccountId = item.Group.Campaign.Account.ID;

            //if (!string.IsNullOrWhiteSpace(item.DomainURL))
            //{
            //    result.DomainURL = item.DomainURL;
            //}
            //else
            //{
            //    if (item.Group.Campaign.Advertiser != null)
            //        result.DomainURL = item.Group.Campaign.Advertiser.DomainURL;
            //}

            if (item.Keyword != null)
            {
                result.Keyword = MapperHelper.Map<KeywordDto>(item.Keyword);
            }
            else
            {
                if (item.Group.Campaign.Keyword != null)
                {
                    result.Keyword = MapperHelper.Map<KeywordDto>(item.Group.Campaign.Keyword);
                }
            }
            if (item.Language != null)
            {
                result.Language = MapperHelper.Map<LanguageDto>(item.Language);
            }
            
            if (result.CreativeUnitsContent != null)
            {
                var creativeUnit = result.CreativeUnitsContent.FirstOrDefault();
                if (creativeUnit != null && creativeUnit.AdCreativeVendorIds != null)
                {
                    result.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                    result.CreativeVendorIds = new List<int>();
                    foreach (var itemVendor in creativeUnit.AdCreativeVendorIds)
                    {
                        result.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.VendorId, UnitId = itemVendor.UnitId, VendorText = itemVendor.VendorText });

                        result.CreativeVendorIds.Add(itemVendor.VendorId);
                    }


                }
            }
            //item.AppSiteAdQueues != null && item.AppSiteAdQueues.Count > 0
            result.Include = item.AppSiteAdQueues.Count == 0 || item.AppSiteAdQueues.First() == null || (item.AppSiteAdQueues.First().Include == Include.Include);
            if (item.Group.Campaign.CampaignType == CampaignType.Normal || item.Group.Campaign.CampaignType == CampaignType.ProgrammaticGuaranteed)
            {
                result.Warnings = item.Group.Campaign.GetAdminWarnings();
            }

            if (result.AdCreativeVendorIds != null && result.AdCreativeVendorIds.Count > 0)
            {

                result.CreativeVendorText = string.Join(",", result.AdCreativeVendorIds.Select(X => X.VendorText).ToList());


            }
            //if (result.CreativeVendorId > 0)
            //{
            //    result.CreativeVendorText = _CreativeVedorRepository.Get(result.CreativeVendorId).Name.GetValue();
            //    //fdfdf
            //}

            result.WrapperContent = item.GetWrapperContent();

            return result;
        }
        private void GetVideoEndCardSummary(AdCreativeSummaryDto result)
        {


            if (result.VideoEndCards != null && result.VideoEndCards.Count > 0)
            {
                var videoEndCardCreativeUnits = creativeUnitRepository.Query(item =>
                                                            item.Groups.Any(x => x.Code == "13")
                                                            ).OrderByDescending(item => item.Width).ToList();

                result.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                foreach (var videoEndCard in result.VideoEndCards)
                {
                    result.VideoEndCardCreativeUnitsContent.Add(videoEndCard.CreativeUnitsContent[0]);
                }

                result.CardType = result.VideoEndCards[0].CardType;
                if (result.CardType == VideoEndCardType.Static)
                {

                    result.IsStatic = true;
                }
                result.AutoCloseWaitInSeconds = result.VideoEndCards[0].AutoCloseWaitInSeconds;
                result.EnableAutoClose = result.VideoEndCards[0].EnableAutoClose;
                if (result.VideoEndCards[0].ActionValue != null)
                    result.AdActionValueVideoEndCardURL = result.VideoEndCards[0].ActionValue.Value;
                result.VideoEndCardsTrackingURL = result.VideoEndCards[0].VideoEndCardsTrackingURL;
                result.VideoEndCardFluidURL = result.VideoEndCards[0].CreativeUnitsContent[0].Content;
                //result.AdActionValueVideoEndCard = new AdActionValueDto { Trackers = new List<AdActionValueTrackerDto>() };

                //if (!string.IsNullOrEmpty(result.AdActionValueVideoEndCardURL))
                //{

                //    result.AdActionValueVideoEndCard.Value = result.AdActionValueVideoEndCardURL;
                //    if (result.VideoEndCardsTrackingURL != null && result.VideoEndCardsTrackingURL.Count > 0)
                //    {
                //        foreach (var URL in result.VideoEndCardsTrackingURL)
                //        {
                //            result.AdActionValueVideoEndCard.Trackers.Add(new AdActionValueTrackerDto { URL = URL });
                //        }

                //    }

                //}




                if (result.CardType == VideoEndCardType.Dynamic)
                {
                    if (result.VideoEndCardCreativeUnitsContent != null && result.VideoEndCardCreativeUnitsContent.Count > 0)
                    {


                        foreach (var cont in result.VideoEndCardCreativeUnitsContent)
                        {
                            var creativUnit = videoEndCardCreativeUnits.Where(M => M.ID == cont.CreativeUnitId).SingleOrDefault();
                            if (!string.IsNullOrEmpty(cont.Content))
                            {
                                result.ImageUrls.Add(new CreativeUnitDto { Width = creativUnit.Width, Height = creativUnit.Height, Url = cont.Content });
                            }
                        }
                    }

                }
                else
                {




                    if (result.VideoEndCardCreativeUnitsContent != null && result.VideoEndCardCreativeUnitsContent.Count > 0)
                    {
                        foreach (var creativeUnit in result.VideoEndCardCreativeUnitsContent)
                        {
                            var creativeUnitName = string.Format("CreativeUnit_{0}_{1}", "13", creativeUnit.ID.ToString());
                            var creativUnit = videoEndCardCreativeUnits.Where(M => M.ID == creativeUnit.CreativeUnitId).SingleOrDefault();
                            var adCreativeUnit2 = new AdCreativeUnitDto
                            {
                                Name = creativeUnit.Name,
                                CreativeUnitId = creativeUnit.CreativeUnitId,
                                DocumentId = creativeUnit.DocumentId,
                                CreativeUnit = new CreativeUnitDto { Width = creativUnit.Width, Height = creativUnit.Height, PreviewHeight = creativUnit.Height, PreviewWidth = creativUnit.Width }
                            };

                            result.VideoEndCardAdImages.Add(adCreativeUnit2);


                        }
                    }
                }

            }

        }
        private void getNativeAdSummaryDto(AdCreativeSummaryDto result, AdCreative item)
        {
            // This code should be revamped to use AutoMapper
            var nativeAdCreative = (NativeAdCreative)item;
            result.NativeAdIcons = new List<AdCreativeUnitDto>();
            result.NativeAdImages = new List<AdCreativeUnitDto>();
            result.ActionText = nativeAdCreative.ActionText;
            result.AppUrl = nativeAdCreative.AppOpenUrl;
            result.StarRating = nativeAdCreative.StarRating;
            result.ShowIfInstalled = nativeAdCreative.ShowIfInstalled;
            result.Description = nativeAdCreative.Description;

            AdCreativeUnitDto creativeUnitDto = new AdCreativeUnitDto();
            var adCreativeUnit = nativeAdCreative.AdCreativeUnits.Single();
            var snapShotDocument = adCreativeUnit.SnapshotDocument;
            var impressionTracker = adCreativeUnit.GetTrackers().Where(p => p.AdGroupEvent.Code == IMPRESSIONEVENT).FirstOrDefault();

            creativeUnitDto.ImpressionTrackerRedirect = impressionTracker != null ? impressionTracker.TrackingUrl : string.Empty;
            creativeUnitDto.ImpressionTrackerJSRedirect = impressionTracker != null ? impressionTracker.TrackingJS : string.Empty;
            creativeUnitDto.SnapshotDocumentId = snapShotDocument == null ? new int?() : nativeAdCreative.AdCreativeUnits.Single().SnapshotDocument.ID;
            creativeUnitDto.Attributes = adCreativeUnit.AttributesMapping.Select(p => MapperHelper.Map<AdCreativeAttributeDto>(p.Attribute)).ToList();
            creativeUnitDto.ID = adCreativeUnit.ID;

            var requiredCreative = nativeAdCreative.Images.Where(p => p.CreativeUnit.SupportedTypes.Any(z => z.AdType.ID == (int)AdTypeIds.NativeAd && z.RequiredType == RequiredType.Required)).First();

            Document requiredCreativeDocument = requiredCreative.Document;
            creativeUnitDto.Name = requiredCreativeDocument.GetNameWithNoExtension();
            creativeUnitDto.DocumentId = requiredCreativeDocument.ID;
            creativeUnitDto.CreativeUnitId = requiredCreative.CreativeUnit.ID;
            creativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(requiredCreative.CreativeUnit);

            if (adCreativeUnit.AdCreativeUnitVendorList != null)
            {
                creativeUnitDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                creativeUnitDto.CreativeVendorIds = new List<int>();
                foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                {
                    creativeUnitDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                    creativeUnitDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                }
            }




            result.CreativeUnitsContent.Add(creativeUnitDto);


            foreach (var nativeAdImage in nativeAdCreative.Images)
            {
                if (nativeAdImage.Document != null)
                {
                    AdCreativeUnitDto adCreativeUnitDto = new AdCreativeUnitDto();
                    adCreativeUnitDto.DocumentId = nativeAdImage.Document.ID;
                    adCreativeUnitDto.Content = string.Empty;
                    var creativeUnit = nativeAdImage.CreativeUnit;
                    adCreativeUnitDto.CreativeUnitId = creativeUnit.ID;
                    adCreativeUnitDto.Name = nativeAdImage.Document.GetNameWithNoExtension();

                    adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(creativeUnit);
                    result.NativeAdImages.Add(adCreativeUnitDto);
                }
            }

            foreach (var nativeAdIcon in nativeAdCreative.Icons)
            {
                if (nativeAdIcon.Document != null)
                {
                    AdCreativeUnitDto adCreativeUnitDto = new AdCreativeUnitDto();
                    adCreativeUnitDto.DocumentId = nativeAdIcon.Document.ID;
                    adCreativeUnitDto.Content = string.Empty;
                    var creativeUnit = nativeAdIcon.CreativeUnit;
                    adCreativeUnitDto.Name = nativeAdIcon.Document.GetNameWithNoExtension();


                    adCreativeUnitDto.CreativeUnitId = creativeUnit.ID;
                    adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(creativeUnit);
                    result.NativeAdIcons.Add(adCreativeUnitDto);
                }

            }
        }

        private void getNativeAd(NativeAdCreative nativeAdItem, AdCreativeSaveDto adCreative)
        {
            nativeAdItem.Description = adCreative.Description;
            nativeAdItem.ShowIfInstalled = adCreative.ShowIfInstalled;
            nativeAdItem.ActionText = adCreative.ActionText;
            nativeAdItem.AppOpenUrl = adCreative.AppUrl;
            nativeAdItem.StarRating = adCreative.StarRating;
        }


        private void getAdCreative(AdCreative item, AdCreativeSaveDto adCreative)
        {
            item.AdText = adCreative.AdText;
            item.Name = adCreative.Name;
            if (adCreative.OrientationType == OrientationType.Any)
                item.OrientationType = null;
            else
                item.OrientationType = adCreative.OrientationType;

            if (adCreative.EnvironmentType == EnvironmentType.All)
                item.EnvironmentType = null;
            else
                item.EnvironmentType = adCreative.EnvironmentType;


            item.IsSecureCompliant = adCreative.IsSecureCompliant;
            if (adCreative.AdActionValue != null)
            {
                //TODO:make sure that all ad action value require removing spaces or just url
                if (item.ActionValue == null)

                {
                    item.ActionValue = new AdActionValue();


                    item.ActionValue.ActionType = item.Group.Objective.AdAction;
                }
                item.ActionValue.Value = string.IsNullOrWhiteSpace(adCreative.AdActionValue.Value)
                                             ? adCreative.AdActionValue.Value
                                             : adCreative.AdActionValue.Value.Replace(" ", "");
                item.ActionValue.Value2 = string.IsNullOrWhiteSpace(adCreative.AdActionValue.Value2)
                                              ? adCreative.AdActionValue.Value2
                                              : adCreative.AdActionValue.Value2.Trim();
                item.ActionValue.AdCreative = item;


                if (adCreative.AdActionValue.Trackers != null)
                {
                    if (adCreative.ID == 0)
                    {
                        item.ActionValue.Trackers = new List<ArabyAds.AdFalcon.Domain.Model.Campaign.AdActionValueTracker>();
                        foreach (var tracker in adCreative.AdActionValue.Trackers)
                        {
                            if (!string.IsNullOrEmpty(tracker.URL))
                            {
                                item.ActionValue.Trackers.Add(new Domain.Model.Campaign.AdActionValueTracker() { Url = tracker.URL, AdActionValue = item.ActionValue });
                            }
                        }
                    }
                    else
                    {
                        var adTrackers = item.ActionValue.Trackers;

                        foreach (var tracker in adCreative.AdActionValue.Trackers)
                        {
                            if (!string.IsNullOrEmpty(tracker.URL))
                            {
                                if (!adTrackers.Any(p => !p.IsDeleted && p.Url == tracker.URL))
                                {
                                    adTrackers.Add(new Domain.Model.Campaign.AdActionValueTracker() { Url = tracker.URL, AdActionValue = item.ActionValue });
                                }
                            }
                        }

                        foreach (var tracker in adTrackers.Where(p => !p.IsDeleted && !adCreative.AdActionValue.Trackers.Any(z => z.URL == p.Url)))
                        {
                            tracker.IsDeleted = true;
                        }
                    }
                }

                //if ((!string.IsNullOrEmpty(item.ActionValue.Value) || !string.IsNullOrEmpty(item.ActionValue.Value2) || (item.ActionValue.Trackers != null && item.ActionValue.Trackers.Count > 0)) && adCreative.IsSecureCompliant)
                //{

                //    item.IsSecureCompliant = true;
                //}
                //else
                //{
                //    item.IsSecureCompliant = false;

                //}
            }
            else
            {

                item.ActionValue = null;
            }

            //else if (adCreative.IsSecureCompliant && (adCreative.TypeId == AdTypeIds.PlainHTML || adCreative.TypeId == AdTypeIds.RichMedia))
            //{
            //    item.IsSecureCompliant = true;
            //}
            //else
            //{
            //    item.IsSecureCompliant = false;
            //}

        }
        private AdCreativeDto getAdCreative(AdCreative adCreative)
        {
            var discount = adCreative.Group.Campaign.GetActiveDiscount();
            var adCreativeDto = new AdCreativeDto
            {
                ID = adCreative.ID,
                AdText = adCreative.AdText,
                Bid = adCreative.GetReadableBid(),
                MinBid = adCreative.Group.GetReadableBid(),
                DiscountDto = discount == null ? null : MapperHelper.Map<DiscountDto>(discount),
                DiscountedBid = adCreative.DiscountedBid,
                Name = adCreative.Name,
                ViewName = adCreative.Group.Objective.AdAction.ViewName,
                AdActionId = adCreative.Group.Objective.AdAction.ID,
                Group = MapperHelper.Map<AdGroupDto>(adCreative.Group),
                AdActionValue = new AdActionValueDto
                {
                    Value = adCreative.ActionValue == null ? null : adCreative.ActionValue.Value,
                    Value2 = adCreative.ActionValue == null ? null : adCreative.ActionValue.Value2,
                    Trackers = adCreative.ActionValue == null ? null : adCreative.ActionValue.Trackers.Where(p => !p.IsDeleted).Select(p => MapperHelper.Map<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto>(p)).ToList()
                },
                CreativeUnitsContent = new List<AdCreativeUnitDto>(),
                NativeAdIcons = new List<AdCreativeUnitDto>(),
                NativeAdImages = new List<AdCreativeUnitDto>(),
                EnvironmentType = adCreative.EnvironmentType != null ? (EnvironmentType)adCreative.EnvironmentType : EnvironmentType.All,
                OrientationType = adCreative.OrientationType != null ? (OrientationType)adCreative.OrientationType : OrientationType.Any,
                IsSecureCompliant = adCreative.IsSecureCompliant,

            };

            adCreativeDto.Group.AdActionTypeCode= adCreative.Group.Objective.AdAction.Code;
            adCreativeDto.EnableEventsPostback = adCreative.EnableEventsPostback;
            adCreativeDto.VerifyTargetingCriteria = adCreative.VerifyTargetingCriteria;
            adCreativeDto.VerifyDailyBudget = adCreative.VerifyDailyBudget;
            adCreativeDto.VerifyCampaignStartAndEndDate = adCreative.VerifyCampaignStartAndEndDate;
            adCreativeDto.ValidateRequestDeviceAndLocationData = adCreative.ValidateRequestDeviceAndLocationData;

            adCreativeDto.VerifyPrerequisiteEvents = adCreative.VerifyPrerequisiteEvents;
            adCreativeDto.UpdateEventsFrequency = adCreative.UpdateEventsFrequency;
            adCreativeDto.VerifyEventsFrequency = adCreative.VerifyEventsFrequency;
            adCreativeDto.UpdateTags = adCreative.UpdateTags;
            adCreativeDto.UniqueId = adCreative.uId;
            switch (adCreative.TypeId)
            {
                case AdTypeIds.Text:
                    {
                        var textCreative = (TextCreative)adCreative;
                        adCreativeDto.TypeId = AdTypeIds.Text;

                        // Add SmartPhone Tile
                        var tileImageDocumentPhone = textCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).SingleOrDefault();
                        var phoneCreativeUnit = textCreative.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).First();
                        var phoneImpressionTracker = phoneCreativeUnit.GetTrackers().FirstOrDefault();
                        string phoneImpressionTrackerUrl = phoneImpressionTracker != null ? phoneImpressionTracker.TrackingUrl : string.Empty;

                        string phoneImpressionTrackerJS = phoneImpressionTracker != null ? phoneImpressionTracker.TrackingJS : string.Empty;
                        AdCreativeUnitDto phoneUnitDto = MapperHelper.Map<AdCreativeUnitDto>(tileImageDocumentPhone);
                        phoneUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(phoneCreativeUnit.CreativeUnit);
                        phoneUnitDto.ImpressionTrackerRedirect = phoneImpressionTrackerUrl;
                        phoneUnitDto.ImpressionTrackerJSRedirect = phoneImpressionTrackerJS;
                        
                        /*if (phoneCreativeUnit.AdCreativeUnitVendor != null)
                        {
                            phoneUnitDto.AdCreativeVendorId = phoneCreativeUnit.AdCreativeUnitVendor.ID;
                            phoneUnitDto.CreativeVendorId = phoneCreativeUnit.AdCreativeUnitVendor.Vendor.ID;

                        }*/
                        if (phoneCreativeUnit.AdCreativeUnitVendorList != null)
                        {
                            phoneUnitDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            phoneUnitDto.CreativeVendorIds = new List<int>();
                            foreach (var itemVendor in phoneCreativeUnit.AdCreativeUnitVendorList)
                            {
                                phoneUnitDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                phoneUnitDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }

                        adCreativeDto.CreativeUnitsContent.Add(phoneUnitDto);

                        // Add Tablet Tile
                        var tileImageDocumentTablet = textCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.Tablet).SingleOrDefault();
                        var tabletCreativeUnit = textCreative.AdCreativeUnits.Where(p => p.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet).First();
                        var tabletImpressionTracker = tabletCreativeUnit.GetTrackers().FirstOrDefault();
                        string tabletImpressionTrackerUrl = tabletImpressionTracker != null ? tabletImpressionTracker.TrackingUrl : string.Empty;
                        string tabletImpressionTrackerJS = tabletImpressionTracker != null ? tabletImpressionTracker.TrackingJS : string.Empty;
                        AdCreativeUnitDto tabletUnitDto = MapperHelper.Map<AdCreativeUnitDto>(tileImageDocumentTablet);
                        tabletUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(tabletCreativeUnit.CreativeUnit);
                        tabletUnitDto.ImpressionTrackerRedirect = tabletImpressionTrackerUrl;
                        tabletUnitDto.ImpressionTrackerJSRedirect = tabletImpressionTrackerJS;
                        //if (tabletCreativeUnit.AdCreativeUnitVendor != null)
                        //{
                        //    tabletUnitDto.AdCreativeVendorId = tabletCreativeUnit.AdCreativeUnitVendor.ID;
                        //    tabletUnitDto.CreativeVendorId = tabletCreativeUnit.AdCreativeUnitVendor.Vendor.ID;

                        //}


                        if (tabletCreativeUnit.AdCreativeUnitVendorList != null)
                        {
                            tabletUnitDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            tabletUnitDto.CreativeVendorIds = new List<int>();
                            foreach (var itemVendor in tabletCreativeUnit.AdCreativeUnitVendorList)
                            {
                                tabletUnitDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                tabletUnitDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }
                        adCreativeDto.CreativeUnitsContent.Add(tabletUnitDto);


                        adCreativeDto.TileImageId = textCreative.TileImage.ID;
                        break;
                    }
                case AdTypeIds.NativeAd:
                    {
                        var nativeAdCreative = (NativeAdCreative)adCreative;
                        adCreativeDto.TypeId = AdTypeIds.NativeAd;
                        adCreativeDto.AdBannerType = adCreative.CretiveUnitDeviceType;
                        adCreativeDto.Description = nativeAdCreative.Description;
                        adCreativeDto.AppUrl = nativeAdCreative.AppOpenUrl;
                        adCreativeDto.StarRating = nativeAdCreative.StarRating;
                        adCreativeDto.ActionText = nativeAdCreative.ActionText;
                        adCreativeDto.ShowIfInstalled = nativeAdCreative.ShowIfInstalled;

                        var creativeUnit = nativeAdCreative.AdCreativeUnits.Single();


                        if (creativeUnit.GetTrackers().Where(p => p.AdGroupEvent.Code == IMPRESSIONEVENT).Count() == 1)
                        {
                            adCreativeDto.CreativeUnitsContent.Add(new AdCreativeUnitDto() { ImpressionTrackerRedirect = creativeUnit.GetTrackers().Where(p => p.AdGroupEvent.Code == IMPRESSIONEVENT).First().TrackingUrl, ImpressionTrackerJSRedirect = creativeUnit.GetTrackers().Where(p => p.AdGroupEvent.Code == IMPRESSIONEVENT).First().TrackingJS, });
                        }
                        else
                        {

                            adCreativeDto.CreativeUnitsContent.Add(new AdCreativeUnitDto()
                            {
                            });
                        }
                        var itemCreativeDto = adCreativeDto.CreativeUnitsContent.FirstOrDefault();


                        if (creativeUnit.AdCreativeUnitVendorList != null && itemCreativeDto != null)
                        {

                            itemCreativeDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            itemCreativeDto.CreativeVendorIds = new List<int>();

                            foreach (var itemVendor in creativeUnit.AdCreativeUnitVendorList)
                            {
                                itemCreativeDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                itemCreativeDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }

                        //if (itemCreativeDto != null)
                        //{
                        //    itemCreativeDto.AdCreativeVendorId = creativeUnit.AdCreativeUnitVendor.ID;
                        //    itemCreativeDto.CreativeVendorId = creativeUnit.AdCreativeUnitVendor.Vendor.ID;

                        //}
                        foreach (var item in nativeAdCreative.Images)
                        {
                            if (item.Document != null)
                            {
                                AdCreativeUnitDto adCreativeUnitDto = new AdCreativeUnitDto();
                                adCreativeUnitDto.DocumentId = item.Document.ID;
                                adCreativeUnitDto.Content = string.Empty;
                                adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(item.CreativeUnit);
                                adCreativeUnitDto.CreativeUnitId = item.CreativeUnit.ID;
                                adCreativeDto.NativeAdImages.Add(adCreativeUnitDto);
                            }
                        }

                        foreach (var item in nativeAdCreative.Icons)
                        {
                            if (item.Document != null)
                            {
                                AdCreativeUnitDto adCreativeUnitDto = new AdCreativeUnitDto();
                                adCreativeUnitDto.DocumentId = item.Document.ID;
                                adCreativeUnitDto.Content = string.Empty;
                                adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(item.CreativeUnit);
                                adCreativeUnitDto.CreativeUnitId = item.CreativeUnit.ID;
                                adCreativeDto.NativeAdIcons.Add(adCreativeUnitDto);
                            }
                        }
                    }
                    break;
                case AdTypeIds.Banner:
                    {
                        var bannerCreative = (BannerCreative)adCreative;
                        adCreativeDto.AdBannerType = adCreative.CretiveUnitDeviceType;
                        adCreativeDto.TypeId = AdTypeIds.Banner;
                        foreach (var adCreativeUnit in bannerCreative.AdCreativeUnits)
                        {
                            adCreativeDto.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }
                        break;
                    }
                case AdTypeIds.PlainHTML:
                    {
                        var plainHtmlCreative = (PlainHtmlCreative)adCreative;
                        adCreativeDto.AdBannerType = adCreative.CretiveUnitDeviceType;
                        adCreativeDto.TypeId = AdTypeIds.PlainHTML;
                        foreach (var adCreativeUnit in plainHtmlCreative.AdCreativeUnits)
                        {
                            adCreativeDto.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }
                        break;
                    }
                case AdTypeIds.TrackingAd:
                    {
                        var adTrackerCreative = (AdTrackerCreative)adCreative;
                        adCreativeDto.AdBannerType = adCreative.CretiveUnitDeviceType;
                        adCreativeDto.TypeId = AdTypeIds.TrackingAd;

                        if (adTrackerCreative.AppMarketingPartner != null)
                        {
                            adCreativeDto.AppMarketingPartnerId = adTrackerCreative.AppMarketingPartner.ID;
                            // adCreativeDto.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Description;
                            adCreativeDto.AppMarketingPartnerName = adTrackerCreative.AppMarketingPartner.Name.Value;

                        }
                        adCreativeDto.ClickTrackerUrl = adTrackerCreative.ClickTrackerUrl;

                        adCreativeDto.PlatformId = adTrackerCreative.Platform?.ID;

                        var adCreativeUnit = adTrackerCreative.AdCreativeUnits.First();

                        if (adCreativeUnit.AdCreativeUnitVendorList != null)
                        {
                            adCreativeDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            adCreativeDto.CreativeVendorIds = new List<int>();
                            foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                            {
                                adCreativeDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                adCreativeDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                            }
                        }
                        /*if (adCreativeUnit.AdCreativeUnitVendor != null)
                        {
                            adCreativeDto.AdCreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.ID;
                            adCreativeDto.CreativeVendorId = adCreativeUnit.AdCreativeUnitVendor.Vendor.ID;

                        }*/
                        //if (adTrackerCreative.AdCreativeUnits != null)
                        //{
                        //    foreach (var adCreativeUnit in adTrackerCreative.AdCreativeUnits)
                        //    {
                        //        adCreativeDto.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        //    }
                        //}
                        break;
                    }
                case AdTypeIds.RichMedia:
                    {
                        var richMediaCreative = (RichMediaCreative)adCreative;
                        adCreativeDto.AdBannerType = adCreative.CretiveUnitDeviceType;
                        adCreativeDto.TypeId = AdTypeIds.RichMedia;
                        adCreativeDto.AdSubType = adCreative.AdSubType;

                        adCreativeDto.ClickMethod = adCreative.ClickMethod;
                        adCreativeDto.RichMediaRequiredProtocol = richMediaCreative.GetRichMediaProtocol() == null
                                                                      ? null
                                                                      : MapperHelper.Map<RichMediaRequiredProtocolDto>(richMediaCreative.GetRichMediaProtocol());

                        if (adCreativeDto.RichMediaRequiredProtocol == null)
                        {

                            adCreativeDto.IsMandatory = true;
                        }
                        else
                        {

                            adCreativeDto.IsMandatory = richMediaCreative.GetisMandRichMediaProtocol();
                        }

                        foreach (var adCreativeUnit in richMediaCreative.AdCreativeUnits)
                        {
                            adCreativeDto.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));
                        }


                        if (adCreative.ClickTags != null && adCreative.ClickTags.Count > 0)
                        {

                            adCreativeDto.ClickTags = new List<ClickTagTrackerDto>();
                            foreach (var tracker in adCreative.ClickTags)
                            {
                                adCreativeDto.ClickTags.Add(MapperHelper.Map<ClickTagTrackerDto>(tracker));

                            }
                        }
                        break;
                    }

                case AdTypeIds.InStreamVideo:
                    {
                        var inStreamVideoCreative = (InStreamVideoCreative)adCreative;
                        var firstadCreativeUnit = adCreative.AdCreativeUnits.FirstOrDefault();
                        adCreativeDto.TypeId = AdTypeIds.InStreamVideo;
                        adCreativeDto.AdSubType = AdSubTypes.VideoLinear;
                        adCreativeDto.Description = inStreamVideoCreative.Description;
                        adCreativeDto.XMlUrl = firstadCreativeUnit != null && firstadCreativeUnit.Document == null ? firstadCreativeUnit.Content : "";
                        adCreativeDto.Xml = firstadCreativeUnit != null && firstadCreativeUnit.Document != null && firstadCreativeUnit.Document.Extension == ".xml" ? Encoding.Default.GetString(firstadCreativeUnit.Document.ReadContent()) : null;
                        adCreativeDto.IsXmlUrl = firstadCreativeUnit != null && firstadCreativeUnit.Document == null;
                        adCreativeDto.IsVideo = firstadCreativeUnit != null && firstadCreativeUnit.Document != null && firstadCreativeUnit.Document.Extension != ".xml";
                        adCreativeDto.IsVpaid = inStreamVideoCreative.AdCustomParameters != null && inStreamVideoCreative.AdCustomParameters.Where(x => !x.IsDeleted).Count() > 0;
                        adCreativeDto.Vpaid_1 = adCreativeDto.IsVpaid && inStreamVideoCreative.AdCustomParameters.Where(x => x.Name == "vpaid1" && !x.IsDeleted).Count() > 0;
                        adCreativeDto.Vpaid_2 = adCreativeDto.IsVpaid && !adCreativeDto.Vpaid_1 && inStreamVideoCreative.AdCustomParameters.Where(x => x.Name == "vpaid2" && !x.IsDeleted).Count() > 0;
                        if (firstadCreativeUnit.Protocol!=null)
                        {
                            if (firstadCreativeUnit.Protocol.Code ==(int) VASTProtocolsVersion.VAST2)
                                adCreativeDto.VASTProtocol = VASTProtocolsVersion.VAST2;
                            else if (firstadCreativeUnit.Protocol.Code == (int)VASTProtocolsVersion.VAST3)
                                adCreativeDto.VASTProtocol = VASTProtocolsVersion.VAST3;
                            else if (firstadCreativeUnit.Protocol.Code == (int) VASTProtocolsVersion.VAST4)
                                adCreativeDto.VASTProtocol = VASTProtocolsVersion.VAST4;
                            else if (firstadCreativeUnit.Protocol.Code == (int)VASTProtocolsVersion.VAST41)
                                adCreativeDto.VASTProtocol = VASTProtocolsVersion.VAST41;
                            else if (firstadCreativeUnit.Protocol.Code == (int)VASTProtocolsVersion.VAST42)
                                adCreativeDto.VASTProtocol = VASTProtocolsVersion.VAST42;
                        }
                     
                        adCreativeDto.VideoEndCardFluid = inStreamVideoCreative.VideoEndCardFluid;
                        if (inStreamVideoCreative != null)
                        {
                            AdCreativeUnitDto adCreativeUnitDto = null;

                            foreach (AdCreativeUnit adCreativeUnit in adCreative.AdCreativeUnits)
                            {
                                adCreativeUnitDto = MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit);
                               if(adCreativeUnit.CreativeUnit!=null)
                                adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(adCreativeUnit.CreativeUnit);
                             
                                adCreativeUnitDto.InStreamVideoCreativeUnit = MapperHelper.Map<InStreamVideoCreativeUnitDto>(adCreativeUnit.InStreamVideoCreativeUnit);
                                adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration = inStreamVideoCreative.DurationInSeconds;


                                if (adCreativeUnit.AdCreativeUnitVendorList != null)
                                {
                                    adCreativeDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                                    adCreativeDto.CreativeVendorIds = new List<int>();
                                    foreach (var itemVendor in adCreativeUnit.AdCreativeUnitVendorList)
                                    {
                                        adCreativeDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.Vendor.ID, UnitId = itemVendor.Unit.ID, VendorText = itemVendor.Vendor.Name.GetValue() });

                                        adCreativeDto.CreativeVendorIds.Add(itemVendor.Vendor.ID);
                                    }
                                }
                                adCreativeDto.CreativeUnitsContent.Add(adCreativeUnitDto);


                                if (adCreativeUnit.Trackers != null && adCreativeUnit.Trackers.Count > 0)
                                {
                                    var impress = adCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code == IMPRESSIONEVENT).ToList();

                                    if (impress != null)
                                    {
                                        adCreativeDto.ImpressionTrackingURL = new List<string>();
                                        adCreativeDto.ImpressionTrackingJS = new List<string>();
                                        foreach (var tracker in impress)
                                        {
                                            if(!string.IsNullOrEmpty(tracker.TrackingUrl))
                                            adCreativeDto.ImpressionTrackingURL.Add(tracker.TrackingUrl);
                                            if (!string.IsNullOrEmpty(tracker.TrackingJS))
                                                adCreativeDto.ImpressionTrackingJS.Add(tracker.TrackingJS);
                                        }
                                    }
                                }
                            }



                            if (inStreamVideoCreative.VideoEndCards != null)
                            {
                                adCreativeDto.VideoEndCards = new List<AdCreativeDto>();

                                //model.AdCreativeDto.AdActionValueVideoEndCardURL = model.VideoEndCards[0].AdActionValue.Value;
                                //model.AdCreativeDto.VideoEndCardsTrackingURL = model.VideoEndCards[0].VideoEndCardsTrackingURL;
                                foreach (VideoEndCardCreative videoEndCardCreative in inStreamVideoCreative.VideoEndCards)
                                {

                                    var resultVideoEndCard = MapperHelper.Map<AdCreativeDto>(videoEndCardCreative);


                                    resultVideoEndCard.CardType = videoEndCardCreative.CardType;
                                    resultVideoEndCard.EnableAutoClose = videoEndCardCreative.EnableAutoClose;
                                    if (videoEndCardCreative.AutoCloseWaitInSeconds.HasValue)
                                        resultVideoEndCard.AutoCloseWaitInSeconds = videoEndCardCreative.AutoCloseWaitInSeconds.Value;
                                    else
                                        resultVideoEndCard.AutoCloseWaitInSeconds = 0;
                                    if (videoEndCardCreative.ActionValue != null)
                                    {
                                        resultVideoEndCard.AdActionValue = new AdActionValueDto
                                        {
                                            Value = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value,
                                            Value2 = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Value2,
                                            Trackers = videoEndCardCreative.ActionValue == null ? null : videoEndCardCreative.ActionValue.Trackers.Where(p => !p.IsDeleted).Select(p => MapperHelper.Map<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative.AdActionValueTrackerDto>(p)).ToList()
                                        };

                                    }

                                    adCreativeDto.VideoEndCards.Add(resultVideoEndCard);
                                    resultVideoEndCard.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                                    foreach (var adCreativeUnit in videoEndCardCreative.AdCreativeUnits)
                                    {
                                        resultVideoEndCard.CreativeUnitsContent.Add(MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit));

                                        if (adCreativeUnit.Trackers != null && adCreativeUnit.Trackers.Count > 0)
                                        {

                                            resultVideoEndCard.VideoEndCardsTrackingURL = new List<string>();
                                            foreach (var tracker in adCreativeUnit.Trackers)
                                            {
                                                resultVideoEndCard.VideoEndCardsTrackingURL.Add(tracker.TrackingUrl);

                                            }
                                        }
                                    }
                                }
                            }


                            if (inStreamVideoCreative.ThirdPartyTrackers != null && inStreamVideoCreative.ThirdPartyTrackers.Count > 0)
                            {

                                adCreativeDto.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                                foreach (var tracker in inStreamVideoCreative.ThirdPartyTrackers)
                                {
                                    adCreativeDto.ThirdPartyTrackers.Add(MapperHelper.Map<ThirdPartyTrackerDto>(tracker));

                                }
                            }
                        }




                        break;
                    }

            }

            return adCreativeDto;
        }
        #endregion

        public bool IsAllowedGroup(AdGroup group)
        {
            return CampaignRepository.IsAllowedGroup(group);

        }
        public ValueMessageWrapper<bool> IsAllowedGroupById(ValueMessageWrapper<int> id)
        {
            return ValueMessageWrapper.Create(CampaignRepository.IsAllowedGroup(id.Value));

        }


        public bool IsAllowedAd(AdCreative ad)
        {   
            return CampaignRepository.IsAllowedAd(ad);
        }

        public ValueMessageWrapper<bool> IsAllowedAdById(ValueMessageWrapper<int> id)
        {
            return ValueMessageWrapper.Create(CampaignRepository.IsAllowedAd(id.Value));
        }

        /// <summary>
        /// use this service operation to get Ad Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreativeId">Id to Get By</param>
        /// <returns>AdCreativeDto that match the Id</returns>
        public AdCreativeDto GetAdCreative(GetAdCreativeRequest requests)
        {

            var item = CampaignRepository.Get(requests.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            bool IsClientLocked = false;

            bool IsClientReadOnly = false;
            AdCreativeDto adCreativeDto = null;
            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == requests.AdgroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        if (requests.AdCreativeId.HasValue)
                        {
                            var adCreative = groupAds.FirstOrDefault(adItem => adItem.ID == requests.AdCreativeId.Value);
                            if (adCreative == null)
                            {

                                //create default new ad
                                adCreativeDto = getDefaultAdCreative(group);
                                adCreativeDto.IsMandatory = true;
                            }
                            else
                            {
                                if (!IsAllowedAd(adCreative) || (item.IsClientLocked && !Domain.Configuration.IsAdmin))
                                //  if (!IsAllowedAd(adCreative))
                                {
                                    IsClientLocked = true;
                                }
                                if (!IsllowedAdvertiserForCamp(item))
                                //  if (!IsAllowedAd(adCreative))
                                {
                                    IsClientReadOnly = true;
                                }
                                adCreativeDto = getAdCreative(adCreative);
                            }

                            adCreativeDto.WrapperContent = adCreative.GetWrapperContent();
                        }
                        else
                        {
                            //create default new ad
                            adCreativeDto = getDefaultAdCreative(group);
                            adCreativeDto.IsMandatory = true;
                        }
                        if ( !IsAllowedGroup(group) || (item.IsClientLocked && !Domain.Configuration.IsAdmin))
                        //if (!IsAllowedGroup(group))
                        {
                            IsClientLocked = true;

                        }
                        if (!IsllowedAdvertiserForCamp(item) )
                        //if (!IsAllowedGroup(group))
                        {
                            IsClientReadOnly = true;

                        }

                        if (requests.AdType.HasValue)
                        {
                            AdType adtype = _AdTypeRepository.Get((int)requests.AdType);

                            if (!_AccountPortalPermissionsRepository.checkAdPermissions(adtype.Permission.Code))
                            {
                                IsClientReadOnly = true;
                            }
                        }

                        adCreativeDto.CampaignName = item.Name;
                        adCreativeDto.IsClientLocked = item.IsClientLocked || IsClientLocked;

                        adCreativeDto.IsClientReadOnly = IsClientReadOnly;
                        adCreativeDto.AdGroupName = group.Name;
                        var deviceTargeting = group.Targetings.OfType<DeviceTargeting>();

                        DeviceTypeTargeting deviceType = deviceTargeting.Count() == 1 ? deviceTargeting.FirstOrDefault().DeviceTypeTargetings.FirstOrDefault() : null;
                        if (deviceType != null)
                        {
                            adCreativeDto.AdGroupDeviceTypeTargeting = MapperHelper.Map<DeviceTypeDto>(deviceType.DeviceType);
                        }
                        adCreativeDto.IsAllAdsPaused = group.IsAllAdsPaused;
                        if (adCreativeDto.CreativeUnitsContent != null && adCreativeDto.CreativeUnitsContent.Count() > 0)
                        {
                            var creativeUnit = adCreativeDto.CreativeUnitsContent.FirstOrDefault();

                            adCreativeDto.AdCreativeVendorIds = new List<AdCreativeUnitVendorDto>();
                            adCreativeDto.CreativeVendorIds = new List<int>();

                            if (creativeUnit.AdCreativeVendorIds != null)
                            {
                                foreach (var itemVendor in creativeUnit.AdCreativeVendorIds)
                                {
                                    adCreativeDto.AdCreativeVendorIds.Add(new AdCreativeUnitVendorDto { ID = itemVendor.ID, VendorId = itemVendor.VendorId, UnitId = itemVendor.UnitId, VendorText = itemVendor.VendorText });

                                    adCreativeDto.CreativeVendorIds.Add(itemVendor.VendorId);
                                }
                            }
                            /*if (creativeUnit != null && creativeUnit.AdCreativeVendorId.HasValue)
                            {
                                adCreativeDto.AdCreativeVendorId = creativeUnit.AdCreativeVendorId.Value;
                                adCreativeDto.CreativeVendorId = creativeUnit.CreativeVendorId.Value;
                            }*/
                        }
                    }
                }
            }
            adCreativeDto.AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty;
            adCreativeDto.AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0;

            adCreativeDto.AdvertiserAccountName = item.AdvertiserAccount != null ? item.AdvertiserAccount.Name.ToString() : string.Empty;

            adCreativeDto.AdvertiserAccountId = item.AdvertiserAccount != null ? item.AdvertiserAccount.ID : 0;
            adCreativeDto.CampaignType = item.CampaignType;
            return adCreativeDto;
        }
        public void AddUpdateVideoMediaFile(VideoMediaFileDto dto)
        {
            var VideoFile = MapperHelper.Map<VideoMediaFile>(dto);
            var item = _VideoMediaFileRepository.Query(m => m.AdCreativeUnit.ID == dto.AdCreativeUnitId && m.OriginalCreativeUnit.ID == dto.CreativeUnitId).SingleOrDefault();
            if (item != null)
            {
                VideoFile.ID = item.ID;

               
                item.VideoType = VideoFile.VideoType;
                item.BitRate = VideoFile.BitRate;

                item.Document = VideoFile.Document;
                item.DeliveryMethod = VideoFile.DeliveryMethod;
                _VideoMediaFileRepository.Save(item);

            }

            else

                _VideoMediaFileRepository.Save(VideoFile);
        }


        public IList<VideoMediaFileDto> GetVideoMediaFiles(ValueMessageWrapper<int> videoAdId)
        {
            var mediaFiles = _VideoMediaFileRepository.Query(M => M.AdCreativeUnit.ID == videoAdId.Value).ToList();

            var listOfMediaFiles = mediaFiles.Select(q => MapperHelper.Map<VideoMediaFileDto>(q));


            return listOfMediaFiles.ToList();
        }
        /// <summary>
        /// use this service operation to Insert/Update Ad Creative Object
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="adGroupId">Ad Group Id to Get By</param>
        /// <param name="adCreative">Hold the Information that Will be Inserted/Updated</param>
        public ValueMessageWrapper<int> SaveAd(SaveAdRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            bool newAd = true;
            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);

                // This code will be changed when the new UI for adcreativeunit that supports adding events not for impression only is implemented.
                var impressionEvent = group.TrackingEvents.Where(p => p.Code.ToLower() == IMPRESSIONEVENT && !p.IsDeleted).FirstOrDefault();

                if (group != null)
                {
                    var isAdmin = Domain.Configuration.IsAdmin;
                    if (!isAdmin && item.IsClientLocked)
                    {
                        throw new CampaignLockedException();
                    }

                    if ((!isAdmin &&  !IsllowedAdvertiserForCamp(item)))
                    {
                        throw new CampaignReadOnlyException();
                    }
                    var adItem = item.GetGroupAds(group).FirstOrDefault(ad => ad.ID == request.AdCreative.ID);
                    if (adItem != null)
                    {
                        request.AdCreative.IsAdChanged = IsAdChanged(adItem, request.AdCreative);
                        if (!IsAllowedAd(adItem))
                        {
                            throw new CampaignLockedException();
                        }

                        if ((!isAdmin && !IsllowedAdvertiserForCamp(item)))
                      {
                            throw new CampaignReadOnlyException();
                        }

                    }
                    #region House Ad
                    if (group.HouseAd != null)
                    {
                        request.AdCreative.AdActionValue = new AdActionValueDto() { Value = group.HouseAd.ForAppSite.GetURL() };
                    }
                    #endregion

                    #region new Ad
                    if (adItem == null)
                    {
                        //new Ad
                        switch (request.AdCreative.TypeId)
                        {
                            case AdTypeIds.Text:
                                {
                                    adItem = new TextCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction }
                                        //,Type = AdType.Text
                                    };


                                    string phoneCreativeUnitImpressionTracker, tabletCreativeUnitImpressionTracker;
                                    phoneCreativeUnitImpressionTracker = tabletCreativeUnitImpressionTracker = null;


                                    //Custom Tile Image

                                    var textAdItem = (TextCreative)adItem;
                                    //get Tile Images
                                    if (request.AdCreative.TileImageId > 0)
                                    {
                                        //Not Custom
                                        textAdItem.TileImage = tileImageRepository.Get(request.AdCreative.TileImageId);

                                        foreach (var titlImageDoc in request.AdCreative.TileImageInformationList)
                                        {
                                            var tileImageSize =
                                                     tileImageSizeRepository.Get(
                                                         titlImageDoc.TileImage.TileImageSize.ID);

                                            if (tileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone)
                                            {
                                                phoneCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }
                                            else
                                            {
                                                tabletCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        var tileImage = new TileImage() { Images = new List<TileImageDocument>(), IsCustom = true };
                                        foreach (var titlImageDoc in request.AdCreative.TileImageInformationList)
                                        {
                                            var tileImageDocument = new TileImageDocument
                                            {
                                                TileImageSize =
                                                    tileImageSizeRepository.Get(
                                                        titlImageDoc.TileImage.TileImageSize.ID),
                                                Document =
                                                    documentRepository.Get(
                                                        titlImageDoc.TileImage.Document.ID),
                                                TileImage = tileImage
                                            };

                                            if (tileImageDocument.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone)
                                            {
                                                phoneCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }
                                            else
                                            {
                                                tabletCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }

                                            tileImageDocument.Document.UpdateUsage();
                                            tileImage.Images.Add(tileImageDocument);

                                        }
                                        textAdItem.TileImage = tileImage;
                                    }
                                    //check the Tile Images
                                    //if no document found throw DataMissingExpectation
                                    if (
                                        (textAdItem.TileImage == null) ||
                                        (textAdItem.TileImage.Images == null) ||
                                        (textAdItem.TileImage.Images.Count == 0))
                                    {
                                        var ex = new DataMissingExpectation();
                                        ex.Errors.Add(new ErrorData() { ID = "TileImageMissingBR" });
                                        throw ex;
                                    }
                                    //Add Creative Units, i added this after akram request it , in the feature we should refractor this code and remove the tile image object and move all the information to the ad creative Units list
                                    //get all text creative units

                                    getAdCreative(adItem, request.AdCreative);
                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                    var creativeUnits = adSupportedCreativeUnitRepository.GetByAdType(AdTypeIds.Text).Select(x => x.CreativeUnit).ToList();

                                    // This code will be changed when the new UI for adcreativeunit that supports adding events not for impression only is implemented.
                                    textAdItem.AddTextCreatives(creativeUnits, impressionEvent, phoneCreativeUnitImpressionTracker, tabletCreativeUnitImpressionTracker);
                                    break;
                                }
                            case AdTypeIds.Banner:
                                {
                                    adItem = new BannerCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                        CretiveUnitDeviceType = request.AdCreative.AdBannerType.Value
                                    };
                                    var bannerAdItem = (BannerCreative)adItem;
                                    //get AdCreativeUnits
                                    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    {
                                        var adCreativeUnit = new AdCreativeUnit
                                        {
                                            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                            Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                            UniqueId = Guid.NewGuid().ToString(),
                                        };
                                        adCreativeUnit.Document.UpdateUsage();
                                        adCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);

                                        adCreativeUnit.SetTrackingJS(impressionEvent, adCreativeUnitDto.ImpressionTrackerJSRedirect);
                                        bannerAdItem.AddCreativeUnit(adCreativeUnit);
                                    }
                                    getAdCreative(bannerAdItem, request.AdCreative);
                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();

                                    break;
                                }
                            case AdTypeIds.TrackingAd:
                                {
                                    adItem = new AdTrackerCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                        CretiveUnitDeviceType = request.AdCreative.AdBannerType.Value
                                    };
                                    var adTrackerAdItem = (AdTrackerCreative)adItem;
                                    if(request.AdCreative.PlatformId.HasValue)
                                    adTrackerAdItem.Platform = new Platform {ID = request.AdCreative.PlatformId.Value };

                                    //   var creativeUnites = GetCreativeUnitBy(DeviceTypeEnum.Any, AdTypeIds.TrackingAd, null, string.Empty);
                                    var creativeUnits = adSupportedCreativeUnitRepository.GetByAdType(AdTypeIds.TrackingAd).Select(x => x.CreativeUnit).ToList();
                                    string uniqueIDStr = Guid.NewGuid().ToString();
                                    var adCreativeUnit = new AdCreativeUnit
                                    {
                                        CreativeUnit = creativeUnitRepository.Get(creativeUnits.First().ID),
                                        Document = null,
                                        UniqueId = uniqueIDStr,
                                    };


                                    adTrackerAdItem.AddCreativeUnit(adCreativeUnit);

                                    //get AdCreativeUnits
                                    //if (request.AdCreative.Banners!=null)
                                    //{
                                    //    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    //    {
                                    //        var adCreativeUnit = new AdCreativeUnit
                                    //        {
                                    //            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                    //            Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                    //            UniqueId = Guid.NewGuid().ToString(),
                                    //        };
                                    //        adCreativeUnit.Document.UpdateUsage();
                                    //        adCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);
                                    //        adTrackerAdItem.AddCreativeUnit(adCreativeUnit);
                                    //    }
                                    //}

                                    getAdCreative(adTrackerAdItem, request.AdCreative);


                                    adTrackerAdItem.AppMarketingPartner = appMarketingPartnerRepository.Get(request.AdCreative.AppMarketingPartnerId);
                                    //To do we need to formate
                                    adTrackerAdItem.ClickTrackerUrl = string.Empty;

                                    adTrackerAdItem.Group = group;
                                    GenerateClickTrackerUrl(adTrackerAdItem, uniqueIDStr);
                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();

                                    break;
                                }
                            case AdTypeIds.NativeAd:
                                adItem = new NativeAdCreative
                                {
                                    ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                    CretiveUnitDeviceType = DeviceTypeEnum.Any,
                                    Icons = new List<NativeAdIcon>(),
                                    Images = new List<NativeAdImage>()
                                };
                                var nativeAdItem = (NativeAdCreative)adItem;


                                AddNativeAdImages(nativeAdItem, request.AdCreative);
                                AddNativeAdIcons(nativeAdItem, request.AdCreative);
                                getNativeAd(nativeAdItem, request.AdCreative);
                                getAdCreative(nativeAdItem, request.AdCreative);
                                adItem.AdCreativeUnits = new List<AdCreativeUnit>();

                                CreativeUnit mappingCreative = null;
                                mappingCreative = creativeUnitRepository.Query(p => p.Code == nativeAdCraetiveUnitCode).Single();

                                AdCreativeUnit mappingNativeAdCreativeUnit = new AdCreativeUnit()
                                {
                                    CreativeUnit = mappingCreative,
                                    UniqueId = Guid.NewGuid().ToString(),
                                    AdCreative = adItem,
                                    Content = adItem.AdText
                                };
                                AdCreativeUnitDto impressionTrackerCreative = null;

                                if(request.AdCreative.Banners!=null)
                                 impressionTrackerCreative = request.AdCreative.Banners.Where(p => !string.IsNullOrEmpty(p.ImpressionTrackerRedirect) || !string.IsNullOrEmpty(p.ImpressionTrackerJSRedirect)).SingleOrDefault();
                                if (impressionTrackerCreative != null)
                                {
                                    mappingNativeAdCreativeUnit.SetTrackingEvent(impressionEvent, impressionTrackerCreative.ImpressionTrackerRedirect);
                                    mappingNativeAdCreativeUnit.SetTrackingJS(impressionEvent, impressionTrackerCreative.ImpressionTrackerJSRedirect);

                                }


                                adItem.AdCreativeUnits.Add(mappingNativeAdCreativeUnit);
                                adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                break;
                            case AdTypeIds.PlainHTML:
                                {
                                    adItem = new PlainHtmlCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                        CretiveUnitDeviceType = request.AdCreative.AdBannerType.Value
                                    };
                                    var plainHtmlCreative = (PlainHtmlCreative)adItem;
                                    //get Ad Creative Unit
                                    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    {
                                        var adCreativeUnit = new AdCreativeUnit
                                        {
                                            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                            Content = adCreativeUnitDto.Content,
                                            UniqueId = Guid.NewGuid().ToString()
                                        };
                                        plainHtmlCreative.AddCreativeUnit(adCreativeUnit);
                                    }
                                    getAdCreative(plainHtmlCreative, request.AdCreative);
                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                    break;
                                }

                            case AdTypeIds.RichMedia:
                                {
                                    adItem = new RichMediaCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                        CretiveUnitDeviceType = request.AdCreative.AdBannerType.Value,
                                        AdSubType = request.AdCreative.AdSubType,

                                    };
                                    var richMediaCreative = (RichMediaCreative)adItem;
                                    if (request.AdCreative.RichMediaRequiredProtocol.HasValue)
                                    {
                                        richMediaCreative.SetRichMediaProtocol(request.AdCreative.RichMediaRequiredProtocol.Value, request.AdCreative.IsMandatory);  // request.AdCreative.RichMediaRequiredProtocol.HasValue ? requiredProtocolRepository.Get(request.AdCreative.RichMediaRequiredProtocol.Value).Name : null;
                                    }
                                    richMediaCreative.ClickMethod = request.AdCreative.ClickMethod;
                                    //get Ad Creative Unit
                                    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    {
                                        AdCreativeUnit adCreativeUnit = null;
                                        switch (request.AdCreative.AdSubType)
                                        {
                                            case AdSubTypes.ExpandableRichMedia:
                                                {
                                                    adCreativeUnit = new AdCreativeUnit
                                                    {
                                                        CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                        Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                                        UniqueId = Guid.NewGuid().ToString()
                                                    };
                                                    adCreativeUnit.Document.UpdateUsage();
                                                    richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    break;
                                                }
                                            case AdSubTypes.HTML5Interstitial:
                                            case AdSubTypes.HTML5RichMedia:
                                                {
                                                    adCreativeUnit = new AdCreativeUnit
                                                    {
                                                        CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                        Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                                        UniqueId = Guid.NewGuid().ToString()
                                                    };
                                                    adCreativeUnit.Document.UpdateUsage();
                                                    richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    break;
                                                }
                                            case AdSubTypes.JavaScriptRichMedia:
                                                {
                                                    adCreativeUnit = new AdCreativeUnit
                                                    {
                                                        CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                        Content = adCreativeUnitDto.Content,
                                                        UniqueId = Guid.NewGuid().ToString()
                                                    };
                                                    richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    break;
                                                }

                                            case AdSubTypes.ExternalUrlInterstitial:
                                                {
                                                    adCreativeUnit = new AdCreativeUnit
                                                    {
                                                        CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                        Content = adCreativeUnitDto.Content,
                                                        UniqueId = Guid.NewGuid().ToString()
                                                    };
                                                    richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    break;
                                                }

                                            case AdSubTypes.JavaScriptInterstitial:
                                                {
                                                    adCreativeUnit = new AdCreativeUnit
                                                    {
                                                        CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                        Content = adCreativeUnitDto.Content,
                                                        UniqueId = Guid.NewGuid().ToString()
                                                    };
                                                    richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    break;
                                                }
                                        }

                                    }


                                    if (request.AdCreative.ClickTags != null && request.AdCreative.ClickTags.Count > 0)
                                        SetClickTags(richMediaCreative, request.AdCreative.ClickTags.ToList());
                                    getAdCreative(richMediaCreative, request.AdCreative);
                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                    break;
                                }
                            case AdTypeIds.InStreamVideo:
                                {

                                    adItem = new InStreamVideoCreative
                                    {
                                        ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                        CretiveUnitDeviceType = DeviceTypeEnum.Any
                                    };

                                    adItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                    getAdCreative(adItem, request.AdCreative);
                                    var InStreamVideoCreative = (InStreamVideoCreative)adItem;
                                    InStreamVideoCreative.VideoEndCardFluid = request.AdCreative.VideoEndCardFluid;
                                    //   AddMediaFilesForInstream(InStreamVideoCreative, request.AdCreative, request.AdCreative.InStreamVideos[0].InStreamVideoCreativeUnit.VideoType, request.AdCreative.InStreamVideos[0].InStreamVideoCreativeUnit.DeliveryMethod);

                                    foreach (AdCreativeUnitDto adCreativeUnitDto in request.AdCreative.InStreamVideos)
                                    {
                                        var inStreamAdCreativeUnit = new InStreamVideoCreativeUnit
                                        {
                                            AdCreativeUnit = new AdCreativeUnit
                                            {
                                                CreativeUnit = creativeUnitRepository.Query(x => x.Code == "28").FirstOrDefault(),
                                                UniqueId = Guid.NewGuid().ToString()
                                            }

                                        };

                                        if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid)
                                        {
                                            if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_1)
                                            {
                                                InStreamVideoCreative.SetProtocol(Protocols.VpaId1,true);
                                            }
                                            else
                                            if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_2)
                                            {
                                                InStreamVideoCreative.SetProtocol(Protocols.VpaId2, true);
                                            }
                                        }
                                        else
                                        {
                                            InStreamVideoCreative.SetProtocol(Protocols.None, true);

                                        }

                                        if (!adCreativeUnitDto.InStreamVideoCreativeUnit.IsVideo)
                                        {

                                            if (!string.IsNullOrEmpty(adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl) && adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl)
                                            {
                                                inStreamAdCreativeUnit.AdCreativeUnit.Content = adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl;
                                                inStreamAdCreativeUnit.AdCreativeUnit.Document = null;
                                                InStreamVideoCreative.CreateOption = CreateOption.VAST;

                                            }
                                            else if (!string.IsNullOrEmpty(adCreativeUnitDto.InStreamVideoCreativeUnit.Xml) && !adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl)
                                            {
                                                byte[] Xml = Encoding.Default.GetBytes(adCreativeUnitDto.InStreamVideoCreativeUnit.Xml);
                                                var documentTpye = _DocumentTypeRepository.Query(M => M.Code == ".xml").FirstOrDefault();

                                                var xmldoc = new Document
                                                {
                                                    Name = request.AdCreative.Name,
                                                    Content = Xml,
                                                    Size = Xml.Length,
                                                    DocumentType = documentTpye,
                                                    UploadedDate = Framework.Utilities.Environment.GetServerTime(),
                                                    Extension = ".xml"
                                                };
                                                xmldoc.IsWebHDFS = true;
                                                xmldoc.Name = xmldoc.StructureTheName(xmldoc.Name + xmldoc.Extension);
                                                xmldoc.WriteContent(xmldoc.Content);



                                                documentRepository.Save(xmldoc);
                                                inStreamAdCreativeUnit.AdCreativeUnit.Document = xmldoc;
                                                InStreamVideoCreative.IsXml = true;
                                                InStreamVideoCreative.CreateOption = CreateOption.VAST;

                                            }
                                            InStreamVideoCreative.OrientationType = request.AdCreative.OrientationType != OrientationType.Any ? request.AdCreative.OrientationType : (OrientationType?)null;

                                            InStreamVideoCreative.DurationInSeconds = request.AdCreative.VideoMediaFile.duration;
                                            InStreamVideoCreative.Description = request.AdCreative.Description;
                                            inStreamAdCreativeUnit.BitRate = request.AdCreative.VideoMediaFile.bitRate;
                                            inStreamAdCreativeUnit.VideoType = mimeTypeRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.VideoType);
                                            inStreamAdCreativeUnit.Width = request.AdCreative.VideoMediaFile.width;
                                            inStreamAdCreativeUnit.Height = request.AdCreative.VideoMediaFile.height;
                                            inStreamAdCreativeUnit.AdCreativeUnit.SnapshotDocument = inStreamAdCreativeUnit.AdCreativeUnit.Document;
                                            if (inStreamAdCreativeUnit.AdCreativeUnit.SnapshotDocument != null) inStreamAdCreativeUnit.AdCreativeUnit.SnapshotDocument.UpdateUsage();

                                            AddMediaFilesForInstream(InStreamVideoCreative, request.AdCreative, request.AdCreative.InStreamVideos[0].InStreamVideoCreativeUnit.DeliveryMethod, inStreamAdCreativeUnit.AdCreativeUnit);

                                            if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST2)
                                                inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_2);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST3)
                                                inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_3);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST4)
                                                inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_4);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST41)
                                                inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST41);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST42)
                                                inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST42);

                                            if (adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.HasValue)
                                            {
                                                inStreamAdCreativeUnit.ThumbnailDoc = documentRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.Value);
                                            }

                                        }
                                        else
                                        {

                                            InStreamVideoCreative.DurationInSeconds = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration;
                                            InStreamVideoCreative.Description = request.AdCreative.Description;
                                            inStreamAdCreativeUnit.BitRate = adCreativeUnitDto.InStreamVideoCreativeUnit.BitRate;
                                            inStreamAdCreativeUnit.VideoType = mimeTypeRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.VideoType);
                                            inStreamAdCreativeUnit.Width = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoWidth;
                                            inStreamAdCreativeUnit.Height = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoHeight;
                                            inStreamAdCreativeUnit.AdCreativeUnit.Document = documentRepository.Get(adCreativeUnitDto.DocumentId.Value);

                                            inStreamAdCreativeUnit.AdCreativeUnit.Content = string.Empty;
                                            InStreamVideoCreative.IsXml = false;
                                            InStreamVideoCreative.CreateOption = CreateOption.Upload;
                                            InStreamVideoCreative.IsDraft = true;
                                            InStreamVideoCreative.OrientationType = creativeUnitRepository.Query(x => x.ID == adCreativeUnitDto.CreativeUnitId).First().OrientationType;


                                        }
                                        if (adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.HasValue)
                                        {
                                            inStreamAdCreativeUnit.ThumbnailDoc = documentRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.Value);
                                        }

                                        inStreamAdCreativeUnit.DeliveryMethod = videoDeliveryMethodRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.DeliveryMethod);
                                        inStreamAdCreativeUnit.OriginalCreativeUnit = creativeUnitRepository.Query(x => x.ID == adCreativeUnitDto.CreativeUnitId).FirstOrDefault();

                                        if (inStreamAdCreativeUnit.AdCreativeUnit.Document != null) { inStreamAdCreativeUnit.AdCreativeUnit.Document.UpdateUsage(); };
                                        inStreamAdCreativeUnit.AdCreativeUnit.InStreamVideoCreativeUnit = inStreamAdCreativeUnit;
                                        InStreamVideoCreative.AddCreativeUnit(inStreamAdCreativeUnit.AdCreativeUnit);
                                        inStreamAdCreativeUnit.AdCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);
                                        foreach (AdCreativeUnitTrackerDto unitTrackerDto in adCreativeUnitDto.InStreamVideoCreativeUnit.ImpressionTrackerRedirectList)
                                        {
                                            var eventTracker = group.TrackingEvents.Where(p => p.ID == unitTrackerDto.AdGroupEventId && !p.IsDeleted).FirstOrDefault();
                                            inStreamAdCreativeUnit.AdCreativeUnit.SetTrackingEvent(eventTracker, unitTrackerDto.ImpressionURls.Select(x => x.URL).ToList());
                                        }

                                        //inStreamAdCreativeUnit.AdCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_2);
                                        var impressionEventObj = group.TrackingEvents.Where(p => p.Code.ToLower() == IMPRESSIONEVENT && !p.IsDeleted).FirstOrDefault();
                                        if (request.AdCreative.ImpressionTrackingURL != null && request.AdCreative.ImpressionTrackingURL.Count > 0)
                                        {
                                            // foreach (var URL in request.AdCreative.VideoEndCardsTrackingURL)
                                            ////  inStreamAdCreativeUnit.AdCreativeUnit
                                            inStreamAdCreativeUnit.AdCreativeUnit.SetTrackingEvent(impressionEventObj, request.AdCreative.ImpressionTrackingURL);
                                            //}
                                        }
                                    }




                                    if (request.AdCreative.VideoEndCards != null && request.AdCreative.VideoEndCards.Count > 0)
                                    {
                                        var impressionEventobj = group.TrackingEvents.Where(p => p.Code.ToLower() == IMPRESSIONEVENT && !p.IsDeleted).FirstOrDefault();
                                        foreach (var adCreativeSave in request.AdCreative.VideoEndCards)
                                        {
                                            if (InStreamVideoCreative.VideoEndCards == null)
                                                InStreamVideoCreative.VideoEndCards = new List<VideoEndCardCreative>();
                                            var adVideoEndCardItem = new VideoEndCardCreative

                                            {
                                                ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                                CretiveUnitDeviceType = DeviceTypeEnum.Any

                                            };
                                            // var bannerAdItem = (BannerCreative)adItem;
                                            //get AdCreativeUnits

                                            adVideoEndCardItem.Parent = InStreamVideoCreative;
                                            foreach (var adCreativeUnitDto in adCreativeSave.CreativeUnitsContent)
                                            {
                                                var adCreativeUnit = new AdCreativeUnit
                                                {
                                                    CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                    //    Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                                    UniqueId = Guid.NewGuid().ToString()
                                                    // Content= adCreativeUnitDto.Content
                                                };
                                                adVideoEndCardItem.Name = adCreativeUnit.CreativeUnit.Width + "X" + adCreativeUnit.CreativeUnit.Height;
                                                adCreativeSave.Name = adCreativeUnit.CreativeUnit.Width + "X" + adCreativeUnit.CreativeUnit.Height;
                                                if (adCreativeSave.CardType == VideoEndCardType.Static)
                                                {

                                                    adCreativeUnit.Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content));


                                                }
                                                else
                                                {
                                                    adCreativeUnit.Content = adCreativeUnitDto.Content;
                                                }
                                                if (adCreativeUnit.Document != null)
                                                    adCreativeUnit.Document.UpdateUsage();

                                                if (request.AdCreative.VideoEndCardsTrackingURL != null && request.AdCreative.VideoEndCardsTrackingURL.Count > 0)
                                                {
                                                    //foreach (var URL in request.AdCreative.VideoEndCardsTrackingURL)
                                                    //{
                                                    adCreativeUnit.SetTrackingEvent(impressionEventobj, request.AdCreative.VideoEndCardsTrackingURL);
                                                    //}
                                                }
                                                adVideoEndCardItem.AddCreativeUnit(adCreativeUnit);
                                            }
                                            getAdCreative(adVideoEndCardItem, adCreativeSave);
                                            adVideoEndCardItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                            adVideoEndCardItem.uId = Guid.NewGuid().ToString();

                                            adVideoEndCardItem.CardType = adCreativeSave.CardType;
                                            adVideoEndCardItem.EnableAutoClose = adCreativeSave.EnableAutoClose;
                                            if (adVideoEndCardItem.EnableAutoClose)
                                                adVideoEndCardItem.AutoCloseWaitInSeconds = adCreativeSave.AutoCloseWaitInSeconds;
                                            else
                                                adVideoEndCardItem.AutoCloseWaitInSeconds = null;
                                            adVideoEndCardItem.Group = group;
                                            InStreamVideoCreative.VideoEndCards.Add(adVideoEndCardItem);

                                            item.SetCreativeStatus(group, adVideoEndCardItem, request.AdCreative.IsAdChanged);

                                            adVideoEndCardItem.TypeId = AdTypeIds.VideoEndCard;
                                            adVideoEndCardItem.AdSubType = AdSubTypes.VideoEndCard;
                                 
                                       

                                            adVideoEndCardItem.IsSecureCompliant = SearchForSecuredUrls(adVideoEndCardItem, adCreativeSave);
                                            adVideoEndCardItem.Type = _AdTypeRepository.Get((int)adVideoEndCardItem.TypeId);
                                            adVideoEndCardItem.TypeForPortal = adVideoEndCardItem.Type;
                                            if (adVideoEndCardItem.AdSubType.HasValue)
                                                adVideoEndCardItem.AdSubTypeForPortal = adVideoEndCardItem.AdSubType;

                                            adVideoEndCardItem.EnableEventsPostback = true;
                                            adVideoEndCardItem.VerifyTargetingCriteria = true;
                                            adVideoEndCardItem.VerifyDailyBudget = true;
                                            adVideoEndCardItem.VerifyCampaignStartAndEndDate = true;
                                            adVideoEndCardItem.ValidateRequestDeviceAndLocationData = true;

                                            adVideoEndCardItem.VerifyPrerequisiteEvents = true;
                                            adVideoEndCardItem.UpdateEventsFrequency = false;
                                            adVideoEndCardItem.VerifyEventsFrequency = false;
                                            adVideoEndCardItem.UpdateTags = true;
                                        }
                                    }

                                    if (request.AdCreative.ThirdPartyTrackers != null && request.AdCreative.ThirdPartyTrackers.Count > 0)
                                        SetThirdPartyTrackers(adItem, request.AdCreative.ThirdPartyTrackers.ToList());
                                    break;
                                }

                        }
                        if (adItem != null)
                        {

                            adItem.EnableEventsPostback = request.AdCreative.EnableEventsPostback;
                            adItem.VerifyTargetingCriteria = request.AdCreative.VerifyTargetingCriteria;

                            adItem.VerifyPrerequisiteEvents = request.AdCreative.VerifyPrerequisiteEvents;
                            adItem.VerifyDailyBudget = request.AdCreative.VerifyDailyBudget;
                            adItem.VerifyCampaignStartAndEndDate = request.AdCreative.VerifyCampaignStartAndEndDate;
                            adItem.ValidateRequestDeviceAndLocationData = request.AdCreative.ValidateRequestDeviceAndLocationData;

                            adItem.UpdateEventsFrequency = request.AdCreative.UpdateEventsFrequency;


                            adItem.UpdateTags = request.AdCreative.UpdateTags;
                            adItem.VerifyEventsFrequency = request.AdCreative.VerifyEventsFrequency;

                            foreach (var AdCreativeUnit in adItem.AdCreativeUnits)
                            {
                                AdCreativeUnit.Version = 1;
                            }
                            adItem.uId = Guid.NewGuid().ToString();
                        }
                    }
                    #endregion
                    #region Update Ad

                    else
                    {

                        adItem.EnableEventsPostback = request.AdCreative.EnableEventsPostback;
                        adItem.VerifyTargetingCriteria = request.AdCreative.VerifyTargetingCriteria;


                        adItem.VerifyDailyBudget = request.AdCreative.VerifyDailyBudget;
                        adItem.VerifyCampaignStartAndEndDate = request.AdCreative.VerifyCampaignStartAndEndDate;
                        adItem.ValidateRequestDeviceAndLocationData = request.AdCreative.ValidateRequestDeviceAndLocationData;

                        adItem.UpdateEventsFrequency = request.AdCreative.UpdateEventsFrequency;

                        adItem.VerifyPrerequisiteEvents = request.AdCreative.VerifyPrerequisiteEvents;
                        adItem.UpdateTags = request.AdCreative.UpdateTags;
                        adItem.VerifyEventsFrequency = request.AdCreative.VerifyEventsFrequency;

                        //update Old Ad
                        newAd = false;
                        switch (request.AdCreative.TypeId)
                        {
                            case AdTypeIds.Text:
                                {
                                    string phoneCreativeUnitImpressionTracker, tabletCreativeUnitImpressionTracker;
                                    phoneCreativeUnitImpressionTracker = tabletCreativeUnitImpressionTracker = null;

                                    var textAdItem = (TextCreative)adItem;

                                    //get Tile Images
                                    //Not Custom  or Old
                                    if (request.AdCreative.TileImageId > 0)
                                    {
                                        textAdItem.TileImage = tileImageRepository.Get(request.AdCreative.TileImageId);

                                        foreach (var titlImageDoc in request.AdCreative.TileImageInformationList)
                                        {
                                            var tileImageSize =
                                                     tileImageSizeRepository.Get(
                                                         titlImageDoc.TileImage.TileImageSize.ID);

                                            if (tileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone)
                                            {
                                                phoneCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }
                                            else
                                            {
                                                tabletCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                            }
                                        }
                                    }
                                    //check if it Custom Tile Image
                                    if ((textAdItem.TileImage == null) || (request.AdCreative.TileImageId < 0) || (textAdItem.TileImage.IsCustom))
                                    {
                                        //Custom Tile Image
                                        var tileImage = textAdItem.TileImage;
                                        if ((tileImage == null) || (!tileImage.IsCustom))
                                        {
                                            tileImage = new TileImage() { Images = new List<TileImageDocument>(), IsCustom = true };
                                        }
                                        foreach (var titlImageDoc in request.AdCreative.TileImageInformationList)
                                        {
                                            var tileImageDocument = tileImage.Images.FirstOrDefault(tileImageItemObj => tileImageItemObj.TileImageSize.ID == titlImageDoc.TileImage.TileImageSize.ID);
                                            if (tileImageDocument == null)
                                            {
                                                //create new tile Image Document Object
                                                tileImageDocument = new TileImageDocument
                                                {
                                                    TileImageSize = tileImageSizeRepository.Get(titlImageDoc.TileImage.TileImageSize.ID),
                                                    Document = documentRepository.Get(titlImageDoc.TileImage.Document.ID),
                                                    TileImage = tileImage
                                                };


                                                if (tileImageDocument.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone)
                                                {
                                                    phoneCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                                }
                                                else
                                                {
                                                    tabletCreativeUnitImpressionTracker = titlImageDoc.ImpressionTrackerRedirect;
                                                }


                                                tileImageDocument.Document.UpdateUsage();
                                                tileImage.Images.Add(tileImageDocument);
                                            }
                                            else
                                            {
                                                if (tileImageDocument.Document.ID != titlImageDoc.TileImage.Document.ID)
                                                {
                                                    tileImageDocument.URL = string.Empty;
                                                }
                                                //set document to not used
                                                tileImageDocument.Document.UpdateUsage(isRemove: true);

                                                tileImageDocument.Document = documentRepository.Get(titlImageDoc.TileImage.Document.ID);
                                                tileImageDocument.Document.UpdateUsage();
                                            }
                                        }
                                        textAdItem.TileImage = tileImage;
                                    }

                                    getAdCreative(adItem, request.AdCreative);
                                    //Add Creative Units, i added this after akram request it , in the feature we should refractor this code and remove the tile image object and move all the information to the ad creative Units list
                                    //get all text creative units
                                    var creativeUnits = adSupportedCreativeUnitRepository.GetByAdType(AdTypeIds.Text).Select(x => x.CreativeUnit).ToList();

                                    // This code will be changed when the new UI for adcreativeunit that supports adding events not for impression only is implemented.
                                    textAdItem.AddTextCreatives(creativeUnits, impressionEvent, phoneCreativeUnitImpressionTracker, tabletCreativeUnitImpressionTracker);
                                    foreach (var adCreativeUnit in textAdItem.AdCreativeUnits)
                                    {
                                        adCreativeUnit.KeepShapshot = false;
                                        if (adCreativeUnit.SnapshotDocument != null)
                                        {
                                            adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                            adCreativeUnit.SnapshotDocument = null;
                                        }

                                        if (adCreativeUnit.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone)
                                        {
                                            if (string.IsNullOrEmpty(phoneCreativeUnitImpressionTracker))
                                            {
                                                adCreativeUnit.RemoveTrackingEvent(impressionEvent);
                                            }
                                            else
                                            {
                                                adCreativeUnit.SetTrackingEvent(impressionEvent, phoneCreativeUnitImpressionTracker);
                                            }
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(tabletCreativeUnitImpressionTracker))
                                            {
                                                adCreativeUnit.RemoveTrackingEvent(impressionEvent);
                                            }
                                            else
                                            {
                                                adCreativeUnit.SetTrackingEvent(impressionEvent, tabletCreativeUnitImpressionTracker);
                                            }
                                        }

                                        adCreativeUnit.Content = textAdItem.AdText;
                                    }

                                    break;
                                }
                            case AdTypeIds.Banner:
                                {

                                    var bannerAdItem = (BannerCreative)adItem;
                                    bannerAdItem.SetAllBannersUnused();
                                    //get AdCreativeUnits
                                    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    {
                                        var adCreativeUnit = bannerAdItem.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);
                                        var documentId = Convert.ToInt32(adCreativeUnitDto.Content);
                                        if (adCreativeUnit == null)
                                        {
                                            adCreativeUnit = new AdCreativeUnit
                                            {
                                                CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                Document = documentRepository.Get(documentId),
                                                UniqueId = Guid.NewGuid().ToString()
                                            };

                                            adCreativeUnit.Document.UpdateUsage();
                                            bannerAdItem.AddCreativeUnit(adCreativeUnit);
                                        }
                                        else
                                        {
                                            if (adCreativeUnit.Document.ID != documentId)
                                            {
                                                adCreativeUnit.Content = string.Empty;
                                                adCreativeUnit.KeepShapshot = false;
                                                adCreativeUnit.SnapshotDocument = null;
                                                adCreativeUnit.Document.UpdateUsage(true);
                                            }

                                            adCreativeUnit.Document = documentRepository.Get(documentId);
                                            adCreativeUnit.Document.UpdateUsage();

                                            //TODO:Move this to the domain
                                            adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;
                                        };

                                        if (string.IsNullOrEmpty(adCreativeUnitDto.ImpressionTrackerRedirect) && string.IsNullOrEmpty(adCreativeUnitDto.ImpressionTrackerJSRedirect))
                                        {
                                            adCreativeUnit.RemoveTrackingEvent(impressionEvent);
                                        }
                                        else
                                        {
                                            adCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);
                                            adCreativeUnit.SetTrackingJS(impressionEvent, adCreativeUnitDto.ImpressionTrackerJSRedirect);
                                        }

                                      
                                    }

                                    //clear any unused banner
                                    bannerAdItem.ClearUnusedBanners();
                                    //clear all deleted banners after the Update
                                    var adCreativeUnits = bannerAdItem.GetCreativeUnits();
                                    foreach (var adCreativeUnit in from creativeUnit in adCreativeUnits
                                                                   where (request.AdCreative.Banners.Any(x => x.CreativeUnitId == creativeUnit.CreativeUnit.ID) == false)
                                                                   select creativeUnit)
                                    {
                                        bannerAdItem.RemoveAdCreativeUnit(adCreativeUnit.CreativeUnit.ID);
                                    }
                                    getAdCreative(bannerAdItem, request.AdCreative);
                                    break;
                                }

                            case AdTypeIds.TrackingAd:
                                {

                                    var addTrackinAdItem = (AdTrackerCreative)adItem;
                                    //addTrackinAdItem.SetAllBannersUnused();
                                    //get AdCreativeUnits
                                    //if (request.AdCreative.Banners!=null)
                                    //{
                                    //    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    //    {
                                    //        var adCreativeUnit = addTrackinAdItem.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);
                                    //        var documentId = Convert.ToInt32(adCreativeUnitDto.Content);
                                    //        if (adCreativeUnit == null)
                                    //        {
                                    //            adCreativeUnit = new AdCreativeUnit
                                    //            {
                                    //                CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                    //                Document = documentRepository.Get(documentId),
                                    //                UniqueId = Guid.NewGuid().ToString()
                                    //            };

                                    //            adCreativeUnit.Document.UpdateUsage();
                                    //            addTrackinAdItem.AddCreativeUnit(adCreativeUnit);
                                    //        }
                                    //        else
                                    //        {
                                    //            if (adCreativeUnit.Document.ID != documentId)
                                    //            {
                                    //                adCreativeUnit.Content = string.Empty;
                                    //                adCreativeUnit.KeepShapshot = false;
                                    //                adCreativeUnit.SnapshotDocument = null;
                                    //                adCreativeUnit.Document.UpdateUsage(true);
                                    //            }

                                    //            adCreativeUnit.Document = documentRepository.Get(documentId);
                                    //            adCreativeUnit.Document.UpdateUsage();

                                    //            //TODO:Move this to the domain
                                    //            adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;
                                    //        };

                                    //        if (string.IsNullOrEmpty(adCreativeUnitDto.ImpressionTrackerRedirect))
                                    //        {
                                    //            adCreativeUnit.RemoveTrackingEvent(impressionEvent);
                                    //        }
                                    //        else
                                    //        {
                                    //            adCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);
                                    //        }
                                    //    }
                                    //}

                                    ////clear any unused banner
                                    //addTrackinAdItem.ClearUnusedBanners();
                                    ////clear all deleted banners after the Update
                                    //var adCreativeUnits = addTrackinAdItem.GetCreativeUnits();
                                    //if (adCreativeUnits!=null)
                                    //{
                                    //    foreach (var adCreativeUnit in from creativeUnit in adCreativeUnits
                                    //                                   where (request.AdCreative.Banners.Any(x => x.CreativeUnitId == creativeUnit.CreativeUnit.ID) == false)
                                    //                                   select creativeUnit)
                                    //    {
                                    //        addTrackinAdItem.RemoveAdCreativeUnit(adCreativeUnit.CreativeUnit.ID);
                                    //    }
                                    //}
                                    if (request.AdCreative.PlatformId.HasValue)
                                        addTrackinAdItem.Platform = new Platform { ID = request.AdCreative.PlatformId.Value };
                                    else
                                        addTrackinAdItem.Platform = null;


                                    getAdCreative(addTrackinAdItem, request.AdCreative);

                                    addTrackinAdItem.AppMarketingPartner = appMarketingPartnerRepository.Get(request.AdCreative.AppMarketingPartnerId);
                                    //To do we need to formate
                                    //adTrackerAdItem.ClickTrackerUrl = request.AdCreative.ClickTrackerUrl;
                                    GenerateClickTrackerUrl(addTrackinAdItem, addTrackinAdItem.GetCreativeUnits()[0].UniqueId);

                                    break;
                                }
                            case AdTypeIds.NativeAd:
                                {
                                    var nativeAdItem = (NativeAdCreative)adItem;
                                    AddNativeAdImages(nativeAdItem, request.AdCreative);
                                    AddNativeAdIcons(nativeAdItem, request.AdCreative);

                                    getNativeAd(nativeAdItem, request.AdCreative);

                                    var adCreativeUnit = nativeAdItem.AdCreativeUnits.Single();
                                    adCreativeUnit.KeepShapshot = false;
                                    if (request.AdCreative!=null && request.AdCreative.Banners!=null)
                                    {
                                        var impressionTrackerCreative = request.AdCreative.Banners.Where(p => !string.IsNullOrEmpty(p.ImpressionTrackerRedirect)).SingleOrDefault();

                                        if (impressionTrackerCreative == null || (string.IsNullOrEmpty(impressionTrackerCreative.ImpressionTrackerRedirect) && string.IsNullOrEmpty(impressionTrackerCreative.ImpressionTrackerJSRedirect)))
                                        {
                                            adCreativeUnit.RemoveTrackingEvent(impressionEvent);
                                        }
                                        else
                                        {
                                            adCreativeUnit.SetTrackingEvent(impressionEvent, impressionTrackerCreative != null ? impressionTrackerCreative.ImpressionTrackerRedirect : string.Empty);

                                            adCreativeUnit.SetTrackingJS(impressionEvent, impressionTrackerCreative != null ? impressionTrackerCreative.ImpressionTrackerJSRedirect : string.Empty);
                                        }
                                    }

                                    if (adCreativeUnit.SnapshotDocument != null)
                                    {
                                        adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                        adCreativeUnit.SnapshotDocument = null;
                                    }

                                    getAdCreative(nativeAdItem, request.AdCreative);

                                }
                                break;
                            case AdTypeIds.PlainHTML:
                                {
                                    var plainHtmlCreative = (PlainHtmlCreative)adItem;
                                    //get AdCreativeUnits
                                    foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                    {
                                        var adCreativeUnit = plainHtmlCreative.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);

                                        if (adCreativeUnit == null)
                                        {
                                            adCreativeUnit = new AdCreativeUnit
                                            {
                                                CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                Content = adCreativeUnitDto.Content,
                                                UniqueId = Guid.NewGuid().ToString()

                                            };
                                            plainHtmlCreative.AddCreativeUnit(adCreativeUnit);
                                        }
                                        else
                                        {
                                            if (adCreativeUnitDto.Content != adCreativeUnit.Content)
                                            {
                                                adCreativeUnit.KeepShapshot = false;
                                                if (adCreativeUnit.SnapshotDocument != null)
                                                {
                                                    adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                                    adCreativeUnit.SnapshotDocument = null;
                                                }
                                            }

                                            adCreativeUnit.Content = adCreativeUnitDto.Content;
                                        }
                                    }
                                    //clear any unused Ad Creative
                                    plainHtmlCreative.ClearUnusedBanners();
                                    var adCreativeUnits = plainHtmlCreative.GetCreativeUnits();
                                    foreach (var adCreativeUnit in from creativeUnit in adCreativeUnits
                                                                   where (request.AdCreative.Banners.Any(x => x.CreativeUnitId == creativeUnit.CreativeUnit.ID) == false)
                                                                   select creativeUnit)
                                    {
                                        plainHtmlCreative.RemoveAdCreativeUnit(adCreativeUnit.CreativeUnit.ID);
                                    }

                                    getAdCreative(plainHtmlCreative, request.AdCreative);
                                    break;
                                }

                            #region RichMedia
                            case AdTypeIds.RichMedia:
                                {
                                    var richMediaCreative = (RichMediaCreative)adItem;
                                    if (request.AdCreative.RichMediaRequiredProtocol.HasValue)
                                    {
                                        richMediaCreative.SetRichMediaProtocol(request.AdCreative.RichMediaRequiredProtocol.Value, request.AdCreative.IsMandatory);
                                        //  richMediaCreative.RichMediaRequiredProtocol =;// : RichMediaProtocols.MRAID1; ;//requiredProtocolRepository.Get(request.AdCreative.RichMediaRequiredProtocol.Value) : null;
                                    }

                                    richMediaCreative.ClickMethod = request.AdCreative.ClickMethod;
                                    switch (request.AdCreative.AdSubType)
                                    {
                                        case AdSubTypes.ExpandableRichMedia:
                                            {
                                                //get AdCreativeUnits
                                                foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                                {
                                                    var adCreativeUnit = richMediaCreative.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);
                                                    var documentId = Convert.ToInt32(adCreativeUnitDto.Content);
                                                    if (adCreativeUnit == null)
                                                    {
                                                        adCreativeUnit = new AdCreativeUnit
                                                        {
                                                            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                            Document = documentRepository.Get(documentId),
                                                            UniqueId = Guid.NewGuid().ToString()
                                                        };
                                                        adCreativeUnit.Document.UpdateUsage();
                                                        richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    }
                                                    else
                                                    {
                                                        var isNew = false;
                                                        if (adCreativeUnit.Document.ID != documentId)
                                                        {
                                                            adCreativeUnit.Content = string.Empty;
                                                            adCreativeUnit.KeepShapshot = false;
                                                            adCreativeUnit.SnapshotDocument = null;
                                                            adCreativeUnit.Document.UpdateUsage(true);
                                                            isNew = true;
                                                        }
                                                        adCreativeUnit.Document = documentRepository.Get(documentId);
                                                        if (isNew)
                                                        {
                                                            adCreativeUnit.Document.UpdateUsage();
                                                        }
                                                        //TODO:Move this to the domain
                                                        adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;
                                                    }
                                                }
                                                break;
                                            }
                                        case AdSubTypes.HTML5Interstitial:
                                        case AdSubTypes.HTML5RichMedia:
                                            {
                                                //get AdCreativeUnits
                                                foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                                {
                                                    var adCreativeUnit = richMediaCreative.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);
                                                    var documentId = Convert.ToInt32(adCreativeUnitDto.Content);
                                                    if (adCreativeUnit == null)
                                                    {
                                                        adCreativeUnit = new AdCreativeUnit
                                                        {
                                                            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                            Document = documentRepository.Get(documentId),
                                                            UniqueId = Guid.NewGuid().ToString()
                                                        };
                                                        adCreativeUnit.Document.UpdateUsage();
                                                        richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    }
                                                    else
                                                    {
                                                        var isNew = false;
                                                        if (adCreativeUnit.Document.ID != documentId)
                                                        {
                                                            adCreativeUnit.Content = string.Empty;
                                                            adCreativeUnit.KeepShapshot = false;
                                                            adCreativeUnit.SnapshotDocument = null;
                                                            adCreativeUnit.Document.UpdateUsage(true);
                                                            isNew = true;
                                                        }
                                                        adCreativeUnit.Document = documentRepository.Get(documentId);
                                                        if (isNew)
                                                        {
                                                            adCreativeUnit.Document.UpdateUsage();
                                                        }
                                                        //TODO:Move this to the domain
                                                        adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;
                                                    }
                                                }
                                                break;
                                            }
                                        case AdSubTypes.JavaScriptInterstitial:
                                        case AdSubTypes.JavaScriptRichMedia:
                                        case AdSubTypes.ExternalUrlInterstitial:
                                            {
                                                //get AdCreativeUnits
                                                foreach (var adCreativeUnitDto in request.AdCreative.Banners)
                                                {
                                                    var adCreativeUnit = richMediaCreative.GetCreativeUnit(adCreativeUnitDto.CreativeUnitId);
                                                    if (adCreativeUnit == null)
                                                    {
                                                        adCreativeUnit = new AdCreativeUnit
                                                        {
                                                            CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                            Content = adCreativeUnitDto.Content,
                                                            UniqueId = Guid.NewGuid().ToString()
                                                        };
                                                        richMediaCreative.AddCreativeUnit(adCreativeUnit);
                                                    }
                                                    else
                                                    {
                                                        if (adCreativeUnitDto.Content != adCreativeUnit.Content)
                                                        {
                                                            adCreativeUnit.KeepShapshot = false;
                                                            if (adCreativeUnit.SnapshotDocument != null)
                                                            {
                                                                adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                                                adCreativeUnit.SnapshotDocument = null;
                                                            }
                                                        }

                                                        adCreativeUnit.Content = adCreativeUnitDto.Content;
                                                    }
                                                }
                                                //clear any unused banner
                                                richMediaCreative.ClearUnusedBanners();
                                                break;
                                            }
                                    }
                                    var adCreativeUnits = richMediaCreative.GetCreativeUnits();
                                    foreach (var adCreativeUnit in from creativeUnit in adCreativeUnits
                                                                   where (request.AdCreative.Banners.Any(x => x.CreativeUnitId == creativeUnit.CreativeUnit.ID) == false)
                                                                   select creativeUnit)
                                    {
                                        richMediaCreative.RemoveAdCreativeUnit(adCreativeUnit.CreativeUnit.ID);
                                    }

                                    if (request.AdCreative.ClickTags != null && request.AdCreative.ClickTags.Count > 0)
                                        SetClickTags(richMediaCreative, request.AdCreative.ClickTags.ToList());
                                    getAdCreative(richMediaCreative, request.AdCreative);
                                    break;
                                }

                            #endregion
                            #region InStreamVideo
                            case AdTypeIds.InStreamVideo:
                                {
                                    var inStreamVideoCreative = (InStreamVideoCreative)adItem;

                                    getAdCreative(inStreamVideoCreative, request.AdCreative);
                                    inStreamVideoCreative.Description = request.AdCreative.Description;
                                    inStreamVideoCreative.VideoEndCardFluid = request.AdCreative.VideoEndCardFluid;
                                    foreach (AdCreativeUnitDto adCreativeUnitDto in request.AdCreative.InStreamVideos)
                                    {

                                        var adCreativeUnit = inStreamVideoCreative.GetCreativeUnits().FirstOrDefault();

                                        if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid)
                                        {
                                            if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_1)
                                            {
                                                inStreamVideoCreative.SetProtocol(Protocols.VpaId1,true);
                                            }
                                            else
                                            if (adCreativeUnitDto.InStreamVideoCreativeUnit.Vpaid_2)
                                            {
                                                inStreamVideoCreative.SetProtocol(Protocols.VpaId2, true);
                                            }
                                        }
                                        else
                                        {
                                            inStreamVideoCreative.SetProtocol(Protocols.None, true);

                                        }

                                        if (!adCreativeUnitDto.InStreamVideoCreativeUnit.IsVideo)
                                        {
                                            if (!string.IsNullOrEmpty(adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl) && adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl)
                                            {

                                                adCreativeUnit.Content = adCreativeUnitDto.InStreamVideoCreativeUnit.XmlUrl;
                                                adCreativeUnit.Document = null;
                                                inStreamVideoCreative.CreateOption = CreateOption.VAST;
                                            }
                                            else if (!string.IsNullOrEmpty(adCreativeUnitDto.InStreamVideoCreativeUnit.Xml) && !adCreativeUnitDto.InStreamVideoCreativeUnit.IsXmlUrl)
                                            {
                                                var documentTpye = _DocumentTypeRepository.Query(M => M.Code == ".xml").SingleOrDefault<DocumentType>();


                                                Document xmldoc = new Document();
                                                byte[] Xml = Encoding.Default.GetBytes(adCreativeUnitDto.InStreamVideoCreativeUnit.Xml);
                                                xmldoc = new Document
                                                {
                                                    Name = request.AdCreative.Name + adCreativeUnit.Version.ToString(),// Framework.Utilities.Environment.GetServerTime().Replace('/','_'),
                                                    Content = Xml,
                                                    Size = Xml.Length,
                                                    DocumentType = documentTpye,
                                                    UploadedDate = Framework.Utilities.Environment.GetServerTime(),
                                                    Extension = ".xml"
                                                };
                                                documentRepository.Save(xmldoc);
                                                adCreativeUnit.Document = xmldoc;


                                                adCreativeUnit.Document.UpdateUsage();
                                                // inStreamVideoCreative.XmlUpload();


                                                inStreamVideoCreative.IsXml = true;


                                                inStreamVideoCreative.CreateOption = CreateOption.VAST;
                                            }

                                            adCreativeUnit.InStreamVideoCreativeUnit.DeliveryMethod = videoDeliveryMethodRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.DeliveryMethod);
                                            inStreamVideoCreative.DurationInSeconds = request.AdCreative.VideoMediaFile.duration;
                                            adCreativeUnit.InStreamVideoCreativeUnit.BitRate = request.AdCreative.VideoMediaFile.bitRate;
                                            adCreativeUnit.InStreamVideoCreativeUnit.VideoType = mimeTypeRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.VideoType);
                                            adCreativeUnit.InStreamVideoCreativeUnit.Width = request.AdCreative.VideoMediaFile.width;
                                            adCreativeUnit.InStreamVideoCreativeUnit.Height = request.AdCreative.VideoMediaFile.height;
                                            AddMediaFilesForInstream(inStreamVideoCreative, request.AdCreative, request.AdCreative.InStreamVideos[0].InStreamVideoCreativeUnit.DeliveryMethod, adCreativeUnit);
                                            adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                            if (adCreativeUnit.SnapshotDocument != null) adCreativeUnit.SnapshotDocument.UpdateUsage();

                                            inStreamVideoCreative.OrientationType = request.AdCreative.OrientationType != OrientationType.Any ? request.AdCreative.OrientationType : (OrientationType?)null;


                                            if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST2)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_2);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST3)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_3);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST4)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_4);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST41)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST41);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST42)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST42);
                                        }
                                        else
                                        {
                                            adCreativeUnit.InStreamVideoCreativeUnit.DeliveryMethod = videoDeliveryMethodRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.DeliveryMethod);

                                            inStreamVideoCreative.DurationInSeconds = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration;
                                            adCreativeUnit.InStreamVideoCreativeUnit.BitRate = adCreativeUnitDto.InStreamVideoCreativeUnit.BitRate;
                                            adCreativeUnit.InStreamVideoCreativeUnit.VideoType = mimeTypeRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.VideoType);
                                            adCreativeUnit.InStreamVideoCreativeUnit.Width = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoWidth;
                                            adCreativeUnit.InStreamVideoCreativeUnit.Height = adCreativeUnitDto.InStreamVideoCreativeUnit.VideoHeight;


                                            adCreativeUnit.Document = documentRepository.Get(adCreativeUnitDto.DocumentId.Value);
                                            adCreativeUnit.Document.UpdateUsage();
                                            inStreamVideoCreative.IsXml = false;
                                            inStreamVideoCreative.CreateOption = CreateOption.Upload;
                                            // adCreativeUnit.InStreamVideoCreativeUnit.
                                            inStreamVideoCreative.IsDraft = true;
                                            adCreativeUnit.Content = string.Empty;

                                            inStreamVideoCreative.OrientationType = creativeUnitRepository.Query(x => x.ID == adCreativeUnitDto.CreativeUnitId).First().OrientationType;
                                            //adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_2);


                                            if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST2)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_2);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST3)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_3);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST4)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST_4);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST41)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST41);
                                            else if (request.AdCreative.VASTProtocol == VASTProtocolsVersion.VAST42)
                                                adCreativeUnit.Protocol = _ProtocolRepository.GetByCode((int)ProtocolCode.VAST42);
                                        }

                                        if (inStreamVideoCreative.CreateOption == CreateOption.Upload)
                                        {
                                            adCreativeUnit.RemoveMediaFiles();
                                        }

                                        if (adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.HasValue)
                                        {
                                            adCreativeUnit.InStreamVideoCreativeUnit.ThumbnailDoc = documentRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.ThumbnailDocId.Value);
                                        }

                                        adCreativeUnit.InStreamVideoCreativeUnit.OriginalCreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId);

                                        adCreativeUnit.CreativeUnit = creativeUnitRepository.Query(X => X.Code == "28").SingleOrDefault();

                                        foreach (AdCreativeUnitTrackerDto unitTrackerDto in adCreativeUnitDto.InStreamVideoCreativeUnit.ImpressionTrackerRedirectList)
                                        {
                                            var eventTracker = group.TrackingEvents.Where(p => p.ID == unitTrackerDto.AdGroupEventId && !p.IsDeleted).FirstOrDefault();

                                            adCreativeUnit.SetTrackingEvent(eventTracker, unitTrackerDto.ImpressionURls.Select(x => x.URL).ToList());
                                        }
                                        var impressionEventObj = group.TrackingEvents.Where(p => p.Code.ToLower() == IMPRESSIONEVENT && !p.IsDeleted).FirstOrDefault();
                                        if (request.AdCreative.ImpressionTrackingURL != null && request.AdCreative.ImpressionTrackingURL.Count > 0)
                                        {
                                            // foreach (var URL in request.AdCreative.VideoEndCardsTrackingURL)
                                            //// {
                                            adCreativeUnit.SetTrackingEvent(impressionEventObj, request.AdCreative.ImpressionTrackingURL);
                                            //}
                                        }
                                    }


                                    if (request.AdCreative.VideoEndCards != null && request.AdCreative.VideoEndCards.Count > 0)
                                    {
                                        var impressionEventObj = group.TrackingEvents.Where(p => p.Code.ToLower() == IMPRESSIONEVENT && !p.IsDeleted).FirstOrDefault();
                                        foreach (var adCreativeSave in request.AdCreative.VideoEndCards)
                                        {
                                            if (inStreamVideoCreative.VideoEndCards == null)
                                                inStreamVideoCreative.VideoEndCards = new List<VideoEndCardCreative>();



                                            var adVideoEndCardItem = new VideoEndCardCreative

                                            {
                                                ActionValue = new Domain.Model.Campaign.Objective.AdActionValue { ActionType = group.Objective.AdAction },
                                                CretiveUnitDeviceType = DeviceTypeEnum.Any
                                            };
                                            var seachVideoEndCardItem = inStreamVideoCreative.VideoEndCards.Where(M => M.AdCreativeUnits != null && M.AdCreativeUnits.Any(x => x.CreativeUnit.ID == adCreativeSave.CreativeUnitsContent[0].CreativeUnitId)).SingleOrDefault();
                                            if (seachVideoEndCardItem != null)
                                            {
                                                adVideoEndCardItem = seachVideoEndCardItem;
                                                adVideoEndCardItem.ClearCreativeUnits();
                                            }
                                            else
                                            {
                                                adVideoEndCardItem.CreationDate = Framework.Utilities.Environment.GetServerTime();
                                                adVideoEndCardItem.uId = Guid.NewGuid().ToString();
                                                adVideoEndCardItem.Parent = inStreamVideoCreative;
                                            }





                                            //get AdCreativeUnits
                                            foreach (var adCreativeUnitDto in adCreativeSave.CreativeUnitsContent)
                                            {
                                                var adCreativeUnit = new AdCreativeUnit
                                                {
                                                    CreativeUnit = creativeUnitRepository.Get(adCreativeUnitDto.CreativeUnitId),
                                                    //    Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content)),
                                                    UniqueId = Guid.NewGuid().ToString()
                                                    // Content= adCreativeUnitDto.Content
                                                };
                                                adVideoEndCardItem.Name = adCreativeUnit.CreativeUnit.Width + "X" + adCreativeUnit.CreativeUnit.Height;
                                                adCreativeSave.Name = adCreativeUnit.CreativeUnit.Width + "X" + adCreativeUnit.CreativeUnit.Height;
                                                if (adCreativeSave.CardType == VideoEndCardType.Static)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(adCreativeUnitDto.Content))
                                                        adCreativeUnit.Document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content));


                                                }
                                                else
                                                {
                                                    adCreativeUnit.Content = adCreativeUnitDto.Content;
                                                }
                                                if (adCreativeUnit.Document != null)
                                                    adCreativeUnit.Document.UpdateUsage();
                                                //adCreativeUnit.SetTrackingEvent(impressionEvent, adCreativeUnitDto.ImpressionTrackerRedirect);
                                                if (request.AdCreative.VideoEndCardsTrackingURL != null && request.AdCreative.VideoEndCardsTrackingURL.Count > 0)
                                                {
                                                    // foreach (var URL in request.AdCreative.VideoEndCardsTrackingURL)
                                                    //// {
                                                    adCreativeUnit.SetTrackingEvent(impressionEventObj, request.AdCreative.VideoEndCardsTrackingURL);
                                                    //}
                                                }


                                                adVideoEndCardItem.AddCreativeUnit(adCreativeUnit);

                                            }

                                            getAdCreative(adVideoEndCardItem, adCreativeSave);
                                            adVideoEndCardItem.CardType = adCreativeSave.CardType;
                                            adVideoEndCardItem.EnableAutoClose = adCreativeSave.EnableAutoClose;
                                            if (adVideoEndCardItem.EnableAutoClose)
                                                adVideoEndCardItem.AutoCloseWaitInSeconds = adCreativeSave.AutoCloseWaitInSeconds;
                                            else
                                                adVideoEndCardItem.AutoCloseWaitInSeconds = null;
                                            adVideoEndCardItem.Group = group;
                                            if (adVideoEndCardItem.ID == 0)
                                                inStreamVideoCreative.VideoEndCards.Add(adVideoEndCardItem);
                                            item.SetCreativeStatus(group, adVideoEndCardItem, request.AdCreative.IsAdChanged);
                                            adVideoEndCardItem.TypeId = AdTypeIds.VideoEndCard;
                                            adVideoEndCardItem.AdSubType = AdSubTypes.VideoEndCard;
                                            adVideoEndCardItem.IsSecureCompliant = SearchForSecuredUrls(adVideoEndCardItem, adCreativeSave);
                                            adVideoEndCardItem.Type = _AdTypeRepository.Get((int)adVideoEndCardItem.TypeId);
                                            adVideoEndCardItem.TypeForPortal = adVideoEndCardItem.Type;
                                            if (adVideoEndCardItem.AdSubType.HasValue)
                                                adVideoEndCardItem.AdSubTypeForPortal = adVideoEndCardItem.AdSubType;


                                            adVideoEndCardItem.EnableEventsPostback = true;
                                            adVideoEndCardItem.VerifyTargetingCriteria = true;
                                            adVideoEndCardItem.VerifyDailyBudget = true;
                                            adVideoEndCardItem.VerifyCampaignStartAndEndDate = true;
                                            adVideoEndCardItem.ValidateRequestDeviceAndLocationData = true;

                                            adVideoEndCardItem.VerifyPrerequisiteEvents = true;
                                            adVideoEndCardItem.UpdateEventsFrequency = false;
                                            adVideoEndCardItem.VerifyEventsFrequency = false;
                                            adVideoEndCardItem.UpdateTags = true;
                                        }

                                        if (inStreamVideoCreative.VideoEndCards != null)
                                        {
                                            AdCreativeSaveDto itemVideoCard = null;

                                            for (int i = 0; i < inStreamVideoCreative.VideoEndCards.Count; i++)
                                            {
                                                if (request.AdCreative.VideoEndCards != null)
                                                    itemVideoCard = request.AdCreative.VideoEndCards.Where(M => M.CreativeUnitsContent != null && M.CreativeUnitsContent.Any(X => X.CreativeUnitId == inStreamVideoCreative.VideoEndCards[i].AdCreativeUnits[0].CreativeUnit.ID)).SingleOrDefault();
                                                if (itemVideoCard == null)
                                                {
                                                    inStreamVideoCreative.VideoEndCards[i].Parent = null;
                                                    inStreamVideoCreative.VideoEndCards[i].Delete();

                                                    // inStreamVideoCreative.VideoEndCards.Remove(inStreamVideoCreative.VideoEndCards[i]);
                                                }
                                            }
                                        }




                                    }
                                    else
                                    {

                                        //inStreamVideoCreative.VideoEndCards.Clear();

                                        if (inStreamVideoCreative.VideoEndCards != null)
                                        {

                                            for (int i = 0; i < inStreamVideoCreative.VideoEndCards.Count; i++)
                                            {
                                                inStreamVideoCreative.VideoEndCards[i].Parent = null;
                                                inStreamVideoCreative.VideoEndCards[i].Delete();
                                                //  inStreamVideoCreative.VideoEndCards.Remove(inStreamVideoCreative.VideoEndCards[i]);
                                            }



                                        }
                                    }


                                    if (request.AdCreative.ThirdPartyTrackers != null && request.AdCreative.ThirdPartyTrackers.Count > 0)
                                        SetThirdPartyTrackers(adItem, request.AdCreative.ThirdPartyTrackers.ToList());
                                    break;

                                    #endregion
                                }


                        }
                        // if (request.AdCreative.IsAdChanged)
                        //{
                        foreach (var AdCreativeUnit in adItem.AdCreativeUnits)
                        {
                            AdCreativeUnit.Version += 1;
                        }
                        //}
                    }
                    #endregion


                    item.AddAdCreative(group, adItem);
                    if (adItem is InStreamVideoCreative && (adItem as InStreamVideoCreative).IsXml)
                    {
                        (adItem as InStreamVideoCreative).XmlUpload();
                    }
                    if (adItem.Group.Campaign.CampaignType == CampaignType.Normal || adItem.Group.Campaign.CampaignType == CampaignType.ProgrammaticGuaranteed)
                    {
                        if(adItem.Group.CostModelWrapper!=null)
                        adItem.SetAdCreativeBid(request.AdCreative.Bid, adItem.Group.CostModelWrapper.Factor);
                        else
                            throw new SelectCostModelException();
                    }
                    adItem.Validate(statusCheck: true);
                    item.SetCreativeStatus(group, adItem, request.AdCreative.IsAdChanged);
                    if (adItem.ActionValue != null && !string.IsNullOrWhiteSpace(adItem.ActionValue.Value))
                    {
                        // trim spaces from first value
                        adItem.ActionValue.Value = adItem.ActionValue.Value.Trim();
                    }
                    if (request.AdCreative.IsAdPaused)
                    {
                        adItem.Pause();
                    }

                    #region House Ad
                    if (group.HouseAd != null)
                    {
                        if (adItem.AppSiteAdQueues == null)
                        {
                            adItem.AppSiteAdQueues = new List<AppSiteAdQueue>();
                        }
                        List<AppSiteAdQueue> deletedItems = adItem.AppSiteAdQueues.Where(l2 => !group.HouseAd.DestinationAppSites.Any(l1 => l1.ID == l2.AppSite.ID)).ToList();
                        foreach (var destinationAppSites in deletedItems)
                        {
                            var appSite = appSiteRepository.Get(destinationAppSites.AppSite.ID);
                            if (appSite != null)
                            {
                                item.RemoveAppSiteAdQueue(group, adItem, appSite);
                            }
                        }
                        foreach (var destinationAppSites in group.HouseAd.DestinationAppSites)
                        {
                            var appSite = appSiteRepository.Get(destinationAppSites.ID);
                            if (appSite != null)
                            {
                                item.AddAppSiteAdQueue(group, adItem, appSite);
                            }
                        }
                    }
                    #endregion


                    adItem.IsSecureCompliant = SearchForSecuredUrls(adItem, request.AdCreative);
                    adItem.Type = _AdTypeRepository.Get((int)adItem.TypeId);
                    if (adItem.ID > 0 && request.AdCreative.IsAdChanged)
                        adItem.UpdatedbyPortal = true;

                    #region CreativeVendor


                    IEnumerable<CreativeVendorKeyword> listOfKeyWordsItems = null;
                    IEnumerable<CreativeVendorKeywordDto> listOfKeyWords = null;


                    //if (request.AdCreative.CreativeVendorIds == null || request.AdCreative.CreativeVendorIds.Count==0)
                    //{

                    ////Framework.Caching.CacheManager.Current.DefaultProvider.Put("", listOfKywordsDto.ToArray());

                    //}
                    if (request.AdCreative.IsCreativeVendorChanged)
                    {
                        foreach (var creativeUnit in adItem.AdCreativeUnits)
                        {

                            SaveUpdateAdCreativeUnit(creativeUnit, request.AdCreative.CreativeVendorIds);
                        }
                    }
                    else
                    {
                        if ((request.AdCreative.CreativeVendorIds == null || request.AdCreative.CreativeVendorIds.Count() == 0) || (request.AdCreative.IsAdChanged && !request.AdCreative.IsCreativeVendorChanged))
                        {
                            IList<int> creativeVendorIds = new List<int>();
                            foreach (var creativeUnit in adItem.AdCreativeUnits)
                            {


                                listOfKeyWords = ArabyAds.Framework.Caching.RedisCache.RedisCacheSerilizer.FormatterSurrogateDeSerialize(Framework.Caching.CacheManager.Current.DefaultProvider.Get<byte[]>("CreativeVendorList")) as IEnumerable<CreativeVendorKeywordDto>;

                                if (listOfKeyWords == null || listOfKeyWords.Count() == 0)
                                {
                                    listOfKeyWordsItems = _CreativeVendorKeywordRepository.GetAll();
                                    listOfKeyWords = listOfKeyWordsItems.Select(q => MapperHelper.Map<CreativeVendorKeywordDto>(q));
                                    if (listOfKeyWords != null)
                                        Framework.Caching.CacheManager.Current.DefaultProvider.Put<byte[]>("CreativeVendorList", ArabyAds.Framework.Caching.RedisCache.RedisCacheSerilizer.FormatterSurrogateSerialize(listOfKeyWords.ToList()));
                                }



                                if (creativeUnit.Trackers != null)
                                {
                                    foreach (AdCreativeUnitTracker Tracker in creativeUnit.GetTrackers())
                                    {

                                        var keyWordListDto = listOfKeyWords.Where(M =>(Tracker.TrackingUrl!=null &&  Tracker.TrackingUrl.Contains(M.Keyword)) ||(Tracker.TrackingJS != null &&  Tracker.TrackingJS.Contains(M.Keyword))).ToList();


                                        if (keyWordListDto != null && keyWordListDto.Count > 0)
                                        {
                                            //   creativeUnit.AdCreativeUnitVendorList = new List<AdCreativeUnitVendor>();
                                            foreach (var keyDto in keyWordListDto)
                                            {
                                                creativeVendorIds.Add(keyDto.VendorId);
                                                // var itemCreativeVendor= new AdCreativeUnitVendor { Unit = creativeUnit, Vendor = new CreativeVendor { ID = keyDto.VendorId } };
                                                // creativeUnit.AdCreativeUnitVendorList.Add(itemCreativeVendor); 
                                            }


                                        }
                                    }


                                }
                                if (request.AdCreative.AdActionValue != null && request.AdCreative.AdActionValue.Trackers != null)
                                {
                                    foreach (AdActionValueTrackerDto Tracker in request.AdCreative.AdActionValue.Trackers)
                                    {
                                        // UrlsListToCheck.Add(Tracker.TrackingUrl);
                                        var keyWordListDto = listOfKeyWords.Where(M => (Tracker.URL!=null &&  Tracker.URL.Contains(M.Keyword)) || (Tracker.JS!=null &&  Tracker.JS.Contains(M.Keyword))).ToList();


                                        if (keyWordListDto != null && keyWordListDto.Count > 0)
                                        {
                                            //creativeUnit.AdCreativeUnitVendorList = new List<AdCreativeUnitVendor>();
                                            foreach (var keyDto in keyWordListDto)
                                            {
                                                creativeVendorIds.Add(keyDto.VendorId);
                                                // var itemCreativeVendor = new AdCreativeUnitVendor { Unit = creativeUnit, Vendor = new CreativeVendor { ID = keyDto.VendorId } };
                                                // creativeUnit.AdCreativeUnitVendorList.Add(itemCreativeVendor);
                                            }


                                        }
                                    }


                                }
                                if (!string.IsNullOrEmpty(creativeUnit.Content))
                                {
                                    var keyWordListDto = listOfKeyWords.Where(M => creativeUnit.Content.Contains(M.Keyword)).ToList();


                                    if (keyWordListDto != null && keyWordListDto.Count > 0)
                                    {
                                        //creativeUnit.AdCreativeUnitVendorList = new List<AdCreativeUnitVendor>();
                                        foreach (var keyDto in keyWordListDto)
                                        {
                                            creativeVendorIds.Add(keyDto.VendorId);
                                            // var itemCreativeVendor = new AdCreativeUnitVendor { Unit = creativeUnit, Vendor = new CreativeVendor { ID = keyDto.VendorId } };
                                            //creativeUnit.AdCreativeUnitVendorList.Add(itemCreativeVendor);
                                        }


                                    }

                                }

                            }


                            creativeVendorIds = creativeVendorIds.Distinct().ToList();
                            if (request.AdCreative.CreativeVendorIds != null)
                                creativeVendorIds = creativeVendorIds.Concat(request.AdCreative.CreativeVendorIds).Distinct().ToList();
                            foreach (var creativeUnit in adItem.AdCreativeUnits)
                            {

                                SaveUpdateAdCreativeUnit(creativeUnit, creativeVendorIds);

                            }
                        }

                    }


                    #endregion

                    #region AdGroup Minumum Unit Price

                    if (group.GetAds() != null && group.GetAds().Count > 0)
                    {

                        var orderList = group.GetAds().OrderBy(M => M.GetBid()).ToList();
                        group.MinimumUnitPrice = orderList[0].GetBid();
                    }

                    #endregion


                    #region nwe Ad Type Bsiness
                    adItem.TypeForPortal = adItem.Type;
                    if(adItem.AdSubType.HasValue)
                    adItem.AdSubTypeForPortal = adItem.AdSubType;

                    #endregion

                    #region data provider allow impression tracker
                    if (!IsCampaignManager() &&  !SearchForAllowImpressionTrackers(group, adItem, request.AdCreative))
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSave") });



                    }

                    if (!IsCampaignManager() &&  SearchForMyAudience(group))
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSaveMyAud") });

                    }
                    #endregion
                    #region WrapperContent 
                    adItem.SetWrapperContent(request.AdCreative.WrapperContent);
                    #endregion
                    CampaignRepository.Save(item);

                    return ValueMessageWrapper.Create(adItem.ID);
                }
            }
            return ValueMessageWrapper.Create(0);
        }

        public ValueMessageWrapper<bool> DoesContainDataProviderAllowImpressionTracker(CampaignIdAdgroupIdMessage request)
        {
            if (IsCampaignManager())
            {
                return ValueMessageWrapper.Create(true);
            }

            var item = CampaignRepository.Get(request.CampaignId);
          
           
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);

            var result = group.IsDataProviderAllowImpression();

        
            return ValueMessageWrapper.Create(result);
          
            }
        public virtual void SetClickTags(AdCreative adCreative, List<ClickTagTrackerDto> trackingEventUrls)
        {

            if (trackingEventUrls == null && adCreative.ID == 0)
            {
                return;
            }
            else if (trackingEventUrls == null)
            {
                if (adCreative.ClickTags != null)
                {
                    foreach (var clic in adCreative.ClickTags)
                    {

                        clic.IsDeleted = true;
                    }
                }
                return;
            }


            if (adCreative.ClickTags == null)
            {
                adCreative.ClickTags = new List<ClickTagTracker>();
            }
            if (adCreative.ID == 0)
            {


                foreach (var url in trackingEventUrls)
                {

                    if (!string.IsNullOrEmpty(url.TrackingUrl))
                    {
                        var ClickTagTrackervar = new ClickTagTracker()
                        {
                            TrackingUrl = url.TrackingUrl,
                            VariableName = url.VariableName,

                            Creative = adCreative
                        };

                        adCreative.ClickTags.Add(ClickTagTrackervar);
                    }
                }
            }
            else
            {

                if (adCreative.ClickTags != null)
                {
                    foreach (var clic in adCreative.ClickTags)
                    {

                        clic.IsDeleted = true;
                    }
                }
                foreach (var url in trackingEventUrls)
                {

                    var creativeUnitTrackingEvent = new ClickTagTracker()
                    {
                        TrackingUrl = url.TrackingUrl,
                        VariableName = url.VariableName,

                        Creative = adCreative
                    };
                    adCreative.ClickTags.Add(creativeUnitTrackingEvent);


                }
            }
            //   this.Trackers = 
        }


        public virtual void SetThirdPartyTrackers(AdCreative adCreative, List<ThirdPartyTrackerDto> trackingEventUrls)
        {

            if (trackingEventUrls == null && adCreative.ID == 0)
            {
                return;
            }
            else if (trackingEventUrls == null)
            {
                if (adCreative.ThirdPartyTrackers != null)
                {
                    foreach (var clic in adCreative.ThirdPartyTrackers)
                    {

                        clic.IsDeleted = true;
                    }
                }
                  var customParam=  adCreative.GetAdCustomParam("omid1");
                if( customParam!=null )
                adCreative.RemoveAdCustomParameter(customParam);

                return;
            }
            else if (trackingEventUrls != null && trackingEventUrls.Count == 0)
            {

                if (adCreative.ThirdPartyTrackers != null)
                {
                    foreach (var clic in adCreative.ThirdPartyTrackers)
                    {

                        clic.IsDeleted = true;
                    }
                }
                var customParam = adCreative.GetAdCustomParam("omid1");
                if (customParam != null)
                    adCreative.RemoveAdCustomParameter(customParam);

                return;
            }
            else if ( trackingEventUrls!=null && trackingEventUrls.Count>0)
            {

                adCreative.AddAdCustomParameter("omid1", "1", true);
            }


            if (adCreative.ThirdPartyTrackers == null)
            {
                adCreative.ThirdPartyTrackers = new List<ThirdPartyTracker>();
            }
            if (adCreative.ID == 0)
            {


                foreach (var url in trackingEventUrls)
                {

                    if (!string.IsNullOrEmpty(url.VendorID))
                    {
                        var ClickTagTrackervar = new ThirdPartyTracker()
                        {
                            VendorID = url.VendorID,
                            ExecutionErrorTrackerURL = url.ExecutionErrorTrackerURL,
                             ParametersURL = url.ParametersURL,
                            ScriptURL = url.ScriptURL,

                            Creative = adCreative
                        };

                        adCreative.ThirdPartyTrackers.Add(ClickTagTrackervar);
                    }
                }
            }
            else
            {

                if (adCreative.ThirdPartyTrackers != null)
                {
                    foreach (var clic in adCreative.ThirdPartyTrackers)
                    {

                        clic.IsDeleted = true;
                    }
                }
                /*foreach (var c in  adCreative.ThirdPartyTrackers)
                {
                    var trackingevent= trackingEventUrls.Where(M => M.ID == c.ID).SingleOrDefault();
                    if (trackingevent==null)
                    {
                        c.IsDeleted = true;
                    }
                }*/
                foreach (var url in trackingEventUrls)
                {

                    //if (url.ID == 0 )
                    //{
                        var creativeUnitTrackingEvent = new ThirdPartyTracker()
                        {
                            VendorID = url.VendorID,
                            ExecutionErrorTrackerURL = url.ExecutionErrorTrackerURL,
                            ParametersURL = url.ParametersURL,
                            ScriptURL = url.ScriptURL,
                           // ID = url.ID,
                            IsDeleted = false,

                            Creative = adCreative
                        };
                        adCreative.ThirdPartyTrackers.Add(creativeUnitTrackingEvent);

                    //}
                    //else {

                        //var track= adCreative.ThirdPartyTrackers.Where(M => M.ID == url.ID).Single();
                        //track.IsDeleted = url.IsDeleted;
                    //}


                }
            }
            //   this.Trackers = 
        }

        private void SaveUpdateAdCreativeUnit(AdCreativeUnit creativeUnit, IList<int> CreativeVendorIds)
        {


            if (creativeUnit.AdCreativeUnitVendorList != null)
            {



                if (CreativeVendorIds != null && CreativeVendorIds.Count > 0)
                {

                    for (int i = 0; i < creativeUnit.AdCreativeUnitVendorList.Count; i++)
                    {
                        var vendorItem = CreativeVendorIds.Where(M => M == creativeUnit.AdCreativeUnitVendorList[i].Vendor.ID).SingleOrDefault();
                        if (vendorItem == 0)
                        {
                            creativeUnit.AdCreativeUnitVendorList.RemoveAt(i);
                            if (i >= 1)
                                i--;
                        }
                    }
                    foreach (var vendorId in CreativeVendorIds)
                    {
                        var singleCreative = creativeUnit.AdCreativeUnitVendorList.Where(M => M.Vendor.ID == vendorId).SingleOrDefault();
                        if (singleCreative == null)
                            creativeUnit.AdCreativeUnitVendorList.Add(new AdCreativeUnitVendor { Vendor = new CreativeVendor { ID = vendorId }, Unit = creativeUnit });

                    }
                }
                else
                {
                    if (creativeUnit.AdCreativeUnitVendorList.Count > 0)
                        creativeUnit.AdCreativeUnitVendorList.Clear();

                }

                //}
            }
            else

            {
                if (CreativeVendorIds != null && CreativeVendorIds.Count > 0)
                {
                    creativeUnit.AdCreativeUnitVendorList = new List<AdCreativeUnitVendor>();
                    foreach (var vendorId in CreativeVendorIds)
                    {
                        creativeUnit.AdCreativeUnitVendorList.Add(new AdCreativeUnitVendor { Vendor = new CreativeVendor { ID = vendorId }, Unit = creativeUnit });

                    }
                }
            }
        }
        private bool SearchForSecuredUrls(AdCreative adCreative, AdCreativeSaveDto dto)
        {
            bool IsSecureCompliant = false;
            List<string> UrlsListToCheck = new List<string>();
            var AdSubtype = adCreative.AdSubType.GetValueOrDefault();
            var isExternalIntersessial = false;
            if (AdSubtype == AdSubTypes.ExternalUrlInterstitial)
            {
                isExternalIntersessial = true;
            }
            if (adCreative.TypeId == AdTypeIds.PlainHTML || (adCreative.TypeId == AdTypeIds.RichMedia && (AdSubtype == AdSubTypes.JavaScriptRichMedia || AdSubtype == AdSubTypes.JavaScriptInterstitial)))
            {
                if (dto.IsSecureCompliant)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                #region ActionArea
                if (adCreative.ActionValue != null)
                {
                    if (adCreative.ActionValue.Trackers != null)
                    {
                        foreach (AdActionValueTracker Tracker in adCreative.ActionValue.Trackers)
                        {
                            if (!Tracker.IsDeleted)
                                UrlsListToCheck.Add(Tracker.Url);
                        }
                    }
                    //if (!string.IsNullOrEmpty(adCreative.ActionValue.Value))
                    //{
                    //    UrlsListToCheck.Add(adCreative.ActionValue.Value);
                    //}
                    //if (!string.IsNullOrEmpty(adCreative.ActionValue.Value2))
                    //{
                    //    UrlsListToCheck.Add(adCreative.ActionValue.Value2);
                    //}
                }
                #endregion
                // and SnapshotUrl
                #region AdCreativeUnits

                if (adCreative.AdCreativeUnits != null)
                {
                    foreach (AdCreativeUnit adCreativeUnit in adCreative.AdCreativeUnits)
                    {

                        if (adCreativeUnit.Trackers != null)
                        {
                            foreach (AdCreativeUnitTracker Tracker in adCreativeUnit.GetTrackers())
                            {
                                UrlsListToCheck.Add(Tracker.TrackingUrl);
                            }
                        }
                        if (isExternalIntersessial)
                        {
                            if (!string.IsNullOrEmpty(adCreativeUnit.Content))
                            {
                                UrlsListToCheck.Add(adCreativeUnit.Content);
                            }
                        }
                    }
                }
                #endregion
            }

            if (UrlsListToCheck.Count > 0)
            {
                IsSecureCompliant = CheckSecuredUrls(UrlsListToCheck);
            }
            else
                IsSecureCompliant = true;
            return IsSecureCompliant;
        }



        private bool SearchForAllowImpressionTrackers(AdGroup adGroup, AdCreative adCreative, AdCreativeSaveDto dto=null)
        {

        


            var result = adGroup.IsDataProviderAllowImpression();

            if (result)
            {
                return true;
            }
          
            if ((adCreative.TypeId == AdTypeIds.RichMedia && ( adCreative.AdSubType == AdSubTypes.ExternalUrlInterstitial ||  adCreative.AdSubType == AdSubTypes.JavaScriptRichMedia || adCreative.AdSubType == AdSubTypes.JavaScriptInterstitial || adCreative.AdSubType == AdSubTypes.HTML5Interstitial || adCreative.AdSubType == AdSubTypes.HTML5RichMedia)))
            {
                return false;
            }
          
            if (adCreative.TypeId == AdTypeIds.InStreamVideo)
            {

                var adItem = (InStreamVideoCreative)adCreative;

                if (adItem.CreateOption==CreateOption.VAST)
                {
                    return false;
                }
            }

            if (adCreative.TypeId == AdTypeIds.PlainHTML)
            {

              
                    return false;
                
            }
            // and SnapshotUrl
            #region AdCreativeUnits

            if (adCreative.AdCreativeUnits != null)
                {
                    foreach (AdCreativeUnit adCreativeUnit in adCreative.AdCreativeUnits)
                    {

                        if (adCreativeUnit.Trackers != null)
                        {
                            foreach (AdCreativeUnitTracker Tracker in adCreativeUnit.GetTrackers())
                            {
                                if (Tracker.AdCreativeUnitTrackerType == AdCreativeUnitTrackerType.URL && !string.IsNullOrWhiteSpace(Tracker.TrackingUrl))
                                    return false;
                            }
                        }
                      
                    }
                }
                #endregion
            
            return true;

        }

        private bool SearchForMyAudience(AdGroup adGroup)
        {




            var result = adGroup.IsDataProviderAllowImpression();
            if (result)
            { return false; }

          return   adGroup.IsUsingMyAudienceListTargeting();

        }
            private bool CheckSecuredUrls(List<string> urls)
        {
            foreach (string url in urls)
            {
                if (!string.IsNullOrEmpty(url))
                { if (url.ToLower().IndexOf("https") != 0)
                        return false;
                }
            }

            return true;
        }
        public void GenerateClickTrackerUrl(AdCreative aditem)
        {
            AdTrackerCreative adTrackerCreative = (AdTrackerCreative)aditem;
            var constraints = adTrackerCreative.ActionValue.ActionType.Constraints;
            if (constraints != null)
            {
                var constraint = constraints.Where(M => M.Platform.ID > 0).SingleOrDefault();
                if (constraint != null)
                {

                    var platformID = constraint.Platform.ID;
                    var appMarketingPartnerObj = adTrackerCreative.AppMarketingPartner;
                    var traker = appMarketingPartnerObj.Trackers.Where(M => M.Platform.ID == platformID).SingleOrDefault();

                    if (traker != null)
                    {

                        var ClickTrackerUrlTemplate = traker.ClickTrackerUrlTemplate;
                        ClickTrackerUrlTemplate = ClickTrackerUrlTemplate.Replace(Configuration.CreativeUnitPrefixNameFormatClickTrackerURL, aditem.uId);
                        adTrackerCreative.ClickTrackerUrl = ClickTrackerUrlTemplate;
                        adCreativeRepository.Save(adTrackerCreative);

                    }
                }


            }

        }
        public void GenerateClickTrackerUrl(AdCreative aditem, string uniqID)
        {
            AdTrackerCreative adTrackerCreative = (AdTrackerCreative)aditem;
            var constraints = adTrackerCreative.ActionValue.ActionType.Constraints;
            if (constraints != null && constraints.Count>0)
            {
                var constraint = constraints.Where(M => M.Platform.ID > 0).SingleOrDefault();
                if (constraint != null)
                {

                    var platformID = constraint.Platform.ID;
                    var appMarketingPartnerObj = adTrackerCreative.AppMarketingPartner;
                    var trakers = appMarketingPartnerObj.Trackers.Where(M => M.Platform!=null && M.Platform.ID == platformID && M.TypeID == 1).ToList();
                    AppMarketingPartnerTracker traker = null;
                    if (trakers != null)
                        traker = trakers.Where(M => M.AdGroupID == aditem.Group.ID).SingleOrDefault();
                    if (traker == null)
                    {
                        traker = trakers.Where(M => M.AdGroupID == null).SingleOrDefault();
                    }
                    if (traker != null)
                    {

                        var ClickTrackerUrlTemplate = traker.TrackerUrlTemplate;
                        ClickTrackerUrlTemplate = ClickTrackerUrlTemplate.Replace(Configuration.CreativeUnitPrefixNameFormatClickTrackerURL, uniqID);
                        adTrackerCreative.ClickTrackerUrl = ClickTrackerUrlTemplate;
                        // adCreativeRepository.Save(adTrackerCreative);

                    }
                }
                else
                {

                    var appMarketingPartnerObj = adTrackerCreative.AppMarketingPartner;
                    var trakers = appMarketingPartnerObj.Trackers.Where(M => M.Platform == null && M.TypeID == 1).ToList();
                    AppMarketingPartnerTracker traker = null;
                    if (trakers != null)
                        traker = trakers.Where(M => M.AdGroupID == aditem.Group.ID).SingleOrDefault();
                    if (traker == null)
                    {
                        traker = trakers.Where(M => M.AdGroupID == null).SingleOrDefault();
                    }
                    if (traker != null)
                    {

                        var ClickTrackerUrlTemplate = traker.TrackerUrlTemplate;
                        ClickTrackerUrlTemplate = ClickTrackerUrlTemplate.Replace(Configuration.CreativeUnitPrefixNameFormatClickTrackerURL, uniqID);
                        adTrackerCreative.ClickTrackerUrl = ClickTrackerUrlTemplate;
                        // adCreativeRepository.Save(adTrackerCreative);

                    }

                }


            }
            else
            {

                var appMarketingPartnerObj = adTrackerCreative.AppMarketingPartner;

               var PlatformId = adTrackerCreative.Platform?.ID;
                IList<AppMarketingPartnerTracker> trakers = null;
                if(PlatformId.HasValue)
                trakers = appMarketingPartnerObj.Trackers.Where(M => M.Platform!=null &&  M.Platform.ID == PlatformId && M.TypeID == 1).ToList();
                else
                 trakers = appMarketingPartnerObj.Trackers.Where(M => M.Platform ==null && M.TypeID == 1).ToList();

                AppMarketingPartnerTracker traker = null;
                if (trakers != null)
                    traker = trakers.Where(M => M.AdGroupID == aditem.Group.ID).SingleOrDefault();
                if (traker == null)
                {
                    traker = trakers.Where(M => M.AdGroupID == null).SingleOrDefault();
                }
                if (traker != null)
                {

                    var ClickTrackerUrlTemplate = traker.TrackerUrlTemplate;
                    ClickTrackerUrlTemplate = ClickTrackerUrlTemplate.Replace(Configuration.CreativeUnitPrefixNameFormatClickTrackerURL, uniqID);
                    adTrackerCreative.ClickTrackerUrl = ClickTrackerUrlTemplate;
                    // adCreativeRepository.Save(adTrackerCreative);

                }

            }
        }
        private void AddNativeAdImages(NativeAdCreative nativeAdItem, AdCreativeSaveDto adCreative)
        {
            foreach (var adCreativeUnitDto in adCreative.NativeAdImages)
            {
                var nativeAdCreative = creativeUnitRepository.Query(p => p.ID == adCreativeUnitDto.CreativeUnitId).SingleOrDefault();
                var nativeImage = nativeAdItem.Images.Where(p => p.CreativeUnit.ID == nativeAdCreative.ID).SingleOrDefault();
                var document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content));

                bool continueFlag = true;

                if (nativeImage != null)
                {
                    if (nativeImage.Document != null)
                    {
                        if (nativeImage.Document.ID != document.ID)
                        {
                            nativeImage.Document.UpdateUsage(true);
                        }
                        else
                        {
                            continueFlag = false;
                        }
                    }
                }
                else
                {
                    nativeImage = new NativeAdImage();
                    nativeImage.AdCreative = nativeAdItem;
                    nativeAdItem.Images.Add(nativeImage);
                }

                if (continueFlag)
                {
                    nativeImage.Document = document;
                    nativeImage.Document.UpdateUsage();
                    nativeImage.URL = null;
                    nativeImage.CreativeUnit = nativeAdCreative;
                    nativeImage.MIMEType = document.DocumentType.MIMETypes.First();
                }
            }


            var itemsToBeDeleted = nativeAdItem.Images.Where(p => !adCreative.NativeAdImages.Any(z => int.Parse(z.Content) == p.Document.ID)).Select(p => p.ID).ToList();

            foreach (var item in itemsToBeDeleted)
            {
                var itemToDelete = nativeAdItem.Images.Where(p => p.ID == item).Single();
                nativeAdItem.Images.Remove(itemToDelete);
            }
        }
        private void AddMediaFilesForInstream(InStreamVideoCreative InStreamCreative, AdCreativeSaveDto adCreative, int DeliveryMethod, AdCreativeUnit AdCreativeUnt)
        {

            if (adCreative.MediaFilesSupported != null)
            {
                if (AdCreativeUnt.MediaFiles == null)
                {
                    AdCreativeUnt.MediaFiles = new List<VideoMediaFile>();

                }
                foreach (var creativeId in adCreative.MediaFilesSupported)
                {
                    var InstreamAdCreative = creativeUnitRepository.Query(p => p.ID == creativeId.CreativeUnitId).SingleOrDefault();
                    var mediaFile = AdCreativeUnt.MediaFiles.Where(p => p.OriginalCreativeUnit.ID == creativeId.CreativeUnitId && p.URL == creativeId.URL).SingleOrDefault();


                    bool continueFlag = true;

                    if (mediaFile == null)
                    {


                        var videoMediaFile = new VideoMediaFile();
                        videoMediaFile.VideoAd = InStreamCreative;
                        videoMediaFile.OriginalCreativeUnit = InstreamAdCreative;
                        //videoMediaFile.DeliveryMethod= InStreamCreative.d
                        if (creativeId.VideoTypeId > 0)
                            videoMediaFile.VideoType = new MIMEType { ID = creativeId.VideoTypeId };
                        videoMediaFile.BitRate = creativeId.bitRate;
                        if (AdCreativeUnt != null)
                            videoMediaFile.AdCreativeUnit = AdCreativeUnt;
                        else
                            videoMediaFile.AdCreativeUnit = null;
                        videoMediaFile.DeliveryMethod = videoDeliveryMethodRepository.Get(DeliveryMethod);

                        videoMediaFile.URL = creativeId.URL;

                        AdCreativeUnt.MediaFiles.Add(videoMediaFile);
                    }


                }


                if (AdCreativeUnt.MediaFiles != null && AdCreativeUnt.MediaFiles.Count > 0)
                {

                    IList<int> itemsToBeDeleted = new List<int>();
                    foreach (var mediaFile in AdCreativeUnt.MediaFiles)
                    {
                        var itemToBeDeleted = adCreative.MediaFilesSupported.Where(z => z.CreativeUnitId == mediaFile.OriginalCreativeUnit.ID && mediaFile.ID > 0).ToList();

                        if ((itemToBeDeleted == null || itemToBeDeleted.Count == 0) && mediaFile.ID > 0)
                        {
                            itemsToBeDeleted.Add(mediaFile.ID);

                        }

                    }
                    //var itemsToBeDeleted = AdCreativeUnt.MediaFiles.Where(p => !adCreative.MediaFilesSupported.Any(z => z.CreativeUnitId == p.OriginalCreativeUnit.ID && z.URL == p.URL && p.ID > 0)).Select(p => p.ID).ToList();

                    foreach (var item in itemsToBeDeleted)
                    {
                        var itemToDelete = AdCreativeUnt.MediaFiles.Where(p => p.ID == item).Single();
                        AdCreativeUnt.MediaFiles.Remove(itemToDelete);
                    }
                }


            }

        }
        private void AddNativeAdIcons(NativeAdCreative nativeAdItem, AdCreativeSaveDto adCreative)
        {
            foreach (var adCreativeUnitDto in adCreative.NativeAdIcons)
            {
                var nativeAdCreative = creativeUnitRepository.Query(p => p.ID == adCreativeUnitDto.CreativeUnitId).SingleOrDefault();
                var nativeIcon = nativeAdItem.Icons.Where(p => p.CreativeUnit.ID == nativeAdCreative.ID).SingleOrDefault();
                var document = documentRepository.Get(Convert.ToInt32(adCreativeUnitDto.Content));

                bool continueFlag = true;

                if (nativeIcon != null)
                {
                    if (nativeIcon.Document != null)
                    {
                        if (nativeIcon.Document.ID != document.ID)
                        {
                            nativeIcon.Document.UpdateUsage(true);
                        }
                        else
                        {
                            continueFlag = false;
                        }
                    }
                }
                else
                {
                    nativeIcon = new NativeAdIcon();
                    nativeIcon.AdCreative = nativeAdItem;
                    nativeAdItem.Icons.Add(nativeIcon);
                }

                if (continueFlag)
                {
                    nativeIcon.Document = document;
                    nativeIcon.Document.UpdateUsage();
                    nativeIcon.URL = null;
                    nativeIcon.CreativeUnit = nativeAdCreative;
                    nativeIcon.MIMEType = document.DocumentType.MIMETypes.First();
                }
            }


            var itemsToBeDeleted = nativeAdItem.Icons.Where(p => !adCreative.NativeAdIcons.Any(z => int.Parse(z.Content) == p.Document.ID)).Select(p => p.ID).ToList();

            foreach (var item in itemsToBeDeleted)
            {
                var itemToDelete = nativeAdItem.Icons.Where(p => p.ID == item).Single();
                nativeAdItem.Icons.Remove(itemToDelete);
            }
        }


        /// <summary>
        /// use this service operation to Delete List of Ads using Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        /// <returns>true id the Delete OPration is successes</returns>
        public ValueMessageWrapper<bool> DeleteAds(CampaignIdAdgroupIdAdIdsMessage request)
        {
            if (request.AdIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                    if (group != null)
                    {
                        var groupAds = item.GetGroupAds(group);
                        if (groupAds != null)
                        {
                            foreach (var adId in request.AdIds)
                            {
                                //get Ad  Object
                                var adObj = groupAds.FirstOrDefault(ad => ad.ID == adId);
                                if (adObj != null)
                                {
                                    item.DeleteGroupAd(group, adObj);
                                }
                            }
                        }

                        if (group.GetAds() != null && group.GetAds().Count >= 1)
                        {

                            var orderList = group.GetAds().OrderBy(M => M.GetBid()).ToList();
                            group.MinimumUnitPrice = orderList[0].GetBid();
                        }
                        else
                        {

                            group.MinimumUnitPrice = group.Bid;
                        }
                    }
                }
            }
            return ValueMessageWrapper.Create(true);
        }

        /// <summary>
        /// use this service operation to run list of Ad Groups Object depend on the Ids
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        public void RunAds(CampaignIdAdgroupIdAdIdsMessage request)
        {
            if (request.AdIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                    if (group != null)
                    {
                        var groupAds = item.GetGroupAds(group);
                        if (groupAds != null)
                        {
                            foreach (var adId in request.AdIds)
                            {
                                //get Ad  Object
                                var adObj = groupAds.FirstOrDefault(ad => ad.ID == adId);
                                if (adObj != null)
                                {
                                    item.ResumeAd(group, adObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// use this service operation to run list of Campaigns Object depend on the Id
        /// </summary>
        /// <param name="campaignId">Campaign Id to Get By</param>
        /// <param name="groupId">Group Id to Get By</param>
        /// <param name="adIds">Ids to Get By</param>
        public void PauseAds(CampaignIdAdgroupIdAdIdsMessage request)
        {
            if (request.AdIds != null)
            {
                var item = CampaignRepository.Get(request.CampaignId);
                CheckCampaign(item);
                ValidateCampaign(item);

                if (item.IsValid)
                {
                    var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                    if (group != null)
                    {
                        var groupAds = item.GetGroupAds(group);
                        if (groupAds != null)
                        {
                            foreach (var adId in request.AdIds)
                            {
                                //get Ad  Object
                                var adObj = groupAds.FirstOrDefault(ad => ad.ID == adId);
                                if (adObj != null)
                                {
                                    item.PauseAd(group, adObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RejectAd(CampaignIdAdgroupIdAdIdMessage request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        //get Ad  Object
                        var adObj = groupAds.FirstOrDefault(ad => ad.ID == request.AdId);
                        if (adObj != null)
                        {
                            item.RejectAd(group, adObj);
                        }
                    }
                }
            }
        }

        public void ApproveAd(ApproveAdDto approveAdDto)
        {
            var item = CampaignRepository.Get(approveAdDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            AdCreative adObj = null;
            if (item.IsValid)
            {
                if (item.CampaignType == CampaignType.Normal || item.CampaignType == CampaignType.ProgrammaticGuaranteed)
                {
                    // check if campaign settings is set 
                    if (item.Keyword == null || item.Advertiser == null)
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignSettingsBR") });
                    }
                }
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == approveAdDto.GroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        //get Ad  Object
                        adObj = groupAds.FirstOrDefault(ad => ad.ID == approveAdDto.AdId);
                        if (adObj != null)
                        {
                            switch (adObj.TypeId)
                            {
                                case AdTypeIds.Banner:
                                    foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                    {
                                        if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                        {
                                            if (adCreativeUnit.SnapshotDocument != null)
                                            {
                                                adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                            }

                                            adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                            adCreativeUnit.SnapshotDocument.UpdateUsage();
                                        }
                                    }
                                    break;
                                case AdTypeIds.TrackingAd:
                                    //if (adObj.AdCreativeUnits!=null)
                                    //{
                                    //foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                    //{
                                    //    if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                    //    {
                                    //        if (adCreativeUnit.SnapshotDocument != null)
                                    //        {
                                    //            adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                    //            adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                    //        }

                                    //        adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                    //        adCreativeUnit.SnapshotDocument.UpdateUsage();
                                    //    }
                                    //}
                                    //}
                                    break;
                                case AdTypeIds.Text:
                                case AdTypeIds.PlainHTML:
                                case AdTypeIds.RichMedia:

                                    if (adObj.AdSubType == AdSubTypes.ExpandableRichMedia)
                                    {
                                        foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                        {
                                            if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                            {
                                                if (adCreativeUnit.SnapshotDocument != null)
                                                {
                                                    adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                    adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                                }

                                                adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                adCreativeUnit.SnapshotDocument.UpdateUsage();
                                            }
                                        }
                                    }
                                    else if (adObj.AdSubType == AdSubTypes.HTML5RichMedia || adObj.AdSubType == AdSubTypes.HTML5Interstitial)
                                    {
                                        var HTML5RichMediaCreative = adObj.AdCreativeUnits.Single();

                                        if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                        {

                                            if (HTML5RichMediaCreative.SnapshotDocument != null)
                                            {
                                                HTML5RichMediaCreative.SnapshotDocument.UpdateUsage(true);
                                            }

                                            HTML5RichMediaCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                            HTML5RichMediaCreative.SnapshotDocument.UpdateUsage();
                                        }
                                        else
                                        {
                                            if (HTML5RichMediaCreative.SnapshotDocument != null)
                                            {
                                                HTML5RichMediaCreative.SnapshotDocument.UpdateUsage(true);
                                                HTML5RichMediaCreative.SnapshotDocument = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var adCreative in adObj.AdCreativeUnits)
                                        {
                                            var snapShotDto = approveAdDto.Snapshots.Where(p => p.CreativeUnitId == adCreative.CreativeUnit.ID).SingleOrDefault();

                                            if (snapShotDto != null)
                                            {
                                                int snapshotDocumentId = int.Parse(snapShotDto.Content);
                                                var snapshotDocument = documentRepository.Get(snapshotDocumentId);

                                                if (!(adCreative.SnapshotDocument != null && adCreative.SnapshotDocument.ID == snapshotDocumentId))
                                                {
                                                    if (adCreative.SnapshotDocument != null)
                                                    {
                                                        adCreative.SnapshotDocument.UpdateUsage(true);
                                                    }

                                                    adCreative.SnapshotDocument = snapshotDocument;
                                                    adCreative.SnapshotDocument.UpdateUsage();
                                                    adCreative.KeepShapshot = false;
                                                }
                                            }
                                            else
                                            {
                                                if (adCreative.SnapshotDocument != null)
                                                {
                                                    adCreative.SnapshotDocument.UpdateUsage(true);
                                                    adCreative.SnapshotDocument = null;
                                                }
                                                adCreative.KeepShapshot = false;
                                            }
                                        }
                                    }
                                    break;
                                case AdTypeIds.NativeAd:

                                    var nativeAdCreative = adObj.AdCreativeUnits.Single();

                                    if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                    {

                                        if (nativeAdCreative.SnapshotDocument != null)
                                        {
                                            nativeAdCreative.SnapshotDocument.UpdateUsage(true);
                                        }

                                        nativeAdCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                        nativeAdCreative.SnapshotDocument.UpdateUsage();
                                    }
                                    else
                                    {
                                        if (nativeAdCreative.SnapshotDocument != null)
                                        {
                                            nativeAdCreative.SnapshotDocument.UpdateUsage(true);
                                            nativeAdCreative.SnapshotDocument = null;
                                        }
                                    }
                                    break;
                                case AdTypeIds.InStreamVideo:
                                    {
                                        var instreamVideoCreative = adObj.AdCreativeUnits.Single();

                                        if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                        {

                                            if (instreamVideoCreative.SnapshotDocument != null)
                                            {
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage(true);

                                                instreamVideoCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage();

                                            }
                                        }
                                        else
                                        {
                                            if (instreamVideoCreative.SnapshotDocument != null)
                                            {
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage(true);
                                                instreamVideoCreative.SnapshotDocument = null;
                                            }
                                        }
                                        //upload snap shots for video end cards urls
                                        var InStreamVideoCreative = (InStreamVideoCreative)instreamVideoCreative.AdCreative;
                                        if (InStreamVideoCreative.CreateOption == CreateOption.Upload && InStreamVideoCreative.VideoEndCards != null && InStreamVideoCreative.VideoEndCards.Count() > 0)
                                        {
                                            foreach (var Snapshot in approveAdDto.Snapshots)
                                            {
                                                if (Snapshot.Content != "")
                                                {
                                                    var AdCreativeUnit = _CreativeUnitRepository.Get(Snapshot.CreativeUnitId);
                                                    if (AdCreativeUnit != null)
                                                    {
                                                        var VideoEndCard = InStreamVideoCreative.VideoEndCards.Where(x => x.AdCreativeUnits.Last().CreativeUnit.Width == AdCreativeUnit.Width && x.AdCreativeUnits.Last().CreativeUnit.Height == AdCreativeUnit.Height).FirstOrDefault();
                                                        if (VideoEndCard != null)
                                                            VideoEndCard.AdCreativeUnits.Last().SnapshotDocument = new Document { ID = Convert.ToInt32(Snapshot.Content) };
                                                    }
                                                }

                                            }
                                        }
                                        InStreamVideoCreative.Approve();
                                        break;
                                    }

                                default:
                                    break;
                            }

                            foreach (var adCreative in adObj.AdCreativeUnits)
                            {

                                if (adCreative.AttributesMapping == null)
                                {
                                    adCreative.AttributesMapping = new List<AdCreativeUnitAttributeMapping>();

                                }
                                AdCreativeUnitDto creativeAttributes = null;
                                if (approveAdDto.AdCreativesAttribues!=null)
                                creativeAttributes = approveAdDto.AdCreativesAttribues.Where(p => p.ID == adCreative.ID).SingleOrDefault();

                                if (creativeAttributes != null)
                                {
                                    var attributesList = new List<AdCreativeAttribute>();

                                    foreach (var attributeDto in creativeAttributes.Attributes)
                                    {
                                        AdCreativeAttribute attribute = new AdCreativeAttribute()
                                        {
                                            ID = attributeDto.ID
                                        };

                                        attributesList.Add(attribute);
                                        var existattribute = adCreative.AttributesMapping.Where(M => M.Attribute.ID == attributeDto.ID).SingleOrDefault();
                                        if (existattribute == null)
                                        {

                                            adCreative.AttributesMapping.Add(new AdCreativeUnitAttributeMapping { AdCreativeUnit = adCreative, Attribute = new AdCreativeAttribute { ID = attributeDto.ID } });
                                        }

                                    }
                                    for (var i = 0; i < adCreative.AttributesMapping.Count; i++)
                                    {
                                        var existattribute = creativeAttributes.Attributes.Where(M => M.ID == adCreative.AttributesMapping[i].Attribute.ID).SingleOrDefault();
                                        if (existattribute == null)
                                        {
                                            adCreative.AttributesMapping.Remove(adCreative.AttributesMapping[i]);
                                        }
                                    }

                                }
                            }


                            item.ApproveAd(group, adObj);
                            adObj.DomainURL = approveAdDto.DomainURL;
                            if (approveAdDto.KeywordId.HasValue)
                            {
                                adObj.Keyword = new Keyword { ID = approveAdDto.KeywordId.Value };
                            }
                            else
                            {
                                adObj.Keyword = null;
                            }
                            if (approveAdDto.LanguageId.HasValue)
                            {
                                adObj.Language = new Language { ID = approveAdDto.LanguageId.Value };
                            }
                            else
                            {
                                adObj.Language = null;
                            }

                        }
                        if (item.Account.AccountRole == AccountRole.DSP)
                        {
                            if (approveAdDto.RunType.Equals("RON", StringComparison.OrdinalIgnoreCase))
                            {
                                item.ClearAppSiteAdQueue(group, adObj);
                            }

                            //if (item.Status!=null && item.Status)
                            //{
                            //    item.ClearAppSiteAdQueue(group, adObj);
                            //}
                            if (group.AdGroupInventorySources != null && group.AdGroupInventorySources.Where(x => !x.IsDeleted).Count() > 0)
                            {
                                var allSources = group.AdGroupInventorySources.Where(x => !x.IsDeleted).Distinct();
                                if (allSources != null)
                                {
                                    foreach (var inventory in allSources)
                                    {
                                       if(approveAdDto.DeletedAppSiteIds!=null)
                                        { 
                                            if (approveAdDto.DeletedAppSiteIds.Where(x => x == inventory.AppSite.ID).Count() > 0)
                                            {
                                                approveAdDto.DeletedAppSiteIds = approveAdDto.DeletedAppSiteIds.Except(approveAdDto.DeletedAppSiteIds.Where(x => x == inventory.AppSite.ID).ToArray()).ToArray();
                                            }
                                        }
                                        adObj.AddAppSiteAdQueue(inventory.AppSite, inventory.Include == true);

                                    }
                                }
                            }
                            else
                            {
                                var sspPartnters = _SSPPartnerRepository.Query(x => !x.IsDeleted).ToList();
                                foreach (var sspPartnter in sspPartnters)
                                {
                                    if (approveAdDto.DeletedAppSiteIds != null && approveAdDto.DeletedAppSiteIds.Where(x => x == sspPartnter.AppSite.ID).Count() > 0)
                                    {
                                        approveAdDto.DeletedAppSiteIds = approveAdDto.DeletedAppSiteIds.Except(approveAdDto.DeletedAppSiteIds.Where(x => x == sspPartnter.AppSite.ID).ToArray()).ToArray();
                                    }

                                    adObj.AddAppSiteAdQueue(sspPartnter.AppSite);
                                }

                            }
                        }
                        if (approveAdDto.RunType.Equals("RON", StringComparison.OrdinalIgnoreCase) && item.Account.AccountRole != AccountRole.DSP)
                        {
                            item.ClearAppSiteAdQueue(group, adObj);
                        }
                        else
                        {
                            if (approveAdDto.AppSiteIds != null)
                            {
                                List<int> newAppsiteIds = approveAdDto.AppSiteIds.Except(adObj.AppSiteAdQueues.Select(x => x.AppSite.ID)).ToList<int>();
                                var response =CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId  = item.ID, Appsites = newAppsiteIds, AdGroupId = adObj.Group.ID });
                                if (response.NotCompatableCampaigns != null && response.NotCompatableCampaigns.Count > 0)
                                {
                                    if (approveAdDto.UpdatedCampaignBidConfigDtos == null || approveAdDto.UpdatedCampaignBidConfigDtos.Count <= 0)
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                                        throw error;
                                        //throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                                    }

                                    foreach (CampaignBidConfigDto bidConfig in response.NotCompatableCampaigns)
                                    {
                                        if (approveAdDto.UpdatedCampaignBidConfigDtos.Select(x => x.Appsite.ID == bidConfig.Appsite.ID && x.Bid > 0).Count() <= 0)
                                        {
                                            var error = new BusinessException();
                                            error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                                            throw error;
                                            //throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                                        }
                                    }
                                }
                                if (approveAdDto.UpdatedCampaignBidConfigDtos!=null)
                                {
                                    foreach (var bidConfig in approveAdDto.UpdatedCampaignBidConfigDtos)
                                    {
                                        if (!string.IsNullOrEmpty(bidConfig.ID))
                                        {
                                            var campaignBidConfig = group.GetCampaignBidConfigs().Where(x => x.ID == Convert.ToInt32(bidConfig.ID)).FirstOrDefault();
                                            campaignBidConfig.SetAdGroupBidConfigsBid(bidConfig.Bid);
                                        }
                                        else
                                        {
                                            var campaignBidConfig = new AdGroupBidConfig() { AdGroup = group, SubPublisherId = bidConfig.SubPublisherId };
                                            campaignBidConfig.AppSite = appSiteRepository.Get(bidConfig.Appsite.ID);
                                            campaignBidConfig.Account = accountRepository.Get(bidConfig.AccountId);
                                            campaignBidConfig.SetAdGroupBidConfigsBid(bidConfig.Bid);
                                            group.AddCampaignBidConfig(campaignBidConfig);
                                        }
                                    }
                                }
                                    foreach (var appSiteId in approveAdDto.AppSiteIds)
                                {
                                    ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appSite = new ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite { ID = appSiteId };
                                    if (appSite != null)
                                    {
                                        item.AddAppSiteAdQueue(group, adObj, appSite, approveAdDto.Include);
                                    }
                                }
                            }
                            if (approveAdDto.DeletedAppSiteIds != null)
                            {
                                foreach (var appSiteId in approveAdDto.DeletedAppSiteIds)
                                {
                                    // var appSite = appSiteRepository.Get(appSiteId);
                                    ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appSite = new ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite { ID = appSiteId };
                                    if (appSite != null)
                                    {
                                        item.RemoveAppSiteAdQueue(group, adObj, appSite);
                                    }
                                }
                            }
                            item.UpdateAppSiteAdQueueType(group, adObj, approveAdDto.Include);
                        }

                        if (approveAdDto.AdsToCopyAppSites != null && approveAdDto.AdsToCopyAppSites.Count() != 0)
                        {
                            foreach (var AdId in approveAdDto.AdsToCopyAppSites)
                            {
                                var adToBeCopied = adRepository.Get(AdId);

                                if (adToBeCopied != null && !adToBeCopied.IsDeleted)
                                {

                                    //adToBeCopied.ClearAppSiteAdQueue();

                                    foreach (var appSiteQueue in adObj.AppSiteAdQueues)
                                    {
                                        if (!adToBeCopied.ExistAppSiteAdQueue(appSiteQueue.AppSite))
                                        {
                                            adToBeCopied.AddAppSiteAdQueue(appSiteQueue.AppSite);
                                        }

                                    }
                                    for (var appSiteQueueC = 0; appSiteQueueC < adToBeCopied.AppSiteAdQueues.Count;)
                                    {
                                        if (!adObj.ExistAppSiteAdQueue(adToBeCopied.AppSiteAdQueues[appSiteQueueC].AppSite))
                                        {
                                            adToBeCopied.RemoveAppSiteAdQueue(adToBeCopied.AppSiteAdQueues[appSiteQueueC].AppSite);
                                            // var appSiteq= adToBeCopied.RemoveAppSiteAdQueueAndReturn(appSiteQueue.AppSite);
                                            // this._AppSiteAdQueueRepository.Remove(appSiteq);
                                        }
                                        else
                                        {
                                            appSiteQueueC++;
                                        }

                                    }


                                    adToBeCopied.UpdateAppSiteAdQueueType(approveAdDto.Include);
                                    adRepository.Save(adToBeCopied);
                                }
                            }
                        }
                    }
                }

                CheckAdvertiserBlock(group, adObj);

                #region data provider allow impression tracker
                if (!SearchForAllowImpressionTrackers(group, adObj, null))
                {
                    throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSave") });

                }
                if (SearchForMyAudience(group))
                {
                    throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSaveMyAud") });

                }
           
                #endregion
                CampaignRepository.Save(item);


            }
        }

        public void ApproveAdForNewUI(ApproveAdDto approveAdDto)
        {
            var item = CampaignRepository.Get(approveAdDto.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            AdCreative adObj = null;
            if (item.IsValid)
            {
                if (item.CampaignType == CampaignType.Normal || item.CampaignType == CampaignType.ProgrammaticGuaranteed)
                {
                    // check if campaign settings is set 
                    if (item.Keyword == null || item.Advertiser == null)
                    {
                        throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignSettingsBR") });
                    }
                }
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == approveAdDto.GroupId);
                if (group != null)
                {
                    var groupAds = item.GetGroupAds(group);
                    if (groupAds != null)
                    {
                        //get Ad  Object
                        adObj = groupAds.FirstOrDefault(ad => ad.ID == approveAdDto.AdId);
                        if (adObj != null)
                        {
                            switch (adObj.TypeId)
                            {
                                case AdTypeIds.Banner:
                                    foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                    {
                                        if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                        {
                                            if (adCreativeUnit.SnapshotDocument != null)
                                            {
                                                adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                            }

                                            adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                            adCreativeUnit.SnapshotDocument.UpdateUsage();
                                        }
                                    }
                                    break;
                                case AdTypeIds.TrackingAd:
                                    //if (adObj.AdCreativeUnits!=null)
                                    //{
                                    //foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                    //{
                                    //    if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                    //    {
                                    //        if (adCreativeUnit.SnapshotDocument != null)
                                    //        {
                                    //            adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                    //            adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                    //        }

                                    //        adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                    //        adCreativeUnit.SnapshotDocument.UpdateUsage();
                                    //    }
                                    //}
                                    //}
                                    break;
                                case AdTypeIds.Text:
                                case AdTypeIds.PlainHTML:
                                case AdTypeIds.RichMedia:

                                    if (adObj.AdSubType == AdSubTypes.ExpandableRichMedia)
                                    {
                                        foreach (var adCreativeUnit in adObj.AdCreativeUnits)
                                        {
                                            if (adCreativeUnit.Document != null && !adCreativeUnit.KeepShapshot)
                                            {
                                                if (adCreativeUnit.SnapshotDocument != null)
                                                {
                                                    adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                    adCreativeUnit.SnapshotDocument.UpdateUsage(true);
                                                }

                                                adCreativeUnit.SnapshotDocument = adCreativeUnit.Document;
                                                adCreativeUnit.SnapshotDocument.UpdateUsage();
                                            }
                                        }
                                    }
                                    else if (adObj.AdSubType == AdSubTypes.HTML5RichMedia || adObj.AdSubType == AdSubTypes.HTML5Interstitial)
                                    {
                                        var HTML5RichMediaCreative = adObj.AdCreativeUnits.Single();

                                        if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                        {

                                            if (HTML5RichMediaCreative.SnapshotDocument != null)
                                            {
                                                HTML5RichMediaCreative.SnapshotDocument.UpdateUsage(true);
                                            }

                                            HTML5RichMediaCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                            HTML5RichMediaCreative.SnapshotDocument.UpdateUsage();
                                        }
                                        else
                                        {
                                            if (HTML5RichMediaCreative.SnapshotDocument != null)
                                            {
                                                HTML5RichMediaCreative.SnapshotDocument.UpdateUsage(true);
                                                HTML5RichMediaCreative.SnapshotDocument = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var adCreative in adObj.AdCreativeUnits)
                                        {
                                            var snapShotDto = approveAdDto.Snapshots.Where(p => p.CreativeUnitId == adCreative.CreativeUnit.ID).SingleOrDefault();

                                            if (snapShotDto != null)
                                            {
                                                int snapshotDocumentId = int.Parse(snapShotDto.Content);
                                                var snapshotDocument = documentRepository.Get(snapshotDocumentId);

                                                if (!(adCreative.SnapshotDocument != null && adCreative.SnapshotDocument.ID == snapshotDocumentId))
                                                {
                                                    if (adCreative.SnapshotDocument != null)
                                                    {
                                                        adCreative.SnapshotDocument.UpdateUsage(true);
                                                    }

                                                    adCreative.SnapshotDocument = snapshotDocument;
                                                    adCreative.SnapshotDocument.UpdateUsage();
                                                    adCreative.KeepShapshot = false;
                                                }
                                            }
                                            else
                                            {
                                                if (adCreative.SnapshotDocument != null)
                                                {
                                                    adCreative.SnapshotDocument.UpdateUsage(true);
                                                    adCreative.SnapshotDocument = null;
                                                }
                                                adCreative.KeepShapshot = false;
                                            }
                                        }
                                    }
                                    break;
                                case AdTypeIds.NativeAd:

                                    var nativeAdCreative = adObj.AdCreativeUnits.Single();

                                    if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                    {

                                        if (nativeAdCreative.SnapshotDocument != null)
                                        {
                                            nativeAdCreative.SnapshotDocument.UpdateUsage(true);
                                        }

                                        nativeAdCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                        nativeAdCreative.SnapshotDocument.UpdateUsage();
                                    }
                                    else
                                    {
                                        if (nativeAdCreative.SnapshotDocument != null)
                                        {
                                            nativeAdCreative.SnapshotDocument.UpdateUsage(true);
                                            nativeAdCreative.SnapshotDocument = null;
                                        }
                                    }
                                    break;
                                case AdTypeIds.InStreamVideo:
                                    {
                                        var instreamVideoCreative = adObj.AdCreativeUnits.Single();

                                        if (approveAdDto.Snapshots != null && approveAdDto.Snapshots.Count != 0)
                                        {

                                            if (instreamVideoCreative.SnapshotDocument != null)
                                            {
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage(true);

                                                instreamVideoCreative.SnapshotDocument = documentRepository.Get(int.Parse(approveAdDto.Snapshots.Single().Content));
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage();

                                            }
                                        }
                                        else
                                        {
                                            if (instreamVideoCreative.SnapshotDocument != null)
                                            {
                                                instreamVideoCreative.SnapshotDocument.UpdateUsage(true);
                                                instreamVideoCreative.SnapshotDocument = null;
                                            }
                                        }
                                        //upload snap shots for video end cards urls
                                        var InStreamVideoCreative = (InStreamVideoCreative)instreamVideoCreative.AdCreative;
                                        if (InStreamVideoCreative.CreateOption == CreateOption.Upload && InStreamVideoCreative.VideoEndCards != null && InStreamVideoCreative.VideoEndCards.Count() > 0)
                                        {
                                            foreach (var Snapshot in approveAdDto.Snapshots)
                                            {
                                                if (Snapshot.Content != "")
                                                {
                                                    var AdCreativeUnit = _CreativeUnitRepository.Get(Snapshot.CreativeUnitId);
                                                    if (AdCreativeUnit != null)
                                                    {
                                                        var VideoEndCard = InStreamVideoCreative.VideoEndCards.Where(x => x.AdCreativeUnits.Last().CreativeUnit.Width == AdCreativeUnit.Width && x.AdCreativeUnits.Last().CreativeUnit.Height == AdCreativeUnit.Height).FirstOrDefault();
                                                        if (VideoEndCard != null)
                                                            VideoEndCard.AdCreativeUnits.Last().SnapshotDocument = new Document { ID = Convert.ToInt32(Snapshot.Content) };
                                                    }
                                                }

                                            }
                                        }
                                        InStreamVideoCreative.Approve();
                                        break;
                                    }

                                default:
                                    break;
                            }

                            foreach (var adCreative in adObj.AdCreativeUnits)
                            {

                                if (adCreative.AttributesMapping == null)
                                {
                                    adCreative.AttributesMapping = new List<AdCreativeUnitAttributeMapping>();

                                }
                                AdCreativeUnitDto creativeAttributes = null;
                                if (approveAdDto.Snapshots != null)
                                    creativeAttributes = approveAdDto.Snapshots.Where(p => p.CreativeUnitId == adCreative.ID).SingleOrDefault();

                                if (creativeAttributes != null)
                                {
                                    var attributesList = new List<AdCreativeAttribute>();

                                    foreach (var attributeDto in creativeAttributes.Attributes)
                                    {
                                        AdCreativeAttribute attribute = new AdCreativeAttribute()
                                        {
                                            ID = attributeDto.ID
                                        };

                                        attributesList.Add(attribute);
                                        var existattribute = adCreative.AttributesMapping.Where(M => M.Attribute.ID == attributeDto.ID).SingleOrDefault();
                                        if (existattribute == null)
                                        {

                                            adCreative.AttributesMapping.Add(new AdCreativeUnitAttributeMapping { AdCreativeUnit = adCreative, Attribute = new AdCreativeAttribute { ID = attributeDto.ID } });
                                        }

                                    }
                                    for (var i = 0; i < adCreative.AttributesMapping.Count; i++)
                                    {
                                        var existattribute = creativeAttributes.Attributes.Where(M => M.ID == adCreative.AttributesMapping[i].Attribute.ID).SingleOrDefault();
                                        if (existattribute == null)
                                        {
                                            adCreative.AttributesMapping.Remove(adCreative.AttributesMapping[i]);
                                        }
                                    }

                                }
                            }


                            item.ApproveAd(group, adObj);
                            adObj.DomainURL = approveAdDto.DomainURL;
                            if (approveAdDto.KeywordId.HasValue)
                            {
                                adObj.Keyword = new Keyword { ID = approveAdDto.KeywordId.Value };
                            }
                            else
                            {
                                adObj.Keyword = null;
                            }
                            if (approveAdDto.LanguageId.HasValue)
                            {
                                adObj.Language = new Language { ID = approveAdDto.LanguageId.Value };
                            }
                            else
                            {
                                adObj.Language = null;
                            }

                        }
                        if (item.Account.AccountRole == AccountRole.DSP)
                        {
                            if (approveAdDto.RunType.Equals("RON", StringComparison.OrdinalIgnoreCase))
                            {
                                item.ClearAppSiteAdQueue(group, adObj);
                            }

                            //if (item.Status!=null && item.Status)
                            //{
                            //    item.ClearAppSiteAdQueue(group, adObj);
                            //}
                            if (group.AdGroupInventorySources != null && group.AdGroupInventorySources.Where(x => !x.IsDeleted).Count() > 0)
                            {
                                var allSources = group.AdGroupInventorySources.Where(x => !x.IsDeleted).Distinct();
                                if (allSources != null)
                                {
                                    foreach (var inventory in allSources)
                                    {
                                        if (approveAdDto.DeletedAppSiteIds != null)
                                        {
                                            if (approveAdDto.DeletedAppSiteIds.Where(x => x == inventory.AppSite.ID).Count() > 0)
                                            {
                                                approveAdDto.DeletedAppSiteIds = approveAdDto.DeletedAppSiteIds.Except(approveAdDto.DeletedAppSiteIds.Where(x => x == inventory.AppSite.ID).ToArray()).ToArray();
                                            }
                                        }
                                        adObj.AddAppSiteAdQueue(inventory.AppSite, inventory.Include == true);

                                    }
                                }
                            }
                            else
                            {
                                var sspPartnters = _SSPPartnerRepository.Query(x => !x.IsDeleted).ToList();
                                foreach (var sspPartnter in sspPartnters)
                                {
                                    if (approveAdDto.DeletedAppSiteIds != null && approveAdDto.DeletedAppSiteIds.Where(x => x == sspPartnter.AppSite.ID).Count() > 0)
                                    {
                                        approveAdDto.DeletedAppSiteIds = approveAdDto.DeletedAppSiteIds.Except(approveAdDto.DeletedAppSiteIds.Where(x => x == sspPartnter.AppSite.ID).ToArray()).ToArray();
                                    }

                                    adObj.AddAppSiteAdQueue(sspPartnter.AppSite);
                                }

                            }
                        }
                        if (approveAdDto.RunType.Equals("RON", StringComparison.OrdinalIgnoreCase) && item.Account.AccountRole != AccountRole.DSP)
                        {
                            item.ClearAppSiteAdQueue(group, adObj);
                        }
                        else
                        {
                            if (approveAdDto.AppSiteIds != null)
                            {
                                List<int> newAppsiteIds = approveAdDto.AppSiteIds.Except(adObj.AppSiteAdQueues.Select(x => x.AppSite.ID)).ToList<int>();
                                var response = CheckAppsitesCostModelCompatableWitCampaign(new CheckAppsitesCostModelCompatableWitCampaignRequest { CampaignId = item.ID, Appsites = newAppsiteIds, AdGroupId = adObj.Group.ID });
                                if (response.NotCompatableCampaigns != null && response.NotCompatableCampaigns.Count > 0)
                                {
                                    if (approveAdDto.UpdatedCampaignBidConfigDtos == null || approveAdDto.UpdatedCampaignBidConfigDtos.Count <= 0)
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                                        throw error;
                                        //throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                                    }

                                    foreach (CampaignBidConfigDto bidConfig in response.NotCompatableCampaigns)
                                    {
                                        if (approveAdDto.UpdatedCampaignBidConfigDtos.Select(x => x.Appsite.ID == bidConfig.Appsite.ID && x.Bid > 0).Count() <= 0)
                                        {
                                            var error = new BusinessException();
                                            error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                                            throw error;
                                            //throw new BusinessException(new List<ErrorData> { new ErrorData("CampaignBidConfigsNotValid") });
                                        }
                                    }
                                }
                                if (approveAdDto.UpdatedCampaignBidConfigDtos != null)
                                {
                                    foreach (var bidConfig in approveAdDto.UpdatedCampaignBidConfigDtos)
                                    {
                                        if (!string.IsNullOrEmpty(bidConfig.ID))
                                        {
                                            var campaignBidConfig = group.GetCampaignBidConfigs().Where(x => x.ID == Convert.ToInt32(bidConfig.ID)).FirstOrDefault();
                                            campaignBidConfig.SetAdGroupBidConfigsBid(bidConfig.Bid);
                                        }
                                        else
                                        {
                                            var campaignBidConfig = new AdGroupBidConfig() { AdGroup = group, SubPublisherId = bidConfig.SubPublisherId };
                                            campaignBidConfig.AppSite = appSiteRepository.Get(bidConfig.Appsite.ID);
                                            campaignBidConfig.Account = accountRepository.Get(bidConfig.AccountId);
                                            campaignBidConfig.SetAdGroupBidConfigsBid(bidConfig.Bid);
                                            group.AddCampaignBidConfig(campaignBidConfig);
                                        }
                                    }
                                }
                                foreach (var appSiteId in approveAdDto.AppSiteIds)
                                {
                                    ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appSite = new ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite { ID = appSiteId };
                                    if (appSite != null)
                                    {
                                        item.AddAppSiteAdQueue(group, adObj, appSite, approveAdDto.Include);
                                    }
                                }
                            }
                            if (approveAdDto.DeletedAppSiteIds != null)
                            {
                                foreach (var appSiteId in approveAdDto.DeletedAppSiteIds)
                                {
                                    // var appSite = appSiteRepository.Get(appSiteId);
                                    ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appSite = new ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite { ID = appSiteId };
                                    if (appSite != null)
                                    {
                                        item.RemoveAppSiteAdQueue(group, adObj, appSite);
                                    }
                                }
                            }


                            List<AppSiteAdQueue> appSiteQue = new List<AppSiteAdQueue>();
                            foreach (var obj in  adObj.AppSiteAdQueues)
                            {
                                appSiteQue.Add(obj);
                            }
                            if (appSiteQue!=null)
                            {
                                foreach (var appSiteId in appSiteQue)
                                {

                                    if (approveAdDto.AppSiteIds!=null)
                                    {
                                        if (appSiteId != null && appSiteId.AppSite != null && !(approveAdDto.AppSiteIds.Where(M => M == appSiteId.AppSite.ID).FirstOrDefault() > 0))
                                        {
                                            item.RemoveAppSiteAdQueue(group, adObj, appSiteId.AppSite);
                                        }
                                    }
                                }
                            }
                            
                            item.UpdateAppSiteAdQueueType(group, adObj, approveAdDto.Include);
                        }

                        if (approveAdDto.AdsToCopyAppSites != null && approveAdDto.AdsToCopyAppSites.Count() != 0)
                        {
                            foreach (var AdId in approveAdDto.AdsToCopyAppSites)
                            {
                                var adToBeCopied = adRepository.Get(AdId);

                                if (adToBeCopied != null && !adToBeCopied.IsDeleted)
                                {

                                    //adToBeCopied.ClearAppSiteAdQueue();

                                    foreach (var appSiteQueue in adObj.AppSiteAdQueues)
                                    {
                                        if (!adToBeCopied.ExistAppSiteAdQueue(appSiteQueue.AppSite))
                                        {
                                            adToBeCopied.AddAppSiteAdQueue(appSiteQueue.AppSite);
                                        }

                                    }
                                    for (var appSiteQueueC = 0; appSiteQueueC < adToBeCopied.AppSiteAdQueues.Count;)
                                    {
                                        if (!adObj.ExistAppSiteAdQueue(adToBeCopied.AppSiteAdQueues[appSiteQueueC].AppSite))
                                        {
                                            adToBeCopied.RemoveAppSiteAdQueue(adToBeCopied.AppSiteAdQueues[appSiteQueueC].AppSite);
                                            // var appSiteq= adToBeCopied.RemoveAppSiteAdQueueAndReturn(appSiteQueue.AppSite);
                                            // this._AppSiteAdQueueRepository.Remove(appSiteq);
                                        }
                                        else
                                        {
                                            appSiteQueueC++;
                                        }

                                    }


                                    adToBeCopied.UpdateAppSiteAdQueueType(approveAdDto.Include);
                                    adRepository.Save(adToBeCopied);
                                }
                            }
                        }
                    }
                }

                CheckAdvertiserBlock(group, adObj);

                #region data provider allow impression tracker
                if (!SearchForAllowImpressionTrackers(group, adObj, null))
                {
                    throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSave") });

                }
                if (SearchForMyAudience(group))
                {
                    throw new BusinessException(new List<ErrorData> { new ErrorData("AllowImpDPAdsSaveMyAud") });

                }

                #endregion
                CampaignRepository.Save(item);


            }
        }

        /// <summary>
        /// Get Ad Creative Summary
        /// </summary>
        /// <returns></returns>
        public AdCreativeSummaryDto GetAdSummary(CampaignIdAdgroupIdAdIdMessage request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);

            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var adItem = item.GetGroupAds(group).FirstOrDefault(ad => ad.ID == request.AdId);

                    if (adItem != null)
                    {
                        if (!IsAllowedAd(adItem))
                        {
                            throw new NotAuthorizedException();
                        }

                        //var result = new AdCreativeSummaryDto();
                        //result.AdText = adItem.AdText;
                        var summary = getAdCreativeSummaryDto(adItem);
                        summary.AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty;

                        summary.AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0;


                        summary.AdvertiserAccountName = item.AdvertiserAccount != null ? item.AdvertiserAccount.Name.ToString() : string.Empty;

                        summary.AdvertiserAccountId = item.AdvertiserAccount != null ? item.AdvertiserAccount.ID : 0;

                        CheckAdSizeToPMPDeal(summary, adItem, group);
                        return summary;
                    }

                }
            }

            return null;
        }

        public AdCreativeFullSummaryDto GetAdFullSummary(CampaignIdAdgroupIdAdIdMessage request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var adItem = item.GetGroupAds(group).FirstOrDefault(ad => ad.ID == request.AdId);
                    if (adItem != null)
                    {

                        AdCreativeFullSummaryDto summary = getAdCreativeFullSummaryDto(adItem);
                        summary.AdvertiserName = item.Advertiser != null ? item.Advertiser.Name.ToString() : string.Empty;

                        summary.AdvertiserId = item.Advertiser != null ? item.Advertiser.ID : 0;


                        summary.AdvertiserAccountName = item.AdvertiserAccount != null ? item.AdvertiserAccount.Name.ToString() : string.Empty;

                        summary.AdvertiserAccountId = item.AdvertiserAccount != null ? item.AdvertiserAccount.ID : 0;
                        CheckAdSizeToPMPDeal(summary, adItem, group);

                        return summary;
                    }
                }
            }
            return null;
        }



        private void CheckAdSizeToPMPDeal(AdCreativeSummaryDto summary, AdCreative adItem, AdGroup group)
        {
            IList<int> sizedeal = new List<int>();
            IList<int> sizeAd = new List<int>();
            IList<int> AdFormatdeal = new List<int>();
            IList<int> AdSizeintersect = new List<int>();
            var foundConflictGSize = false;
            foreach (var pmptargeting in group.Targetings.ToList().OfType<AdPMPDealTargeting>())
            {
                sizedeal = new List<int>();
                sizeAd = new List<int>();
                AdFormatdeal = new List<int>();
                AdSizeintersect = new List<int>();


                var AdSizeTargetings = pmptargeting.Deal.Targetings.ToList().OfType<AdSizePMPDealTargeting>();

                var AdTypeGroupPMPDealTargetings = pmptargeting.Deal.Targetings.ToList().OfType<AdTypeGroupPMPDealTargeting>();
                if (AdSizeTargetings != null)
                {
                    foreach (var itemadsize in AdSizeTargetings)
                    {
                        sizedeal.Add(itemadsize.AdSize.ID);

                    }
                }

                if (AdTypeGroupPMPDealTargetings != null)
                {
                    foreach (var itemadsize in AdTypeGroupPMPDealTargetings)
                    {
                        AdFormatdeal.Add((int)itemadsize.AdTypeGroup);

                    }
                }


                if (summary.CreativeUnitsContent != null)
                {
                    foreach (var itemCreat in summary.CreativeUnitsContent)
                    {
                        if (adItem.TypeId != AdTypeIds.InStreamVideo)
                        {
                            if (AdFormatdeal.Contains((int)adItem.Type.Group))
                                sizeAd.Add(itemCreat.CreativeUnitId);
                        }
                        else
                        {
                            if (itemCreat.InStreamVideoCreativeUnit != null)
                            {
                                if (AdFormatdeal.Contains((int)adItem.Type.Group))
                                    sizeAd.Add(itemCreat.InStreamVideoCreativeUnit.OriginalCreativeUnitID);
                            }

                        }
                    }
                }
                if (summary.NativeAdIcons != null)
                {

                    foreach (var itemCreat in summary.NativeAdIcons)
                    {
                        if (AdFormatdeal.Contains((int)adItem.Type.Group))
                            sizeAd.Add(itemCreat.CreativeUnitId);
                    }

                }
                if (summary.NativeAdImages != null)
                {
                    foreach (var itemCreat in summary.NativeAdImages)
                    {
                        if (AdFormatdeal.Contains((int)adItem.Type.Group))
                            sizeAd.Add(itemCreat.CreativeUnitId);
                    }
                }


                if (sizedeal.Count > 0 && (adItem.Type.Group == AdTypeGroup.Banner || adItem.TypeId == AdTypeIds.NativeAd || (adItem.TypeId == AdTypeIds.InStreamVideo && adItem.AdSubType == AdSubTypes.VideoLinear)))
                {



                    if (sizedeal.Count > 0)
                    {
                        if (sizeAd.Count > sizedeal.Count)
                        {
                            AdSizeintersect = sizeAd.Intersect(sizedeal).ToList();

                            if (AdSizeintersect.Count < sizedeal.Count)
                            {
                                foundConflictGSize = true;
                                //break;

                            }
                        }
                        else
                        {
                            AdSizeintersect = sizedeal.Intersect(sizeAd).ToList();

                            if (AdSizeintersect.Count < sizeAd.Count)
                            {

                                foundConflictGSize = true;
                                //break;
                            }
                            else if (sizeAd.Count == 0)
                            {
                                foundConflictGSize = true;
                            }
                        }
                    }
                }


                if (foundConflictGSize)
                {
                    break;
                }



            }



            if (foundConflictGSize)
            {
                if (summary.Warnings != null)
                {

                    summary.Warnings.Add(new ErrorData { ID = "PMPDealTargetingConfictSize" });
                }
                else

                {

                    summary.Warnings = new List<ErrorData>();
                }
            }

        }

        /// <summary>
        /// use this service operation to get list of Campaign Summary Objects depend on the criteria
        /// </summary>
        /// <returns>CampaignSummaryDtos that match the criteria</returns>
        public IList<CampaignSummaryDto> GetAdsSummary(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdsSummaryCriteria wcriteria)
        {


            AdsSummaryCriteria criteria = new AdsSummaryCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            ValidateAccount(Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId, Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId);

            var ads = adRepository.Query(criteria.GetExpression());



            var campaignsSummary = new List<CampaignSummaryDto>();
            var listByCampaign = (from t in ads
                                  group t by new { t.Group.Campaign }
                                      into grp
                                  select new
                                  {
                                      grp.Key.Campaign,
                                      Ads = grp.ToList()
                                  }).ToList();

            foreach (var campaign in listByCampaign)
            {

                var campaignSummary = MapperHelper.Map<CampaignSummaryDto>(campaign.Campaign);
                campaignSummary.CampaignTypeEnum = (int)campaign.Campaign.CampaignType;
                campaignSummary.AdGroupsSummary = new List<AdGroupSummaryDto>();
                var groups = (from t in campaign.Ads
                              group t by new { t.Group }
                                  into grp
                              select new
                              {
                                  grp.Key.Group,
                                  Ads = grp.ToList()
                              }).ToList();

                foreach (var @group in groups)
                {
                    var adGroupSummary = MapperHelper.Map<AdGroupSummaryDto>(@group.Group);
                    adGroupSummary.AdsSummary = new List<AdCreativeSummaryDto>();
                    foreach (var adCreative in @group.Ads)
                    {
                        var adsSummary = MapperHelper.Map<AdCreativeSummaryDto>(adCreative);
                        adsSummary.Campaign = MapperHelper.Map<CampaignsSummaryDtoBase>(adCreative.Group.Campaign);
                        adsSummary.Campaign.AdvertiserAccountId = adsSummary.AdvertiserAccountId;

                        adsSummary.Group = MapperHelper.Map<AdGroupSummaryDtoBase>(adCreative.Group);

                        adGroupSummary.AdsSummary.Add(adsSummary);
                    }
                    campaignSummary.AdGroupsSummary.Add(adGroupSummary);
                }

                campaignsSummary.Add(campaignSummary);
            }
            return campaignsSummary;
        }

        public FormattedContentDto FormatAdCreativeContent(FormatAdCreativeContentRequest request)
        {
            var adCreativeUnit = new AdCreativeUnit() { Content = request.Content };
            var adcreativeFormatter = AdCreativeContentFormatterBase.GetAdCreativeContentFormatter(adCreativeUnit);
            if (request.CreativeId > 0)
                adCreativeUnit.CreativeUnit = _CreativeUnitRepository.Get(request.CreativeId);

            adcreativeFormatter.FormatContent();

            var formattedContent = new FormattedContentDto()
            {
                Content = adcreativeFormatter.AdCreativeUnit.Content,
                IsValid = adcreativeFormatter.IsFormatted()
            };

            return formattedContent;
        }

        public ResponseDto CloneAd(CloneAdRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
                if (group != null)
                {
                    var adItem = item.GetGroupAds(group).FirstOrDefault(ad => ad.ID == request.AdId);
                    if (adItem != null)
                    {
                        if (IsAllowedAd(adItem) && (!item.IsClientLocked || Domain.Configuration.IsAdmin))
                        {
                            var clone = adItem.Clone();
                            if (!string.IsNullOrWhiteSpace(request.Name))
                            {
                                clone.Name = request.Name;
                            }
                            group.AddAd(clone);
                            CampaignRepository.Save(item);
                            return new ResponseDto { Massage = string.Format(ResourceManager.Instance.GetResource("Ad", "Clone"), clone.Name), success = true };
                        }
                        else
                        {
                            return new ResponseDto { Massage = ResourceManager.Instance.GetResource("LockedWarning", "Campaign"), success = false };


                        }
                    }
                }
            }
            return new ResponseDto { Massage = ResourceManager.Instance.GetResource("CloneAdError", "Errors"), success = false };
        }

        public ValueMessageWrapper<bool> IsFormattedAdCreativeContent(string content)
        {
            var adCreativeUnit = new AdCreativeUnit() { Content = content };
            var adcreativeFormatter = AdCreativeContentFormatterBase.GetAdCreativeContentFormatter(adCreativeUnit);
            return ValueMessageWrapper.Create(adcreativeFormatter.IsFormatted());
        }

        #endregion

        #region Private members

        private void ValidateAccount(int? accountId, int? userId)
        {
            bool isManager = IsCampaignManager();

            if (!isManager)
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
        }

        private void ValidateCampaign(Domain.Model.Campaign.Campaign campaign, bool statusCheck = false, bool validateDates = false)
        {
            bool isManager = IsCampaignManager();

            campaign.Validate(!isManager, statusCheck, validateDates);
        }

        private bool IsCampaignManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager");
        }

        private decimal GetCampaignSpend(int CampaignID)
        {
            //get total campaign spend
            var performance = campaignPerformanceRepository.Query(x => x.CampaignId == CampaignID);
            return performance.Sum(x => x.Spend);
        }

        private ReturnBid GetMinBid(TargetingSaveDto targetingSaveDto, AdGroup adGroupObj)
        {
            var bidDto = targetingSaveDto.BinInfo;

            //get Keywords
            #region Keywords
            var Keywords = new List<int>();
            if (targetingSaveDto.NewKeywords != null)
            {
                foreach (var newKeyword in targetingSaveDto.NewKeywords)
                {
                    int id;
                    if (Int32.TryParse(newKeyword, out id))
                    {
                        Keywords.Add(id);
                    }
                }
            }
            if (targetingSaveDto.DeletedKeywords != null)
            {
                foreach (var deletedKeyword in targetingSaveDto.DeletedKeywords)
                {
                    Keywords.Remove(deletedKeyword);
                }
            }
            bidDto.Keywords = Keywords.ToArray();
            #endregion

            //check if the the device Targeting is model (3,4)
            // the we need to add the paltforms and manufacturers for this models
            if (
                (targetingSaveDto.DeviceTargetingTypeId == 3) ||
                (targetingSaveDto.DeviceTargetingTypeId == 4))
            {
                var platforms = new List<int>();
                var manufacturers = new List<int>();
                platforms.AddRange(targetingSaveDto.Platforms.Keys);
                manufacturers.AddRange(targetingSaveDto.Manufacturers);
                foreach (var model in targetingSaveDto.Models)
                {
                    var deviceObj = deviceRepository.Get(model);
                    if (deviceObj != null)
                    {
                        if (!platforms.Contains(deviceObj.Platform.ID))
                        {
                            platforms.Add(deviceObj.Platform.ID);
                        }
                        if (!manufacturers.Contains(deviceObj.Manufacturer.ID))
                        {
                            manufacturers.Add(deviceObj.Manufacturer.ID);
                        }
                    }
                }
                bidDto.Platforms = platforms.ToArray();
                bidDto.Manufacturers = manufacturers.ToArray();
            }
            var parameters = MapperHelper.Map<BidParameter>(bidDto);
            return bidManager.GetBid(parameters);
        }

        private AdGroupTrackingEventResultDto GetDefaultAdGroupTrackingEvents(AdGroup adgroup, int costModelWrapperId)
        {
            var adActionType = adgroup.Objective.AdAction;
            var adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapper == null || p.CostModelWrapperEnum == (CostModelWrapperEnum)costModelWrapperId);

            var result = new AdGroupTrackingEventResultDto { TotalCount = 0, Items = new List<AdGroupTrackingEventDto>() };

            if (adActionTrackingEvents != null && adActionTrackingEvents.Count() != 0)
            {
                foreach (var item in adActionTrackingEvents
                                               .OrderBy(p => p.ID).ToList())
                {
                    AdGroupTrackingEventDto adGroupTrackingEvent = MapperHelper.Map<AdGroupTrackingEventDto>(item);
                    result.Items.Add(adGroupTrackingEvent);
                }
            }

            result.TotalCount = adActionTrackingEvents.Count();
            return result;
        }

        private void AddDefaultAdGroupTrackingEventPrerequisites(AdGroup adgroup)
        {
            var adActionType = adgroup.Objective.AdAction;
            var adActionTrackingEvents = adActionType.AdActionTrackingEvents.Where(p => p.CostModelWrapper == null || p.CostModelWrapperEnum == adgroup.CostModelWrapperEnum);

            foreach (var item in adActionTrackingEvents)
            {
                var adGroupTrackingEvent = adgroup.GetTrackingEvents().Where(p => p.Code == item.Event.Code).SingleOrDefault();
                if(adGroupTrackingEvent!=null)
                { 
                        string preRequisite = string.Join(",", adgroup.GetTrackingEvents().Where(p => item.GetAllPrerequisitesCodes().Contains(p.Code)).Select(p => p.ID));

                        if (!string.IsNullOrEmpty(preRequisite))
                        {
                            adGroupTrackingEvent.PreRequisites = preRequisite;
                        }
                }
            }
        }



        private void AddDefaultAdGroupTrackingEvent(AdGroup adgroup, int costModelWrapperId, int? oldCostModelWrapper)
        {
            List<string> oldAdGroupTrackingEvents = new List<string>();

            if (oldCostModelWrapper.HasValue && oldCostModelWrapper != adgroup.CostModelWrapper.ID)
            {
                foreach (var trackingEvent in adgroup.TrackingEvents.Where(p => !p.IsDeleted))
                {

                    var result = IsDeleteTrackingEventAllowed( new IsDeleteTrackingEventAllowedRequest { CampaignId = adgroup.Campaign.ID, AdgroupId = adgroup.ID, AdGroupTrackingEventCodes = new List<string>() { trackingEvent.Code }, CheckStandards = false, NewCostModelWrapperId = costModelWrapperId });

                    if (result.Value.Key)
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
                    adGroupTrackingEvent.IsTracking = true;

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

                    if (item.Event.IsConversion)
                    {

                        // AdGroupConversionEvent adGroupConversionEvent = MapperHelper.Map<AdGroupConversionEvent>(item);
                        adGroupTrackingEvent.IsConversion = true;
                        // adGroupConversionEvent.IsTracking = true;
                        //adGroupTrackingEvent.
                        if (adGroupTrackingEvent.IsBillable)
                        {
                            adGroupTrackingEvent.IsPrimary = true;
                        }
                        //adgroup.ConversionEvents.Add(adGroupConversionEvent);
                    }


                }
            }
        }


        public void AddDefaultAdGroupTrackingEventById(AddDefaultAdGroupTrackingEventByIdRequest request)
        {

        var adgroupObj=     adGroupRepository.Get(request.AdGroupId);
            adgroupObj.CostModelWrapper = new CostModelWrapper { ID= request.CostModelWrapperId };
            AddDefaultAdGroupTrackingEvent(adgroupObj, request.CostModelWrapperId, request.OldCostModelWrapper);
        }
        private void SaveAdGroupTrackingEventsPrerequisites(Domain.Model.Campaign.Campaign campaign, AdGroup adgroup, Dictionary<string, List<string>> codePreprequisites)
        {
            CheckCampaign(campaign);
            ValidateCampaign(campaign);

            if (campaign.IsValid)
            {
                if (adgroup != null && !adgroup.IsDeleted)
                {
                    if (!adgroup.IsDefaultPrerequisitesSaved)
                    {
                        AddDefaultAdGroupTrackingEventPrerequisites(adgroup);
                        adgroup.IsDefaultPrerequisitesSaved = true;
                    }

                    foreach (var item in codePreprequisites)
                    {
                        if (!string.IsNullOrEmpty(item.Key))
                        {
                            var trackingEvent = adgroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == item.Key).SingleOrDefault();

                            if (trackingEvent != null)
                            {
                                var prerequisitiesIds = "";

                                if (item.Value != null)
                                    prerequisitiesIds = string.Join(",", adgroup.TrackingEvents.Where(p => !p.IsDeleted && item.Value.Contains(p.Code)).Select(p => p.ID));

                                if (!string.IsNullOrEmpty(prerequisitiesIds))
                                {
                                    trackingEvent.PreRequisites = prerequisitiesIds;
                                }
                                else
                                {
                                    trackingEvent.PreRequisites = null;
                                }
                            }
                        }
                    }
                }

                CampaignRepository.Save(campaign);
            }
        }


        private void AddAdGroupTrackingEvent(AdGroup adGroup, AdGroupTrackingEventSaveDto trackingEventSaveDto)
        {
            IList<int> intList = new List<int>();
            if (adGroup != null && !adGroup.IsDeleted)
            {

                // One Is Billable tracking event is allowed for each adgroup
                if (adGroup.TrackingEvents.Any(p => p.IsBillable && !p.IsDeleted))
                {
                    if (trackingEventSaveDto.IsBillable)
                    {
                        throw new BusinessException(new List<ErrorData>() { new ErrorData("DuplicateIsBillable") });
                    }
                }

                var adGroupTrackingEvent = MapperHelper.Map<AdGroupTrackingEvent>(trackingEventSaveDto);
                adGroupTrackingEvent.IsCustom = true;
                adGroupTrackingEvent.AdGroup = adGroup;
                adGroupTrackingEvent.IsTracking = true;
               
                int mos = 0;

                if (trackingEventSaveDto.SegmentsId!=null)
                {
                    intList = trackingEventSaveDto.SegmentsId.Split(',')
                        .Where(m => int.TryParse(m, out mos))
                        .Select(m => int.Parse(m))
                        .ToList();
                }
                if (adGroup.TrackingEvents == null)
                    adGroup.TrackingEvents = new List<AdGroupTrackingEvent>();

                string columnPrefixName = Configuration.StatisticsColumnPrefixName;

                for (int i = 1; i <= Configuration.MaxAdGroupTrackingEvents; i++)
                {
                    string statisticsColumnName = string.Format("{0}{1}", columnPrefixName, i);
                    if (!adGroup.TrackingEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName) && !adGroup.ConversionEvents.Any(p => !p.IsDeleted && p.StatisticsColumnName == statisticsColumnName))
                    {
                        adGroupTrackingEvent.StatisticsColumnName = statisticsColumnName;
                        break;
                    }
                }

                //checkSystemEventFraud 'back end' the logic here is diffrent form a named with same name . we need to check if either a name or a code of any system event is being used , with a different compatible , which means that no name should has a not owned code , and the same goes for the code
                var SystemTrackingEventCode = trackingEventRepository.Query(x => x.Code == adGroupTrackingEvent.Code && x.EventName != adGroupTrackingEvent.Description).FirstOrDefault();
                var SystemTrackingEventName = trackingEventRepository.Query(x => x.Code != adGroupTrackingEvent.Code && x.EventName == adGroupTrackingEvent.Description).FirstOrDefault();
                /*if (SystemTrackingEventCode != null || SystemTrackingEventName != null)
                {
                    throw new BusinessException(new List<ErrorData>() { new ErrorData("TrackinfEvnetsFraud") });

                }*/

                //now if we are adding any already exits event and it's associated with a the current adgroup  , then we only need to update it not add it again !
                var orginalEvent = adGroupTrackingEventRepository.Query(p => p.Code == adGroupTrackingEvent.Code && p.AdGroup.ID == adGroup.ID && p.IsDeleted==false).FirstOrDefault();
                var orginalEventConv = _adGroupEventRepository.Query(p => p.Code == adGroupTrackingEvent.Code && p.AdGroup.ID == adGroup.ID && p.IsDeleted == false).FirstOrDefault();
                
                if (orginalEvent != null)
                {
                    orginalEvent.IsDeleted = false;
                    orginalEvent.AllPreRequisitesRequired = adGroupTrackingEvent.AllPreRequisitesRequired;
                    orginalEvent.AllowDuplicate = adGroupTrackingEvent.AllowDuplicate;
                    orginalEvent.IsTracking = adGroupTrackingEvent.IsTracking;
                    orginalEvent.IsConversion = adGroupTrackingEvent.IsConversion;
                    orginalEvent.IsBillable = adGroupTrackingEvent.IsBillable;
                    orginalEvent.PreRequisites = adGroupTrackingEvent.PreRequisites;
                    orginalEvent.StatisticsColumnName = adGroupTrackingEvent.StatisticsColumnName;
                    orginalEvent.ValidFor = adGroupTrackingEvent.ValidFor;
                    if (orginalEvent.AudienceSegmentListsMap == null)
                    {

                        orginalEvent.AudienceSegmentListsMap = new List<AudienceSegmentEventMap>();
                    }
                    foreach (var item in intList)
                    {

                        if (orginalEvent.AudienceSegmentListsMap.Where(M => M.AudienceSegment.ID == item).SingleOrDefault() == null)
                        {
                            orginalEvent.AudienceSegmentListsMap.Add(new AudienceSegmentEventMap { AudienceSegment = new AudienceSegment { ID = item }, Event = orginalEvent });
                        }

                    }

                    for (var i = 0; i < orginalEvent.AudienceSegmentListsMap.Count; i++)
                    {

                        if (!intList.Contains(orginalEvent.AudienceSegmentListsMap[i].AudienceSegment.ID))
                        {
                            orginalEvent.AudienceSegmentListsMap.Remove(orginalEvent.AudienceSegmentListsMap[i]);
                            if (i != 0)
                            {
                                i--;
                            }

                        }
                    }
                }
                else if (orginalEventConv!=null)
                {
                   
                    AdGroupTrackingEvent trackingEvent = new AdGroupTrackingEvent();
                    trackingEvent.ID = orginalEventConv.ID;
                    trackingEvent.IsTracking = true;
                    orginalEventConv.IsTracking = true;




                    trackingEvent.AllPreRequisitesRequired = adGroupTrackingEvent.AllPreRequisitesRequired;
                    trackingEvent.AllowDuplicate = adGroupTrackingEvent.AllowDuplicate;


                    trackingEvent.IsBillable = adGroupTrackingEvent.IsBillable;
                    trackingEvent.PreRequisites = adGroupTrackingEvent.PreRequisites;


                 




                    if (trackingEvent.PixelListsMap != null && trackingEvent.PixelListsMap.Count > 0)
                    {
                        trackingEvent.PixelListsMap = new List<PixelEventMap>();
                        foreach (var AudienceSegment in trackingEvent.PixelListsMap)
                        {

                            trackingEvent.PixelListsMap.Add(
                                new PixelEventMap
                                {
                                    IsDeleted = AudienceSegment.IsDeleted,
                                    Event = trackingEvent,
                                    Pixel = AudienceSegment.Pixel


                                }


                                );
                        }

                    }

                    if (trackingEvent.AudienceSegmentListsMap != null && trackingEvent.AudienceSegmentListsMap.Count > 0)
                    {
                        trackingEvent.AudienceSegmentListsMap = new List<AudienceSegmentEventMap>();
                        foreach (var AudienceSegment in trackingEvent.AudienceSegmentListsMap)
                        {

                            trackingEvent.AudienceSegmentListsMap.Add(
                                new AudienceSegmentEventMap
                                {
                                    IsDeleted = AudienceSegment.IsDeleted,
                                    Event = trackingEvent,
                                    AudienceSegment = AudienceSegment.AudienceSegment


                                }


                                );
                        }

                    }

                //    _adGroupEventRepository.Save(orginalEventConv);
                    //  adGroup.TrackingEvents.Add(trackingEvent);
                    adGroupTrackingEventRepository.Save(trackingEvent);
                }
                else
                {
                    if (intList != null && intList.Count > 0) {
                        adGroupTrackingEvent.AudienceSegmentListsMap = new List<AudienceSegmentEventMap>();

                        foreach (var item in intList)
                        {

                            adGroupTrackingEvent.AudienceSegmentListsMap.Add(new AudienceSegmentEventMap { AudienceSegment = new AudienceSegment { ID = item }, Event = adGroupTrackingEvent });
                        }

                    }
                    adGroup.TrackingEvents.Add(adGroupTrackingEvent);
                }

                // we dont need to add it into 'account tracking events table' , or event loop through all similar tracking events if it's a system event, so lets do a check !
                var SystemTrackingEvent = trackingEventRepository.Query(x => x.Code == adGroupTrackingEvent.Code ).FirstOrDefault();

                if (SystemTrackingEvent!=null && SystemTrackingEvent.IsConversion)
                {
                    adGroupTrackingEvent.IsConversion = true;
                
                }

                if (SystemTrackingEvent == null)
                {

                    // now after we updated it just for this adgruop apart of the name , we want to change the name for this adgruop and the remaining adgruops just in case it's name has changed 
                    var orginalEventList = adGroupTrackingEventRepository.Query(p => p.Code == adGroupTrackingEvent.Code).ToList();
                    if (orginalEventList != null && orginalEventList.Count != 0)
                    {
                        foreach (AdGroupTrackingEvent Event in orginalEventList)
                        {
                            Event.Description = adGroupTrackingEvent.Description;
                            adGroupTrackingEventRepository.Save(Event);
                        }
                    }

                    // lets update Account Events
                    updateAccountTrackingEvents(adGroup, adGroupTrackingEvent);
                }

            }
        }
        public void UpdateAdGroupTrackingEvent( AdGroupTrackingEventSaveDto trackingEventSaveDto)
        {
            IList<int> intList = new List<int>();

            var adGroupTrackingEvent = MapperHelper.Map<AdGroupTrackingEvent>(trackingEventSaveDto);


            int mos = 0;

            if (trackingEventSaveDto.SegmentsId != null)
            {
                intList = trackingEventSaveDto.SegmentsId.Split(',')
                    .Where(m => int.TryParse(m, out mos))
                    .Select(m => int.Parse(m))
                    .ToList();
            }



            var orginalEvent = adGroupTrackingEventRepository.Query(p => p.ID== trackingEventSaveDto.ID).FirstOrDefault();
            if (orginalEvent != null)
            {
              
                if (orginalEvent.AudienceSegmentListsMap == null)
                {

                    orginalEvent.AudienceSegmentListsMap = new List<AudienceSegmentEventMap>();
                }
                foreach (var item in intList)
                {

                    if (orginalEvent.AudienceSegmentListsMap.Where(M => M.AudienceSegment.ID == item).SingleOrDefault() == null)
                    {
                        orginalEvent.AudienceSegmentListsMap.Add(new AudienceSegmentEventMap { AudienceSegment = new AudienceSegment { ID = item }, Event = orginalEvent });
                    }

                }

                for (var i = 0; i < orginalEvent.AudienceSegmentListsMap.Count; i++)
                {

                    if (!intList.Contains(orginalEvent.AudienceSegmentListsMap[i].AudienceSegment.ID))
                    {
                        orginalEvent.AudienceSegmentListsMap.Remove(orginalEvent.AudienceSegmentListsMap[i]);
                        if (i != 0)
                        {
                            i--;
                        }

                    }
                }
            }

           
        }
        private void updateAccountTrackingEvents(AdGroup adGroup, AdGroupTrackingEvent adGroupTrackingEvent)
        {
            //add it to "account tracking event" table , witch acts as a cach table 
            var Event = AccountTrackingEventRepository.Query(p => p.Code == adGroupTrackingEvent.Code).FirstOrDefault();
            ArabyAds.AdFalcon.Domain.Model.Account.AccountTrackingEvents AccountTrackingEvent = null;
            //already exists
            if (Event != null)
            {
                Event.Description = adGroupTrackingEvent.Description;
                AccountTrackingEventRepository.Save(Event);
            }
            else // new 
            {

                //MapperHelper.Map<ArabyAds.AdFalcon.Domain.Model.Account.AccountTrackingEvents>(adGroupTrackingEvent);
                AccountTrackingEvent = new AccountTrackingEvents();
                if (AccountTrackingEvent != null)
                {
                    AccountTrackingEvent.Description = adGroupTrackingEvent.Description;
                    AccountTrackingEvent.Code = adGroupTrackingEvent.Code;
                    AccountTrackingEvent.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                    AccountTrackingEvent.UserID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
                    AccountTrackingEventRepository.Save(AccountTrackingEvent);
                }
            }
        }

        private void updateAccountTrackingEventsSupportConversion(AdGroup adGroup, AdGroupConversionEvent adGroupTrackingEvent)
        {
            //add it to "account tracking event" table , witch acts as a cach table 
            var Event = AccountTrackingEventRepository.Query(p => p.Code == adGroupTrackingEvent.Code).FirstOrDefault();
            ArabyAds.AdFalcon.Domain.Model.Account.AccountTrackingEvents AccountTrackingEvent = null;
            //already exists
            if (Event != null)
            {
                Event.Description = adGroupTrackingEvent.Description;
                AccountTrackingEventRepository.Save(Event);
            }
            else // new 
            {
                AccountTrackingEvent = MapperHelper.Map<ArabyAds.AdFalcon.Domain.Model.Account.AccountTrackingEvents>(adGroupTrackingEvent);
               // AccountTrackingEvent.Code = "Conv" + GetConversionCounter();

                if (AccountTrackingEvent != null)
                {
                    AccountTrackingEvent.IsConversion = true;
                    AccountTrackingEvent.AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
                    AccountTrackingEvent.UserID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;
                    AccountTrackingEventRepository.Save(AccountTrackingEvent);
                }
            }
        }

        private int GetConversionCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "My Conversions");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        public ValueMessageWrapper<bool> checkSystemEventFraud(CheckSystemEventFraudRequest request)
        {
            var SystemTrackingEvent = trackingEventRepository.GetAll().ToList();
            var result = SystemTrackingEvent.Where(x => x.Code == request.Code || x.GetDescription() == request.Name).FirstOrDefault();
            return ValueMessageWrapper.Create(result != null);
        }
        private bool CheckIfTrackingEventPrerequisiteDeleteIsAllowed(AdGroup adGroup, IList<int> deletedIds)
        {
            if (adGroup != null && adGroup.TrackingEvents != null && deletedIds != null)
            {
                foreach (var item in deletedIds)
                {
                    var trackingEvent = adGroup.TrackingEvents.Where(p => !p.IsDeleted && p.ID == item).SingleOrDefault();

                    if (trackingEvent != null)
                    {
                        var affectedItems = adGroup.TrackingEvents.Where(p => !p.IsDeleted && p.PreRequisitesList.Contains(trackingEvent.ID)).ToList();

                        if (affectedItems != null && affectedItems.Count != 0)
                        {
                            foreach (var affectedItem in affectedItems)
                            {
                                if (!deletedIds.Contains(affectedItem.ID))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        private bool CheckIfTrackingEventPrerequisiteDeleteIsAllowed(AdGroup adGroup, IList<string> deletedIds)
        {
            if (adGroup != null && adGroup.TrackingEvents != null && deletedIds != null)
            {
                foreach (var item in deletedIds)
                {
                    var trackingEvent = adGroup.TrackingEvents.Where(p => !p.IsDeleted && p.Code == item).SingleOrDefault();

                    if (trackingEvent != null)
                    {
                        var affectedItems = adGroup.TrackingEvents.Where(p => !p.IsDeleted && p.PreRequisitesList.Contains(trackingEvent.ID)).ToList();

                        if (affectedItems != null && affectedItems.Count != 0)
                        {
                            foreach (var affectedItem in affectedItems)
                            {
                                if (!deletedIds.Contains(affectedItem.Code))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        private bool IsAdChanged(AdCreative adItem, AdCreativeSaveDto obj)
        {

            if (adItem.TypeId != AdTypeIds.TrackingAd)
            {
                if (adItem.AdText != obj.AdText)
                {
                    return true;
                }
            }
            //if (adItem.Name != obj.Name)
            //{
            //    return true;
            //}
            if (obj.AdBannerType != null && adItem.CretiveUnitDeviceType != DeviceTypeEnum.Any)
            {
                if (adItem.CretiveUnitDeviceType != obj.AdBannerType)
                {
                    return true;
                }
            }
            if (adItem.EnvironmentType != obj.EnvironmentType)
            {
                return true;
            }
            if (adItem.OrientationType != obj.OrientationType)
            {
                return true;
            }
            if (adItem.ActionValue != null && obj.AdActionValue != null)
            {
                if (adItem.ActionValue.Value != obj.AdActionValue.Value)
                {
                    return true;
                }
                if (adItem.ActionValue.Value2 != obj.AdActionValue.Value2)
                {
                    return true;
                }
                var trackers = adItem.ActionValue.Trackers.Where(p => !p.IsDeleted).ToList();

                if (obj.AdActionValue.Trackers != null)
                {
                    if (trackers.Count != obj.AdActionValue.Trackers.Count)
                    {
                        return true;
                    }
                }

                if (obj.AdActionValue.Trackers != null)
                {
                    for (int i = 0; i < trackers.Count; i++)
                    {
                        if (trackers[i].Url != obj.AdActionValue.Trackers[i].URL)
                        {
                            return true;
                        }
                    }
                }

            }
            var isExist = 0;
            switch (adItem.TypeId)
            {
                case AdTypeIds.Text:
                    {
                        TextCreative textCreative = (TextCreative)adItem;
                        if (textCreative.TileImage.ID != obj.TileImageId)
                        {
                            return true;
                        }
                        TileImageInformationDto tabletTileImageDocumentDto = new TileImageInformationDto();
                        TileImageInformationDto phoneTileImageDocumentDto = new TileImageInformationDto();
                        AdCreativeUnit smartPhoneUnit = null;
                        AdCreativeUnit tabletUnit = null;
                        var phoneTileImageDocument = textCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).FirstOrDefault();
                        var tabletTileImageDocument = textCreative.TileImage.Images.Where(p => p.TileImageSize.DeviceType.ID == (int)DeviceTypeEnum.Tablet).FirstOrDefault();

                        if (phoneTileImageDocument != null)
                            phoneTileImageDocumentDto = obj.TileImageInformationList.Where(p => p.TileImage.Document.ID == phoneTileImageDocument.Document.ID).SingleOrDefault();
                        if (tabletTileImageDocument != null)
                            tabletTileImageDocumentDto = obj.TileImageInformationList.Where(p => p.TileImage.Document.ID == tabletTileImageDocument.Document.ID).SingleOrDefault();

                        smartPhoneUnit = textCreative.GetCreativeUnits().Where(y => y.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.SmartPhone).FirstOrDefault();
                        tabletUnit = textCreative.GetCreativeUnits().Where(y => y.CreativeUnit.DeviceType.ID == (int)DeviceTypeEnum.Tablet).FirstOrDefault();

                        if (phoneTileImageDocumentDto != null && smartPhoneUnit.Trackers.Count > 0 && smartPhoneUnit.Trackers[0].TrackingUrl != phoneTileImageDocumentDto.ImpressionTrackerRedirect)
                        {
                            return true;
                        }
                        if (tabletTileImageDocumentDto != null && tabletUnit.Trackers.Count > 0 && tabletUnit.Trackers[0].TrackingUrl != tabletTileImageDocumentDto.ImpressionTrackerRedirect)
                        {
                            return true;
                        }
                        if (tabletTileImageDocumentDto != null && tabletUnit.Trackers.Count <= 0 && !string.IsNullOrEmpty(tabletTileImageDocumentDto.ImpressionTrackerRedirect))
                        {
                            return true;
                        }

                        if (phoneTileImageDocumentDto != null && smartPhoneUnit.Trackers.Count > 0 && smartPhoneUnit.Trackers[0].TrackingJS != phoneTileImageDocumentDto.ImpressionTrackerJSRedirect)
                        {
                            return true;
                        }
                        if (tabletTileImageDocumentDto != null && tabletUnit.Trackers.Count > 0 && tabletUnit.Trackers[0].TrackingJS != tabletTileImageDocumentDto.ImpressionTrackerJSRedirect)
                        {
                            return true;
                        }
                        if (tabletTileImageDocumentDto != null && tabletUnit.Trackers.Count <= 0 && !string.IsNullOrEmpty(tabletTileImageDocumentDto.ImpressionTrackerJSRedirect))
                        {
                            return true;
                        }
                        ; break;
                    }
                case AdTypeIds.Banner:
                    {
                        BannerCreative bannerCreative = (BannerCreative)adItem;
                        if (bannerCreative.AdCreativeUnits.Count != obj.Banners.Count)
                        {
                            return true;
                        }

                        var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList();
                        if (adCreativeUnits.Count == obj.Banners.Count)
                        {
                            for (int i = 0; i < adCreativeUnits.Count; i++)
                            {
                                //if (adCreativeUnits[i].Document != null && adCreativeUnits[i].Document.ID.ToString() != obj.Banners[i].Content)
                                //{
                                //    return true;
                                //}
                                if (adCreativeUnits[i].Document != null)
                                {
                                    isExist = obj.Banners.Where(x => x.Content == adCreativeUnits[i].Document.ID.ToString()).Count();
                                    if (isExist <= 0)
                                        return true;
                                }

                                if (adCreativeUnits[i].Trackers.Count == 0 && !string.IsNullOrEmpty(obj.Banners[i].ImpressionTrackerRedirect))
                                {
                                    return true;
                                }
                                if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingUrl != obj.Banners[i].ImpressionTrackerRedirect)
                                {
                                    return true;
                                }

                                if (adCreativeUnits[i].Trackers.Count == 0 && !string.IsNullOrEmpty(obj.Banners[i].ImpressionTrackerJSRedirect))
                                {
                                    return true;
                                }
                                if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingJS != obj.Banners[i].ImpressionTrackerJSRedirect)
                                {
                                    return true;
                                }
                            }
                        }; break;

                    }
                case AdTypeIds.TrackingAd:
                    {
                        AdTrackerCreative adTrackerCreative = (AdTrackerCreative)adItem;

                        if (adTrackerCreative.GetReadableBid() != obj.Bid)
                        {
                            return true;
                        }

                        //if (adTrackerCreative.AdCreativeUnits.Count != obj.Banners.Count)
                        //{
                        //    return true;
                        //}
                        //var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList();
                        //if (adCreativeUnits.Count == obj.Banners.Count)
                        //{
                        //    for (int i = 0; i < adCreativeUnits.Count; i++)
                        //    {
                        //        //if (adCreativeUnits[i].Document != null && adCreativeUnits[i].Document.ID.ToString() != obj.Banners[i].Content)
                        //        //{
                        //        //    return true;
                        //        //}
                        //        if (adCreativeUnits[i].Document != null)
                        //        {
                        //            isExist = obj.Banners.Where(x => x.Content == adCreativeUnits[i].Document.ID.ToString()).Count();
                        //            if (isExist <= 0)
                        //                return true;
                        //        }

                        //        if (adCreativeUnits[i].Trackers.Count == 0 && !string.IsNullOrEmpty(obj.Banners[i].ImpressionTrackerRedirect))
                        //        {
                        //            return true;
                        //        }
                        //        if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingUrl != obj.Banners[i].ImpressionTrackerRedirect)
                        //        {
                        //            return true;
                        //        }
                        //    }
                        //}; 

                        break;

                    }
                case AdTypeIds.NativeAd:
                    {
                        NativeAdCreative nativeAd = (NativeAdCreative)adItem;

                        if (nativeAd.Description != obj.Description)
                        {
                            return true;
                        }
                        if (nativeAd.ActionText != obj.ActionText)
                        {
                            return true;
                        }
                        if (nativeAd.AppOpenUrl != obj.AppUrl)
                        {
                            return true;
                        }

                        if (nativeAd.Images.Count != obj.NativeAdImages.Count)
                        {
                            return true;
                        }
                        for (int i = 0; i < nativeAd.Images.Count; i++)
                        {
                            //if (nativeAd.Images[i].Document != null && nativeAd.Images[i].Document.ID.ToString() != obj.NativeAdImages[i].Content)
                            //{
                            //    return true;
                            //}
                            if (nativeAd.Images[i].Document != null)
                            {
                                isExist = obj.NativeAdImages.Where(x => x.Content == nativeAd.Images[i].Document.ID.ToString()).Count();
                                if (isExist <= 0)
                                    return true;
                            }
                        }

                        if (nativeAd.Icons.Count != obj.NativeAdIcons.Count)
                        {
                            return true;
                        }
                        for (int i = 0; i < nativeAd.Icons.Count; i++)
                        {
                            if (nativeAd.Icons[i].Document != null)
                            {
                                isExist = obj.NativeAdIcons.Where(x => x.Content == nativeAd.Icons[i].Document.ID.ToString()).Count();
                                if (isExist <= 0)
                                    return true;
                            }
                        }
                        var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList();
                        if (obj.Banners.Count == 0 && adCreativeUnits.Count > 0 && adCreativeUnits[0].Trackers.Count > 0 && !string.IsNullOrEmpty(adCreativeUnits[0].Trackers[0].TrackingUrl))
                        {
                            return true;
                        }
                        if (obj.Banners.Count > 0)
                        {
                            for (int i = 0; i < adCreativeUnits.Count; i++)
                            {
                                if (adCreativeUnits[i].Trackers.Count == 0 && obj.Banners.Count > 0)
                                {
                                    return true;
                                }
                                if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingUrl != obj.Banners[i].ImpressionTrackerRedirect)
                                {
                                    return true;
                                }

                                if (adCreativeUnits[i].Trackers.Count == 0 && obj.Banners.Count > 0)
                                {
                                    return true;
                                }
                                if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingJS != obj.Banners[i].ImpressionTrackerJSRedirect)
                                {
                                    return true;
                                }
                            }
                        }
                        ; break;
                    }
                case AdTypeIds.RichMedia:
                    {
                        RichMediaCreative richmedia = (RichMediaCreative)adItem;
                        if (obj.RichMediaRequiredProtocol.HasValue)
                        {
                            if (richmedia.GetRichMediaProtocol() != null && richmedia.GetRichMediaProtocol().ID != obj.RichMediaRequiredProtocol.Value)
                            {
                                return true;
                            }
                            if (richmedia.GetRichMediaProtocol() == null && obj.RichMediaRequiredProtocol.HasValue && obj.RichMediaRequiredProtocol.Value != (int)RichMediaProtocols.None)
                            {
                                return true;
                            }
                        }

                        if (richmedia.ClickMethod != obj.ClickMethod)
                            {
                            return true;
                        }
                        var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList(); ;
                        if (adCreativeUnits.Count != obj.Banners.Count)
                        {
                            return true;
                        }

                        for (int i = 0; i < adCreativeUnits.Count; i++)
                        {
                            switch (adItem.AdSubType)
                            {
                                case AdSubTypes.ExpandableRichMedia:
                                    {

                                        if (adCreativeUnits[i].Document != null)
                                        {
                                            isExist = obj.Banners.Where(x => x.Content == adCreativeUnits[i].Document.ID.ToString()).Count();
                                            if (isExist <= 0)
                                                return true;
                                        }
                                        break;
                                    }
                                case AdSubTypes.HTML5Interstitial:
                                    
                                case AdSubTypes.HTML5RichMedia:
                                    {

                                        if (adCreativeUnits[i].Document != null)
                                        {
                                            isExist = obj.Banners.Where(x => x.Content == adCreativeUnits[i].Document.ID.ToString()).Count();
                                            if (isExist <= 0)
                                                return true;
                                        }
                                        break;
                                    }
                                case AdSubTypes.JavaScriptRichMedia:
                                case AdSubTypes.ExternalUrlInterstitial:
                                case AdSubTypes.JavaScriptInterstitial:
                                    {
                                        if (!string.IsNullOrEmpty(adCreativeUnits[i].Content) && adCreativeUnits[i].Content != obj.Banners[i].Content)
                                        {
                                            return true;
                                        }
                                        break;
                                    }
                            }

                        }
                    }; break;
                case AdTypeIds.PlainHTML:
                    {
                        PlainHtmlCreative nativeAd = (PlainHtmlCreative)adItem;
                        var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList();
                        if (adCreativeUnits.Count != obj.Banners.Count)
                        {
                            return true;
                        }
                        for (int i = 0; i < adCreativeUnits.Count; i++)
                        {
                            if (adCreativeUnits[i].Content != obj.Banners[i].Content)
                            {
                                return true;
                            }
                        }
                    }; break;
                case AdTypeIds.InStreamVideo:
                    {

                        var adCreativeUnits = adItem.GetCreativeUnits().Where(x => !x.IsDeleted).ToList();
                        for (int i = 0; i < adCreativeUnits.Count; i++)
                        {
                            if (adCreativeUnits[i].Document != null && adCreativeUnits[i].Document.ID != obj.InStreamVideos[i].DocumentId)
                            {
                                return true;
                            }
                            var inStreamVideoCreativeUnit = obj.InStreamVideos[i].InStreamVideoCreativeUnit;
                            if (inStreamVideoCreativeUnit.ImpressionTrackerRedirectList != null && inStreamVideoCreativeUnit.ImpressionTrackerRedirectList.Count > 0)
                            {
                                if (adCreativeUnits[i].Trackers.Count == 0 && inStreamVideoCreativeUnit.ImpressionTrackerRedirectList.Count > 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    foreach (AdCreativeUnitTrackerDto unitTrackerDto in inStreamVideoCreativeUnit.ImpressionTrackerRedirectList)
                                    {
                                        var eventTracker = adItem.Group.TrackingEvents.Where(p => p.ID == unitTrackerDto.AdGroupEventId && !p.IsDeleted).FirstOrDefault();

                                        var creativeUnitTrackingEvents = adCreativeUnits[i].Trackers.Where(p => string.Equals(p.AdGroupEvent.Code, eventTracker.Code, StringComparison.InvariantCultureIgnoreCase) && !p.IsDeleted).ToList();

                                        var trackingEventUrls = unitTrackerDto.ImpressionURls.Select(x => x.URL).ToList();
                                        if (creativeUnitTrackingEvents.Count != trackingEventUrls.Count)
                                        {
                                            return true;
                                        }
                                        for (int uIndex = 0; uIndex < creativeUnitTrackingEvents.Count; uIndex++)
                                        {
                                            if (creativeUnitTrackingEvents[uIndex].TrackingUrl != trackingEventUrls[uIndex])
                                            {
                                                return true;
                                            }
                                        }
                                    }

                                    //for (int x = 0; x < inStreamVideoCreativeUnit.ImpressionTrackerRedirectList.Count; x++)
                                    //{
                                    //    //   if (adCreativeUnits[i].Trackers[x].CreativeUnit.InStreamVideoCreativeUnit.  InStreamVideoCreativeUnit.ImpressionTrackerRedirectList[x].ImpressionURls.Count != )
                                    //}
                                }
                                //if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingUrl != inStreamVideoCreativeUnit.ImpressionTrackerRedirectList[i].Url)
                                //{
                                //    return true;
                                //}
                            }
                        }

                        //if (obj.Banners.Count > 0)
                        //{
                        //    for (int i = 0; i < adCreativeUnits.Count; i++)
                        //    {
                        //        if (adCreativeUnits[i].Trackers.Count == 0 && obj.Banners.Count > 0)
                        //        {
                        //            return true;
                        //        }
                        //        if (adCreativeUnits[i].Trackers.Count > 0 && adCreativeUnits[i].Trackers[0].TrackingUrl != obj.Banners[i].ImpressionTrackerRedirect)
                        //        {
                        //            return true;
                        //        }
                        //    }
                        //}
                    }
                    break;

            }

            return false;

        }
        #endregion

        /// <summary>
        /// Check campaing pricing model compatable with appsite
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="appsites">
        ///appsites which should be checking if pricing model compatible with campaign pricing model
        /// </param>
        /// <param name="notCompatibleAppSiteList"></param>
        /// <param name="goupId"></param>
        /// <param name="groupCostModelWrapperID"></param>
        /// <param name="checkExisting"></param>
        /// <returns></returns>
        public CheckAppsitCostModelCompatableWitCampaignsResponse CheckAppsitesCostModelCompatableWitCampaign(CheckAppsitesCostModelCompatableWitCampaignRequest request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            var response = new CheckAppsitCostModelCompatableWitCampaignsResponse();
            response.NotCompatableCampaigns = new List<CampaignBidConfigDto>();
            List<int> adGroupIds = new List<int>();
            if (request.Appsites == null)
            { request.Appsites = new List<int>(); }

            if (item.IsValid)
            {
                if (request.AdGroupId.HasValue)
                {
                    adGroupIds.Add(request.AdGroupId.Value);
                }
                else
                {
                    adGroupIds = item.AdGroups.Select(x => x.ID).ToList();
                }
                List<int> allAppsites = new List<int>();
                allAppsites.AddRange(request.Appsites);
                foreach (int adGoupId in adGroupIds)
                {
                    var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGoupId);
                    if (group != null)
                    {
                        if (!request.GroupCostModelWrapperID.HasValue)
                        {
                            if (group.CostModelWrapper == null)
                                continue;

                            request.GroupCostModelWrapperID = group.CostModelWrapper.CostModel.ID;
                        }


                        if (request.CheckExisting)
                        {
                            allAppsites.AddRange(GetCampaignAllAssignedAppsites(request.CampaignId, adGoupId));
                            //  IEnumerable<int>[] existingAppsites = group.Ads.Select(x => x.AppSiteAdQueues.Select(q => q.AppSite.ID)).ToArray();
                        }
                        ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appsite = null;
                        CampaignBidConfigDto campaignBidConfigDto = null;

                        foreach (int appSiteId in allAppsites.Distinct())
                        {
                            appsite = appSiteRepository.Get(appSiteId);
                            if (appsite.IsDeleted)
                            {
                                continue;
                            }
                            var appsitePricingModel = appsite.AppSiteServerSetting.GetPricingModel();

                            if (appsitePricingModel != null && appsitePricingModel.ID != request.GroupCostModelWrapperID)
                            {
                                var adGroupBidConfigs = group.GetCampaignBidConfigs().Where(x => x.AppSite.ID == appsite.ID).ToList();
                                if (adGroupBidConfigs.Count > 0)
                                {
                                    foreach (var adGroupBidConfig in adGroupBidConfigs)
                                    {
                                        if (adGroupBidConfig != null)
                                        {

                                            campaignBidConfigDto = MapperHelper.Map<CampaignBidConfigDto>(adGroupBidConfig);
                                            //make the bid user readable by multiply it with it's factor
                                            campaignBidConfigDto.Bid = adGroupBidConfig.GetUserReadableValue();


                                        }
                                        response.NotCompatableCampaigns.Add(campaignBidConfigDto);
                                    }
                                }
                                else
                                {
                                    campaignBidConfigDto = new CampaignBidConfigDto()
                                    {
                                        Appsite = MapperHelper.Map<AppSiteBasicDto>(appsite),
                                        AccountId = appsite.Account.ID,
                                        AccountName = appsite.Account.GetName(),
                                        AdGroupId = group.ID,
                                        AdGrouptName = group.Name,
                                        AppsiteName = appsite.Name,
                                        MinBid = group.GetReadableBid(),
                                        CampaingName = item.Name,
                                        AdGroupPricingModel = group.CostModelWrapper != null ? group.CostModelWrapper.CostModel.GetDescription() : string.Empty,
                                        AppsitePricingModel = (appsite.AppSiteServerSetting.GetPricingModel() == null ? "" : appsite.AppSiteServerSetting.GetPricingModel().GetDescription())
                                    };


                                    response.NotCompatableCampaigns.Add(campaignBidConfigDto);
                                }
                            }
                        }
                    }
                    request.GroupCostModelWrapperID = null;
                }
            }

            if (response.NotCompatableCampaigns.Count == 0)
                response.Success = true;

            return response;
        }
        public bool CheckAppsitesCompatibleWithSSPPartnerRTBSetings(int campaignId, List<int> appsites = null, int? goupId = null, bool checkExisting = false)
        {
            var item = CampaignRepository.Get(campaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            List<int> adGroupIds = new List<int>();
            if (appsites == null)
            { appsites = new List<int>(); }

            if (item.IsValid)
            {
                if (goupId.HasValue)
                {
                    adGroupIds.Add(goupId.Value);
                }
                else
                {
                    //adGroupIds = item.AdGroups.Where().Select(x => x.ID).ToList();
                    foreach (var adGroupObj in item.AdGroups)
                    {

                        if (adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().FirstOrDefault() != null)

                        {
                            var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > 0).ToList();
                            if (results != null && results.Count > 0)
                                adGroupIds.Add(adGroupObj.ID);
                        }

                    }
                }
                List<int> allAppsites = new List<int>();
                allAppsites.AddRange(appsites);
                if (adGroupIds != null && adGroupIds.Count > 0)
                {
                    if (checkExisting)
                    {
                        foreach (int adGoupId in adGroupIds)
                        {
                            var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGoupId);
                            if (group != null)
                            {




                                allAppsites.AddRange(GetCampaignAllAssignedAppsites(campaignId, adGoupId));
                                //  IEnumerable<int>[] existingAppsites = group.Ads.Select(x => x.AppSiteAdQueues.Select(q => q.AppSite.ID)).ToArray();

                                ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite appsite = null;


                                //foreach (int appSiteId in allAppsites.Distinct())
                                //{
                                //    appsite = appSiteRepository.Get(appSiteId);
                                //    if (appsite.IsDeleted)
                                //    {
                                //        continue;
                                //    }

                                //}
                            }

                        }
                    }
                    allAppsites = allAppsites.Distinct().ToList();


                    var result = _SSPPartnerRepository.CheckWeatherMeetGefoenceResricions(allAppsites);
                    if (result > 0)
                    {
                        foreach (var adGroupObj in item.AdGroups)
                        {
                            if (adGroupIds.Contains(adGroupObj.ID))
                            {
                                var results = adGroupObj.Targetings.ToList().OfType<GeoFencingTargeting>().Where(M => M.Radius > result).ToList();
                                if (results != null && results.Count > 0)
                                {
                                    return false;
                                }
                            }
                        }

                    }
                }
            }



            return true;
        }

        public List<int> GetCampaignAllAssignedAppsites(int campaignId, int adGoupId)
        {
            var item = CampaignRepository.Get(campaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            if (item.IsValid)
            {
                List<int> allAssignedAppsites = new List<int>();

                var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == adGoupId);
                if (group != null)
                {
                    int[] assignedAppsites = item.GetCampaignAssignedAppsite().Select(x => x.AppSite.ID).ToArray();
                    allAssignedAppsites.AddRange(assignedAppsites);
                    int[] assignedBidConfigs = group.GetCampaignBidConfigs().Select(x => x.AppSite.ID).ToArray();
                    allAssignedAppsites.AddRange(assignedBidConfigs);
                    var groupAds = group.Ads.Select(x => x.AppSiteAdQueues.Select(q => q.AppSite.ID));
                    foreach (var ad in groupAds)
                    {
                        // assignedAppsites.ins
                        allAssignedAppsites.AddRange(ad.ToArray());
                    }
                }
                return allAssignedAppsites;
            }
            return null;

        }

        public List<int> GetAppSiteAdQueues(CampaignIdAdgroupIdAdIdMessage request)
        {
            var item = CampaignRepository.Get(request.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);
            var group = item.GetGroups().FirstOrDefault(adGroup => adGroup.ID == request.AdgroupId);
            if (group != null)
            {
                var groupAds = item.GetGroupAds(group).OrderByDescending(x => x.ID);
                if (groupAds != null)
                {
                    var adObj = groupAds.FirstOrDefault(ad => ad.ID == request.AdId);
                    if (adObj != null)
                    {
                        return adObj.AppSiteAdQueues.Select(x => x.AppSite.ID).ToList();
                        //return	 Mapper.CreateMap<AppSiteAdQueue, AppSiteAdQueueDto>()

                    }
                }
            }


            return new List<int>();
        }

        public ValueMessageWrapper<AdTypeIds> GetAddTypesByAddGroupAction(ValueMessageWrapper<int> adGroupId)
        {

            var Objective = this.adGroupRepository.Query(item => item.ID == adGroupId.Value).Select(M => M.Objective).SingleOrDefault();

            if (Objective.AdAction.ID >= 2 && Objective.AdAction.ID <= 5)
            {
                return ValueMessageWrapper.Create(AdTypeIds.TrackingAd);

            }
            return ValueMessageWrapper.Create(AdTypeIds.Undefined);
        }
        public ValueMessageWrapper<int> GetActionTypeByadGroup(ValueMessageWrapper<int> adGroupId)
        {

            var Objective = this.adGroupRepository.Query(item => item.ID == adGroupId.Value).Select(M => M.Objective).SingleOrDefault();

            return ValueMessageWrapper.Create(Objective.AdAction.ID);
        }
        public ValueMessageWrapper<int> GetCreativeVendorForAdCreativeUnit(ValueMessageWrapper<int> AdCreativeUnitId)
        {

            var adCreativeUnitVendor = _AdCreativeUnitVendorRepository.Query(M => M.Unit.ID == AdCreativeUnitId.Value).FirstOrDefault();

            if (adCreativeUnitVendor != null)
                return ValueMessageWrapper.Create(adCreativeUnitVendor.Vendor.ID);


            return ValueMessageWrapper.Create(0);
        }

        public IList<AdCreativeDto> GetDraftVideoAd()
        {
            var creatives = this._InStreamVideoCreativeRepository.Query(M => M.IsDeleted == false && M.IsDraft == true && M.CreateOption == CreateOption.Upload).ToList();
            IList<AdCreativeDto> results = new List<AdCreativeDto>();


            AdCreativeDto dto = new AdCreativeDto();
            foreach (var adCreative in creatives)
            {
                var inStreamVideoCreative = (InStreamVideoCreative)adCreative;
                var firstadCreativeUnit = adCreative.AdCreativeUnits.FirstOrDefault();
                dto = new AdCreativeDto();
                dto.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                dto.ID = adCreative.ID;
                if (inStreamVideoCreative != null)
                {
                    AdCreativeUnitDto adCreativeUnitDto = null;

                    foreach (AdCreativeUnit adCreativeUnit in adCreative.AdCreativeUnits)
                    {
                        adCreativeUnitDto = MapperHelper.Map<AdCreativeUnitDto>(adCreativeUnit);
                       // adCreativeUnitDto.CreativeUnit = MapperHelper.Map<CreativeUnitDto>(adCreativeUnit);
                       // adCreativeUnitDto.InStreamVideoCreativeUnit = MapperHelper.Map<InStreamVideoCreativeUnitDto>(adCreativeUnit.InStreamVideoCreativeUnit);
                        adCreativeUnitDto.InStreamVideoCreativeUnit.VideoDuration = inStreamVideoCreative.DurationInSeconds;
                        var creativeUnitDto = _CreativeUnitRepository.Get(adCreativeUnitDto.InStreamVideoCreativeUnit.OriginalCreativeUnitID);
                        adCreativeUnitDto.InStreamVideoCreativeUnit.Code = creativeUnitDto.Code;


                        dto.CreativeUnitsContent.Add(adCreativeUnitDto);
                    }
                    results.Add(dto);
                }


            }

            return results;
        }

        public void PublishVideoAd(ValueMessageWrapper<int> videoAdint)
        {
        

            var videoAd = _InStreamVideoCreativeRepository.Get(videoAdint.Value);
            videoAd.Group.Campaign.Account.SetTenant();
            videoAd.IsDraft = false;
            videoAd.PublishVideoFiles();
            _InStreamVideoCreativeRepository.Save(videoAd);

        }
        public IList<VideoConversionCreativeUnitDto> GetVideoConversionCreativeUnits(string code)
        {

            var videoCrvAds = _VideoConversionCreativeUnitRepository.Query(M => M.Code == code).ToList();
            IList<VideoConversionCreativeUnitDto> creativeUnits = new List<VideoConversionCreativeUnitDto>();

            if (videoCrvAds != null)
            {
                foreach (var videoCrvAd in videoCrvAds)
                {
                    creativeUnits.Add(new VideoConversionCreativeUnitDto { ID = videoCrvAd.ID, BitRate = videoCrvAd.BitRate, Code = videoCrvAd.Code, CreativeUnitId = videoCrvAd.CreativeUnit.ID, AudioBitRate = videoCrvAd.AudioBitRate, VideoFrameRate = videoCrvAd.VideoFrameRate });

                }
            }



            return creativeUnits;

        }

        public void UploadTest(UploadTestRequest request)
        {
            ArabyAds.Framework.Utilities.CDNHelper.Upload(request.FullDirectory, request.Content);

        }

        public string GetDirectory(ValueMessageWrapper<int> index)
        {
            try
            {
                string baseDirectory = "media/http/i/";

                string temp = string.Empty;
                var subFolder = string.Empty;

                //we need to create folder for it
                subFolder = ArabyAds.Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                ArabyAds.Framework.Utilities.CDNHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = (index.Value * RandomNumber(1, 100000)).ToString();
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!ArabyAds.Framework.Utilities.CDNHelper.DirectoryExists(temp))
                    {
                        ArabyAds.Framework.Utilities.CDNHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;
                    }
                }


                // create folder for current ad if not found
                subFolder = string.Format("{0}/{1}", subFolder, (index.Value * RandomNumber(1, 100000)).ToString());
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);

                ArabyAds.Framework.Utilities.CDNHelper.CreateDirectory(temp);

                subFolder = string.Format("{0}/Snapshots", subFolder);
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                ArabyAds.Framework.Utilities.CDNHelper.CreateDirectory(temp);
                return temp;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        private int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        #region advertiser
        public ValueMessageWrapper<int> GetAdvertiserIdByCampaignId(ValueMessageWrapper<int> id)
        {
            var camp = CampaignRepository.Get(id.Value);
            return ValueMessageWrapper.Create(camp.Advertiser != null ? camp.Advertiser.ID : 0);

        }
        public ValueMessageWrapper<int> GetAdvertiserAccountIdByCampaignId(ValueMessageWrapper<int> id)
        {
            var camp = CampaignRepository.Get(id.Value);
            return ValueMessageWrapper.Create(camp.AdvertiserAccount != null ? camp.AdvertiserAccount.ID : 0);

        }
        #endregion


        public bool IsSubUserHasWriteMode(int advertiseraccId)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return true;
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {
                return false;

            }
            var obj = _advertiserAccountUserRepository.Query(x => x.Link.ID == advertiseraccId && x.IsDeleted == false && x.User.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().UserId.Value).FirstOrDefault();
            if (obj != null)
                return obj.Write;




            return false;
        }

        public bool IsllowedAdvertiserForCamp(ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign camp)
        {
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
                return true;
            var advLink = camp.AdvertiserAccount;
            if (advLink==null)
            {
                return false;
            }

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().IsReadOnly)
            {
                return false;

            }
            if (!advLink.IsRestricted)
            {
                return true;
            }
            var obj = _advertiserAccountUserRepository.Query(x => x.Link.ID == advLink.ID && x.IsDeleted == false && x.User.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().UserId.Value).FirstOrDefault();
            if (obj != null)
                return obj.Write;




            return false;
        }

        public ValueMessageWrapper<int> GetPublisherCounterCurrentWeek()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateSQLQuery("SELECT LastClosedDate FROM adfalcon_hdp.tasks_closed_dates where Code='Analytics-Publishers-Counters' ");

            var WeekObj = query.UniqueResult();
            var DateTimeObj = Convert.ToDateTime(WeekObj);

            return ValueMessageWrapper.Create(Convert.ToInt32(DateTimeObj.ToString("yyyyMMdd")));
        }

        public ValueMessageWrapper<int> GetAudienceListCounter(string dpName)
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();



            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", dpName);
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return  ValueMessageWrapper.Create(Convert.ToInt32(count));
        }


        public int CalculateBinIndex(int Id)
        {
            //100 should be configured
            var result = (Id % 100);
            var count = 1;
         var audienceSegmentOcuupobj=    _AudienceSegmentOccupationRepository.Get(result+1);
            var bindIndex = result  ;
            //10000 should be configured

            while (count <= 100)
            {
                if (bindIndex + 1 == 101)
                    bindIndex = 1;
                else
                    bindIndex = bindIndex + 1;
                audienceSegmentOcuupobj = _AudienceSegmentOccupationRepository.Get(bindIndex);

                if (!((audienceSegmentOcuupobj.NumberOfSegments + 1) > 10000))
                {
                    audienceSegmentOcuupobj.NumberOfSegments = audienceSegmentOcuupobj.NumberOfSegments + 1;
                    _AudienceSegmentOccupationRepository.Save(audienceSegmentOcuupobj);
                    return bindIndex;
                }
                count = count + 1;
            }

            return -1;
            //if ((audienceSegmentOcuupobj.NumberOfSegments + 1) > 10000)
            //{


            //}
            //else
            //{
            //    audienceSegmentOcuupobj.NumberOfSegments = audienceSegmentOcuupobj.NumberOfSegments + 1;
            //    _AudienceSegmentOccupationRepository.Save(audienceSegmentOcuupobj);
            //    return bindIndex;
            //}
        }

        public ValueMessageWrapper<bool> IsFormatedAdCreativUnit(string content)
        {
            var adCreativeUnit = new AdCreativeUnit() { Content = content };
            var adcreativeFormatter = AdCreativeContentFormatterBase.GetAdCreativeContentFormatter(adCreativeUnit);
            return ValueMessageWrapper.Create(adcreativeFormatter.IsFormatted());


        }
    }



}
