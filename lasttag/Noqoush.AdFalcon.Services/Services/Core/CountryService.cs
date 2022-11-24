using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services
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

        public IEnumerable<Interfaces.DTOs.Core.LocationDto> GetAllStates(int parentCountryId)
        {
            IEnumerable<State> states = stateRepository.Query(M => M.Parent.ID == parentCountryId).ToList();
            return states.Select(countryDto => MapperHelper.Map<LocationDto>(countryDto)).ToList();
        }
    }
}
