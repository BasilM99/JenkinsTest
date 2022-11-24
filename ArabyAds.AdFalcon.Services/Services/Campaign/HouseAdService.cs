using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
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
using System.Net;
using ArabyAds.Framework.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Domain.Repositories.Account.PMP;
using ArabyAds.AdFalcon.Domain.Repositories.Core.Video;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Persistence.ReportsGP.Repositories;

namespace ArabyAds.AdFalcon.Services.Services.Campaign
{
    public class HouseAdService : IHouseAdService
    {
        private ISummaryRepository summaryRepository;
        private IHouseAdRepository houseAdRepository;
        private ICampaignService campaignService;
        private ICampaignRepository campaignRepository;
        private IAppSiteRepository appSiteRepository;
        private IAdGroupRepository adGroupRepository;
        public HouseAdService(ISummaryRepository summaryRepository,
                                IHouseAdRepository houseAdRepository,
                                ICampaignRepository campaignRepository,
            IAppSiteRepository appSiteRepository
            , IAdGroupRepository AdGroupRepository

            )
        {
            this.summaryRepository = summaryRepository;
            this.houseAdRepository = houseAdRepository;
            this.campaignRepository = campaignRepository;
            this.appSiteRepository = appSiteRepository;
            this.adGroupRepository = AdGroupRepository;
            //TODO:Use IOC To Load this Object
            this.campaignService = new CampaignService(
                    IoC.Instance.Resolve<IAdGroupBidModifierRepository>(),
                 IoC.Instance.Resolve<ICampaignBidModifierRepository>(),

                      IoC.Instance.Resolve<IAdGroupEventRepository>(),
                 IoC.Instance.Resolve<ICampaignFrequencyCappingRepository>(),
                IoC.Instance.Resolve<ICampaignRepository>(),
                                IoC.Instance.Resolve<IAccountPortalPermissionsRepository>(),

                IoC.Instance.Resolve<IPlatformRepository>(),
                                IoC.Instance.Resolve<IAdTypeRepository>(),

                IoC.Instance.Resolve<IOperatorRepository>(),

                IoC.Instance.Resolve<ITargetingTypeRepository>(),
                IoC.Instance.Resolve<ILocationRepository>(),
                IoC.Instance.Resolve<IManufacturerRepository>(),
                IoC.Instance.Resolve<IKeyWordRepository>(),
                IoC.Instance.Resolve<IAdActionTypeRepository>(),
                IoC.Instance.Resolve<IAdGroupObjectiveTypeRepository>(),
                IoC.Instance.Resolve<IBidManager>(),
                IoC.Instance.Resolve<IDeviceRepository>(),
                IoC.Instance.Resolve<IAgeGroupRepository>(),
                IoC.Instance.Resolve<IGenderRepository>(),
                IoC.Instance.Resolve<ICreativeUnitRepository>(),
                IoC.Instance.Resolve<IDocumentRepository>(),
                IoC.Instance.Resolve<ITileImageRepository>(),
                IoC.Instance.Resolve<ITileImageSizeRepository>(),
                IoC.Instance.Resolve<ISummaryRepository>(),
                IoC.Instance.Resolve<IAccountRepository>(),
                IoC.Instance.Resolve<IDeviceTargetingTypeRepository>(),
                IoC.Instance.Resolve<IAdRepository>(),
                IoC.Instance.Resolve<IConfigurationManager>(),
                IoC.Instance.Resolve<IAppSiteRepository>(),
                IoC.Instance.Resolve<IDeviceCapabilityRepository>(),
                IoC.Instance.Resolve<ICampaignPerformanceRepository>(),
                IoC.Instance.Resolve<IPartyRepository>(),
                IoC.Instance.Resolve<ICostElementRepository>(),
                IoC.Instance.Resolve<IRichMediaRequiredProtocolRepository>(),
                IoC.Instance.Resolve<IAdSupportedCreativeUnitRepository>(),
                IoC.Instance.Resolve<IDeviceTypeRepository>(),
                IoC.Instance.Resolve<IAdTypeRepository>(),
                IoC.Instance.Resolve<ICostModelWrapperRepository>(),
                IoC.Instance.Resolve<ITrackingEventRepository>(),
                IoC.Instance.Resolve<IAdGroupTrackingEventRepository>(),
                     IoC.Instance.Resolve<IAdGroupConversionEventRepository>(),
                IoC.Instance.Resolve<IMIMETypeRepository>(),
                IoC.Instance.Resolve<IAdActionTypeTrackingEventRepository>(),
                  IoC.Instance.Resolve<IInStreamVideoCreativeUnitRepository>(),
                   IoC.Instance.Resolve<IAccountTrackingEventsRepository>(),
                   IoC.Instance.Resolve<IVideoDeliveryMethodRepository>(),
                    IoC.Instance.Resolve<IVideoTypeRepository>(), IoC.Instance.Resolve<IAdRequestTypePlatformVersionRepository>(),
                     IoC.Instance.Resolve<IAdGroupRepository>(), IoC.Instance.Resolve<IAppMarketingPartnerRepository>(), IoC.Instance.Resolve<IAdCreativeRepository>(), IoC.Instance.Resolve<IAdRequestTargetingRepository>(), IoC.Instance.Resolve<IReportSchedulerRepository>(), IoC.Instance.Resolve<IAdvertiserRepository>(), IoC.Instance.Resolve<IAppSiteAdQueueRepository>(), IoC.Instance.Resolve<IPMPDealRepository>(), IoC.Instance.Resolve<ISSPPartnerRepository>(), IoC.Instance.Resolve<ISubAppsiteRepository>(), IoC.Instance.Resolve<IBusinessPartnerRepository>(), IoC.Instance.Resolve<IImpressionMetricTargetingRepository>(), IoC.Instance.Resolve<IImpressionMetricRepository>()
, IoC.Instance.Resolve<IAdCreativeUnitVendorRepository>(), IoC.Instance.Resolve<IAdCreativeUnitVendorRepository>(), IoC.Instance.Resolve<ICreativeVendorKeywordRepository>()
, IoC.Instance.Resolve<ICreativeVendorRepository>(), IoC.Instance.Resolve<IDocumentTypeRepository>()

, IoC.Instance.Resolve<IPlacementTypeRepository>()
, IoC.Instance.Resolve<IPlaybackMethodsRepository>()
, IoC.Instance.Resolve<IInStreamPositionRepository>()
, IoC.Instance.Resolve<ISkippableAdsRepository>()
, IoC.Instance.Resolve<IVideoMediaFileRepository>()
, IoC.Instance.Resolve<IInStreamVideoCreativeRepository>()
, IoC.Instance.Resolve<ICreativeUnitRepository>()
, IoC.Instance.Resolve<IVideoConversionCreativeUnitRepository>()

, IoC.Instance.Resolve<IProtocolRepository>(),
IoC.Instance.Resolve<IMetricVendorRepository>(),
IoC.Instance.Resolve<IAdvertiserAccountRepository>(),

IoC.Instance.Resolve<IAdvertiserAccountUserRepository>(),
IoC.Instance.Resolve<IAudienceSegmentRepository>(),
IoC.Instance.Resolve<IDPPartnerRepository>(),
IoC.Instance.Resolve<IAdvertiserAccountMasterAppSiteRepository>()
, IoC.Instance.Resolve<IFeeRepository>()

, IoC.Instance.Resolve<IAdGroupDynamicBiddingConfigRepository>()
, IoC.Instance.Resolve<IAudienceSegmentOccupationRepository>()

, IoC.Instance.Resolve<IAdvertiserAccountUserRepository>(), houseAdRepository
, IoC.Instance.Resolve<ICampaignTroubleshootingRepository>()
, IoC.Instance.Resolve<IContextualSegmentRepository>()
);

        }
        private void CheckHouseAd(HouseAd item)
        {
            if (item == null)
            {
                throw new DataNotFoundException();
            }
        }
        private void CheckCampaign(Domain.Model.Campaign.Campaign item)
        {
            if (item == null)
            {
                throw new DataNotFoundException();
            }
        }
        public CampaignListResultDto QueryHouseAdsCampaignsCratiria(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CampaignCriteria criteria)
        {

           


            criteria.CampaignType = CampaignType.AdHouse;
            criteria.OtherCampaignType = CampaignType.Undefined;
            return campaignService.QueryByCratiria(criteria);
        }

        public CampaignDto GetHouseAdCampaign(ValueMessageWrapper<int> id)
        {
            return campaignService.Get(new GetCampaignRequest { CampaignId = id.Value, Type = CampaignType.AdHouse, Othertype = CampaignType.Undefined });
        }

        public HouseAdDto Get(ValueMessageWrapper<int> id)
        {
            var item = houseAdRepository.Get(id.Value);
            CheckHouseAd(item);
            ValidateCampaign(item.AdGroup.Campaign);

            if (item.AdGroup.Campaign.IsValid)
            {
                return MapperHelper.Map<HouseAdDto>(item);
            }
            return null;
        }
        public HouseAdDto GetByAdGroup(ValueMessageWrapper<int> id)
        {

            var item = this.adGroupRepository.Get(id.Value);
                if(item.HouseAd!=null)
                return MapperHelper.Map<HouseAdDto>(item.HouseAd);
          
            return null;
        }
        public ValueMessageWrapper<int> SaveAdGroup(HouseAdGroupDto houseAdGroup)
        {
            var item = campaignRepository.Get(houseAdGroup.CampaignId);
            CheckCampaign(item);
            ValidateCampaign(item);

            if (item.IsValid)
            {
                AdGroup adGroup;
                var ForAppSite = appSiteRepository.Get(houseAdGroup.ForAppSite);
                if (houseAdGroup.ID <= 0)
                {
                    //new
                    var agGroupDto = new AdGroupDto()
                    {
                        CampaignId = houseAdGroup.CampaignId,
                        Name = houseAdGroup.Name,

                    };

                    switch (ForAppSite.Type.ID)
                    {
                        case (int)AppSiteTypeEnum.Android:
                            {
                                agGroupDto.ActionTypeId = AdActionTypeIds.DownloadAndroidApplication;
                                agGroupDto.ObjectiveTypeId = AdGroupObjectiveTypeIds.PromoteAppSite;
                                break;
                            }
                        case (int)AppSiteTypeEnum.iOS:
                            {
                                agGroupDto.ActionTypeId = AdActionTypeIds.DownloadiOSUniversalApplication;
                                agGroupDto.ObjectiveTypeId = AdGroupObjectiveTypeIds.PromoteAppSite;
                                break;
                            }
                        case (int)AppSiteTypeEnum.MobileWeb:
                            {
                                agGroupDto.ActionTypeId = AdActionTypeIds.ClickToMobileWeb;
                                agGroupDto.ObjectiveTypeId = AdGroupObjectiveTypeIds.PromoteContent;
                                break;
                            }
                    }

                    var adgroupId = campaignService.SaveAdGroup(new SaveAdGroupRequest { AdGroup = agGroupDto, ReturnId = false }).Value;
                    adGroup = item.GetGroups().First(x => x.ID == adgroupId);

                    adGroup.HouseAd = new HouseAd(adGroup)
                    {
                        Account = item.Account,
                        User = item.User,
                        DeliveryMode = houseAdGroup.DeliveryMode,
                        ForAppSite = ForAppSite
                    };

                }
                else
                {
                    //update
                    adGroup = item.GetGroups().First(x => x.ID == houseAdGroup.ID);
                    adGroup.HouseAd.ClearDestinationAppSite();

                    adGroup.HouseAd.DeliveryMode = houseAdGroup.DeliveryMode;
                    adGroup.HouseAd.ForAppSite = ForAppSite;
                    adGroup.Name = houseAdGroup.Name;
                }



                foreach (var destinationAppSite in houseAdGroup.DestinationAppSites)
                {
                    adGroup.HouseAd.AddDestinationAppSite(appSiteRepository.Get(destinationAppSite));
                }
                switch (adGroup.HouseAd.DeliveryMode)
                {
                    case HouseAdDeliveryMode.FullyAllocate:
                        {
                            adGroup.CPMValue = Int16.MaxValue;
                            adGroup.Bid = 0;
                            adGroup.MinimumUnitPrice = 0;
                            break;
                        }
                    case HouseAdDeliveryMode.WhenNoAds:
                        {
                            adGroup.CPMValue = null;
                            adGroup.Bid = 0;
                            adGroup.MinimumUnitPrice = 0;
                            break;
                        }
                }
                houseAdRepository.Save(adGroup.HouseAd);
                return ValueMessageWrapper.Create(adGroup.ID);
            }
            return ValueMessageWrapper.Create(0);
        }

        #region Private Members

        private void ValidateCampaign(Domain.Model.Campaign.Campaign campaign)
        {
            bool isManager = IsCampaignManager();

            campaign.Validate(!isManager);
        }

        private bool IsCampaignManager()
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager");
        }


        #endregion
    }
}
