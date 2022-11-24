
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract]
    [CacheHeader("TrackingEvent", null, CacheStore = "MemoryCache")]
    public interface ITrackingEventService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TrackingEventDto> GetCostModelEvents();
    }
}
