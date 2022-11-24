using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core.Security;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    /// <summary>
    /// Set all returned action results to non cacheable unless OutputCache Attribute
    /// </summary>
    public class NotCacheable : ActionFilterAttribute
    {
        /// <summary>
        /// Set all returned action results to non cacheable unless OutputCache Attribute
        /// </summary>
        /// <param name="filterContext">Executing action context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] Atts = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(OutputCacheAttribute), true);
            if (Atts == null || Atts.Count() == 0)
            {
                //filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
                //filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                //filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                //filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //filterContext.HttpContext.Response.Cache.SetNoStore();

                filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                filterContext.HttpContext.Response.Headers["Expires"] = "-1";
                filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            }
            Atts = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            object[]  Atts2 = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CustomAuthorize), true);


            if (Atts != null && Atts.Count() > 0)
            {
                if (filterContext.Controller is Controller controller)
                    controller.ViewBag.ShowMenu = true;
            }
            else if (Atts2 != null && Atts2.Count() > 0)
            {
                if (filterContext.Controller is Controller controller)
                    controller.ViewBag.ShowMenu = true;

            }
            else
            {
                if (filterContext.Controller is Controller controller)
                    Atts = controller.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);
                //Atts = typeof(controller.).met.GetCustomAttributes(typeof(AuthorizeAttribute), true);
                if (Atts != null && Atts.Count() > 0)
                {
                    if (filterContext.Controller is Controller controllerbase)
                        controllerbase.ViewBag.ShowMenu = true;
                }

                if (filterContext.Controller is Controller controller2)
                    Atts2 = controller2.GetType().GetCustomAttributes(typeof(CustomAuthorize), true);
                //Atts = typeof(controller.).met.GetCustomAttributes(typeof(AuthorizeAttribute), true);
                if (Atts2 != null && Atts2.Count() > 0)
                {
                    if (filterContext.Controller is Controller controllerbase)
                        controllerbase.ViewBag.ShowMenu = true;
                }

            }
            base.OnActionExecuting(filterContext);
        }
    }

}
