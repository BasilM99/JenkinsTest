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
    public class CostModelWrapperService : ICostModelWrapperService
    {
        private readonly ICostModelWrapperRepository _CostModelWrapperRepository = null;
        public CostModelWrapperService(ICostModelWrapperRepository costModelWrapperRepository)
        {
            this._CostModelWrapperRepository = costModelWrapperRepository;
        }


        public IEnumerable<CostModelWrapperDto> GetAll()
        {
            var costModelWrappers = _CostModelWrapperRepository.GetAll();

            var costModelWrappersDtoList = costModelWrappers.Select(p => MapperHelper.Map<CostModelWrapperDto>(p)).ToList();

            return costModelWrappersDtoList;
        }
    }
}
