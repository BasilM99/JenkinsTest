
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Runtime.Serialization;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract]
    [CacheHeader("AppMarketingPartnersLookup", null)]
    public interface IAppMarketingPartnerService
    {
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [NoAuthentication]
        [FaultContract(typeof(ServiceFault))]
        [Cachable(IsGetAll = true)]
        IEnumerable<AppMarketingPartnerDto> GetAll();
    }
}
