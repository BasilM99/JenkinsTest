using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    [Serializable]
    [ProtoInclude(100,typeof(DeviceDto))]
    [ProtoInclude(101,typeof(CurrencyDto))]
    [ProtoInclude(102,typeof(ManufacturerDto))]
    [ProtoInclude(103,typeof(PlatformDto))]
    [ProtoInclude(104,typeof(KeywordDto))]
    [ProtoInclude(105,typeof(KeywordSaveDto))]
    [ProtoInclude(106,typeof(LocationDto))]
    [ProtoInclude(107,typeof(OperatorDto))]
    [ProtoInclude(108,typeof(DeviceCapabilityDto))]
    [ProtoInclude(109,typeof(CostElementDto))]
    [ProtoInclude(110,typeof(FeeDto))]
    [ProtoInclude(111,typeof(JobPositionDto))]
    [ProtoInclude(112,typeof(AdCreativeAttributeDto))]
    [ProtoInclude(113,typeof(CostModelWrapperDto))]
    [ProtoInclude(114,typeof(CostModelDto))]
    [ProtoInclude(115,typeof(AdvertiserDto))]
    [ProtoInclude(116,typeof(CreativeVendorDto))]
    [ProtoInclude(117,typeof(LanguageDto))]
    [ProtoInclude(118,typeof(LanguageSaveDto))]
    [ProtoInclude(119,typeof(AccountFundTransStatusDto))]
    [ProtoInclude(120,typeof(AccountFundTransTypeDto))]
    [ProtoInclude(121,typeof(AdPermissionDto))]
    [ProtoInclude(122,typeof(AdSubtypeDto))]
    [ProtoInclude(123, typeof(AppSiteStatusDto))]
    [ProtoInclude(124, typeof(AdTypeDto))]
    [ProtoInclude(125, typeof(CreativeUnitDto))]
    [ProtoInclude(126, typeof(CreativeUnitGroupDto))]
    [ProtoInclude(127, typeof(FormatDto))]
    [ProtoInclude(128, typeof(RichMediaRequiredProtocolDto))]
 
    [ProtoInclude(129, typeof(TileImageDocumentDto))]
    [ProtoInclude(130, typeof(TileImageSizeDto))]
    [ProtoInclude(131, typeof(AdActionTypeConstraintDto))]
    [ProtoInclude(132, typeof(AdActionTypeDto))]
    [ProtoInclude(133, typeof(ObjectiveTypeDto))]
    [ProtoInclude(134, typeof(AdRequestPlatformDto))]
    [ProtoInclude(135, typeof(AdRequestTypeDto))]
    [ProtoInclude(136, typeof(DeviceTargetingTypeDto))]
    [ProtoInclude(137, typeof(TargetingTypeDto))]
    [ProtoInclude(138, typeof(AppMarketingPartnerDto))]
    [ProtoInclude(139, typeof(CreativeFormatsDto))]
    [ProtoInclude(140, typeof(CreativeVendorDto))]
    [ProtoInclude(141, typeof(MetricVendorDto))]
    [ProtoInclude(142, typeof(AgeGroupDto))]
    [ProtoInclude(143, typeof(AudienceSegmentDto))]
    [ProtoInclude(144, typeof(CompanyTypeDto))]
    //[ProtoInclude(145, typeof(FeeDto))]
    [ProtoInclude(146, typeof(CountryDto))]
    [ProtoInclude(147, typeof(DeviceTypeDto))]
    [ProtoInclude(148, typeof(DocumentTypeDto))]
    [ProtoInclude(149, typeof(GenderDto))]
    [ProtoInclude(150, typeof(ImpressionMetricDto))]
    [ProtoInclude(151, typeof(JobPositionDto))]
    [ProtoInclude(152, typeof(MetricDto))]
    [ProtoInclude(153, typeof(VideoDeliveryMethodDto))]
    [ProtoInclude(154, typeof(VideoTypeDto))]
    [ProtoInclude(155, typeof(metriceGroupDto))]
    [ProtoInclude(156, typeof(TileImageDto))]
    [ProtoInclude(157, typeof(AppMarketingPartnerDto))]
    [ProtoInclude(158, typeof(AccountFundTypeDto))]
    [ProtoInclude(159, typeof(PaymentTypeDto))]
    [ProtoInclude(160, typeof(MetricResultDto))]
    [ProtoInclude(161, typeof(KPIConfigDto))]

 
    public class LookupDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public LocalizedStringDto Name { get; set; }

       [ProtoMember(3)]
        public string CustomName { get; set; }
    }

    [ProtoContract]
    [Serializable]
    public class LocalizedStringDto
    {
        IList<LocalizedValueDto> values = new List<LocalizedValueDto>();
        private static List<string> _Languages;

        public LocalizedStringDto()
        {
        }

         static LocalizedStringDto()
        {
            _Languages = new List<string>();
            _Languages.Add("en-US");
            _Languages.Add("ar-JO");
        }

        public static LocalizedStringDto ConvertToLocalizedStringDto(string name)
        {
            LocalizedStringDto localized = new LocalizedStringDto();

            int i=0;
            foreach (var item in LocalizedStringDto._Languages)
            {
                LocalizedValueDto value = new LocalizedValueDto();
                value.ID = i;
                value.Culture = item;
                value.Value = name;
                localized.Values.Add(value);
            }

            return localized;
        }

       [ProtoMember(1)]
        public int ID { get; set; }
        
       [ProtoMember(2)]
        [Required(ResourceName = "RequiredMessage")]
        public string GroupKey { get; set; }

       [ProtoMember(3)]
        public IList<LocalizedValueDto> Values
        {
            get { return values ?? (values = new List<LocalizedValueDto>()); }
            set { values = value; }
        }
        public virtual LocalizedValueDto SetValue(string value, string CultureCode)
        {
            if (ContainsKey(CultureCode))
            {
                //if (string.IsNullOrEmpty(value))
                //{
                //    //TODO:OSAleh fix this
                //    throw new NotImplementedException();
                //}
                if(!string.IsNullOrEmpty(value))
                {
                    //throw new NotImplementedException();
                    Values.First(localizedValue => localizedValue.Culture.Equals(CultureCode, StringComparison.OrdinalIgnoreCase))
                        .Value = value.Trim();
                }
            }
            else if (!string.IsNullOrEmpty(value))
            {
                Values.Add(getLocalizedValue(CultureCode, value));
            }
            return null;
        }

        protected LocalizedValueDto getLocalizedValue(string CultureCode, string value)
        {
            return new LocalizedValueDto { Culture = CultureCode, Value = value };
        }
        public LocalizedValueDto SetValue(string value)
        {
            return SetValue(value, Thread.CurrentThread.CurrentUICulture.Name);
        }
        public bool ContainsKey(string Key)
        {
            //TODO:Osaleh Check if the dic is faster
            return Values.Any(localizedValue => localizedValue.Culture.Equals(Key, StringComparison.OrdinalIgnoreCase));
        }
        public string Get(string Key)
        {

            if (ContainsKey(Key))
                return Values.First(localizedValue => localizedValue.Culture.Equals(Key, StringComparison.OrdinalIgnoreCase)).Value;
            else
            {
                return string.Empty;
            }
        }
        public string GetValue(string CultureCode)
        {
            if (ContainsKey(CultureCode))
                return Get(CultureCode);
            return string.Empty;
        }
        public string GetValue()
        {
            return GetValue(Thread.CurrentThread.CurrentUICulture.Name);
        }
       [ProtoMember(4)]
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(DefaultCulture))
                return GetValue();
                else
                    return GetValue(DefaultCulture);
            }
            set
            {
                SetValue(value);
            }
        }

        //[DataMember]
        public string DefaultCulture
        {
            get;
            set;
        }
        public static LocalizedStringDto GetDefault()
        {
           return new LocalizedStringDto
            {
                Values = new List<LocalizedValueDto>
                                                 {
                                                     new LocalizedValueDto() {Culture = "en-US", Value = string.Empty},
                                                     new LocalizedValueDto() {Culture = "ar-JO", Value = string.Empty}
                                                 }
            };
        }
        public override string ToString()
        {
            return Value;
        }
        public static implicit operator string(LocalizedStringDto localizedStringDto)
        {
            return localizedStringDto.Value;
        }
    }

    [ProtoContract]
    [Serializable]
    public class LocalizedValueDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        [Required(ResourceName = "RequiredMessage")]
        public string Culture { get; set; }

       [ProtoMember(3)]
        [Required(ResourceName = "RequiredMessage")]
        public string Value { get; set; }
    }
}
