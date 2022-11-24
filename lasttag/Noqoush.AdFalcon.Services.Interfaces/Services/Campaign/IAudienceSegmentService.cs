
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Runtime.Serialization;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
{
   

    [ServiceContract]
    //[CacheHeader("AudienceSegmentService", null)]
    public interface IAudienceSegmentService
    {
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        //[Noqoush.Framework.Caching.Cachable(IsSelfCachable = true, IsGetAll =true)]
        [NoAuthentication]
        List<TreeDto> GetAll(int? CampaignId);
        [OperationContract]
     //   [Noqoush.Framework.Caching.Cachable()]

        List<TreeDto> GetByDataProvider(int Id, bool showNotSelectable = false);

        [OperationContract]
        AudienceSegmentDto get(int id);
        [OperationContract]

        bool Save(AudienceSegmentDto obj);

        [OperationContract]
        List<TreeDto> GetByDataProviderWithPrice(int Id, bool showNotSelectable = false);
        [OperationContract]
        string getAudianceSegmentsByDataProviderForExternal(int Id, int IdAccAdv);

        [OperationContract]
        IList<AudienceSegmentDto>   getAudianceSegmentsByDataProvider(int Id, string q,  string cultures);

        [OperationContract]
        IList<AudienceSegmentDto> getAudianceSegmentsByDataProviderToWrite(int Id, string q, string cultures);
    }
}
