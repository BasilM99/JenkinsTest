using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Noqoush.Framework.Attributes;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces
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
