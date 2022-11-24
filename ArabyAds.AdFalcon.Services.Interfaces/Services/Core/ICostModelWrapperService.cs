using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;

//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract]
    [CacheHeader("CostModelWrapperService", null, CacheStore = "MemoryCache")]
    public interface ICostModelWrapperService
    {
        /// <summary>
        /// use this service operation to get All costmodelwrappers
        /// </summary>
        /// <returns>List DeviceDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<CostModelWrapperDto> GetAll();
    }
}
