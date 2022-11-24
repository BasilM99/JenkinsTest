using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
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
