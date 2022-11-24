using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.Framework.Attributes;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("CreativeUnitService", null)]
    public interface ICreativeUnitService
    {
        /// <summary>
        /// use this service operation to get All  Creative Unit Dtos 
        /// </summary>
        /// <returns>List Creative Unit Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]
        IEnumerable<CreativeUnitDto> GetAll();

        /// <summary>
        /// use this service operation to get All  Creative Unit Dtos depends on criteria
        /// </summary>
        /// <returns>List Creative Unit Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<CreativeUnitDto> GetBy(DeviceTypeEnum deviceType, AdTypeIds? adType, AdSubTypes? adSubType, string group);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
//        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<CreativeUnitDto> GetByCriteria(int? creativeUnitId, int deviceTypeId, string group, int? adTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        CreativeUnitDto GetById(int id);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<string> GetAllSupportedFormat();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        List<CreativeUnitDto> GetByGroupCode(string group);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        string GetGroupByCreativeCode(string CreativeCode);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        string GetGroupByCreativeByID(int CreativeId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        List<CreativeUnitDto> GetByCriteriaWidthHeight(int? creativeUnitId, int deviceTypeId, string group, int? adTypeId, int Width, int Height);
    }
}
