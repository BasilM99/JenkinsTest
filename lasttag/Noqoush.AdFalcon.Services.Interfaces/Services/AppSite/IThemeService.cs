using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.Services.Interfaces.Services
{
    [ServiceContract()]
    [CacheHeader("ThemeService", null)]
    public interface IThemeService
    {
        /// <summary>
        /// use this service operation to get All non-Custom themes 
        /// </summary>
        /// <returns>List Non-Custom ThemeDtos </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<ThemeDto> GetAll();

        /// <summary>
        /// use this service operation to get  non-Custom themes by id
        /// </summary>
        /// <returns>Non-Custom ThemeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = false)]
        ThemeDto Get(int id);
    }
}
