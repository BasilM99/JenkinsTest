using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Noqoush.Framework;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Persistence.NHibernate;
using Noqoush.Framework.Security;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Host
{
    public class Global : Noqoush.Framework.Web.HttpApplicationBase
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Noqoush.Framework.IoC.Instance.GetType();
            Noqoush.Framework.EventBroker.EventBroker.Instance.GetType();
            Assembly[] assemblies = new Assembly[] { Assembly.Load("Noqoush.AdFalcon.Persistence"), Assembly.Load("Noqoush.Framework.DomainServices") };
            var UoWFactory = new NHibernateUnitOfWorkFactory();
            UoWFactory.Initialize(assemblies);
            UnitOfWork.SetUnitOfWorkFactory(UoWFactory);
            RegisterSecurityProxy();
            log4net.Config.XmlConfigurator.Configure();
            MappingRegister.RegisterMapping();
            Framework.Behaviors.BehaviorInvoker.AddBehavior(new AuthenticationRequiredBehavior());
            Framework.Behaviors.BehaviorInvoker.AddIgnoredBehaviorType(typeof(Framework.Caching.CachableBehavior));
        }
        private static void RegisterSecurityProxy()
        {
            //securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        protected void Application_Error(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }
        protected void Application_End(object sender, EventArgs e)
        {
            //Noqoush.AdFalcon.Domain.Configuration.KafkaEventPublisher.Shutdown();
        }
    }
}