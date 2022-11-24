using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{


    public class AddCookieResultServiceFilter : IResultFilter
    {


        public void OnResultExecuting(ResultExecutingContext context)
        {

            object[] Atts = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(OutputCacheAttribute), true);
            if (!(Atts == null || Atts.Length == 0))
            { return; }


            CookieOptions option = new CookieOptions();
            option.Secure = true;
            option.SameSite = SameSiteMode.None;
            option.Expires = Framework.Utilities.Environment.GetServerTime().AddYears(1);
            option.Domain = Config.CookieDomain;
            if (!(context.HttpContext.Response.ContentType!=null && context.HttpContext.Response.ContentType.ToLower().Contains("application/octet-stream") )&& !(context.HttpContext.Response.StatusCode == (int)System.Net.HttpStatusCode.MovedPermanently || context.HttpContext.Response.StatusCode == (int)System.Net.HttpStatusCode.Found) /* && !HttpContextHelper.Current.Response.HeadersWritten*/)
                context.HttpContext.Response.Cookies.Append("AdFalcon_UICulture", Thread.CurrentThread.CurrentUICulture.Name, option);
            /*context.HttpContext.Response.AddHeader("Content-Security-Policy", "upgrade-insecure-requests");
            context.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            context.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name, cookie");
            context.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            */
        }
        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            HttpContextHelper.Current.Response.Cookies.Append(key, value, option);
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Can't add to headers here because response has started.

        }
    }
}
