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
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.Framework.Attributes;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
{

    public class CreativeVendorSearcher
    {
        public static List<CreativeVendorDto> GetByQuery(IEnumerable<CreativeVendorDto> items, CreativeVendorCriteria criteria)
        {
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
    [CacheHeader("CreativeVendorService", typeof(CreativeVendorSearcher))]
    public interface ICreativeVendorService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<CreativeVendorDto> GetAll();


        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CreativeVendorDto Get(int id);

        ///// <summary>
        ///// use this service operation to get Top n Keyword
        ///// </summary>
        ///// <returns>List KeywordDto </returns>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //IEnumerable<AdvertiserDto> GetTop(int? Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       [Noqoush.Framework.Caching.Cachable()]
        List<CreativeVendorDto> GetByQuery(CreativeVendorCriteria criteria);
    }
}