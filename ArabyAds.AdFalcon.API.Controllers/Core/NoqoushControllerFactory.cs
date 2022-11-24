using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ArabyAds.AdFalcon.API.Controllers.Core
{
    public class NoqoushControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {
        
        private const string controllerIsntExist = "{0} controller isnt registered";
        private List<Assembly> _ControllersAssemblies;
        private Dictionary<string, Type> _ControllersDictionary = new Dictionary<string, Type>();

        public NoqoushControllerFactory(List<Assembly> controllersAssemblies)
        {
            _ControllersAssemblies = controllersAssemblies;
        }


        public override IController  CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {

                Type controllerType = null;

                string subDomainName = requestContext.RouteData.Values["apitype"] == null ? string.Empty : requestContext.RouteData.Values["apitype"].ToString();

                string typeName = string.Format("{0}{1}Controller", subDomainName, controllerName);


                // Try to get the type for this controller from dictionary
                _ControllersDictionary.TryGetValue(typeName, out controllerType);

                if (controllerType == null)
                {
                    typeName = string.Format("{0}Controller", controllerName);

                    // Try to get the type for this controller from dictionary
                    _ControllersDictionary.TryGetValue(typeName, out controllerType);

                }

                if (controllerType == null)
                {
                     typeName = string.Format("{0}{1}Controller", subDomainName, controllerName);

                    // Get the type for this interface
                    controllerType = GetControllerService(typeName);

                    if (controllerType == null)
                    {
                         typeName = string.Format("{0}Controller", controllerName);

                         // Get the type for this interface
                         controllerType = GetControllerService(typeName);
                    }

                    if (controllerType != null)
                    {
                        AddControllerTypeToDictionary(typeName, controllerType);
                    }
                }

                if (controllerType == null)
                    return null;

                // Return the correct controller for this interface
                if (IoC.Instance.IsRegistered(controllerType))
                {
                    requestContext.RouteData.Values["controller"] = typeName.Replace("Controller", "");
                    var controller = IoC.Instance.Resolve(controllerType) as IController;
                    return controller;
                }
                else
                {
                    return base.GetControllerInstance(requestContext, controllerType);
                }

            }
            catch (Exception x)
            {
                throw x;
            }
        }

        private void AddControllerTypeToDictionary(string controllerName, Type controllerType)
        {
            lock (this)
            {
                if (!_ControllersDictionary.ContainsKey(controllerName))
                {
                    _ControllersDictionary.Add(controllerName, controllerType);
                }
            }
        }

        /// <summary>
        /// Get the type by controlelrname
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private Type GetControllerService(string serviceName)
        {
            Type controllerService = null;
            foreach (var item in _ControllersAssemblies)
            {
                controllerService = item.GetTypes().Where(p => p.Name.ToLower() == serviceName.ToLower()).SingleOrDefault();

                if (controllerService != null)
                    break;
            }

            return controllerService;
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