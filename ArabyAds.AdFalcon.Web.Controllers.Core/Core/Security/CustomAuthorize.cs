using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Core.Security
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User != null && (filterContext.HttpContext.User is ArabyAdsPrincipal))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
            }

            filterContext.Result = new RedirectResult(string.Format("/User/Login?returnurl={0}", filterContext.HttpContext.Request.GetRawUrl()));

            // filterContext.Result = new RedirectResult("/");
            //throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NoHttps : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
