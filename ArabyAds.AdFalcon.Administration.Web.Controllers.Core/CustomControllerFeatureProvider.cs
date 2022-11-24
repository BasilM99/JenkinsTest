using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc.Controllers;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Core
{
    public class CustomControllerFeatureProvider : ControllerFeatureProvider
    {
        private IEnumerable<Type> _excludedControllerTypes;
        public CustomControllerFeatureProvider(Assembly subControllers, Assembly baseControllers) :base()
        {
            _excludedControllerTypes = GetInheritedControllersTypes(subControllers, baseControllers);
        }
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isController = base.IsController(typeInfo);

            if (isController)
            {
                foreach (var type in _excludedControllerTypes)
                {
                    if (typeInfo.Equals(type))
                    {
                        isController = false;
                        break;
                    }
                }
            
            }
            return isController;
        }

        private IList<Type> GetInheritedControllersTypes(Assembly subControllers, Assembly baseControllers)
        {
            var ControllerTypeNameSuffix = "Controller";
            var inheritedControllers = new List<Type>();

            var baseControllersTypes = baseControllers.ExportedTypes.Where(t => t.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase));
            var subControllersTypes = subControllers.ExportedTypes.Where(t => t.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase));

            foreach (var baseControllerType in baseControllersTypes)
            {
                foreach (var subControllerType in subControllersTypes)
                {
                    if (baseControllerType.IsAssignableFrom(subControllerType))
                    {
                        inheritedControllers.Add(baseControllerType);
                        break;
                    }
                }
            }


            return inheritedControllers;
        }
    }
}
