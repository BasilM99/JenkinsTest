using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services
{
    public class ThemeService : IThemeService
    {
        private ITextAdThemeRepository textAdThemeRepository = null;
        public ThemeService(ITextAdThemeRepository textAdThemeRepository)
        {
            this.textAdThemeRepository = textAdThemeRepository;
        }
        public IEnumerable<ThemeDto> GetAll()
        {
            IEnumerable<TextAdTheme> list = textAdThemeRepository.Query(item => item.IsCustom == false);


            return list.Select(themeDto => MapperHelper.Map<ThemeDto>(themeDto)).ToList();
        }

        /// <summary>
        /// use this service operation to get  non-Custom themes by id
        /// </summary>
        /// <returns>Non-Custom ThemeDto </returns>
        public ThemeDto Get(ValueMessageWrapper<int> id)
        {
            return MapperHelper.Map<ThemeDto>(textAdThemeRepository.Get(id.Value));
        }
    }
}
