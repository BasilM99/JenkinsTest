using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
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
