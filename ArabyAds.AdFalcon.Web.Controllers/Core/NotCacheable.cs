using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Core
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
            object[] Atts = filterContext.ActionDescriptor.GetCustomAttributes(typeof(OutputCacheAttribute), true);
            if (Atts == null || Atts.Count() == 0)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
            }
            Atts = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            if (Atts != null && Atts.Count() > 0)
            {
                filterContext.Controller.ViewBag.ShowMenu = true;
            }
            else
            {
                Atts = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true);
                if (Atts != null && Atts.Count() > 0)
                {
                    filterContext.Controller.ViewBag.ShowMenu = true;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

}
