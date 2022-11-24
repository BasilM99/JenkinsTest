using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
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
