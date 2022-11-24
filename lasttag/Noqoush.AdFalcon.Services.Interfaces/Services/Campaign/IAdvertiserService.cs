
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.Caching;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.Framework.Attributes;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.EventBroker;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Campaign
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
    [CacheHeader("AdvertiserService", typeof(AdvertiserSearcher))]
    public interface IAdvertiserService
    {

        /// <summary>
        /// use this service operation to get All Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable(IsGetAll = true)]
        [NoAuthentication]

        IEnumerable<AdvertiserDto> GetAll();


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [Noqoush.Framework.Caching.Cachable(IsSelfCachable =true)]
        AdvertiserResult GetByQueryPagination(AdvertiserCriteria criteria);

        /// <summary>
        /// use this service operation to get Keyword by Id
        /// </summary>
        /// <returns>KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserDto Get(int id);

        ///// <summary>
        ///// use this service operation to get Top n Keyword
        ///// </summary>
        ///// <returns>List KeywordDto </returns>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //IEnumerable<AdvertiserDto> GetTop(int? Count);

        /// <summary>
        /// use this service operation to get Top n Keyword
        /// </summary>
        /// <returns>List KeywordDto </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [Noqoush.Framework.Caching.Cachable()]
        IEnumerable<AdvertiserDto> GetByQuery(AdvertiserCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountListResultDto GetAccountAdvertiser(AdvertiserAccountCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccount(AdvertiserAccountDto item);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool Delete(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void ValidateAdvertiser(int advertiserId, bool statusCheck = false);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void unArchive(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsSubUserHasWriteMode(int advertiseraccId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveAdvertiserAccountSettings(AdvertiserAccountSettings item);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountSettings GetAdvertiserAccountSettings(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool IsReadOrWriteAdvertiserAccount(int AdvertiserAccountId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteResultDto GetAdvertiserAccountMasterAppSite(AdvertiserAccountMasterAppSiteCriteria criteria);
      [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccountMasterAppSite(AdvertiserAccountMasterAppSiteDto item);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        bool DeleteAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        bool ActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeActivateAdvertiserAccountMasterAppSite(IEnumerable<int> Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountListDto GetAccountAdvertiserById(int Id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [EventBroker("Save Content List")]
        bool DeleteAdvertiserAccountMasterAppSiteItem(IEnumerable<int> Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SaveAdvertiserAccountMasterAppSiteItem(AdvertiserAccountMasterAppSiteItemDto item);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteItemResultDto GetAdvertiserAccountMasterAppSiteItem(AdvertiserAccountMasterAppSiteItemCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AdvertiserAccountMasterAppSiteDto GetAdvertiserAccountMasterAppSiteById(int Id);



        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        void SaveAudienceSegmentPerAdvertiser(AudienceSegmentDto item);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteAudienceSegmentPerAdvertiser(IEnumerable<int> Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AudienceSegmentResultResultDto GetAudienceSegmentsPerAdvertiser(AudienceSegmentCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool UnDeleteAudienceSegmentPerAdvertiser(IEnumerable<int> Ids);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AudienceSegmentDto GetAudienceSegmentDto(int Id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        int GetRootIdofFirstParty();


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [EventBroker("Save Content List")]
        bool DeActivatePixel(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        bool ActivatePixel(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
       // [EventBroker("Save Content List")]
        bool DeletePixel(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        //[EventBroker("Save Content List")]
        void SavePixel(PixelDto item);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PixelDto GetPixelById(int Id);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PixelResultDto GetPixel(PixelCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PixelResultDto GetPixelsPerAdvertiser(PixelCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        void SaveAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountReadOnlyUserDto> GetAdvertiserAccountReadOnlySettings(AdvertiserAccountSettingsForReadOnly item);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool DeleteAudienceSegmentPerAdvertiserForAdmin(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        void SaveAudienceSegmentPerAdvertiserForAdmin(AudienceSegmentDto item);

    }
}
