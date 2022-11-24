using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AutoMapper;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.AppSite.Filtering;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.DomainServices.Localization;
using ArabyAds.Framework.Resources;
using ArabyAds.AdFalcon.Domain.Model.Account.Fund;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.Framework.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using AutoMapper.Configuration;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Mapping
{
    public static class MappingRegister
    {
        static IDPPartnerRepository DPPartnerRepository;
        static IAudienceSegmentRepository audianSegRep;
        static ICompanyTypeRepository companyTypeRepository;
        static ICountryRepository countryRepository;
        static ILanguageRepository languageRepository;
        static ITrackingEventRepository trackingEventRepository;
        static IAccountRepository accountRepository;
        static IDocumentTypeRepository _documentTypeRepository = null;

        static IAccountTrackingEventsRepository AccountTrackingEventRepository;
        static IAdRequestTypePlatformVersionRepository AdRequestTypePlatformVersionRepository;
        private const string IMPRESSIONEVENT = "000imp";
        private const string CLICKEVENT = "000clk";
        private static MapperConfiguration _mapperConfiguration;
        public static void RegisterMapping()
        {
            ArabyAds.Framework.IoC.Instance.GetType();
            countryRepository = Framework.IoC.Instance.Resolve<ICountryRepository>();
            companyTypeRepository = Framework.IoC.Instance.Resolve<ICompanyTypeRepository>();
            audianSegRep = Framework.IoC.Instance.Resolve<IAudienceSegmentRepository>();
            languageRepository = Framework.IoC.Instance.Resolve<ILanguageRepository>();
            trackingEventRepository = Framework.IoC.Instance.Resolve<ITrackingEventRepository>();
            AccountTrackingEventRepository = Framework.IoC.Instance.Resolve<IAccountTrackingEventsRepository>();
            AdRequestTypePlatformVersionRepository = Framework.IoC.Instance.Resolve<IAdRequestTypePlatformVersionRepository>();
            accountRepository = Framework.IoC.Instance.Resolve<IAccountRepository>();
            DPPartnerRepository = Framework.IoC.Instance.Resolve<IDPPartnerRepository>();
            _documentTypeRepository = Framework.IoC.Instance.Resolve<IDocumentTypeRepository>();
            var cfg = new MapperConfigurationExpression();
            cfg.AllowNullCollections = true;

            RegisterCoreMapping(cfg);
            RegisteLocalizedStringMapping(cfg);
            RegisterAppSiteMapping(cfg);
            RegisterUserMapping(cfg);
            RegisterAppSiteSettingsMapping(cfg);
            RegisterTextFilterToTextFilterDtoMapping(cfg);
            RegisterTextFilterDtoToTextFilterMapping(cfg);
            RegisterlanguageFilterDtoToLanguageFilterMapping(cfg);
            RegisterOperatorMapping(cfg);
            RegisterDeviceMapping(cfg);
            RegisterTargetingMapping(cfg);
            RegisterLocationMapping(cfg);
            RegisterAdActionTypeMapping(cfg);
            RegisterAdActionTypeConstraintMapping(cfg);
            RegisterUrlFilterToUrlFilterDtoMapping(cfg);
            RegisterUrlFilterDtoToUrlFilterMapping(cfg);
            RegisterlanguageFilterToLanguageFilterDtoMapping(cfg);
            RegisterBankAccountDtoToBankAccount(cfg);
            RegisterCampaignDtoMapping(cfg);
            RegisterFundtoFundDtoMapping(cfg);
            RegisterAdCreativeSummaryDtoMapping(cfg);
            RegisterAdGroupSummaryDtoMapping(cfg);
            RegisterCampaignSummaryDtoMapping(cfg);
            RegisterPerformanceDtoMapping(cfg);
            RegisterFundTransDtoMapping(cfg);
            RegisterPaymentDtoMapping(cfg);
            RegisterReportDtoDtoMapping(cfg);
            RegisterCampaignDtoToCampaignMapping(cfg);
            RegisterAccountToAccountAPIAccessDto(cfg);
            RegisterReturnBidToReturnBidDto(cfg);
            RegisterAdActionTypeTrackingEventToAdGroupTrackingEventDto(cfg);
            RegisterAdActionTypeTrackingEventToAdGroupTrackingEvent(cfg);
            RegisterAdGroupTrackingEventToAdGroupTrackingEventDto(cfg);
            RegisterAdGroupTrackingEventSaveDtoToAdGroupTrackingEvent(cfg);
            RegisterAdActionValueTrackerToAdActionValueTrackerDto(cfg);
            RegisterTrackingEventToTrackingEventDto(cfg);
            RegisterCampaignFrequencyCappingSaveDtoToCampaignFrequencyCapping(cfg);
            RegisterCampaignBidConfigDtoCampaignBidConfigMapping(cfg);
            RegisterReportSchedulerDtoDtoMapping(cfg);
            RegisterAdRequestTargetingDtoMapping(cfg);
            RegisterSSPDtoDtoMapping(cfg);
            RegisterPMPDealDtoDtoMapping(cfg);
            RegisterAudienceSegmentMapping(cfg);
            RegisterVideoCardDto(cfg);
            RegisteImpressionLogDtoMapping(cfg);
            RegistermetriceGroupColumnDtoMapping(cfg);
            RegisterAdvertiserAccountDtoMapping(cfg);
            RegisterCampaignAssignedAppsiteToCampaignAssignedAppsiteDto(cfg);
            RegisterAccountDSPSettingsDtoDtoMapping(cfg);
            MapQBProfile(cfg);

            _mapperConfiguration = new MapperConfiguration(cfg);
            Mapper = _mapperConfiguration.CreateMapper();


        }

        public static IMapper Mapper { get; private set; }

        private static void MapQBProfile(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ColumnQB, ColumnQBDto>();
            cfg.CreateMap<ColumnQBDto, ColumnQB>();
            cfg.CreateMap<Dimension, DimensionDto>();
            cfg.CreateMap<DimensionDto, Dimension>();
            cfg.CreateMap<Fact, FactDto>()
               .ForMember(dest => dest.Dimensions,
                 z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.Dimensions != null)
                     {
                         return context.Mapper.Map<IEnumerable<Dimension>, ICollection<DimensionDto>>(src.Dimensions.Where(M => !M.IsDeleted));
                     }
                     return null;
                 }
                ))

                     .ForMember(dest => dest.Measures,

                 z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.Measures != null)
                     {
                         return context.Mapper.Map<IEnumerable<Measure>, ICollection<MeasureDto>>(src.Measures.Where(M => !M.IsDeleted));
                     }
                     return null;
                 }


                ))

                ;
            cfg.CreateMap<FactDto, Fact>();

            cfg.CreateMap<Measure, MeasureDto>();
            cfg.CreateMap<MeasureDto, Measure>();

            cfg.CreateMap<EntityQB, EntityQBDto>();
            cfg.CreateMap<EntityQBDto, EntityQB>();
        }

        private static void RegisterAdvertiserAccountDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdGroupBidModifierDto, CampaignBidModifier>()
              .ForMember(p => p.Campaign, x => x.MapFrom((src, dest) =>
              {
                  if (src.CampaignId > 0)
                  {

                      Campaign acc = new Campaign() { ID = src.CampaignId };
                      return acc;
                  }
                  return null;
              })).ForMember(dest => dest.DimentionType, opt => opt.MapFrom(item => item.DimensionType))

            ;

            cfg.CreateMap<CampaignBidModifier, AdGroupBidModifierDto>()
          .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(item => item.Campaign != null ? item.Campaign.ID : 0))
                  .ForMember(dest => dest.DimensionTypeId, opt => opt.MapFrom(item => (int)item.DimentionType))
            .ForMember(dest => dest.DimensionTypeStr, opt => opt.MapFrom(item => item.DimentionType.ToText()))
            ;


            cfg.CreateMap<AdGroupBidModifierDto, AdGroupBidModifier>()
                  .ForMember(p => p.Campaign, x => x.MapFrom((src, dest) =>
                  {
                      if (src.CampaignId > 0)
                      {

                          Campaign acc = new Campaign() { ID = src.CampaignId };
                          return acc;
                      }
                      return null;
                  })).ForMember(p => p.AdGroup, x => x.MapFrom((src, dest) =>
                  {
                      if (src.AdGroupId > 0)
                      {

                          AdGroup acc = new AdGroup() { ID = src.AdGroupId };
                          return acc;
                      }
                      return null;
                  })).ForMember(dest => dest.DimentionType, opt => opt.MapFrom(item => item.DimensionType))


                ;

            cfg.CreateMap<AdGroupBidModifier, AdGroupBidModifierDto>()
          .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(item => item.Campaign != null ? item.Campaign.ID : 0))
          .ForMember(dest => dest.AdGroupId, opt => opt.MapFrom(item => item.AdGroup != null ? item.AdGroup.ID : 0))
          .ForMember(dest => dest.DimensionTypeId, opt => opt.MapFrom(item => (int)item.DimentionType))
            .ForMember(dest => dest.DimensionTypeStr, opt => opt.MapFrom(item => item.DimentionType.ToText()))
            
            ;




            cfg.CreateMap<AdvertiserAccountDto, AdvertiserAccount>();

            cfg.CreateMap<AdvertiserAccount, AdvertiserAccountDto>()
          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0));

            cfg.CreateMap<AdvertiserAccountListDto, AdvertiserAccount>();

            cfg.CreateMap<AdvertiserAccount, AdvertiserAccountListDto>()
          .ForMember(dest => dest.AdvertiserId, opt => opt.MapFrom(item => item.Advertiser != null ? item.Advertiser.ID : 0))
          .ForMember(dest => dest.AdvertiserItem, opt => opt.MapFrom(src => src.Advertiser));

            cfg.CreateMap<AdvertiserAccountUserDto, AdvertiserAccountUser>();

            cfg.CreateMap<AdvertiserAccountUser, AdvertiserAccountUserDto>();


            cfg.CreateMap<AdvertiserAccountReadOnlyUserDto, AdvertiserAccountUser>();

            cfg.CreateMap<AdvertiserAccountUser, AdvertiserAccountReadOnlyUserDto>();


            cfg.CreateMap<AdvertiserAccountMasterAppSiteDto, AdvertiserAccountMasterAppSite>();

            cfg.CreateMap<AdvertiserAccountMasterAppSite, AdvertiserAccountMasterAppSiteDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))
                    ;



            cfg.CreateMap<PixelDto, Pixel>();

            cfg.CreateMap<Pixel, PixelDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))

                                .ForMember(p => p.SegmentsMapId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                   .ForMember(p => p.SegmentsId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.ID.ToString()).ToArray())) : string.Empty))
            .ForMember(p => p.SegmentString, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.Name.Value.ToString()).ToArray())) : string.Empty))

                    ;




            cfg.CreateMap<AdvertiserAccountMasterAppSiteItemDto, AdvertiserAccountMasterAppSiteItem>();

            cfg.CreateMap<AdvertiserAccountMasterAppSiteItem, AdvertiserAccountMasterAppSiteItemDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0));


        }

        private static void RegisterReportDtoDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CampaignCommonReportDto, AdGeoLocationDto>()
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(item => item.Name))
              .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(item => item.SubName));
        }

        private static void RegisterAdRequestTargetingDtoMapping(MapperConfigurationExpression cfg)
        {


            cfg.CreateMap<AdRequestTypeDto, AdRequestType>();
            cfg.CreateMap<AdRequestPlatformDto, AdRequestPlatform>();
            cfg.CreateMap<AdRequestTypePlatformVersion, AdRequestTypePlatformVersionDto>();

            cfg.CreateMap<AdRequestType, AdRequestTypeDto>();
            cfg.CreateMap<AdRequestPlatform, AdRequestPlatformDto>();
            cfg.CreateMap<AdRequestTypePlatformVersionDto, AdRequestTypePlatformVersion>();


            cfg.CreateMap<AdRequestTargeting, AdRequestTargetingDto>();


            cfg.CreateMap<AdRequestTargetingDto, AdRequestTargeting>();


            cfg.CreateMap<ImpressionMetricTargeting, ImpressionMetricTargetingDto>()

                .ForMember(dest => dest.MetricVendor, opt =>
                opt.MapFrom((src, dest, property, context) =>
                {
                    if (src.MetricVendor == null)
                    {
                        return new MetricVendorDto
                        {
                            ID = 0,
                            Code = "Any",
                            Description = "Any",
                            Name = LocalizedStringDto.ConvertToLocalizedStringDto(ResourceManager.Instance.GetResource("Any"))
                        };
                    }
                    else
                    {
                        return context.Mapper.Map<MetricVendorDto>(src.MetricVendor);
                    }
                }));


            cfg.CreateMap<ImpressionMetricTargetingDto, ImpressionMetricTargeting>();

        }
        private static void RegisterPaymentDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PaymentDto, Payment>();
            cfg.CreateMap<PaymentType, PaymentTypeDto>();
            cfg.CreateMap<Payment, PaymentDto>();
        }

        private static AccountFundTransStatus GetFundTransStatus(FundTransactionDto fundTransactionDto)
        {
            if (fundTransactionDto.FundTransStatus == null)
                return null;
            var accountFundTransStatusRepository = Framework.IoC.Instance.Resolve<IAccountFundTransStatusRepository>();
            return accountFundTransStatusRepository.Get(fundTransactionDto.FundTransStatus.ID);
        }
        private static AccountFundTransType GetFundTransType(FundTransactionDto fundTransactionDto)
        {
            if (fundTransactionDto.FundTransType == null)
                return null;
            var accountFundTransTypeRepository = Framework.IoC.Instance.Resolve<IAccountFundTransTypeRepository>();
            return accountFundTransTypeRepository.Get(fundTransactionDto.FundTransType.ID);
        }

        private static AccountFundTransStatus GetFundTransStatus(FundTransactionResponseDto fundTransactionDto)
        {
            if (fundTransactionDto.FundTransStatus == null)
                return null;
            var accountFundTransStatusRepository = Framework.IoC.Instance.Resolve<IAccountFundTransStatusRepository>();
            return accountFundTransStatusRepository.Get(fundTransactionDto.FundTransStatus.ID);
        }
        
        private static void RegisterFundTransDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountFundTransStatusDto, AccountFundTransStatus>();
            cfg.CreateMap<AccountFundTransTypeDto, AccountFundTransType>();
            cfg.CreateMap<AccountFundTypeDto, AccountFundType>();
            cfg.CreateMap<AccountFundTransStatus, AccountFundTransStatusDto>();
             //   .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));
            cfg.CreateMap<AccountFundTransType, AccountFundTransTypeDto>();
               //  .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));
            cfg.CreateMap<AccountFundType, AccountFundTypeDto>();
            // .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));

            cfg.CreateMap<AccountFundTransHistory, FundTransactionDto>();
            cfg.CreateMap<AccountFundTransHistoryPgw, PgwFundTransactionResponseDto>(); 
            cfg.CreateMap<PgwFundTransactionResponseDto, AccountFundTransHistoryPgw>()
            .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(src => GetFundTransStatus(src)));

            cfg.CreateMap<AccountFundTransHistoryPaypal, PayPalFundTransactionResponseDto>();
            cfg.CreateMap<PayPalFundTransactionResponseDto, AccountFundTransHistoryPaypal>()
            .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(src => GetFundTransStatus(src)));


            cfg.CreateMap<AccountFundTransHistoryPgw, FundTransactionDto>();
            cfg.CreateMap<AccountFundTransHistoryPaypal, FundTransactionDto>();

            cfg.CreateMap<FundTransactionDto, AccountFundTransHistory>()
            .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(src => GetFundTransStatus(src)))
            .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(src => GetFundTransType(src)));

            cfg.CreateMap<FundTransactionDto, AccountFundTransHistoryPgw>()
          .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(src => GetFundTransStatus(src)))
          .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(src => GetFundTransType(src)));


            cfg.CreateMap<FundTransactionDto, AccountFundTransHistoryPaypal>()
          .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(src => GetFundTransStatus(src)))
          .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(src => GetFundTransType(src)));



            cfg.CreateMap<AccountFundTransHistory, AccountFundTransHistoryPgw>();

            cfg.CreateMap<AccountFundTransHistoryPgw, AccountFundTransHistoryPaypal>();



        }

        private static void RegisterPerformanceDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CampaignPerformance, CampaignPerformanceDto>();
        }

        private static void RegisterCampaignSummaryDtoMapping(MapperConfigurationExpression cfg)
        {
            
                            cfg.CreateMap<CompanyType, CompanyTypeDto>();
            cfg.CreateMap<CompanyTypeDto , CompanyType>();
            cfg.CreateMap<Campaign, CampaignsSummaryDtoBase>()
                               //.ForMember(dest => dest.AccountName, opt => opt.MapFrom(item => item.Account == null ? string.Empty : item.Account.GetName()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetAdBaseStatus(src)))
                .ForMember(dest => dest.CampaignTypeEnum, opt => opt.MapFrom(item => (int)item.CampaignType))
                .ForMember(dest => dest.CampaignType, opt => opt.MapFrom(
                    (src, dest) => src.CampaignType == CampaignType.AdHouse ?
                    ResourceManager.Instance.GetResource("AdHouse", "Campaign") :
                    ResourceManager.Instance.GetResource("NormalAd", "Campaign")));
            cfg.CreateMap<Campaign, CampaignSummaryDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(item => item.Account == null ? string.Empty : item.Account.GetName()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetAdBaseStatus(src)))
                  .ForMember(dest => dest.CampaignTypeEnum, opt => opt.MapFrom(item => (int)item.CampaignType))
                .ForMember(dest => dest.CampaignType, opt => opt.MapFrom(
                    (src, dest) => src.CampaignType == CampaignType.AdHouse ?
                    ResourceManager.Instance.GetResource("AdHouse", "Campaign") :
                    ResourceManager.Instance.GetResource("NormalAd", "Campaign")));
        }

        private static void RegisterAdGroupSummaryDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdGroup, AdGroupSummaryDtoBase>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetAdBaseStatus(src)))
                .ForMember(dest => dest.Objective, opt => opt.MapFrom(item => item.Objective.Objective == null ? string.Empty : item.Objective.Objective.Name.ToString()))
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(item => item.Objective.AdAction == null ? string.Empty : item.Objective.AdAction.Name.ToString()));

            cfg.CreateMap<AdGroup, AdGroupSummaryDto>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetAdBaseStatus(src)))
                 .ForMember(dest => dest.Objective, opt => opt.MapFrom(item => item.Objective.Objective == null ? string.Empty : item.Objective.Objective.Name.ToString()))
                 .ForMember(dest => dest.ActionType, opt => opt.MapFrom(item => item.Objective.AdAction == null ? string.Empty : item.Objective.AdAction.Name.ToString()));
        }
        private static void RegisterAccountDSPSettingsDtoDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DSPAccountSettingContact, AccountDSPsettingContactDTO>()
          .ForMember(dest => dest.AccountSettingId,
            opt => opt.MapFrom(item => item != null ? item.DSPAccountSetting.ID : 0));




            cfg.CreateMap<DSPAccountSetting, AccountDSPsettingDTO>().
                ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.CountryId, opt => opt.MapFrom(z => z.Country == null ? 0 : z.Country.ID))
                 .ForMember(dst => dst.StateId, opt => opt.MapFrom(z => z.State == null ? 0 : z.State.ID))

                .ForMember(dest => dest.AllContacts,

                 z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.Contacts != null)
                     {
                         return context.Mapper.Map<IEnumerable<DSPAccountSettingContact>, IList<AccountDSPsettingContactDTO>>(src.Contacts.Where(M => M.IsDeleted == false));
                     }
                     return null;
                 }


                ))

            //.ForMember(dest => dest.Status, opt => opt.MapFrom(
            //    item => item.IsActive == true ?
            //    ResourceManager.Instance.GetResource("Active", "JobGrid") :
            //    ResourceManager.Instance.GetResource("NotActive", "JobGrid")));

            ;

            cfg.CreateMap<AccountDSPsettingContactDTO, DSPAccountSettingContact>()
         .ForMember(p => p.DSPAccountSetting, x => x.MapFrom((src, dest) =>
         {
             if (src.AccountSettingId > 0)
             {

                 DSPAccountSetting acc = new DSPAccountSetting() { ID = src.AccountSettingId };
                 return acc;
             }
             return null;
         }));

            cfg.CreateMap<AccountDSPsettingDTO, DSPAccountSetting>()
                   .ForMember(p => p.Account, x => x.MapFrom((src, dest) =>
                   {
                       if (src.AccountId > 0)
                       {

                           Account acc = new Account() { ID = src.AccountId };
                           return acc;
                       }
                       return null;
                   }))
                    .ForMember(p => p.Country, x => x.MapFrom((src, dest) =>
                    {
                        if (src.CountryId > 0)
                        {
                            Country doc = new Country() { ID = src.CountryId };
                            return doc;
                        }

                        return null;
                    }))
                     .ForMember(p => p.State, x => x.MapFrom((src, dest) =>
                     {
                         if (src.StateId > 0)
                         {
                             State doc = new State() { ID = src.StateId };
                             return doc;
                         }

                         return null;
                     }));


        }
        private static void RegisterReportSchedulerDtoDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ReportRecipient, ReportRecipientDTO>()
          .ForMember(dest => dest.ReportSchedulerID,
            opt => opt.MapFrom(item => item.ReportScheduler != null ? item.ReportScheduler.ID : 0));

            cfg.CreateMap<ReportCriteria, ReportCriteriaSchedulerDto>()
.ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0));

            cfg.CreateMap<ReportCriteriaSchedulerDto, ReportCriteria>()
.ForMember(dest => dest.User, opt => opt.MapFrom(item => item.UserId > 0 ? new User { ID = item.UserId.Value } : null))
            .ForMember(dest => dest.Account, opt => opt.MapFrom(item => item.AccountId > 0 ? new User { ID = item.AccountId.Value } : null));


            cfg.CreateMap<ReportRecipientDTO, ReportRecipient>()
       .ForMember(p => p.ReportScheduler, x => x.MapFrom((src, dest) =>
       {
           if (src.ReportSchedulerID > 0)
           {

               ReportScheduler acc = new ReportScheduler() { ID = src.ReportSchedulerID };
               return acc;
           }
           return null;
       }));
            cfg.CreateMap<ReportScheduler, ReportSchedulerDto>().
                ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.LastDocumnetGeneratedId, opt => opt.MapFrom(z => z.LastDocumnetGenerated == null ? 0 : z.LastDocumnetGenerated.ID))
                                .ForMember(dst => dst.CriteriaSchedulerId, opt => opt.MapFrom(z => z.ReportCriteria == null ? 0 : z.ReportCriteria.ID))
                //  .ForMember(dest => dest.CriteriaScheduler, x => x.MapFrom(z => MapperHelper.Map<ReportCriteriaSchedulerDto>(z.ReportCriteria)))

                .ForMember(dest => dest.AllReportRecipient,

                 z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.AllRecipient != null)
                     {
                         return context.Mapper.Map<IEnumerable<ReportRecipient>, List<ReportRecipientDTO>>(src.AllRecipient.Where(M => M.IsDeleted == false));
                     }
                     return null;
                 }


                )).ForMember(dest => dest.Status, opt => opt.MapFrom(
                    (src, dest) => src.IsActive == true ?
                    ResourceManager.Instance.GetResource("Active", "JobGrid") :
                    ResourceManager.Instance.GetResource("NotActive", "JobGrid"))); ;

          

            cfg.CreateMap<ReportSchedulerDto, ReportScheduler>()
                   .ForMember(p => p.Account, x => x.MapFrom((src, dest) =>
                   {
                       if (src.AccountId > 0)
                       {

                           Account acc = new Account() { ID = src.AccountId };
                           return acc;
                       }
                       return null;
                   })).ForMember(dest => dest.AllRecipient,

                 z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.AllReportRecipient != null)
                     {
                         return context.Mapper.Map<IEnumerable<ReportRecipientDTO>, List<ReportRecipient>>(src.AllReportRecipient.Where(M => M.IsDeleted == false));
                     }
                     return null;
                 }


                ))
                    .ForMember(p => p.LastDocumnetGenerated, x => x.MapFrom((src, dest) =>
                    {
                        if (src.LastDocumnetGeneratedId > 0)
                        {
                            Document doc = new Document() { ID = src.LastDocumnetGeneratedId };
                            return doc;
                        }

                        return null;
                    }));



        }
        private static void RegisterAdCreativeSummaryDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ClickTagTracker, ClickTagTrackerDto>();
            cfg.CreateMap<ThirdPartyTracker, ThirdPartyTrackerDto>();
            cfg.CreateMap<ClickTagTrackerDto , ClickTagTracker>();
            cfg.CreateMap<ThirdPartyTrackerDto , ThirdPartyTracker>();
            cfg.CreateMap<AppSiteAdQueue, AppSiteAdQueueDto>();
            cfg.CreateMap<AppSiteAdQueue, AppSiteAdQueueDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.AppSite.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.AppSite == null ? string.Empty : item.AppSite.Name))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(z => z.AppSite.Account.ID))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(z => string.Format("{0} {1}", z.AppSite.Account.PrimaryUser.FirstName, z.AppSite.Account.PrimaryUser.LastName)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => item.AppSite == null ? string.Empty : item.AppSite.Type.Name.Value));


            cfg.CreateMap<AdCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
              .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
               {
                   if (src.ClickTags != null)
                   {
                       return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                   }
                   return null;
               }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));

            cfg.CreateMap<AdCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
               .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                {
                    if (src.ClickTags != null)
                    {
                        return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                    }
                    return null;
                }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));
            cfg.CreateMap<AdCreative, AdCreativeFullSummaryDto>()
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                 {
                     if (src.ClickTags != null)
                     {
                         return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                     }
                     return null;
                 }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));
            cfg.CreateMap<RichMediaRequiredProtocol, RichMediaRequiredProtocolDto>();
            cfg.CreateMap<RichMediaRequiredProtocolDto, RichMediaRequiredProtocol>();
            cfg.CreateMap<RichMediaCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom((src, dest, property, context) =>
                {
                    var RMProtocol = src.GetRichMediaProtocol();
                    if (RMProtocol != null)
                    {
                        return context.Mapper.Map<RichMediaRequiredProtocolDto>(RMProtocol);
                    }
                    return null;
                }))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                 {
                     if (src.ClickTags != null)
                     {
                         return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                     }
                     return null;
                 }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));
            cfg.CreateMap<RichMediaCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom((src, dest, property, context) =>
                {
                    var RMProtocol = src.GetRichMediaProtocol();
                    if (RMProtocol != null)
                    {
                        return context.Mapper.Map<RichMediaRequiredProtocolDto>(RMProtocol);
                    }
                    return null;
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                 {
                     if (src.ClickTags != null)
                     {
                         return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                     }
                     return null;
                 }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));
            cfg.CreateMap<RichMediaCreative, AdCreativeFullSummaryDto>()
               .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
              .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom((src, dest, property, context) =>
              {
                  var RMProtocol = src.GetRichMediaProtocol();
                  if (RMProtocol != null)
                  {
                      return context.Mapper.Map<RichMediaRequiredProtocolDto>(RMProtocol);
                  }
                  return null;
              }))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                 {
                     if (src.ClickTags != null)
                     {
                         return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                     }
                     return null;
                 }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));


            cfg.CreateMap<VideoEndCardCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))

                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                    .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                    {
                        if (src.ClickTags != null)
                        {
                            return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                        }
                        return null;
                    }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));

            cfg.CreateMap<VideoEndCardCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))

                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                    .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                    {
                        if (src.ClickTags != null)
                        {
                            return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                        }
                        return null;
                    }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));

            cfg.CreateMap<VideoEndCardCreative, AdCreativeFullSummaryDto>()
               .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))

              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                  .ForMember(p => p.ClickTags, z => z.MapFrom((src, dest) =>
                  {
                      if (src.ClickTags != null)
                      {
                          return src.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom((src, dest) =>
                      {
                          if (src.ThirdPartyTrackers != null)
                          {
                              return src.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }));

        }

        private static void RegisterFundtoFundDtoMapping(MapperConfigurationExpression cfg)
        {
            //cfg.CreateMap<Fund, FundDto>().ForMember(p => p.FundType, opt => opt.MapFrom(x => x.Type.Name.ToString()));
        }

        private static void RegisterBankAccountDtoToBankAccount(MapperConfigurationExpression cfg)
        {

            cfg.CreateMap<AccountPaymentDetails, PaymentDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));



            cfg.CreateMap<AccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            cfg.CreateMap<BankAccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            cfg.CreateMap<PayPalAccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            cfg.CreateMap<AccountPaymentDetailDto, BankAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());
            cfg.CreateMap<AccountPaymentDetailDto, PayPalAccountPaymentDetails>()
                //.ForMember(p => p.UserName, opt => opt.MapFrom(x => !string.IsNullOrWhiteSpace(x.UserName)?x.UserName:)
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            cfg.CreateMap<BankAccountPaymentDetails, AccountPaymentDetailDto>();
            cfg.CreateMap<PayPalAccountPaymentDetails, AccountPaymentDetailDto>();

            cfg.CreateMap<BankAccountPaymentDetails, AccountPaymentDetailFundDto>();
            cfg.CreateMap<PayPalAccountPaymentDetails, AccountPaymentDetailFundDto>();

            cfg.CreateMap<AccountPaymentDetailFundDto, BankAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());
            cfg.CreateMap<AccountPaymentDetailFundDto, PayPalAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());



            // cfg.CreateMap<SystemBankAccount, SystemBankAccountDto>();
            //cfg.CreateMap<SystemPayPalAccount, SystemPayPalAccountDto>();
        }

        private static void RegisterAccountToAccountAPIAccessDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Account, AccountAPIAccessDto>()
                .ForMember(p => p.AccountId, p => p.MapFrom(z => z.ID))
                .ForMember(p => p.APIClientId, z => z.MapFrom((src, dest) =>
                {
                    if (src.APIAccess != null)
                    {
                        return src.APIAccess.APIClientId;
                    }
                    return null;
                }))
                .ForMember(p => p.APISecretKey, z => z.MapFrom((src, dest) =>
                {
                    if (src.APIAccess != null)
                    {
                        return src.APIAccess.APISecretKey;
                    }
                    return null;
                }));
        }

        private static void RegisterCoreMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Tenant, TenantDto>();
            cfg.CreateMap<AccountFundPgw, PgwDto>();
            cfg.CreateMap<TextAdTheme, ThemeDto>();
            cfg.CreateMap<AccountSummary, AccountSummaryDto>();
            cfg.CreateMap<KPIConfig, KPIConfigDto>();
            cfg.CreateMap<Language, LanguageDto>();
            cfg.CreateMap<Language, LanguageSaveDto>();

            cfg.CreateMap<Keyword, KeywordDto>();
            cfg.CreateMap<Keyword, KeywordSaveDto>();
            cfg.CreateMap<AdCreativeAttribute, AdCreativeAttributeDto>();
            cfg.CreateMap<AdCreativeAttributeDto, AdCreativeAttribute>();
            cfg.CreateMap<Platform, PlatformDto>();
            cfg.CreateMap<CreativeVendorKeyword, CreativeVendorKeywordDto>()
           .ForMember(dest => dest.VendorId, opt => opt.MapFrom(item => item.Vendor == null ? 0 : item.Vendor.ID));
            cfg.CreateMap<CreativeVendorKeywordDto, CreativeVendorKeyword>()
.ForMember(dest => dest.Vendor, opt => opt.MapFrom(item => item.VendorId == 0 ? null : new CreativeVendor { ID = item.VendorId }));


            cfg.CreateMap<CreativeVendor, CreativeVendorDto>();
            cfg.CreateMap<PlatformVersion, PlatformVersionDto>();
            cfg.CreateMap<Manufacturer, ManufacturerDto>();
            cfg.CreateMap<Device, DeviceDto>()
                 .ForMember(p => p.Code, z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.Codes != null)
                     {
                         return string.Join(",", src.Codes.Select(M => M.Code));
                     }
                     return null;
                 }));
            cfg.CreateMap<Country, CountryDto>()
                .ForMember(p => p.Code, x => x.MapFrom(z => z.TwoLettersCode));
            cfg.CreateMap<Gender, GenderDto>();
            cfg.CreateMap<ImpressionMetric, ImpressionMetricDto>();
            cfg.CreateMap<MetricVendor, MetricVendorDto>();
            cfg.CreateMap<TileImageSize, TileImageSizeDto>()
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(item => item.DeviceType.ID));
            cfg.CreateMap<TileImage, TileImageDto>();
            cfg.CreateMap<Document, DocumentBaseDto>()
             .ForMember(dest => dest.UsedNameUp, opt => opt.MapFrom(item => item.GetNameWithNoExtension()));
            cfg.CreateMap<Document, DocumentDto>()
                      .ForMember(dest => dest.UsedNameUp, opt => opt.MapFrom(item => item.GetNameWithNoExtension()));
            cfg.CreateMap<TileImageDocument, TileImageDocumentDto>();
            cfg.CreateMap<CreativeUnitFormat, FormatDto>();
            cfg.CreateMap<CreativeUnit, CreativeUnitDto>();
            cfg.CreateMap<TileImageFormat, FormatDto>();
            cfg.CreateMap<CreativeUnitGroup, CreativeUnitGroupDto>();
            cfg.CreateMap<AgeGroup, AgeGroupDto>();
            cfg.CreateMap<DeviceType, DeviceTypeDto>();



            cfg.CreateMap<DeviceCapability, DeviceCapabilityDto>()
                .ForMember(dest => dest.WurflCapabilities, opt => opt.MapFrom(item => item.WurflCapabilities))
                .ForMember(dest => dest.WurflValue, opt => opt.MapFrom(item => item.WurflValue))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => item.Type));
            cfg.CreateMap<CostModelWrapper, CostModelWrapperDto>()
                .ForMember(dest => dest.CostModel, opt => opt.MapFrom(item => item.CostModel.ID));
            cfg.CreateMap<CostModel, CostModelDto>();

            /*cfg.CreateMap<Device, DeviceDto>()
    .ForMember(dest => dest.Platform, opt => opt.MapFrom(item => MapperHelper.Map<PlatformDto>(item.Platform)))
    .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(item => MapperHelper.Map<ManufacturerDto>(item.Manufacturer)));*/
            cfg.CreateMap<Currency, CurrencyDto>();
            cfg.CreateMap<CostElementValueDto, CostItemValue>().ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => MapperHelper.Map<CostModelWrapper>(item.CostModelWrapper)));
            cfg.CreateMap<CostItemValue, CostElementValueDto>().ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => MapperHelper.Map<CostModelWrapperDto>(item.CostModelWrapper)));

           // CostItemType FeeCalculatedFrom CostElementCalculatedFrom
            cfg.CreateMap<CostElement, CostElementDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type))
                 .ForMember(p => p.Values, z => z.MapFrom((src, dest, property, context) =>
                 {
                     if (src.Values != null)
                     {
                         return context.Mapper.Map<IList<CostElementValueDto>>(src.Values);
                     }
                     return null;
                 }))
                ;

            cfg.CreateMap<Fee, FeeDto>()
              .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type))
              .ForMember(p => p.Values, z => z.MapFrom((src, dest, property, context) =>
              {
                  if (src.Values != null)
                  {
                      return context.Mapper.Map<IList<CostElementValueDto>>(src.Values);
                  }
                  return null;
              }))
              ;
       
            cfg.CreateMap<JobPosition, JobPositionDto>();
            cfg.CreateMap<Party, EmployeeDto>();
            cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.JobPositionId, opt => opt.MapFrom(item => item.JobPosition == null ? 0 : item.JobPosition.ID))
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                          .ForMember(dest => dest.AccountName, opt => opt.MapFrom(item => item.Account == null ? "" : item.Account.GetName()));

            cfg.CreateMap<Party, BusinessPartnerDto>();
            cfg.CreateMap<BusinessPartner, BusinessPartnerDto>()
                   .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                                .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

                 .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID))
                    .ForMember(dest => dest.AdvertiserList, z => z.MapFrom((src, dest) =>
                    {
                        if (src.AdvertiserBlockList != null)
                        {
                            return src.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                        }
                        return new List<int>();
                    }))

                        .ForMember(dest => dest.BlockedDomains, z => z.MapFrom((src, dest) =>
                        {
                            if (src.DomainBlockList != null)
                            {
                                string result = string.Empty;
                                foreach (var d in src.DomainBlockList)
                                {
                                    result = result + d.Domain + "\n";
                                };
                                return result;
                            }
                            return string.Empty;
                        }))
                        .ForMember(dest => dest.AccountList, z => z.MapFrom((src, dest) =>
                        {
                            if (src.AccountWhiteList != null)
                            {
                                return src.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
                            }
                            return new List<int>();
                        }));



            //.ForMember(dest => dest.WebCreativeFormatsList, z => z.MapFrom(x =>
            //{
            //    if (x.WebCreativeFormatsList != null)
            //    {
            //        return x.WebCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
            //    }
            //    return new List<int>();
            //})).ForMember(dest => dest.MobileCreativeFormatsList, z => z.MapFrom(x =>
            //{
            //    if (x.MobileCreativeFormatsList != null)
            //    {
            //        return x.MobileCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
            //    }
            //    return new List<int>();
            //}));




            cfg.CreateMap<SSPPartner, BusinessPartnerDto>()
                      .ForMember(dest => dest.ImpressionTrackers, i => i.MapFrom(item => item.NumberOfSupportedImpressionTrackersInNative))
              .ForMember(dest => dest.ClicksTrackers, opt => opt.MapFrom(item => item.NumberOfSupportedClickTrackersInNative))
                           .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))
                .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID))
                .ForMember(dest => dest.AdvertiserList, z => z.MapFrom((src, dest) =>
                {
                    if (src.AdvertiserBlockList != null)
                    {
                        return src.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                    }
                    return new List<int>();
                }))
                   .ForMember(dest => dest.BlockedDomains, z => z.MapFrom((src, dest) =>
                   {
                       if (src.DomainBlockList != null)
                       {
                           string result = string.Empty;
                           foreach (var d in src.DomainBlockList)
                           {
                               result = result + d.Domain + "\n";
                           };
                           return result;
                       }
                       return string.Empty;
                   }))
                  .ForMember(dest => dest.AccountList, z => z.MapFrom((src, dest) =>
                  {
                      if (src.AccountWhiteList != null)
                      {
                          return src.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
                      }
                      return new List<int>();
                  }))

                      .ForMember(dest => dest.WebCreativeFormatsList, z => z.MapFrom((src, dest) =>
                      {
                          if (src.WebCreativeFormatsList != null)
                          {
                              return src.WebCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
                          }
                          return new List<int>();
                      }))
                       .ForMember(dest => dest.MobileCreativeFormatsList, z => z.MapFrom((src, dest) =>
                       {
                           if (src.MobileCreativeFormatsList != null)
                           {
                               return src.MobileCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
                           }
                           return new List<int>();
                       }));

            cfg.CreateMap<DSPPartner, BusinessPartnerDto>()
              .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                      .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

            .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID)).
            ForMember(dest => dest.AdvertiserList, z => z.MapFrom((src, dest) =>
            {
                if (src.AdvertiserBlockList != null)
                {
                    return src.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                }
                return new List<int>();
            }))
               .ForMember(dest => dest.BlockedDomains, z => z.MapFrom((src, dest) =>
               {
                   if (src.DomainBlockList != null)
                   {
                       string result = string.Empty;
                       foreach (var d in src.DomainBlockList)
                       {
                           result = result + d.Domain + "\n";
                       };
                       return result;
                   }
                   return string.Empty;
               }))
                .ForMember(dest => dest.AccountList, z => z.MapFrom((src, dest) =>
                {
                    if (src.AccountWhiteList != null)
                    {
                        return src.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
                    }
                    return new List<int>();
                }))

            ;


            cfg.CreateMap<DPPartner, BusinessPartnerDto>()
  .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
.ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
.ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

.ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID))
.ForMember(dest => dest.AdvertiserList, z => z.MapFrom((src, dest) =>
{
    if (src.AdvertiserBlockList != null)
    {
        return src.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
    }
    return new List<int>();
}))
  .ForMember(dest => dest.AccountList, z => z.MapFrom((src, dest) =>
  {
      if (src.AccountWhiteList != null)
      {
          return src.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
      }
      return new List<int>();
  }))
;


            cfg.CreateMap<AdGroupCostElement, AdGroupCostElementDto>()
                //.ForMember(dest => dest.Value, opt => opt.MapFrom(item =>item.CostElement == null || item.CostElement.Type == CostElementType.Fixed? item.Value :item.Value*100))
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(item => item.Beneficiary == null ? string.Empty : item.Beneficiary.ToString()))
                .ForMember(dest => dest.BeneficiaryId, opt => opt.MapFrom(item => item.Beneficiary == null ? (int?)null : item.Beneficiary.ID))
                 .ForMember(dest => dest.Provider, opt => opt.MapFrom(item => item.Provider == null ? string.Empty : item.Provider.Name))
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(item => item.Provider == null ? (int?)null : item.Provider.ID))
                .ForMember(dest => dest.CostElement, opt => opt.MapFrom(item => item.CostElement == null ? string.Empty : item.CostElement.GetCustomDescription()))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(item => item.CostElement == null || item.CostElement.Type == CalculationType.Fixed ? "$" : "%"))
                .ForMember(dest => dest.CostElementId, opt => opt.MapFrom(item => item.CostElement == null ? (int?)null : item.CostElement.ID))
                .ForMember(dest => dest.IsOneTime, opt => opt.MapFrom(item => item.CostElement.IsOneTime))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(item => Math.Round(item.GetReadableValue(), 2)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => item.CostElement.Type));


            cfg.CreateMap<AdGroupFee, AdGroupFeeDto>()
           //.ForMember(dest => dest.Value, opt => opt.MapFrom(item =>item.CostElement == null || item.CostElement.Type == CostElementType.Fixed? item.Value :item.Value*100))
           .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(item => item.Beneficiary == null ? string.Empty : item.Beneficiary.ToString()))
           .ForMember(dest => dest.BeneficiaryId, opt => opt.MapFrom(item => item.Beneficiary == null ? (int?)null : item.Beneficiary.ID))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(item => item.Provider == null ? string.Empty : item.Provider.Name))
           .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(item => item.Provider == null ? (int?)null : item.Provider.ID))
           .ForMember(dest => dest.Fee, opt => opt.MapFrom(item => item.Fee == null ? string.Empty : item.Fee.GetDescription()))
           .ForMember(dest => dest.TypeName, opt => opt.MapFrom(item => item.Fee == null || item.Fee.Type == CalculationType.Fixed ? "$" : "%"))
           .ForMember(dest => dest.FeeId, opt => opt.MapFrom(item => item.Fee == null ? (int?)null : item.Fee.ID))

           .ForMember(dest => dest.Value, opt => opt.MapFrom(item => Math.Round(item.GetReadableValue(), 2)))
           .ForMember(dest => dest.Type, opt => opt.MapFrom(item => item.Fee.Type));



            cfg.CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type));
            cfg.CreateMap<Campaign, CampaignSettingsDto>()
                .ForMember(dest => dest.ValidCostModelWrapper, opt => opt.MapFrom(item => item.GetValidCostModelWrapper().HasValue ? (int?)item.GetValidCostModelWrapper() : (int?)null))
                .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.CostModelWrapper.HasValue ? (int?)item.CostModelWrapper : (int?)null));

            cfg.CreateMap<CampaignServerSetting, CampaignServerSettingDto>()
                .ForMember(p => p.Name, x => x.MapFrom(z => z.Campaign.Name))
                .ForMember(p => p.ID, x => x.MapFrom(z => z.Campaign.ID));

            cfg.CreateMap<CampaignFrequencyCapping, CampaignFrequencyCappingDto>()
                .ForMember(p => p.EventId, x => x.MapFrom(z => z.Event.ID))
                .ForMember(p => p.EventName, x => x.MapFrom(z => z.Event.EventName))
                  .ForMember(p => p.EventDescription, x => x.MapFrom(z => trackingEventRepository.GetAll().Where(y => y.EventName == z.Event.EventName).FirstOrDefault().GetDescription()));


            cfg.CreateMap<CampaignServerSettingDto, CampaignServerSetting>().ForMember(p => p.AgencyCommission, x => x.MapFrom(z => (AgencyCommission)z.AgencyCommission));
            cfg.CreateMap<LanguageDto, Language>();
            cfg.CreateMap<LanguageSaveDto, Language>();

            cfg.CreateMap<KeywordDto, Keyword>();
            cfg.CreateMap<KeywordSaveDto, Keyword>();
            cfg.CreateMap<LocationDto, LocationBase>();
            cfg.CreateMap<CreativeVendorDto, CreativeVendor>();
            cfg.CreateMap<PlatformDto, Platform>();
            cfg.CreateMap<ManufacturerDto, Manufacturer>();
            cfg.CreateMap<DeviceDto, Device>();
            cfg.CreateMap<GenderDto, Gender>();
            cfg.CreateMap<Metric, MetricDto>();
            cfg.CreateMap<MetricDto, Metric>();

            cfg.CreateMap<Metric, MetricResultDto>();
            cfg.CreateMap<MetricResultDto, Metric>();
            cfg.CreateMap<PaymentTypeDto, PaymentType>();

            cfg.CreateMap<ImpressionMetricDto, ImpressionMetric>();
            cfg.CreateMap<MetricVendorDto, MetricVendor>();
            cfg.CreateMap<TileImageSizeDto, TileImageSize>();
            cfg.CreateMap<TileImageDto, TileImage>();
            cfg.CreateMap<DocumentTypeDto,DocumentType>();
            cfg.CreateMap<DocumentType,DocumentTypeDto>();

            cfg.CreateMap<DocumentDto, Document>()
            .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(item => _documentTypeRepository.Get(item.DocumentTypeId)));

            cfg.CreateMap<TileImageDocumentDto, TileImageDocument>();
            cfg.CreateMap<FormatDto, CreativeUnitFormat>();
            cfg.CreateMap<CreativeUnitGroupDto, CreativeUnitGroup>();
            cfg.CreateMap<FormatDto, TileImageFormat>();
            cfg.CreateMap<AgeGroupDto, AgeGroup>();
            cfg.CreateMap<DeviceCapabilityDto, DeviceCapability>();
            cfg.CreateMap<CurrencyDto, Currency>();
            cfg.CreateMap<CostElementDto, CostElement>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (CalculationType)item.TypeId));

            cfg.CreateMap<FeeDto, Fee>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (CalculationType)item.TypeId));
            cfg.CreateMap<JobPositionDto, JobPosition>();
            cfg.CreateMap<PartyDto, Employee>();
            cfg.CreateMap<Employee, PartyDto>().ForMember(dest => dest.AccountName,
                             opt => opt.MapFrom(x => x.Account != null ? x.Account.GetName() : ""));

            cfg.CreateMap<PartyDto, Account>();
            cfg.CreateMap<Account, PartyDto>();

            cfg.CreateMap<PartyDto, Party>();
            cfg.CreateMap<Party, PartyDto>();

            cfg.CreateMap<PartyDto, BusinessPartner>().ForMember(dest => dest.Type,
                             opt => opt.Ignore());
            cfg.CreateMap<BusinessPartner, PartyDto>();
            cfg.CreateMap<PartyDto, SSPPartner>().ForMember(dest => dest.Type,
                         opt => opt.Ignore());

            cfg.CreateMap<PartyDto, DPPartner>().ForMember(dest => dest.Type,
             opt => opt.Ignore());
            cfg.CreateMap<DPPartner, PartyDto>();
            cfg.CreateMap<PartyDto, DSPPartner>().ForMember(dest => dest.Type,
                         opt => opt.Ignore());
            cfg.CreateMap<AdGroupCostElementDto, AdGroupCostElement>();
            cfg.CreateMap<AdGroupFeeDto, AdGroupFee>();
            cfg.CreateMap<DiscountDto, Discount>()
              .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (DiscountType)item.TypeId));

            cfg.CreateMap<HouseAd, HouseAdDto>();
            //.ForMember(dest => dest.DestinationAppSites, opt => opt.MapFrom(item=>string.Join(",",item.DestinationAppSites.Select(x=>x.ID))));
            cfg.CreateMap<CostModelWrapperDto, CostModelWrapper>()
                .ForMember(p => p.CostModel, x => x.MapFrom((src, dest) =>
                {

                    CostModel costModel = new CostModel() { ID = src.ID };
                    return costModel;
                }));

            cfg.CreateMap<AppMarketingPartnerTracker, AppMarketingPartnerTrackerDto>();

            cfg.CreateMap<AppMarketingPartnerDto, AppMarketingPartner>();
            cfg.CreateMap<AppMarketingPartner,AppMarketingPartnerDto>();

            
            cfg.CreateMap<Advertiser, AdvertiserDto>();
            cfg.CreateMap<AdvertiserDto, Advertiser>();

            cfg.CreateMap<CreativeFormat, CreativeFormatsDto>();
            cfg.CreateMap<CreativeFormatsDto, CreativeFormat>();



            cfg.CreateMap<SSPPartnerWhiteIP, WhitleListIPDto>()
              .ForMember(dest => dest.IPString,
                         opt => opt.MapFrom(item => new IPAddress(item.IP).ToString()))
                 .ForMember(p => p.SSPPartnerId, x => x.MapFrom(z => z.SSPPartner.ID));

            cfg.CreateMap<WhitleListIPDto, SSPPartnerWhiteIP>()
  .ForMember(dest => dest.IP,
             opt => opt.MapFrom(item => IpHelper.ConvertIPToBytes(item.IPString)))
     .ForMember(p => p.SSPPartner, opt => opt.MapFrom(item => new SSPPartner { ID = item.SSPPartnerId }));


        }

        private static void RegisterCampaignDtoToCampaignMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CampaignDto, Campaign>()
                .ForMember(dest => dest.CostModelWrapper, opt => opt.Ignore());
        }

        private static void RegisterCampaignBidConfigDtoCampaignBidConfigMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdGroupBidConfig, CampaignBidConfigDto>()
              .ForMember(p => p.AdGroupId, x => x.MapFrom(z => z.AdGroup.ID))
              .ForMember(p => p.AdGrouptName, x => x.MapFrom(z => z.AdGroup.Name))
              .ForMember(p => p.CampaingName, x => x.MapFrom(z => z.AdGroup.Campaign.Name))
              .ForMember(p => p.MinBid, x => x.MapFrom(z => z.AdGroup.GetReadableBid()))
              .ForMember(p => p.AccountName, x => x.MapFrom(z => z.Account.GetName()))
              .ForMember(p => p.AccountId, x => x.MapFrom(z => z.Account.ID))
              .ForMember(p => p.AdGroupPricingModel, x => x.MapFrom((src, dest) =>
              {
                  if (src.AdGroup.CostModelWrapper != null)
                      return src.AdGroup.CostModelWrapper.GetDescription();
                  return ResourceManager.Instance.GetResource("Default", "Campaign");
              }
                 ))
              .ForMember(p => p.AppsitePricingModel, x => x.MapFrom((src, dest) =>
              {
                  var pricingModel = src.AppSite.AppSiteServerSetting.GetPricingModel();
                  if (pricingModel != null)
                      return pricingModel.GetDescription();
                  return ResourceManager.Instance.GetResource("Default", "Campaign");
              }

                 )).ForMember(p => p.AppsitePricingModelId, x => x.MapFrom((src, dest) =>
                 {
                     var pricingModel = src.AppSite.AppSiteServerSetting.GetPricingModel();
                     if (pricingModel != null)
                         return pricingModel.ID;
                     return -1;
                 }));


            cfg.CreateMap<AdGroupInventorySource, InventorySourceDto>()
            .ForMember(p => p.AdGroupId, x => x.MapFrom(z => z.AdGroup.ID))
            .ForMember(p => p.AdGrouptName, x => x.MapFrom(z => z.AdGroup.Name))
            .ForMember(p => p.CampaingName, x => x.MapFrom(z => z.AdGroup.Campaign.Name))
             .ForMember(p => p.SSPId, x => x.MapFrom(z => z.Partner.ID))
            .ForMember(p => p.ExchangeName, x => x.MapFrom(z => z.Partner.Name))
            .ForMember(p => p.AccountId, x => x.MapFrom(z => z.Partner.Account.ID))
                  .ForMember(p => p.SubPublisherMarketId, x => x.MapFrom((src, dest) =>
                  {
                      if (src.SubAppsite != null)
                      {
                          return src.SubAppsite.SubPublisherMarketId;
                      }
                      return string.Empty;
                  }))





            .ForMember(p => p.SubPublisher, x => x.MapFrom((src, dest) =>
            {
                if (src.SubAppsite != null)
                {
                    return src.SubAppsite.SubPublisherName;
                }
                return null;
            }))

             .ForMember(p => p.subPublisherId, x => x.MapFrom((src, dest) =>
             {
                 if (src.SubAppsite != null)
                 {
                     return src.SubAppsite.SubPublisherId;
                 }
                 return null;
             }));
        }

        private static void RegisterCampaignDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Campaign, CampaignDto>()
                .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.CostModelWrapper.HasValue ? (int?)item.CostModelWrapper.Value : (int?)null))
                .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(item => item.User != null ? item.User.GetName() : string.Empty));

            ; cfg.CreateMap<AdCreativeUnitVendor, AdCreativeUnitVendorDto>()
        .ForMember(dest => dest.UnitId, opt => opt.MapFrom(item => item.Unit != null ? item.Unit.ID : 0))
            .ForMember(dest => dest.VendorId, opt => opt.MapFrom(item => item.Vendor != null ? item.Vendor.ID : 0))
  .ForMember(dest => dest.VendorText, opt => opt.MapFrom(item => item.Vendor != null ? item.Vendor.Name.GetValue() : string.Empty))
            ;

            cfg.CreateMap<AdCreativeUnitVendorDto, AdCreativeUnitVendor>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(item => item.UnitId > 0 ? new AdCreativeUnit { ID = item.UnitId } : null))
                .ForMember(dest => dest.Vendor, opt => opt.MapFrom(item => item.VendorId > 0 ? new CreativeVendor { ID = item.VendorId } : null))

                ;


            cfg.CreateMap<AdGroupObjectiveType, ObjectiveTypeDto>();
            cfg.CreateMap<ObjectiveTypeDto, AdGroupObjectiveType>();
            cfg.CreateMap<Campaign, CampaignListDto>().ForMember(dest => dest.StatusId, opt => opt.MapFrom(item => item.Status != null ?  item.Status.ID : 0));
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()));

            cfg.CreateMap<AdGroup, AdGroupListDto>()
                 .ForMember(dest => dest.StatusId, opt => opt.MapFrom(item => item.Status != null ? item.Status.ID : 0));
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()));
            cfg.CreateMap<AdGroup, AdGroupDto>()
                 .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(item => (AdActionTypeIds)item.Objective.AdAction.ID))
                 .ForMember(dest => dest.ObjectiveTypeId, opt => opt.MapFrom(item => (AdGroupObjectiveTypeIds)item.Objective.Objective.ID))
                 .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => item.Objective.AdType == null ? new AdTypeIds?() : (AdTypeIds)item.Objective.AdType.ID))
                  .ForMember(dest => dest.IsCostModelChanged, opt => opt.MapFrom(item => item.IsCostModelChanged));

            cfg.CreateMap<AdGroupDto, AdGroup>();

            cfg.CreateMap<AdCreative, AdCreativeDto>();

            cfg.CreateMap<AdCreative, AdListDto>()
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status.GetDescription()))
                  .ForMember(dest => dest.StatusId, opt => opt.MapFrom(item => item.Status == null ? 0 : item.Status.ID));
            cfg.CreateMap<AdCreative, AdbIDListDto>()
                .ForMember(p => p.Bid, x => x.MapFrom(z => z.GetReadableBid().ToString()));

            cfg.CreateMap<AdActionValue, AdActionValueDto>()
                .ForMember(p => p.Trackers, z => z.MapFrom((src, dest, property, context) =>
                {
                    if (src.Trackers != null)
                    {
                        return context.Mapper.Map<IList<AdActionValueTrackerDto>>(src.Trackers.Where(w => !w.IsDeleted));
                    }
                    return null;
                }));
            //cfg.CreateMap<AdActionValue, AdActionValueRichMediaDto>();


            cfg.CreateMap<AdActionValueDto, AdActionValue>();
           // cfg.CreateMap<AdCreativeUnit, CreativeUnitDto>();
            cfg.CreateMap<AdCreativeUnit, AdCreativeUnitDto>()
                .ForMember(dest => dest.CreativeUnitId, opt => opt.MapFrom(item => item.CreativeUnit.ID))
                //  .ForMember(dest => dest.InStreamVideoCreativeUnit, opt => opt.MapFrom(item => MapperHelper.Map<InStreamVideoCreativeUnitDto>(item.CreativeUnit)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : item.CreativeUnit.GetDescription()))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(item => item.Document != null ? item.Document.ID : (int?)null))
                  .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(item => item.Document != null ? item.Document.Extension : string.Empty))
                 .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : string.Empty))
                .ForMember(dest => dest.SnapshotDocumentId, opt => opt.MapFrom(item => item.SnapshotDocument != null ? item.SnapshotDocument.ID : (int?)null))
                  .ForMember(dest => dest.AdCreativeVendorIds, opt => opt.MapFrom((src, dest) =>
                  {
                      //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                      IEnumerable<AdCreativeUnitVendorDto> adCreativeVendors = src.AdCreativeUnitVendorList.Select(x =>
                          new AdCreativeUnitVendorDto()
                          {
                              ID = x.ID,
                              VendorId = x.Vendor.ID,
                              UnitId = x.Unit.ID,
                              VendorText = x.Vendor.Name.GetValue()



                          });
                      //  IList<AdActionValueTrackerDto>
                      return adCreativeVendors;
                  }))
                 .ForMember(dest => dest.CreativeVendorIds, opt => opt.MapFrom((src, dest) =>
                 {
                     //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                     IEnumerable<int> adCreativeVendors = src.AdCreativeUnitVendorList.Select(x =>
                      x.Vendor.ID);
                     //  IList<AdActionValueTrackerDto>
                     return adCreativeVendors;
                 }))

                .ForMember(dest => dest.ImpressionTrackerRedirect, opt => opt.MapFrom((src, dest) =>
                {

                    var impressionTracker = src.GetTrackers().FirstOrDefault();
                    if (impressionTracker != null)
                    {
                        return impressionTracker.TrackingUrl;
                    }

                    return string.Empty;
                }))
                    .ForMember(dest => dest.ImpressionTrackerJSRedirect, opt => opt.MapFrom((src, dest) =>
                    {

                        var impressionTracker = src.GetTrackers().FirstOrDefault();
                        if (impressionTracker != null)
                        {
                            return impressionTracker.TrackingJS;
                        }

                        return string.Empty;
                    }))


                ;

            cfg.CreateMap<InStreamVideoCreativeUnit, InStreamVideoCreativeUnitDto>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
            .ForMember(dest => dest.BitRate, opt => opt.MapFrom(item => item.BitRate))
            .ForMember(dest => dest.VideoHeight, opt => opt.MapFrom(item => item.Height))
            .ForMember(dest => dest.VideoWidth, opt => opt.MapFrom(item => item.Width))
            .ForMember(dest => dest.ThumbnailDocId, opt => opt.MapFrom(item => item.ThumbnailDoc != null ? item.ThumbnailDoc.ID : 0))
                    .ForMember(dest => dest.OriginalCreativeUnitID, opt => opt.MapFrom(item => item.OriginalCreativeUnit != null ? item.OriginalCreativeUnit.ID : 0))
            .ForMember(dest => dest.VideoType, opt => opt.MapFrom(item => item.VideoType.ID))
            .ForMember(dest => dest.VideoTypeCode, opt => opt.MapFrom(item => item.VideoType.MIME))
            .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(item => item.DeliveryMethod.ID))

            //.ForMember(dest => dest.CreativeVendorId, opt => opt.MapFrom(item => item.AdCreativeUnit.AdCreativeUnitVendor != null ? item.AdCreativeUnit.AdCreativeUnitVendor.ID : (int?)null))
            .ForMember(dest => dest.ImpressionTrackerRedirectList, opt => opt.MapFrom((src, dest) =>
            {
                //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                IEnumerable<AdCreativeUnitTrackerDto> impressionTracker = src.AdCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code.ToLower() != IMPRESSIONEVENT && M.AdGroupEvent.Code.ToLower() != CLICKEVENT).Select(x =>
                      new AdCreativeUnitTrackerDto()
                      {
                          ImpressionURls = src.AdCreativeUnit.GetTrackers().Where(y => y.AdGroupEvent.ID == x.AdGroupEvent.ID).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList()
                          ,
                          // Get the localized Name from  trackingEvent 
                          AdGroupEventName = trackingEventRepository.GetAll().Where(y => y.EventName == x.AdGroupEvent.Description).FirstOrDefault().GetDescription(),  // x.AdGroupEvent.Description,
                          Url = x.TrackingUrl,
                          AdGroupEventId = x.AdGroupEvent.ID,
                          AdCreativeUnitId = x.CreativeUnit.ID
                      });

                impressionTracker = impressionTracker
                .GroupBy(x => x.AdGroupEventId)
                .Select(x => x.First());
                //  IList<AdActionValueTrackerDto>
                return impressionTracker;
            }))

             .ForMember(dest => dest.ImpressionTrackerJSRedirectList, opt => opt.MapFrom((src, dest) =>
             {
                 //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                 IEnumerable<AdCreativeUnitTrackerDto> impressionTracker = src.AdCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code.ToLower() != IMPRESSIONEVENT && M.AdGroupEvent.Code.ToLower() != CLICKEVENT).Select(x =>
                       new AdCreativeUnitTrackerDto()
                       {
                           ImpressionURls = src.AdCreativeUnit.GetTrackers().Where(y => y.AdGroupEvent.ID == x.AdGroupEvent.ID).Select(y => new AdActionValueTrackerDto() { JS = y.TrackingJS }).ToList()
                           ,
                           // Get the localized Name from  trackingEvent 
                           AdGroupEventName = trackingEventRepository.GetAll().Where(y => y.EventName == x.AdGroupEvent.Description).FirstOrDefault().GetDescription(),  // x.AdGroupEvent.Description,
                           Url = x.TrackingJS,
                           AdGroupEventId = x.AdGroupEvent.ID,
                           AdCreativeUnitId = x.CreativeUnit.ID
                       });

                 impressionTracker = impressionTracker
                 .GroupBy(x => x.AdGroupEventId)
                 .Select(x => x.First());
                 //  IList<AdActionValueTrackerDto>
                 return impressionTracker;
             }))
            ;



            // .ForMember(dest => dest.SnapshotDocumentId, opt => opt.MapFrom(item => item.SnapshotDocument != null ? item.SnapshotDocument.ID : (int?)null));
            //ALid
            //  .ForMember(dest => dest.CreativeUnit, opt => opt.MapFrom(item => MapperHelper.Map<CreativeUnitDto>(item.CreativeUnit)))

            //   .ForMember(dest => dest.InStreamVideoCreativeUnit, opt => opt.MapFrom(item => MapperHelper.Map<InStreamVideoCreativeUnitDto>(item.InStreamVideoCreativeUnit)));
            // .ForMember(dest => dest.InStreamVideoCreativeUnit.ImpressionTrackerRedirectList, opt => opt.MapFrom(item =>
            //{
            //    IEnumerable<AdCreativeUnitTrackerDto> impressionTracker = item.GetTrackers().Select(x => new AdCreativeUnitTrackerDto() { Url = x.TrackingUrl, AdGroupEventId = x.AdGroupEvent.ID, AdCreativeUnitId = x.CreativeUnit.ID });
            //    return impressionTracker;
            //}));


            cfg.CreateMap<TileImageDocument, AdCreativeUnitDto>()
                .ForMember(dest => dest.CreativeUnitId, opt => opt.MapFrom(item => item.TileImageSize.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : string.Empty))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(item => item.Document != null ? item.Document.ID : (int?)null));

           

            cfg.CreateMap<AdType, LookupDto>();
            cfg.CreateMap<LookupDto, AdType>();
            cfg.CreateMap<LookupDto, MatchType>();
            cfg.CreateMap< MatchType,LookupDto > ();
            cfg.CreateMap<LookupDto, AdCreativeStatus>();
            cfg.CreateMap<AdCreativeStatus, LookupDto>();

            cfg.CreateMap<CompanyType, LookupDto>();
            cfg.CreateMap<LookupDto, CompanyType>();

            cfg.CreateMap<AdSubType, AdSubtypeDto>()
                                .ForMember(dest => dest.AdTypeId, opt => opt.MapFrom(item => item.AdType.ID))
                                .ForMember(dest => dest.AdActionTypeIds, opt => opt.MapFrom(item => item.AdTypeActions.Select(x => x.ActionType.ID)))
                                .ForMember(dest=>dest.Permission , opt=> opt.MapFrom(item => item.Permission))
                                ;


            cfg.CreateMap<AdSubType, LookupDto>();
            cfg.CreateMap<LookupDto, AdSubType>();

         


            cfg.CreateMap<NativeAdLayout, LookupDto>();
            cfg.CreateMap<LookupDto, NativeAdLayout>();
        }
        private static void RegisterAdActionTypeConstraintMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdActionTypeConstraint, AdActionTypeConstraintDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                .ForMember(dest => dest.DeviceConstraint, opt => opt.MapFrom(item => item.DeviceConstraint));
        }

        private static void RegisterAdActionTypeMapping(MapperConfigurationExpression cfg)
        {

            cfg.CreateMap<PortalPermision, AdPermissionDto>();
            cfg.CreateMap<AdPermissionDto, PortalPermision>();
            cfg.CreateMap<LookupDto, PortalPermision>();

            cfg.CreateMap<AdActionCostModelWrapper, AdActionCostModelWrapperDto>()
           .ForMember(dest => dest.CostModelWrapperId,
             opt => opt.MapFrom(item => item.CostModelWrapper != null ? item.CostModelWrapper.ID : 0));

            cfg.CreateMap<AdType, AdTypeDto>().ForMember(dest => dest.AdPermission, opt => opt.MapFrom(item => item.Permission));

            cfg.CreateMap<AdActionTypeBase, AdActionTypeDto>()
                  .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
            .ForMember(dest => dest.ShowInAppId, opt => opt.MapFrom(item => item.ShowInAppId))
            .ForMember(dest => dest.CostModelWrappers, opt => opt.MapFrom(item => item.AdActionCostModelWrappers == null ? null : item.AdActionCostModelWrappers.Select(p => p.CostModelWrapper.ID).ToList()));
        }

        private static void RegisterLocationMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LocationBase, Continent>();
            cfg.CreateMap<LocationBase, Country>();
            cfg.CreateMap<LocationBase, State>();
            cfg.CreateMap<LocationBase, City>();

            cfg.CreateMap<LocationBase, LocationDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(item => item.Parent != null ? item.Parent.ID : new int?()))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(item => item.Locations))
                .ForMember(dest => dest.Type, opt => opt.MapFrom((src, dest) =>
                {
                    if (src is Country)
                    {
                        return LocationType.Country;
                    }
                    if (src is City)
                    {
                        return LocationType.City;
                    }
                    if (src is State)
                    {
                        return LocationType.State;
                    }
                    return LocationType.Continent;

                }));

        }

       
        private static void RegisterTargetingMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TargetingBase, TargetingBaseDto>()
                     .Include<VideoTargeting, VideoTargetingDto>()
                .Include<DeviceTargeting, DeviceTargetingDto>()
                .Include<GeographicTargeting, GeographicTargetingDto>()
                .Include<OperatorTargeting, OperatorTargetingDto>()
                   .Include<AdRequestTargeting, AdRequestTargetingDto>()
                     .Include<ImpressionMetricTargeting, ImpressionMetricTargetingDto>()
                .Include<IPTargeting, IPTargetingDto>()
                .Include<DemographicTargeting, DemographicTargetingDto>()
                .Include<KeywordTargeting, KeywordTargetingDto>()
                   .Include<LanguageTargeting, LanguageTargetingDto>()

                .Include<URLTargeting, URLTargetingDto>()
                .Include<GeoFencingTargeting, GeoFencingTargetingDto>();
            //cfg.CreateMap<DeviceTargeting, DeviceTargetingDto>();
            cfg.CreateMap<DeviceTargeting, DeviceTargetingDto>()
                .ForMember(dest => dest.Platforms, opt => opt.MapFrom((src, dest, property, context) =>
                {
                    List<PlatformDto> platformsDto = context.Mapper.Map<List<PlatformDto>>(src.Platforms);
                    foreach (var platform in platformsDto)
                    {
                        if (src.PlatformsTargeting.Where(p => p.Platform.ID == platform.ID).SingleOrDefault().IsAll)
                        {
                            platform.IsSelected = true;
                        }

                        PlatformTargeting targetingPlatform = src.PlatformsTargeting.Where(p => p.Platform.ID == platform.ID).SingleOrDefault();
                        if (targetingPlatform != null && !string.IsNullOrEmpty(targetingPlatform.MinimumVersion))
                        {
                            platform.Versions.Where(p => p.Code == targetingPlatform.MinimumVersion).SingleOrDefault().IsSelected = true;
                        }
                    }
                    return platformsDto;
                }
                  ))
                .ForMember(dest => dest.Devices, opt => opt.MapFrom(item => item.Devices))
                .ForMember(dest => dest.TargetingType, opt => opt.MapFrom(item => item.TargetingType))
                .ForMember(dest => dest.DeviceCapabilities, opt => opt.MapFrom(item => item.DeviceCapabilities))
                .ForMember(dest => dest.DeviceTypes, opt => opt.MapFrom(item => item.DeviceTypes))
                .ForMember(dest => dest.Manufacturers, opt => opt.MapFrom(item => item.ManufacturersIsAll));

            cfg.CreateMap<GeographicTargeting, GeographicTargetingDto>();
            cfg.CreateMap<OperatorTargeting, OperatorTargetingDto>();
            cfg.CreateMap<DemographicTargeting, DemographicTargetingDto>();
            cfg.CreateMap<KeywordTargeting, KeywordTargetingDto>();
            cfg.CreateMap<LanguageTargeting, LanguageTargetingDto>();
            cfg.CreateMap<VideoTargeting, VideoTargetingDto>();
            cfg.CreateMap<URLTargeting, URLTargetingDto>();
            cfg.CreateMap<GeoFencingTargeting, GeoFencingTargetingDto>();
            cfg.CreateMap<IPTargeting, IPTargetingDto>()
                .ForMember(dest => dest.StartRange,
                           opt => opt.MapFrom(item => new IPAddress(item.StartRange).ToString()))
                .ForMember(dest => dest.EndRange,
                           opt => opt.MapFrom(item => new IPAddress(item.EndRange).ToString()));
            cfg.CreateMap<Demographic, DemographicDto>();
            cfg.CreateMap<DeviceTargetingType, DeviceTargetingTypeDto>();
            cfg.CreateMap<TargetingType, TargetingTypeDto>();
        }

        private static void RegisterDeviceMapping(MapperConfigurationExpression cfg)
        {
            // cfg.CreateMap<CostItem, Fee>();
            // cfg.CreateMap<CostItem, CostElement>();
            

            cfg.CreateMap<LookupDto, ManagedLookupBase>()
            .Include<DeviceDto, Device>()
            .Include<ManufacturerDto, Manufacturer>()
            .Include<PlatformDto, Platform>()
            .Include<MetricVendorDto, MetricVendor>()

            //.Include<CostElementDto, CostItem>()
            .Include<CostElementDto, CostElement>()

                .Include<FeeDto, Fee>()
            .Include<CurrencyDto, Currency>()
            .Include<CreativeVendorDto, CreativeVendor>()
            .Include<AdvertiserDto, Advertiser>()
                        .Include<CreativeFormatsDto, CreativeFormat>()

            .Include<DeviceCapabilityDto, DeviceCapability>()
            .Include<AdCreativeAttributeDto, AdCreativeAttribute>();

            cfg.CreateMap<ManagedLookupBase, LookupDto>()
            .Include<Device, DeviceDto>()
            .Include<MetricVendor, MetricVendorDto>()
            .Include<Manufacturer, ManufacturerDto>()
            .Include<CreativeVendor, CreativeVendorDto>()
            .Include<Platform, PlatformDto>()
            
         .Include<CostItem, CostElementDto>()
            //.Include<CostElement, CostElementDto>()
            //.Include<Fee, FeeDto>()
            .Include<Currency, CurrencyDto>()
                        .Include<CreativeFormat, CreativeFormatsDto>()

            .Include<Advertiser, AdvertiserDto>()
            .Include<DeviceCapability, DeviceCapabilityDto>()
            .Include<AdCreativeAttribute, AdCreativeAttributeDto>()
            .Include<Keyword, KeywordSaveDto>()
                        .Include<Language, LanguageSaveDto>()

            .Include<CostModelWrapper, CostModelWrapperDto>()
            .Include<CostModel, CostModelDto>()
            .Include<LocationBase, LocationDto>();
            cfg.CreateMap<CostItem, CostElementDto>();

            cfg.CreateMap<Platform,PlatformDto>().ForMember(p => p.Versions, z => z.MapFrom((src, dest, property, context) =>
            {
                if (src.Versions != null)
                {
                    return context.Mapper.Map<IList<PlatformVersionDto>>(src.Versions);
                }
                return null;
            }));
            cfg.CreateMap<LookupDto, Currency>();
            cfg.CreateMap<LookupDto, Operator>();
            cfg.CreateMap<LookupDto, DeviceType>();
        }

        private static void RegisterAudienceSegmentMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AudienceSegment, AudienceSegmentDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                    .ForMember(dst => dst.ParentId, opt => opt.MapFrom(z => z.Parent != null ? z.Parent.ID : 0))
                                .ForMember(dst => dst.ProviderId, opt => opt.MapFrom(z => z.Provider != null ? z.Provider.ID : 0))
                                 .ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                                         .ForMember(dst => dst.UserId, opt => opt.MapFrom(z => z.User != null ? z.User.ID : 0))
                                  .ForMember(dst => dst.AdvertiserId, opt => opt.MapFrom(z => z.Advertiser != null ? z.Advertiser.ID : 0))
                                         .ForMember(dst => dst.ProviderName, opt => opt.MapFrom(z => z.Provider != null ? z.Provider.Name : string.Empty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                    .ForMember(dest => dest.CodeUQ, opt => opt.MapFrom(item => item.Code))
                            .ForMember(dest => dest.IsSelectedable, opt => opt.MapFrom(item => item.Selectable))
                                        .ForMember(dest => dest.IsPermissionNeed, opt => opt.MapFrom(item => item.IsPermissionNeed));



            cfg.CreateMap<AudienceSegmentDto, AudienceSegment>()
               .ForMember(dest => dest.Provider, opt => opt.MapFrom(item => DPPartnerRepository.Get(item.ProviderId)))
               .ForMember(dest => dest.Selectable, opt => opt.MapFrom(item => item.IsSelectedable))
               .ForMember(dest => dest.Code, opt => opt.MapFrom(item => item.CodeUQ))
                 .ForMember(p => p.User, x => x.MapFrom(z => z.UserId > 0 ? new User { ID = z.UserId } : null))
                .ForMember(p => p.Account, x => x.MapFrom(z => z.AccountId > 0 ? new Account { ID = z.AccountId } : null))
                 .ForMember(p => p.Advertiser, x => x.MapFrom(z => z.AdvertiserId > 0 ? new AdvertiserAccount { ID = z.AdvertiserId } : null))


                              .ForMember(dest => dest.IsPermissionNeed, opt => opt.MapFrom(item => item.IsPermissionNeed))

               .ForMember(dest => dest.Parent, opt => opt.MapFrom(item => audianSegRep.Get(item.ParentId)));







            cfg.CreateMap<ContextualSegment, AudienceSegmentDto>()
              .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                  .ForMember(dst => dst.ParentId, opt => opt.MapFrom(z => z.Parent != null ? z.Parent.ID : 0))
                              .ForMember(dst => dst.ProviderId, opt => opt.MapFrom(z => z.Provider != null ? z.Provider.ID : 0))
                               .ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                                       .ForMember(dst => dst.UserId, opt => opt.MapFrom(z => z.User != null ? z.User.ID : 0))
                                .ForMember(dst => dst.AdvertiserId, opt => opt.MapFrom(z => z.Advertiser != null ? z.Advertiser.ID : 0))
                                       .ForMember(dst => dst.ProviderName, opt => opt.MapFrom(z => z.Provider != null ? z.Provider.Name : string.Empty))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                  .ForMember(dest => dest.CodeUQ, opt => opt.MapFrom(item => item.Code))
                          .ForMember(dest => dest.IsSelectedable, opt => opt.MapFrom(item => item.Selectable))
                                      .ForMember(dest => dest.IsPermissionNeed, opt => opt.MapFrom(item => item.IsPermissionNeed))

                                        .ForMember(dest => dest.Positive, opt => opt.MapFrom(item => item.TargetingIntent == "positive"))
                                      ;



            cfg.CreateMap<AudienceSegmentDto, ContextualSegment>()
               .ForMember(dest => dest.Provider, opt => opt.MapFrom(item => DPPartnerRepository.Get(item.ProviderId)))
               .ForMember(dest => dest.Selectable, opt => opt.MapFrom(item => item.IsSelectedable))
               .ForMember(dest => dest.Code, opt => opt.MapFrom(item => item.CodeUQ))
                 .ForMember(p => p.User, x => x.MapFrom(z => z.UserId > 0 ? new User { ID = z.UserId } : null))
                .ForMember(p => p.Account, x => x.MapFrom(z => z.AccountId > 0 ? new Account { ID = z.AccountId } : null))
                 .ForMember(p => p.Advertiser, x => x.MapFrom(z => z.AdvertiserId > 0 ? new AdvertiserAccount { ID = z.AdvertiserId } : null))


                              .ForMember(dest => dest.IsPermissionNeed, opt => opt.MapFrom(item => item.IsPermissionNeed))

               .ForMember(dest => dest.Parent, opt => opt.MapFrom(item => audianSegRep.Get(item.ParentId)))
                   // .ForMember(dest => dest.Positive, opt => opt.MapFrom(item => item.TargetingIntent == "positive"))

               ;


        }
        private static void RegisterOperatorMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Operator, OperatorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name));

            //cfg.CreateMap<Operator, CountryOperatorDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.Location.ID))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Location.Name))
            //    .ForMember(dest => dest.Operators, opt => opt.MapFrom(GeOperatorsDto));
        }
        private static void RegisteLocalizedStringMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LocalizedValue, LocalizedValueDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(item => item.Culture))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(item => item.Value));

            cfg.CreateMap<LocalizedString, LocalizedStringDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(item => item.Values))
                .ForMember(dest => dest.Value, opt => opt.Ignore());


            cfg.CreateMap<LocalizedValueDto, LocalizedValue>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(item => item.Culture))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(item => item.Value));

            cfg.CreateMap<LocalizedStringDto, LocalizedString>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.GroupKey, opt => opt.MapFrom(item => item.GroupKey))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(item => item.Values))
                .ForMember(dest => dest.Value, opt => opt.Ignore());



            //cfg.CreateMap<LocalizedString, LocalizedStringDto>()
            //    .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
            //    .ForMember(dest => dest.Values, opt => opt.MapFrom(item => item.Values))
            //    .ForMember(dest => dest.Value, opt => opt.Ignore());
        }

        private static string GetAdBaseStatus(AdBase<Campaign, AdCampaignStatus> adBase)
        {
            return adBase.Status.Name.ToString();
        }
        private static string GetAdBaseStatus(AdBase<AdGroup, AdGroupStatus> adBase)
        {
            return adBase.Status.Name.ToString();
        }
        private static string GetAdBaseStatus(AdBase<AdCreative, AdCreativeStatus> adBase)
        {
            return adBase.Status.Name.ToString();
        }
        private static string GetAppSiteType(AppSite appSite)
        {
            return appSite.Type != null ? appSite.Type.Name.ToString() : string.Empty;
        }

        private static string GetAppSiteStatus(AppSite appSite)
        {
            return appSite.Status != null ? appSite.Status.Name.ToString() : string.Empty;
        }
        private static string GetAppAdHouse(AppSite appSite)
        {
            //TODO:Osaleh to return real data
            return "Off";
        }
        public static AppSiteType MapAppSiteType(AppSiteDto appSite)
        {
            var appSiteType = new AppSiteType { ID = appSite.Type.Id, IsApp = appSite.Type.IsApp };
            return appSiteType;
        }
        public static TextAdTheme MapAppSiteTheme(AppSiteDto appSite)
        {
            var textAdThemeRepository = Framework.IoC.Instance.Resolve<ITextAdThemeRepository>();
            if (!appSite.Theme.IsCustom)
            {
                return textAdThemeRepository.Get(appSite.Theme.Id);
            }
            else
            {
                var textAdTheme = textAdThemeRepository.Get(appSite.Theme.Id) ?? new TextAdTheme();//new TextAdTheme();
                textAdTheme.BackgroundColor = appSite.Theme.BackgroundColor;
                textAdTheme.TextColor = appSite.Theme.TextColor;
                textAdTheme.IsCustom = appSite.Theme.IsCustom;
                return textAdTheme;
            }
        }
        private static AppSiteTypeDto GetAppSiteTypeDto(AppSite appSite, ResolutionContext context)
        {
            if (appSite.Type == null)
                return null;
            return context.Mapper.Map<AppSiteTypeDto>(appSite.Type);
        }
        private static ThemeDto GetAppSiteThemeDto(AppSite appSite, ResolutionContext context)
        {
            if (appSite.Theme == null)
                return null;
            return context.Mapper.Map<ThemeDto>(appSite.Theme);
        }
        private static string GetAppSiteURLDto(AppSite appSite)
        {
            if (appSite is ArabyAds.AdFalcon.Domain.Model.AppSite.App)
                return ((ArabyAds.AdFalcon.Domain.Model.AppSite.App)appSite).MarketURL;
            else
            {
                return ((Site)appSite).SiteURL;
            }
        }
        private static IEnumerable<KeywordDto> GetAppSiteKeywordsDto(AppSite appSite, ResolutionContext context)
        {
            if (appSite.Keywords == null)
                return null;
            return appSite.Keywords.Select(appsiteKeyword => context.Mapper.Map<KeywordDto>(appsiteKeyword.Keyword)).ToList();
        }


        private static void RegisterAppSiteMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AppSiteStatus, AppSiteStatusDto>();

            cfg.CreateMap<AppSite, AppSiteBasicDto>();
            cfg.CreateMap<AppSiteType, AppSiteTypeDto>();
            cfg.CreateMap<AppSite, AppSiteListDtoBase>()
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetAppSiteType(src)))
                  .ForMember(dest => dest.AccountId, opt => opt.MapFrom(z => z.Account.ID))
                  .ForMember(dest => dest.AccountName, opt => opt.MapFrom(z => string.Format("{0} {1}", z.Account.PrimaryUser.FirstName, z.Account.PrimaryUser.LastName)));

            cfg.CreateMap<AppSite, AppSiteListDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(x => x.Account.GetAccountName()))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetAppSiteType(src)))
                  .ForMember(dest => dest.TypeId, opt => opt.MapFrom(x => x.Type.ID))
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetAppSiteStatus(src)))
                  .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status != null ? src.Status.ID : 0))
                  .ForMember(dest => dest.AdHouse, opt => opt.MapFrom(src => GetAppAdHouse(src)));

            cfg.CreateMap<AppSite, AppSiteDto>()
                  .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(p => p.IsPublished))
                   .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                  .ForMember(dest => dest.AccountLanguage, opt =>  opt.MapFrom(item => item.Account.PrimaryUser.Language.Name))
                  .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom((src, dest, propery, context) =>
                  {
                      AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                      accountInfo.AccountEmail = src.Account.PrimaryUser.EmailAddress;
                      accountInfo.AccountName = src.Account.PrimaryUser.GetName();
                      accountInfo.AccountCompanyName = src.Account.PrimaryUser.Company;
                      accountInfo.Country = MapCountryToCountryDto(src, context);
                      return accountInfo;
                  }))
                  .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(p => p.User != null ? p.User.GetName() : string.Empty))
                  .ForMember(dest => dest.URL, opt => opt.MapFrom(src => GetAppSiteURLDto(src)))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom((src, dest, propery, context) => GetAppSiteKeywordsDto(src, context))) ;

            cfg.CreateMap<ArabyAds.AdFalcon.Domain.Model.AppSite.App, AppSiteDto>()
                   .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                  .ForMember(dest => dest.AccountLanguage, opt => opt.MapFrom(item => item.Account.PrimaryUser.Language.Name))
                  .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom((src, dest, propery, context) =>
                  {
                      AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                      accountInfo.AccountEmail = src.Account.PrimaryUser.EmailAddress;
                      accountInfo.AccountName = src.Account.PrimaryUser.GetName();
                      accountInfo.AccountCompanyName = src.Account.PrimaryUser.Company;
                      accountInfo.Country = MapCountryToCountryDto(src, context);
                      return accountInfo;
                  }))
                  .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(p => p.User != null ? p.User.GetName() : string.Empty))
                  .ForMember(dest => dest.URL, opt => opt.MapFrom(src => GetAppSiteURLDto(src)))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom((src, dest, propery, context) => GetAppSiteKeywordsDto(src, context)));
            cfg.CreateMap<SubAppsite, SubAppsiteDto>();
            

            //cfg.CreateMap<AppSiteDto, AppSite>()
            //    .ForMember(dest => dest.Status, opt => opt.Ignore())
            //    .ForMember(dest => dest.Type, opt => opt.Ignore())
            //    .ForMember(dest => dest.Theme, opt => opt.MapFrom(MapAppSiteTheme))
            //    .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp));


            cfg.CreateMap<AppSiteDto, ArabyAds.AdFalcon.Domain.Model.AppSite.App>()
                  .ForMember(dest => dest.Status, opt => opt.Ignore())
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapAppSiteType(src)))
                  .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => MapAppSiteTheme(src)))
                  .ForMember(dest => dest.MarketURL, opt => opt.MapFrom(item => item.URL));

            cfg.CreateMap<AppSiteDto, Site>()
                  .ForMember(dest => dest.Status, opt => opt.Ignore())
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapAppSiteType(src)))
                  .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => MapAppSiteTheme(src)))
                  .ForMember(dest => dest.SiteURL, opt => opt.MapFrom(item => item.URL));


            cfg.CreateMap<ArabyAds.AdFalcon.Domain.Model.AppSite.App, AppSiteDtoBase>()
                    .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom((src, dest, propery, context) =>
                    {
                        AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                        accountInfo.AccountEmail = src.Account.PrimaryUser.EmailAddress;
                        accountInfo.AccountName = src.Account.PrimaryUser.GetName();
                        accountInfo.AccountCompanyName = src.Account.PrimaryUser.Company;
                        accountInfo.Country = MapCountryToCountryDto(src, context);
                        return accountInfo;
                    }))
                  .ForMember(dest => dest.AdminComment, opt => opt.MapFrom(item => item.LastAdminComment))
                  .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                  .ForMember(dest => dest.URL, opt => opt.MapFrom(item => item.MarketURL))
                  .ForMember(dest => dest.AccountLanguage, opt => opt.MapFrom(item => item.Account.PrimaryUser.Language.Name))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom((src, dest, propery, context) => GetAppSiteKeywordsDto(src, context)));

            cfg.CreateMap<Site, AppSiteDtoBase>()
                 .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom((src, dest, propery, context) =>
                 {
                     AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                     accountInfo.AccountEmail = src.Account.PrimaryUser.EmailAddress;
                     accountInfo.AccountName = src.Account.PrimaryUser.GetName();
                     accountInfo.AccountCompanyName = src.Account.PrimaryUser.Company;
                     accountInfo.Country = MapCountryToCountryDto(src, context);
                     return accountInfo;
                 }))
                //.ForMember(dest => dest.Keywords, opt => opt.MapFrom(GetAppSiteKeywordsDto))
                .ForMember(dest => dest.AdminComment, opt => opt.MapFrom(item => item.LastAdminComment))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(item => item.SiteURL))
                .ForMember(dest => dest.AccountLanguage, opt => opt.MapFrom(item => item.Account.PrimaryUser.Language.Name))
                .ForMember(dest => dest.Keywords, opt => opt.MapFrom((src, dest, propery, context) => GetAppSiteKeywordsDto(src, context)));

            cfg.CreateMap<Site, BasicAppSiteInformation>()
                .ForMember(p => p.AppsiteUrl, opt => opt.MapFrom(x => x.GetURL()))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.PrimaryUser.GetAccountName()));

            cfg.CreateMap<ArabyAds.AdFalcon.Domain.Model.AppSite.App, BasicAppSiteInformation>()
                .ForMember(p => p.AppsiteUrl, opt => opt.MapFrom(x => x.GetURL()))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.PrimaryUser.GetAccountName()));

          
            
        }

        private static CountryDto MapCountryToCountryDto(AppSite appsite, ResolutionContext context)
        {
            Country country = appsite.Account.PrimaryUser.Country;
            return new CountryDto()
            {
                ID = country.ID,
                Name = context.Mapper.Map<LocalizedStringDto>(country.Name)
            };
        }

        private static void RegisterReturnBidToReturnBidDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ReturnBid, ReturnBidDto>();
            cfg.CreateMap<BidDto, BidParameter>();
        }

        private static void RegisterAdActionTypeTrackingEventToAdGroupTrackingEventDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdActionTypeTrackingEvent, AdGroupTrackingEventDto>()
                .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
                    .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
                 .ForMember(p => p.Name, x => x.MapFrom(z => z.Event.Name))
                .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
                .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
                 .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                   .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
                .ForMember(p => p.PreRequisites, x => x.MapFrom((src, dest) =>
                {
                    return string.Join(",", src.GetAllPrerequisitesCodes());
                }))
                .ForMember(p => p.IsCustom, x => x.MapFrom(src => false))
                .ForMember(p => p.Id, x => x.Ignore());
        }

        private static void RegisterAdActionTypeTrackingEventToAdGroupTrackingEvent(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdActionTypeTrackingEvent, AdGroupTrackingEvent>()
                .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
                .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
                .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
                .ForMember(p => p.StatisticsColumnName, x => x.MapFrom(z => z.StatisticsColumnName.ToString()))
               .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
                .ForMember(p => p.IsCustom, x => x.MapFrom(src => false))
                  .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                     .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
                .ForMember(p => p.PreRequisites, x => x.MapFrom<string>(src => null))
                 .ForMember(p => p.ID, x => x.Ignore());


            cfg.CreateMap<AdActionTypeTrackingEvent, AdGroupConversionEvent>()
               .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
               .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
               .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
               .ForMember(p => p.StatisticsColumnName, x => x.MapFrom(z => z.StatisticsColumnName.ToString()))
              .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
               .ForMember(p => p.IsCustom, x => x.MapFrom<bool>(src => false))
                 .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                    .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
               .ForMember(p => p.PreRequisites, x => x.MapFrom<string>(src => null))
                .ForMember(p => p.ID, x => x.Ignore());
        }


        private static void RegisterAdGroupTrackingEventToAdGroupTrackingEventDto(MapperConfigurationExpression cfg)
        {

            cfg.CreateMap<AdGroupTrackingEvent, AdGroupTrackingEventDto>()
                   .ForMember(p => p.Name, x => x.MapFrom((src, dest) =>

                   {
                       var itemevent = trackingEventRepository.GetAll().Where(y => y.Code == src.Code).FirstOrDefault();




                       if (itemevent != null)
                           return itemevent.GetDescription();
                       else
                           return null;
                   }
                ))
                        .ForMember(p => p.SegmentsMapId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                   .ForMember(p => p.SegmentsId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.ID.ToString()).ToArray())) : string.Empty))
            .ForMember(p => p.SegmentString, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.Name.Value.ToString()).ToArray())) : string.Empty))

                .ForMember(p => p.PreRequisites, x => x.Ignore());

            cfg.CreateMap<AdGroupTrackingEventDto, AdGroupTrackingEvent>()
                .ForMember(p => p.ID, x => x.MapFrom(src => src.Id));

            cfg.CreateMap<AccountTrackingEvents, AdGroupTrackingEventDto>()
            .ForMember(p => p.Id, x => x.MapFrom(src => src.ID));

            cfg.CreateMap<AdGroupTrackingEventDto, AccountTrackingEvents>();

            cfg.CreateMap<AdGroupTrackingEvent, AccountTrackingEvents>();


            // .ForMember(p => p.ID, x => x.MapFrom(src => src.Id));
            cfg.CreateMap<AdGroupConversionEvent, AdGroupConversionEventDto>()
                 .ForMember(p => p.Name, x => x.MapFrom(z => trackingEventRepository.GetAll().Where(y => y.Code == z.Code).FirstOrDefault().GetDescription()))
                      .ForMember(p => p.PixelsMapId, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                 .ForMember(p => p.PixelsId, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.Pixel.ID.ToString()).ToArray())) : string.Empty))
          .ForMember(p => p.PixelString, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.Pixel.Name.ToString()).ToArray())) : string.Empty))

              .ForMember(p => p.PreRequisites, x => x.Ignore());
            cfg.CreateMap<AdGroupConversionEventDto, AdGroupConversionEvent>()
                .ForMember(p => p.ID, x => x.MapFrom(src => src.Id));
        }

        private static void RegisterAdGroupTrackingEventSaveDtoToAdGroupTrackingEvent(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdGroupTrackingEventSaveDto, AdGroupTrackingEvent>()
                .ForMember(p => p.PreRequisites, x => x.MapFrom<string>(src => null));

            cfg.CreateMap<AdGroupConversionEventSaveDto, AdGroupConversionEvent>()
              .ForMember(p => p.PreRequisites, x => x.MapFrom<string>(src => null));
        }
        private static void RegisterCampaignAssignedAppsiteToCampaignAssignedAppsiteDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CampaignAssignedAppsite, CampaignAssignedAppsitesDto>();
                
        }

        private static void RegisterAdActionValueTrackerToAdActionValueTrackerDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AdActionValueTracker, AdActionValueTrackerDto>();
        }
        private static void RegisterVideoCardDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<VideoEndCardTracker, VideoEndCardTrackerDto>()
                   .ForMember(p => p.CardId, x => x.MapFrom(z => z.Card.ID));

            cfg.CreateMap<VideoEndCardTrackerDto, VideoEndCardTracker>().ForMember(p => p.Card, x => x.MapFrom(z => z.CardId > 0 ? new VideoEndCard { ID = z.CardId } : null));



            cfg.CreateMap<VideoEndCard, VideoEndCardDto>()
          .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document.ID))
               .ForMember(p => p.CreativeUnitId, x => x.MapFrom(z => z.CreativeUnit.ID))
                    .ForMember(p => p.AdCreativeId, x => x.MapFrom(z => z.VideoAd.ID))



          ;


            cfg.CreateMap<VideoEndCardDto, VideoEndCard>().
                ForMember(p => p.Document, x => x.MapFrom(z => z.DocumentId > 0 ? new Document { ID = z.DocumentId } : null))
                      .ForMember(p => p.CreativeUnit, x => x.MapFrom(z => z.CreativeUnitId > 0 ? new CreativeUnit { ID = z.CreativeUnitId } : null))
                           .ForMember(p => p.VideoAd, x => x.MapFrom(z => z.AdCreativeId > 0 ? new InStreamVideoCreative { ID = z.AdCreativeId } : null))


                ;

            /* [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int DocumentId { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int CreativeUnitId { get; set; }

        [DataMember]
        public int AdCreativeId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }*/


            /* [DataMember]
   public int DocumentId { get; set; }

   [DataMember]
   public int VideoTypeId { get; set; }
   [DataMember]
   public int DeliveryMethodId { get; set; }


   [DataMember]
   public int VideoAdId { get; set; }
   [DataMember]
   public string URL { get; set; }
   [DataMember]
   public int CreativeUnitId { get; set; }

   [DataMember]
   public int AdCreativeId { get; set; }*/

            cfg.CreateMap<VideoMediaFile, VideoMediaFileDto>()
                     .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document.ID))
                     .ForMember(p => p.CreativeUnitId, x => x.MapFrom(z => z.OriginalCreativeUnit.ID))
                     .ForMember(p => p.AdCreativeId, x => x.MapFrom(z => z.VideoAd == null ? 0 : z.VideoAd.ID))
                     .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document == null ? 0 : z.Document.ID))
                     .ForMember(p => p.VideoTypeId, x => x.MapFrom(z => z.VideoType == null ? 0 : z.VideoType.ID))
                     .ForMember(p => p.DeliveryMethodId, x => x.MapFrom(z => z.DeliveryMethod == null ? 0 : z.DeliveryMethod.ID))
                     .ForMember(p => p.AdCreativeUnitId, x => x.MapFrom(z => z.AdCreativeUnit == null ? 0 : z.AdCreativeUnit.ID))
           ;


            cfg.CreateMap<VideoMediaFileDto, VideoMediaFile>().
                ForMember(p => p.Document, x => x.MapFrom(z => z.DocumentId > 0 ? new Document { ID = z.DocumentId } : null))
                      .ForMember(p => p.VideoType, x => x.MapFrom(z => z.VideoTypeId > 0 ? new MIMEType { ID = z.VideoTypeId } : null))
                           .ForMember(p => p.DeliveryMethod, x => x.MapFrom(z => z.DeliveryMethodId > 0 ? new VideoDeliveryMethod { ID = z.DeliveryMethodId } : null))
                                            .ForMember(p => p.OriginalCreativeUnit, x => x.MapFrom(z => z.CreativeUnitId > 0 ? new CreativeUnit { ID = z.CreativeUnitId } : null))
                                              .ForMember(p => p.VideoAd, x => x.MapFrom(z => z.VideoAdId > 0 ? new InStreamVideoCreative { ID = z.VideoAdId } : null))
                                                 .ForMember(p => p.AdCreativeUnit, x => x.MapFrom(z => z.AdCreativeUnitId > 0 ? new AdCreativeUnit { ID = z.AdCreativeUnitId } : null));

            cfg.CreateMap<VideoDeliveryMethod, VideoDeliveryMethodDto>();
            //cfg.CreateMap<AdActionValue, AdActionValueDto>()
            // .ForMember(p => p.Trackers, z => z.MapFrom(x =>
            // {
            //     if (x.Trackers != null)
            //     {
            //         return x.Trackers.Where(w => !w.IsDeleted).Select(q => MapperHelper.Map<AdActionValueTrackerDto>(q));
            //     }
            //     return null;
            // }));
            ////cfg.CreateMap<AdActionValue, AdActionValueRichMediaDto>();


            //cfg.CreateMap<AdActionValueDto, AdActionValue>();

        }
        private static void RegisterTrackingEventToTrackingEventDto(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TrackingEvent, TrackingEventDto>()
                  .ForMember(p => p.EventDescription, x => x.MapFrom(z => z.Name.Value))
                   .ForMember(p => p.DefaultFrequencyCapping, x => x.MapFrom(z => z.DefaultFrequencyCapping));
        }

        private static void RegisterCampaignFrequencyCappingSaveDtoToCampaignFrequencyCapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CampaignFrequencyCappingSaveDto, CampaignFrequencyCapping>();
            cfg.CreateMap<CampaignFrequencyCappingDto, CampaignFrequencyCapping>();
        }

        #region AppSiteSettingsMapping

        private static void RegisterAppSiteSettingsMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AppSiteServerSettingDto, AppSiteServerSetting>().ForMember(p => p.NativeAdLayout, x => x.MapFrom((src, dest) =>
            {
                if (src.NativeLayoutId > 0)
                {

                    NativeAdLayout acc = new NativeAdLayout() { ID = src.NativeLayoutId };
                    return acc;
                }
                return null;
            }));


            cfg.CreateMap<AppSiteRevenueCalculationSettingDto, AppSiteRevenueCalculationSetting>()
                .ForMember(p => p.Value, x => x.MapFrom((src, dest) =>
                {
                    if (src.CalculationMode == CalculationMode.Percentage)
                    {
                        return src.Value / 100;
                    }
                    return src.Value;
                }));


            cfg.CreateMap<AppSiteServerSetting, AppSiteServerSettingDto>()
                 .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.GetPricingModel()))
                 .ForMember(dst => dst.NativeLayoutId, opt => opt.MapFrom(z => z.NativeAdLayout != null ? z.NativeAdLayout.ID : 0));



            cfg.CreateMap<AppSiteRevenueCalculationSetting, AppSiteRevenueCalculationSettingDto>()
                 .ForMember(p => p.Value, x => x.MapFrom((src, dest) =>
                 {
                     if (src.CalculationMode == CalculationMode.Percentage)
                     {
                         return src.Value * 100;
                     }
                     return src.Value;
                 }));


            cfg.CreateMap<AppSiteEvent, AppSiteEventDto>()
                .ForMember(p => p.EventId, x => x.MapFrom(z => z.Event.ID))
                .ForMember(p => p.EventName, x => x.MapFrom(z => z.Event.EventName))
                .ForMember(p => p.MinBid, x => x.MapFrom((src, dest) =>
                {
                    decimal? mindBid = new decimal?();
                    if (src.MinBid.HasValue)
                    {
                        mindBid = src.MinBid.Value * src.Event.CostModelWrapper.Factor;
                    }

                    return mindBid;
                }));

            cfg.CreateMap<AppSiteEventDto, AppSiteEvent>();
            cfg.CreateMap<AppSiteSetting, SettingsDto>().ForMember(p => p.AppSiteName, opt => opt.MapFrom((src,dest,obj) =>  src.AppSite != null? src.AppSite.Name: null  ));
            cfg.CreateMap<SettingsDto, AppSiteSetting>();
        }

        //private static Country MapUserDtoCountry(UserDto userDto)
        //{
        //    Country countryInfo = new Country();
        //    countryInfo.ID = userDto.Country;
        //    return countryInfo;
        //}
        #endregion
        #region UserMapping

        private static void RegisterUserMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserDto, User>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(src => MapUserDtoCountry(src)))
                  .ForMember(p => p.Language, opt => opt.MapFrom(src => MapUserDtoLanguage(src)))
             .ForMember(p => p.RegistredIP, opt => opt.MapFrom(x => !string.IsNullOrEmpty(x.IPAddress) ? Encoding.ASCII.GetBytes(x.IPAddress) : null));


            cfg.CreateMap<AccountDSPRequest, UserDto>()
                                  .ForMember(p => p.Country, opt => opt.MapFrom(src => MapUserCountry(src)));


            cfg.CreateMap<User, UserDto>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(src => MapUserCountry(src)))
                  .ForMember(p => p.Language, opt => opt.MapFrom(src => MapUserLanguage(src)))
                  //.ForMember(p => p.UserAgreementVersion, opt => opt.MapFrom(x => x.Account.UserAgreementVersion))
                  //.ForMember(p => p.AllowAPIAccess, opt => opt.MapFrom(x => x.Account.AllowAPIAccess))
                  //.ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.GetName()))
                  .ForMember(p => p.IPAddress, opt => opt.MapFrom(x => x.RegistredIP != null && x.RegistredIP.Count() > 0 ? Encoding.ASCII.GetString(x.RegistredIP) : ""));
            cfg.CreateMap<ArabyAds.AdFalcon.Domain.Model.Account.Account, UserDto>()
                .ForMember(p => p.Country, opt => opt.MapFrom(src => MapAccountUserCountry(src)))
                .ForMember(p => p.Language, opt => opt.MapFrom(src => MapUserAccountLanguage(src)))
                .ForMember(p => p.AccountRole, opt => opt.MapFrom(x => (int)x.AccountRole))
                .ForMember(p => p.AccountId, opt => opt.MapFrom(x => x.ID))
                .ForMember(p => p.IsAccountDSP, opt => opt.MapFrom(x => x.AccountRole == AccountRole.DSP))
                .ForMember(p => p.Id, opt => opt.MapFrom(x => x.PrimaryUser.ID))
                          .ForMember(p => p.AccountId, opt => opt.MapFrom(x => x.ID))
                .ForMember(p => p.Company, opt => opt.MapFrom(x => x.PrimaryUser.Company))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(x => x.PrimaryUser.EmailAddress))
                .ForMember(p => p.FirstName, opt => opt.MapFrom(x => x.PrimaryUser.FirstName))
                .ForMember(p => p.LastName, opt => opt.MapFrom(x => x.PrimaryUser.LastName))
                    .ForMember(p => p.VATValue, opt => opt.MapFrom(x => x.GetVATValue()))
                .ForMember(p => p.UserAgreementVersion, opt => opt.MapFrom(x => x.UserAgreementVersion))
                .ForMember(p => p.AllowAPIAccess, opt => opt.MapFrom(x => x.AllowAPIAccess))
                .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.GetName()))
                .ForMember(p => p.IPAddress, opt => opt.MapFrom(x => x.PrimaryUser.RegistredIP != null && x.PrimaryUser.RegistredIP.Count() > 0 ? Encoding.ASCII.GetString(x.PrimaryUser.RegistredIP) : ""));


            cfg.CreateMap<AccountDSPRequestDto, AccountDSPRequest>()
           .ForMember(p => p.Country, opt => opt.MapFrom(src => MapUserDtoCountry(src)))
                 .ForMember(p => p.CompanyType, opt => opt.MapFrom(src => MapUserDtoCompanyType(src)));

            cfg.CreateMap<AccountDSPRequest, AccountDSPRequestDto>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(src => MapUserCountry(src)))
                     .ForMember(p => p.CompanyType, opt => opt.MapFrom(src => MapUserCompanyType(src)))

                  .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account != null ? x.Account.GetName() : string.Empty));

            cfg.CreateMap<AccountInvitation, InvitationDto>()
          .ForMember(p => p.accountid, opt => opt.MapFrom(x => x.Account.ID));

        }

        private static Country MapUserDtoCountry(UserDto userDto)
        {

            return countryRepository.Get(userDto.Country);
        }
        private static Country MapUserDtoCountry(AccountDSPRequestDto userDto)
        {

            return countryRepository.Get(userDto.Country);
        }
        private static CompanyType MapUserDtoCompanyType(AccountDSPRequestDto userDto)
        {

            return companyTypeRepository.Get(userDto.CompanyType);
        }
        private static Language MapUserDtoLanguage(UserDto userDto)
        {
            return languageRepository.Get(userDto.Language);
        }
        public static int MapDocument(Document doc)
        {
            if (doc != null)
                return doc.ID;
            else
            {
                return 0;
            }
        }

        public static int MapAccountUserCountry(Account userInfo)
        {
            if (userInfo.PrimaryUser.Country != null)
                return userInfo.PrimaryUser.Country.ID;
            else
            {
                return 0;
            }
        }
        public static int MapUserCountry(User userInfo)
        {
            if (userInfo.Country != null)
                return userInfo.Country.ID;
            else
            {
                return 0;
            }
        }
        public static int MapUserCountry(AccountDSPRequest userInfo)
        {
            if (userInfo.Country != null)
                return userInfo.Country.ID;
            else
            {
                return 0;
            }
        }

        public static int MapUserCompanyType(AccountDSPRequest userInfo)
        {
            if (userInfo.CompanyType != null)
                return userInfo.CompanyType.ID;
            else
            {
                return 0;
            }
        }
        public static int MapUserLanguage(User userInfo)
        {
            if (userInfo.Language != null)
            {
                return userInfo.Language.ID;
            }
            else
            {
                return 0;
            }
        }
        public static int MapUserAccountLanguage(Account userInfo)
        {
            if (userInfo.PrimaryUser.Language != null)
            {
                return userInfo.PrimaryUser.Language.ID;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region FiltersMapping

        private static void RegisterTextFilterToTextFilterDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.MatchTypeId,
                                                                    opt => opt.MapFrom(p => p.MatchType.ID));

            cfg.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.MatchTypeText,
                                                                    opt => opt.MapFrom(p => p.MatchType.Name));

            cfg.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.TextFilterId,
                                                                 opt => opt.MapFrom(p => p.ID));
        }

        private static void RegisterTextFilterDtoToTextFilterMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TextFilterDto, TextFilter>().ForMember(p => p.MatchType,
                                                        opt => opt.MapFrom(src => MapMatchType(src)));

        }

        private static void RegisterUrlFilterToUrlFilterDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UrlFilter, UrlFilterDto>().ForMember(p => p.UrlFilterId, p => p.MapFrom(x => x.ID));
        }

        private static void RegisterUrlFilterDtoToUrlFilterMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UrlFilterDto, UrlFilter>().ForMember(p => p.ID, p => p.MapFrom(x => x.UrlFilterId));
        }

        public static MatchType MapMatchType(TextFilterDto textFilterDto)
        {
            MatchType matchType = new MatchType();
            matchType.ID = textFilterDto.MatchTypeId;

            return matchType;
        }

        private static void RegisterlanguageFilterDtoToLanguageFilterMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LanguageFilterDto, LanguageFilter>()
                .ForMember(p => p.Language, opt => opt.MapFrom(src => MapLanguage(src)))
                .ForMember(p => p.ID, opt => opt.MapFrom(p => p.languageFilterId));
        }

        private static void RegisterlanguageFilterToLanguageFilterDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LanguageFilter, LanguageFilterDto>()
                .ForMember(p => p.languageFilterId, opt => opt.MapFrom(p => p.ID))
                .ForMember(p => p.LanguageId , opt => opt.MapFrom(p => p.Language.ID));
        }

        private static void RegisterLanguageFilterToLanguageFilterDtoMapping(MapperConfigurationExpression cfg)
        {
            throw new NotImplementedException();
        }

        public static Language MapLanguage(LanguageFilterDto languageDto)
        {
            return languageRepository.Get(languageDto.LanguageId);
        }

        #endregion

        private static void RegisterSSPDtoDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<BusinessPartner, PartnerDto>();

            cfg.CreateMap<PartnerDto, BusinessPartner>()
                     .ForMember(p => p.Type, opt => opt.Ignore());

            cfg.CreateMap<PartnerSite, PartnerSiteDto>().ForMember(dst => dst.PartnerID, opt => opt.MapFrom(z => z.Partner != null ? z.Partner.ID : 0));


            cfg.CreateMap<PartnerSiteDto, PartnerSite>().ForMember(p => p.Partner, x => x.MapFrom((src, dest) =>
            {
                if (src.PartnerID > 0)
                {

                    BusinessPartner acc = new BusinessPartner() { ID = src.PartnerID };
                    return acc;
                }
                return null;
            }));
            // AdFalconCampaignId


            cfg.CreateMap<DealCampaignMapping, DealCampaignMappingDto>().ForMember(dst => dst.PartnerID, opt => opt.MapFrom(z => z.Partner != null ? z.Partner.ID : 0))
                .ForMember(dst => dst.AdFalconCampaignId, opt => opt.MapFrom(z => z.Campaign != null ? z.Campaign.ID : 0))
                    .ForMember(dst => dst.CampaignName, opt => opt.MapFrom(z => z.Campaign != null ? z.Campaign.Name : ""));


            cfg.CreateMap<DealCampaignMappingDto, DealCampaignMapping>().ForMember(p => p.Partner, x => x.MapFrom((src, dest) =>
            {
                if (src.PartnerID > 0)
                {

                    BusinessPartner acc = new BusinessPartner() { ID = src.PartnerID };
                    return acc;
                }
                return null;
            }));
            //.ForMember(p => p.Campaign, x => x.MapFrom(z =>
            //{
            //    if (z.AdFalconCampaignId > 0)
            //    {

            //        Campaign acc = new Campaign() { ID = z.AdFalconCampaignId };
            //        return acc;
            //    }
            //    return null;
            //}));


            cfg.CreateMap<SiteZone, SiteZoneDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0));


            cfg.CreateMap<SiteZoneDto, SiteZone>().ForMember(p => p.Site, x => x.MapFrom((src, dest) =>
            {
                if (src.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = src.SiteID };
                    return acc;
                }
                return null;
            }));



            cfg.CreateMap<FloorPrice, FloorPriceConfigDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0))
            .ForMember(dst => dst.ZoneID, opt => opt.MapFrom(z => z.Zone != null ? z.Zone.ID : 0))
                   .ForMember(dst => dst.TargetingId, opt => opt.MapFrom(z => z.TargetingId != null ? z.TargetingId : -1));



            cfg.CreateMap<FloorPriceConfigDto, FloorPrice>().ForMember(p => p.Site, x => x.MapFrom((src, dest) =>
            {
                if (src.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = src.SiteID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.Zone, x => x.MapFrom((src, dest) =>
            {
                if (src.ZoneID > 0)
                {

                    SiteZone acc = new SiteZone() { ID = src.ZoneID };
                    return acc;
                }
                return null;
            }));


            cfg.CreateMap<SiteZoneMapping, SiteZoneMappingDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0))
         .ForMember(dst => dst.ZoneID, opt => opt.MapFrom(z => z.Zone != null ? z.Zone.ID : 0))
         .ForMember(dst => dst.AdTypeID, opt => opt.MapFrom(z => z.AdType != null ? z.AdType.ID : 0))
         .ForMember(dst => dst.DeviceTypeID, opt => opt.MapFrom(z => z.DeviceType != null ? z.DeviceType.ID : 0))
            .ForMember(dst => dst.AppSiteID, opt => opt.MapFrom(z => z.AppSite != null ? z.AppSite.ID : 0))
                  .ForMember(dst => dst.AdTypeString, opt => opt.MapFrom(z => z.AdType != null ? z.AdType.Name.GetValue() : ""))
         .ForMember(dst => dst.DeviceTypeString, opt => opt.MapFrom(z => z.DeviceType != null ? z.DeviceType.Name.GetValue() : ""))

          .ForMember(dst => dst.AppSiteString, opt => opt.MapFrom(z => z.AppSite != null ? z.AppSite.Name : ""))
         ;


            cfg.CreateMap<SiteZoneMappingDto, SiteZoneMapping>().ForMember(p => p.Site, x => x.MapFrom((src, dest) =>
            {
                if (src.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = src.SiteID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.Zone, x => x.MapFrom((src, dest) =>
            {
                if (src.ZoneID > 0)
                {

                    SiteZone acc = new SiteZone() { ID = src.ZoneID };
                    return acc;
                }
                return null;
            }))

            .ForMember(p => p.AdType, x => x.MapFrom((src, dest) =>
            {
                if (src.AdTypeID > 0)
                {

                    AdType acc = new AdType() { ID = src.AdTypeID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.DeviceType, x => x.MapFrom((src, dest) =>
            {
                if (src.DeviceTypeID > 0)
                {

                    DeviceType acc = new DeviceType() { ID = src.DeviceTypeID };
                    return acc;
                }
                return null;
            }))



            ;







        }

        private static void RegisterPMPDealDtoDtoMapping(MapperConfigurationExpression cfg)
        {


            cfg.CreateMap<PMPDeal, PMPDealDto>().ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(z => z.User != null ? z.User.ID : 0))

                    .ForMember(dst => dst.AdvertiserId, opt => opt.MapFrom(z => z.Advertiser != null ? z.Advertiser.ID : 0))
                  .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(item => item.Advertiser == null ? string.Empty : item.Advertiser.Name.ToString()))

                    .ForMember(dst => dst.AdvertiserAccountId, opt => opt.MapFrom(z => z.AdvertiserAccount != null ? z.AdvertiserAccount.ID : 0))
                  .ForMember(dest => dest.AdvertiserAccountName, opt => opt.MapFrom(item => item.AdvertiserAccount == null ? string.Empty : item.AdvertiserAccount.Name.ToString()))


                  .ForMember(dest => dest.ExchangeName, opt => opt.MapFrom(item => item.Exchange == null ? string.Empty : item.Exchange.Name.ToString()))

                  .ForMember(dst => dst.ExchangeId, opt => opt.MapFrom(z => z.Exchange != null ? z.Exchange.ID : 0));


            cfg.CreateMap<PMPDealDto, PMPDeal>().ForMember(p => p.Account, x => x.MapFrom((src, dest) =>
            {
                if (src.AccountId > 0)
                {

                    Account acc = new Account() { ID = src.AccountId };
                    return acc;
                }
                return null;
            }))

            .ForMember(p => p.User, x => x.MapFrom((src, dest) =>
            {
                if (src.UserId > 0)
                {

                    User acc = new User() { ID = src.UserId };
                    return acc;
                }
                return null;
            }))

            //.ForMember(p => p.Publisher, x => x.MapFrom(z =>
            //{
            //    if (z.PublisherId > 0)
            //    {

            //        Account acc = new Account() { ID = z.PublisherId };
            //        return acc;
            //    }
            //    return null;
            //}))

            .ForMember(p => p.Exchange, x => x.MapFrom((src, dest) =>
            {
                if (src.ExchangeId > 0)
                {

                    SSPPartner acc = new SSPPartner() { ID = src.ExchangeId };
                    return acc;
                }
                return null;
            }))
             .ForMember(p => p.Advertiser, x => x.MapFrom((src, dest) =>
             {
                 if (src.AdvertiserId > 0)
                 {

                     Advertiser adv = new Advertiser() { ID = src.AdvertiserId };
                     return adv;
                 }
                 return null;
             }))

             .ForMember(p => p.AdvertiserAccount, x => x.MapFrom((src, dest) =>
             {
                 if (src.AdvertiserAccountId > 0)
                 {

                     AdvertiserAccount adv = new AdvertiserAccount() { ID = src.AdvertiserAccountId };
                     return adv;
                 }
                 return null;
             }))



            ;
            // AdFalconCampaignId






        }
        private static void RegisteImpressionLogDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ImpressionLog, ImpressionLogDto>()
           .ForMember(dst => dst.Day, opt => opt.MapFrom(z => DateTime.ParseExact(z.Day.ToString(), "yyyyMMdd", null)));

            cfg.CreateMap<ImpressionLogDto, ImpressionLog>()
            .ForMember(dst => dst.Day, opt => opt.MapFrom(z => Convert.ToInt32(z.Day.ToString("yyyyMMdd"))));

        }


        private static void RegistermetriceGroupColumnDtoMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<metriceColumn, metriceColumnDto>();
            //.ForMember(dst => dst.Header, opt => opt.MapFrom(z => ResourceManager.Instance.GetResource(z.HeaderResourceKey, z.HeaderResourceSet)));


            cfg.CreateMap<metriceColumnDto, metriceColumn>();


            cfg.CreateMap<metriceGroup, metriceGroupDto>();
            cfg.CreateMap<metriceGroupDto, metriceGroup>();

            cfg.CreateMap<metriceGroupColumn, metriceGroupColumnDto>();
            cfg.CreateMap<metriceGroupColumnDto, metriceGroupColumn>();

        }
    }
}
