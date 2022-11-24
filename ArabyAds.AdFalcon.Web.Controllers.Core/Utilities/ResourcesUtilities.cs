using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities
{
    public static class ResourcesUtilities
    {
        public static string GetResource(string resourceKey, string resourceSet)
        {
            return   ArabyAds.Framework.Resources.ResourceManager.Instance.GetResource( resourceKey, resourceSet, Thread.CurrentThread.CurrentCulture).ToString();
        }

        public static string GetResource(string resourceKey)
        {
            return ArabyAds.Framework.Resources.ResourceManager.Instance.GetResource( resourceKey, "Global", Thread.CurrentThread.CurrentCulture).ToString();
        }

        public static string GetResource(string resourceKey, string resourceSet, CultureInfo cultureInfo)
        {
            return   ArabyAds.Framework.Resources.ResourceManager.Instance.GetResource( resourceKey, resourceSet, cultureInfo).ToString();
        }
    }
}
