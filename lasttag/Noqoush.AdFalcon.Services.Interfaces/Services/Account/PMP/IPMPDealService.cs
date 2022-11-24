using Noqoush.AdFalcon.Domain.Common.Repositories.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.Framework.WCF.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.Services.Account.PMP
{
   

    [ServiceContract()]
    public interface IPMPDealService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PMPDealDto GetDealPMPDeal(int Id);
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        PMPDealSaveDto SavePMPDeal(PMPDealDto dto);
             [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        ResultPMPDealDto QueryByCratiriaForPMPDeal(PMPDealCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        bool Delete(IEnumerable<int> Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]

        IList<Interfaces.DTOs.Core.TreeDto> GetAdFormatsTree(IList<int> AdFormats);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<PMPDealDto> GetAllPMPDealsByAccount(int AccountId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<CampaignDto> getCampsBydeal(int dealid);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdGroupDto> getDealCampsAdgruops(int dealId, int campId);


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        IList<AdvertiserAccountDto> getAdvertiserAccountsBydeal(int dealid);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        CampaignListResultDto getCampsAdvertiserBydeal(int dealid, int advertiserId);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //bool SaveTargeting(PMPTargetingSaveDto targetingSaveDto);
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //PMPTargetingGetDto GetTargeting(int dealId);
    }
}