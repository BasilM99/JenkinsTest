using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services
{
    public class CountryService : ICountryService
    {
        private ICountryRepository countryRepository;


        private IStateRepository stateRepository;
        public CountryService(ICountryRepository repository, IStateRepository stateRep)
        {
            this.countryRepository = repository;
            this.stateRepository = stateRep;
        }
        public Interfaces.DTOs.Core.CountryDto GetByCode(string Code)
        {
            Country country = countryRepository.Query(M=>M.TwoLettersCode == Code && M.Type==2).ToList().SingleOrDefault();
            return MapperHelper.Map<CountryDto>(country);
        }
        public IEnumerable<Interfaces.DTOs.Core.CountryDto> GetAll()
        {
            IEnumerable<Country> countries = countryRepository.GetAll();
            return countries.Select(countryDto => MapperHelper.Map<CountryDto>(countryDto)).ToList();
        }

        public IEnumerable<Interfaces.DTOs.Core.LocationDto> GetAllStates(ValueMessageWrapper<int> parentCountryId)
        {
            IEnumerable<State> states = stateRepository.Query(M => M.Parent.ID == parentCountryId.Value).ToList();
            return states.Select(countryDto => MapperHelper.Map<LocationDto>(countryDto)).ToList();
        }
    }
}
