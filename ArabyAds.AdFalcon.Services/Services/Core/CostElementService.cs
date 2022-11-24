using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
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
