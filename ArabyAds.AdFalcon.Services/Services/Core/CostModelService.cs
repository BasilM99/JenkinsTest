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
    public class CostModelService : ICostModelService
    {
        private readonly ICostModelRepository _CostModelRepository = null;
        public CostModelService(ICostModelRepository costModelRepository)
        {
            this._CostModelRepository = costModelRepository;
        }


        public IEnumerable<CostModelDto> GetAll()
        {
            var costModels = _CostModelRepository.GetAll();

            var costModelsDtoList = costModels.Select(p => MapperHelper.Map<CostModelDto>(p)).ToList();

            return costModelsDtoList;
        }
    }
}
