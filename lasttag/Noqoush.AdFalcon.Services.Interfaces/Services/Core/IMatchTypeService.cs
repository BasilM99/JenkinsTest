using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract]
    [CacheHeader("MatchTypeList", null)]
    public interface IMatchTypeService
    {
        /// <summary>
        /// Get all match types
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        List<LookupDto> GetAll();
    }
}
