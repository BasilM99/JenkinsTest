using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework.Attributes;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{

    public class LanguageSearcher
    {
        public static IEnumerable<LanguageDto> GetByQuery(IEnumerable<LanguageDto> items, LanguageCriteria criteria)
        {
            if (criteria.Value==null)
            {
                criteria.Value = string.Empty;
            }
            if (string.IsNullOrEmpty(criteria.Culture))
            {
                return items.Where(x => x.Name.Value.StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var result = items.Where(x => x.Name.GetValue(criteria.Culture).StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
                if (result != null)
                {
                    foreach (var item in result)
                    {

                        item.Name.DefaultCulture = criteria.Culture;
                    }
                }
                return result;
            }
        }
    }

    [ServiceContract]
    [CacheHeader(LookupNames.language, typeof(LanguageSearcher), CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface ILanguageService
    {
        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns>LanguageDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<LanguageDto> GetAll();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable()]
        IEnumerable<LanguageDto> GetByQuery(LanguageCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
      //  [ArabyAds.Framework.Caching.Cachable(IsSelfCachable =true)]
        [NoAuthentication]
        IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAllForUI();
    }
}
