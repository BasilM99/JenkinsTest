using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative
{
    [ServiceContract()]
    [CacheHeader("TileImageService", null)]
    public interface ITileImageService
    {
        /// <summary>
        /// use this service operation to get All  Title Image Dtos 
        /// </summary>
        /// <returns>List Title Image Dtos  </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
      //  [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<TileImageDto> GetAll();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TileImageSizeDto> GetAllSizes();


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
      //  [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        TileImageDto GetAllByAdAction(int adActionId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
      //  [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        TileImageSizeDto GetSizeByParentId(int parentTileImageSizeId);
    }
}
