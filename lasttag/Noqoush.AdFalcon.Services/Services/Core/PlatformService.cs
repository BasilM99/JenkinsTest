using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class PlatformService : IPlatformService
    {

        private IPlatformRepository platformRepository = null;
        public PlatformService(IPlatformRepository platformRepository)
        {
            this.platformRepository = platformRepository;
        }

        public IEnumerable<PlatformDto> GetAll()
        {
            //var list = platformRepository.GetAll();
            var list = platformRepository.GetAll();
            var platformsDto = list.Select(operatorDto => MapperHelper.Map<PlatformDto>(operatorDto)).ToList();

            return platformsDto;
        }
        public IEnumerable<TreeDto> GetAllPlatformTree()
        {
            var returnList = new List<TreeDto>();
            //var list = platformRepository.GetAll();
            var list = platformRepository.Query(x => x.IsVisible);
            foreach (var item in list)
            {
                var treeDto = new TreeDto
                                  {
                                      Id = item.ID.ToString(),
                                      Childs = new List<TreeDto>(),
                                      Name = MapperHelper.Map<LocalizedStringDto>(item.Name)
                                  };
                returnList.Add(treeDto);
            }
            return returnList;
        }
    }
}
