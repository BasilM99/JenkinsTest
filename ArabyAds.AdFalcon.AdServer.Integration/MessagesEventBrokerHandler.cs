using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using ArabyAds.AdFalcon.AdServer.Integration.Services.Interfaces;
using ArabyAds.Framework.EventBroker;

namespace ArabyAds.AdFalcon.AdServer.Integration
{
    public class MessagesEventBrokerHandler : ISubscriberHandler
    {

        public void HandleEvent(EventArgsBase args)
        {
            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:AdServer MessagesEventBroker handling Event {1} , MessagesEventBroker", "Event Broker", args.EventName);
            //Handel Events

            //call handler WCF
            try
            {
                var serivce = Framework.IoC.Instance.Resolve<IMessagesEventBrokerService>();
                serivce.HandelEvent(args);
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in AdServer MessagesEventBroker handling Event {1} , MessagesEventBroker , \n message:{2}", "Event Broker", args.EventName, ex.ToString());
                throw;
            }
            switch (args.EventName)
            {
                case "":
                    {
                        break;
                    }
            }
        }


      

    }
}
