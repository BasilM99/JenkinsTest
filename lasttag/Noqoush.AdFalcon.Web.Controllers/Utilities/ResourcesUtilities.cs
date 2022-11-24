using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities
{
    public static class ResourcesUtilities
    {
        public static string GetResource(string resourceKey, string resourceSet)
        {
            return HttpContext.GetGlobalResourceObject(resourceSet, resourceKey, Thread.CurrentThread.CurrentCulture).ToString();
        }

        public static string GetResource(string resourceKey)
        {
            return HttpContext.GetGlobalResourceObject("Global", resourceKey, Thread.CurrentThread.CurrentCulture).ToString();
        }

        public static string GetResource(string resourceKey, string resourceSet, CultureInfo cultureInfo)
        {
            return HttpContext.GetGlobalResourceObject(resourceSet, resourceKey, cultureInfo).ToString();
        }
    }
}
