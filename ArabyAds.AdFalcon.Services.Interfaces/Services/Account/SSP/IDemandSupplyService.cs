using System.Collections.Generic;
using System.ServiceModel;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.EventBroker;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account.SSP
{
    [ServiceContract()]
    public interface ISupplyService
    {
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultSiteZoneMapping QueryByCratiriaForSiteZoneMapping(SiteZoneMappingCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultFloorPriceConfigDto QueryByCratiriaForFloorPrice(FloorPriceCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultSiteZoneDto QueryByCratiriaForSiteZone(SiteZoneCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultPartnerSiteDto QueryByCratiriaForSitePartner(PartnerSiteCriteria criteria);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultPartnerDto QueryByCratiriaForPartner(PartnerCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveSitePartner(PartnerSiteDto dto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveSiteZone(SiteZoneDto dto);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteSitePartner(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteSiteZone(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteBusinessPartner(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteSiteZoneMapping(int[] Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteFloorPrice(int[] Ids);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        SiteZoneDto GetSiteZone(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PartnerSiteDto GetSitePartner(ValueMessageWrapper<int> Id );
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        FloorPriceConfigDto GetFloorPrice(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveFloorPrice(FloorPriceConfigDto dto);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        LookupListResultDto GetBidConfigTypeList();

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        SiteZoneMappingDto GetSiteZoneMapping(ValueMessageWrapper<int> Id);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //void SaveSiteZoneMapping(SiteZoneMappingDto dto , int[] appSiteIds);

          [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        void SaveSiteZoneMapping(SaveSiteZoneMappingRequest request);

        
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultDealCampaignMappingDto QueryByCratiriaForDealCampaignMapping(DealCampaignMappingCriteria criteria);


         [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         DealCampaignMappingDto GetDealCampaignMapping(ValueMessageWrapper<int> Id);
         [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         void SaveDealCampaignMapping(DealCampaignMappingDto dto);
         [OperationContract]
         //[FaultContract(typeof(ServiceFault))]
         void DeleteDealCampaignMapping(int[] Ids);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        FloorPriceConfigDto GetBaseFloorPrice(GetBaseFloorPriceRequest request);
    }
}
