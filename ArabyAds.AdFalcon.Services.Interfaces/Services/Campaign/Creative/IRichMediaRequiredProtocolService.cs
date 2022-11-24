using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("RichMediaRequiredProtocolService", null, CacheStore = "MemoryCache")]
    public interface IRichMediaRequiredProtocolService
    {
        /// <summary>
        /// use this service operation to get All  RichMedia Required Protocol Dtos 
        /// </summary>
        /// <returns>List RichMedia Protocol Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<RichMediaRequiredProtocolDto> GetAll();
    }
}
