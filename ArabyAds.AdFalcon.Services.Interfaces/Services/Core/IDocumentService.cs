using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.Framework.Attributes;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{
    [ServiceContract()]
    public interface IDocumentService
    {
        #region Document
        ///// <summary>
        ///// use this service operation to get list of Documents Object depend on the criteria
        ///// </summary>
        ///// <param name="criteria">criteria to Query By</param>
        ///// <returns>List of DocumentDto that match the criteria</returns>
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //IEnumerable<DocumentDto> QueryByCratiria(DocumentCriteria criteria);

        /// <summary>
        /// use this service operation to Delete List of Documents using Ids
        /// </summary>
        /// <param name="documentIds">List of Document Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> Delete(IEnumerable<int> documentIds);

        /// <summary>
        /// use this service operation to Insert/Update Document Object
        /// </summary>
        /// <param name="document">Hold the Information that Will be Inserted/Updated</param>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> Save(DocumentDto document);

        /// <summary>
        /// use this service operation to get list of Documents Object depend on the Id
        /// </summary>
        /// <param name="documentId">Id to Get By</param>
        /// <returns>DocumentListDto that match the Id</returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        DocumentDto Get(ValueMessageWrapper<int> documentId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [NoAuthentication]
        ValueMessageWrapper<int> SaveFromInputPath(DocumentDto document);


        #endregion
    }
}
