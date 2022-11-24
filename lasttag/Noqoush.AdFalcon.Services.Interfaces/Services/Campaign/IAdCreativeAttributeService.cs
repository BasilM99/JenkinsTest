using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    [CacheHeader("AdCreativeAttributesService", null)]
    public interface IAdCreativeAttributeService
    {
        /// <summary>
        /// Get All AdCreativeAttribute
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [Cachable(IsGetAll = true)]
        List<AdCreativeAttributeDto> GetAll();
    }
}
