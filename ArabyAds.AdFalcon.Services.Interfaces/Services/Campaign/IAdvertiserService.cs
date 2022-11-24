
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.Caching;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.Framework.Attributes;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework.EventBroker;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign
{

    public class AdvertiserSearcher
    {
        public static IEnumerable<AdvertiserDto> GetByQuery(IEnumerable<AdvertiserDto> items, AdvertiserCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.Culture))
            {
                if(!string.IsNullOrEmpty(criteria.Value))
                return items.Where(x => x.Name.Value.StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
                else
                    return items;
            }
            else
            {
                var result = items.Where(x => x.Name.GetValue(criteria.Culture).StartsWith(criteria.Value, StringComparison.OrdinalIgnoreCase));
                if (result != null)
                {
                    foreach (var item in result)
                    {

                        item.Name.DefaultCulture = criteria.Culture;
                    }
                }
                return result;
            }
        }
    }

    [ServiceContract()]
    [CacheHeader(LookupNames.Advertiser, typeof(AdvertiserSearcher), CacheStore = "MemoryCache", EnableLocalCacheInvalidation = true)]
    public interface IAdvertiserService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<AdvertiserDto> GetAll();


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
       // [ArabyAds.Framework.Caching.Cachable(IsSelfCachable =true)]
        AdvertiserResult GetByQueryPagination(AdvertiserCriteria criteria);

        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserDto Get(ValueMessageWrapper<int> id);

        ///// <summary>
        ///// use this service operation to get Top n Keyword
        ///// </summary>
        ///// <returns>List KeywordDto </returns>
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //IEnumerable<AdvertiserDto> GetTop(int? Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        [ArabyAds.Framework.Caching.Cachable()]
        IEnumerable<AdvertiserDto> GetByQuery(AdvertiserCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountListResultDto GetAccountAdvertiser(AdvertiserAccountCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccount(AdvertiserAccountDto item);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> Delete(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void ValidateAdvertiser(ValidateAdvertiserRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void unArchive(ValueMessageWrapper<int> id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsSubUserHasWriteMode(ValueMessageWrapper<int> advertiseraccId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveAdvertiserAccountSettings(AdvertiserAccountSettings item);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountSettings GetAdvertiserAccountSettings(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> IsReadOrWriteAdvertiserAccount(ValueMessageWrapper<int> AdvertiserAccountId);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteResultDto GetAdvertiserAccountMasterAppSite(AdvertiserAccountMasterAppSiteCriteria criteria);
      [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccountMasterAppSite(AdvertiserAccountMasterAppSiteDto item);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        ValueMessageWrapper<bool> DeleteAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        ValueMessageWrapper<bool> ActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountListDto GetAccountAdvertiserById(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        // [EventBroker("Save Content List")]
        ValueMessageWrapper<bool> DeleteAdvertiserAccountMasterAppSiteItem(IEnumerable<int> Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccountMasterAppSiteItem(AdvertiserAccountMasterAppSiteItemDto item);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteItemResultDto GetAdvertiserAccountMasterAppSiteItem(AdvertiserAccountMasterAppSiteItemCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteDto GetAdvertiserAccountMasterAppSiteById(ValueMessageWrapper<int> Id);



        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        void SaveAudienceSegmentPerAdvertiser(AudienceSegmentDto item);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteAudienceSegmentPerAdvertiser(IEnumerable<int> Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AudienceSegmentResultResultDto GetAudienceSegmentsPerAdvertiser(AudienceSegmentCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> UnDeleteAudienceSegmentPerAdvertiser(IEnumerable<int> Ids);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        AudienceSegmentDto GetAudienceSegmentDto(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetRootIdofFirstParty();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<int> GetContextualRootIdofFirstParty(string contextualFirstPartyCode);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        // [EventBroker("Save Content List")]
        ValueMessageWrapper<bool> DeActivatePixel(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        ValueMessageWrapper<bool> ActivatePixel(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        // [EventBroker("Save Content List")]
        ValueMessageWrapper<bool> DeletePixel(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SavePixel(PixelDto item);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PixelDto GetPixelById(ValueMessageWrapper<int> Id);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PixelResultDto GetPixel(PixelCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PixelResultDto GetPixelsPerAdvertiser(PixelCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        void SaveAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountReadOnlyUserDto> GetAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> DeleteAudienceSegmentPerAdvertiserForAdmin(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        void SaveAudienceSegmentPerAdvertiserForAdmin(AudienceSegmentDto item);

        [OperationContract]
        AdvertiserAccountListDto GetAccountAdvertiserInfoById(ValueMessageWrapper<int> Id);

        [OperationContract]
        ValueMessageWrapper<bool> IsSubUserHasReadMode(ValueMessageWrapper<int> advertiseraccId);
    }
}
