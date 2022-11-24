using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{

    public class LanguageSearcher
    {
        public static IEnumerable<LanguageDto> GetByQuery(IEnumerable<LanguageDto> items, LanguageCriteria criteria)
        {
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
    [CacheHeader("LanguageService", typeof(LanguageSearcher))]
    public interface ILanguageService
    {
        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns>LanguageDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        [Cachable(IsGetAll = true)]
        IEnumerable<LanguageDto> GetAll();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable()]
        IEnumerable<LanguageDto> GetByQuery(LanguageCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
      //  [Noqoush.Framework.Caching.Cachable(IsSelfCachable =true)]
        [NoAuthentication]
        IEnumerable<Interfaces.DTOs.Core.LanguageDto> GetAllForUI();
    }
}
