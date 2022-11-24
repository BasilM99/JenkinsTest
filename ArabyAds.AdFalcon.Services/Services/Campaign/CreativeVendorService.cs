using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Campaign
{
    public class CreativeVendorService : ICreativeVendorService
    {
        private ICreativeVendorRepository CreativeVendorRepository = null;

        private IAdCreativeUnitVendorRepository AdCreativeUnitVendorRepository = null;
        private IConfigurationManager configurationManager = null;
        public CreativeVendorService(ICreativeVendorRepository creativeVendorRepository, IConfigurationManager configurationManager, IAdCreativeUnitVendorRepository adCreativeUnitVendorRepository)
        {
            this.CreativeVendorRepository = creativeVendorRepository;
            this.configurationManager = configurationManager;
            this.AdCreativeUnitVendorRepository = adCreativeUnitVendorRepository;
        }

        public CreativeVendorDto Get(ValueMessageWrapper<int> id)
        {
            var cv = CreativeVendorRepository.Get(id.Value);
            if (cv != null)
            {
                return MapperHelper.Map<CreativeVendorDto>(cv);
            }
            return null;
        }

        public IEnumerable<CreativeVendorDto> GetAll()
        {
            IEnumerable<CreativeVendor> list = CreativeVendorRepository.GetAll();
            return list.Select(AdvertiserDto => MapperHelper.Map<CreativeVendorDto>(AdvertiserDto)).ToList();
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
     
     
        public List<CreativeVendorDto> GetByQuery(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CreativeVendorCriteria wcriteria)
        {

            CreativeVendorCriteria criteria = new CreativeVendorCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var list = CreativeVendorRepository.Query(criteria.GetExpression());
            return list.Select(AdvertiserDto => MapperHelper.Map<CreativeVendorDto>(AdvertiserDto)).ToList();
        }

    }
}
