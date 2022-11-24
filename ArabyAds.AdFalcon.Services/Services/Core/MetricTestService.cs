using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class MetricService : IMetricService
    {
        public IMetricRepository _MetricRepository;

        public MetricService(IMetricRepository metricRepository)
        {
            _MetricRepository = metricRepository;
        }

        public List<MetricDto> GetAll()
        {
            List<Metric> metricList = _MetricRepository.GetAll().ToList();
            List<MetricDto> metricDtoList = metricList.Select(p => MapperHelper.Map<MetricDto>(p)).ToList();
            return metricDtoList;
        }

        public List<MetricResultDto> GetMetricResultsAll()
        {
            List<Metric> metricList = _MetricRepository.GetAll().ToList();
            List<MetricResultDto> metricDtoList = metricList.Select(p => MapperHelper.Map<MetricResultDto>(p)).ToList();
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
