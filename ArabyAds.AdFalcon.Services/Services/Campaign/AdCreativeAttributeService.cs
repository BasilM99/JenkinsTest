using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Services.Campaign 
{
    public class AdCreativeAttributeService : IAdCreativeAttributeService
    {
        private IAdCreativeAttributeRepository _AdCreativeAttributeRepository;

        public AdCreativeAttributeService(IAdCreativeAttributeRepository adCreativeAttributeRepository)
        {
            _AdCreativeAttributeRepository = adCreativeAttributeRepository;
        }

        public List<AdCreativeAttributeDto> GetAll()
        {
            List<AdCreativeAttribute> attributes = _AdCreativeAttributeRepository.GetAll().ToList();

            return attributes.Select(p => MapperHelper.Map<AdCreativeAttributeDto>(p)).ToList();
        }
    }
}
