using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Response;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Text;


using System.Linq.Expressions;
using ArabyAds.Framework.Attributes;
using System.ServiceModel;

namespace ArabyAds.AdFalcon.Services.Interfaces.Services.Core
{
    [ServiceContract()]

    public interface IKPIService
    {
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsForCampaigns(ValueMessageWrapper<bool> isDefault);
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsForAdvertisers(ValueMessageWrapper<bool> isDefault);
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsForDeals(ValueMessageWrapper<bool> isDefault);

        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsForPublishers(ValueMessageWrapper<bool> isDefault);
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsForDataProviders(ValueMessageWrapper<bool> isDefault);
        [OperationContract]

        KPIConfigDto GetKPIConfig(ValueMessageWrapper<int> id);
        [OperationContract]

        IList<KPIDTO> GetKPIs(GetKPIRequest request);





        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsAllForCampaigns();
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsAllForAdvertisers();
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsAllForDeals();

        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsAllForPublishers();
        [OperationContract]

        IList<KPIConfigDto> GetKPIConfigsAllForDataProviders();
    }
}
