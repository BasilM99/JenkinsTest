using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class JobPositionService : IJobPositionService
    {
        private IJobPositionRepository jobPositionRepository;

        public JobPositionService(IJobPositionRepository jobPositionRepository)
        {
            this.jobPositionRepository = jobPositionRepository;
        }

        public IEnumerable<JobPositionDto> GetAll()
        {
            var jobPosition = jobPositionRepository.GetAll();
            return jobPosition.Select(x => MapperHelper.Map<JobPositionDto>(x)).ToList();
        }

        public IEnumerable<JobPositionDto> GetByName(string prefix)
        {
            prefix = prefix.ToLower();
            var jobPosition = jobPositionRepository.GetAll().Where(x => x.Name.Value.ToLower().Contains(prefix));
            return jobPosition.Select(x => MapperHelper.Map<JobPositionDto>(x)).ToList();
        }
    }
}
