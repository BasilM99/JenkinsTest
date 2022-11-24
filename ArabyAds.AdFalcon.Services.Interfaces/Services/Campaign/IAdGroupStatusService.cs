//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using System.Text;
//using Noqoush.AdFalcon.Domain.Common.Repositories;
//using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
//using Noqoush.Framework.Attributes;
//using Noqoush.Framework.Caching;
//using Noqoush.Framework.Caching;
//using Noqoush.Framework.Persistence;
////using Noqoush.Framework.WCF.ExceptionHandling;
//using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

//namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
//{
//    [ServiceContract()]
//    [CacheHeader("AdGroupStatusService", null)]
//    public interface IAdGroupStatusService
//    {
//        ///// <summary>
//        ///// use this service operation to get All  Ad Group Status Dtos 
//        ///// </summary>
//        ///// <returns>List Ad Group Status Dtos  </returns>
//        //[OperationContract]
//        ////[FaultContract(typeof(ServiceFault))]
//        //[Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
//        //IEnumerable<LookupDto> GetAll();
//    }
//}
