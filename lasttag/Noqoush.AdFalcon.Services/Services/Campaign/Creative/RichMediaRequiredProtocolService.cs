using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Campaign.Creative
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
