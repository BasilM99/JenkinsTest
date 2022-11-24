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

    [ServiceContract()]
    [CacheHeader("DocumentTypeService", typeof(IDocumentTypeService))]
    public interface IDocumentTypeService
    {
        /// <summary>
        /// use this service operation to get All Document Types
        /// </summary>
        /// <returns>List DocumentTypeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        IEnumerable<DocumentTypeDto> GetAll();



        /// <summary>
        /// use this service operation to get All Document Types
        /// </summary>
        /// <returns>List DocumentTypeDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        DocumentTypeDto GetByCode(string code);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsSelfCachable = true)]
        [NoAuthentication]
        int GetMIMEById(int Id);

    }
}
