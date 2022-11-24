using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
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
