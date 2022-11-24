using ArabyAds.AdFalcon.Domain.Common.Repositories.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;
//using ArabyAds.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Account.PMP
{
   

    [ServiceContract()]
    public interface IPMPDealService
    {
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PMPDealDto GetDealPMPDeal(ValueMessageWrapper<int> Id);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        PMPDealSaveDto SavePMPDeal(PMPDealDto dto);
             [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ResultPMPDealDto QueryByCratiriaForPMPDeal(PMPDealCriteria criteria);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        ValueMessageWrapper<bool> Delete(IEnumerable<int> Ids);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]

        IList<Interfaces.DTOs.Core.TreeDto> GetAdFormatsTree(IList<int> AdFormats);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<PMPDealDto> GetAllPMPDealsByAccount(ValueMessageWrapper<int> AccountId);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<CampaignDto> getCampsBydeal(ValueMessageWrapper<int> dealid);

        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdGroupDto> getDealCampsAdgruops(GetDealCampsAdgroupsRequest request);


        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountDto> getAdvertiserAccountsBydeal(ValueMessageWrapper<int> dealid);
       
        [OperationContract]
        IList<DropDownDto> GetAllPMPDealsByUserAndAdvertiser(ValueMessageWrapper<int> AdvertiserAccountId);
        [OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        CampaignListResultDto getCampsAdvertiserBydeal(GetCampsAdvertiserBydealRequest request);
        
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //bool SaveTargeting(PMPTargetingSaveDto targetingSaveDto);
        //[OperationContract]
        ////[FaultContract(typeof(ServiceFault))]
        //PMPTargetingGetDto GetTargeting(int dealId);
    }
}