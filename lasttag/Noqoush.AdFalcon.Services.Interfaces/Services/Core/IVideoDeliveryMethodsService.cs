using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader("VideoDeliveryMethodLookup", null)]
    public interface IVideoDeliveryMethodsService
    {
        /// <summary>
        /// Get all Currencies
        /// </summary>
        /// <returns>all CurrencyDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<VideoDeliveryMethodDto> GetAll();
    }
}
