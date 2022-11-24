using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class ManufacturerService : IManufacturerService
    {

        private IManufacturerRepository manufacturerRepository = null;
        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        public IEnumerable<ManufacturerDto> GetAll()
        {
            var list = manufacturerRepository.GetAll();
            return list.Select(operatorDto => MapperHelper.Map<ManufacturerDto>(operatorDto)).ToList();
        }
        public IEnumerable<TreeDto> GetAllManufacturerTree()
        {
            var returnList = new List<TreeDto>();
            var list = manufacturerRepository.GetAll().OrderBy(item=>item.Order);
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
