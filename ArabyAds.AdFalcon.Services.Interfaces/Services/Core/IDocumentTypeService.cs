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
using ArabyAds.Framework;


namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{

    [ServiceContract()]
    [CacheHeader("DocumentTypeService", typeof(IDocumentTypeService), CacheStore = "MemoryCache")]
    public interface IDocumentTypeService
    {
        /// <summary>
        /// use this service operation to get All Document Types
        /// </summary>
        /// <returns>List DocumentTypeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<DocumentTypeDto> GetAll();



        /// <summary>
        /// use this service operation to get All Document Types
        /// </summary>
        /// <returns>List DocumentTypeDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        DocumentTypeDto GetByCode(string code);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        ValueMessageWrapper<int> GetMIMEById(ValueMessageWrapper<int> Id);

    }
}
