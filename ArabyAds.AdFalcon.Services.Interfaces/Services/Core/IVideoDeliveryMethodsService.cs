using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader("VideoDeliveryMethodLookup", null, CacheStore = "MemoryCache")]
    public interface IVideoDeliveryMethodsService
    {
        /// <summary>
        /// Get all Currencies
        /// </summary>
        /// <returns>all CurrencyDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<VideoDeliveryMethodDto> GetAll();
    }
}
