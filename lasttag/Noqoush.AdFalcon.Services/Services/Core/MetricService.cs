using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class MetricTestService : IMetricTestService
    {
        public IMetricRepository _MetricRepository;

        public MetricTestService(IMetricRepository metricRepository)
        {
            _MetricRepository = metricRepository;
        }

        public List<MetricDto> GetAll()
        {
            List<Metric> metricList = _MetricRepository.GetAll().ToList();
            List<MetricDto> metricDtoList = metricList.Select(p => MapperHelper.Map<MetricDto>(p)).ToList();
            return metricDtoList;
        }


        public MetricDto GetByCode(string code)
        {
            Metric metric = _MetricRepository.Query(p => p.Code == code).SingleOrDefault();
            MetricDto metricDto = MapperHelper.Map<MetricDto>(metric);

            return metricDto;
        }
    }
}
