using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Linq.Expressions;
using Noqoush.Framework.Attributes;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Core
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
    [CacheHeader("DeviceService", typeof (DeviceSearcher))]
    public interface IDeviceService
    {
        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List DeviceDto </returns>
        [OperationContract]
        [FaultContract(typeof (ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<DeviceDto> GetAll();

        /// <summary>
        /// use this service operation to get list of Device Objects depend on the query
        /// </summary>
        /// <param name="criteria">query Text to search By</param>
        /// <returns>List of DeviceDto that match the query</returns>
        [OperationContract]
        [FaultContract(typeof (ServiceFault))]
        [Noqoush.Framework.Caching.Cachable()]
        IEnumerable<DeviceDto> SearchByQuery(string query);

        /// <summary>
        /// use this service operation to get Tree List of Device Objects  depend on the query
        /// </summary>
        /// <param name="criteria">query Text to search By</param>
        /// <param name="deviceTypeId">Filter the result base on this devicetypeid</param>
        /// <returns>Tree List of DeviceDto that match the query</returns>
        [OperationContract]
        [FaultContract(typeof (ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> SearchByQueryTree(int deviceTypeId, string query);

        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        [FaultContract(typeof (ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        IEnumerable<TreeDto> GetAllDeviceTree();

        /// <summary>
        /// use this service operation to get All Devices
        /// </summary>
        /// <returns>List TreeDto </returns>
        [OperationContract]
        [FaultContract(typeof (ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        IEnumerable<TreeDto> GetDeviceTree(int platformId, int deviceConstraint);
    }
}
