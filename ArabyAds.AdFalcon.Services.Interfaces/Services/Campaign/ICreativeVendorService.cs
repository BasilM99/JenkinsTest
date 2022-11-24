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
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{

    public class CreativeVendorSearcher
    {
        public static List<CreativeVendorDto> GetByQuery(IEnumerable<CreativeVendorDto> items, CreativeVendorCriteria criteria)
        {

            if (criteria.Value == null)
            {
                criteria.Value = string.Empty;
            }
            if (string.IsNullOrEmpty(criteria.Culture))
            {
                return items.Where(x => x.Name.Value.StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase)).ToList();
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
                return result.ToList();
            }
        }
    }

  
    [ServiceContract()]
    [CacheHeader(LookupNames.CreativeVendor, typeof(CreativeVendorSearcher), CacheStore = "MemoryCache", EnableLocalCacheInvalidation =  true)]
    public interface ICreativeVendorService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<CreativeVendorDto> GetAll();


        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CreativeVendorDto Get(ValueMessageWrapper<int> id);

        ///// <summary>
        ///// use this service operation to get Top n Keyword
        ///// </summary>
        ///// <returns>List KeywordDto </returns>
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //IEnumerable<AdvertiserDto> GetTop(int? Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
       [ArabyAds.Framework.Caching.Cachable()]
        List<CreativeVendorDto> GetByQuery(CreativeVendorCriteria criteria);
    }
}