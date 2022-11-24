using System;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Persistence.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core.Video;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;
using NHibernate;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.Framework.Caching;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class LookupService : ILookupService
    {
        private IDeviceTypeRepository deviceTypeRepository;
        private IDeviceCodeRepository deviceCodeRepository;

        private ILookupRepository lookupRepository;
        private ICreativeVendorKeywordRepository CreativeVendorKeywordRepository;
        private ILanguageRepository LanguageRepository;
        private ICreativeFormatsRepository _creativeFormatsRepository;
        public LookupService(IDeviceTypeRepository deviceTypeRepository, ILookupRepository lookupRepository, ICreativeVendorKeywordRepository creativeVendorKeywordRepository, ILanguageRepository LanguageRepository, ICreativeFormatsRepository CreativeFormatsRepository,IDeviceCodeRepository DeviceCodeRepository)
        {
            this.deviceTypeRepository = deviceTypeRepository;
            this.lookupRepository = lookupRepository;
            CreativeVendorKeywordRepository = creativeVendorKeywordRepository;
            this.LanguageRepository = LanguageRepository;
            _creativeFormatsRepository = CreativeFormatsRepository;
            _creativeFormatsRepository = CreativeFormatsRepository;
            deviceCodeRepository=DeviceCodeRepository;

        }

        #region Helpers
        private TRepository GetRepository<TRepository>(string lookupName)
            where TRepository : class
        {
            //return LookupEntries.FindLookupRepository<TRepository>();
            return IoC.Instance.Resolve<TRepository>(); ;
        }
        private void SaveEntity<TEntity>(LookupDto data, IKeyedRepository<TEntity, int> repository)
           where TEntity : ManagedLookupBase, new()
        {

            ValidateEntity(data, repository);
            var item = repository.Get(data.ID);
            if (item != null)
            {
                //update
                item.Name.GroupKey = data.Name.GroupKey;
                foreach (var localizedValueDto in data.Name.Values)
                {
                    item.Name.SetValue(localizedValueDto.Value, localizedValueDto.Culture);
                }
                UpdateItem(item, data, repository);
            }
            else
            {
                //Insert
                item = MapperHelper.Map<TEntity>(data);
                var Advertiser = item as Advertiser;

                if (Advertiser != null)
                {
                    Advertiser.UniqueId = Guid.NewGuid().ToString();
                }

                var CostItemV = item as CostItem;
                if (CostItemV != null && CostItemV.CostItemType == CostItemType.CostElement)
                {
                    var CostElementb = item as CostElement;
                    CostElementb.CostItemType = CostItemType.CostElement;
                    CalculateCostElementCategory(CostElementb, data as CostElementDto);


                }



                if (CostItemV != null && CostItemV.CostItemType == CostItemType.Fee)
                {
                    var CostElementF = item as Fee;
                    CostElementF.CostItemType = CostItemType.Fee;
                    CalculateFeeCategory(CostElementF, data as FeeDto);
                }
                var location = item as LocationBase;

                if (location != null)
                {
                    location.CodeAlpha2 = location.TwoLettersCode;
                    if ((data as LocationDto).ParentId.HasValue)
                    {
                        location.Parent = repository.Get((data as LocationDto).ParentId.Value) as LocationBase;
                    }

                    switch (location.Type)
                    {
                        case (int)LocationType.Continent:
                            {
                                var continent = MapperHelper.Map<Continent>(location);
                                item = continent as TEntity;
                            }
                            break;
                        case (int)LocationType.Country:
                            {
                                var country = MapperHelper.Map<Country>(location);
                                item = country as TEntity;
                            }
                            break;
                        case (int)LocationType.State:
                            var state = MapperHelper.Map<State>(location);
                            item = state as TEntity;
                            break;
                        case (int)LocationType.City:
                            var city = MapperHelper.Map<City>(location);
                            item = city as TEntity;
                            break;
                        default:
                            break;
                    }

                }

                var CreativeVendor = item as CreativeVendor;
                if (CreativeVendor != null)
                {
                    (item as CreativeVendor).Keywords = new List<CreativeVendorKeyword>();
                    foreach (CreativeVendorKeywordDto keyWord in (data as CreativeVendorDto).InsertedKeywords)
                    {
                        var keyWordItem = MapperHelper.Map<CreativeVendorKeyword>(keyWord);
                        keyWordItem.Vendor = (item as CreativeVendor);
                        (item as CreativeVendor).Keywords.Add(keyWordItem);
                    }

                    if (CreativeVendor.ID == 0)
                    {
                        CreativeVendor.Code = GeVendorCounter();

                    }
                }

            }
            foreach (var localizedValue in item.Name.Values)
            {
                if (!string.IsNullOrEmpty(item.Name.Value))
                {
                    item.Name.Value = item.Name.Value.Trim();
                }
                localizedValue.LocalizedString = item.Name;
            }

            // update device
            var device = item as Device;
            if (device != null)
            {
                (item as Device).DeviceType = deviceTypeRepository.Get((data as DeviceDto).DeviceTypeId);

                if ((item as Device).Codes == null)
                {
                    (item as Device).Codes = new List<DeviceCode>();
                }

                IList<string> Codes = (data as DeviceDto).Code.Split(",");
                foreach (var code in Codes)
                {
                    if ((item as Device).Codes.Where(M => M.Code == code).SingleOrDefault() == null)
                    {
                        if (deviceCodeRepository.Query(M => M.Code == code && M.Device.ID != item.ID).FirstOrDefault() == null)
                            (item as Device).Codes.Add(new DeviceCode { Device = (item as Device), Code = code });
                        else
                        {
                            var error = new BusinessException();
                            error.Errors.Add(new ErrorData { ID = "DeviceDuplicateCode" });
                            throw error;
                        }


                    }


                }
             


            }



            repository.Save(item);
        }
        private int GeVendorCounter()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "CreativeVendor");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        public void CalculateCostElementCategory(CostElement costElem, CostElementDto dto)
        {

            CostItemCategroyFlags catEnum = CostItemCategroyFlags.Undefined;

            CostItemCategroyFlags catFeeEnum = CostItemCategroyFlags.Undefined;

            if (dto.IsData)
            {
                catEnum = CostItemCategroyFlags.Data | catEnum;
            }
            if (dto.IsThirdParty)
            {
                catEnum = CostItemCategroyFlags.ThirdParty | catEnum;


            }
            if (dto.IsAVR)
            {
                catEnum = CostItemCategroyFlags.AVR | catEnum;


            }
            if (dto.IsPlatform)
            {
                catEnum = CostItemCategroyFlags.Platform | catEnum;


            }
            if (dto.IsExchangeDiscrepancy)
            {
                catEnum = CostItemCategroyFlags.ExchangeDiscrepancy | catEnum;


            }
            if (dto.IsAdfalconRevenue)
            {
                catEnum = CostItemCategroyFlags.AdfalconRevenue | catEnum;


            }


            if (dto.IsDataFee)
            {
                catFeeEnum = CostItemCategroyFlags.Data | catFeeEnum;


            }
            if (dto.IsThirdPartyFee)
            {
                catFeeEnum = CostItemCategroyFlags.ThirdParty | catFeeEnum;


            }
            if (dto.IsAVRFee)
            {
                catFeeEnum = CostItemCategroyFlags.AVR | catFeeEnum;


            }
            if (dto.IsPlatformFee)
            {
                catFeeEnum = CostItemCategroyFlags.Platform | catFeeEnum;


            }
            if (dto.IsExchangeDiscrepancyFee)
            {
                catFeeEnum = CostItemCategroyFlags.ExchangeDiscrepancy | catFeeEnum;


            }

            if (dto.IsAdfalconRevenueFee)
            {
                catFeeEnum = CostItemCategroyFlags.AdfalconRevenue | catFeeEnum;


            }
            dto.Category = Convert.ToInt64(catEnum);
            dto.CalculatedFromFeeCategory = Convert.ToInt64(catFeeEnum);

            costElem.Category = dto.Category;
            costElem.CalculatedFromFeeCategory = dto.CalculatedFromFeeCategory;
        }

        public void RefereseCalculateCostElementCategory(CostElement costElem, CostElementDto dto)
        {
            CostItemCategroyFlags catEnum = (CostItemCategroyFlags)dto.Category;



            CostItemCategroyFlags catFeeEnum = (CostItemCategroyFlags)dto.CalculatedFromFeeCategory;

            dto.IsData = catEnum.HasFlag(CostItemCategroyFlags.Data);

            dto.IsThirdParty = catEnum.HasFlag(CostItemCategroyFlags.ThirdParty);

            dto.IsAVR = catEnum.HasFlag(CostItemCategroyFlags.AVR);

            dto.IsExchangeDiscrepancy = catEnum.HasFlag(CostItemCategroyFlags.ExchangeDiscrepancy);

            dto.IsAdfalconRevenue = catEnum.HasFlag(CostItemCategroyFlags.AdfalconRevenue);

            dto.IsPlatform = catEnum.HasFlag(CostItemCategroyFlags.Platform);


            dto.IsDataFee = catFeeEnum.HasFlag(CostItemCategroyFlags.Data);

            dto.IsThirdPartyFee = catFeeEnum.HasFlag(CostItemCategroyFlags.ThirdParty);
            dto.IsPlatformFee = catFeeEnum.HasFlag(CostItemCategroyFlags.Platform);
            dto.IsAVRFee = catFeeEnum.HasFlag(CostItemCategroyFlags.AVR);
            dto.IsAdfalconRevenueFee = catFeeEnum.HasFlag(CostItemCategroyFlags.AdfalconRevenue);
            dto.IsExchangeDiscrepancyFee = catFeeEnum.HasFlag(CostItemCategroyFlags.ExchangeDiscrepancy);
            dto.Category = costElem.Category;
            dto.CalculatedFromFeeCategory = costElem.CalculatedFromFeeCategory;

        }


        public void CalculateFeeCategory(Fee costElem, FeeDto dto)
        {

            CostItemCategroyFlags catEnum = CostItemCategroyFlags.Undefined;

            CostItemCategroyFlags catFeeEnum = CostItemCategroyFlags.Undefined;

            if (dto.IsData)
            {
                catEnum = CostItemCategroyFlags.Data | catEnum;


            }
            if (dto.IsThirdParty)
            {
                catEnum = CostItemCategroyFlags.ThirdParty | catEnum;


            }
            if (dto.IsAVR)
            {
                catEnum = CostItemCategroyFlags.AVR | catEnum;


            }
            if (dto.IsPlatform)
            {
                catEnum = CostItemCategroyFlags.Platform | catEnum;


            }
            if (dto.IsExchangeDiscrepancy)
            {
                catEnum = CostItemCategroyFlags.ExchangeDiscrepancy | catEnum;


            }

            if (dto.IsAdfalconRevenue)
            {
                catEnum = CostItemCategroyFlags.AdfalconRevenue | catEnum;


            }
            dto.Category = Convert.ToInt64(catEnum);
            //dto.CalculatedFromFeeCategory = Convert.ToInt64(catFeeEnum);

            costElem.Category = dto.Category;
            //  costElem.CalculatedFromFeeCategory = dto.CalculatedFromFeeCategory;
        }

        public void RefereseCalculateFeeCategory(Fee costElem, FeeDto dto)
        {
            CostItemCategroyFlags catEnum = (CostItemCategroyFlags)dto.Category;





            dto.IsData = catEnum.HasFlag(CostItemCategroyFlags.Data);

            dto.IsThirdParty = catEnum.HasFlag(CostItemCategroyFlags.ThirdParty);

            dto.IsAVR = catEnum.HasFlag(CostItemCategroyFlags.AVR);
            dto.IsPlatform = catEnum.HasFlag(CostItemCategroyFlags.Platform);
            dto.IsExchangeDiscrepancy = catEnum.HasFlag(CostItemCategroyFlags.ExchangeDiscrepancy);

            dto.IsAdfalconRevenue = catEnum.HasFlag(CostItemCategroyFlags.AdfalconRevenue);
            dto.Category = costElem.Category;


        }
        private void ValidateEntity<TEntity>(LookupDto data, IKeyedRepository<TEntity, int> repository)
            where TEntity : ManagedLookupBase, new()
        {
            if (data is KeywordSaveDto)
            {
                var keywordWithSameCode = (repository as IKeyWordRepository).Query(p => p.Code == (data as KeywordSaveDto).Code).ToList();
                var keywordWithTheSameName = (repository as IKeyWordRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as KeywordSaveDto).Name.GetValue("en-US")))).ToList();

                if ((keywordWithSameCode.Count != 0 && data.ID == 0) || (keywordWithSameCode.Count == 1 && data.ID != 0 && keywordWithSameCode.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "KeywordDuplicateCode" });
                    throw error;
                }

                if ((keywordWithTheSameName.Count != 0 && data.ID == 0) || (keywordWithTheSameName.Count == 1 && data.ID != 0 && keywordWithTheSameName.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DuplicatedName" });
                    throw error;
                }

                keywordWithTheSameName = (repository as IKeyWordRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as KeywordSaveDto).Name.GetValue("ar-JO")))).ToList();

                if ((keywordWithTheSameName.Count != 0 && data.ID == 0) || (keywordWithTheSameName.Count == 1 && data.ID != 0 && keywordWithTheSameName.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DuplicatedName" });
                    throw error;
                }
            }
            if (data is LanguageSaveDto)
            {
                var LangWithSameCode = (repository as ILanguageRepository).Query(p => p.Code == (data as LanguageSaveDto).Code).ToList();
                var LangWithTheSameName = (repository as ILanguageRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as LanguageSaveDto).Name.GetValue("en-US")))).ToList();

                if ((LangWithSameCode.Count != 0 && data.ID == 0) || (LangWithSameCode.Count == 1 && data.ID != 0 && LangWithSameCode.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "CodeDuplicated" });
                    throw error;
                }

                if ((LangWithTheSameName.Count != 0 && data.ID == 0) || (LangWithTheSameName.Count == 1 && data.ID != 0 && LangWithTheSameName.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DuplicatedName" });
                    throw error;
                }

                LangWithTheSameName = (repository as ILanguageRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as LanguageSaveDto).Name.GetValue("ar-JO")))).ToList();

                if ((LangWithTheSameName.Count != 0 && data.ID == 0) || (LangWithTheSameName.Count == 1 && data.ID != 0 && LangWithTheSameName.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "DuplicatedName" });
                    throw error;
                }
            }

            if (data is AdvertiserDto)
            {

                var AdvertiserWithSameCode = (repository as IAdvertiserRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as AdvertiserDto).Name.GetValue("en-US")))).ToList();

                if ((AdvertiserWithSameCode.Count != 0 && data.ID == 0) || (AdvertiserWithSameCode.Count == 1 && data.ID != 0 && AdvertiserWithSameCode.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "AdvertiserDuplicateCode" });
                    throw error;
                }
                AdvertiserWithSameCode = (repository as IAdvertiserRepository).Query(x => x.Name.Values.Any(v => v.Value.Equals((data as AdvertiserDto).Name.GetValue("ar-JO")))).ToList();

                if ((AdvertiserWithSameCode.Count != 0 && data.ID == 0) || (AdvertiserWithSameCode.Count == 1 && data.ID != 0 && AdvertiserWithSameCode.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "AdvertiserDuplicateCode" });
                    throw error;
                }


                AdvertiserWithSameCode = (repository as IAdvertiserRepository).Query(z => z.DomainURL.Contains((data as AdvertiserDto).DomainURL)).Where(z => z.DomainURL.Equals((data as AdvertiserDto).DomainURL, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if ((AdvertiserWithSameCode.Count != 0 && data.ID == 0) || (AdvertiserWithSameCode.Count == 1 && data.ID != 0 && AdvertiserWithSameCode.Single().ID != data.ID))
                {
                    var error = new BusinessException();
                    error.Errors.Add(new ErrorData { ID = "AdvertiserDuplicateCode" });
                    throw error;
                }
            }
        }

        private void UpdateItem<TEntity>(ManagedLookupBase item, LookupDto itemDto, IKeyedRepository<TEntity, int> repository)
              where TEntity : ManagedLookupBase, new()
        {
            var device = item as Device;
            if (device != null)
            {
                UpdateDevice(device, (DeviceDto)itemDto);
                return;
            }
            var CostItemV = item as CostItem;
            if (CostItemV != null && CostItemV.CostItemType == CostItemType.CostElement)
            {
                var costElement = item as CostElement;


                UpdateCostElement(costElement, (CostElementDto)itemDto);
                return;
            }

            if (CostItemV != null && CostItemV.CostItemType == CostItemType.Fee)
            {
                var fee = item as Fee;
                UpdateFee(fee, (FeeDto)itemDto);
                return;
            }

            var manufacturer = item as Manufacturer;
            if (manufacturer != null)
            {
                manufacturer.Order = ((ManufacturerDto)itemDto).Order;
            }

            var platform = item as Platform;
            if (platform != null)
            {
                platform.IsVisible = ((PlatformDto)itemDto).IsVisible;
            }

            var deviceCapability = item as DeviceCapability;
            if (deviceCapability != null)
            {
                deviceCapability.WurflCapabilities = ((DeviceCapabilityDto)itemDto).WurflCapabilities;
                deviceCapability.WurflValue = ((DeviceCapabilityDto)itemDto).WurflValue;
                deviceCapability.Type = (DeviceCapabilityType)((DeviceCapabilityDto)itemDto).Type;
            }

            var adCreativeAttribute = item as AdCreativeAttribute;
            if (adCreativeAttribute != null)
            {
                var adCreativeAttributeDto = itemDto as AdCreativeAttributeDto;
                adCreativeAttribute.IsSupported = adCreativeAttributeDto.IsSupported;
                adCreativeAttribute.Description = adCreativeAttributeDto.Description;
                adCreativeAttribute.Code = adCreativeAttributeDto.Code;
            }

            var keyword = item as Keyword;
            if (keyword != null)
            {
                keyword.Code = (itemDto as KeywordSaveDto).Code;
            }
            var Language = item as Language;
            if (Language != null)
            {
                Language.Code = (itemDto as LanguageSaveDto).Code;
                Language.ForPortal = (itemDto as LanguageSaveDto).ForPortal;

            }

            var advertiser = item as Advertiser;
            if (advertiser != null)
            {
                advertiser.DomainURL = (itemDto as AdvertiserDto).DomainURL;
            }

            var vendro = item as CreativeVendor;
            if (vendro != null)
            {


                foreach (CreativeVendorKeywordDto keyWord in (itemDto as CreativeVendorDto).InsertedKeywords)
                {
                    CreativeVendorKeyword KeyWordItem = MapperHelper.Map<CreativeVendorKeyword>(keyWord);
                    KeyWordItem.Vendor = vendro;
                    vendro.Keywords.Add(KeyWordItem);

                }
                foreach (CreativeVendorKeywordDto keyWord in (itemDto as CreativeVendorDto).DeletedKeywords)
                {
                    var KeyWordItem = vendro.Keywords.Where(x => x.Keyword == keyWord.Keyword).FirstOrDefault();
                    vendro.Keywords.Remove(KeyWordItem);
                }

            }


            var location = item as LocationBase;
            if (location != null)
            {
                var locationDto = (itemDto as LocationDto);
                location.TwoLettersCode = locationDto.TwoLettersCode;
                location.ThreeLettersCode = locationDto.ThreeLettersCode;
                location.MobileCountryCode = locationDto.MobileCountryCode;
                location.CodeAlpha2 = locationDto.TwoLettersCode;
                location.Parent = locationDto.ParentId.HasValue ? repository.Get(locationDto.ParentId.Value) as LocationBase : null;
            }
        }

        public void UpdateDevice(Device item, DeviceDto itemDto)
        {
            item.Manufacturer = GetRepository<IManufacturerRepository>(LookupNames.Manufacturer).Get(itemDto.Manufacturer.ID);
            item.Platform = GetRepository<IPlatformRepository>(LookupNames.Platform).Get(itemDto.Platform.ID);
            item.Code = itemDto.Code;


            if (item.Codes == null)
            {
                item.Codes = new List<DeviceCode>();
            }

            IList<string> Codes = itemDto.Code.Split(",");
            foreach (var code in Codes)
            {
                if (item.Codes.Where(M => M.Code == code).SingleOrDefault() == null)
                {
                    if (deviceCodeRepository.Query(M => M.Code == code && M.Device.ID != item.ID).FirstOrDefault() == null)
                        item.Codes.Add(new DeviceCode { Device = item, Code = code });
                    else
                    {
                        var error = new BusinessException();
                        error.Errors.Add(new ErrorData { ID = "DeviceDuplicateCode" });
                        throw error;
                    }
                   // item.Codes.Add(new DeviceCode { Device = item, Code = code });
                }


            }


        
            var codes = item.Codes.Select(M=>M.Code).ToList();
            foreach (var code in codes)
            {
                if (Codes.Where(M => M == code).SingleOrDefault() == null)
                {

                   var codeObj= item.Codes.Where(M => M.Code == code).SingleOrDefault();
                    codeObj.IsDeleted = true;
                    item.Codes.Remove(codeObj);
                }


            }


            item.DeviceType = deviceTypeRepository.Get(itemDto.DeviceTypeId);
        }
        public void UpdateCostElement(CostElement item, CostElementDto itemDto)
        {
            foreach (var costElementDto in itemDto.Values)
            {
                var costElementValue = item.Values.Where(p => p.CostModelWrapper.ID == costElementDto.CostModelWrapper.ID).SingleOrDefault();

                if (costElementValue != null)
                {
                    costElementValue.Value = costElementDto.Value;
                }
                else
                {
                    costElementValue = new CostItemValue();
                    costElementValue.Value = costElementDto.Value;
                    costElementValue.CostModelWrapper = new CostModelWrapper() { ID = costElementDto.CostModelWrapper.ID };
                    item.Values = item.Values.Concat(new[] { costElementValue }).ToList();
                }
            }
            item.Scope = itemDto.Scope;
            item.IsOneTime = itemDto.IsOneTime;
            item.Type = (CalculationType)itemDto.TypeId;
            item.CostElementCalculatedFrom = itemDto.CostElementCalculatedFrom;
            CalculateCostElementCategory(item, itemDto);
        }


        public void UpdateFee(Fee item, FeeDto itemDto)
        {
            foreach (var costElementDto in itemDto.Values)
            {
                var costElementValue = item.Values.Where(p => p.CostModelWrapper.ID == costElementDto.CostModelWrapper.ID).SingleOrDefault();

                if (costElementValue != null)
                {
                    costElementValue.Value = costElementDto.Value;
                }
                else
                {
                    costElementValue = new CostItemValue();
                    costElementValue.Value = costElementDto.Value;
                    costElementValue.CostModelWrapper = new CostModelWrapper() { ID = costElementDto.CostModelWrapper.ID };
                    item.Values = item.Values.Concat(new[] { costElementValue }).ToList();
                }
            }
            item.IsAutoAdded = itemDto.IsAutoAdded;
            item.IsBillable = itemDto.IsBillable;
            item.Type = (CalculationType)itemDto.TypeId;
            item.FeeCalculatedFrom = (FeeCalculatedFrom)itemDto.FeeCalculatedFrom;

            CalculateFeeCategory(item, itemDto);
        }
        #endregion
        #region Get
        public string GetLookupName(ValueMessageWrapper<int> id)
        {
            var lookup = lookupRepository.Query(x => x.ID == id.Value).FirstOrDefault();
            return lookup != null ? lookup.Name.Value : "";
        }

        public LookupListResultDto GetCostPageLookup(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria wcriteria)
        {

            LookupCriteria criteria = new LookupCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            IEnumerable<ManagedLookupBase> lookupList = null;
            var totalCount = 0;
            switch (criteria.LookType.ToLower())
            {
                case LookupNames.CostElement:
                    {
                        var repository = GetRepository<ICostElementRepository>(LookupNames.CostElement);
                        lookupList = repository.Query(criteria.GetLookupExpression<CostElement>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<CostElement>()).Count();
                        var items = lookupList.Select(currencyDto => MapperHelper.Map<CostElementDto>(currencyDto)).ToList();
                        return new LookupListResultDto() { Items = items, TotalCount = totalCount };

                        //var repository = GetRepository<ICostElementRepository>(LookupNames.CostElement);
                        //lookupList = repository.Query(criteria.GetLookupExpression<CostElement>(), criteria.Page - 1,
                        //    criteria.Size, item => item.ID, true);
                        //totalCount = repository.Query(criteria.GetLookupExpression<CostElement>()).Count();
                        //break;
                    }
                default:
                    {
                        lookupList = new List<ManagedLookupBase>();
                        return new LookupListResultDto() { };

                    }
            }

            //var items = lookupList.Select(currencyDto => MapperHelper.Map<LookupDto>(currencyDto)).ToList();
            //var returnResult = new LookupListResultDto() { Items = items, TotalCount = totalCount };

            //return returnResult;
        }

        public LookupListResultDto GetAllPageLookup(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria wcriteria)
        {
            LookupCriteria criteria = new LookupCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            if (string.IsNullOrEmpty(criteria.Name))
            {
                criteria.Name = string.Empty;
            }
            IEnumerable<ManagedLookupBase> lookupList = null;
            var totalCount = 0;
            switch (criteria.LookType.ToLower())
            {
                case LookupNames.Device:
                    {
                        DeviceLookupCriteria Dcriteria = new DeviceLookupCriteria();
                        Dcriteria.CopyFromCommonToDomain(wcriteria);

                        if (string.IsNullOrEmpty(Dcriteria.Name))
                        {
                            Dcriteria.Name = string.Empty;
                        }
                        var deviceCriteria = Dcriteria;
                        var repository = GetRepository<IDeviceRepository>(LookupNames.Device);
                        lookupList = repository.Query(deviceCriteria.GetDeviceExpression(), Dcriteria.Page - 1, Dcriteria.Size, item => item.ID, true);
                        totalCount = repository.Query(deviceCriteria.GetDeviceExpression()).Count();
                        break;
                    }
                case LookupNames.Currency:
                    {
                        var repository = GetRepository<ICurrencyRepository>(LookupNames.Currency);
                        lookupList = repository.Query(criteria.GetLookupExpression<Currency>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Currency>()).Count();
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        var repository = GetRepository<IManufacturerRepository>(LookupNames.Manufacturer);
                        lookupList = repository.Query(criteria.GetLookupExpression<Manufacturer>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Manufacturer>()).Count();
                        break;
                    }
                case LookupNames.Platform:
                    {
                        var repository = GetRepository<IPlatformRepository>(LookupNames.Platform);
                        lookupList = repository.Query(criteria.GetLookupExpression<Platform>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Platform>()).Count();
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        var repository = GetRepository<IKeyWordRepository>(LookupNames.Keyword);
                        lookupList = repository.Query(criteria.GetLookupExpression<Keyword>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Keyword>()).Count();
                        break;
                    }
                case LookupNames.CompanyType:
                    {
                        var repository = GetRepository<ICompanyTypeRepository>(LookupNames.CompanyType);
                        lookupList = repository.Query(criteria.GetLookupExpression<CompanyType>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<CompanyType>()).Count();
                        break;
                    }
                case LookupNames.CreativeVendor:
                    {

                        var repository = GetRepository<ICreativeVendorRepository>(LookupNames.CreativeVendor);
                        lookupList = repository.Query(criteria.GetLookupExpression<CreativeVendor>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<CreativeVendor>()).Count();
                        break;
                    }

                case LookupNames.LocationBase:
                    {
                        var repository = GetRepository<ILocationRepository>(LookupNames.LocationBase);
                        lookupList = repository.Query(criteria.GetLookupExpression<LocationBase>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<LocationBase>()).Count();
                        break;
                    }
                case LookupNames.Operator:
                    {
                        var repository = GetRepository<IOperatorRepository>(LookupNames.Operator);
                        lookupList = repository.Query(criteria.GetLookupExpression<Operator>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Operator>()).Count();
                        break;
                    }

                case LookupNames.audiencesegment:
                    {
                        var repository = GetRepository<IAudienceSegmentRepository>(LookupNames.audiencesegment);
                        lookupList = repository.Query(criteria.GetLookupExpression<AudienceSegment>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<AudienceSegment>()).Count();
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        var repository = GetRepository<IDeviceCapabilityRepository>(LookupNames.DeviceCapability);
                        lookupList = repository.Query(criteria.GetLookupExpression<DeviceCapability>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<DeviceCapability>()).Count();
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        var repository = GetRepository<ICostElementRepository>(LookupNames.CostElement);
                        lookupList = repository.Query(criteria.GetLookupExpression<CostElement>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<CostElement>()).Count();
                        break;
                    }
                case LookupNames.Fee:
                    {
                        var repository = GetRepository<IFeeRepository>(LookupNames.Fee);
                        lookupList = repository.Query(criteria.GetLookupExpression<Fee>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Fee>()).Count();
                        break;
                    }
                case LookupNames.AppMarketingPartner:
                    {
                        var repository = GetRepository<IAppMarketingPartnerRepository>(LookupNames.AppMarketingPartner);
                        lookupList = repository.Query(criteria.GetLookupExpression<AppMarketingPartner>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<AppMarketingPartner>()).Count();
                        break;
                    }
                case LookupNames.JobPosition:
                    {
                        var repository = GetRepository<IJobPositionRepository>(LookupNames.JobPosition);
                        lookupList = repository.Query(criteria.GetLookupExpression<JobPosition>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<JobPosition>()).Count();
                        break;
                    }
                case LookupNames.impressionmetric:
                    {
                        var repository = GetRepository<ImpressionMetricRepository>(LookupNames.impressionmetric);
                        lookupList = repository.Query(criteria.GetLookupExpression<ImpressionMetric>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<ImpressionMetric>()).Count();
                        break;
                    }
                case LookupNames.Attributes:
                    {
                        var repository = GetRepository<IAdCreativeAttributeRepository>(LookupNames.Attributes);
                        lookupList = repository.Query(criteria.GetLookupExpression<AdCreativeAttribute>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<AdCreativeAttribute>()).Count();
                        break;
                    }
                case LookupNames.Advertiser:
                    {
                        var repository = GetRepository<IAdvertiserRepository>(LookupNames.Advertiser);
                        lookupList = repository.Query(criteria.GetLookupExpression<Advertiser>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Advertiser>()).Count();
                        break;
                    }
                case LookupNames.AgeGroup:
                    {
                        var repository = GetRepository<IAgeGroupRepository>(LookupNames.AgeGroup);
                        lookupList = repository.Query(criteria.GetLookupExpression<AgeGroup>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<AgeGroup>()).Count();
                        break;
                    }
                case LookupNames.ActionType:
                    {
                        var repository = GetRepository<IAdActionTypeRepository>(LookupNames.ActionType);
                        lookupList = repository.Query(criteria.GetLookupExpression<AdActionTypeBase>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<AdActionTypeBase>()).Count();
                        break;
                    }
                case LookupNames.DeviceType:
                    {
                        var repository = GetRepository<IDeviceTypeRepository>(LookupNames.DeviceType);
                        lookupList = repository.Query(criteria.GetLookupExpression<DeviceType>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<DeviceType>()).Count();
                        break;
                    }
                case LookupNames.Geographic:
                    {
                        var repository = GetRepository<ILocationRepository>(LookupNames.Geographic);
                        lookupList = repository.Query(criteria.GetLookupExpression<LocationBase>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<LocationBase>()).Count();
                        break;
                    }
                case LookupNames.language:
                    {
                        var repository = GetRepository<ILanguageRepository>(LookupNames.language);
                        lookupList = repository.Query(criteria.GetLookupExpression<Language>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<Language>()).Count();
                        break;
                    }
                /*case LookupNames.viewabilityvendor:
                    {
                        var repository = GetRepository<IViewAbilityVendorRepository>(LookupNames.viewabilityvendor);
                        lookupList = repository.Query(criteria.GetLookupExpression<ViewAbilityVendor>(), criteria.Page - 1, criteria.Size, item => item.ID, true);
                        totalCount = repository.Query(criteria.GetLookupExpression<ViewAbilityVendor>()).Count();
                        break;
                    }*/
                default:
                    {
                        lookupList = new List<ManagedLookupBase>();
                        break;
                    }
            }

            var items = lookupList.Select(currencyDto => MapperHelper.Map<LookupDto>(currencyDto)).ToList();
            var returnResult = new LookupListResultDto() { Items = items, TotalCount = totalCount };

            return returnResult;
        }
        public LookupListResultDto GetAllLookup(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteriaBase wcriteria)
        {



            LookupCriteriaBase criteria = new LookupCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);
            IEnumerable<ManagedLookupBase> lookupList = null;
            //IEnumerable<LookupBase> lookupList3 = null;
            var totalCount = 0;
            switch (criteria.LookType.ToLower())
            {

                case LookupNames.DeviceType:
                    {
                        var repository = GetRepository<IDeviceTypeRepository>(LookupNames.DeviceType);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }



                case LookupNames.Device:
                    {
                        var repository = GetRepository<IDeviceRepository>(LookupNames.Manufacturer);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Currency:
                    {
                        var repository = GetRepository<ICurrencyRepository>(LookupNames.Currency);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        var repository = GetRepository<IManufacturerRepository>(LookupNames.Manufacturer);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Platform:
                    {
                        var repository = GetRepository<IPlatformRepository>(LookupNames.Platform);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        var repository = GetRepository<IKeyWordRepository>(LookupNames.Keyword);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.LocationBase:
                    {
                        var repository = GetRepository<ILocationRepository>(LookupNames.LocationBase);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.impressionmetric:
                    {
                        var repository = GetRepository<ImpressionMetricRepository>(LookupNames.impressionmetric);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Operator:
                    {
                        var repository = GetRepository<IOperatorRepository>(LookupNames.Operator);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        var repository = GetRepository<IDeviceCapabilityRepository>(LookupNames.DeviceCapability);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        var repository = GetRepository<ICostElementRepository>(LookupNames.CostElement);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.Fee:
                    {
                        var repository = GetRepository<IFeeRepository>(LookupNames.Fee);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.AppMarketingPartner:
                    {
                        var repository = GetRepository<IAppMarketingPartnerRepository>(LookupNames.AppMarketingPartner);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.JobPosition:
                    {
                        var repository = GetRepository<IJobPositionRepository>(LookupNames.JobPosition);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.CreativeVendor:
                    {
                        var repository = GetRepository<ICreativeVendorRepository>(LookupNames.CreativeVendor);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }

                case LookupNames.Advertiser:
                    {
                        var repository = GetRepository<IAdvertiserRepository>(LookupNames.Advertiser);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.BusinessPartnerType:
                    {
                        var repository = GetRepository<IBusinessPartnerTypeRepository>(LookupNames.BusinessPartnerType);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                case LookupNames.language:
                    {
                        var repository = GetRepository<ILanguageRepository>(LookupNames.language);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }
                /*case LookupNames.viewabilityvendor:
                    {
                        var repository = GetRepository<IViewAbilityVendorRepository>(LookupNames.viewabilityvendor);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        break;
                    }*/
                default:
                    {
                        lookupList = new List<ManagedLookupBase>();
                        break;
                    }
            }

            var items = lookupList.Select(currencyDto => MapperHelper.Map<LookupDto>(currencyDto)).ToList();
            return new LookupListResultDto() { Items = items, TotalCount = totalCount };
        }



        public LookupListResultDto GetAllLookupByType(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteriaBase wcriteria)
        {

            LookupCriteriaBase criteria = new LookupCriteriaBase();
            criteria.CopyFromCommonToDomain(wcriteria);
            IEnumerable<ManagedLookupBase> lookupList = null;
            //IEnumerable<LookupBase> lookupList3 = null;
            var totalCount = 0;
            switch (criteria.LookType.ToLower())
            {


                case LookupNames.CostElement:
                    {
                        var repository = GetRepository<ICostElementRepository>(LookupNames.CostElement);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        var items = lookupList.Select(currencyDto => MapperHelper.Map<CostElementDto>(currencyDto)).ToList();
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                if (item.CostElementCalculatedFrom != CostElementCalculatedFrom.Undefined)
                                {
                                    item.CustomName = item.Name.ToString() + " (" + item.CostElementCalculatedFrom.ToText() + ")";
                                }
                                else
                                {
                                    item.CustomName = item.Name.ToString();
                                }
                            }
                        }
                        return new LookupListResultDto() { Items = items, CostElementItems = items, TotalCount = totalCount };

                    }
                case LookupNames.Fee:
                    {
                        var repository = GetRepository<IFeeRepository>(LookupNames.Fee);
                        lookupList = repository.GetAll();
                        totalCount = lookupList.Count();
                        var items = lookupList.Select(currencyDto => MapperHelper.Map<FeeDto>(currencyDto)).ToList();
                        return new LookupListResultDto() { Items = items, TotalCount = totalCount };

                    }




                default:
                    {
                        lookupList = new List<ManagedLookupBase>();
                        return new LookupListResultDto() { };

                    }
            }



        }
        public LookupDto GetLookup(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupGetCriteria wcriteria)
        {

            LookupGetCriteria criteria = new LookupGetCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            ManagedLookupBase item = null;
            switch (criteria.LookType.ToLower())
            {
                case LookupNames.Device:
                    {
                        item = GetRepository<IDeviceRepository>(criteria.LookType).Get(criteria.Id) ?? new Device();// _deviceRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.impressionmetric:
                    {
                        item = GetRepository<IImpressionMetricRepository>(criteria.LookType).Get(criteria.Id) ?? new ImpressionMetric();
                        // item = _currencyRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.Currency:
                    {
                        item = GetRepository<ICurrencyRepository>(criteria.LookType).Get(criteria.Id) ?? new Currency();
                        // item = _currencyRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        item = GetRepository<IManufacturerRepository>(criteria.LookType).Get(criteria.Id) ?? new Manufacturer();
                        //item = _manufacturerRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.Platform:
                    {
                        item = GetRepository<IPlatformRepository>(criteria.LookType).Get(criteria.Id) ?? new Platform();
                        //item = _platformRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        item = GetRepository<IKeyWordRepository>(criteria.LookType).Get(criteria.Id) ?? new Keyword();
                        break;
                    }
                case LookupNames.CompanyType:
                    {
                        item = GetRepository<ICompanyTypeRepository>(criteria.LookType).Get(criteria.Id) ?? new CompanyType();
                        break;
                    }
                case LookupNames.CreativeVendor:
                    {
                        item = GetRepository<ICreativeVendorRepository>(criteria.LookType).Get(criteria.Id) ?? new CreativeVendor();
                        break;
                    }


                case LookupNames.LocationBase:
                    {
                        item = GetRepository<ILocationRepository>(criteria.LookType).Get(criteria.Id) ?? new LocationBase()
                        {
                            Type = 1
                        };
                        return MapperHelper.Map<LocationDto>(item);
                    }
                case LookupNames.Operator:
                    {
                        item = GetRepository<IOperatorRepository>(criteria.LookType).Get(criteria.Id) ?? new Operator();
                        break;
                    }

                case LookupNames.audiencesegment:
                    {
                        item = GetRepository<IAudienceSegmentRepository>(criteria.LookType).Get(criteria.Id) ?? new AudienceSegment();
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        item = GetRepository<IDeviceCapabilityRepository>(criteria.LookType).Get(criteria.Id) ?? new DeviceCapability();
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        item = GetRepository<ICostElementRepository>(criteria.LookType).Get(criteria.Id) ?? new CostElement() { CostElementCalculatedFrom = CostElementCalculatedFrom.BillableCost, CostItemType = CostItemType.CostElement, Type = CalculationType.Fixed };


                        break;
                    }
                case LookupNames.Fee:
                    {
                        item = GetRepository<IFeeRepository>(criteria.LookType).Get(criteria.Id) ?? new Fee() { FeeCalculatedFrom = FeeCalculatedFrom.ANC, CostItemType = CostItemType.Fee, Type = CalculationType.Fixed };
                        break;
                    }
                case LookupNames.JobPosition:
                    {
                        item = GetRepository<IJobPositionRepository>(criteria.LookType).Get(criteria.Id) ?? new JobPosition();
                        break;
                    }
                case LookupNames.AppMarketingPartner:
                    {
                        item = GetRepository<IAppMarketingPartnerRepository>(criteria.LookType).Get(criteria.Id) ?? new AppMarketingPartner();
                        break;
                    }
                case LookupNames.Attributes:
                    {
                        item = GetRepository<IAdCreativeAttributeRepository>(criteria.LookType).Get(criteria.Id) ?? new AdCreativeAttribute();
                        break;
                    }
                case LookupNames.Advertiser:
                    {
                        item = GetRepository<IAdvertiserRepository>(criteria.LookType).Get(criteria.Id) ?? new Advertiser();
                        break;
                    }
                case LookupNames.Demographic:
                case LookupNames.AgeGroup:
                    {
                        item = GetRepository<IAgeGroupRepository>(criteria.LookType).Get(criteria.Id) ?? new AgeGroup();
                        break;
                    }
                case LookupNames.ActionType:
                    {
                        item = GetRepository<IAdActionTypeRepository>(criteria.LookType).Get(criteria.Id) ?? new AdActionTypeBase();
                        break;
                    }
                case LookupNames.DeviceType:
                    {
                        item = GetRepository<IDeviceTypeRepository>(criteria.LookType).Get(criteria.Id) ?? new DeviceType();
                        break;
                    }
                case LookupNames.Geographic:
                    {
                        item = GetRepository<ILocationRepository>(criteria.LookType).Get(criteria.Id) ?? new LocationBase();
                        break;
                    }
                case LookupNames.language:
                    {
                        item = GetRepository<ILanguageRepository>(criteria.LookType).Get(criteria.Id) ?? new Language();
                        break;
                    }
                /* case LookupNames.viewabilityvendor:
                     {
                         item = GetRepository<IViewAbilityVendorRepository>(criteria.LookType).Get(criteria.Id) ?? new ViewAbilityVendor();
                         break;
                     }*/
                default:
                    {
                        item = new ManagedLookupBase();
                        break;
                    }
            }
            if (item == null)
            {
                item = new ManagedLookupBase();
            }
            var itemDto = MapperHelper.Map<LookupDto>(item);
            if (item.ID > 0 && criteria.LookType.ToLower() == LookupNames.CostElement)
            {
                var itemDto2 = MapperHelper.Map<CostElementDto>(item);
                RefereseCalculateCostElementCategory(item as CostElement, itemDto2);
                return itemDto2;
            }
            else if (criteria.LookType.ToLower() == LookupNames.CostElement)
            {
                var itemDto2 = MapperHelper.Map<CostElementDto>(item);
                return itemDto2;
            }


            if (item.ID > 0 && criteria.LookType.ToLower() == LookupNames.Fee)
            {
                var itemDto2 = MapperHelper.Map<FeeDto>(item);
                RefereseCalculateFeeCategory(item as Fee, itemDto2);
                return itemDto2;
            }
            else if (criteria.LookType.ToLower() == LookupNames.Fee)
            {
                var itemDto2 = MapperHelper.Map<FeeDto>(item);
                return itemDto2;
            }
            return itemDto;
        }
        public string GetLookupTextByCode(GetLookupTextByCodeRequest request)
        {
            ManagedLookupBase item = null;
            switch (request.LookupType.ToLower())
            {
                case LookupNames.InStreamPosition:
                    {
                        item = GetRepository<IInStreamPositionRepository>(request.LookupType).Query(M => M.Code == request.Code).SingleOrDefault() ?? new InStreamPosition();
                        break;
                    }
                case LookupNames.PlaybackMethods:
                    {
                        item = GetRepository<IPlaybackMethodsRepository>(request.LookupType).Query(M => M.Code == request.Code).SingleOrDefault() ?? new PlaybackMethods();
                        // item = _currencyRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.SkippableAds:
                    {
                        item = GetRepository<ISkippableAdsRepository>(request.LookupType).Query(M => M.Code == request.Code).SingleOrDefault() ?? new SkippableAds();
                        // item = _currencyRepository.Get(criteria.Id);
                        break;
                    }
                case LookupNames.PlacementType:
                    {
                        item = GetRepository<IPlacementTypeRepository>(request.LookupType).Query(M => M.Code == request.Code).SingleOrDefault() ?? new PlacementType();
                        //item = _manufacturerRepository.Get(criteria.Id);
                        break;
                    }

                default:
                    {
                        item = new ManagedLookupBase();
                        break;
                    }
            }
            if (item == null)
            {
                //item = new ManagedLookupBase();
                throw new Exception(string.Format("Code Not Found  '{0}' \n '{1}' LookupType", request.Code, request.LookupType));

            }
            ManagedLookupBase itemManged = (ManagedLookupBase)item;

            return itemManged.Name.Value.ToString();

        }
        public LookupListResultDto GetAdTypes()
        {


            var repository = GetRepository<IAdTypeRepository>(LookupNames.AdType);
            var adsType = repository.GetAll();
            var totalCount = adsType.Count();
            var items = adsType.Select(currencyDto => MapperHelper.Map<LookupDto>(currencyDto)).ToList();
            return new LookupListResultDto() { Items = items, TotalCount = totalCount };


        }

        public LookupListResultDto GetNativeAdLayouts()
        {


            var repository = GetRepository<INativeAdLayoutRepository>(LookupNames.NativeAdLayout);
            var adsType = repository.GetAll();
            var totalCount = adsType.Count();
            var items = adsType.Select(currencyDto => MapperHelper.Map<LookupDto>(currencyDto)).ToList();
            return new LookupListResultDto() { Items = items, TotalCount = totalCount };


        }

        public List<CreativeVendorKeywordDto> GetVendorkeywords(ValueMessageWrapper<int> id)
        {
            List<CreativeVendorKeyword> list = CreativeVendorKeywordRepository.Query(x => x.ID == id.Value).ToList();
            var items = MapperHelper.Map<List<CreativeVendorKeywordDto>>(list);
            return items;
        }

        public IEnumerable<CreativeFormatsDto> CreativeFormatsGetByQuery(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CreativeFormatsCriteria wcriteria)
        {

            CreativeFormatsCriteria criteria = new CreativeFormatsCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);

            var list = _creativeFormatsRepository.Query(criteria.GetExpression());
            return list.Select(AdvertiserDto => MapperHelper.Map<CreativeFormatsDto>(AdvertiserDto)).ToList();
        }
        #endregion

        #region Save
        public void SaveLookup(SaveLookupRequest request)
        {
            switch (request.LookType.ToLower())
            {
                case LookupNames.Device:
                    {
                        SaveEntity(request.Data, GetRepository<IDeviceRepository>(request.LookType));
                        break;
                    }
                case LookupNames.impressionmetric:
                    {
                        SaveEntity(request.Data, GetRepository<ImpressionMetricRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Currency:
                    {
                        SaveEntity(request.Data, GetRepository<ICurrencyRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        SaveEntity(request.Data, GetRepository<IManufacturerRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Platform:
                    {
                        SaveEntity(request.Data, GetRepository<IPlatformRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        SaveEntity(request.Data, GetRepository<IKeyWordRepository>(request.LookType));
                        break;
                    }
                case LookupNames.LocationBase:
                    {
                        SaveEntity(request.Data, GetRepository<ILocationRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Operator:
                    {
                        SaveEntity(request.Data, GetRepository<IOperatorRepository>(request.LookType));
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        SaveEntity(request.Data, GetRepository<IDeviceCapabilityRepository>(request.LookType));
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        SaveEntity(request.Data, GetRepository<ICostElementRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Fee:
                    {
                        SaveEntity(request.Data, GetRepository<IFeeRepository>(request.LookType));
                        break;
                    }
                case LookupNames.JobPosition:
                    {
                        SaveEntity(request.Data, GetRepository<IJobPositionRepository>(request.LookType));
                        break;
                    }
                case LookupNames.Advertiser:
                    {
                        SaveEntity(request.Data, GetRepository<IAdvertiserRepository>(request.LookType));

                        break;
                    }
                case LookupNames.Attributes:
                    {
                        SaveEntity(request.Data, GetRepository<IAdCreativeAttributeRepository>(request.LookType));
                        break;
                    }
                case LookupNames.language:
                    {
                        SaveEntity(request.Data, GetRepository<ILanguageRepository>(request.LookType));
                        break;
                    }
                /*case LookupNames.viewabilityvendor:
                    {
                        SaveEntity(data, GetRepository<IViewAbilityVendorRepository>(lookType));
                        break;
                    }*/
                case LookupNames.CreativeVendor:
                    {

                        SaveEntity(request.Data, GetRepository<ICreativeVendorRepository>(request.LookType));

                        break;
                    }
            }
            // notify that the a lookup has been updated and the local cache should be invalidated 
            Domain.Configuration.KafkaEventPublisher.Publish(new InvalidateLocalCache { TypeName = request.LookType } );
        }
        #endregion

        public LookupListResultDto GetParentLocations(ValueMessageWrapper<int> typeId)
        {
            IEnumerable<LookupDto> lookupList = null;
            //  IEnumerable<LookupDto> Items = null;
            switch (typeId.Value)
            {
                //case (int)LocationType.Continent:
                //    {
                //        var continentRep = IoC.Instance.Resolve<IContinentRepository>();
                //        var items = continentRep.GetAll();
                //        lookupList = items.Select(currencyDto => MapperHelper.Map<LocationDto>(currencyDto)).ToList();
                //    }
                //   break;
                case (int)LocationType.Country:
                    {
                        var continentRep = IoC.Instance.Resolve<IContinentRepository>();
                        var items = continentRep.GetAll();
                        lookupList = items.Select(currencyDto => MapperHelper.Map<LocationDto>(currencyDto)).ToList();
                    }
                    break;
                case (int)LocationType.State:

                    var countryRep = IoC.Instance.Resolve<ICountryRepository>();
                    //    items = countryRep.GetAll();
                    lookupList = countryRep.GetAll().Select(currencyDto => MapperHelper.Map<LocationDto>(currencyDto)).ToList();

                    break;
                case (int)LocationType.City:
                    var stateRep = IoC.Instance.Resolve<IStateRepository>();
                    //var items = stateRep.GetAll();
                    lookupList = stateRep.GetAll().Select(currencyDto => MapperHelper.Map<LocationDto>(currencyDto)).ToList();

                    break;
                default:
                    break;
            }


            //   var lookupList = repository.GetAll().Where(x => x.Type == typeId).ToList();
            // var items = lookupList.Select(currencyDto => MapperHelper.Map<LocationDto>(currencyDto)).ToList();

            var totalCount = lookupList.Count();
            return new LookupListResultDto() { Items = lookupList, TotalCount = totalCount };
        }


        //public bool ValidateLangCode(int? id, string code)
        //{
        //    Language item = null;
        //    if (id.HasValue)
        //    {
        //        item = LanguageRepository.Query(x => x.Code == code && x.ID != id).SingleOrDefault();

        //    }
        //    else
        //    {

        //        item = LanguageRepository.Query(x => x.Code == code).SingleOrDefault();

        //    }

        //    if (item != null)
        //    {
        //        return false;
        //    }

        //    return true;

        //}
    }
}
