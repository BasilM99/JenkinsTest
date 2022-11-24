using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class CostElementService : ICostElementService
    {
        private readonly ICostElementRepository costElementRepository = null;
        public CostElementService(ICostElementRepository costElementRepository)
        {
            this.costElementRepository =costElementRepository;
        }

        public IEnumerable<CostElementDto> GetAll()
        {
            var languageList =costElementRepository.GetAll();
            var items = languageList.Select(costElement => MapperHelper.Map<CostElementDto>(costElement)).ToList();
            return items;
        }
    }
}
