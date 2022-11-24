using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("RichMediaRequiredProtocolService", null)]
    public interface IRichMediaRequiredProtocolService
    {
        /// <summary>
        /// use this service operation to get All  RichMedia Required Protocol Dtos 
        /// </summary>
        /// <returns>List RichMedia Protocol Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<RichMediaRequiredProtocolDto> GetAll();
    }
}
