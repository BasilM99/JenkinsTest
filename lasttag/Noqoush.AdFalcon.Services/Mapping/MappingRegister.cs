using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AutoMapper;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.AppSite.Filtering;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Model.Campaign.Performance;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.DomainServices.Localization;
using Noqoush.Framework.Resources;
using Noqoush.AdFalcon.Domain.Model.Account.Fund;
using Noqoush.AdFalcon.Domain.Services;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.Framework.Utilities;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Account.DPP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
using Noqoush.AdFalcon.Domain.Model.QueryBuilder;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using Noqoush.AdFalcon.Base;
using Noqoush.AdFalcon.Services.Interfaces.Core;


using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;

using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.DPP;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Services.Mapping
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
        public static void RegisterMapping()
        {
            Noqoush.Framework.IoC.Instance.GetType();
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
            RegisterCoreMapping();
            RegisteLocalizedStringMapping();
            RegisterAppSiteMapping();
            RegisterUserMapping();
            RegisterAppSiteSettingsMapping();
            RegisterTextFilterToTextFilterDtoMapping();
            RegisterTextFilterDtoToTextFilterMapping();
            RegisterlanguageFilterDtoToLanguageFilterMapping();
            RegisterOperatorMapping();
            RegisterDeviceMapping();
            RegisterTargetingMapping();
            RegisterLocationMapping();
            RegisterAdActionTypeMapping();
            RegisterAdActionTypeConstraintMapping();
            RegisterUrlFilterToUrlFilterDtoMapping();
            RegisterUrlFilterDtoToUrlFilterMapping();
            RegisterlanguageFilterToLanguageFilterDtoMapping();
            RegisterBankAccountDtoToBankAccount();
            RegisterCampaignDtoMapping();
            RegisterFundtoFundDtoMapping();
            RegisterAdCreativeSummaryDtoMapping();
            RegisterAdGroupSummaryDtoMapping();
            RegisterCampaignSummaryDtoMapping();
            RegisterPerformanceDtoMapping();
            RegisterFundTransDtoMapping();
            RegisterPaymentDtoMapping();
            RegisterReportDtoDtoMapping();
            RegisterCampaignDtoToCampaignMapping();
            RegisterAccountToAccountAPIAccessDto();
            RegisterReturnBidToReturnBidDto();
            RegisterAdActionTypeTrackingEventToAdGroupTrackingEventDto();
            RegisterAdActionTypeTrackingEventToAdGroupTrackingEvent();
            RegisterAdGroupTrackingEventToAdGroupTrackingEventDto();
            RegisterAdGroupTrackingEventSaveDtoToAdGroupTrackingEvent();
            RegisterAdActionValueTrackerToAdActionValueTrackerDto();
            RegisterTrackingEventToTrackingEventDto();
            RegisterCampaignFrequencyCappingSaveDtoToCampaignFrequencyCapping();
            RegisterCampaignBidConfigDtoCampaignBidConfigMapping();
            RegisterReportSchedulerDtoDtoMapping();
            RegisterAdRequestTargetingDtoMapping();
            RegisterSSPDtoDtoMapping();
            RegisterPMPDealDtoDtoMapping();
            RegisterAudienceSegmentMapping();
            RegisterVideoCardDto();
            RegisteImpressionLogDtoMapping();
            RegistermetriceGroupColumnDtoMapping();
            RegisterAdvertiserAccountDtoMapping();

            RegisterAccountDSPSettingsDtoDtoMapping();
            MapQBProfile();

        }

        private static void MapQBProfile()
        {
            Mapper.CreateMap<ColumnQB, ColumnQBDto>();
            Mapper.CreateMap<ColumnQBDto, ColumnQB>();

            Mapper.CreateMap<Dimension, DimensionDto>();
        
            Mapper.CreateMap<DimensionDto, Dimension>();

                 

            Mapper.CreateMap<Fact, FactDto>()
                   .ForMember(dest => dest.Dimensions,

                 z => z.MapFrom(x =>
                 {
                     if (x.Dimensions != null)
                     {
                         return x.Dimensions.Where(M => M.IsDeleted == false).Select(q => MapperHelper.Map<DimensionDto>(q));
                     }
                     return null;
                 }


                ))

                     .ForMember(dest => dest.Measures,

                 z => z.MapFrom(x =>
                 {
                     if (x.Measures != null)
                     {
                         return x.Measures.Where(M => M.IsDeleted == false).Select(q => MapperHelper.Map<MeasureDto>(q));
                     }
                     return null;
                 }


                ))

                ;
            Mapper.CreateMap<FactDto, Fact>();

            Mapper.CreateMap<Measure, MeasureDto>();
            Mapper.CreateMap<MeasureDto, Measure>();

            Mapper.CreateMap<EntityQB, EntityQBDto>();
            Mapper.CreateMap<EntityQBDto, EntityQB>();
        }

        private static void RegisterAdvertiserAccountDtoMapping()
        {
            Mapper.CreateMap<AdvertiserAccountDto, AdvertiserAccount>();

            Mapper.CreateMap<AdvertiserAccount, AdvertiserAccountDto>()
          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0));

            Mapper.CreateMap<AdvertiserAccountListDto, AdvertiserAccount>();

            Mapper.CreateMap<AdvertiserAccount, AdvertiserAccountListDto>()
          .ForMember(dest => dest.AdvertiserId, opt => opt.MapFrom(item => item.Advertiser != null ? item.Advertiser.ID : 0))
          .ForMember(dest => dest.AdvertiserItem, opt => opt.MapFrom(item => item.Advertiser != null ? MapperHelper.Map<AdvertiserDto>(item.Advertiser) : null));

            Mapper.CreateMap<AdvertiserAccountUserDto, AdvertiserAccountUser>();

            Mapper.CreateMap<AdvertiserAccountUser, AdvertiserAccountUserDto>();


            Mapper.CreateMap<AdvertiserAccountReadOnlyUserDto, AdvertiserAccountUser>();

            Mapper.CreateMap<AdvertiserAccountUser, AdvertiserAccountReadOnlyUserDto>();


            Mapper.CreateMap<AdvertiserAccountMasterAppSiteDto, AdvertiserAccountMasterAppSite>();

            Mapper.CreateMap<AdvertiserAccountMasterAppSite, AdvertiserAccountMasterAppSiteDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))
                    ;



            Mapper.CreateMap<PixelDto, Pixel>();

            Mapper.CreateMap<Pixel, PixelDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0))

                                .ForMember(p => p.SegmentsMapId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                   .ForMember(p => p.SegmentsId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.ID.ToString()).ToArray())) : string.Empty))
            .ForMember(p => p.SegmentString, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.Name.Value.ToString()).ToArray())) : string.Empty))

                    ;




            Mapper.CreateMap<AdvertiserAccountMasterAppSiteItemDto, AdvertiserAccountMasterAppSiteItem>();

            Mapper.CreateMap<AdvertiserAccountMasterAppSiteItem, AdvertiserAccountMasterAppSiteItemDto>()
          .ForMember(dest => dest.LinkId, opt => opt.MapFrom(item => item.Link != null ? item.Link.ID : 0))
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
                          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0));


        }

        private static void RegisterReportDtoDtoMapping()
        {
            Mapper.CreateMap<CampaignCommonReportDto, AdGeoLocationDto>()
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(item => item.Name))
              .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(item => item.SubName));
        }

        private static void RegisterAdRequestTargetingDtoMapping()
        {


            Mapper.CreateMap<AdRequestTypeDto, AdRequestType>();
            Mapper.CreateMap<AdRequestPlatformDto, AdRequestPlatform>();
            Mapper.CreateMap<AdRequestTypePlatformVersion, AdRequestTypePlatformVersionDto>()
            .ForMember(dest => dest.AdRequestPlatform, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestPlatformDto>(item.AdRequestPlatform)))
            .ForMember(dest => dest.AdRequestType, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestTypeDto>(item.AdRequestType)));

            Mapper.CreateMap<AdRequestType, AdRequestTypeDto>();
            Mapper.CreateMap<AdRequestPlatform, AdRequestPlatformDto>();
            Mapper.CreateMap<AdRequestTypePlatformVersionDto, AdRequestTypePlatformVersion>()
            .ForMember(dest => dest.AdRequestPlatform, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestPlatform>(item.AdRequestPlatform)))
            .ForMember(dest => dest.AdRequestType, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestType>(item.AdRequestType)));


            Mapper.CreateMap<AdRequestTargeting, AdRequestTargetingDto>()

             .ForMember(dest => dest.AdRequestPlatform, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestPlatformDto>(item.AdRequestPlatform)))
                     .ForMember(dest => dest.AdRequestType, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestTypeDto>(item.AdRequestType)));

            Mapper.CreateMap<AdRequestTargetingDto, AdRequestTargeting>()

         .ForMember(dest => dest.AdRequestPlatform, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestPlatform>(item.AdRequestPlatform)))
                 .ForMember(dest => dest.AdRequestType, opt => opt.MapFrom(item => MapperHelper.Map<AdRequestType>(item.AdRequestType)));

            Mapper.CreateMap<ImpressionMetricTargeting, ImpressionMetricTargetingDto>()

         .ForMember(dest => dest.ImpressionMetric, opt => opt.MapFrom(item => MapperHelper.Map<ImpressionMetricDto>(item.ImpressionMetric)))
                .ForMember(dest => dest.MetricVendor, opt =>
                opt.MapFrom(item =>
                item.MetricVendor != null ?
                MapperHelper.Map<MetricVendorDto>(item.MetricVendor) :
                new MetricVendorDto
                {
                    ID = 0,
                    Code = "Any",
                    Description = "Any",
                    Name = LocalizedStringDto.ConvertToLocalizedStringDto(ResourceManager.Instance.GetResource("Any"))
                }
                ));


            Mapper.CreateMap<ImpressionMetricTargetingDto, ImpressionMetricTargeting>()
           .ForMember(dest => dest.MetricVendor, opt => opt.MapFrom(item => MapperHelper.Map<MetricVendor>(item.MetricVendor)))

         .ForMember(dest => dest.ImpressionMetric, opt => opt.MapFrom(item => MapperHelper.Map<ImpressionMetric>(item.ImpressionMetric)));

        }
        private static void RegisterPaymentDtoMapping()
        {
            Mapper.CreateMap<PaymentDto, Payment>();
            Mapper.CreateMap<PaymentType, PaymentTypeDto>();
            Mapper.CreateMap<Payment, PaymentDto>();
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

        private static void RegisterFundTransDtoMapping()
        {
            Mapper.CreateMap<AccountFundTransStatusDto, AccountFundTransStatus>();
            Mapper.CreateMap<AccountFundTransTypeDto, AccountFundTransType>();
            Mapper.CreateMap<AccountFundTypeDto, AccountFundType>();
            Mapper.CreateMap<AccountFundTransStatus, AccountFundTransStatusDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));
            Mapper.CreateMap<AccountFundTransType, AccountFundTransTypeDto>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));
            Mapper.CreateMap<AccountFundType, AccountFundTypeDto>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name ?? null));

            Mapper.CreateMap<AccountFundTransHistory, FundTransactionDto>()
              .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTransStatusDto>(item.FundTransStatus)))
              .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTransTypeDto>(item.FundTransType)))
              .ForMember(dest => dest.FundType, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTypeDto>(item.AccountFundType)));

            Mapper.CreateMap<AccountFundTransHistoryPgw, FundTransactionDto>()
                .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTransStatusDto>(item.FundTransStatus)))
                .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTransTypeDto>(item.FundTransType)))
                .ForMember(dest => dest.FundType, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundTypeDto>(item.AccountFundType)));

            Mapper.CreateMap<FundTransactionDto, AccountFundTransHistory>()
            .ForMember(dest => dest.FundTransStatus, opt => opt.MapFrom(GetFundTransStatus))
            .ForMember(dest => dest.FundTransType, opt => opt.MapFrom(GetFundTransType))
            .ForMember(dest => dest.AccountFundType, opt => opt.MapFrom(item => MapperHelper.Map<AccountFundType>(item.FundType)));

        }

        private static void RegisterPerformanceDtoMapping()
        {
            Mapper.CreateMap<CampaignPerformance, CampaignPerformanceDto>();
        }

        private static void RegisterCampaignSummaryDtoMapping()
        {
            Mapper.CreateMap<Campaign, CampaignsSummaryDtoBase>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(GetAdBaseStatus))
                .ForMember(dest => dest.CampaignTypeEnum, opt => opt.MapFrom(item => (int)item.CampaignType))
                .ForMember(dest => dest.CampaignType, opt => opt.MapFrom(
                    item => item.CampaignType == CampaignType.AdHouse ?
                    ResourceManager.Instance.GetResource("AdHouse", "Campaign") :
                    ResourceManager.Instance.GetResource("NormalAd", "Campaign")));
            Mapper.CreateMap<Campaign, CampaignSummaryDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(item => item.Account == null ? string.Empty : item.Account.GetName()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(GetAdBaseStatus));
        }

        private static void RegisterAdGroupSummaryDtoMapping()
        {
            Mapper.CreateMap<AdGroup, AdGroupSummaryDtoBase>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(GetAdBaseStatus))
                .ForMember(dest => dest.Objective, opt => opt.MapFrom(item => item.Objective.Objective == null ? string.Empty : item.Objective.Objective.Name.ToString()))
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(item => item.Objective.AdAction == null ? string.Empty : item.Objective.AdAction.Name.ToString()));

            Mapper.CreateMap<AdGroup, AdGroupSummaryDto>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(GetAdBaseStatus))
                 .ForMember(dest => dest.Objective, opt => opt.MapFrom(item => item.Objective.Objective == null ? string.Empty : item.Objective.Objective.Name.ToString()))
                 .ForMember(dest => dest.ActionType, opt => opt.MapFrom(item => item.Objective.AdAction == null ? string.Empty : item.Objective.AdAction.Name.ToString()));
        }
        private static void RegisterAccountDSPSettingsDtoDtoMapping()
        {
            Mapper.CreateMap<DSPAccountSettingContact, AccountDSPsettingContactDTO>()
          .ForMember(dest => dest.AccountSettingId,
            opt => opt.MapFrom(item => item != null ? item.DSPAccountSetting.ID : 0));




            Mapper.CreateMap<DSPAccountSetting, AccountDSPsettingDTO>().
                ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.CountryId, opt => opt.MapFrom(z => z.Country == null ? 0 : z.Country.ID))
                 .ForMember(dst => dst.StateId, opt => opt.MapFrom(z => z.State == null ? 0 : z.State.ID))

                .ForMember(dest => dest.AllContacts,

                 z => z.MapFrom(x =>
                 {
                     if (x.Contacts != null)
                     {
                         return x.Contacts.Where(M => M.IsDeleted == false).Select(q => MapperHelper.Map<AccountDSPsettingContactDTO>(q));
                     }
                     return null;
                 }


                ))

            //.ForMember(dest => dest.Status, opt => opt.MapFrom(
            //    item => item.IsActive == true ?
            //    ResourceManager.Instance.GetResource("Active", "JobGrid") :
            //    ResourceManager.Instance.GetResource("NotActive", "JobGrid")));

            ;

            Mapper.CreateMap<AccountDSPsettingContactDTO, DSPAccountSettingContact>()
         .ForMember(p => p.DSPAccountSetting, x => x.MapFrom(z =>
         {
             if (z.AccountSettingId > 0)
             {

                 DSPAccountSetting acc = new DSPAccountSetting() { ID = z.AccountSettingId };
                 return acc;
             }
             return null;
         }));

            Mapper.CreateMap<AccountDSPsettingDTO, DSPAccountSetting>()
                   .ForMember(p => p.Account, x => x.MapFrom(z =>
                   {
                       if (z.AccountId > 0)
                       {

                           Account acc = new Account() { ID = z.AccountId };
                           return acc;
                       }
                       return null;
                   }))
                    .ForMember(p => p.Country, x => x.MapFrom(z =>
                    {
                        if (z.CountryId > 0)
                        {
                            Country doc = new Country() { ID = z.CountryId };
                            return doc;
                        }

                        return null;
                    }))
                     .ForMember(p => p.State, x => x.MapFrom(z =>
                     {
                         if (z.StateId > 0)
                         {
                             State doc = new State() { ID = z.StateId };
                             return doc;
                         }

                         return null;
                     }))

                    .ForMember(dest => dest.Contacts,

                 z => z.MapFrom(x =>
                 {
                     if (x.AllContacts != null)
                     {
                         return x.AllContacts.Select(q => MapperHelper.Map<DSPAccountSettingContact>(q));
                     }
                     return null;
                 }


                )); ;



        }
        private static void RegisterReportSchedulerDtoDtoMapping()
        {
            Mapper.CreateMap<ReportRecipient, ReportRecipientDTO>()
          .ForMember(dest => dest.ReportSchedulerID,
            opt => opt.MapFrom(item => item.ReportScheduler != null ? item.ReportScheduler.ID : 0));

            Mapper.CreateMap<ReportCriteria, ReportCriteriaSchedulerDto>()
.ForMember(dest => dest.UserId, opt => opt.MapFrom(item => item.User != null ? item.User.ID : 0))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account != null ? item.Account.ID : 0));

            Mapper.CreateMap<ReportCriteriaSchedulerDto, ReportCriteria>()
.ForMember(dest => dest.User, opt => opt.MapFrom(item => item.UserId > 0 ? new User { ID = item.UserId.Value } : null))
            .ForMember(dest => dest.Account, opt => opt.MapFrom(item => item.AccountId > 0 ? new User { ID = item.AccountId.Value } : null));



            Mapper.CreateMap<ReportScheduler, ReportSchedulerDto>().
                ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.LastDocumnetGeneratedId, opt => opt.MapFrom(z => z.LastDocumnetGenerated == null ? 0 : z.LastDocumnetGenerated.ID))
                                .ForMember(dst => dst.CriteriaSchedulerId, opt => opt.MapFrom(z => z.ReportCriteria == null ? 0 : z.ReportCriteria.ID))
                //  .ForMember(dest => dest.CriteriaScheduler, x => x.MapFrom(z => MapperHelper.Map<ReportCriteriaSchedulerDto>(z.ReportCriteria)))

                .ForMember(dest => dest.AllReportRecipient,

                 z => z.MapFrom(x =>
                  {
                      if (x.AllRecipient != null)
                      {
                          return x.AllRecipient.Where(M => M.IsDeleted == false).Select(q => MapperHelper.Map<ReportRecipientDTO>(q));
                      }
                      return null;
                  }


                )).ForMember(dest => dest.Status, opt => opt.MapFrom(
                    item => item.IsActive == true ?
                    ResourceManager.Instance.GetResource("Active", "JobGrid") :
                    ResourceManager.Instance.GetResource("NotActive", "JobGrid"))); ;

            Mapper.CreateMap<ReportRecipientDTO, ReportRecipient>()
         .ForMember(p => p.ReportScheduler, x => x.MapFrom(z =>
                {
                    if (z.ReportSchedulerID > 0)
                    {

                        ReportScheduler acc = new ReportScheduler() { ID = z.ReportSchedulerID };
                        return acc;
                    }
                    return null;
                }));

            Mapper.CreateMap<ReportSchedulerDto, ReportScheduler>()
                   .ForMember(p => p.Account, x => x.MapFrom(z =>
                {
                    if (z.AccountId > 0)
                    {

                        Account acc = new Account() { ID = z.AccountId };
                        return acc;
                    }
                    return null;
                }))
                    .ForMember(p => p.LastDocumnetGenerated, x => x.MapFrom(z =>
                    {
                        if (z.LastDocumnetGeneratedId > 0)
                        {
                            Document doc = new Document() { ID = z.LastDocumnetGeneratedId };
                            return doc;
                        }

                        return null;
                    })).ForMember(dest => dest.AllRecipient,

                 z => z.MapFrom(x =>
                 {
                     if (x.AllReportRecipient != null)
                     {
                         return x.AllReportRecipient.Select(q => MapperHelper.Map<ReportRecipient>(q));
                     }
                     return null;
                 }


                )); ;



        }
        private static void RegisterAdCreativeSummaryDtoMapping()
        {
            Mapper.CreateMap<AppSiteAdQueue, AppSiteAdQueueDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.AppSite.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.AppSite == null ? string.Empty : item.AppSite.Name))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(z => z.AppSite.Account.ID))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(z => string.Format("{0} {1}", z.AppSite.Account.PrimaryUser.FirstName, z.AppSite.Account.PrimaryUser.LastName)));


            Mapper.CreateMap<AdCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                   .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                   {
                       if (x.ClickTags != null)
                       {
                           return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                       }
                       return null;
                   }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom(x =>
                      {
                          if (x.ThirdPartyTrackers != null)
                          {
                              return x.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }))

                ;

            Mapper.CreateMap<AdCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))

                  .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                  {
                      if (x.ClickTags != null)
                      {
                          return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))

                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom(x =>
                      {
                          if (x.ThirdPartyTrackers != null)
                          {
                              return x.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }))

                ;

            Mapper.CreateMap<AdCreative, AdCreativeFullSummaryDto>()
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))

                .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                {
                    if (x.ClickTags != null)
                    {
                        return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                    }
                    return null;
                }))
                      .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom(x =>
                      {
                          if (x.ThirdPartyTrackers != null)
                          {
                              return x.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                          }
                          return null;
                      }))
              ;







            Mapper.CreateMap<RichMediaCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom(item => item.GetRichMediaProtocol() != null ? MapperHelper.Map<RichMediaRequiredProtocolDto>(item.GetRichMediaProtocol()) : null))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))

                  .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                  {
                      if (x.ClickTags != null)
                      {
                          return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))

                        .ForMember(p => p.ThirdPartyTrackers, z => z.MapFrom(x =>
                        {
                            if (x.ThirdPartyTrackers != null)
                            {
                                return x.ThirdPartyTrackers.Select(q => MapperHelper.Map<ThirdPartyTrackerDto>(q));
                            }
                            return null;
                        }))


                ;

            Mapper.CreateMap<RichMediaCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
                .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom(item => item.GetRichMediaProtocol() != null ? MapperHelper.Map<RichMediaRequiredProtocolDto>(item.GetRichMediaProtocol()) : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))

                  .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                  {
                      if (x.ClickTags != null)
                      {
                          return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))
                ;

            Mapper.CreateMap<RichMediaCreative, AdCreativeFullSummaryDto>()
               .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null ? item.ActionValue.ActionType.ViewName : string.Empty))
              .ForMember(dest => dest.RichMediaRequiredProtocol, opt => opt.MapFrom(item => item.GetRichMediaProtocol() != null ? MapperHelper.Map<RichMediaRequiredProtocolDto>(item.GetRichMediaProtocol()) : null))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                {
                    if (x.ClickTags != null)
                    {
                        return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                    }
                    return null;
                }))

              ;



            Mapper.CreateMap<VideoEndCardCreative, AdCreativeSummaryDtoBase>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))

                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))

                  .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                  {
                      if (x.ClickTags != null)
                      {
                          return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))
                ;

            Mapper.CreateMap<VideoEndCardCreative, AdCreativeSummaryDto>()
                .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
                .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))

                .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                  .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                  {
                      if (x.ClickTags != null)
                      {
                          return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                      }
                      return null;
                  }))

                ;

            Mapper.CreateMap<VideoEndCardCreative, AdCreativeFullSummaryDto>()
               .ForMember(dest => dest.Bid, opt => opt.MapFrom(item => item.GetReadableBid()))
              .ForMember(dest => dest.AdBannerType, opt => opt.MapFrom(item => item.CretiveUnitDeviceType))
              .ForMember(dest => dest.ViewName, opt => opt.MapFrom(item => item.ActionValue != null && item.ActionValue.ActionType != null ? item.ActionValue.ActionType.ViewName : string.Empty))

              .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()))
                .ForMember(p => p.ClickTags, z => z.MapFrom(x =>
                {
                    if (x.ClickTags != null)
                    {
                        return x.ClickTags.Select(q => MapperHelper.Map<ClickTagTrackerDto>(q));
                    }
                    return null;
                }))
              ;

        }

        private static void RegisterFundtoFundDtoMapping()
        {
            //Mapper.CreateMap<Fund, FundDto>().ForMember(p => p.FundType, opt => opt.MapFrom(x => x.Type.Name.ToString()));
        }

        private static void RegisterBankAccountDtoToBankAccount()
        {

            Mapper.CreateMap<AccountPaymentDetails, PaymentDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));



            Mapper.CreateMap<AccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            Mapper.CreateMap<BankAccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            Mapper.CreateMap<PayPalAccountPaymentDetails, PaymentFullDetailDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.GetDescription()));

            Mapper.CreateMap<AccountPaymentDetailDto, BankAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());
            Mapper.CreateMap<AccountPaymentDetailDto, PayPalAccountPaymentDetails>()
                //.ForMember(p => p.UserName, opt => opt.MapFrom(x => !string.IsNullOrWhiteSpace(x.UserName)?x.UserName:)
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            Mapper.CreateMap<BankAccountPaymentDetails, AccountPaymentDetailDto>();
            Mapper.CreateMap<PayPalAccountPaymentDetails, AccountPaymentDetailDto>();

            Mapper.CreateMap<BankAccountPaymentDetails, AccountPaymentDetailFundDto>();
            Mapper.CreateMap<PayPalAccountPaymentDetails, AccountPaymentDetailFundDto>();

            Mapper.CreateMap<AccountPaymentDetailFundDto, BankAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());
            Mapper.CreateMap<AccountPaymentDetailFundDto, PayPalAccountPaymentDetails>().ForMember(p => p.IsDeleted, opt => opt.Ignore());



            // Mapper.CreateMap<SystemBankAccount, SystemBankAccountDto>();
            //Mapper.CreateMap<SystemPayPalAccount, SystemPayPalAccountDto>();
        }

        private static void RegisterAccountToAccountAPIAccessDto()
        {
            Mapper.CreateMap<Account, AccountAPIAccessDto>()
                .ForMember(p => p.AccountId, p => p.MapFrom(z => z.ID))
                .ForMember(p => p.APIClientId, z => z.MapFrom(x =>
                {
                    if (x.APIAccess != null)
                    {
                        return x.APIAccess.APIClientId;
                    }
                    return null;
                }))
                .ForMember(p => p.APISecretKey, z => z.MapFrom(x =>
                {
                    if (x.APIAccess != null)
                    {
                        return x.APIAccess.APISecretKey;
                    }
                    return null;
                }));
        }

        private static void RegisterCoreMapping()
        {
            Mapper.CreateMap<Language, LanguageDto>();
            Mapper.CreateMap<Language, LanguageSaveDto>();

            Mapper.CreateMap<Keyword, KeywordDto>();
            Mapper.CreateMap<Keyword, KeywordSaveDto>();
            Mapper.CreateMap<AdCreativeAttribute, AdCreativeAttributeDto>();
            Mapper.CreateMap<Platform, PlatformDto>();
            Mapper.CreateMap<CreativeVendorKeyword, CreativeVendorKeywordDto>()
           .ForMember(dest => dest.VendorId, opt => opt.MapFrom(item => item.Vendor == null ? 0 : item.Vendor.ID));
            Mapper.CreateMap<CreativeVendorKeywordDto, CreativeVendorKeyword>()
.ForMember(dest => dest.Vendor, opt => opt.MapFrom(item => item.VendorId == 0 ? null : new CreativeVendor { ID = item.VendorId }));


            Mapper.CreateMap<CreativeVendor, CreativeVendorDto>()

                .ForMember(p => p.Keywords, z => z.MapFrom(x =>
                {
                    if (x.Keywords != null)
                    {
                        return x.Keywords.Select(q => MapperHelper.Map<CreativeVendorKeywordDto>(q));
                    }
                    return null;
                }));
            Mapper.CreateMap<PlatformVersion, PlatformVersionDto>();
            Mapper.CreateMap<Manufacturer, ManufacturerDto>();
            Mapper.CreateMap<Device, DeviceDto>();
            Mapper.CreateMap<Country, CountryDto>()
                .ForMember(p => p.Code, x => x.MapFrom(z => z.TwoLettersCode));
            Mapper.CreateMap<Gender, GenderDto>();
            Mapper.CreateMap<ImpressionMetric, ImpressionMetricDto>();
            Mapper.CreateMap<MetricVendor, MetricVendorDto>();
            Mapper.CreateMap<TileImageSize, TileImageSizeDto>()
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(item => item.DeviceType.ID));
            Mapper.CreateMap<TileImage, TileImageDto>();
            Mapper.CreateMap<Document, DocumentBaseDto>()
             .ForMember(dest => dest.UsedNameUp, opt => opt.MapFrom(item => item.GetNameWithNoExtension()));
            Mapper.CreateMap<Document, DocumentDto>()
                      .ForMember(dest => dest.UsedNameUp, opt => opt.MapFrom(item => item.GetNameWithNoExtension() ));
            Mapper.CreateMap<TileImageDocument, TileImageDocumentDto>();
            Mapper.CreateMap<CreativeUnitFormat, FormatDto>();
            Mapper.CreateMap<CreativeUnit, CreativeUnitDto>();
            Mapper.CreateMap<TileImageFormat, FormatDto>();
            Mapper.CreateMap<CreativeUnitGroup, CreativeUnitGroupDto>();
            Mapper.CreateMap<AgeGroup, AgeGroupDto>();
            Mapper.CreateMap<DeviceType, DeviceTypeDto>();



            Mapper.CreateMap<DeviceCapability, DeviceCapabilityDto>()
                .ForMember(dest => dest.WurflCapabilities, opt => opt.MapFrom(item => item.WurflCapabilities))
                .ForMember(dest => dest.WurflValue, opt => opt.MapFrom(item => item.WurflValue))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => item.Type));
            Mapper.CreateMap<CostModelWrapper, CostModelWrapperDto>()
                .ForMember(dest => dest.CostModel, opt => opt.MapFrom(item => item.CostModel.ID));
            Mapper.CreateMap<CostModel, CostModelDto>();

            /*Mapper.CreateMap<Device, DeviceDto>()
    .ForMember(dest => dest.Platform, opt => opt.MapFrom(item => MapperHelper.Map<PlatformDto>(item.Platform)))
    .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(item => MapperHelper.Map<ManufacturerDto>(item.Manufacturer)));*/
            Mapper.CreateMap<Currency, CurrencyDto>();
            Mapper.CreateMap<CostElement, CostElementDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type));
    
            Mapper.CreateMap<Fee, FeeDto>()
              .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type));
            Mapper.CreateMap<CostElementValueDto, CostItemValue>();
            Mapper.CreateMap<CostItemValue, CostElementValueDto>();
            Mapper.CreateMap<JobPosition, JobPositionDto>();
            Mapper.CreateMap<Party, EmployeeDto>();
            Mapper.CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.JobPositionId, opt => opt.MapFrom(item => item.JobPosition == null ? 0 : item.JobPosition.ID))
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                          .ForMember(dest => dest.AccountName, opt => opt.MapFrom(item => item.Account == null ? "" : item.Account.GetName()));

            Mapper.CreateMap<Party, BusinessPartnerDto>();
            Mapper.CreateMap<BusinessPartner, BusinessPartnerDto>()
                   .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                                .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

                 .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID))
                    .ForMember(dest => dest.AdvertiserList, z => z.MapFrom(x =>
                    {
                        if (x.AdvertiserBlockList != null)
                        {
                            return x.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                        }
                        return new List<int>();
                    }))

                        .ForMember(dest => dest.BlockedDomains, z => z.MapFrom(x =>
                        {
                            if (x.DomainBlockList != null)
                            {
                                string result = string.Empty;
                                foreach (var d in x.DomainBlockList)
                                {
                                    result = result + d.Domain + "\n";
                                };
                                return result;
                            }
                            return string.Empty;
                        }))
                        .ForMember(dest => dest.AccountList, z => z.MapFrom(x =>
                        {
                            if (x.AccountWhiteList != null)
                            {
                                return x.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
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




            Mapper.CreateMap<SSPPartner, BusinessPartnerDto>()
                      .ForMember(dest => dest.ImpressionTrackers, i => i.MapFrom(item => item.NumberOfSupportedImpressionTrackersInNative))
              .ForMember(dest => dest.ClicksTrackers, opt => opt.MapFrom(item => item.NumberOfSupportedClickTrackersInNative))
                           .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))
                .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID)).ForMember(dest => dest.AdvertiserList, z => z.MapFrom(x =>
                {
                    if (x.AdvertiserBlockList != null)
                    {
                        return x.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                    }
                    return new List<int>();
                }))
                   .ForMember(dest => dest.BlockedDomains, z => z.MapFrom(x =>
                   {
                       if (x.DomainBlockList != null)
                       {
                           string result = string.Empty;
                           foreach (var d in x.DomainBlockList)
                           {
                               result = result + d.Domain + "\n";
                           };
                           return result;
                       }
                       return string.Empty;
                   }))
                  .ForMember(dest => dest.AccountList, z => z.MapFrom(x =>
                  {
                      if (x.AccountWhiteList != null)
                      {
                          return x.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
                      }
                      return new List<int>();
                  }))
                 
                      .ForMember(dest => dest.WebCreativeFormatsList, z => z.MapFrom(x =>
                      {
                          if (x.WebCreativeFormatsList != null)
                          {
                              return x.WebCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
                          }
                          return new List<int>();
                      }))
                       .ForMember(dest => dest.MobileCreativeFormatsList, z => z.MapFrom(x =>
                       {
                           if (x.MobileCreativeFormatsList != null)
                           {
                               return x.MobileCreativeFormatsList.Select(q => (q.CreativeFormat.ID)).ToList();
                           }
                           return new List<int>();
                       }));

            Mapper.CreateMap<DSPPartner, BusinessPartnerDto>()
              .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
          .ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
                      .ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

            .ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID)).
            ForMember(dest => dest.AdvertiserList, z => z.MapFrom(x =>
            {
                if (x.AdvertiserBlockList != null)
                {
                    return x.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
                }
                return new List<int>();
            }))
               .ForMember(dest => dest.BlockedDomains, z => z.MapFrom(x =>
               {
                   if (x.DomainBlockList != null)
                   {
                       string result = string.Empty;
                       foreach (var d in x.DomainBlockList)
                       {
                           result = result + d.Domain + "\n";
                       };
                       return result;
                   }
                   return string.Empty;
               }))
                .ForMember(dest => dest.AccountList, z => z.MapFrom(x =>
                {
                    if (x.AccountWhiteList != null)
                    {
                        return x.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
                    }
                    return new List<int>();
                }))

            ;


            Mapper.CreateMap<DPPartner, BusinessPartnerDto>()
  .ForMember(dest => dest.BusinessPartnerTypeId, opt => opt.MapFrom(item => item.Type == null ? 0 : item.Type.ID))
.ForMember(dest => dest.AccountId, opt => opt.MapFrom(item => item.Account == null ? 0 : item.Account.ID))
.ForMember(dest => dest.documentId, opt => opt.MapFrom(item => item.Icon == null ? 0 : item.Icon.ID))

.ForMember(dest => dest.AppSiteId, opt => opt.MapFrom(item => item.AppSite == null ? 0 : item.AppSite.ID)).ForMember(dest => dest.AdvertiserList, z => z.MapFrom(x =>
{
    if (x.AdvertiserBlockList != null)
    {
        return x.AdvertiserBlockList.Select(q => (q.Advertiser.ID)).ToList();
    }
    return new List<int>();
}))
  .ForMember(dest => dest.AccountList, z => z.MapFrom(x =>
  {
      if (x.AccountWhiteList != null)
      {
          return x.AccountWhiteList.Select(q => (q.Account.ID)).ToList();
      }
      return new List<int>();
  }))
;


            Mapper.CreateMap<AdGroupCostElement, AdGroupCostElementDto>()
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


            Mapper.CreateMap<AdGroupFee, AdGroupFeeDto>()
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



            Mapper.CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => (int)item.Type));
            Mapper.CreateMap<Campaign, CampaignSettingsDto>()
                .ForMember(dest => dest.Keyword, opt => opt.MapFrom(item => item.Keyword == null ? null : MapperHelper.Map<KeywordDto>(item.Keyword)))
                 .ForMember(dest => dest.Advertiser, opt => opt.MapFrom(item => item.Advertiser == null ? null : MapperHelper.Map<AdvertiserDto>(item.Advertiser)))
                .ForMember(dest => dest.ValidCostModelWrapper, opt => opt.MapFrom(item => item.GetValidCostModelWrapper().HasValue ? (int?)item.GetValidCostModelWrapper() : (int?)null))
                .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.CostModelWrapper.HasValue ? (int?)item.CostModelWrapper : (int?)null));

            Mapper.CreateMap<CampaignServerSetting, CampaignServerSettingDto>()
                .ForMember(p => p.Name, x => x.MapFrom(z => z.Campaign.Name))
                .ForMember(p => p.ID, x => x.MapFrom(z => z.Campaign.ID));

            Mapper.CreateMap<CampaignFrequencyCapping, CampaignFrequencyCappingDto>()
                .ForMember(p => p.EventId, x => x.MapFrom(z => z.Event.ID))
                .ForMember(p => p.EventName, x => x.MapFrom(z => z.Event.EventName))
                  .ForMember(p => p.EventDescription, x => x.MapFrom(z => trackingEventRepository.GetAll().Where(y => y.EventName == z.Event.EventName).FirstOrDefault().GetDescription()));


            Mapper.CreateMap<CampaignServerSettingDto, CampaignServerSetting>();
            Mapper.CreateMap<LanguageDto, Language>();
            Mapper.CreateMap<LanguageSaveDto, Language>();

            Mapper.CreateMap<KeywordDto, Keyword>();
            Mapper.CreateMap<KeywordSaveDto, Keyword>();
            Mapper.CreateMap<LocationDto, LocationBase>();
            Mapper.CreateMap<CreativeVendorDto, CreativeVendor>();
            Mapper.CreateMap<PlatformDto, Platform>();
            Mapper.CreateMap<ManufacturerDto, Manufacturer>();
            Mapper.CreateMap<DeviceDto, Device>();
            Mapper.CreateMap<GenderDto, Gender>();

            Mapper.CreateMap<ImpressionMetricDto, ImpressionMetric>();
            Mapper.CreateMap<MetricVendorDto, MetricVendor>();
            Mapper.CreateMap<TileImageSizeDto, TileImageSize>();
            Mapper.CreateMap<TileImageDto, TileImage>();
            Mapper.CreateMap<DocumentDto, Document>()
            .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(item => _documentTypeRepository.Get(item.DocumentTypeId)));

            Mapper.CreateMap<TileImageDocumentDto, TileImageDocument>();
            Mapper.CreateMap<FormatDto, CreativeUnitFormat>();
            Mapper.CreateMap<CreativeUnitGroupDto, CreativeUnitGroup>();
            Mapper.CreateMap<FormatDto, TileImageFormat>();
            Mapper.CreateMap<AgeGroupDto, AgeGroup>();
            Mapper.CreateMap<DeviceCapabilityDto, DeviceCapability>();
            Mapper.CreateMap<CurrencyDto, Currency>();
            Mapper.CreateMap<CostElementDto, CostElement>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (CalculationType)item.TypeId));

            Mapper.CreateMap<FeeDto, Fee>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (CalculationType)item.TypeId));
            Mapper.CreateMap<JobPositionDto, JobPosition>();
            Mapper.CreateMap<PartyDto, Employee>();
            Mapper.CreateMap<Employee, PartyDto>().ForMember(dest => dest.AccountName,
                             opt => opt.MapFrom(x => x.Account != null ? x.Account.GetName() : ""));
            Mapper.CreateMap<PartyDto, BusinessPartner>().ForMember(dest => dest.Type,
                             opt => opt.Ignore());
            Mapper.CreateMap<PartyDto, SSPPartner>().ForMember(dest => dest.Type,
                         opt => opt.Ignore());

            Mapper.CreateMap<PartyDto, DPPartner>().ForMember(dest => dest.Type,
             opt => opt.Ignore());
            Mapper.CreateMap<DPPartner, PartyDto>();
            Mapper.CreateMap<PartyDto, DSPPartner>().ForMember(dest => dest.Type,
                         opt => opt.Ignore());
            Mapper.CreateMap<AdGroupCostElementDto, AdGroupCostElement>();
            Mapper.CreateMap<AdGroupFeeDto, AdGroupFee>();
            Mapper.CreateMap<DiscountDto, Discount>()
              .ForMember(dest => dest.Type, opt => opt.MapFrom(item => (DiscountType)item.TypeId));

            Mapper.CreateMap<HouseAd, HouseAdDto>()
                  .ForMember(dest => dest.AdGroup,
                             opt => opt.MapFrom(item => MapperHelper.Map<AdGroupDto>(item.AdGroup)))
                  .ForMember(dest => dest.ForAppSite,
                             opt => opt.MapFrom(item => MapperHelper.Map<AppSiteBasicDto>(item.ForAppSite)));
            //.ForMember(dest => dest.DestinationAppSites, opt => opt.MapFrom(item=>string.Join(",",item.DestinationAppSites.Select(x=>x.ID))));
            Mapper.CreateMap<CostModelWrapperDto, CostModelWrapper>()
                .ForMember(p => p.CostModel, x => x.MapFrom(z =>
                {

                    CostModel costModel = new CostModel() { ID = z.ID };
                    return costModel;
                }));

            Mapper.CreateMap<AppMarketingPartnerTracker, AppMarketingPartnerTrackerDto>()
                   .ForMember(dest => dest.Platform,
                     opt => opt.MapFrom(item => MapperHelper.Map<PlatformDto>(item.Platform)));

            Mapper.CreateMap<AppMarketingPartnerDto, AppMarketingPartner>()
                  .ForMember(dest => dest.Trackers, z => z.MapFrom(x =>
                  {
                      if (x.Trackers != null)
                      {
                          return x.Trackers.Select(q => MapperHelper.Map<AppMarketingPartnerTrackerDto>(q));
                      }
                      return null;
                  }));

            Mapper.CreateMap<Advertiser, AdvertiserDto>();
            Mapper.CreateMap<AdvertiserDto, Advertiser>();

            Mapper.CreateMap<CreativeFormat, CreativeFormatsDto>();
            Mapper.CreateMap<CreativeFormatsDto, CreativeFormat>();



            Mapper.CreateMap<SSPPartnerWhiteIP, WhitleListIPDto>()
              .ForMember(dest => dest.IPString,
                         opt => opt.MapFrom(item => new IPAddress(item.IP).ToString()))
                 .ForMember(p => p.SSPPartnerId, x => x.MapFrom(z => z.SSPPartner.ID));

            Mapper.CreateMap<WhitleListIPDto, SSPPartnerWhiteIP>()
  .ForMember(dest => dest.IP,
             opt => opt.MapFrom(item => IpHelper.ConvertIPToBytes(item.IPString)))
     .ForMember(p => p.SSPPartner, opt => opt.MapFrom(item => new SSPPartner { ID = item.SSPPartnerId }));


        }

        private static void RegisterCampaignDtoToCampaignMapping()
        {
            Mapper.CreateMap<CampaignDto, Campaign>()
                .ForMember(dest => dest.CostModelWrapper, opt => opt.Ignore());
        }

        private static void RegisterCampaignBidConfigDtoCampaignBidConfigMapping()
        {
            Mapper.CreateMap<AdGroupBidConfig, CampaignBidConfigDto>()
              .ForMember(p => p.AdGroupId, x => x.MapFrom(z => z.AdGroup.ID))
              .ForMember(p => p.AdGrouptName, x => x.MapFrom(z => z.AdGroup.Name))
              .ForMember(p => p.CampaingName, x => x.MapFrom(z => z.AdGroup.Campaign.Name))
              .ForMember(p => p.MinBid, x => x.MapFrom(z => z.AdGroup.GetReadableBid()))
              .ForMember(p => p.AccountName, x => x.MapFrom(z => z.Account.GetName()))
              .ForMember(p => p.AccountId, x => x.MapFrom(z => z.Account.ID))
              .ForMember(p => p.AdGroupPricingModel, x => x.MapFrom(z =>
              {
                  if (z.AdGroup.CostModelWrapper != null)
                      return z.AdGroup.CostModelWrapper.GetDescription();
                  return ResourceManager.Instance.GetResource("Default", "Campaign");
              }
                 ))
              .ForMember(p => p.AppsitePricingModel, x => x.MapFrom(z =>
                 {
                     var pricingModel = z.AppSite.AppSiteServerSetting.GetPricingModel();
                     if (pricingModel != null)
                         return pricingModel.GetDescription();
                     return ResourceManager.Instance.GetResource("Default", "Campaign");
                 }

                 )).ForMember(p => p.AppsitePricingModelId, x => x.MapFrom(z =>
                 {
                     var pricingModel = z.AppSite.AppSiteServerSetting.GetPricingModel();
                     if (pricingModel != null)
                         return pricingModel.ID;
                     return -1;
                 }))
              .ForMember(p => p.Appsite, opt => opt.MapFrom(item => MapperHelper.Map<AppSiteBasicDto>(item.AppSite)));


            Mapper.CreateMap<AdGroupInventorySource, InventorySourceDto>()
            .ForMember(p => p.AdGroupId, x => x.MapFrom(z => z.AdGroup.ID))
            .ForMember(p => p.AdGrouptName, x => x.MapFrom(z => z.AdGroup.Name))
            .ForMember(p => p.CampaingName, x => x.MapFrom(z => z.AdGroup.Campaign.Name))
             .ForMember(p => p.SSPId, x => x.MapFrom(z => z.Partner.ID))
            .ForMember(p => p.ExchangeName, x => x.MapFrom(z => z.Partner.Name))
            .ForMember(p => p.AccountId, x => x.MapFrom(z => z.Partner.Account.ID))
                  .ForMember(p => p.SubPublisherMarketId, x => x.MapFrom(z =>
                  {
                      if (z.SubAppsite != null)
                      {
                          return z.SubAppsite.SubPublisherMarketId;
                      }
                      return string.Empty;
                  }))


            


            .ForMember(p => p.Appsite, opt => opt.MapFrom(item => MapperHelper.Map<AppSiteBasicDto>(item.AppSite)))
            .ForMember(p => p.SubPublisher, x => x.MapFrom(z =>
            {
                if (z.SubAppsite != null)
                {
                    return z.SubAppsite.SubPublisherName;
                }
                return null;
            }))

             .ForMember(p => p.subPublisherId, x => x.MapFrom(z =>
             {
                 if (z.SubAppsite != null)
                 {
                     return z.SubAppsite.SubPublisherId;
                 }
                 return null;
             }));
        }

        private static void RegisterCampaignDtoMapping()
        {
            Mapper.CreateMap<Campaign, CampaignDto>()
                .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.CostModelWrapper.HasValue ? (int?)item.CostModelWrapper.Value : (int?)null))
                .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(item => item.User != null ? item.User.GetName() : string.Empty))
                  .ForMember(dest => dest.Advertiser, opt => opt.MapFrom(item => item.Advertiser == null ? null : MapperHelper.Map<AdvertiserDto>(item.Advertiser)))

                ; Mapper.CreateMap<AdCreativeUnitVendor, AdCreativeUnitVendorDto>()
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(item => item.Unit != null ? item.Unit.ID : 0))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(item => item.Vendor != null ? item.Vendor.ID : 0))
      .ForMember(dest => dest.VendorText, opt => opt.MapFrom(item => item.Vendor != null ? item.Vendor.Name.GetValue() : string.Empty))
                ;

            Mapper.CreateMap<AdCreativeUnitVendorDto, AdCreativeUnitVendor>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(item => item.UnitId > 0 ? new AdCreativeUnit { ID = item.UnitId } : null))
                .ForMember(dest => dest.Vendor, opt => opt.MapFrom(item => item.VendorId > 0 ? new CreativeVendor { ID = item.VendorId } : null))

                ;



            Mapper.CreateMap<Campaign, CampaignListDto>();
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()));

            Mapper.CreateMap<AdGroup, AdGroupListDto>();
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status == null ? string.Empty : item.Status.Name.ToString()));
            Mapper.CreateMap<AdGroup, AdGroupDto>()
                 .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(item => (AdActionTypeIds)item.Objective.AdAction.ID))
                 .ForMember(dest => dest.ObjectiveTypeId, opt => opt.MapFrom(item => (AdGroupObjectiveTypeIds)item.Objective.Objective.ID))
                 .ForMember(dest => dest.TypeId, opt => opt.MapFrom(item => item.Objective.AdType == null ? new AdTypeIds?() : (AdTypeIds)item.Objective.AdType.ID))
                  .ForMember(dest => dest.IsCostModelChanged, opt => opt.MapFrom(item => item.IsCostModelChanged));

            Mapper.CreateMap<AdCreative, AdCreativeDto>()
                .ForMember(dest => dest.Group, opt => opt.MapFrom(item => MapperHelper.Map<AdGroupDto>(item.Group)))
            .ForMember(dest => dest.AdType, opt => opt.MapFrom(item => MapperHelper.Map<AdTypeDto>(item.Type)));

            Mapper.CreateMap<AdCreative, AdListDto>()
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(item => item.Status.GetDescription()))

             .ForMember(dest => dest.StatusId, opt => opt.MapFrom(item => item.Status== null ?0 : item.Status.ID));
            Mapper.CreateMap<AdCreative, AdbIDListDto>()
                .ForMember(p => p.Bid, x => x.MapFrom(z => z.GetReadableBid().ToString()));

            Mapper.CreateMap<AdActionValue, AdActionValueDto>()
                .ForMember(p => p.Trackers, z => z.MapFrom(x =>
                {
                    if (x.Trackers != null)
                    {
                        return x.Trackers.Where(w => !w.IsDeleted).Select(q => MapperHelper.Map<AdActionValueTrackerDto>(q));
                    }
                    return null;
                }));
            //Mapper.CreateMap<AdActionValue, AdActionValueRichMediaDto>();


            Mapper.CreateMap<AdActionValueDto, AdActionValue>();

            Mapper.CreateMap<AdCreativeUnit, AdCreativeUnitDto>()
                .ForMember(dest => dest.CreativeUnitId, opt => opt.MapFrom(item => item.CreativeUnit.ID))
                .ForMember(dest => dest.CreativeUnit, opt => opt.MapFrom(item => MapperHelper.Map<CreativeUnitDto>(item.CreativeUnit)))
                //  .ForMember(dest => dest.InStreamVideoCreativeUnit, opt => opt.MapFrom(item => MapperHelper.Map<InStreamVideoCreativeUnitDto>(item.CreativeUnit)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : item.CreativeUnit.GetDescription()))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(item => item.Document != null ? item.Document.ID : (int?)null))

                 .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : string.Empty))
                .ForMember(dest => dest.SnapshotDocumentId, opt => opt.MapFrom(item => item.SnapshotDocument != null ? item.SnapshotDocument.ID : (int?)null))
                  .ForMember(dest => dest.AdCreativeVendorIds, opt => opt.MapFrom(item =>
                  {
                      //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                      IEnumerable<AdCreativeUnitVendorDto> adCreativeVendors = item.AdCreativeUnitVendorList.Select(x =>
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
                 .ForMember(dest => dest.CreativeVendorIds, opt => opt.MapFrom(item =>
                 {
                     //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                     IEnumerable<int> adCreativeVendors = item.AdCreativeUnitVendorList.Select(x =>
                      x.Vendor.ID);
                     //  IList<AdActionValueTrackerDto>
                     return adCreativeVendors;
                 }))

                .ForMember(dest => dest.ImpressionTrackerRedirect, opt => opt.MapFrom(item =>
                {

                    var impressionTracker = item.GetTrackers().FirstOrDefault();
                    if (impressionTracker != null)
                    {
                        return impressionTracker.TrackingUrl;
                    }

                    return string.Empty;
                }))
                    .ForMember(dest => dest.ImpressionTrackerJSRedirect, opt => opt.MapFrom(item =>
                    {

                        var impressionTracker = item.GetTrackers().FirstOrDefault();
                        if (impressionTracker != null)
                        {
                            return impressionTracker.TrackingJS;
                        }

                        return string.Empty;
                    }))


                ;

            Mapper.CreateMap<InStreamVideoCreativeUnit, InStreamVideoCreativeUnitDto>()
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
            .ForMember(dest => dest.ImpressionTrackerRedirectList, opt => opt.MapFrom(item =>
            {
                //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                IEnumerable<AdCreativeUnitTrackerDto> impressionTracker = item.AdCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code.ToLower() != IMPRESSIONEVENT && M.AdGroupEvent.Code.ToLower() != CLICKEVENT).Select(x =>
                      new AdCreativeUnitTrackerDto()
                      {
                          ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(y => y.AdGroupEvent.ID == x.AdGroupEvent.ID).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList()
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

             .ForMember(dest => dest.ImpressionTrackerJSRedirectList, opt => opt.MapFrom(item =>
             {
                 //    var ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(x=> x.AdGroupEvent.ID = 1).Select(y => new AdActionValueTrackerDto() { URL = y.TrackingUrl }).ToList();
                 IEnumerable<AdCreativeUnitTrackerDto> impressionTracker = item.AdCreativeUnit.GetTrackers().Where(M => M.AdGroupEvent.Code.ToLower() != IMPRESSIONEVENT && M.AdGroupEvent.Code.ToLower() != CLICKEVENT).Select(x =>
                       new AdCreativeUnitTrackerDto()
                       {
                           ImpressionURls = item.AdCreativeUnit.GetTrackers().Where(y => y.AdGroupEvent.ID == x.AdGroupEvent.ID).Select(y => new AdActionValueTrackerDto() { JS = y.TrackingJS }).ToList()
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


            Mapper.CreateMap<TileImageDocument, AdCreativeUnitDto>()
                .ForMember(dest => dest.CreativeUnitId, opt => opt.MapFrom(item => item.TileImageSize.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Document != null ? item.Document.GetNameWithNoExtension() : string.Empty))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(item => item.Document != null ? item.Document.ID : (int?)null));


            Mapper.CreateMap<AdType, LookupDto>();
            Mapper.CreateMap<LookupDto, AdType>();

            Mapper.CreateMap<CompanyType, LookupDto>();
            Mapper.CreateMap<LookupDto, CompanyType>();

            Mapper.CreateMap<AdSubType, AdSubtypeDto>()
                                .ForMember(dest => dest.AdTypeId, opt => opt.MapFrom(item => item.AdType.ID))
                                .ForMember(dest => dest.AdActionTypeIds, opt => opt.MapFrom(item => item.AdTypeActions.Select(x => x.ActionType.ID)))
                                .ForMember(dest => dest.Permission, opt => opt.MapFrom(item => MapperHelper.Map<AdPermissionDto>(item.Permission)));


            Mapper.CreateMap<AdSubType, LookupDto>();
            Mapper.CreateMap<LookupDto, AdSubType>();

            Mapper.CreateMap<PortalPermision, AdPermissionDto>();
            Mapper.CreateMap<AdPermissionDto, PortalPermision>();
            Mapper.CreateMap<LookupDto, PortalPermision>();


            Mapper.CreateMap<NativeAdLayout, LookupDto>();
            Mapper.CreateMap<LookupDto, NativeAdLayout>();
        }
        private static void RegisterAdActionTypeConstraintMapping()
        {
            Mapper.CreateMap<AdActionTypeConstraint, AdActionTypeConstraintDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                .ForMember(dest => dest.DeviceConstraint, opt => opt.MapFrom(item => item.DeviceConstraint))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(item => MapperHelper.Map<PlatformDto>(item.Platform)));
        }

        private static void RegisterAdActionTypeMapping()
        {
            Mapper.CreateMap<AdActionCostModelWrapper, AdActionCostModelWrapperDto>()
           .ForMember(dest => dest.CostModelWrapperId,
             opt => opt.MapFrom(item => item.CostModelWrapper != null ? item.CostModelWrapper.ID : 0));

            Mapper.CreateMap<AdType, AdTypeDto>()
.ForMember(dest => dest.Subtypes, opt => opt.MapFrom(item => item.SubTypes == null ? null : item.SubTypes.Select(x => MapperHelper.Map<AdSubType>(x)).ToList()))
            .ForMember(dest => dest.AdPermission, opt => opt.MapFrom(item => item.Permission == null ? null : MapperHelper.Map<AdPermissionDto>(item.Permission)));

            Mapper.CreateMap<AdActionTypeBase, AdActionTypeDto>()
                  .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
            .ForMember(dest => dest.ShowInAppId, opt => opt.MapFrom(item => item.ShowInAppId))
            .ForMember(dest => dest.AdActionCostModelWrappers, opt => opt.MapFrom(item => item.AdActionCostModelWrappers == null ? null : item.AdActionCostModelWrappers.Select(x => MapperHelper.Map<AdActionCostModelWrapperDto>(x)).ToList()))
.ForMember(dest => dest.AdTypes, opt => opt.MapFrom(item => item.AdTypes == null ? null : item.AdTypes.Select(x => MapperHelper.Map<AdTypeDto>(x)).ToList()))
            .ForMember(dest => dest.CostModelWrappers, opt => opt.MapFrom(item => item.AdActionCostModelWrappers == null ? null : item.AdActionCostModelWrappers.Select(p => p.CostModelWrapper.ID).ToList()));
        }

        private static void RegisterLocationMapping()
        {
            Mapper.CreateMap<LocationBase, Continent>();
            Mapper.CreateMap<LocationBase, Country>();
            Mapper.CreateMap<LocationBase, State>();
            Mapper.CreateMap<LocationBase, City>();

            Mapper.CreateMap<LocationBase, LocationDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(item => item.Parent != null ? item.Parent.ID : new int?()))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(item => item.Locations))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(item =>
                {
                    if (item is Country)
                    {
                        return LocationType.Country;
                    }
                    if (item is City)
                    {
                        return LocationType.City;
                    }
                    if (item is State)
                    {
                        return LocationType.State;
                    }
                    return LocationType.Continent;

                }));

        }

        private static void RegisterTargetingMapping()
        {
            Mapper.CreateMap<TargetingBase, TargetingBaseDto>()
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
            //Mapper.CreateMap<DeviceTargeting, DeviceTargetingDto>();
            Mapper.CreateMap<DeviceTargeting, DeviceTargetingDto>()
                .ForMember(dest => dest.Platforms, opt => opt.MapFrom(item =>
                    {
                        List<Platform> platforms = item.Platforms.ToList();
                        List<PlatformDto> platformsDto = new List<PlatformDto>(platforms.Select(p => MapperHelper.Map<PlatformDto>(p)));
                        foreach (var platform in platformsDto)
                        {
                            if (item.PlatformsTargeting.Where(p => p.Platform.ID == platform.ID).SingleOrDefault().IsAll)
                            {
                                platform.IsSelected = true;
                            }

                            PlatformTargeting targetingPlatform = item.PlatformsTargeting.Where(p => p.Platform.ID == platform.ID).SingleOrDefault();
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

            Mapper.CreateMap<GeographicTargeting, GeographicTargetingDto>();
            Mapper.CreateMap<OperatorTargeting, OperatorTargetingDto>();
            Mapper.CreateMap<DemographicTargeting, DemographicTargetingDto>();
            Mapper.CreateMap<KeywordTargeting, KeywordTargetingDto>();
            Mapper.CreateMap<LanguageTargeting, LanguageTargetingDto>();
            Mapper.CreateMap<VideoTargeting, VideoTargetingDto>();
            Mapper.CreateMap<URLTargeting, URLTargetingDto>();
            Mapper.CreateMap<GeoFencingTargeting, GeoFencingTargetingDto>();
            Mapper.CreateMap<IPTargeting, IPTargetingDto>()
                .ForMember(dest => dest.StartRange,
                           opt => opt.MapFrom(item => new IPAddress(item.StartRange).ToString()))
                .ForMember(dest => dest.EndRange,
                           opt => opt.MapFrom(item => new IPAddress(item.EndRange).ToString()));
            Mapper.CreateMap<Demographic, DemographicDto>();
            Mapper.CreateMap<DeviceTargetingType, DeviceTargetingTypeDto>();
            Mapper.CreateMap<TargetingType, TargetingTypeDto>();
        }

        private static void RegisterDeviceMapping()
        {
           // Mapper.CreateMap<CostItem, Fee>();
           // Mapper.CreateMap<CostItem, CostElement>();
           

            Mapper.CreateMap<LookupDto, ManagedLookupBase>()
            .Include<DeviceDto, Device>()
            .Include<ManufacturerDto, Manufacturer>()
            .Include<PlatformDto, Platform>()

         //.Include<CostElementDto, CostItem>()
            .Include<CostElementDto, CostElement>()
       
                .Include<FeeDto, Fee>()
            .Include<CurrencyDto, Currency>()
            .Include<CreativeVendorDto, CreativeVendor>()
            .Include<AdvertiserDto, Advertiser>()
                        .Include<CreativeFormatsDto, CreativeFormat>()

            .Include<DeviceCapabilityDto, DeviceCapability>()
            .Include<AdCreativeAttributeDto, AdCreativeAttribute>();

            Mapper.CreateMap<ManagedLookupBase, LookupDto>()
            .Include<Device, DeviceDto>()

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


        }

        private static void RegisterAudienceSegmentMapping()
        {
            Mapper.CreateMap<AudienceSegment, AudienceSegmentDto>()
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



            Mapper.CreateMap<AudienceSegmentDto, AudienceSegment>()
               .ForMember(dest => dest.Provider, opt => opt.MapFrom(item => DPPartnerRepository.Get(item.ProviderId)))
               .ForMember(dest => dest.Selectable, opt => opt.MapFrom(item => item.IsSelectedable))
               .ForMember(dest => dest.Code, opt => opt.MapFrom(item => item.CodeUQ))
                 .ForMember(p => p.User, x => x.MapFrom(z => z.UserId > 0 ? new User { ID = z.UserId } : null))
                .ForMember(p => p.Account, x => x.MapFrom(z => z.AccountId > 0 ? new Account { ID = z.AccountId } : null))
                 .ForMember(p => p.Advertiser, x => x.MapFrom(z => z.AdvertiserId > 0 ? new AdvertiserAccount { ID = z.AdvertiserId } : null))


                              .ForMember(dest => dest.IsPermissionNeed, opt => opt.MapFrom(item => item.IsPermissionNeed))

               .ForMember(dest => dest.Parent, opt => opt.MapFrom(item => audianSegRep.Get(item.ParentId)));

        }
        private static void RegisterOperatorMapping()
        {
            Mapper.CreateMap<Operator, OperatorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Name));

            //Mapper.CreateMap<Operator, CountryOperatorDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(item => item.Location.ID))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(item => item.Location.Name))
            //    .ForMember(dest => dest.Operators, opt => opt.MapFrom(GeOperatorsDto));
        }
        private static void RegisteLocalizedStringMapping()
        {
            Mapper.CreateMap<LocalizedValue, LocalizedValueDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(item => item.Culture))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(item => item.Value));

            Mapper.CreateMap<LocalizedString, LocalizedStringDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(item => item.Values))
                .ForMember(dest => dest.Value, opt => opt.Ignore());


            Mapper.CreateMap<LocalizedValueDto, LocalizedValue>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(item => item.Culture))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(item => item.Value));

            Mapper.CreateMap<LocalizedStringDto, LocalizedString>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(item => item.ID))
                .ForMember(dest => dest.GroupKey, opt => opt.MapFrom(item => item.GroupKey))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(item => item.Values))
                .ForMember(dest => dest.Value, opt => opt.Ignore());



            //Mapper.CreateMap<LocalizedString, LocalizedStringDto>()
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
        private static AppSiteTypeDto GetAppSiteTypeDto(AppSite appSite)
        {
            if (appSite.Type == null)
                return null;
            return MapperHelper.Map<AppSiteTypeDto>(appSite.Type);
        }
        private static ThemeDto GetAppSiteThemeDto(AppSite appSite)
        {
            if (appSite.Theme == null)
                return null;
            return MapperHelper.Map<ThemeDto>(appSite.Theme);
        }
        private static string GetAppSiteURLDto(AppSite appSite)
        {
            if (appSite is App)
                return ((App)appSite).MarketURL;
            else
            {
                return ((Site)appSite).SiteURL;
            }
        }
        private static IEnumerable<KeywordDto> GetAppSiteKeywordsDto(AppSite appSite)
        {
            if (appSite.Keywords == null)
                return null;
            return appSite.Keywords.Select(appsiteKeyword => MapperHelper.Map<KeywordDto>(appsiteKeyword.Keyword)).ToList();
        }


        private static void RegisterAppSiteMapping()
        {
            Mapper.CreateMap<AppSite, AppSiteBasicDto>();

            Mapper.CreateMap<AppSite, AppSiteListDtoBase>()
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(GetAppSiteType))
                  .ForMember(dest => dest.AccountId, opt => opt.MapFrom(z => z.Account.ID))
                  .ForMember(dest => dest.AccountName, opt => opt.MapFrom(z => string.Format("{0} {1}", z.Account.PrimaryUser.FirstName, z.Account.PrimaryUser.LastName)));

            Mapper.CreateMap<AppSite, AppSiteListDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(x => x.Account.GetAccountName()))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(GetAppSiteType))
                  .ForMember(dest => dest.TypeId, opt => opt.MapFrom(x => x.Type.ID))
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(GetAppSiteStatus))
                  .ForMember(dest => dest.AdHouse, opt => opt.MapFrom(GetAppAdHouse));

            Mapper.CreateMap<AppSite, AppSiteDto>()
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(GetAppSiteTypeDto))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom(GetAppSiteKeywordsDto))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(GetAppSiteThemeDto))
                  .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(p => p.IsPublished))
                      .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(p => p.User != null ? p.User.GetName() : string.Empty))
                  .ForMember(dest => dest.URL, opt => opt.MapFrom(GetAppSiteURLDto));

            Mapper.CreateMap<App, AppSiteDto>()
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(GetAppSiteTypeDto))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom(GetAppSiteKeywordsDto))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(GetAppSiteThemeDto))
                                        .ForMember(dest => dest.SupUserName, opt => opt.MapFrom(p => p.User != null ? p.User.GetName() : string.Empty))

                  .ForMember(dest => dest.URL, opt => opt.MapFrom(GetAppSiteURLDto));

            //Mapper.CreateMap<AppSiteDto, AppSite>()
            //    .ForMember(dest => dest.Status, opt => opt.Ignore())
            //    .ForMember(dest => dest.Type, opt => opt.Ignore())
            //    .ForMember(dest => dest.Theme, opt => opt.MapFrom(MapAppSiteTheme))
            //    .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp));


            Mapper.CreateMap<AppSiteDto, App>()
                  .ForMember(dest => dest.Status, opt => opt.Ignore())
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(MapAppSiteType))
                  .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(MapAppSiteTheme))
                  .ForMember(dest => dest.MarketURL, opt => opt.MapFrom(item => item.URL));

            Mapper.CreateMap<AppSiteDto, Site>()
                  .ForMember(dest => dest.Status, opt => opt.Ignore())
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(MapAppSiteType))
                  .ForMember(dest => dest.IsApp, opt => opt.MapFrom(item => item.Type.IsApp))
                  .ForMember(dest => dest.Theme, opt => opt.MapFrom(MapAppSiteTheme))
                  .ForMember(dest => dest.SiteURL, opt => opt.MapFrom(item => item.URL));


            Mapper.CreateMap<App, AppSiteDtoBase>()
                    .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom(x =>
                    {
                        AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                        accountInfo.AccountEmail = x.Account.PrimaryUser.EmailAddress;
                        accountInfo.AccountName = x.Account.PrimaryUser.GetName();
                        accountInfo.AccountCompanyName = x.Account.PrimaryUser.Company;
                        accountInfo.Country = MapCountryToCountryDto(x);
                        return accountInfo;
                    }))
                  .ForMember(dest => dest.AdminComment, opt => opt.MapFrom(item => item.LastAdminComment))
                  .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                  .ForMember(dest => dest.URL, opt => opt.MapFrom(item => item.MarketURL))
                  .ForMember(dest => dest.Keywords, opt => opt.MapFrom(GetAppSiteKeywordsDto))
                  .ForMember(dest => dest.AccountLanguage, opt => opt.MapFrom(item => item.Account.PrimaryUser.Language.Name));

            Mapper.CreateMap<Site, AppSiteDtoBase>()
                 .ForMember(dest => dest.AccountInfo, opt => opt.MapFrom(x =>
                 {
                     AppSiteAccountInfo accountInfo = new AppSiteAccountInfo();
                     accountInfo.AccountEmail = x.Account.PrimaryUser.EmailAddress;
                     accountInfo.AccountName = x.Account.PrimaryUser.GetName();
                     accountInfo.AccountCompanyName = x.Account.PrimaryUser.Company;
                     accountInfo.Country = MapCountryToCountryDto(x);
                     return accountInfo;
                 }))
                 .ForMember(dest => dest.Keywords, opt => opt.MapFrom(GetAppSiteKeywordsDto))
                .ForMember(dest => dest.AdminComment, opt => opt.MapFrom(item => item.LastAdminComment))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(item => item.Status.Name))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(item => item.SiteURL))
                .ForMember(dest => dest.AccountLanguage, opt => opt.MapFrom(item => item.Account.PrimaryUser.Language.Name));

            Mapper.CreateMap<Site, BasicAppSiteInformation>()
                .ForMember(p => p.AppsiteUrl, opt => opt.MapFrom(x => x.GetURL()))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.PrimaryUser.GetAccountName()));

            Mapper.CreateMap<App, BasicAppSiteInformation>()
                .ForMember(p => p.AppsiteUrl, opt => opt.MapFrom(x => x.GetURL()))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(x => x.Account.PrimaryUser.EmailAddress))
                .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.PrimaryUser.GetAccountName()));
        }

        private static CountryDto MapCountryToCountryDto(AppSite appsite)
        {
            Country country = appsite.Account.PrimaryUser.Country;
            return new CountryDto()
            {
                ID = country.ID,
                Name = MapperHelper.Map<LocalizedStringDto>(country.Name)
            };
        }

        private static void RegisterReturnBidToReturnBidDto()
        {
            Mapper.CreateMap<ReturnBid, ReturnBidDto>();
        }

        private static void RegisterAdActionTypeTrackingEventToAdGroupTrackingEventDto()
        {
            Mapper.CreateMap<AdActionTypeTrackingEvent, AdGroupTrackingEventDto>()
                .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
                    .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
                 .ForMember(p => p.Name, x => x.MapFrom(z => z.Event.Name))
                .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
                .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
                 .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                   .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
                .ForMember(p => p.PreRequisites, x => x.MapFrom(z =>
                {
                    return string.Join(",", z.GetAllPrerequisitesCodes());
                }))
                .ForMember(p => p.IsCustom, x => x.UseValue<bool>(false))
                .ForMember(p => p.Id, x => x.Ignore());
        }

        private static void RegisterAdActionTypeTrackingEventToAdGroupTrackingEvent()
        {
            Mapper.CreateMap<AdActionTypeTrackingEvent, AdGroupTrackingEvent>()
                .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
                .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
                .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
                .ForMember(p => p.StatisticsColumnName, x => x.MapFrom(z => z.StatisticsColumnName.ToString()))
               .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
                .ForMember(p => p.IsCustom, x => x.UseValue<bool>(false))
                  .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                     .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
                .ForMember(p => p.PreRequisites, x => x.UseValue(null))
                 .ForMember(p => p.ID, x => x.Ignore());


            Mapper.CreateMap<AdActionTypeTrackingEvent, AdGroupConversionEvent>()
               .ForMember(p => p.Description, x => x.MapFrom(z => z.Event.EventName))
               .ForMember(p => p.ValidFor, x => x.MapFrom(z => z.Event.ValidFor))
               .ForMember(p => p.Code, x => x.MapFrom(z => z.Event.Code.ToString()))
               .ForMember(p => p.StatisticsColumnName, x => x.MapFrom(z => z.StatisticsColumnName.ToString()))
              .ForMember(p => p.AllPreRequisitesRequired, x => x.MapFrom(z => z.AllPreRequisitesRequired))
               .ForMember(p => p.IsCustom, x => x.UseValue<bool>(false))
                 .ForMember(p => p.AllowDuplicate, x => x.MapFrom(z => z.AllowDuplicate))
                    .ForMember(p => p.IsBillable, x => x.MapFrom(z => z.IsBillable))
               .ForMember(p => p.PreRequisites, x => x.UseValue(null))
                .ForMember(p => p.ID, x => x.Ignore());
        }


        private static void RegisterAdGroupTrackingEventToAdGroupTrackingEventDto()
        {

            Mapper.CreateMap<AdGroupTrackingEvent, AdGroupTrackingEventDto>()
                   .ForMember(p => p.Name, x => x.MapFrom(z =>

                   {
                       var itemevent = trackingEventRepository.GetAll().Where(y => y.Code == z.Code).FirstOrDefault();




                       if (itemevent != null)
                           return itemevent.GetDescription();
                       else
                           return null;
                   }
                ))
                        .ForMember(p => p.SegmentsMapId, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                   .ForMember(p => p.SegmentsId, x => x.MapFrom(z=>z.AudienceSegmentListsMap!=null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.ID.ToString()).ToArray())) : string.Empty))
            .ForMember(p => p.SegmentString, x => x.MapFrom(z => z.AudienceSegmentListsMap != null ? (string.Join(",", z.AudienceSegmentListsMap.Select(b => b.AudienceSegment.Name.Value.ToString()).ToArray())) : string.Empty))
            
                .ForMember(p => p.PreRequisites, x => x.Ignore());



            Mapper.CreateMap<AdGroupConversionEvent, AdGroupConversionEventDto>()
                 .ForMember(p => p.Name, x => x.MapFrom(z => trackingEventRepository.GetAll().Where(y => y.Code == z.Code).FirstOrDefault().GetDescription()))
                      .ForMember(p => p.PixelsMapId, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.ID.ToString()).ToArray())) : string.Empty))
                 .ForMember(p => p.PixelsId, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.Pixel.ID.ToString()).ToArray())) : string.Empty))
          .ForMember(p => p.PixelString, x => x.MapFrom(z => z.PixelListsMap != null ? (string.Join(",", z.PixelListsMap.Select(b => b.Pixel.Name.ToString()).ToArray())) : string.Empty))

              .ForMember(p => p.PreRequisites, x => x.Ignore());
        }

        private static void RegisterAdGroupTrackingEventSaveDtoToAdGroupTrackingEvent()
        {
            Mapper.CreateMap<AdGroupTrackingEventSaveDto, AdGroupTrackingEvent>()
                .ForMember(p => p.PreRequisites, x => x.UseValue(null));

            Mapper.CreateMap<AdGroupConversionEventSaveDto, AdGroupConversionEvent>()
              .ForMember(p => p.PreRequisites, x => x.UseValue(null));
        }
        //private static void RegisterCampaignAssignedAppsiteToCampaignAssignedAppsiteDto()
        //{
        //    Mapper.CreateMap<CampaignAssignedAppsite, CampaignAssignedAppsitesDto>()
        //        .ForMember(p => p.Appsite, x => x.UseValue(null));
        //}

        private static void RegisterAdActionValueTrackerToAdActionValueTrackerDto()
        {
            Mapper.CreateMap<AdActionValueTracker, AdActionValueTrackerDto>();
        }
        private static void RegisterVideoCardDto()
        {
            Mapper.CreateMap<VideoEndCardTracker, VideoEndCardTrackerDto>()
                   .ForMember(p => p.CardId, x => x.MapFrom(z => z.Card.ID));

            Mapper.CreateMap<VideoEndCardTrackerDto, VideoEndCardTracker>().ForMember(p => p.Card, x => x.MapFrom(z => z.CardId > 0 ? new VideoEndCard { ID = z.CardId } : null));



            Mapper.CreateMap<VideoEndCard, VideoEndCardDto>()
          .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document.ID))
               .ForMember(p => p.CreativeUnitId, x => x.MapFrom(z => z.CreativeUnit.ID))
                    .ForMember(p => p.AdCreativeId, x => x.MapFrom(z => z.VideoAd.ID))



          ;


            Mapper.CreateMap<VideoEndCardDto, VideoEndCard>().
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

            Mapper.CreateMap<VideoMediaFile, VideoMediaFileDto>()
                     .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document.ID))
                     .ForMember(p => p.CreativeUnitId, x => x.MapFrom(z => z.OriginalCreativeUnit.ID))
                     .ForMember(p => p.AdCreativeId, x => x.MapFrom(z => z.VideoAd == null ? 0 : z.VideoAd.ID))
                     .ForMember(p => p.DocumentId, x => x.MapFrom(z => z.Document == null ? 0 : z.Document.ID))
                     .ForMember(p => p.VideoTypeId, x => x.MapFrom(z => z.VideoType == null ? 0 : z.VideoType.ID))
                     .ForMember(p => p.DeliveryMethodId, x => x.MapFrom(z => z.DeliveryMethod == null ? 0 : z.DeliveryMethod.ID))
                     .ForMember(p => p.AdCreativeUnitId, x => x.MapFrom(z => z.AdCreativeUnit == null ? 0 : z.AdCreativeUnit.ID))
           ;


            Mapper.CreateMap<VideoMediaFileDto, VideoMediaFile>().
                ForMember(p => p.Document, x => x.MapFrom(z => z.DocumentId > 0 ? new Document { ID = z.DocumentId } : null))
                      .ForMember(p => p.VideoType, x => x.MapFrom(z => z.VideoTypeId > 0 ? new MIMEType { ID = z.VideoTypeId } : null))
                           .ForMember(p => p.DeliveryMethod, x => x.MapFrom(z => z.DeliveryMethodId > 0 ? new VideoDeliveryMethod { ID = z.DeliveryMethodId } : null))
                                            .ForMember(p => p.OriginalCreativeUnit, x => x.MapFrom(z => z.CreativeUnitId > 0 ? new CreativeUnit { ID = z.CreativeUnitId } : null))
                                              .ForMember(p => p.VideoAd, x => x.MapFrom(z => z.VideoAdId > 0 ? new InStreamVideoCreative { ID = z.VideoAdId } : null))
                                                 .ForMember(p => p.AdCreativeUnit, x => x.MapFrom(z => z.AdCreativeUnitId > 0 ? new AdCreativeUnit { ID = z.AdCreativeUnitId } : null));


            //Mapper.CreateMap<AdActionValue, AdActionValueDto>()
            // .ForMember(p => p.Trackers, z => z.MapFrom(x =>
            // {
            //     if (x.Trackers != null)
            //     {
            //         return x.Trackers.Where(w => !w.IsDeleted).Select(q => MapperHelper.Map<AdActionValueTrackerDto>(q));
            //     }
            //     return null;
            // }));
            ////Mapper.CreateMap<AdActionValue, AdActionValueRichMediaDto>();


            //Mapper.CreateMap<AdActionValueDto, AdActionValue>();

        }
        private static void RegisterTrackingEventToTrackingEventDto()
        {
            Mapper.CreateMap<TrackingEvent, TrackingEventDto>()
                  .ForMember(p => p.EventDescription, x => x.MapFrom(z => z.Name.Value))
                   .ForMember(p => p.DefaultFrequencyCapping, x => x.MapFrom(z => z.DefaultFrequencyCapping));
        }

        private static void RegisterCampaignFrequencyCappingSaveDtoToCampaignFrequencyCapping()
        {
            Mapper.CreateMap<CampaignFrequencyCappingSaveDto, CampaignFrequencyCapping>();
            Mapper.CreateMap<CampaignFrequencyCappingDto, CampaignFrequencyCapping>();
        }

        #region AppSiteSettingsMapping

        private static void RegisterAppSiteSettingsMapping()
        {
            Mapper.CreateMap<AppSiteServerSettingDto, AppSiteServerSetting>().ForMember(p => p.NativeAdLayout, x => x.MapFrom(z =>
                   {
                       if (z.NativeLayoutId > 0)
                       {

                           NativeAdLayout acc = new NativeAdLayout() { ID = z.NativeLayoutId };
                           return acc;
                       }
                       return null;
                   }));


            Mapper.CreateMap<AppSiteRevenueCalculationSettingDto, AppSiteRevenueCalculationSetting>()
                .ForMember(p => p.Value, x => x.MapFrom(z =>
                {
                    if (z.CalculationMode == CalculationMode.Percentage)
                    {
                        return z.Value / 100;
                    }
                    return z.Value;
                }));


            Mapper.CreateMap<AppSiteServerSetting, AppSiteServerSettingDto>()
                 .ForMember(dest => dest.CostModelWrapper, opt => opt.MapFrom(item => item.GetPricingModel()))
                 .ForMember(dst => dst.NativeLayoutId, opt => opt.MapFrom(z => z.NativeAdLayout != null ? z.NativeAdLayout.ID : 0));



            Mapper.CreateMap<AppSiteRevenueCalculationSetting, AppSiteRevenueCalculationSettingDto>()
                 .ForMember(p => p.Value, x => x.MapFrom(z =>
                 {
                     if (z.CalculationMode == CalculationMode.Percentage)
                     {
                         return z.Value * 100;
                     }
                     return z.Value;
                 }));


            Mapper.CreateMap<AppSiteEvent, AppSiteEventDto>()
                .ForMember(p => p.EventId, x => x.MapFrom(z => z.Event.ID))
                .ForMember(p => p.EventName, x => x.MapFrom(z => z.Event.EventName))
                .ForMember(p => p.MinBid, x => x.MapFrom(z =>
                {
                    decimal? mindBid = new decimal?();
                    if (z.MinBid.HasValue)
                    {
                        mindBid = z.MinBid.Value * z.Event.CostModelWrapper.Factor;
                    }

                    return mindBid;
                }));

            Mapper.CreateMap<AppSiteEventDto, AppSiteEvent>();
            //Mapper.CreateMap<SettingsDto, AppSiteSetting>().ForMember(p => p.Language, opt => opt.MapFrom(MapUserDtoLanguage));
        }

        //private static Country MapUserDtoCountry(UserDto userDto)
        //{
        //    Country countryInfo = new Country();
        //    countryInfo.ID = userDto.Country;
        //    return countryInfo;
        //}
        #endregion
        #region UserMapping

        private static void RegisterUserMapping()
        {
            Mapper.CreateMap<UserDto, User>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(MapUserDtoCountry))
                  .ForMember(p => p.Language, opt => opt.MapFrom(MapUserDtoLanguage))
             .ForMember(p => p.RegistredIP, opt => opt.MapFrom(x => !string.IsNullOrEmpty(x.IPAddress) ? Encoding.ASCII.GetBytes(x.IPAddress) : null));


            Mapper.CreateMap<AccountDSPRequest, UserDto>()
                                  .ForMember(p => p.Country, opt => opt.MapFrom(MapUserCountry));


            Mapper.CreateMap<User, UserDto>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(MapUserCountry))
                  .ForMember(p => p.Language, opt => opt.MapFrom(MapUserLanguage))
                  //.ForMember(p => p.UserAgreementVersion, opt => opt.MapFrom(x => x.Account.UserAgreementVersion))
                  //.ForMember(p => p.AllowAPIAccess, opt => opt.MapFrom(x => x.Account.AllowAPIAccess))
                  //.ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account.GetName()))
                  .ForMember(p => p.IPAddress, opt => opt.MapFrom(x => x.RegistredIP != null && x.RegistredIP.Count() > 0 ? Encoding.ASCII.GetString(x.RegistredIP) : ""));
            Mapper.CreateMap<Noqoush.AdFalcon.Domain.Model.Account.Account, UserDto>()
                .ForMember(p => p.Country, opt => opt.MapFrom(MapAccountUserCountry))
                .ForMember(p => p.Language, opt => opt.MapFrom(MapUserAccountLanguage))
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


            Mapper.CreateMap<AccountDSPRequestDto, AccountDSPRequest>()
           .ForMember(p => p.Country, opt => opt.MapFrom(MapUserDtoCountry))
                 .ForMember(p => p.CompanyType, opt => opt.MapFrom(MapUserDtoCompanyType));

            Mapper.CreateMap<AccountDSPRequest, AccountDSPRequestDto>()
                  .ForMember(p => p.Country, opt => opt.MapFrom(MapUserCountry))
                     .ForMember(p => p.CompanyType, opt => opt.MapFrom(MapUserCompanyType))

                  .ForMember(p => p.AccountName, opt => opt.MapFrom(x => x.Account != null ? x.Account.GetName() : string.Empty));

            Mapper.CreateMap<AccountInvitation, InvitationDto>()
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

        private static void RegisterTextFilterToTextFilterDtoMapping()
        {
            Mapper.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.MatchTypeId,
                                                                    opt => opt.MapFrom(p => p.MatchType.ID));

            Mapper.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.MatchTypeText,
                                                                    opt => opt.MapFrom(p => p.MatchType.Name));

            Mapper.CreateMap<TextFilter, TextFilterDto>().ForMember(p => p.TextFilterId,
                                                                 opt => opt.MapFrom(p => p.ID));
        }

        private static void RegisterTextFilterDtoToTextFilterMapping()
        {
            Mapper.CreateMap<TextFilterDto, TextFilter>().ForMember(p => p.MatchType,
                                                        opt => opt.MapFrom(MapMatchType));

        }

        private static void RegisterUrlFilterToUrlFilterDtoMapping()
        {
            Mapper.CreateMap<UrlFilter, UrlFilterDto>().ForMember(p => p.UrlFilterId, p => p.MapFrom(x => x.ID));
        }

        private static void RegisterUrlFilterDtoToUrlFilterMapping()
        {
            Mapper.CreateMap<UrlFilterDto, UrlFilter>().ForMember(p => p.ID, p => p.MapFrom(x => x.UrlFilterId));
        }

        public static MatchType MapMatchType(TextFilterDto textFilterDto)
        {
            MatchType matchType = new MatchType();
            matchType.ID = textFilterDto.MatchTypeId;

            return matchType;
        }

        private static void RegisterlanguageFilterDtoToLanguageFilterMapping()
        {
            Mapper.CreateMap<LanguageFilterDto, LanguageFilter>().ForMember(p => p.Language,
                                                        opt => opt.MapFrom(MapLanguage));

            Mapper.CreateMap<LanguageFilterDto, LanguageFilter>().ForMember(p => p.ID,
                                                 opt => opt.MapFrom(p => p.languageFilterId));
        }

        private static void RegisterlanguageFilterToLanguageFilterDtoMapping()
        {
            Mapper.CreateMap<LanguageFilter, LanguageFilterDto>().ForMember(p => p.languageFilterId,
                                            opt => opt.MapFrom(p => p.ID));
        }

        private static void RegisterLanguageFilterToLanguageFilterDtoMapping()
        {
            throw new NotImplementedException();
        }

        public static Language MapLanguage(LanguageFilterDto languageDto)
        {
            return languageRepository.Get(languageDto.LanguageId);
        }

        #endregion

        private static void RegisterSSPDtoDtoMapping()
        {
            Mapper.CreateMap<BusinessPartner, PartnerDto>();

            Mapper.CreateMap<PartnerDto, BusinessPartner>()
                     .ForMember(p => p.Type, opt => opt.Ignore());

            Mapper.CreateMap<PartnerSite, PartnerSiteDto>().ForMember(dst => dst.PartnerID, opt => opt.MapFrom(z => z.Partner != null ? z.Partner.ID : 0));


            Mapper.CreateMap<PartnerSiteDto, PartnerSite>().ForMember(p => p.Partner, x => x.MapFrom(z =>
                   {
                       if (z.PartnerID > 0)
                       {

                           BusinessPartner acc = new BusinessPartner() { ID = z.PartnerID };
                           return acc;
                       }
                       return null;
                   }));
            // AdFalconCampaignId


            Mapper.CreateMap<DealCampaignMapping, DealCampaignMappingDto>().ForMember(dst => dst.PartnerID, opt => opt.MapFrom(z => z.Partner != null ? z.Partner.ID : 0))
                .ForMember(dst => dst.AdFalconCampaignId, opt => opt.MapFrom(z => z.Campaign != null ? z.Campaign.ID : 0))
                    .ForMember(dst => dst.CampaignName, opt => opt.MapFrom(z => z.Campaign != null ? z.Campaign.Name : ""));


            Mapper.CreateMap<DealCampaignMappingDto, DealCampaignMapping>().ForMember(p => p.Partner, x => x.MapFrom(z =>
            {
                if (z.PartnerID > 0)
                {

                    BusinessPartner acc = new BusinessPartner() { ID = z.PartnerID };
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


            Mapper.CreateMap<SiteZone, SiteZoneDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0));


            Mapper.CreateMap<SiteZoneDto, SiteZone>().ForMember(p => p.Site, x => x.MapFrom(z =>
            {
                if (z.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = z.SiteID };
                    return acc;
                }
                return null;
            }));



            Mapper.CreateMap<FloorPrice, FloorPriceConfigDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0))
            .ForMember(dst => dst.ZoneID, opt => opt.MapFrom(z => z.Zone != null ? z.Zone.ID : 0))
                   .ForMember(dst => dst.TargetingId, opt => opt.MapFrom(z => z.TargetingId != null ? z.TargetingId : -1));



            Mapper.CreateMap<FloorPriceConfigDto, FloorPrice>().ForMember(p => p.Site, x => x.MapFrom(z =>
            {
                if (z.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = z.SiteID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.Zone, x => x.MapFrom(z =>
            {
                if (z.ZoneID > 0)
                {

                    SiteZone acc = new SiteZone() { ID = z.ZoneID };
                    return acc;
                }
                return null;
            }));


            Mapper.CreateMap<SiteZoneMapping, SiteZoneMappingDto>().ForMember(dst => dst.SiteID, opt => opt.MapFrom(z => z.Site != null ? z.Site.ID : 0))
         .ForMember(dst => dst.ZoneID, opt => opt.MapFrom(z => z.Zone != null ? z.Zone.ID : 0))
         .ForMember(dst => dst.AdTypeID, opt => opt.MapFrom(z => z.AdType != null ? z.AdType.ID : 0))
         .ForMember(dst => dst.DeviceTypeID, opt => opt.MapFrom(z => z.DeviceType != null ? z.DeviceType.ID : 0))
            .ForMember(dst => dst.AppSiteID, opt => opt.MapFrom(z => z.AppSite != null ? z.AppSite.ID : 0))
                  .ForMember(dst => dst.AdTypeString, opt => opt.MapFrom(z => z.AdType != null ? z.AdType.Name.GetValue() : ""))
         .ForMember(dst => dst.DeviceTypeString, opt => opt.MapFrom(z => z.DeviceType != null ? z.DeviceType.Name.GetValue() : ""))

          .ForMember(dst => dst.AppSiteString, opt => opt.MapFrom(z => z.AppSite != null ? z.AppSite.Name : ""))
         ;


            Mapper.CreateMap<SiteZoneMappingDto, SiteZoneMapping>().ForMember(p => p.Site, x => x.MapFrom(z =>
            {
                if (z.SiteID > 0)
                {

                    PartnerSite acc = new PartnerSite() { ID = z.SiteID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.Zone, x => x.MapFrom(z =>
            {
                if (z.ZoneID > 0)
                {

                    SiteZone acc = new SiteZone() { ID = z.ZoneID };
                    return acc;
                }
                return null;
            }))

            .ForMember(p => p.AdType, x => x.MapFrom(z =>
            {
                if (z.AdTypeID > 0)
                {

                    AdType acc = new AdType() { ID = z.AdTypeID };
                    return acc;
                }
                return null;
            }))
            .ForMember(p => p.DeviceType, x => x.MapFrom(z =>
            {
                if (z.DeviceTypeID > 0)
                {

                    DeviceType acc = new DeviceType() { ID = z.DeviceTypeID };
                    return acc;
                }
                return null;
            }))



            ;







        }

        private static void RegisterPMPDealDtoDtoMapping()
        {


            Mapper.CreateMap<PMPDeal, PMPDealDto>().ForMember(dst => dst.AccountId, opt => opt.MapFrom(z => z.Account != null ? z.Account.ID : 0))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(z => z.User != null ? z.User.ID : 0))

                    .ForMember(dst => dst.AdvertiserId, opt => opt.MapFrom(z => z.Advertiser != null ? z.Advertiser.ID : 0))
                  .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(item => item.Advertiser == null ? string.Empty : item.Advertiser.Name.ToString()))

                    .ForMember(dst => dst.AdvertiserAccountId, opt => opt.MapFrom(z => z.AdvertiserAccount != null ? z.AdvertiserAccount.ID : 0))
                  .ForMember(dest => dest.AdvertiserAccountName, opt => opt.MapFrom(item => item.AdvertiserAccount == null ? string.Empty : item.AdvertiserAccount.Name.ToString()))


                  .ForMember(dest => dest.ExchangeName, opt => opt.MapFrom(item => item.Exchange == null ? string.Empty : item.Exchange.Name.ToString()))

                  .ForMember(dst => dst.ExchangeId, opt => opt.MapFrom(z => z.Exchange != null ? z.Exchange.ID : 0));


            Mapper.CreateMap<PMPDealDto, PMPDeal>().ForMember(p => p.Account, x => x.MapFrom(z =>
            {
                if (z.AccountId > 0)
                {

                    Account acc = new Account() { ID = z.AccountId };
                    return acc;
                }
                return null;
            }))

            .ForMember(p => p.User, x => x.MapFrom(z =>
            {
                if (z.UserId > 0)
                {

                    User acc = new User() { ID = z.UserId };
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

            .ForMember(p => p.Exchange, x => x.MapFrom(z =>
            {
                if (z.ExchangeId > 0)
                {

                    SSPPartner acc = new SSPPartner() { ID = z.ExchangeId };
                    return acc;
                }
                return null;
            }))
             .ForMember(p => p.Advertiser, x => x.MapFrom(z =>
             {
                 if (z.AdvertiserId > 0)
                 {

                     Advertiser adv = new Advertiser() { ID = z.AdvertiserId };
                     return adv;
                 }
                 return null;
             }))

             .ForMember(p => p.AdvertiserAccount, x => x.MapFrom(z =>
             {
                 if (z.AdvertiserAccountId > 0)
                 {

                     AdvertiserAccount adv = new AdvertiserAccount() { ID = z.AdvertiserAccountId };
                     return adv;
                 }
                 return null;
             }))



            ;
            // AdFalconCampaignId






        }
        private static void RegisteImpressionLogDtoMapping()
        {
            Mapper.CreateMap<ImpressionLog, ImpressionLogDto>()
           .ForMember(dst => dst.Day, opt => opt.MapFrom(z => DateTime.ParseExact(z.Day.ToString(), "yyyyMMdd", null)));

            Mapper.CreateMap<ImpressionLogDto, ImpressionLog>()
            .ForMember(dst => dst.Day, opt => opt.MapFrom(z => Convert.ToInt32(z.Day.ToString("yyyyMMdd"))));

        }


        private static void RegistermetriceGroupColumnDtoMapping()
        {
            Mapper.CreateMap<metriceColumn, metriceColumnDto>();
            //.ForMember(dst => dst.Header, opt => opt.MapFrom(z => ResourceManager.Instance.GetResource(z.HeaderResourceKey, z.HeaderResourceSet)));


            Mapper.CreateMap<metriceColumnDto, metriceColumn>();


            Mapper.CreateMap<metriceGroup, metriceGroupDto>();
            Mapper.CreateMap<metriceGroupDto, metriceGroup>();

            Mapper.CreateMap<metriceGroupColumn, metriceGroupColumnDto>();
            Mapper.CreateMap<metriceGroupColumnDto, metriceGroupColumn>();

        }
    }
}
