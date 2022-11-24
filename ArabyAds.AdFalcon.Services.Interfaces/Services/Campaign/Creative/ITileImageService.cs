using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("TileImageService", null, CacheStore = "MemoryCache")]
    public interface ITileImageService
    {
        /// <summary>
        /// use this service operation to get All  Title Image Dtos 
        /// </summary>
        /// <returns>List Title Image Dtos  </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
       [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<TileImageDto> GetAll();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        // [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [Cachable(IsGetAll = false, IsSelfCachable = true)]
        IEnumerable<TileImageSizeDto> GetAllSizes();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //  [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [Cachable(IsGetAll = false, IsSelfCachable = true)]
        TileImageDto GetAllByAdAction(ValueMessageWrapper<int> adActionId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //  [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [Cachable(IsGetAll = false, IsSelfCachable = true)]
        TileImageSizeDto GetSizeByParentId(ValueMessageWrapper<int> parentTileImageSizeId);
    }
}
