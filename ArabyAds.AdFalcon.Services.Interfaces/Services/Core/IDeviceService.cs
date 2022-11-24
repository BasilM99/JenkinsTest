using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Linq.Expressions;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    public class DeviceSearcher
    {
        private static bool DeviceMatch(DeviceDto deviceDto, string query)
        {
            var words = query.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return true;
        }
        public static IEnumerable<DeviceDto> SearchByQuery(IEnumerable<DeviceDto> items, string query)
        {
            return items.Where(c => DeviceMatch(c, query));
        }
    }

    [ServiceContract()]
    [CacheHeader(LookupNames.Device, typeof (DeviceSearcher), CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IDeviceService
    {
        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List DeviceDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<DeviceDto> GetAll();

        /// <summary>
        /// use this service operation to get list of Device Objects depend on the query
        /// </summary>
        /// <param name="criteria">query Text to search By</param>
        /// <returns>List of DeviceDto that match the query</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable()]
        IEnumerable<DeviceDto> SearchByQuery(string query);

        /// <summary>
        /// use this service operation to get Tree List of Device Objects  depend on the query
        /// </summary>
        /// <param name="criteria">query Text to search By</param>
        /// <param name="deviceTypeId">Filter the result base on this devicetypeid</param>
        /// <returns>Tree List of DeviceDto that match the query</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> SearchByQueryTree(SearchByQueryTreeRequest request);

        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<TreeDto> GetAllDeviceTree();

        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetDeviceTree(GetDeviceTreeRequest request);


        [OperationContract]

        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]

        IEnumerable<DeviceDto> SearchByQueryandDeviceType(SearchByQueryTreeRequest request);
    }
}
