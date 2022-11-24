using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework.Persistence;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    [CacheHeader("ObjectiveTypeService", null, CacheStore = "MemoryCache")]
    public interface IObjectiveTypeService
    {
        /// <summary>
        /// use this service operation to get All  Objective Type Dtos
        /// </summary>
        /// <returns>List ObjectiveType Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
       [ArabyAds.Framework.Caching.Cachable(IsGetAll = false,IsSelfCachable =true)]
        IEnumerable<ObjectiveTypeDto> GetAll();

        [OperationContract]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = false, IsSelfCachable = true)]
        IEnumerable<AdActionTypeDto> GetAdActionTypeAllForWeb();
    }
}
