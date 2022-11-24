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
