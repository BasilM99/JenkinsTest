using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Web.ClientValidation;

using System.Security.Permissions;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Connections;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Extensions;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class GridAction : ActionFilterAttribute
    {

        public bool EnableCustomBinding { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
        }

    }


    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _accessor = null;
        static HttpContextHelper()
        {
            _accessor = IoC.Instance.Resolve<IHttpContextAccessor>();
        }

        public static HttpContext Current
        {
            get
            {
                return _accessor.HttpContext;
            }
        }



    }

    public static class HttpRequestExtensions
    {
        public static Uri GetRawUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
        
          

            return new Uri(request.GetEncodedUrl());
        }

    public static string GetDomain(this HttpRequest request)
    {

        string host = request.GetRawUrl().GetComponents(UriComponents.Host, UriFormat.Unescaped); ;

        if (host.StartsWith("www."))
            return host.Substring(4);
        else
            return host;
    }
}
    public interface IPathProvider
    {
        string MapPath(string path);
    }

    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;

        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
            return filePath;
        }
    }


public static class HttpExtentions
{
    public static void MapRoute(this RouteBuilder builder, string route, Func<HttpContext, Task<ActionResult>> handler)
    {
        builder.MapRoute(route, async (c) =>
        {
            var actionResult = await handler(c).ConfigureAwait(false);
           // await actionResult.ExecuteResult().ConfigureAwait(false);
        });
    }
    public static string GetParamValue(this HttpRequest httpRequest, string paramKey)
    {
        try
        {
            //string paramValue = null;
            //if (httpRequest.HasFormContentType)
            //    paramValue = httpRequest.Form[paramKey].ToString();
            //else
            //    paramValue = httpRequest.Query[paramKey].ToString();

            string paramValue = httpRequest.Query[paramKey].ToString();
            if (paramValue.IsNullOrEmpty() && httpRequest.HasFormContentType)
                paramValue = httpRequest.Form[paramKey].ToString();

            return paramValue != string.Empty ? paramValue : null;
        }
        catch (TaskCanceledException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "GetParamValue", "task canceled exception");
            return null;
        }
        catch (ConnectionResetException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "GetParamValue", "connection reset exception");
            return null;
        }
        

        //return httpRequest.Query[paramKey] != StringValues.Empty ? httpRequest.Query[paramKey].ToString() : httpRequest.Form[paramKey] != StringValues.Empty ? ;
    }
    public static Dictionary<string, string> GetParamsWithPrefix(this HttpRequest httpRequest, string keyPrefix)
    {
        try
        {
            Dictionary<string, string> @params = new Dictionary<string, string>();
            foreach (var key in httpRequest.Query.Keys)
            {
                if (key.StartsWithIgnoreCase(keyPrefix))
                    @params.Add(key, httpRequest.Query[key]);
            }

            if (@params.Count == 0 && httpRequest.HasFormContentType)
            {
                foreach (var key in httpRequest.Form.Keys)
                {
                    if (key.StartsWithIgnoreCase(keyPrefix))
                        @params.Add(key, httpRequest.Form[key]);
                }
            }

            return @params;
        }
        catch (TaskCanceledException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "GetParamsWithPrefix", "task canceled exception");
            return null;
        }
        catch (ConnectionResetException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "GetParamsWithPrefix", "connection reset exception");
            return null;
        }
       
    }
    public static string ReadBody(this HttpRequest httpRequest)
    {
        try
        {
            string body = null;
            using (StreamReader reader = new StreamReader(httpRequest.Body))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
        catch (TaskCanceledException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadBody", "task canceled exception");
            return null;
        }
        catch (ConnectionResetException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadBody", "connection reset exception");
            return null;
        }
       
    }
    public static async Task<string> ReadBodyAsync(this HttpRequest httpRequest)
    {
        try
        {
            string body = null;
            using (StreamReader reader = new StreamReader(httpRequest.Body))
            {
                body = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return body;
        }
        catch (TaskCanceledException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadBodyAsync", "task canceled exception");
            return null;
        }
        catch (ConnectionResetException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadBodyAsync", "connection reset exception");
            return null;
        }
       
    }
    public static async Task<string> ReadAsync(this HttpRequest httpRequest)
    {
        try
        {
            byte[] buffer = new byte[Convert.ToInt32(httpRequest.ContentLength)];
            await httpRequest.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            return Encoding.UTF8.GetString(buffer);
        }
        catch (TaskCanceledException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadAsync", "task canceled exception");
            return null;
        }
        catch (ConnectionResetException)
        {
            //LoggerHelper.Warn("ExtentionMethods", "ReadAsync", "connection reset exception");
            return null;
        }
       
    }
    //public static void RunAsCustomService(this IWebHost host)
    //{
    //    var webHostService = new CustomWebHostService(host);
    //    ServiceBase.Run(webHostService);
    //}
}


public static class ExtensionMethods
{



    public static NameValueCollection ToNameValueCollection(this IQueryCollection Query)
    {

        NameValueCollection nameValueCollection = new NameValueCollection();
        foreach (var item in Query)
        {
            nameValueCollection.Add(item.Key, item.Value);
        }
        return nameValueCollection;
    }

    public static NameValueCollection ToNameValueCollection(this Dictionary<string, StringValues> Query)
    {

        NameValueCollection nameValueCollection = new NameValueCollection();
        foreach (var item in Query)
        {
            nameValueCollection.Add(item.Key, item.Value);
        }
        return nameValueCollection;
    }

    public static bool ExtEquals(this int[] compareArray, int[] compareToArray)
    {
        if (compareToArray == null) return false;
        if (compareArray.Length != compareToArray.Length) return false;

        for (int i = 0; i < compareArray.Length; i++)
        {
            if (compareArray[i] != compareToArray[i])
                return false;
        }

        return true;
    }

    public static int HoursTime(this DateTime datetime)
    {
        if (datetime.Hour >= 0 && datetime.Hour < 6)
            return 1;
        else if (datetime.Hour >= 6 && datetime.Hour < 12)
            return 2;
        else if (datetime.Hour >= 12 && datetime.Hour < 18)
            return 3;
        else //if (hour24 >= 18 && hour24 < 24)
            return 4;
    }


    public static void AddHeader(this HttpResponse Response, string key , string value)
    {
        if(!Response.Headers.ContainsKey(key))
         Response.Headers.Add(key , value);
    }
    public static void End(this HttpResponse Response)
    {
        Response.StatusCode = StatusCodes.Status200OK;
    }
    public static void Flush(this HttpResponse Response)
    {
        Response.Body.Flush();
    }


   
    public static DateTime RemoveMinutesAndSeconds(this DateTime datetime)
    {
        return datetime.Date.AddHours(datetime.Hour);
    }

    public static bool HasColumn(this IDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    }

    public static bool IsNullOrEmpty(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return true;
        else
            return false;
    }

    public static bool IsNotNullOrEmpty(this string str)
    {
        if (!string.IsNullOrEmpty(str))
            return true;
        else
            return false;
    }

    public static bool EqualsIgnoreCase(this string str, string compareValue)
    {
        if (str == null)
        {
            if (compareValue == null)
                return true;
            else
                return false;
        }

        if (str.Equals(compareValue, StringComparison.OrdinalIgnoreCase))
            return true;
        else
            return false;
    }

    public static bool StartsWithIgnoreCase(this string str, string startValue)
    {
        if (str == null)
        {
            if (startValue == null)
                return true;
            else
                return false;
        }

        if (str.StartsWith(startValue, StringComparison.OrdinalIgnoreCase))
            return true;
        else
            return false;
    }

    public static bool In(this string str, params string[] values)
    {
        if (str.IsNullOrEmpty())
            return false;

        for (int i = 0; i < values.Length; i++)
        {
            if (str.Equals(values[i], StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public static string RemoveFromStart(this string s, string prefix, bool ignoreCase)
    {
        if (s.IsNullOrEmpty())
            return s;

        if (s.StartsWith(prefix, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
        {
            return s.Substring(prefix.Length);
        }
        else
        {
            return s;
        }
    }

    public static string RemoveFromEnd(this string s, string suffix, bool ignoreCase)
    {
        if (s.IsNullOrEmpty())
            return s;

        if (s.EndsWith(suffix, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
        {
            return s.Substring(0, s.Length - suffix.Length);
        }
        else
        {
            return s;
        }
    }

    public static T[] Union<T>(this T[] set, params T[] items)
    {
        if (items == null)
            return set;

        // Join the arrays
        T[] result = new T[set.Length + items.Length];
        set.CopyTo(result, 0);
        items.CopyTo(result, set.Length);
        return result;
    }

    public static void AddRange<T>(this HashSet<T> set, ICollection<T> collection)
    {
        if (collection == null)
            return;

        foreach (var item in collection)
            set.Add(item);
    }
    //public static void AddRange<T>(this ICollection<T> set, ICollection<T> collection)
    //{
    //    if (collection == null)
    //        return;

    //    foreach (var item in collection)
    //        set.Add(item);
    //}

    public static decimal Round(this decimal value, int decimalFractions = 12)
    {
        return Math.Round(value, decimalFractions);
    }

    public static IList<string> Domains(this Uri uri)
    {
        List<string> domains = new List<string>();
        if (uri.IsAbsoluteUri)
        {
            string host = uri.Host.RemoveFromStart("www.", true);
            if (uri.HostNameType.Equals(UriHostNameType.Dns))
                GetDomains(host);
            else
                domains.Add(host);
        }
        else
        {
            string urlWithoutQuery = uri.OriginalString.RemoveFromStart("www.", true);
            if (urlWithoutQuery.Contains("/?"))
                urlWithoutQuery = urlWithoutQuery.Remove(urlWithoutQuery.IndexOf("/?"));
            else if (urlWithoutQuery.Contains("?"))
                urlWithoutQuery = urlWithoutQuery.Remove(urlWithoutQuery.IndexOf("?"));

            GetDomains(urlWithoutQuery);
        }

        return domains;

        void GetDomains(string domain)
        {
            int index = domain.IndexOf('.');
            if (index == -1)
            {
                if (domains.Count == 0)
                    domains.Add(domain);
                return;
            }

            domains.Add(domain);
            string nextDomain = domain.Substring(index + 1);
            GetDomains(nextDomain);
        }
    }

    public static ushort EpochDays(this DateTimeOffset utcDate)
    {
        return (ushort)(utcDate.ToUnixTimeSeconds() / 86_400);
    }
    public static long ToUnixTimeSeconds(this DateTime utcDate)
    {
        //int DaysPerYear = 365;
        //int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        //int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        //int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
        //int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162
        //long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970; // 621,355,968,000,000,000
        long UnixEpochSeconds = 62135596800;// UnixEpochTicks / TimeSpan.TicksPerSecond; // 62,135,596,800
        long seconds = utcDate.Ticks / TimeSpan.TicksPerSecond;
        return seconds - UnixEpochSeconds;
    }

    public static IList<T> Clone<T>(this IList<T> list)
    {
        if (list == null)
            return null;

        //IList<T> clonedList = new List<T>(list);
        IList<T> clonedList = new List<T>();
        for (int i = 0; i < list.Count; i++)
            clonedList.Add(list[i]);

        return clonedList;
    }

    public static IList<T> CloneClone<T>(this IList<T> list) where T : class, ICloneable
    {
        if (list == null)
            return null;

        IList<T> clonedList = new List<T>();
        for (int i = 0; i < list.Count; i++)
            clonedList.Add(list[i].Clone() as T);

        return clonedList;
    }
}


/// <summary>
/// <see cref="IUrlHelper"/> extension methods.
/// </summary>
public static class UrlHelperExtensionsExtended
{
    /// <summary>
    /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
    /// route values.
    /// </summary>
    /// <param name="url">The URL helper.</param>
    /// <param name="actionName">The name of the action method.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The route values.</param>
    /// <returns>The absolute URL.</returns>
    public static string AbsoluteAction(
        this IUrlHelper url,
        string actionName,
        string controllerName,
        object routeValues = null)
    {
        return url.Action(actionName, controllerName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
    }

    /// <summary>
    /// Generates a fully qualified URL to the specified content by using the specified content path. Converts a
    /// virtual (relative) path to an application absolute path.
    /// </summary>
    /// <param name="url">The URL helper.</param>
    /// <param name="contentPath">The content path.</param>
    /// <returns>The absolute URL.</returns>
    public static string AbsoluteContent(
        this IUrlHelper url,
        string contentPath)
    {
        HttpRequest request = url.ActionContext.HttpContext.Request;
        return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
    }

    /// <summary>
    /// Generates a fully qualified URL to the specified route by using the route name and route values.
    /// </summary>
    /// <param name="url">The URL helper.</param>
    /// <param name="routeName">Name of the route.</param>
    /// <param name="routeValues">The route values.</param>
    /// <returns>The absolute URL.</returns>
    public static string AbsoluteRouteUrl(
        this IUrlHelper url,
        string routeName,
        object routeValues = null)
    {
        return url.RouteUrl(routeName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
    }
}




