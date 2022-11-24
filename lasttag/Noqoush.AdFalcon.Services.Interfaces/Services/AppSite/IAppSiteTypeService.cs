using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    [CacheHeader("AppSiteTypeService", null)]
    public interface IAppSiteTypeService
    {
        /// <summary>
        /// use this service operation to get All  App Site Type Dtos 
        /// </summary>
        /// <returns>List App Site Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<AppSiteTypeDto> GetAll();

        /// <summary>
        /// use this service operation to get   App Site Type Dtos by id
        /// </summary>
        /// <returns> App Site Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        AppSiteTypeDto Get(int id);
    }
}
