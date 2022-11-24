using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services
{
    public class LanguageService : ILanguageService
    {
        private ILanguageRepository languageRepository = null;
        public LanguageService(ILanguageRepository languageRepository)
        {
            this.languageRepository = languageRepository;
        }

        public IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAll()
        {
            IEnumerable<Language> languageList = languageRepository.GetAll();

            var items = languageList.Select(languageDto => MapperHelper.Map<LanguageDto>(languageDto)).ToList();

            return items;
        }
        public IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAllForUI()
        {
            IEnumerable<Language> languageList = languageRepository.Query(M=>M.ForPortal==true);

            var items = languageList.Select(languageDto => MapperHelper.Map<LanguageDto>(languageDto)).ToList();

            return items;
        }

        public IEnumerable<LanguageDto> GetByQuery(Noqoush.AdFalcon.Domain.Common.Repositories.Core.LanguageCriteria wcriteria)
        {

            LanguageCriteria criteria = new LanguageCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var list = languageRepository.Query(criteria.GetExpression());
            return list.Select(AdvertiserDto => MapperHelper.Map<LanguageDto>(AdvertiserDto)).ToList();
        }
    }
}
