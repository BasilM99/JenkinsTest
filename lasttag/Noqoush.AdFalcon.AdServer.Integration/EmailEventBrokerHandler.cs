using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.Framework.DomainServices.EventBroker;
using Noqoush.Framework.EventBroker;

namespace Noqoush.AdFalcon.AdServer.Integration
{
    public class EmailEventBrokerHandler : ISubscriberHandler
    {

        public void HandleEvent(EventArgsBase args)
        {
            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:AdServer Email Sender handling Event {1} , EmailEventBrokerHandler", "Event Broker", args.EventName);
            //Handel Events
      
            //call handler WCF
            try { var serivce = Framework.IoC.Instance.Resolve<IEmailHandlerService>();
                serivce.HandelEvent(args); }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in AdServer Email Sender handling Event {1} , EmailEventBrokerHandler , \n message:{2}", "Event Broker", args.EventName, ex.ToString());
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
