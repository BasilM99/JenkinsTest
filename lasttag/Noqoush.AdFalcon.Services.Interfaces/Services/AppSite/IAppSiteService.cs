using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Security.Permissions;
using Noqoush.Framework.Attributes;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.AppSite
{
    [ServiceContract()]
    public interface IAppSiteService
    {
        /// <summary>
        /// use this service operation to get list of AppSites Object depend on the criteria
        /// </summary>
        /// <param name="criteria">criteria to Query By</param>
        /// <returns>AppSiteListResultDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        SubAppSiteListResultDto SearchSubAppsites(AllAppSiteCriteria criteria);
        /// <summary>
        /// use this service operation to get list of AppSites Object depend on the criteria
        /// </summary>
        /// <param name="criteria">criteria to Query By</param>
        /// <returns>AppSiteListResultDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteListResultDto QueryByCratiria(AppSiteCriteriaBase criteria);


        /// <summary>
        /// use this service operation to get list of AppSites Object depend on the AppOps criteria
        /// </summary>
        /// <param name="criteria">criteria to Query By</param>
        /// <returns>AppSiteListResultDto that match the criteria</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteListResultDto QueryByAppOpsCratiria(AppSiteCriteria criteria);

        /// <summary>
        /// use this service operation to get all Active AppSites
        /// </summary>
        /// <returns>All Active AppSites</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteListResultDtoBase GetAllActive(AllAppSiteCriteria criteria);

        /// <summary>
        /// use this service operation to Delete List of AppSites using Ids
        /// </summary>
        /// <param name="appSiteIds">List of AppSite Ids to be deleted</param>
        /// <returns>true id the Delete OPration is successes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Delete App")]
        bool Delete(IEnumerable<int> appSiteIds);

        /// <summary>
        /// use this service operation to Insert/Update AppSite Object
        /// </summary>
        /// <param name="appSite">Hold the Information that Will be Inserted/Updated</param>
        /// <returns>Object Hold the AppSite id and PublisherId after the Inserting/Updating Operation</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("Save App")]
        SaveAppSiteDtoResult Save(AppSiteDto appSite);

        /// <summary>
        /// use this service operation to get AppSite Object depend on the Id
        /// </summary>
        /// <param name="appSiteId">Id to Get By</param>
        /// <returns>AppSiteListDto that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteDto Get(int appSiteId);

        /// <summary>
        /// use this service operation to get AppSiteBasic Info Object depend on the Id
        /// </summary>
        /// <param name="appSiteId">Id to Get By</param>
        /// <returns>AppSiteListDtoBase that match the Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteDtoBase GetBasicInfo(int appSiteId);

        /// <summary>
        /// use this service operation to get  AppSites Settings Object depend on the Id
        /// </summary>
        /// <param name="appSiteId">App Site Id to Get By</param>
        /// <returns>Settings Dto that match the AppSite Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        SettingsDto GetSettings(int appSiteId);

        /// <summary>
        /// use this service operation to Save AppSites Settings Object 
        /// </summary>
        /// <param name="appSiteId">AppSites Settings Object to be saved</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveSettings(SettingsDto settings);



        /// <summary>
        /// use this service operation to get  AppSites Admin Configurations Object depend on the Id
        /// </summary>
        /// <param name="appSiteId">App Site Id to Get By</param>
        /// <returns>Admin Configurations Dto that match the AppSite Id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteAdminConfigDto GetServerSettings(int appSiteId);

        /// <summary>
        /// use this service operation to Save AppSites Admin Configurations Object 
        /// </summary>
        /// <param name="siteAdminConfig">AppSites Admin Configurations Object to be saved</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveServerSettings(AppSiteAdminConfigDto siteAdminConfig);


        /// <summary>
        /// update appsite status by Ids
        /// </summary>
        /// <param name="appSiteApprovalDto">AppSite Approval Information</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        [EventBroker("AppSiteApprove")]
        void Approval(AppSiteApprovalDto appSiteApprovalDto);

        /// <summary>
        /// Get text filters for this appsite
        /// </summary>
        /// <param name="appSiteId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<TextFilterDto> GetAppSiteTextFilters(int appSiteId);

        /// <summary>
        /// Delete filter asociated with this appsite
        /// </summary>
        /// <param name="filterId">FilterId</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteAppsiteFilter(int filterId);

        /// <summary>
        /// Update textfilter associated with this AppSiteId
        /// </summary>
        /// <param name="textFilterDto"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool UpdateAppSiteTextFilter(TextFilterDto textFilterDto, int appsiteId);

        /// <summary>
        /// Add appsite textfilter
        /// </summary>
        /// <param name="textFilterDto"></param>
        /// <param name="appSiteId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool AddAppSiteTextFilter(int appSiteId, TextFilterDto textFilterDto);


        /// <summary>
        /// Get url filters for this appsite
        /// </summary>
        /// <param name="appSiteId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<UrlFilterDto> GetAppSiteUrlFilters(int appSiteId);

        /// <summary>
        /// Update url filter for this appsite
        /// </summary>
        /// <param name="urlFilterDto"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool UpdateAppSiteUrlFilter(UrlFilterDto urlFilterDto, int appSiteId);


        /// <summary>
        /// Add appsite urlFilter
        /// </summary>
        /// <param name="urlFilterDto"></param>
        /// <param name="appSiteId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool AddAppSiteUrlFilter(int appSiteId, UrlFilterDto urlFilterDto);

        /// <summary>
        /// Add appsite urlFilter
        /// </summary>
        /// <param name="appSiteId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<LanguageFilterDto> GetAppSiteLanguageFilters(int appSiteId);

        /// <summary>
        /// Get Sub Appsites
        /// </summary>
        /// <param name="appSiteId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<SubAppsiteDto> GetSubAppsites(int appSiteId, string subPublisherId);


        /// <summary>
        /// Add appsite languagefilter
        /// </summary>
        /// <param name="languageFilterDto"></param>
        /// <param name="appSiteId"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool AddAppSiteLanguageFilter(int appSiteId, LanguageFilterDto languageFilterDto);

        /// <summary>
        /// update appsite languagefilter
        /// </summary>
        /// <param name="languageFilterDto"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool UpdateAppSiteLanguageFilter(LanguageFilterDto languageFilterDto, int appSiteId);

        /// <summary>
        /// Return all the appsites for this account
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<AppSiteDto> GetAppSitesByAccountId(int accountId);

        /// <summary>
        /// Get TreeDto of appsites for this account
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<TreeDto> GetAppSitesTreeByAccountId(int accountId);

        [NoAuthentication]
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        AppSiteBasicDto GetAppSiteByPublisherId(string publisherId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        BasicAppSiteInformation GetPrimaryUserBasicInformation(int appsiteId);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool CehckAppsitCostModelCompatableWitCampaigns(int appSiteId, int appSiteCostModel, out  IList<CampaignBidConfigDto> notCompatibleAppSiteList);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        SubAppSiteListResultDto QueryByCratiriaForSubAppsites(AllAppSiteCriteria criteria);
      

    }
}
