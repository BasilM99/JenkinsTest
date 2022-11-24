using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class RefreshIntervalService : IRefreshIntervalService
    {
        private IRefreshIntervalRepository refreshIntervalRepository = null;
        public RefreshIntervalService(IRefreshIntervalRepository refreshIntervalRepository)
        {
            this.refreshIntervalRepository = refreshIntervalRepository;
        }
        public IEnumerable<LookupDto> GetAll()
        {
            var list = refreshIntervalRepository.GetAll();
            return list.Select(lookupDto => MapperHelper.Map<LookupDto>(lookupDto)).ToList();
        }
    }
}
