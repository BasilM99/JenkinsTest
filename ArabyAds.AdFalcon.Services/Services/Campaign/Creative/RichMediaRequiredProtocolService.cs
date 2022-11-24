using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Campaign.Creative
{
    public class RichMediaRequiredProtocolService : IRichMediaRequiredProtocolService
    {
        private readonly IRichMediaRequiredProtocolRepository _requiredProtocolRepository = null;
        public RichMediaRequiredProtocolService(IRichMediaRequiredProtocolRepository requiredProtocolRepository)
        {
            _requiredProtocolRepository = requiredProtocolRepository;
        }

        public IEnumerable<RichMediaRequiredProtocolDto> GetAll()
        {
            var list = _requiredProtocolRepository.GetAll();
            return list.Select(richMediaRequiredProtocolDto => MapperHelper.Map<RichMediaRequiredProtocolDto>(richMediaRequiredProtocolDto)).ToList();
        }
    }
}
