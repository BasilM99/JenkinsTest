using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.Framework.Attributes;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{
    public class KeywordSearcher
    {
        public static IEnumerable<KeywordDto> GetByQuery(IEnumerable<KeywordDto> items, KeywordCriteria criteria)
        {
            return items.Where(x => x.Name.Value.StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
        }
    }

    [ServiceContract()]
    [CacheHeader("KeywordService", typeof(KeywordSearcher))]
    public interface IKeywordService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<KeywordDto> GetAll();


        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        KeywordDto Get(int id);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IEnumerable<KeywordDto> GetTop(int? Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable()]
        IEnumerable<KeywordDto> GetByQuery(KeywordCriteria criteria);
    }
}
