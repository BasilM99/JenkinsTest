using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    [KnownType(typeof(DeviceDto))]
    [KnownType(typeof(CurrencyDto))]
    [KnownType(typeof(ManufacturerDto))]
    [KnownType(typeof(PlatformDto))]
    [KnownType(typeof(KeywordDto))]
    [KnownType(typeof(KeywordSaveDto))]
    [KnownType(typeof(LocationDto))]
    [KnownType(typeof(OperatorDto))]
    [KnownType(typeof(DeviceCapabilityDto))]
    [KnownType(typeof(CostElementDto))]
    [KnownType(typeof(FeeDto))]
    [KnownType(typeof(JobPositionDto))]
    [KnownType(typeof(AdCreativeAttributeDto))]
    [KnownType(typeof(CostModelWrapperDto))]
    [KnownType(typeof(CostModelDto))]
    [KnownType(typeof(AdvertiserDto))]
    [KnownType(typeof(CreativeVendorDto))]

    [KnownType(typeof(LanguageDto))]
    [KnownType(typeof(LanguageSaveDto))]
    public class LookupDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public LocalizedStringDto Name { get; set; }

        [DataMember]
        public string CustomName { get; set; }
    }

    [DataContract]
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

        [DataMember]
        public int ID { get; set; }
        
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string GroupKey { get; set; }

        [DataMember]
        public IList<LocalizedValueDto> Values
        {
            get { return values ?? (values = new List<LocalizedValueDto>()); }
            set { values = value; }
        }
        public virtual LocalizedValueDto SetValue(string value, string CultureCode)
        {
            if (ContainsKey(CultureCode))
            {
                if (string.IsNullOrEmpty(value))
                {
                    //TODO:OSAleh fix this
                    throw new NotImplementedException();
                }
                else
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
        [DataMember]
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

    [DataContract]
    public class LocalizedValueDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string Culture { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public string Value { get; set; }
    }
}
