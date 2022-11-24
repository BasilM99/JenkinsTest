using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("CreativeUnitService", null,CacheStore = "MemoryCache")]
    public interface ICreativeUnitService
    {
        /// <summary>
        /// use this service operation to get All  Creative Unit Dtos 
        /// </summary>
        /// <returns>List Creative Unit Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]
        IEnumerable<CreativeUnitDto> GetAll();

        /// <summary>
        /// use this service operation to get All  Creative Unit Dtos depends on criteria
        /// </summary>
        /// <returns>List Creative Unit Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<CreativeUnitDto> GetBy(GetCreativeUnitRequest request);

        /// <returns>List Creative Unit Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]

        IEnumerable<CreativeUnitDto> GetAllBy();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        List<CreativeUnitDto> GetByCriteria(GetCreativeUnitByCriteriaRequest request);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        CreativeUnitDto GetById(ValueMessageWrapper<int> id);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<string> GetAllSupportedFormat();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        List<CreativeUnitDto> GetByGroupCode(string group);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        string GetGroupByCreativeCode(string CreativeCode);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        string GetGroupByCreativeByID(ValueMessageWrapper<int> CreativeId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        List<CreativeUnitDto> GetByCriteriaWidthHeight(GetCreativeUnitByCriteriaWithDimensionsRequest request);


        [OperationContract]
       [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]

        List<CreativeUnitDto> GetByCriteriaWithouDeviceType(GetCreativeUnitByCriteriaRequest request);
    
    }
}
