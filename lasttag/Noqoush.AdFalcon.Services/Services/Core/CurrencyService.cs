using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository currencyRepository = null;
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public IEnumerable<CurrencyDto> GetAll()
        {
            var languageList = currencyRepository.GetAll();
            var items = languageList.Select(currencyDto => MapperHelper.Map<CurrencyDto>(currencyDto)).ToList();
            return items;
        }
    }
}
