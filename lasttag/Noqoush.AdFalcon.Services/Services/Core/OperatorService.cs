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
    public class OperatorService : IOperatorService
    {

        private IOperatorRepository operatorRepository = null;
        public OperatorService(IOperatorRepository operatorRepository)
        {
            this.operatorRepository = operatorRepository;
        }

        public IEnumerable<OperatorDto> GetAll()
        {
            var list = operatorRepository.GetAll();
            return list.Select(operatorDto => MapperHelper.Map<OperatorDto>(operatorDto)).ToList();
        }
        public IEnumerable<TreeDto> GetAllCountryOperator()
        {
            var returnList = new List<TreeDto>();
            var list = operatorRepository.GetAll();
            var items = (from operater in list where operater.Location !=null  group operater by operater.Location.ID into g select new { Id = g.Key, Operators = g.ToList() }).ToList();
            foreach (var item in items)
            {
                var countryOperatorDto = new TreeDto {Id = item.Id.ToString(), Childs = new List<TreeDto>()};
                foreach (var Operator in item.Operators)
                {
                    var operaterTreeNode = new TreeDto
                                               {
                                                   Id = Operator.ID.ToString(),
                                                   Name = MapperHelper.Map<LocalizedStringDto>(Operator.Name)
                                               };
                    countryOperatorDto.Childs.Add(operaterTreeNode);
                }
                countryOperatorDto.Name = MapperHelper.Map<LocalizedStringDto>(item.Operators.First().Location.Name);
                returnList.Add(countryOperatorDto);
            }
            return returnList;
        }
        /// <summary>
        /// use this service operation to get All  Operators by country Ids
        /// </summary>
        /// <param name="countryIds">country Ids to filter by</param>
        /// <returns>List CountryOperatorDto </returns>
        public IEnumerable<TreeDto> GetAllOperatorByCountryIds(int[] countryIds)
        {
            var returnList = new List<TreeDto>();
            var list = operatorRepository.Query(item=> countryIds.Contains(item.Location.ID));
            var items = (from operater in list where operater.Location !=null  group operater by operater.Location.ID into g select new { Id = g.Key, Operators = g.ToList() }).ToList();
            foreach (var item in items)
            {
                var countryOperatorDto = new TreeDto {Id = item.Id.ToString(), Childs = new List<TreeDto>()};
                foreach (var Operator in item.Operators)
                {
                    var operaterTreeNode = new TreeDto
                                               {
                                                   Id = Operator.ID.ToString(),
                                                   Name = MapperHelper.Map<LocalizedStringDto>(Operator.Name)
                                               };
                    countryOperatorDto.Childs.Add(operaterTreeNode);
                }
                countryOperatorDto.Name = MapperHelper.Map<LocalizedStringDto>(item.Operators.First().Location.Name);
                returnList.Add(countryOperatorDto);
            }
            return returnList;
        }
    }
}
