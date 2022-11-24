using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Noqoush.Framework;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
   public class NoqoushControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {
       protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
       {
          
           if (controllerType != null)
           {
               return (IController) Noqoush.Framework.IoC.Instance.Resolve(controllerType);
           }
           return base.GetControllerInstance(requestContext, controllerType);
       }
       /// <summary>      
       /// Release the controller at the end of it's life cycle       
       /// </summary>       
       /// <param name="controller">The Interface to an MVC controller</param>       
       public override void ReleaseController(IController controller)
       {
           var disposableController = controller as IDisposable;
           if (disposableController != null)
           {
               disposableController.Dispose();
           }
           IoC.Instance.Release(controller);
       }
    }
}
