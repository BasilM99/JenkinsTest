using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using System.Globalization;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Core
{
    public class LoclizationFilter : IResourceFilter
    {


        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //object[] Atts = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(OutputCacheAttribute), true);
            //if (!(Atts == null || Atts.Count() == 0))
            //{ return; }


                RouteData routeData = context.RouteData;
           
            if (routeData != null)
            {

                var userLangs = context.HttpContext.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();

                var culture = Thread.CurrentThread.CurrentCulture;
                if (routeData.Values["language"] != null &&
                    !string.IsNullOrWhiteSpace(routeData.Values["language"].ToString()))
                {
                    // set the culture from the route data (url)
                    var lang = routeData.Values["language"].ToString();
                    culture = CultureInfo.CreateSpecificCulture(getCulture(lang));
                }
                else
                {
                    // load the culture info from the cookie
                    var cookie = context.HttpContext.Request.Cookies["AdFalcon_UICulture"];
                    var langHeader = string.Empty;
                    if (cookie != null)
                    {
                        // set the culture by the cookie content
                        langHeader = cookie;
                        culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                    }
                    else
                    {
                        // set the culture by the location if not specified
                        if ((userLangs != null) && (!string.IsNullOrEmpty(userLangs)))
                        {
                            langHeader = firstLang;
                            culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                        }
                        else
                        {
                            langHeader = "en-Us";
                            culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                        }
                    }

                    routeData.Values["language"] = langHeader;
                }

                //set Current Thread Culture
                //culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                culture.DateTimeFormat.ShortDatePattern = Config.ShortDateFormat;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Framework.OperationContext.Current.CultureCode = culture.Name;
            }
        }


        private string getCulture(string lang)
        {
            lang = lang.ToLower();
            if (lang.Contains("-"))//en-US,en-UK
            {
                lang = lang.Substring(0, 2);
            }
            var returnStr = "en-US";
            switch (lang)
            {
                case "ar":
                    returnStr = "ar-JO";
                    break;
                case "en":
                    returnStr = "en-US";
                    break;
            }
            return returnStr;
        }
    }
}
