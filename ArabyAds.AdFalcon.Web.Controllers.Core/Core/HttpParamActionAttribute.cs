using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    //public class HttpParamActionAttribute : ActionNameSelectorAttribute
    //{
    //    public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
    //    {
    //        if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
    //            return true;

    //        if (!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
    //            return false;

    //        var request = controllerContext.RequestContext.HttpContextHelper.Request;
    //        return request[methodInfo.Name] != null;
    //    }
    //}
}
