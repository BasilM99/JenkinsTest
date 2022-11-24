//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Routing;
//using System.Web.SessionState;
//using ArabyAds.Framework;
//using Microsoft.AspNet.Mvc.Controllers;

//namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
//{
//   public class NoqoushControllerFactory : DefaultControllerFactory
//    {
//       protected override IController CreateController(Microsoft.AspNetCore.Routing.RequestContext requestContext, Type controllerType)
//       {
          
//           if (controllerType != null)
//           {
//               return (IController) ArabyAds.Framework.IoC.Instance.Resolve(controllerType);
//           }
//           return base.GetControllerInstance(requestContext, controllerType);
//       }
//       /// <summary>      
//       /// Release the controller at the end of it's life cycle       
//       /// </summary>       
//       /// <param name="controller">The Interface to an MVC controller</param>       
//       public override void ReleaseController(IController controller)
//       {
//           var disposableController = controller as IDisposable;
//           if (disposableController != null)
//           {
//               disposableController.Dispose();
//           }
//           IoC.Instance.Release(controller);
//       }
//    }
//}
