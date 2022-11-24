using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract()]
    [CacheHeader("ThemeService", null, CacheStore = "MemoryCache")]
    public interface IThemeService
    {
        /// <summary>
        /// use this service operation to get All non-Custom themes 
        /// </summary>
        /// <returns>List Non-Custom ThemeDtos </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<ThemeDto> GetAll();

        /// <summary>
        /// use this service operation to get  non-Custom themes by id
        /// </summary>
        /// <returns>Non-Custom ThemeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = false)]
        ThemeDto Get(ValueMessageWrapper<int> id);
    }
}
