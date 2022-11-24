using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class AgeGroupService : IAgeGroupService
    {
        private IAgeGroupRepository ageGroupRepository;

        public AgeGroupService(IAgeGroupRepository ageGroupRepository)
        {
            this.ageGroupRepository = ageGroupRepository;
        }

        public IEnumerable<AgeGroupDto> GetAll()
        {
            var ageGroups = ageGroupRepository.GetAll();
            return ageGroups.Select(countryDto => MapperHelper.Map<AgeGroupDto>(countryDto)).ToList();
        }
    }
}
