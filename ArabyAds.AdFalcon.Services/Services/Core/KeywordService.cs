using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;

namespace ArabyAds.AdFalcon.Services
{
    public class KeywordService : IKeywordService
    {
        private IKeyWordRepository keyWordRepository = null;
        private IConfigurationManager configurationManager = null;
        public KeywordService(IKeyWordRepository keyWordRepository, IConfigurationManager configurationManager)
        {
            this.keyWordRepository = keyWordRepository;
            this.configurationManager = configurationManager;
        }

        public KeywordDto Get(ValueMessageWrapper<int> id)
        {
            var keyword = keyWordRepository.Get(id.Value);
            if (keyword != null)
            {
                return MapperHelper.Map<KeywordDto>(keyword);
            }
            return null;
        }

        public IEnumerable<KeywordDto> GetAll()
        {
            IEnumerable<Keyword> list = keyWordRepository.GetAll();

            list = list.Where(M => M.IsDeleted == false).Where(M=>M.IsHidden ==false).AsEnumerable() ;
            return list.Select(keywordDto => MapperHelper.Map<KeywordDto>(keywordDto)).ToList();
        }

        private int GetRankForTag(double Usage, double TotalCount)
        {
            if (TotalCount == 0)
                return 1;

            var result = (Usage / TotalCount) * 100;
            if (result <= 5)
                return 1;
            if (result <= 15)
                return 2;
            if (result <= 30)
                return 3;
            if (result <= 50)
                return 4;
            if (result <= 70)
                return 5;
            if (result <= 90)
                return 6;
            return 7;
        }

        public IEnumerable<KeywordDto> GetTop(ValueMessageWrapper<int?> count)
        {
            //first get top 1000 order by usage then get top count order by random Guid
            //toDO:OSaleh to change the maxTop from configuration not static
            var maxTop = 30;
            //TODO: Malik to add cache to Configuration Setting Service
            //configurationManager.GetConfigurationSetting(null, null, "MaxTagCount");
            if (!count.Value.HasValue)
            {
                count.Value = maxTop;
            }
            if (count.Value > maxTop)
                count.Value = maxTop;
            IEnumerable<Keyword> list = keyWordRepository.GetTop(maxTop).OrderBy(x => new Guid()).Take(count.Value.Value).ToList();

            list = list.Where(M => M.IsDeleted == false).Where(M => M.IsHidden == false).AsEnumerable();

            var dtosList = list.Select(keywordDto => MapperHelper.Map<KeywordDto>(keywordDto)).ToList();
            int totalCount = dtosList.Sum(item => item.Usage);
            foreach (var keywordDto in dtosList)
            {
                keywordDto.Rank = GetRankForTag(keywordDto.Usage, totalCount);
            }
            return dtosList;
        }

        public IEnumerable<KeywordDto> GetByQuery(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.KeywordCriteria wcriteria)
        {


            KeywordCriteria criteria = new KeywordCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var list = keyWordRepository.Query(criteria.GetExpression());
            list = list.Where(M => M.IsDeleted == false).Where(M => M.IsHidden == false).AsEnumerable();

            return list.Select(keywordDto => MapperHelper.Map<KeywordDto>(keywordDto)).ToList();
        }

    }
}
