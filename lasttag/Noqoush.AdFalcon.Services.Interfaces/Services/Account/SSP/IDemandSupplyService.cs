using System.Collections.Generic;
using System.ServiceModel;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.SSP
{
    [ServiceContract()]
    public interface ISupplyService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultSiteZoneMapping QueryByCratiriaForSiteZoneMapping(SiteZoneMappingCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultFloorPriceConfigDto QueryByCratiriaForFloorPrice(FloorPriceCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultSiteZoneDto QueryByCratiriaForSiteZone(SiteZoneCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultPartnerSiteDto QueryByCratiriaForSitePartner(PartnerSiteCriteria criteria);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultPartnerDto QueryByCratiriaForPartner(PartnerCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveSitePartner(PartnerSiteDto dto);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveSiteZone(SiteZoneDto dto);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteSitePartner(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteSiteZone(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteBusinessPartner(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteSiteZoneMapping(int[] Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void DeleteFloorPrice(int[] Ids);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        SiteZoneDto GetSiteZone(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PartnerSiteDto GetSitePartner(int Id );
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        FloorPriceConfigDto GetFloorPrice(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveFloorPrice(FloorPriceConfigDto dto);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetBidConfigTypeList();

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        SiteZoneMappingDto GetSiteZoneMapping(int Id);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //void SaveSiteZoneMapping(SiteZoneMappingDto dto , int[] appSiteIds);

          [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        void SaveSiteZoneMapping(SiteZoneMappingDto dto, IList<AssignedAppsitesDto> appSites);

        
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultDealCampaignMappingDto QueryByCratiriaForDealCampaignMapping(DealCampaignMappingCriteria criteria);


         [OperationContract]
         [FaultContract(typeof(ServiceFault))]
         DealCampaignMappingDto GetDealCampaignMapping(int Id);
         [OperationContract]
         [FaultContract(typeof(ServiceFault))]
         void SaveDealCampaignMapping(DealCampaignMappingDto dto);
         [OperationContract]
         [FaultContract(typeof(ServiceFault))]
         void DeleteDealCampaignMapping(int[] Ids);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        FloorPriceConfigDto GetBaseFloorPrice(int SiteId, int ZoneId);
    }
}
