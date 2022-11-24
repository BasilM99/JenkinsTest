using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
//using System.Web.Security;
using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
//using Combres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ITempDataProvider = Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider;
using Microsoft.AspNetCore.Mvc.Routing;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using Microsoft.AspNetCore.Rewrite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.Web;
using ArabyAds.Framework.Web.JsonConverters;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Core
{
    public class RewriteRules
    {
        public static void RedirectRequests(RewriteContext context)
        {
            //Your logic

            var request = context.HttpContext.Request;
            var path = request.Path.Value;

            var userLangs = request.Headers.ContainsKey("Accept-Language") && !string.IsNullOrWhiteSpace(request.Headers["Accept-Language"])  ?  request.Headers["Accept-Language"].ToString():"en";
            var firstLang = userLangs.Split(',').FirstOrDefault();
            var defultCulture = string.IsNullOrWhiteSpace(firstLang) ? "en" : firstLang.Substring(0, 2);

            // Add your conditions of redirecting
            if (  (path.Split("/")[1] != "en") && (path.Split("/")[1] != "ar"))// If the url does not contain culture
            {
                if ( !request.Path.Value.Contains("Common/Resources")  && (!request.Headers.ContainsKey("X-ADFALCON-API"))  && (string.IsNullOrEmpty(request.Headers["X-Requested-With"])) && request.Method.ToLower() !="post" && !request.Path.Value.Contains("assets") && !request.Path.Value.Contains(".ashx") && !request.Path.Value.Contains(".jpg") && !request.Path.Value.Contains(".gif") &&!request.Path.Value.Contains(".jpeg") && !request.Path.Value.Contains(".png") && !request.Path.Value.Contains(".css") && !request.Path.Value.Contains(".js") && !request.Path.Value.Contains("Document"))
                    context.HttpContext.Response.Redirect($"/{defultCulture}{ Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedPathAndQuery(request)}");

            }

        }
    }
}
