
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
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
        //[ArabyAds.Framework.Caching.Cachable(IsSelfCachable = true, IsGetAll =true)]
        [NoAuthentication]
        List<TreeDto> GetAll(ValueMessageWrapper<int?> CampaignId);
        [OperationContract]
     //   [ArabyAds.Framework.Caching.Cachable()]

        List<TreeDto> GetByDataProvider(GetByDataProviderRequest request);

        [OperationContract]
        AudienceSegmentDto get(ValueMessageWrapper<int> id);
        [OperationContract]

        ValueMessageWrapper<bool> Save(AudienceSegmentDto obj);

        [OperationContract]
        List<TreeDto> GetByDataProviderWithPrice(GetByDataProviderRequest request);
        [OperationContract]
        string getAudianceSegmentsByDataProviderForExternal(GetByDataProviderExtRequest request);

        [OperationContract]
        IList<AudienceSegmentDto>   getAudianceSegmentsByDataProvider(GetAudienceSegByDataProviderRequest request);

        [OperationContract]
        IList<AudienceSegmentDto> getAudianceSegmentsByDataProviderToWrite(GetAudienceSegByDataProviderRequest request);

        [OperationContract]
        List<TreeDto> GetAllForContextual(ValueMessageWrapper<int?> CampainId);
        [OperationContract]
        AudienceSegmentDto getContextual(ValueMessageWrapper<int> id);

        [OperationContract]
        List<TreeDto> GetAllForContextualBrandSafty(ValueMessageWrapper<int?> CampainId);



    }
}
