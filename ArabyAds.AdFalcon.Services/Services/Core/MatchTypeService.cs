using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class MatchTypeService : IMatchTypeService
    {
        
        private IMatchTypeRepository _matchTypeRepository = null;
        public MatchTypeService(IMatchTypeRepository matchTypeRepository)
        {
            this._matchTypeRepository = matchTypeRepository;
        }

        public List<Interfaces.DTOs.Core.LookupDto> GetAll()
        {
            List<MatchType> list = _matchTypeRepository.GetAll().ToList();

            List<LookupDto> listDto = list.Select(p => MapperHelper.Map<LookupDto>(p)).ToList();

            return listDto;
        }
    }
}
