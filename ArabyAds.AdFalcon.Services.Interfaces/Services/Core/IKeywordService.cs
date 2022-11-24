using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.Framework;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{
    public class KeywordSearcher
    {
        public static IEnumerable<KeywordDto> GetByQuery(IEnumerable<KeywordDto> items, KeywordCriteria criteria)
        {
            return items.Where(x => x.Name.Value.StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
        }
    }

    [ServiceContract()]
    [CacheHeader(LookupNames.Keyword, typeof(KeywordSearcher), CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IKeywordService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<KeywordDto> GetAll();


        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        KeywordDto Get(ValueMessageWrapper<int> id);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IEnumerable<KeywordDto> GetTop(ValueMessageWrapper<int?> Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable()]
        IEnumerable<KeywordDto> GetByQuery(KeywordCriteria criteria);
    }
}
