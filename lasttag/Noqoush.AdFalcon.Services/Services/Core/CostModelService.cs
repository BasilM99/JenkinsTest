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
