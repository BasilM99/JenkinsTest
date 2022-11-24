using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Integration;
using ArabyAds.Framework.Attributes;
using ArabyAds.Framework.EventBroker;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.AdServer.Integration.Services.Interfaces
{
    [ServiceContract()]
    public interface IEmailHandlerService
    {
        [OperationContract()]
        [NoAuthentication]
        void HandelEvent(EventArgsBase args);
    }



    [ServiceContract()]
    public interface IMessagesEventBrokerService
    {
        [OperationContract()]
        [NoAuthentication]
        void HandelEvent(EventArgsBase args);
    }
}
