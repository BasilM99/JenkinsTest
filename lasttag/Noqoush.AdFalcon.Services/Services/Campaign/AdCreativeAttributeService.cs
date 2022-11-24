using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Services.Services.Campaign 
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
